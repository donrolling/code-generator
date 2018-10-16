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
	public class LanguageDapperRepository : DapperRepository, ILanguageRepository {	
		public LanguageDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(Language language) {
			var sql = "Execute [dbo].[Language_Insert] @Name, @FileExtension, @IsActive, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			
			_params.Add("Name", language.Name);
			_params.Add("FileExtension", language.FileExtension);
			_params.Add("IsActive", language.IsActive);
			_params.Add("CreatedById", language.CreatedById);
			_params.Add("CreatedDate", language.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(Language language) {
			var sql = "Execute [dbo].[Language_Update] @Id, @Name, @FileExtension, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, language);
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[Language_Delete] @Id";
			return base.Execute(sql, new { Id = id });
		}

		public Language SelectById(long id) {
			return new Language_SelectById_Function().CallFunction(this, id);
		}

		public IEnumerable<Language> ReadAll(long userId, bool readActive = true, bool readInactive = false) {
			return new Language_ReadAll_Function().CallFunction<Language>(this, readActive, readInactive);
		}
	}
}