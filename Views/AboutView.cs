using AntdUI;
using GreenHat.Models;
using System.Windows.Forms;

namespace GreenHat.Views
{
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            InitializeComponent();
            InitTableColumns();
            InitTableData();
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
                    Width = "755"
                }
            };
        }

        private void InitTableData()
        {
            AntList<SysTable> list = new AntList<SysTable>();
            list.Add(new SysTable()
            {
                Name = "作者",
                Desc = "向永俊"
            });
            list.Add(new SysTable()
            {
                Name = "QQ",
                Desc = "827514947"
            });
            list.Add(new SysTable()
            {
                Name = "微信",
                Desc = "xyj0763"
            });
            list.Add(new SysTable()
            {
                Name = "邮箱",
                Desc = "827514947@qq.com"
            });
            list.Add(new SysTable()
            {
                Name = "Github地址",
                Desc = "https://github.com/xiangyongjun"
            });
            list.Add(new SysTable()
            {
                Name = "Gitee地址",
                Desc = "https://gitee.com/xiangyongjun"
            });
            //list.Add(new SysInfo()
            //{
            //    Name = "官方网站",
            //    Desc = "https://greenhat.icu"
            //});
            list.Add(new SysTable()
            {
                Name = "技术栈",
                Desc = "C#  .NET Framework4.8  Antd UI SqlSugar"
            });
            table.Binding(list);
        }
    }
}
