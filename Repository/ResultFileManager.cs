#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public enum GolangLibs
    {
        fmt,
    }

    public class ResultFileManager
    {
        private static bool isFirstLib = true;
        private static List<string> resultFileData = ["package main", "", "func main() {", "}"];
        private static List<GolangLibs> usedLibs = [];

        public static void AddResultFileData(string echoInGoStr)
        {
            resultFileData.Insert(resultFileData.Count - 1, echoInGoStr);
        }

        public static void AddLibToResultFile(GolangLibs lib)
        {
            if (isFirstLib)
            {
                resultFileData.Insert(1, "");
                isFirstLib = false;
            }

            if (!usedLibs.Contains(GolangLibs.fmt))
            {
                resultFileData.Insert(2, $"import \"{lib.ToString()}\"");
                usedLibs.Add(GolangLibs.fmt);
            }
        }

        public static void RunEndJobs()
        {
            BuildResultFile();
            BuildBinaryFile();
            ExecuteBinaryFile();
        }

        private static void BuildResultFile()
        {
            try
            {
                File.WriteAllLines(
                    EnvManager.GetEnvironmentVariable(EnvironmentKeys.ResultFilePath),
                    resultFileData
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while building result file: " + ex.Message);
            }
        }

        private static void BuildBinaryFile()
        {
            try
            {
                // Start the process
                using Process process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "go",
                    Arguments =
                        $"build -o {EnvManager.GetEnvironmentVariable(EnvironmentKeys.ResultBinaryFileName)} {EnvManager.GetEnvironmentVariable(EnvironmentKeys.ResultFilePath)}",
                    CreateNoWindow = true, // No separate window
                };
                process.Start();

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private static void ExecuteBinaryFile()
        {
            try
            {
                using Process process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments =
                        $"-c \"{EnvManager.GetEnvironmentVariable(EnvironmentKeys.ResultBinaryFilePath)}\"",
                    CreateNoWindow = true, // No separate window
                };
                process.Start();

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while executing the binary: " + ex.Message);
            }
        }
    }
}
