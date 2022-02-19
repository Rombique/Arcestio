using Arcestio.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Arcestio.Logic
{
	public static class DiConfiguration
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IScriptsReader, ScriptsReader>();
			services.AddSingleton<IWorker, Worker>();
			services.AddSingleton<IProviderWrapper, ProviderWrapper>();
		}
	}
}