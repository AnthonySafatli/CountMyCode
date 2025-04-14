using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CountMyCode
{
    internal class FileItem
    {
        private bool childrenLoaded = false;
        public FileItem? Parent { get; set; }
        public string Path { get; set; }
        public Status ItemType { get; set; }
        public Status Status { get; set; }
        public List<FileItem> Children { get; set; } = new List<FileItem>();

        public string DisplayName
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        public FileItem(FileItem parent, string path, Status status)
        {
            Parent = parent;
            Path = path;
            Status = status;
            ItemType = status;
        }

        public FileItem(string path, Status status)
            : this(null, path, status) { }

        private void LoadChildren()
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

            childrenLoaded = true;
        }

        public void Menu()
        {
            Console.Clear();

            if (!childrenLoaded)
                LoadChildren();

            FileItem? selectedFileItem = Children.Count > 0 ? Children[0] : null;

            while (true)
            {
                RenderMenu(selectedFileItem);
                
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); 

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (selectedFileItem == null)
                        continue;

                    int index = Children.IndexOf(selectedFileItem);
                    if (index > 0)
                        selectedFileItem = Children[index - 1];
                    else
                        selectedFileItem = Children[Children.Count - 1];
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (selectedFileItem == null)
                        continue;

                    int index = Children.IndexOf(selectedFileItem);
                    if (index < Children.Count - 1)
                        selectedFileItem = Children[index + 1];
                    else
                        selectedFileItem = Children[0];
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (selectedFileItem?.ItemType == Status.Folder)
                        selectedFileItem.Menu();
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (Parent != null)
                        return;
                }
                else if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    if (selectedFileItem == null)
                        continue;

                    ToggleIgnore(selectedFileItem);
                }
            }
        }
        
        public void ToggleIgnore(FileItem? item)
        {
            if (item == null)
                return;

            item.Status = item.Status == Status.Ignored ? item.ItemType : Status.Ignored;
        }

        public void RenderMenu(FileItem? selectedFileItem)
        {
            Console.Clear();

            RenderHeader();

            foreach (FileItem item in Children)
            {
                RenderFileItem(item, selectedFileItem);
            }

            if (Children.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("No items found.");
            }

            RenderInstructions();
        }

        public void RenderHeader()
        {
            Console.ForegroundColor = Parent == null ? ConsoleColor.DarkCyan : ConsoleColor.Cyan;
            Console.WriteLine("| " + Path);
            Console.WriteLine();
        }

        public void RenderBackToParent(FileItem? selectedFileItem)
        {
            if (Parent != null)
            {
                Console.ForegroundColor = selectedFileItem == null ? ConsoleColor.Green : ConsoleColor.White;
                Console.WriteLine("[..] Back to parent");
            }
        }

        public void RenderFileItem(FileItem fileItem, FileItem? selectedFileItem)
        {
            if (fileItem.Status == Status.Ignored)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = fileItem == selectedFileItem ? ConsoleColor.Green : ConsoleColor.White;

            Console.Write($"[{fileItem.Status}] ");

            if (fileItem == selectedFileItem)
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(fileItem.DisplayName);
        }

        public void RenderInstructions()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();

            if (Children.Count > 1)
                Console.WriteLine("UP / DOWN | Navigate up or down");
            if (Children.Any(x => x.ItemType == Status.Folder))
                Console.WriteLine("RIGHT     | Enter a folder to view its contents");
            if (Parent != null)
                Console.WriteLine("LEFT      | Go back to the folders parents");
            Console.WriteLine("SPACE     | Toggle if an item is ignored by the audit");
        }
    }
}
