using System;
using System.Collections.Generic;
using Business.Common.DataTables;
using Dapper;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {    
    public class ProjectTemplate_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "ProjectTemplate_SelectById";
		
		public IEnumerable<ProjectTemplate_SelectById_Result> CallFunction(IDapperRepository repository, long projectId, long templateId) {
			return CallFunction<ProjectTemplate_SelectById_Result>(repository, projectId, templateId);
		}

		public IEnumerable<T> CallFunction<T>(IDapperRepository repository, long projectId, long templateId) where T : class {
			var signature = ProjectTemplate_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = ProjectTemplate_SelectById_Parameters.Get_DataFunction_DynamicParameters(projectId, templateId);
			return repository.Query<T>(sql, parameters);
		}
		
		public IPresentable<ProjectTemplate_SelectById_Result> CallFunction_Paged(IDapperRepository repository, PageInfo pageInfo, long projectId, long templateId) {
			var signature = ProjectTemplate_SelectById_Parameters.Get_DataFunction_Signature();
			var parameters = ProjectTemplate_SelectById_Parameters.Get_DataFunction_DynamicParameters(projectId, templateId);
			return base.Paged<ProjectTemplate_SelectById_Result>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}
	
    public static class ProjectTemplate_SelectById_Parameters {
		public const string ProjectId = "projectId";
		public const string TemplateId = "templateId";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@projectId, ", "@templateId");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long projectId, long templateId) {
			var parameters = new DynamicParameters();
			parameters.Add(ProjectTemplate_SelectById_Parameters.ProjectId, projectId);
			parameters.Add(ProjectTemplate_SelectById_Parameters.TemplateId, templateId);
			return parameters;
		}
	}
	
    public static class ProjectTemplate_SelectById_Properties {
		public const string ProjectId = "ProjectId";
		public const string TemplateId = "TemplateId";
		public const string IsActive = "IsActive";
		public const string CreatedById = "CreatedById";
		public const string CreatedDate = "CreatedDate";
		public const string UpdatedById = "UpdatedById";
		public const string UpdatedDate = "UpdatedDate";
	}
	
	public class ProjectTemplate_SelectById_Result {
		public long ProjectId { get; set; }
		public long TemplateId { get; set; }
		public bool IsActive { get; set; }
		public long CreatedById { get; set; }
		public DateTime CreatedDate { get; set; }
		public long UpdatedById { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}