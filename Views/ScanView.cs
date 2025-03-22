using AntdUI;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private int all;
        private int count;
        private int total;
        private BindingList<ScanTable> tableList = new BindingList<ScanTable>();
        private string type;
        private int totalTime;
        private string curPath;

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
                    Width = "450"
                },
                new Column("Detail", Localization.Get("查杀引擎", "查杀引擎"), ColumnAlign.Center)
                {
                    Width = "100"
                },
                new Column("Type", Localization.Get("病毒类型", "病毒类型"), ColumnAlign.Center)
                {
                    Width = "200"
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
                InitializeScan(list);
            });
        }

        public void full_button_Click(object sender, EventArgs e)
        {
            type = Localization.Get("全盘查杀", "全盘查杀");
            SysConfig.AddLog("病毒防护", "全盘查杀", $"开始时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            scan_timer.Enabled = true;
            cts = new CancellationTokenSource();
            InitializeScan(FileScan.GetDiskList());
        }

        public void dir_button_Click(object sender, EventArgs e)
        {
            AntdUI.FolderBrowserDialog dialog = new AntdUI.FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                InitializeScan(new List<string> { dialog.DirectoryPath });
            }
        }

        public void file_button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = $"{Localization.Get("所有文件", "所有文件")} (*.*)|*.*";
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    InitializeScan(dialog.FileNames.ToList());
                }
            }
        }

        private void InitializeScan(List<string> paths)
        {
            type = Localization.Get("自定义查杀", "自定义查杀");
            SysConfig.AddLog("病毒防护", "自定义查杀", $"开始时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            scan_timer.Enabled = true;

            cts = new CancellationTokenSource();
            pauseEvent = new ManualResetEventSlim(true);

            Task.Run(() => StartScan(paths, cts.Token), cts.Token);
        }

        private void StartScan(List<string> paths, CancellationToken token)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    totalTime = 0;
                    scan_timer.Enabled = true;
                    tableList.Clear();
                    time_label.Text = $"{Localization.Get("已用时间", "已用时间")}：00:00:00";
                    all = 0;
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
                }));

                // 扫描模式
                int processorCount = Environment.ProcessorCount;
                if (performance_button.Text == Localization.Get("效能模式", "效能模式"))
                    processorCount = 1;
                else if (performance_button.Text == Localization.Get("正常模式", "正常模式"))
                    processorCount = Math.Max(1, processorCount / 4);

                // 生产者任务
                var fileQueue = new BlockingCollection<string>();
                Task producer = Task.Run(() =>
                {
                    FileScan.Scan(paths, filePath =>
                    {
                        token.ThrowIfCancellationRequested();
                        fileQueue.Add(filePath, token);
                        Interlocked.Increment(ref all);
                        return true;
                    }, processorCount);
                    fileQueue.CompleteAdding();
                }, token);

                // 消费者任务
                Task consumer = Task.Run(() =>
                {
                    Parallel.ForEach(fileQueue.GetConsumingEnumerable(token),
                        new ParallelOptions { MaxDegreeOfParallelism = processorCount, CancellationToken = token },
                        path =>
                        {
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                pauseEvent.Wait(token);

                                Interlocked.Increment(ref total);
                                curPath = path;
                                if (Engine.IsVirus(path, out string[] result, true))
                                {
                                    Interlocked.Increment(ref count);
                                    Task.Run(() =>
                                    {
                                        lock (tableList)
                                        {
                                            tableList.Add(new ScanTable
                                            {
                                                Path = path,
                                                Engine = result[0],
                                                Type = result[1],
                                                Detail = new CellLink(path, Localization.Get("查看详情", "查看详情")),
                                                State = new CellTag(Localization.Get("待处理", "待处理"), TTypeMini.Error)
                                            });
                                            SysConfig.AddLog("病毒防护", "发现病毒", $"文件：{path}");
                                        }
                                    });
                                }
                            }
                            catch { }
                        });
                }, token);

                Task.WaitAll(producer, consumer);
            }
            catch { }
            finally
            {
                Invoke(new Action(() => FinalizeScan()));
            }
        }

        private void FinalizeScan()
        {
            scan_timer.Enabled = false;
            TimeSpan duration = TimeSpan.FromMilliseconds(totalTime);
            Invoke(new Action(() =>
            {
                quick_button.Enabled = true;
                full_button.Enabled = true;
                custom_button.Enabled = true;
                performance_button.Enabled = true;
                header.Description = $"{Localization.Get("查杀结束", "查杀结束")} {DateTime.Now.ToString()}";
                pause_button.Visible = false;
                continue_button.Visible = false;
                stop_button.Visible = false;
                black_button.Visible = true;
                remove_button.Visible = true;
                stop_button.Loading = false;
                pause_button.Enabled = true;
                continue_button.Enabled = true;
                progress.Value = 1;

                time_label.Text = $"{Localization.Get("已用时间", "已用时间")}：{duration:hh\\:mm\\:ss}";
                header.Description = curPath;
                header.SubText = $"{Localization.Get("已扫描", "已扫描")}：{total}，{Localization.Get("威胁数量", "威胁数量")}：{count}";

                SysConfig.AddLog("病毒防护", type, $"查杀结束，找到 {count} 个威胁，用时：{duration:hh\\:mm\\:ss}");

                Modal.open(new Modal.Config(mainForm, Localization.Get("查杀结束", "查杀结束"), $"{Localization.Get("找到", "找到")} {count} {Localization.Get("个威胁", "个威胁")}，{Localization.Get("用时", "用时")}：{duration:hh\\:mm\\:ss}", TType.Success)
                {
                    CloseIcon = true,
                    BtnHeight = 0,
                    MaskClosable = false
                });
            }));
        }

        private void pause_button_Click(object sender, EventArgs e)
        {
            pauseEvent.Reset();
            pause_button.Visible = false;
            continue_button.Visible = true;
            scan_timer.Enabled = false;
        }

        private void continue_button_Click(object sender, EventArgs e)
        {
            pauseEvent.Set();
            pause_button.Visible = true;
            continue_button.Visible = false;
            scan_timer.Enabled = true;
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            pauseEvent.Reset();
            pause_button.Enabled = false;
            continue_button.Enabled = false;
            stop_button.Loading = true;
            cts?.Cancel();
        }

        private void table_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.RowIndex == 0) return;
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
            performance_button.SelectedValue = $" {Localization.Get("正常模式", "正常模式")}";
        }

        private void custom_button_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            string name = e.Value.ToString().TrimStart();
            if (name.Equals(Localization.Get("目录查杀", "目录查杀"))) dir_button_Click(null, null);
            else if (name.Equals(Localization.Get("文件查杀", "文件查杀"))) file_button_Click(null, null);
            custom_button.SelectedValue = null;
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
            performance_button.Text = Localization.Get("正常模式", "正常模式");
            black_button.Text = Localization.Get("隔离所选", "隔离所选");
            remove_button.Text = Localization.Get("删除所选", "删除所选");
            continue_button.Text = Localization.Get("继续查杀", "继续查杀");
            pause_button.Text = Localization.Get("暂停查杀", "暂停查杀");
            stop_button.Text = Localization.Get("停止查杀", "停止查杀");
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

        private void scan_timer_Tick(object sender, EventArgs e)
        {
            totalTime += 100;
            TimeSpan duration = TimeSpan.FromMilliseconds(totalTime);
            progress.Value = (float)total / (float)all;
            time_label.Text = $"{Localization.Get("已用时间", "已用时间")}：{duration:hh\\:mm\\:ss}";
            header.Description = curPath;
            header.SubText = $"{Localization.Get("已扫描", "已扫描")}：{total}，{Localization.Get("威胁数量", "威胁数量")}：{count}";
        }
    }
}
