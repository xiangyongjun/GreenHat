using AntdUI;
using GreenHat.utils;
using Microsoft.ML;
using Newtonsoft.Json.Linq;
using PeNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace GreenHat.Utils
{
    internal static class Engine
    {
        private static string basePath = $@"{AppDomain.CurrentDomain.BaseDirectory}engine\";

        private static bool greenHatEnable = false;
        private static bool talonflameEnable = false;

        private static PredictionEngine<PEData, MalwarePrediction> greenHatEngine;
        private static string talonflameKey = "";

        public static void Init()
        {
            greenHatEnable = SysConfig.GetSetting("绿帽子机器学习引擎").Enabled;
            talonflameEnable = SysConfig.GetSetting("猎剑云引擎").Enabled;

            if (greenHatEnable)
            {
                Task.Run(() =>
                {
                    var mlContext = new MLContext();
                    var stream = new FileStream($"{basePath}model.zip", FileMode.Open);
                    var model = mlContext.Model.Load(stream, out var schema);
                    greenHatEngine = mlContext.Model.CreatePredictionEngine<PEData, MalwarePrediction>(model);
                });
            }
        }

        public static bool IsVirus(string path, out string[] result, bool isScan = false)
        {
            result = new string[2] { "", "" };
            List<Task<string[]>> tasks = new List<Task<string[]>>();
            if (greenHatEnable)
            {
                tasks.Add(Task.Run(() => new string[] { Localization.Get("绿帽子机器学习引擎", "绿帽子机器学习引擎"), GreenHatScan(path) }));
            }
            if (isScan && talonflameEnable)
            {
                tasks.Add(Task.Run(() => new string[] { Localization.Get("猎剑云引擎", "猎剑云引擎"), TalonflameScan(path) }));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (Task<string[]> task in tasks)
            {
                string[] virus = task.GetAwaiter().GetResult();
                if (string.IsNullOrEmpty(virus[1])) continue;
                if (result[0].Length > 0) result[0] += "、";
                result[0] += virus[0];
                result[1] = virus[1];
            }
            return !string.IsNullOrEmpty(result[0]);
        }

        public static string GreenHatScan(string path)
        {
            string result = null;
            try
            {
                // 检测pe文件
                if (PeFile.IsPeFile(path))
                {
                    var pe = PEFeatureExtractor.ExtractFeatures(path);
                    if (pe != null)
                    {
                        lock (greenHatEngine)
                        {
                            var prediction = greenHatEngine.Predict(pe);
                            return prediction.IsMalware && prediction.Probability >= 0.8 ? $"{MalwareClassifier.Classify(pe.ImportFuncs)}.{(int)(prediction.Probability * 100)}" : null;
                        }
                    }
                }
                // 检测msi文件
                else if (MsiExtractor.IsMsi(path))
                {
                    if (WinTrust.VerifyFileSignature(path)) return null;
                    MsiExtractor.ExtractPeFiles(path, (byte[] buffer, bool isInsideZip) =>
                    {
                        if (!PeFile.IsPeFile(buffer)) return true;
                        string tempPath = Path.GetTempFileName();
                        File.WriteAllBytes(tempPath, buffer);
                        try
                        {
                            var msiPe = PEFeatureExtractor.ExtractFeatures(tempPath);
                            if (msiPe != null)
                            {
                                lock (greenHatEngine)
                                {
                                    var prediction = greenHatEngine.Predict(msiPe);
                                    if (prediction.IsMalware && prediction.Probability >= 0.8)
                                    {
                                        result = $"{MalwareClassifier.Classify(msiPe.ImportFuncs)}.{(int)(prediction.Probability * 100)}";
                                        return false;
                                    }
                                }
                            }
                        }
                        catch { }
                        finally
                        {
                            File.Delete(tempPath);
                        }
                        return true;
                    });
                    return result;
                }
                // 检测压缩包文件
                else if (ZipExtractor.IsCompressedFile(path))
                {
                    ZipExtractor.Scan(path, (byte[] buffer) =>
                    {
                        if (!PeFile.IsPeFile(buffer)) return true;
                        string tempPath = Path.GetTempFileName();
                        try
                        {
                            File.WriteAllBytes(tempPath, buffer);
                            var zipPe = PEFeatureExtractor.ExtractFeatures(tempPath);
                            if (zipPe == null) return true;
                            lock (greenHatEngine)
                            {
                                var prediction = greenHatEngine.Predict(zipPe);
                                if (prediction.IsMalware && prediction.Probability >= 0.8)
                                {
                                    result = $"{MalwareClassifier.Classify(zipPe.ImportFuncs)}.{(int)(prediction.Probability * 100)}";
                                    return false;
                                }
                            }
                        }
                        catch { }
                        finally
                        {
                            File.Delete(tempPath);
                        }
                        return true;
                    });
                    return result;
                }
                // 检测镜像文件
                else if (IsoExtractor.IsIsoFile(path))
                {
                    IsoExtractor.Scan(path, (byte[] buffer) =>
                    {
                        if (!PeFile.IsPeFile(buffer)) return true;
                        string tempPath = Path.GetTempFileName();
                        try
                        {
                            File.WriteAllBytes(tempPath, buffer);
                            var isoPe = PEFeatureExtractor.ExtractFeatures(tempPath);
                            if (isoPe == null) return true;
                            lock (greenHatEngine)
                            {
                                var prediction = greenHatEngine.Predict(isoPe);
                                if (prediction.IsMalware && prediction.Probability >= 0.8)
                                {
                                    result = $"{MalwareClassifier.Classify(isoPe.ImportFuncs)}.{(int)(prediction.Probability * 100)}";
                                    return false;
                                }
                            }  
                        }
                        catch { }
                        finally
                        {
                            File.Delete(tempPath);
                        }
                        return true;
                    });
                    return result;
                }
            }
            catch { }
            return result;
        }

        public static int GetGreenHatModelVersion()
        {
            JObject version = JObject.Parse(File.ReadAllText($"{basePath}model.json"));
            return (int)version.GetValue("version");
        }

        public static async Task UpdateGreenHatEngine()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GreenHat");
                    HttpResponseMessage response = await client.GetAsync("https://greenhat.icu/engine/model.json");
                    if (!response.IsSuccessStatusCode) return;
                    string json = await response.Content.ReadAsStringAsync();
                    JObject versionInfo = JObject.Parse(json);
                    if ((int)versionInfo["version"] <= GetGreenHatModelVersion()) return;
                    Type type = new PEData().GetType();
                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (greenHatEngine.OutputSchema.Count - 1 < (int)versionInfo["schema"]) return;

                    using (HttpClient download = new HttpClient())
                    {
                        download.DefaultRequestHeaders.Add("User-Agent", "GreenHat");
                        byte[] fileBytes = await download.GetByteArrayAsync("https://greenhat.icu/engine/model.zip");
                        if (File.Exists($"{basePath}model.json")) File.Delete($"{basePath}model.json");
                        if (File.Exists($"{basePath}model.zip")) File.Delete($"{basePath}model.zip");
                        File.WriteAllText($"{basePath}model.json", json);
                        File.WriteAllBytes($"{basePath}model.zip", fileBytes);
                        Init();
                    }
                }
            }
            catch { }
        }

        public static string TalonflameScan(string path)
        {
            try
            {
                TOTP totp = new TOTP(talonflameKey);
                string url = $"https://www.virusmark.com/scan_get?sha256={Tools.GetSHA256(path)}&token={totp.Now()}";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        JObject obj = JObject.Parse(responseBody);
                        return (int)obj.GetValue("score") >= 70 ? $"{obj.GetValue("tag").ToString().Split('@')[0].Replace("Malware.", "")}" : null;
                    }
                }
            }
            catch { }
            return null;
        }

        public static string GetTalonflameKey()
        {
            return talonflameKey;
        }

        public static void Dispose()
        {
        }
    }
}
