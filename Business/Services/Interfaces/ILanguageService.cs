using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface ILanguageService {
		TransactionResponse Create(Language language);
		TransactionResponse Update(Language language);
		TransactionResponse Delete(long id);
	}
}