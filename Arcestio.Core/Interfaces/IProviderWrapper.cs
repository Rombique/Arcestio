namespace Arcestio.Core.Interfaces
{
	public interface IProviderWrapper
	{
		public ISchemaVersionService SchemaVersionService { get; }
		public IMigrationService MigrationService { get; }
	}
}