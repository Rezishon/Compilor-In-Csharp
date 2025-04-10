using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compilor_In_Csharp.Repository
{
    public class FileHandler
    {
        public static async Task<string[]> ReadFile(string filePath)
        {
            return await File.ReadAllLinesAsync(filePath);
        }
    }
}