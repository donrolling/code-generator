using System;
using System.Web;
using System.Web.Security;
using Business.Common;
using Business.Common;
using Business.Common.Configuration;
using Business.Common.Logging;
using Business.Service.Interfaces;
using Data.Models.Entities;
using log4net;

namespace Business.Services.Membership {
	public class AuthenticationPersistenceService : ServiceBase, IAuthenticationPersistenceService {
		public AuthenticationPersistenceService(ILogger logger) : base(logger) { }
		
		public User RetrieveUserFromWebContext(IMembershipService membershipService) {
			var userId = getUserIdFromCookie();
			if (userId > 0) {
				return membershipService.Get(userId);
			}
			return null;
		}

		public void PersistUserInWebContext(User user, bool rememberMe) {
			try {
				FormsAuthentication.SetAuthCookie(user.Id.ToString(), rememberMe);
			} catch (Exception ex) {
				this.Logger.Log(LogSeverity.Error, ex);
			}
		}

		private long getUserIdFromCookie() {
			HttpCookie cookie = HttpContextFactory.Current != null ? HttpContextFactory.Current.Request.Cookies[FormsAuthentication.FormsCookieName] : null;
			if (cookie == null) {
				return 0;
			}

			FormsAuthenticationTicket ticket = null;
			try {
				if (cookie.Value.Length == 0) {
					return 0;
				}
				ticket = FormsAuthentication.Decrypt(cookie.Value);
			} catch { }
			if (ticket == null) {
				return 0;
			}

			long userId = 0;
			long.TryParse(ticket.Name, out userId);
			return userId; //zero will indicate guest user
		}

		public void SignOut() {
			try {
				HttpContextFactory.Current.Session.Remove("User");
			} catch { }
			try {
				FormsAuthentication.SignOut();
			} catch { }
		}

		private static string logError(Exception ex, string additionalInformation = "") {
			var message = string.Concat("Email Send Error. \n\t", ex.Message, string.IsNullOrEmpty(additionalInformation) ? "" : string.Concat("\n\t\tAdditional Information: ", additionalInformation));
			var log = LogManager.GetLogger(typeof(AuthenticationPersistenceService));
			log.Error(message);
			return message;
		}
	}
}
