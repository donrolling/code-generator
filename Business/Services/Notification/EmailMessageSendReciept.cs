using System;
using System.Collections.Generic;

namespace Business.Services.Notification {
	public enum MailMessageResultType {
		Error = 0,
		Success = 1
	}

	public class EmailMessageSendReciept {
		private bool _isHtml = false;
		private bool _isHtmlList = true;

		public EmailMessageSendReciept() {
		}

		public EmailMessageSendReciept(bool isHtml) {
			_isHtml = isHtml;
		}

		public EmailMessageSendReciept(bool isHtml, bool isHtmlList) {
			_isHtml = isHtml;
			_isHtmlList = isHtmlList;
			if (_isHtmlList) {
				_isHtml = true;
			}
		}

		private bool _success = true;
		public bool Success {
			get { return _success; }
			set { _success = value; }
		}

		private string _message = string.Empty;
		public string Message {
			get {
				return messageToString();
			}
			set{
				_message = value;
			}
		}

		private List<string> _messages = new List<string>();
		public List<string> Messages {
			get { return _messages; }
			set{ _messages = value; }
		}

		public void AddMessage(List<string> messages, MailMessageResultType messageType) {
			foreach (string message in messages) {
				AddMessage(message, messageType);
			}
		}

		public void AddMessage(string message, MailMessageResultType messageType) {
			_messages.Add(message);
			switch (messageType) {
				case MailMessageResultType.Error:
					_success = false;
					break;
				case MailMessageResultType.Success: //never set success here because it is true by default and if any errors are added, then it should become and remain false.
					break;
			}
		}

		private string messageToString() {
			string retval = string.Empty;

			const string beginMessageListCharacter = "<ul>";
			const string endMessageListCharacter = "</ul>";

			string beginLineCharacter = string.Empty;
			if (_isHtml && _isHtmlList) {
				beginLineCharacter = "<li>";
			}

			string endLineCharacter;
			if (_isHtml) {
				if (_isHtmlList) {
					endLineCharacter = "</li>";
				} else {
					endLineCharacter = "<br />";
				}
			} else {
				endLineCharacter = Environment.NewLine;
			}

			int i = 1;
			foreach (string message in _messages) {
				if (_isHtml) {
					if (_isHtmlList) {
						if (i == 1) {
							retval += beginMessageListCharacter;
						}
						retval += beginLineCharacter;
					}
				}
				retval += message + endLineCharacter;
				if (_isHtml && _isHtmlList && i == _messages.Count) {
					retval += endMessageListCharacter;
				}
				i++;
			}

			return retval;
		}
	}

}
