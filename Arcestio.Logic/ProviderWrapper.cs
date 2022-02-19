using Arcestio.Core;
using Arcestio.Core.Interfaces;
using Arcestio.PostrgesqlProvider;

namespace Arcestio.Logic
{
	public class ProviderWrapper : IProviderWrapper
	{
		public ISchemaVersionService SchemaVersionService { get; }
		public IMigrationService MigrationService { get; }

		public ProviderWrapper(CommandLineOptions options)
		{
			if (options.DatabaseProvider == Constants.Postgresql)
			{
				SchemaVersionService = new SchemaVersionService(options.ConnectionString);
				MigrationService = new MigrationService(options.ConnectionString);
			}
		}
	}
}