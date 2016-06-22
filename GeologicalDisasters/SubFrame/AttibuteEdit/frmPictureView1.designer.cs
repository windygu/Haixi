namespace JCZF.SubFrame
{
    partial class frmPictureView1
    {
        /// <summary>
        /// 必需的设计器变量。

        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。

        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。

        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPictureView));
            this.axAutoVueX1 = new AxAUTOVUEXLib.AxAutoVueX();
            this.btnPan = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnFullView = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axAutoVueX1)).BeginInit();
            this.SuspendLayout();
            // 
            // axAutoVueX1
            // 
            this.axAutoVueX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axAutoVueX1.Enabled = true;
            this.axAutoVueX1.Location = new System.Drawing.Point(0, 0);
            this.axAutoVueX1.Name = "axAutoVueX1";
            this.axAutoVueX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAutoVueX1.OcxState")));
            this.axAutoVueX1.Size = new System.Drawing.Size(440, 378);
            this.axAutoVueX1.TabIndex = 0;
            // 
            // btnPan
            // 
            this.btnPan.BackColor = System.Drawing.Color.Transparent;
            this.btnPan.Image = ((System.Drawing.Image)(resources.GetObject("btnPan.Image")));
            this.btnPan.Location = new System.Drawing.Point(80, 0);
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(26, 23);
            this.btnPan.TabIndex = 1;
            this.btnPan.UseVisualStyleBackColor = false;
            this.btnPan.Click += new System.EventHandler(this.btnPan_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.Location = new System.Drawing.Point(28, 0);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(26, 23);
            this.btnZoomOut.TabIndex = 1;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.Location = new System.Drawing.Point(2, 0);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(26, 23);
            this.btnZoomIn.TabIndex = 1;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnFullView
            // 
            this.btnFullView.BackColor = System.Drawing.Color.Transparent;
            this.btnFullView.Image = ((System.Drawing.Image)(resources.GetObject("btnFullView.Image")));
            this.btnFullView.Location = new System.Drawing.Point(54, 0);
            this.btnFullView.Name = "btnFullView";
            this.btnFullView.Size = new System.Drawing.Size(26, 23);
            this.btnFullView.TabIndex = 1;
            this.btnFullView.UseVisualStyleBackColor = false;
            this.btnFullView.Click += new System.EventHandler(this.btnFullView_Click);
            // 
            // frmPictureView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 378);
            this.Controls.Add(this.btnFullView);
            this.Controls.Add(this.btnPan);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.axAutoVueX1);
            this.Name = "frmPictureView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图片";
            this.Load += new System.EventHandler(this.frmPictureView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axAutoVueX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxAUTOVUEXLib.AxAutoVueX axAutoVueX1;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnPan;
        private System.Windows.Forms.Button btnFullView;

    }
}