using CountMyCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode
{
    internal class App
    {
        private readonly FileItem _initialFolder;
        private Dictionary<string, string> _programmingExtensions;

        internal App(string initialPath)
        {
            _initialFolder = new FileItem(initialPath, Status.Folder);
            _programmingExtensions = InitializeProgrammingExtensions();
        }

        internal void Run()
        {
            _initialFolder.RunMenu();

            _initialFolder.AddExtensions(_programmingExtensions);

            AuditStats stats = _initialFolder.GetStats();

            Console.Clear();
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
    }
}
