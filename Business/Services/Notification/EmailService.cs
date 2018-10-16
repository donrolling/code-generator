using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Hosting;
using Business.Service.Interfaces;
using log4net;

namespace Business.Services.Notification {
	public class EmailService : IEmailService {
		public string SmtpServer { get; private set; }
		public string AdminEmail { get; private set; }

		private bool _isTest = false;
		private string _emailPath { get; set; }

		public EmailService(string smtpServer, string adminSendFrom, string emailPath = "", bool isTest = false) {
			SmtpServer = smtpServer;
			if (string.IsNullOrEmpty(SmtpServer)) {
				throw new Exception("SMTPServer is null.");
			}
			AdminEmail = adminSendFrom;
			if (string.IsNullOrEmpty(AdminEmail)) {
				throw new Exception("AdminEmail is null.");
			}

			try { //this doesn't need to succeed unless we're in test mode
				_emailPath = emailPath;
				if (string.IsNullOrEmpty(_emailPath)) {
					throw new Exception("AdminEmail is null.");
				}
			} catch { }
			_isTest = isTest;
		}
		
		public EmailMessageSendReciept SendEmail(BusinessMailMessage businessMailMessage) {
			var emailMessageSendReciept = MessagePreparation.ValidateInputs(businessMailMessage.MailMessage);
			if (string.IsNullOrEmpty(SmtpServer)) {
				emailMessageSendReciept.Messages.Add("SMTP Server value is empty.");
			}
			if (!emailMessageSendReciept.Success) {
				return emailMessageSendReciept;
			} 
			var smtpClient = setupSmtpClient();
			try {
				smtpClient.Send(businessMailMessage.MailMessage);
			} catch (Exception ex) {
				logErrorMessage(emailMessageSendReciept, businessMailMessage.MailMessage, ex);
				emailMessageSendReciept.AddMessage(ex.Message.ToString(), MailMessageResultType.Error);
			}

			return emailMessageSendReciept;
		}

		private SmtpClient setupSmtpClient() { 
			var smtpClient = new SmtpClient();
			smtpClient.Host = SmtpServer;

			if (_isTest && !string.IsNullOrEmpty(_emailPath)) {
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
				var emailPickupDirectory = _emailPath.Contains(':') ? _emailPath : HostingEnvironment.MapPath(_emailPath);
				if (!Directory.Exists(emailPickupDirectory)) {
					Directory.CreateDirectory(emailPickupDirectory);
				}
				smtpClient.PickupDirectoryLocation = emailPickupDirectory;
			}
			return smtpClient;
		}

		private void logErrorMessage(EmailMessageSendReciept emailMessageSendReciept, MailMessage mailMessage, Exception ex) { 
			emailMessageSendReciept.AddMessage("Exception: " + ex.Message, MailMessageResultType.Error);
			if (ex.InnerException != null && ex.InnerException.Message != null) {
				emailMessageSendReciept.AddMessage("Inner Exception: " + ex.InnerException.Message, MailMessageResultType.Error);
			}
			emailMessageSendReciept.AddMessage("Subject = " + mailMessage.Subject, MailMessageResultType.Error);
			emailMessageSendReciept.AddMessage("From Address = " + mailMessage.From, MailMessageResultType.Error);
			emailMessageSendReciept.AddMessage("Smtp Server = " + SmtpServer, MailMessageResultType.Error);
			emailMessageSendReciept.AddMessage("Is Html = " + mailMessage.IsBodyHtml, MailMessageResultType.Error);

			logError(ex, emailMessageSendReciept.Message);
		}		
		private string logError(Exception ex, string additionalInformation = "") {
			var message = string.Concat("Email Send Error. \n\t", ex.Message, string.IsNullOrEmpty(additionalInformation) ? "" : string.Concat("\n\t\tAdditional Information: ", additionalInformation));
			var log = LogManager.GetLogger(typeof(EmailService));
			log.Error(message);
			return message;
		}
	}
}