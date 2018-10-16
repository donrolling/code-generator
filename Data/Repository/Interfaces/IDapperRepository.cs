using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Data.Repository.Interfaces {
	public interface IDapperRepository : IRepository {
		IEnumerable<T> Query<T>(string sql, dynamic param, CommandType commandType = CommandType.Text) where T : class;
		IEnumerable<R> Query<T, U, R>(string sql, Func<T, U, R> func, dynamic param, string splitOn = null) where T : class where U : class where R : class;
		IEnumerable<R> Query<T, U, V, R>(string sql, Func<T, U, V, R> func, dynamic param, string splitOn = null) where T : class where U : class where R : class where V : class;
		IEnumerable<R> Query<T, U, V, W, R>(string sql, Func<T, U, V, W, R> func, dynamic param, string splitOn = null) where T : class where U : class where V : class where W : class where R : class;

		T ReturnFirst<T>(string sql, DynamicParameters _params) where T : struct;
		string ReturnFirstString(string sql, DynamicParameters _params);
	}
}