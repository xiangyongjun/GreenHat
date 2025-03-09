using OpenMcdf;
using PeNet;
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
                compoundFile.RootStorage.VisitEntries(item =>
                {
                    if (!end && item.IsStream)
                    {
                        var stream = compoundFile.RootStorage.GetStream(item.Name);
                        byte[] buffer = stream.GetData();
                        if (PeFile.IsPeFile(buffer) && !callback(buffer, false)) end = true;
                        else if (IsCab(buffer))
                        {
                            var files = ZipExtractor.ExtractFiles(buffer);
                            foreach (var file in files)
                            {
                                if (!callback(file, true))
                                {
                                    end = true;
                                    break;
                                }
                            }
                        }
                    }
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

        public static bool IsCab(byte[] data)
        {
            if (data == null || data.Length < 4) return false;
            return data[0] == 0x4D &&
                   data[1] == 0x53 &&
                   data[2] == 0x43 &&
                   data[3] == 0x46;
        }

        public static bool CheckPageStructure(byte[] source)
        {
            if (source == null && source.Length < 140) return false;
            byte[] target = { 0x56, 0xf7, 0x70, 0x7d };
            return source.AsSpan().LastIndexOf(target) != -1;
        }
    }
}
