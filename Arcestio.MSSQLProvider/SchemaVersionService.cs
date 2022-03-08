using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Arcestio.Core;
using Arcestio.Core.Entities;
using Arcestio.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace Arcestio.MSSQLProvider
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
			var sql = $"IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='{Constants.TableName}' AND XTYPE='U') " +
							"BEGIN " +
							$"CREATE TABLE [dbo].[{Constants.TableName}]" +
							"( " +
							"id INT IDENTITY(1,1) PRIMARY KEY, " +
							"version VARCHAR(255) NOT NULL, " +
							"description VARCHAR(255) NOT NULL, " +
							"path VARCHAR(255) NOT NULL, " +
							"script VARCHAR(255) NOT NULL, " +
							"hashcode INT NOT NULL, " +
							"installedon BIGINT NOT NULL, " +
							"executiontime BIGINT NOT NULL, " +
							"message VARCHAR(2000), " +
							"success BIT NOT NULL" +
							") " +
							"END";

			await using var connection = _provider.Create();
			await connection.OpenAsync();
			var command = new SqlCommand(sql, connection);
			await command.ExecuteNonQueryAsync();
			await connection.CloseAsync();
		}

		public async Task AddNewRowAsync(SchemaVersion version)
		{
			var sql = $"INSERT INTO [dbo].[{Constants.TableName}] " +
							$"({nameof(version.Version)}, {nameof(version.Description)}, " +
							$"{nameof(version.Path)}, {nameof(version.Script)}, " +
							$"{nameof(version.HashCode)}, {nameof(version.InstalledOn)}, " +
							$"{nameof(version.ExecutionTime)}, {nameof(version.Message)}, {nameof(version.Success)}) " +
							$" VALUES ( '{version.Version}', '{version.Description}', " +
							$"'{version.Path}', '{version.Script}', " +
							$"{version.HashCode}, {version.InstalledOn}, " +
							$"{version.ExecutionTime}, '{version.Message.Replace("\'", @"""")}', {(version.Success ? 1 : 0)} )";
			await using var connection = _provider.Create();
			await connection.OpenAsync();
			var command = new SqlCommand(sql, connection);
			await command.ExecuteNonQueryAsync();
			await connection.CloseAsync();
		}

		public async Task<ICollection<SchemaVersion>> GetAllSchemaVersionsAsync()
		{
			var result = new List<SchemaVersion>();
			var sql = $"SELECT * FROM [dbo].[{Constants.TableName}]";
			await using var connection = _provider.Create();
			var command = new SqlCommand(sql, connection);
			await connection.OpenAsync();
			await using var reader = await command.ExecuteReaderAsync();
			if (reader.HasRows)
			{
				while (await reader.ReadAsync())
				{
					var schemaVersion = GetSchemaVersionFromRecord(reader);
					result.Add(schemaVersion);
				}
			}

			await connection.CloseAsync();
			return result;
		}

		public async Task<SchemaVersion> TryGetSchemaVersionAsync(string path, string version)
		{
			SchemaVersion result = null;
			var sql = $"SELECT * FROM [dbo].[{Constants.TableName}] WHERE path = '{path}' AND version = '{version}'";
			await using var connection = _provider.Create();
			var command = new SqlCommand(sql, connection);
			await connection.OpenAsync();
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
				Id = record.GetInt32(0),
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