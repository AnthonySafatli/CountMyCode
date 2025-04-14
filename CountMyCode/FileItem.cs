using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode
{
    internal class FileItem
    {
        public string Path { get; set; }
        public Status Status { get; set; }
        public List<FileItem> Children { get; set; } = new List<FileItem>();

        public FileItem(string path, Status status)
        {
            Path = path;
            Status = status;
        }
    }
}
