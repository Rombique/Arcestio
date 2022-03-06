using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Arcestio.Core;
using Arcestio.Core.Entities;
using Arcestio.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Arcestio.Logic
{
	public class Worker : IWorker
	{
		private readonly ILogger<Worker> _logger;
		private readonly IScriptsReader _scriptsReader;
		private readonly IProviderWrapper _providerWrapper;
		private readonly string[] _folderNames;
		private ICollection<Folder> _foldersList = new List<Folder>();

		public Worker(
			ILogger<Worker> logger,
			IProviderWrapper providerWrapper,
			IScriptsReader scriptsReader,
			CommandLineOptions options)
		{
			_logger = logger;
			_scriptsReader = scriptsReader;
			_folderNames = options.Folders.Split(",");
			_providerWrapper = providerWrapper;
		}
		
		public async Task DoWorkAsync()
		{
			await _providerWrapper.SchemaVersionService.CreateTableIfNotExistsAsync();
			await SetupFoldersAsync();

			var migrationResults = (await GetMigrationsResultsAsync()).ToList();
			var migrationWithErrors = migrationResults.Where(p => p.IsNotValid).ToList();
			migrationWithErrors.ForEach(p =>
				_logger.LogCritical($"Migration {p.Name} from {p.Folder} is not valid!" +
				                    $"\nPrevious hash: {p.PrevHash}. Current hash: {p.CurrentHash}."));
			if (migrationWithErrors.Any())
			{
				throw new Exception("One or more migrations have not been validated");
			}

			await TryMigrateScriptsAsync(migrationResults);
		}

		private async Task SetupFoldersAsync()
		{
			foreach (var folderName in _folderNames)
			{
				var scripts = await _scriptsReader.GetScriptsInFolderAsync(folderName);
				var subfolders = scripts.GroupBy(p => p.Path);
				foreach (var subfolder in subfolders)
				{
					var newFolder = new Folder
					{
						Path = subfolder.Key,
						Scripts = subfolder.Where(p => p.Path == subfolder.Key).ToList()
					};
					_foldersList.Add(newFolder);
				}
			}
		}

		private async Task TryMigrateScriptsAsync(ICollection<MigrationResult> migrationResults)
		{
			foreach (var folder in _foldersList)
			{
				var scripts = folder.Scripts;
				var alreadyMigratedScripts = scripts
					.Where(p => migrationResults.Any(mr => mr.Name == p.Name));
				var notMigratedYet = scripts
					.Except(alreadyMigratedScripts);
				foreach (var script in notMigratedYet)
				{
					await TryMigrateScriptAsync(script);
				}
			}
		}

		private async Task TryMigrateScriptAsync(Script script)
		{
			var existedSchemaVersion = 
				await _providerWrapper.SchemaVersionService.TryGetSchemaVersionAsync(script.Folder, script.Version);
			var hashCode = script.GetHashCode();
			
			if (existedSchemaVersion == null || hashCode != existedSchemaVersion.HashCode)
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
						ex.Message,
						false
					);
					await _providerWrapper.SchemaVersionService.AddNewRowAsync(schemaVersion);
					_logger.LogError($"Migration {script.Name} was executed with error:\n {ex.Message}");
					throw;
				}
			}
		}

		private async Task<IEnumerable<MigrationResult>> GetMigrationsResultsAsync()
		{
			var schemaVersions = await _providerWrapper.SchemaVersionService.GetAllSchemaVersionsAsync();
			var checkMigrations = _foldersList
				.Select(p => TryValidateFolderScriptsAsync(schemaVersions, p))
				.ToList();
			return checkMigrations.SelectMany(p => p).ToList();
		}
		
		private ConcurrentBag<MigrationResult> TryValidateFolderScriptsAsync(ICollection<SchemaVersion> schemaVersions, Folder folder)
		{
			var result = new ConcurrentBag<MigrationResult>();
			schemaVersions
				.Where(p => p.Path == folder.Path)
				.AsParallel()
				.ForAll(sv =>
			{
				var successScriptInFolder = folder
					.Scripts
					.SingleOrDefault(s => s.Name == sv.Script && s.Version == sv.Version && sv.Success);
				
				if (successScriptInFolder == null &&
				    folder.Scripts.Any(s => s.Name == sv.Script && s.Version == sv.Version)) 
					return;
				
				var migrationResult = new MigrationResult
				{
					Folder = sv.Path,
					Name = sv.Script,
					CurrentHash = successScriptInFolder?.GetHashCode(),
					PrevHash = sv.HashCode
				};
				result.Add(migrationResult);
			});
			return result;
		}

		private static SchemaVersion GetSchemaVersion(
			Script script, 
			long executionTime,
			string message,
			bool success)
		{
			return new SchemaVersion
			{
				Version = script.Version,
				Description = script.Description,
				Script = script.Name,
				ExecutionTime = executionTime,
				HashCode = script.GetHashCode(),
				InstalledOn = (long) new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds,
				Path = script.Path,
				Message = message,
				Success = success
			};
		}
	}
}