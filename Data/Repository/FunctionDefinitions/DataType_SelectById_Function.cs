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
    public class DataType_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "DataType_SelectById";
		
		public DataType CallFunction(IDapperRepository repository, long id) {
			var signature = DataType_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = DataType_SelectById_Parameters.Get_DataFunction_DynamicParameters(id);
			return repository.Query<DataType>(sql, parameters).FirstOrDefault();
		}
	}
	
    public static class DataType_SelectById_Parameters {
		public const string Id = "id";		

		public static string Get_DataFunction_Signature() {
			return "@id";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long id) {
			var parameters = new DynamicParameters();
			parameters.Add(DataType_SelectById_Parameters.Id, id);
			return parameters;
		}
	}
}