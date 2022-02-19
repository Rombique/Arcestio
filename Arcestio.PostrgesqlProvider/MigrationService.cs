using System.Threading.Tasks;
using Arcestio.Core.Interfaces;
using Npgsql;

namespace Arcestio.PostrgesqlProvider
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
			var command = new NpgsqlCommand(sql, connection);
			connection.Open();
			await command.ExecuteNonQueryAsync();
			await connection.CloseAsync();
		}
	}
}