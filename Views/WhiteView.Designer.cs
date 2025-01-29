namespace GreenHat.Views
{
    partial class WhiteView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhiteView));
            this.table = new AntdUI.Table();
            this.add_button = new AntdUI.Button();
            this.remove_button = new AntdUI.Button();
            this.path_input = new AntdUI.Input();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.EmptyHeader = true;
            this.table.EmptyText = "暂无数据";
            this.table.Location = new System.Drawing.Point(1, 63);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(926, 447);
            this.table.TabIndex = 40;
            // 
            // add_button
            // 
            this.add_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.add_button.IconSize = new System.Drawing.Size(20, 20);
            this.add_button.IconSvg = resources.GetString("add_button.IconSvg");
            this.add_button.Location = new System.Drawing.Point(215, 15);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(100, 32);
            this.add_button.TabIndex = 43;
            this.add_button.Text = "添加文件";
            this.add_button.Type = AntdUI.TTypeMini.Success;
            this.add_button.WaveSize = 0;
            this.add_button.Click += new System.EventHandler(this.add_button_Click);
            // 
            // remove_button
            // 
            this.remove_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.remove_button.IconSize = new System.Drawing.Size(13, 13);
            this.remove_button.IconSvg = resources.GetString("remove_button.IconSvg");
            this.remove_button.Location = new System.Drawing.Point(329, 15);
            this.remove_button.Name = "remove_button";
            this.remove_button.Size = new System.Drawing.Size(100, 32);
            this.remove_button.TabIndex = 44;
            this.remove_button.Text = "删除所选";
            this.remove_button.Type = AntdUI.TTypeMini.Error;
            this.remove_button.WaveSize = 0;
            this.remove_button.Click += new System.EventHandler(this.remove_button_Click);
            // 
            // path_input
            // 
            this.path_input.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.path_input.Location = new System.Drawing.Point(1, 15);
            this.path_input.Name = "path_input";
            this.path_input.PlaceholderText = "搜索";
            this.path_input.PrefixSvg = "";
            this.path_input.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.path_input.Size = new System.Drawing.Size(200, 32);
            this.path_input.SuffixSvg = resources.GetString("path_input.SuffixSvg");
            this.path_input.TabIndex = 45;
            this.path_input.WaveSize = 0;
            this.path_input.TextChanged += new System.EventHandler(this.path_input_TextChanged);
            // 
            // WhiteView
            // 
            this.Controls.Add(this.path_input);
            this.Controls.Add(this.remove_button);
            this.Controls.Add(this.add_button);
            this.Controls.Add(this.table);
            this.Name = "WhiteView";
            this.Size = new System.Drawing.Size(928, 510);
            this.ResumeLayout(false);

        }

        #endregion
        private AntdUI.Table table;
        private AntdUI.Button add_button;
        private AntdUI.Button remove_button;
        private AntdUI.Input path_input;
    }
}
