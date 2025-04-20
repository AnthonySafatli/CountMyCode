using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Utils
{
    public static class FileUtils
    {
        // TODO: Add error checking for inaccessible files

        public static async IAsyncEnumerable<string> ReadLinesAsync(string path)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();
                if (line != null)
                {
                    yield return line;
                }
            }
        }

        public static bool IsBinary(string filePath, int requiredConsecutiveNul = 1)
        {
            const int charsToCheck = 8000;
            const char nulChar = '\0';

            int nulCount = 0;

            using (var streamReader = new StreamReader(filePath))
            {
                for (var i = 0; i < charsToCheck; i++)
                {
                    if (streamReader.EndOfStream)
                        return false;

                    if ((char)streamReader.Read() == nulChar)
                    {
                        nulCount++;

                        if (nulCount >= requiredConsecutiveNul)
                            return true;
                    }
                    else
                    {
                        nulCount = 0;
                    }
                }
            }

            return false;
        }

    }
}
