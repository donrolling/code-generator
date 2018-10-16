using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {

	public class PropertyRelationshipService : IPropertyRelationshipService {		
		public IPropertyRelationshipRepository PropertyRelationshipRepository { get; set; }

		public PropertyRelationshipService(IPropertyRelationshipRepository propertyRelationshipRepository) {
			this.PropertyRelationshipRepository = propertyRelationshipRepository;
		}

		public TransactionResponse Create(PropertyRelationship propertyRelationship){
			var result = this.PropertyRelationshipRepository.Create(propertyRelationship);
			return result;
		}

		public TransactionResponse Update(PropertyRelationship propertyRelationship){
			var result = this.PropertyRelationshipRepository.Update(propertyRelationship);
			return result;
		}

		public TransactionResponse Delete(long property1Id){
			var result = this.PropertyRelationshipRepository.Delete(property1Id);
			return result;
		}
	}
}