namespace GreenHat.Views
{
    partial class AboutView
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
            this.header = new AntdUI.PageHeader();
            this.table = new AntdUI.Table();
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
            this.header.Text = "关于作者";
            this.header.UseTitleFont = true;
            // 
            // table
            // 
            this.table.EmptyHeader = true;
            this.table.EmptyText = "暂无数据";
            this.table.Location = new System.Drawing.Point(3, 86);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(912, 482);
            this.table.TabIndex = 41;
            this.table.VisibleHeader = false;
            // 
            // AboutView
            // 
            this.Controls.Add(this.table);
            this.Controls.Add(this.header);
            this.Name = "AboutView";
            this.Size = new System.Drawing.Size(928, 598);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader header;
        private AntdUI.Table table;
    }
}
