using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class RoleService : IRoleService {		
		public IRoleRepository RoleRepository { get; set; }

		public RoleService(IRoleRepository roleRepository) {
			this.RoleRepository = roleRepository;
		}

		public TransactionResponse Create(Role role){
			var result = this.RoleRepository.Create(role);
			return result;
		}

		public TransactionResponse Update(Role role){
			var result = this.RoleRepository.Update(role);
			return result;
		}

		public TransactionResponse Delete(long id){
			var result = this.RoleRepository.Delete(id);
			return result;
		}
	}
}