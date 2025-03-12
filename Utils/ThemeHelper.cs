using Microsoft.Win32;
using System.Drawing;

namespace GreenHat.Utils
{
    public class ThemeHelper
    {
        private static bool isLightMode = IsSystemLightMode();

        public static bool IsSystemLightMode()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\GreenHat");
            if (key != null && key.GetValue("isLightMode") != null)
            {
                return key.GetValue("isLightMode").ToString().Equals("1");
            }
            key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key != null)
            {
                int appsUseLightTheme = (int)key.GetValue("AppsUseLightTheme", -1);
                return appsUseLightTheme == 1;
            }
            return true;
        }

        public static bool IsLightMode()
        {
            return isLightMode;
        }

        public static void SetColorMode(AntdUI.Window window, bool isLight)
        {
            isLightMode = isLight;
            if (isLight)
            {
                AntdUI.Config.IsLight = true;
                window.BackColor = Color.White;
                window.ForeColor = Color.Black;
            }
            else
            {
                AntdUI.Config.IsDark = true;
                window.BackColor = Color.FromArgb(31, 31, 31);
                window.ForeColor = Color.White;
            }
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\GreenHat", true);
            if (key != null)
            {
                key.SetValue("isLightMode", isLightMode ? "1" : "0");
            }
        }
    }
}
