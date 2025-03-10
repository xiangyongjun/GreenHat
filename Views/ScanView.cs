using AntdUI;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.IO;
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
                new ColumnCheck("Selected")
                {
                    Fixed = true,
                    Width = "60"
                },
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
                    Width = "240"
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

        public void dir_button_Click(object sender, EventArgs e)
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

        public void file_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "所有文件 (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<string> list = new List<string>();
                type = "自定义查杀";
                SysConfig.AddLog("病毒防护", "自定义查杀", $"开始时间：{DateTime.Now.ToString()}");
                scan_timer.Enabled = true;
                foreach (var fileName in dialog.FileNames)
                {
                    list.Add(fileName);
                }
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
            dir_button.Enabled = false;
            file_button.Enabled = false;
            remove_button.Visible = false;
            black_button.Visible = false;
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
                        tableList.Add(new ScanTable()
                        {
                            Path = path,
                            Engine = result[0],
                            Type = result[1],
                            Detail = new CellLink(path, "查看详情"),
                            State = new CellTag("待处理", TTypeMini.Error)
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
            dir_button.Enabled = true;
            file_button.Enabled = true;
            header.Description = $"查杀结束 {DateTime.Now.ToString()}";
            scan_timer.Enabled= false;
            pause_button.Visible = true;
            continue_button.Visible = false;
            pause_button.Visible = false;
            stop_button.Visible = false;
            black_button.Visible = true;
            remove_button.Visible = true;
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
            if (e.ColumnIndex == 0) return;
            ScanTable scanTable = (ScanTable)e.Record;
            switch (e.ColumnIndex)
            {
                case 1:
                    Tools.OpenFileInExplorer(scanTable.Path);
                    break;
                case 2:
                    Modal.open(new Modal.Config(mainForm, "查杀引擎", scanTable.Engine, TType.Info)
                    {
                        CloseIcon = true,
                        BtnHeight = 0
                    });
                    break;
            }
        }

        private void black_button_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                foreach (ScanTable item in tableList)
                {
                    if (item.Selected && item.State.Text.Equals("待处理"))
                    {
                        SysConfig.AddBlack(item.Path, item.Type);
                        item.State.Text = "已隔离";
                        item.State.Type = TTypeMini.Warn;
                    }
                }
                AntdUI.Message.success(mainForm, "隔离完毕！", autoClose: 3);
            });
        }

        private void remove_button_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                foreach (ScanTable item in tableList)
                {
                    if (item.Selected && item.State.Text.Equals("待处理"))
                    {
                        SysConfig.AddLog("其他", "删除查杀文件", $"操作时间：{DateTime.Now.ToString()}");
                        File.Delete(item.Path);
                        item.State.Text = "已删除";
                        item.State.Type = TTypeMini.Success;
                    }
                }
                AntdUI.Message.success(mainForm, "删除完毕！", autoClose: 3);
            });
        }
    }
}
