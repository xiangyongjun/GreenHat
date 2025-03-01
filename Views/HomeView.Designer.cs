namespace GreenHat.Views
{
    partial class HomeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeView));
            this.header = new AntdUI.PageHeader();
            this.scan_button = new AntdUI.Dropdown();
            this.update_button = new AntdUI.Button();
            this.white_button = new AntdUI.Button();
            this.black_button = new AntdUI.Button();
            this.table = new AntdUI.Table();
            this.mem_progress = new AntdUI.Progress();
            this.cpu_progress = new AntdUI.Progress();
            this.label2 = new AntdUI.Label();
            this.label3 = new AntdUI.Label();
            this.label4 = new AntdUI.Label();
            this.disk_progress = new AntdUI.Progress();
            this.update_timer = new System.Windows.Forms.Timer(this.components);
            this.count_label = new AntdUI.Label();
            this.github_button = new AntdUI.Button();
            this.deepseek_button = new AntdUI.Dropdown();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.Description = "绿帽子安全防护正在保护您的电脑安全";
            this.header.DividerShow = true;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.header.Size = new System.Drawing.Size(928, 74);
            this.header.TabIndex = 28;
            this.header.Text = "已保护 1 天";
            this.header.UseTitleFont = true;
            // 
            // scan_button
            // 
            this.scan_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.scan_button.IconRatio = 1F;
            this.scan_button.IconSize = new System.Drawing.Size(30, 30);
            this.scan_button.IconSvg = resources.GetString("scan_button.IconSvg");
            this.scan_button.Location = new System.Drawing.Point(10, 101);
            this.scan_button.Name = "scan_button";
            this.scan_button.Placement = AntdUI.TAlignFrom.BR;
            this.scan_button.RespondRealAreas = true;
            this.scan_button.ShowArrow = true;
            this.scan_button.Size = new System.Drawing.Size(206, 50);
            this.scan_button.TabIndex = 29;
            this.scan_button.Text = "开始查杀";
            this.scan_button.Type = AntdUI.TTypeMini.Success;
            this.scan_button.WaveSize = 0;
            this.scan_button.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.scan_button_SelectedValueChanged);
            // 
            // update_button
            // 
            this.update_button.BorderWidth = 2F;
            this.update_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.update_button.Ghost = true;
            this.update_button.IconSize = new System.Drawing.Size(15, 15);
            this.update_button.IconSvg = resources.GetString("update_button.IconSvg");
            this.update_button.Location = new System.Drawing.Point(599, 543);
            this.update_button.Name = "update_button";
            this.update_button.Size = new System.Drawing.Size(100, 38);
            this.update_button.TabIndex = 32;
            this.update_button.Text = "检查更新";
            this.update_button.Type = AntdUI.TTypeMini.Success;
            this.update_button.Click += new System.EventHandler(this.update_button_Click);
            // 
            // white_button
            // 
            this.white_button.BorderWidth = 2F;
            this.white_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.white_button.Ghost = true;
            this.white_button.IconSize = new System.Drawing.Size(15, 15);
            this.white_button.IconSvg = resources.GetString("white_button.IconSvg");
            this.white_button.Location = new System.Drawing.Point(705, 543);
            this.white_button.Name = "white_button";
            this.white_button.Size = new System.Drawing.Size(100, 38);
            this.white_button.TabIndex = 33;
            this.white_button.Text = "信任区";
            this.white_button.Type = AntdUI.TTypeMini.Success;
            this.white_button.Click += new System.EventHandler(this.white_button_Click);
            // 
            // black_button
            // 
            this.black_button.BorderWidth = 2F;
            this.black_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.black_button.Ghost = true;
            this.black_button.IconSize = new System.Drawing.Size(15, 15);
            this.black_button.IconSvg = resources.GetString("black_button.IconSvg");
            this.black_button.Location = new System.Drawing.Point(811, 543);
            this.black_button.Name = "black_button";
            this.black_button.Size = new System.Drawing.Size(100, 38);
            this.black_button.TabIndex = 34;
            this.black_button.Text = "隔离区";
            this.black_button.Type = AntdUI.TTypeMini.Success;
            this.black_button.Click += new System.EventHandler(this.black_button_Click);
            // 
            // table
            // 
            this.table.EmptyHeader = true;
            this.table.EmptyText = "加载中";
            this.table.Enabled = false;
            this.table.Location = new System.Drawing.Point(10, 206);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(900, 315);
            this.table.TabIndex = 35;
            this.table.VisibleHeader = false;
            // 
            // mem_progress
            // 
            this.mem_progress.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.mem_progress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mem_progress.Location = new System.Drawing.Point(717, 96);
            this.mem_progress.Name = "mem_progress";
            this.mem_progress.Shape = AntdUI.TShapeProgress.Circle;
            this.mem_progress.Size = new System.Drawing.Size(76, 70);
            this.mem_progress.TabIndex = 49;
            this.mem_progress.Text = "";
            // 
            // cpu_progress
            // 
            this.cpu_progress.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.cpu_progress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cpu_progress.Location = new System.Drawing.Point(598, 96);
            this.cpu_progress.Name = "cpu_progress";
            this.cpu_progress.Shape = AntdUI.TShapeProgress.Circle;
            this.cpu_progress.Size = new System.Drawing.Size(76, 70);
            this.cpu_progress.TabIndex = 50;
            this.cpu_progress.Text = "";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label2.Location = new System.Drawing.Point(598, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 23);
            this.label2.TabIndex = 51;
            this.label2.Text = "处理器占用";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label3.Location = new System.Drawing.Point(717, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 23);
            this.label3.TabIndex = 52;
            this.label3.Text = "内存占用";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label4.Location = new System.Drawing.Point(835, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 23);
            this.label4.TabIndex = 54;
            this.label4.Text = "硬盘占用";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // disk_progress
            // 
            this.disk_progress.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.disk_progress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.disk_progress.Location = new System.Drawing.Point(835, 96);
            this.disk_progress.Name = "disk_progress";
            this.disk_progress.Shape = AntdUI.TShapeProgress.Circle;
            this.disk_progress.Size = new System.Drawing.Size(76, 70);
            this.disk_progress.TabIndex = 53;
            this.disk_progress.Text = "";
            // 
            // update_timer
            // 
            this.update_timer.Enabled = true;
            this.update_timer.Interval = 3000;
            this.update_timer.Tick += new System.EventHandler(this.update_timer_Tick);
            // 
            // count_label
            // 
            this.count_label.BackColor = System.Drawing.Color.Transparent;
            this.count_label.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.count_label.Location = new System.Drawing.Point(10, 551);
            this.count_label.Name = "count_label";
            this.count_label.Size = new System.Drawing.Size(235, 23);
            this.count_label.TabIndex = 55;
            this.count_label.Text = "隔离数量：0个";
            // 
            // github_button
            // 
            this.github_button.BorderWidth = 2F;
            this.github_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.github_button.Ghost = true;
            this.github_button.IconSize = new System.Drawing.Size(15, 15);
            this.github_button.IconSvg = resources.GetString("github_button.IconSvg");
            this.github_button.Location = new System.Drawing.Point(493, 543);
            this.github_button.Name = "github_button";
            this.github_button.Size = new System.Drawing.Size(100, 38);
            this.github_button.TabIndex = 56;
            this.github_button.Text = "官方主页";
            this.github_button.Type = AntdUI.TTypeMini.Success;
            this.github_button.Click += new System.EventHandler(this.github_button_Click);
            // 
            // deepseek_button
            // 
            this.deepseek_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.deepseek_button.IconRatio = 1F;
            this.deepseek_button.IconSize = new System.Drawing.Size(30, 30);
            this.deepseek_button.IconSvg = resources.GetString("deepseek_button.IconSvg");
            this.deepseek_button.ListAutoWidth = false;
            this.deepseek_button.Location = new System.Drawing.Point(242, 101);
            this.deepseek_button.Name = "deepseek_button";
            this.deepseek_button.Placement = AntdUI.TAlignFrom.BR;
            this.deepseek_button.Size = new System.Drawing.Size(291, 50);
            this.deepseek_button.TabIndex = 57;
            this.deepseek_button.Text = "DeepSeek 恶意文件分析";
            this.deepseek_button.Type = AntdUI.TTypeMini.Success;
            this.deepseek_button.WaveSize = 0;
            this.deepseek_button.Click += new System.EventHandler(this.deepseek_button_Click);
            // 
            // HomeView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.deepseek_button);
            this.Controls.Add(this.github_button);
            this.Controls.Add(this.count_label);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.disk_progress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cpu_progress);
            this.Controls.Add(this.mem_progress);
            this.Controls.Add(this.table);
            this.Controls.Add(this.black_button);
            this.Controls.Add(this.white_button);
            this.Controls.Add(this.update_button);
            this.Controls.Add(this.scan_button);
            this.Controls.Add(this.header);
            this.Name = "HomeView";
            this.Size = new System.Drawing.Size(928, 598);
            this.Load += new System.EventHandler(this.Home_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader header;
        private AntdUI.Dropdown scan_button;
        private AntdUI.Button update_button;
        private AntdUI.Button white_button;
        private AntdUI.Button black_button;
        private AntdUI.Table table;
        private AntdUI.Progress mem_progress;
        private AntdUI.Progress cpu_progress;
        private AntdUI.Label label2;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Progress disk_progress;
        private System.Windows.Forms.Timer update_timer;
        private AntdUI.Label count_label;
        private AntdUI.Button github_button;
        private AntdUI.Dropdown deepseek_button;
    }
}
