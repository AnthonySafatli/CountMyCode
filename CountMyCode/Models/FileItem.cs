using CountMyCode.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace CountMyCode.Models
{
    internal class FileItem
    {
        private bool childrenLoaded = false;
        internal FileItem? Parent { get; set; }
        internal string Path { get; set; }
        internal Status ItemType { get; set; }
        internal Status Status { get; set; }
        internal List<FileItem> Children { get; set; } = new List<FileItem>();
        internal List<bool> Options { get; set; } = new List<bool>();

        internal string DisplayName
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        internal bool IsIgnored
        {
            get
            {
                if (Status == Status.Ignored)
                    return true;

                FileItem? lineageFileItem = Parent;
                while (lineageFileItem != null)
                {
                    if (lineageFileItem.Status == Status.Ignored)
                        return true;

                    lineageFileItem = lineageFileItem.Parent;
                }

                return false;
            }
        }

        internal FileItem(FileItem? parent, string path, Status status)
        {
            Parent = parent;
            Path = path;
            Status = status;
            ItemType = status;
        }

        internal FileItem(string path, Status status)
            : this(null, path, status) 
        {
            // Auto ignore non text files
            Options.Add(true);

            if (Options.Count != Enum.GetValues(typeof(OptionIndexes)).Length)
                throw new ApplicationException("You need to initialize all options");
        }

        private void LoadChildren()
        {
            // TODO: Check if the path is a directory

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

        /// <summary>
        /// return true => run the audit 
        /// returns null => restart the program
        /// </summary>
        internal bool? RunMenu()
        {
            Console.Clear();

            if (!childrenLoaded)
                LoadChildren();

            FileItem? selectedFileItem = Children.Count > 0 ? Children[0] : null;

            bool isInOptionsMenu = false;
            int selectedOptionIndex = 0;

            while (true)
            {
                RenderMenu(isInOptionsMenu, selectedFileItem, selectedOptionIndex);
                
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
                    {
                        bool? menuResult = selectedFileItem.RunMenu();
                        if (menuResult == true || menuResult == null)
                            return menuResult; 
                    }
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (Parent != null)
                        return false;
                }
                else if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    if (isInOptionsMenu)
                    {
                        Options[selectedOptionIndex] = !Options[selectedOptionIndex];
                        continue;
                    }

                    if (selectedFileItem == null)
                        continue;

                    ToggleIgnore(selectedFileItem);
                }
                else if (keyInfo.Key == ConsoleKey.Tab)
                {
                    if (Options.Count > 0)
                        isInOptionsMenu = !isInOptionsMenu;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return true;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return null;
                }
            }
        }
        
        private void ToggleIgnore(FileItem? item)
        {
            if (item == null)
                return;

            item.Status = item.Status == Status.Ignored ? item.ItemType : Status.Ignored;
        }

        private void RenderMenu(bool isInOptionsMenu, FileItem? selectedFileItem, int selectedOptionIndex)
        {
            Console.Clear();

            RenderHeader();

            RenderFileItems(isInOptionsMenu, selectedFileItem);

            RenderOptions(isInOptionsMenu, selectedOptionIndex);

            RenderInstructions(isInOptionsMenu);
        }


        private void RenderHeader()
        {
            var foregroundColour = Parent == null ? ConsoleColor.DarkCyan : ConsoleColor.Cyan;

            Console.ForegroundColor = foregroundColour;
            Console.Write("| ");

            if (IsIgnored)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[Ignored] ");
            }

            Console.ForegroundColor = foregroundColour;
            Console.WriteLine(Path);
            Console.WriteLine();
        }
        private void RenderFileItems(bool isInOptionsMenu, FileItem? selectedFileItem)
        {
            foreach (FileItem item in Children)
            {
                RenderFileItem(item, isInOptionsMenu, selectedFileItem);
            }

            if (Children.Count == 0)
            {
                Console.ForegroundColor = isInOptionsMenu ? ConsoleColor.DarkGray : ConsoleColor.White;
                Console.WriteLine("No items found.");
            }
        }

        private void RenderFileItem(FileItem fileItem, bool isInOptionsMenu, FileItem? selectedFileItem)
        {
            int statusPadding = 0;
            if (fileItem.Status == Status.Folder)
                statusPadding = 1;
            else if (fileItem.Status == Status.File)
                statusPadding = 3;

            RenderInputLine(!isInOptionsMenu, fileItem.Status.ToString(), fileItem.Status == Status.Ignored, statusPadding, fileItem.DisplayName, fileItem == selectedFileItem);
        }

        private void RenderOptions(bool isInOptionsMenu, int selectedOptionIndex)
        {
            if (Options.Count == 0)
                return;

            Console.WriteLine();
            Console.ForegroundColor = isInOptionsMenu ? ConsoleColor.White : ConsoleColor.DarkGray;

            for (int i = 0; i < Options.Count; i++)
            {
                string optionText = Options[i] ? "ON" : "OFF";
                string optionDescription = "";

                switch (i)
                {
                    case (int)OptionIndexes.IgnoreNonTextFiles:
                        optionDescription = "Ignore non-text files";
                        break;
                    default:
                        throw new ApplicationException("Options list and enum are out of sync");
                }

                RenderInputLine(isInOptionsMenu, optionText, !Options[i], Options[i] ? 1 : 0, optionDescription, i == selectedOptionIndex);
            }
        }

        private void RenderInputLine(bool isActive, string status, bool isStatusBad, int statusPadding, string itemName, bool isSelected)
        {
            ConsoleColor foregroundColour = ConsoleColor.DarkGray;

            if (isActive)
            {
                if (isStatusBad)
                    foregroundColour = ConsoleColor.Red;
                else
                    foregroundColour = isSelected ? ConsoleColor.Green : ConsoleColor.White;
            }

            Console.ForegroundColor = foregroundColour;

            Console.Write($"[{status}] ");
            Console.Write(new string(' ', statusPadding));

            if (isSelected && isActive)
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(itemName);
        }

        private void RenderInstructions(bool isInOptionsMenu)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();

            if ((!isInOptionsMenu && Children.Count > 1) || (isInOptionsMenu && Options.Count > 1))
                Console.WriteLine("UP / DOWN   | Navigate up or down");
            if (!isInOptionsMenu && Children.Any(x => x.ItemType == Status.Folder))
                Console.WriteLine("RIGHT       | Enter a folder to view its contents");
            if (Parent != null)
                Console.WriteLine("LEFT        | Go back to the folders parents");
            if ((!isInOptionsMenu && Children.Count > 0) || (isInOptionsMenu && Options.Count > 0))
                Console.WriteLine("SPACE       | " + (isInOptionsMenu ? "Toggle an option" : "Toggle if an item is ignored by the audit")); 
            if (Options.Count > 0)
                Console.WriteLine($"TAB         | {(isInOptionsMenu ? "Exit" : "Enter")} options menu");
            
            Console.WriteLine("ENTER      | Run the audit");
            Console.WriteLine("ESC        | Choose another folder");
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

            return InputUtils.GetInput();
        }

        internal List<string> GetExtensions()
        {
            List<string> extensions = new List<string>();
            foreach (FileItem item in Children)
            {
                if (item.Status == Status.Ignored)
                    continue;

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

        internal List<FileItem> GetFiles(bool ignoreNonTextFiles)
        {
            if (!childrenLoaded)
                LoadChildren();

            List<FileItem> files = new List<FileItem>();

            foreach (FileItem item in Children)
            {
                if (item.Status == Status.Ignored)
                    continue;

                if (item.ItemType == Status.File)
                {
                    if (ignoreNonTextFiles)
                    {
                        if (FileUtils.IsBinary(item.Path))
                        {
                            item.Status = Status.Ignored;
                            continue;
                        }
                    }

                    files.Add(item);
                }
                else if (item.ItemType == Status.Folder)
                {
                    files.AddRange(item.GetFiles(ignoreNonTextFiles));
                }
            }

            return files;
        }

        internal async Task<AuditStats> ProcessFileAsync(string path)
        {
            AuditStats audit = new AuditStats();

            await foreach (var line in FileUtils.ReadLinesAsync(path))
            {
                audit.LinesOfCode++;
                audit.Characters += line.Length;

                if (line.Contains("TODO", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("FIXEME", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("HACK", StringComparison.OrdinalIgnoreCase))
                {
                    audit.Todos++;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    audit.EmptyLinesVs++;
                }

                audit.WhiteSpaceVs += line.Count(c => string.IsNullOrWhiteSpace(c.ToString()));
            }

            return audit;
        }

        internal async Task<AuditStats> RunAudit()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Running the audit. Counting your code. Please wait...");

            List<FileItem> files = GetFiles(Options[(int)OptionIndexes.IgnoreNonTextFiles]);
            
            List<Task<AuditStats>> auditTasks = new List<Task<AuditStats>>();
            List<FileInfo> fileInfos = new List<FileInfo>();

            foreach (FileItem file in files)
            {
                auditTasks.Add(ProcessFileAsync(file.Path));
                fileInfos.Add(new FileInfo(file.Path));
            }

            AuditStats finalAudit = new AuditStats();

            AuditStats[] auditResults = await Task.WhenAll(auditTasks);
            for (int i = 0; i < fileInfos.Count; i++)
            {
                FileItem fileItem = files[i];
                AuditStats audit = auditResults[i];
                FileInfo fileInfo = fileInfos[i];

                double fileSize = fileInfo.Length / 1024.0; // Convert to KB

                // Get file related items

                finalAudit.Files++;
                finalAudit.KbOfCode += fileSize;

                // Get line related items

                finalAudit.LinesOfCode += audit.LinesOfCode;
                finalAudit.Characters += audit.Characters;
                finalAudit.Todos += audit.Todos;

                finalAudit.EmptyLinesVs += audit.EmptyLinesVs;
                finalAudit.WhiteSpaceVs += audit.WhiteSpaceVs;

                // Get records

                if (fileSize > finalAudit.LargestByKb)
                {
                    finalAudit.LargestByKb = fileSize;
                    finalAudit.LargestByKbFile = fileItem.Path.Remove(0, Path.Length);
                }

                if (audit.Characters > finalAudit.LargestByChars)
                {
                    finalAudit.LargestByChars = audit.Characters;
                    finalAudit.LargestByCharsFile = fileItem.Path.Remove(0, Path.Length);
                }

                if (audit.LinesOfCode > finalAudit.LargestByLines)
                {
                    finalAudit.LargestByLines = audit.LinesOfCode;
                    finalAudit.LargestByLinesFile = fileItem.Path.Remove(0, Path.Length);
                }

                double density = (double) audit.Characters / audit.LinesOfCode;
                if (density > finalAudit.HighestDensity)
                {
                    finalAudit.HighestDensity = density;
                    finalAudit.HighestDensityFile = fileItem.Path.Remove(0, Path.Length);
                }

                int daysFromLastEdit = (int)(DateTime.Now - fileInfo.LastWriteTime).TotalDays;
                if (daysFromLastEdit > finalAudit.OldestFileDays)
                {
                    finalAudit.OldestFileDays = daysFromLastEdit;
                    finalAudit.OldestFile = fileItem.Path.Remove(0, Path.Length);
                }

                int daysFromCreation = (int)(DateTime.Now - fileInfo.CreationTime).TotalDays;
                if (daysFromCreation > finalAudit.NewestFileDays)
                {
                    finalAudit.NewestFileDays = daysFromCreation;
                    finalAudit.NewestFile = fileItem.Path.Remove(0, Path.Length);
                }
            }

            finalAudit.Languages = files.Select(x => System.IO.Path.GetExtension(x.Path)).Distinct().Count();

            double emptyLines = (double) finalAudit.EmptyLinesVs / finalAudit.LinesOfCode;
            finalAudit.EmptyLinesVs = (int)(emptyLines * 100);

            double whiteSpace = (double)finalAudit.WhiteSpaceVs / finalAudit.Characters;
            finalAudit.WhiteSpaceVs = (int)(whiteSpace * 100);

            return finalAudit;
        }
    }

    internal enum OptionIndexes
    {
        IgnoreNonTextFiles = 0,
    }
}
