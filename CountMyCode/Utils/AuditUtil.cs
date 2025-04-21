using CountMyCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Utils
{
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
                    finalAudit.LargestByKbFile = fileItem.DisplayPath;
                }

                if (audit.Characters > finalAudit.LargestByChars)
                {
                    finalAudit.LargestByChars = audit.Characters;
                    finalAudit.LargestByCharsFile = fileItem.DisplayPath;
                }

                if (audit.LinesOfCode > finalAudit.LargestByLines)
                {
                    finalAudit.LargestByLines = audit.LinesOfCode;
                    finalAudit.LargestByLinesFile = fileItem.DisplayPath;
                }

                double density = (double)audit.Characters / audit.LinesOfCode;
                if (density > finalAudit.HighestDensity)
                {
                    finalAudit.HighestDensity = density;
                    finalAudit.HighestDensityFile = fileItem.DisplayPath;
                }

                int daysFromLastEdit = (int)(DateTime.Now - fileInfo.LastWriteTime).TotalDays;
                if (daysFromLastEdit > finalAudit.OldestFileDays)
                {
                    finalAudit.OldestFileDays = daysFromLastEdit;
                    finalAudit.OldestFile = fileItem.DisplayPath;
                }

                int daysFromCreation = (int)(DateTime.Now - fileInfo.CreationTime).TotalDays;
                if (daysFromCreation > finalAudit.NewestFileDays)
                {
                    finalAudit.NewestFileDays = daysFromCreation;
                    finalAudit.NewestFile = fileItem.DisplayPath;
                }
            }

            finalAudit.Languages = files.Select(x => System.IO.Path.GetExtension(x.Path)).Distinct().Count();

            double emptyLines = (double)finalAudit.EmptyLinesVs / finalAudit.LinesOfCode;
            finalAudit.EmptyLinesVs = (int)(emptyLines * 100);

            double whiteSpace = (double)finalAudit.WhiteSpaceVs / finalAudit.Characters;
            finalAudit.WhiteSpaceVs = (int)(whiteSpace * 100);

            return finalAudit;
        }

        internal static async Task<AuditStats> ProcessFileAsync(string path)
        {
            AuditStats audit = new AuditStats();

            await foreach (string line in FileUtils.ReadLinesAsync(path))
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
    }
}
