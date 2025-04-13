using GreenHat.Utils;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GreenHat
{
    internal static class Program
    {
        private static MainWindow mainWindow;

        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            using (Mutex mutex = new Mutex(true, Application.ProductName, out createdNew))
            {
                if (createdNew)
                {
                    Application.ThreadExit += (sender, e) =>
                    {
                        GreenHatEngine.Dispose();
                    };
                    Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    AntdUI.Localization.DefaultLanguage = "zh-CN";
                    AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    mainWindow = new MainWindow(args);
                    Application.Run(mainWindow);
                }
                else
                {
                    if (args.Length > 0)
                    {
                        if (File.Exists(args[0]) || Directory.Exists(args[0]))
                        {
                            NamedPipeClient.Connect();
                            NamedPipeClient.SendMessage($"Scan\0{args[0]}");
                        }
                    }
                    else
                    {
                        NamedPipeClient.Connect();
                        NamedPipeClient.SendMessage($"Show\0Show");
                    }
                    Environment.Exit(1);
                }
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AntdUI.Notification.error(mainWindow, "未处理的UI线程异常", e.Exception.Message, autoClose: 3, align: AntdUI.TAlignFrom.TR);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AntdUI.Notification.error(mainWindow, "未处理的非UI线程异常", e.ToString(), autoClose: 3, align: AntdUI.TAlignFrom.TR);
        }
    }
}
