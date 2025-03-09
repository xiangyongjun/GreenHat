using AntdUI;
using GreenHat.Entitys;
using GreenHat.Models;
using GreenHat.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        public CloudMarkForm()
        {
            InitializeComponent();
            InitData();
            InitTableColumns();
            InitTableData();
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
                new Column("Name", "文件名称", ColumnAlign.Left)
                {
                    Width = "320"
                },
                new Column("Type", "类型", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("Time", "鉴定时间", ColumnAlign.Center)
                {
                    Width = "200"
                }
            };
        }

        private void InitTableData()
        {
            cloudList = SysConfig.GetCloudList();
            if (cloudList.Count > 0)
            {
                table.SelectedIndex = 1;
                SetCloudInfo(cloudList[0]);
            }
            else ResetInfo();
                AntList<CloudTable> list = new AntList<CloudTable>();
            foreach (Cloud item in cloudList)
            {
                list.Add(new CloudTable()
                {
                    Icon = new CellImage(Tools.Base64ToBitmap(item.Icon)),
                    Name = item.Name,
                    Type = new CellTag(item.Type, item.Score >= 70 ? TTypeMini.Error : item.Score >= 60 ? TTypeMini.Warn : item.Score == 0 ? TTypeMini.Info : TTypeMini.Success),
                    Time = item.Time.ToString()
                });
            }
            table.Binding(list);
        }

        private void SetCloudInfo(Cloud cloud)
        {
            curPath = cloud.Path;
            type_label.Text = $"{(cloud.Score >= 70 ? "病毒文件" : cloud.Score >= 60 ? "可疑文件" : cloud.Score == 0 ? "未知文件" : "安全文件")}";
            type_label.ForeColor = cloud.Score >= 70 ? Color.FromArgb(220, 68, 70) : cloud.Score >= 60 ? Color.FromArgb(250, 173, 20) : cloud.Score == 0 ? Color.FromArgb(22, 119, 255) : Color.FromArgb(76, 175, 80);
            tag.Text = cloud.Tag;
            tag.Type = cloud.Score >= 70 ? TTypeMini.Error : cloud.Score >= 60 ? TTypeMini.Warn : cloud.Score == 0 ? TTypeMini.Info : TTypeMini.Success;
            tag.Visible = true;
            path_label.Text = $"{cloud.Path}";
            filesize_label.Text = $"文件大小：{cloud.FileSize / 1024}KB";
            filetype_label.Text = $"文件类型：{Path.GetExtension(cloud.Path).Replace(".", "").ToUpper()}";
            createtime_label.Text = $"创建日期：{cloud.CreateTime.ToString()}";
            md5_label.Text = $"文件摘要：{cloud.Md5}";
            sha256_label.Text = $"SHA256：{cloud.Sha256}";
            result_label.Text = $"鉴定结果：{cloud.Score}分，云判定为{(cloud.Score >= 70 ? "病毒" : cloud.Score >= 60 ? "可疑" : cloud.Score == 0 ? "未知" : "安全")}文件";
            cloudtime_label.Text = $"鉴定时间：{cloud.Time.ToString()}";
        }

        private void ResetInfo()
        {
            curPath = "";
            type_label.Text = "等待鉴定";
            type_label.ForeColor = Color.FromArgb(76, 175, 80);
            tag.Text = "";
            tag.Type = TTypeMini.Success;
            tag.Visible = false;
            path_label.Text = "";
            filesize_label.Text = "文件大小：";
            filetype_label.Text = "文件类型：";
            createtime_label.Text = "创建日期：";
            md5_label.Text = "文件摘要：";
            sha256_label.Text = "SHA256：";
            result_label.Text = "鉴定结果：";
            cloudtime_label.Text = "鉴定时间：";
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
            openFileDialog.Title = "请选择一个文件";
            openFileDialog.Filter = "所有文件 (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Task.Run(() =>
                {
                    try
                    {
                        open_button.Loading = true;
                        clear_button.Enabled = false;
                        string sha256 = Tools.GetSHA256(openFileDialog.FileName);
                        TOTP totp = new TOTP(Engine.GetLieJianKey());
                        string url = $"https://www.virusmark.com/scan_get?sha256={sha256}&token={totp.Now()}";
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = client.GetAsync(url).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = response.Content.ReadAsStringAsync().Result;
                                JObject obj = JObject.Parse(responseBody);

                                Cloud cloud = new Cloud();
                                cloud.Name = Path.GetFileName(openFileDialog.FileName);
                                cloud.Path = openFileDialog.FileName;
                                cloud.Type = (int)obj.GetValue("score") >= 70 ? "病毒文件" : (int)obj.GetValue("score") >= 60 ? "可疑文件" : (int)obj.GetValue("score") == 0 ? "未知文件" : "安全文件";
                                cloud.Score = (int)obj.GetValue("score");
                                cloud.Tag = obj.GetValue("tag").ToString();
                                cloud.Icon = Tools.GetIconBase64(openFileDialog.FileName);
                                cloud.Sha256 = sha256;
                                cloud.Md5 = Tools.GetMd5(openFileDialog.FileName);
                                cloud.Time = DateTime.Now;
                                cloud.FileSize = (int)new FileInfo(openFileDialog.FileName).Length;
                                cloud.CreateTime = File.GetCreationTime(openFileDialog.FileName);
                                if (!SysConfig.AddCloud(cloud)) return;

                                InitTableData();

                                if (cloud.Score == 0 && cloud.FileSize <= 52428800)
                                {
                                    AntdUI.Modal.open(new Modal.Config(this, "绿帽子安全防护", "发现新的未知文件，是否上传鉴定？")
                                    {
                                        Icon = TType.Info,
                                        CloseIcon = true,
                                        Mask = false,
                                        OkText = "确定上传",
                                        CancelText = "取消上传",
                                        OnOk = config =>
                                        {
                                            UploadUnknownFile(openFileDialog.FileName, cloud.Sha256);
                                            return true;
                                        }
                                    });
                                }
                            }
                        }
                    }
                    catch
                    {
                        AntdUI.Message.error(this, "鉴定失败，可能由于网络原因！", autoClose: 3);
                    }
                    finally
                    {
                        open_button.Loading = false;
                        clear_button.Enabled = true;
                    }
                });
            }
        }

        private async void UploadUnknownFile(string path, string sha256)
        {
            try
            {
                TOTP totp = new TOTP(Engine.GetLieJianKey());
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
            SysConfig.ClearCloud();
            InitTableData();
        }
    }
}
