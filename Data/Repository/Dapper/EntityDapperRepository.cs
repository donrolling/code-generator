using System;
using System.Collections.Generic;
using System.Data;
using Business.Common.Logging;
using Business.Common.Responses;
using Dapper;
using Data.Models.Entities;
using Data.Repository.FunctionDefinitions;
using Data.Repository.Interfaces;

namespace Data.Repository.Dapper {
	public class EntityDapperRepository : DapperRepository, IEntityRepository {	
		public EntityDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(Entity entity) {
			var sql = "Execute [dbo].[Entity_Insert] @ProjectId, @Name, @IsActive, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();

			_params.Add("ProjectId", entity.ProjectId);
			_params.Add("Name", entity.Name);
			_params.Add("IsActive", entity.IsActive);
			_params.Add("CreatedById", entity.CreatedById);
			_params.Add("CreatedDate", entity.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(Entity entity) {
			var sql = "Execute [dbo].[Entity_Update] @Id, @ProjectId, @Name, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, entity);
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[Entity_Delete] @Id";
			var result = base.Execute(sql, new { Id = id });
			return result;
		}

		public Entity SelectById(long id) {
			return new Entity_SelectById_Function().CallFunction(this, id);
		}

		public IEnumerable<Entity> ReadAll(long userId, bool readActive = true, bool readInactive = false) {
			return new Entity_ReadAll_Function().CallFunction<Entity>(this, readActive, readInactive);
		}
	}
}