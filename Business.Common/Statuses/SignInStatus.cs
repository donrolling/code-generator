namespace Business.Common.Statuses {
	public enum SignInStatus {
		Success,
		Failure,
		Error,
		UsernameNotFound,
		UserNotActive,
		InvalidUsername,
		InvalidPassword,
		SuccessWithTempPassword,
		InvalidAuthToken
	}
}
