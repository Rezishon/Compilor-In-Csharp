#undef DEBUG
using System.Threading.Tasks;
using Irony.Parsing;
using Laxer;
using Repository;

namespace Compiler_In_Csharp;

class Program
{
    static async Task Main(string[] args)
    {
        EnvManager.SetEnvs();
        string[] bashFileLines = await File.ReadAllLinesAsync(@"./sample.bash");
#if DEBUG
                    Console.WriteLine("File Content:");
                    foreach (var item in bashFileLines)
                    {
                        Console.WriteLine("  " + item);
                    }
                    Console.WriteLine("-------------------");
#endif

        var grammar = new BashGrammar();
        var languageData = new LanguageData(grammar);
        var parser = new Parser(languageData);
        // // Test cases
        // string[] testScripts = new string[]
        // {
        //     "echo hello rezi",
        //     // "a=20",
        //     // "echo 123",
        //     // "myvar=\"some value\"",
        //     // "echo $myvar", // Note: $var expansion is *not* handled by this simple grammar yet!
        //     // // It will parse '$myvar' as a single Identifier for now.
        //     // // Handling '$' for variable expansion requires more complex rules.
        //     // "echo 'line 1'; a=10; echo \"line 2\"",myvar
        //     // "var=value\necho $var", // Newline as separator
        // };

        foreach (var script in bashFileLines)
        {
            ParseTree parseTree = parser.Parse(script);
            {
            }
        }
        {

        }

    }
}
