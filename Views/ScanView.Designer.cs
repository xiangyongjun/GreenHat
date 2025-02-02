﻿namespace GreenHat.Views
{
    partial class ScanView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanView));
            this.header = new AntdUI.PageHeader();
            this.quick_button = new AntdUI.Button();
            this.full_button = new AntdUI.Button();
            this.custom_button = new AntdUI.Button();
            this.pause_button = new AntdUI.Button();
            this.stop_button = new AntdUI.Button();
            this.time_label = new AntdUI.Label();
            this.table = new AntdUI.Table();
            this.scan_timer = new System.Windows.Forms.Timer(this.components);
            this.continue_button = new AntdUI.Button();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.DividerShow = true;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.header.Size = new System.Drawing.Size(928, 74);
            this.header.SubText = "";
            this.header.TabIndex = 29;
            this.header.Text = "病毒查杀";
            this.header.UseTitleFont = true;
            // 
            // quick_button
            // 
            this.quick_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.quick_button.IconSize = new System.Drawing.Size(18, 18);
            this.quick_button.IconSvg = resources.GetString("quick_button.IconSvg");
            this.quick_button.Location = new System.Drawing.Point(3, 91);
            this.quick_button.Name = "quick_button";
            this.quick_button.Size = new System.Drawing.Size(100, 32);
            this.quick_button.TabIndex = 34;
            this.quick_button.Text = "快速查杀";
            this.quick_button.Type = AntdUI.TTypeMini.Success;
            this.quick_button.WaveSize = 0;
            this.quick_button.Click += new System.EventHandler(this.quick_button_Click);
            // 
            // full_button
            // 
            this.full_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.full_button.IconSize = new System.Drawing.Size(15, 15);
            this.full_button.IconSvg = resources.GetString("full_button.IconSvg");
            this.full_button.Location = new System.Drawing.Point(117, 91);
            this.full_button.Name = "full_button";
            this.full_button.Size = new System.Drawing.Size(100, 32);
            this.full_button.TabIndex = 35;
            this.full_button.Text = "全盘查杀";
            this.full_button.Type = AntdUI.TTypeMini.Success;
            this.full_button.WaveSize = 0;
            this.full_button.Click += new System.EventHandler(this.full_button_Click);
            // 
            // custom_button
            // 
            this.custom_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.custom_button.IconSize = new System.Drawing.Size(13, 13);
            this.custom_button.IconSvg = resources.GetString("custom_button.IconSvg");
            this.custom_button.Location = new System.Drawing.Point(231, 91);
            this.custom_button.Name = "custom_button";
            this.custom_button.Size = new System.Drawing.Size(100, 32);
            this.custom_button.TabIndex = 36;
            this.custom_button.Text = "自定义查杀";
            this.custom_button.Type = AntdUI.TTypeMini.Success;
            this.custom_button.WaveSize = 0;
            this.custom_button.Click += new System.EventHandler(this.custom_button_Click);
            // 
            // pause_button
            // 
            this.pause_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pause_button.IconSize = new System.Drawing.Size(12, 12);
            this.pause_button.IconSvg = resources.GetString("pause_button.IconSvg");
            this.pause_button.Location = new System.Drawing.Point(345, 91);
            this.pause_button.Name = "pause_button";
            this.pause_button.Size = new System.Drawing.Size(100, 32);
            this.pause_button.TabIndex = 37;
            this.pause_button.Text = "暂停查杀";
            this.pause_button.Type = AntdUI.TTypeMini.Warn;
            this.pause_button.Visible = false;
            this.pause_button.WaveSize = 0;
            this.pause_button.Click += new System.EventHandler(this.pause_button_Click);
            // 
            // stop_button
            // 
            this.stop_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stop_button.IconSize = new System.Drawing.Size(15, 15);
            this.stop_button.IconSvg = resources.GetString("stop_button.IconSvg");
            this.stop_button.Location = new System.Drawing.Point(459, 91);
            this.stop_button.Name = "stop_button";
            this.stop_button.Size = new System.Drawing.Size(100, 32);
            this.stop_button.TabIndex = 38;
            this.stop_button.Text = "结束查杀";
            this.stop_button.Type = AntdUI.TTypeMini.Error;
            this.stop_button.Visible = false;
            this.stop_button.WaveSize = 0;
            this.stop_button.Click += new System.EventHandler(this.stop_button_Click);
            // 
            // time_label
            // 
            this.time_label.BackColor = System.Drawing.Color.Transparent;
            this.time_label.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.time_label.Location = new System.Drawing.Point(760, 98);
            this.time_label.Name = "time_label";
            this.time_label.Size = new System.Drawing.Size(155, 23);
            this.time_label.TabIndex = 39;
            this.time_label.Text = "";
            this.time_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // table
            // 
            this.table.EmptyHeader = true;
            this.table.EmptyText = "暂无数据";
            this.table.Location = new System.Drawing.Point(3, 139);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(912, 443);
            this.table.TabIndex = 40;
            this.table.CellClick += new AntdUI.Table.ClickEventHandler(this.table_CellClick);
            // 
            // scan_timer
            // 
            this.scan_timer.Interval = 1000;
            this.scan_timer.Tick += new System.EventHandler(this.scan_timer_Tick);
            // 
            // continue_button
            // 
            this.continue_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.continue_button.IconSize = new System.Drawing.Size(12, 12);
            this.continue_button.IconSvg = resources.GetString("continue_button.IconSvg");
            this.continue_button.Location = new System.Drawing.Point(345, 91);
            this.continue_button.Name = "continue_button";
            this.continue_button.Size = new System.Drawing.Size(100, 32);
            this.continue_button.TabIndex = 42;
            this.continue_button.Text = "继续查杀";
            this.continue_button.Type = AntdUI.TTypeMini.Success;
            this.continue_button.Visible = false;
            this.continue_button.WaveSize = 0;
            this.continue_button.Click += new System.EventHandler(this.continue_button_Click);
            // 
            // ScanView
            // 
            this.Controls.Add(this.continue_button);
            this.Controls.Add(this.table);
            this.Controls.Add(this.time_label);
            this.Controls.Add(this.stop_button);
            this.Controls.Add(this.pause_button);
            this.Controls.Add(this.custom_button);
            this.Controls.Add(this.full_button);
            this.Controls.Add(this.quick_button);
            this.Controls.Add(this.header);
            this.Name = "ScanView";
            this.Size = new System.Drawing.Size(928, 598);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader header;
        private AntdUI.Button quick_button;
        private AntdUI.Button full_button;
        private AntdUI.Button custom_button;
        private AntdUI.Button pause_button;
        private AntdUI.Button stop_button;
        private AntdUI.Label time_label;
        private AntdUI.Table table;
        private System.Windows.Forms.Timer scan_timer;
        private AntdUI.Button continue_button;
    }
}
