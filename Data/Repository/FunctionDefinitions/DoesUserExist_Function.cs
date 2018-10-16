using Dapper;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {    
    public class DoesUserExist_Function : BaseDataDefinition {
		public const string DatabaseSchema = "Authentication";
		public const string UserDefinedFunctionName = "DoesUserExist";

		public bool CallFunction(IDapperRepository repository, string emailAddress) {		
			var signature = DoesUserExist_Parameters.Get_DataFunction_Signature();
			var sql = this.GetScalarFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = DoesUserExist_Parameters.Get_DataFunction_DynamicParameters(emailAddress);
			return repository.ReturnFirst<bool>(sql, parameters);
		}
	}
	
    public static class DoesUserExist_Parameters {
		public const string EmailAddress = "emailAddress";		

		public static string Get_DataFunction_Signature() {
			return "@emailAddress";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(string emailAddress) {
			var parameters = new DynamicParameters();
			parameters.Add(EmailAddress, emailAddress);
			return parameters;
		}
	}
	
    public static class DoesUserExist_Properties {

	}
	
}