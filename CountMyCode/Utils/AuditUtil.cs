using CountMyCode.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Utils;

internal static class AuditUtil
{
    internal static async Task<AuditStats> RunAudit(FileItem auditedFolder)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Running the audit. Counting your code. Please wait...");

        List<FileItem> files = auditedFolder.GetFiles();

        List<Task<AuditStats>> auditTasks = new List<Task<AuditStats>>();
        List<FileInfo> fileInfos = new List<FileInfo>();

        foreach (FileItem file in files)
        {
            auditTasks.Add(ProcessFileAsync(file.Path, auditedFolder.LanguageMenu?.ProgrammingExtensions));
            fileInfos.Add(new FileInfo(file.Path));
        }

        AuditStats finalAudit = new AuditStats();

        AuditStats[] auditResults = await Task.WhenAll(auditTasks);
        for (int i = 0; i < fileInfos.Count; i++)
        {
            FileItem fileItem = files[i];
            AuditStats audit = auditResults[i];
            FileInfo fileInfo = fileInfos[i];

            string displayPath = fileItem.DisplayPath.Substring(auditedFolder.DisplayPath.Length);
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

            // Get language related items

            string language = audit.FilesByLanguage.First().Name;
            
            LanguageStats? filesByLanguage = finalAudit.FilesByLanguage.FirstOrDefault(x => x.Name == language);
            if (filesByLanguage == null)
            {
                filesByLanguage = new LanguageStats
                {
                    Name = language,
                };
                finalAudit.FilesByLanguage.Add(filesByLanguage);
            }
            filesByLanguage.Amount += audit.FilesByLanguage.First().Amount;

            LanguageStats? linesByLanguage = finalAudit.LinesByLanguage.FirstOrDefault(x => x.Name == language);
            if (linesByLanguage == null)
            {
                linesByLanguage = new LanguageStats
                {
                    Name = language,
                };
                finalAudit.LinesByLanguage.Add(linesByLanguage);
            }
            linesByLanguage.Amount += audit.LinesByLanguage.First().Amount;

            LanguageStats? charactersByLanguage = finalAudit.CharactersByLanguage.FirstOrDefault(x => x.Name == language);
            if (charactersByLanguage == null)
            {
                charactersByLanguage = new LanguageStats
                {
                    Name = language,
                };
                finalAudit.CharactersByLanguage.Add(charactersByLanguage);
            }
            charactersByLanguage.Amount += audit.CharactersByLanguage.First().Amount;

            // Get records
            if (fileSize > finalAudit.LargestByKb)
            {
                finalAudit.LargestByKb = fileSize;
                finalAudit.LargestByKbFile = displayPath;
            }

            if (audit.Characters > finalAudit.LargestByChars)
            {
                finalAudit.LargestByChars = audit.Characters;
                finalAudit.LargestByCharsFile = displayPath;
            }

            if (audit.LinesOfCode > finalAudit.LargestByLines)
            {
                finalAudit.LargestByLines = audit.LinesOfCode;
                finalAudit.LargestByLinesFile = displayPath;
            }

            double density = (double)audit.Characters / audit.LinesOfCode;
            if (density > finalAudit.HighestDensity)
            {
                finalAudit.HighestDensity = density;
                finalAudit.HighestDensityFile = displayPath;
            }

            int daysFromLastEdit = (int)(DateTime.Now - fileInfo.LastWriteTime).TotalDays;
            if (daysFromLastEdit > finalAudit.OldestFileDays)
            {
                finalAudit.OldestFileDays = daysFromLastEdit;
                finalAudit.OldestFile = displayPath;
            }

            int daysFromCreation = (int)(DateTime.Now - fileInfo.CreationTime).TotalDays;
            if (daysFromCreation > finalAudit.NewestFileDays)
            {
                finalAudit.NewestFileDays = daysFromCreation;
                finalAudit.NewestFile = displayPath;
            }
        }

        finalAudit.Languages = files.Select(x => System.IO.Path.GetExtension(x.Path)).Distinct().Count();

        for (int i = 0; i < finalAudit.FilesByLanguage.Count; i++)
        {
            Color randomColour = ColourUtils.GetRandomHSBColor();
            string colourHex = ColourUtils.ToHex(randomColour);

            finalAudit.FilesByLanguage[i].Colour = colourHex;
            finalAudit.LinesByLanguage[i].Colour = colourHex;
            finalAudit.CharactersByLanguage[i].Colour = colourHex;
        }

        double emptyLines = (double)finalAudit.EmptyLinesVs / finalAudit.LinesOfCode;
        finalAudit.EmptyLinesVs = (int)(emptyLines * 100);

        double whiteSpace = (double)finalAudit.WhiteSpaceVs / finalAudit.Characters;
        finalAudit.WhiteSpaceVs = (int)(whiteSpace * 100);

        return finalAudit;
    }

    internal static async Task<AuditStats> ProcessFileAsync(string path, Dictionary<string, string> languages)
    {
        AuditStats audit = new AuditStats();

        string? language = languages.GetValueOrDefault(Path.GetExtension(path));
        language ??= "Other";

        LanguageStats languageStat = new LanguageStats
        {
            Name = language
        };

        audit.FilesByLanguage.Add(languageStat.Clone());
        audit.LinesByLanguage.Add(languageStat.Clone());
        audit.CharactersByLanguage.Add(languageStat.Clone());

        audit.FilesByLanguage.First().Amount++;

        await foreach (string line in FileUtils.ReadLinesAsync(path))
        {
            audit.LinesOfCode++;
            audit.Characters += line.Length;

            audit.LinesByLanguage.First().Amount++;
            audit.CharactersByLanguage.First().Amount += line.Length;

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
}
