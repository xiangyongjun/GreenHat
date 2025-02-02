using AntdUI;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenHat.Views
{
    public partial class ScanView : UserControl
    {
        private MainWindow mainForm;
        private CancellationTokenSource cts;
        private ManualResetEventSlim pauseEvent;
        private int time = 0;
        private int count = 0;
        private int total = 0;
        private AntList<ScanTable> tableList = new AntList<ScanTable>();
        private string type;

        public ScanView(MainWindow mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            InitTableColumns();
            InitTableData();       
        }

        private void InitTableColumns()
        {
            table.Columns = new ColumnCollection() {
                new Column("Path", "文件路径", ColumnAlign.Left)
                {
                    Width = "410"
                },
                new Column("Detail", "查杀引擎", ColumnAlign.Center)
                {
                    Width = "100"
                },
                new Column("Type", "病毒类型", ColumnAlign.Center)
                {
                    Width = "300"
                },
                new Column("State", "状态", ColumnAlign.Center)
                {
                    Width = "100"
                },
            };
        }

        private void InitTableData()
        {
            table.Binding(tableList);
        }

        public void quick_button_Click(object sender, EventArgs e)
        {
            type = "快速查杀";
            SysConfig.AddLog("病毒防护", "快速查杀", $"开始时间：{DateTime.Now.ToString()}");
            scan_timer.Enabled = true;
            Task.Run(() =>
            {
                cts = new CancellationTokenSource();
                List<string> list = new List<string>();
                list.AddRange(FileScan.GetStartupList());
                list.AddRange(FileScan.GetProcessList());
                list.AddRange(FileScan.GetServiceList());
                list.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
                list.Add(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
                list.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                list.Add(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                Task.Run(() => StartScan(list), cts.Token);
            });
        }

        public void full_button_Click(object sender, EventArgs e)
        {
            type = "全盘查杀";
            SysConfig.AddLog("病毒防护", "全盘查杀", $"开始时间：{DateTime.Now.ToString()}");
            scan_timer.Enabled = true;
            Task.Run(() =>
            {
                cts = new CancellationTokenSource();
                Task.Run(() => StartScan(FileScan.GetDiskList()), cts.Token);
            });
        }

        public void custom_button_Click(object sender, EventArgs e)
        {
            AntdUI.FolderBrowserDialog dialog = new AntdUI.FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<string> list = new List<string>();
                type = "自定义查杀";
                SysConfig.AddLog("病毒防护", "自定义查杀", $"开始时间：{DateTime.Now.ToString()}");
                scan_timer.Enabled = true;
                list.Add(dialog.DirectoryPath);
                Task.Run(() =>
                {
                    cts = new CancellationTokenSource();
                    Task.Run(() => StartScan(list), cts.Token);
                });
            }
        }

        private void StartScan(List<string> paths)
        {
            if (!quick_button.Enabled) return;
            tableList.Clear();
            pauseEvent = new ManualResetEventSlim(true);
            time_label.Text = "已用时间：00:00:00";
            time = 0;
            count = 0;
            total = 0;
            quick_button.Enabled = false;
            full_button.Enabled = false;
            custom_button.Enabled = false;
            stop_button.Visible = true;
            pause_button.Visible = true;
            FileScan.Scan(paths, path =>
            {
                pauseEvent.Wait(cts.Token);
                if (cts.Token.IsCancellationRequested) return false;
                total++;
                header.Description = path;
                string[] result;
                header.SubText = $"已扫描：{total}，威胁数量：{count}";
                if (Engine.IsVirus(path, out result, true))
                {
                    try
                    {
                        SysConfig.AddBlack(path, result[1]);
                        tableList.Add(new ScanTable()
                        {
                            Path = path,
                            Engine = result[0],
                            Type = result[1],
                            Detail = new CellLink(path, "查看详情"),
                            State = new CellTag("已隔离", TTypeMini.Error)
                        });
                        count++;
                        header.SubText = $"已扫描：{total}，威胁数量：{count}";
                    }
                    catch { }
                }
                return true;
            });
            if (!cts.IsCancellationRequested) stop_button_Click(null, null);
        }

        private void pause_button_Click(object sender, EventArgs e)
        {
            pauseEvent.Reset();
            pause_button.Visible = false;
            continue_button.Visible = true;
        }

        private void continue_button_Click(object sender, EventArgs e)
        {
            pauseEvent.Set();
            pause_button.Visible = true;
            continue_button.Visible = false;
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            quick_button.Enabled = true;
            full_button.Enabled = true;
            custom_button.Enabled = true;
            header.Description = $"查杀结束 {DateTime.Now.ToString()}";
            scan_timer.Enabled= false;
            pause_button.Visible = true;
            continue_button.Visible = false;
            pause_button.Visible = false;
            stop_button.Visible = false;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(time);
            SysConfig.AddLog("病毒防护", type, $"查杀结束，找到 {count} 个威胁，用时：{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
            Modal.open(new Modal.Config(mainForm, "查杀完成", $"找到 {count} 个威胁，用时：{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}", TType.Success)
            {
                CloseIcon = true,
                BtnHeight = 0
            });
        }

        private void scan_timer_Tick(object sender, EventArgs e)
        {
            time += 1000;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(time);
            time_label.Text = $"已用时间：{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        private void table_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.ColumnIndex != 1) return;
            ScanTable scanTable = (ScanTable)e.Record;
            Modal.open(new Modal.Config(mainForm, "查杀引擎", scanTable.Engine, TType.Info)
            {
                CloseIcon = true,
                BtnHeight = 0
            });
        }
    }
}
