using System.Data;
using Npgsql;

namespace Arcestio.PostrgesqlProvider
{
	public class DbProvider : IDbProvider
	{
		public string ConnectionString { get; }

		public DbProvider(string connectionString) =>
			ConnectionString = connectionString;

		public NpgsqlConnection Create()
			=> new NpgsqlConnection(ConnectionString);
	}
}