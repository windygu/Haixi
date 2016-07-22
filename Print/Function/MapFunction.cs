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
	/// MapFunction ��ժҪ˵����
	/// ���õĵ�ͼ����
	/// </summary>
	public class MapFunction
	{
		#region  ���ݳ�Ա

		//��ͼ����ؼ�
		public AxMapControl axMapControl;
		//��ͼ�ؼ�
		public AxPageLayoutControl axPageLayoutControl;

		#region ��ͼ�������

		// ���ڷŴ󹤾�
		private ICommand pZoomInCommand=null ; 
		// ������С����
		private ICommand pZoomOutCommand =null;  
		//�����������Ź���
		private ICommand pZoomPanCommand =null;  
		// ���ڹ̶��Ŵ󹤾�
		private ICommand pZoomInFixedCommand = null;
		// ���ڹ̶���С����
		private ICommand pZoomOutFixedCommand = null;
		// ����ȫͼ����
		private ICommand pFullExtentCommand=null;  
		// ����ǰһ��ͼ����
		private ICommand pToLastExtentBackCommand=null;  
		// ���ں�һ��ͼ���� 
		private ICommand pToLastExtentForwardCommand=null;  				
		// ����ˢ�¹���
		private ICommand pMapRefreshommand=null;  
		// �������ι���
		private ICommand pPanCommand  =null;

		#endregion

		#region	��ͼ����
		//�̶��Ŵ�
		private ICommand pageZoomInFixCommand=null ; 
		//�̶���С
		private ICommand pageZoomOutFixCommand=null; 
		//ǰһ��ͼ	
		private ICommand pageZoomPageToLastExtentBackCommand=null; 
		//��һ��ͼ 
		private ICommand pageZoomPageToLastExtentForwardCommand=null; 
		//����		  
		private ICommand pageZoomPagePanTool=null; 
		//��ʾ����	 
		private ICommand pageZoomPageZoomTool=null; 
		//�Ŵ�		 
		private ICommand pageZoomInTool =null; 
		//��С		 
		private ICommand pageZoomOutTool=null; 
		//���ŵ�ҳ����    
		private ICommand pageZoomPageWidthCommand =null; 
		//���ŵ�����1��1   
		private ICommand pageZoomZoom100PercentCommand =null; 
		//���ŵ���ҳ       
		private ICommand pageZoomZoomWholePageCommand =null; 
		//ѡ�񹤾�          
		private ICommand pageSelectTool  =null; 
		//��Բ              
		private ICommand pageNewCircleTool  =null; 
		//������         
		private ICommand pageNewCurveTool  =null; 
		//����Բ     	
		private ICommand pageNewEllipseTool =null; 
		//�����  	    
		private ICommand pageNewFrameTool=null; 
		//����������    
		private ICommand pageNewFreeHandTool =null; 
		//����          
		private ICommand pageNewLineTool =null; 
		//�������      
		private ICommand pageNewPolygonTool =null; 
		//������		
		private ICommand pageNewRectangleTool =null; 





		#endregion

		#region ��������

		//�ָ��ϴβ���
		private ICommand pRedoCommand  =null;   
		//�����ϴβ���
		private ICommand pUndoCommand  =null;   
		//��ѡѡ��Ҫ��
		private ICommand pSelectFeaturesToolCommand  =null;  
		//��ͼ��ѡ��Ҫ��
		private ICommand pSelectByGraphicsCommand =null;  
		//���ѡ��
		private ICommand pClearSelectionCommand =null;  
		private ICommand pSelectFeaturesCommand =null;  
		
		//���µ�ͼ�ĵ�
		private ICommand pOpenDocCommand  =null;  
		//���Ϊ
		private ICommand pSaveAsDocCommand =null;  
		

		// ��������ת���֣�ͼ�񹤾�
		private ICommand pRotateLeftCommand=null; 	
		// ��������ת���֣�ͼ�񹤾�
		private ICommand pRotateRightCommand=null; 
		// ����ȡ����ͼ��ת����
		private ICommand pClearMapRotationCommand=null;  

		//�չ���
		public ITool pNullTool = (ITool) new  Controls3DAnalystContourTool ();

		#endregion

		//MapControlͼ������
		//String[] lname;

		#endregion

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="axMapControl"></param>
		public MapFunction(AxMapControl axMapControl,AxPageLayoutControl axPageLayoutControl)
		{
			this.axMapControl = axMapControl;
			this.axPageLayoutControl = axPageLayoutControl;
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="axMapControl"></param>
		public MapFunction()
		{			
		}


		#region ��ͼ���������ͼ����

		/// <summary>
		/// ѡ���������ѡ�Ϳ�ѡ�����пɼ�ͼ���еĶ���ѡ��
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
		/// �Ŵ����
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
		/// ��С����
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
		/// �������Ų���
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
		/// �̶��Ŵ����
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
		/// �̶���С����
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
		/// ȫͼ����
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
		/// ǰһ��ͼ����
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
		/// ��һ��ͼ����
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
		/// ˢ�²���
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
		/// ���β���
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

		#region ��ͼ������ͼ����

		#region ͬ��ͼ���

		/// <summary>
		/// �Ŵ�
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
		/// ��С
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
		/// �̶��Ŵ����
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
		/// �̶���С����
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
		/// ǰһ��ͼ
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
		/// ��һ��ͼ
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
		/// ����
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

		#region ����

		/// <summary>
		/// ��ʾ����
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
		/// ���ŵ�ҳ����
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
		/// ���ŵ�����1��1
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
		/// ���ŵ���ҳ
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

		#region ѡ��

		/// <summary>
		/// ѡ�񹤾�
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

		#region ��������״

		/// <summary>
		/// ��Բ
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
		/// ������
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
		/// ����Բ
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
		/// �����
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
		/// ����������
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
		/// ����
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
		/// �������
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
		/// ������
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

		#region ѡ��Ҫ�صȹ���

		/// <summary>
		///	��ѡ��ѡ��Ҫ�ز���
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
		///	���Ҫ��ѡ�����
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
		///	�ָ��ϴβ�������
		/// </summary>
		public void Redo()
		{
			pRedoCommand = new ControlsRedoCommandClass();
			pRedoCommand.OnCreate (axMapControl.GetOcx ());
			pRedoCommand.OnClick ();
			axMapControl.CurrentTool = pRedoCommand as ITool ;
		}
		
		/// <summary>
		///	�����ϴβ�������
		/// </summary>
		public void Undo()
		{
			pUndoCommand = new ControlsUndoCommandClass();
			pUndoCommand.OnCreate (axMapControl.GetOcx ());
			pUndoCommand.OnClick ();
			axMapControl.CurrentTool = pUndoCommand as ITool ;
		}

		/// <summary>
		/// /��ͼ��ѡ��Ҫ��
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
		///	�򿪵�ͼ�ĵ�����
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
		///	����ͼ�ĵ�����
		/// </summary>
		public void SaveAsDoc()
		{
			pSaveAsDocCommand = new ControlsSaveAsDocCommandClass();
			pSaveAsDocCommand.OnCreate (axMapControl.GetOcx ());
			pSaveAsDocCommand.OnClick ();
			axMapControl.CurrentTool = pSaveAsDocCommand as ITool ;
		}

		/// <summary>
		/// ����ת
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
		/// ����ת
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
		/// ȡ����ת
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

		#region ��ͼ��λ���������ȹ���

		/// <summary>
		/// �õ���ǰMapControl����ĵ�ͼ�ĵ�λ
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
		/// �������
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
				// TODO �Զ����� catch ��
				Debug.WriteLine("MeasureLength has errors:"+e.Message);
			}
			return length.ToString();
		
		}
	
		/// <summary>
		/// �������
		/// </summary>
		/// <returns></returns>
		public String MeasureArea(IGeometry trackPolygon)
		{
			double area=0.0;		
			try 
			{
				//����ͶӰ
				SpatialReferenceEnvironment pSpatRefFact =new SpatialReferenceEnvironment();
				ISpatialReference pProjCoordSystem =
					pSpatRefFact.CreateProjectedCoordinateSystem(21476);

				//���������polygon
				IPolygon iPolygon=(IPolygon)trackPolygon;
				iPolygon.Project(pProjCoordSystem) ;

				//���polygon�����
				IArea iArea = (IArea)iPolygon;
				area=iArea.Area;

			} 
			catch (Exception e) 
			{
				// TODO �Զ����� catch ��
				Debug.WriteLine("MeasureArea has errors:"+e.Message);
			}
			return area.ToString() ;
		
		}

		#endregion

		#region ����դ��Ӱ��RasterDataset

		/// <summary>
		/// ����SDE���ݿ⣬��RasterDatasetդ��Ӱ�����ݼ�
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
			{ //���ص�RasterDataset
				
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
				Debug.WriteLine("�����������ݿ��RasterDataset��������"+e.Message );
			}
			return rasterDataset;
		}


	
		/// <summary>
		/// �жϵ�ǰͼ���Ƿ�����˲�ѯ����RasterDataset
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
					 //����������
					 if(nameOfRaster == nameOfLayer)
					 {
					 	have = true;
						break;
					 }
				}
				
			}
			catch (Exception e)
			{
				Debug.WriteLine("����MapControl�Ƿ���iRasterDataset������"+e.Message );
			}
			return have;

		}

		/// <summary>
		/// ��MapControl�����SDE���ݿ����Ӱ��
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
				Debug.WriteLine("������MapControl�����RasterDataset��������"+e.Message );
			}
		}

		#endregion

		#region ����դ��Ӱ��in RasterCatalog


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
		/// ����դ��Ӱ��RasterCatalog
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

		#region �ϴ����ص�դ��Ӱ��
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

		#region ɾ�����е�դ��Ӱ��

		/// <summary>
		/// �Ƴ�����դ��ͼ��
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
				Debug.WriteLine("---ɾ�����е�դ��Ӱ�����----"+e.Message );
			}
		}

		/// <summary>
		/// �ر�����դ��ͼ��ʹ����Ӱ��ͼ��visible=false
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
				Debug.WriteLine("---ɾ�����е�դ��Ӱ�����----"+e.Message );
			}
		}


		#endregion

		#region �����ռ���ز���

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

		#region ɾ�����е�դ��Ӱ��


		/// <summary>
		/// ���ӷ���
		/// </summary>
		/// <param name="m_Geometry">���ӷ����ķ�Χ</param>
		/// <param name="mLayer">������ӷ�����ͼ��</param>
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
				 
				//����ͼ��������
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

		#region ��ȡ��ǰͼ��

		/// <summary>
		///  ��ȡ���е�ʸ��ͼ�㣬����ʸ��ͼ�����飬������rasterӰ��
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
//			int layerCount;													//ͼ����
//			try 
//			{
//				layerCount = this.axMapControl.LayerCount;
//				for(int i=0; i<layerCount; i++)
//				{
//					qrylayer = this.axMapControl.get_Layer(i);
//					//�ж�qrylayer�Ƿ�ΪFeatureLayer����
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
//				Debug.WriteLine("������ȡ���е�ͼ������(������raster)��������"+e.Message );
//			}
//			return arrOfFeatlayer;
		}

		/// <summary>
		///  ��ȡ���е�դ��ͼ�㣬����դ��ͼ������
		/// </summary>
		/// <returns></returns>
		public ArrayList getRasterLayers()
		{
			ILayer qrylayer = null;
			RasterLayer rasterLayer = null;
			ArrayList arrOfRasterlayer=new ArrayList();
			int layerCount;													//ͼ����
			try 
			{
				layerCount = this.axMapControl.LayerCount;
				for(int i=0; i<layerCount; i++)
				{
					qrylayer = this.axMapControl.get_Layer(i);
					//�ж�qrylayer�Ƿ�ΪRasterLayer����
					if(!qrylayer.Name.Equals(null) && (qrylayer is RasterLayer))
					{
						rasterLayer = (RasterLayer)qrylayer;
						arrOfRasterlayer.Add(rasterLayer) ;						
					}
				}		
			}
			catch (Exception e) 
			{
				Debug.WriteLine("������ȡ���е�դ��ͼ�㡪����"+e.Message );
			}
			return arrOfRasterlayer;
		}

		/// <summary>
		///  ��ȡ���е�ͼ�����飬����rasterӰ��
		/// </summary>
		/// <returns></returns>
		public ArrayList getAllLayers()
		{
			ILayer qrylayer = null;
			ArrayList arrOfFeatlayer=new ArrayList();
			int layerCount;													//ͼ����
			try 
			{
				layerCount = this.axMapControl.LayerCount;
				for(int i=0; i<layerCount; i++)
				{
					qrylayer = this.axMapControl.get_Layer(i);
					//�ж�qrylayer�Ƿ�ΪFeatureLayer����
					if(!qrylayer.Name.Equals(null))
					{
						arrOfFeatlayer.Add(qrylayer) ;						
					}
				}		
			}
			catch (Exception e) 
			{
				Debug.WriteLine("������ȡ���е�ͼ������(����raster)��������"+e.Message );
			}
			return arrOfFeatlayer;
		}

		/// <summary>
		/// �õ�MapControl��ͼ����������
		/// </summary>
		/// <returns></returns>
		public String[] getLayerNames()
		{
			string[] layerNames = null;
			try
			{
				FeatureLayer qrylayer=null;
				int nums=this.getFeatureLayers().Count;						//�õ�ͼ����Ŀ
				Console.Write( "\t{0}", nums );
				
				ArrayList arrayOflayers = this.getFeatureLayers();			//�õ�ͼ������
				layerNames = new string[nums];								//����ͼ����������
				int i = 0;
		
				IEnumerator myEnumerator = arrayOflayers.GetEnumerator();   //����ͼ������
				while ( myEnumerator.MoveNext() )
				{
					qrylayer=(FeatureLayer)myEnumerator.Current;
					layerNames[i]=qrylayer.Name;
					i++;
				}		
			}
			catch (Exception e) 
			{
				Debug.WriteLine("���� �õ�MapControl��ͼ���������顪����"+e.Message );
			}
			return layerNames;
			
		}
		
		/// <summary>
		/// �õ���ѡ��layer
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
//				int layerCount;													//ͼ����
//				
//					layerCount = this.axMapControl.LayerCount;
//					for(int i=0; i<layerCount; i++)
//					{
//						m_layer = this.axMapControl.get_Layer(i);
//						//�ж�qrylayer�Ƿ�ΪFeatureLayer����
//						if(m_layer.Visible==true && (m_layer is FeatureLayer))
//						{
//							m_curLayer = (FeatureLayer)m_layer;		
//							break;			
//						}
//					}			
				
			}
			catch (Exception e) 
			{
                //clsFunction.Function.MessageBoxError("��ȡ��ǰͼ��ʱ��������"+ e.Message);
			}
			return m_curLayer;
		}

		/// <summary>
		/// ����ͼ�����õ�ͼ��
		/// </summary>
		/// <param name="layerName"></param>
		/// <returns></returns>
		public IFeatureLayer getFeatureLayerByName(String layerName)
		{
			IFeatureLayer fLayer=null;
			IFeatureLayer resOfLayer=null;
			try 
			{
				ArrayList arrayOflayers=this.getFeatureLayers();				//�õ�ͼ������
		
				IEnumerator myEnumerator = arrayOflayers.GetEnumerator();		//����ͼ������
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
				Debug.WriteLine("���� �õ���ѡ��layer������"+e.Message );
			}
			return resOfLayer;	
		}

		
		/// <summary>
		/// ����ͼ�����Ƶ�ͼ��������Ͽ�
		/// </summary>	
//		private void addItemToCombo1()
//		{
//			try
//			{
//				comboBox1.Items.Add("��ѡ��ͼ��");
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
		/// ͼ����Ͽ�ѡ��仯ʱ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
//		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
//		{
//			//��ǰѡ�������
//			curIndex =comboBox1.SelectedIndex;
//			if(curIndex == 0)
//			{
//				MessageBox.Show("��ѡ��ͼ��"); 
//			}
//			else
//			{
//				//��ǰѡ���ͼ��
//				curLayer= getCurrentFeatureLayer(curIndex-1);
//				//��ǰѡ���ͼ������
//				selLayerName = curLayer.Name; 
//				//MessageBox.Show("ѡ�е�ͼ����Ϊ" + selLayerName); 	
//			}
//			
//		}
		
		#endregion
	
		#region ����ͼ����

		/// <summary>
		/// �õ�ĳͼ�㣬ĳ�ֶε�����ֵ�б�
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <returns></returns>
		public ArrayList getValueFixLayerFixField(AxMapControl axMapControl,string layerName, string fieldName)
		{
			//���ص�����
			ArrayList arryOfValue = new ArrayList() ;
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//���Ҫ����
					IFeatureClass ifeaturclass = fLayer.FeatureClass;

					//�ҵ���ѯ�ֶ����ڵı��
					int numOfField = ifeaturclass.FindField(fieldName);
			
					if(numOfField == -1 )
					{
						MessageBox.Show("�Բ��𣬸�ͼ��û�д��ֶ�");
					}
					else
					{
						//�������ֵ		
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
					MessageBox.Show("�Բ��𣬸�ͼ�㲻��FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO �Զ����� catch ��
				Debug.WriteLine("---getValueFixLayerFixField has an error---:"+e.Message) ;
			}	

			return arryOfValue;
		}


		/// <summary>
		/// ��ͼ���������
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <returns></returns>
		public ITableSort sortValueFixLayerFixField(AxMapControl axMapControl,string layerName, string fieldName)
		{
			//���ص�����
			ITableSort pTableSort = new TableSort();
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//���Ҫ����
					IFeatureClass ifeaturclass = fLayer.FeatureClass;

					//�ҵ���ѯ�ֶ����ڵı��
					int numOfField = ifeaturclass.FindField(fieldName);
			
					if(numOfField == -1 )
					{
						MessageBox.Show("�Բ��𣬸�ͼ��û�д��ֶ�");
					}
					else
					{
						//����һ����ʱ�Ĺ����ռ�
						IWorkspace pScratchWorkspace;
						IScratchWorkspaceFactory pScratchWorkspaceFactory=  new ScratchWorkspaceFactory();
						pScratchWorkspace = pScratchWorkspaceFactory.DefaultScratchWorkspace;

					
						//���ѡ��
						ISelectionSet pSelectionSet = ifeaturclass.Select(null,esriSelectionType.esriSelectionTypeIDSet,
							esriSelectionOption.esriSelectionOptionNormal,pScratchWorkspace);
						//����
						//TableSort����
						pTableSort.Fields = "TFBH";
						pTableSort.SelectionSet = pSelectionSet;
						pTableSort.Sort(null);
					}

				}
				else
				{
					MessageBox.Show("�Բ��𣬸�ͼ�㲻��FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO �Զ����� catch ��
				Debug.WriteLine("---���򡪡�����sortValueFixLayerFixField has an error---:"+e.Message) ;
			}	

			return pTableSort;
		}

		/// <summary>
		/// �����ֶ��������ڵ����к�
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <param name="layerName"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public int getNumberOfFieldName(AxMapControl axMapControl,string layerName, string fieldName)
		{
			//���ص����к�
			int num = 0;
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//���Ҫ����
					IFeatureClass ifeaturclass = fLayer.FeatureClass;

					//�ҵ���ѯ�ֶ����ڵı��
					int numOfField = ifeaturclass.FindField(fieldName);
			
					if(numOfField == -1 )
					{
						MessageBox.Show("�Բ��𣬸�ͼ��û�д��ֶ�");
					}
					else
					{
						num = numOfField;
							
					}

				}
				else
				{
					MessageBox.Show("�Բ��𣬸�ͼ�㲻��FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO �Զ����� catch ��
				Debug.WriteLine("---getValueFixLayerFixField has an error---:"+e.Message) ;
			}	

			return num;
			
		}


		/// <summary>
		/// �õ�ĳͼ���Ҫ����Ŀ
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <returns></returns>
		public  int getCountFixLayer(AxMapControl axMapControl,string layerName)
		{
			//���ص�Ҫ����Ŀ
			int resOfCount = 0;
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//���Ҫ����
					IFeatureClass ifeaturclass = fLayer.FeatureClass;
					resOfCount =  ifeaturclass.FeatureCount(null);
				}
				else
				{
					MessageBox.Show("�Բ��𣬸�ͼ�㲻��FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO �Զ����� catch ��
				Debug.WriteLine("---getCountFixLayer has an error---:"+e.Message) ;
			}	
			return resOfCount;
		}


		/// <summary>
		/// �õ�ĳͼ�㡢Ҫ��ΪobjectId���ֶε�ֵ�����������ڵ㣩
		/// </summary>
		/// <param name="axMapControl"></param>
		/// <returns></returns>
		public  string  getValueFixLayerFixFieldFixOid(AxMapControl axMapControl,string layerName, string fieldName,int objectId)
		{
			//���ص�����
			string resOfValue = null;
			try
			{
				ILayer iLayer =getFeatureLayerByName(layerName);

				if(iLayer is FeatureLayer)
				{
					FeatureLayer fLayer = (FeatureLayer)iLayer;	

					//���Ҫ����
					IFeatureClass ifeaturclass = fLayer.FeatureClass;

					//�ҵ���ѯ�ֶ����ڵı��
					int numOfField = ifeaturclass.FindField(fieldName);
			
					if(numOfField == -1 )
					{
						MessageBox.Show("�Բ��𣬸�ͼ��û�д��ֶ�");
					}
					else
					{
						//��ѯ��䣬��ѯoidΪobjectid��Ҫ��	
						IFeature ifeature = ifeaturclass.GetFeature(objectId); 
							
						if(ifeature != null)
						{
							resOfValue = ifeature.get_Value(numOfField).ToString();	
						}	
					}

				}
				else
				{
					MessageBox.Show("�Բ��𣬸�ͼ�㲻��FeatureLayer");
				}

			}
			catch (Exception e) 
			{
				// TODO �Զ����� catch ��
				Debug.WriteLine("---getValueFixLayerFixField has an error---:"+e.Message) ;
			}	

			return resOfValue;
		}

		#endregion

		#region ��λ

		/// <summary>
		/// ����ͼ���������ֶκ��ֶ�ֵ��λ��ĳҪ��
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
				//�õ����м�ͼ��
				//ILayer iLayer =  axMapControl.get_Layer(layNum);
				ILayer iLayer = this.getFeatureLayerByName(layerName);
				if(iLayer !=null && iLayer is FeatureLayer)
					fLayer = (FeatureLayer)iLayer;

				//��ѯ���       
				QueryFilter queryFilter = new QueryFilter();
				queryFilter.WhereClause =  field + " = '" + Value + "'" ;
				
				//Ҫ��ѡ��
				IFeatureCursor pCursor = null;
				pCursor =fLayer.Search(queryFilter,false);

				IFeature feature=pCursor.NextFeature();
				
				IEnvelope m_Envelope = new Envelope() as IEnvelope;

				if(feature!=null)
				{
					//��ͼ��λ����ǰ�����Ҫ��
					m_Envelope.PutCoords(feature.Extent.XMin,feature.Extent.YMin,feature.Extent.XMax,feature.Extent.YMax);
					axMapControl.Extent = m_Envelope;
					//������ʾ
					axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,iLayer, null);
					axMapControl.ActiveView.Refresh();	

				}	
				else
					MessageBox.Show("û�д�Ҫ��"); 
			}
			catch(Exception ee)
			{
				Debug.WriteLine("---locatedByFieldValue has errors---:"+ee.Message);
			}	
		}

		#endregion

		#region ѡ��Ҫ��

		/// <summary>
		/// �ڵ�ǰͼ�㣬����ѡ���geometry��ѡ��Ҫ��
		/// </summary>
		/// <param name="selectGeometry"></param>
		/// <param name="fLayer"></param>
		public ArrayList selectFeatures(IGeometry selectGeometry,FeatureLayer fLayer)
		{
			//ArrayList:��ǰͼ����ѡ�е�Ҫ��
			ArrayList selectedFeatures = new ArrayList() ;

			try 
			{
				/*ִ�в�ѯ*/			   
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
		/// �ڵ�ǰͼ�㣬����ѡ���geometry��ѡ��Ҫ��
		/// </summary>
		/// <param name="selectGeometry"></param>
		/// <param name="fLayer"></param>
		public IFeature selectFeatures_FEA(IGeometry selectGeometry,FeatureLayer fLayer)
		{
			IFeature pFeature = null;

			try 
			{
				/*ִ�в�ѯ*/			   
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
