using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Business.Common.Responses;
using Business.Common.Statuses;
using Data.Models;
using Data.Models.DataTransferObjects;
using Data.Models.Entities;
using Data.Repository.FunctionDefinitions;

namespace Business.Service.Interfaces {
	public interface IMembershipService {
		User Get(long id);

		TransactionResponse AssignPassword(User user, string password, bool tempPassword = true);
		TransactionResponse ResetPassword(User user, string accountLoginLink);
		TransactionResponse ResetPassword(string emailAddress, string accountLoginLink);

		LoginStatus SignIn(string emailAddress, string password, bool rememberMe);
		LoginStatus SignIn(User user, bool rememberMe = false);
		LoginStatus SignIn_ViaToken(string authenticationToken, string sharedKey);

		User Current();
		long CurrentUserId();
		bool IsLoggedIn();
		bool RedirectToLoginIfNotLoggedIn(HttpContextBase httpContextBase);
		void RedirectToLogin(HttpContextBase httpContextBase);
		void RedirectToUnauthorized(HttpContextBase httpContextBase);

		GetUserByEmailAddress_Result GetByEmailAddress(string userEmail);
		
		AuthorizationResult Authorize(bool systemAdminAccessRequired);
		AuthorizationResult Authorize(AuthorizationContext filterContext, bool systemAdminAccessRequired);

		void SignOut();
		bool DoesUserExist(string emailAddress);
		
		bool IsInRole(long userId, SystemRole systemRole);
		bool IsInRole(SystemRole systemRole);
		bool IsInRole(List<SystemRole> systemRole);

		User GetCurrentUser_ThrowErrorIfUserDoesNotExist();
	}
}
