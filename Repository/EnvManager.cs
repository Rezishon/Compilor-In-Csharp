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

            Environment.SetEnvironmentVariable("BaseDirectory", baseDirectory);
            Environment.SetEnvironmentVariable("GolangFilePath", $"{baseDirectory}GolangFile.go");
            Environment.SetEnvironmentVariable(
                "GolangBinaryFilePath",
                $"{baseDirectory}golang_binary_file"
            );
        }
    }
}
