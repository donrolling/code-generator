using System;
using Business.Services.Notification;

namespace Business.Service.Interfaces {
	public interface IMessageService {
		EmailMessageSendReciept GeneralError_Send(Exception ex, string toList, string fromAddress, string computerName, string serverIP);
		EmailMessageSendReciept GeneralError_Send(string toList, string fromAddress, string computerName, string serverIP);
		EmailMessageSendReciept PasswordResetEmail_Send(string userEmail, string url, string password, string fromAddress);
		EmailMessageSendReciept NewUserEmail_Send(string userEmail, string url, string password, string fromAddress);
		EmailMessageSendReciept UserEmailEmpty_Send(string toList, string fromAddress, string computerName, string serverIP);
		EmailMessageSendReciept UserNotFound_Send(string toList, string fromAddress);
		EmailMessageSendReciept UserNotSystemAdmin_Send(string toList, string fromAddress, string computerName, string serverIP);
		EmailMessageSendReciept ServiceStarted(string serviceName, string toList, string fromAddress);
		EmailMessageSendReciept ServiceStopped(string serviceName, string toList, string fromAddress);
	}
}
