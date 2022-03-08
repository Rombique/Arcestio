using Microsoft.Data.SqlClient;

namespace Arcestio.MSSQLProvider
{
	public interface IDbProvider
	{
		string ConnectionString { get; }
		SqlConnection Create();
	}
}