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
	public class Role_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "Role_SelectById";
		
		public Role CallFunction(IDapperRepository repository, long id) {
			var signature = Role_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = Role_SelectById_Parameters.Get_DataFunction_DynamicParameters(id);
			return repository.Query<Role>(sql, parameters).FirstOrDefault();
		}
	}
	
    public static class Role_SelectById_Parameters {
		public const string Id = "id";		

		public static string Get_DataFunction_Signature() {
			return "@id";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long id) {
			var parameters = new DynamicParameters();
			parameters.Add(Role_SelectById_Parameters.Id, id);
			return parameters;
		}
	}
}