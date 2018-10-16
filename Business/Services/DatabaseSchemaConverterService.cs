using Microsoft.SqlServer.Management.Smo;
using Model.Application;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Business.Services {
	public class DatabaseSchemaConverterService {
		public Server Server { get; internal set; }
		public string ServerName { get; internal set; }
		public string Username { get; internal set; }
		public string Password { get; internal set; }
		public string DatabaseName { get; internal set; }

		private readonly Database _database;
		private readonly List<string> _ignoreTables = new List<string> { "hibernate_unique_key", "sysdiagrams", "__RefactorLog" };
		private readonly List<string> _ignoreFunctions = new List<string>();

		public DatabaseSchemaConverterService(string connectionString) {
			if (string.IsNullOrEmpty(connectionString)) {
				throw new Exception("Connection String is required.");
			}
			var builder = new SqlConnectionStringBuilder(connectionString);
			ServerName = builder.DataSource;
			DatabaseName = builder.InitialCatalog;
			Server = new Server(ServerName);
			_database = new Database(Server, DatabaseName);
		}

		public List<EntityViewModel> GetEntitiesFromDatabase() {
			_database.Refresh();
			var result = new List<EntityViewModel>();
			foreach (Table table in _database.Tables) {
				if (_ignoreTables.Contains(table.Name)) {
					continue;
				}
				var entity = convertTableToEntity(table);
				if (entity == null || !entity.IsValid) {
					continue;
				}
				result.Add(entity);
			}
			return result;
		}

		public List<EntityViewModel> GetEntitiesFromDatabase(string entityName) {
			_database.Refresh();
			var result = new List<EntityViewModel>();
			foreach (Table table in _database.Tables) {
				if (table.Name != entityName) {
					continue;
				}
				if (_ignoreTables.Contains(table.Name)) {
					continue;
				}
				var entity = convertTableToEntity(table);
				if (entity == null || !entity.IsValid) {
					continue;
				}
				result.Add(entity);
			}
			return result;
		}

		public List<Entity> GetFunctionsFromDatabase() {
			_database.Refresh();
			var result = new List<Entity>();
			foreach (UserDefinedFunction function in _database.UserDefinedFunctions) {
				if (function.Schema == "sys" || _ignoreFunctions.Contains(function.Name)) {
					continue;
				}
				var entity = convertFunctionToEntity(function);
				if (entity == null) {
					continue;
				}
				result.Add(entity);
			}
			return result;
		}

		private EntityViewModel convertTableToEntity(Table table) {
			var entity = new EntityViewModel();
			EntityService.SetName(entity, table.Name);
			entity.Schema = table.Schema;
			//doesn't support dual keys
			foreach (Column column in table.Columns) {
				var property = new PropertyViewModel {
					SqlDataType = column.DataType.SqlDataType,
					PrimaryKey = column.InPrimaryKey
				};
				if (column.InPrimaryKey) {
					EntityService.SetKey(entity, column.Name, column.DataType.SqlDataType);
				}
				EntityService.SetName(property, column.Name);
				property.Nullable = column.Nullable;
				if (column.DefaultConstraint != null) {
					property.DefaultValue = column.DefaultConstraint.Text;
				}
				property.Length = column.DataType.MaximumLength;
				entity.Properties.Add(property);
			}
			return entity;
		}

		private Entity convertFunctionToEntity(UserDefinedFunction function) {

			return new Entity();
		}
	}
}