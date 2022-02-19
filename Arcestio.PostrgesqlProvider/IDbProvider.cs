using System.Data;
using Npgsql;

namespace Arcestio.PostrgesqlProvider
{
	public interface IDbProvider
	{
		string ConnectionString { get; }
		NpgsqlConnection Create();
	}
}