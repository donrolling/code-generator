using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface ITemplateService {
		TransactionResponse Create(Template model);
		TransactionResponse Update(Template model);
		TransactionResponse Delete(long id);
		Template Get(long id, long userId);
	}
}