using AntdUI;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Threading.Tasks;

namespace GreenHat.Utils
{
    internal static class FileMonitor
    {
        private static List<FileSystemWatcher> fileWatchers = new List<FileSystemWatcher>();
        private static ManagementEventWatcher usbWatcher;

        public static void Start()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            string[] filters = "*.exe|*.dll|*.sys|*.bat|*.scr|*.msi|*.vbs|*.js|*.jse|*.wsf|*.ps1|*.py|*.sh|*.doc|*.docx|*.xls|*.xlsx|*.ppt|*.pptx|*.pdf|*.rtf|*.zip|*.rar|*.7z|*.tar|*.gz|*.reg|*.iso|*.img|*.html|*.htm|*.hta|*.jar|*.class".Split('|');
            foreach (DriveInfo drive in drives)
            {
                if (!drive.IsReady) continue;
                if (drive.DriveType != DriveType.Fixed && drive.DriveType != DriveType.Removable) continue;
                foreach (string filter in filters)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher
                    {
                        Path = drive.Name,
                        IncludeSubdirectories = true,
                        Filter = filter,
                        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                        EnableRaisingEvents = true
                    };
                    watcher.Created += FileHandle;
                    //watcher.Changed += FileHandle;
                    watcher.Renamed += FileHandle;
                    fileWatchers.Add(watcher);
                }
            }
            usbWatcher = new ManagementEventWatcher("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
            usbWatcher.EventArrived += new EventArrivedEventHandler((object sender, EventArrivedEventArgs e) => Restart());
            usbWatcher.Start();
        }

        private static void FileHandle(object sender, FileSystemEventArgs e)
        {
            string[] temp = e.FullPath.Split('\\');
            Task.Run(() => FileTask(temp[temp.Length - 1], e.FullPath));
        }

        private static void FileTask(string name, string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return;
                string[] result;
                if (!Database.IsWhite(path) && Engine.IsVirus(path, out result))
                {
                    Database.AddLog("病毒防护", "病毒拦截", $"文件：{path}");
                    InterceptQueue.Add(new InterceptForm(Localization.Get("文件实时监控", "文件实时监控"), name, path, result[0], result[1]));
                }
            }
            catch { }
        }

        public static void Stop()
        {
            usbWatcher.Stop();
            foreach (FileSystemWatcher item in fileWatchers)
            {
                item.Dispose();
            }
            fileWatchers.Clear();
        }

        public static void Restart()
        {
            Stop();
            Start();
        }
    }
}