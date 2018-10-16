using System;
using System.Collections.Generic;
using Dapper;
using Data.Presentation;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;
using Business.Common.DataTables;

namespace Data.Repository.FunctionDefinitions {
	public class GetUserByEmailAddress_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "GetUserByEmailAddress";

		public IEnumerable<GetUserByEmailAddress_Result> CallFunction(IDapperRepository repository, string emailAddress) {		
			var signature = GetUserByEmailAddress_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = GetUserByEmailAddress_Parameters.Get_DataFunction_DynamicParameters(emailAddress);
			return repository.Query<GetUserByEmailAddress_Result>(sql, parameters);
		}
		
		public IPresentable<GetUserByEmailAddress_Result> CallFunction_Paged(IDapperRepository repository, PageInfo pageInfo, string emailAddress) {
			var signature = GetUserByEmailAddress_Parameters.Get_DataFunction_Signature();
			var parameters = GetUserByEmailAddress_Parameters.Get_DataFunction_DynamicParameters(emailAddress);
			return base.Paged<GetUserByEmailAddress_Result>(repository, parameters, signature, DatabaseSchema, UserDefinedFunctionName, pageInfo);
		}
	}
	
	public static class GetUserByEmailAddress_Parameters {
		public const string EmailAddress = "emailAddress";		

		public static string Get_DataFunction_Signature() {
			return "@emailAddress";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(string emailAddress) {
			var parameters = new DynamicParameters();
			parameters.Add(GetUserByEmailAddress_Parameters.EmailAddress, emailAddress);
			return parameters;
		}
	}
	
	public static class GetUserByEmailAddress_Properties {
		public const string Id = "Id";
		public const string Email = "Email";
		public const string Password = "Password";
		public const string PasswordSalt = "Salt";
		public const string IsActive = "IsActive";
		public const string CreatedById = "CreatedById";
		public const string UpdatedById = "UpdatedById";
		public const string CreatedDate = "CreatedDate";
		public const string UpdatedDate = "UpdatedDate";
	}
	
	public class GetUserByEmailAddress_Result {
		public long Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool IsActive { get; set; } = true;
		public long CreatedById { get; set; }
		public long UpdatedById { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}