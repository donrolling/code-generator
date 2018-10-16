using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IPropertyRelationshipRepository : IRepository {	
		TransactionResponse Create(PropertyRelationship propertyRelationship);
		TransactionResponse Update(PropertyRelationship propertyRelationship);
		TransactionResponse Delete(long property1Id);
	}
}