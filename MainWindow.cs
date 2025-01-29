using AntdUI;
using GreenHat.Utils;
using GreenHat.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GreenHat
{
    public partial class MainWindow : Window
    {
        private bool isLight = true;
        private List<UserControl> controls = new List<UserControl>();
        private bool firstHide = false;

        public MainWindow(bool hide = false)
        {
            firstHide = hide;
            SysConfig.AddLog("其他", "程序启动", $"操作时间：{DateTime.Now.ToString()}");
            Engine.Init();
            InitializeComponent();
            CheckEngine();
            CheckService();
            InitView();
            InitData();
            BindEventHandler();
            GetVersion();
            InitTray();
            InitMonitor();
        }

        private void CheckEngine()
        {
            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}engine"))
            {
                AntdUI.Modal.open(new Modal.Config(this, "警告", "检测到本地引擎丢失，防护功能将会失效！", TType.Warn)
                {
                    CloseIcon = true,
                    BtnHeight = 0
                });
            }
        }

        private void CheckService()
        {
            if (SysConfig.GetSetting("开机启动").Enabled || Tools.ServiceExists("GreenHatService")) return;
            Tools.CreateAndStartService("GreenHatService", $"{AppDomain.CurrentDomain.BaseDirectory}\\GreenHatService.exe");
        }

        private void InitData()
        {
            isLight = ThemeHelper.IsLightMode();
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
            segmented_SelectIndexChanged(null, new IntEventArgs(0));
            if (firstHide)
            {
                firstHide = false;
                Hide();
            }
        }

        private void InitTray()
        {
            NotifyIcon trayIcon;
            trayIcon = new NotifyIcon();
            trayIcon.Text = "绿帽子安全防护";
            trayIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            trayIcon.DoubleClick += (sender, e) =>
            {
                Show();
                Focus();
            };
            trayIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    AntdUI.ContextMenuStrip.open(this, item => 
                    {
                        switch (item.Text)
                        {
                            case "打开绿帽子":
                                Show();
                                break;
                            case "退出绿帽子":
                                AntdUI.Modal.open(new Modal.Config(this, "绿帽子安全防护", "您确定要退出绿帽子安全防护吗？\n届时绿帽子将无法保护您的电脑安全，您的电脑存在被恶意程序攻击的风险。")
                                {
                                    Icon = TType.Info,
                                    CloseIcon = true,
                                    Mask = false,
                                    OkText = "仍然退出",
                                    CancelText = "继续保护",
                                    OnOk = config =>
                                    {
                                        Engine.Dispose();
                                        Process.GetCurrentProcess().Kill();
                                        return true;
                                    }
                                });
                                break;
                        }
                    }, 
                    new IContextMenuStripItem[] {
                        new ContextMenuStripItem("打开绿帽子"),
                        new ContextMenuStripItem("退出绿帽子")
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
            switch (type)
            {
                case "快速查杀":
                    scanView.quick_button_Click(null, null);
                    break;
                case "全盘查杀":
                    scanView.full_button_Click(null, null);
                    break;
                case "自定义查杀":
                    scanView.custom_button_Click(null, null);
                    break;
            }
        }
    }
}
