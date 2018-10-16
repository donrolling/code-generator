using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IRoleRepository : IRepository {	
		TransactionResponse Create(Role role);
		TransactionResponse Update(Role role);
		TransactionResponse Delete(long id);
	}
}