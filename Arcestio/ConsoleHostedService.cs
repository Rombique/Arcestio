using System;
using System.Threading;
using System.Threading.Tasks;
using Arcestio.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Arcestio
{
	public class ConsoleHostedService : IHostedService
	{
		private int _exitCode;
		
		private readonly ILogger<ConsoleHostedService> _logger;
		private readonly IHostApplicationLifetime _lifetime;
		private readonly IConsoleAppRunner _appRunner;
		
		public ConsoleHostedService(
			ILogger<ConsoleHostedService> logger,
			IHostApplicationLifetime lifetime,
			IConsoleAppRunner appRunner
			)
		{
			_logger = logger;
			_lifetime = lifetime;
			_appRunner = appRunner;
			_exitCode = 1;
		}
		
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_lifetime.ApplicationStarted.Register(() => Task.Run(OnApplicationStarted, cancellationToken));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private async Task OnApplicationStarted()
		{
			try
			{
				_exitCode = await _appRunner.Main(Environment.GetCommandLineArgs());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception in console application");
			}
			finally
			{
				Environment.ExitCode = _exitCode;
				_lifetime.StopApplication();
			}
		}
	}
}