using GreenHat.utils;
using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GreenHat.Utils
{
    internal static class Engine
    {
        private static string basePath = $"{AppDomain.CurrentDomain.BaseDirectory}engine\\";
        private static PredictionEngine<PEData, MalwarePrediction> greenHatEngine;

        public static void Init()
        {
            if (SysConfig.GetSetting("绿帽子机器学习引擎").Enabled)
            {
                Task.Run(() =>
                {
                    var mlContext = new MLContext();
                    var stream = new FileStream($@"{basePath}Model.zip", FileMode.Open);
                    var model = mlContext.Model.Load(stream, out var schema);
                    greenHatEngine = mlContext.Model.CreatePredictionEngine<PEData, MalwarePrediction>(model);
                });
            }
        }

        public static bool IsVirus(string path, out string[] result, bool isScan = false)
        {
            result = new string[2] { "", "" };
            List<Task<string>> tasks = new List<Task<string>>();
            if (SysConfig.GetSetting("绿帽子机器学习引擎").Enabled)
            {
                tasks.Add(Task.Run(() => $"绿帽子机器学习引擎\t{GreenHatScan(path)}"));
            }
            if (isScan && SysConfig.GetSetting("猎剑云引擎").Enabled)
            {
                tasks.Add(Task.Run(() => $"猎剑云引擎\t{LieJianScan(path)}" ));
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

        public static string GreenHatScan(string path)
        {
            try
            {
                var pe = PEFeatureExtractor.ExtractFeatures(path);
                if (pe != null)
                {
                    var prediction = greenHatEngine.Predict(pe);
                    return prediction.IsMalware && pe.TrustSigned != 1 && pe.HasDirSigned != 1 && prediction.Probability >= 0.8 ? $"Win/Malicious.GreenHat.{(int)(prediction.Probability * 100)}" : null;
                }
            }
            catch { }
            return null;
        }

        public static string LieJianScan(string path)
        {
            try
            {
                TOTP totp = new TOTP("");
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

        public static void Dispose()
        {
        }
    }
}
