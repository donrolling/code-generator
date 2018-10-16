using System;
using System.Collections.Generic;
using Business.Common.DataTables;
using Dapper;
using Data.Models.Entities;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {    
    public class Project_ReadAll_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "Project_ReadAll";
		
		public IEnumerable<Project> CallFunction(IDapperRepository repository, long userId, bool readActive, bool readInactive) {
			return CallFunction<Project>(repository, userId, readActive, readInactive);
		}

		public IEnumerable<T> CallFunction<T>(IDapperRepository repository, long userId, bool readActive, bool readInactive) where T : class {
			var signature = Project_ReadAll_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = Project_ReadAll_Parameters.Get_DataFunction_DynamicParameters(userId, readActive, readInactive);
			return repository.Query<T>(sql, parameters);
		}
		
		public IPresentable<Project> CallFunction_Paged(IDapperRepository repository, PageInfo pageInfo, long userId, bool readActive, bool readInactive) {
			var signature = Project_ReadAll_Parameters.Get_DataFunction_Signature();
			var parameters = Project_ReadAll_Parameters.Get_DataFunction_DynamicParameters(userId, readActive, readInactive);
			return base.Paged<Project>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}
	
    public static class Project_ReadAll_Parameters {
		public const string UserId = "userId";
		public const string ReadActive = "readActive";
		public const string ReadInactive = "readInactive";

		public static string Get_DataFunction_Signature() {
			return "@userId, @readActive, @readInactive";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long userId, bool readActive, bool readInactive) {
			var parameters = new DynamicParameters();
			parameters.Add(Project_ReadAll_Parameters.UserId, userId);
			parameters.Add(Project_ReadAll_Parameters.ReadActive, readActive);
			parameters.Add(Project_ReadAll_Parameters.ReadInactive, readInactive);
			return parameters;
		}
    }
}