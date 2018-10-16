using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class ProjectTemplateService : IProjectTemplateService {
		public IProjectTemplateRepository ProjectTemplateRepository { get; set; }

		public ProjectTemplateService(IProjectTemplateRepository projecttemplateRepository) {
			this.ProjectTemplateRepository = projecttemplateRepository;
		}

		public TransactionResponse Create(ProjectTemplate projecttemplate){
			var result = this.ProjectTemplateRepository.Create(projecttemplate);
			return result;
		}

		public TransactionResponse Update(ProjectTemplate projecttemplate){
			var result = this.ProjectTemplateRepository.Update(projecttemplate);
			return result;
		}

		public TransactionResponse Delete(long projectId){
			var result = this.ProjectTemplateRepository.Delete(projectId);
			return result;
		}

		public IEnumerable<Template> GetByProject(long projectId) {
			var result = this.ProjectTemplateRepository.ReadAll(projectId);
			if (result == null) {
				return new List<Template>();
			}
			return result;
		}
	}
}