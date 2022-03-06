using System.Collections.Generic;
using System.Threading.Tasks;
using Arcestio.Core.Entities;

namespace Arcestio.Core.Interfaces
{
	public interface IScriptsReader
	{
		public Task<ICollection<Script>> GetScriptsInFolderAsync(string folderName);
	}
}