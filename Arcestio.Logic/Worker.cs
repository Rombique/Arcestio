using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Arcestio.Core;
using Arcestio.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Arcestio.Logic
{
	public class Worker : IWorker
	{
		private readonly ILogger<Worker> _logger;
		private readonly IScriptsReader _scriptsReader;
		private readonly IProviderWrapper _providerWrapper;
		private readonly IHasher _hasher;
		private readonly string[] _folderNames;

		public Worker(
			ILogger<Worker> logger,
			IProviderWrapper providerWrapper,
			IScriptsReader scriptsReader,
			IHasher hasher,
			CommandLineOptions options)
		{
			_logger = logger;
			_scriptsReader = scriptsReader;
			_folderNames = options.Folders.Split(",");
			_providerWrapper = providerWrapper;
			_hasher = hasher;
		}
		
		public async Task DoWorkAsync()
		{
			await _providerWrapper.SchemaVersionService.CreateTableIfNotExistsAsync();
			foreach (var folder in _folderNames)
			{
				var scripts = await _scriptsReader.GetScriptsInFolderAsync(folder);
				await TryMigrateScriptsAsync(scripts);
			}
			await Task.CompletedTask;
		}

		private async Task TryMigrateScriptsAsync(IEnumerable<Script> scripts)
		{
			foreach (var script in scripts)
				await TryMigrateScriptAsync(script);
		}

		private async Task TryMigrateScriptAsync(Script script)
		{
			var existedSchemaVersion = 
				await _providerWrapper.SchemaVersionService.TryGetSchemaVersionAsync(script.Folder, script.Version);
			var hash = _hasher.Hash(script.Name + script.SQL);

			if (existedSchemaVersion is {Success: false} && existedSchemaVersion.Hash == hash)
			{
				
			}
			else if (existedSchemaVersion == null || hash != existedSchemaVersion.Hash)
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				try
				{
					await _providerWrapper.MigrationService.TryExecuteScript(script.SQL);
					stopwatch.Stop();
					var schemaVersion = GetSchemaVersion(
						script,
						stopwatch.ElapsedMilliseconds,
						hash,
						"",
						true
					);
					await _providerWrapper.SchemaVersionService.AddNewRowAsync(schemaVersion);
					_logger.LogInformation($"Migration {script.Name} executed successfuly");
				}
				catch (Exception ex)
				{
					stopwatch.Stop();
					var schemaVersion = GetSchemaVersion(
						script,
						stopwatch.ElapsedMilliseconds,
						hash,
						ex.Message,
						false
					);
					await _providerWrapper.SchemaVersionService.AddNewRowAsync(schemaVersion);
					_logger.LogError($"Migration {script.Name} was executed with error:\n {ex.Message}");
					throw;
				}
			}
		}

		private static SchemaVersion GetSchemaVersion(
			Script script, 
			long executionTime, 
			string hash,
			string message,
			bool success)
		{
			return new SchemaVersion
			{
				Version = script.Version,
				Description = script.Description,
				Script = script.Name,
				ExecutionTime = executionTime,
				Hash = hash,
				InstalledOn = (long) new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds,
				Folder = script.Folder,
				Message = message,
				Success = success
			};
		}
	}
}