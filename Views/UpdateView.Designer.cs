namespace GreenHat.Views
{
    partial class UpdateView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateView));
            this.old_label = new AntdUI.Label();
            this.new_label = new AntdUI.Label();
            this.progress = new AntdUI.Progress();
            this.update_button = new AntdUI.Button();
            this.download_label = new AntdUI.Label();
            this.SuspendLayout();
            // 
            // old_label
            // 
            this.old_label.Location = new System.Drawing.Point(3, 13);
            this.old_label.Name = "old_label";
            this.old_label.Size = new System.Drawing.Size(184, 23);
            this.old_label.TabIndex = 0;
            this.old_label.Text = "当前版本：";
            // 
            // new_label
            // 
            this.new_label.Location = new System.Drawing.Point(3, 42);
            this.new_label.Name = "new_label";
            this.new_label.Size = new System.Drawing.Size(184, 23);
            this.new_label.TabIndex = 1;
            this.new_label.Text = "最新版本：";
            // 
            // progress
            // 
            this.progress.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.progress.Location = new System.Drawing.Point(72, 71);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(421, 23);
            this.progress.TabIndex = 2;
            this.progress.Text = "";
            this.progress.Visible = false;
            // 
            // update_button
            // 
            this.update_button.Enabled = false;
            this.update_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.update_button.IconSize = new System.Drawing.Size(15, 15);
            this.update_button.IconSvg = resources.GetString("update_button.IconSvg");
            this.update_button.Location = new System.Drawing.Point(397, 108);
            this.update_button.Name = "update_button";
            this.update_button.Size = new System.Drawing.Size(100, 32);
            this.update_button.TabIndex = 44;
            this.update_button.Text = "立即更新";
            this.update_button.Type = AntdUI.TTypeMini.Success;
            this.update_button.WaveSize = 0;
            this.update_button.Click += new System.EventHandler(this.update_button_Click);
            // 
            // download_label
            // 
            this.download_label.Location = new System.Drawing.Point(3, 71);
            this.download_label.Name = "download_label";
            this.download_label.Size = new System.Drawing.Size(67, 23);
            this.download_label.TabIndex = 45;
            this.download_label.Text = "下载进度：";
            this.download_label.Visible = false;
            // 
            // UpdateView
            // 
            this.Controls.Add(this.download_label);
            this.Controls.Add(this.update_button);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.new_label);
            this.Controls.Add(this.old_label);
            this.Name = "UpdateView";
            this.Size = new System.Drawing.Size(500, 143);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Label old_label;
        private AntdUI.Label new_label;
        private AntdUI.Progress progress;
        private AntdUI.Button update_button;
        private AntdUI.Label download_label;
    }
}
