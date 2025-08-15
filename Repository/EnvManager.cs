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
