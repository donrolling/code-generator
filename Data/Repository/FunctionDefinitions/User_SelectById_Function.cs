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
    public class User_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "User_SelectById";
		
		public User CallFunction(IDapperRepository repository, long id) {
			var signature = User_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = User_SelectById_Parameters.Get_DataFunction_DynamicParameters(id);
			return repository.Query<User>(sql, parameters).FirstOrDefault();
		}
	}
	
    public static class User_SelectById_Parameters {
		public const string Id = "id";		

		public static string Get_DataFunction_Signature() {
			return "@id";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long id) {
			var parameters = new DynamicParameters();
			parameters.Add(User_SelectById_Parameters.Id, id);
			return parameters;
		}
	}
}