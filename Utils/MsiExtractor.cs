using OpenMcdf;
using System;
using System.IO;

namespace GreenHat.Utils
{
    class MsiExtractor
    {
        public static void ExtractPeFiles(string msiFilePath, Func<byte[], bool, bool> callback)
        {
            bool end = false;
            using (var compoundFile = new CompoundFile(msiFilePath))
            {
                compoundFile.RootStorage.VisitEntries(entry =>
                {
                    try
                    {
                        if (!end && entry.IsStream)
                        {
                            var stream = compoundFile.RootStorage.GetStream(entry.Name);
                            byte[] buffer = stream.GetData();
                            if (!callback(buffer, false)) end = true;
                            else if (ZipExtractor.IsCompressedFile(buffer))
                            {
                                ZipExtractor.Scan(buffer, (byte[] zipBuffer) =>
                                {
                                    if (end) return false;
                                    bool result = callback(buffer, true);
                                    if (!result) end = true;
                                    return result;
                                });
                            }
                        }
                    }
                    catch { }
                }, false);
            }
        }

        public static bool IsMsi(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return false;
            byte[] msiHeader = new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] fileHeader = new byte[8];
                int bytesRead = fs.Read(fileHeader, 0, fileHeader.Length);
                if (bytesRead != fileHeader.Length) return false;
                for (int i = 0; i < fileHeader.Length; i++)
                {
                    if (fileHeader[i] != msiHeader[i]) return false;
                }
            }
            return true;
        }
    }
}
