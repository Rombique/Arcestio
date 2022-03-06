using System.Threading.Tasks;
using Arcestio.Core;
using Arcestio.Core.Interfaces;
using Arcestio.Logic;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Arcestio
{
	public class Startup
	{
		static async Task Main(string[] args)
		{
			await Parser.Default.ParseArguments<CommandLineOptions>(args)
				.WithParsedAsync(async options => await InitializeApplication(args, options));
		}
		
		private static async Task InitializeApplication(string[] args, CommandLineOptions options)
		{
			var builder = new HostBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					services.AddOptions();
					services.AddSingleton(options);
					services.AddHostedService<ConsoleHostedService>();
					services.Add(new ServiceDescriptor(
						typeof(IConsoleAppRunner),
						typeof(ConsoleAppRunner),
						ServiceLifetime.Singleton));
					DiConfiguration.ConfigureServices(services);
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.ClearProviders();
					logging.AddConsole(opts =>
					{
						opts.IncludeScopes = false;
					});
				});

			await builder.RunConsoleAsync();
		}
	}
}