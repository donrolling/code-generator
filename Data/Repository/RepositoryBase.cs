using Business.Common;
using Business.Common.Logging;

namespace Data.Repository {
	public class RepositoryBase {
		public IRepositoryLogger Logger { get; set; }

		public RepositoryBase(IRepositoryLogger logger) {
			Logger = logger;
		}
	}
}
