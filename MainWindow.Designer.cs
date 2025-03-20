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
            AntdUI.SegmentedItem segmentedItem21 = new AntdUI.SegmentedItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            AntdUI.SegmentedItem segmentedItem22 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem23 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem24 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem25 = new AntdUI.SegmentedItem();
            this.titlebar = new AntdUI.PageHeader();
            this.dropdown_translate = new AntdUI.Dropdown();
            this.button_color = new AntdUI.Button();
            this.mainPanel = new AntdUI.Panel();
            this.segmented = new AntdUI.Segmented();
            this.titlebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlebar
            // 
            this.titlebar.Controls.Add(this.dropdown_translate);
            this.titlebar.Controls.Add(this.button_color);
            this.titlebar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlebar.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titlebar.Location = new System.Drawing.Point(0, 0);
            this.titlebar.MaximizeBox = false;
            this.titlebar.Name = "titlebar";
            this.titlebar.ShowButton = true;
            this.titlebar.ShowIcon = true;
            this.titlebar.Size = new System.Drawing.Size(1034, 40);
            this.titlebar.SubText = "";
            this.titlebar.TabIndex = 0;
            this.titlebar.Text = "绿帽子安全防护";
            // 
            // dropdown_translate
            // 
            this.dropdown_translate.Dock = System.Windows.Forms.DockStyle.Right;
            this.dropdown_translate.Ghost = true;
            this.dropdown_translate.IconRatio = 0.65F;
            this.dropdown_translate.IconSvg = "TranslationOutlined";
            this.dropdown_translate.Items.AddRange(new object[] {
            "简体中文",
            "繁体中文",
            "English",
            "Русский",
            "日本語",
            "한국어"});
            this.dropdown_translate.Location = new System.Drawing.Point(838, 0);
            this.dropdown_translate.Name = "dropdown_translate";
            this.dropdown_translate.Radius = 0;
            this.dropdown_translate.Size = new System.Drawing.Size(50, 40);
            this.dropdown_translate.TabIndex = 4;
            this.dropdown_translate.WaveSize = 0;
            this.dropdown_translate.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.dropdown_translate_SelectedValueChanged);
            // 
            // button_color
            // 
            this.button_color.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_color.Ghost = true;
            this.button_color.IconRatio = 0.6F;
            this.button_color.IconSvg = "SunOutlined";
            this.button_color.Location = new System.Drawing.Point(888, 0);
            this.button_color.Name = "button_color";
            this.button_color.Radius = 0;
            this.button_color.Size = new System.Drawing.Size(50, 40);
            this.button_color.TabIndex = 1;
            this.button_color.ToggleIconSvg = "MoonOutlined";
            this.button_color.WaveSize = 0;
            // 
            // mainPanel
            // 
            this.mainPanel.Location = new System.Drawing.Point(105, 42);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(928, 598);
            this.mainPanel.TabIndex = 9;
            // 
            // segmented
            // 
            this.segmented.BackActive = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.segmented.BarPosition = AntdUI.TAlignMini.Right;
            this.segmented.Cursor = System.Windows.Forms.Cursors.Hand;
            this.segmented.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.segmented.Full = true;
            segmentedItem21.Badge = null;
            segmentedItem21.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem21.BadgeBack = null;
            segmentedItem21.BadgeMode = false;
            segmentedItem21.BadgeOffsetX = 0;
            segmentedItem21.BadgeOffsetY = 0;
            segmentedItem21.BadgeSize = 0.6F;
            segmentedItem21.BadgeSvg = null;
            segmentedItem21.IconActiveSvg = "";
            segmentedItem21.IconSvg = resources.GetString("segmentedItem21.IconSvg");
            segmentedItem21.ID = "0";
            segmentedItem21.Text = "主页";
            segmentedItem22.Badge = null;
            segmentedItem22.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem22.BadgeBack = null;
            segmentedItem22.BadgeMode = false;
            segmentedItem22.BadgeOffsetX = 0;
            segmentedItem22.BadgeOffsetY = 0;
            segmentedItem22.BadgeSize = 0.6F;
            segmentedItem22.BadgeSvg = null;
            segmentedItem22.IconActiveSvg = "";
            segmentedItem22.IconSvg = resources.GetString("segmentedItem22.IconSvg");
            segmentedItem22.ID = "1";
            segmentedItem22.Text = "查杀";
            segmentedItem23.Badge = null;
            segmentedItem23.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem23.BadgeBack = null;
            segmentedItem23.BadgeMode = false;
            segmentedItem23.BadgeOffsetX = 0;
            segmentedItem23.BadgeOffsetY = 0;
            segmentedItem23.BadgeSize = 0.6F;
            segmentedItem23.BadgeSvg = null;
            segmentedItem23.IconActiveSvg = "";
            segmentedItem23.IconSvg = resources.GetString("segmentedItem23.IconSvg");
            segmentedItem23.ID = "2";
            segmentedItem23.Text = "日志";
            segmentedItem24.Badge = null;
            segmentedItem24.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem24.BadgeBack = null;
            segmentedItem24.BadgeMode = false;
            segmentedItem24.BadgeOffsetX = 0;
            segmentedItem24.BadgeOffsetY = 0;
            segmentedItem24.BadgeSize = 0.6F;
            segmentedItem24.BadgeSvg = null;
            segmentedItem24.IconActiveSvg = "";
            segmentedItem24.IconSvg = resources.GetString("segmentedItem24.IconSvg");
            segmentedItem24.ID = "3";
            segmentedItem24.Text = "设置";
            segmentedItem25.Badge = null;
            segmentedItem25.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem25.BadgeBack = null;
            segmentedItem25.BadgeMode = false;
            segmentedItem25.BadgeOffsetX = 0;
            segmentedItem25.BadgeOffsetY = 0;
            segmentedItem25.BadgeSize = 0.6F;
            segmentedItem25.BadgeSvg = null;
            segmentedItem25.IconActiveSvg = "";
            segmentedItem25.IconSvg = resources.GetString("segmentedItem25.IconSvg");
            segmentedItem25.ID = "4";
            segmentedItem25.Text = "关于";
            this.segmented.Items.Add(segmentedItem21);
            this.segmented.Items.Add(segmentedItem22);
            this.segmented.Items.Add(segmentedItem23);
            this.segmented.Items.Add(segmentedItem24);
            this.segmented.Items.Add(segmentedItem25);
            this.segmented.Location = new System.Drawing.Point(12, 50);
            this.segmented.Name = "segmented";
            this.segmented.SelectIndex = 0;
            this.segmented.Size = new System.Drawing.Size(80, 578);
            this.segmented.TabIndex = 0;
            this.segmented.Vertical = true;
            this.segmented.SelectIndexChanged += new AntdUI.IntEventHandler(this.segmented_SelectIndexChanged);
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(1034, 640);
            this.ControlBox = false;
            this.Controls.Add(this.segmented);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.titlebar);
            this.EnableHitTest = false;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
        private AntdUI.Dropdown dropdown_translate;
    }
}