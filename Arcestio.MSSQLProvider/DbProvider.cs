using Microsoft.Data.SqlClient;

namespace Arcestio.MSSQLProvider
{
	public class DbProvider : IDbProvider
	{
		public string ConnectionString { get; }
		
		public DbProvider(string connectionString) =>
			ConnectionString = connectionString;
		
		public SqlConnection Create()
			=> new SqlConnection(ConnectionString);
	}
}