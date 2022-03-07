using System.Linq;
using Arcestio.Core.Extensions;

namespace Arcestio.Core.Entities
{
	public class Script
	{
		public string Name { get; }
		public string Path { get; }
		public string Folder { get; }
		public string SQL { get; }

		public string Version => Name.Split("_").First();
		public string Description => Name.Split("_", 2).Last();
		
		public Script()
		{
		}
		
		public Script(string name, string folder, string path, string sql)
		{
			Name = name;
			Folder = folder;
			SQL = sql;
			Path = path;
		}

		public override int GetHashCode()
		{
			return SQL.GetDeterministicHashCode();
		}
	}
}