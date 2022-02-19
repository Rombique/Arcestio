using CommandLine;

namespace Arcestio.Core
{
	public class CommandLineOptions
	{
		[Option(
			shortName: 'd',
			longName: "DatabaseProvider", 
			Required = true, 
			HelpText = "SQL database provider")]
		public string DatabaseProvider { get; set; }
		
		[Option(
			shortName: 'c', 
			longName: "ConnectionString",
			Required = true, 
			HelpText = "ConnectionString")]
		public string ConnectionString { get; set; }
		
		[Option(
			shortName: 'f', 
			longName: "Folders",
			Required = true, 
			HelpText = "Folders")]
		public string Folders { get; set; }
		
		[Option(
			shortName: 'p', 
			longName: "Path",
			Required = false, 
			Default = "SQL",
			HelpText = "Path to *.sql files")]
		public string Path { get; set; }
	}
}