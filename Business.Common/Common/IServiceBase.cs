using Business.Common.Logging;

namespace Business.Common {
	public interface IServiceBase {
		ILogger Logger { get; }
	}
}
