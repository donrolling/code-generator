namespace Business.Configuration {
	public enum ConnectionStringKey {
		Main
	}

	public enum ConfigKey {
		AppState,
		SMTPServer,
		LoginAddress,
		AdminSendTo,
		AdminSendFrom,
		EmailPath,
		Membership_LoginURL,
		Membership_UnauthorizedURL
	}	

	public enum EmailTemplate {
		HelloWorld
	}

	public enum OutputType {
		Zip,
		Files
	}
}