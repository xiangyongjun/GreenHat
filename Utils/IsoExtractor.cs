using DiscUtils;
using DiscUtils.Iso9660;
using System;
using System.IO;

namespace GreenHat.Utils
{
    class IsoExtractor
    {
        public static void Scan(string path, Func<byte[], bool> callback)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var cdReader = new CDReader(stream, true);
                ProcessDirectory(cdReader.Root, callback);
            }
        }

        private static void ProcessDirectory(DiscDirectoryInfo directory, Func<byte[], bool> callback)
        {
            foreach (var file in directory.GetFiles())
            {
                using (var stream = file.OpenRead())
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    byte[] fileData = memoryStream.ToArray();
                    callback(fileData);
                }
            }
            foreach (var subDir in directory.GetDirectories())
            {
                ProcessDirectory(subDir, callback);
            }
        }

        public static bool IsIsoFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (stream.Length < 32774) return false;
                stream.Seek(32768, SeekOrigin.Begin);
                byte[] header = new byte[6];
                int bytesRead = stream.Read(header, 0, 6);
                if (bytesRead != 6) return false;
                return header[0] == 0x01 &&
                       header[1] == 0x43 && // 'C'
                       header[2] == 0x44 && // 'D'
                       header[3] == 0x30 && // '0'
                       header[4] == 0x30 && // '0'
                       header[5] == 0x31;   // '1'
            }
        }

        public static bool IsIsoFile(byte[] data)
        {
            const int HEADER_OFFSET = 0x8000 + 1;
            const string MAGIC = "CD001";
            if (data.Length < HEADER_OFFSET + MAGIC.Length) return false;
            string header = System.Text.Encoding.ASCII.GetString(data, HEADER_OFFSET, MAGIC.Length);
            return header == MAGIC;
        }
    }
}