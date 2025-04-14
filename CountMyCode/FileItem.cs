using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CountMyCode
{
    internal class FileItem
    {
        private bool childrenLoaded = false;
        internal FileItem? Parent { get; set; }
        internal string Path { get; set; }
        internal Status ItemType { get; set; }
        internal Status Status { get; set; }
        internal List<FileItem> Children { get; set; } = new List<FileItem>();

        internal string DisplayName
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        internal FileItem(FileItem parent, string path, Status status)
        {
            Parent = parent;
            Path = path;
            Status = status;
            ItemType = status;
        }

        internal FileItem(string path, Status status)
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

        internal void RunMenu()
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
                        selectedFileItem.RunMenu();
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
                else if (keyInfo.Key == ConsoleKey.Escape || keyInfo.Key == ConsoleKey.Enter)
                {
                    return;
                }
            }
        }
        
        private void ToggleIgnore(FileItem? item)
        {
            if (item == null)
                return;

            item.Status = item.Status == Status.Ignored ? item.ItemType : Status.Ignored;
        }

        private void RenderMenu(FileItem? selectedFileItem)
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

        private void RenderHeader()
        {
            Console.ForegroundColor = Parent == null ? ConsoleColor.DarkCyan : ConsoleColor.Cyan;
            Console.WriteLine("| " + Path);
            Console.WriteLine();
        }

        private void RenderBackToParent(FileItem? selectedFileItem)
        {
            if (Parent != null)
            {
                Console.ForegroundColor = selectedFileItem == null ? ConsoleColor.Green : ConsoleColor.White;
                Console.WriteLine("[..] Back to parent");
            }
        }

        private void RenderFileItem(FileItem fileItem, FileItem? selectedFileItem)
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

        private void RenderInstructions()
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

        internal string GetLanguageName(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException("Extension cannot be null or empty.", nameof(extension));

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write($"Please enter a name for this language: ");

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(extension);

            while (true)
            {
                string? fileName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Console.WriteLine("No name entered. Try again.");
                    Console.WriteLine();
                }
                else
                {
                    return fileName;
                }
            }
        }

        internal List<string> GetExtensions()
        {
            List<string> extensions = new List<string>();
            foreach (FileItem item in Children)
            {
                if (item.ItemType == Status.File)
                {
                    string extension = System.IO.Path.GetExtension(item.Path);
                    if (!extensions.Contains(extension) && !string.IsNullOrWhiteSpace(extension))
                        extensions.Add(extension);
                }
                else if (item.ItemType == Status.Folder)
                {
                    extensions.AddRange(item.GetExtensions());
                }
            }
            return extensions;
        }

        internal void AddExtensions(Dictionary<string, string> programmingExtensions)
        {
            List<string> extensions = GetExtensions();

            foreach (string extension in extensions)
            {
                if (!programmingExtensions.ContainsKey(extension))
                {
                    string fileName = GetLanguageName(extension);
                    programmingExtensions.Add(extension, fileName);
                }
            }
        }
    }
}
