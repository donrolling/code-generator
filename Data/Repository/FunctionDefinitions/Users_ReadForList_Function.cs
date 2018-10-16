using System.Collections.Generic;
using Business.Common.DataTables;
using Dapper;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {    
    public class Users_ReadForList_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "Users_ReadForList";

		public IEnumerable<Users_ReadForList_Result> CallFunction(IDapperRepository repository, long userId) {
			var signature = Users_ReadForList_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = Users_ReadForList_Parameters.Get_DataFunction_DynamicParameters(userId);
			return repository.Query<Users_ReadForList_Result>(sql, parameters);
		}

		public IPresentable<Users_ReadForList_Result> CallFunction_Paged(IDapperRepository repository, PageInfo pageInfo, long userId) {
			var signature = Users_ReadForList_Parameters.Get_DataFunction_Signature();
			var parameters = Users_ReadForList_Parameters.Get_DataFunction_DynamicParameters(userId);
			return base.Paged<Users_ReadForList_Result>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}
	
    public static class Users_ReadForList_Parameters {
		public const string UserId = "userId";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@userId");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long userId) {
			var parameters = new DynamicParameters();
			parameters.Add(UserId, userId);
			return parameters;
		}
	}
	
    public static class Users_ReadForList_Properties {
		public const string Id = "Id";
		public const string EmailAddress = "Email";
	}
	
	public class Users_ReadForList_Result {
		public long Id { get; set; }
		public string EmailAddress { get; set; }
	}
}