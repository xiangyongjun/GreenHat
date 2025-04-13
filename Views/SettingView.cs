using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Properties;
using GreenHat.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
                        if (settingTable.Name.Equals(Localization.Get("进程防护", "进程防护")))
                        {
                            GreenHatConfig.ProcessEnable = value;
                            if (value) ProcessMonitor.Start();
                            else ProcessMonitor.Stop();
                        }
                        else if (settingTable.Name.Equals(Localization.Get("文件防护", "文件防护")))
                        {
                            GreenHatConfig.FileEnable = value;
                            if (value) FileMonitor.Start();
                            else FileMonitor.Stop();
                        }
                        else if (settingTable.Name.Equals(Localization.Get("引导防护", "引导防护")))
                        {
                            GreenHatConfig.MbrEnable = value;
                            if (value) MbrProtect.Open();
                            else MbrProtect.Close();
                        }
                        else if (settingTable.Name.Equals(Localization.Get("右键菜单", "右键菜单")))
                        {
                            GreenHatConfig.RightMenuEnable = value;
                            if (value) RightMenuManager.AddMenu();
                            else RightMenuManager.RemoveMenu();
                        }
                        else if (settingTable.Name.Equals(Localization.Get("开机启动", "开机启动")))
                        {
                            GreenHatConfig.AutoStartEnable = value;
                           if (value) Tools.CreateAndStartService("GreenHatService", $"{AppDomain.CurrentDomain.BaseDirectory}GreenHatService.exe");
                           else Tools.DeleteService("GreenHatService");
                        }
                        else if (settingTable.Name.Equals(Localization.Get("绿帽子机器学习引擎", "绿帽子机器学习引擎")))
                        {
                            GreenHatConfig.GreenHatEnable = value;
                            Task.Run(async () =>{
                                await Task.Delay(100);
                                GreenHatEngine.Init();
                            });
                        }
                        else if (settingTable.Name.Equals(Localization.Get("脚本查杀引擎", "脚本查杀引擎")))
                        {
                            GreenHatConfig.ScriptEnable = value;
                            Task.Run(async () =>{
                                await Task.Delay(100);
                                GreenHatEngine.Init();
                            });
                        }
                        else if (settingTable.Name.Equals(Localization.Get("猎剑云引擎", "猎剑云引擎")))
                        {
                            GreenHatConfig.TalonflameEnable = value;
                            Task.Run(async () =>{
                                await Task.Delay(100);
                                GreenHatEngine.Init();
                            });
                        }
                        string state = value ? Localization.Get("开启", "开启") : Localization.Get("关闭", "关闭");
                        Database.AddLog("其他", $"{state}{settingTable.Name}", $"操作时间：{DateTime.Now.ToString()}");
                        return value;
                    }
                }
            };
        }

        private void InitTableData()
        {
            BindingList<SettingTable> list = new BindingList<SettingTable>();
            list.Add(new SettingTable()
            {
                Icon = new CellImage(Bitmap.FromStream(new MemoryStream(Resources.Process))),
                Name = Localization.Get("进程防护", "进程防护"),
                Desc = Localization.Get("实时监控拦截所有正在打开的可疑程序", "实时监控拦截所有正在打开的可疑程序"),
                Enabled = GreenHatConfig.ProcessEnable
            });
            list.Add(new SettingTable()
            {
                Icon = new CellImage(Bitmap.FromStream(new MemoryStream(Resources.File))),
                Name = Localization.Get("文件防护", "文件防护"),
                Desc = Localization.Get("实时监控拦截所有正在写入的可疑文件", "实时监控拦截所有正在写入的可疑文件"),
                Enabled = GreenHatConfig.FileEnable
            });
            list.Add(new SettingTable()
            {
                Icon = new CellImage(Bitmap.FromStream(new MemoryStream(Resources.Mbr))),
                Name = Localization.Get("引导防护", "引导防护"),
                Desc = Localization.Get("实时监控系统引导扇区是否被非法篡改", "实时监控系统引导扇区是否被非法篡改"),
                Enabled = GreenHatConfig.MbrEnable
            });
            list.Add(new SettingTable()
            {
                Icon = new CellImage(Bitmap.FromStream(new MemoryStream(Resources.Menu))),
                Name = Localization.Get("右键菜单", "右键菜单"),
                Desc = Localization.Get("添加文件右键菜单，一键快速查杀", "添加文件右键菜单，一键快速查杀"),
                Enabled = GreenHatConfig.RightMenuEnable
            });
            list.Add(new SettingTable()
            {
                Icon = new CellImage(Bitmap.FromStream(new MemoryStream(Resources.Power))),
                Name = Localization.Get("开机启动", "开机启动"),
                Desc = Localization.Get("程序跟随系统开机启动", "程序跟随系统开机启动"),
                Enabled = GreenHatConfig.AutoStartEnable
            });
            list.Add(new SettingTable()
            {
                Icon = new CellImage(Bitmap.FromStream(new MemoryStream(Resources.GreenHat))),
                Name = Localization.Get("绿帽子机器学习引擎", "绿帽子机器学习引擎"),
                Desc = Localization.Get("绿帽子自研的恶意软件检测机学引擎（师承科洛，感谢猎剑云提供的样本）", "绿帽子自研的恶意软件检测机学引擎（师承科洛，感谢猎剑云提供的样本）"),
                Enabled = GreenHatConfig.GreenHatEnable
            });
            list.Add(new SettingTable()
            {
                Icon = new CellImage(Bitmap.FromStream(new MemoryStream(Resources.Script))),
                Name = Localization.Get("脚本查杀引擎", "脚本查杀引擎"),
                Desc = Localization.Get("绿帽子自研的脚本查杀引擎（使用沙盒分析行为进行查杀）", "绿帽子自研的脚本查杀引擎（使用沙盒分析行为进行查杀）"),
                Enabled = GreenHatConfig.GreenHatEnable
            });
            list.Add(new SettingTable()
            {
                Icon = new CellImage(Bitmap.FromStream(new MemoryStream(Resources.Talonflame))),
                Name = Localization.Get("猎剑云引擎", "猎剑云引擎"),
                Desc = Localization.Get("鹰眼鉴定，秒速响应（云引擎查杀时才会启用）", "鹰眼鉴定，秒速响应（云引擎查杀时才会启用）"),
                Enabled = GreenHatConfig.TalonflameEnable
            });
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
