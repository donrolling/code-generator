using Dapper;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions { 
    public class IsUserSystemAdmin_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "IsUserSystemAdmin";

		public bool CallFunction(IDapperRepository repository, long userId) {		
			var signature = IsUserSystemAdmin_Parameters.Get_DataFunction_Signature();
			var sql = this.GetScalarFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = IsUserSystemAdmin_Parameters.Get_DataFunction_DynamicParameters(userId);
			return repository.ReturnFirst<bool>(sql, parameters);
		}
	}
	
    public static class IsUserSystemAdmin_Parameters {
		public const string UserId = "userId";		

		public static string Get_DataFunction_Signature() {
			return string.Concat("@userId");
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long userId) {
			var parameters = new DynamicParameters();
			parameters.Add(UserId, userId);
			return parameters;
		}
	}
	
    public static class IsUserSystemAdmin_Properties {

	}
	
}