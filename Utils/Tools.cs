using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace GreenHat.Utils
{
    internal static class Tools
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSuspendProcess(IntPtr processHandle);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtResumeProcess(IntPtr processHandle);

        public static Bitmap Base64ToBitmap(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                return new Bitmap(ms);
            }
        }

        public static void CreateAndStartService(string serviceName, string binPath)
        {
            try
            {
                ExecuteCommand("sc.exe", $"create {serviceName} binPath= \"{binPath}\"", AppDomain.CurrentDomain.BaseDirectory);
                ExecuteCommand("sc.exe", $"config {serviceName} start= AUTO", AppDomain.CurrentDomain.BaseDirectory);
                ExecuteCommand("net.exe", $"start {serviceName}", AppDomain.CurrentDomain.BaseDirectory);
            }
            catch { }
        }

        public static bool ServiceExists(string serviceName)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "sc";
                process.StartInfo.Arguments = $"query {serviceName}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output.Contains("STATE");
            }
        }

        public static void DeleteService(string serviceName)
        {
            ExecuteCommand("net.exe", $"stop {serviceName}", AppDomain.CurrentDomain.BaseDirectory);
            ExecuteCommand("sc.exe", $"delete {serviceName}", AppDomain.CurrentDomain.BaseDirectory);
        }

        public static string ExecuteCommand(string path, string args = "", string dir = "")
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WorkingDirectory = dir;
                process.StartInfo.Verb = "runas";
                process.Start();
                return process.StandardOutput.ReadToEnd();
            }
        }

        public static void EncryptFile(string inputFile, string outputFile, string key)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] iv = new byte[16];
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(iv);
                }
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = iv;
                    using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create))
                    {
                        fsOutput.Write(iv, 0, iv.Length);
                        using (CryptoStream cryptoStream = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
                        {
                            fsInput.CopyTo(cryptoStream);
                        }
                    }
                }
            }
            catch { }
        }

        public static void DecryptFile(string inputFile, string outputFile, string key)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] iv = new byte[16];
                using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
                {
                    fsInput.Read(iv, 0, iv.Length);
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = keyBytes;
                        aes.IV = iv;
                        using (CryptoStream cryptoStream = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create))
                        {
                            cryptoStream.CopyTo(fsOutput);
                        }
                    }
                }
            }
            catch { }
        }

        public static void SuspendProcess(Process process)
        {
            NtSuspendProcess(process.Handle);
        }

        public static void ResumeProcess(Process process)
        {
            NtResumeProcess(process.Handle);
        }

        public static string GetMd5(string path)
        {
            FileStream fileStream = File.OpenRead(path);
            string result = BitConverter.ToString(MD5.Create().ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
            fileStream.Close();
            return result;
        }
    }
}
