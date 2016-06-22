namespace JCZF.SubFrame
{
    partial class uctTJFX_1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxDWRY = new System.Windows.Forms.GroupBox();
            this.uctXZQTree = new XZQTree.uctXZQTree_Dev();
            this.cmbDW = new System.Windows.Forms.ComboBox();
            this.cmbRY = new System.Windows.Forms.ComboBox();
            this.chkRY = new System.Windows.Forms.CheckBox();
            this.chkXZQ = new System.Windows.Forms.CheckBox();
            this.chkDW = new System.Windows.Forms.CheckBox();
            this.groupBoxSJ = new System.Windows.Forms.GroupBox();
            this.dateTimePicker_End = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.dateTimePicker_Start = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.superTooltip1 = new DevComponents.DotNetBar.SuperTooltip();
            this.groupBoxDWRY.SuspendLayout();
            this.groupBoxSJ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimePicker_End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimePicker_Start)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxDWRY
            // 
            this.groupBoxDWRY.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxDWRY.Controls.Add(this.uctXZQTree);
            this.groupBoxDWRY.Controls.Add(this.cmbDW);
            this.groupBoxDWRY.Controls.Add(this.cmbRY);
            this.groupBoxDWRY.Controls.Add(this.chkRY);
            this.groupBoxDWRY.Controls.Add(this.chkXZQ);
            this.groupBoxDWRY.Controls.Add(this.chkDW);
            this.groupBoxDWRY.Location = new System.Drawing.Point(0, 108);
            this.groupBoxDWRY.Name = "groupBoxDWRY";
            this.groupBoxDWRY.Size = new System.Drawing.Size(235, 117);
            this.groupBoxDWRY.TabIndex = 8;
            this.groupBoxDWRY.TabStop = false;
            this.groupBoxDWRY.Text = "行政区、单位、人员";
            // 
            // uctXZQTree
            // 
            this.uctXZQTree.Location = new System.Drawing.Point(66, 23);
            this.uctXZQTree.Name = "uctXZQTree";
            this.uctXZQTree.Size = new System.Drawing.Size(163, 21);
            this.uctXZQTree.TabIndex = 2;
            this.uctXZQTree.uctXZQTree_DevSelectEvent += new XZQTree.uctXZQTree_Dev.uctXZQTree_DevSelectEventHandler(this.uctXZQTree_uctXZQTree_DevSelectEvent);
            // 
            // cmbDW
            // 
            this.cmbDW.Enabled = false;
            this.cmbDW.FormattingEnabled = true;
            this.cmbDW.Location = new System.Drawing.Point(67, 56);
            this.cmbDW.Name = "cmbDW";
            this.cmbDW.Size = new System.Drawing.Size(163, 20);
            this.cmbDW.TabIndex = 1;
            this.cmbDW.SelectedIndexChanged += new System.EventHandler(this.cmbDW_SelectedIndexChanged);
            // 
            // cmbRY
            // 
            this.cmbRY.Enabled = false;
            this.cmbRY.FormattingEnabled = true;
            this.cmbRY.Location = new System.Drawing.Point(66, 89);
            this.cmbRY.Name = "cmbRY";
            this.cmbRY.Size = new System.Drawing.Size(163, 20);
            this.cmbRY.TabIndex = 1;
            // 
            // chkRY
            // 
            this.chkRY.AutoSize = true;
            this.chkRY.ForeColor = System.Drawing.Color.Black;
            this.chkRY.Location = new System.Drawing.Point(4, 93);
            this.chkRY.Name = "chkRY";
            this.chkRY.Size = new System.Drawing.Size(72, 16);
            this.chkRY.TabIndex = 0;
            this.chkRY.Text = "人  员：";
            this.chkRY.UseVisualStyleBackColor = true;
            this.chkRY.CheckedChanged += new System.EventHandler(this.chkRY_CheckedChanged);
            // 
            // chkXZQ
            // 
            this.chkXZQ.AutoSize = true;
            this.chkXZQ.ForeColor = System.Drawing.Color.Black;
            this.chkXZQ.Location = new System.Drawing.Point(4, 28);
            this.chkXZQ.Name = "chkXZQ";
            this.chkXZQ.Size = new System.Drawing.Size(72, 16);
            this.chkXZQ.TabIndex = 0;
            this.chkXZQ.Text = "行政区：";
            this.chkXZQ.UseVisualStyleBackColor = true;
            this.chkXZQ.CheckedChanged += new System.EventHandler(this.chkDW_CheckedChanged);
            // 
            // chkDW
            // 
            this.chkDW.AutoSize = true;
            this.chkDW.ForeColor = System.Drawing.Color.Black;
            this.chkDW.Location = new System.Drawing.Point(4, 60);
            this.chkDW.Name = "chkDW";
            this.chkDW.Size = new System.Drawing.Size(72, 16);
            this.chkDW.TabIndex = 0;
            this.chkDW.Text = "单  位：";
            this.chkDW.UseVisualStyleBackColor = true;
            this.chkDW.CheckedChanged += new System.EventHandler(this.chkDW_CheckedChanged);
            // 
            // groupBoxSJ
            // 
            this.groupBoxSJ.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxSJ.Controls.Add(this.dateTimePicker_End);
            this.groupBoxSJ.Controls.Add(this.dateTimePicker_Start);
            this.groupBoxSJ.Controls.Add(this.labelX18);
            this.groupBoxSJ.Controls.Add(this.labelX19);
            this.groupBoxSJ.Location = new System.Drawing.Point(0, 6);
            this.groupBoxSJ.Name = "groupBoxSJ";
            this.groupBoxSJ.Size = new System.Drawing.Size(235, 88);
            this.groupBoxSJ.TabIndex = 7;
            this.groupBoxSJ.TabStop = false;
            this.groupBoxSJ.Text = "统计时间范围";
            // 
            // dateTimePicker_End
            // 
            // 
            // 
            // 
            this.dateTimePicker_End.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimePicker_End.ButtonDropDown.Visible = true;
            this.dateTimePicker_End.Location = new System.Drawing.Point(63, 53);
            // 
            // 
            // 
            this.dateTimePicker_End.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            this.dateTimePicker_End.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimePicker_End.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimePicker_End.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimePicker_End.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimePicker_End.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimePicker_End.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimePicker_End.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimePicker_End.MonthCalendar.DisplayMonth = new System.DateTime(2011, 5, 1, 0, 0, 0, 0);
            this.dateTimePicker_End.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTimePicker_End.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimePicker_End.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimePicker_End.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimePicker_End.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimePicker_End.MonthCalendar.TodayButtonVisible = true;
            this.dateTimePicker_End.Name = "dateTimePicker_End";
            this.dateTimePicker_End.Size = new System.Drawing.Size(166, 21);
            this.dateTimePicker_End.TabIndex = 3;
            // 
            // dateTimePicker_Start
            // 
            // 
            // 
            // 
            this.dateTimePicker_Start.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimePicker_Start.ButtonDropDown.Visible = true;
            this.dateTimePicker_Start.Location = new System.Drawing.Point(64, 24);
            // 
            // 
            // 
            this.dateTimePicker_Start.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            this.dateTimePicker_Start.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimePicker_Start.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimePicker_Start.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimePicker_Start.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimePicker_Start.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimePicker_Start.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimePicker_Start.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimePicker_Start.MonthCalendar.DisplayMonth = new System.DateTime(2011, 5, 1, 0, 0, 0, 0);
            this.dateTimePicker_Start.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTimePicker_Start.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimePicker_Start.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimePicker_Start.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimePicker_Start.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimePicker_Start.MonthCalendar.TodayButtonVisible = true;
            this.dateTimePicker_Start.Name = "dateTimePicker_Start";
            this.dateTimePicker_Start.Size = new System.Drawing.Size(166, 21);
            this.dateTimePicker_Start.TabIndex = 3;
            // 
            // labelX18
            // 
            this.labelX18.ForeColor = System.Drawing.Color.Black;
            this.labelX18.Location = new System.Drawing.Point(32, 56);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(30, 23);
            this.labelX18.TabIndex = 2;
            this.labelX18.Text = "到：";
            // 
            // labelX19
            // 
            this.labelX19.ForeColor = System.Drawing.Color.Black;
            this.labelX19.Location = new System.Drawing.Point(32, 24);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(30, 23);
            this.labelX19.TabIndex = 2;
            this.labelX19.Text = "从：";
            // 
            // superTooltip1
            // 
            //this.superTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            // 
            // uctTJFX_1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBoxDWRY);
            this.Controls.Add(this.groupBoxSJ);
            this.Name = "uctTJFX_1";
            this.Size = new System.Drawing.Size(250, 255);
            this.Load += new System.EventHandler(this.uctTJFX_1_Load);
            this.groupBoxDWRY.ResumeLayout(false);
            this.groupBoxDWRY.PerformLayout();
            this.groupBoxSJ.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateTimePicker_End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimePicker_Start)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDWRY;
        private System.Windows.Forms.ComboBox cmbDW;
        private System.Windows.Forms.ComboBox cmbRY;
        private System.Windows.Forms.GroupBox groupBoxSJ;
        private DevComponents.DotNetBar.LabelX labelX18;
        private DevComponents.DotNetBar.LabelX labelX19;
        public System.Windows.Forms.CheckBox chkXZQ;
        public System.Windows.Forms.CheckBox chkRY;
        public System.Windows.Forms.CheckBox chkDW;
        private XZQTree.uctXZQTree_Dev uctXZQTree;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimePicker_End;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimePicker_Start;
        private DevComponents.DotNetBar.SuperTooltip superTooltip1;
    }
}
