using System;
using System.Collections.Generic;
using Business.Common.DataTables;
using Dapper;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {
	public class LanguageDataTypeConversion_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "LanguageDataTypeConversion_SelectById";
		
		public IEnumerable<LanguageDataTypeConversion_SelectById_Result> CallFunction(IDapperRepository repository, long languageId, long dataTypeId) {
			return CallFunction<LanguageDataTypeConversion_SelectById_Result>(repository, languageId, dataTypeId);
		}

		public IEnumerable<T> CallFunction<T>(IDapperRepository repository, long languageId, long dataTypeId) where T : class {
			var signature = LanguageDataTypeConversion_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = LanguageDataTypeConversion_SelectById_Parameters.Get_DataFunction_DynamicParameters(languageId, dataTypeId);
			return repository.Query<T>(sql, parameters);
		}
		
		public IPresentable<LanguageDataTypeConversion_SelectById_Result> CallFunction_Paged(IDapperRepository repository, PageInfo pageInfo, long languageId, long dataTypeId) {
			var signature = LanguageDataTypeConversion_SelectById_Parameters.Get_DataFunction_Signature();
			var parameters = LanguageDataTypeConversion_SelectById_Parameters.Get_DataFunction_DynamicParameters(languageId, dataTypeId);
			return base.Paged<LanguageDataTypeConversion_SelectById_Result>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}
	
    public static class LanguageDataTypeConversion_SelectById_Parameters {
		public const string LanguageId = "languageId";
		public const string DataTypeId = "dataTypeId";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@languageId, ", "@dataTypeId");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long languageId, long dataTypeId) {
			var parameters = new DynamicParameters();
			parameters.Add(LanguageDataTypeConversion_SelectById_Parameters.LanguageId, languageId);
			parameters.Add(LanguageDataTypeConversion_SelectById_Parameters.DataTypeId, dataTypeId);
			return parameters;
		}
	}
	
    public static class LanguageDataTypeConversion_SelectById_Properties {
		public const string LanguageId = "LanguageId";
		public const string DataTypeId = "DataTypeId";
		public const string Output = "Output";
		public const string IsActive = "IsActive";
		public const string CreatedById = "CreatedById";
		public const string CreatedDate = "CreatedDate";
		public const string UpdatedById = "UpdatedById";
		public const string UpdatedDate = "UpdatedDate";
	}
	
	public class LanguageDataTypeConversion_SelectById_Result {
		public long LanguageId { get; set; }
		public long DataTypeId { get; set; }
		public string Output { get; set; }
		public bool IsActive { get; set; }
		public long CreatedById { get; set; }
		public DateTime CreatedDate { get; set; }
		public long UpdatedById { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}