using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace GreenHat.Utils
{
    internal static class Engine
    {
        private static string basePath = $"{AppDomain.CurrentDomain.BaseDirectory}engine\\";

        public static void Init()
        {
            if (SysConfig.GetSetting("科洛机器学习引擎").Enabled)
            {
                Task.Run(() =>
                {
                    Tools.ExecuteCommand($"{basePath}KoloclassifyTool\\updateTool.exe", "--koloclassifytool", $"{basePath}KoloclassifyTool");
                });
            }
            if (SysConfig.GetSetting("ANK云雀轻量机学引擎").Enabled && Process.GetProcessesByName("ANK_OEMSERVE").Length == 0)
            {
                Task.Run(() => 
                {
                    Tools.ExecuteCommand($"{basePath}Ank\\ANK_OEMSERVE.exe", "", $"{basePath}Ank");
                });
            }
        }

        public static bool IsVirus(string path, out string[] result, bool isScan = false)
        {
            result = new string[2] { "", "" };
            if (IsSignature(path)) return false;
            List<Task<string>> tasks = new List<Task<string>>();
            if (SysConfig.GetSetting("科洛机器学习引擎").Enabled)
            {
                tasks.Add(Task.Run(() => $"科洛机器学习引擎\t{KoloScan(path)}"));
            }
            if (SysConfig.GetSetting("ANK云雀轻量机学引擎").Enabled)
            {
                tasks.Add(Task.Run(() => $"ANK云雀轻量机学引擎\t{AnkScan(path)}"));
            }
            if (SysConfig.GetSetting("T-Safety光弧YARA引擎").Enabled)
            {
                tasks.Add(Task.Run(() => $"T-Safety光弧YARA引擎\t{TSafetyScan(path)}"));
            }
            if (isScan && SysConfig.GetSetting("猎剑云引擎").Enabled)
            {
                tasks.Add(Task.Run(() => $"猎剑云引擎\t{LieJianScan(path)}" ));
            }
            if (isScan && SysConfig.GetSetting("czk杀毒引擎").Enabled)
            {
                tasks.Add(Task.Run(() => $"czk杀毒引擎\t{CzkScan(path)}"));
            }
            if (isScan && SysConfig.GetSetting("科洛云端威胁情报中心").Enabled)
            {
                tasks.Add(Task.Run(() => $"科洛云端威胁情报中心\t{KcstScan(path)}"));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (Task<string> task in tasks)
            {
                string[] virus = task.GetAwaiter().GetResult().Split('\t');
                if (virus.Length < 2 || string.IsNullOrEmpty(virus[1])) continue;
                if (result[0].Length > 0) result[0] += "、";
                result[0] += virus[0];
                result[1] = virus[1];
            }
            return !string.IsNullOrEmpty(result[0]);
        }

        public static bool IsSignature(string path)
        {
            try
            {
                string result = Tools.ExecuteCommand($"{basePath}KoloclassifyTool\\signature.exe", path, $"{basePath}KoloclassifyTool");
                if (string.IsNullOrEmpty(result)) return false;
                return result.Trim() == "0";
            }
            catch { }
            return false;
        }

        public static string KoloScan(string path)
        {
            try
            {
                string result = Tools.ExecuteCommand($"{basePath}KoloclassifyTool\\KoloclassifyTool.exe", $"--model model.joblib --file {path}", $"{basePath}KoloclassifyTool");
                if (string.IsNullOrEmpty(result)) return null;
                return result.Trim() == "1" ? "Win/Malicious.KoloVD" : null;
            }
            catch { }
            return null;
        }

        public static string AnkScan(string path)
        {
            try
            {
                string result = Tools.ExecuteCommand($"{basePath}Ank\\OEM_ANKCORE.exe", $"\"{path}\"", $"{basePath}Ank");
                if (string.IsNullOrEmpty(result)) return null;
                return double.Parse(result.Trim()) >= 0.9 ? "Win/Malicious.ANK" : null;
            }
            catch {}
            return null;
        }

        public static string TSafetyScan(string path)
        {
            return null;
        }

        public static string LieJianScan(string path)
        {
            try
            {
                TOTP totp = new TOTP("VSF2OU6B2YAXZ7426372QOGV6Y");
                string url = $"https://www.virusmark.com/scan_get?md5={Tools.GetMd5(path)}&token={totp.Now()}";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        JObject obj = JObject.Parse(responseBody);
                        return (int)obj.GetValue("score") >= 80 ? obj.GetValue("tag").ToString().Replace("HDE:", "").Replace("DIN:", "") : null;
                    }
                }
            }
            catch { }
            return null;
        }

        public static string CzkScan(string path)
        {
            try
            {
                string url = "https://weilai.szczk.top/api/cloud.php";
                Dictionary<string, string> formData = new Dictionary<string, string>
                {
                    { "form", "json" },
                    { "md5", Tools.GetMd5(path) },
                    { "key", "bYuR1IoQiJLqlYOF9WAJMU5JIe7zt+h1GcGs2cLm6Kk=" }
                };
                using (HttpClient client = new HttpClient())
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(formData);
                    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        JObject obj = JObject.Parse(responseBody);
                        return !obj.GetValue("result").ToString().Equals("safe") ? "Win/Malicious.CZK" : null;
                    }
                }
            }
            catch { }
            return null;
        }

        public static string KcstScan(string path)
        {
            try
            {
                string result = Tools.ExecuteCommand($"{basePath}KoloclassifyTool\\Kolocloud.exe", $"-g -f {path} -g", $"{basePath}KoloclassifyTool");
                if (string.IsNullOrEmpty(result)) return null;
                JObject obj = JObject.Parse(result);
                int score = (int)obj["Server"]["return"]["安全指数"];
                return score < 70 ? "Win/Malicious.KCST" : null;
            }
            catch { }
            return null;
        }

        public static void Dispose()
        {
            if (Process.GetProcessesByName("updateTool").Length > 0)
            {
                Process[] processes = Process.GetProcessesByName("updateTool");
                foreach (Process processe in processes) processe.Kill();
            }
            if (Process.GetProcessesByName("ANK_OEMSERVE").Length > 0)
            {
                Process[] processes = Process.GetProcessesByName("ANK_OEMSERVE");
                foreach (Process processe in processes) processe.Kill();
            }
        }
    }
}
