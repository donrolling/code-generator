using Business.Services.Notification;

namespace Business.Service.Interfaces {
	public interface IEmailService {
		string AdminEmail { get; }
		string SmtpServer { get; }

		EmailMessageSendReciept SendEmail(BusinessMailMessage BusinessMailMessage);
	}
}