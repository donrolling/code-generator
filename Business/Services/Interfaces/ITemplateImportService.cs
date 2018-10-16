using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface ITemplateImportService {
		TransactionResponse Create(TemplateImport model);
		TransactionResponse Update(TemplateImport model);
		TransactionResponse Delete(long id);
	}
}