using System;
using Business.Common.Responses;

namespace Business.Common.Logging {
	public interface IRepositoryLogger {
		string Log(ActionType actionType, string message);
		string Log(ActionType actionType, Exception ex, string additionalInformation);
		string Log(ActionType actionType, Exception ex, string type, dynamic param, string additionalInformation);
		string Log<T>(ActionType actionType, Exception ex, dynamic param, string additionalInformation) where T : class;
		string Log<T>(ActionType actionType, Exception ex, string additionalInformation) where T : class;
	}
}
