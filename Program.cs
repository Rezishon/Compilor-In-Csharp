using System.Threading.Tasks;
using Repository;
namespace Compilor_In_Csharp;

class Program
{
    static async Task Main(string[] args)
    {
        try
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
