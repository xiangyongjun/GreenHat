using AntdUI;
using GreenHat;
using GreenHat.Utils;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System.Diagnostics;
using System.Threading.Tasks;

class ProcessMonitor
{
    private static TraceEventSession session = new TraceEventSession("GreenHatProcessMonitorSession");

    static ProcessMonitor()
    {
        session.BufferSizeMB = 1024;
        session.CpuSampleIntervalMSec = 0;
        session.EnableProvider("Microsoft-Windows-Kernel-Process", TraceEventLevel.Informational, matchAnyKeywords: 0x10);
        session.Source.Dynamic.All += ProcessTask;
    }

    public static void Start()
    {
        Task.Run(session.Source.Process);
    }

    public static void Stop()
    {
        session.Stop();
    }

    private static void ProcessTask(TraceEvent data)
    {
        try
        {
            if (!data.EventName.StartsWith("ProcessStart")) return;
            Process process = Process.GetProcessById((int)data.PayloadByName("ProcessID"));
            string name = process.ProcessName;
            string path = process.MainModule?.FileName;
            if (string.IsNullOrEmpty(path)) return;
            Tools.SuspendProcess(process);
            string[] result;
            if (!Database.IsWhite(path) && GreenHatEngine.IsVirus(path, out result))
            {
                Database.AddLog("病毒防护", "病毒拦截", $"文件：{path}");
                InterceptQueue.Add(new InterceptForm(Localization.Get("进程实时监控", "进程实时监控"), result[0], result[1], process));
            }
            else
            { 
                Tools.ResumeProcess(process);
            }
        }
        catch { }
    }
}