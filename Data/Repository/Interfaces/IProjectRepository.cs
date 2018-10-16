using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IProjectRepository : IRepository {	
		TransactionResponse Create(Project project);
		TransactionResponse Update(Project project);
		TransactionResponse Delete(long id);
		Project SelectById(long id);
		IEnumerable<Project> ReadAll(long userId, bool readActive = true, bool readInactive = false);
	}
}