using System.Collections.Generic;
using System.IO;
using SevenZipExtractor;

namespace GreenHat.Utils
{
    class ZipExtractor
    {
        public static List<byte[]> ExtractFiles(byte[] cabData)
        {
            var extractedFiles = new List<byte[]>();
            using (var archiveStream = new MemoryStream(cabData))
            using (var archiveFile = new ArchiveFile(archiveStream))
            {
                foreach (Entry entry in archiveFile.Entries)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        entry.Extract(memoryStream);
                        extractedFiles.Add(memoryStream.ToArray());
                    }
                }
            }
            return extractedFiles;
        }
    }
}