using CountMyCode.Utils;
using System.IO;

namespace CountMyCode;

internal class Program
{
    static async Task Main(string[] args)
    {
        bool takeInput = args.Length == 0;

        int portNumber = 5000;
        while (SocketUtils.IsPortInUse(portNumber)) portNumber++;

        try
        {
            while (true)
            {
                string initialPath = takeInput ? "" : args[0];
                if (takeInput)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Enter the path to the directory you want to analyze");
                    Console.Write("Leave blank to analyze ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(Environment.CurrentDirectory);
                    initialPath = InputUtils.GetInput(true);
                    takeInput = false;
                }

                if (string.IsNullOrWhiteSpace(initialPath))
                    initialPath = Environment.CurrentDirectory;
        
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
            Console.Clear();
            Console.WriteLine("FATAL: An error occurred while accessing the file system!\n");
            Console.WriteLine(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            Console.Clear();
            Console.WriteLine("FATAL: An error occurred while accessing the file system!\n");
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            Console.Clear();
            Console.WriteLine("FATAL: An unexpected error occurred!\n");
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.ResetColor();
        }
    }
}
