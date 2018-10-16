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
	public class DataTypeDapperRepository : DapperRepository, IDataTypeRepository {	
		public DataTypeDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }
		
		public TransactionResponse Create(DataType datatype) {
			var sql = "Execute [dbo].[DataType_Insert] @Name, @DotNetEnumValue, @IsActive, @CreatedById, @CreatedDate, @UpdatedById, @UpdatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			_params.Add("Name", datatype.Name);
			_params.Add("DotNetEnumValue", datatype.DotNetEnumValue);
			_params.Add("IsActive", datatype.IsActive);
			_params.Add("CreatedById", datatype.CreatedById);
			_params.Add("CreatedDate", datatype.CreatedDate);
			_params.Add("UpdatedById", datatype.UpdatedById);
			_params.Add("UpdatedDate", datatype.UpdatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(DataType datatype) {
			var sql = "Execute [dbo].[DataType_Update] @Id, @Name, @DotNetEnumValue, @IsActive, @CreatedById, @CreatedDate, @UpdatedById, @UpdatedDate";
			var result = base.Execute(sql, datatype);
			return result;
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[DataType_Delete] @Id";
			var result = base.Execute(sql, new { Id = id });
			return result;
		}
		public Property SelectById(long id) {
			return new Property_SelectById_Function().CallFunction(this, id);
		}

		public IEnumerable<Property> ReadAll(long userId, bool readActive = true, bool readInactive = false) {
			return new Property_ReadAll_Function().CallFunction<Property>(this, readActive, readInactive);
		}
	}
}