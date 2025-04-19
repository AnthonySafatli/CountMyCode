using System.IO;

namespace CountMyCode
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the path to the directory you want to analyze:");
            string? initialPath = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(initialPath))
            {
                // initialPath = Environment.CurrentDirectory;
                initialPath = "C:\\Users\\Anthony\\source\\repos\\CountMyCode";
            }

            App app = new App(args, initialPath);
            await app.Run();

            Console.ReadLine();
        }
    }
}
