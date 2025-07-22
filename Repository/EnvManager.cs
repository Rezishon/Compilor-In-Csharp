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

    public static class EnvManager
    {
        public static void SetEnvs()
        {
            string baseDirectory = AppContext.BaseDirectory;

            SetEnvironmentVariable(EnvironmentKeys.BaseDirectory, baseDirectory);
            SetEnvironmentVariable(EnvironmentKeys.ResultFileName, "GolangFile.go");
            SetEnvironmentVariable(
                EnvironmentKeys.ResultFilePath,
                $"{baseDirectory}{GetEnvironmentVariable(EnvironmentKeys.ResultFileName)}"
            );
            SetEnvironmentVariable(EnvironmentKeys.ResultBinaryFileName, $"golang_binary_file");
            SetEnvironmentVariable(
                EnvironmentKeys.ResultBinaryFilePath,
                $"{baseDirectory}{GetEnvironmentVariable(EnvironmentKeys.ResultBinaryFileName)}"
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
