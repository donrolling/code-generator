using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IUserProjectRoleRepository : IRepository {	
		TransactionResponse Create(UserProjectRole userProjectRole);
		TransactionResponse Update(UserProjectRole userProjectRole);
		TransactionResponse Delete(long userId);
	}
}