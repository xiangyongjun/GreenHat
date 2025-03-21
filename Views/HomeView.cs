using AntdUI;
using GreenHat.Models;
using GreenHat.Utils;
using Hardware.Info;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenHat.Views
{
    public partial class HomeView : UserControl
    {
        private MainWindow mainForm;

        public HomeView(MainWindow mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            InitTableColumns();
            GetProtectDays();
            Task.Run(InitTableData);
            update_timer_Tick(null, null);
            Task.Run(UpdateEngine);
        }

        private void Home_Load(object sender, EventArgs e)
        {
            engine_label.Text = $"{Localization.Get("模型版本", "模型版本")}：{DateTimeOffset.FromUnixTimeSeconds(Engine.GetGreenHatModelVersion() + 28800).ToString("yyyy-MM-dd HH:mm")}";
            scan_button.Items.Clear();
            scan_button.Items.AddRange(new SelectItem[] {
                new SelectItem($" {Localization.Get("快速查杀", "快速查杀")}"){
                    Online = 1
                },
                new SelectItem($" {Localization.Get("全盘查杀", "全盘查杀")}"){
                    Online = 1
                },
                new SelectItem($" {Localization.Get("目录查杀", "目录查杀")}"){
                    Online = 1
                },
                new SelectItem($" {Localization.Get("文件查杀", "文件查杀")}"){
                    Online = 1
                }
            });
        }

        private void InitTableColumns()
        {
            table.Columns = new ColumnCollection() {
                new Column("Name", Localization.Get("名称", "名称"), ColumnAlign.Left)
                {
                    Width = "150"
                },
                new Column("Desc", Localization.Get("描述", "描述"), ColumnAlign.Left)
                {
                    Width = "745"
                }
            };
        }

        private void InitTableData()
        {
            HardwareInfo hardwareInfo = new HardwareInfo();
            hardwareInfo.RefreshOperatingSystem();
            hardwareInfo.RefreshCPUList();
            hardwareInfo.RefreshMotherboardList();
            hardwareInfo.RefreshMemoryList();
            hardwareInfo.RefreshDriveList();
            hardwareInfo.RefreshVideoControllerList();
            hardwareInfo.RefreshMonitorList();
            BindingList<SysTable> list = new BindingList<SysTable>();
            list.Add(new SysTable() { 
                Name = Localization.Get("操作系统", "操作系统"),
                Desc = hardwareInfo.OperatingSystem.Name
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("处理器", "处理器"),
                Desc = $"{hardwareInfo.CpuList[0].Name} {hardwareInfo.CpuList[0].NumberOfCores}{Localization.Get("核", "核")} {hardwareInfo.CpuList[0].NumberOfLogicalProcessors}{Localization.Get("线程", "线程")}"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("主板", "主板"),
                Desc = $"{hardwareInfo.MotherboardList[0].Manufacturer} {hardwareInfo.MotherboardList[0].Product}"
            });
            int totalMem = 0;
            hardwareInfo.MemoryList.ForEach(mem =>
            {
                totalMem += (int)(hardwareInfo.MemoryList[0].Capacity / 1024 / 1024 / 1024);
            });
            string ddr = "";
            if (hardwareInfo.MemoryList[0].Speed >= 4800) ddr = "5";
            else if (hardwareInfo.MemoryList[0].Speed > 2400) ddr = "4";
            else if (hardwareInfo.MemoryList[0].Speed > 800) ddr = "3";
            else if (hardwareInfo.MemoryList[0].Speed > 400) ddr = "2";
            list.Add(new SysTable()
            {
                Name = Localization.Get("内存", "内存"),
                Desc = $"{hardwareInfo.MemoryList[0].Manufacturer} DDR{ddr} {hardwareInfo.MemoryList[0].Speed}Mhz {totalMem.ToString()}GB"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("硬盘", "硬盘"),
                Desc = $"{hardwareInfo.DriveList[0].Model} {hardwareInfo.DriveList[0].Size / 1024 / 1024 / 1024}GB"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("显卡", "显卡"),
                Desc = $"{hardwareInfo.VideoControllerList[0].Name} {Math.Ceiling((double)hardwareInfo.VideoControllerList[0].AdapterRAM / 1024 / 1024 / 1024)}GB"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("显示器", "显示器"),
                Desc = $"{hardwareInfo.MonitorList[0].ManufacturerName} {hardwareInfo.MonitorList[0].UserFriendlyName}"
            });
            table.Binding(list);
        }

        private void white_button_Click(object sender, EventArgs e)
        {
            Modal.open(new Modal.Config(mainForm, Localization.Get("信任区", "信任区"), new WhiteView(mainForm))
            {
                CloseIcon = true,
                BtnHeight = 0,
                MaskClosable = false,
                Padding = new Size(15, 15)
            });
        }

        private void black_button_Click(object sender, EventArgs e)
        {
            Modal.open(new Modal.Config(null, Localization.Get("隔离区", "隔离区"), new BlackView(mainForm))
            {
                CloseIcon = true,
                BtnHeight = 0,
                MaskClosable = false,
                Padding = new Size(15, 15)
            });
        }

        private static float GetCpuUsage()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return float.Parse(obj["PercentProcessorTime"].ToString());
                }
            }
            return 0;
        }

        private static float GetMemoryUsage()
        {
            PerformanceCounter performanceCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            float memoryUsage = performanceCounter.NextValue();
            return memoryUsage;
        }

        private static float GetDiskUsage()
        {
            float result = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 3");
            foreach (ManagementObject mo in searcher.Get())
            {
                long size = Convert.ToInt64(mo["Size"]);
                long freeSpace = Convert.ToInt64(mo["FreeSpace"]);
                float usage = (float)(size - freeSpace) / size * 100;
                result += usage;
            }
            return result / searcher.Get().Count;
        }

        private void UpdateUsage()
        {
            cpu_progress.Value = GetCpuUsage() / 100;
            mem_progress.Value = GetMemoryUsage() / 100;
            disk_progress.Value = GetDiskUsage() / 100;
        }

        private void update_timer_Tick(object sender, EventArgs e)
        {
            Task.Run(UpdateUsage);
        }

        private void GetProtectDays()
        {
            string registryPath = @"Software\GreenHat";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true);
            if (key == null || key.GetValue("installTime") == null)
            {
                key = Registry.CurrentUser.CreateSubKey(registryPath);
                key.SetValue("installTime", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'"));
            }
            DateTime creationTime = DateTime.Parse(key.GetValue("installTime").ToString());
            TimeSpan timeDifference = DateTime.Now - creationTime;
            int daysDifference = timeDifference.Days;
            header.Text = $"{Localization.Get("已保护", "已保护")} {daysDifference + 1} {Localization.Get("天", "天")}";
            header.Description = Localization.Get("绿帽子安全防护正在保护您的电脑安全", "绿帽子安全防护正在保护您的电脑安全");
            key.Close();
        }

        private void scan_button_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            mainForm.GoToScan(e.Value.ToString().TrimStart());
            scan_button.SelectedValue = null;
        }

        private async void update_button_Click(object sender, EventArgs e)
        {
            try
            {
                update_button.Loading = true;
                await UpdateEngine();
                string temp = await GetLatestVersionAsync();
                Assembly assembly = Assembly.GetExecutingAssembly();
                Version oldVersion = assembly.GetName().Version;
                Version newVersion = Version.Parse((string.IsNullOrEmpty(temp) ? oldVersion.ToString() : temp));
                bool hasNew = false;
                string content = $"\n{Localization.Get("当前", "当前")} {oldVersion} {Localization.Get("版本已是最新", "版本已是最新")}！";
                if (newVersion.CompareTo(oldVersion) > 0)
                {
                    hasNew = true;
                    content = $"\n{Localization.Get("当前最新版本为", "当前最新版本为")} {newVersion}，{Localization.Get("是否前去官网下载升级", "是否前去官网下载升级")}？";
                }
                Modal.open(new Modal.Config(Localization.Get("检查更新", "检查更新"), content)
                {
                    Icon = hasNew ? TType.Info : TType.Success,
                    Mask = true,
                    CloseIcon = true,
                    MaskClosable = false,
                    Padding = new Size(15, 15),
                    CancelText = Localization.Get("取消", "取消"),
                    OkText = Localization.Get("确定", "确定"),
                    OkType = TTypeMini.Success,
                    OnOk = config =>
                    {
                        if (hasNew) website_button_Click(null, null);
                        return true;
                    }
                });
            }
            finally
            {
                update_button.Loading = false;
            }
        }

        private void website_button_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://greenhat.icu/",
                UseShellExecute = true
            });
        }

        private void cloud_button_Click(object sender, EventArgs e)
        {
            new CloudMarkForm().ShowDialog();
        }

        private async Task<string> GetLatestVersionAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://greenhat.icu/download/version.json";
                    client.DefaultRequestHeaders.Add("User-Agent", "GreenHat");
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject versionInfo = JObject.Parse(json);
                        return versionInfo["version"].ToString().ToLower().Replace("v", "");
                    }
                }
            }
            catch { }
            return null;
        }

        private async Task UpdateEngine()
        {
            await Engine.UpdateGreenHatEngine();
            await Task.Delay(100);
            engine_label.Text = $"{Localization.Get("模型版本", "模型版本")}：{DateTimeOffset.FromUnixTimeSeconds(Engine.GetGreenHatModelVersion() + 28800).ToString("yyyy-MM-dd HH:mm")}";
        }

        public override void Refresh()
        {
            GetProtectDays();
            scan_button.Text = Localization.Get("开始查杀", "开始查杀");
            cloud_button.Text = Localization.Get("文件云鉴定器", "文件云鉴定器");
            website_button.Text = Localization.Get("官方主页", "官方主页");
            update_button.Text = Localization.Get("检查更新", "检查更新");
            white_button.Text = Localization.Get("信任区", "信任区");
            black_button.Text = Localization.Get("隔离区", "隔离区");
            label2.Text = Localization.Get("处理器占用", "处理器占用");
            label3.Text = Localization.Get("内存占用", "内存占用");
            label4.Text = Localization.Get("硬盘占用", "硬盘占用");
            table.EmptyText = Localization.Get("加载中", "加载中");
            Task.Run(() => Home_Load(null, null));
            Task.Run(InitTableColumns);
            Task.Run(InitTableData);
            base.Refresh();
        }
    }
}
