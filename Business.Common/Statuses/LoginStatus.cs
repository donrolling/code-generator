namespace Business.Common.Statuses {
	public class LoginStatus {
		public SignInStatus Status { get; set; }
		public long Id { get; set; }
		public string Message { get; set; }

		public static LoginStatus GetLoginStatus(SignInStatus loginStatus){
			return GetLoginStatus(loginStatus, 0);
		}
		public static LoginStatus GetLoginStatus(SignInStatus loginStatus, long id){
			var status = new LoginStatus { Status = loginStatus, Id = id };
			
			var message = string.Empty;
			switch (loginStatus) {
				case SignInStatus.Success:
					message = "Success.";
					break;
				case SignInStatus.Error:
					message = "Unknown error.";
					break;
				case SignInStatus.Failure:
				case SignInStatus.InvalidPassword:
				case SignInStatus.UsernameNotFound:
					message = "Username or password is not correct.";
					break;
			}
			status.Message = message;

			return status;
		}
	}
}