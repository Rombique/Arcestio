using System.Threading.Tasks;
using Arcestio.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace Arcestio.MSSQLProvider
{
	public class MigrationService : IMigrationService
	{
		private readonly IDbProvider _provider;

		public MigrationService(string connectionString)
		{
			_provider = new DbProvider(connectionString);
		}
		
		public async Task TryExecuteScript(string sql)
		{
			await using var connection = _provider.Create();
			await connection.OpenAsync();
			var command = new SqlCommand(sql, connection);
			await command.ExecuteNonQueryAsync();
			await connection.CloseAsync();
		}
	}
}