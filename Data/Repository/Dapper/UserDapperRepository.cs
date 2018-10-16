using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Business.Common;
using Business.Common.DataTables;
using Business.Common.Logging;
using Business.Common.Responses;
using Dapper;
using Data.Models.Entities;
using Data.Presentation;
using Data.Repository.FunctionDefinitions;
using Data.Repository.Interfaces;

namespace Data.Repository.Dapper {
	public class UserDapperRepository : DapperRepository, IUserRepository {
		public UserDapperRepository(string connectionString, IRepositoryLogger logger) : base(connectionString, logger) { }

		public User Get(long id) {
			var sql = "select * from [dbo].[User] where Id = @userId";
			return this.Query<User>(sql, new { userId = id }).FirstOrDefault();
		}
		
		public TransactionResponse Create(User user, string password, string salt) {
			var sql = "Execute [dbo].[User_Insert] @Email, @Password, @Salt, @CreatedById, @CreatedDate, @Id = @Id OUTPUT";
			var _params = new DynamicParameters();
			_params.Add("Email", user.Email);
			_params.Add("Password", password);
			_params.Add("Salt", salt);
			_params.Add("CreatedById", user.CreatedById);
			_params.Add("CreatedDate", user.CreatedDate);
			_params.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

			var result = this.Execute(sql, _params);
			return result;
		}

		public TransactionResponse Update(User user) {
			var sql = "Execute [dbo].[User_Update] @Id, @Email, @IsActive, @UpdatedById, @UpdatedDate";
			return base.Execute(sql, user);
		}

		public TransactionResponse UpdatePassword(long userId, string password, string salt, long updatedById, DateTime updatedDate) {
			var sql = string.Format("execute {0}.User_UpdatePassword @Id, @Password, @Salt, @UpdatedById, @UpdatedDate");
			return this.Execute(sql, new { Id = userId, Password = password, PasswordSalt = salt, UpdatedById = updatedById, UpdatedDate = updatedDate });
		}

		public TransactionResponse Delete(long id, long currentUserId) {
			var sql = "Execute [dbo].[User_Delete] @id";
			return base.Execute(sql, new { id = id });
		}
		public IEnumerable<Users_ReadForList_Result> GetAll(long userId) {
			return new Users_ReadForList_Function().CallFunction(this, userId);
		}

		public GetUserByEmailAddress_Result GetByEmailAddress(string emailAddress) {
			return new GetUserByEmailAddress_Function().CallFunction(this, emailAddress).FirstOrDefault();
		}

		public IPresentable<Users_ReadForList_Result> Paged_Users(long userId, PageInfo pageInfo) {
			return new Users_ReadForList_Function().CallFunction_Paged(this, pageInfo, userId);
		}

		public int GetUserCount() {
			var sql = @"select count(*) from [dbo].[User]";
			using (var connection = new SqlConnection(ConnectionString)) {
				var result = SqlMapper.Query<int>(connection, sql, new { }).First();
				return result;
			}
		}

		public bool DoesUserExist(string emailAddress) {
			return new DoesUserExist_Function().CallFunction(this, emailAddress);
		}

		public bool IsInRole(long userId, string systemRole) {
			return new IsInRole_Function().CallFunction(this, userId, systemRole);
		}

		public User SelectById(long id) {
			return new User_SelectById_Function().CallFunction(this, id);
		}

		public IEnumerable<User> ReadAll(long userId, bool readActive = true, bool readInactive = false) {
			return new User_ReadAll_Function().CallFunction<User>(this, readActive, readInactive);
		}
	}
}