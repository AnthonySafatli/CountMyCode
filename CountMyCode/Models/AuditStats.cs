using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Models;

public class AuditStats
{
    public int Files { get; set; }
    public int LinesOfCode { get; set; }
    public int Characters { get; set; }
    public int Languages { get; set; }
    public double KbOfCode { get; set; }
    public int Todos { get; set; }
    public double AvgLinesPerFile => Files == 0 ? 0 : (double)LinesOfCode / Files;
    public double AvgCharsPerFile => Files == 0 ? 0 : (double)Characters / Files;
    public double AvgKbPerFile => Files == 0 ? 0 : (double)KbOfCode / Files;

    public List<LanguageStats> FilesByLanguage { get; set; } = [];
    public List<LanguageStats> LinesByLanguage { get; set; } = [];
    public List<LanguageStats> CharactersByLanguage { get; set; } = [];

    public int EmptyLinesVs { get; set; }
    public int WhiteSpaceVs { get; set; }

    public double LargestByKb { get; set; }
    public string LargestByKbFile { get; set; } = string.Empty;
    public int LargestByChars { get; set; }
    public string LargestByCharsFile { get; set; } = string.Empty;
    public int LargestByLines { get; set; }
    public string LargestByLinesFile { get; set; } = string.Empty;
    public double HighestDensity { get; set; }
    public string HighestDensityFile { get; set; } = string.Empty;
    public int OldestFileDays { get; set; }
    public string OldestFile { get; set; } = string.Empty;
    public int NewestFileDays { get; set; }
    public string NewestFile { get; set; } = string.Empty;
}

public class LanguageStats
{
    public string Name { get; set; }
    public string Colour { get; set; }
    public int Amount { get; set; }

    public LanguageStats Clone() {
        return new LanguageStats
        {
            Name = Name,
            Colour = Colour,
            Amount = Amount
        };
    }
}