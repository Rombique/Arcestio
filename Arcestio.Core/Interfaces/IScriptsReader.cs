using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arcestio.Core.Interfaces
{
	public interface IScriptsReader
	{
		public Task<List<Script>> GetScriptsInFolderAsync(string folderName);
	}
}