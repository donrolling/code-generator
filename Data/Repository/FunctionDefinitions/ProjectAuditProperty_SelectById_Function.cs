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
	public class ProjectAuditProperty_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "ProjectAuditProperty_SelectById";
		
		public ProjectAuditProperty CallFunction(IDapperRepository repository, long projectId, long propertyId) {
			var signature = ProjectAuditProperty_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = ProjectAuditProperty_SelectById_Parameters.Get_DataFunction_DynamicParameters(projectId, propertyId);
			return repository.Query<ProjectAuditProperty>(sql, parameters).FirstOrDefault();
		}
	}
	
    public static class ProjectAuditProperty_SelectById_Parameters {
		public const string ProjectId = "projectId";
		public const string PropertyId = "propertyId";		

		public static string Get_DataFunction_Signature() {
			return "@projectId, @propertyId";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long projectId, long propertyId) {
			var parameters = new DynamicParameters();
			parameters.Add(ProjectAuditProperty_SelectById_Parameters.ProjectId, projectId);
			parameters.Add(ProjectAuditProperty_SelectById_Parameters.PropertyId, propertyId);
			return parameters;
		}
	}
}