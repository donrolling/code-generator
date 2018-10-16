using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class DataTypeService : IDataTypeService {
		public IDataTypeRepository DataTypeRepository { get; set; }

		public DataTypeService(IDataTypeRepository datatypeRepository) {
			this.DataTypeRepository = datatypeRepository;
		}

		public TransactionResponse Create(DataType datatype){
			var result = this.DataTypeRepository.Create(datatype);
			return result;
		}

		public TransactionResponse Update(DataType datatype){
			var result = this.DataTypeRepository.Update(datatype);
			return result;
		}

		public TransactionResponse Delete(long id){
			var result = this.DataTypeRepository.Delete(id);
			return result;
		}
	}
}