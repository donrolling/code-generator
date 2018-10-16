using Business.Common.Responses;

namespace Data.Repository.Interfaces {
	public interface IRepository {
		string ConnectionString { get; }
		string ServerName { get; }
		string DatabaseName { get; }

		TransactionResponse Execute(string sql, dynamic param);
	}
}
