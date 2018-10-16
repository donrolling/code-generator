using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.DataTables;
using Dapper;
using Data.Models.Entities;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {
	public class UserProjectRole_ReadAll_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "UserProjectRole_ReadAll";
		
		public IEnumerable<UserProjectRole> CallFunction(IDapperRepository repository, bool readActive, bool readInactive) {
			var signature = UserProjectRole_ReadAll_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = UserProjectRole_ReadAll_Parameters.Get_DataFunction_DynamicParameters(readActive, readInactive);
			return repository.Query<UserProjectRole>(sql, parameters);
		}
	}
	
    public static class UserProjectRole_ReadAll_Parameters {
		public const string ReadActive = "readActive";
		public const string ReadInactive = "readInactive";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@readActive, ", "@readInactive");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(bool readActive, bool readInactive) {
			var parameters = new DynamicParameters();
			parameters.Add(UserProjectRole_ReadAll_Parameters.ReadActive, readActive);
			parameters.Add(UserProjectRole_ReadAll_Parameters.ReadInactive, readInactive);
			return parameters;
		}
	}
}