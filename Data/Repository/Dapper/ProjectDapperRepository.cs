using System;
using System.Collections.Generic;
using System.Data;
using Business.Common.Logging;
using Business.Common.Responses;
using Dapper;
using Data.Models.Entities;
using Data.Repository.Dapper;
using Data.Repository.FunctionDefinitions;
using Data.Repository.Interfaces;

namespace Data.Repository.Dapper {
	public class ProjectDapperRepository : DapperRepository, IProjectRepository {	
		public ProjectDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(Project project) {
			var sql = "Execute [dbo].[Project_Insert] @Name, @IsActive, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();

			_params.Add("Name", project.Name);
			_params.Add("IsActive", project.IsActive);
			_params.Add("CreatedById", project.CreatedById);
			_params.Add("CreatedDate", project.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(Project project) {
			var sql = "Execute [dbo].[Project_Update] @Id, @Name, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, project);
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[Project_Delete] @Id";
			return base.Execute(sql, new { Id = id });
		}

		public Project SelectById(long id) {
			return new Project_SelectById_Function().CallFunction(this, id);
		}

		public IEnumerable<Project> ReadAll(long userId, bool readActive = true, bool readInactive = false) {
			return new Project_ReadAll_Function().CallFunction(this, userId, readActive, readInactive);
		}
	}
}