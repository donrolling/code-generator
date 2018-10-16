using System;

namespace Business.Common.Logging {
	public interface ILogger {
		string Log(LogSeverity logSeverity, string message, string loggerName = "");
		string Log(LogSeverity logSeverity, Exception ex, string loggerName = "");
		string Log(LogSeverity logSeverity, Exception ex, string message, string loggerName = "");
		string Log(LogSeverity logSeverity, WebCommunicatorResponses statusCode, string command, string additionalInformation, string loggerName = "");
	}
}
