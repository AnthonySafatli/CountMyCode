using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode
{
    internal class App
    {
        readonly string _initialPath;

        public App(string initalPath)
        {
            _initialPath = initalPath;
        }

        public void Run()
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
