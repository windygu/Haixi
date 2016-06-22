namespace JCZF.SubFrame.AttibuteEdit
{
    partial class FormMenuButton
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
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnVideos = new System.Windows.Forms.Button();
            this.btnPhotos = new System.Windows.Forms.Button();
            this.btnHC = new System.Windows.Forms.Button();
            this.btnZbdc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnProperties
            // 
            this.btnProperties.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnProperties.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProperties.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProperties.Location = new System.Drawing.Point(0, 0);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(125, 41);
            this.btnProperties.TabIndex = 0;
            this.btnProperties.Text = "巡查报告";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnVideos
            // 
            this.btnVideos.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnVideos.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnVideos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVideos.Location = new System.Drawing.Point(0, 83);
            this.btnVideos.Name = "btnVideos";
            this.btnVideos.Size = new System.Drawing.Size(125, 41);
            this.btnVideos.TabIndex = 3;
            this.btnVideos.Text = "视  频";
            this.btnVideos.UseVisualStyleBackColor = true;
            this.btnVideos.Click += new System.EventHandler(this.btnVideos_Click);
            // 
            // btnPhotos
            // 
            this.btnPhotos.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPhotos.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPhotos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPhotos.Location = new System.Drawing.Point(0, 41);
            this.btnPhotos.Name = "btnPhotos";
            this.btnPhotos.Size = new System.Drawing.Size(125, 42);
            this.btnPhotos.TabIndex = 4;
            this.btnPhotos.Text = "照  片";
            this.btnPhotos.UseVisualStyleBackColor = true;
            this.btnPhotos.Click += new System.EventHandler(this.btnPhotos_Click);
            // 
            // btnHC
            // 
            this.btnHC.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHC.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnHC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHC.Location = new System.Drawing.Point(0, 124);
            this.btnHC.Name = "btnHC";
            this.btnHC.Size = new System.Drawing.Size(125, 41);
            this.btnHC.TabIndex = 5;
            this.btnHC.Text = "分 析";
            this.btnHC.UseVisualStyleBackColor = true;
            this.btnHC.Click += new System.EventHandler(this.btnHC_Click);
            // 
            // btnZbdc
            // 
            this.btnZbdc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnZbdc.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnZbdc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZbdc.Location = new System.Drawing.Point(0, 165);
            this.btnZbdc.Name = "btnZbdc";
            this.btnZbdc.Size = new System.Drawing.Size(125, 41);
            this.btnZbdc.TabIndex = 6;
            this.btnZbdc.Text = "坐标导出";
            this.btnZbdc.UseVisualStyleBackColor = true;
            this.btnZbdc.Click += new System.EventHandler(this.btnZbdc_Click);
            // 
            // FormMenuButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(125, 206);
            this.Controls.Add(this.btnZbdc);
            this.Controls.Add(this.btnHC);
            this.Controls.Add(this.btnPhotos);
            this.Controls.Add(this.btnVideos);
            this.Controls.Add(this.btnProperties);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMenuButton";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "巡查地块";
            this.Load += new System.EventHandler(this.FormMenuButton_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnVideos;
        private System.Windows.Forms.Button btnPhotos;
        private System.Windows.Forms.Button btnHC;
        private System.Windows.Forms.Button btnZbdc;
    }
}