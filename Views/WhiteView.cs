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
            add_button.Text = Localization.Get("添加文件", "添加文件");
            dir_button.Text = Localization.Get("添加目录", "添加目录");
            remove_button.Text = Localization.Get("删除所选", "删除所选");
            path_input.PlaceholderText = Localization.Get("搜索", "搜索");
            table.EmptyText = Localization.Get("暂无数据", "暂无数据");
            table.Columns = new ColumnCollection() {
                new ColumnCheck("Selected")
                {
                    Fixed = true, 
                    Width = "60"
                },
                new Column("Path", Localization.Get("文件路径", "文件路径"), ColumnAlign.Left)
                {
                    Width = "665"
                },
                new Column("Time", Localization.Get("加入时间", "加入时间"), ColumnAlign.Center)
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
            openFileDialog.Filter = $"{Localization.Get("所有文件", "所有文件")} (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                SysConfig.AddWhite(filePath);
                InitTableData();
                AntdUI.Message.success(mainForm, $"{Localization.Get("添加成功", "添加成功")}！", autoClose: 3);
            }
        }

        private void remove_button_Click(object sender, EventArgs e)
        {
            if (whiteTable.Count == 0 || !whiteTable.Any(it => it.Selected))
            {
                AntdUI.Message.warn(mainForm, $"{Localization.Get("请选择要删除的行", "请选择要删除的行")}！", autoClose: 3);
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
                AntdUI.Message.success(mainForm, $"{Localization.Get("删除成功", "删除成功")}！", autoClose: 3);
            }
        }

        private void dir_button_Click(object sender, EventArgs e)
        {
            AntdUI.FolderBrowserDialog dialog = new AntdUI.FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Task.Run(() =>
                {
                    add_button.Enabled = false;
                    remove_button.Enabled = false;
                    dir_button.Loading = true;
                    FileScan.Scan(new List<string>() { dialog.DirectoryPath }, (path) =>
                    {
                        SysConfig.AddWhite(path);
                        return true;
                    });
                    add_button.Enabled = true;
                    remove_button.Enabled = true;
                    dir_button.Loading = false;
                    InitTableData();
                    AntdUI.Message.success(mainForm, $"{Localization.Get("添加成功", "添加成功")}！", autoClose: 3);
                });
            }
        }
    }
}
