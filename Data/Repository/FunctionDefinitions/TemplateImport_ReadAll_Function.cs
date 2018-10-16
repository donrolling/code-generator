using System;
using System.Collections.Generic;
using Dapper;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;
using Business.Common.DataTables;

namespace Data.Repository.FunctionDefinitions {    
    public class TemplateImport_ReadAll_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "TemplateImport_ReadAll";
		
		public IEnumerable<TemplateImport_ReadAll_Result> CallFunction(IDapperRepository repository, bool readActive, bool readInactive) {
			return CallFunction<TemplateImport_ReadAll_Result>(repository, readActive, readInactive);
		}

		public IEnumerable<T> CallFunction<T>(IDapperRepository repository, bool readActive, bool readInactive) where T : class {
			var signature = TemplateImport_ReadAll_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = TemplateImport_ReadAll_Parameters.Get_DataFunction_DynamicParameters(readActive, readInactive);
			return repository.Query<T>(sql, parameters);
		}
		
		public IPresentable<TemplateImport_ReadAll_Result> CallFunction_Paged(IDapperRepository repository, PageInfo pageInfo, bool readActive, bool readInactive) {
			var signature = TemplateImport_ReadAll_Parameters.Get_DataFunction_Signature();
			var parameters = TemplateImport_ReadAll_Parameters.Get_DataFunction_DynamicParameters(readActive, readInactive);
			return base.Paged<TemplateImport_ReadAll_Result>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}
	
    public static class TemplateImport_ReadAll_Parameters {
		public const string ReadActive = "readActive";
		public const string ReadInactive = "readInactive";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@readActive, ", "@readInactive");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(bool readActive, bool readInactive) {
			var parameters = new DynamicParameters();
			parameters.Add(TemplateImport_ReadAll_Parameters.ReadActive, readActive);
			parameters.Add(TemplateImport_ReadAll_Parameters.ReadInactive, readInactive);
			return parameters;
		}
	}
	
    public static class TemplateImport_ReadAll_Properties {
		public const string Id = "Id";
		public const string Name = "Name";
		public const string DotNetEnumValue = "DotNetEnumValue";
		public const string IsActive = "IsActive";
		public const string CreatedById = "CreatedById";
		public const string CreatedDate = "CreatedDate";
		public const string UpdatedById = "UpdatedById";
		public const string UpdatedDate = "UpdatedDate";
	}
	
	public class TemplateImport_ReadAll_Result {
		public long Id { get; set; }
		public string Name { get; set; }
		public int DotNetEnumValue { get; set; }
		public bool IsActive { get; set; }
		public long CreatedById { get; set; }
		public DateTime CreatedDate { get; set; }
		public long UpdatedById { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}