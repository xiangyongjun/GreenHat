using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Utils;
using Hardware.Info;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net.Http;
using System.Net.Http.Headers;
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
        }

        private void Home_Load(object sender, EventArgs e)
        {
            scan_button.Items.Clear();
            scan_button.Items.AddRange(new SelectItem[] {
                new SelectItem(" 快速查杀"){
                    Online = 1
                },
                new SelectItem(" 全盘查杀"){
                    Online = 1
                },
                new SelectItem(" 目录查杀"){
                    Online = 1
                },
                new SelectItem(" 文件查杀"){
                    Online = 1
                }
            });
            UpdateCount();
        }

        private void InitTableColumns()
        {
            table.Columns = new ColumnCollection() {
                new Column("Name", "名称", ColumnAlign.Left)
                {
                    Width = "150"
                },
                new Column("Desc", "描述", ColumnAlign.Left)
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
            AntList<SysTable> list = new AntList<SysTable>();
            list.Add(new SysTable() { 
                Name = "操作系统",
                Desc = hardwareInfo.OperatingSystem.Name
            });
            list.Add(new SysTable()
            {
                Name = "处理器",
                Desc = $"{hardwareInfo.CpuList[0].Name} {hardwareInfo.CpuList[0].NumberOfCores}核 {hardwareInfo.CpuList[0].NumberOfLogicalProcessors}线程"
            });
            list.Add(new SysTable()
            {
                Name = "主板",
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
                Name = "内存",
                Desc = $"{hardwareInfo.MemoryList[0].Manufacturer} DDR{ddr} {hardwareInfo.MemoryList[0].Speed}Mhz {totalMem.ToString()}GB"
            });
            list.Add(new SysTable()
            {
                Name = "硬盘",
                Desc = $"{hardwareInfo.DriveList[0].Model} {hardwareInfo.DriveList[0].Size / 1024 / 1024 / 1024}GB"
            });
            list.Add(new SysTable()
            {
                Name = "显卡",
                Desc = $"{hardwareInfo.VideoControllerList[0].Name} {hardwareInfo.VideoControllerList[0].AdapterRAM / 1024 / 1024 / 1024}GB"
            });
            list.Add(new SysTable()
            {
                Name = "显示器",
                Desc = $"{hardwareInfo.MonitorList[0].ManufacturerName} {hardwareInfo.MonitorList[0].UserFriendlyName}"
            });
            table.Binding(list);
        }

        private void white_button_Click(object sender, EventArgs e)
        {
            Modal.open(new Modal.Config(mainForm, "信任区", new WhiteView(mainForm))
            {
                CloseIcon = true,
                BtnHeight = 0,
                MaskClosable = false,
                Padding = new Size(15, 15)
            });
        }

        private void black_button_Click(object sender, EventArgs e)
        {
            Modal.open(new Modal.Config(null, "隔离区", new BlackView(mainForm))
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
            header.Text = $"已保护 {daysDifference + 1} 天";
            key.Close();
        }

        private void scan_button_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            mainForm.GoToScan(e.Value.ToString().Replace(" ", ""));
        }

        public void UpdateCount()
        {
            count_label.Text = $"隔离数量：{SysConfig.CountBlack()}个";
        }

        private void update_button_Click(object sender, EventArgs e)
        {
            Modal.open(new Modal.Config(null, "检查更新", new UpdateView(mainForm))
            {
                CloseIcon = true,
                BtnHeight = 0,
                MaskClosable = false,
                Padding = new Size(15, 15)
            });
        }

        private void github_button_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/xiangyongjun/GreenHat",
                UseShellExecute = true
            });
        }

        private void cloud_button_Click(object sender, EventArgs e)
        {
            new CloudMarkForm().ShowDialog();
        }
    }
}
