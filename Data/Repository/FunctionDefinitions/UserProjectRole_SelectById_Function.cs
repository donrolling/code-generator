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
	public class UserProjectRole_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "UserProjectRole_SelectById";
		
		public UserProjectRole CallFunction(IDapperRepository repository, long userId, long projectId) {
			var signature = UserProjectRole_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = UserProjectRole_SelectById_Parameters.Get_DataFunction_DynamicParameters(userId, projectId);
			return repository.Query<UserProjectRole>(sql, parameters).FirstOrDefault();
		}
	}
	
    public static class UserProjectRole_SelectById_Parameters {
		public const string UserId = "userId";
		public const string ProjectId = "projectId";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@userId, ", "@projectId");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long userId, long projectId) {
			var parameters = new DynamicParameters();
			parameters.Add(UserProjectRole_SelectById_Parameters.UserId, userId);
			parameters.Add(UserProjectRole_SelectById_Parameters.ProjectId, projectId);
			return parameters;
		}
	}
}