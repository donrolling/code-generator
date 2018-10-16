using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IUserProjectRoleService {
		TransactionResponse Create(UserProjectRole model);
		TransactionResponse Update(UserProjectRole model);
		TransactionResponse Delete(long userId);
	}
}