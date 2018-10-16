using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IUserService {
		TransactionResponse Create(User model, string password, string salt);
		TransactionResponse Update(User model);
		TransactionResponse Delete(long id);
	}
}