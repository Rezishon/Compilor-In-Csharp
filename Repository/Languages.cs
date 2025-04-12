using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class Languages
    {
        public static Dictionary<string, Dictionary<string, string>> LanguagesDic { get; } = new Dictionary<string, Dictionary<string, string>>{
            {
                "Bash", new Dictionary<string, string>{
                    {"CommentRegex",@"(#\s*(\w*\W*)+)|(^#\s*(\w*\W*)+)*"},
                    {"CommentRegexException",@"(""(\s*\S*)*#(\s*\S*)*"")"},
                }
            }
        };
    }
}