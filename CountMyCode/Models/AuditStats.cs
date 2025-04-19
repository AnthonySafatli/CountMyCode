using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Models
{
    internal class AuditStats
    {
        public int Files { get; set; }
        public int LinesOfCode { get; set; }
        public int Languages { get; set; }
        public int MbOfCode { get; set; }
        public int Todos { get; set; }
        public int AvgLinesPerFile { get; set; }
        public int AvgCharsPerFile { get; set; }
        public int AvgMbPerFile { get; set; }

        public int EmptyLinesVs { get; set; }
        public int WhiteSpaceVs { get; set; }

        public double LargestByMb { get; set; }
        public string LargestByMbFile { get; set; } = string.Empty;
        public int LargestByChars { get; set; }
        public string LargestByCharsFile { get; set; } = string.Empty;
        public int LargestByLines { get; set; }
        public string LargestByLinesFile { get; set; } = string.Empty;
        public double HighestDensity { get; set; }
        public string HighestDensityFile { get; set; } = string.Empty;
        public int OldestFileDays { get; set; }
        public string OldestFile { get; set; } = string.Empty;
        public int NewestFileDays { get; set; }
        public int NewestFile { get; set; }
    }
}
