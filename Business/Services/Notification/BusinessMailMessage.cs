using System.Net.Mail;

namespace Business.Services.Notification {
	public class BusinessMailMessage {
		public MailMessage MailMessage { get; set; }
		public MessageTemplates MessageTemplate { get; set; }

		public BusinessMailMessage(MailMessage mailMessage, MessageTemplates messageTemplate) {
			MailMessage = mailMessage;
			MessageTemplate = messageTemplate;
		}
	}
}
