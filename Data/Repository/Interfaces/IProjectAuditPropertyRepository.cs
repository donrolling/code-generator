using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IProjectAuditPropertyRepository : IRepository {	
		TransactionResponse Create(ProjectAuditProperty projectAuditProperty);
		TransactionResponse Update(ProjectAuditProperty projectAuditProperty);
		TransactionResponse Delete(long projectId);
	}
}