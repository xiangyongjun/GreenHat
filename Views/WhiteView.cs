using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GreenHat.Views
{
    public partial class WhiteView : UserControl
    {
        private MainWindow mainForm;
        private AntList<WhiteTable> whiteTable;

        public WhiteView(MainWindow mainForm)
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
                    Width = "665"
                },
                new Column("Time", "加入时间", ColumnAlign.Center)
                {
                    Width = "200"
                }
            };
        }

        private void InitTableData()
        {
            List<White> whiteList = SysConfig.GetWhiteList(path_input.Text);
            whiteTable = new AntList<WhiteTable>();
            foreach (White item in whiteList)
            {
                whiteTable.Add(new WhiteTable()
                {
                    Selected = false,
                    Id = item.Id,
                    Path = item.Path,
                    Time = item.Time.ToString(),
                });
            }
            table.Binding(whiteTable);
        }

        private void path_input_TextChanged(object sender, EventArgs e)
        {
            InitTableData();
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有文件 (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                SysConfig.AddWhite(filePath);
                InitTableData();
                AntdUI.Message.success(mainForm, "添加成功！", autoClose: 3);
            }
        }

        private void remove_button_Click(object sender, EventArgs e)
        {
            if (whiteTable.Count == 0 || !whiteTable.Any(it => it.Selected))
            {
                AntdUI.Message.warn(mainForm, "请选择要删除的行！", autoClose: 3);
            }
            else 
            {
                List<int> ids = new List<int>();
                foreach (WhiteTable item in whiteTable)
                {
                    if (item.Selected) ids.Add(item.Id);
                }
                SysConfig.RemoveWhite(ids);
                InitTableData();
                AntdUI.Message.success(mainForm, "删除成功！", autoClose: 3);
            }
        }
    }
}
