using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IProjectAuditPropertyService {
		TransactionResponse Create(ProjectAuditProperty model);
		TransactionResponse Update(ProjectAuditProperty model);
		TransactionResponse Delete(long projectId);
	}
}