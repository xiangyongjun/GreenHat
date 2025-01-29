using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenHat.Views
{
    public partial class BlackView : UserControl
    {
        private MainWindow mainForm;
        AntList<BlackTable> blackTable;

        public BlackView(MainWindow mainForm)
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
                    Width = "425"
                },
                new Column("Type", "类型", ColumnAlign.Center)
                {
                    Width = "240"
                },
                new Column("Time", "隔离时间", ColumnAlign.Center)
                {
                    Width = "200"
                }
            };
        }

        private void InitTableData()
        {
            List<Black> blackList = SysConfig.GetBlackList(path_input.Text);
            blackTable = new AntList<BlackTable>();
            foreach (Black item in blackList)
            {
                blackTable.Add(new BlackTable()
                {
                    Selected = false,
                    Id = item.Id,
                    Path = item.Path,
                    Type = item.Type,
                    Time = item.Time.ToString(),
                });
            }
            table.Binding(blackTable);
        }

        private void restore_button_Click(object sender, System.EventArgs e)
        {
            if (blackTable.Count == 0 || !blackTable.Any(it => it.Selected))
            {
                AntdUI.Message.warn(mainForm, "请选择恢复的行！", autoClose: 3);
            }
            else
            {
                Task.Run(() =>
                {
                    SysConfig.AddLog("其他", "恢复隔离区文件", $"操作时间：{DateTime.Now.ToString()}");
                    restore_button.Loading = true;
                    List<int> ids = new List<int>();
                    foreach (BlackTable item in blackTable)
                    {
                        if (item.Selected) ids.Add(item.Id);
                    }
                    SysConfig.RestoreBlack(ids);
                    InitTableData();
                    restore_button.Loading = false;
                    AntdUI.Message.success(mainForm, "恢复成功！", autoClose: 3);
                });
            }
        }

        private void remove_button_Click(object sender, System.EventArgs e)
        {
            if (blackTable.Count == 0 || !blackTable.Any(it => it.Selected))
            {
                AntdUI.Message.warn(mainForm, "请选择删除的行！", autoClose: 3);
            }
            else
            {
                Task.Run(() =>
                {
                    SysConfig.AddLog("其他", "删除隔离区文件", $"操作时间：{DateTime.Now.ToString()}");
                    remove_button.Loading = true;
                    List<int> ids = new List<int>();
                    foreach (BlackTable item in blackTable)
                    {
                        if (item.Selected) ids.Add(item.Id);
                    }
                    SysConfig.RemoveBlack(ids);
                    InitTableData();
                    remove_button.Loading = false;
                    AntdUI.Message.success(mainForm, "删除成功！", autoClose: 3);
                });
            }
        }

        private void path_input_TextChanged(object sender, System.EventArgs e)
        {
            InitTableData();
        }
    }
}
