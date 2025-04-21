using CountMyCode.Utils;
using System.IO;

namespace CountMyCode
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter the path to the directory you want to analyze:\n");
            string initialPath = InputUtils.GetInput(true);

            if (string.IsNullOrWhiteSpace(initialPath))
            {
                // initialPath = Environment.CurrentDirectory;
                initialPath = "C:\\Users\\Anthony\\source\\repos\\CountMyCode";
            }

            int portNumber = 5000;
            
            App app = new App(args, initialPath);
            await app.Run(portNumber);

            Console.WriteLine();
            Console.WriteLine($"Audit is running on http://localhost:{portNumber}/audit");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
