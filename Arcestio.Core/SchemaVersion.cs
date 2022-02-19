namespace Arcestio.Core
{
	public class SchemaVersion
	{
		public long Id { get; set; }
		public string Version { get; set; }
		public string Description { get; set; }
		public string Folder { get; set; }
		public string Script { get; set; }
		public string Hash { get; set; }
		public long InstalledOn { get; set; }
		public long ExecutionTime { get; set; }
		public string Message { get; set; }
		public bool Success { get; set; }
	}
}