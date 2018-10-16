using System;
using System.Collections.Generic;
using Business.Common.DataTables;
using Dapper;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {    
    public class TemplateImport_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "TemplateImport_SelectById";
		
		public IEnumerable<TemplateImport_SelectById_Result> CallFunction(IDapperRepository repository, long id) {
			return CallFunction<TemplateImport_SelectById_Result>(repository, id);
		}

		public IEnumerable<T> CallFunction<T>(IDapperRepository repository, long id) where T : class {
			var signature = TemplateImport_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = TemplateImport_SelectById_Parameters.Get_DataFunction_DynamicParameters(id);
			return repository.Query<T>(sql, parameters);
		}
		
		public IPresentable<TemplateImport_SelectById_Result> CallFunction_Paged(IDapperRepository repository, PageInfo pageInfo, long id) {
			var signature = TemplateImport_SelectById_Parameters.Get_DataFunction_Signature();
			var parameters = TemplateImport_SelectById_Parameters.Get_DataFunction_DynamicParameters(id);
			return base.Paged<TemplateImport_SelectById_Result>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}
	
    public static class TemplateImport_SelectById_Parameters {
		public const string Id = "id";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@id");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long id) {
			var parameters = new DynamicParameters();
			parameters.Add(TemplateImport_SelectById_Parameters.Id, id);
			return parameters;
		}
	}
	
    public static class TemplateImport_SelectById_Properties {
		public const string Id = "Id";
		public const string TemplateId = "TemplateId";
		public const string IsActive = "IsActive";
		public const string CreatedById = "CreatedById";
		public const string CreatedDate = "CreatedDate";
		public const string UpdatedById = "UpdatedById";
		public const string UpdatedDate = "UpdatedDate";
	}
	
	public class TemplateImport_SelectById_Result {
		public long Id { get; set; }
		public long TemplateId { get; set; }
		public bool IsActive { get; set; }
		public long CreatedById { get; set; }
		public DateTime CreatedDate { get; set; }
		public long UpdatedById { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}