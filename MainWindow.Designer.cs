namespace GreenHat
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AntdUI.SegmentedItem segmentedItem6 = new AntdUI.SegmentedItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            AntdUI.SegmentedItem segmentedItem7 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem8 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem9 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem10 = new AntdUI.SegmentedItem();
            this.titlebar = new AntdUI.PageHeader();
            this.button_color = new AntdUI.Button();
            this.mainPanel = new AntdUI.Panel();
            this.segmented = new AntdUI.Segmented();
            this.titlebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlebar
            // 
            this.titlebar.Controls.Add(this.button_color);
            this.titlebar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlebar.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titlebar.Location = new System.Drawing.Point(0, 0);
            this.titlebar.MaximizeBox = false;
            this.titlebar.Name = "titlebar";
            this.titlebar.ShowButton = true;
            this.titlebar.ShowIcon = true;
            this.titlebar.Size = new System.Drawing.Size(1024, 40);
            this.titlebar.SubText = "";
            this.titlebar.TabIndex = 0;
            this.titlebar.Text = "绿帽子安全防护";
            // 
            // button_color
            // 
            this.button_color.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_color.Ghost = true;
            this.button_color.IconRatio = 0.6F;
            this.button_color.IconSvg = "SunOutlined";
            this.button_color.Location = new System.Drawing.Point(878, 0);
            this.button_color.Name = "button_color";
            this.button_color.Radius = 0;
            this.button_color.Size = new System.Drawing.Size(50, 40);
            this.button_color.TabIndex = 1;
            this.button_color.ToggleIconSvg = "MoonOutlined";
            this.button_color.WaveSize = 0;
            // 
            // mainPanel
            // 
            this.mainPanel.Location = new System.Drawing.Point(95, 42);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(928, 598);
            this.mainPanel.TabIndex = 9;
            // 
            // segmented
            // 
            this.segmented.BackActive = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.segmented.Cursor = System.Windows.Forms.Cursors.Hand;
            this.segmented.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.segmented.ForeActive = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.segmented.Full = true;
            segmentedItem6.Badge = null;
            segmentedItem6.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem6.BadgeBack = null;
            segmentedItem6.BadgeMode = false;
            segmentedItem6.BadgeOffsetX = 0;
            segmentedItem6.BadgeOffsetY = 0;
            segmentedItem6.BadgeSize = 0.6F;
            segmentedItem6.BadgeSvg = null;
            segmentedItem6.IconActiveSvg = resources.GetString("segmentedItem6.IconActiveSvg");
            segmentedItem6.IconSvg = resources.GetString("segmentedItem6.IconSvg");
            segmentedItem6.Text = "主页";
            segmentedItem7.Badge = null;
            segmentedItem7.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem7.BadgeBack = null;
            segmentedItem7.BadgeMode = false;
            segmentedItem7.BadgeOffsetX = 0;
            segmentedItem7.BadgeOffsetY = 0;
            segmentedItem7.BadgeSize = 0.6F;
            segmentedItem7.BadgeSvg = null;
            segmentedItem7.IconActiveSvg = resources.GetString("segmentedItem7.IconActiveSvg");
            segmentedItem7.IconSvg = resources.GetString("segmentedItem7.IconSvg");
            segmentedItem7.Text = "查杀";
            segmentedItem8.Badge = null;
            segmentedItem8.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem8.BadgeBack = null;
            segmentedItem8.BadgeMode = false;
            segmentedItem8.BadgeOffsetX = 0;
            segmentedItem8.BadgeOffsetY = 0;
            segmentedItem8.BadgeSize = 0.6F;
            segmentedItem8.BadgeSvg = null;
            segmentedItem8.IconActiveSvg = resources.GetString("segmentedItem8.IconActiveSvg");
            segmentedItem8.IconSvg = resources.GetString("segmentedItem8.IconSvg");
            segmentedItem8.Text = "日志";
            segmentedItem9.Badge = null;
            segmentedItem9.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem9.BadgeBack = null;
            segmentedItem9.BadgeMode = false;
            segmentedItem9.BadgeOffsetX = 0;
            segmentedItem9.BadgeOffsetY = 0;
            segmentedItem9.BadgeSize = 0.6F;
            segmentedItem9.BadgeSvg = null;
            segmentedItem9.IconActiveSvg = resources.GetString("segmentedItem9.IconActiveSvg");
            segmentedItem9.IconSvg = resources.GetString("segmentedItem9.IconSvg");
            segmentedItem9.Text = "设置";
            segmentedItem10.Badge = null;
            segmentedItem10.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem10.BadgeBack = null;
            segmentedItem10.BadgeMode = false;
            segmentedItem10.BadgeOffsetX = 0;
            segmentedItem10.BadgeOffsetY = 0;
            segmentedItem10.BadgeSize = 0.6F;
            segmentedItem10.BadgeSvg = null;
            segmentedItem10.IconActiveSvg = resources.GetString("segmentedItem10.IconActiveSvg");
            segmentedItem10.IconSvg = resources.GetString("segmentedItem10.IconSvg");
            segmentedItem10.Text = "关于";
            this.segmented.Items.Add(segmentedItem6);
            this.segmented.Items.Add(segmentedItem7);
            this.segmented.Items.Add(segmentedItem8);
            this.segmented.Items.Add(segmentedItem9);
            this.segmented.Items.Add(segmentedItem10);
            this.segmented.Location = new System.Drawing.Point(12, 50);
            this.segmented.Name = "segmented";
            this.segmented.SelectIndex = 0;
            this.segmented.Size = new System.Drawing.Size(70, 578);
            this.segmented.TabIndex = 0;
            this.segmented.Vertical = true;
            this.segmented.SelectIndexChanged += new AntdUI.IntEventHandler(this.segmented_SelectIndexChanged);
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(1024, 640);
            this.ControlBox = false;
            this.Controls.Add(this.segmented);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.titlebar);
            this.EnableHitTest = false;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MainWindow";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GreenHat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.titlebar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader titlebar;
        private AntdUI.Button button_color;
        private AntdUI.Panel mainPanel;
        private AntdUI.Segmented segmented;
    }
}