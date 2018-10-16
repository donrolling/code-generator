using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface ITemplateImportRepository : IRepository {	
		TransactionResponse Create(TemplateImport templateimport);
		TransactionResponse Update(TemplateImport templateimport);
		TransactionResponse Delete(long id);
	}
}