using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;
using Data.Models.ViewModels;

namespace Business.Services {
	public interface IProjectService {
		TransactionResponse Create(Project model);
		TransactionResponse Update(Project model);
		TransactionResponse Delete(long id);
		IEnumerable<Project> GetUserProjects(long userId);
		ProjectDetailViewModel GetProjectDetailViewModel(long id, long userId);
	}
}