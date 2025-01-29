using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace GreenHat.Utils
{
    internal static class MbrProtect
    {
        const uint GENERIC_READ = 0x80000000;
        const uint GENERIC_WRITE = 0x40000000;
        const uint OPEN_EXISTING = 3;
        const uint FILE_SHARE_NONE = 0;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        private static List<IntPtr> handles = new List<IntPtr>();

        public static void Open()
        {
            for (int i = 0; i < 10; i++)
            {
                string drivePath = $"\\\\.\\PhysicalDrive{i}";
                IntPtr handle = CreateFile(
                    drivePath,
                    GENERIC_READ | GENERIC_WRITE,
                    FILE_SHARE_NONE,
                    IntPtr.Zero,
                    OPEN_EXISTING,
                    0,
                    IntPtr.Zero);

                if (handle == (IntPtr)(-1)) continue;
            }
        }

        public static void Close()
        {
            foreach (IntPtr handle in handles)
            {
                CloseHandle(handle);
            }
        }
    }
}
