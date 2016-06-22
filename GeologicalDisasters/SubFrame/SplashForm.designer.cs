namespace JCZF
{
    partial class SplashForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.lStatusInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarX1
            // 
            this.progressBarX1.BackColor = System.Drawing.Color.Transparent;
            this.progressBarX1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.progressBarX1.Location = new System.Drawing.Point(0, 320);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(593, 29);
            this.progressBarX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.progressBarX1.TabIndex = 1;
            this.progressBarX1.Text = "progressBarX1";
            // 
            // lStatusInfo
            // 
            this.lStatusInfo.AutoSize = true;
            this.lStatusInfo.BackColor = System.Drawing.Color.Transparent;
            this.lStatusInfo.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lStatusInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lStatusInfo.Location = new System.Drawing.Point(26, 302);
            this.lStatusInfo.Name = "lStatusInfo";
            this.lStatusInfo.Size = new System.Drawing.Size(161, 14);
            this.lStatusInfo.TabIndex = 2;
            this.lStatusInfo.Text = "正在初始化，请稍候……";
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(591, 348);
            this.Controls.Add(this.lStatusInfo);
            this.Controls.Add(this.progressBarX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "启动窗体";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private System.Windows.Forms.Label lStatusInfo;

    }
}