using AntdUI;
using GreenHat.Languages;
using GreenHat.Utils;
using GreenHat.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GreenHat
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private bool isLight = true;
        private List<UserControl> controls = new List<UserControl>();
        private bool firstHide = false;
        private NotifyIcon trayIcon = new NotifyIcon();

        public MainWindow(bool hide = false)
        {
            firstHide = hide;
            InitializeComponent();
            CheckEngine();
            CheckService();
            InitView();
            InitData();
            BindEventHandler();
            GetVersion();
            InitTray();
            InitMonitor();
            LoadLanguage();
            Engine.Init();
            SysConfig.AddLog("其他", "程序启动", $"操作时间：{DateTime.Now.ToString()}");
        }

        private void CheckEngine()
        {
            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}engine"))
            {
                AntdUI.Modal.open(new Modal.Config(this, Localization.Get("警告", "警告"), Localization.Get("检测到本地引擎丢失，防护功能将会失效！", "检测到本地引擎丢失，防护功能将会失效！"), TType.Warn)
                {
                    CloseIcon = true,
                    BtnHeight = 0
                });
            }
        }

        private void CheckService()
        {
            string servicePath = $"{AppDomain.CurrentDomain.BaseDirectory}GreenHatService.exe";
            if (SysConfig.GetSetting("开机启动").Enabled || Tools.ServiceExists("GreenHatService")) return;
            Tools.CreateAndStartService("GreenHatService", servicePath);
        }

        private void InitData()
        {
            isLight = ThemeHelper.IsSystemLightMode();
            button_color.Toggle = !isLight;
            ThemeHelper.SetColorMode(this, isLight);
            Config.ShowInWindow = true;
        }

        private void InitView()
        {
            controls.Add(new HomeView(this));
            controls.Add(new ScanView(this));
            controls.Add(new LogView(this));
            controls.Add(new SettingView(this));
            controls.Add(new AboutView());
            foreach (UserControl control in controls)
            {
                AutoDpi(control);
                control.Dock = DockStyle.Fill;
            }
        }

        private void GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            string[] arr = version.ToString().Split('.');
            titlebar.SubText = $"{arr[0]}.{arr[1]}.{arr[2]}";
        }

        private void BindEventHandler()
        {
            button_color.Click += Button_color_Click;
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                isLight = ThemeHelper.IsLightMode();
                button_color.Toggle = !isLight;
                ThemeHelper.SetColorMode(this, isLight);
            }
        }

        private void Button_color_Click(object sender, EventArgs e)
        {
            isLight = !isLight;
            button_color.Toggle = !isLight;
            ThemeHelper.SetColorMode(this, isLight);
        }

        private void segmented_SelectIndexChanged(object sender, IntEventArgs e)
        {
            UserControl control = controls[e.Value];
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(control);
            switch (e.Value)
            {
                case 0:
                    HomeView homeView = (HomeView)controls[e.Value];
                    homeView.UpdateCount();
                    break;
                case 2:
                    LogView logView = (LogView)controls[e.Value];
                    logView.InitTableData();
                    break;
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            Instance = this;
            segmented_SelectIndexChanged(null, new IntEventArgs(0));
            if (firstHide)
            {
                firstHide = false;
                Hide();
            }
        }

        private void InitTray()
        {
            trayIcon.Text = Localization.Get("绿帽子安全防护", "绿帽子安全防护");
            trayIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            trayIcon.DoubleClick += (sender, e) =>
            {
                TopMost = true;
                Show();
                Focus();
                TopMost = false;
            };
            trayIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    TopMost = true;
                    AntdUI.ContextMenuStrip.open(this, item => 
                    {
                        if (item.Text.Equals(Localization.Get("打开绿帽子", "打开绿帽子")))
                        {
                            Show();
                            TopMost = false;
                        }
                        else if (item.Text.Equals(Localization.Get("退出绿帽子", "退出绿帽子")))
                        {
                            AntdUI.Modal.open(new Modal.Config(this, Localization.Get("绿帽子安全防护", "绿帽子安全防护"), Localization.Get("您确定要退出绿帽子安全防护吗？\n届时绿帽子将无法保护您的电脑安全，您的电脑存在被恶意程序攻击的风险。", "您确定要退出绿帽子安全防护吗？\n届时绿帽子将无法保护您的电脑安全，您的电脑存在被恶意程序攻击的风险。"))
                            {
                                Icon = TType.Info,
                                CloseIcon = true,
                                Mask = false,
                                OkText = Localization.Get("仍然退出", "仍然退出"),
                                CancelText = Localization.Get("继续保护", "继续保护"),
                                OnOk = config =>
                                {
                                    Engine.Dispose();
                                    Process.GetCurrentProcess().Kill();
                                    return true;
                                }
                            });
                        }
                    }, 
                    new IContextMenuStripItem[] {
                        new ContextMenuStripItem(Localization.Get("打开绿帽子", "打开绿帽子")),
                        new ContextMenuStripItem(Localization.Get("退出绿帽子", "退出绿帽子"))
                    });
                }
            };
            trayIcon.Visible = true;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void InitMonitor()
        {
            if (SysConfig.GetSetting("进程防护").Enabled) ProcessMonitor.Start();
            if (SysConfig.GetSetting("文件防护").Enabled) FileMonitor.Start();
            if (SysConfig.GetSetting("引导防护").Enabled) MbrProtect.Open();
        }

        public void GoToScan(string type)
        {
            segmented.SelectIndex = 1;
            segmented_SelectIndexChanged(null, new IntEventArgs(1));
            ScanView scanView = (ScanView)controls[1];
            if (type.Equals(Localization.Get("快速查杀", "快速查杀"))) scanView.quick_button_Click(null, null);
            else if (type.Equals(Localization.Get("全盘查杀", "全盘查杀"))) scanView.full_button_Click(null, null);
            else if (type.Equals(Localization.Get("目录查杀", "目录查杀"))) scanView.dir_button_Click(null, null);
            else if (type.Equals(Localization.Get("文件查杀", "文件查杀"))) scanView.file_button_Click(null, null);
        }

        private void dropdown_translate_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            LoadLanguage(e.Value.ToString());
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\GreenHat", true);
            if (key != null)
            {
                key.SetValue("lang", e.Value.ToString());
            }
        }

        private void LoadLanguage(string lang = null)
        {
            if (String.IsNullOrEmpty(lang))
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\GreenHat");
                if (key != null && key.GetValue("lang") != null)
                {
                    lang = key.GetValue("lang").ToString();
                }
                else
                {
                    lang = CultureInfo.InstalledUICulture.Name
                        .Replace("zh-TW", "繁体中文")
                        .Replace("en-US", "English")
                        .Replace("ru-RU", "Русский")
                        .Replace("ja-JP", "日本語")
                        .Replace("ko-KR", "한국어");
                }
            }
            switch (lang)
            {
                case "繁体中文":
                    Localization.Provider = new zh_TW();
                    Localization.SetLanguage("zh-TW");
                    break;
                case "English":
                    Localization.Provider = new en_US();
                    Localization.SetLanguage("en-US");
                    break;
                case "Русский":
                    Localization.Provider = new ru_RU();
                    Localization.SetLanguage("ru-RU");
                    break;
                case "日本語":
                    Localization.Provider = new ja_JP();
                    Localization.SetLanguage("ja-JP");
                    break;
                case "한국어":
                    Localization.Provider = new ko_KR();
                    Localization.SetLanguage("ko-KR");
                    break;
                default:
                    Localization.Provider = null;
                    Localization.SetLanguage("zh-CN");
                    break;
            }
            Refresh();
        }

        override public void Refresh()
        {
            titlebar.Text = Localization.Get("绿帽子安全防护", "绿帽子安全防护");
            trayIcon.Text = Localization.Get("绿帽子安全防护", "绿帽子安全防护");
            segmented.Items.ForEach(item =>
            {
                switch (item.ID)
                {
                    case "0":
                        item.Text = Localization.Get("主页", "主页");
                        break;
                    case "1":
                        item.Text = Localization.Get("查杀", "查杀");
                        break;
                    case "2":
                        item.Text = Localization.Get("日志", "日志");
                        break;
                    case "3":
                        item.Text = Localization.Get("设置", "设置");
                        break;
                    case "4":
                        item.Text = Localization.Get("关于", "关于");
                        break;
                }
            });
            foreach (var control in controls)
            {
                control.Refresh();
            }
            base.Refresh();
        }
    }
}
