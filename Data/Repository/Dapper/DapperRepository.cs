using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Business.Common;
using Business.Common.Logging;
using Business.Common.Responses;
using Business.Common.Statuses;
using Dapper;
using Data.Repository.Interfaces;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Data.Repository.Dapper {
	public class DapperRepository : RepositoryBase, IDapperRepository {
		private string _connectionString;
		public string ConnectionString {
			get {
				if (_connectionString == null) {
					throw new Exception("Repository must be initialized with a connection string.");
				}
				return _connectionString;
			}
			set {
				_connectionString = value;
				var builder = new SqlConnectionStringBuilder(_connectionString);
				ServerName = builder.DataSource;
				DatabaseName = builder.InitialCatalog;
			}
		}
		public string ServerName { get; internal set; }
		public string DatabaseName { get; internal set; }

		public DapperRepository(string connectionString, IRepositoryLogger logger) : base(logger) {
			ConnectionString = connectionString;
		}

		public TransactionResponse Execute(string sql, dynamic param) {
			var status = Status.Success;
			var errorMessage = string.Empty;
			try {
				var transactionResult = 0;
				using (var connection = new SqlConnection(ConnectionString)) {
					connection.Open();
					using (var transaction = connection.BeginTransaction()) {
						transactionResult = SqlMapper.Execute(connection, sql, param, transaction);
						transaction.Commit();
					}
				}
				return TransactionResponse.GetTransactionResponse(ActionType.Execute, status, StatusDetail.OK, 0, errorMessage, sql, param, transactionResult);
			} catch (Exception ex) {
				status = Status.Failure;
				errorMessage = this.Logger.Log(ActionType.Execute, ex, sql, param, string.Empty);
			}

			return TransactionResponse.GetTransactionResponse(ActionType.Execute, status, StatusDetail.Unknown, 0, errorMessage, sql, param);
		}

		/// <summary>
		/// Uses ADO to execute large scritps that won't be returning 
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public TransactionResponse ExecuteScript(string sql) {
			var status = Status.Success;
			var errorMessage = string.Empty;
			try {
				var connection = new SqlConnection(this.ConnectionString);
				var server = new Server(new ServerConnection(connection));
				var transactionResult = server.ConnectionContext.ExecuteNonQuery(sql);
				
				return TransactionResponse.GetTransactionResponse(ActionType.Execute, status, StatusDetail.OK, 0, errorMessage, sql, null, transactionResult);
			} catch (Exception ex) {
				status = Status.Failure;
				errorMessage = this.Logger.Log(ActionType.Execute, ex, sql, null, string.Empty);
			}

			return TransactionResponse.GetTransactionResponse(ActionType.Execute, status, StatusDetail.Unknown, 0, errorMessage, sql, null);
		}

		public IEnumerable<T> QuerySimple<T>(string sql, dynamic param, CommandType commandType = CommandType.Text) where T : struct {
			using (var connection = new SqlConnection(ConnectionString)) {
				connection.Open();
				try {
					var result = SqlMapper.Query<T>(connection, sql, param, commandType: commandType);
					return result;
				} catch (Exception ex) {
					this.Logger.Log(ActionType.Get, ex, sql);
					return null;
				}
			}
		}

		public IEnumerable<T> Query<T>(string sql, dynamic param, CommandType commandType = CommandType.Text) where T : class {
			using (var connection = new SqlConnection(ConnectionString)) {
				connection.Open();
				try {
					var result = SqlMapper.Query<T>(connection, sql, param, commandType: commandType);
					return result;
				} catch (Exception ex) {
					this.Logger.Log(ActionType.Get, ex, sql);
					return null;
				}
			}
		}
		public IEnumerable<R> Query<T, U, R>(string sql, Func<T, U, R> func, dynamic param, string splitOn = null)
			where T : class
			where U : class
			where R : class {
			using (var connection = new SqlConnection(ConnectionString)) {
				connection.Open();
				return SqlMapper.Query<T, U, R>(connection, sql, func, param, splitOn: splitOn);
			}
		}
		public IEnumerable<R> Query<T, U, V, R>(string sql, Func<T, U, V, R> func, dynamic param, string splitOn = null)
			where T : class
			where U : class
			where R : class
			where V : class {
			using (var connection = new SqlConnection(ConnectionString)) {
				connection.Open();
				return SqlMapper.Query<T, U, V, R>(connection, sql, func, param, splitOn: splitOn);
			}
		}
		public IEnumerable<R> Query<T, U, V, W, R>(string sql, Func<T, U, V, W, R> func, dynamic param, string splitOn = null)
			where T : class
			where U : class
			where V : class
			where W : class
			where R : class {
			using (var connection = new SqlConnection(ConnectionString)) {
				connection.Open();
				return SqlMapper.Query<T, U, V, W, R>(connection, sql, func, param, splitOn: splitOn);
			}
		}

		public T ReturnFirst<T>(string sql, DynamicParameters _params) where T : struct {
			try {
				using (var connection = new SqlConnection(ConnectionString)) {
					var result = SqlMapper.Query<T>(connection, sql, _params).FirstOrDefault();
					return result;
				}
			} catch (Exception ex) {
				this.Logger.Log(ActionType.Get, ex, "DapperRepository ReturnFirst<T>", _params, sql);
			}
			T retval = default(T);
			return retval;
		}
		public string ReturnFirstString(string sql, DynamicParameters _params) {
			try {
				using (var connection = new SqlConnection(ConnectionString)) {
					var result = SqlMapper.Query<string>(connection, sql, _params).FirstOrDefault();
					return result;
				}
			} catch (Exception ex) {
				this.Logger.Log(ActionType.Get, ex, "DapperRepository ReturnFirst<T>", _params, sql);
				throw ex;
			}
		}

		protected string formatError(Exception ex) {
			var msg = ex.Message.ToString();
			if (ex.InnerException != null) {
				msg += string.Concat("\nInner Exception: ", ex.InnerException.Message.ToString());
			}
			return msg;
		}

		private long getId<T>(T entity) where T : class {
			try {
				var value = (long)typeof(T).GetProperty("Id").GetValue(entity, null);
				return value;
			} catch (Exception) {
				return 0;
			}
		}
	}
}