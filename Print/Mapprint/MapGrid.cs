using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
//using ESRI.ArcGIS.Utility.COMSupport;
using ESRI.ArcGIS.Framework;
using stdole;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;

namespace Print.Mapprint
{
	/// <summary>
	/// MapGrid 的摘要说明。
	/// </summary>
	public class MapGrid : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.CheckBox checkBox9;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		private AxPageLayoutControl axPageLayoutControl;
		private IActiveView m_ActiveView;
		private IGraphicsContainer m_GraphicsContainer;
		private IMapFrame m_MapFrame;
		private IMap m_Map;
		private IMapGrids m_MapGrids;
		private IMapGrid m_MapGrid;
		private IMeasuredGrid  m_MeasuredGrid;
		private ESRI.ArcGIS.esriSystem.IArray[] m_StylesArray ;
		private IStyleGallery m_StyleGallery;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.RadioButton radioButton6;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Label label10;
		private ISimpleLineSymbol m_SimpleLineSymbol;
		private ISimpleLineSymbol m_BorderSimpleLineSymbol;
		private IGridLabel m_GridLabel = null;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textBox6;
		private ISimpleMarkerSymbol m_MarkerSymbol = new SimpleMarkerSymbol();
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private ISimpleMapGridBorder m_SimpleMapGridBorder = new SimpleMapGridBorderClass();

		public MapGrid(AxPageLayoutControl axPageLayoutControl)
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
			//获得pagelayout基本对象
			this.axPageLayoutControl = axPageLayoutControl;
			m_ActiveView =(IActiveView)this.axPageLayoutControl.PageLayout;
			m_Map = m_ActiveView.FocusMap;

			//获得版式视图
			m_GraphicsContainer = (IGraphicsContainer)this.axPageLayoutControl.PageLayout;
			m_MapFrame = (IMapFrame)m_GraphicsContainer.FindFrame(m_Map);		
			m_MapGrids = (IMapGrids)m_MapFrame;

            m_SimpleLineSymbol = new SimpleLineSymbol();
            m_BorderSimpleLineSymbol = new SimpleLineSymbol() as ISimpleLineSymbol;
			m_StylesArray = new ESRI.ArcGIS.esriSystem.ArrayClass[1];

			//radioButton1.Checked = true;

			//初始化COMBOBOX1
			comboBox1.Items.Add("实线");
			comboBox1.Items.Add("虚线");
			comboBox1.Items.Add("点线");
			comboBox1.Items.Add("点与短横交替线");
			comboBox1.Items.Add("短横与两点交替线");
			comboBox1.Items.Add("不可见");
			comboBox1.SelectedIndex = 0;
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "步骤一";
            // 
            // radioButton2
            // 
            this.radioButton2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton2.Location = new System.Drawing.Point(155, 38);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(104, 24);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.Text = "经纬线格网";
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton1.Location = new System.Drawing.Point(18, 36);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(104, 24);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.Text = "标准公里格网";
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(48, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "选择需要绘制的格网类型";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox2.Location = new System.Drawing.Point(16, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 143);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "步骤二";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(8, 32);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(136, 101);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "公里格网";
            // 
            // label13
            // 
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(92, 73);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 16);
            this.label13.TabIndex = 5;
            this.label13.Text = "米";
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(92, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 16);
            this.label12.TabIndex = 4;
            this.label12.Text = "米";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(8, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "y轴";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(8, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "X轴";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(36, 69);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(56, 21);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "1000";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(36, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(56, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "1000";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(54, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "输入格网间距";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.textBox4);
            this.groupBox4.Controls.Add(this.textBox3);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Location = new System.Drawing.Point(152, 32);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(136, 101);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Tag = "8";
            this.groupBox4.Text = "经纬线格网";
            // 
            // label15
            // 
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(112, 70);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(16, 23);
            this.label15.TabIndex = 6;
            this.label15.Text = "°";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(4, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "纬线方向";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(4, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 3;
            this.label6.Text = "经线方向";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(62, 70);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(50, 21);
            this.textBox4.TabIndex = 2;
            this.textBox4.Text = "1";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(62, 30);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(50, 21);
            this.textBox3.TabIndex = 1;
            this.textBox3.Text = "1";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label14
            // 
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(112, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(16, 23);
            this.label14.TabIndex = 5;
            this.label14.Text = "°";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.checkBox3);
            this.groupBox5.Controls.Add(this.checkBox2);
            this.groupBox5.Controls.Add(this.checkBox1);
            this.groupBox5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox5.Location = new System.Drawing.Point(16, 266);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(296, 57);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "步骤三";
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(48, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 23);
            this.label8.TabIndex = 3;
            this.label8.Text = "选择需要添加的格网要素";
            // 
            // checkBox3
            // 
            this.checkBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBox3.Location = new System.Drawing.Point(152, 23);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(104, 24);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "Tick点";
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBox2.Location = new System.Drawing.Point(88, 23);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(64, 24);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "标签";
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBox1.Location = new System.Drawing.Point(16, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 24);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "边框";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.textBox6);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.textBox5);
            this.groupBox6.Controls.Add(this.radioButton6);
            this.groupBox6.Controls.Add(this.radioButton5);
            this.groupBox6.Controls.Add(this.radioButton4);
            this.groupBox6.Controls.Add(this.radioButton3);
            this.groupBox6.Controls.Add(this.comboBox1);
            this.groupBox6.Controls.Add(this.checkBox9);
            this.groupBox6.Controls.Add(this.checkBox4);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox6.Location = new System.Drawing.Point(16, 340);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(296, 192);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "步骤四";
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.Location = new System.Drawing.Point(174, 91);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(32, 21);
            this.textBox6.TabIndex = 25;
            this.textBox6.Text = "3";
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(14, 94);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 23);
            this.label11.TabIndex = 24;
            this.label11.Text = "输入标签或者tick点大小";
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(14, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 16);
            this.label10.TabIndex = 23;
            this.label10.Text = "输入线宽";
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Location = new System.Drawing.Point(80, 62);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(32, 21);
            this.textBox5.TabIndex = 22;
            this.textBox5.Text = "3";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // radioButton6
            // 
            this.radioButton6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton6.Location = new System.Drawing.Point(200, 30);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(64, 24);
            this.radioButton6.TabIndex = 21;
            this.radioButton6.Text = "格网";
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton5.Location = new System.Drawing.Point(72, 30);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(48, 24);
            this.radioButton5.TabIndex = 20;
            this.radioButton5.Text = "标签";
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton4.Location = new System.Drawing.Point(136, 30);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(64, 24);
            this.radioButton4.TabIndex = 19;
            this.radioButton4.Text = "Tick点";
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton3.Location = new System.Drawing.Point(16, 30);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(48, 24);
            this.radioButton3.TabIndex = 18;
            this.radioButton3.Text = "边框";
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.Location = new System.Drawing.Point(110, 150);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.Text = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // checkBox9
            // 
            this.checkBox9.Enabled = false;
            this.checkBox9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBox9.Location = new System.Drawing.Point(14, 150);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(112, 24);
            this.checkBox9.TabIndex = 16;
            this.checkBox9.Text = "选择线型";
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.Enabled = false;
            this.checkBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBox4.Location = new System.Drawing.Point(14, 125);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(112, 24);
            this.checkBox4.TabIndex = 1;
            this.checkBox4.Text = "选择颜色";
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(54, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 16);
            this.label9.TabIndex = 0;
            this.label9.Text = "选择要设置的要素";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(200, 552);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "取消";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(64, 552);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 31);
            this.button2.TabIndex = 5;
            this.button2.Text = "完成";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MapGrid
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(344, 598);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MapGrid";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "地理格网";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
        /// <summary>
        /// 单选框——选择公里格网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
		{
			
		
				m_MeasuredGrid = new MeasuredGridClass();
				m_MapGrid = m_MeasuredGrid as IMapGrid;
				CreateGrid();


                //m_MeasuredGrid.XIntervalSize = Convert.ToDouble(textBox1.Text);
                //this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
			
		}
        /// <summary>
        /// 单选框——选择经纬网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
		{
			
				m_MeasuredGrid = new GraticuleClass();
				m_MapGrid = m_MeasuredGrid as IMapGrid;
				CreateGrid();
		
		}

		/// <summary>
		/// 函数——建立格网
		/// </summary>
		private void CreateGrid()
		{
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" ) return;
            
                m_MeasuredGrid.XIntervalSize = Convert.ToDouble(textBox1.Text);
            
            

			m_MapGrids.ClearMapGrids();
			m_ActiveView.Refresh();
			//设置measuredgrid属性
			if(radioButton1.Checked)
			{
				m_MeasuredGrid.FixedOrigin = true;
			    //IDisplayTransformation m_DisplayTransformation;
				//m_DisplayTransformation =this.axPageLayoutControl.ActiveView.ScreenDisplay.DisplayTransformation
				//m_MeasuredGrid.Units = m_Map.MapUnits;
                m_MeasuredGrid.Units = esriUnits.esriMeters;
				m_MeasuredGrid.XIntervalSize = Convert.ToDouble(textBox1.Text);
				m_MeasuredGrid.XOrigin = -15000;
				m_MeasuredGrid.YIntervalSize = Convert.ToDouble(textBox2.Text);
				m_MeasuredGrid.YOrigin = -15000;
				m_MapGrid.Name = "Measured Grid";

			}

			if(radioButton2.Checked)
			{
				m_MeasuredGrid.FixedOrigin = true;
				m_MeasuredGrid.Units = m_Map.MapUnits;
				m_MeasuredGrid.XIntervalSize = Convert.ToDouble(textBox3.Text);
				m_MeasuredGrid.XOrigin = -180;
				m_MeasuredGrid.YIntervalSize = Convert.ToDouble(textBox4.Text);
				m_MeasuredGrid.YOrigin = -90;
				m_MapGrid.Name = "Graticule";
			}
			


			//设置线的属性
			m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
			m_SimpleLineSymbol.Width = 1;

			//设置线的颜色
			if(checkBox4.Checked)
			{ 
	
			}
			else
			{
				IRgbColor m_color;
				m_color = new RgbColor() as IRgbColor;
				m_color.Red = 0;
				m_color.Green = 0;
				m_color.Blue = 0;
				m_SimpleLineSymbol.Color = m_color;
			}
			m_MapGrid.LineSymbol = m_SimpleLineSymbol;
			
			//设置投影属性
			IProjectedGrid  m_ProjectedGrid;
			m_ProjectedGrid = m_MeasuredGrid as IProjectedGrid;
			if(radioButton1.Checked)
			{
				//m_ProjectedGrid.SpatialReference = this.axPageLayoutControl.ActiveView.FocusMap.SpatialReference;
			}
			
			if (radioButton2.Checked)
			{
				/*//ISpatialReferenceDialog2 m_SpatialReferenceDialog;
				//IProjectedCoordinateSystem m_SpatialReference;
				SpatialReferenceEnvironment m_SpatialReferenceEnvironment;
				m_SpatialReferenceEnvironment = new SpatialReferenceEnvironment();
				IGeographicCoordinateSystem geographicCoordinateSystem = null;
				IProjectedCoordinateSystem projectedCoordinateSystem = null;
				ISpatialReference SR;
				geographicCoordinateSystem = m_SpatialReferenceEnvironment.CreateGeographicCoordinateSystem((int) esriSRGeoCSType.esriSRGeoCS_Clarke1858);
				projectedCoordinateSystem = m_SpatialReferenceEnvironment.CreateProjectedCoordinateSystem((int) esriSRProjCSType.esriSRProjCS_Beijing1954GK_18N);
				SR = projectedCoordinateSystem;
				//SR = axPageLayoutControl.ActiveView.FocusMap.SpatialReference;
				axPageLayoutControl.ActiveView.FocusMap.SpatialReference = SR;
				//m_ProjectedGrid.SpatialReference = m_Map.SpatialReference ;
				//m_MeasuredGrid = m_ProjectedGrid as IMeasuredGrid;*/

			}
			//m_ProjectedGrid.SpatialReference = axPageLayoutControl.ActiveView.FocusMap.SpatialReference;
			
			//添加到版式中
		    
			m_MapGrids.AddMapGrid(m_MapGrid);
			m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);

		}
        /// <summary>
        /// 按钮——完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button2_Click(object sender, System.EventArgs e)
		{
            MapGrid.ActiveForm.Hide();
			
		}
        /// <summary>
        /// 复选框——添加格网边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
		{
		     CreateBorder();
		}

		/// <summary>
		/// 复选框————添加标签
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox2_CheckedChanged(object sender, System.EventArgs e)
		{
            if (checkBox2.Checked)
            {
                CreateLabel();
            }
            else
            {
                if (m_MapGrids != null && m_MapGrid != null)
                {
                    m_MapGrids.DeleteMapGrid(m_MapGrid);
                    m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                }
            }
		}
		/// <summary>
		/// 复选框————添加tick点
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox3_CheckedChanged(object sender, System.EventArgs e)
		{
		    CreateTickMark();
		}

        /// <summary>
        /// 复选框——选择网格颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void checkBox4_CheckedChanged(object sender, System.EventArgs e)
		{
            try
            {
                /*if(checkBox4.Checked)
                {
                    tagRECT m_Rect;
                    m_Rect = new tagRECT();
                    IRgbColor m_CurrColor;
                    m_CurrColor = new RgbColor();
                    m_CurrColor.Red = 255;
                    m_CurrColor.Blue = 255;
                    m_CurrColor.Green = 255;
                    //创建色板对象
                    esriFramework.IColorPalette m_ColorPalette;
                    m_ColorPalette = new ColorPalette();
                    MessageBox.Show("请选择网格线颜色");
                    if (!m_ColorPalette.TrackPopupMenu(ref m_Rect, m_CurrColor,false,0))
                    {
                        MessageBox.Show("取消");
                    }
                    else
                    {
                        IColor new_Color;
                        new_Color = m_ColorPalette.Color;
                        m_SimpleLineSymbol.Color = new_Color;
                    }
                }*/

                if (checkBox4.Checked)
                {

                    //设置边框颜色
                    if (radioButton3.Checked & checkBox1.Checked)
                    {
                        //MessageBox.Show("请选择边框颜色");
                        this.m_BorderSimpleLineSymbol.Color = CreateColor();
                    }
                    else if (radioButton3.Checked & checkBox1.Checked == false)
                    {
                        MessageBox.Show("请添加边框");
                    }

                    //设置标签颜色
                    if (radioButton5.Checked)
                    {
                        //MessageBox.Show("请选择标签颜色");
                        m_MapGrid.LabelFormat.Color = CreateColor();
                    }
                    //设置tick点颜色
                    if (radioButton4.Checked & checkBox3.Checked)
                    {
                        //MessageBox.Show("请选择TICK点颜色");
                        m_MarkerSymbol.Color = CreateColor();
                    }
                    else if (radioButton4.Checked & checkBox3.Checked == false)
                    {
                        MessageBox.Show("请添加TICK点");
                    }

                    //设置网格颜色
                    if (radioButton6.Checked)
                    {
                        //MessageBox.Show("请选择格网颜色");
                        m_SimpleLineSymbol.Color = CreateColor();
                    }
                    //刷新
                    checkBox4.Checked = false;
                    m_MapGrids.AddMapGrid(m_MapGrid);
                    this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                }

            }
            catch
            {
            }
		}
        /// <summary>
        /// 复选框——选择线型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void checkBox9_CheckedChanged(object sender, System.EventArgs e)
		{
  
		}
        
        /// <summary>
        /// 选择复合框——改变线型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                int index = comboBox1.SelectedIndex;
                if (radioButton6.Checked)//设置网格颜色
                {
                    switch (index)
                    {
                        case (0): m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid; break;
                        case (1): m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDash; break;
                        case (2): m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDot; break;
                        case (3): m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDashDot; break;
                        case (4): m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDashDotDot; break;
                        case (5): m_SimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSNull; break;
                    }

                }
                if (radioButton3.Checked)//设置边框颜色
                {
                    switch (index)
                    {
                        case (0): m_BorderSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid; break;
                        case (1): m_BorderSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDash; break;
                        case (2): m_BorderSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDot; break;
                        case (3): m_BorderSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDashDot; break;
                        case (4): m_BorderSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDashDotDot; break;
                        case (5): m_BorderSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSNull; break;
                    }
                    m_SimpleMapGridBorder.LineSymbol = m_BorderSimpleLineSymbol;
                    m_MapGrid.Border = (IMapGridBorder)m_SimpleMapGridBorder;
                    //this.m_MapGrids.AddMapGrid(m_MapGrid);
                    //this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
                }
                //			m_MapGrids.AddMapGrid(m_MapGrid);
                //			this.checkBox9.Checked = false;
                m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
            }
            catch (SystemException errs)
            {
                MessageBox.Show(errs.Message, "错误提示", MessageBoxButtons.OK);
            }

		}

       /// <summary>
       /// 函数——建立边框
       /// </summary>
		private void CreateBorder()
		{
			try
			{
				//m_SimpleMapGridBorder = new SimpleMapGridBorderClass();
				//设置简单边框属性
				//m_BorderSimpleLineSymbol = new SimpleLineSymbol();
				m_BorderSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                
				//设置颜色
				IRgbColor m_color;
				m_color = new RgbColor() as IRgbColor;
				m_color.Red = 0;
				m_color.Green = 0;
				m_color.Blue = 0;
				m_BorderSimpleLineSymbol.Color = m_color;
				
				m_BorderSimpleLineSymbol.Width = 3;

				m_SimpleMapGridBorder.LineSymbol = m_BorderSimpleLineSymbol;
				//this.m_MapGrid.Border = (IMapGridBorder)m_SimpleMapGridBorder;
				this.m_MapFrame.Border = m_SimpleMapGridBorder as IBorder;

				this.m_MapGrids.AddMapGrid(m_MapGrid);
				this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
			}
			catch{}
		}
        /// <summary>
        /// 函数——建立label
        /// </summary>
		private void CreateLabel()
		{
			//标签格式设置

            //字体
			stdole.IFontDisp m_Font; 
			m_Font = new StdFont() as IFontDisp;
			m_Font.Name = "Arial";
			m_Font.Size = 12;

			//颜色
			IRgbColor n_color;
			n_color = new RgbColor() as IRgbColor;
			n_color.Red = 255;
			n_color.Green = 0;
			n_color.Blue = 0;

			if(radioButton1.Checked)
			{   
				IFormattedGridLabel m_FormattedGridLabel;
				m_FormattedGridLabel = new FormattedGridLabelClass();
				m_GridLabel = m_FormattedGridLabel as IGridLabel;
				INumericFormat m_NumericFormat;
				m_NumericFormat = new NumericFormatClass();
				m_NumericFormat.AlignmentOption = esriNumericAlignmentEnum.esriAlignRight;
				m_NumericFormat.RoundingOption = esriRoundingOptionEnum.esriRoundNumberOfDecimals;
				m_NumericFormat.RoundingValue = 0;
				m_NumericFormat.ShowPlusSign = false;
				m_NumericFormat.UseSeparator = false ;
                m_NumericFormat.AlignmentWidth = axPageLayoutControl.Extent.XMax.ToString("0").Length;
				m_NumericFormat.ZeroPad = false ;
                
				m_FormattedGridLabel = m_NumericFormat as IFormattedGridLabel;
                
				
				m_GridLabel.Font = m_Font;
				m_MapGrid.LabelFormat = m_GridLabel;
			}
			if(radioButton2.Checked)
			{
				IDMSGridLabel  m_DMSGridLabel;
                m_DMSGridLabel = new DMSGridLabelClass();
				
                m_DMSGridLabel.LabelType = esriDMSGridLabelType.esriDMSGridLabelStandard;
				m_DMSGridLabel.ShowZeroMinutes = true;
                m_DMSGridLabel.ShowZeroSeconds = true;
				ILatLonFormat m_LatLonFormat = new LatLonFormatClass();
				m_LatLonFormat.ShowDirections = true;
                m_DMSGridLabel.LatLonFormat = m_LatLonFormat;
                m_DMSGridLabel.MinutesFont = m_Font;
                m_DMSGridLabel.SecondsFont = m_Font;
                m_DMSGridLabel.MinutesColor = n_color;
				m_DMSGridLabel.SecondsColor = n_color;

				m_GridLabel = m_DMSGridLabel as IGridLabel;
				m_MapGrid.LabelFormat = m_GridLabel;

			}
			
			m_GridLabel.Color = n_color;
			m_GridLabel.LabelOffset = 2;	
			//m_MapGrid.LabelFormat = m_GridLabel;
		

			m_MapGrids.AddMapGrid(m_MapGrid);
			m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
		}
        /// <summary>
        /// 函数——创建tickmark
        /// </summary>
		private void CreateTickMark()
		{
			//IStyleGallery  m_StyleGallery = new StyleGalleryClass();
			//IEnumStyleGalleryItem m_EnumStyleGalleryItem = new EnumStyleGalleryItemClass();
			//IStyleGalleryItem m_StyleGalleryItem;
			
			//m_StyleGalleryItem =  m_StyleGallery.get_Items("Marker Symbols","ESRI.style","") as IStyleGalleryItem;
            //m_EnumStyleGalleryItem.Reset();
			//m_StyleGalleryItem =  m_EnumStyleGalleryItem.Next();
           // m_MarkerSymbol = m_StyleGalleryItem.Item as IMarkerSymbol;
			IRgbColor m_color;
			m_color = new RgbColor() as IRgbColor;
			m_color.Red = 0;
			m_color.Green = 255;
			m_color.Blue = 0;
            m_MarkerSymbol.Color = m_color;
			m_MarkerSymbol.Size = 6;
            m_MarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCross;
			m_MapGrid.TickMarkSymbol = m_MarkerSymbol;
			m_MapGrids.AddMapGrid(m_MapGrid);
			m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
		}

		
        /// <summary>
        /// 函数——选择颜色对话框
        /// </summary>
        /// <returns></returns>
		private IColor CreateColor()
		{
			tagRECT m_Rect;
			m_Rect = new tagRECT();
			IRgbColor m_CurrColor;
			IColor new_Color = null;
			m_CurrColor = new RgbColor();
			m_CurrColor.Red = 255;
			m_CurrColor.Blue = 255;
			m_CurrColor.Green = 255;
			//创建色板对象
			ESRI.ArcGIS.Framework.IColorPalette m_ColorPalette;
			m_ColorPalette = new ColorPalette();
			if (!m_ColorPalette.TrackPopupMenu(ref m_Rect, m_CurrColor,false,0))
			{
				MessageBox.Show("取消");
			}
			else
			{
				new_Color = m_ColorPalette.Color;
			}
			return new_Color;
		}
        
        /// <summary>
        /// 单选框——设置标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void radioButton5_CheckedChanged(object sender, System.EventArgs e)
		{
			
			if(radioButton5.Checked)
			{
				this.checkBox4.Enabled = true;
                this.textBox6.Enabled = true;
			}
			else
			{
				this.checkBox4.Enabled = false;
				this.textBox6.Enabled = false;
			}
		}
        /// <summary>
        /// 单选框——设置tick点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void radioButton4_CheckedChanged(object sender, System.EventArgs e)
		{
			
		   if(radioButton4.Checked)
		   {
			   this.checkBox4.Enabled = true;
			   this.textBox6.Enabled = true;
		   }
			else
		   {
			   this.checkBox4.Enabled = false;
			   this.textBox6.Enabled = false;
		   }
		}
        /// <summary>
        /// 单选框——设置格网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void radioButton6_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radioButton6.Checked)
			{
				this.textBox5.Enabled = true;
				this.checkBox4.Enabled = true;
				this.checkBox9.Enabled = true;
				this.comboBox1.Enabled = true;
			}
			else
			{
				this.textBox5.Enabled = false;
				this.checkBox4.Enabled = false;
				this.checkBox9.Enabled = false;
				this.comboBox1.Enabled = false;
			}
		}
        /// <summary>
        /// 单选框——设置边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void radioButton3_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radioButton3.Checked)
			{
				this.textBox5.Enabled = true;
				this.checkBox4.Enabled = true;
				this.checkBox9.Enabled = true;
				this.comboBox1.Enabled = true;
			}
			else
			{
				this.textBox5.Enabled = false;
				this.checkBox4.Enabled = false;
				this.checkBox9.Enabled = false;
				this.comboBox1.Enabled = false;
			}
		}	
        /// <summary>
        /// 输入框——设置边框线宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void textBox5_TextChanged(object sender, System.EventArgs e)
		{
			if(radioButton3.Checked)
			{
				m_BorderSimpleLineSymbol.Width = Convert.ToDouble(textBox5.Text);
			}
			if(radioButton6.Checked)
			{
				m_SimpleLineSymbol.Width = Convert.ToDouble(textBox5.Text);
			}
			
			m_MapGrids.AddMapGrid(m_MapGrid);
			this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
		}
        /// <summary>
        /// 输入框——设置tick点或者标签文字大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void textBox6_TextChanged(object sender, System.EventArgs e)
		{
			//设置标签文字大小
			if(radioButton5.Checked)
			{
				m_MapGrid.LabelFormat.Font.Size = Convert.ToDecimal(textBox6.Text);
			}
			//设置tick点
			if(radioButton4.Checked)
			{
				m_MarkerSymbol.Size = Convert.ToDouble(textBox6.Text);
			}
			m_MapGrids.AddMapGrid(m_MapGrid);
			this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
		}
        /// <summary>
        /// 按钮——取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button1_Click(object sender, System.EventArgs e)
		{
		    m_MapGrids.ClearMapGrids();
			this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
			MapGrid.ActiveForm.Hide();
		}
        /// <summary>
        /// 改变公里格网间隔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
		    m_MeasuredGrid.XIntervalSize = Convert.ToDouble(textBox1.Text);
			this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
		}

		private void textBox2_TextChanged(object sender, System.EventArgs e)
		{
			m_MeasuredGrid.YIntervalSize = Convert.ToDouble(textBox2.Text);
			this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);
		}
        /// <summary>
        /// 改变经纬网间隔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void textBox3_TextChanged(object sender, System.EventArgs e)
		{
		    m_MeasuredGrid.XIntervalSize = Convert.ToDouble(textBox3.Text);
			this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);

		}

		private void textBox4_TextChanged(object sender, System.EventArgs e)
		{
			m_MeasuredGrid.YIntervalSize = Convert.ToDouble(textBox3.Text);
			this.m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground,null,null);

		}

		

	}
}
