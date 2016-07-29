using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using Print.Function;
using Print.Mapprint;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;

namespace Print.PageLayoutControl
{
	/// <summary>
	/// frmPageLayoutControl 的摘要说明。
	/// </summary>
	public class frmPageLayoutControl : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.ImageList PageimageList11;
		private System.Windows.Forms.ImageList PageimageList0;
		private System.Windows.Forms.ToolBar PagetoolBar;
		private System.Windows.Forms.MenuItem menuItem6;
		public ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl;
		//常用功能变量
        public MapFunction mapFuntion;
		public AxMapControl axMapControl;
		public IMapDocument m_MapDocument;
		//菜单调用的制图工具
		private Mapprint.MapGrid mapGridForm;
		private Mapprint.MapSurround mapSurroundForm;
		private Mapprint.PageSet pageSetForm;
		private System.Windows.Forms.ImageList imageList6;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.ToolBarButton toolBarButton4;
		private System.Windows.Forms.ToolBarButton toolBarButton5;
		private System.Windows.Forms.ToolBarButton toolBarButton6;
		private System.Windows.Forms.ToolBarButton toolBarButton7;
		private System.Windows.Forms.ToolBarButton toolBarButton8;
		private System.Windows.Forms.ToolBarButton toolBarButton9;
		private System.Windows.Forms.ToolBarButton toolBarButton10;
		private System.Windows.Forms.ToolBarButton toolBarButton11;
		private System.Windows.Forms.ToolBarButton toolBarButton12;
		private System.Windows.Forms.ToolBarButton toolBarButton13;
		private System.Windows.Forms.ToolBarButton toolBarButton14;
		private System.Windows.Forms.ToolBarButton toolBarButton15;
		private System.Windows.Forms.ToolBarButton toolBarButton16;
        private System.Windows.Forms.ToolBarButton toolBarButton17;
        private AxLicenseControl axLicenseControl1;
		
		private System.ComponentModel.IContainer components;

		public frmPageLayoutControl()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
            //this.axPageLayoutControl = axpage;
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPageLayoutControl));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.PageimageList11 = new System.Windows.Forms.ImageList();
            this.PageimageList0 = new System.Windows.Forms.ImageList();
            this.PagetoolBar = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton11 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton12 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton13 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton14 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton15 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton16 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton17 = new System.Windows.Forms.ToolBarButton();
            this.imageList6 = new System.Windows.Forms.ImageList();
            this.axPageLayoutControl = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem6});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "地理格网";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "地图整饰";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "页面设置及打印";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = -1;
            this.menuItem4.Text = "";
            // 
            // PageimageList11
            // 
            this.PageimageList11.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("PageimageList11.ImageStream")));
            this.PageimageList11.TransparentColor = System.Drawing.Color.Transparent;
            this.PageimageList11.Images.SetKeyName(0, "");
            this.PageimageList11.Images.SetKeyName(1, "");
            this.PageimageList11.Images.SetKeyName(2, "");
            this.PageimageList11.Images.SetKeyName(3, "");
            this.PageimageList11.Images.SetKeyName(4, "");
            this.PageimageList11.Images.SetKeyName(5, "");
            this.PageimageList11.Images.SetKeyName(6, "");
            this.PageimageList11.Images.SetKeyName(7, "");
            this.PageimageList11.Images.SetKeyName(8, "");
            this.PageimageList11.Images.SetKeyName(9, "");
            this.PageimageList11.Images.SetKeyName(10, "");
            this.PageimageList11.Images.SetKeyName(11, "");
            this.PageimageList11.Images.SetKeyName(12, "");
            this.PageimageList11.Images.SetKeyName(13, "");
            this.PageimageList11.Images.SetKeyName(14, "");
            this.PageimageList11.Images.SetKeyName(15, "");
            this.PageimageList11.Images.SetKeyName(16, "");
            this.PageimageList11.Images.SetKeyName(17, "");
            this.PageimageList11.Images.SetKeyName(18, "");
            // 
            // PageimageList0
            // 
            this.PageimageList0.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("PageimageList0.ImageStream")));
            this.PageimageList0.TransparentColor = System.Drawing.Color.Transparent;
            this.PageimageList0.Images.SetKeyName(0, "");
            this.PageimageList0.Images.SetKeyName(1, "");
            this.PageimageList0.Images.SetKeyName(2, "");
            this.PageimageList0.Images.SetKeyName(3, "");
            this.PageimageList0.Images.SetKeyName(4, "");
            this.PageimageList0.Images.SetKeyName(5, "");
            this.PageimageList0.Images.SetKeyName(6, "");
            this.PageimageList0.Images.SetKeyName(7, "");
            this.PageimageList0.Images.SetKeyName(8, "");
            this.PageimageList0.Images.SetKeyName(9, "");
            this.PageimageList0.Images.SetKeyName(10, "");
            this.PageimageList0.Images.SetKeyName(11, "");
            // 
            // PagetoolBar
            // 
            this.PagetoolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.toolBarButton2,
            this.toolBarButton3,
            this.toolBarButton4,
            this.toolBarButton5,
            this.toolBarButton6,
            this.toolBarButton7,
            this.toolBarButton8,
            this.toolBarButton9,
            this.toolBarButton10,
            this.toolBarButton11,
            this.toolBarButton12,
            this.toolBarButton13,
            this.toolBarButton14,
            this.toolBarButton15,
            this.toolBarButton16,
            this.toolBarButton17});
            this.PagetoolBar.DropDownArrows = true;
            this.PagetoolBar.ImageList = this.imageList6;
            this.PagetoolBar.Location = new System.Drawing.Point(0, 0);
            this.PagetoolBar.Name = "PagetoolBar";
            this.PagetoolBar.ShowToolTips = true;
            this.PagetoolBar.Size = new System.Drawing.Size(588, 82);
            this.PagetoolBar.TabIndex = 12;
            this.PagetoolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.PagetoolBar_ButtonClick);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 0;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Tag = "0";
            this.toolBarButton1.ToolTipText = "平移";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.ImageIndex = 1;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Tag = "1";
            this.toolBarButton2.ToolTipText = "放大";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.ImageIndex = 2;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Tag = "2";
            this.toolBarButton3.ToolTipText = "缩小";
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.ImageIndex = 3;
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Tag = "3";
            this.toolBarButton4.ToolTipText = "固定比例放大";
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.ImageIndex = 4;
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Tag = "4";
            this.toolBarButton5.ToolTipText = "固定比例缩小";
            // 
            // toolBarButton6
            // 
            this.toolBarButton6.ImageIndex = 5;
            this.toolBarButton6.Name = "toolBarButton6";
            this.toolBarButton6.Tag = "5";
            this.toolBarButton6.ToolTipText = "缩放到1:1";
            // 
            // toolBarButton7
            // 
            this.toolBarButton7.ImageIndex = 6;
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.Tag = "6";
            this.toolBarButton7.ToolTipText = "缩放到页面宽度";
            // 
            // toolBarButton8
            // 
            this.toolBarButton8.ImageIndex = 7;
            this.toolBarButton8.Name = "toolBarButton8";
            this.toolBarButton8.Tag = "7";
            this.toolBarButton8.ToolTipText = "缩放到全图";
            // 
            // toolBarButton9
            // 
            this.toolBarButton9.ImageIndex = 8;
            this.toolBarButton9.Name = "toolBarButton9";
            this.toolBarButton9.Tag = "8";
            this.toolBarButton9.ToolTipText = "查看上一页";
            // 
            // toolBarButton10
            // 
            this.toolBarButton10.ImageIndex = 9;
            this.toolBarButton10.Name = "toolBarButton10";
            this.toolBarButton10.Tag = "9";
            this.toolBarButton10.ToolTipText = "查看后一页";
            // 
            // toolBarButton11
            // 
            this.toolBarButton11.ImageIndex = 10;
            this.toolBarButton11.Name = "toolBarButton11";
            this.toolBarButton11.Tag = "10";
            this.toolBarButton11.ToolTipText = "调整页面大小";
            // 
            // toolBarButton12
            // 
            this.toolBarButton12.ImageIndex = 11;
            this.toolBarButton12.Name = "toolBarButton12";
            this.toolBarButton12.Tag = "11";
            this.toolBarButton12.ToolTipText = "画图框";
            // 
            // toolBarButton13
            // 
            this.toolBarButton13.ImageIndex = 12;
            this.toolBarButton13.Name = "toolBarButton13";
            this.toolBarButton13.Tag = "12";
            this.toolBarButton13.ToolTipText = "画矩形";
            // 
            // toolBarButton14
            // 
            this.toolBarButton14.ImageIndex = 13;
            this.toolBarButton14.Name = "toolBarButton14";
            this.toolBarButton14.Tag = "13";
            this.toolBarButton14.ToolTipText = "画椭圆";
            // 
            // toolBarButton15
            // 
            this.toolBarButton15.ImageIndex = 14;
            this.toolBarButton15.Name = "toolBarButton15";
            this.toolBarButton15.Tag = "14";
            this.toolBarButton15.ToolTipText = "画圆";
            // 
            // toolBarButton16
            // 
            this.toolBarButton16.ImageIndex = 15;
            this.toolBarButton16.Name = "toolBarButton16";
            this.toolBarButton16.Tag = "15";
            this.toolBarButton16.ToolTipText = "画曲线";
            // 
            // toolBarButton17
            // 
            this.toolBarButton17.ImageIndex = 16;
            this.toolBarButton17.Name = "toolBarButton17";
            this.toolBarButton17.Tag = "16";
            this.toolBarButton17.ToolTipText = "画任意曲线";
            // 
            // imageList6
            // 
            this.imageList6.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList6.ImageStream")));
            this.imageList6.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList6.Images.SetKeyName(0, "");
            this.imageList6.Images.SetKeyName(1, "");
            this.imageList6.Images.SetKeyName(2, "");
            this.imageList6.Images.SetKeyName(3, "");
            this.imageList6.Images.SetKeyName(4, "");
            this.imageList6.Images.SetKeyName(5, "");
            this.imageList6.Images.SetKeyName(6, "");
            this.imageList6.Images.SetKeyName(7, "");
            this.imageList6.Images.SetKeyName(8, "");
            this.imageList6.Images.SetKeyName(9, "");
            this.imageList6.Images.SetKeyName(10, "");
            this.imageList6.Images.SetKeyName(11, "");
            this.imageList6.Images.SetKeyName(12, "");
            this.imageList6.Images.SetKeyName(13, "");
            this.imageList6.Images.SetKeyName(14, "");
            this.imageList6.Images.SetKeyName(15, "");
            this.imageList6.Images.SetKeyName(16, "");
            // 
            // axPageLayoutControl
            // 
            this.axPageLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axPageLayoutControl.Location = new System.Drawing.Point(0, 82);
            this.axPageLayoutControl.Name = "axPageLayoutControl";
            this.axPageLayoutControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl.OcxState")));
            this.axPageLayoutControl.Size = new System.Drawing.Size(588, 239);
            this.axPageLayoutControl.TabIndex = 13;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(416, 50);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 14;
            // 
            // frmPageLayoutControl
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(588, 321);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axPageLayoutControl);
            this.Controls.Add(this.PagetoolBar);
            this.Menu = this.mainMenu1;
            this.Name = "frmPageLayoutControl";
            this.Text = "地图打印";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPageLayoutControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region 工具栏按钮单击事件

		private void PagetoolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			string strOftag = e.Button.Tag.ToString() ;
			int tag = Convert.ToInt16(strOftag); 

			switch(tag)
			{
				case 0:
					this.mapFuntion.pageZoomPagePan();
					break;
				case 1:
					this.mapFuntion.pageZoomIn();
					break;
				case 2:
					this.mapFuntion.pageZoomOut();
					break;
				case 3:
					this.mapFuntion.pageZoomInFixed();
					break;
				case 4:
					this.mapFuntion.pageZoomOutFixed();
					break;
				case 5:
					this.mapFuntion.pageZoomZoom100Percent();
					break;
				case 6:
					this.mapFuntion.pageZoomPageWidth();
					break;
				case 7:
					this.mapFuntion.pageZoomZoomWholePage();
					break;
				case 8:
					this.mapFuntion.pageZoomPageToLastExtentBack();
					break;
				case 9:
					this.mapFuntion.pageZoomPageToLastExtentForward();
					break;
				case 10:
					this.mapFuntion.pageSelect();
					break;
				case 11:
					this.mapFuntion.pageNewFrame();
					break;
				case 12:
					this.mapFuntion.pageNewRectangle();
					break;
				case 13:
					this.mapFuntion.pageNewEllipse();
					break;
				case 14:
					this.mapFuntion.pageNewCircle();
					break;
				case 15:
					this.mapFuntion.pageNewCurve();
					break;
				case 16:
					this.mapFuntion.pageNewFreeHand();
					break;
				default:
					break;
			}
		}

		#endregion

		#region 菜单

		/// <summary>
		/// 菜单——地理格网
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem1_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.mapGridForm== null || mapGridForm.IsDisposed == true)
				{
					mapGridForm = new MapGrid(this.axPageLayoutControl); 
					mapGridForm.Owner = this;//设置ProfileGraphDrawForm对象的Owner为Form1

				}
                this.mapGridForm.Left = this.Left + 10;
                this.mapGridForm.Top = this.Top + 160;
				this.mapGridForm.Show();
               
			}
			catch (Exception e1)
			{
				Debug.WriteLine("地图格网出错："+e1.Message );
			}
		
		}

		/// <summary>
		/// 菜单——地图整饰
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem2_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.mapSurroundForm== null || this.mapSurroundForm.IsDisposed == true)
				{
					mapSurroundForm = new MapSurround(this.axPageLayoutControl);
					mapSurroundForm.Owner = this;//设置ProfileGraphDrawForm对象的Owner为Form1

				}
                this.mapSurroundForm.Left = this.Left +10;
                this.mapSurroundForm.Top = this.Top+160;

				this.mapSurroundForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("地图整饰要素出错："+e1.Message );
			}
		
		}


		/// <summary>
		/// 菜单——输出——页面设置及打印
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem6_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.pageSetForm== null || this.pageSetForm.IsDisposed == true)
				{
					pageSetForm =  new PageSet(this.axPageLayoutControl);
					pageSetForm.Owner = this;//设置ProfileGraphDrawForm对象的Owner为Form1

				}
                this.pageSetForm.Left = this.Left  + 10;
                this.pageSetForm.Top = this.Top + 160;

				this.pageSetForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("页面设置出错："+e1.Message );
			}
		
		}


		#endregion

		private void frmPageLayoutControl_Load(object sender, System.EventArgs e)
		{
            this.axPageLayoutControl.LoadMxFile(@"G:\MyProject\海西综合评价系统\数据库\邵武DEM.mxd");
            //mapFuntion = new MapFunction(axMapControl, axPageLayoutControl);
            
           
		}

        

        
	}
}
