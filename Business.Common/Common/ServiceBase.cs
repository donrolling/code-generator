using Business.Common.Logging;

namespace Business.Common {
	public class ServiceBase : IServiceBase {
		public ILogger Logger { get; private set; }

		public ServiceBase(ILogger logger) {
			Logger = logger;
		}

		protected bool prepareForSave<T>(T entity, long currentUserId, bool isNew = false) where T : class {
			return Auditing.SetAuditFields<T>(entity, currentUserId, isNew);
		}
	}
}