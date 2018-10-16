using System.Linq;
using Dapper;
using Data.Models.Entities;
using Data.Repository.Dapper;
using Data.Repository.Interfaces;

namespace Data.Repository.FunctionDefinitions {
	public class Entity_SelectById_Function : BaseDataDefinition {
		public const string DatabaseSchema = "dbo";
		public const string UserDefinedFunctionName = "Entity_SelectById";

		public Entity CallFunction(IDapperRepository repository, long id) {
			var signature = Entity_SelectById_Parameters.Get_DataFunction_Signature();
			var sql = this.GetTableValuedFunctionCallSQL(signature, DatabaseSchema, UserDefinedFunctionName);
			var parameters = Entity_SelectById_Parameters.Get_DataFunction_DynamicParameters(id);
			return repository.Query<Entity>(sql, parameters).FirstOrDefault();
		}
	}

	public static class Entity_SelectById_Parameters {
		public const string Id = "id";

		public static string Get_DataFunction_Signature() {
			return "@id";
		}

		public static DynamicParameters Get_DataFunction_DynamicParameters(long id) {
			var parameters = new DynamicParameters();
			parameters.Add(Entity_SelectById_Parameters.Id, id);
			return parameters;
		}
	}
}