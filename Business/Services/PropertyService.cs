using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class PropertyService : IPropertyService {
		public IPropertyRepository PropertyRepository { get; set; }

		public PropertyService(IPropertyRepository propertyRepository) {
			this.PropertyRepository = propertyRepository;
		}

		public TransactionResponse Create(Property property){
			var result = this.PropertyRepository.Create(property);
			return result;
		}

		public TransactionResponse Update(Property property){
			var result = this.PropertyRepository.Update(property);
			return result;
		}

		public TransactionResponse Delete(long id){
			var result = this.PropertyRepository.Delete(id);
			return result;
		}
	}
}