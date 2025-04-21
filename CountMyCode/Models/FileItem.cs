using CountMyCode.Utils;
using Microsoft.AspNetCore.Mvc;
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
        internal bool ChildrenLoaded { get; set; }
        internal FileItem? Parent { get; set; }
        internal string Path { get; set; }
        internal Status ItemType { get; set; }
        internal Status Status { get; set; }
        internal List<FileItem> Children { get; set; } = new List<FileItem>();
        internal List<bool> Options { get; set; } = new List<bool>();
        internal Menu? Menu { get; set; }
        internal LanguageMenu? LanguageMenu { get; set; }

        internal string DisplayName
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        internal string DisplayPath 
        {
            get
            {
                FileItem? lineageFileItem = Parent;
                while (lineageFileItem != null)
                {
                    lineageFileItem = lineageFileItem.Parent;
                }

                return Path.Remove(0, lineageFileItem?.Path.Length ?? 0);
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
            ChildrenLoaded = false;
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

        internal void LoadChildren(bool recursive = false)
        {
            // TODO: Check if the path is a directory

            if (ItemType == Status.File)
                return;

            if (ChildrenLoaded)
                return;

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

            if (recursive)
            {
                foreach (FileItem item in Children)
                {
                    if (item.ItemType == Status.Folder)
                        item.LoadChildren(true);
                }
            }

            ChildrenLoaded = true;
        }

        internal void ToggleIgnore(FileItem? item)
        {
            if (item == null)
                return;

            item.Status = item.Status == Status.Ignored ? item.ItemType : Status.Ignored;
        }

        internal bool? RunMenu()
        {
            Menu ??= new Menu(this);
            return Menu.RunMenu();
        }

        internal bool RunExtensionsMenu()
        {
            LanguageMenu ??= new LanguageMenu(this);
            Dictionary<string, string>? result = LanguageMenu.RunExtensionsMenu();
            return result != null;
        }

        internal List<FileItem> GetFiles()
        {
            if (!ChildrenLoaded)
                LoadChildren();

            List<FileItem> files = new List<FileItem>();

            foreach (FileItem item in Children)
            {
                if (item.Status == Status.Ignored)
                    continue;

                if (item.ItemType == Status.File)
                {
                    files.Add(item);
                }
                else if (item.ItemType == Status.Folder)
                {
                    files.AddRange(item.GetFiles());
                }
            }

            return files;
        }

        internal void FilterItems(bool? ignoreNonTextItems = null)
        {
            LoadChildren(true);

            ignoreNonTextItems ??= Options[(int)OptionIndexes.IgnoreNonTextFiles];

            foreach (FileItem item in Children)
            {
                if (ignoreNonTextItems.Value)
                {
                    if (item.Status == Status.Folder)
                    {
                        item.FilterItems(ignoreNonTextItems);
                    }
                    else if (item.Status == Status.File)
                    {
                        if (FileUtils.IsBinary(item.Path))
                            item.Status = Status.Ignored;
                    }
                }
            }
        }
    }

    internal enum OptionIndexes
    {
        IgnoreNonTextFiles = 0,
    }
}
