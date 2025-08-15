using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Repository;

namespace Compiler_In_Csharp.Repository
{
    public class DatabaseManagement
    {
        #region Database row types
        public record VariableTypeRow(int Id, string Name);
        #endregion

        private enum VariableTypes
        {
            String,
            Number,
        }

        #region Connection strings
        private static readonly string onDiskDatabaseFilePath =
            $"{EnvManager.GetEnvironmentVariable(EnvironmentKeys.BaseDirectory)}Compiler-In-Csharp.db";
        private static readonly string _OnDiskConnectionString =
            $"Data Source={onDiskDatabaseFilePath}";
        private static readonly string _inMemoryConnectionString =
            "Data Source=Compiler-In-Csharp-Db;Mode=Memory;Cache=Shared;";
        #endregion

        DatabaseManagement()
        {
            using var inMemoryConnection = new SqliteConnection(_inMemoryConnectionString);
            inMemoryConnection.Open();
        }

        ~DatabaseManagement()
        {
            using var inMemoryConnection = new SqliteConnection(_inMemoryConnectionString);
            inMemoryConnection.Close();
        }

        public static async void ConnectToDatabase()
        {
            try
            {
                using var inMemoryConnection = new SqliteConnection(_inMemoryConnectionString);
                await inMemoryConnection.OpenAsync();
                Console.WriteLine("Connected to SQLite in memory database asynchronously!");
                await CreatePreLoadTables(inMemoryConnection);
                await InsertPreLoadData(inMemoryConnection);

                await ReadFrom_VariableTypesTable_Async(inMemoryConnection);
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SQLite Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        #region CreateTable

        private static async Task CreateEnvironmentVariablesTable(SqliteConnection connection)
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                var _command = connection.CreateCommand();
                _command.CommandText = EnvManager.GetEnvironmentVariable(
                    QueryStrings.CreateEnvironmentVariablesTable
                );
                await _command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private static async Task CreateVariablesTable(SqliteConnection connection)
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                var _command = connection.CreateCommand();
                _command.CommandText = EnvManager.GetEnvironmentVariable(
                    QueryStrings.CreateVariablesTable
                );
                await _command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private static async Task CreateVariableTypesTable(SqliteConnection connection)
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                var _command = connection.CreateCommand();
                _command.CommandText = EnvManager.GetEnvironmentVariable(
                    QueryStrings.CreateVariableTypesTable
                );
                await _command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (SqliteException sql)
            {
                transaction.Rollback();
                throw new SqliteException(sql.Message, 0);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private static async Task CreateKeywordsTable(SqliteConnection connection)
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                var _command = connection.CreateCommand();
                _command.CommandText = EnvManager.GetEnvironmentVariable(
                    QueryStrings.CreateKeywordsTable
                );
                await _command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private static async Task CreateKeywordTypesTable(SqliteConnection connection)
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                var _command = connection.CreateCommand();
                _command.CommandText = EnvManager.GetEnvironmentVariable(
                    QueryStrings.CreateKeywordTypesTable
                );
                await _command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Pre load methods
        private async static Task CreatePreLoadTables(SqliteConnection connection)
        {
            await CreateVariableTypesTable(connection);
            await CreateVariablesTable(connection);
        }

        private static async Task InsertPreLoadData(SqliteConnection connection)
        {
            await InsertTo_VariableTypesTable_Async(VariableTypes.String, connection);
            await InsertTo_VariableTypesTable_Async(VariableTypes.Number, connection);
        }
        #endregion

        #region Insert methods
        private static async Task InsertTo_VariableTypesTable_Async(
            VariableTypes typeName,
            SqliteConnection connection
        )
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    @$"
            INSERT INTO VariableTypesTable (Name)
            VALUES ('{typeName.ToString()}');";
                // command.Parameters.AddWithValue("$name", name);
                // command.Parameters.AddWithValue("$age", age);
                await command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (SqliteException sql)
            {
                transaction.Rollback();
                throw new SqliteException(sql.Message, 0);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Read methods
        static async Task<List<VariableTypeRow>> ReadFrom_VariableTypesTable_Async(
            SqliteConnection connection
        )
        {
            var rows = new List<VariableTypeRow>();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM VariableTypesTable;";

                using var reader = await command.ExecuteReaderAsync(); // Asynchronous reader
                if (!reader.HasRows)
                {
                    Console.WriteLine("No data found asynchronously.");
                    return [];
                }

                while (await reader.ReadAsync()) // Asynchronous read
                {
                    var id = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    rows.Add(new VariableTypeRow(id, name));
                }
                foreach (var item in rows)
                {
                    Console.WriteLine(item.Name);
                }
                return rows;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SqliteException: {ex.Message}");
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return [];
            }
        }
        #endregion
    }
}
