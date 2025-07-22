using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class ResultFileManager
    {
        private static List<string> resultFileData =
        [
            "package main",
            "",
            "import \"fmt\"",
            "",
            "func main() {",
            "}",
        ];

        public static void AddResultFileData(string echoInGoStr)
        {
            resultFileData.Insert(resultFileData.Count - 1, echoInGoStr);
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
    }
}
