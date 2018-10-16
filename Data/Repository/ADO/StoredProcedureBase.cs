using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Business.Common;
using Business.Common.Logging;
using Business.Common.Responses;
using Business.Common.Statuses;

namespace Data.Repository.ADO {
	public abstract class StoredProcedureBase {
		protected IRepositoryLogger Logger { get; set; }

		public string ConnectionString { get; protected set; }
		public bool ProcedureExecuted { get; protected set; }

		public StoredProcedureBase(IRepositoryLogger logger) {
			this.Logger = logger;
		}

		private readonly List<SqlParameter> _inputParameters = new List<SqlParameter>();
		public List<SqlParameter> InputParameters {
			get {
				return this._inputParameters;
			}
		}

		protected List<SqlParameter> _outputParameters = new List<SqlParameter>();
		public List<SqlParameter> OutputParameters {
			get {
				return this._outputParameters;
			}
		}

		/// <summary>
		/// This is the default timeout for sql commands
		/// </summary>
		public const int DefaultCommentTimeout = 30;
		
		/// <summary>
		/// May be set by any class that inherits from StoredProcedureBase
		/// </summary>
		private int _commandTimeout = DefaultCommentTimeout;
		/// <summary>
		/// In seconds.
		/// </summary>
		protected int CommandTimeout {
			get { return _commandTimeout; }
			set { _commandTimeout = value; }
		}

		/// <summary>
		/// Must be set by the class that inherits from StoredProcedureBase
		/// </summary>
		public string ProceedureName { get; protected set; }

		protected int _requiredInputParameterCount = -1;
		protected int _requiredOutputParameterCount = -1;
		
		/// <summary>
		/// Don't need to use a using statement here, it's inherited
		/// </summary>
		/// <param name="reader">
		/// SqlDataReader
		/// </param>
		protected abstract void BindOutputParameters(SqlCommand command, SqlDataReader reader);
		
		/// <summary>
		///     this shouldn't change between procs, but belongs here rather than in the abstract class because of the enums (maybe figure out how to abstract this away later?)
		/// </summary>
		protected abstract void InitializeDefaults();
		
		/// <summary>
		///     set any output params
		/// </summary>
		protected abstract void InitializeOutputParameters();

		protected TransactionResponse Execute() {
			this.InitializeOutputParameters();

			this.validateParameters();
			
			var status = Status.Success;
			var errorMessage = string.Empty;
			try {
				var transactionResult = this.execute();
				return TransactionResponse.GetTransactionResponse(ActionType.Execute, status, StatusDetail.OK, 0, errorMessage, this.ProceedureName, null, transactionResult);
			} catch (Exception ex) {
				status = Status.Failure;
				errorMessage = this.Logger.Log(ActionType.Execute, ex, this.ProceedureName, null, string.Empty);
				this.Logger.Log(ActionType.Execute, errorMessage);
			}

			return TransactionResponse.GetTransactionResponse(ActionType.Execute, status, StatusDetail.Unknown, 0, errorMessage, this.ProceedureName, null);
		}

		private int execute() {
			int retval = 0;
			using (var _connection = new SqlConnection(this.ConnectionString)) {
				var _command = new SqlCommand(this.ProceedureName, _connection);
				_command.CommandType = CommandType.StoredProcedure;
				_command.CommandText = this.ProceedureName;

				foreach (var inputParameter in this.InputParameters) {
					_command.Parameters.Add(inputParameter);
				}

				foreach (var outputParameter in this.OutputParameters) {
					_command.Parameters.Add(outputParameter);
				}

				if (this.CommandTimeout != DefaultCommentTimeout) {
					_command.CommandTimeout = this.CommandTimeout;
				} else {
					_command.CommandTimeout = DefaultCommentTimeout;
				}

				_connection.Open();

				using (var reader = _command.ExecuteReader()) {
					this.BindOutputParameters(_command, reader);
				}

				this.ProcedureExecuted = true;
			}

			return retval;
		}

		private void validateParameters() {
			if (string.IsNullOrEmpty(this.ProceedureName)) {
				throw new Exception("Proceedure Name is required to be set from the child constructor.");
			}

			if (string.IsNullOrEmpty(this.ConnectionString)) {
				throw new Exception("Connection String is required to be set from the child constructor.");
			}

			if (this.InputParameters.Count < this._requiredInputParameterCount) {
				throw new Exception(
				"All the required input parameters must be set for this proceedure to run properly.");
			}

			if (this.OutputParameters.Count < this._requiredOutputParameterCount) {
				throw new Exception(
				"All the required output parameters must be set for this proceedure to run properly.");
			}
		}
	}
}