using Microsoft.Win32;
using System.Windows.Forms;

namespace GreenHat.Utils
{
    class RightMenuManager
    {
        private static bool KeyExists(string subKeyPath)
        {
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(subKeyPath))
            {
                return key != null;
            }
        }

        public static void AddMenu()
        {
            if (!KeyExists(@"*\shell\使用 绿帽子安全防护 进行扫描"))
            {
                RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"*\shell\使用 绿帽子安全防护 进行扫描");
                key.SetValue("Icon", Application.ExecutablePath);
                RegistryKey cmdKey = key.CreateSubKey("command");
                cmdKey.SetValue("", $@"{Application.ExecutablePath} ""%1""");
            }
            if (!KeyExists(@"Directory\shell\使用 绿帽子安全防护 进行扫描"))
            {
                RegistryKey dirKey = Registry.ClassesRoot.CreateSubKey(@"Directory\shell\使用 绿帽子安全防护 进行扫描");
                dirKey.SetValue("Icon", Application.ExecutablePath);
                RegistryKey dirCmdKey = dirKey.CreateSubKey("command");
                dirCmdKey.SetValue("", $@"{Application.ExecutablePath} ""%1""");
            }
        }

        public static void RemoveMenu()
        {
            if (KeyExists(@"*\shell\使用 绿帽子安全防护 进行扫描"))
            {
                Registry.ClassesRoot.DeleteSubKeyTree(@"*\shell\使用 绿帽子安全防护 进行扫描");
            }
            if (KeyExists(@"Directory\shell\使用 绿帽子安全防护 进行扫描"))
            {
                Registry.ClassesRoot.DeleteSubKeyTree(@"Directory\shell\使用 绿帽子安全防护 进行扫描");
            }
        }
    }
}