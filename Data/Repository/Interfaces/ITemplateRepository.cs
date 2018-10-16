using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Data.Repository.Interfaces {
	public interface ITemplateRepository : IRepository {	
		TransactionResponse Create(Template template);
		TransactionResponse Update(Template template);
		TransactionResponse Delete(long id);
		Template Get(long id);
	}
}