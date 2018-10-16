using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IEntityService {
		TransactionResponse Create(Entity model);
		TransactionResponse Update(Entity model);
		TransactionResponse Delete(long id);
	}
}