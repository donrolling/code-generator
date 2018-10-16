using System;
using System.Web;

namespace Business.Common.Errors {
	public static class ErrorHandling {
		public static Tuple<int, Exception> GetErrorCodeAndMessage(HttpServerUtility httpServerUtility) {
			var errorCode = 0;
			var ex = httpServerUtility.GetLastError();
			if (ex is HttpException) {
				errorCode = ((HttpException)ex).GetHttpCode();
			}
			httpServerUtility.ClearError();
			return Tuple.Create(errorCode, ex);
		}
	}
}