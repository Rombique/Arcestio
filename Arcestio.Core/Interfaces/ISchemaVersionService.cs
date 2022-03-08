using System.Collections.Generic;
using System.Threading.Tasks;
using Arcestio.Core.Entities;

namespace Arcestio.Core.Interfaces
{
	public interface ISchemaVersionService
	{
		public Task CreateTableIfNotExistsAsync();
		public Task AddNewRowAsync(SchemaVersion version);
		public Task<ICollection<SchemaVersion>> GetSchemaVersionsForFolderAsync(string folder);
		public Task<ICollection<SchemaVersion>> GetAllSchemaVersionsAsync();
		public Task<SchemaVersion> TryGetSchemaVersionAsync(string path, string version);
	}
}