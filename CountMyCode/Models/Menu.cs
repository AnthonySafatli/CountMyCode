using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Models
{
    internal class Menu
    {
        internal FileItem FileItem { get; set; }
        internal bool IsInOptionsMenu { get; set; } = false;
        internal FileItem? SelectedFileItem { get; set; } = null;
        internal int ScrollStartIndex { get; set; } = 0;
        internal int SelectedOptionIndex { get; set; } = 0;

        public Menu(FileItem fileItem)
        {
            FileItem = fileItem;
        }

        /// <summary>
        /// return true => run the audit 
        /// returns null => restart the program
        /// </summary>
        internal bool? RunMenu()
        {
            Console.Clear();

            if (!FileItem.ChildrenLoaded)
                FileItem.LoadChildren();

            SelectedFileItem ??= FileItem.Children.FirstOrDefault();

            while (true)
            {
                RenderMenu();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (SelectedFileItem == null)
                        continue;

                    int index = FileItem.Children.IndexOf(SelectedFileItem);
                    if (index > 0)
                        SelectedFileItem = FileItem.Children[index - 1];
                    else
                        SelectedFileItem = FileItem.Children.Last();
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (SelectedFileItem == null)
                        continue;

                    int index = FileItem.Children.IndexOf(SelectedFileItem);
                    if (index < FileItem.Children.Count - 1)
                        SelectedFileItem = FileItem.Children[index + 1];
                    else
                        SelectedFileItem = FileItem.Children[0];
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (SelectedFileItem?.ItemType != Status.Folder)
                        continue;

                    bool? menuResult = SelectedFileItem.RunMenu();
                    if (menuResult == true || menuResult == null)
                        return menuResult;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (FileItem.Parent != null)
                        return false;
                }
                else if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    if (IsInOptionsMenu)
                    {
                        FileItem.Options[SelectedOptionIndex] = !FileItem.Options[SelectedOptionIndex];
                        continue;
                    }

                    if (SelectedFileItem == null)
                        continue;

                    FileItem.ToggleIgnore(SelectedFileItem);
                }
                else if (keyInfo.Key == ConsoleKey.Tab)
                {
                    if (FileItem.Options.Count > 0)
                        IsInOptionsMenu = !IsInOptionsMenu;
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

        private void RenderMenu()
        {
            Console.Clear();

            RenderHeader();

            RenderFileItems();

            RenderOptions();

            RenderInstructions();
        }


        private void RenderHeader()
        {
            ConsoleColor foregroundColour = FileItem.Parent == null ? ConsoleColor.DarkCyan : ConsoleColor.Cyan;

            Console.ForegroundColor = foregroundColour;
            Console.Write("| ");

            if (FileItem.IsIgnored)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[Ignored] ");
            }

            Console.ForegroundColor = foregroundColour;
            Console.WriteLine(FileItem.Path);
            Console.WriteLine();
        }

        private void RenderFileItems()
        {
            if (FileItem.Children.Count == 0)
            {
                Console.ForegroundColor = IsInOptionsMenu ? ConsoleColor.DarkGray : ConsoleColor.White;
                Console.WriteLine("No items found.");
                return;
            }

            int currentIndex = SelectedFileItem == null ? 0 : FileItem.Children.IndexOf(SelectedFileItem);
            int displayAreaHeight = Console.WindowHeight - (13 + FileItem.Options.Count);
            int totalItems = FileItem.Children.Count;

            // Adjust ScrollStartIndex if currentIndex is out of view
            if (currentIndex < ScrollStartIndex)
            {
                ScrollStartIndex = currentIndex;
            }
            else if (currentIndex >= ScrollStartIndex + displayAreaHeight)
            {
                ScrollStartIndex = currentIndex - displayAreaHeight + 1;
            }

            // Clamp ScrollStartIndex to prevent overflow
            if (ScrollStartIndex > totalItems - displayAreaHeight)
            {
                ScrollStartIndex = Math.Max(0, totalItems - displayAreaHeight);
            }

            // Show top ellipsis if not at the start
            if (ScrollStartIndex > 0)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  ...");
            }

            // Render visible items
            int linesRendered = 0;
            for (int i = ScrollStartIndex; i < totalItems && linesRendered < displayAreaHeight; i++, linesRendered++)
            {
                RenderFileItem(FileItem.Children[i]);
            }

            // Show bottom ellipsis if not at the end
            if (ScrollStartIndex + linesRendered < totalItems)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  ...");
            }
        }

        private void RenderFileItem(FileItem fileItem)
        {
            int statusPadding = 0;
            if (fileItem.Status == Status.Folder)
                statusPadding = 1;
            else if (fileItem.Status == Status.File)
                statusPadding = 3;

            RenderInputLine(!IsInOptionsMenu, fileItem.Status.ToString(), fileItem.Status == Status.Ignored, statusPadding, fileItem.DisplayName, fileItem == SelectedFileItem);
        }

        private void RenderOptions()
        {
            if (FileItem.Options.Count == 0)
                return;

            Console.WriteLine();
            Console.ForegroundColor = IsInOptionsMenu ? ConsoleColor.White : ConsoleColor.DarkGray;

            for (int i = 0; i < FileItem.Options.Count; i++)
            {
                string optionText = FileItem.Options[i] ? "ON" : "OFF";
                string optionDescription = "";

                switch (i)
                {
                    case (int)OptionIndexes.IgnoreNonTextFiles:
                        optionDescription = "Ignore non-text files";
                        break;
                    default:
                        throw new ApplicationException("Options list and enum are out of sync");
                }

                RenderInputLine(IsInOptionsMenu, optionText, !FileItem.Options[i], FileItem.Options[i] ? 1 : 0, optionDescription, i == SelectedOptionIndex);
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

            Console.Write(isSelected && isActive ? "> " : "  ");
            Console.Write($"[{status}] ");
            Console.Write(new string(' ', statusPadding));

            if (isSelected && isActive)
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(itemName);
        }

        private void RenderInstructions()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();

            if ((!IsInOptionsMenu && FileItem.Children.Count > 1) || (IsInOptionsMenu && FileItem.Options.Count > 1))
                Console.WriteLine("  UP / DOWN   | Navigate up or down");
            if (!IsInOptionsMenu && FileItem.Children.Any(x => x.ItemType == Status.Folder))
                Console.WriteLine("  RIGHT       | Enter a folder to view its contents");
            if (FileItem.Parent != null)
                Console.WriteLine("  LEFT        | Go back to the folders parents");
            if ((!IsInOptionsMenu && FileItem.Children.Count > 0) || (IsInOptionsMenu && FileItem.Options.Count > 0))
                Console.WriteLine("  SPACE       | " + (IsInOptionsMenu ? "Toggle an option" : "Toggle if an item is ignored by the audit"));
            if (FileItem.Options.Count > 0)
                Console.WriteLine($"  TAB         | {(IsInOptionsMenu ? "Exit" : "Enter")} options menu");

            Console.WriteLine("  ENTER       | Run the audit");
            Console.WriteLine("  ESC         | Choose another folder");
        }
    }
}
