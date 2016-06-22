namespace JCZF.SubFrame
{
    partial class frmShowPic
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.m_btnSave = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label25 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.listView2 = new System.Windows.Forms.ListView();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupBox7.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(182, 279);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Location = new System.Drawing.Point(194, 3);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(539, 425);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 3;
            this.panelEx1.Text = "panelEx1";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.m_btnSave);
            this.groupBox7.Controls.Add(this.textBox4);
            this.groupBox7.Controls.Add(this.label27);
            this.groupBox7.Controls.Add(this.textBox3);
            this.groupBox7.Controls.Add(this.label26);
            this.groupBox7.Controls.Add(this.dateTimePicker1);
            this.groupBox7.Controls.Add(this.label25);
            this.groupBox7.Controls.Add(this.textBox2);
            this.groupBox7.Controls.Add(this.label24);
            this.groupBox7.Controls.Add(this.button1);
            this.groupBox7.Controls.Add(this.textBox1);
            this.groupBox7.Controls.Add(this.label23);
            this.groupBox7.Location = new System.Drawing.Point(194, 433);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(539, 128);
            this.groupBox7.TabIndex = 41;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "上传资料";
            // 
            // m_btnSave
            // 
            this.m_btnSave.Location = new System.Drawing.Point(404, 93);
            this.m_btnSave.Name = "m_btnSave";
            this.m_btnSave.Size = new System.Drawing.Size(75, 23);
            this.m_btnSave.TabIndex = 40;
            this.m_btnSave.Text = "保存";
            this.m_btnSave.UseVisualStyleBackColor = true;
            this.m_btnSave.Click += new System.EventHandler(this.m_btnSave_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(52, 101);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(243, 21);
            this.textBox4.TabIndex = 39;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(9, 104);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(29, 12);
            this.label27.TabIndex = 38;
            this.label27.Text = "概述";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(227, 56);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(198, 21);
            this.textBox3.TabIndex = 37;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(181, 60);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(29, 12);
            this.label26.TabIndex = 36;
            this.label26.Text = "单位";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(46, 56);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(110, 21);
            this.dateTimePicker1.TabIndex = 35;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(11, 61);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(29, 12);
            this.label25.TabIndex = 34;
            this.label25.Text = "时间";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(450, 18);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(70, 21);
            this.textBox2.TabIndex = 33;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(414, 20);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(29, 12);
            this.label24.TabIndex = 32;
            this.label24.Text = "作者";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(341, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 21);
            this.button1.TabIndex = 31;
            this.button1.Text = "选择";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(263, 21);
            this.textBox1.TabIndex = 30;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(7, 21);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(59, 12);
            this.label23.TabIndex = 29;
            this.label23.Text = "照片/视频";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "选择图片";
            this.openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF,JPEG)|*.BMP;*.JPG;*.GIF;JPEG|Video Files(*.AVI,*.MP" +
                "3,*.RMVB)|*.AVI;*.MP3;*.RMVB|All files (*.*)|*.*";
            this.openFileDialog1.ReadOnlyChecked = true;
            // 
            // listView2
            // 
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.Location = new System.Drawing.Point(0, 279);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(182, 276);
            this.listView2.TabIndex = 42;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.DoubleClick += new System.EventHandler(this.listView2_DoubleClick);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.listView2);
            this.groupPanel1.Controls.Add(this.listView1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(188, 561);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 42;
            // 
            // frmShowPic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 561);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.panelEx1);
            this.Name = "frmShowPic";
            this.Text = "显示图片";
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button m_btnSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListView listView2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;




    }
}