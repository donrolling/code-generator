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
	public class RoleDapperRepository : DapperRepository, IRoleRepository {	
		public RoleDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(Role role) {
			var sql = "Execute [dbo].[Role_Insert] @Name, @DisplayName, @IsActive, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			
			_params.Add("Name", role.Name);
			_params.Add("DisplayName", role.DisplayName);
			_params.Add("IsActive", role.IsActive);
			_params.Add("CreatedById", role.CreatedById);
			_params.Add("CreatedDate", role.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(Role role) {
			var sql = "Execute [dbo].[Role_Update] @Id, @Name, @DisplayName, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, role);
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[Role_Delete] @Id";
			return base.Execute(sql, new { Id = id });
		}

		public Role SelectById(long id) {
			return new Role_SelectById_Function().CallFunction(this, id);
		}

		public IEnumerable<Role> ReadAll(long userId, bool readActive = true, bool readInactive = false) {
			return new Role_ReadAll_Function().CallFunction<Role>(this, readActive, readInactive);
		}
	}
}