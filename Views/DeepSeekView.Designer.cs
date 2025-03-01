namespace GreenHat.Views
{
    partial class DeepSeekView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeepSeekView));
            this.file_button = new AntdUI.Button();
            this.input = new AntdUI.Input();
            this.SuspendLayout();
            // 
            // file_button
            // 
            this.file_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.file_button.IconSize = new System.Drawing.Size(14, 14);
            this.file_button.IconSvg = resources.GetString("file_button.IconSvg");
            this.file_button.Location = new System.Drawing.Point(0, 17);
            this.file_button.Name = "file_button";
            this.file_button.Size = new System.Drawing.Size(153, 32);
            this.file_button.TabIndex = 35;
            this.file_button.Text = "选择需要分析的文件";
            this.file_button.Type = AntdUI.TTypeMini.Success;
            this.file_button.WaveSize = 0;
            this.file_button.Click += new System.EventHandler(this.file_button_Click);
            // 
            // input
            // 
            this.input.AutoScroll = true;
            this.input.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.input.Location = new System.Drawing.Point(-4, 66);
            this.input.Margin = new System.Windows.Forms.Padding(0);
            this.input.Multiline = true;
            this.input.Name = "input";
            this.input.PlaceholderText = "";
            this.input.ReadOnly = true;
            this.input.Size = new System.Drawing.Size(936, 448);
            this.input.TabIndex = 36;
            // 
            // DeepSeekView
            // 
            this.Controls.Add(this.input);
            this.Controls.Add(this.file_button);
            this.Name = "DeepSeekView";
            this.Size = new System.Drawing.Size(928, 510);
            this.ResumeLayout(false);

        }

        #endregion
        private AntdUI.Button file_button;
        private AntdUI.Input input;
    }
}
