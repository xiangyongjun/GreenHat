using Microsoft.Win32;

namespace GreenHat.Utils
{
    internal static class GreenHatConfig
    {
        private static string subKey = @"Software\GreenHat";
        private static bool processEnable;
        private static bool fileEnable;
        private static bool mbrEnable;
        private static bool rightMenuEnable;
        private static bool autoStartEnable;
        private static bool greenHatEnable;
        private static bool scriptEnable;
        private static bool talonflameEnable;

        static GreenHatConfig()
        {
            if (Registry.CurrentUser.OpenSubKey(subKey) == null) Registry.CurrentUser.CreateSubKey(subKey, true);
            processEnable = GetBoolValue("processEnable");
            fileEnable = GetBoolValue("fileEnable");
            mbrEnable = GetBoolValue("mbrEnable");
            rightMenuEnable = GetBoolValue("rightMenuEnable");
            autoStartEnable = GetBoolValue("autoStartEnable");
            greenHatEnable = GetBoolValue("greenHatEnable");
            scriptEnable = GetBoolValue("scriptEnable");
            talonflameEnable = GetBoolValue("talonflameEnable", 0);
        }

        private static bool GetBoolValue(string name, int value = 1)
        {
            object result = Registry.CurrentUser.OpenSubKey(subKey)?.GetValue(name, value);
            return result != null && (int)result == 1;
        }

        private static void SetBoolValue(string name, bool value)
        {
            Registry.CurrentUser.OpenSubKey(subKey, true)?.SetValue(name, value ? 1 : 0);
        }

        public static bool ProcessEnable
        {
            get
            {
                return processEnable;
            }
            set
            {
                if (ProcessEnable == value) return;
                processEnable = value;
                SetBoolValue("processEnable", value);
            }
        }

        public static bool FileEnable
        {
            get
            {
                return fileEnable;
            }
            set
            {
                if (FileEnable == value) return;
                fileEnable = value;
                SetBoolValue("fileEnable", value);
            }
        }

        public static bool MbrEnable
        {
            get
            {
                return mbrEnable;
            }
            set
            {
                if (MbrEnable == value) return;
                mbrEnable = value;
                SetBoolValue("mbrEnable", value);
            }
        }

        public static bool RightMenuEnable
        {
            get
            {
                return rightMenuEnable;
            }
            set
            {
                if (RightMenuEnable == value) return;
                rightMenuEnable = value;
                SetBoolValue("rightMenuEnable", value);
            }
        }

        public static bool AutoStartEnable
        {
            get
            {
                return autoStartEnable;
            }
            set
            {
                if (AutoStartEnable == value) return;
                autoStartEnable = value;
                SetBoolValue("autoStartEnable", value);
            }
        }

        public static bool GreenHatEnable
        {
            get
            {
                return greenHatEnable;
            }
            set
            {
                if (GreenHatEnable == value) return;
                greenHatEnable = value;
                SetBoolValue("greenHatEnable", value);
            }
        }

        public static bool ScriptEnable
        {
            get
            {
                return scriptEnable;
            }
            set
            {
                if (ScriptEnable == value) return;
                scriptEnable = value;
                SetBoolValue("scriptEnable", value);
            }
        }

        public static bool TalonflameEnable
        {
            get
            {
                return talonflameEnable;
            }
            set
            {
                if (TalonflameEnable == value) return;
                talonflameEnable = value;
                SetBoolValue("talonflameEnable", value);
            }
        }
    }
}
