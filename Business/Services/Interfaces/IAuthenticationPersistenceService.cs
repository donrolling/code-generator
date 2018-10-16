using Data.Models.Entities;

namespace Business.Service.Interfaces {
	public interface IAuthenticationPersistenceService {
		void PersistUserInWebContext(User user, bool rememberMe);
		User RetrieveUserFromWebContext(IMembershipService membershipService);
		void SignOut();
	}
}
