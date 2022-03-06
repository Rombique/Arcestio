using System.Collections.Generic;

namespace Arcestio.Core.Entities
{
	public class Folder
	{
		public string Path { get; set; }
		public ICollection<Script> Scripts { get; set; }
	}
}