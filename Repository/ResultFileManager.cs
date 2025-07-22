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
    }
}
