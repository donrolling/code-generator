using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IPropertyRepository : IRepository {	
		TransactionResponse Create(Property property);
		TransactionResponse Update(Property property);
		TransactionResponse Delete(long id);
	}
}