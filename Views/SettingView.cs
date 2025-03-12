using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenHat.Views
{
    public partial class SettingView : UserControl
    {
        private MainWindow mainForm;

        public SettingView(MainWindow mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            InitTableColumns();
            Task.Run(InitTableData);
        }

        private void InitTableColumns()
        {
            table.Columns = new ColumnCollection() {
                new Column("Icon", "", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("Name", Localization.Get("功能", "功能"), ColumnAlign.Left)
                {
                    Width = "190"
                },
                new Column("Desc", Localization.Get("描述", "描述"), ColumnAlign.Left)
                {
                    Width = "520"
                },
                new ColumnSwitch("Enabled", Localization.Get("是否启用", "是否启用"), ColumnAlign.Center){
                    Width = "120",
                    Call = (value, record, i_row, i_col) => {
                        SettingTable settingTable = (SettingTable)record;
                        string name = "";
                        if (settingTable.Name.Equals(Localization.Get("进程防护", "进程防护")))
                        {
                            name = "进程防护";
                            if (value) ProcessMonitor.Start();
                            else ProcessMonitor.Stop();
                        }
                        else if (settingTable.Name.Equals(Localization.Get("文件防护", "文件防护")))
                        {
                            name = "文件防护";
                            if (value) FileMonitor.Start();
                            else FileMonitor.Stop();
                        }
                        else if (settingTable.Name.Equals(Localization.Get("引导防护", "引导防护")))
                        {
                            name = "引导防护";
                            if (value) MbrProtect.Open();
                            else MbrProtect.Close();
                        }
                        else if (settingTable.Name.Equals(Localization.Get("开机启动", "开机启动")))
                        {
                            name = "开机启动";
                           if (value) Tools.CreateAndStartService("GreenHatService", $"{AppDomain.CurrentDomain.BaseDirectory}GreenHatService.exe");
                           else Tools.DeleteService("GreenHatService");
                        }
                        else if (settingTable.Name.Equals(Localization.Get("绿帽子机机器学习引擎", "绿帽子机机器学习引擎")))
                        {
                            name = "绿帽子机机器学习引擎";
                        }
                        else if (settingTable.Name.Equals(Localization.Get("猎剑云引擎", "猎剑云引擎")))
                        {
                            name = "猎剑云引擎";
                        }
                        else
                        {
                            Task.Run(async () =>{
                                await Task.Delay(100);
                                Engine.Init();
                            });
                        }
                        string state = value ? Localization.Get("开启", "开启") : Localization.Get("关闭", "关闭");
                        SysConfig.AddLog("其他", $"{state}{settingTable.Name}", $"操作时间：{DateTime.Now.ToString()}");
                        return SysConfig.SetSettingEnabled(name, value) ? value : !value;
                    }
                }
            };
        }

        private void InitTableData()
        {
            List<Setting> settingList = SysConfig.GetSettingList();
            AntList<SettingTable> list = new AntList<SettingTable>();
            foreach (Setting item in settingList)
            {
                list.Add(new SettingTable()
                {
                    Icon = new CellImage(Tools.Base64ToBitmap(item.Icon)),
                    Name = Localization.Get(item.Name, item.Name),
                    Desc = Localization.Get(item.Desc, item.Desc),
                    Enabled = item.Enabled
                });
            }
            table.Binding(list);
        }

        override public void Refresh()
        {
            header.Text = Localization.Get("防护设置", "防护设置");
            header.Description = Localization.Get("查看、管理各项安全防护功能", "查看、管理各项安全防护功能");
            table.EmptyText = Localization.Get("暂无数据", "暂无数据");
            InitTableColumns();
            InitTableData();
            base.Refresh();
        }
    }
}
