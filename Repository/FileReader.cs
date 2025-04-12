using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            List<string> stringsList = new List<string>();

            foreach (var item in strings)
            {
                if (Regex.IsMatch(item, Languages.LanguagesDic["Bash"]["CommentRegex"]))
                {
                    if (!Regex.IsMatch(item, Languages.LanguagesDic["Bash"]["CommentRegexException"]))
                    {
                        strings[Array.IndexOf(strings, item)] = Regex.Replace(item, Languages.LanguagesDic["Bash"]["CommentRegex"], "");
                    }
                }
            }

            foreach (var item in strings)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    stringsList.Add(item.Trim());
                }
            }

            return stringsList;
        }
    }
}