using AntdUI;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                new Column("Path", Localization.Get("文件路径", "文件路径"), ColumnAlign.Left)
                {
                    Width = "410"
                },
                new Column("Detail", Localization.Get("查杀引擎", "查杀引擎"), ColumnAlign.Center)
                {
                    Width = "100"
                },
                new Column("Type", Localization.Get("病毒类型", "病毒类型"), ColumnAlign.Center)
                {
                    Width = "240"
                },
                new Column("State", Localization.Get("状态", "状态"), ColumnAlign.Center)
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
            type = Localization.Get("快速查杀", "快速查杀");
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
            type = Localization.Get("全盘查杀", "全盘查杀");
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
                type = Localization.Get("自定义查杀", "自定义查杀");
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
            dialog.Filter = $"{Localization.Get("所有文件", "所有文件")} (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<string> list = new List<string>();
                type = Localization.Get("自定义查杀", "自定义查杀");
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
            time_label.Text = $"{Localization.Get("已用时间", "已用时间")}：00:00:00";
            time = 0;
            count = 0;
            total = 0;
            quick_button.Enabled = false;
            full_button.Enabled = false;
            custom_button.Enabled = false;
            performance_button.Enabled = false;
            remove_button.Visible = false;
            black_button.Visible = false;
            stop_button.Visible = true;
            pause_button.Visible = true;
            int processorCount = Environment.ProcessorCount;
            if (performance_button.Text.Equals(Localization.Get("效能模式", "效能模式"))) processorCount = 1;
            else if (performance_button.Text.Equals(Localization.Get("正常模式", "正常模式"))) processorCount = processorCount / 2;
            FileScan.Scan(paths, path =>
                {
                    pauseEvent.Wait(cts.Token);
                    if (cts.Token.IsCancellationRequested) return false;
                    total++;
                    header.Description = path;
                    string[] result;
                    header.SubText = $"{Localization.Get("已扫描", "已扫描")}：{total}，{Localization.Get("威胁数量", "威胁数量")}：{count}";
                    if (Engine.IsVirus(path, out result, true))
                    {
                        try
                        {
                            tableList.Add(new ScanTable()
                            {
                                Path = path,
                                Engine = result[0],
                                Type = result[1],
                                Detail = new CellLink(path, Localization.Get("查看详情", "查看详情")),
                                State = new CellTag(Localization.Get("待处理", "待处理"), TTypeMini.Error)
                            });
                            count++;
                            header.SubText = $"{Localization.Get("已扫描", "已扫描")}：{total}，{Localization.Get("威胁数量", "威胁数量")}：{count}";
                        }
                        catch { }
                    }
                    return true;
                }, processorCount);
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
            performance_button.Enabled = true;
            header.Description = $"{Localization.Get("查杀结束", "查杀结束")} {DateTime.Now.ToString()}";
            scan_timer.Enabled= false;
            pause_button.Visible = true;
            continue_button.Visible = false;
            pause_button.Visible = false;
            stop_button.Visible = false;
            black_button.Visible = true;
            remove_button.Visible = true;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(time);
            SysConfig.AddLog("病毒防护", type, $"查杀结束，找到 {count} 个威胁，用时：{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
            Modal.open(new Modal.Config(mainForm, Localization.Get("查杀结束", "查杀结束"), $"{Localization.Get("找到", "找到")} {count} {Localization.Get("个威胁", "个威胁")}，{Localization.Get("用时", "用时")}：{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}", TType.Success)
            {
                CloseIcon = true,
                BtnHeight = 0
            });
        }

        private void scan_timer_Tick(object sender, EventArgs e)
        {
            time += 1000;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(time);
            time_label.Text = $"{Localization.Get("已用时间", "已用时间")}：{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
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
                    Modal.open(new Modal.Config(mainForm, Localization.Get("查杀引擎", "查杀引擎"), scanTable.Engine, TType.Info)
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
                    if (item.Selected && item.State.Text.Equals(Localization.Get("待处理", "待处理")))
                    {
                        SysConfig.AddBlack(item.Path, item.Type);
                        item.State.Text = Localization.Get("已隔离", "已隔离");
                        item.State.Type = TTypeMini.Warn;
                    }
                }
                AntdUI.Message.success(mainForm, $"{Localization.Get("隔离完毕", "隔离完毕")}！", autoClose: 3);
            });
        }

        private void remove_button_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                foreach (ScanTable item in tableList)
                {
                    if (item.Selected && item.State.Text.Equals(Localization.Get("待处理", "待处理")))
                    {
                        SysConfig.AddLog("其他", "删除查杀文件", $"操作时间：{DateTime.Now.ToString()}");
                        File.Delete(item.Path);
                        item.State.Text = Localization.Get("已删除", "已删除");
                        item.State.Type = TTypeMini.Success;
                    }
                }
                AntdUI.Message.success(mainForm, $"{Localization.Get("删除完毕", "删除完毕")}！", autoClose: 3);
            });
        }

        private void ScanView_Load(object sender, EventArgs e)
        {
            custom_button.Items.Clear();
            custom_button.Items.AddRange(new SelectItem[] {
                new SelectItem($" {Localization.Get("目录查杀", "目录查杀")}"){
                    Online = 1
                },
                new SelectItem($" {Localization.Get("文件查杀", "文件查杀")}"){
                    Online = 1
                }
            });
            performance_button.Items.Clear();
            performance_button.Items.AddRange(new SelectItem[] {
                new SelectItem($" {Localization.Get("效能模式", "效能模式")}"){
                    Online = 1,
                    OnlineCustom = Color.FromArgb(220, 68, 70)
                },
                new SelectItem($" {Localization.Get("正常模式", "正常模式")}"){
                    Online = 1,
                    OnlineCustom = Color.FromArgb(76, 175, 80)
                },
                new SelectItem($" {Localization.Get("性能模式", "性能模式")}"){
                    Online = 1,
                    OnlineCustom = Color.FromArgb(22, 119, 255)
                }
            });
        }

        private void custom_button_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            string name = e.Value.ToString().TrimStart();
            if (name.Equals(Localization.Get("目录查杀", "目录查杀"))) dir_button_Click(null, null);
            else if (name.Equals(Localization.Get("文件查杀", "文件查杀"))) file_button_Click(null, null);
        }

        private void performance_button_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            performance_button.Text = e.Value.ToString().TrimStart();
        }

        public override void Refresh()
        {
            header.Text = Localization.Get("病毒查杀", "病毒查杀");
            header.SubText = "";
            header.Description = "";
            quick_button.Text = Localization.Get("快速查杀", "快速查杀");
            full_button.Text = Localization.Get("全盘查杀", "全盘查杀");
            custom_button.Text = Localization.Get("自定义查杀", "自定义查杀");
            performance_button.Text = Localization.Get("性能模式", "性能模式");
            black_button.Text = Localization.Get("隔离所选", "隔离所选");
            remove_button.Text = Localization.Get("删除所选", "删除所选");
            continue_button.Text = Localization.Get("继续查杀", "继续查杀");
            pause_button.Text = Localization.Get("暂停查杀", "暂停查杀");
            time_label.Text = "";
            table.EmptyText = Localization.Get("暂无数据", "暂无数据");
            ScanView_Load(null, null);
            InitTableColumns();
            Task.Run(() =>
            {
                foreach (var scan in tableList)
                {
                    scan.Detail.Text = Localization.Get("查看详情", "查看详情");
                }
            });
            base.Refresh();
        }
    }
}
