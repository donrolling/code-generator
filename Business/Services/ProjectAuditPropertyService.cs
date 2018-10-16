using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class ProjectAuditPropertyService : IProjectAuditPropertyService {		
		public IProjectAuditPropertyRepository ProjectAuditPropertyRepository { get; set; }

		public ProjectAuditPropertyService(IProjectAuditPropertyRepository projectAuditPropertyRepository) {
			this.ProjectAuditPropertyRepository = projectAuditPropertyRepository;
		}

		public TransactionResponse Create(ProjectAuditProperty projectAuditProperty){
			var result = this.ProjectAuditPropertyRepository.Create(projectAuditProperty);
			return result;
		}

		public TransactionResponse Update(ProjectAuditProperty projectAuditProperty){
			var result = this.ProjectAuditPropertyRepository.Update(projectAuditProperty);
			return result;
		}

		public TransactionResponse Delete(long projectId){
			var result = this.ProjectAuditPropertyRepository.Delete(projectId);
			return result;
		}
	}
}