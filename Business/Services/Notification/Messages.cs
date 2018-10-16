using System;
using System.Collections.Generic;
using Business.Common.Logging;

namespace Business.Services.Notification {
	public static class Messages {
		public static BusinessMailMessage ResetPasswordEmail(string emailAddress, string url, string password, string fromAddress) {
			var template = MessageTemplates.PasswordReset;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, emailAddress, fromAddress);
			mailMessage.Body = mailMessage.Body.Replace("[[UserName]]", emailAddress).Replace("[[Password]]", password).Replace("[[URL]]", url);
			return new BusinessMailMessage(mailMessage, template);
		}

		public static BusinessMailMessage NewUserEmail(string emailAddress, string url, string password, string fromAddress) {
			var template = MessageTemplates.NewUser;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, emailAddress, fromAddress);
			mailMessage.Body = mailMessage.Body.Replace("[[UserName]]", emailAddress).Replace("[[Password]]", password).Replace("[[URL]]", url);
			return new BusinessMailMessage(mailMessage, template);
		}

		public static BusinessMailMessage ServiceStarted(string service, string toList, string fromAddress) {
			var template = MessageTemplates.ServiceStarted;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, toList, fromAddress);
			mailMessage.Body = mailMessage.Body.Replace("[[Service]]", service).Replace("[[DateStamp]]", DateTime.Now.ToString()).Replace("[[ComputerName]]", MachineInfo.ComputerName).Replace("[[ServerIP]]", MachineInfo.IP_Address);
			return new BusinessMailMessage(mailMessage, template);
		}

		public static BusinessMailMessage ServiceStopped(string service, string toList, string fromAddress) {
			var template = MessageTemplates.ServiceStopped;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, toList, fromAddress);
			mailMessage.Body = mailMessage.Body.Replace("[[Service]]", service).Replace("[[DateStamp]]", DateTime.Now.ToString()).Replace("[[ComputerName]]", MachineInfo.ComputerName).Replace("[[ServerIP]]", MachineInfo.IP_Address);
			return new BusinessMailMessage(mailMessage, template);
		}

		public static BusinessMailMessage UserEmailEmpty(string toList, string fromAddress, string computerName, string serverIP) {
			var template = MessageTemplates.UserEmailEmpty;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, toList, fromAddress);
			mailMessage.Body = mailMessage.Body.Replace("[[ComputerName]]", MachineInfo.ComputerName).Replace("[[ServerIP]]", MachineInfo.IP_Address);
			return new BusinessMailMessage(mailMessage, template);
		}

		public static BusinessMailMessage UserNotFound(string toList, string fromAddress) {
			var template = MessageTemplates.UserNotFound;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, toList, fromAddress);
			mailMessage.Body = mailMessage.Body.Replace("[[ComputerName]]", MachineInfo.ComputerName).Replace("[[ServerIP]]", MachineInfo.IP_Address);
			return new BusinessMailMessage(mailMessage, template);
		}

		public static BusinessMailMessage UserNotSystemAdmin(string toList, string fromAddress, string computerName, string serverIP) {
			var template = MessageTemplates.UserNotSystemAdmin;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, toList, fromAddress);
			mailMessage.Body = mailMessage.Body.Replace("[[ComputerName]]", MachineInfo.ComputerName).Replace("[[ServerIP]]", MachineInfo.IP_Address);
			return new BusinessMailMessage(mailMessage, template);
		}

		public static BusinessMailMessage GeneralError(string toList, string fromAddress, string computerName, string serverIP) {
			var template = MessageTemplates.GeneralError;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, toList, fromAddress);
			mailMessage.Body = mailMessage.Body.Replace("[[ErrorMessage]]", "Unknown").Replace("[[ComputerName]]", MachineInfo.ComputerName).Replace("[[ServerIP]]", MachineInfo.IP_Address);
			return new BusinessMailMessage(mailMessage, template);
		}

		public static BusinessMailMessage GeneralError(Exception ex, string toList, string fromAddress, string computerName, string serverIP) {
			var template = MessageTemplates.GeneralError;
			var mailMessage = MessagePreparation.PrepareXMLEmailTemplate(template, toList, fromAddress);
			var errorMessage = getErrorMessage(ex);
			mailMessage.Body = mailMessage.Body.Replace("[[ErrorMessage]]", errorMessage).Replace("[[ComputerName]]", MachineInfo.ComputerName).Replace("[[ServerIP]]", MachineInfo.IP_Address);
			return new BusinessMailMessage(mailMessage, template);
		}

		private static string getErrorMessage(Exception ex) {
			if (ex == null) { return string.Empty; }
			return string.Concat("<p>Error:<br />", ex.Message, "</p><p>Inner Exception:<br />", ex.InnerException == null ? string.Empty : ex.InnerException.Message, "</p><p>Stack Trace:<br />", ex.StackTrace, "</p>");
		}

		private static List<string> prepareEmailAddresses(string toList) { 
			var goodEmails = new List<string>();
			var emailAddresses = toList.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string email in emailAddresses) {
				var emailAddress = email.Replace("\n", string.Empty).Replace(" ", string.Empty);
				if (!string.IsNullOrEmpty(emailAddress)) {
					goodEmails.Add(emailAddress);
				}
			}
			return goodEmails;
		}
	}
}
