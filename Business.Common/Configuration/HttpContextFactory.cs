using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using Business.Common.Configuration.FakeObjects;

namespace Business.Common.Configuration {
	public class HttpContextFactory{
		private static HttpContextBase _context;
		public static HttpContextBase Current{
			get{
				if (_context != null)
					return _context;

				if (HttpContext.Current == null)
					throw new InvalidOperationException("HttpContext not available");

				return new HttpContextWrapper(HttpContext.Current);
			}
		}
		public static void SetCurrentContext(HttpContextBase context){
			_context = context;
		}

		public static void SetHttpContext(HttpContextBase httpContext) {
			SetCurrentContext(httpContext);
		}

		public static HttpContextBase GetTestHttpContext() {
			return GetTestHttpContext(string.Empty, string.Empty);
		}
		public static HttpContextBase GetTestHttpContext(string url) {
			return GetTestHttpContext(string.Empty, url);
		}
		public static HttpContextBase GetTestHttpContext(string identityUsername, string url) {
			var formParams = new NameValueCollection { { "iDisplayStart", "0" }, { "iDisplayLength", "10" }, { "iSortCol_0", "0" }, { "iSortDir_0", "asc" } }; //for data tables
			var queryStringParams = new NameValueCollection { };
			var cookies = new HttpCookieCollection();
			var sessionItems = new SessionStateItemCollection();
			var identity = new FakeIdentity(identityUsername);
			var roles = new string[] { };
			var principle = new FakePrincipal(identity, roles);

			Uri uri = string.IsNullOrEmpty(url) ? null : new Uri(url);
			var httpContext = new FakeHttpContext(
									principle,
									formParams,
									queryStringParams,
									cookies,
									sessionItems,
									uri
								);

			return httpContext;
		}
	}
}