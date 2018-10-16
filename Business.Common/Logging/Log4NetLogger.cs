using System;
using log4net;

namespace Business.Common.Logging {
	public class Log4NetLogger : ILogger {
		public const string DefaultLoggerName = "Business.Common";
		
		public ILog Logger(string name = "") { 
			return LogManager.GetLogger(getLoggerName(name));
		}

		public ILog Logger(Type	type) { 
			return LogManager.GetLogger(type);
		}

		public string Log(LogSeverity logSeverity, string message, string loggerName = ""){
			log(logSeverity, message, loggerName);
			return message;
		}

		public string Log(LogSeverity logSeverity, Exception ex, string loggerName = "") {
			return Log(logSeverity, ex, string.Empty, loggerName);
		}

		public string Log(LogSeverity logSeverity, Exception ex, string message, string loggerName = ""){
			var logEntry = string.Concat(getMessageFromException(ex), getAdditionalInformation(message));
			log(logSeverity, logEntry, loggerName);
			return logEntry;
		}

		public string Log(LogSeverity logSeverity, WebCommunicatorResponses statusCode, string command, string additionalInformation, string loggerName = "") {
			var logEntry = string.Concat(
							"Windows Service Error. \nCommand '[", command, "]': \n\t",
							"Http Status Code: ", statusCode.ToString(), " \n\t",
							string.IsNullOrEmpty(additionalInformation) ? "" : string.Concat("\n\t\tAdditional Information: ", additionalInformation)
							);
			log(logSeverity, logEntry, loggerName);
			return logEntry;
		}

		private void log(LogSeverity logSeverity, string message, string loggerName = "") {
			var log = Logger(loggerName);
			switch (logSeverity) {
				case LogSeverity.Debug:
					log.Debug(message);
					break;
				case LogSeverity.Error:
					log.Error(message);
					break;
				case LogSeverity.Warn:
					log.Warn(message);
					break;
				case LogSeverity.Info:
					log.Info(message);
					break;
				case LogSeverity.Fatal:
					log.Fatal(message);
					break;
				default:
					log.Error(message);
					break;
			}
		}

		private string getLoggerName(string loggerName = "") { 
			return string.IsNullOrEmpty(loggerName) ? DefaultLoggerName : loggerName;
		}

		private string getMessageFromException(Exception ex) {
			return string.Concat("Error: \nCommand '[", ex.TargetSite.Name, "]': \n\t", ex.Message, "\n\t", ex.StackTrace.ToString());
		}

		private string getAdditionalInformation(string message) {
			return string.IsNullOrEmpty(message) ? "" : string.Concat("\n\t\tAdditional Information: ", message);
		}
	}
}