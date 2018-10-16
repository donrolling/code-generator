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
	public class TemplateDapperRepository : DapperRepository, ITemplateRepository {	
		public TemplateDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public Template Get(long id) {
			throw new NotImplementedException();
		}

		public TransactionResponse Create(Template template) {
			var sql = "Execute [dbo].[Template_Insert] @ProjectId, @LanguageId, @Type, @Name, @Schema, @Namespace, @OutputFilename, @RelativeOutputPath, @RunOnce, @Text, @IsActive, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			
			_params.Add("ProjectId", template.ProjectId);
			_params.Add("LanguageId", template.LanguageId);
			_params.Add("Type", template.Type);
			_params.Add("Name", template.Name);
			_params.Add("Schema", template.Schema);
			_params.Add("Namespace", template.Namespace);
			_params.Add("OutputFilename", template.OutputFilename);
			_params.Add("RelativeOutputPath", template.RelativeOutputPath);
			_params.Add("RunOnce", template.RunOnce);
			_params.Add("Text", template.Text);
			_params.Add("IsActive", template.IsActive);
			_params.Add("CreatedById", template.CreatedById);
			_params.Add("CreatedDate", template.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(Template template) {
			var sql = "Execute [dbo].[Template_Update] @Id, @ProjectId, @LanguageId, @Type, @Name, @Schema, @Namespace, @OutputFilename, @RelativeOutputPath, @RunOnce, @Text, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, template);
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[Template_Delete] @Id";
			return base.Execute(sql, new { Id = id });
		}

		public Template SelectById(long id) {
			return new Template_SelectById_Function().CallFunction(this, id);
		}

		public IEnumerable<Template> ReadAll(long userId, bool readActive = true, bool readInactive = false) {
			return new Template_ReadAll_Function().CallFunction<Template>(this, readActive, readInactive);
		}
	}
}