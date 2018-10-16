using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Business.Common.Logging;

namespace Business.Common {
	public static class WebCommunicator {
		public static Tuple<string, WebCommunicatorResponses> MakeWebRequest(string path, ILogger logger) {
			var responseHtml = string.Empty;
			var responseType = WebCommunicatorResponses.Unavailable;
			StreamReader responseStream = null;
			try {
				var webRequest = (HttpWebRequest)WebRequest.Create(path);
				webRequest.Method = "GET";
				webRequest.AllowAutoRedirect = false;
				webRequest.CookieContainer = new CookieContainer();
				webRequest.ContentType = "application/json; charset=utf-8";
				var webResponse = (HttpWebResponse)webRequest.GetResponse();
				responseType = GetResponseType(webResponse);
				responseStream = new StreamReader(webResponse.GetResponseStream());
				responseHtml = responseStream.ReadToEnd();
			} catch (Exception ex) {
				logger.Log(LogSeverity.Error, ex.Message.ToString());
			} finally {
				if (responseStream != null) {
					responseStream.Close();
				}
			}
			return Tuple.Create<string, WebCommunicatorResponses>(responseHtml, responseType);
		}

		public static async Task<Tuple<string, WebCommunicatorResponses>> MakeWebRequestAsync(string path, ILogger logger) {
			var responseHtml = string.Empty;
			var responseType = WebCommunicatorResponses.Unavailable;
			StreamReader responseStream = null;
			try {
				var webRequest = (HttpWebRequest)WebRequest.Create(path);
				webRequest.Method = "GET";
				webRequest.AllowAutoRedirect = false;
				webRequest.CookieContainer = new CookieContainer();
				var webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
				responseType = GetResponseType(webResponse);
				responseStream = new StreamReader(webResponse.GetResponseStream());
				responseHtml = responseStream.ReadToEnd();
			} catch (Exception ex) {
				logger.Log(LogSeverity.Info, ex.Message.ToString());
			} finally {
				if (responseStream != null) {
					responseStream.Close();
				}
			}
			return Tuple.Create<string, WebCommunicatorResponses>(responseHtml, responseType);
		}

		public static WebCommunicatorResponses GetResponseType(HttpWebResponse webResponse) {
			var statusCode = webResponse.StatusCode;
			var result = WebCommunicatorResponses.Normal;
			switch (statusCode) {
				case HttpStatusCode.Ambiguous:
				case HttpStatusCode.BadGateway:
				case HttpStatusCode.BadRequest:
				case HttpStatusCode.Conflict:
				case HttpStatusCode.ExpectationFailed:
				case HttpStatusCode.GatewayTimeout:
				case HttpStatusCode.Gone:
				case HttpStatusCode.HttpVersionNotSupported:
				case HttpStatusCode.LengthRequired:
				case HttpStatusCode.MethodNotAllowed:
				case HttpStatusCode.Moved:
				case HttpStatusCode.NoContent:
				case HttpStatusCode.NotAcceptable:
				case HttpStatusCode.NotFound:
				case HttpStatusCode.NotImplemented:
				case HttpStatusCode.ResetContent:
				case HttpStatusCode.ServiceUnavailable:
				case HttpStatusCode.UnsupportedMediaType:
				case HttpStatusCode.Unused:
					result = WebCommunicatorResponses.Unavailable;
					break;
				case HttpStatusCode.Accepted:
				case HttpStatusCode.Continue:
				case HttpStatusCode.Created:
				case HttpStatusCode.Found:
				case HttpStatusCode.NotModified:
				case HttpStatusCode.OK:
				case HttpStatusCode.PartialContent:
				case HttpStatusCode.RedirectKeepVerb:
				case HttpStatusCode.RedirectMethod:
				case HttpStatusCode.SwitchingProtocols:
				case HttpStatusCode.UseProxy:
					result = WebCommunicatorResponses.Normal;
					break;
				case HttpStatusCode.Forbidden:
				case HttpStatusCode.NonAuthoritativeInformation:
				case HttpStatusCode.PaymentRequired:
				case HttpStatusCode.ProxyAuthenticationRequired:
				case HttpStatusCode.Unauthorized:
					result = WebCommunicatorResponses.AccessDenied;
					break;
				case HttpStatusCode.InternalServerError:
				case HttpStatusCode.PreconditionFailed:
				case HttpStatusCode.RequestEntityTooLarge:
				case HttpStatusCode.RequestTimeout:
				case HttpStatusCode.RequestUriTooLong:
				case HttpStatusCode.RequestedRangeNotSatisfiable:
					result = WebCommunicatorResponses.ServerError;
					break;
			}
			return result;
		}
	}
}