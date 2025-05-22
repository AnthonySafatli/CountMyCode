using CountMyCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Models
{
    internal class LanguageMenu
    {
        internal Dictionary<string, string> ProgrammingExtensions { get; set; }
        internal FileItem FileItem { get; set; } 

        public LanguageMenu(FileItem fileItem)
        {
            FileItem = fileItem;
            ProgrammingExtensions = InitializeProgrammingExtensions();
        }

        private Dictionary<string, string> InitializeProgrammingExtensions()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { ".cs", "C#" },
                { ".cshtml", "Razor (C#)" },
                { ".csx", "C# Script" },
                { ".vb", "VB.NET" },
                { ".fs", "F#" },
                { ".fsx", "F# Script" },

                { ".java", "Java" },
                { ".kt", "Kotlin" },
                { ".kts", "Kotlin Script" },
                { ".scala", "Scala" },
                { ".groovy", "Groovy" },

                { ".js", "JavaScript" },
                { ".jsx", "JavaScript (React)" },
                { ".ts", "TypeScript" },
                { ".tsx", "TypeScript (React)" },
                { ".coffee", "CoffeeScript" },

                { ".py", "Python" },
                { ".r", "R" },
                { ".jl", "Julia" },

                { ".php", "PHP" },
                { ".html", "HTML" },
                { ".htm", "HTML" },
                { ".xml", "XML" },
                { ".json", "JSON" },
                { ".yaml", "YAML" },
                { ".yml", "YAML" },

                { ".css", "CSS" },
                { ".scss", "SASS" },
                { ".sass", "SASS" },
                { ".less", "LESS" },
                { ".styl", "Stylus" },

                { ".go", "Go" },
                { ".rs", "Rust" },
                { ".swift", "Swift" },

                { ".m", "Objective-C" },
                { ".mm", "Objective-C++" },

                { ".c", "C" },
                { ".cpp", "C++" },
                { ".cc", "C++" },
                { ".cxx", "C++" },
                { ".h", "C / C++ Header" },
                { ".hpp", "C++ Header" },
                { ".hh", "C++ Header" },
                { ".hxx", "C++ Header" },

                { ".sh", "Shell Script" },
                { ".bash", "Bash Script" },
                { ".zsh", "Zsh Script" },
                { ".bat", "Batch File" },
                { ".cmd", "Command Script" },
                { ".ps1", "PowerShell" },

                { ".pl", "Perl" },
                { ".pm", "Perl Module" },

                { ".lua", "Lua" },
                { ".dart", "Dart" },
                { ".ex", "Elixir" },
                { ".exs", "Elixir Script" },
                { ".clj", "Clojure" },
                { ".cljs", "ClojureScript" },
                { ".lisp", "Lisp" },
                { ".scm", "Scheme" },
                { ".rkt", "Racket" },

                { ".asm", "Assembly" },
                { ".s", "Assembly" },

                { ".sql", "SQL" },
                { ".db", "Database File" },
                { ".sqlite", "SQLite Database" },

                { ".md", "Markdown" },
                { ".markdown", "Markdown" },
                { ".rst", "reStructuredText" },
                { ".adoc", "AsciiDoc" },

                { ".txt", "Text" },
                { ".log", "Log File" },
                { ".cfg", "Config File" },
                { ".conf", "Config File" },
                { ".ini", "INI File" },
                { ".env", "Environment Config" },
                { ".toml", "TOML Config" },
                { ".lock", "Lock File" },

                { ".dockerfile", "Dockerfile" },
                { ".dockerignore", "Docker Ignore File" },
                { ".gitignore", "Git Ignore File" },
                { ".gitattributes", "Git Attributes" },
                { ".gitmodules", "Git Submodules" },

                { ".make", "Makefile" },
                { ".mk", "Makefile" },
                { ".cmake", "CMake" },
                { ".ninja", "Ninja Build" },

                { ".tsconfig.json", "TypeScript Config" },
                { ".babelrc", "Babel Config" },
                { ".eslintrc", "ESLint Config" },
                { ".prettierrc", "Prettier Config" },
                { ".editorconfig", "EditorConfig" },

                { ".ejs", "Embedded JS Template" },
                { ".hbs", "Handlebars" },
                { ".mustache", "Mustache" },
                { ".twig", "Twig" },
                { ".erb", "ERB (Ruby)" },
                { ".jinja", "Jinja Template" },
                { ".pug", "Pug (Jade)" },
                { ".haml", "HAML" },

                { ".wasm", "WebAssembly" },
                { ".jar", "Java Archive" },
                { ".war", "Java Web Archive" },
                { ".apk", "Android Package" },
                { ".ipa", "iOS Package" },
                { ".app", "Mac App Bundle" },
                { ".exe", "Windows Executable" },
                { ".dll", "Dynamic Link Library" },
                { ".so", "Shared Object (Linux)" },
                { ".dylib", "Dynamic Library (macOS)" },
                { ".out", "Binary Executable" },
                { ".a", "Static Library" },
                { ".o", "Object File" },
                { ".bin", "Binary File" },
                { ".class", "Java Class File" },

                { ".sln", "Visual Studio Solution" },
                { ".csproj", "C# Project File" },
                { ".vcxproj", "C++ Project File" },
                { ".xcodeproj", "Xcode Project" },
                { ".xcworkspace", "Xcode Workspace" },
                { ".proj", "Project File" },
                { ".nuspec", "NuGet Spec File" },
                { ".pom", "Maven POM File" },
                { ".gradle", "Gradle Build File" },
                { ".bzl", "Bazel Build File" },
                { ".bazel", "Bazel Workspace File" },

                { ".vagrantfile", "Vagrant Config" },
                { ".jenkinsfile", "Jenkins Pipeline" },
            };
        }

        internal string GetLanguageName(string extension, List<FileItem> files, string prefixText, string? previousName)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(prefixText);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please enter a name for this language: ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(extension);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine("This extension is used in these files:");
            Console.WriteLine();

            var filesWithExtension = files.Where(x => Path.GetExtension(x.Path) == extension);

            int amount = 5;
            files = filesWithExtension.Take(amount).ToList();
            foreach (FileItem file in files)
            {
                Console.WriteLine(file.DisplayPath);
            }

            if (filesWithExtension.Count() > amount)
            {
                Console.WriteLine($"... and {filesWithExtension.Count() - amount} more");
            }

            Console.WriteLine();
            Console.WriteLine("!esc    | Go back to menu");
            Console.WriteLine("!prev   | Go to previous extension");
            Console.WriteLine("!next   | Go to next extension");
            Console.WriteLine("!ignore | Ignore all files with this extension");
            Console.WriteLine("!forget | Go back to menu and disregard changes");

            if (previousName != null)
            {
                Console.WriteLine();
                Console.Write("Previously entered language name: ");
                Console.Write(previousName);
                Console.WriteLine("| Press enter to skip or type to replace.");
            }

            return InputUtils.GetInput(true);
        }

        internal List<string> GetExtensions(FileItem fileItem)
        {
            List<string> extensions = [];
            foreach (FileItem item in fileItem.Children)
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
                    extensions.AddRange(GetExtensions(item));
                }
            }
            return extensions;
        }

        internal Dictionary<string, string>? RunExtensionsMenu()
        {
            FileItem.LoadChildren(true);
            List<FileItem> files = FileItem.GetFiles();

            List<string> extensions = GetExtensions(FileItem);
            extensions = extensions.Where(x => !ProgrammingExtensions.ContainsKey(x)).ToList();

            Dictionary<int, string> stagedNames = [];

            while (true)
            {
                for (int i = 0; i < extensions.Count; i++)
                {
                    string extension = extensions[i];

                    if (string.IsNullOrWhiteSpace(extension))
                        continue;

                    stagedNames.TryGetValue(i, out string? previousName);
                    string languageName = GetLanguageName(extension, files, $"[{i + 1} / {extensions.Count}] ", previousName);

                    if (languageName.ToLower() == "!forget")
                        return null;

                    if (languageName.ToLower() == "!esc")
                    {
                        ApplyStagedNames(extensions, stagedNames);
                        return null;
                    }

                    if (languageName.ToLower() == "!prev")
                    {
                        i = i == 0 ? extensions.Count - 2 : i - 2;
                        continue;
                    }

                    if (languageName.ToLower() == "!next" || string.IsNullOrWhiteSpace(languageName))
                    {
                        continue;
                    }

                    stagedNames.Add(i, languageName);
                }

                if (stagedNames.Count == extensions.Count)
                    break;
            }

            ApplyStagedNames(extensions, stagedNames);

            return ProgrammingExtensions;
        }

        private void ApplyStagedNames(List<string> extensions, Dictionary<int, string> stagedNames)
        {
            foreach (var stagedName in stagedNames)
            {
                int index = stagedName.Key;
                string language = stagedName.Value;

                if (language.ToLower() == "!ignore")
                {
                    string extension = extensions[index];
                    IgnoreExtension(FileItem, extension);
                    continue;
                }

                ProgrammingExtensions.Add(extensions[index], language);
            }
        }


        internal void IgnoreExtension(FileItem fileItem, string extension)
        {
            foreach (FileItem item in fileItem.Children)
            {
                if (item.ItemType == Status.Folder)
                    IgnoreExtension(item, extension);

                if (item.ItemType == Status.File)
                {
                    string itemExtension = System.IO.Path.GetExtension(item.Path);
                    if (itemExtension == extension)
                        item.Status = Status.Ignored;
                }
            }
        }
    }
}
