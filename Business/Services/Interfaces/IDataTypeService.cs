using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Data.Models.Entities;

namespace Business.Services {
	public interface IDataTypeService {
		TransactionResponse Create(DataType model);
		TransactionResponse Update(DataType model);
		TransactionResponse Delete(long id);
	}
}