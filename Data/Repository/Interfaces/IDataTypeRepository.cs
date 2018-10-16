using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Data.Repository.Interfaces {
	public interface IDataTypeRepository : IRepository {	
		TransactionResponse Create(DataType datatype);
		TransactionResponse Update(DataType datatype);
		TransactionResponse Delete(long id);
	}
}