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
        {
            List<string> strings = await FileHandler.ReadFile(@"/home/rezishon/storage/Project/Compilor-In-Csharp/sample.bash");
            foreach (var item in strings)
            {
                System.Console.WriteLine(item);
            }
        }
        catch (Exception)
        {

            throw;
        }

    }
}
