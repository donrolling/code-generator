using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IPropertyRelationshipService {
		TransactionResponse Create(PropertyRelationship model);
		TransactionResponse Update(PropertyRelationship model);
		TransactionResponse Delete(long property1Id);
	}
}