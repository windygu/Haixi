namespace JCZF.SubFrame.WGGL
{
    partial class frmWCGL_JG_WG
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelEx_Button = new DevComponents.DotNetBar.PanelEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnRemove = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.btnExtend = new DevComponents.DotNetBar.ButtonX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DropDownListBJZFDW = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtBJZFRYBZ = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.DropDownListBJZFRY = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtXZQMC = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBJZFFZR = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSJZFFZR = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DropDownListSJZFDW = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtSJZFRYBZ = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.DropDownListSJZFRY = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.panelEx_Button.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx_Button
            // 
            this.panelEx_Button.AutoScroll = true;
            this.panelEx_Button.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx_Button.Controls.Add(this.panel2);
            this.panelEx_Button.Controls.Add(this.panel1);
            this.panelEx_Button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx_Button.Location = new System.Drawing.Point(0, 440);
            this.panelEx_Button.Name = "panelEx_Button";
            this.panelEx_Button.Size = new System.Drawing.Size(892, 171);
            this.panelEx_Button.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_Button.Style.BackColor1.Color = System.Drawing.Color.AliceBlue;
            this.panelEx_Button.Style.BackColor2.Color = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelEx_Button.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx_Button.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx_Button.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx_Button.Style.GradientAngle = 90;
            this.panelEx_Button.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Controls.Add(this.buttonX1);
            this.panel2.Controls.Add(this.btnExtend);
            this.panel2.Location = new System.Drawing.Point(170, 128);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 33);
            this.panel2.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(35, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "保存";
            this.btnSave.Tooltip = "保存用户选择的网格关系，如果以前有，则将以前的存入历史网格";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRemove.Location = new System.Drawing.Point(145, 7);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 20;
            this.btnRemove.Text = "删除网格";
            this.btnRemove.Tooltip = "删除执法网格";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(263, 7);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.TabIndex = 21;
            this.buttonX1.Text = "关闭";
            this.buttonX1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExtend
            // 
            this.btnExtend.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExtend.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExtend.Location = new System.Drawing.Point(373, 7);
            this.btnExtend.Name = "btnExtend";
            this.btnExtend.Size = new System.Drawing.Size(75, 23);
            this.btnExtend.TabIndex = 21;
            this.btnExtend.Text = "扩展单位";
            this.btnExtend.Click += new System.EventHandler(this.btnExtend_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Location = new System.Drawing.Point(0, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(848, 116);
            this.panel1.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DropDownListBJZFDW);
            this.groupBox1.Controls.Add(this.txtBJZFRYBZ);
            this.groupBox1.Controls.Add(this.labelX5);
            this.groupBox1.Controls.Add(this.DropDownListBJZFRY);
            this.groupBox1.Controls.Add(this.labelX7);
            this.groupBox1.Controls.Add(this.labelX8);
            this.groupBox1.Location = new System.Drawing.Point(234, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 107);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "本级执法人员";
            // 
            // DropDownListBJZFDW
            // 
            this.DropDownListBJZFDW.DisplayMember = "Text";
            this.DropDownListBJZFDW.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DropDownListBJZFDW.FormattingEnabled = true;
            this.DropDownListBJZFDW.ItemHeight = 15;
            this.DropDownListBJZFDW.Location = new System.Drawing.Point(84, 18);
            this.DropDownListBJZFDW.Name = "DropDownListBJZFDW";
            this.DropDownListBJZFDW.Size = new System.Drawing.Size(205, 21);
            this.DropDownListBJZFDW.TabIndex = 13;
            this.DropDownListBJZFDW.SelectedIndexChanged += new System.EventHandler(this.DropDownListBJZFDW_SelectedIndexChanged);
            // 
            // txtBJZFRYBZ
            // 
            // 
            // 
            // 
            this.txtBJZFRYBZ.Border.Class = "TextBoxBorder";
            this.txtBJZFRYBZ.Location = new System.Drawing.Point(84, 74);
            this.txtBJZFRYBZ.Name = "txtBJZFRYBZ";
            this.txtBJZFRYBZ.Size = new System.Drawing.Size(205, 21);
            this.txtBJZFRYBZ.TabIndex = 15;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(11, 18);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(82, 23);
            this.labelX5.TabIndex = 1;
            this.labelX5.Text = "执法 单 位：";
            // 
            // DropDownListBJZFRY
            // 
            this.DropDownListBJZFRY.DisplayMember = "Text";
            this.DropDownListBJZFRY.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DropDownListBJZFRY.FormattingEnabled = true;
            this.DropDownListBJZFRY.ItemHeight = 15;
            this.DropDownListBJZFRY.Location = new System.Drawing.Point(84, 46);
            this.DropDownListBJZFRY.Name = "DropDownListBJZFRY";
            this.DropDownListBJZFRY.Size = new System.Drawing.Size(205, 21);
            this.DropDownListBJZFRY.TabIndex = 14;
            this.DropDownListBJZFRY.SelectedIndexChanged += new System.EventHandler(this.DropDownListBJZFRY_SelectedIndexChanged);
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(11, 48);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(82, 23);
            this.labelX7.TabIndex = 1;
            this.labelX7.Text = "执法负责人：";
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(11, 74);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(103, 23);
            this.labelX8.TabIndex = 1;
            this.labelX8.Text = "备      注：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtXZQMC);
            this.groupBox2.Controls.Add(this.txtBJZFFZR);
            this.groupBox2.Controls.Add(this.txtSJZFFZR);
            this.groupBox2.Controls.Add(this.labelX1);
            this.groupBox2.Controls.Add(this.labelX3);
            this.groupBox2.Controls.Add(this.labelX4);
            this.groupBox2.Location = new System.Drawing.Point(9, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 107);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "执法网格信息";
            // 
            // txtXZQMC
            // 
            this.txtXZQMC.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtXZQMC.Border.Class = "TextBoxBorder";
            this.txtXZQMC.Location = new System.Drawing.Point(103, 20);
            this.txtXZQMC.Name = "txtXZQMC";
            this.txtXZQMC.ReadOnly = true;
            this.txtXZQMC.Size = new System.Drawing.Size(106, 21);
            this.txtXZQMC.TabIndex = 10;
            // 
            // txtBJZFFZR
            // 
            this.txtBJZFFZR.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtBJZFFZR.Border.Class = "TextBoxBorder";
            this.txtBJZFFZR.Location = new System.Drawing.Point(103, 74);
            this.txtBJZFFZR.Name = "txtBJZFFZR";
            this.txtBJZFFZR.ReadOnly = true;
            this.txtBJZFFZR.Size = new System.Drawing.Size(106, 21);
            this.txtBJZFFZR.TabIndex = 12;
            // 
            // txtSJZFFZR
            // 
            this.txtSJZFFZR.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtSJZFFZR.Border.Class = "TextBoxBorder";
            this.txtSJZFFZR.Location = new System.Drawing.Point(103, 47);
            this.txtSJZFFZR.Name = "txtSJZFFZR";
            this.txtSJZFFZR.ReadOnly = true;
            this.txtSJZFFZR.Size = new System.Drawing.Size(106, 21);
            this.txtSJZFFZR.TabIndex = 11;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(8, 22);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(108, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "行政区名称：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(8, 49);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(108, 23);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "上级执法负责人：";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(8, 76);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(108, 23);
            this.labelX4.TabIndex = 1;
            this.labelX4.Text = "本级执法负责人：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DropDownListSJZFDW);
            this.groupBox3.Controls.Add(this.txtSJZFRYBZ);
            this.groupBox3.Controls.Add(this.labelX2);
            this.groupBox3.Controls.Add(this.DropDownListSJZFRY);
            this.groupBox3.Controls.Add(this.labelX9);
            this.groupBox3.Controls.Add(this.labelX10);
            this.groupBox3.Location = new System.Drawing.Point(542, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(295, 107);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "上级执法人员";
            // 
            // DropDownListSJZFDW
            // 
            this.DropDownListSJZFDW.DisplayMember = "Text";
            this.DropDownListSJZFDW.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DropDownListSJZFDW.FormattingEnabled = true;
            this.DropDownListSJZFDW.ItemHeight = 15;
            this.DropDownListSJZFDW.Location = new System.Drawing.Point(85, 19);
            this.DropDownListSJZFDW.Name = "DropDownListSJZFDW";
            this.DropDownListSJZFDW.Size = new System.Drawing.Size(204, 21);
            this.DropDownListSJZFDW.TabIndex = 16;
            this.DropDownListSJZFDW.SelectedIndexChanged += new System.EventHandler(this.DropDownListSJZFDW_SelectedIndexChanged);
            // 
            // txtSJZFRYBZ
            // 
            // 
            // 
            // 
            this.txtSJZFRYBZ.Border.Class = "TextBoxBorder";
            this.txtSJZFRYBZ.Location = new System.Drawing.Point(85, 75);
            this.txtSJZFRYBZ.Name = "txtSJZFRYBZ";
            this.txtSJZFRYBZ.Size = new System.Drawing.Size(204, 21);
            this.txtSJZFRYBZ.TabIndex = 18;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 19);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(82, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "执法 单 位：";
            // 
            // DropDownListSJZFRY
            // 
            this.DropDownListSJZFRY.DisplayMember = "Text";
            this.DropDownListSJZFRY.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DropDownListSJZFRY.FormattingEnabled = true;
            this.DropDownListSJZFRY.ItemHeight = 15;
            this.DropDownListSJZFRY.Location = new System.Drawing.Point(85, 47);
            this.DropDownListSJZFRY.Name = "DropDownListSJZFRY";
            this.DropDownListSJZFRY.Size = new System.Drawing.Size(204, 21);
            this.DropDownListSJZFRY.TabIndex = 17;
            this.DropDownListSJZFRY.SelectedIndexChanged += new System.EventHandler(this.DropDownListSJZFRY_SelectedIndexChanged);
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(12, 49);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(82, 23);
            this.labelX9.TabIndex = 1;
            this.labelX9.Text = "执法负责人：";
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(12, 75);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(103, 23);
            this.labelX10.TabIndex = 1;
            this.labelX10.Text = "备      注：";
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.Size = new System.Drawing.Size(892, 440);
            this.dataGridViewX1.TabIndex = 6;
            this.dataGridViewX1.Click += new System.EventHandler(this.dataGridViewX1_Click);
            // 
            // frmWCGL_JG_WG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 611);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.panelEx_Button);
            this.Name = "frmWCGL_JG_WG";
            this.Text = "执法网格管理";
            this.Load += new System.EventHandler(this.frmWCGL_JG_WG_Load);
            this.SizeChanged += new System.EventHandler(this.frmWCGL_JG_WG_SizeChanged);
            this.panelEx_Button.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnRemove;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnExtend;
        public DevComponents.DotNetBar.PanelEx panelEx_Button;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBJZFRYBZ;
        private DevComponents.DotNetBar.Controls.ComboBoxEx DropDownListBJZFRY;
        private DevComponents.DotNetBar.Controls.ComboBoxEx DropDownListBJZFDW;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBJZFFZR;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSJZFFZR;
        private DevComponents.DotNetBar.Controls.TextBoxX txtXZQMC;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX5;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSJZFRYBZ;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx DropDownListSJZFRY;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.ComboBoxEx DropDownListSJZFDW;
        private DevComponents.DotNetBar.LabelX labelX10;
        public DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}