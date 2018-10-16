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
	public class PropertyRelationship_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "PropertyRelationship_SelectById";
		
		public PropertyRelationship CallFunction(IDapperRepository repository, long property1Id, long property2Id) {
			var signature = PropertyRelationship_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = PropertyRelationship_SelectById_Parameters.Get_DataFunction_DynamicParameters(property1Id, property2Id);
			return repository.Query<PropertyRelationship>(sql, parameters).FirstOrDefault();
		}
	}
	
    public static class PropertyRelationship_SelectById_Parameters {
		public const string Property1Id = "property1Id";
		public const string Property2Id = "property2Id";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@property1Id, ", "@property2Id");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long property1Id, long property2Id) {
			var parameters = new DynamicParameters();
			parameters.Add(PropertyRelationship_SelectById_Parameters.Property1Id, property1Id);
			parameters.Add(PropertyRelationship_SelectById_Parameters.Property2Id, property2Id);
			return parameters;
		}
	}
}