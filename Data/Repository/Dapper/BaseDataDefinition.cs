using System.Collections.Generic;
using Business.Common;
using Business.Common.DataTables;
using Dapper;
using Data.Presentation;
using Data.Repository.Interfaces;

namespace Data.Repository.Dapper {
	public class BaseDataDefinition {
		/// <summary>
		/// Creates the string neccessary to call a scalar sql function.
		/// </summary>
		/// <param name="signature">The parameters that make up the signature of the sql function</param>
		/// <param name="databaseSchema">The sql database schema of the function that is being called.</param>
		/// <param name="userDefinedFunctionName">The name of the sql function that is being called</param>
		/// <returns></returns>
		public string GetScalarFunctionCallSQL(string signature, string databaseSchema, string userDefinedFunctionName) {
			return SQL_Helper.GetScalarFunctionCallSQL(signature, databaseSchema, userDefinedFunctionName);
		}

		/// <summary>
		/// Creates the string neccessary to call a table-valued sql function.
		/// </summary>
		/// <param name="selectedFields">The fields that should be selected from the result set</param>
		/// <param name="signature">The parameters that make up the signature of the sql function</param>
		/// <param name="databaseSchema">The sql database schema of the function that is being called.</param>
		/// <param name="userDefinedFunctionName">The name of the sql function that is being called</param>
		/// <returns></returns>
		public string GetTableValuedFunctionCallSQL(string selectedFields, string signature, string databaseSchema, string userDefinedFunctionName) {
			return SQL_Helper.GetTableValuedFunctionCallSQL(selectedFields, signature, databaseSchema, userDefinedFunctionName);
		}

		/// <summary>
		/// Creates the string neccessary to call a table-valued sql function.
		/// </summary>
		/// <param name="signature">The parameters that make up the signature of the sql function</param>
		/// <param name="databaseSchema">The sql database schema of the function that is being called.</param>
		/// <param name="userDefinedFunctionName">The name of the sql function that is being called</param>
		/// <returns></returns>
		public string GetTableValuedFunctionCallSQL(string signature, string databaseSchema, string userDefinedFunctionName) {
			return SQL_Helper.GetTableValuedFunctionCallSQL(string.Empty, signature, databaseSchema, userDefinedFunctionName);
		}

		public IPresentable<T> PagedSQL<T>(IDapperRepository repository, DynamicParameters dynamicParameters, string baseQuery, PageInfo pageInfo) where T : class {
			SQL_Helper.GetDynamicData_ForPaging(dynamicParameters, pageInfo.PageStart, pageInfo.PageSize);
			SQL_Helper.Add_SearchFilter_Parameters(ref baseQuery, dynamicParameters, pageInfo.Filters);
			var query = SQL_Helper.WrapQueryInRowNumberQuery(baseQuery, pageInfo.OrderBy);
			var data = repository.Query<T>(query, dynamicParameters);
			int total = SQL_Helper.GetQueryTotal(baseQuery, dynamicParameters, repository.ConnectionString);
			var result = new PresentationList<T>(data, pageInfo.PageSize, total);
			return result;
		}

		public IPresentable<T> Paged<T>(IDapperRepository repository, DynamicParameters dynamicParameters, string signature, string databaseSchema, string userDefinedFunctionName, PageInfo pageInfo) where T : class { 
			var baseQuery = SQL_Helper.GetTableValuedFunctionCallSQL(string.Empty, signature, databaseSchema, userDefinedFunctionName);
			SQL_Helper.GetDynamicData_ForPaging(dynamicParameters, pageInfo.PageStart, pageInfo.PageSize);
			SQL_Helper.Add_SearchFilter_Parameters(ref baseQuery, dynamicParameters, pageInfo.Filters);
			var query = SQL_Helper.WrapQueryInRowNumberQuery(baseQuery, pageInfo.OrderBy);
			var data = repository.Query<T>(query, dynamicParameters);
			int total = SQL_Helper.GetQueryTotal(baseQuery, dynamicParameters, repository.ConnectionString);
			var result = new PresentationList<T>(data, pageInfo.PageSize, total);
			return result;
		}

		/// <summary>
		/// Calls a sql table-valued function.
		/// </summary>
		/// <typeparam name="T">The dto or entity data-type to be populated and returned by the dataset.</typeparam>
		/// <param name="repository">The database context to use to make the function call.</param>
		/// <param name="sql">The SQL script to use to make the function call.</param>
		/// <param name="parameters">The Dynamic Parameters object that makes up the values to be passed to the function in the signature.</param>
		/// <returns></returns>
		public IEnumerable<T> CallTableValuedFunction<T>(IDapperRepository repository, string sql, DynamicParameters parameters) where T : class {
			return repository.Query<T>(sql, parameters);
		}
	}
}