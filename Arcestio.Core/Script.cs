using System.Linq;

namespace Arcestio.Core
{
	public class Script
	{
		public string Name { get; }
		public string Folder { get; }
		public string SQL { get; }

		public string Version => Name.Split("_").First();
		public string Description => Name.Split("_").Last();

		public string FileName => $"{Name}.sql";
		

		public Script()
		{
		}
		
		public Script(string name, string folder, string sql)
		{
			Name = name;
			Folder = folder;
			SQL = sql;
		}
	}
}