namespace JCZF.SubFrame.AttibuteEdit
{
    partial class uctPicture
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.listViewEx1 = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(122, 454);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(54, 26);
            this.btnAdd.TabIndex = 31;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.Add_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF,JPEG)|*.BMP;*.JPG;*.GIF;JPEG|Video Files(*.AVI,*.MP" +
                "3,*.MP4,*.3GP,*.RMVB)|*.AVI;*.MP3;*.MP4;*.3GP;*.RMVB|All files (*.*)|*.*";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.pictureBox2);
            this.groupBox7.Controls.Add(this.listViewEx1);
            this.groupBox7.Location = new System.Drawing.Point(3, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(386, 445);
            this.groupBox7.TabIndex = 40;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "照片及视频";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(5, 20);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(374, 257);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 41;
            this.pictureBox2.TabStop = false;
            // 
            // listViewEx1
            // 
            // 
            // 
            // 
            this.listViewEx1.Border.Class = "ListViewBorder";
            this.listViewEx1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewEx1.FullRowSelect = true;
            this.listViewEx1.GridLines = true;
            this.listViewEx1.Location = new System.Drawing.Point(6, 283);
            this.listViewEx1.MultiSelect = false;
            this.listViewEx1.Name = "listViewEx1";
            this.listViewEx1.Size = new System.Drawing.Size(374, 156);
            this.listViewEx1.TabIndex = 40;
            this.listViewEx1.UseCompatibleStateImageBehavior = false;
            this.listViewEx1.View = System.Windows.Forms.View.Details;
            this.listViewEx1.SelectedIndexChanged += new System.EventHandler(this.listViewEx1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 45;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "文件名称";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 293;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "标识";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(212, 454);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(54, 26);
            this.btnDel.TabIndex = 31;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(320, 454);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(54, 26);
            this.btnReturn.TabIndex = 31;
            this.btnReturn.Text = "返回";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // uctPicture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnAdd);
            this.Name = "uctPicture";
            this.Size = new System.Drawing.Size(392, 500);
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox7;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewEx1;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}