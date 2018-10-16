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
	public class DapperProjectAuditPropertyRepository : DapperRepository, IProjectAuditPropertyRepository {	
		public DapperProjectAuditPropertyRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(ProjectAuditProperty projectAuditProperty) {
			var sql = "Execute [dbo].[ProjectAuditProperty_Insert] @ProjectId, @PropertyId, @IsActive, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			
			_params.Add("ProjectId", projectAuditProperty.ProjectId);
			_params.Add("PropertyId", projectAuditProperty.PropertyId);
			_params.Add("IsActive", projectAuditProperty.IsActive);
			_params.Add("CreatedById", projectAuditProperty.CreatedById);
			_params.Add("CreatedDate", projectAuditProperty.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(ProjectAuditProperty projectAuditProperty) {
			var sql = "Execute [dbo].[ProjectAuditProperty_Update] @ProjectId, @PropertyId, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, projectAuditProperty);
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[ProjectAuditProperty_Delete] @Id";
			return base.Execute(sql, new { Id = id });
		}

		public ProjectAuditProperty SelectById(long projectId, long propertyId) {
			return new ProjectAuditProperty_SelectById_Function().CallFunction(this, projectId, propertyId);
		}

		public IEnumerable<ProjectAuditProperty> ReadAll(long userId, bool readActive = true, bool readInactive = false) {
			return new ProjectAuditProperty_ReadAll_Function().CallFunction<ProjectAuditProperty>(this, readActive, readInactive);
		}
	}
}