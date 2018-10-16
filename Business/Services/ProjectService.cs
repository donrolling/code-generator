using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Business.Common.Statuses;
using Business.Service.Interfaces;
using Data.Models.Entities;
using Data.Models.ViewModels;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class ProjectService : IProjectService {
		public IMembershipService MembershipService { get; set; }
		public IProjectRepository ProjectRepository { get; set; }
		public IProjectTemplateService ProjectTemplateService { get; set; }

		public ProjectService(IMembershipService membershipService, IProjectRepository projectRepository, IProjectTemplateService projectTemplateService) {
			this.ProjectRepository = projectRepository;
			this.MembershipService = membershipService;
			this.ProjectTemplateService = projectTemplateService;
		}

		public Project Get(long id) {
			var project =  this.ProjectRepository.SelectById(id);
			//todo: check permissions here
			//if (project.UserId != this.MembershipService.CurrentUserId()) {
			//	return null;
			//}
			return project;
		}

		public TransactionResponse Create(Project project) {
			var result = this.ProjectRepository.Create(project);
			return result;
		}

		public TransactionResponse Update(Project project) {
			//todo: check permissions here
			//if (project.UserId != this.MembershipService.CurrentUserId()) {
			//	return TransactionResponse.GetTransactionResponse(ActionType.Update, Status.Failure, StatusDetail.Error, project.Id, "User cannot update projects that he does not own.");
			//}
			var result = this.ProjectRepository.Update(project);
			return result;
		}

		public TransactionResponse Delete(long id) {
			var project = this.Get(id);
			//todo: check permissions here
			//if (project.UserId != this.MembershipService.CurrentUserId()) {
			//	return TransactionResponse.GetTransactionResponse(ActionType.Update, Status.Failure, StatusDetail.Error, project.Id, "User cannot delete projects that he does not own.");
			//}
			var result = this.ProjectRepository.Delete(id);
			return result;
		}

		public IEnumerable<Project> GetUserProjects(long userId) {
			var projects = this.ProjectRepository.ReadAll(userId);
			projects = projects ?? new List<Project>();
			return projects;
		}

		public ProjectDetailViewModel GetProjectDetailViewModel(long id, long userId) {
			var result = new ProjectDetailViewModel();

			result.Project = this.Get(id);
			result.Templates = this.ProjectTemplateService.GetByProject(id);

			return result;
		}
	}
}