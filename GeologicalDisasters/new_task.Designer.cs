namespace ComprehensiveEvaluation
{
    partial class new_task
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.e_type = new DevComponents.DotNetBar.LabelX();
            this.land_choose = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.risk_choose = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.ecology_choose = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.panelEx1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Controls.Add(this.groupBox1);
            this.panelEx1.Controls.Add(this.e_type);
            this.panelEx1.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(298, 343);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "评价类型：";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(148, 308);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 2;
            this.buttonX1.Text = "下一步";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ecology_choose);
            this.groupBox1.Controls.Add(this.risk_choose);
            this.groupBox1.Controls.Add(this.land_choose);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(25, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 219);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择基础数据库";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "土地生态服务评价基础数据库：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "土地灾害风险评价基础数据库：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "土地适宜性评价基础数据库：";
            // 
            // e_type
            // 
            // 
            // 
            // 
            this.e_type.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.e_type.Location = new System.Drawing.Point(94, 32);
            this.e_type.Name = "e_type";
            this.e_type.Size = new System.Drawing.Size(182, 23);
            this.e_type.TabIndex = 0;
            // 
            // land_choose
            // 
            this.land_choose.DisplayMember = "Text";
            this.land_choose.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.land_choose.FormattingEnabled = true;
            this.land_choose.ItemHeight = 15;
            this.land_choose.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3});
            this.land_choose.Location = new System.Drawing.Point(8, 52);
            this.land_choose.Name = "land_choose";
            this.land_choose.Size = new System.Drawing.Size(220, 21);
            this.land_choose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.land_choose.TabIndex = 4;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "土地适宜性评价基础数据库";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "土地灾害风险评价基础数据库";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "土地生态服务评价基础数据库";
            // 
            // risk_choose
            // 
            this.risk_choose.DisplayMember = "Text";
            this.risk_choose.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.risk_choose.FormattingEnabled = true;
            this.risk_choose.ItemHeight = 15;
            this.risk_choose.Items.AddRange(new object[] {
            this.comboItem4,
            this.comboItem5,
            this.comboItem6});
            this.risk_choose.Location = new System.Drawing.Point(6, 112);
            this.risk_choose.Name = "risk_choose";
            this.risk_choose.Size = new System.Drawing.Size(220, 21);
            this.risk_choose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.risk_choose.TabIndex = 4;
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "土地适宜性评价基础数据库";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "土地灾害风险评价基础数据库";
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "土地生态服务评价基础数据库";
            // 
            // ecology_choose
            // 
            this.ecology_choose.DisplayMember = "Text";
            this.ecology_choose.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ecology_choose.FormattingEnabled = true;
            this.ecology_choose.ItemHeight = 15;
            this.ecology_choose.Items.AddRange(new object[] {
            this.comboItem7,
            this.comboItem8,
            this.comboItem9});
            this.ecology_choose.Location = new System.Drawing.Point(5, 181);
            this.ecology_choose.Name = "ecology_choose";
            this.ecology_choose.Size = new System.Drawing.Size(220, 21);
            this.ecology_choose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ecology_choose.TabIndex = 4;
            // 
            // comboItem7
            // 
            this.comboItem7.Text = "土地适宜性评价基础数据库";
            // 
            // comboItem8
            // 
            this.comboItem8.Text = "土地灾害风险评价基础数据库";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "土地生态服务评价基础数据库";
            // 
            // new_task
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 343);
            this.Controls.Add(this.panelEx1);
            this.Name = "new_task";
            this.Text = "新建评价任务";
            this.Load += new System.EventHandler(this.new_task_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX e_type;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx land_choose;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx ecology_choose;
        private DevComponents.Editors.ComboItem comboItem7;
        private DevComponents.Editors.ComboItem comboItem8;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.DotNetBar.Controls.ComboBoxEx risk_choose;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.Editors.ComboItem comboItem6;
    }
}