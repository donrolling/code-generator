using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface ILanguageRepository : IRepository {	
		TransactionResponse Create(Language language);
		TransactionResponse Update(Language language);
		TransactionResponse Delete(long id);
	}
}