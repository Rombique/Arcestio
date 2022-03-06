using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Arcestio.Core;
using Arcestio.Core.Entities;
using Arcestio.Core.Interfaces;

namespace Arcestio.Logic
{
	public class ScriptsReader : IScriptsReader
	{
		private readonly string _folderPath;

		public ScriptsReader(CommandLineOptions options)
			: this(options.Path)
		{
		}
		
		public ScriptsReader(string folderPath)
		{
			_folderPath = folderPath;
		}

		public async Task<ICollection<Script>> GetScriptsInFolderAsync(string folderName)
		{
			var folderPath = $"{_folderPath}/{folderName}";
			var result = new List<Script>();
			if (!Directory.Exists(folderPath))
				return result;

			var files = Directory.EnumerateFiles(folderPath, "*.sql", SearchOption.AllDirectories).ToList();

			foreach (var file in files)
			{
				var filename = Path.GetFileNameWithoutExtension(file);
				var path = Path.GetDirectoryName(file);
				var content = await File.ReadAllTextAsync(file);
				var script = new Script(filename, folderName, path, content);
				result.Add(script);
			}

			return result;
		}
	}
}