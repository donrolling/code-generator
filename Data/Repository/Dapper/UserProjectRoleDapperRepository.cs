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
	public class UserProjectRoleDapperRepository : DapperRepository, IUserProjectRoleRepository {	
		public UserProjectRoleDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public TransactionResponse Create(UserProjectRole userProjectRole) {
			var sql = "Execute [dbo].[UserProjectRole_Insert] @UserId, @ProjectId, @RoleId, @IsActive, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			
			_params.Add("UserId", userProjectRole.UserId);
			_params.Add("ProjectId", userProjectRole.ProjectId);
			_params.Add("RoleId", userProjectRole.RoleId);
			_params.Add("IsActive", userProjectRole.IsActive);
			_params.Add("CreatedById", userProjectRole.CreatedById);
			_params.Add("CreatedDate", userProjectRole.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = base.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(UserProjectRole userProjectRole) {
			var sql = "Execute [dbo].[UserProjectRole_Update] @UserId, @ProjectId, @RoleId, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, userProjectRole);
		}

		public TransactionResponse Delete(long id) {
			var sql = "Execute [dbo].[UserProjectRole_Delete] @Id";
			return base.Execute(sql, new { Id = id });
		}

		public UserProjectRole SelectById(long userId, long projectId) {
			return new UserProjectRole_SelectById_Function().CallFunction(this, userId, projectId);
		}

		public IEnumerable<UserProjectRole> ReadAll(bool readActive = true, bool readInactive = false) {
			return new UserProjectRole_ReadAll_Function().CallFunction(this, readActive, readInactive);
		}
	}
}