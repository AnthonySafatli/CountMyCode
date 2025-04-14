using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode
{
    internal class App
    {
        private FileItem initialFolder;

        public App(string initialPath)
        {
            initialFolder = new FileItem(initialPath, Status.Folder);
        }

        public void Run()
        {
            initialFolder.DisplayMenu();
        }
    }
}
