using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Arcestio.Core;
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

		public async Task<List<Script>> GetScriptsInFolderAsync(string folderName)
		{
			var folderPath = $"{_folderPath}/{folderName}";
			var result = new List<Script>();
			if (!Directory.Exists(folderPath))
				return result;

			foreach (var file in Directory.EnumerateFiles(folderPath, "*.sql", SearchOption.AllDirectories))
			{
				var filename = file.Split("\\").Last().Split(".").First();
				var content = await File.ReadAllTextAsync(file);
				var script = new Script(filename, folderName, content);
				result.Add(script);
			}

			return result;
		}
	}
}