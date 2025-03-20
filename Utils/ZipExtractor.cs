using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SevenZipExtractor;

namespace GreenHat.Utils
{
    class ZipExtractor
    {
        public static void Scan(string path, Func<byte[], bool> callback)
        {
            using (var archiveStream = new FileStream(path, FileMode.Open))
            using (var archiveFile = new ArchiveFile(archiveStream))
            {
                foreach (var entry in archiveFile.Entries)
                {
                    if (entry.IsEncrypted || entry.IsFolder) continue;
                    HashSet<string> names = new HashSet<string>() { ".exe", ".sys", ".dll", ".ocx", ".com", ".obj", ".pyd" };
                    if (!names.Any(it => entry.FileName.ToLower().EndsWith(it))) continue;
                    using (var memoryStream = new MemoryStream())
                    {
                        entry.Extract(memoryStream);
                        if (!callback(memoryStream.ToArray())) break;
                    }
                }
            }
        }

        public static void Scan(byte[] data, Func<byte[], bool> callback)
        {
            using (var archiveStream = new MemoryStream(data))
            using (var archiveFile = new ArchiveFile(archiveStream))
            {
                foreach (var entry in archiveFile.Entries)
                {
                    if (entry.IsEncrypted || entry.IsFolder) continue;
                    HashSet<string> names = new HashSet<string>() { ".exe", ".sys", ".dll", ".ocx", ".com", ".obj", ".pyd" };
                    if (!names.Any(it => entry.FileName.ToLower().EndsWith(it))) continue;
                    using (var memoryStream = new MemoryStream())
                    {
                        entry.Extract(memoryStream);
                        if (!callback(memoryStream.ToArray())) break;
                    }
                }
            }
        }

        public static bool IsCompressedFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] buffer = new byte[10];
                fs.Read(buffer, 0, buffer.Length);
                return IsCompressedFile(buffer);
            }
        }

        public static bool IsCompressedFile(byte[] buffer)
        {
            try
            {
                byte[][] CompressionSignatures = new byte[][]
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },      // ZIP
                    new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07 }, // RAR
                    new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }, // 7z
                    new byte[] { 0x1F, 0x8B },                 // GZIP
                    new byte[] { 0x4D, 0x53, 0x43, 0x46 },  // CAB
                    new byte[] { 0xFD, 0x37, 0x7A, 0x58, 0x5A, 0x00 }  // XZ
                };
                using (var stream = new MemoryStream(buffer))
                {
                    byte[] header = new byte[10];
                    int bytesRead = stream.Read(header, 0, header.Length);
                    foreach (var signature in CompressionSignatures)
                    {
                        if (bytesRead >= signature.Length)
                        {
                            bool match = true;
                            for (int i = 0; i < signature.Length; i++)
                            {
                                if (header[i] != signature[i])
                                {
                                    match = false;
                                    break;
                                }
                            }
                            if (match) return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
    }
}