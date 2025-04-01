using AntdUI;
using GreenHat.Models;
using System.ComponentModel;
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
                new Column("Name", Localization.Get("名称", "名称"), ColumnAlign.Left)
                {
                    Width = "150"
                },
                new Column("Desc", Localization.Get("描述", "描述"), ColumnAlign.Left)
                {
                    Width = "755"
                }
            };
        }

        private void InitTableData()
        {
            BindingList<SysTable> list = new BindingList<SysTable>();
            list.Add(new SysTable()
            {
                Name = Localization.Get("作者", "作者"),
                Desc = "向永俊"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("QQ", "QQ"),
                Desc = "827514947"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("微信", "微信"),
                Desc = "xyj0763"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("邮箱", "邮箱"),
                Desc = "827514947@qq.com"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("GitHub地址", "GitHub地址"),
                Desc = "https://github.com/xiangyongjun"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("Gitee地址", "Gitee地址"),
                Desc = "https://gitee.com/xiangyongjun"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("官方网站", "官方网站"),
                Desc = "https://greenhat.icu"
            });
            list.Add(new SysTable()
            {
                Name = Localization.Get("技术栈", "技术栈"),
                Desc = "C# - .NET Framework4.8 - Antd UI - SqlSugar - ML.NET"
            });
            table.Binding(list);
        }

        public override void Refresh()
        {
            header.Text = Localization.Get("关于作者", "关于作者");
            InitTableColumns();
            InitTableData();
            base.Refresh();
        }
    }
}
