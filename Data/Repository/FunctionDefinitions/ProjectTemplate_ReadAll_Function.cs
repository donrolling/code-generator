using System;
using System.Collections.Generic;
using Business.Common.DataTables;
using Dapper;
using Data.Models.Entities;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {    
    public class ProjectTemplate_ReadAll_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "ProjectTemplate_ReadAll";
		
		public IEnumerable<Template> CallFunction(IDapperRepository repository, long projectId, bool readActive, bool readInactive) {
			return CallFunction<Template>(repository, projectId, readActive, readInactive);
		}

		public IEnumerable<T> CallFunction<T>(IDapperRepository repository, long projectId, bool readActive, bool readInactive) where T : class {
			var signature = ProjectTemplate_ReadAll_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = ProjectTemplate_ReadAll_Parameters.Get_DataFunction_DynamicParameters(projectId, readActive, readInactive);
			return repository.Query<T>(sql, parameters);
		}
		
		public IPresentable<Template> CallFunction_Paged(IDapperRepository repository, long projectId, PageInfo pageInfo, bool readActive, bool readInactive) {
			var signature = ProjectTemplate_ReadAll_Parameters.Get_DataFunction_Signature();
			var parameters = ProjectTemplate_ReadAll_Parameters.Get_DataFunction_DynamicParameters(projectId, readActive, readInactive);
			return base.Paged<Template>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}
	
    public static class ProjectTemplate_ReadAll_Parameters {
	    public const string ProjectId = "projectId";
        public const string ReadActive = "readActive";
		public const string ReadInactive = "readInactive";

		public static string Get_DataFunction_Signature() {
			return "@projectId, @readActive, @readInactive";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long projectId, bool readActive, bool readInactive) {
			var parameters = new DynamicParameters();
			parameters.Add(ProjectTemplate_ReadAll_Parameters.ProjectId, projectId);
			parameters.Add(ProjectTemplate_ReadAll_Parameters.ReadActive, readActive);
			parameters.Add(ProjectTemplate_ReadAll_Parameters.ReadInactive, readInactive);
			return parameters;
		}
	}
}