namespace JCZF.SubFrame
{
    partial class uctWGCX_JG
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_DataGridView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            ((System.ComponentModel.ISupportInitialize)(this.m_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // m_DataGridView
            // 
            this.m_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_DataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.m_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_DataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.m_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.m_DataGridView.MultiSelect = false;
            this.m_DataGridView.Name = "m_DataGridView";
            this.m_DataGridView.ReadOnly = true;
            this.m_DataGridView.RowTemplate.Height = 23;
            this.m_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_DataGridView.Size = new System.Drawing.Size(501, 354);
            this.m_DataGridView.TabIndex = 57;
            this.m_DataGridView.Click += new System.EventHandler(this.dataGridViewX2_Click);
            // 
            // uctWGCX_JG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_DataGridView);
            this.Name = "uctWGCX_JG";
            this.Size = new System.Drawing.Size(501, 354);
            ((System.ComponentModel.ISupportInitialize)(this.m_DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevComponents.DotNetBar.Controls.DataGridViewX m_DataGridView;

    }
}
