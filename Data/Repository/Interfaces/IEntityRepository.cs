using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IEntityRepository : IRepository {	
		TransactionResponse Create(Entity entity);
		TransactionResponse Update(Entity entity);
		TransactionResponse Delete(long id);
	}
}