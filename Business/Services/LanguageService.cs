using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class LanguageService : ILanguageService {		
		public ILanguageRepository LanguageRepository { get; set; }

		public LanguageService(ILanguageRepository languageRepository) {
			this.LanguageRepository = languageRepository;
		}

		public TransactionResponse Create(Language language){
			var result = this.LanguageRepository.Create(language);
			return result;
		}

		public TransactionResponse Update(Language language){
			var result = this.LanguageRepository.Update(language);
			return result;
		}
		
		public TransactionResponse Delete(long id){
			var result = this.LanguageRepository.Delete(id);
			return result;
		}
	}
}