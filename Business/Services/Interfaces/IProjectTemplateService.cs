using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IProjectTemplateService {
		TransactionResponse Create(ProjectTemplate model);
		TransactionResponse Update(ProjectTemplate model);
		TransactionResponse Delete(long projectId);
		IEnumerable<Template> GetByProject(long projectId);
	}
}