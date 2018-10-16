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
	public class ProjectTemplateDapperRepository : DapperRepository, IProjectTemplateRepository {	
		public ProjectTemplateDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(ProjectTemplate projecttemplate) {
			var sql = "Execute [dbo].[ProjectTemplate_Insert] @IsActive, @CreatedById, @CreatedDate, @UpdatedById, @UpdatedDate, @ProjectId = @ProjectId OUTPUT";
			var _params = new DynamicParameters();
			_params.Add("IsActive", projecttemplate.IsActive);
			_params.Add("CreatedById", projecttemplate.CreatedById);
			_params.Add("CreatedDate", projecttemplate.CreatedDate);
			_params.Add("UpdatedById", projecttemplate.UpdatedById);
			_params.Add("UpdatedDate", projecttemplate.UpdatedDate);
			_params.Add("ProjectId", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(ProjectTemplate projecttemplate) {
			var sql = "Execute [dbo].[ProjectTemplate_Update] @ProjectId, @TemplateId, @IsActive, @CreatedById, @CreatedDate, @UpdatedById, @UpdatedDate";
			var result = base.Execute(sql, projecttemplate);
			return result;
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[ProjectTemplate_Delete] @Id";
			var result = base.Execute(sql, new { Id = id });
			return result;
		}

		public IEnumerable<Template> ReadAll(long projectId, bool readActive = true, bool readInactive = false) {
			return new ProjectTemplate_ReadAll_Function().CallFunction(this, projectId, readActive, readInactive);
		}
	}
}