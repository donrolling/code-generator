using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IRoleService {
		TransactionResponse Create(Role model);
		TransactionResponse Update(Role model);
		TransactionResponse Delete(long id);
	}
}