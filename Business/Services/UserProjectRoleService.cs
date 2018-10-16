using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {

	public class UserProjectRoleService : IUserProjectRoleService {		
		public IUserProjectRoleRepository UserProjectRoleRepository { get; set; }

		public UserProjectRoleService(IUserProjectRoleRepository userProjectRoleRepository) {
			this.UserProjectRoleRepository = userProjectRoleRepository;
		}

		public TransactionResponse Create(UserProjectRole userProjectRole){
			var result = this.UserProjectRoleRepository.Create(userProjectRole);
			return result;
		}

		public TransactionResponse Update(UserProjectRole userProjectRole){
			var result = this.UserProjectRoleRepository.Update(userProjectRole);
			return result;
		}

		public TransactionResponse Delete(long userId){
			var result = this.UserProjectRoleRepository.Delete(userId);
			return result;
		}
	}
}