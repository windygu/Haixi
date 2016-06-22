namespace JCZF.SubFrame
{
    partial class uctTJFX_XCCQ_JG
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_DataGridView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.btnSendMessage = new DevComponents.DotNetBar.ButtonX();
            this.btnExportData = new DevComponents.DotNetBar.ButtonX();
            this.panel_SendMessage = new DevComponents.DotNetBar.PanelEx();
            this.txtMessage = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSendMessage1 = new DevComponents.DotNetBar.ButtonX();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer_ExportData = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_DataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel_SendMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_DataGridView
            // 
            this.m_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_DataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_DataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.m_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.m_DataGridView.Name = "m_DataGridView";
            this.m_DataGridView.ReadOnly = true;
            this.m_DataGridView.RowTemplate.Height = 23;
            this.m_DataGridView.Size = new System.Drawing.Size(519, 328);
            this.m_DataGridView.TabIndex = 2;
            this.m_DataGridView.Click += new System.EventHandler(this.m_DataGridView_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSendMessage);
            this.panel1.Controls.Add(this.btnExportData);
            this.panel1.Location = new System.Drawing.Point(60, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(129, 96);
            this.panel1.TabIndex = 9;
            this.panel1.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClose.Location = new System.Drawing.Point(0, 63);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(128, 32);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关  闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSendMessage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSendMessage.Location = new System.Drawing.Point(0, 32);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(128, 32);
            this.btnSendMessage.TabIndex = 1;
            this.btnSendMessage.Text = "发送短信";
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // btnExportData
            // 
            this.btnExportData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportData.Location = new System.Drawing.Point(0, 1);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(128, 32);
            this.btnExportData.TabIndex = 0;
            this.btnExportData.Text = "导出数据";
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // panel_SendMessage
            // 
            this.panel_SendMessage.CanvasColor = System.Drawing.SystemColors.Control;
            this.panel_SendMessage.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panel_SendMessage.Controls.Add(this.txtMessage);
            this.panel_SendMessage.Controls.Add(this.btnSendMessage1);
            this.panel_SendMessage.Controls.Add(this.txtPhoneNumber);
            this.panel_SendMessage.Controls.Add(this.txtName);
            this.panel_SendMessage.Controls.Add(this.label2);
            this.panel_SendMessage.Controls.Add(this.label1);
            this.panel_SendMessage.Location = new System.Drawing.Point(188, 116);
            this.panel_SendMessage.Name = "panel_SendMessage";
            this.panel_SendMessage.Size = new System.Drawing.Size(270, 96);
            this.panel_SendMessage.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panel_SendMessage.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panel_SendMessage.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panel_SendMessage.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panel_SendMessage.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panel_SendMessage.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panel_SendMessage.Style.GradientAngle = 90;
            this.panel_SendMessage.TabIndex = 8;
            this.panel_SendMessage.Visible = false;
            // 
            // txtMessage
            // 
            // 
            // 
            // 
            this.txtMessage.Border.Class = "TextBoxBorder";
            this.txtMessage.Location = new System.Drawing.Point(6, 32);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(211, 60);
            this.txtMessage.TabIndex = 3;
            this.txtMessage.WatermarkText = "短信内容";
            // 
            // btnSendMessage1
            // 
            this.btnSendMessage1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSendMessage1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSendMessage1.Location = new System.Drawing.Point(223, 32);
            this.btnSendMessage1.Name = "btnSendMessage1";
            this.btnSendMessage1.Size = new System.Drawing.Size(44, 60);
            this.btnSendMessage1.TabIndex = 4;
            this.btnSendMessage1.Text = "发送";
            this.btnSendMessage1.Click += new System.EventHandler(this.btnSendMessage1_Click);
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Location = new System.Drawing.Point(172, 6);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(95, 21);
            this.txtPhoneNumber.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(42, 6);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(77, 21);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "手机号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "姓名：";
            // 
            // timer_ExportData
            // 
            this.timer_ExportData.Tick += new System.EventHandler(this.timer_ExportData_Tick);
            // 
            // uctTJFX_XCCQ_JG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_SendMessage);
            this.Controls.Add(this.m_DataGridView);
            this.Name = "uctTJFX_XCCQ_JG";
            this.Size = new System.Drawing.Size(519, 328);
            ((System.ComponentModel.ISupportInitialize)(this.m_DataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel_SendMessage.ResumeLayout(false);
            this.panel_SendMessage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public DevComponents.DotNetBar.Controls.DataGridViewX m_DataGridView;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.ButtonX btnClose;
        private DevComponents.DotNetBar.ButtonX btnSendMessage;
        private DevComponents.DotNetBar.ButtonX btnExportData;
        private DevComponents.DotNetBar.PanelEx panel_SendMessage;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMessage;
        private DevComponents.DotNetBar.ButtonX btnSendMessage1;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer_ExportData;
    }
}
