using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CountMyCode
{
    internal class FileItem
    {
        public FileItem? Parent { get; set; }
        public string Path { get; set; }
        public Status Status { get; set; }
        public List<FileItem> Children { get; set; } = new List<FileItem>();

        public FileItem(FileItem parent, string path, Status status)
        {
            Parent = parent;
            Path = path;
            Status = status;
        }

        public FileItem(string path, Status status)
            : this(null, path, status) 
        {
            string[] files = Directory.GetFiles(Path, "*", SearchOption.TopDirectoryOnly);
            string[] directories = Directory.GetDirectories(Path, "*", SearchOption.TopDirectoryOnly);

            foreach (string directory in directories)
            {
                Children.Add(new FileItem(this, directory, Status.Folder));
            }

            foreach (string file in files)
            {
                Children.Add(new FileItem(this, file, Status.File));
            }
        }

        public void DisplayMenu()
        {
            Console.Clear();

            FileItem? selectedFileItem = Children.Count > 0 ? Children[0] : null;

            if (selectedFileItem == null)
                throw new ArgumentNullException("The folder is empty?");

            RenderFileItems(selectedFileItem);

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); 

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    int index = Children.IndexOf(selectedFileItem);
                    if (index > 0)
                        selectedFileItem = Children[index - 1];

                    RenderFileItems(selectedFileItem);
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    int index = Children.IndexOf(selectedFileItem);
                    if (index < Children.Count - 1)
                        selectedFileItem = Children[index + 1];

                    RenderFileItems(selectedFileItem);
                }
            }
        }

        public void RenderFileItems(FileItem selectedFileItem)
        {
            Console.Clear();

            foreach (FileItem item in Children)
            {
                RenderFileItem(item, selectedFileItem);
            }
        }

        public void RenderFileItem(FileItem fileItem, FileItem selectedFileItem)
        {
            if (fileItem == selectedFileItem)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(System.IO.Path.GetFileName(fileItem.Path));
        }
    }
}
