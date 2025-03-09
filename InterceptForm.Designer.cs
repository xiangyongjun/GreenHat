namespace GreenHat
{
    partial class InterceptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InterceptForm));
            this.titlebar = new AntdUI.PageHeader();
            this.label1 = new AntdUI.Label();
            this.divider1 = new AntdUI.Divider();
            this.label2 = new AntdUI.Label();
            this.divider2 = new AntdUI.Divider();
            this.black_button = new AntdUI.Button();
            this.pass_button = new AntdUI.Button();
            this.white_button = new AntdUI.Button();
            this.label3 = new AntdUI.Label();
            this.label4 = new AntdUI.Label();
            this.path_label = new AntdUI.Label();
            this.type_label = new AntdUI.Label();
            this.engine_label = new AntdUI.Label();
            this.label6 = new AntdUI.Label();
            this.SuspendLayout();
            // 
            // titlebar
            // 
            this.titlebar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlebar.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titlebar.Location = new System.Drawing.Point(0, 0);
            this.titlebar.MaximizeBox = false;
            this.titlebar.MinimizeBox = false;
            this.titlebar.Name = "titlebar";
            this.titlebar.ShowButton = true;
            this.titlebar.ShowIcon = true;
            this.titlebar.Size = new System.Drawing.Size(560, 40);
            this.titlebar.SubText = "";
            this.titlebar.TabIndex = 0;
            this.titlebar.Text = "实时监控";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "发现风险";
            // 
            // divider1
            // 
            this.divider1.Location = new System.Drawing.Point(12, 109);
            this.divider1.Name = "divider1";
            this.divider1.OrientationMargin = 0F;
            this.divider1.Size = new System.Drawing.Size(536, 23);
            this.divider1.TabIndex = 2;
            this.divider1.Text = "";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(14, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "建议您立即处理";
            // 
            // divider2
            // 
            this.divider2.Location = new System.Drawing.Point(12, 238);
            this.divider2.Name = "divider2";
            this.divider2.OrientationMargin = 0F;
            this.divider2.Size = new System.Drawing.Size(536, 23);
            this.divider2.TabIndex = 4;
            this.divider2.Text = "";
            // 
            // black_button
            // 
            this.black_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.black_button.IconRatio = 0F;
            this.black_button.IconSize = new System.Drawing.Size(16, 16);
            this.black_button.IconSvg = "";
            this.black_button.Location = new System.Drawing.Point(446, 263);
            this.black_button.Name = "black_button";
            this.black_button.Size = new System.Drawing.Size(100, 32);
            this.black_button.TabIndex = 44;
            this.black_button.Text = "立即处理";
            this.black_button.Type = AntdUI.TTypeMini.Error;
            this.black_button.WaveSize = 0;
            this.black_button.Click += new System.EventHandler(this.black_button_Click);
            // 
            // pass_button
            // 
            this.pass_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pass_button.IconRatio = 0F;
            this.pass_button.IconSize = new System.Drawing.Size(16, 16);
            this.pass_button.IconSvg = "";
            this.pass_button.Location = new System.Drawing.Point(218, 263);
            this.pass_button.Name = "pass_button";
            this.pass_button.Size = new System.Drawing.Size(100, 32);
            this.pass_button.TabIndex = 45;
            this.pass_button.Text = "暂不处理";
            this.pass_button.WaveSize = 0;
            this.pass_button.Click += new System.EventHandler(this.pass_button_Click);
            // 
            // white_button
            // 
            this.white_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.white_button.IconRatio = 0F;
            this.white_button.IconSize = new System.Drawing.Size(16, 16);
            this.white_button.IconSvg = "";
            this.white_button.Location = new System.Drawing.Point(332, 263);
            this.white_button.Name = "white_button";
            this.white_button.Size = new System.Drawing.Size(100, 32);
            this.white_button.TabIndex = 45;
            this.white_button.Text = "添加信任";
            this.white_button.Type = AntdUI.TTypeMini.Success;
            this.white_button.WaveSize = 0;
            this.white_button.Click += new System.EventHandler(this.white_button_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 23);
            this.label3.TabIndex = 46;
            this.label3.Text = "风险文件：";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 47;
            this.label4.Text = "风险类型：";
            // 
            // path_label
            // 
            this.path_label.AutoEllipsis = true;
            this.path_label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.path_label.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.path_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.path_label.Location = new System.Drawing.Point(93, 139);
            this.path_label.Name = "path_label";
            this.path_label.Size = new System.Drawing.Size(451, 23);
            this.path_label.TabIndex = 48;
            this.path_label.Text = "";
            this.path_label.Click += new System.EventHandler(this.path_label_Click);
            // 
            // type_label
            // 
            this.type_label.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.type_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.type_label.Location = new System.Drawing.Point(93, 173);
            this.type_label.Name = "type_label";
            this.type_label.Size = new System.Drawing.Size(451, 23);
            this.type_label.TabIndex = 49;
            this.type_label.Text = "";
            // 
            // engine_label
            // 
            this.engine_label.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.engine_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.engine_label.Location = new System.Drawing.Point(93, 209);
            this.engine_label.Name = "engine_label";
            this.engine_label.Size = new System.Drawing.Size(451, 23);
            this.engine_label.TabIndex = 51;
            this.engine_label.Text = "";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(14, 209);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 23);
            this.label6.TabIndex = 50;
            this.label6.Text = "查杀引擎：";
            // 
            // InterceptForm
            // 
            this.ClientSize = new System.Drawing.Size(560, 309);
            this.ControlBox = false;
            this.Controls.Add(this.engine_label);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.type_label);
            this.Controls.Add(this.path_label);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.white_button);
            this.Controls.Add(this.pass_button);
            this.Controls.Add(this.black_button);
            this.Controls.Add(this.divider2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.divider1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titlebar);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "InterceptForm";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GreenHat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InterceptForm_FormClosed);
            this.Shown += new System.EventHandler(this.InterceptForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader titlebar;
        private AntdUI.Label label1;
        private AntdUI.Divider divider1;
        private AntdUI.Label label2;
        private AntdUI.Divider divider2;
        private AntdUI.Button black_button;
        private AntdUI.Button pass_button;
        private AntdUI.Button white_button;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Label path_label;
        private AntdUI.Label type_label;
        private AntdUI.Label engine_label;
        private AntdUI.Label label6;
    }
}