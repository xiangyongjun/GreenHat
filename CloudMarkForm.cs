using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenHat
{
    public partial class CloudMarkForm : Window
    {
        private string curPath = "";
        private List<Cloud> cloudList;

        public CloudMarkForm(string path = "")
        {
            InitializeComponent();
            ResetInfo();
            InitData();
            InitTableColumns();
            InitTableData();
            if (!String.IsNullOrEmpty(path)) CloudScan(path);
        }

        private void InitData()
        {
            ThemeHelper.SetColorMode(this, ThemeHelper.IsLightMode());
            Config.ShowInWindow = true;
        }

        private void InitTableColumns()
        {
            table.Columns = new ColumnCollection() {
                new Column("Icon", "", ColumnAlign.Center)
                {
                    Width = "60"
                },
                new Column("Name", Localization.Get("文件名称", "文件名称"), ColumnAlign.Left)
                {
                    Width = "320"
                },
                new Column("Type", Localization.Get("类型", "类型"), ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("Time", Localization.Get("鉴定时间", "鉴定时间"), ColumnAlign.Center)
                {
                    Width = "200"
                }
            };
        }

        private void InitTableData()
        {
            cloudList = Database.GetCloudList();
            if (cloudList.Count > 0)
            {
                table.SelectedIndex = 1;
                SetCloudInfo(cloudList[0]);
            }
            BindingList<CloudTable> list = new BindingList<CloudTable>();
            foreach (Cloud item in cloudList)
            {
                list.Add(new CloudTable()
                {
                    Icon = new CellImage(Tools.Base64ToBitmap(item.Icon)),
                    Name = Localization.Get(item.Name, item.Name),
                    Type = new CellTag(Localization.Get(item.Type, item.Type), item.Score >= 70 ? TTypeMini.Error : item.Score >= 60 ? TTypeMini.Warn : item.Score == 0 ? TTypeMini.Info : TTypeMini.Success),
                    Time = item.Time.ToString()
                });
            }
            table.Binding(list);
        }

        private void SetCloudInfo(Cloud cloud)
        {
            curPath = cloud.Path;
            type_label.Text = $"{(cloud.Score >= 70 ? Localization.Get("病毒文件", "病毒文件") : cloud.Score >= 60 ? Localization.Get("可疑文件", "可疑文件") : cloud.Score == 0 ? Localization.Get("未知文件", "未知文件") : Localization.Get("安全文件", "安全文件"))}";
            type_label.ForeColor = cloud.Score >= 70 ? Color.FromArgb(220, 68, 70) : cloud.Score >= 60 ? Color.FromArgb(250, 173, 20) : cloud.Score == 0 ? Color.FromArgb(22, 119, 255) : Color.FromArgb(76, 175, 80);
            tag.Text = cloud.Tag;
            tag.Type = cloud.Score >= 70 ? TTypeMini.Error : cloud.Score >= 60 ? TTypeMini.Warn : cloud.Score == 0 ? TTypeMini.Info : TTypeMini.Success;
            tag.Visible = true;
            path_label.Text = $"{cloud.Path}";
            filesize_label.Text = $"{Localization.Get("文件大小", "文件大小")}：{cloud.FileSize / 1024}KB";
            filetype_label.Text = $"{Localization.Get("文件类型", "文件类型")}：{Path.GetExtension(cloud.Path).Replace(".", "").ToUpper()}";
            createtime_label.Text = $"{Localization.Get("创建日期", "创建日期")}：{cloud.CreateTime.ToString()}";
            md5_label.Text = $"{Localization.Get("文件摘要", "文件摘要")}：{cloud.Md5}";
            sha256_label.Text = $"SHA256：{cloud.Sha256}";
            result_label.Text = $"{Localization.Get("鉴定结果", "鉴定结果")}：{cloud.Score}{Localization.Get("分", "分")}，{Localization.Get("云判定为", "云判定为")}{(cloud.Score >= 70 ? Localization.Get("病毒文件", "病毒文件") : cloud.Score >= 60 ? Localization.Get("可疑文件", "可疑文件") : cloud.Score == 0 ? Localization.Get("未知文件", "未知文件") : Localization.Get("安全文件", "安全文件"))}";
            cloudtime_label.Text = $"{Localization.Get("鉴定时间", "鉴定时间")}：{cloud.Time.ToString()}";
        }

        private void ResetInfo()
        {
            open_button.Text = Localization.Get("选择文件鉴定", "选择文件鉴定");
            clear_button.Text = Localization.Get("清空鉴定记录", "清空鉴定记录");
            titlebar.Text = Localization.Get("猎剑文件鉴定云", "猎剑文件鉴定云");
            fileinfo_label.Text = Localization.Get("文件信息", "文件信息");
            filepath_label.Text = $"{Localization.Get("文件路径", "文件路径")}：";
            table.EmptyText = Localization.Get("暂无数据", "暂无数据");
            curPath = "";
            type_label.Text = Localization.Get("等待鉴定", "等待鉴定");
            type_label.ForeColor = Color.FromArgb(76, 175, 80);
            tag.Text = "";
            tag.Type = TTypeMini.Success;
            tag.Visible = false;
            path_label.Text = "";
            filesize_label.Text = $"{Localization.Get("文件大小", "文件大小")}：";
            filetype_label.Text = $"{Localization.Get("文件类型", "文件类型")}：";
            createtime_label.Text = $"{Localization.Get("创建日期", "创建日期")}：";
            md5_label.Text = $"{Localization.Get("文件摘要", "文件摘要")}：";
            sha256_label.Text = $"SHA256：";
            result_label.Text = $"{Localization.Get("鉴定结果", "鉴定结果")}：";
            cloudtime_label.Text = $"{Localization.Get("鉴定时间", "鉴定时间")}：";
        }

        private void path_label_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(curPath)) return;
            Tools.OpenFileInExplorer(curPath);
        }

        private void table_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.RowIndex == 0 || e.RowIndex > cloudList.Count) return;
            SetCloudInfo(cloudList[e.RowIndex - 1]);
        }

        private void open_button_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = $"{Localization.Get("所有文件", "所有文件")} (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CloudScan(openFileDialog.FileName);
            }
        }

        private void CloudScan(string path)
        {
            Task.Run(() =>
            {
                try
                {
                    open_button.Loading = true;
                    clear_button.Enabled = false;
                    string sha256 = Tools.GetSHA256(path);
                    TOTP totp = new TOTP(GreenHatEngine.GetTalonflameKey());
                    string url = $"https://www.virusmark.com/scan_get?sha256={sha256}&token={totp.Now()}";
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = client.GetAsync(url).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = response.Content.ReadAsStringAsync().Result;
                            JObject obj = JObject.Parse(responseBody);

                            Cloud cloud = new Cloud();
                            cloud.Name = Path.GetFileName(path);
                            cloud.Path = path;
                            cloud.Type = (int)obj.GetValue("score") >= 70 ? Localization.Get("病毒文件", "病毒文件") : (int)obj.GetValue("score") >= 60 ? Localization.Get("可疑文件", "可疑文件") : (int)obj.GetValue("score") == 0 ? Localization.Get("未知文件", "未知文件") : Localization.Get("安全文件", "安全文件");
                            cloud.Score = (int)obj.GetValue("score");
                            cloud.Tag = obj.GetValue("tag").ToString();
                            cloud.Icon = Tools.GetIconBase64(path);
                            cloud.Sha256 = sha256;
                            cloud.Md5 = Tools.GetMd5(path);
                            cloud.Time = DateTime.Now;
                            cloud.FileSize = (int)new FileInfo(path).Length;
                            cloud.CreateTime = File.GetCreationTime(path);
                            if (!Database.AddCloud(cloud)) return;

                            InitTableData();

                            if (cloud.Score == 0 && cloud.FileSize <= 52428800)
                            {
                                AntdUI.Modal.open(new Modal.Config(this, Localization.Get("绿帽子安全防护", "绿帽子安全防护"), Localization.Get("发现新的未知文件，是否上传鉴定？", "发现新的未知文件，是否上传鉴定？"))
                                {
                                    Icon = TType.Info,
                                    CloseIcon = true,
                                    Mask = false,
                                    OkText = Localization.Get("确定上传", "确定上传"),
                                    CancelText = Localization.Get("取消上传", "取消上传"),
                                    OnOk = config =>
                                    {
                                        UploadUnknownFile(path, cloud.Sha256);
                                        return true;
                                    }
                                });
                            }
                        }
                    }
                }
                catch
                {
                    AntdUI.Message.error(this, Localization.Get("鉴定失败，可能由于网络原因！", "鉴定失败，可能由于网络原因！"), autoClose: 3);
                }
                finally
                {
                    open_button.Loading = false;
                    clear_button.Enabled = true;
                }
            });
        }

        private async void UploadUnknownFile(string path, string sha256)
        {
            try
            {
                TOTP totp = new TOTP(GreenHatEngine.GetTalonflameKey());
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(totp.Now()), "token");
                    content.Add(new StringContent(sha256), "sha256");
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(path));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    content.Add(fileContent, "file", Path.GetFileName(path));
                    using (HttpClient client = new HttpClient())
                    {
                        string url = "https://www.virusmark.com/scan";
                        HttpResponseMessage response = await client.PostAsync(url, content);
                    }
                }
            }
            catch { }
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            Database.ClearCloud();
            ResetInfo();
            InitTableData();
        }
    }
}
