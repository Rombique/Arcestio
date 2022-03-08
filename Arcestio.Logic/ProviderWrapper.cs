using Arcestio.Core;
using Arcestio.Core.Interfaces;
using Postgresql = Arcestio.PostrgesqlProvider;
using MSSQL = Arcestio.MSSQLProvider;

namespace Arcestio.Logic
{
	public class ProviderWrapper : IProviderWrapper
	{
		public ISchemaVersionService SchemaVersionService { get; }
		public IMigrationService MigrationService { get; }

		public ProviderWrapper(CommandLineOptions options)
		{
			switch (options.DatabaseProvider)
			{
				case Constants.Postgresql:
					SchemaVersionService = new Postgresql.SchemaVersionService(options.ConnectionString);
					MigrationService = new Postgresql.MigrationService(options.ConnectionString);
					break;
				case Constants.MSSQL:
					SchemaVersionService = new MSSQL.SchemaVersionService(options.ConnectionString);
					MigrationService = new MSSQL.MigrationService(options.ConnectionString);
					break;
			}
		}
	}
}