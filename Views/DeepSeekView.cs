using System.IO;
using System.Net.Http;
using System.Text;
using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using GreenHat.Utils;
using PeNet;
using System.Linq;
using System.Diagnostics;

namespace GreenHat.Views
{
    public partial class DeepSeekView : UserControl
    {
        private MainWindow mainForm;

        public DeepSeekView(MainWindow mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        private async void CheckFile(string filePath)
        {
            input.Text = "";

            // 定义 API URL 和 Bearer Token
            string apiUrl = "https://api.siliconflow.cn/v1/chat/completions";
            string token = "sk-ntruycdijbbxdzvitgbimmainapddcnqxfwvgkdwjpltcqvu";

            string peString = GetPEString(filePath);

            // 构造请求体
            var requestBody = new
            {
                model = "deepseek-ai/DeepSeek-R1-Distill-Qwen-7B",
                messages = new[]
                {
                    new { role = "user", content = String.IsNullOrEmpty(peString) ? $"帮我分析一下这个文件是不是恶意文件，扩展名：{Path.GetExtension(filePath)}，数据如下：{File.ReadAllText(filePath)}" : peString },
                },
                stream = true,
                stop = (string)null,
                temperature = 0.7,
                top_p = 0.7,
                top_k = 50,
                frequency_penalty = 0.5,
                n = 1,
                max_tokens = 16384,
                response_format = new { type = "text" }
            };

            // 将请求体序列化为 JSON
            string jsonBody = JsonConvert.SerializeObject(requestBody);

            // 创建 HttpClient 实例
            HttpClient client = new HttpClient();

            // 设置请求头
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // 创建 HttpContent
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            file_button.Loading = true;
            file_button.Text = "请等待...";
            try
            {
                // 发送 POST 请求并获取响应
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // 确保响应成功
                if (!response.IsSuccessStatusCode) 
                {
                    Trace.WriteLine(await response.Content.ReadAsStringAsync());
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }

                // 获取响应流
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new StreamReader(responseStream);

                // 逐行读取流式响应
                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(line) || !line.StartsWith("data: ")) continue;
                    string text = line.Substring(6).Trim();
                    if (text.Equals("[DONE]")) break;
                    JObject obj = JObject.Parse(text);
                    string dsReasoning = obj["choices"]?[0]?["delta"]?["reasoning_content"]?.ToString();
                    string dsContent = obj["choices"]?[0]?["delta"]?["content"]?.ToString();
                    if (!String.IsNullOrEmpty(dsReasoning)) input.AppendText(dsReasoning);
                    else input.AppendText(dsContent);
                }
            }
            catch
            {
                AntdUI.Message.error(mainForm, "发生错误，文件可能过大！", autoClose: 3);
            }
            finally
            {
                file_button.Loading = false;
                file_button.Text = "选择需要分析的文件";
            }
        }

        private void file_button_Click(object sender, EventArgs e)
        {
            // 创建 OpenFileDialog 实例
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置对话框标题和过滤器
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "可执行文件|*.exe;*.dll;*.sys;*.ocx;*.bat;*.cmd;*.ps1;*.vbs;*.js;*.wsf";

            // 显示对话框并检查用户是否选择了文件
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件路径
                string filePath = openFileDialog.FileName;
                Task.Run(() => CheckFile(filePath));
            }
        }

        private string GetPEString(string filePath)
        {
            try
            {
                if (!PeFile.IsPeFile(filePath)) return null;

                // 将文件读取为字节数组
                byte[] fileBytes = File.ReadAllBytes(filePath);

                var pe = new PeFile(fileBytes);

                string importTable = "";
                if (pe.ImportedFunctions != null)
                {
                    importTable = string.Join(", ", pe.ImportedFunctions.Select(item => item.Name));
                }

                string exportTable = "";
                if (pe.ExportedFunctions != null)
                {
                    exportTable = string.Join(", ", pe.ExportedFunctions.Select(item => item.Name));
                }

                return $"帮我分析一下这个PE文件是不是恶意软件，文件头部分的base64数据为：{GetPeHeaderAsBase64(filePath)}，请解码分析后请用中文回答";
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return null;
        }

        static string GetPeHeaderAsBase64(string filePath)
        {
            if (!File.Exists(filePath)) return "";

            // 定义读取的字节数
            const int headerSize = 16384;

            // 读取文件头部字节
            byte[] headerBytes;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                headerBytes = new byte[headerSize];
                int bytesRead = fs.Read(headerBytes, 0, headerSize);

                // 如果文件小于headerSize，则调整数组大小
                if (bytesRead < headerSize)
                {
                    Array.Resize(ref headerBytes, bytesRead);
                }
            }

            // 转换为Base64字符串
            return Convert.ToBase64String(headerBytes);
        }
    }
}
