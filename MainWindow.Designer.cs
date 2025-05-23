﻿namespace GreenHat
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
            AntdUI.SegmentedItem segmentedItem1 = new AntdUI.SegmentedItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            AntdUI.SegmentedItem segmentedItem2 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem3 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem4 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem5 = new AntdUI.SegmentedItem();
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
            segmentedItem1.Badge = null;
            segmentedItem1.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem1.BadgeBack = null;
            segmentedItem1.BadgeMode = false;
            segmentedItem1.BadgeOffsetX = 0;
            segmentedItem1.BadgeOffsetY = 0;
            segmentedItem1.BadgeSize = 0.6F;
            segmentedItem1.BadgeSvg = null;
            segmentedItem1.IconActiveSvg = "";
            segmentedItem1.IconSvg = resources.GetString("segmentedItem1.IconSvg");
            segmentedItem1.ID = "0";
            segmentedItem1.Text = "主页";
            segmentedItem2.Badge = null;
            segmentedItem2.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem2.BadgeBack = null;
            segmentedItem2.BadgeMode = false;
            segmentedItem2.BadgeOffsetX = 0;
            segmentedItem2.BadgeOffsetY = 0;
            segmentedItem2.BadgeSize = 0.6F;
            segmentedItem2.BadgeSvg = null;
            segmentedItem2.IconActiveSvg = "";
            segmentedItem2.IconSvg = resources.GetString("segmentedItem2.IconSvg");
            segmentedItem2.ID = "1";
            segmentedItem2.Text = "查杀";
            segmentedItem3.Badge = null;
            segmentedItem3.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem3.BadgeBack = null;
            segmentedItem3.BadgeMode = false;
            segmentedItem3.BadgeOffsetX = 0;
            segmentedItem3.BadgeOffsetY = 0;
            segmentedItem3.BadgeSize = 0.6F;
            segmentedItem3.BadgeSvg = null;
            segmentedItem3.IconActiveSvg = "";
            segmentedItem3.IconSvg = resources.GetString("segmentedItem3.IconSvg");
            segmentedItem3.ID = "2";
            segmentedItem3.Text = "日志";
            segmentedItem4.Badge = null;
            segmentedItem4.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem4.BadgeBack = null;
            segmentedItem4.BadgeMode = false;
            segmentedItem4.BadgeOffsetX = 0;
            segmentedItem4.BadgeOffsetY = 0;
            segmentedItem4.BadgeSize = 0.6F;
            segmentedItem4.BadgeSvg = null;
            segmentedItem4.IconActiveSvg = "";
            segmentedItem4.IconSvg = resources.GetString("segmentedItem4.IconSvg");
            segmentedItem4.ID = "3";
            segmentedItem4.Text = "设置";
            segmentedItem5.Badge = null;
            segmentedItem5.BadgeAlign = AntdUI.TAlignFrom.TR;
            segmentedItem5.BadgeBack = null;
            segmentedItem5.BadgeMode = false;
            segmentedItem5.BadgeOffsetX = 0;
            segmentedItem5.BadgeOffsetY = 0;
            segmentedItem5.BadgeSize = 0.6F;
            segmentedItem5.BadgeSvg = null;
            segmentedItem5.IconActiveSvg = "";
            segmentedItem5.IconSvg = resources.GetString("segmentedItem5.IconSvg");
            segmentedItem5.ID = "4";
            segmentedItem5.Text = "关于";
            this.segmented.Items.Add(segmentedItem1);
            this.segmented.Items.Add(segmentedItem2);
            this.segmented.Items.Add(segmentedItem3);
            this.segmented.Items.Add(segmentedItem4);
            this.segmented.Items.Add(segmentedItem5);
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
            this.Load += new System.EventHandler(this.MainWindow_Load);
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