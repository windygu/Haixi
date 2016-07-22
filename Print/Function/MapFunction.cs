using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
//using clsDataAccess ;

using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesRasterUI ;

//using ESRI.ArcGIS.;
//using ESRI.ArcGIS.CatalogUI;


using ESRI.ArcGIS.Display;



namespace Print.Function
{
	/// <summary>
	/// MapFunction 的摘要说明。
	/// 公用的地图功能
	/// </summary>
	public class MapFunction
	{
		#region  数据成员

		//地图浏览控件
		public AxMapControl axMapControl;
		//制图控件
		public AxPageLayoutControl axPageLayoutControl;

		#region 地图浏览工具

		// 窗口放大工具
		private ICommand pZoomInCommand=null ; 
		// 窗口缩小工具
		private ICommand pZoomOutCommand =null;  
		//窗口自由缩放工具
		private ICommand pZoomPanCommand =null;  
		// 窗口固定放大工具
		private ICommand pZoomInFixedCommand = null;
		// 窗口固定缩小工具
		private ICommand pZoomOutFixedCommand = null;
		// 窗口全图工具
		private ICommand pFullExtentCommand=null;  
		// 窗口前一视图工具
		private ICommand pToLastExtentBackCommand=null;  
		// 窗口后一视图工具 
		private ICommand pToLastExtentForwardCommand=null;  				
		// 窗口刷新工具
		private ICommand pMapRefreshommand=null;  
		// 窗口漫游工具
		private ICommand pPanCommand  =null;

		#endregion

		#region	制图工具
		//固定放大
		private ICommand pageZoomInFixCommand=null ; 
		//固定缩小
		private ICommand pageZoomOutFixCommand=null; 
		//前一视图	
		private ICommand pageZoomPageToLastExtentBackCommand=null; 
		//后一视图 
		private ICommand pageZoomPageToLastExtentForwardCommand=null; 
		//漫游		  
		private ICommand pageZoomPagePanTool=null; 
		//显示比例	 
		private ICommand pageZoomPageZoomTool=null; 
		//放大		 
		private ICommand pageZoomInTool =null; 
		//缩小		 
		private ICommand pageZoomOutTool=null; 
		//缩放到页面宽度    
		private ICommand pageZoomPageWidthCommand =null; 
		//缩放到比例1：1   
		private ICommand pageZoomZoom100PercentCommand =null; 
		//缩放到整页       
		private ICommand pageZoomZoomWholePageCommand =null; 
		//选择工具          
		private ICommand pageSelectTool  =null; 
		//画圆              
		private ICommand pageNewCircleTool  =null; 
		//画曲线         
		private ICommand pageNewCurveTool  =null; 
		//画椭圆     	
		private ICommand pageNewEllipseTool =null; 
		//画框架  	    
		private ICommand pageNewFrameTool=null; 
		//画任意曲线    
		private ICommand pageNewFreeHandTool =null; 
		//画线          
		private ICommand pageNewLineTool =null; 
		//画多边形      
		private ICommand pageNewPolygonTool =null; 
		//画矩形		
		private ICommand pageNewRectangleTool =null; 





		#endregion

		#region 其他工具

		//恢复上次操作
		private ICommand pRedoCommand  =null;   
		//撤销上次操作
		private ICommand pUndoCommand  =null;   
		//框选选择要素
		private ICommand pSelectFeaturesToolCommand  =null;  
		//按图形选择要素
		private ICommand pSelectByGraphicsCommand =null;  
		//清除选择
		private ICommand pClearSelectionCommand =null;  
		private ICommand pSelectFeaturesCommand =null;  
		
		//打开新地图文档
		private ICommand pOpenDocCommand  =null;  
		//另存为
		private ICommand pSaveAsDocCommand =null;  
		

		// 窗口左旋转文字，图像工具
		private ICommand pRotateLeftCommand=null; 	
		// 窗口右旋转文字，图像工具
		private ICommand pRotateRightCommand=null; 
		// 窗口取消地图旋转工具
		private ICommand pClearMapRotationCommand=null;  

		//空工具
		public ITool pNullTool = (ITool) new  Controls3DAnalystContourTool ();

		#endregion

		//MapControl图层名称
		//String[] lname;

		#endregion

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="axMapControl"></param>
		public MapFunction(AxMapControl axMapControl,AxPageLayoutControl axPageLayoutControl)
		{
			this.axMapControl = axMapControl;
			this.axPageLayoutControl = axPageLayoutControl;
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="axMapControl"></param>
		public MapFunction()
		{			
		}


		#region 地图浏览——视图功能

		/// <summary>
		/// 选择操作，点选和框选，所有可见图层中的都被选中
		/// </summary>
		public void Select()
		{
			try
			{
                pSelectFeaturesCommand = new ControlsSelectFeaturesToolClass() as ICommand ;
				pSelectFeaturesCommand.OnCreate (axMapControl.GetOcx ());
				pSelectFeaturesCommand.OnClick ();
				axMapControl.CurrentTool = pSelectFeaturesCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ZoomIn has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 放大操作
		/// </summary>
		public void ZoomIn()
		{
			try
			{
				pZoomInCommand = new ControlsMapZoomInToolClass();
				pZoomInCommand.OnCreate (axMapControl.GetOcx ());
				pZoomInCommand.OnClick ();
				axMapControl.CurrentTool = pZoomInCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ZoomIn has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 缩小操作
		/// </summary>
		public void ZoomOut()
		{
			try
			{
				pZoomOutCommand = new  ControlsMapZoomOutToolClass();
				pZoomOutCommand.OnCreate (axMapControl.GetOcx ());
				pZoomOutCommand.OnClick ();
				axMapControl.CurrentTool = pZoomOutCommand as ITool ;	
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ZoomOut has errors:"+ee.Message);
			}	
			
		}

		/// <summary>
		/// 自由缩放操作
		/// </summary>
		public void ZoomPan()
		{
			try
			{
				pZoomPanCommand = new ControlsMapZoomPanTool()as ICommand  ;
				pZoomPanCommand.OnCreate (axMapControl.GetOcx ());
				pZoomPanCommand.OnClick ();
				axMapControl.CurrentTool = pZoomPanCommand as ITool ;	
				
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ZoomPan has errors:"+ee.Message);
			}	

		}
		
		/// <summary>
		/// 固定放大操作
		/// </summary>
		public void ZoomInFixed()
		{
			try
			{
				pZoomInFixedCommand =  new ControlsMapZoomInFixedCommandClass ();
				pZoomInFixedCommand.OnCreate(axMapControl.GetOcx());
				pZoomInFixedCommand.OnClick();
				axMapControl.CurrentTool = pZoomInFixedCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ZoomInFixed has errors:"+ee.Message);
			}	
		
		}
		
		/// <summary>
		/// 固定缩小操作
		/// </summary>
		public void ZoomOutFixed()
		{
			try
			{
				pZoomOutFixedCommand =  new ControlsMapZoomOutFixedCommandClass ();
				pZoomOutFixedCommand.OnCreate(axMapControl.GetOcx());
				pZoomOutFixedCommand.OnClick();
				axMapControl.CurrentTool = pZoomOutFixedCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ZoomOutFixed has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 全图操作
		/// </summary>
		public void FullExtent()
		{
			try
			{
				pFullExtentCommand =  new ControlsMapFullExtentCommandClass();
				pFullExtentCommand.OnCreate(axMapControl.GetOcx());
				pFullExtentCommand.OnClick();
				axMapControl.CurrentTool = pFullExtentCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("FullExtent has errors:"+ee.Message);
			}	
		}
		
		/// <summary>
		/// 前一视图操作
		/// </summary>
		public void BringToFront()
		{
			try
			{
				pToLastExtentBackCommand=  new ControlsMapZoomToLastExtentBackCommandClass ();
				pToLastExtentBackCommand.OnCreate(axMapControl.GetOcx());
				pToLastExtentBackCommand.OnClick();
				axMapControl.CurrentTool = pToLastExtentBackCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("BringToFront has errors:"+ee.Message);
			}	
		}
		

		/// <summary>
		/// 后一视图操作
		/// </summary>
		public void BringForward()
		{
			try
			{
				pToLastExtentForwardCommand =  new ControlsMapZoomToLastExtentForwardCommandClass();
				pToLastExtentForwardCommand.OnCreate(axMapControl.GetOcx());
				pToLastExtentForwardCommand.OnClick();
				axMapControl.CurrentTool = pToLastExtentForwardCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("BringForward has errors:"+ee.Message);
			}
		}
		
		/// <summary>
		/// 刷新操作
		/// </summary>
		public void Refresh()
		{
			try
			{
				pMapRefreshommand =  new ControlsMapZoomToLastExtentForwardCommandClass();
				pMapRefreshommand.OnCreate(axMapControl.GetOcx());
				pMapRefreshommand.OnClick();
				axMapControl.CurrentTool = pMapRefreshommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("Refresh has errors:"+ee.Message);
			}
		}

		/// <summary>
		/// 漫游操作
		/// </summary>
		public void Pan()
		{
			try
			{
				pPanCommand = new ControlsMapPanToolClass();
				pPanCommand.OnCreate (axMapControl.GetOcx ());
				pPanCommand.OnClick ();
				axMapControl.CurrentTool = pPanCommand as ITool ;	
//				int i=axMapControl.CurrentTool.Cursor;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("Pan has errors:"+ee.Message);
			}
		}

		#endregion

		#region 制图——视图功能

		#region 同地图浏览

		/// <summary>
		/// 放大
		/// </summary>
		public void pageZoomIn()
		{
			try
			{
				pageZoomInTool =  new ControlsPageZoomInTool();
				pageZoomInTool.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomInTool.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomInTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomInTool has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 缩小
		/// </summary>
		public void pageZoomOut()
		{
			try
			{
				pageZoomOutTool =  new ControlsPageZoomOutTool ();
				pageZoomOutTool.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomOutTool.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomOutTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomOutTool has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 固定放大操作
		/// </summary>
		public void pageZoomInFixed()
		{
			try
			{
				pageZoomInFixCommand =  new ControlsPageZoomInFixedCommand();
				pageZoomInFixCommand.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomInFixCommand.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomInFixCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomInFixCommand has errors:"+ee.Message);
			}	
		
		}
		
		/// <summary>
		/// 固定缩小操作
		/// </summary>
		public void pageZoomOutFixed()
		{
			try
			{
				pageZoomOutFixCommand =  new ControlsPageZoomOutFixedCommand();
				pageZoomOutFixCommand.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomOutFixCommand.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomOutFixCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomOutFixCommand has errors:"+ee.Message);
			}	
		}
        
		/// <summary>
		/// 前一视图
		/// </summary>
		public void pageZoomPageToLastExtentForward()
		{
			try
			{
				pageZoomPageToLastExtentForwardCommand =  new ControlsPageZoomPageToLastExtentForwardCommand();
				pageZoomPageToLastExtentForwardCommand.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomPageToLastExtentForwardCommand.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomPageToLastExtentForwardCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomPageToLastExtentForwardCommand has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 后一视图
		/// </summary>
		public void pageZoomPageToLastExtentBack()
		{
			try
			{
				pageZoomPageToLastExtentBackCommand =  new ControlsPageZoomPageToLastExtentBackCommand ();
				pageZoomPageToLastExtentBackCommand.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomPageToLastExtentBackCommand.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomPageToLastExtentBackCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomPageToLastExtentBackCommand has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 漫游
		/// </summary>
		public void pageZoomPagePan()
		{
			try
			{
				pageZoomPagePanTool =  new ControlsPagePanTool();
				pageZoomPagePanTool.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomPagePanTool.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomPagePanTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomPagePanTool has errors:"+ee.Message);
			}	
		}

		#endregion

		#region 缩放

		/// <summary>
		/// 显示比例
		/// </summary>
		public void pageZoomPageZoom()
		{
			try
			{
				pageZoomPageZoomTool =  new ControlsPageZoomToolControl ();
				pageZoomPageZoomTool.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomPageZoomTool.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomPageZoomTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomPageZoom has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 缩放到页面宽度
		/// </summary>
		public void pageZoomPageWidth()
		{
			try
			{
				pageZoomPageWidthCommand =  new ControlsPageZoomPageWidthCommand();
				pageZoomPageWidthCommand.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomPageWidthCommand.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomPageWidthCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomPageWidthCommand has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 缩放到比例1：1
		/// </summary>
		public void pageZoomZoom100Percent()
		{
			try
			{
				pageZoomZoom100PercentCommand =  new ControlsPageZoom100PercentCommand();
				pageZoomZoom100PercentCommand.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomZoom100PercentCommand.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomZoom100PercentCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomZoom100PercentCommand has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 缩放到整页
		/// </summary>
		public void pageZoomZoomWholePage()
		{
			try
			{
				pageZoomZoomWholePageCommand =  new ControlsPageZoomWholePageCommand ();
				pageZoomZoomWholePageCommand.OnCreate(axPageLayoutControl.GetOcx());
				pageZoomZoomWholePageCommand.OnClick();
				axPageLayoutControl.CurrentTool = pageZoomZoomWholePageCommand as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageZoomZoomWholePage has errors:"+ee.Message);
			}	
		}

		#endregion

		#region 选择

		/// <summary>
		/// 选择工具
		/// </summary>
		public void pageSelect()
		{
			try
			{
				pageSelectTool =  new ControlsSelectTool();
				pageSelectTool.OnCreate(axPageLayoutControl.GetOcx());
				pageSelectTool.OnClick();
				axPageLayoutControl.CurrentTool = pageSelectTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ControlsSelectTool  has errors:"+ee.Message);
			}	
		}

		#endregion

		#region 画各种形状

		/// <summary>
		/// 画圆
		/// </summary>
		public void pageNewCircle()
		{
			try
			{
				pageNewCircleTool =  new ControlsNewCircleTool();
				pageNewCircleTool.OnCreate(axPageLayoutControl.GetOcx());
				pageNewCircleTool.OnClick();
				axPageLayoutControl.CurrentTool = pageNewCircleTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageNewCircleTool has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 画曲线
		/// </summary>
		public void pageNewCurve()
		{
			try
			{
				pageNewCurveTool =  new ControlsNewCurveTool();
				pageNewCurveTool.OnCreate(axPageLayoutControl.GetOcx());
				pageNewCurveTool.OnClick();
				axPageLayoutControl.CurrentTool = pageNewCurveTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageNewCurveTool has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 画椭圆
		/// </summary>
		public void pageNewEllipse()
		{
			try
			{
				pageNewEllipseTool =  new ControlsNewEllipseTool();
				pageNewEllipseTool.OnCreate(axPageLayoutControl.GetOcx());
				pageNewEllipseTool.OnClick();
				axPageLayoutControl.CurrentTool = pageNewEllipseTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageNewEllipseTool has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 画框架
		/// </summary>
		public void pageNewFrame()
		{
			try
			{
				pageNewFrameTool =  new ControlsNewFrameTool();
				pageNewFrameTool.OnCreate(axPageLayoutControl.GetOcx());
				pageNewFrameTool.OnClick();
				axPageLayoutControl.CurrentTool = pageNewFrameTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageNewFrameTool  has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 画任意曲线
		/// </summary>
		public void pageNewFreeHand()
		{
			try
			{
				pageNewFreeHandTool =  new ControlsNewFreeHandTool();
				pageNewFreeHandTool.OnCreate(axPageLayoutControl.GetOcx());
				pageNewFreeHandTool.OnClick();
				axPageLayoutControl.CurrentTool = pageNewFreeHandTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageNewFreeHandTool has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 画线
		/// </summary>
		public void pageNewLine()
		{
			try
			{
				pageNewLineTool =  new ControlsNewLineTool();
				pageNewLineTool.OnCreate(axPageLayoutControl.GetOcx());
				pageNewLineTool.OnClick();
				axPageLayoutControl.CurrentTool = pageNewLineTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageNewLineTool has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 画多边形
		/// </summary>
		public void pageNewPolygon()
		{
			try
			{
				pageNewPolygonTool =  new ControlsNewPolygonTool ();
				pageNewPolygonTool.OnCreate(axPageLayoutControl.GetOcx());
				pageNewPolygonTool.OnClick();
				axPageLayoutControl.CurrentTool = pageNewPolygonTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("pageNewPolygonTool has errors:"+ee.Message);
			}	
		}

		/// <summary>
		/// 画矩形
		/// </summary>
		public void pageNewRectangle()
		{
			try
			{
				pageNewRectangleTool =  new ControlsNewRectangleTool ();
				pageNewRectangleTool.OnCreate(axPageLayoutControl.GetOcx());
				pageNewRectangleTool.OnClick();
				axPageLayoutControl.CurrentTool = pageNewRectangleTool as ITool ;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ControlsNewRectangleTool  has errors:"+ee.Message);
			}	
		}

		#endregion

		#endregion

		#region 选择要素等功能

		/// <summary>
		///	框选，选择要素操作
		/// </summary>
		public void SelectFeaturesTool()
		{
			try
			{
				pSelectFeaturesToolCommand = new ControlsSelectFeaturesToolClass();
				pSelectFeaturesToolCommand.OnCreate (axMapControl.GetOcx ());
				pSelectFeaturesToolCommand.OnClick ();
				axMapControl.CurrentTool = pSelectFeaturesToolCommand as ITool ;				
			}
			catch(Exception ee)
			{
				Debug.WriteLine("SelectFeaturesTool has errors:"+ee.Message);
			}
		}
        
		
		/// <summary>
		///	清除要素选择操作
		/// </summary>
		public void ClearSelectionCommand()
		{
			try
			{
				pClearSelectionCommand = new ControlsClearSelectionCommandClass ();
				pClearSelectionCommand.OnCreate (axMapControl.GetOcx ());
				pClearSelectionCommand.OnClick ();
				axMapControl.CurrentTool = pClearSelectionCommand as ITool ;	
			}
			catch(Exception ee)
			{
				Debug.WriteLine("ClearSelectionCommand has errors:"+ee.Message);
			}
			
		}
		
		
		/// <summary>
		///	恢复上次操作操作
		/// </summary>
		public void Redo()
		{
			pRedoCommand = new ControlsRedoCommandClass();
			pRedoCommand.OnCreate (axMapControl.GetOcx ());
			pRedoCommand.OnClick ();
			axMapControl.CurrentTool = pRedoCommand as ITool ;
		}
		
		/// <summary>
		///	撤销上次操作操作
		/// </summary>
		public void Undo()
		{
			pUndoCommand = new ControlsUndoCommandClass();
			pUndoCommand.OnCreate (axMapControl.GetOcx ());
			pUndoCommand.OnClick ();
			axMapControl.CurrentTool = pUndoCommand as ITool ;
		}

		/// <summary>
		/// /按图形选择要素
		/// </summary>
		public void SelectByGraphicsCommand()
		{
			try
			{
				pSelectByGraphicsCommand  = new ControlsSelectByGraphicsCommandClass ();
				pSelectByGraphicsCommand.OnCreate (axMapControl.GetOcx ());
				pSelectByGraphicsCommand.OnClick ();
				axMapControl.CurrentTool = pSelectByGraphicsCommand as ITool ;
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}
		/// <summary>
		///	打开地图文档操作
		/// </summary>
		public void OpenDoc()
		{
			pOpenDocCommand = new ControlsOpenDocCommandClass();
			pOpenDocCommand.OnCreate (axMapControl.GetOcx ());
			pOpenDocCommand.OnClick ();
			axMapControl.CurrentTool = pOpenDocCommand as ITool ;
			axPageLayoutControl.CurrentTool = pOpenDocCommand as ITool;
		}
		
		/// <summary>
		///	另存地图文档操作
		/// </summary>
		public void SaveAsDoc()
		{
			pSaveAsDocCommand = new ControlsSaveAsDocCommandClass();
			pSaveAsDocCommand.OnCreate (axMapControl.GetOcx ());
			pSaveAsDocCommand.OnClick ();
			axMapControl.CurrentTool = pSaveAsDocCommand as ITool ;
		}

		/// <summary>
		/// 左旋转
		/// </summary>
		public void pRotateLeft()
		{
			try
			{
				pRotateLeftCommand =  new ControlsRotateLeftCommandClass();
				pRotateLeftCommand.OnCreate(axMapControl.GetOcx());
				pRotateLeftCommand.OnClick();
				axMapControl.CurrentTool = pRotateLeftCommand as ITool ;
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}
		
		/// <summary>
		/// 右旋转
		/// </summary>
		public void pRotateRight()
		{
			try
			{
				pRotateRightCommand =  new ControlsRotateRightCommandClass ();
				pRotateRightCommand.OnCreate(axMapControl.GetOcx());
				pRotateRightCommand.OnClick();
				axMapControl.CurrentTool = pRotateRightCommand as ITool ;
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}
		
		/// <summary>
		/// 取消旋转
		/// </summary>
		public void pClearMapRotation()
		{
			try
			{
				pClearMapRotationCommand =  new ControlsMapClearMapRotationCommandClass  ();
				pClearMapRotationCommand.OnCreate(axMapControl.GetOcx());
				pClearMapRotationCommand.OnClick();
				axMapControl.CurrentTool = pClearMapRotationCommand as ITool ;
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}

		#endregion

		#region 地图单位，量测距离等功能

		/// <summary>
		/// 得到当前MapControl载入的地图的单位
		/// </summary>
		/// <param name="mapUnits"></param>
		/// <returns></returns>
		public String getMapUnits(esriUnits mapUnits)
		{
			String sMapUnits =  null;
			try
			{
				switch (mapUnits)
				{
					case esriUnits.esriCentimeters:
						sMapUnits = "Centimeters";
						break;
					case esriUnits.esriDecimalDegrees:
						sMapUnits = "Decimal Degrees";
						break;
					case esriUnits.esriDecimeters:
						sMapUnits = "Decimeters";
						break;
					case esriUnits.esriFeet:
						sMapUnits = "Feet";
						break;
					case esriUnits.esriInches:
						sMapUnits = "Inches";
						break;
					case esriUnits.esriKilometers:
						sMapUnits = "Kilometers";
						break;
					case esriUnits.esriMeters:
						sMapUnits = "Meters";
						break;
					case esriUnits.esriMiles:
						sMapUnits = "Miles";
						break;
					case esriUnits.esriMillimeters:
						sMapUnits = "Millimeters";
						break;
					case esriUnits.esriNauticalMiles:
						sMapUnits = "NauticalMiles";
						break;
					case esriUnits.esriPoints:
						sMapUnits = "Points";
						break;
					case esriUnits.esriUnknownUnits:
						sMapUnits = "Unknown";
						break;
					case esriUnits.esriYards:
						sMapUnits = "Yards";
						break;
				}
			}
			catch(Exception ee)
			{
				Debug.WriteLine("getMapUnits has errors:"+ee.Message);
			}
			return sMapUnits;

		}
		
		/// <summary>
		/// 量测距离
		/// </summary>
		/// <returns></returns>
		public String MeasureLength(IGeometry trackLine)
		{
			double length=0.0;	
		
			try 
			{
				//toolbar.setCurrentToolByRef(mapZoomIn);
				SpatialReferenceEnvironment pSpatRefFact =new SpatialReferenceEnvironment();
				ISpatialReference pProjCoordSystem =
					pSpatRefFact.CreateProjectedCoordinateSystem(21476);
				//esriSRProjCS_Beijing1954_3_Degree_GK_CM_102E
				IPolyline pLine = (IPolyline)(trackLine);
				pLine.Project(pProjCoordSystem);
				length = pLine.Length; 		
			} 
			catch (Exception e) 
			{
				// TODO 自动生成 catch 块
				Debug.WriteLine("MeasureLength has errors:"+e.Message);
			}
			return length.ToString();
		
		}
	
		/// <summary>
		/// 量测面积
		/// </summary>
		/// <returns></returns>
		public String MeasureArea(IGeometry trackPolygon)
		{
			double area=0.0;		
			try 
			{
				//设置投影
				SpatialReferenceEnvironment pSpatRefFact =new SpatialReferenceEnvironment();
				ISpatialReference pProjCoordSystem =
					pSpatRefFact.CreateProjectedCoordinateSystem(21476);

				//获得所画的polygon
				IPolygon iPolygon=(IPolygon)trackPolygon;
				iPolygon.Project(pProjCoordSystem) ;

				//获得polygon的面积
				IArea iArea = (IArea)iPolygon;
				area=iArea.Area;

			} 
			catch (Exception e) 
			{
				// TODO 自动生成 catch 块
				Debug.WriteLine("MeasureArea has errors:"+e.Message);
			}
			return area.ToString() ;
		
		}

		#endregion

		#region 加载栅格影像RasterDataset

		/// <summary>
		/// 连接SDE数据库，打开RasterDataset栅格影像数据集
		/// </summary>
		/// <param name="server"></param>
		/// <param name="instance"></param>
		/// <param name="database"></param>
		/// <param name="user"></param>
		/// <param name="password"></param>
		/// <param name="version"></param>
		/// <param name="rasterDatasetName"></param>
		/// <returns></returns>
		public IRasterDataset OpenArcSDE(string server, string instance,
		                                 string database, string user,
		                                 string password, string version,
		                                 string rasterDatasetName)
		{
			IRasterDataset rasterDataset = null;

			try
			{ //返回的RasterDataset
				
				// Open an ArcSDE raster dataset with the given name
				// server, instance, database, user, password, version are database connection info
				// rasterDatasetName is the name of the raster dataset to be opened //Open the ArcSDE workspace 
				IPropertySet propertySet = new PropertySetClass();
			
				propertySet.SetProperty("server", server);
				propertySet.SetProperty("instance", instance);
				propertySet.SetProperty("database", database);
				propertySet.SetProperty("user", user);
				propertySet.SetProperty("password", password);
				propertySet.SetProperty("version", version);
		
				// cocreate the workspace factory
				IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();
		
				// Open the raster workspace using the previously defined porperty set and 
				//  QI to the desired IRasterWorkspaceEx interface to access the existing dataset
				IRasterWorkspaceEx rasterWorkspaceEx = workspaceFactory.Open(propertySet, 0) as IRasterWorkspaceEx;
		
				//Open the ArcSDE raster dataset
				rasterDataset = rasterWorkspaceEx.OpenRasterDataset(rasterDatasetName);
				
			}
			catch (Exception e)
			{
				Debug.WriteLine("——连接数据库打开RasterDataset出错——："+e.Message );
			}
			return rasterDataset;
		}


	
		/// <summary>
		/// 判断当前图层是否加入了查询到的RasterDataset
		/// </summary>
		/// <param name="nameOfRaster"></param>
		/// <returns></returns>
		public bool haveRasterDataset(string nameOfRaster)
		{
			bool have = false;
			try
			{
				ArrayList arrOflayer=new ArrayList();
				arrOflayer = this.getAllLayers();
				for(int i=0; i<arrOflayer.Count;i++)
				{
					 ILayer iLayer= (ILayer)arrOflayer[i];
					 string nameOfLayer = iLayer.Name;
					 //Debug.WriteLine("nameOfRaster is"+nameOfRaster);
					 //Debug.WriteLine("nameOfLayer is"+nameOfLayer);
					 //如果名字相等
					 if(nameOfRaster == nameOfLayer)
					 {
					 	have = true;
						break;
					 }
				}
				
			}
			catch (Exception e)
			{
				Debug.WriteLine("——MapControl是否有iRasterDataset——："+e.Message );
			}
			return have;

		}

		/// <summary>
		/// 在MapControl里加载SDE数据库里的影像
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <param name="rasterDataset"></param>
		public void  AddRasterLayer(AxMapControl axMapControl, IRasterDataset rasterDataset,int p_intInsertIndex,string p_strLayerName)
		{
			try
			{ // rasterDataset represents a RasterDataset from raster workspace, access workspace or sde workspace.
				// map represents the Map to add the layer to once it is created

				// Create a raster layer. Use CreateFromRaster method when creating from a Raster.
				IRasterLayer rasterLayer = new RasterLayerClass();
				rasterLayer.CreateFromDataset(rasterDataset);
                if (p_strLayerName != null && p_strLayerName.Length > 0)
                {
                    rasterLayer.Name = p_strLayerName;
                }
                //rasterLayer.Name=rasterDataset.

				// Add the raster layer to Map 
				axMapControl.AddLayer(rasterLayer,p_intInsertIndex);

				// QI for availabilty of the IActiveView interface for a screen update
                //IActiveView activeView = axMapControl.ActiveView;

//				if (activeView != null)
//					activeView.Refresh();

                //IEnvelope m_Envelope = new Envelope() as IEnvelope;
//				m_Envelope = rasterLayer.AreaOfInterest;
				//m_Envelope.PutCoords(rasterLayer.RasterXMin,feature.Extent.YMin,feature.Extent.XMax,feature.Extent.YMax);
//				axMapControl.Extent = m_Envelope;
					
			}
			catch (Exception e)
			{
				Debug.WriteLine("——在MapControl里加载RasterDataset出错——："+e.Message );
			}
		}

		#endregion

		#region 加载栅格影像in RasterCatalog


		public IRasterWorkspaceEx OpenIRasterWorkspaceEx(string server, string instance, 
			string database, string user,string password, string version)
		{

			// Open an ArcSDE raster Catalog with the given name
			// server, instance, database, user, password, version are database connection info
			// rasterCatalogName is the name of the raster Catalog

			//Open the ArcSDE workspace
			IPropertySet propertySet = new PropertySetClass();
	
			propertySet.SetProperty("server", server);
			propertySet.SetProperty("instance", instance);
			propertySet.SetProperty("database", database);
			propertySet.SetProperty("user", user);
			propertySet.SetProperty("password", password);
			propertySet.SetProperty("version", version);

			// cocreate the workspace factory
			IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();

			// Open the raster workspace using the previously defined porperty set
			// and QI to the desired IRasterWorkspaceEx interface to access the existing catalog
			IRasterWorkspaceEx rasterWorkspaceEx = workspaceFactory.Open(propertySet, 0) as IRasterWorkspaceEx;		

			return rasterWorkspaceEx;
		}

		/// <summary>
		/// 加载栅格影像RasterCatalog
		/// </summary>
		/// <param name="server"></param>
		/// <param name="instance"></param>
		/// <param name="database"></param>
		/// <param name="user"></param>
		/// <param name="password"></param>
		/// <param name="rasterCatalogName"></param>
		/// <param name="version"></param>
		/// <returns></returns>
		public IRasterCatalog OpenArcSDEinRasterDataset(string server, string instance, 
		                                                string database, string user,
		                                                string password, string rasterCatalogName, 
		                                                string version)
		{

			// Open an ArcSDE raster Catalog with the given name
			// server, instance, database, user, password, version are database connection info
			// rasterCatalogName is the name of the raster Catalog

			//Open the ArcSDE workspace
			IPropertySet propertySet = new PropertySetClass();
	
			propertySet.SetProperty("server", server);
			propertySet.SetProperty("instance", instance);
			propertySet.SetProperty("database", database);
			propertySet.SetProperty("user", user);
			propertySet.SetProperty("password", password);
			propertySet.SetProperty("version", version);

			// cocreate the workspace factory
			IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();

			// Open the raster workspace using the previously defined porperty set
			// and QI to the desired IRasterWorkspaceEx interface to access the existing catalog
			IRasterWorkspaceEx rasterWorkspaceEx = workspaceFactory.Open(propertySet, 0) as IRasterWorkspaceEx;

			//Open the ArcSDE raster Catalog
			IRasterCatalog rasterCatalog = null;
			rasterCatalog = rasterWorkspaceEx.OpenRasterCatalog(rasterCatalogName);

			return rasterCatalog;
		}
		public IRaster openRasterDataset(IRasterWorkspaceEx pRasterWorkspaceEx,string sRasterDatasetname)
		{
			IRasterDataset pRasterDataset=pRasterWorkspaceEx.OpenRasterDataset(sRasterDatasetname);
			IRaster pRaster=pRasterDataset.CreateDefaultRaster();
			return pRaster;
		}
		public IRasterCatalog openRasterCatalog(IRasterWorkspaceEx pRasterWorkspaceEx,string pCatalogname)
		{
			IRasterCatalog openRasterCatalog=pRasterWorkspaceEx.OpenRasterCatalog(pCatalogname);
			return openRasterCatalog;
		}
		public IRasterDataset createRasterDataset(string sDir,string sInput)
		{
			IWorkspaceFactory pRasterWsFact;
			IRasterWorkspace pRasterWs;
			pRasterWsFact=new RasterWorkspaceFactoryClass();
			pRasterWs=(IRasterWorkspace)pRasterWsFact.OpenFromFile(sDir, 0);
			IRasterDataset pRasterDataset=pRasterWs.OpenRasterDataset(sInput);
			return pRasterDataset;
		}
		public IRaster GetRasterFromRasterCatalog(IRasterCatalog p_RasterCatalog,string sRasterName)
		{
				ITable pTable=(ITable)p_RasterCatalog;
				IRaster pRaster=null;
				IRasterValue pValue=new RasterValueClass();
			try
			{
				IRow pRow;
				IQueryFilter pQueryFilter=null;
				pQueryFilter=new QueryFilter() as IQueryFilter ;
				pQueryFilter.WhereClause="Name="+"'"+sRasterName.ToUpper()+"'";
				ICursor pCursor=pTable.Search(pQueryFilter,true);
				pRow=pCursor.NextRow();
				if (pRow==null)
				{
					pQueryFilter.WhereClause="Name="+"'"+sRasterName.ToLower()+"'";
					pCursor=pTable.Search(pQueryFilter,true);
					pRow=pCursor.NextRow();
				}
				if (pRow==null) return null;
				int sRaster = pTable.FindField("Raster");
				pValue=pRow.get_Value(sRaster) as  IRasterValue;
				pRaster=pValue.Raster;
			}
			catch(System.Exception errs)
			{		
				throw errs;
			}
			finally
			{
				pValue=null;				
			}
			return pRaster;
		}

		public IRasterDataset GetRasterDatasetFromRasterCatalog(IRasterCatalog p_RasterCatalog,string sRasterName)
		{
			ITable pTable=(ITable)p_RasterCatalog;
			IRasterDataset  m_IRasterDataset=null;
			IRasterValue pValue=new RasterValueClass();
			try
			{
				IRow pRow;
				IQueryFilter pQueryFilter=null;
				pQueryFilter=new QueryFilter() as IQueryFilter ;
				pQueryFilter.WhereClause="Name="+"'"+sRasterName.ToUpper()+"'";
				ICursor pCursor=pTable.Search(pQueryFilter,true);
				pRow=pCursor.NextRow();
				if (pRow==null)
				{
					pQueryFilter.WhereClause="Name="+"'"+sRasterName.ToLower()+"'";
					pCursor=pTable.Search(pQueryFilter,true);
					pRow=pCursor.NextRow();
				}
				if (pRow==null) return null;
				int sRaster = pTable.FindField("Raster");
				pValue=pRow.get_Value(sRaster) as  IRasterValue;
				m_IRasterDataset=pValue.RasterDataset;
			}
			catch(System.Exception errs)
			{		
				throw errs;
			}
			finally
			{
				pValue=null;				
			}
			return m_IRasterDataset;
		}
		public IRasterDataset RasterCatalogtoRasterDataset(IRasterCatalog p_RasterCatalog,string sRasterName)
		{
			ITable pTable=(ITable)p_RasterCatalog;
			IRasterDataset pRasterDataset=null;
			IRasterValue pValue=new RasterValueClass();
			IRow pRow;
			IQueryFilter pQueryFilter=null;
			pQueryFilter=new QueryFilter() as IQueryFilter ;
			pQueryFilter.WhereClause="Name="+"'"+sRasterName+"'";
			ICursor pCursor=pTable.Search(pQueryFilter,true);
			pRow=pCursor.NextRow();
			int sRaster = pTable.FindField("Raster");
			pValue=pRow.get_Value(sRaster) as IRasterValue;
			pRasterDataset=pValue.RasterDataset;
			return pRasterDataset;
		}
		public IRaster RasterDatasettoRaster(IRasterDataset pRasterDataset)
		{
			IRaster pRaster=pRasterDataset.CreateDefaultRaster();
			return  pRaster;
		}

		#endregion

		#region 上传下载的栅格影像
		public void LoadToRasterColumn(IRasterCatalog p_RasterCatalog,IRasterDataset pRasterDs,string sRasterName)
		{
			ITable pTable=(ITable)p_RasterCatalog;
			IRow pRow;
			int rowcount=pTable.RowCount(null);
			IRasterValue pValue=new RasterValueClass();
			pRow=pTable.CreateRow();
			for(int pCnt=0;pCnt<pRow.Fields.FieldCount;pCnt++)
			{
				string aaq=pRow.Fields.get_Field(pCnt).Name;
				string aa=pRow.Fields.get_Field(pCnt).Type.ToString();
				if(pRow.Fields.get_Field(pCnt).Type==esriFieldType.esriFieldTypeString)
					pRow.set_Value(pCnt,sRasterName);
				if (pRow.Fields.get_Field(pCnt).Type==esriFieldType.esriFieldTypeRaster )
				{
					pValue.RasterDataset=pRasterDs;
					pRow.set_Value(pCnt,pValue);
				}
			}
		}
		public void LoadToSDEAsRasterCatalog(string sServer,string sInstance,string sUser,string sPassword,string sDB,string sDir,string sInput,string sSDECatalog)
		{
			IRasterSdeServerOperation2 pSDEOp;
			IRasterSdeStorage2 pSDEStorage;
			IRasterSdeConnection2 pSDEConn;
			IWorkspaceFactory pRasterWsFact;
			IWorkspace pRasterWs;
			IEnumDataset pEnumDataset;
			IDataset pRasterDs;
			int i=0;
			string sRaster;
			//get all the rasterdataset
			pRasterWsFact=new RasterWorkspaceFactoryClass();
			pRasterWs=pRasterWsFact.OpenFromFile(sDir, 0);
			pEnumDataset =pRasterWs.get_Datasets(esriDatasetType.esriDTRasterDataset);
			//initiallize RasterSDELoader
			RasterSdeLoaderClass rasterSDEstorage=new RasterSdeLoaderClass();
			pSDEConn=rasterSDEstorage;
			//set connection property
			pSDEConn.ServerName = sServer;
			pSDEConn.Instance = sInstance;
			pSDEConn.Database = sDB;
			pSDEConn.UserName = sUser;
			pSDEConn.Password = sPassword;
			//set raster catalogname
			IRasterSdeCatalog pSDECatalog=rasterSDEstorage;
			pSDECatalog.CatalogName=sSDECatalog;
			//overlap all the rasterdataset,the first rasterdat will be putted into SDE as catalog;
			pEnumDataset.Reset();
			pRasterDs=pEnumDataset.Next();
			while(pRasterDs!=null)
			{
				sRaster=pRasterDs.Name;
				string path=sDir+@"\"+sRaster;
				pSDEConn.InputRasterName=path;
				string rasterName=sRaster;
				sRaster=rasterName.Replace('.','_');
				pSDECatalog.RasterName=sRaster;
				//set storage type
				pSDEStorage= rasterSDEstorage;
				//set spatial reference
				if(i==0)
				{
					IGeoDataset pGeoDs=(IGeoDataset)pRasterDs;
					//get spatial reference
					ISpatialReference spref;
					spref=pGeoDs.SpatialReference;
					pSDEStorage.SpatialReference=spref;
				}
				//set Set compression type
				pSDEStorage.CompressionType=esriRasterSdeCompressionTypeEnum.esriRasterSdeCompressionTypeRunLength;
				//set tile size
				pSDEStorage.TileHeight=128;
				pSDEStorage.TileWidth=128;
				//set pyramid option
				pSDEStorage.PyramidOption =esriRasterSdePyramidOptEnum.esriRasterSdePyramidBuildWithFirstLevel;
				pSDEStorage.PyramidResampleType =rstResamplingTypes.RSP_BilinearInterpolation;
				//loading
				pSDEOp=rasterSDEstorage;
				if (i==0)
				{
					pSDEOp.Create();
				}
				else
				{
					pSDEOp.Insert();
				}
				// Calculate stats
				pSDEOp.ComputeStatistics();
				i=1;
				pRasterDs=pEnumDataset.Next();
			}
			//Cleanup
			pSDEConn=null;
			pSDEStorage=null;
			pSDEOp=null;
			pRasterWsFact = null;
			pRasterWs = null;
			//			pGeoDs = null;

		}

		#endregion

		#region 删除所有的栅格影像

		/// <summary>
		/// 移除所有栅格图像
		/// </summary>
		/// <param name="arryOfRasterLayer"></param>
		public void deleteAllRasterLayers(ArrayList arryOfRasterLayer)   
		{
			try
			{
				IMap iMap = this.axMapControl.Map;

				for(int i=0; i<arryOfRasterLayer.Count; i++)
				{
					ILayer iLayer = (ILayer)arryOfRasterLayer[i];
					iMap.DeleteLayer(iLayer) ;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("---删除所有的栅格影像出错----"+e.Message );
			}
		}

		/// <summary>
		/// 关闭所有栅格图像，使所有影像图像visible=false
		/// </summary>
		/// <param name="arryOfRasterLayer"></param>
		public void UnvisibleAllRasterLayers()   
		{
			try
			{
				ILayer m_ILayer;
				for (int i=this.axMapControl.LayerCount -1;i>=0 ;i--)
				{
					m_ILayer=this.axMapControl.get_Layer(i);
					if (m_ILayer is IRasterLayer )
					{
						m_ILayer.Visible=false;
					}
				}

				this.axMapControl.Refresh();
				
			}
			catch (Exception e)
			{
				Debug.WriteLine("---删除所有的栅格影像出错----"+e.Message );
			}
		}


		#endregion

		#region 工作空间相关操作

		public IRasterWorkspaceEx rasterWorkspaceEx(string sServer,string sInstance,string sUser,string sPassword,string sDB)
		{
			IPropertySet propertySet = new PropertySetClass();
			propertySet.SetProperty("server", sServer);
			propertySet.SetProperty("instance", sInstance);
			propertySet.SetProperty("database", sDB);
			propertySet.SetProperty("user", sUser);
			propertySet.SetProperty("password", sPassword);
			propertySet.SetProperty("version", "sde.DEFAULT");
			// cocreate the workspace factory
			IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();
			// Open the raster workspace using the previously defined porperty set
			// and QI to the desired IRasterWorkspaceEx interface to access the existing catalog
			IRasterWorkspaceEx rasterWorkspaceEx = workspaceFactory.Open(propertySet, 0) as IRasterWorkspaceEx;


			return rasterWorkspaceEx;
		}
		#endregion

		#region 删除所有的栅格影像


		/// <summary>
		/// 叠加分析
		/// </summary>
		/// <param name="m_Geometry">叠加分析的范围</param>
		/// <param name="mLayer">参与叠加分析的图层</param>
		/// <returns></returns>
		public ArrayList Overlay(IGeometry m_Geometry,IFeatureLayer mLayer)
		{
			try
			{
				ISpatialFilter m_SpatialFilter= new SpatialFilter() as ISpatialFilter;
				m_SpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
				//				IGeometry m_Geometry = mfeature.Shape;
				m_SpatialFilter.Geometry = m_Geometry;
				IFeatureCursor m_FeatureCursor = null; 
				m_FeatureCursor = mLayer.Search(m_SpatialFilter,true);
				 
				//查找图幅号名称
				IFeature selFeature = m_FeatureCursor.NextFeature();
				ArrayList tfbh = new ArrayList(); 
				int i = 0;
				while(selFeature!= null)
				{
					int index =selFeature.Fields.FindField("TFBH");
					string thevalue = selFeature.get_Value(index).ToString();
					Debug.WriteLine(thevalue);
					// IField filed = selFeature.Fields.get_Field(index);
					tfbh.Add(thevalue);
					selFeature = m_FeatureCursor.NextFeature();
					i++;
				}
				return tfbh;

			}
			catch(System.Exception errs)
			{
				throw errs;	
			}
			
		}
		#endregion		

		#region 获取当前图层

		/// <summary>
		///  获取所有的矢量图层，返回矢量图层数组，不包括raster影像
		/// </summary>
		/// <returns></returns>
		public ArrayList getFeatureLayers()
		{
			ArrayList pArrayList = new ArrayList();

			UID pUid = new UIDClass();
			pUid.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";

			IEnumLayer pLayers = this.axMapControl.ActiveView.FocusMap.get_Layers(pUid,true);

			ILayer layer = pLayers.Next();

			while(layer != null)
			{
				pArrayList.Add(layer);
				layer = pLayers.Next();
			}

			return pArrayList;

//			ILayer qrylayer = null;
//			FeatureLayer qryFeatLayer = null;
//			GroupLayer m_GroupLayer=null;
//			ArrayList arrOfFeatlayer=new ArrayList();
//			int layerCount;													//图层数
//			try 
//			{
//				layerCount = this.axMapControl.LayerCount;
//				for(int i=0; i<layerCount; i++)
//				{
//					qrylayer = this.axMapControl.get_Layer(i);
//					//判断qrylayer是否为FeatureLayer类型
//					if(!qrylayer.Name.Equals(null) && (qrylayer is FeatureLayer))
//					{						
//						qryFeatLayer = (FeatureLayer)qrylayer;
//						arrOfFeatlayer.Add(qryFeatLayer) ;						
//					}
////					else if (!qrylayer.Name.Equals(null) && (qrylayer is GroupLayer))
////					{
////						m_GroupLayer=(GroupLayer)qrylayer;
////						m_GroupLayer.
////					}
//				}		
//			}
//			catch (Exception e) 
//			{
//				Debug.WriteLine("——获取所有的图层数组(不包括raster)出错——："+e.Message );
//			}
//			return arrOfFeatlayer;
		}

		/// <summary>
		///  获取所有的栅格图层，返回栅格图层数组
		/// </summary>
		/// <returns></returns>
		public ArrayList getRasterLayers()
		{
			ILayer qrylayer = null;
			RasterLayer rasterLayer = null;
			ArrayList arrOfRasterlayer=new ArrayList();
			int layerCount;													//图层数
			try 
			{
				layerCount = this.axMapControl.LayerCount;
				for(int i=0; i<layerCount; i++)
				{
					qrylayer = this.axMapControl.get_Layer(i);
					//判断qrylayer是否为RasterLayer类型
					if(!qrylayer.Name.Equals(null) && (qrylayer is RasterLayer))
					{
						rasterLayer = (RasterLayer)qrylayer;
						arrOfRasterlayer.Add(rasterLayer) ;						
					}
				}		
			}
			catch (Exception e) 
			{
				Debug.WriteLine("——获取所有的栅格图层——："+e.Message );
			}
			return arrOfRasterlayer;
		}

		/// <summary>
		///  获取所有的图层数组，包括raster影像
		/// </summary>
		/// <returns></returns>
		public ArrayList getAllLayers()
		{
			ILayer qrylayer = null;
			ArrayList arrOfFeatlayer=new ArrayList();
			int layerCount;													//图层数
			try 
			{
				layerCount = this.axMapControl.LayerCount;
				for(int i=0; i<layerCount; i++)
				{
					qrylayer = this.axMapControl.get_Layer(i);
					//判断qrylayer是否为FeatureLayer类型
					if(!qrylayer.Name.Equals(null))
					{
						arrOfFeatlayer.Add(qrylayer) ;						
					}
				}		
			}
			catch (Exception e) 
			{
				Debug.WriteLine("——获取所有的图层数组(包括raster)出错——："+e.Message );
			}
			return arrOfFeatlayer;
		}

		/// <summary>
		/// 得到MapControl的图层名称数组
		/// </summary>
		/// <returns></returns>
		public String[] getLayerNames()
		{
			string[] layerNames = null;
			try
			{
				FeatureLayer qrylayer=null;
				int nums=this.getFeatureLayers().Count;						//得到图层数目
				Console.Write( "\t{0}", nums );
				
				ArrayList arrayOflayers = this.getFeatureLayers();			//得到图层数组
				layerNames = new string[nums];								//定义图层名称数组
				int i = 0;
		
				IEnumerator myEnumerator = arrayOflayers.GetEnumerator();   //遍历图层数组
				while ( myEnumerator.MoveNext() )
				{
					qrylayer=(FeatureLayer)myEnumerator.Current;
					layerNames[i]=qrylayer.Name;
					i++;
				}		
			}
			catch (Exception e) 
			{
				Debug.WriteLine("—— 得到MapControl的图层名称数组——："+e.Message );
			}
			return layerNames;
			
		}
		
		/// <summary>
		/// 得到所选的layer
		/// </summary>
		/// <param name="selLayer"></param>
		/// <returns></returns>
		public FeatureLayer getCurrentFeatureLayer(int selLayer)
		{
			FeatureLayer m_curLayer = null;
			try
			{
				ArrayList arrOfFeatlayer = this.getFeatureLayers();
				
				if(arrOfFeatlayer.Count>0)
					m_curLayer = (FeatureLayer) arrOfFeatlayer[selLayer];
//				ILayer m_layer = null;
//				int layerCount;													//图层数
//				
//					layerCount = this.axMapControl.LayerCount;
//					for(int i=0; i<layerCount; i++)
//					{
//						m_layer = this.axMapControl.get_Layer(i);
//						//判断qrylayer是否为FeatureLayer类型
//						if(m_layer.Visible==true && (m_layer is FeatureLayer))
//						{
//							m_curLayer = (FeatureLayer)m_layer;		
//							break;			
//						}
//					}			
				
			}
			catch (Exception e) 
			{
                //clsFunction.Function.MessageBoxError("获取当前图层时发生错误！"+ e.Message);
			}
			return m_curLayer;
		}

		/// <summary>
		/// 根据图层名得到图层
		/// </summary>
		/// <param name="layerName"></param>
		/// <returns></returns>
		public IFeatureLayer getFeatureLayerByName(String layerName)
		{
			IFeatureLayer fLayer=null;
			IFeatureLayer resOfLayer=null;
			try 
			{
				ArrayList arrayOflayers=this.getFeatureLayers();				//得到图层数组
		
				IEnumerator myEnumerator = arrayOflayers.GetEnumerator();		//遍历图层数组
				while ( myEnumerator.MoveNext() )
				{
					fLayer=(IFeatureLayer)myEnumerator.Current;
					if(fLayer!= null)
					{
						if(fLayer.Name.ToUpper()==layerName.ToUpper())
						{
							resOfLayer= fLayer;
							break;
						}	
					}
				}		
			} 		
			catch (Exception e) 
			{
				Debug.WriteLine("—— 得到所选的layer——："+e.Message );
			}
			return resOfLayer;	
		}

		
		/// <summary>
		/// 加入图层名称到图层名称组合框
		/// </summary>	
//		private void addItemToCombo1()
//		{
//			try
//			{
//				comboBox1.Items.Add("请选择图层");
//				String [] lnames=this.getLayerNames();
//				for(int i=0;i<lnames.Length;i++)
//				{
//					comboBox1.Items.Add(lnames[i]);
//				}
//				//comboBox1.SelectedIndex = 0;
//			}
//			catch(Exception e)
//			{
//				Debug.WriteLine(e.Message);
//			}
//		}

		/// <summary>
		/// 图层组合框选项变化时
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
//		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
//		{
//			//当前选择的序数
//			curIndex =comboBox1.SelectedIndex;
//			if(curIndex == 0)
//			{
//				MessageBox.Show("请选择图层"); 
//			}
//			else
//			{
//				//当前选择的图层
//				curLayer= getCurrentFeatureLayer(curIndex-1);
//				//当前选择的图层名称
//				selLayerName = curLayer.Name; 
//				//MessageBox.Show("选中的图层名为" + selLayerName); 	
//			}
//			
//		}
		
		#endregion
	
		#region 构建图幅树

		/// <summary>
		/// 得到某图层，某字段的所有值列表
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <returns></returns>
		public ArrayList getValueFixLayerFixField(AxMapControl axMapControl,string layerName, string fieldName)
		{
			//返回的数组
			ArrayList arryOfValue = new ArrayList() ;
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//获得要素类
					IFeatureClass ifeaturclass = fLayer.FeatureClass;

					//找到查询字段所在的编号
					int numOfField = ifeaturclass.FindField(fieldName);
			
					if(numOfField == -1 )
					{
						MessageBox.Show("对不起，该图层没有此字段");
					}
					else
					{
						//获得所有值		
						//double j = ifeaturclass.FeatureCount(null);
						for(int i=1;i <= ifeaturclass.FeatureCount(null); i++)
						{
							string strOfValue = ifeaturclass.GetFeature(i).get_Value(numOfField).ToString();
							arryOfValue.Add(strOfValue); 
							//j = i;
						}	
					}

				}
				else
				{
					MessageBox.Show("对不起，该图层不是FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO 自动生成 catch 块
				Debug.WriteLine("---getValueFixLayerFixField has an error---:"+e.Message) ;
			}	

			return arryOfValue;
		}


		/// <summary>
		/// 按图幅编号排序
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <returns></returns>
		public ITableSort sortValueFixLayerFixField(AxMapControl axMapControl,string layerName, string fieldName)
		{
			//返回的排序
			ITableSort pTableSort = new TableSort();
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//获得要素类
					IFeatureClass ifeaturclass = fLayer.FeatureClass;

					//找到查询字段所在的编号
					int numOfField = ifeaturclass.FindField(fieldName);
			
					if(numOfField == -1 )
					{
						MessageBox.Show("对不起，该图层没有此字段");
					}
					else
					{
						//产生一个临时的工作空间
						IWorkspace pScratchWorkspace;
						IScratchWorkspaceFactory pScratchWorkspaceFactory=  new ScratchWorkspaceFactory();
						pScratchWorkspace = pScratchWorkspaceFactory.DefaultScratchWorkspace;

					
						//获得选择集
						ISelectionSet pSelectionSet = ifeaturclass.Select(null,esriSelectionType.esriSelectionTypeIDSet,
							esriSelectionOption.esriSelectionOptionNormal,pScratchWorkspace);
						//排序
						//TableSort对象
						pTableSort.Fields = "TFBH";
						pTableSort.SelectionSet = pSelectionSet;
						pTableSort.Sort(null);
					}

				}
				else
				{
					MessageBox.Show("对不起，该图层不是FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO 自动生成 catch 块
				Debug.WriteLine("---排序————sortValueFixLayerFixField has an error---:"+e.Message) ;
			}	

			return pTableSort;
		}

		/// <summary>
		/// 返回字段名称所在的序列号
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <param name="layerName"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public int getNumberOfFieldName(AxMapControl axMapControl,string layerName, string fieldName)
		{
			//返回的序列号
			int num = 0;
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//获得要素类
					IFeatureClass ifeaturclass = fLayer.FeatureClass;

					//找到查询字段所在的编号
					int numOfField = ifeaturclass.FindField(fieldName);
			
					if(numOfField == -1 )
					{
						MessageBox.Show("对不起，该图层没有此字段");
					}
					else
					{
						num = numOfField;
							
					}

				}
				else
				{
					MessageBox.Show("对不起，该图层不是FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO 自动生成 catch 块
				Debug.WriteLine("---getValueFixLayerFixField has an error---:"+e.Message) ;
			}	

			return num;
			
		}


		/// <summary>
		/// 得到某图层的要素数目
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <returns></returns>
		public  int getCountFixLayer(AxMapControl axMapControl,string layerName)
		{
			//返回的要素数目
			int resOfCount = 0;
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//获得要素类
					IFeatureClass ifeaturclass = fLayer.FeatureClass;
					resOfCount =  ifeaturclass.FeatureCount(null);
				}
				else
				{
					MessageBox.Show("对不起，该图层不是FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO 自动生成 catch 块
				Debug.WriteLine("---getCountFixLayer has an error---:"+e.Message) ;
			}	
			return resOfCount;
		}


		/// <summary>
		/// 得到某图层、要素为objectId的字段的值（构建二级节点）
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <returns></returns>
		public  string  getValueFixLayerFixFieldFixOid(AxMapControl axMapControl,string layerName, string fieldName,int objectId)
		{
			//返回的数组
			string resOfValue = null;
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//获得要素类
					IFeatureClass ifeaturclass = fLayer.FeatureClass;

					//找到查询字段所在的编号
					int numOfField = ifeaturclass.FindField(fieldName);
			
					if(numOfField == -1 )
					{
						MessageBox.Show("对不起，该图层没有此字段");
					}
					else
					{
						//查询语句，查询oid为objectid的要素	
						IFeature ifeature = ifeaturclass.GetFeature(objectId); 
							
						if(ifeature != null)
						{
							resOfValue = ifeature.get_Value(numOfField).ToString();	
						}	
					}

				}
				else
				{
					MessageBox.Show("对不起，该图层不是FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO 自动生成 catch 块
				Debug.WriteLine("---getValueFixLayerFixField has an error---:"+e.Message) ;
			}	

			return resOfValue;
		}

		#endregion

		#region 定位

		/// <summary>
		/// 根据图层索引，字段和字段值定位到某要素
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <param name="layerName"></param>
		/// <param name="field"></param>
		/// <param name="Value"></param>
		public void locatedByFieldValue(AxMapControl axMapControl,string layerName,string field,string Value)
		{
			try
			{
				FeatureLayer fLayer = null;
				//得到县市级图层
				//ILayer iLayer =  axMapControl.get_Layer(layNum);
				ILayer iLayer = this.getFeatureLayerByName(layerName);
				if(iLayer !=null && iLayer is FeatureLayer)
					fLayer = (FeatureLayer)iLayer;

				//查询语句       
				QueryFilter queryFilter = new QueryFilter();
				queryFilter.WhereClause =  field + " = '" + Value + "'" ;
				
				//要素选择集
				IFeatureCursor pCursor = null;
				pCursor =fLayer.Search(queryFilter,false);

				IFeature feature=pCursor.NextFeature();
				
				IEnvelope m_Envelope = new Envelope() as IEnvelope;

				if(feature!=null)
				{
					//地图定位到当前所查的要素
					m_Envelope.PutCoords(feature.Extent.XMin,feature.Extent.YMin,feature.Extent.XMax,feature.Extent.YMax);
					axMapControl.Extent = m_Envelope;
					//高亮显示
					axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,iLayer, null);
					axMapControl.ActiveView.Refresh();	

				}	
				else
					MessageBox.Show("没有此要素"); 
			}
			catch(Exception ee)
			{
				Debug.WriteLine("---locatedByFieldValue has errors---:"+ee.Message);
			}	
		}

		#endregion

		#region 选择要素

		/// <summary>
		/// 在当前图层，根据选择的geometry，选择要素
		/// </summary>
		/// <param name="selectGeometry"></param>
		/// <param name="fLayer"></param>
		public ArrayList selectFeatures(IGeometry selectGeometry,FeatureLayer fLayer)
		{
			//ArrayList:当前图层中选中的要素
			ArrayList selectedFeatures = new ArrayList() ;

			try 
			{
				/*执行查询*/			   
				SpatialFilter spatialFilter = new SpatialFilter();
				spatialFilter.set_GeometryEx(selectGeometry,false);
				spatialFilter.GeometryField = "Shape";
				switch(fLayer.FeatureClass.ShapeType)
				{
					case esriGeometryType.esriGeometryPoint:
						spatialFilter.SpatialRel  = esriSpatialRelEnum.esriSpatialRelContains;break;
					case esriGeometryType.esriGeometryPolyline:
						spatialFilter.SpatialRel  = esriSpatialRelEnum.esriSpatialRelCrosses;break;
					case esriGeometryType.esriGeometryPolygon:
						spatialFilter.SpatialRel  = esriSpatialRelEnum.esriSpatialRelIntersects;break;

				}
				this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,fLayer,null);

				IFeatureSelection featureSelection;
				featureSelection = fLayer as IFeatureSelection;

				//Execute the selection
				IFeatureCursor pCursor = null;
				pCursor = fLayer.Search(spatialFilter, false) ;
				//Retrieve the selected features

				featureSelection.Clear();
				
				IFeature pFeature = null;
				pFeature = pCursor.NextFeature();
				while (pFeature != null) 
				{
					featureSelection.Add(pFeature);
					selectedFeatures.Add(pFeature);	
					pFeature = pCursor.NextFeature();		
				}
	
				this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,fLayer,null);
			} 
			catch (IOException e1) 
			{
				Debug.WriteLine(e1.Message);
			}
			return selectedFeatures;
		}


		/// <summary>
		/// 在当前图层，根据选择的geometry，选择要素
		/// </summary>
		/// <param name="selectGeometry"></param>
		/// <param name="fLayer"></param>
		public IFeature selectFeatures_FEA(IGeometry selectGeometry,FeatureLayer fLayer)
		{
			IFeature pFeature = null;

			try 
			{
				/*执行查询*/			   
				SpatialFilter spatialFilter = new SpatialFilter();
				spatialFilter.set_GeometryEx(selectGeometry,false);
				spatialFilter.GeometryField = "Shape";
				spatialFilter.SpatialRel  = esriSpatialRelEnum.esriSpatialRelIntersects ;
				this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,fLayer,null);

				IFeatureSelection featureSelection;
				featureSelection = fLayer as IFeatureSelection;

				//Execute the selection
				IFeatureCursor pCursor = null;
				pCursor = fLayer.Search(spatialFilter, false) ;
				//Retrieve the selected features

				featureSelection.Clear();
				
				pFeature = pCursor.NextFeature();
				if (pFeature != null) 
				{
					featureSelection.Add(pFeature);		
				}
	
				this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,fLayer,null);
				this.axMapControl.ActiveView.Refresh();
			} 
			catch (IOException e1) 
			{
				Debug.WriteLine(e1.Message);
			}

			return pFeature;
		}
		#endregion

	}

}
