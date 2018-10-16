using Business.Common;
using Business.Common.Configuration;
using Business.Common.Errors;
using Business.Common.Logging;
using Business.Common.Responses;
using Business.Common.Statuses;
using Business.Configuration;
using Business.Service.Interfaces;
using Data.Models;
using Data.Models.DataTransferObjects;
using Data.Models.Entities;
using Data.Repository.FunctionDefinitions;
using Data.Repository.Interfaces;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Business.Services.Membership {
	public class MembershipService : ServiceBase, IMembershipService {
		public IUserRepository UserRepository { get; set; }
		public IMessageService MessageService { get; set; }
		public IAuthenticationPersistenceService AuthenticationPersistenceService { get; set; }

		public MembershipService(IUserRepository userRepository, IMessageService messageService, IAuthenticationPersistenceService authenticationPersistenceService, ILogger logger)
			: base(logger) {
			UserRepository = userRepository;
			MessageService = messageService;
			AuthenticationPersistenceService = authenticationPersistenceService;
		}

		public TransactionResponse AssignPassword(User user, string password, bool tempPassword = true) {
			return assignPassword(user, password, user, tempPassword);
		}

		private TransactionResponse assignPassword(User user, User currentUser, bool tempPassword = true) {
			return assignPassword(user, string.Empty, currentUser, tempPassword);
		}

		private TransactionResponse assignPassword(User user, string password, User currentUser, bool tempPassword = true) {
			var pm = string.IsNullOrEmpty(password) ? new PasswordService() : new PasswordService(password);
			var result = assignPassword(user, pm.EncryptedPassword, pm.Salt, currentUser, tempPassword);
			result.TransactionResult = pm.Password;
			return result;
		}

		private TransactionResponse assignPassword(User user, string password, string salt, User currentUser, bool tempPassword = true) {
			if (user == null || currentUser == null || user.Id == 0) {
				return TransactionResponse.GetTransactionResponse(ActionType.Update, Status.Failure, StatusDetail.Unknown);
			}

			var result = UserRepository.UpdatePassword(user.Id, password, salt, currentUser.Id, DateTime.Now);
			return result;
		}

		/// <summary>
		/// Resets a user's password.
		/// </summary>
		/// <param name="user"></param>
		/// <returns>clear text password</returns>
		public TransactionResponse ResetPassword(User user, string loginUrl) {
			var saveResult = assignPassword(user, Current());
			var password = saveResult.TransactionResult.ToString();
			var fromAddress = Config.Setting(ConfigKey.AdminSendFrom.ToString());
			if (saveResult.Status == Status.Success) {
				this.MessageService.PasswordResetEmail_Send(user.Email, password, loginUrl, fromAddress);
			}
			return saveResult;
		}

		public TransactionResponse ResetPassword(string emailAddress, string accountLoginLink) {
			var user = this.ConvertToUser(GetByEmailAddress(emailAddress));
			if (user != null) {
				return ResetPassword(user, accountLoginLink);
			}
			return TransactionResponse.GetTransactionResponse(ActionType.Update, Status.Failure, StatusDetail.ItemNotFound);
		}

		public LoginStatus SignIn(string emailAddress, string password, bool rememberMe) {
			var user = GetByEmailAddress(emailAddress);
			if (user == null) {
				return LoginStatus.GetLoginStatus(SignInStatus.InvalidUsername);
			}
			if (!user.IsActive) {
				return LoginStatus.GetLoginStatus(SignInStatus.UserNotActive, user.Id);
			}
			var success = user.Password == new PasswordService(password, user.Salt).EncryptedPassword;
			if (!success) {
				return LoginStatus.GetLoginStatus(SignInStatus.InvalidPassword, user.Id);
			}
			return SignIn(this.ConvertToUser(user), rememberMe);
		}

		public LoginStatus SignIn(User user, bool rememberMe = false) {
			this.SignOut();
			if (user == null) {
				return LoginStatus.GetLoginStatus(SignInStatus.InvalidUsername);
			}
			if (!user.IsActive) {
				return LoginStatus.GetLoginStatus(SignInStatus.UserNotActive, user.Id);
			}
			this.AuthenticationPersistenceService.PersistUserInWebContext(user, rememberMe);
			return LoginStatus.GetLoginStatus(SignInStatus.Success, user.Id);
		}

		/// <summary>
		/// Later we may make this go back to a proper auth token of some kind, but for now it is simply a shared key with ticks on it to ensure that the request isn't old.
		/// </summary>
		/// <param name="authenticationTokenSlug"></param>
		/// <param name="sharedKey"></param>
		/// <returns></returns>
		public LoginStatus SignIn_ViaToken(string authenticationTokenSlug, string sharedKey) {
			var authToken = AuthTokenService.ParseAuthenticationToken(authenticationTokenSlug);
			if (authToken != sharedKey && !authToken.Equals(sharedKey)) {
				return LoginStatus.GetLoginStatus(SignInStatus.InvalidAuthToken);
			}
			//var user = this.UserAuthTokensService.Authenticate(authToken);
			//for now, we'll use the email address in the AdminSendFrom and login as that user.
			var emailAddress = Config.Setting(ConfigKey.AdminSendFrom.ToString());
			var user = this.UserRepository.GetByEmailAddress(emailAddress);
			if (user == null || user.Id == 0) {
				return new LoginStatus {
					Id = 0,
					Message = "Invalid authentication token.",
					Status = SignInStatus.InvalidAuthToken
				};
			}
			return new LoginStatus {
				Id = user.Id,
				Message = "Success.",
				Status = SignInStatus.Success
			};
		}

		public void SignOut() {
			AuthenticationPersistenceService.SignOut();
		}

		public bool DoesUserExist(string emailAddress) {
			var user = GetByEmailAddress(emailAddress);
			if (user != null) {
				return true;
			}
			return false;
		}

		public User Current() {
			var user = AuthenticationPersistenceService.RetrieveUserFromWebContext(this);
			return user ?? GuestUser();
		}

		public long CurrentUserId() {
			var user = AuthenticationPersistenceService.RetrieveUserFromWebContext(this);
			return user == null ? GuestUser().Id : user.Id;
		}

		public bool IsLoggedIn() {
			var user = Current();
			return user != null && user.Id != 0;
		}

		public bool IsInRole(SystemRole systemRole) {
			var user = this.Current();
			if (user == null || user.Id == 0) {
				return false;
			}
			return UserRepository.IsInRole(user.Id, systemRole.ToString());
		}

		public bool IsInRole(List<SystemRole> systemRoles) {
			var user = this.Current();
			if (user == null || user.Id == 0) {
				return false;
			}
			foreach (var systemRole in systemRoles) {
				var isInRole = UserRepository.IsInRole(user.Id, systemRole.ToString());
				if (isInRole) {
					return true;
				}
			}
			return false;
		}

		public bool IsInRole(long userId, SystemRole systemRole) {
			return UserRepository.IsInRole(userId, systemRole.ToString());
		}

		public AuthorizationResult Authorize(bool systemAdminAccessRequired) {
			var user = Current();
			if (user == null || user.Id == 0) {
				return new AuthorizationResult { AccessFailureReason = AccessFailureReason.EmptyUser };
			}

			if (systemAdminAccessRequired) {
				return new AuthorizationResult { AccessFailureReason = AccessFailureReason.NonSystemAdmin };
			}

			bool hasAccess = checkPermission(user);
			if (!hasAccess) {
				return new AuthorizationResult { AccessFailureReason = AccessFailureReason.AccessDenied };
			}

			return new AuthorizationResult();
		}

		public AuthorizationResult Authorize(AuthorizationContext filterContext, bool systemAdminAccessRequired) {
			var authResult = this.Authorize(systemAdminAccessRequired);
			if (authResult.AccessFailureReason == AccessFailureReason.EmptyUser) {
				var view = filterContext.HttpContext.Request.Path;
				if (view.Contains("Account/Login")) {
					return new AuthorizationResult();//let him in
				}
			}
			return authResult;
		}

		private bool checkPermission(User user) {//are we gonna need this later?
			return true;
		}

		public static User GuestUser() {
			//this user doesn't have an email address, and therefore cannot be persisted. That is good.
			var user = new User {
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				IsActive = true
			};
			user.CreatedById = user.Id;
			user.UpdatedById = user.Id;
			return user;
		}

		public GetUserByEmailAddress_Result GetByEmailAddress(string emailAddress) {
			if (!string.IsNullOrEmpty(emailAddress)) {
				return UserRepository.GetByEmailAddress(emailAddress);
			}
			return null;
		}

		public User Get(long id) {
			return UserRepository.Get(id);
		}

		public User GetCurrentUser_ThrowErrorIfUserDoesNotExist() {
			var user = this.Current();
			if (user == null || user.Id == 0) {
				throw Exceptions.CurrentUserDoesNotExist;
			}
			return user;
		}

		public bool UserLoggedIn() {
			var user = this.Current();
			return !(user == null || user.Id == 0);
		}

		public bool RedirectToLoginIfNotLoggedIn(HttpContextBase httpContextBase) {
			var isLoggedIn = this.UserLoggedIn();
			if (!isLoggedIn) {
				this.RedirectToLogin(httpContextBase);
				return false;
			}
			return true;
		}

		public void RedirectToLogin(HttpContextBase httpContextBase) {
			var loginURL = VirtualPathUtility.ToAbsolute(Config.Setting(ConfigKey.Membership_LoginURL.ToString()));
			httpContextBase.Response.Redirect(loginURL, true);
		}

		public void RedirectToUnauthorized(HttpContextBase httpContextBase) {
			var loginURL = VirtualPathUtility.ToAbsolute(Config.Setting(ConfigKey.Membership_UnauthorizedURL.ToString()));
			httpContextBase.Response.Redirect(loginURL, true);
		}
		public User ConvertToUser(GetUserByEmailAddress_Result getUserByEmailAddress) {
			if (getUserByEmailAddress == null) {
				return new User();
			}
			var user = new User();
			user.InjectFrom(getUserByEmailAddress);
			return user;
		}
	}
}