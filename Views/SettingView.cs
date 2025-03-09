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
            InitTableData();
        }

        private void InitTableColumns()
        {
            table.Columns = new ColumnCollection() {
                new Column("Icon", "", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("Name", "功能", ColumnAlign.Left)
                {
                    Width = "200"
                },
                new Column("Desc", "描述", ColumnAlign.Left)
                {
                    Width = "550"
                },
                new ColumnSwitch("Enabled", "是否启用", ColumnAlign.Center){
                    Call = (value, record, i_row, i_col) => {
                        SettingTable settingTable = (SettingTable)record;
                        switch (settingTable.Name)
                        {
                            case "进程防护":
                                if (value) ProcessMonitor.Start();
                                else ProcessMonitor.Stop();
                                break;
                            case "文件防护":
                                if (value) FileMonitor.Start();
                                else FileMonitor.Stop();
                                break;
                            case "引导防护":
                                if (value) MbrProtect.Open();
                                else MbrProtect.Close();
                                break;
                            case "开机启动":
                                if (value) Tools.CreateAndStartService("GreenHatService", $"{AppDomain.CurrentDomain.BaseDirectory}GreenHatService.exe");
                                else Tools.DeleteService("GreenHatService");
                                break;
                            default:
                                Task.Run(async () =>{
                                    await Task.Delay(100);
                                    Engine.Init();
                                });
                                break;
                        };
                        string state = value ? "开启" : "关闭";
                        SysConfig.AddLog("其他", $"{state}{settingTable.Name}", $"操作时间：{DateTime.Now.ToString()}");
                        return SysConfig.SetSettingEnabled(settingTable.Name, value) ? value : !value;
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
                    Name = item.Name,
                    Desc = item.Desc,
                    Enabled = item.Enabled
                });
            }
            table.Binding(list);
        }
    }
}
