using Dapper;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {    
    public class IsInRole_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "IsInRole";

		public bool CallFunction(IDapperRepository repository, long userId, string systemRole) {		
			var signature = IsInRole_Parameters.Get_DataFunction_Signature();
			var sql = this.GetScalarFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = IsInRole_Parameters.Get_DataFunction_DynamicParameters(userId, systemRole);
			return repository.ReturnFirst<bool>(sql, parameters);
		}
	}
	
    public static class IsInRole_Parameters {
		public const string UserId = "userId";
		public const string SystemRole = "systemRole";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@userId, ", "@systemRole");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long userId, string systemRole) {
			var parameters = new DynamicParameters();
			parameters.Add(UserId, userId);
			parameters.Add(SystemRole, systemRole);
			return parameters;
		}
	}
	
    public static class IsInRole_Properties {

	}
	
}