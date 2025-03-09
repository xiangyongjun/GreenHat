namespace GreenHat
{
    partial class CloudMarkForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloudMarkForm));
            this.titlebar = new AntdUI.PageHeader();
            this.type_label = new AntdUI.Label();
            this.filesize_label = new AntdUI.Label();
            this.createtime_label = new AntdUI.Label();
            this.filetype_label = new AntdUI.Label();
            this.sha256_label = new AntdUI.Label();
            this.cloudtime_label = new AntdUI.Label();
            this.result_label = new AntdUI.Label();
            this.path_label = new AntdUI.Label();
            this.tag = new AntdUI.Tag();
            this.divider1 = new AntdUI.Divider();
            this.label2 = new AntdUI.Label();
            this.md5_label = new AntdUI.Label();
            this.label4 = new AntdUI.Label();
            this.table = new AntdUI.Table();
            this.divider2 = new AntdUI.Divider();
            this.open_button = new AntdUI.Button();
            this.clear_button = new AntdUI.Button();
            this.SuspendLayout();
            // 
            // titlebar
            // 
            this.titlebar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlebar.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titlebar.Icon = ((System.Drawing.Image)(resources.GetObject("titlebar.Icon")));
            this.titlebar.Location = new System.Drawing.Point(0, 0);
            this.titlebar.MaximizeBox = false;
            this.titlebar.MinimizeBox = false;
            this.titlebar.Name = "titlebar";
            this.titlebar.ShowButton = true;
            this.titlebar.ShowIcon = true;
            this.titlebar.Size = new System.Drawing.Size(730, 40);
            this.titlebar.SubText = "";
            this.titlebar.TabIndex = 1;
            this.titlebar.Text = "猎剑文件鉴定云";
            // 
            // type_label
            // 
            this.type_label.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.type_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.type_label.Location = new System.Drawing.Point(10, 46);
            this.type_label.Name = "type_label";
            this.type_label.Size = new System.Drawing.Size(113, 32);
            this.type_label.TabIndex = 2;
            this.type_label.Text = "等待鉴定：";
            // 
            // filesize_label
            // 
            this.filesize_label.AutoEllipsis = true;
            this.filesize_label.Location = new System.Drawing.Point(14, 171);
            this.filesize_label.Name = "filesize_label";
            this.filesize_label.Size = new System.Drawing.Size(335, 23);
            this.filesize_label.TabIndex = 3;
            this.filesize_label.Text = "文件大小：";
            // 
            // createtime_label
            // 
            this.createtime_label.AutoEllipsis = true;
            this.createtime_label.Location = new System.Drawing.Point(14, 200);
            this.createtime_label.Name = "createtime_label";
            this.createtime_label.Size = new System.Drawing.Size(335, 23);
            this.createtime_label.TabIndex = 4;
            this.createtime_label.Text = "创建日期：";
            // 
            // filetype_label
            // 
            this.filetype_label.AutoEllipsis = true;
            this.filetype_label.Location = new System.Drawing.Point(383, 171);
            this.filetype_label.Name = "filetype_label";
            this.filetype_label.Size = new System.Drawing.Size(335, 23);
            this.filetype_label.TabIndex = 5;
            this.filetype_label.Text = "文件类型：";
            // 
            // sha256_label
            // 
            this.sha256_label.AutoEllipsis = true;
            this.sha256_label.Location = new System.Drawing.Point(14, 229);
            this.sha256_label.Name = "sha256_label";
            this.sha256_label.Size = new System.Drawing.Size(335, 23);
            this.sha256_label.TabIndex = 6;
            this.sha256_label.Text = "SHA256：";
            // 
            // cloudtime_label
            // 
            this.cloudtime_label.AutoEllipsis = true;
            this.cloudtime_label.Location = new System.Drawing.Point(14, 258);
            this.cloudtime_label.Name = "cloudtime_label";
            this.cloudtime_label.Size = new System.Drawing.Size(335, 23);
            this.cloudtime_label.TabIndex = 8;
            this.cloudtime_label.Text = "鉴定时间：";
            // 
            // result_label
            // 
            this.result_label.AutoEllipsis = true;
            this.result_label.Location = new System.Drawing.Point(383, 229);
            this.result_label.Name = "result_label";
            this.result_label.Size = new System.Drawing.Size(335, 23);
            this.result_label.TabIndex = 12;
            this.result_label.Text = "鉴定结果：";
            // 
            // path_label
            // 
            this.path_label.AutoEllipsis = true;
            this.path_label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.path_label.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.path_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.path_label.Location = new System.Drawing.Point(90, 87);
            this.path_label.Name = "path_label";
            this.path_label.Size = new System.Drawing.Size(628, 23);
            this.path_label.TabIndex = 17;
            this.path_label.Text = "";
            this.path_label.Click += new System.EventHandler(this.path_label_Click);
            // 
            // tag
            // 
            this.tag.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.tag.Location = new System.Drawing.Point(121, 49);
            this.tag.Name = "tag";
            this.tag.Size = new System.Drawing.Size(14, 8);
            this.tag.TabIndex = 21;
            this.tag.Text = "";
            this.tag.Type = AntdUI.TTypeMini.Success;
            this.tag.Visible = false;
            // 
            // divider1
            // 
            this.divider1.Location = new System.Drawing.Point(14, 113);
            this.divider1.Name = "divider1";
            this.divider1.OrientationMargin = 0F;
            this.divider1.Size = new System.Drawing.Size(704, 23);
            this.divider1.TabIndex = 22;
            this.divider1.Text = "";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 32);
            this.label2.TabIndex = 23;
            this.label2.Text = "文件信息";
            // 
            // md5_label
            // 
            this.md5_label.AutoEllipsis = true;
            this.md5_label.Location = new System.Drawing.Point(383, 200);
            this.md5_label.Name = "md5_label";
            this.md5_label.Size = new System.Drawing.Size(335, 23);
            this.md5_label.TabIndex = 24;
            this.md5_label.Text = "文件摘要：";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 23);
            this.label4.TabIndex = 25;
            this.label4.Text = "文件路径：";
            // 
            // table
            // 
            this.table.Location = new System.Drawing.Point(14, 314);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(704, 311);
            this.table.TabIndex = 26;
            this.table.CellClick += new AntdUI.Table.ClickEventHandler(this.table_CellClick);
            // 
            // divider2
            // 
            this.divider2.Location = new System.Drawing.Point(14, 285);
            this.divider2.Name = "divider2";
            this.divider2.OrientationMargin = 0F;
            this.divider2.Size = new System.Drawing.Size(704, 23);
            this.divider2.TabIndex = 27;
            this.divider2.Text = "";
            // 
            // open_button
            // 
            this.open_button.IconSvg = resources.GetString("open_button.IconSvg");
            this.open_button.Location = new System.Drawing.Point(10, 635);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(350, 44);
            this.open_button.TabIndex = 28;
            this.open_button.Text = "选择文件鉴定";
            this.open_button.Type = AntdUI.TTypeMini.Success;
            this.open_button.Click += new System.EventHandler(this.open_button_Click);
            // 
            // clear_button
            // 
            this.clear_button.IconSvg = resources.GetString("clear_button.IconSvg");
            this.clear_button.Location = new System.Drawing.Point(368, 635);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(350, 44);
            this.clear_button.TabIndex = 29;
            this.clear_button.Text = "清空鉴定记录";
            this.clear_button.Type = AntdUI.TTypeMini.Error;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // CloudMarkForm
            // 
            this.ClientSize = new System.Drawing.Size(730, 690);
            this.Controls.Add(this.clear_button);
            this.Controls.Add(this.open_button);
            this.Controls.Add(this.divider2);
            this.Controls.Add(this.table);
            this.Controls.Add(this.path_label);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.md5_label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.divider1);
            this.Controls.Add(this.tag);
            this.Controls.Add(this.result_label);
            this.Controls.Add(this.cloudtime_label);
            this.Controls.Add(this.sha256_label);
            this.Controls.Add(this.filetype_label);
            this.Controls.Add(this.createtime_label);
            this.Controls.Add(this.filesize_label);
            this.Controls.Add(this.type_label);
            this.Controls.Add(this.titlebar);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "CloudMarkForm";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GreenHat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AntdUI.PageHeader titlebar;
        private AntdUI.Label type_label;
        private AntdUI.Label filesize_label;
        private AntdUI.Label createtime_label;
        private AntdUI.Label filetype_label;
        private AntdUI.Label sha256_label;
        private AntdUI.Label cloudtime_label;
        private AntdUI.Label result_label;
        private AntdUI.Label path_label;
        private AntdUI.Tag tag;
        private AntdUI.Divider divider1;
        private AntdUI.Label label2;
        private AntdUI.Label md5_label;
        private AntdUI.Label label4;
        private AntdUI.Table table;
        private AntdUI.Divider divider2;
        private AntdUI.Button open_button;
        private AntdUI.Button clear_button;
    }
}