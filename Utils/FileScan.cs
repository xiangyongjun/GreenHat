using Microsoft.Win32;
using ShellLink;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GreenHat.Utils
{
    internal static class FileScan
    {
        public static void Scan(List<string> paths, Func<string, bool> callback, int MaxDegreeOfParallelism = 1)
        {
            if (paths == null || paths.Count == 0) return;

            // 使用 Parallel.ForEach 实现多线程处理
            Parallel.ForEach(paths, new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, path =>
            {
                try
                {
                    if (string.IsNullOrEmpty(path)) return;

                    if (File.Exists(path))
                    {
                        // 如果是文件，调用回调函数
                        if (!callback(path)) return;
                    }
                    else if (Directory.Exists(path))
                    {
                        // 如果是目录，枚举文件并递归处理子目录
                        foreach (string file in Directory.EnumerateFiles(path))
                        {
                            if (!callback(file)) return;
                        }

                        // 获取子目录并递归调用 Scan
                        var subDirectories = Directory.GetDirectories(path);
                        if (subDirectories.Length > 0)
                        {
                            Scan(new List<string>(subDirectories), callback, MaxDegreeOfParallelism);
                        }
                    }
                }
                catch { }
            });
        }

        public static List<string> GetDiskList()
        {
            List<string> result = new List<string>();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                result.Add(drive.Name);
            }
            return result;
        }

        public static List<string> GetStartupList()
        {
            List<string> result = new List<string>();
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
            {
                if (key != null) 
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        result.Add(key.GetValue(valueName)?.ToString());
                    }
                }
            }
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
            {
                if (key != null)
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        result.Add(key.GetValue(valueName)?.ToString());
                    }
                }
            }
            string userStartupPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            if (Directory.Exists(userStartupPath))
            {
                foreach (string file in Directory.GetFiles(userStartupPath))
                {
                    result.Add(file);
                }
            }
            string allUsersStartupPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup));
            if (Directory.Exists(allUsersStartupPath))
            {
                foreach (string file in Directory.GetFiles(allUsersStartupPath))
                {
                    result.Add(file);
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                try
                {
                    Match match = Regex.Match(result[i], "\"(.*?)\"");
                    if (match.Success)
                    {
                        result[i] = match.Groups[1].Value;
                    }
                    if (Path.GetExtension(result[i]).ToLower() != ".lnk") continue;
                    match = Regex.Match(Shortcut.ReadFromFile(result[i]).StringData.NameString, "\"(.*?)\"");
                    if (match.Success)
                    {
                        result[i] = match.Groups[1].Value;
                    }
                }
                catch { }
            }
            return result.Distinct().ToList();
        }

        public static List<string> GetProcessList()
        {
            List<string> result = new List<string>();
            string query = "SELECT ExecutablePath FROM Win32_Process";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    try
                    {
                        string path = (string)obj.GetPropertyValue("ExecutablePath");
                        if (!string.IsNullOrEmpty(path)) result.Add(path);
                    }
                    catch { }
                }
            }
            return result.Distinct().ToList();
        }

        public static List<string> GetServiceList()
        {
            List<string> result = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT PathName FROM Win32_Service");
            foreach (ManagementObject service in searcher.Get())
            {
                string pathName = service["PathName"]?.ToString();
                if (string.IsNullOrEmpty(pathName)) continue;
                Match match = Regex.Match(pathName, @"^(.*?\.exe)(?=\s|$)");
                if (match.Success)
                {
                    pathName = match.Groups[1].Value;
                }
                result.Add(Regex.Replace(pathName, "\"", ""));
            }
            return result.Distinct().ToList();
        }
    }
}
