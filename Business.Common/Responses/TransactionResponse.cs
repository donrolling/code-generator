using System;
using Business.Common.Statuses;

namespace Business.Common.Responses {
	public class TransactionResponse {
		/// <summary>
		/// Should be the id of the new or updated entity.
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// Type of database transaction performed.
		/// </summary>
		public ActionType ActionType { get; set; }
		/// <summary>
		/// Enumeration of success or failure.
		/// </summary>
		public Status Status { get; set; }
		/// <summary>
		/// General enumeration to describe common reasons for transaction failure.
		/// </summary>
		public StatusDetail StatusDetail { get; set; }
		/// <summary>
		/// Quick way to see if the transaction succeeded.
		/// </summary>
		public bool Success { get; set; }
		/// <summary>
		/// Will generally give the System.Exception message.
		/// </summary>
		public string ErrorMessage { get; set; }
		/// <summary>
		/// When possible, the repository will put the sql string here when a transaction fails.
		/// </summary>
		public string SQL { get; set; }
		/// <summary>
		/// Provides the values passed into the parameterized sql string
		/// </summary>
		public dynamic Params { get; set; }
		/// <summary>
		/// If a value other than the Id is expected, it can be passed back here.
		/// </summary>
		public object TransactionResult { get; set; }

		public void IfFailed_ThrowException() {
			if (!this.Success) {
				this.ThrowException();
			}
		}

		public void ThrowException() { //todo: you could add more stuff
			throw new Exception(this.ErrorMessage);
		}

		public static TransactionResponse GetTransactionResponse(ActionType actionType, Status status, StatusDetail statusDetail) {
			return GetTransactionResponse(actionType, status, 0, string.Empty, string.Empty, null);
		}

		public static TransactionResponse GetTransactionResponse(ActionType actionType, Status status, StatusDetail statusDetail, long id) {
			return GetTransactionResponse(actionType, status, statusDetail, id, string.Empty, string.Empty, null);
		}

		public static TransactionResponse GetTransactionResponse(ActionType actionType, Status status, StatusDetail statusDetail, long id, string errorMessage) {
			return GetTransactionResponse(actionType, status, statusDetail, id, errorMessage, string.Empty, null);
		}

		public static TransactionResponse GetTransactionResponse(ActionType actionType, Status status, StatusDetail statusDetail, long id, string errorMessage, string sql) {
			return GetTransactionResponse(actionType, status, statusDetail, id, errorMessage, sql, null);
		}

		public static TransactionResponse GetTransactionResponse(ActionType actionType, Status status, StatusDetail statusDetail, string errorMessage, string sql, dynamic parameters) {
			return GetTransactionResponse(actionType, status, statusDetail, 0, errorMessage, sql, parameters);
		}

		public static TransactionResponse GetTransactionResponse(ActionType actionType, Status status, StatusDetail statusDetail, long id, string errorMessage, string sql, dynamic parameters) {
			return GetTransactionResponse(actionType, status, statusDetail, id, errorMessage, sql, parameters, new { });
		}

		public static TransactionResponse GetTransactionResponse(ActionType actionType, Status status, StatusDetail statusDetail, long id, string errorMessage, string sql, dynamic parameters, dynamic transactionResult) {
			var result = new TransactionResponse {
				ActionType = actionType,
				Status = status,
				StatusDetail = statusDetail,
				Id = id,
				ErrorMessage = errorMessage,
				SQL = sql,
				Params = parameters,
				Success = status == Status.Success,
				TransactionResult = transactionResult
			};
			//todo: need to clean the following code up. The try catch is super wasteful and is happening a lot.
			if (id == 0) {
				try {
					result.Id = parameters.Get<long>("Id");
				} catch (Exception) {
					if (result.Id == 0) {
						try {
							if (parameters.Id != 0) {
								result.Id = parameters.Id;								
							}
						} catch (Exception) { }
					}
				}
			}

			return result;
		}
	}
}
