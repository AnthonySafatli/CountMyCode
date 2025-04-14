using System.IO;

namespace CountMyCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to the directory you want to analyze:");
            string? initialPath = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(initialPath))
            {
                // initialPath = Environment.CurrentDirectory;
                initialPath = "C:\\Users\\Anthony\\source\\repos\\CountMyCode";
            }

            App app = new App(initialPath);
            app.Run();

            Console.ReadLine();
        }
    }
}
