using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public enum EnvironmentKeys
    {
        BaseDirectory,
        ResultFileName,
        ResultFilePath,
        ResultBinaryFileName,
        ResultBinaryFilePath,
    }

    public enum QueryStrings
    {
        CreateEnvironmentVariablesTable,
        CreateVariablesTable,
        CreateVariableTypesTable,
        CreateKeywordsTable,
        CreateKeywordTypesTable,
    }

    public static class EnvManager
    {
        public static void SetEnvs()
        {
            #region Environment Keys

            SetEnvironmentVariable(EnvironmentKeys.BaseDirectory, AppContext.BaseDirectory);
            SetEnvironmentVariable(EnvironmentKeys.ResultFileName, "GolangFile.go");
            SetEnvironmentVariable(
                EnvironmentKeys.ResultFilePath,
                $"{GetEnvironmentVariable(EnvironmentKeys.BaseDirectory)}{GetEnvironmentVariable(EnvironmentKeys.ResultFileName)}"
            );
            SetEnvironmentVariable(EnvironmentKeys.ResultBinaryFileName, $"golang_binary_file");
            SetEnvironmentVariable(
                EnvironmentKeys.ResultBinaryFilePath,
                $"{GetEnvironmentVariable(EnvironmentKeys.BaseDirectory)}{GetEnvironmentVariable(EnvironmentKeys.ResultBinaryFileName)}"
            );
            #endregion
            SetEnvironmentVariable(
                QueryStrings.CreateEnvironmentVariablesTable,
                @"
            CREATE TABLE IF NOT EXISTS EnvironmentVariablesTable (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Value TEXT 
            );"
            );

            SetEnvironmentVariable(
                QueryStrings.CreateVariablesTable,
                @"
            CREATE TABLE IF NOT EXISTS VariablesTable (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Type INTEGER NOT NULL,
                Value TEXT,
                ScopeIndex INTEGER NOT NULL,
                FOREIGN KEY (Type) REFERENCES VariableTypesTable(Id)
            );"
            );

            SetEnvironmentVariable(
                QueryStrings.CreateVariableTypesTable,
                @"
            CREATE TABLE IF NOT EXISTS VariableTypesTable (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                SampleValue TEXT
            );"
            );

            SetEnvironmentVariable(
                QueryStrings.CreateKeywordsTable,
                @"
            CREATE TABLE IF NOT EXISTS KeywordsTable (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Type INTEGER NOT NULL,
                Name TEXT NOT NULL,
                FOREIGN KEY (Type) REFERENCES KeywordTypesTable(Id)
            );"
            );

            SetEnvironmentVariable(
                QueryStrings.CreateKeywordTypesTable,
                @"
            CREATE TABLE IF NOT EXISTS KeywordTypesTable (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
            );"
            );
        }

        public static string GetEnvironmentVariable(EnvironmentKeys key)
        {
            return Environment.GetEnvironmentVariable(key.ToString())
                ?? throw new InvalidOperationException($"Environment variable {key} not found");
        }

        private static void SetEnvironmentVariable(EnvironmentKeys key, string value)
        {
            Environment.SetEnvironmentVariable(key.ToString(), value);
        }
    }
}
