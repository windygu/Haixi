namespace JCZF.SubFrame
{
    partial class UserProperyShow
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
            this.m_PropertyDataGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.m_PropertyDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // m_PropertyDataGrid
            // 
            this.m_PropertyDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_PropertyDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_PropertyDataGrid.Location = new System.Drawing.Point(0, 0);
            this.m_PropertyDataGrid.Name = "m_PropertyDataGrid";
            this.m_PropertyDataGrid.RowTemplate.Height = 23;
            this.m_PropertyDataGrid.Size = new System.Drawing.Size(514, 298);
            this.m_PropertyDataGrid.TabIndex = 2;
            this.m_PropertyDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_PropertyDataGrid_CellClick);
            // 
            // UserProperyShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_PropertyDataGrid);
            this.Name = "UserProperyShow";
            this.Size = new System.Drawing.Size(514, 298);
            this.Load += new System.EventHandler(this.UserProperyShow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_PropertyDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView m_PropertyDataGrid;


    }
}
