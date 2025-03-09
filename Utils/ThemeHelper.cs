using Microsoft.Win32;
using System.Drawing;

namespace GreenHat.Utils
{
    public class ThemeHelper
    {
        private static bool isLightMode = IsSystemLightMode();

        /// <summary>
        /// 判断系统是否浅色
        /// </summary>
        /// <returns></returns>
        public static bool IsSystemLightMode()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key != null)
            {
                int appsUseLightTheme = (int)key.GetValue("AppsUseLightTheme", -1);
                if (appsUseLightTheme == 1)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断是否浅色
        /// </summary>
        /// <returns></returns>
        public static bool IsLightMode()
        {
            return isLightMode;
        }

        /// <summary>
        /// 设置明暗颜色
        /// </summary>
        /// <param name="window">父窗口</param>
        /// <param name="isLight">是否亮色</param>
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
                AntdUI.Config.IsDark = true;// 设置为深色模式
                window.BackColor = Color.FromArgb(31, 31, 31);
                window.ForeColor = Color.White;
            }
        }
    }
}
