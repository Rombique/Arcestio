using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Arcestio.Core;
using Arcestio.Core.Entities;
using Arcestio.Core.Interfaces;
using Npgsql;

namespace Arcestio.PostrgesqlProvider
{
	public class SchemaVersionService : ISchemaVersionService
	{
		private readonly IDbProvider _provider;
		
		public SchemaVersionService(string connectionString)
		{
			_provider = new DbProvider(connectionString);
		}

		public async Task CreateTableIfNotExistsAsync()
		{
			var sql = $"CREATE TABLE IF NOT EXISTS {Constants.TableName}" +
			          "( " +
			          "id serial PRIMARY KEY, " +
			          "version varchar NOT NULL, " +
			          "description varchar NOT NULL, " +
			          "path varchar NOT NULL, " +
			          "script varchar NOT NULL, " +
			          "hashcode int NOT NULL, " +
			          "installedon bigint NOT NULL, " +
			          "executiontime bigint NOT NULL, " +
			          "message varchar, " +
			          "success boolean NOT NULL" +
			          ")";

			await using var connection = _provider.Create();
			var command = new NpgsqlCommand(sql, connection);
			connection.Open();
			await command.ExecuteNonQueryAsync();
			await connection.CloseAsync();
		}

		public async Task AddNewRowAsync(SchemaVersion version)
		{
			var sql = $"INSERT INTO {Constants.TableName} " +
			          $"({nameof(version.Version)}, {nameof(version.Description)}, " +
			          $"{nameof(version.Path)}, {nameof(version.Script)}, " + $"{nameof(version.HashCode)}, {nameof(version.InstalledOn)}, " +
			          $"{nameof(version.ExecutionTime)}, {nameof(version.Message)}, {nameof(version.Success)}) " +
			          $" VALUES ( '{version.Version}', '{version.Description}', " +
			          $"'{version.Path}', '{version.Script}', " +
			          $"{version.HashCode}, {version.InstalledOn}, " +
			          $"{version.ExecutionTime}, '{version.Message.Replace("\'", @"""")}', {version.Success} )";
			await using var connection = _provider.Create();
			var command = new NpgsqlCommand(sql, connection);
			connection.Open();
			await command.ExecuteNonQueryAsync();
			await connection.CloseAsync();
		}

		public async Task<ICollection<SchemaVersion>> GetAllSchemaVersionsAsync()
		{
			var result = new List<SchemaVersion>();
			var sql = $"SELECT * FROM {Constants.TableName}";
			await using var connection = _provider.Create();
			await using var command = new NpgsqlCommand(sql, connection);
			connection.Open();
			await using var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				var schemaVersion = GetSchemaVersionFromRecord(reader);
				result.Add(schemaVersion);
			}
			await connection.CloseAsync();
			return result;
		}

		public async Task<SchemaVersion> TryGetSchemaVersionAsync(string path, string version)
		{
			SchemaVersion result = null;
			var sql = $"SELECT * FROM {Constants.TableName} WHERE path = '{path}' AND version = '{version}'";
			await using var connection = _provider.Create();
			await using var command = new NpgsqlCommand(sql, connection);
			connection.Open();
			await using var reader = await command.ExecuteReaderAsync();
			if (await reader.ReadAsync())
				result = GetSchemaVersionFromRecord(reader);
			
			await connection.CloseAsync();
			return result;
		}

		private SchemaVersion GetSchemaVersionFromRecord(IDataRecord record)
		{
			return new SchemaVersion()
			{
				Id = record.GetInt64(0),
				Version = record.GetString(1),
				Description = record.GetString(2),
				Path = record.GetString(3),
				Script = record.GetString(4),
				HashCode = record.GetInt32(5),
				InstalledOn = record.GetInt64(6),
				ExecutionTime = record.GetInt64(7),
				Message = record.GetString(8),
				Success = record.GetBoolean(9)
			};
		}
	}
}