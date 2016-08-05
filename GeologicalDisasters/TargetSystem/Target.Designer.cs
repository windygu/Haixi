namespace ComprehensiveEvaluation.TargetSystem
{
    partial class Target
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Target));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.target_list = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.target_list2 = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button2 = new System.Windows.Forms.Button();
            this.information = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.button2);
            this.splitContainerControl1.Panel2.Controls.Add(this.information);
            this.splitContainerControl1.Panel2.Controls.Add(this.button1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(362, 347);
            this.splitContainerControl1.SplitterPosition = 307;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.target_list);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.target_list2);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(362, 307);
            this.splitContainerControl2.SplitterPosition = 159;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // target_list
            // 
            this.target_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.target_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.target_list.Location = new System.Drawing.Point(0, 0);
            this.target_list.Name = "target_list";
            this.target_list.Size = new System.Drawing.Size(159, 307);
            this.target_list.TabIndex = 0;
            this.target_list.UseCompatibleStateImageBehavior = false;
            this.target_list.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.target_list_ItemChecked);
            this.target_list.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.target_list_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "评价指标名称";
            this.columnHeader1.Width = 159;
            // 
            // target_list2
            // 
            this.target_list2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.target_list2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.target_list2.Location = new System.Drawing.Point(0, 0);
            this.target_list2.Name = "target_list2";
            this.target_list2.Size = new System.Drawing.Size(198, 307);
            this.target_list2.TabIndex = 0;
            this.target_list2.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "评价因子";
            this.columnHeader2.Width = 265;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(275, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "下一步";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // information
            // 
            this.information.AutoSize = true;
            this.information.Location = new System.Drawing.Point(25, 10);
            this.information.Name = "information";
            this.information.Size = new System.Drawing.Size(113, 12);
            this.information.TabIndex = 1;
            this.information.Text = "请选择参与评价指标";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(182, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "上一步";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "指标.jpg");
            // 
            // Target
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 347);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "Target";
            this.Text = "指标管理";
            this.Load += new System.EventHandler(this.Target_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView target_list;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView target_list2;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label information;
        private System.Windows.Forms.Button button2;

    }
}