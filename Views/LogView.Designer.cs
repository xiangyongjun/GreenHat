namespace GreenHat.Views
{
    partial class LogView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogView));
            this.header = new AntdUI.PageHeader();
            this.table = new AntdUI.Table();
            this.date_range = new AntdUI.DatePickerRange();
            this.type_select = new AntdUI.Select();
            this.export_button = new AntdUI.Button();
            this.clear_button = new AntdUI.Button();
            this.count_label = new AntdUI.Label();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.DividerShow = true;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Enabled = false;
            this.header.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.header.Size = new System.Drawing.Size(928, 74);
            this.header.TabIndex = 29;
            this.header.Text = "安全日志";
            this.header.UseTitleFont = true;
            // 
            // table
            // 
            this.table.EmptyHeader = true;
            this.table.EmptyText = "暂无数据";
            this.table.Location = new System.Drawing.Point(3, 135);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(912, 447);
            this.table.TabIndex = 40;
            // 
            // date_range
            // 
            this.date_range.AllowClear = true;
            this.date_range.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.date_range.Location = new System.Drawing.Point(3, 89);
            this.date_range.Name = "date_range";
            this.date_range.PlaceholderEnd = "结束日期";
            this.date_range.PlaceholderStart = "开始日期";
            this.date_range.SelectionColor = System.Drawing.Color.Empty;
            this.date_range.Size = new System.Drawing.Size(300, 32);
            this.date_range.TabIndex = 41;
            this.date_range.WaveSize = 0;
            this.date_range.ValueChanged += new AntdUI.DateTimesEventHandler(this.date_range_ValueChanged);
            // 
            // type_select
            // 
            this.type_select.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.type_select.Items.AddRange(new object[] {
            "全部",
            "病毒防护",
            "其他"});
            this.type_select.List = true;
            this.type_select.Location = new System.Drawing.Point(317, 89);
            this.type_select.Name = "type_select";
            this.type_select.PlaceholderText = "";
            this.type_select.SelectedIndex = 0;
            this.type_select.SelectedValue = "全部";
            this.type_select.Size = new System.Drawing.Size(150, 32);
            this.type_select.TabIndex = 42;
            this.type_select.Text = "全部";
            this.type_select.WaveSize = 0;
            this.type_select.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.type_select_SelectedValueChanged);
            // 
            // export_button
            // 
            this.export_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.export_button.IconSize = new System.Drawing.Size(15, 15);
            this.export_button.IconSvg = resources.GetString("export_button.IconSvg");
            this.export_button.Location = new System.Drawing.Point(481, 89);
            this.export_button.Name = "export_button";
            this.export_button.Size = new System.Drawing.Size(100, 32);
            this.export_button.TabIndex = 43;
            this.export_button.Text = "导出日志";
            this.export_button.Type = AntdUI.TTypeMini.Success;
            this.export_button.WaveSize = 0;
            this.export_button.Click += new System.EventHandler(this.export_button_Click);
            // 
            // clear_button
            // 
            this.clear_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clear_button.IconSize = new System.Drawing.Size(13, 13);
            this.clear_button.IconSvg = resources.GetString("clear_button.IconSvg");
            this.clear_button.Location = new System.Drawing.Point(595, 89);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(100, 32);
            this.clear_button.TabIndex = 44;
            this.clear_button.Text = "清空日志";
            this.clear_button.Type = AntdUI.TTypeMini.Error;
            this.clear_button.WaveSize = 0;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // count_label
            // 
            this.count_label.BackColor = System.Drawing.Color.Transparent;
            this.count_label.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.count_label.Location = new System.Drawing.Point(763, 97);
            this.count_label.Name = "count_label";
            this.count_label.Size = new System.Drawing.Size(152, 23);
            this.count_label.TabIndex = 45;
            this.count_label.Text = "日志条数：0";
            this.count_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LogView
            // 
            this.Controls.Add(this.count_label);
            this.Controls.Add(this.clear_button);
            this.Controls.Add(this.export_button);
            this.Controls.Add(this.type_select);
            this.Controls.Add(this.date_range);
            this.Controls.Add(this.table);
            this.Controls.Add(this.header);
            this.Name = "LogView";
            this.Size = new System.Drawing.Size(928, 598);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader header;
        private AntdUI.Table table;
        private AntdUI.DatePickerRange date_range;
        private AntdUI.Select type_select;
        private AntdUI.Button export_button;
        private AntdUI.Button clear_button;
        private AntdUI.Label count_label;
    }
}
