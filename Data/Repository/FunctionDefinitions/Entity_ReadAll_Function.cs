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
	public class Entity_ReadAll_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "Entity_ReadAll";

		public IEnumerable<Entity_ReadAll_Result> CallFunction(IDapperRepository repository, bool readActive, bool readInactive) {
			return CallFunction<Entity_ReadAll_Result>(repository, readActive, readInactive);
		}

		public IEnumerable<T> CallFunction<T>(IDapperRepository repository, bool readActive, bool readInactive) where T : class {
			var signature = Entity_ReadAll_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = Entity_ReadAll_Parameters.Get_DataFunction_DynamicParameters(readActive, readInactive);
			return repository.Query<T>(sql, parameters);
		}

		public IPresentable<Entity_ReadAll_Result> CallFunction_Paged(IDapperRepository repository, PageInfo pageInfo, bool readActive, bool readInactive) {
			var signature = Entity_ReadAll_Parameters.Get_DataFunction_Signature();
			var parameters = Entity_ReadAll_Parameters.Get_DataFunction_DynamicParameters(readActive, readInactive);
			return base.Paged<Entity_ReadAll_Result>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}

	public static class Entity_ReadAll_Parameters {
		public const string ReadActive = "readActive";
		public const string ReadInactive = "readInactive";

		public static string Get_DataFunction_Signature() {
			return string.Concat("@readActive, ", "@readInactive");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(bool readActive, bool readInactive) {
			var parameters = new DynamicParameters();
			parameters.Add(Entity_ReadAll_Parameters.ReadActive, readActive);
			parameters.Add(Entity_ReadAll_Parameters.ReadInactive, readInactive);
			return parameters;
		}
	}

	public static class Entity_ReadAll_Properties {
		public const string Id = "Id";
		public const string ProjectId = "ProjectId";
		public const string Name = "Name";
		public const string IsActive = "IsActive";
		public const string CreatedById = "CreatedById";
		public const string CreatedDate = "CreatedDate";
		public const string UpdatedById = "UpdatedById";
		public const string UpdatedDate = "UpdatedDate";
	}

	public class Entity_ReadAll_Result {
		public long Id { get; set; }
		public long ProjectId { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public long CreatedById { get; set; }
		public DateTime CreatedDate { get; set; }
		public long UpdatedById { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}