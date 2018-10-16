using System;
using System.Collections.Generic;
using System.Data;
using Business.Common.Logging;
using Business.Common.Responses;
using Dapper;
using Data.Models.Entities;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.Dapper {
	public class TemplateImportDapperRepository : DapperRepository, ITemplateImportRepository {	
		public TemplateImportDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(TemplateImport templateimport) {
			var sql = "Execute [dbo].[TemplateImport_Insert] @TemplateId, @IsActive, @CreatedById, @CreatedDate, @UpdatedById, @UpdatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			_params.Add("TemplateId", templateimport.TemplateId);
			_params.Add("IsActive", templateimport.IsActive);
			_params.Add("CreatedById", templateimport.CreatedById);
			_params.Add("CreatedDate", templateimport.CreatedDate);
			_params.Add("UpdatedById", templateimport.UpdatedById);
			_params.Add("UpdatedDate", templateimport.UpdatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(TemplateImport templateimport) {
			var sql = "Execute [dbo].[TemplateImport_Update] @Id, @TemplateId, @IsActive, @CreatedById, @CreatedDate, @UpdatedById, @UpdatedDate";
			var result = base.Execute(sql, templateimport);
			return result;
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[TemplateImport_Delete] @Id";
			var result = base.Execute(sql, new { Id = id });
			return result;
		}
	}
}