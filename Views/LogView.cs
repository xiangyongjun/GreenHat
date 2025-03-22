using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenHat.Views
{
    public partial class LogView : UserControl
    {
        private MainWindow mainForm;
        private BindingList<LogTable> tableList;

        public LogView(MainWindow mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            InitTableColumns();
            Task.Run(InitTableData);
        }

        private void InitTableColumns()
        {
            table.Columns = new ColumnCollection() {
                new Column("Time", Localization.Get("时间", "时间"), ColumnAlign.Center)
                {
                    Width = "150"
                },
                new Column("Type", Localization.Get("类别", "类别"), ColumnAlign.Center)
                {
                    Width = "150"
                },
                new Column("Func", Localization.Get("功能", "功能"), ColumnAlign.Center)
                {
                    Width = "150"
                },
                new Column("Desc", Localization.Get("概要", "概要"), ColumnAlign.Left)
                {
                    Width = "462"
                }
            };
        }

        public void InitTableData()
        {
            List<Log> logList = date_range.Value?.Length == 2 ?
                SysConfig.GetLogList(type_select.SelectedValue.ToString(), date_range.Value[0], date_range.Value[1])
                : SysConfig.GetLogList(type_select.SelectedValue.ToString());
            tableList = new BindingList<LogTable>();
            foreach (Log item in logList)
            {
                tableList.Add(new LogTable()
                {
                    Time = item.Time.ToString(),
                    Type = Localization.Get(item.Type, item.Type),
                    Func = item.Func,
                    Desc = item.Desc
                });
            }
            table.Binding(tableList);
        }

        private void date_range_ValueChanged(object sender, DateTimesEventArgs e)
        {
            InitTableData();
        }

        private void type_select_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            InitTableData();
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            SysConfig.ClearLog();
            InitTableData();
            AntdUI.Message.success(mainForm, $"{Localization.Get("清空成功", "清空成功")}！", autoClose: 3);
        }

        private void export_button_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = $"{Localization.Get("CSV文件", "CSV文件")} (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                StreamWriter file = new StreamWriter(fileName);
                file.WriteLine($"{Localization.Get("时间", "时间")},{Localization.Get("类别", "类别")},{Localization.Get("功能", "功能")},{Localization.Get("概要", "概要")}");
                foreach (LogTable item in tableList)
                {
                    file.WriteLine($"{item.Time},{item.Type},{item.Func},{item.Desc}");
                }
                file.Close();
                AntdUI.Message.success(mainForm, $"{Localization.Get("导出成功", "导出成功")}！", autoClose: 3);
            }
        }

        public override void Refresh()
        {
            header.Text = Localization.Get("安全日志", "安全日志");
            date_range.PlaceholderStart = Localization.Get("开始日期", "开始日期");
            date_range.PlaceholderEnd = Localization.Get("结束日期", "结束日期");
            export_button.Text = Localization.Get("导出日志", "导出日志");
            clear_button.Text = Localization.Get("清空日志", "清空日志");
            type_select.Text = Localization.Get("全部", "全部");
            type_select.Items.Clear();
            type_select.Items.Add(Localization.Get("全部", "全部"));
            type_select.Items.Add(Localization.Get("病毒防护", "病毒防护"));
            type_select.Items.Add(Localization.Get("其他", "其他"));
            table.EmptyText = Localization.Get("暂无数据", "暂无数据");
            InitTableColumns();
            Task.Run(InitTableData);
            base.Refresh();
        }
    }
}
