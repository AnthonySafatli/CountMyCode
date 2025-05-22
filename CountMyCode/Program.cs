using CountMyCode.Utils;
using System.IO;

namespace CountMyCode
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int portNumber = 5000;

            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Enter the path to the directory you want to analyze:\n");
                    string initialPath = InputUtils.GetInput(true);

                    if (string.IsNullOrWhiteSpace(initialPath))
                    {
                        // initialPath = Environment.CurrentDirectory;
                        initialPath = "C:\\Users\\Anthony\\source\\repos\\CountMyCode";
                    }
            
                    App app = new App(args, initialPath);
                    bool success = await app.Run(portNumber);
                    if (!success)
                        continue;

                    break;
                }

                Console.WriteLine();
                Console.WriteLine($"Audit is running on http://localhost:{portNumber}/audit");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
            }
            catch (IOException e)
            {
                Console.WriteLine("FATAL: An error occurred while accessing the file system!\n");
                Console.WriteLine(e.Message);
            }
        }
    }
}
