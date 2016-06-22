namespace JCZF.SubFrame
{
    partial class frmHCQZBLR
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
            this.txtX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnDelet = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtXH = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colXH = new System.Windows.Forms.ColumnHeader();
            this.colX = new System.Windows.Forms.ColumnHeader();
            this.colY = new System.Windows.Forms.ColumnHeader();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(156, 22);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(91, 21);
            this.txtX.TabIndex = 0;
            this.txtX.Text = "0";
            this.txtX.Leave += new System.EventHandler(this.txtX_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "X：";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(304, 22);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(91, 21);
            this.txtY.TabIndex = 0;
            this.txtY.Text = "0";
            this.txtY.Leave += new System.EventHandler(this.txtY_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(258, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y：";
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(373, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(54, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(80, 20);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(54, 23);
            this.btnModify.TabIndex = 2;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelet
            // 
            this.btnDelet.Location = new System.Drawing.Point(365, 33);
            this.btnDelet.Name = "btnDelet";
            this.btnDelet.Size = new System.Drawing.Size(54, 23);
            this.btnDelet.TabIndex = 2;
            this.btnDelet.Text = "删除";
            this.btnDelet.UseVisualStyleBackColor = true;
            this.btnDelet.Visible = false;
            this.btnDelet.Click += new System.EventHandler(this.btnDelet_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(11, 20);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(54, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "文件";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "序号：";
            // 
            // txtXH
            // 
            this.txtXH.Enabled = false;
            this.txtXH.Location = new System.Drawing.Point(53, 21);
            this.txtXH.Name = "txtXH";
            this.txtXH.Size = new System.Drawing.Size(41, 21);
            this.txtXH.TabIndex = 0;
            this.txtXH.Text = "1";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(235, 20);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(63, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "图形导入";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Location = new System.Drawing.Point(1, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 302);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "坐标列表";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colXH,
            this.colX,
            this.colY});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(8, 14);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(400, 282);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // colXH
            // 
            this.colXH.Text = "序号";
            this.colXH.Width = 62;
            // 
            // colX
            // 
            this.colX.Text = "X坐标";
            this.colX.Width = 159;
            // 
            // colY
            // 
            this.colY.Text = "Y坐标";
            this.colY.Width = 158;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtX);
            this.groupBox2.Controls.Add(this.txtXH);
            this.groupBox2.Controls.Add(this.txtY);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(1, 307);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(419, 56);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.btnImport);
            this.groupBox3.Controls.Add(this.btnAdd);
            this.groupBox3.Controls.Add(this.btnModify);
            this.groupBox3.Controls.Add(this.btnGenerate);
            this.groupBox3.Controls.Add(this.btnDelet);
            this.groupBox3.Location = new System.Drawing.Point(1, 367);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(419, 56);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(156, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "图形预览";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(313, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "导出坐标文件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmHCQZBLR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 430);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmHCQZBLR";
            this.Text = "坐标录入";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHCQZBLR_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnDelet;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtXH;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colXH;
        private System.Windows.Forms.ColumnHeader colX;
        private System.Windows.Forms.ColumnHeader colY;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}