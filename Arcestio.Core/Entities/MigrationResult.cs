namespace Arcestio.Core.Entities
{
	public class MigrationResult
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public int PrevHash { get; set; }
		public int? CurrentHash { get; set; }

		public bool IsNotValid => PrevHash != CurrentHash;
	}
}