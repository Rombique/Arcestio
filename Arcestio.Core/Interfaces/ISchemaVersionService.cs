using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arcestio.Core.Interfaces
{
	public interface ISchemaVersionService
	{
		public Task CreateTableIfNotExistsAsync();
		public Task AddNewRowAsync(SchemaVersion version);
		public Task<IEnumerable<SchemaVersion>> GetSchemaVersionsForFolderAsync(string folder);
		public Task<SchemaVersion> TryGetSchemaVersionAsync(string type, string version);
	}
}