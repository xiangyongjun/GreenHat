using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GreenHat.Views
{
    public partial class LogView : UserControl
    {
        private MainWindow mainForm;
        private AntList<LogTable> tableList;

        public LogView(MainWindow mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            InitTableColumns();
            InitTableData();
        }

        private void InitTableColumns()
        {
            table.Columns = new ColumnCollection() {
                new Column("Time", "时间", ColumnAlign.Center)
                {
                    Width = "150"
                },
                new Column("Type", "类别", ColumnAlign.Center)
                {
                    Width = "150"
                },
                new Column("Func", "功能", ColumnAlign.Center)
                {
                    Width = "150"
                },
                new Column("Desc", "概要", ColumnAlign.Left)
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
            tableList = new AntList<LogTable>();
            foreach (Log item in logList)
            {
                tableList.Add(new LogTable()
                {
                    Time = item.Time.ToString(),
                    Type = item.Type,
                    Func = item.Func,
                    Desc = item.Desc
                });
            }
            table.Binding(tableList);
            count_label.Text = $"日志条数：{tableList.Count}";
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
            AntdUI.Message.success(mainForm, "清空成功！", autoClose: 3);
        }

        private void export_button_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV文件 (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                StreamWriter file = new StreamWriter(fileName);
                file.WriteLine("时间,类别,功能,概要");
                foreach (LogTable item in tableList)
                {
                    file.WriteLine($"{item.Time},{item.Type},{item.Func},{item.Desc}");
                }
                file.Close();
                AntdUI.Message.success(mainForm, "导出成功！", autoClose: 3);
            }
        }
    }
}
