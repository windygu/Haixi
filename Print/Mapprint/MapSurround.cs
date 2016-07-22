using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.DisplayUI;
//using ESRI.ArcGIS.Utility.COMSupport;
using stdole;
using ESRI.ArcGIS.Geometry;
using Point = ESRI.ArcGIS.Geometry.Point;
using ESRI.ArcGIS.Controls;

namespace Print.Mapprint
{
	/// <summary>
	/// MapSurround 的摘要说明。
	/// </summary>
	public class MapSurround : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox9;
		private System.Windows.Forms.CheckBox checkBox10;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.TextBox txtFontSize;
		private System.Windows.Forms.Button txtColor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		//新定义
		private AxPageLayoutControl axPageLayoutControl;
		private IActiveView m_ActiveView;
		private IGraphicsContainer m_GraphicsContainer;
		private IMapFrame m_MapFrame;
		private IMap m_Map;
		private IMapGrids m_MapGrids;
		private IMapGrid m_MapGrid;
		private IMeasuredGrid  m_MeasuredGrid;
		private ISimpleTextSymbol m_SimpleTextSymbol;
		private ITextElement m_TextElement;
		stdole.IFontDisp m_Font;
		private IElement m_Element;
		private IScaleBar m_ScaleBar;
		private IMarkerNorthArrow m_MarkerNorthArrow;
		
		private ESRI.ArcGIS.esriSystem.IArray[] m_StylesArray = new ESRI.ArcGIS.esriSystem.ArrayClass[1];
		private IStyleGallery m_StyleGallery;
		private IPoint m_Point;
		
        //设置字体
		private bool isBold = false;
		private bool isItalic = false;
		private bool isUnderline = false;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button btnCancel;
        private Label label5;
		
		
		private System.ComponentModel.IContainer components;

		public MapSurround(AxPageLayoutControl axPageLayoutControl)
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
			//初始化
			//获得pagelayout基本对象
			this.axPageLayoutControl = axPageLayoutControl;
			m_ActiveView =(IActiveView)this.axPageLayoutControl.PageLayout;
			m_Map = m_ActiveView.FocusMap;
			//MessageBox.Show(m_Map.Name);

			//获得版式视图
			m_GraphicsContainer = (IGraphicsContainer)this.axPageLayoutControl.PageLayout;
			m_MapFrame = (IMapFrame)m_GraphicsContainer.FindFrame(m_Map);		
			m_MapGrids = (IMapGrids)m_MapFrame;

			//m_MapGrids.AddMapGrid(m_MapGrid);
			//m_MapGrid = m_MapGrids.get_MapGrid(0);

			m_MeasuredGrid = new MeasuredGridClass();
			m_MapGrid = m_MeasuredGrid as IMapGrid;
			//m_MeasuredGrid = new GraticuleClass();
			//m_MapGrid = m_MeasuredGrid as IMapGrid;
			//关于文字设置

			this.m_Point = m_Point;
			this.m_SimpleTextSymbol =  new TextSymbol() as ISimpleTextSymbol;
			this.m_Font = new StdFont() as IFontDisp;
			m_TextElement = new TextElement() as ITextElement;

            //比例尺样式
			this.comboBox2.Items.Add("单线交互式比例尺");
			this.comboBox2.Items.Add("双线交互式比例尺");
			this.comboBox2.Items.Add("中空式比例尺");
			this.comboBox2.Items.Add("线式比例尺");
			this.comboBox2.Items.Add("分割式比例尺");
			this.comboBox2.Items.Add("阶梯式比例尺");
			this.comboBox2.SelectedIndex = 0;

			//字体类型
			this.comboBox3.Items.Add("Arial");
			this.comboBox3.Items.Add("宋体");
			this.comboBox3.Items.Add("隶书");
			this.comboBox3.Items.Add("黑体");
			this.comboBox3.Items.Add("幼圆");
			this.comboBox3.Items.Add("楷体_GB2312");
			this.comboBox3.Items.Add("方正姚体");
			this.comboBox3.Items.Add("华文新魏");
			this.comboBox3.SelectedIndex = 0;

			this.m_MarkerNorthArrow = new ESRI.ArcGIS.Carto.MarkerNorthArrowClass();


		}

		/// <summary>  
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSurround));
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            ""}, 0, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("宋体", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))));
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem("", 1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem("", 2);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem("", 3);
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem("", 4);
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem("", 5);
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem("", 6);
            System.Windows.Forms.ListViewItem listViewItem23 = new System.Windows.Forms.ListViewItem("", 7);
            System.Windows.Forms.ListViewItem listViewItem24 = new System.Windows.Forms.ListViewItem("", 8);
            System.Windows.Forms.ListViewItem listViewItem25 = new System.Windows.Forms.ListViewItem("", 9);
            System.Windows.Forms.ListViewItem listViewItem26 = new System.Windows.Forms.ListViewItem("", 10);
            System.Windows.Forms.ListViewItem listViewItem27 = new System.Windows.Forms.ListViewItem("", 11);
            System.Windows.Forms.ListViewItem listViewItem28 = new System.Windows.Forms.ListViewItem("", 12);
            System.Windows.Forms.ListViewItem listViewItem29 = new System.Windows.Forms.ListViewItem("", 13);
            System.Windows.Forms.ListViewItem listViewItem30 = new System.Windows.Forms.ListViewItem("", 14);
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFontSize = new System.Windows.Forms.TextBox();
            this.txtColor = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择需要添加的要素";
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(40, 32);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(104, 24);
            this.checkBox3.TabIndex = 3;
            this.checkBox3.Text = "背景";
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(56, 442);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 32);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(216, 442);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox6
            // 
            this.checkBox6.Location = new System.Drawing.Point(40, 56);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(104, 24);
            this.checkBox6.TabIndex = 8;
            this.checkBox6.Text = "辅助线";
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // checkBox7
            // 
            this.checkBox7.Location = new System.Drawing.Point(152, 32);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(104, 24);
            this.checkBox7.TabIndex = 9;
            this.checkBox7.Text = "网点";
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // checkBox9
            // 
            this.checkBox9.Location = new System.Drawing.Point(40, 88);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(112, 24);
            this.checkBox9.TabIndex = 13;
            this.checkBox9.Text = "边框      线型";
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // checkBox10
            // 
            this.checkBox10.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.checkBox10.Location = new System.Drawing.Point(8, -4);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(80, 24);
            this.checkBox10.TabIndex = 14;
            this.checkBox10.Text = "文字标注";
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(152, 88);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(168, 20);
            this.comboBox1.TabIndex = 15;
            this.comboBox1.Text = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "字号";
            // 
            // txtFontSize
            // 
            this.txtFontSize.Location = new System.Drawing.Point(48, 113);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.Size = new System.Drawing.Size(48, 21);
            this.txtFontSize.TabIndex = 18;
            this.txtFontSize.Text = "72";
            this.txtFontSize.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // txtColor
            // 
            this.txtColor.Location = new System.Drawing.Point(16, 151);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(96, 23);
            this.txtColor.TabIndex = 19;
            this.txtColor.Text = "选择字体颜色";
            this.txtColor.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFontSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Controls.Add(this.comboBox3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtColor);
            this.groupBox1.Controls.Add(this.checkBox10);
            this.groupBox1.Location = new System.Drawing.Point(32, 248);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 188);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(208, 151);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(56, 23);
            this.button6.TabIndex = 27;
            this.button6.Text = "更多……";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.Location = new System.Drawing.Point(176, 151);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(24, 23);
            this.button5.TabIndex = 26;
            this.button5.Text = "U";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.Location = new System.Drawing.Point(152, 151);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 23);
            this.button4.TabIndex = 25;
            this.button4.Text = "I";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(128, 151);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 23);
            this.button3.TabIndex = 24;
            this.button3.Text = "B";
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(48, 24);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(232, 83);
            this.richTextBox1.TabIndex = 23;
            this.richTextBox1.Text = "";
            // 
            // comboBox3
            // 
            this.comboBox3.Location = new System.Drawing.Point(152, 113);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(128, 20);
            this.comboBox3.TabIndex = 22;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(119, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 21;
            this.label4.Text = "字体";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "文";
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(40, 120);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(136, 24);
            this.checkBox2.TabIndex = 22;
            this.checkBox2.Text = "比例尺    样式";
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged_1);
            // 
            // comboBox2
            // 
            this.comboBox2.Location = new System.Drawing.Point(152, 120);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(168, 20);
            this.comboBox2.TabIndex = 23;
            this.comboBox2.Text = "comboBox2";
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new System.Drawing.Point(152, 56);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(104, 24);
            this.checkBox4.TabIndex = 24;
            this.checkBox4.Text = "图例";
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged_1);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            // 
            // listView1
            // 
            this.listView1.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.FullRowSelect = true;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19,
            listViewItem20,
            listViewItem21,
            listViewItem22,
            listViewItem23,
            listViewItem24,
            listViewItem25,
            listViewItem26,
            listViewItem27,
            listViewItem28,
            listViewItem29,
            listViewItem30});
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(104, 160);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(216, 72);
            this.listView1.TabIndex = 25;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.checkBox1.Location = new System.Drawing.Point(40, 152);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 24);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "指北针";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // checkBox5
            // 
            this.checkBox5.Location = new System.Drawing.Point(248, 32);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(104, 24);
            this.checkBox5.TabIndex = 26;
            this.checkBox5.Text = "图片";
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged_1);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(19, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "字";
            // 
            // MapSurround
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(360, 487);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox9);
            this.Controls.Add(this.checkBox7);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapSurround";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "地图整饰要素";
            this.Load += new System.EventHandler(this.MapSurround_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		/// <summary>
		/// 添加地图整饰要素窗体——按钮－－确定
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, System.EventArgs e)
		{
			MapSurround.ActiveForm.Hide();
		
		}
		/// <summary>
		/// 地图整饰要素窗体——单选框——格网
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
		{ 
			
			//设置measuredgrid属性
			m_MeasuredGrid.FixedOrigin = true;
			m_MeasuredGrid.Units = m_Map.MapUnits;
			m_MeasuredGrid.XIntervalSize = 5000;
			m_MeasuredGrid.XOrigin = -15000;
			m_MeasuredGrid.YIntervalSize = 5000;
			m_MeasuredGrid.YOrigin = -15000;

			//设置线的属性
			ISimpleLineSymbol m_SimpleLineSymbol;
			m_SimpleLineSymbol = new SimpleLineSymbol();
			m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
			m_SimpleLineSymbol.Width = 3;
			IRgbColor m_color;
			m_color = new RgbColor() as IRgbColor;
			m_color.Red = 0;
			m_color.Green = 0;
			m_color.Blue = 0;
			m_SimpleLineSymbol.Color = m_color;
			m_MapGrid.LineSymbol = m_SimpleLineSymbol;
			m_MapGrid.Name = "Measured Grid";



			//设置投影属性
			IProjectedGrid  m_ProjectedGrid;
			m_ProjectedGrid = m_MeasuredGrid as IProjectedGrid;
			m_ProjectedGrid.SpatialReference = m_Map.SpatialReference;
			m_MapGrid.Name = "Measured Grid";

			
			//添加到版式中
			m_MapGrids.AddMapGrid(m_MapGrid);
			m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);

		}
		/// <summary>
		/// 地图整饰要素窗体——单选框——网格边框
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox2_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				ISimpleMapGridBorder m_SimpleMapGridBorder;
				m_SimpleMapGridBorder = new SimpleMapGridBorderClass();
				//设置简单边框属性
				ISimpleLineSymbol m_SimpleLineSymbol;
				m_SimpleLineSymbol = new SimpleLineSymbol();
				m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
				//设置颜色+
				IRgbColor m_color;
				m_color = new RgbColor() as IRgbColor;
				m_color.Red = 255;
				m_color.Green = 0;
				m_color.Blue = 0;
				m_SimpleLineSymbol.Color = m_color;

				m_SimpleLineSymbol.Width = 4;

				m_SimpleMapGridBorder.LineSymbol = m_SimpleLineSymbol;
				this.m_MapGrid.Border = (IMapGridBorder)m_SimpleMapGridBorder;
				this.m_MapGrids.AddMapGrid(m_MapGrid);
			
				this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
			}
			catch{}
		}
		/// <summary>
		///  地图整饰要素窗体——单选框——标签
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox4_CheckedChanged(object sender, System.EventArgs e)
		{
			//标签格式设置
			IFormattedGridLabel m_FormattedGridLabel;
			m_FormattedGridLabel = new FormattedGridLabelClass();
			IGridLabel m_GridLabel;
			m_GridLabel = m_FormattedGridLabel as IGridLabel;

			stdole.IFontDisp m_Font; 
			m_Font = new StdFont() as IFontDisp;
			m_Font.Name = "Arial";
			m_Font.Size = 16;
			m_GridLabel.Font = m_Font;

			IRgbColor n_color;
			n_color = new RgbColor() as IRgbColor;
			n_color.Red = 255;
			n_color.Green = 0;
			n_color.Blue = 0;
			m_GridLabel.Color = n_color;
			m_GridLabel.LabelOffset = 2;

			INumericFormat m_NumericFormat;
			m_NumericFormat = new NumericFormatClass();
			m_NumericFormat.AlignmentOption = esriNumericAlignmentEnum.esriAlignRight;
			m_NumericFormat.RoundingOption = esriRoundingOptionEnum.esriRoundNumberOfDecimals;
			m_NumericFormat.RoundingValue = 2;
			m_NumericFormat.ShowPlusSign = false;
			m_NumericFormat.UseSeparator = true;
			m_NumericFormat.ZeroPad = true;
			m_FormattedGridLabel = m_NumericFormat as IFormattedGridLabel;
			
			m_MapGrid.LabelFormat = m_GridLabel;

			m_MapGrids.AddMapGrid(m_MapGrid);
			m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
		
		}
		/// <summary>
		/// 地图整饰要素窗体——单选框——经纬网
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox5_CheckedChanged(object sender, System.EventArgs e)
		{
			//设置measuredgrid属性
			m_MeasuredGrid.FixedOrigin = true;
			m_MeasuredGrid.Units = m_Map.MapUnits;
			m_MeasuredGrid.XIntervalSize = 10;
			m_MeasuredGrid.XOrigin = -180;
			m_MeasuredGrid.YIntervalSize = 10;
			m_MeasuredGrid.YOrigin = -90;

			//设置线的属性
			ISimpleLineSymbol m_SimpleLineSymbol;
			m_SimpleLineSymbol = new SimpleLineSymbol();
			m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
			m_SimpleLineSymbol.Width = 3;
			IRgbColor m_color;
			m_color = new RgbColor() as IRgbColor;
			m_color.Red = 0;
			m_color.Green = 0;
			m_color.Blue = 0;
			m_SimpleLineSymbol.Color = m_color;
			m_MapGrid.LineSymbol = m_SimpleLineSymbol;

			//设置投影属性
			IProjectedGrid  m_ProjectedGrid;
			m_ProjectedGrid = m_MeasuredGrid as IProjectedGrid;
			m_ProjectedGrid.SpatialReference = m_Map.SpatialReference;

			m_MapGrid.Name = "Graticule";
 
			
			//添加到版式中
			m_MapGrids.AddMapGrid(m_MapGrid);
			m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);

		
		}
		/// <summary>
		/// 地图整饰要素窗体——单选框——背景色
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox3_CheckedChanged(object sender, System.EventArgs e)
		{  
			IPage m_Page;
			m_Page = axPageLayoutControl.PageLayout.Page;
			//ColorDialog m_ColorDialog = new ColorDialog();
			//IBackground m_Background = this.m_MapFrame.Background;
	
			//创建调色板
			tagRECT m_Rect;
			m_Rect = new tagRECT();
			IRgbColor m_CurrColor;
			m_CurrColor = new RgbColor();
			m_CurrColor.Red = 255;
			m_CurrColor.Blue = 255;
			m_CurrColor.Green = 255;
			//创建色板对象
			ESRI.ArcGIS.Framework.IColorPalette m_ColorPalette;
			m_ColorPalette = new ColorPalette();
			if (checkBox3.Checked)
			{
				//MessageBox.Show("请选择背景颜色");
				if (!m_ColorPalette.TrackPopupMenu(ref m_Rect, m_CurrColor,false,0))
				{
					MessageBox.Show("取消");
				}
				else
				{
					IColor new_Color;
					new_Color = m_ColorPalette.Color;
					m_Page.BackgroundColor = new_Color;
					
				}
			}
			else
			{
				//MessageBox.Show("取消");
				m_Page.BackgroundColor = m_CurrColor;
			}
			
		}
		/// <summary>
		/// 地图整饰要素窗体——单选框——辅助线
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox6_CheckedChanged(object sender, System.EventArgs e)
		{   
			ISnapGuides m_SnapGuides;
			m_SnapGuides = axPageLayoutControl.PageLayout.HorizontalSnapGuides;
			if(checkBox6.Checked)
			{
				m_SnapGuides.AddGuide(5);
				m_SnapGuides.AreVisible = true;
				m_ActiveView.Refresh();
			}
			else
			{
				m_SnapGuides.RemoveAllGuides();
				m_ActiveView.Refresh();
			}
			
            
		}
		/// <summary>
		/// 地图整饰要素窗体——单选框——网点
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox7_CheckedChanged(object sender, System.EventArgs e)
		{
			ISnapGrid m_SnapGrid;
			m_SnapGrid = axPageLayoutControl.PageLayout.SnapGrid;
			if(checkBox7.Checked)
			{
				m_SnapGrid.VerticalSpacing = 2;
				m_SnapGrid.HorizontalSpacing = 1;
				m_SnapGrid.IsVisible = true;
				m_ActiveView.Refresh();
			}
			else
			{
				m_SnapGrid.IsVisible = false;
				m_ActiveView.Refresh();
			}
         
		}
		/// <summary>
		/// 地图整饰要素窗体——单选框——标尺
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//		private void checkBox8_CheckedChanged(object sender, System.EventArgs e)
		//		{
		//			IRulerSettings m_RulerSettings;
		//            m_RulerSettings = axPageLayoutControl.PageLayout.RulerSettings;
		//			if(checkBox8.Checked)
		//			{
		//				m_RulerSettings.SmallestDivision = 2;
		//				m_ActiveView.Refresh();
		//				//axPageLayoutControl.CtlRefresh(esriViewDrawPhase.esriViewBackground,Type.Missing,Type.Missing);
		//
		//
		//			}
		//			
		//		}
		/// <summary>
		/// 地图整饰要素窗体——复选框——边框
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox9_CheckedChanged(object sender, System.EventArgs e)
		{
			/*IPage m_Page;
			m_Page = axPageLayoutControl.PageLayout.Page;
			object symbol = null;

			//设置线的属性
			ISimpleLineSymbol m_SimpleLineSymbol;
			m_SimpleLineSymbol = new SimpleLineSymbol();
			m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
			m_SimpleLineSymbol.Width = 20;
			//设置线的颜色
			tagRECT m_Rect;
			m_Rect = new tagRECT();
			IRgbColor m_CurrColor;
			m_CurrColor = new RgbColor();
			m_CurrColor.Red = 255;
			m_CurrColor.Blue = 255;
			m_CurrColor.Green = 255;
			esriFramework.IColorPalette m_ColorPalette;
			m_ColorPalette = new ColorPalette();
			if (checkBox9.Checked)
			{
				MessageBox.Show("请选择边框颜色");
				if (!m_ColorPalette.TrackPopupMenu(ref m_Rect, m_CurrColor,false,0))
				{
					MessageBox.Show("取消");
				}
				else
				{
					IColor new_Color;
					new_Color = m_ColorPalette.Color;
					m_SimpleLineSymbol.Color= new_Color;
					//m_Page.Border =  (IBorder)m_SimpleLineSymbol;
					m_Border = m_SimpleLineSymbol as IBorder;
					m_Page.Border = m_Border;
					m_Page.DrawBorder(m_ActiveView.ScreenDisplay);
					m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
				}
			}
			else
			{
				MessageBox.Show("取消");
				m_SimpleLineSymbol.Color = m_CurrColor;
			}*/
			
			
            
		}
		/// <summary>
		/// 复选框——添加文字
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox10_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if(checkBox10.Checked)
				{
					this.richTextBox1.Enabled=true;
					this.txtFontSize.Enabled=true;
					//新建文字对象并设置属性
				
					m_TextElement.Text = this.richTextBox1.Text;
					//m_TextElement.Symbol.Size = 40;
				
					m_TextElement.ScaleText = true;
        
				
					IRgbColor m_color;
					m_color = new RgbColor() as IRgbColor;
					m_color.Red = 0;
					m_color.Green = 0;
					m_color.Blue = 0;
					m_SimpleTextSymbol.Color = m_color;
				 
					
					m_Font.Name = "Arial";
					m_Font.Size = Convert.ToDecimal(this.txtFontSize.Text);
					m_SimpleTextSymbol.Font = m_Font;
					//m_TextElement.Symbol.Font = m_Font;
					//m_TextElement.Symbol.Color = m_color;
			
					m_TextElement.Symbol = m_SimpleTextSymbol;

					//设置文字字符的几何形体属性
					m_Element =  m_TextElement as IElement;
					//m_Element.Geometry = m_Point;
					IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
					IEnvelope m_Envelope = this.axPageLayoutControl.ActiveView.Extent;
					pt.PutCoords((m_Envelope.XMax -m_Envelope.XMin)/2,(m_Envelope.YMax -m_Envelope.YMin)/2);
					m_Element.Geometry = pt;
		
					//添加到Page对象中并刷新以实时显示
					//try
					//{
					ITrackCancel m_TrackCancel;
					m_TrackCancel = new CancelTrackerClass();
					m_Element.Draw(m_ActiveView.ScreenDisplay,m_TrackCancel);
					m_GraphicsContainer.AddElement(m_Element,0);
                    axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewBackground,null,null );
				   			   	
				}

				else
				{
//					this.richTextBox1.Enabled=false;
//					this.txtFontSize.Enabled=false;
				}
			
			}
			catch
			{
				MessageBox.Show("操作错误，没有在制图中输入点");
							
			}
			
		}
		/// <summary>
		/// 初始化ComboBox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MapSurround_Load(object sender, System.EventArgs e)
		{
			m_StylesArray[0] = new ESRI.ArcGIS.esriSystem.ArrayClass();
			UpdateArrayAndComboBoxFromStyleGallery("Borders", m_StylesArray[0], comboBox1);

		}
		/// <summary>
		/// 调用函数－－线型选择
		/// </summary>
		/// <param name="styleClass"></param>
		/// <param name="array"></param>
		/// <param name="ComboBox"></param>
		private void UpdateArrayAndComboBoxFromStyleGallery(string styleClass, ESRI.ArcGIS.esriSystem.IArray array, System.Windows.Forms.ComboBox  ComboBox)
		{
			//If the StyleGallery hasn't been used before
			if (m_StyleGallery == null)
			{
				m_StyleGallery = new StyleGalleryClass();
				IntPtr pointer = System.Runtime.InteropServices.Marshal.GetComInterfaceForObject(m_StyleGallery, typeof(IStyleGallery)); 
			}

			//Get IEnumStyleGalleryItem interface and retrieve all styles within the class
			IEnumStyleGalleryItem enumStyleGallery = m_StyleGallery.get_Items(styleClass, "ESRI.style", "");
			enumStyleGallery.Reset();
						
			//Clear out the list box
			ComboBox.Items.Clear();

			//Get IStyleGalleryItem interface
			IStyleGalleryItem styleItem = enumStyleGallery.Next();

			//Loop through the style gallery items
			while (styleItem != null)
			{
				//Add the style to the array
				array.Add(styleItem.Item);
				//Add the style name to the list box
				ComboBox.Items.Add(styleItem.Name);
				styleItem = enumStyleGallery.Next();
			}
			ComboBox.SelectedIndex = 0;
		}
		/// <summary>
		/// 地图整饰要素窗体——复合列表——选择线型
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			object symbol = null;
			symbol = m_StylesArray[0].get_Element(comboBox1.SelectedIndex);

			//Get IPage interface
			IPage page = axPageLayoutControl.Page;
			if (checkBox9.Checked)
			{
				//Apply the symbol as a property to the page
				page.Border = (IBorder) symbol;
				axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewBackground,null,null);

			}

			
		}
		/// <summary>
		/// 输入框——改变字体大小
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox2_TextChanged(object sender, System.EventArgs e)
		{
			//this.m_SimpleTextSymbol.Font.Size = Convert.ToDecimal(textBox2.Text);
			this.m_Font.Size = Convert.ToDecimal(this.txtFontSize.Text);
			this.m_SimpleTextSymbol.Font = this.m_Font;
			this.m_TextElement.Symbol = this.m_SimpleTextSymbol;
			//m_GraphicsContainer.Reset();
			axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics,null,null);
			
		}
		/// <summary>
		/// 按钮——取消
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, System.EventArgs e)
		{
			// m_GraphicsContainer.DeleteAllElements();
            if (m_GraphicsContainer!=null && m_Element != null)
            {
                m_GraphicsContainer.DeleteElement(this.m_Element);
                axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }

			
			
			MapSurround.ActiveForm.Close();
		}
        /// <summary>
        /// 按钮——设定文字颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button3_Click(object sender, System.EventArgs e)
		{
			tagRECT m_Rect;
			m_Rect = new tagRECT();
			IRgbColor m_CurrColor;
			m_CurrColor = new RgbColor();
			m_CurrColor.Red = 255;
			m_CurrColor.Blue = 255;
			m_CurrColor.Green = 255;
			ESRI.ArcGIS.Framework.IColorPalette m_ColorPalette;
			m_ColorPalette = new ColorPalette();
			if (checkBox10.Checked)
			{
				if (!m_ColorPalette.TrackPopupMenu(ref m_Rect, m_CurrColor,false,0))
				{
					MessageBox.Show("取消");
				}
				else
				{
					IColor new_Color;
					new_Color = m_ColorPalette.Color;
					this.m_SimpleTextSymbol.Color = new_Color;
					this.m_TextElement.Symbol= m_SimpleTextSymbol;
					//m_GraphicsContainer.Reset();
					//axPageLayoutControl.CtlRefresh(esriViewDrawPhase.esriViewGraphics,Type.Missing,Type.Missing);
					m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
				}
			}
			
		}
        /// <summary>
        /// 复选框——添加指北针
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void checkBox1_CheckedChanged_1(object sender, System.EventArgs e)
		{  
			IActiveView m_ActiveView = this.axPageLayoutControl.PageLayout as IActiveView;
			IEnvelope m_Envelope = new Envelope() as IEnvelope;
			UID m_ID = new UID();
			IMapSurround m_MapSurround;
			
			//ICharacterMarkerSymbol m_CharacterMarkerSymbol;

			m_ID.Value = "esriCarto.MarkerNorthArrow";
			m_Envelope.PutCoords(5,22,8,26);
			m_MapSurround = CreateSurround(m_ID,m_Envelope,"NorthArrow",axPageLayoutControl.ActiveView.FocusMap,axPageLayoutControl.PageLayout);

			//设定指北针样式
			
			m_MarkerNorthArrow = m_MapSurround as IMarkerNorthArrow;
//			m_CharacterMarkerSymbol = (ICharacterMarkerSymbol)m_MarkerNorthArrow.MarkerSymbol;
//			m_CharacterMarkerSymbol.CharacterIndex = 50;
//			m_MarkerNorthArrow.MarkerSymbol = m_CharacterMarkerSymbol;
            m_MapSurround = m_MarkerNorthArrow as IMapSurround;

		    m_ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics,null,null);
		}

        /// <summary>
        /// 函数——建立mapsurround对象
        /// </summary>
        /// <param name="m_ID"></param>
        /// <param name="m_Envelope"></param>
        /// <param name="StringName"></param>
        /// <param name="m_Map"></param>
        /// <param name="m_PageLayout"></param>
        /// <returns></returns>
		private IMapSurround CreateSurround(UID m_ID,IEnvelope m_Envelope, String  StringName,IMap m_Map, IPageLayout m_PageLayout)
		{
			IGraphicsContainer m_GraphicsContainer;
			IActiveView m_ActiveView;
			IMapSurroundFrame m_MapSurroundFrame;
			IMapSurround m_MapSurround;
			IMapFrame m_MapFrame;
			IElement m_Element;
              
			m_GraphicsContainer = (IGraphicsContainer)m_PageLayout;
			m_MapFrame = m_GraphicsContainer.FindFrame(m_Map) as IMapFrame;
			m_MapSurroundFrame = m_MapFrame.CreateSurroundFrame(m_ID, null);
			m_MapSurroundFrame.MapSurround.Name = StringName;

            m_Element = m_MapSurroundFrame as IElement;
			m_ActiveView =(IActiveView) m_PageLayout;
            m_Element.Geometry = m_Envelope;
            m_Element.Activate(m_ActiveView.ScreenDisplay);

			ITrackCancel m_TrackCancel;
			m_TrackCancel = new CancelTrackerClass();

			m_Element.Draw(m_ActiveView.ScreenDisplay,m_TrackCancel);
            m_GraphicsContainer.AddElement(m_Element,0);
			//axPageLayoutControl.AddElement(m_Element,m_MapSurroundFrame,m_Envelope,m_MarkerNorthArrow,0);
			return m_MapSurroundFrame.MapSurround;
			
		}
        /// <summary>
        /// 复选框——添加比例尺
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void checkBox2_CheckedChanged_1(object sender, System.EventArgs e)
		{
			//IActiveView m_ActiveView = this.axPageLayoutControl.PageLayout as IActiveView;
			IActiveView m_ActiveView;
			IEnvelope m_Envelope = new Envelope() as IEnvelope;
			UID m_ID = new UID();
			IMapSurround m_MapSurround;
			;

			m_ID.Value = "esriCarto.ScaleBar";
			m_Envelope.PutCoords(5,22,20,24);
			//m_MapSurround = CreateSurround(m_ID,m_Envelope,"ScaleBar",axPageLayoutControl.ActiveView.FocusMap,axPageLayoutControl.PageLayout);
			
			IGraphicsContainer m_GraphicsContainer;
			m_GraphicsContainer = this.axPageLayoutControl.PageLayout as IGraphicsContainer;
			m_ActiveView = m_GraphicsContainer as IActiveView;
			IMap m_Map;
			m_Map = m_ActiveView.FocusMap;

			IMapFrame m_MapFrame;
			m_MapFrame = m_GraphicsContainer.FindFrame(m_Map) as IMapFrame;
			IMapSurroundFrame m_MapSurroundFrame;
			m_MapSurroundFrame = m_MapFrame.CreateSurroundFrame(m_ID,null);

            
			//设置比例尺属性
			//m_ScaleBar = new AlternatingScaleBar();
            m_ScaleBar.Division = 4;
			m_ScaleBar.Divisions = 4;
			m_ScaleBar.LabelGap = 4;
			m_ScaleBar.Map =  m_ActiveView .FocusMap;
		    m_ScaleBar.Name = "比例尺";
			m_ScaleBar.Subdivisions = 2;
			m_ScaleBar.UnitLabel = "千米";
			m_ScaleBar.UnitLabelGap = 4;
			m_ScaleBar.UnitLabelPosition = esriScaleBarPos.esriScaleBarAfterLabels;
			m_ScaleBar.Units = esriUnits.esriKilometers;

            m_MapSurround = m_ScaleBar;
            m_MapSurroundFrame.MapSurround = m_MapSurround;
			
			m_Element = m_MapSurroundFrame as IElement;
			m_Element.Geometry = m_Envelope;
			m_Element.Activate(m_ActiveView.ScreenDisplay);

			ITrackCancel m_TrackCancel;
			m_TrackCancel = new CancelTrackerClass();

            if(checkBox2.Checked)
            {
				m_Element.Draw(m_ActiveView.ScreenDisplay,m_TrackCancel);
				m_GraphicsContainer.AddElement(m_Element,0);
				m_ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics,null,null);
            }
			
		}
        /// <summary>
        /// 复合框——改变比例尺样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		    int index = this.comboBox2.SelectedIndex;
			switch(index)
			{
				case 0:this.m_ScaleBar = new AlternatingScaleBar();break;
				case 1:this.m_ScaleBar = new DoubleAlternatingScaleBar();break;
				case 2:this.m_ScaleBar = new HollowScaleBar();break;
				case 3:this.m_ScaleBar = new ScaleLine();break;
				case 4:this.m_ScaleBar = new SingleDivisionScaleBar();break;
				case 5:this.m_ScaleBar = new SteppedScaleLine();break;
			}
			 m_ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics,null,null);
            
		}
        /// <summary>
        /// 复选框——添加图例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void checkBox4_CheckedChanged_1(object sender, System.EventArgs e)
		{
			IActiveView m_ActiveView = this.axPageLayoutControl.PageLayout as IActiveView;
			IEnvelope m_Envelope = new Envelope() as IEnvelope;
			UID m_ID = new UID();
			IMapSurround m_MapSurround;
			IMap m_map = m_ActiveView.FocusMap;

			m_ID.Value = "esriCarto.Legend";
			m_Envelope.PutCoords(5,22,8,26);
			m_MapSurround = CreateSurround(m_ID,m_Envelope,"Legend",axPageLayoutControl.ActiveView.FocusMap,axPageLayoutControl.PageLayout);
            
			//设置legend属性
			ILegend m_Legend = new Legend();
            m_Legend = m_map.get_MapSurround(0) as ILegend;
            ILegendItem m_LegendItem = m_Legend.get_Item(0);
            ILegendFormat m_LegendFormat = m_Legend.Format;
			m_LegendItem.ShowLabels = true;
            m_LegendItem.ShowLayerName = false;
			m_LegendItem.ShowHeading = true;
			m_LegendItem.ShowDescriptions = false;

//			  IPatch m_Patch = new AreaPatchClass();
//            m_Patch.Geometry = m_Envelope;
//            m_LegendFormat.DefaultAreaPatch =  m_Patch as IAreaPatch;
			m_Legend.Name = "图例";
			m_Legend.Title = "图例";
			m_Legend.Refresh();

            m_MapSurround = m_Legend;
			m_ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics,null,null);
		}
        /// <summary>
        /// listView——改变指北针样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ICharacterMarkerSymbol m_CharacterMarkerSymbol;
			m_CharacterMarkerSymbol = (ICharacterMarkerSymbol)m_MarkerNorthArrow.MarkerSymbol;
			IEnumerator ienum = this.listView1.SelectedIndices.GetEnumerator();
			while(ienum.MoveNext())
			{
				string sel = ienum.Current.ToString();  
//				MessageBox.Show(sel); 
				int index = Convert.ToInt32(sel);
				switch(index)
				{
					case 0:m_CharacterMarkerSymbol.CharacterIndex = 45;break;
					case 1:m_CharacterMarkerSymbol.CharacterIndex = 46;break;
					case 2:m_CharacterMarkerSymbol.CharacterIndex = 49;break;
					case 3:m_CharacterMarkerSymbol.CharacterIndex = 51;break;
					case 4:m_CharacterMarkerSymbol.CharacterIndex = 59;break;
					case 5:m_CharacterMarkerSymbol.CharacterIndex = 66;break;
					case 6:m_CharacterMarkerSymbol.CharacterIndex = 75;break;
					case 7:m_CharacterMarkerSymbol.CharacterIndex = 86;break;
					case 8:m_CharacterMarkerSymbol.CharacterIndex = 94;break;
					case 9:m_CharacterMarkerSymbol.CharacterIndex = 113;break;
					case 10:m_CharacterMarkerSymbol.CharacterIndex = 33;break;
                    case 11:m_CharacterMarkerSymbol.CharacterIndex = 41;break;
					case 12:m_CharacterMarkerSymbol.CharacterIndex = 172;break;
					case 13:m_CharacterMarkerSymbol.CharacterIndex = 177;break;
					case 14:m_CharacterMarkerSymbol.CharacterIndex = 179;break;
				}
			    m_MarkerNorthArrow.MarkerSymbol = m_CharacterMarkerSymbol;
				m_ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics,null,null);

			}

		}
        /// <summary>
        /// 复选框——插入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void checkBox5_CheckedChanged_1(object sender, System.EventArgs e)
		{
		    IPictureElement m_PictureElement;
			m_PictureElement = new BmpPictureElementClass();

			//得到文件名：路径＋名称＋后缀
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Title = "添加位图文件";
			openFileDialog1.DefaultExt = ".bmp";
			openFileDialog1.Filter = "Bitmap (*.bmp)|*.bmp";
			if(openFileDialog1.ShowDialog() != DialogResult.OK) return;		
			string  str_filepath;
			str_filepath = openFileDialog1.FileName;			// 得到打开的文件路径

			//确定使用位图文件
			m_PictureElement.ImportPictureFromFile(str_filepath);
			//保持文件长宽比例
			m_PictureElement.MaintainAspectRatio = true;
            
			IEnvelope m_Envelope = new Envelope() as IEnvelope;
			m_Envelope.PutCoords(5,22,8,26);
			this.m_Element = m_PictureElement as IElement;

			//确定元素Geometry属性
			m_Element.Geometry = m_Envelope;

			ITrackCancel m_TrackCancel;
			m_TrackCancel = new CancelTrackerClass();

			if(checkBox5.Checked)
			{
				m_Element.Draw(m_ActiveView.ScreenDisplay,m_TrackCancel);
				m_GraphicsContainer.AddElement(m_Element,0);
				m_ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics,null,null);
			}   

		}
        /// <summary>
        /// 复合框——改变字体类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void comboBox3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int  index = this.comboBox3.SelectedIndex;
			switch(index)
			{
				case 0: this.m_Font.Name = "Arial";break;
				case 1: this.m_Font.Name = "宋体";break;
				case 2: this.m_Font.Name = "隶书";break;
				case 3: this.m_Font.Name = "黑体";break;
				case 4: this.m_Font.Name = "幼圆";break;
				case 5: this.m_Font.Name = "楷体_GB2312";break;
				case 6: this.m_Font.Name = "方正姚体";break;
				case 7: this.m_Font.Name = "华文新魏";break;				
			}
			this.m_SimpleTextSymbol.Font = this.m_Font;
			this.m_TextElement.Symbol = this.m_SimpleTextSymbol;
			axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics,null,null);
		}
        /// <summary>
        /// 按钮——字体加粗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button3_Click_1(object sender, System.EventArgs e)
		{
            isBold = !isBold;
            if(isBold)
            {
              	  this.m_Font.Bold = true;
            }
			else this.m_Font.Bold = false;
			this.m_SimpleTextSymbol.Font = this.m_Font;
			this.m_TextElement.Symbol = this.m_SimpleTextSymbol;
            axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);

		}
        /// <summary>
        /// 按钮——斜体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button4_Click(object sender, System.EventArgs e)
		{
		    this.isItalic = !this.isItalic;
			if(isItalic)
			{
				this.m_Font.Italic= true;
			}
			else this.m_Font.Italic = false;
			this.m_SimpleTextSymbol.Font = this.m_Font;
			this.m_TextElement.Symbol = this.m_SimpleTextSymbol;
            axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);

		}
        /// <summary>
        /// 按钮——下划线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button5_Click(object sender, System.EventArgs e)
		{
		    this.isUnderline = !this.isUnderline;
			if(isUnderline)
			{
				this.m_Font.Underline= true;
			}
			else this.m_Font.Underline = false;
			this.m_SimpleTextSymbol.Font = this.m_Font;
			this.m_TextElement.Symbol = this.m_SimpleTextSymbol;
            axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);

		}
        /// <summary>
        /// 按钮——更多……
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button6_Click(object sender, System.EventArgs e)
		{
		    this.m_SimpleTextSymbol = new TextSymbol() as ISimpleTextSymbol;
			ISymbol m_Symbol = m_SimpleTextSymbol as ISymbol;
			ISymbolSelector m_SymbolSelector = new SymbolSelectorClass();
			bool ok = false;
            m_SymbolSelector.AddSymbol(m_Symbol);
			ok = m_SymbolSelector.SelectSymbol(0);
			if(ok)
			{
				m_Symbol = m_SymbolSelector.GetSymbolAt(0);
			}
            this.m_SimpleTextSymbol = m_Symbol as ISimpleTextSymbol;
            this.m_TextElement.Symbol =m_SimpleTextSymbol;
            axPageLayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);

		}
       
        
//		private void stylenum_TextChanged(object sender, System.EventArgs e)
//		{
//			try
//			{
//				ICharacterMarkerSymbol m_CharacterMarkerSymbol;
//				m_CharacterMarkerSymbol = (ICharacterMarkerSymbol)m_MarkerNorthArrow.MarkerSymbol;
//				m_CharacterMarkerSymbol.CharacterIndex = Convert.ToInt32(this.stylenum.Text);
//				m_MarkerNorthArrow.MarkerSymbol = m_CharacterMarkerSymbol;
//				m_ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics,null,null);
//			}
//			catch{}
//			
//		}
       
	}
}
