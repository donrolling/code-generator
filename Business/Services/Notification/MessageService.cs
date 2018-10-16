using System;
using Business.Common;
using Business.Common;
using Business.Common.Logging;
using Business.Service.Interfaces;

namespace Business.Services.Notification {
	public class MessageService : ServiceBase, IMessageService {
		public IEmailService EmailService { get; set; }

		const string LoggerName = "MessageService";

		public MessageService(IEmailService emailService, ILogger logger) : base(logger) {
			this.EmailService = emailService;
		}

		public EmailMessageSendReciept GeneralError_Send(Exception ex, string toList, string fromAddress, string computerName, string serverIP) {
			var message = Messages.GeneralError(ex, toList, fromAddress, computerName, serverIP);
			return sendMessage(message);
		}

		public EmailMessageSendReciept GeneralError_Send(string toList, string fromAddress, string computerName, string serverIP) {
			var message = Messages.GeneralError(toList, fromAddress, computerName, serverIP);
			return sendMessage(message);
		}

		public EmailMessageSendReciept PasswordResetEmail_Send(string userEmail, string url, string password, string fromAddress) {
			var message = Messages.ResetPasswordEmail(userEmail, url, password, fromAddress);
			return sendMessage(message);
		}

		public EmailMessageSendReciept NewUserEmail_Send(string userEmail, string url, string password, string fromAddress) {
			var message = Messages.NewUserEmail(userEmail, url, password, fromAddress);
			return sendMessage(message);
		}

		public EmailMessageSendReciept UserEmailEmpty_Send(string toList, string fromAddress, string computerName, string serverIP) {
			var message = Messages.UserEmailEmpty(toList, fromAddress, computerName, serverIP);
			return sendMessage(message);
		}

		public EmailMessageSendReciept UserNotFound_Send(string toList, string fromAddress) {
			var message = Messages.UserNotFound(toList, fromAddress);
			return sendMessage(message);
		}

		public EmailMessageSendReciept UserNotSystemAdmin_Send(string toList, string fromAddress, string computerName, string serverIP) {
			var message = Messages.UserNotSystemAdmin(toList, fromAddress, computerName, serverIP);
			return sendMessage(message);
		}

		public EmailMessageSendReciept ServiceStarted(string serviceName, string toList, string fromAddress) {
			var message = Messages.ServiceStarted(serviceName, toList, fromAddress);
			return sendMessage(message);
		}

		public EmailMessageSendReciept ServiceStopped(string serviceName, string toList, string fromAddress) {
			var message = Messages.ServiceStopped(serviceName, toList, fromAddress);
			return sendMessage(message);
		}

		private EmailMessageSendReciept sendMessage(BusinessMailMessage message) { 
			var result = this.EmailService.SendEmail(message);
			if (!result.Success) {
				this.Logger.Log(LogSeverity.Warn, result.Message, LoggerName);
			}
			return result;
		}
	}
}