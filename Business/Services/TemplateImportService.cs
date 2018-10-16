using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class TemplateImportService : ITemplateImportService {
		public ITemplateImportRepository TemplateImportRepository { get; set; }

		public TemplateImportService(ITemplateImportRepository templateimportRepository) {
			this.TemplateImportRepository = templateimportRepository;
		}

		public TransactionResponse Create(TemplateImport templateimport){
			var result = this.TemplateImportRepository.Create(templateimport);
			return result;
		}

		public TransactionResponse Update(TemplateImport templateimport){
			var result = this.TemplateImportRepository.Update(templateimport);
			return result;
		}

		public TransactionResponse Delete(long id){
			var result = this.TemplateImportRepository.Delete(id);
			return result;
		}
	}
}