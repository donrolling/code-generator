using Business.Common;
using Business.Common;
using Business.Common.Logging;
using Business.Service.Interfaces;
using Data.Repository.Interfaces;

namespace Business.Services.Membership {
	public class UserPermissionService : ServiceBase, IUserPermissionService {
		public IUserRepository UserRepository { get; set; }

		public UserPermissionService(IUserRepository userRepository, ILogger logger) : base(logger) {
			UserRepository = userRepository;
		}
		
		public bool IsInRole(long userId, string systemRole) {
			return UserRepository.IsInRole(userId, systemRole);
		}
	}
}
