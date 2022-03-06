namespace Arcestio.Core.Entities
{
	public class SchemaVersion
	{
		public long Id { get; set; }
		public string Version { get; set; }
		public string Description { get; set; }
		public string Path { get; set; }
		public string Script { get; set; }
		public int HashCode { get; set; }
		public long InstalledOn { get; set; }
		public long ExecutionTime { get; set; }
		public string Message { get; set; }
		public bool Success { get; set; }
	}
}