using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class TemplateService : ITemplateService {
		public ITemplateRepository TemplateRepository { get; set; }

		public TemplateService(ITemplateRepository templateRepository) {
			this.TemplateRepository = templateRepository;
		}

		public TransactionResponse Create(Template template){
			var result = this.TemplateRepository.Create(template);
			return result;
		}

		public TransactionResponse Update(Template template){
			var result = this.TemplateRepository.Update(template);
			return result;
		}

		public TransactionResponse Delete(long id){
			var result = this.TemplateRepository.Delete(id);
			return result;
		}

		public Template Get(long id, long userId) {
			//todo permissions
			return this.TemplateRepository.Get(id);
		}
	}
}