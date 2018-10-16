using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Business.Common;
using Business.Common.DataTables;
using Dapper;

namespace Data.Repository {
	public static class SQL_Helper {
		public static int GetQueryTotal(string baseQuery, DynamicParameters dynamicData, string connectionString) {
			var countQuery = GetCountQuery(baseQuery);
			int total = 0;
			using (var connection = new SqlConnection(connectionString)) {
				connection.Open();
				total = SqlMapper.Query<int>(connection, countQuery, dynamicData).First();
			}
			return total;
		}
		
		public static string WrapQueryInRowNumberQuery(string baseQuery, string orderBy) {
			if (string.IsNullOrEmpty(orderBy)) {
				orderBy = "Id";
			}
			var frontAndBack = GetQueryFrontAndBack(baseQuery);
			//this assumes that the last FROM in your query is the one with the joins and stuff.
			//it helps if you have subselects, but could get hosed in certain circumstances
			var realQuery = string.Concat(
				"WITH result AS(",
				frontAndBack.Item1,
				", ROW_NUMBER() OVER (ORDER BY ",
				orderBy,
				") AS RowNumber ",
				frontAndBack.Item2,
				") SELECT * FROM result WHERE RowNumber BETWEEN @rowStart and @rowEnd");
			return realQuery;
		}

		public static string GetCountQuery(string query) {
			var back = GetQueryFrontAndBack(query).Item2;
			if (back.Contains("order by")) {
				var pos = back.IndexOf("order by");
				back = back.Substring(0, pos);
			}
			var countQuery = string.Concat("select count(*) as Total ", back);
			return countQuery;
		}

		public static Tuple<string, string> GetQueryFrontAndBack(string baseQuery) {
			var lastFrom = baseQuery.LastIndexOf("from", StringComparison.CurrentCultureIgnoreCase);
			var frontPart = baseQuery.Substring(0, lastFrom);
			var backPart = baseQuery.Substring(lastFrom, baseQuery.Length - lastFrom);
			return Tuple.Create<string, string>(frontPart, backPart);
		}

		public static string PrepareForLikeQuery(string searchText, bool searchLeftSide, bool searchRightSide) {
			if (string.IsNullOrEmpty(searchText)) {
				return searchText;
			}

			var value = searchText.Replace("%", "[%]").Replace("[", "[[]").Replace("]", "[]]");

			if (searchLeftSide) {
				value = "%" + value;
			}
			if (searchRightSide) {
				value += "%";
			}

			return value;
		}

		public static string PrepareLikeParameter(string likeParameter) {
			return string.Concat(" ", likeParameter, " LIKE @", likeParameter);
		}

		public static void GetDynamicData_ForPaging(DynamicParameters dynamicParameters, int pageStart, int pageSize) {
			dynamicParameters.Add("rowStart", pageStart + 1);
			var rowEnd = pageSize > 0 ? pageStart + pageSize : 100;//if 0 is passed in, then default to 100. IDK if this is a good idea, but it should work for now.
			dynamicParameters.Add("rowEnd", rowEnd);
		}

		public static DynamicParameters GetDynamicData_ForPaging(int pageStart, int pageSize) { 
			var dynamicParameters = new DynamicParameters();
			dynamicParameters.Add("rowStart", pageStart + 1);
			var rowEnd = pageSize > 0 ? pageStart + pageSize : 100;//if 0 is passed in, then default to 100. IDK if this is a good idea, but it should work for now.
			dynamicParameters.Add("rowEnd", rowEnd);
			return dynamicParameters;
		}
		
		/// <summary>
		/// Creates the string neccessary to call a scalar sql function.
		/// </summary>
		/// <param name="signature">The parameters that make up the signature of the sql function</param>
		/// <param name="databaseSchema">The sql database schema of the function that is being called.</param>
		/// <param name="userDefinedFunctionName">The name of the sql function that is being called</param>
		/// <returns></returns>
		public static string GetScalarFunctionCallSQL(string signature, string databaseSchema, string userDefinedFunctionName) {
			if (string.IsNullOrEmpty(databaseSchema)) {
				databaseSchema = "dbo";
			}
			var baseCall = string.Concat("SELECT [{0}].[{1}]({2})");
			return string.Format(baseCall, databaseSchema, userDefinedFunctionName, signature);
		}
		
		/// <summary>
		/// Creates the string neccessary to call a table-valued sql function.
		/// </summary>
		/// <param name="selectedFields">The fields that should be selected from the result set</param>
		/// <param name="signature">The parameters that make up the signature of the sql function</param>
		/// <param name="databaseSchema">The sql database schema of the function that is being called.</param>
		/// <param name="userDefinedFunctionName">The name of the sql function that is being called</param>
		/// <returns></returns>
		public static string GetTableValuedFunctionCallSQL(string selectedFields, string signature, string databaseSchema, string userDefinedFunctionName) {
			if (string.IsNullOrEmpty(databaseSchema)) {
				databaseSchema = "dbo";
			}
			if (string.IsNullOrEmpty(selectedFields)) {
				selectedFields = "*";
			}
			var baseCall = string.Concat("SELECT {0} FROM [{1}].[{2}]({3})");
			return string.Format(baseCall, selectedFields, databaseSchema, userDefinedFunctionName, signature);
		}
		
		public static void Add_SearchFilter_Parameters(ref string baseQuery, DynamicParameters dynamicParameters, List<SearchFilter> searchFilters) {
			if (searchFilters == null || !searchFilters.Any()) {
				return;
			}
			var filterCount = searchFilters.Count;
			if (filterCount <= 0) {
				return;
			}
			for (int i = 0; i < filterCount; i++) {
				var filter = searchFilters[i];
				if (isFilterValid(filter)) {
					if (!baseQuery.Contains("WHERE")) {
						baseQuery += " WHERE";
					}
					addFilter(ref baseQuery, i, filterCount, filter, dynamicParameters);
				}
			}
		}

		private static void addFilter(ref string baseQuery, int index, int filterCount, SearchFilter filter, DynamicParameters dynamicParameters) {
			var conditionSeperator = (filterCount > 1 && index > 0 && index < filterCount - 1) ? " and" : "";//todo: should be able to pass in and / or
			baseQuery += string.Concat(conditionSeperator, PrepareLikeParameter(filter.Name));
			if (filter.Type == typeof(string)) {
				var filterString = PrepareForLikeQuery(filter.Value.ToString(), true, true);
				dynamicParameters.Add(filter.Name, filterString);
			}else {
				dynamicParameters.Add(filter.Name, filter.Value);
			}
		}

		private static bool isFilterValid(SearchFilter filter) {
			if (filter.Type == typeof(string) && string.IsNullOrEmpty(filter.Value.ToString())) {
				return false;
			}
			//todo: add other things?
			return true;
		}
	}
}