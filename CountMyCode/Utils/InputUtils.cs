using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Utils;

public static class InputUtils
{
    public static string GetInput(bool acceptNull = false)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.Write("> ");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string? input = Console.ReadLine();

            if (!acceptNull && string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input cannot be empty. Please try again.");
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.Write("> ");
                continue;
            }

            return input ?? string.Empty;
        }
    }
}
