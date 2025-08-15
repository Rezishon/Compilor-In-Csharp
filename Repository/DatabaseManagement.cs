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
        private static readonly string onDiskDatabaseFilePath =
            $"{EnvManager.GetEnvironmentVariable(EnvironmentKeys.BaseDirectory)}Compiler-In-Csharp.db";
        private static readonly string _OnDiskConnectionString =
            $"Data Source={onDiskDatabaseFilePath}";
        private static readonly string _inMemoryConnectionString =
            "Data Source=Compiler-In-Csharp-Db;Mode=Memory;Cache=Shared;";

        DatabaseManagement()
        {
            using var inMemoryConnection = new SqliteConnection(_inMemoryConnectionString);

            inMemoryConnection.Open();
        }
    }
}
