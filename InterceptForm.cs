using AntdUI;
using GreenHat.Models;
using GreenHat.Utils;
using System.Diagnostics;

namespace GreenHat
{
    public partial class InterceptForm : Window
    {
        private string title;
        private string name;
        private string path;
        private string engine;
        private string type;
        private Process process;

        public InterceptForm(string title, string name, string path, string engine, string type)
        {
            InitializeComponent();
            InitData();
            this.title = title;
            this.name = name;
            this.path = path;
            this.engine = engine;
            this.type = type;
        }

        public InterceptForm(string title, string engine, string type, Process process)
        {
            InitializeComponent();
            InitData();
            this.title = title;
            name = process.ProcessName;
            path = process.MainModule?.FileName;
            this.process = process;
            this.engine = engine;
            this.type = type;
        }

        private void InitData()
        {
            ThemeHelper.SetColorMode(this, ThemeHelper.IsLightMode());
            Config.ShowInWindow = true;
        }

        private void InterceptForm_Shown(object sender, System.EventArgs e)
        {
            titlebar.Text = title;
            Text = title;
            path_label.Text = path;
            type_label.Text = type;
            engine_label.Text = engine;
            TopMost = true;
        }

        private void pass_button_Click(object sender, System.EventArgs e)
        {
            if (process != null) Tools.ResumeProcess(process);
            Database.AddLog("病毒防护", "暂不处理", $"文件：{path}");
            Close();
        }

        private void white_button_Click(object sender, System.EventArgs e)
        {
            if (process != null) Tools.ResumeProcess(process);
            Database.AddWhite(path);
            Close();
        }

        private void black_button_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (process != null) process.Kill();
                Database.AddBlack(path, type);
            }
            catch { }
            Close();
        }

        private void InterceptForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            InterceptQueue.Next();
        }

        public string GetPath()
        {
            return path;
        }

        private void path_label_Click(object sender, System.EventArgs e)
        {
            Tools.OpenFileInExplorer(path);
        }
    }
}
