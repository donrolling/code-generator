using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IPropertyService {
		TransactionResponse Create(Property model);
		TransactionResponse Update(Property model);
		TransactionResponse Delete(long id);
	}
}