using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Xml.Linq;

namespace Business.Services.Notification {
	public static class MessagePreparation {
		public static MailMessage GetMailMessage(string body, string subject, string from, string to, string cc, string bcc, bool isBodyHtml) {
			return GetMailMessage(body, subject, from, to, cc, bcc, isBodyHtml, null);
		}

		public static MailMessage GetMailMessage(string body, string subject, string from, string to, string cc, string bcc, bool isBodyHtml, List<Attachment> attachments) {
			var emailMessage = new MailMessage {
				Body = body,
				Subject = subject,
				IsBodyHtml = isBodyHtml
			};

			emailMessage.From = new MailAddress(from);
			prepareAddresses(to).Select(a => a).ToList().ForEach(a => emailMessage.To.Add(a));
			if (!string.IsNullOrEmpty(cc)) { prepareAddresses(cc).Select(a => a).ToList().ForEach(a => emailMessage.CC.Add(a)); }
			if (!string.IsNullOrEmpty(bcc)) { prepareAddresses(bcc).Select(a => a).ToList().ForEach(a => emailMessage.Bcc.Add(a)); }

			return emailMessage;
		}
		
		public static MailMessage PrepareXMLEmailTemplate(MessageTemplates messageTemplates, string toList, string fromAddress) {
			return PrepareXMLEmailTemplate(messageTemplates, toList, fromAddress, string.Empty, string.Empty, true);
		}
		public static MailMessage PrepareXMLEmailTemplate(MessageTemplates messageTemplate, string to, string from, string cc, string bcc, bool isBodyHtml) {
			var path = GetPathFromMessageTemplate(messageTemplate);
            var data = XDocument.Load(path);

			var body = (from d in data.Descendants() where d.Name == "Message" select d.Value).FirstOrDefault();
            var subject = (from d in data.Descendants() where d.Name == "Subject" select d.Value).FirstOrDefault();
			
			var emailMessage = new MailMessage {
				Body = body,
				Subject = subject,
				IsBodyHtml = isBodyHtml
			};
			emailMessage.From = new MailAddress(from);

			prepareAddresses(to).Select(a => a).ToList().ForEach(a => emailMessage.To.Add(a));

			if (string.IsNullOrEmpty(cc)) {
				cc = (from d in data.Descendants() where d.Name == "CcAddressList" select d.Value).FirstOrDefault() == null ?
				(from d in data.Descendants() where d.Name == "CcAddressList" select d.Value).FirstOrDefault()
				: string.Empty;
			}
			prepareAddresses(cc).Select(a => a).ToList().ForEach(a => emailMessage.CC.Add(a));

			if (string.IsNullOrEmpty(bcc)) {
				bcc = (from d in data.Descendants() where d.Name == "BccAddressList" select d.Value).FirstOrDefault() == null ?
				(from d in data.Descendants() where d.Name == "BccAddressList" select d.Value).FirstOrDefault()
				: string.Empty;
			}
			prepareAddresses(bcc).Select(a => a).ToList().ForEach(a => emailMessage.Bcc.Add(a));

            return emailMessage;
        }

		public static string GetPathFromMessageTemplate(MessageTemplates messageTemplate) {
			var basepath = getBasePath();
			var filename = string.Concat(messageTemplate.ToString(), ".xml");
			var path = Path.Combine(basepath, filename);
			return path;
		}

		private static string getBasePath() { 
			var tempBasepath = Assembly.GetCallingAssembly().Location;
			var pos = tempBasepath.LastIndexOf(@"\");
			var basepath = tempBasepath.Substring(0, pos + 1);
			return Path.Combine(basepath, @"Services\Notification\Templates\");
		}

		private static MailAddressCollection prepareAddresses(string addresses) {
			var mailAddressCollection = new MailAddressCollection();
			if (!string.IsNullOrEmpty(addresses)) {
				addresses = addresses.Replace(";", ",");
				if (addresses.Contains(",")) {
					List<string> addressList = new List<string>(addresses.Split(Convert.ToChar(",")));
					foreach (var address in addressList) {
						var mailAddress = new MailAddress(address);
						mailAddressCollection.Add(mailAddress);
					}
				} else {
					var mailAddress = new MailAddress(addresses);
					mailAddressCollection.Add(mailAddress);
				}
			}
			return mailAddressCollection;
		}

		public static EmailMessageSendReciept ValidateInputs(MailMessage mailMessage) {
			EmailMessageSendReciept emailMessageSendReciept = new EmailMessageSendReciept();
			if (string.IsNullOrEmpty(mailMessage.Subject)) {
				emailMessageSendReciept.AddMessage("Subject is empty.", MailMessageResultType.Error);
			}
			if (mailMessage.To.Count == 0) {
				emailMessageSendReciept.AddMessage("Email recipient is empty.", MailMessageResultType.Error);
			}
			if (string.IsNullOrEmpty(mailMessage.From.Address)) {
				emailMessageSendReciept.AddMessage("Email sender is empty.", MailMessageResultType.Error);
			}
			if (string.IsNullOrEmpty(mailMessage.Body)) {
				emailMessageSendReciept.AddMessage("Email message body is missing", MailMessageResultType.Error);
			}
			return emailMessageSendReciept;
		}
	}
}
