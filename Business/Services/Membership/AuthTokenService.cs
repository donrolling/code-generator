using System;
using Business.Common.DatesAndTimes;

namespace Business.Services.Membership {
	public static class AuthTokenService {
		/// <summary>
		/// This may change later, but for now we just have a shared key
		/// sharedKey|ticks is the format of the authenticationToken string
		/// </summary>
		/// <param name="authenticationTokenSlug"></param>
		/// <returns></returns>
		public static string ParseAuthenticationToken(string authenticationTokenSlug) { 
			var values = authenticationTokenSlug.Split('|');
			var authenticationToken = values[0];
			var tickString = values[1];
			long ticks = 0;
			long.TryParse(tickString, out ticks);
			var dateTime = new DateTime(ticks);
			if (!TimeHelper.Between(dateTime, DateTime.Now.AddMinutes(-5), DateTime.Now.AddMinutes(5))){
				return string.Empty;
			}
			return authenticationToken;
		}

		public static string GetAuthToken(string sharedKey) {
			return string.Concat(sharedKey, "|", DateTime.Now.Ticks.ToString());
		}
	}
}