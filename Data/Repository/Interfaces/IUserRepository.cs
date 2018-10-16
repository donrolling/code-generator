using System;
using System.Collections.Generic;
using Business.Common;
using Business.Common.DataTables;
using Business.Common.Responses;
using Data.Models.Entities;
using Data.Presentation;
using Data.Repository.FunctionDefinitions;

namespace Data.Repository.Interfaces {
	public interface IUserRepository : IRepository {
		User Get(long id);
		TransactionResponse Create(User user, string password, string salt);
		TransactionResponse Update(User user);
		TransactionResponse UpdatePassword(long userId, string password, string salt, long updatedById, DateTime updatedDate);
		TransactionResponse Delete(long userId, long currentUserId);
		bool DoesUserExist(string emailAddress);
		GetUserByEmailAddress_Result GetByEmailAddress(string emailAddress);
		int GetUserCount();
		bool IsInRole(long userId, string systemRole);
		IPresentable<Users_ReadForList_Result> Paged_Users(long userId, PageInfo pageInfo);
		IEnumerable<Users_ReadForList_Result> GetAll(long currentUserId);
	}
}