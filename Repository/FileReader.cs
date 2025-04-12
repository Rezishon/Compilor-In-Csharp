using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public static class FileHandler
    {
        public static async Task<List<string>> ReadFile(string filePath)
        {
            return CommentRemover(await File.ReadAllLinesAsync(filePath));
        }

        public static List<string> CommentRemover(string[] strings)
        {
        }
    }
}