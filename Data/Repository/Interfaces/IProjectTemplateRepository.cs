using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IProjectTemplateRepository : IRepository {	
		TransactionResponse Create(ProjectTemplate projecttemplate);
		TransactionResponse Update(ProjectTemplate projecttemplate);
		TransactionResponse Delete(long id);
		IEnumerable<Template> ReadAll(long projectId, bool readActive = true, bool readInactive = false);
	}
}