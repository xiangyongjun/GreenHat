﻿using System.Reflection;
using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using GreenHat.Utils;
using AntdUI;

namespace GreenHat.Views
{
    public partial class UpdateView : UserControl
    {
        private MainWindow mainForm;
        private string downloadUrl;

        public UpdateView(MainWindow mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            CheckVersion();
        }

        private async void CheckVersion()
        {
            old_label.Text = $"{Localization.Get("当前版本", "当前版本")}：";
            new_label.Text = $"{Localization.Get("最新版本", "最新版本")}：";
            update_button.Text = Localization.Get("立即更新", "立即更新");
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version oldVersion = assembly.GetName().Version;
            string temp = await GetLatestVersionAsync();
            Version newVersion = Version.Parse((string.IsNullOrEmpty(temp) ? oldVersion.ToString() : temp));
            old_label.Text = $"{Localization.Get("当前版本", "当前版本")}：{oldVersion}";
            new_label.Text = $"{Localization.Get("最新版本", "最新版本")}：{newVersion}";
            if (newVersion.CompareTo(oldVersion) > 0) update_button.Enabled = true;
            else update_button.Text = Localization.Get("已是最新", "已是最新");
        }

        private async Task<string> GetLatestVersionAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://api.github.com/repos/xiangyongjun/GreenHat/releases/latest";
                    client.DefaultRequestHeaders.Add("User-Agent", "GreenHat");
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject releaseInfo = JObject.Parse(json);
                        downloadUrl = releaseInfo["assets"][0]["browser_download_url"].ToString();
                        return releaseInfo["tag_name"].ToString().ToLower().Replace("v", "");
                    }
                }
            }
            catch { }
            return null;
        }

        private async void update_button_Click(object sender, EventArgs e)
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}update.zip";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    update_button.Text = Localization.Get("正在更新", "正在更新");
                    update_button.Enabled = false;
                    update_button.Loading = true;
                    download_label.Visible = true;
                    progress.Visible = true;
                    if (File.Exists(path)) File.Delete(path);
                    HttpResponseMessage response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    long? totalBytes = response.Content.Headers.ContentLength;
                    long receivedBytes = 0;
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[8192];
                        int bytesRead;
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            receivedBytes += bytesRead;
                            if (totalBytes.HasValue)
                            {
                                float progressPercentage = (float)receivedBytes / totalBytes.Value;
                                progress.Value = progressPercentage;
                            }
                        }
                    }
                }
            }
            finally
            {
                update_button.Loading = false;
                Tools.DeleteService("GreenHatService");
                Engine.Dispose();
                Tools.ExecuteCommand($"{AppDomain.CurrentDomain.BaseDirectory}update.bat", "", $"{AppDomain.CurrentDomain.BaseDirectory}");
            }
        }
    }
}
