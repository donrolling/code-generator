using System;
using System.Collections.Generic;
using System.Data;
using Business.Common.Logging;
using Business.Common.Responses;

using Dapper;
using Data.Models.Entities;
using Data.Repository.Dapper;
using Data.Repository.FunctionDefinitions;
using Data.Repository.Interfaces;

namespace Data.Repository.Dapper {
	public class PropertyRelationshipDapperRepository : DapperRepository, IPropertyRelationshipRepository {	
		public PropertyRelationshipDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(PropertyRelationship propertyRelationship) {
			var sql = "Execute [dbo].[PropertyRelationship_Insert] @Property1Id, @Property2Id, @Name, @Type, @IsActive, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			
			_params.Add("Property1Id", propertyRelationship.Property1Id);
			_params.Add("Property2Id", propertyRelationship.Property2Id);
			_params.Add("Name", propertyRelationship.Name);
			_params.Add("Type", propertyRelationship.Type);
			_params.Add("IsActive", propertyRelationship.IsActive);
			_params.Add("CreatedById", propertyRelationship.CreatedById);
			_params.Add("CreatedDate", propertyRelationship.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(PropertyRelationship propertyRelationship) {
			var sql = "Execute [dbo].[PropertyRelationship_Update] @Property1Id, @Property2Id, @Name, @Type, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, propertyRelationship);
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[PropertyRelationship_Delete] @Id";
			return base.Execute(sql, new { Id = id });
		}

		public PropertyRelationship SelectById(long property1Id, long property2Id) {
			return new PropertyRelationship_SelectById_Function().CallFunction(this, property1Id, property2Id);
		}

		public IEnumerable<PropertyRelationship> ReadAll(bool readActive = true, bool readInactive = false) {
			return new PropertyRelationship_ReadAll_Function().CallFunction<PropertyRelationship>(this, readActive, readInactive);
		}
	}
}