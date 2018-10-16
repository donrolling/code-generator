using System;
using System.Reflection;
using System.Text;
using Business.Common.Responses;
using log4net;

namespace Business.Common.Logging {
	public class RepositoryLogger : IRepositoryLogger {
		public string Log<T>(ActionType actionType, Exception ex, string additionalInformation) where T : class {
			return Log<T>(actionType, ex, null, additionalInformation);
		}	

		public string Log<T>(ActionType actionType, Exception ex, dynamic param, string additionalInformation) where T : class {
			var type = typeof(T);
			return Log(actionType, ex, type.Name, param, additionalInformation);
		}

		public string Log(ActionType actionType, Exception ex, string additionalInformation) {
			return Log(actionType, ex, null, null, additionalInformation);
		}
		
		public string Log(ActionType actionType, Exception ex, string type, dynamic param, string additionalInformation) {
			var errorType = actionType.ToString();
			var message = string.Concat(
							"Respository Error. \nCommand '[", errorType, "]' ",
							string.IsNullOrEmpty(type) ? "for unknown type" : string.Concat("for type ", type),
							": \n\t",
							ex.Message, 
							string.IsNullOrEmpty(additionalInformation) ? "" : string.Concat("\n\t\tAdditional Information: ", additionalInformation)
							);
			if (param != null) {
				var pMessage = new StringBuilder();
				pMessage.Append("\n\tParameter Information:\n");
				pMessage.Append(getParamInfo(param));
				message += pMessage;
			}

			var log = LogManager.GetLogger("Respository");
			log.Error(message);
			return message;
		}

		private string getParamInfo(dynamic parameters) {
			var sb = new StringBuilder();
			foreach(var prop in parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
				var pattern = "{0} : {1}";
				sb.AppendLine(string.Format(pattern, prop.Name, prop.GetValue(parameters, null)));
			}
			return sb.ToString();
		}

		public string Log(ActionType actionType, string message) {
			var log = LogManager.GetLogger("Respository");
			var errorType = actionType.ToString();
			var msg = string.Concat(
							"Respository Error. \nCommand '[", errorType, "]' ",
							string.IsNullOrEmpty(message) ? "" : string.Concat("\n\t\tMessage: ", message));
			log.Error(msg);
			return msg;
		}
	}
}