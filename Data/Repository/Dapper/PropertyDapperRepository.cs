using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Logging;
using Business.Common.Responses;
using Dapper;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Data.Repository.Dapper {
	public class PropertyDapperRepository : DapperRepository, IPropertyRepository {
		public PropertyDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(Property property) {
			var sql = "Execute [dbo].[Property_Insert] @EntityId, @DataTypeId, @Name, @IsPrimaryKey, @IsNullable, @Length, @DefaultValue, @IsActive, @CreatedById, @CreatedDate, @UpdatedById, @UpdatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			_params.Add("EntityId", property.EntityId);
			_params.Add("DataTypeId", property.DataTypeId);
			_params.Add("Name", property.Name);
			_params.Add("IsPrimaryKey", property.IsPrimaryKey);
			_params.Add("IsNullable", property.IsNullable);
			_params.Add("Length", property.Length);
			_params.Add("DefaultValue", property.DefaultValue);
			_params.Add("IsActive", property.IsActive);
			_params.Add("CreatedById", property.CreatedById);
			_params.Add("CreatedDate", property.CreatedDate);
			_params.Add("UpdatedById", property.UpdatedById);
			_params.Add("UpdatedDate", property.UpdatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(Property property) {
			var sql = "Execute [dbo].[Property_Update] @Id, @EntityId, @DataTypeId, @Name, @IsPrimaryKey, @IsNullable, @Length, @DefaultValue, @IsActive, @CreatedById, @CreatedDate, @UpdatedById, @UpdatedDate";
			var result = base.Execute(sql, property);
			return result;
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[Property_Delete] @Id";
			var result = base.Execute(sql, new { Id = id });
			return result;
		}
	}
}
