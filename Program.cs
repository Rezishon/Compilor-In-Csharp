#undef DEBUG
using System.Threading.Tasks;
using Compiler_In_Csharp.Repository;
using Irony.Parsing;
using Laxer;
using Repository;

namespace Compiler_In_Csharp;

class Program
{
    static async Task Main(string[] args)
    {
        EnvManager.SetEnvs();
        string[] bashFileLines = await File.ReadAllLinesAsync(@"./sample.bsh");
        DatabaseManagement.ConnectToDatabase();
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
        var lineNumber = 0;

        foreach (var bashLine in bashFileLines)
        {
            lineNumber++;
            ParseTree parseTree = parser.Parse(bashLine);

            #region Error in parsing
            if (parseTree.Status == ParseTreeStatus.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Parsing Failed! {");
                foreach (var message in parseTree.ParserMessages)
                {
                    Console.WriteLine(
                        $"Error: {message.Message} at {message.Location} of line {lineNumber}\n}}"
                    );
                }
                Console.ResetColor();

                return;
            }
#if DEBUG
            Console.WriteLine($"\n--- Parsing Script: ---\n{script}\n-----------------------");

            if (parseTree.Status == ParseTreeStatus.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Parsing Failed!");
                foreach (var message in parseTree.ParserMessages)
                {
                    Console.WriteLine($"Error: {message.Message} at {message.Location}");
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Parsing Successful!");
                Console.ResetColor();
                // You can inspect the parseTree.Root to see the AST
                // For a visual representation, use Irony's Grammar Explorer (recommended for development!)
                Console.WriteLine("Parse Tree Root: " + parseTree.Root.Term.Name);
                // Optionally print a simple representation of the tree:
                PrintParseTreeNode(parseTree.Root, 0);
            }
#endif
            #endregion

            foreach (var item in parseTree.Tokens)
            {
                Console.WriteLine(item.Terminal);
            }
            switch (parseTree.Tokens[0].Terminal.ToString())
            {
                case "echo":
                    ResultFileManager.AddLibToResultFile(GolangLibs.fmt);

                    string echoInGoStr = ProcessEchoCommand(parseTree);
                    ResultFileManager.AddResultFileData(echoInGoStr);
                    break;
                case "Identifier":
                    //TODO: if user add a Identifier and didn't used it, it will get error
                    ResultFileManager.AddResultFileData(bashLine.Replace("$", "").Replace("=", ":="));
                    break;
            }
        }
        ResultFileManager.RunEndJobs();
    }

    private static string ProcessEchoCommand(ParseTree parseTree)
    {
        // Skip the 'echo' token and process remaining tokens
        var arguments = parseTree.Tokens.Skip(1);

        var paramType = arguments.ToList()[0].Terminal.Name;

        if (paramType == "Number")
        {
            var processedArgs = new List<string>();

            foreach (var token in arguments)
            {
                string text = token.Text;

                processedArgs.Add(text);
            }

            string output = string.Join("", processedArgs);

            return $"fmt.Println({output})";
        }
        else if (paramType == "DoubleQuotedString")
        {
            var processedArgs = new List<string>();

            foreach (var token in arguments)
            {
                string text = token.Text;

                // If it's a quoted string, keep the content but remove the quotes
                if (text.StartsWith("\"") || text.StartsWith("'") || text.StartsWith("`"))
                {
                    text = text.Trim('"', '\'', '`');
                }
                processedArgs.Add(text);
            }

            string output = string.Join("", processedArgs);

            return $"fmt.Println(\"{output}\")";
        }
        else if (paramType == "Variable")
        {
            var processedArgs = new List<string>();

            foreach (var token in arguments)
            {
                string text = token.Text;
                processedArgs.Add(text.Replace("$", ""));
            }

            string output = string.Join("", processedArgs);
            // return $"fmt.Println({arguments.ToList()[0].Value.ToString()!.Replace("$", "")})";
            return $"fmt.Println({output})";
        }
        return "fmt.Println()";
    }

    private static void PrintParseTreeNode(ParseTreeNode node, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 2);
        Console.WriteLine(
            $"{indent}{node.Term.Name} ({node.Span.Location.Line}:{node.Span.Location.Column}): {node.Token?.Text}"
        );
    }

    private static bool ProcessCondition(
        double firstExpression,
        string compareTypes,
        double secondExpression
    )
    {
        switch (compareTypes)
        {
            case "-eq":
                return firstExpression == secondExpression;
            case "-ne":
                return firstExpression != secondExpression;
            case "-gt":
                return firstExpression > secondExpression;
            case "-ge":
                return firstExpression >= secondExpression;
            case "-lt":
                return firstExpression < secondExpression;
            case "-le":
                return firstExpression <= secondExpression;
            default:
                return false;
        }
    }
}
