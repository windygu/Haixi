using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using MainFrame.Form.MapAttribute;
using MainFrame.Form.Search;
using MainFrame.Form.Function;
using MainFrame.Form.Render;
using MainFrame.Form.analysis;
using MainFrame.Form.Mapprint;
using SuperFlow;

namespace Print.MapControl
{
	/// <summary>
	/// frmMapControl ��ժҪ˵����
	/// </summary>
	public class frmMapControl : System.Windows.Forms.Form
	{
		#region  ����ؼ�

		public AxMapControl axMapControl;
		public System.Windows.Forms.StatusBar statusBar1;
		private StatusBarPanel ZhuangtaiPanel;
		private StatusBarPanel TufuPanel;
		public StatusBarPanel ZuobiaoPanel;
		public StatusBarPanel BiliPanel;
		private SuperFlow.OfficeMenus officeMenus1=new OfficeMenus();


		#endregion

		#region ���ݱ���

		//������ͼ������
		private string m_strQXXZQY="����������";
		private string m_strDSXZQY="����������";
		private string m_strSJXZQY="ʡ��������";
		////////////////////////////////////////////

		//ESRI
		private IAoInitialize m_AoInitialize = new AoInitializeClass();		 //The initialization object
		//����������
		private SuperToolBar.ToolBarManager m_BarManager;
		//��ͼ���ù��ܱ���
		public MainFrame.Form.Function.MapFunction mapFuntion;
		//��ʼ��ͼ·��
		public string sFilePath;
		//��ͼ��λ
		private esriUnits mapUnits;			
		private string sMapUnits;	
		//ͼ������
		ArrayList arryOfLayer = new ArrayList();
		//���ͼ��������
		public ITOCControl m_TOCControl;
		public  ESRI.ArcGIS.Controls.AxTOCControl axTOCControl;
		//ͼ��ѡ���
		public System.Windows.Forms.ComboBox LayersComboBox;
		//Ӱ���ѡ���
//		public System.Windows.Forms.ComboBox cmbImageRaster;
		//��ǰѡ���ͼ��
		public FeatureLayer curLayer;
		//Mxd�ļ�
		public IMapDocument m_MapDocument = new MapDocumentClass();
		public System.Windows.Forms.OpenFileDialog openFileDialog1;   //���ļ��Ի���
		//�ж��������,�������
		public bool isMeasureLength =false;
		public bool isMeasureArea =false;
		//�жϵ�ѡ�������ѡ��Բѡ����ѡ
		public bool isDianXuan = false;
		public bool isDuobxXuan =false;
		public bool isYuanXuan =false;
		public bool isKuangXuan =false;
		//�鿴���Դ���
		public Form.MapAttribute.IDentifyDialog identifyForm;
		//�ж��Ƿ���˲鿴����
		public bool isAttribute =false;

		//�༭
		public bool isPoint = false;
		public bool isPolyline = false;
		public bool isPolygon = false;
		public bool EndEditing = false;
		public bool isDelete = false;
		//�༭�����ռ�
		public IWorkspaceEdit m_WorkspaceEdit;
		private IFeatureLayer m_FeatureLayer = null;
		public IFeatureClass m_FeatureClass = null;
		//��ͼ��������
		private INewLineFeedback m_NewLineFeedback = null;
		private INewPolygonFeedback m_NewPolygonFeedback = null;
		
		//�������ݿ�
//		public clsDataAccess.DataAccess objectDataAccess;
		

		//�鿴����
		//ѡ��Ҫ������
		public  ArrayList arryOfSelFea= new ArrayList() ;
		//�ռ����
		public Form.Search.QuikFindForm m_QuikFindForm;
		public Form.Search.SelectByAttributeForm m_SelectByAttributeForm;
		public Form.analysis.BufferForm m_BufferForm;
		public Form.analysis.OverlayForm m_OverlayForm;
		
		//��ͼ��ר����ͼ��������
		public Form.Render.SimpleRenderForm m_SimpleRenderForm;
		public Form.Render.ClassBreakRendererForm m_ClassBreakRendererForm;
		public Form.Render.UniqueValueRendererForm m_UniqueValueRendererForm;
		public Form.Render.ProportionalSymbolRendererForm m_ProportionalSymbolRendererForm;
		public Form.Render.DotDensityRendererForm m_DotDensityRendererForm;
		public Form.Render.ChartRenderForm m_ChartRenderForm; 

		//��ӡ
		public AxPageLayoutControl axPageLayoutControl;
        public Form.PageLayoutControl.frmPageLayoutControl frmPageLayoutControl;
		//�����ӡ����
		public Form.Mapprint.regionalprint regionalPrintForm;
		private bool indexRY = false;
		private INewEnvelopeFeedback m_NewEnvelopeFeedback = null;
		public  IElement m_RectangleEle;


		//�رմ����¼�
		public delegate void CloseFormEventHandler();
		public event CloseFormEventHandler CloseEvent;


		#endregion

		IPoint m_IPointStart=new ESRI.ArcGIS.Geometry.PointClass();
		IPoint m_IPointEnd=new ESRI.ArcGIS.Geometry.PointClass();
		public int ShowImagesTimes;
        //public frmSetImageCatalog m_frmSetImageCatalog;//�趨�û�ʹ�õ�Ӱ��ͼ���ĸ�rastercatalog��
		public string m_strCurrentRasterCatalog;//��¼�û�ʹ�õ�Ӱ��ͼ���ĸ�rastercatalog��

        //public clsDataAccess.DataAccess objectDataAccess;
        private System.Windows.Forms.ProgressBar progressBar1;
        private AxLicenseControl axLicenseControl1;
        private AxMapControl axMapControl1;
        private AxPageLayoutControl axPageLayoutControl1;
        private AxLicenseControl axLicenseControl2;
		
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMapControl()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
			ShowImagesTimes=0;
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapControl));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axLicenseControl2 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(259, 56);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(212, 126);
            this.axMapControl1.TabIndex = 0;
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.Location = new System.Drawing.Point(355, 237);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(115, 36);
            this.axPageLayoutControl1.TabIndex = 1;
            // 
            // axLicenseControl2
            // 
            this.axLicenseControl2.Enabled = true;
            this.axLicenseControl2.Location = new System.Drawing.Point(226, 233);
            this.axLicenseControl2.Name = "axLicenseControl2";
            this.axLicenseControl2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl2.OcxState")));
            this.axLicenseControl2.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl2.TabIndex = 2;
            // 
            // frmMapControl
            // 
            this.ClientSize = new System.Drawing.Size(604, 329);
            this.Controls.Add(this.axLicenseControl2);
            this.Controls.Add(this.axPageLayoutControl1);
            this.Controls.Add(this.axMapControl1);
            this.Name = "frmMapControl";
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl2)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region ���������ر��¼�

		/// <summary>
		/// ���������¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmMapControl_Load(object sender, EventArgs e)
		{			
			m_frmSetImageCatalog=new frmSetImageCatalog();	
		}

		public void InitMap()
		{
			this.progressBar1.Visible=true;
			this.progressBar1.Maximum =100;
			this.progressBar1.Minimum  =0;
			this.progressBar1.Value  =20;

			//����Office�˵���ʽ�͸���������
			this.AddToolBarToDockWindow();	
			this.progressBar1.Value  =30;

            //axMapControl.Refresh();//����һ��ʼ�ѿ��Ľ���

//			InitcmbImageRaster();

			this.progressBar1.Value  =50;
			//�򿪵�ͼ
			if(this.sFilePath != null)
			{
				LoadFile(sFilePath);
			}
			else
			{
				clsFunction.Function.MessageBoxError(this,"û���ҵ�ָ�����ļ���");
				return ;
			}
			this.progressBar1.Value  =80;

			//��ͼ��λ
			this.mapUnits = axMapControl.MapUnits;
			sMapUnits =this.mapFuntion.getMapUnits(mapUnits);	
			
			//����ͼ����
			m_TOCControl.SetBuddyControl(axMapControl);

			this.progressBar1.Value  =90;

			//�õ�ʸ��ͼ�����飬�����ַ�����Ͽ�
//			this.addItemToLayersComboBox();
//			if(LayersComboBox.Items.Count > 1)
//				LayersComboBox.SelectedIndex = 1;

			this.progressBar1.Value  =95;

			//			/////��õ�ǰMapControl�򿪵ĵ�ͼ�ĵ���·��
			//			string sFilePath  = this.axMapControl.DocumentFilename;
			//��ʼ��page����

            //axMapControl.Refresh();

			this.progressBar1.Value  =100;

			this.progressBar1.Visible=false;
		}

//		private void InitcmbImageRaster()
//		{
//			if (this.objectDataAccess!=null)
//			{
//				string selectCMD1="select name from GDB_USERMETADATA where DATASETTYPE ='16'";
//				System.Data.DataRowCollection m_DataRowCollection=objectDataAccess.getDataRowsByQueryString(selectCMD1);
//				for(int i=0;i<m_DataRowCollection.Count ;i++)
//				{					
//					this.cmbImageRaster.Items.Add(m_DataRowCollection[i][0].ToString());
//				}
//				if(this.cmbImageRaster.Items.Count >0)
//				{
//					this.cmbImageRaster.SelectedIndex=0;
//				}
//				
//			}
//		}
        /// <summary>
        /// ����ر��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void frmMapControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//Release COM objects and shut down the AoInitilaize object,��ֹ�ڴ��������
//			if (this.cmbImageRaster!=null)
//			{
//				this.cmbImageRaster.Visible=false;
//			}
			if (this.LayersComboBox!=null)
			{
				this.LayersComboBox.Visible=false;
			}
			ESRI.ArcGIS.Utility.COMSupport.AOUninitialize.Shutdown();
			if(this.CloseEvent  != null)
			{
				CloseEvent();
			}
			m_AoInitialize.Shutdown();
            
		
		}

		/// <summary>
		/// �˵���ʽ��ʾ��������������ʾ
		/// @return void
		/// </summary> 
		private void AddToolBarToDockWindow()
		{
			try
			{
				m_BarManager = new SuperToolBar.ToolBarManager(this,this, this.officeMenus1);
				//this.PagetoolBar.Text = "��ͼ������";
				//�˵�����ʽ���Office�˵���ʽ
				this.officeMenus1.Start(this);	
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}


		#endregion

		#region �˵�

		#region �ļ�

		/// <summary>
		/// �˵������ļ�������ר���ͼ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem7_Click(object sender, EventArgs e)
		{
			try
			{
				//this.mapFuntion.OpenDoc();
				//Open a file dialog for opening map documents
				this.openFileDialog1 = new OpenFileDialog();
				openFileDialog1.Title = "�򿪵�ͼ�ĵ�";
				openFileDialog1.Filter = "Map Documents (*.mxd)|*.mxd";
				openFileDialog1.ShowDialog();

				// Exit if no map document is selected
				sFilePath = openFileDialog1.FileName;
				if (sFilePath == "")
				{
					return;
				}
				this.LoadFile(sFilePath);	
				this.addItemToLayersComboBox();
			}
			catch
			{
			}
		
		}

		/// <summary>
		/// �˵������ļ���������ļ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem8_Click(object sender, EventArgs e)
		{
			try
			{
				//this.axMapControl.ClearLayers();
				this.openFileDialog1 = new OpenFileDialog();				//���ļ��Ի���
				openFileDialog1.Title = "����ļ�";
//				openFileDialog1.DefaultExt = ".lyr";
				openFileDialog1.Filter = "shapefile (*.shp)|*.shp|Image (*.img)|*.img|TIFF (*.tif)|*.tif|All Support Files (*.shp;*.img;*.tif)|*.shp;*.img;*.tif";
				//openFileDialog1.Filter = "Layer Documents(*.lyr)|*.lyr";
			
				if(openFileDialog1.ShowDialog() != DialogResult.OK) // �����ok���Ժ�
					return;		

				string  str_filepath;
				str_filepath = openFileDialog1.FileName;			// �õ��򿪵��ļ�·��
				
				if(str_filepath != null )
				{
					FileInfo FileInfo = new FileInfo(str_filepath);
					string str_filename = FileInfo.Name;
					string str_fileformat = FileInfo.Extension;
					string path = FileInfo.DirectoryName;
	
					
					// ���ļ�����mapcontrol
					//shapefile�ļ�
					if(str_fileformat == ".shp")
					{
						this.axMapControl.AddShapeFile(path,str_filename) ;
					}
					else if(str_fileformat == ".img" || str_fileformat == ".tif")
					{
						// �����ļ�Rasterͼ��
						IRasterLayer RasterLayer;
						RasterLayer = new RasterLayerClass();		
						RasterLayer.CreateFromFilePath(str_filepath);		// ���ļ�·������

						if(RasterLayer != null)
						{
							RasterLayer.Name = str_filename ;				//����Rasterͼ��
							// ��Rasterͼ�����Scene
							this.axMapControl.AddLayer(RasterLayer,0);
							// �����ػ�SceneViewer
                            //this.axMapControl.Refresh();
						}
						else
						{
							MessageBox.Show("����tif���ݳ���");
						}
					}
					else if(str_fileformat == ".lyr")
					{
						this.axMapControl.AddLayerFromFile(str_filepath,0);
						
					}
					
					// ˢ��
					//ͼ���б�仯
					this.addItemToLayersComboBox();											 //��ͼ�����ƶ���ͼ���б�	
					curLayer= this.mapFuntion.getCurrentFeatureLayer(0);
					this.axMapControl.Refresh();

					//pageLayoutControl �� mapControl ����һ��
					//this.CopyAndOverwriteMap();
				}
				else
				{
					MessageBox.Show("�������ݳ���");
				}
			}
			catch (Exception ee) 
			{
				Debug.WriteLine(ee.Message);
			}
		
		}

		/// <summary>
		/// �˵������ļ����������ͼ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem9_Click(object sender, EventArgs e)
		{
			try
			{

				//��õ�ǰMapControl�򿪵ĵ�ͼ�ĵ���·��
				string sFilePath  = this.axMapControl.DocumentFilename; 
				//Create a new map document
				m_MapDocument = new MapDocumentClass();
				//Open the map document selected
				m_MapDocument.Open(sFilePath,"");

				//��ȡ��ǰ��ͼ�ؼ��ĵ�ͼ
				if (m_MapDocument.get_IsReadOnly(m_MapDocument.DocumentFilename) == true) 
				{
					MessageBox.Show("�����ͼ�ļ���ֻ����");		//Check that the document is not read only
					return;
				}
				
				m_MapDocument.ReplaceContents((IMxdContents) axMapControl.Object);
				m_MapDocument.SetActiveView(axMapControl.ActiveView);
				
				//Save with the current relative path setting
				m_MapDocument.Save(m_MapDocument.UsesRelativePaths,true);
				MessageBox.Show("�ɹ������ͼ�ļ�");
			}
			catch(Exception ee)
			{
				Debug.WriteLine("--�����ͼ����:--"+ee.Message);
			}
		
		}

		/// <summary>
		/// �˵������ļ��������Ϊ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem10_Click(object sender, EventArgs e)
		{
			try
			{	
				this.mapFuntion.SaveAsDoc();
			}
			catch
			{
			}
		
		}
		/// <summary>
		/// �˵������ļ������˳�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem11_Click(object sender, EventArgs e)
		{
			//this.m_TOCControl.SetBuddyControl(null);
			frmMapControl.ActiveForm.Close();
		}

		/// <summary>
		/// ����������mapdocument�ļ�
		/// </summary>
		/// <param name="sFilePath"></param>
		private void OpenDocument(string sFilePath)
		{
			//Create a new map document
			m_MapDocument = new MapDocumentClass();
			//Open the map document selected
			m_MapDocument.Open(sFilePath,"");
			 
			//Set the PageLayoutControl page layout to the map document page layout
			//axPageLayoutControl.PageLayout = m_MapDocument.PageLayout;
		}
        
		/// <summary>
		/// ���������ؼ��������ļ�
		/// </summary>
		/// <param name="path"></param>
		public void LoadFile(string path)
		{
			//�����ͼ�ļ��������ͼ�ؼ�
			if (axMapControl.CheckMxFile(path))
			{
				axMapControl.LoadMxFile(path,Type.Missing,Type.Missing);
				//axPageLayoutControl.LoadMxFile(sFilePath,Type.Missing);
			}

			//Open document
			OpenDocument((path));	
		}

		/// <summary>
		/// ����ͼ�����Ƶ�ͼ��������Ͽ�
		/// </summary>	
		public void addItemToLayersComboBox()
		{
			try
			{
				this.LayersComboBox.Items.Clear();
				LayersComboBox.Items.Add("��ѡ��ͼ��");
				String [] lnames=this.mapFuntion.getLayerNames();
				for(int i=0;i<lnames.Length;i++)
				{
					LayersComboBox.Items.Add(lnames[i]);
				}
				curLayer= this.mapFuntion.getCurrentFeatureLayer(1);
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}

		#endregion

		#region ��ͼ

		/// <summary>
		/// �˵�������ͼ�����Ŵ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem12_Click(object sender, EventArgs e)
		{
			this.mapFuntion.ZoomIn();
		
		}
   
		/// <summary>
		///  �˵�������ͼ������С
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem13_Click(object sender, EventArgs e)
		{
		
			this.mapFuntion.ZoomOut();
		}
		
		/// <summary>
		///  �˵�������ͼ��������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem14_Click(object sender, EventArgs e)
		{
		
			this.mapFuntion.Pan();
		}

		/// <summary>
		///  �˵�������ͼ������������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem15_Click(object sender, EventArgs e)
		{
			this.mapFuntion.ZoomPan();
		}

		/// <summary>
		///  �˵�������ͼ����ȫͼ��ʾ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem16_Click(object sender, EventArgs e)
		{
			this.mapFuntion.FullExtent();
		}

		/// <summary>
		///  �˵�������ͼ����ˢ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem17_Click(object sender, EventArgs e)
		{
			this.mapFuntion.Refresh();
		}
       
		/// <summary>
		/// �˵�������ͼ������ѡ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem40_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.isDianXuan = true; 
				this.isKuangXuan = false;  
				this.isDuobxXuan = false;
				this.isYuanXuan = false;
				//��õ�ǰ�������Ĺ��ߣ�ʹ����ʱ��Ч
				ITool iTool = this.axMapControl.CurrentTool;
				if(iTool!=null)
					this.axMapControl.CurrentTool = this.mapFuntion.pNullTool;

				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("������ѡ��������"+ee.Message);
			}
		
		}

		/// <summary>
		///  �˵�������ͼ������ѡ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem37_Click(object sender, EventArgs e)
		{
			try
			{
				this.isDianXuan = false;
				this.isKuangXuan = true;  
				this.isDuobxXuan = false;
				this.isYuanXuan = false;
				//��õ�ǰ�������Ĺ��ߣ�ʹ����ʱ��Ч
				ITool iTool = this.axMapControl.CurrentTool;
				if(iTool!=null)
					this.axMapControl.CurrentTool = this.mapFuntion.pNullTool;

				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("������ѡ��������"+ee.Message);
			}
		}

		/// <summary>
		///  �˵�������ͼ����Բѡ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem38_Click(object sender, EventArgs e)
		{
			try
			{
				this.isDianXuan = false;
				this.isYuanXuan = true;  
				this.isDuobxXuan = false;
				this.isKuangXuan = false;
				//��õ�ǰ�������Ĺ��ߣ�ʹ����ʱ��Ч
				ITool iTool = this.axMapControl.CurrentTool;
				if(iTool!=null)
					this.axMapControl.CurrentTool = this.mapFuntion.pNullTool;
				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("����Բѡ��������"+ee.Message);
			}
		
		}

		/// <summary>
		///  �˵�������ͼ���������ѡ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem39_Click(object sender, EventArgs e)
		{
			try
			{
				this.isDuobxXuan = true;  
				this.isKuangXuan  = false;
				this.isYuanXuan = false;
				this.isDianXuan = false;
				//��õ�ǰ�������Ĺ��ߣ�ʹ����ʱ��Ч
				ITool iTool = this.axMapControl.CurrentTool;
				if(iTool!=null)
					this.axMapControl.CurrentTool = this.mapFuntion.pNullTool;
				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("���������ѡ��������"+ee.Message);
			}
		}

		/// <summary>
		///  �˵�������ͼ���������ѡ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem19_Click(object sender, EventArgs e)
		{
			//��ղ�ѯ��ѡҪ��
			this.axMapControl.Map.ClearSelection();
			this.axMapControl.ActiveView.Refresh();
			arryOfSelFea.Clear();
		
		}

		/// <summary>
		///  �˵�������ͼ�����鿴����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem20_Click(object sender, EventArgs e)
		{
			try
			{
				//��õ�ǰ�������Ĺ��ߣ�ʹ����ʱ��Ч
				ITool iTool = this.axMapControl.CurrentTool;
				if(iTool!=null)
					this.axMapControl.CurrentTool = this.mapFuntion.pNullTool;

				if (this.identifyForm == null || identifyForm.IsDisposed == true)
				{
					identifyForm = new IDentifyDialog(this.axMapControl,arryOfSelFea);
					identifyForm.CloseIdentifyFormEvent +=new MainFrame.Form.MapAttribute.IDentifyDialog.CloseIdentifyFormEventHandler(identifyForm_CloseIdentifyFormEvent);
					identifyForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1
				}
				this.identifyForm.Show();
				this.isAttribute = true; 
						
				//�������Բ�ѯ����
				this.identifyForm.setSelFeature(arryOfSelFea,curLayer);	
				//�����ʽ
				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerIdentify;  
				
			}
			catch (Exception e1)
			{
				Debug.WriteLine("�鿴���Գ���"+e1.Message );
			}
		
		}

		/// <summary>
		/// ��������ͼ�����������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem45_Click(object sender, System.EventArgs e)
		{
			this.isMeasureLength = true;
			if(this.isMeasureLength)
			{
				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerPencil;
			}
			
		}

		/// <summary>
		/// �˵�������ͼ�����������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem46_Click(object sender, System.EventArgs e)
		{
			this.isMeasureArea = true;
			if(this.isMeasureArea)
			{
				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerPencil;
			}			
			
			//�ռ�ο�
			ISpatialReference isr = axMapControl.SpatialReference;
		
		}

		/// <summary>
		/// ���Դ���ر��¼�,�����ʽ�ص�ȱʡֵ 
		/// </summary>
		public void identifyForm_CloseIdentifyFormEvent()
		{
			//Debug.Write("�ر�") ;
			this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;  
		}

		#endregion

		#region �༭

		/// <summary>
		/// �˵������༭������ӵ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem21_Click(object sender, EventArgs e)
		{
			if(this.m_FeatureClass != null)
			{
				switch(this.m_FeatureClass.ShapeType)
				{
					case esriGeometryType.esriGeometryPoint:
						this.isPoint  = true;
						this.isPolyline = false;
						this.isPolygon = false;
						break;
					case esriGeometryType.esriGeometryPolyline:
						this.isPoint  = false;
						this.isPolyline = true;
						this.isPolygon = false;
						break;
					case esriGeometryType.esriGeometryPolygon:
						this.isPoint  = false;
						this.isPolyline = false;
						this.isPolygon = true;
						break;			
				}
			}
				
		}

		/// <summary>
		/// �˵������༭������ʼ�༭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem22_Click(object sender, EventArgs e)
		{
			string FeatureClassName = this.LayersComboBox.SelectedItem.ToString();
			OpenFeatureClass(FeatureClassName);
			this.EndEditing = true;
			//��ʼ�༭
			m_WorkspaceEdit.StartEditing(true);
			m_WorkspaceEdit.StartEditOperation();
		
		}

		/// <summary>
		///  �˵������༭���������༭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem26_Click(object sender, System.EventArgs e)
		{
			this.EndEditing = false;
			this.isPoint = false;
			this.isPolygon = false;
			this.isPolyline = false;
			this.isDelete = false;
			axMapControl.CurrentTool = this.mapFuntion.pNullTool;
			this.m_WorkspaceEdit.StopEditOperation();
			this.m_WorkspaceEdit.StopEditing(true);

		
		}

		/// <summary>
		/// �˵������༭����ɾ��Ҫ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem23_Click(object sender, EventArgs e)
		{
			//����ѡ��Ҫ�ع���
			try
			{
//				ICommand pControlsSelectFeatures;
//				pControlsSelectFeatures=  new ControlsSelectFeaturesToolClass();
//				pControlsSelectFeatures.OnCreate(axMapControl.GetOcx());
//				pControlsSelectFeatures.OnClick();
//				axMapControl.CurrentTool = pControlsSelectFeatures as ITool ;

				this.isDianXuan = true; 
				this.isKuangXuan = false;  
				this.isDuobxXuan = false;
				this.isYuanXuan = false;
				//��õ�ǰ�������Ĺ��ߣ�ʹ����ʱ��Ч
				ITool iTool = this.axMapControl.CurrentTool;
				if(iTool!=null)
					this.axMapControl.CurrentTool = this.mapFuntion.pNullTool;

				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
				this.isDelete = true;
			}
			catch(Exception ee)
			{
				Debug.WriteLine("BringToFront has errors:"+ee.Message);
			}	
		
		}


		/// <summary>
		/// ע�Ǳ༭�ĺ�������featureclass
		/// </summary>
		/// <param name="FeatureClassName"></param>
		public  void OpenFeatureClass(string FeatureClassName)
		{

			//�򿪹����ռ�
			//			IFeatureWorkspace m_FeatureWorkspace;
			//			IWorkspaceFactory m_WSF = new AccessWorkspaceFactoryClass();
			//			IWorkspace m_WS = m_WSF.OpenFromFile("c:\\data\\test.mdb",0);
			//			m_FeatureWorkspace = m_WS as IFeatureWorkspace;

			//��sde���ݿ�
			IFeatureWorkspace m_FeatureWorkspace ;
			IWorkspaceFactory m_WSF = new SdeWorkspaceFactoryClass();
			//����sde����
			IPropertySet  m_PropertySet = new PropertySetClass();
			m_PropertySet.SetProperty("Server",objectDataAccess.Server);
			m_PropertySet.SetProperty("Instance",objectDataAccess.Service );
			m_PropertySet.SetProperty("Database",objectDataAccess.DataBase);   
			m_PropertySet.SetProperty("user",objectDataAccess.UserID);
			m_PropertySet.SetProperty("password",objectDataAccess.Password);
			m_PropertySet.SetProperty("version","sde.DEFAULT");

			IWorkspace m_WS = m_WSF.Open(m_PropertySet,0) ;
			m_FeatureWorkspace = m_WS as IFeatureWorkspace;

			//��FeatureClass
			//m_FeatureClass = m_FeatureWorkspace.OpenFeatureClass(FeatureClassName) as IFeatureClass;
			m_FeatureClass = this.mapFuntion.getFeatureLayerByName(FeatureClassName).FeatureClass ;
//			m_FeatureLayer = new FeatureLayerClass();
//			m_FeatureLayer.FeatureClass = m_FeatureClass;
//			m_FeatureLayer.Name = m_FeatureClass.AliasName;
//			//m_FeatureLayer.SpatialReference = this.axMapControl.ActiveView.FocusMap.SpatialReference;
         
//			//�ڿؼ������featureclass
//			IMap m_Map;
//			m_Map = this.axMapControl.Map;
//			m_Map.AddLayer(m_FeatureLayer);

			//����༭�����ռ�
			m_WorkspaceEdit = m_FeatureWorkspace as IWorkspaceEdit;
			this.axMapControl.ActiveView.Refresh();
			this.axTOCControl.ActiveView.Refresh();
		}

		#endregion

		#region �ռ����

		/// <summary>
		/// �˵������ռ������������������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem27_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_BufferForm == null || m_BufferForm.IsDisposed == true)
				{
					m_BufferForm = new BufferForm(this.axMapControl); 
					m_BufferForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_BufferForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("��������������"+e1.Message );
			}	
		
		}

		/// <summary>
		///  �˵������ռ�����������÷���
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem28_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_OverlayForm == null || m_OverlayForm.IsDisposed == true)
				{
					m_OverlayForm = new OverlayForm(this.axMapControl); 
					m_OverlayForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_OverlayForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("���÷�������"+e1.Message );
			}	
		
		}

		/// <summary>
		/// �˵������ռ���������ռ��ѯ�������ٲ�ѯ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem43_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (m_QuikFindForm == null || m_QuikFindForm.IsDisposed == true)
				{
					m_QuikFindForm = new QuikFindForm(this);
					m_QuikFindForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_QuikFindForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("���ٲ�ѯ��"+e1.Message );
			}	
		
		
		}

		/// <summary>
		/// �˵������ռ���������ռ��ѯ����������ѯ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem44_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (m_SelectByAttributeForm == null || m_SelectByAttributeForm.IsDisposed == true)
				{
					m_SelectByAttributeForm = new  SelectByAttributeForm(this.axMapControl);
					m_SelectByAttributeForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_SelectByAttributeForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("������ѯ��"+e1.Message );
			}	
		
		}

		#endregion
		
		#region ר���ͼ����

		/// <summary>
		/// �˵�����ר���ͼ����������һ���ŷ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem29_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_SimpleRenderForm == null || m_SimpleRenderForm.IsDisposed == true)
				{
					m_SimpleRenderForm = new SimpleRenderForm(this.axMapControl,this.axTOCControl); 
					m_SimpleRenderForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_SimpleRenderForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("��һ���ŷ�����"+e1.Message );
			}	
		}

		/// <summary>
		/// �˵�����ר���ͼ��������������ŷ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem30_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_UniqueValueRendererForm == null || m_UniqueValueRendererForm.IsDisposed == true)
				{
					m_UniqueValueRendererForm = new UniqueValueRendererForm(this.axMapControl,this.axTOCControl); 
					m_UniqueValueRendererForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_UniqueValueRendererForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("������ŷ�����"+e1.Message );
			}
		
		}

		/// <summary>
		/// �˵�����ר���ͼ���������ּ�ɫ�ʷ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem31_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_ClassBreakRendererForm == null || m_ClassBreakRendererForm.IsDisposed == true)
				{
					m_ClassBreakRendererForm = new ClassBreakRendererForm(this.axMapControl,this.axTOCControl); 
					m_ClassBreakRendererForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_ClassBreakRendererForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("�ּ�ɫ�ʷ�����"+e1.Message );
			}

		
		}

		/// <summary>
		/// �˵�����ר���ͼ������������ͳ��ͼ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem32_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_ChartRenderForm == null || m_ChartRenderForm.IsDisposed == true)
				{
					m_ChartRenderForm = new ChartRenderForm(this.axMapControl,this.axTOCControl); 
					m_ChartRenderForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_ChartRenderForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("����ͳ��ͼ�����"+e1.Message );
			}
		
		}

		/// <summary>
		/// �˵�����ר���ͼ�����������ʷ��ŷ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem33_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_ProportionalSymbolRendererForm == null || m_ProportionalSymbolRendererForm.IsDisposed == true)
				{
					m_ProportionalSymbolRendererForm = new ProportionalSymbolRendererForm(this.axMapControl,this.axTOCControl); 
					m_ProportionalSymbolRendererForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_ProportionalSymbolRendererForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("���ʷ��ŷ�����"+e1.Message );
			}
		}
		

		/// <summary>
		/// �˵�����ר���ͼ����������ֵ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem34_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.m_DotDensityRendererForm== null || m_DotDensityRendererForm.IsDisposed == true)
				{
					m_DotDensityRendererForm = new DotDensityRendererForm(this.axMapControl,this.axTOCControl); 
					m_DotDensityRendererForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.m_DotDensityRendererForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("��ֵ������"+e1.Message );
			}
		
		}

		#endregion

		#region ��ӡ

		/// <summary>
		/// �˵�������ӡ������ӡ��ǰҳ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem1_Click(object sender, System.EventArgs e)
		{		

			//��õ�ǰMapControl�򿪵ĵ�ͼ�ĵ���·��
			string sFilePath  = this.axMapControl.DocumentFilename; 
			//Create a new map document
			m_MapDocument = new MapDocumentClass();
			//Open the map document selected
			m_MapDocument.Open(sFilePath,"");

			if (frmPageLayoutControl == null || frmPageLayoutControl.IsDisposed == true)
			{
                frmPageLayoutControl = new Form.PageLayoutControl.frmPageLayoutControl();
				frmPageLayoutControl.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

			}
			this.axPageLayoutControl =frmPageLayoutControl.axPageLayoutControl;
			//frmPageLayoutControl.axPageLayoutControl.PageLayout = m_MapDocument.PageLayout;
			//��page���ݲ���
			PassToPage();
			//ͨ��copy����������ͼ����һ��
			this.CopyAndOverwritePage();
			this.frmPageLayoutControl.Show();
  
		}

		/// <summary>
		/// �˵�������ӡ���������ӡ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (frmPageLayoutControl == null || frmPageLayoutControl.IsDisposed == true)
				{
                    frmPageLayoutControl = new Form.PageLayoutControl.frmPageLayoutControl();
					frmPageLayoutControl.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.axPageLayoutControl =frmPageLayoutControl.axPageLayoutControl;

				if (this.regionalPrintForm== null || this.regionalPrintForm.IsDisposed == true)
				{
					regionalPrintForm =  new regionalprint(this.axPageLayoutControl,this.axMapControl,this.m_MapDocument,this.mapFuntion);
					regionalPrintForm.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

				}
				this.regionalPrintForm.Show();
			}
			catch (Exception e1)
			{
				Debug.WriteLine("�������ӡ����"+e1.Message );
			}
		
		}

		/// <summary>
		/// ��page���崫�����
		/// </summary>
		public void PassToPage()
		{
		    this.mapFuntion = this.InitMapFunction(this.axMapControl,this.axPageLayoutControl);
			frmPageLayoutControl.mapFuntion = this.mapFuntion;
            frmPageLayoutControl.axMapControl = this.axMapControl;
			frmPageLayoutControl.m_MapDocument = this.m_MapDocument;
		}

		/// <summary>
		/// ����������ʼ��MapFunction
		/// </summary>
		/// <param name="p_axMapControl"></param>
		/// <param name="p_axPageLayoutControl"></param>
		/// <returns></returns>
		private Function.MapFunction InitMapFunction(AxMapControl p_axMapControl,AxPageLayoutControl p_axPageLayoutControl)
		{
			Function.MapFunction m_mapFuntion;
			m_mapFuntion= new MapFunction(this.axMapControl,this.axPageLayoutControl);
			return m_mapFuntion;
		}

		#endregion

		#region ����

		/// <summary>
		/// �˵�����������������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem35_Click(object sender, EventArgs e)
		{
		
		}

		/// <summary>
		/// �˵����������������ڡ���
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem36_Click(object sender, EventArgs e)
		{
		
		}

		#endregion

		#endregion

		#region MapControl����¼�

		/// <summary>
		/// MapControl��굥���¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
		{
			try
			{
				//����¼�
				if(e.button == 1)
				{
					m_IPointStart.X=e.mapX ;
					m_IPointStart.Y=e.mapY  ;

					#region �����ӡ

                    //if (this.regionalPrintForm != null && this.regionalPrintForm.IsDisposed != true)
                    //{
                    //    this.indexRY = this.regionalPrintForm.index;
				
                    //    if(indexRY )
                    //    {
                    //        //�������
                    //        this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;      
                    //        //						//��ʼ����
                    //        //						IPoint pt = new PointClass();
                    //        //						int x = e.x;
                    //        //						int y = e.y;
                    //        //						pt = axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    //        //						//���û�п�ʼ���ߣ�����һ��feedback���󣬲���start������ʼ
                    //        //						if(this.m_NewEnvelopeFeedback == null)
                    //        //						{
                    //        //							this.m_NewEnvelopeFeedback = new NewEnvelopeFeedback();
                    //        //							this.m_NewEnvelopeFeedback.Display = this.axMapControl.ActiveView.ScreenDisplay;
                    //        //							this.m_NewEnvelopeFeedback.Start(pt);
                    //        //							if(e.button ==1)
                    //        //							{
                    //        //								this.m_NewEnvelopeFeedback.Constraint = esriEnvelopeConstraints.esriEnvelopeConstraintsNone;
                    //        //							} 
                    //        //						}
                    //        this.EndEditing = true;
                    //        this.isPolygon = true;
                    //    }
                    //}

					#endregion

					#region ����

					//�����������
					if(this.isMeasureLength)
					{
						//���������ʽ
						//					this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerPencil; 
						IGeometry  iGeometry =  this.axMapControl.TrackLine(); 
						string length = this.mapFuntion.MeasureLength(iGeometry); 
						MessageBox.Show("����ľ���Ϊ" + length  +" "+"��"); 
						this.isMeasureLength = false;
					}
					//�����������
					if(this.isMeasureArea)
					{
						//���������ʽ
						//					this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerPencil; 
						IGeometry  iGeometry =  this.axMapControl.TrackPolygon(); 
						string area = this.mapFuntion.MeasureArea(iGeometry);
						double f_area = Convert.ToDouble(area);
						if(f_area < 0)
						{
							f_area = 0 - f_area;
							area = f_area.ToString();
						}

						MessageBox.Show("��������Ϊ" + area +" "+"ƽ����"); 
						this.isMeasureArea = false;
					}

					#endregion

					#region ѡ��Ҫ��

					#region Ĭ��Ϊ��ѡ��ʽ
					if (this.isDianXuan)
					{
						//Create a point and get the IPoint interface
						IPoint point = new PointClass();
						//Set points coordinates
						point.PutCoords(e.mapX, e.mapY);

						//QI for ITopologicalOperator interface through IPoint interface
						ITopologicalOperator topologicalOperator = (ITopologicalOperator) point;
						//Create a polygon by buffering the point and get the IPolygon interface
						double distance;
						distance = ComputeDistance(4);
						IPolygon polygon = (IPolygon) topologicalOperator.Buffer(distance);
						//QI for IRelationalOperator interface through IPolygon interface
						//IRelationalOperator relationalOperator = (IRelationalOperator) polygon;
						//��ȡ��ѡҪ��
						//curLayer=this.mapFuntion.getCurrentFeatureLayer(1);

						//arryOfSelFea =this.mapFuntion.selectFeatures(polygon, curLayer);	
						arryOfSelFea =this.GetClosestFeature(polygon, curLayer);
						//�������Բ�ѯ����
						if(this.identifyForm!=null&&this.identifyForm.IsDisposed == false)
						{
							this.identifyForm.setSelFeature(arryOfSelFea,curLayer);	
						}	

					}
						#endregion

						//�����ǵ�ѡ��ʽ
					else
					{
						#region ���¶����ѡ

						if(this.isDuobxXuan)
						{
							//����ϴ���ѡ
							this.axMapControl.Map.ClearSelection();
							arryOfSelFea.Clear();
							//��ȡ��ѡҪ��
							arryOfSelFea =this.mapFuntion.selectFeatures(this.axMapControl.TrackPolygon(), curLayer);		
							//�������Ա�
							if(this.identifyForm!=null && identifyForm.IsDisposed == false)
							{
								//�������Բ�ѯ����
								this.identifyForm.setSelFeature(arryOfSelFea,curLayer);
							}
							isDuobxXuan = false;
							isDianXuan = true;

						}
				
						#endregion

						#region ����Բѡ

						if(this.isYuanXuan)
						{
							//����ϴ���ѡ
							this.axMapControl.Map.ClearSelection();
							arryOfSelFea.Clear();
							//��ȡ��ѡҪ��
							arryOfSelFea =this.mapFuntion.selectFeatures(this.axMapControl.TrackCircle(), curLayer);		
							//�������Ա�
							if(this.identifyForm!=null && identifyForm.IsDisposed == false)
							{
								//�������Բ�ѯ����
								this.identifyForm.setSelFeature(arryOfSelFea,curLayer);
							}
							isYuanXuan = false;
							isDianXuan = true;
						}		
						#endregion

						#region ���¿�ѡ

						if(this.isKuangXuan)
						{
							//����ϴ���ѡ
							this.axMapControl.Map.ClearSelection();
							arryOfSelFea.Clear();
							//��ȡ��ѡҪ��
							arryOfSelFea =this.mapFuntion.selectFeatures(this.axMapControl.TrackRectangle(), curLayer);		
							//�������Ա�
							if(this.identifyForm!=null && identifyForm.IsDisposed == false)
							{
								//�������Բ�ѯ����
								this.identifyForm.setSelFeature(arryOfSelFea,curLayer);
							}
							isKuangXuan = false;
							isDianXuan = true;
				
						}
					}

					#endregion

					#endregion

					#region �༭

					if(EndEditing )
					{
						//�༭��ӵ�
						#region �༭��ӵ�
						if(this.isPoint)
						{

							//���������ʽ
							this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair; 
							//						//��ʼ�༭
							//						m_WorkspaceEdit.StartEditing(true);
							//						m_WorkspaceEdit.StartEditOperation();
							//��ӵ㣬���ü�����Ϣ
							IFeature m_Feature = null;
							m_Feature = this.m_FeatureClass.CreateFeature();
							IPoint pt = new PointClass();
							int x = e.x;
							int y = e.y;
							pt = axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
							//					pt.PutCoords(e.mapX,e.mapY);
							m_Feature.Shape = pt as IGeometry;
				
							//����Ҫ�أ�ֹͣ�༭
							m_Feature.Store();
							//						m_WorkspaceEdit.StopEditOperation();
							//						m_WorkspaceEdit.StopEditing(true);
							this.axMapControl.ActiveView.Refresh();
	
						}
						#endregion

						//�༭�����
						#region �༭�����
						if(this.isPolyline)
						{
						

							//�������
							this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair; 
							//��ʼ����
							IPoint pt = new PointClass();
							int x = e.x;
							int y = e.y;
							pt = axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
							//���û�п�ʼ���ߣ�����һ��feedback���󣬲���start������ʼ
							if(this.m_NewLineFeedback == null)
							{
								this.m_NewLineFeedback = new NewLineFeedbackClass();
								this.m_NewLineFeedback.Display = this.axMapControl.ActiveView.ScreenDisplay;
								this.m_NewLineFeedback.Start(pt);
							}
								//����Ѿ�����������µ�
							else
							{
								this.m_NewLineFeedback.AddPoint(pt);
							}

						}
						#endregion

						//�༭�����
						#region �༭�����
						if(this.isPolygon)
						{
						

							//�������
							this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;      
							//��ʼ����
							IPoint pt = new PointClass();
							int x = e.x;
							int y = e.y;
							pt = axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
							//���û�п�ʼ���ߣ�����һ��feedback���󣬲���start������ʼ
							if(this.m_NewPolygonFeedback == null)
							{
								this.m_NewPolygonFeedback = new NewPolygonFeedback();
								this.m_NewPolygonFeedback.Display = this.axMapControl.ActiveView.ScreenDisplay;
								this.m_NewPolygonFeedback.Start(pt);
							}
								//����Ѿ�����������µ�
							else
							{
								this.m_NewPolygonFeedback.AddPoint(pt);
							}

						
						}
						#endregion
					}

					#endregion

				}
 
				//�Ҽ��¼�
				if(e.button == 2)
				{
					
					//�չ���
					ITool pNullTool = (ITool) new Controls3DAnalystContourTool ();
					this.axMapControl.CurrentTool = pNullTool;
//						this.axMapControl.CurrentTool = null;
					EndEditing = true;
					this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair; 

					
					
				}

				
			}
			catch (Exception e1)
			{
				Debug.WriteLine("----ѡ��Ҫ�أ�axMapControl��굥���¼� has errors----"+e1.Message);
			}
	
		}

		private ArrayList GetClosestFeature(IGeometry selectGeometry,FeatureLayer fLayer)
		{

			this.axMapControl.ActiveView.FocusMap.ClearSelection();
			IFeatureSelection featureSelection;
			featureSelection = fLayer as IFeatureSelection;

			//this.arryOfSelFea.Clear();
			string[] layerNames = null;
			//int num = this.axMapControl.ActiveView.FocusMap.LayerCount;
			layerNames = this.mapFuntion.getLayerNames();
            int num = layerNames.Length;
			string selName = null;

			ArrayList pSelected = new ArrayList();
			FeatureLayer curSelLayer = null;
			IFeatureLayer templayer = null;
			
			IEnvelope pSrchEnv = selectGeometry.Envelope;
			UID pUID = new ESRI.ArcGIS.esriSystem.UIDClass();
			pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";

			for(int i = 0;i< num ;i++)
			{
				selName = layerNames[i];
                templayer = this.mapFuntion.getFeatureLayerByName(selName);
				//templayer =(IFeatureLayer)this.axMapControl.ActiveView.FocusMap.get_Layer(i);
				curSelLayer = templayer as FeatureLayer;
				IGeoFeatureLayer pGeoLayer = curSelLayer as IGeoFeatureLayer;
				if(pGeoLayer.Selectable && pGeoLayer.FeatureClass != null && pGeoLayer.Visible == true)				
				{
					IIdentify2 pID;
					pID = pGeoLayer as IIdentify2;
					IArray pIDArray;
					pIDArray  = pID.Identify(pSrchEnv, null);

					IFeatureIdentifyObj ipFeatIdObj;
					IRowIdentifyObject pRowObj;
					IFeature pFeature;
					if(pIDArray != null)
					{
						ArrayList arrayofFeaInOneLayer = new ArrayList();
						arrayofFeaInOneLayer.Add(layerNames[i]);
						for(int j = 0; j< pIDArray.Count;j++)
						{
							if(pIDArray.get_Element(j) is IFeatureIdentifyObj)
							{
								ipFeatIdObj = (IFeatureIdentifyObj)pIDArray.get_Element(j);
								pRowObj = (IRowIdentifyObject)ipFeatIdObj;
								pFeature =(IFeature)pRowObj.Row;
								if(pGeoLayer.Name == fLayer.Name)
								{
									featureSelection.Add(pFeature);
								}
                                arrayofFeaInOneLayer.Add(pFeature);
							}																											  
						}
						pSelected.Add(arrayofFeaInOneLayer);
						pIDArray.RemoveAll();
					}                   					
				}                 
			}
			this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,fLayer,null);
            return pSelected;
		}
		
		/// <summary>
		/// ��������������ѻ������
		/// </summary>
		/// <returns></returns>
		private double ComputeDistance(double pixelunits)
		{
			double realWorldDisplayExtent;
			double sizeofOnePixel;
//			tagRECT DeviceFrame = this.axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.DeviceFrame;
//            pixelExtent = DeviceFrame.right-DeviceFrame.left;
			IPoint p1 = this.axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperLeft;
			IPoint p2 = this.axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperRight;
			int x1, x2, y1, y2;
			this.axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(p1, out x1, out y1);
			this.axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(p2, out x2, out y2);
			double pixelExtent = x2 - x1;
            realWorldDisplayExtent = this.axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            sizeofOnePixel = realWorldDisplayExtent/pixelExtent;
			double distance = pixelunits * sizeofOnePixel;
			return distance;
		}
        
		/// <summary>
		///  �����MapControl���ƶ��¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
		{
			#region ״̬��

			double doubleOfScale = this.axMapControl.MapScale;
			int intOfScale = (int)doubleOfScale;
			string strOfScale = intOfScale.ToString(); 

			//����
			this.ZuobiaoPanel.Text ="����: "+ "X:"+e.mapX.ToString(".00") + "; Y:" + e.mapY.ToString(".00")+" "+sMapUnits;
			//��ʾ����
			this.BiliPanel.Text = "��ʾ����: 1:"+strOfScale ;

			#endregion

			#region �༭

			if(this.EndEditing)
			{
				//�༭����
				if(this.isPoint)
				{
					//�������
					this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair; 							
				}

				//�༭����
				if(this.isPolyline)
				{
					//�������
					this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair; 
					IPoint pt = new PointClass();
					int x = e.x;
					int y = e.y;
					pt = axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
					if(this.m_NewLineFeedback != null)
					{
						this.m_NewLineFeedback.MoveTo(pt);
					}
				}
									
				//�༭����
				if(this.isPolygon)
				{
					//�������
					this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair; 
					IPoint pt = new PointClass();
					int x = e.x;
					int y = e.y;
					pt = axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
					if(this.m_NewPolygonFeedback != null)
					{
						this.m_NewPolygonFeedback.MoveTo(pt);
					}
				}

			}

			else
			{
				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
			}
			#endregion
			m_IPointEnd.X=e.mapX ;
			m_IPointEnd.Y=e.mapY  ;

			#region �����ӡ

//			if(this.indexRY)
//			{
//				//�������
//				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair; 
//				IPoint pt = new PointClass();
//				int x = e.x;
//				int y = e.y;
//				pt = axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
//				if(this.m_NewEnvelopeFeedback != null)
//				{
//					this.m_NewEnvelopeFeedback.MoveTo(pt);
//				}
//			}

			#endregion

			if (this.identifyForm.Visible==true  )
			{
				this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerIdentify;  
			}

			
		}
  
		/// <summary>
		/// �����MapControl��˫���¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axMapControl_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
		{
			try
			{
				#region �༭

                 //˫�����߽���
				if (EndEditing)
				{
					if(this.isPolyline)
					{
						IGeometry m_Geometry ;
						m_Geometry = this.m_NewLineFeedback.Stop();

						//��ʼ�༭
						if (m_Geometry!= null)
						{
//							m_WorkspaceEdit.StartEditing(true);
//							m_WorkspaceEdit.StartEditOperation();

							//����feature
							IFeature m_Feature = this.m_FeatureClass.CreateFeature();
							m_Feature.Shape = m_Geometry;

							//����Ҫ�أ�ֹͣ�༭
							m_Feature.Store();
//							this.m_WorkspaceEdit.StopEditOperation();
//							this.m_WorkspaceEdit.StopEditing(true);
							this.axMapControl.ActiveView.Refresh();
						}
						this.m_NewLineFeedback = null;
              
					}

					//˫���������
					if(this.isPolygon)
					{
						IGeometry m_Geometry ;
						m_Geometry = this.m_NewPolygonFeedback.Stop();

						//��ʼ�༭
						if (m_Geometry != null)
						{
							if(this.m_FeatureClass != null)
							{
								//							m_WorkspaceEdit.StartEditing(true);
								//							m_WorkspaceEdit.StartEditOperation();
								//����feature
								IFeature m_Feature = this.m_FeatureClass.CreateFeature();
								m_Feature.Shape = m_Geometry;

								//����Ҫ�أ�ֹͣ�༭
								m_Feature.Store();
								//							this.m_WorkspaceEdit.StopEditOperation();
								//							this.m_WorkspaceEdit.StopEditing(true);
								this.axMapControl.ActiveView.Refresh();
								
							}

							if(indexRY)
							{
								//�������
								this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;	
								try
								{
									if (this.regionalPrintForm != null && this.regionalPrintForm.IsDisposed != true)
									{
										m_Geometry.SpatialReference = this.axMapControl.ActiveView.FocusMap.SpatialReference;
										this.regionalPrintForm.m_Geometry = m_Geometry;
										this.regionalPrintForm.index = false;
									}
								}
								catch (Exception e1)
								{
									Console.Write(e1);
								}
					
								AddTOMapControl(m_Geometry);
								ZoomTOMapControl(m_Geometry);
			
								indexRY = false;
								this.m_NewPolygonFeedback = null;
								this.EndEditing = false;
								this.isPolygon = false;
							}				
							
						}

						this.m_NewPolygonFeedback = null;
					
					}
				}
				#endregion
			}
			
			catch (Exception e1)
			{
				Debug.WriteLine("----axMapControl_OnDoubleClick has errors----"+e1.Message);
			}

		}


		/// <summary>
		/// �����MapControl���ɿ��¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axMapControl_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
		{
			try
			{
				#region �༭����ɾ��

				//							frmMapControl.isPolygon = false;
				//							frmMapControl.isPolyline = false;
				//							frmMapControl.isDelete = false;



				if(this.EndEditing)
				{
					if(this.isDelete)
					{
						int count = this.axMapControl.ActiveView.FocusMap.SelectionCount;
						if(count > 0)
						{
							string message = "ȷ��Ҫɾ����";
							string caption = "";
							MessageBoxButtons buttons = MessageBoxButtons.YesNo;

					
							DialogResult result = MessageBox.Show(this, message, caption, buttons);





							if(result == DialogResult.Yes)
							{
								DeleteFeature();	
							}
							this.axMapControl.ActiveView.FocusMap.ClearSelection();
							this.axMapControl.ActiveView.Refresh();
							
							//�¼ӵ�
//							this.EndEditing = false;
//							this.isDelete = false;
							this.m_NewLineFeedback =null;
							this.m_NewPolygonFeedback =null;
							
						}
					}
				}




				#endregion

				#region ��ӡ��������

//				if(indexRY)
//				{
//					//�������
//					this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;	
//					IGeometry m_Geometry ;
//					m_Geometry = this.m_NewEnvelopeFeedback.Stop();
//					if(m_Geometry != null)
//					{
//						try
//						{
//							if (this.regionalPrintForm != null && this.regionalPrintForm.IsDisposed != true)
//							{
//								this.regionalPrintForm.m_Geometry = m_Geometry;
//								this.regionalPrintForm.index = false;
//							}
//						}
//						catch (Exception e1)
//						{
//							Console.Write(e1);
//						}
//						
//						AddTOMapControl(m_Geometry);
//						ZoomTOMapControl(m_Geometry);
//				
//						indexRY = false;
//						this.m_NewEnvelopeFeedback = null;
//					}				
//
//				}

				#endregion
			
				//���ݵ�ǰ���ڷ�Χ�ͱ����ߵ�����ӦӰ��ͼ��
				//�ڸô���֮���Ӱ��ͼ������ʾ�����ݱ�����ѡ����ʾ��ȵ�Ӱ��ͼ������ʾ
							
//				ShowImages();



			}
			catch (Exception e1)
			{
				Debug.WriteLine("----axMapControl_OnMouseUp has errors----"+e1.Message);
			}

		}

		/// <summary>
		/// �������������ͼ�ε���ǰmap��
		/// </summary>
		/// <param name="m_Geometry"></param>
		private void AddTOMapControl(IGeometry m_Geometry)
		{
			try
			{ 
				//������mapcontrol����
				IRgbColor RgbColor = new RgbColorClass();
				RgbColor.Red = 255;
				RgbColor.Green = 0;
				RgbColor.Blue = 0;

				//�½�һ��������
				ISimpleFillSymbol m_SimpleFillSymbol = new SimpleFillSymbolClass();
				m_SimpleFillSymbol.Color = RgbColor;
				m_SimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

				//���polygonҪ��
				//				IElement m_Rectangle = new PolygonElementClass();
				//				m_Rectangle.Geometry = m_Geometry;
				//				IFillShapeElement m_FillEle = m_Rectangle as IFillShapeElement;
				//				m_FillEle.Symbol = m_SimpleFillSymbol;
				//				m_Rectangle = m_FillEle as IElement;
				//
				//				IElementProperties m_ElementProperties;
				//				m_ElementProperties = m_Rectangle as  IElementProperties;
				//				m_ElementProperties.Name = "NewRectangle";

				IRectangleElement m_RECT = new RectangleElementClass();
				IElement m_Ele = m_RECT as IElement;
				m_Ele.Geometry = m_Geometry;

				IFillShapeElement m_FillEle = m_Ele as IFillShapeElement;
				m_FillEle.Symbol = m_SimpleFillSymbol;
				m_Ele = m_FillEle as IElement;

				this.m_RectangleEle = m_Ele;
				this.regionalPrintForm.m_RECT = m_Ele;

				IGraphicsContainer m_GraphicsContainer = this.axMapControl.ActiveView as IGraphicsContainer;
				m_GraphicsContainer.AddElement(m_Ele,0);
				this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics,null,null);
				this.axMapControl.ActiveView.Refresh();
			}
			catch (Exception e)
			{
				Console.Write(e);
			} 
		}

		/// <summary>
		/// �����������ŵ�ͼ����������ͼ��
		/// </summary>
		/// <param name="m_Geometry"></param>
		private void ZoomTOMapControl(IGeometry m_Geometry)
		{
			//IEnvelope m_Envelope = this.axMapControl.ActiveView.Extent;
			IEnvelope m_Envelope = m_Geometry.Envelope;
			this.axMapControl.ActiveView.Extent = m_Envelope;
			this.axMapControl.ActiveView.Refresh();
			
		}

		/// <summary>
		/// ��������ɾ��feature
		/// </summary>
		private void DeleteFeature()
		{
			IEnumFeature m_EnumFeature;
			ISelection m_Sel;
			long SelCou;
			SelCou = this.axMapControl.ActiveView.FocusMap.SelectionCount;
			//���û��ѡ��Ҫ��
			if (SelCou == 0)
			{
				MessageBox.Show("û��ѡ��Ҫ�أ�������ѡ�񣡣�");
				return;
			}

			//ѡȡѡ���Ҫ�ؼ�
			m_Sel = this.axMapControl.ActiveView.FocusMap.FeatureSelection;
			m_EnumFeature = (IEnumFeature) m_Sel;
			IFeature m_Feature;
			//��ÿһ������ͼ�ν���ɾ��
			m_Feature = m_EnumFeature.Next();
			int i = 0;
			while(m_Feature != null)
			{
				m_Feature.Delete();
				m_Feature = m_EnumFeature.Next();
				i++;
			}
            
		}

		/// <summary>
		/// MapControlѡ�񼯱仯�¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axMapControl_OnSelectionChanged(object sender, EventArgs e)
		{
			try
			{
				arryOfSelFea.Clear();
								
				//�õ�ѡ���Ҫ��
				ISelection selFeature = this.axMapControl.Map.FeatureSelection;
				IEnumFeature pEnumFeat = (IEnumFeature)selFeature;								
				
				IFeature ifeature = pEnumFeat.Next(); 
				while (ifeature != null)
				{
					//����Ҫ������
					arryOfSelFea.Add(ifeature); 
					ifeature = pEnumFeat.Next(); 
				}

				
			}
			catch(Exception ee)
			{
				Debug.WriteLine("MapControl ѡ��Ҫ�ر仯�� "+ee.Message);
			}
		
		}
        
		
		#endregion

		#region MapControl��PageLayoutControl��������

		/// <summary>
		/// MapControl��PageLayoutControl������������ͼ
		/// </summary>
		public void CopyAndOverwritePage()
		{
			try
			{ //Get IObjectCopy interface
				IObjectCopy objectCopy = new ObjectCopyClass();

				//Get IUnknown interface (map to copy)
				object toCopyMap = this.axMapControl.ActiveView.FocusMap;

				//Get IUnknown interface (copied map)
				object copiedMap = objectCopy.Copy(toCopyMap);

				//Get IUnknown interface (map to overwrite)
				object toOverwriteMap = this.axPageLayoutControl.ActiveView.FocusMap;

				//Overwrite the MapControl's map
				objectCopy.Overwrite(copiedMap, ref toOverwriteMap);

				//Refresh the display
				this.axPageLayoutControl.ActiveView.Refresh();

				//SetMapExtent();
			}
			catch (Exception e)
			{
				Debug.WriteLine("----CopyAndOverwriteMap has errors----"+e.Message);
			}
		}

		/// <summary>
		/// MapControl��PageLayoutControl���������õ�ͼ��Χ
		/// </summary>
		private void SetMapExtent()
		{
			//Get IActiveView interface
			IActiveView activeView = (IActiveView) this.axMapControl.ActiveView.FocusMap;
			//Set the control's extent
			this.axPageLayoutControl.Extent = activeView.Extent;
			//Refresh the display
			this.axPageLayoutControl.ActiveView.Refresh();
		}

		#endregion

		#region Ӱ����ʾ�¼�

		public void ShowImages()
		{
            this.axTOCControl.SetActiveView(null );
			//ESRI.ArcGIS.Geometry.IEnvelope m_IEnvelopeMapCurrentExtent= this.axMapControl.ActiveView.Extent;
			if (this.axMapControl==null) return;

			if (this.axMapControl.MapScale<30000)
			{
				this.axMapControl.Refresh();
			}
//			System.Threading.Thread.Sleep(500);
//			if (this.cmbImageRaster.Items.Count <=0 || this.cmbImageRaster.SelectedItem==null)
//			{
//				clsFunction.Function.MessageBoxInformation(this,"û��ָ��Ӱ��⣡");
//				return;
//			}

			double m_dbScale;
			m_dbScale=this.axMapControl.MapScale  ;

			IFeature XianShifeature;
			IFeatureLayer iLayerTufu;
			ArrayList TufuResult = new ArrayList() ;
			ArrayList m_ArrayListTFBH=new ArrayList();
			string[] m_strTFBH;

			XianShifeature =null;
			iLayerTufu = null;
            iLayerTufu = mapFuntion.getFeatureLayerByName("10000�ַ�ͼ��");
			this.progressBar1.Visible=true;
			this.progressBar1.Minimum =0;
			try
			{
                if (this.axMapControl.MapScale >= 120000)
				{
					OpenRasterFromSDE("LNIMAGEJW");
				}
                //else if (this.axMapControl.MapScale  <500000 && this.axMapControl.MapScale  >=120000)
                //{
                //    OpenDSXZQRasterFromSDE();
                //}
				else 
				{					
					//���е���
					if( iLayerTufu != null)
					{
						TufuResult.Clear();

						//��õ�ǰ���ڷ�Χ�ڵ�ͼ��
						IEnvelope m_IEnvelope;						
						m_IEnvelope=GetUserViewExtent();
						IGeometry m_Geometry =(IGeometry)m_IEnvelope;

						TufuResult =mapFuntion.Overlay(m_Geometry,iLayerTufu);

						//���ݿ����Ƿ���ڸ�ͼ����Ӱ��
						m_ArrayListTFBH=GetInDataBase(TufuResult);

						//���ݱ�����ȷ����ʾ��Щͼ��

						m_strTFBH=GetShowImageName(m_ArrayListTFBH,m_dbScale);
						if (m_strTFBH==null || m_strTFBH.Length  <1) return;
						LoadImage(m_strTFBH);

						//					this.axMapControl.Refresh();
					
					}

					TufuResult=null;
					m_ArrayListTFBH=null;
					m_strTFBH=null;
					ShowImagesTimes=ShowImagesTimes+1;
				}
//				this.axMapControl.Refresh();
				
			}
			catch(Exception errs)
			{
				clsFunction.Function.MessageBoxError(this,errs.Message);
			}
			finally
			{
                this.axTOCControl.SetActiveView(this.axMapControl.ActiveView);
				this.progressBar1.Visible=false;
			}

		}

		private IEnvelope GetUserViewExtent()
		{
			//��õ�ǰ���ڷ�Χ�ڵ�ͼ��
			double m_dbMapScale=0;
			IEnvelope m_IEnvelope;	

			EnvelopeClass  m_IEnvelopeNew=new EnvelopeClass();	
			m_dbMapScale=this.axMapControl.MapScale ;

				
			m_IEnvelope=  this.axMapControl.Extent;
			if (axMapControl.CurrentTool!=null && ShowImagesTimes!=1)
			{
				string m_strCurrentTool=axMapControl.CurrentTool.ToString();
				if (m_strCurrentTool=="ESRI.ArcGIS.ControlCommands.ControlsMapPanToolClass")
				{//��ƽ�ƺ���Ҫ���ƽ�ƺ�Ĵ��ڷ�Χ�����ܽ�ƽ�ƺ�Ĵ��ڷ�Χ�ڵ�ͼ����ȷ��ʾ
					if (m_IPointStart!=null && m_IPointEnd!=null)
					{
						m_IEnvelope.Offset(m_IPointStart.X-m_IPointEnd.X,m_IPointStart.Y -m_IPointEnd.Y);
					}
				}
				if (m_strCurrentTool=="ESRI.ArcGIS.ControlCommands.ControlsMapZoomInToolClass" && Math.Abs( m_IPointEnd.X-m_IPointStart.X)>0.01)
				{
					double m_dbXMax=(m_IPointEnd.X>m_IPointStart.X ? m_IPointEnd.X:m_IPointStart.X);
					double m_dbXMin=(m_IPointEnd.X<m_IPointStart.X ? m_IPointEnd.X:m_IPointStart.X);
					double m_dbYMax=(m_IPointEnd.Y >m_IPointStart.Y  ? m_IPointEnd.Y:m_IPointStart.Y );
					double m_dbYMin=(m_IPointEnd.Y <m_IPointStart.Y  ? m_IPointEnd.Y:m_IPointStart.Y );					

					m_IEnvelopeNew.XMax=m_dbXMax;
					m_IEnvelopeNew.XMin=m_dbXMin;
					m_IEnvelopeNew.YMax=m_dbYMax;
					m_IEnvelopeNew.YMin=m_dbYMin;
					
					m_IEnvelope=m_IEnvelopeNew;			
					
				}
			}
			m_IEnvelopeNew=null;

			double m_dbExtent=1;
			double m_dx=0;
			double m_dy=0;
			if  (m_dbMapScale>=300000)
			{
				m_dbExtent=1;
			}			
			else if  (m_dbMapScale<300000 && m_dbMapScale>=100000)
			{
				m_dbExtent=1.2;
			}
			else if  (m_dbMapScale<100000 && m_dbMapScale>=50000)
			{
				m_dbExtent=1.3;
			}
			else if  (m_dbMapScale<50000 && m_dbMapScale>=30000)
			{
				m_dbExtent=1.5;
			}
			else if  (m_dbMapScale<30000 && m_dbMapScale>=20000)
			{
				m_dbExtent=1.6;
			}
			else if  (m_dbMapScale<20000 && m_dbMapScale>10000)
			{
				m_dbExtent=1.8;
			}
			else
			{
				m_dbExtent=2.0;
			}
			m_dx=m_IEnvelope.Width*(m_dbExtent-1);
			m_dy=m_IEnvelope.Height*(m_dbExtent-1);
			m_IEnvelope.Expand(m_dx,m_dy,false);

			return m_IEnvelope;
		}

		

		/// <summary>
		/// ���ݱ����������ʾ�ĵ�ͼӰ���ļ�������
		/// </summary>
		/// <param name="p_TufuResult"></param>
		/// <param name="p_dbMapScale"></param>
		/// <returns></returns>
		public string[]  GetShowImageName(ArrayList p_ArrayListTFBH,double p_dbMapScale)
		{
			string m_strJB="";
			string[] m_strTFBH=new string[p_ArrayListTFBH.Count  ];
//			if (p_dbMapScale>=3000000)
//			{
//				m_strJB="_5";
//			}
//			else if  (p_dbMapScale<3000000 && p_dbMapScale>=1000000)
//			{
//				m_strJB="_4";
//			}
//			else if  (p_dbMapScale<1000000 && p_dbMapScale>=100000)
//			{
//				m_strJB="_3";
//			}
//			else if  (p_dbMapScale<100000 && p_dbMapScale>=50000)
//			{
//				m_strJB="_2";
//			}
//			else if  (p_dbMapScale<50000 && p_dbMapScale>30000)
//			{
//				m_strJB="_1";
//			}
//			else
//			{
//				m_strJB="";
//			}
//			if (p_dbMapScale>=350000)
//			{
//				OpenRasterFromSDE("SDE.LNIMAGEJW");
//			}
			//else 
			if  (p_dbMapScale<350000 && p_dbMapScale>=150000)
			{
				m_strJB="_3";
			}
			else if  (p_dbMapScale<150000 && p_dbMapScale>=50000)
			{
				m_strJB="_2";
			}
			else if  (p_dbMapScale<50000 && p_dbMapScale>20000)
			{
				m_strJB="_1";
			}
			else
			{
				m_strJB="";
			}

			this.progressBar1.Maximum=p_ArrayListTFBH.Count;
			for (int i=0;i<p_ArrayListTFBH.Count   ;i++)
			{
				this.progressBar1.Value=i;
				this.progressBar1.Refresh();
                m_strTFBH[i] = p_ArrayListTFBH[i].ToString();//20080606
                //m_strTFBH[i]=p_ArrayListTFBH[i].ToString()+m_strJB;
                //m_strTFBH[i]=m_strTFBH[i].Replace("(","");
                //m_strTFBH[i]=m_strTFBH[i].Replace(")","");
                //m_strTFBH[i]=m_strTFBH[i].Replace("-","_");
			}
			return m_strTFBH;

		}

		public bool OpenRasterFromSDE(string rasterDatasetName)
		{
			IRasterDataset rasterDataset = null;

			//���ص�RasterDataset
				
			// Open an ArcSDE raster dataset with the given name
			// server, instance, database, user, password, version are database connection info
			// rasterDatasetName is the name of the raster dataset to be opened //Open the ArcSDE workspace 
			//����sde����
			IPropertySet  m_PropertySet = new PropertySetClass();
			m_PropertySet.SetProperty("Server",this.objectDataAccess.Server );
			m_PropertySet.SetProperty("Instance",this.objectDataAccess.Service  );
			m_PropertySet.SetProperty("Database",this.objectDataAccess.DataBase );   
			m_PropertySet.SetProperty("user",this.objectDataAccess.UserID );
			m_PropertySet.SetProperty("password",this.objectDataAccess.Password );
			m_PropertySet.SetProperty("version","sde.DEFAULT");
		
			// cocreate the workspace factory
			IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();		

			IRasterWorkspaceEx rasterWorkspaceEx = workspaceFactory.Open(m_PropertySet, 0) as IRasterWorkspaceEx;
		
			//Open the ArcSDE raster dataset
			try
			{			
				
				if(mapFuntion.haveRasterDataset(rasterDatasetName)==false)					
				{
					rasterDataset = rasterWorkspaceEx.OpenRasterDataset(rasterDatasetName);
                    this.mapFuntion.AddRasterLayer(this.axMapControl, rasterDataset, this.axMapControl.LayerCount, rasterDatasetName);		
				}
				else
				{
					for(int i=0;i<this.axMapControl.LayerCount ;i++)
					{
						ILayer m_ILayer=this.axMapControl.get_Layer(i);
						if ( m_ILayer is RasterLayer )
						{
							if (m_ILayer.Name !=rasterDatasetName)
							{
								if (m_ILayer.Visible==true)
								{
									m_ILayer.Visible=false;
								}
							}
							else
							{
								if (m_ILayer.Visible==false)
								{
									m_ILayer.Visible=true;
								}
							}
						}
					}
				}
			}
			catch(Exception errs)
			{
				clsFunction.Function.MessageBoxError(this,errs.Message);
			}
			finally
			{
				m_PropertySet=null;
			}
			return false;
		}

		public bool OpenDSXZQRasterFromSDE()
		{

			//	IFeature XianShifeature;
			IFeatureLayer iLayerDSXZQY;
			ArrayList m_LayerDSXZQY = new ArrayList() ;
			//	ArrayList m_ArrayListTFBH=new ArrayList();
			string[] m_strQHDM;

			//	XianShifeature =null;
			iLayerDSXZQY = null;
			iLayerDSXZQY = mapFuntion.getFeatureLayerByName(m_strDSXZQY );

			//���е���
			if( iLayerDSXZQY != null)
			{

				//��õ�ǰ���ڷ�Χ�ڵ�ͼ��
				IEnvelope m_IEnvelope;						
				m_IEnvelope=GetUserViewExtent();
				IGeometry m_Geometry =(IGeometry)m_IEnvelope;

				m_LayerDSXZQY =GetQHDMByOverlay(m_Geometry,iLayerDSXZQY);		
					
	

				IRasterDataset rasterDataset = null;

				//���ص�RasterDataset
				
				// Open an ArcSDE raster dataset with the given name
				// server, instance, database, user, password, version are database connection info
				// rasterDatasetName is the name of the raster dataset to be opened //Open the ArcSDE workspace 
				//����sde����
				IPropertySet  m_PropertySet = new PropertySetClass();
				m_PropertySet.SetProperty("Server",this.objectDataAccess.Server );
				m_PropertySet.SetProperty("Instance",this.objectDataAccess.Service  );
				m_PropertySet.SetProperty("Database",this.objectDataAccess.DataBase );   
				m_PropertySet.SetProperty("user",this.objectDataAccess.UserID );
				m_PropertySet.SetProperty("password",this.objectDataAccess.Password );
				m_PropertySet.SetProperty("version","sde.DEFAULT");
		
				// cocreate the workspace factory
				IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();		

				IRasterWorkspaceEx rasterWorkspaceEx = workspaceFactory.Open(m_PropertySet, 0) as IRasterWorkspaceEx;
		
				//Open the ArcSDE raster dataset
				try
				{		
					for(int i=0;i<this.axMapControl.LayerCount ;i++)
					{
						ILayer m_ILayer=this.axMapControl.get_Layer(i);

                        if (m_ILayer is RasterLayer && m_ILayer.Name.Length>=4)
						{
							if (m_ILayer.Name.Substring(0,4).ToUpper() !="JW21")
							{
								if (m_ILayer.Visible==true)
								{
									m_ILayer.Visible=false;
								}
							}
							else
							{
								bool m_NeedShow=false;
								for (int n=0;n<m_LayerDSXZQY.Count;n++)
								{
									if (m_ILayer.Name.ToUpper()=="JW"+m_LayerDSXZQY[n].ToString())
									{
										m_NeedShow=true;
										if (m_ILayer.Visible==false)
										{
											m_ILayer.Visible=true;											
										}
										break;
									}											
								}
								if (m_ILayer.Visible==true && m_NeedShow==false)
								{
									m_ILayer.Visible=false;
								}
							}
						}
					}

					for (int m=0;m<m_LayerDSXZQY.Count ;m++)
					{
						string m_rasterDatasetName="JW"+m_LayerDSXZQY[m].ToString();
						if(mapFuntion.haveRasterDataset(m_rasterDatasetName)==false)					
						{
							rasterDataset = rasterWorkspaceEx.OpenRasterDataset(m_rasterDatasetName);
                            this.mapFuntion.AddRasterLayer(this.axMapControl, rasterDataset, this.axMapControl.LayerCount, m_rasterDatasetName);		
						}						
					}	
					return true;
				}
				catch(Exception errs)
				{
					clsFunction.Function.MessageBoxError(this,errs.Message);
				}
				return false;
			}
			return false;
		}

		/// <summary>
		/// ���ӷ���
		/// </summary>
		/// <param name="m_Geometry">���ӷ����ķ�Χ</param>
		/// <param name="mLayer">������ӷ�����ͼ��</param>
		/// <returns></returns>
		public ArrayList GetQHDMByOverlay(IGeometry m_Geometry,IFeatureLayer mLayer)
		{
			try
			{
				ISpatialFilter m_SpatialFilter= new SpatialFilterClass();
				m_SpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
				//				IGeometry m_Geometry = mfeature.Shape;
				m_SpatialFilter.Geometry = m_Geometry;
				IFeatureCursor m_FeatureCursor = null; 
				m_FeatureCursor = mLayer.Search(m_SpatialFilter,true);
				 
				//����ͼ��������
				IFeature selFeature = m_FeatureCursor.NextFeature();
				ArrayList m_QHDM = new ArrayList(); 
				int i = 0;
				while(selFeature!= null)
				{
					int index =selFeature.Fields.FindField("QHDM");
					string thevalue = selFeature.get_Value(index).ToString();
					Debug.WriteLine(thevalue);
					// IField filed = selFeature.Fields.get_Field(index);
					m_QHDM.Add(thevalue);
					selFeature = m_FeatureCursor.NextFeature();
					i++;
				}
				return m_QHDM;

			}
			catch(Exception errs)
			{
				throw errs;	
			}
			
		}
		


		/// <summary>
		/// 
		/// </summary>
		/// <param name="p_strTFBH"></param>
		private void LoadImage(string[] p_strTFBH)
		{
			//���Ƚ���ǰ��Ӱ���ļ����������ͼ�е�ͼ��Աȣ������ͼ�㣨ң��Ӱ�񣩲��ڸ������У��������Ϊ���ɼ��������������Ϊ�ɼ�
			//��������е�Ӱ��û�д򿪣������ݿ��д򿪸��ļ�
			//������״μ���ң��Ӱ�񣨻����ң��Ӱ����״���ӣ�����Ҫ�жϣ�ֱ�ӽ�ͼ����ص�ǰ��ͼ�Ϳ�����
			int m_IntHasShowImage=0;
			ILayer fLayer=null;
			bool m_IsShow=false;
			int[] p_intIsShow=new int[p_strTFBH.Length ] ;
			string m_strLayerName="";
			string m_strTempLayerName="";
			int m_intCountTemp=0;

			ArrayList arrayOflayers=mapFuntion.getAllLayers();				//�õ�ͼ������		
			IEnumerator myEnumerator = arrayOflayers.GetEnumerator();		//����ͼ������	
			
		
	
			if (ShowImagesTimes>1)
			{


				this.progressBar1.Maximum=arrayOflayers.Count ;

				Debug.Write("����ͼ������Ѿ���ʾ��ͼ���Ƿ���ʾ");
				Debug.Write(DateTime.Now.ToLongTimeString());
				while ( myEnumerator.MoveNext() )
				{///����ͼ������Ѿ���ʾ��ͼ���Ƿ���ʾ
					fLayer=(ILayer)myEnumerator.Current;
					m_IsShow=false;
					m_intCountTemp=m_intCountTemp+1;
					this.progressBar1.Value=m_intCountTemp;
					this.progressBar1.Refresh();

					if(fLayer!= null)
					{
						m_strLayerName=fLayer.Name.ToUpper();

						if (m_strLayerName=="LNIMAGEJW")
						{
							fLayer.Visible=false;
							continue;
						}
						for (int i=0;i<p_strTFBH.Length ;i++)
						{//�ж�ͼ���Ƿ���������
							if (m_strLayerName.Length-p_strTFBH[i].Length<0) continue;
							m_strTempLayerName=m_strLayerName.Substring(m_strLayerName.Length-p_strTFBH[i].Length, p_strTFBH[i].Length);
						
							if(m_strTempLayerName.ToUpper().Equals(p_strTFBH[i].ToUpper()))
							{				
								p_intIsShow[i]=1;//��Ӱ���Ѿ�����,����Ҫ�ٴ�����
								m_IsShow=true;//��Ӱ����Ҫ��ʾ
								m_IntHasShowImage=m_IntHasShowImage+1;
								break;
							}
						}
						if(m_IsShow==true)
						{
							if (fLayer.Visible==false)
								fLayer.Visible=true;
						}
						else
						{
							if (fLayer is RasterLayer )
							{
								if (fLayer.Visible==true)
									fLayer.Visible=false;
							}
						}
				
					}
				}
		
				if (m_IntHasShowImage==p_strTFBH.Length ) return;//���е�Ӱ���Ѿ���ʾ
			}
			//����û����ʾ��ͼ����뵱ǰ��ͼ
			Debug.Write(DateTime.Now.ToLongTimeString());
			Debug.Write("����ͼ��");
			Debug.Write(DateTime.Now.ToLongTimeString());

			IRasterWorkspaceEx m_IRasterWorkspaceEx=mapFuntion.OpenIRasterWorkspaceEx(objectDataAccess.Server,objectDataAccess.Service,
			                                                                          objectDataAccess.DataBase,objectDataAccess.UserID,objectDataAccess.Password, "sde.DEFAULT");
			
			IRasterCatalog m_IRasterCatalog=null;
			if (m_strCurrentRasterCatalog!="" && m_strCurrentRasterCatalog!=null)
			{
				m_IRasterCatalog=mapFuntion.openRasterCatalog(m_IRasterWorkspaceEx,""+m_strCurrentRasterCatalog);
			}
			else
			{
				m_IRasterCatalog=mapFuntion.openRasterCatalog(m_IRasterWorkspaceEx,"Raster2005");
			}
			if (ShowImagesTimes>1)
			{

				this.progressBar1.Maximum=p_strTFBH.Length  ;

				for (int j=0;j<p_strTFBH.Length;j++)
				{
					this.progressBar1.Value=j;
					this.progressBar1.Refresh();

					if (p_intIsShow[j]!=1)
					{
						OpenImageFile(m_IRasterCatalog,p_strTFBH[j]);
					}
					else
					{
					}
				}
			}
			if (ShowImagesTimes==1)
			{//�״μ���Ӱ��
				this.progressBar1.Maximum=p_strTFBH.Length  ;
				for (int j=0;j<p_strTFBH.Length;j++)
				{		
					this.progressBar1.Value=j;
					this.progressBar1.Refresh();

					OpenImageFile(m_IRasterCatalog,p_strTFBH[j]);					
				}
			}
			Debug.Write(DateTime.Now.ToLongTimeString());
//			this.axMapControl.Refresh();
		}

		/// <summary>
		/// �����ص���ͼ����Ų����ݿ����Ƿ����Ӱ������
		/// </summary>
		/// <returns></returns>
		public ArrayList GetInDataBase(ArrayList p_LayerNames)
		{
			ArrayList m_ArrayListImages=new ArrayList();
			string[] m_strImagesTemp;
			bool got = false;
			int sum = p_LayerNames.Count;

			int m_intTemp=-1;
			string m_TFBH = "";
            string m_TFBHTIF = "";
            string m_TFBHIMG = "";

			try                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
			{
				//�����ݿ��ѯʱ��ѯ�������ܳ���1000���������Ҫ�����ǽ��з�������ѯ��Ȼ�󽫲�ѯ��Ľ���ٺϲ���һ��������				
				this.progressBar1.Maximum=p_LayerNames.Count   ;

				Debug.Write("��ѯ��ǰ��Χ��ͼ�������ݿ����Ƿ����Ӱ��");
				Debug.Write(DateTime.Now.ToLongTimeString());

				for (int k = 0; k<p_LayerNames.Count; k++)
				{
					this.progressBar1.Value=k;
					this.progressBar1.Refresh();

					m_intTemp=m_intTemp+1;	
					
					if (((k %999==0) && k!=0) || k==p_LayerNames.Count-1 || (k==0 && p_LayerNames.Count==1))
					{
							
						m_TFBH = m_TFBH + "'" + p_LayerNames[k].ToString()+ "'";                        
                        
						m_intTemp=-1;
					}
					else
					{
						m_TFBH = m_TFBH + "'" + p_LayerNames[k].ToString() + "',";                        
					}
					if (m_intTemp!=-1  )
					{							
						continue;
					}
					else
					{
						m_strImagesTemp=ReadImagesTFBH(m_TFBH);
						if (m_strImagesTemp==null || m_strImagesTemp.Length <1) 
						{
							m_TFBH =  "";		
							continue;
						}
						for (int m=0;m<m_strImagesTemp.Length ;m++)
						{
							m_ArrayListImages.Add((object)m_strImagesTemp[m]);
						}
					}							
					
					m_TFBH =  "";					
				}
				Debug.Write(DateTime.Now.ToLongTimeString());
				return m_ArrayListImages;
			}
			catch(Exception errs)
			{
				MessageBox.Show(this,errs.Message,"������ʾ",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			
			return null;
		}

		private string[] ReadImagesTFBH(string p_TFBH)
		{
            string m_TFBHNew;
			string[] m_strImagesTemp;
			DataRowCollection m_DataRowCollection;
           

            char[] m_charSplit=new char[1];
            m_charSplit[0]=',';

            string[] m_TFBH = p_TFBH.Split(m_charSplit, System.StringSplitOptions.None);
            string m_TFBHPRETIF = "";
            string m_TFBHPREIMG = "";
            string m_TFBHTIF = "";
            string m_TFBHIMG = "";
            for (int i = 0; i < m_TFBH.Length; i++)
            {
                m_TFBHPRETIF = m_TFBHPRETIF + m_TFBH[i].Remove(m_TFBH[i].Length - 1, 1) + ".tif',";   
                m_TFBHNew = m_TFBH[i].Replace(")", "");
                m_TFBHNew = m_TFBHNew.Replace("(", "");
                m_TFBHTIF = m_TFBHTIF + m_TFBHNew.Remove(m_TFBHNew.Length - 1, 1) + ".tif',";               
            }

            m_TFBHTIF = m_TFBHTIF.Remove(m_TFBHTIF.Length - 1, 1);
            m_TFBHIMG = m_TFBHTIF.Replace(".tif",".img");

            m_TFBHPRETIF = m_TFBHPRETIF.Remove(m_TFBHPRETIF.Length - 1, 1);
            m_TFBHPREIMG = m_TFBHPRETIF.Replace(".tif", ".img");


			//string strSQL = "SELECT SATELITE,SENSOR,SENSORMODE,CLOUDNUMB,SUNAZIMUTH,SUNELEVATION,DOWNORUP,ORBITNUM,SIDEANGLE,ULLATITUDE,ULLONGITUDE,URLATITUDE,URLONGITUDE,LRLATITUDE,LRLONGITUDE,LLLATITUDE,LLLONGITUDE,COLUMNID,HROWID,QCQUIREDATE,RECEIVINGSTATION,PRODUCTGRADE,QUALITY,PRODUCEDATE,PRODUCER,LOADINTIME,LOADINSTAFFNAME,TFBH,LAYERNAME,RASTERCATALOG FROM YGYX WHERE TFBH IN ( " + p_TFBH.ToUpper() + ") or TFBH IN ( " + p_TFBH.ToLower() + ")";
            string strSQL = "SELECT * FROM " + this.m_strCurrentRasterCatalog + " WHERE name IN ( " + p_TFBH.ToUpper() + ") or name IN ( " + p_TFBH.ToLower() + ")  or name IN ( " +
                      m_TFBHPRETIF.ToUpper() + ") or name IN ( " + m_TFBHPRETIF.ToLower() + ")  or name IN ( " + m_TFBHPREIMG.ToUpper() + ")  or name IN ( " + m_TFBHPREIMG.ToLower() + ")  or name IN ( " +
                      m_TFBHTIF.ToLower() + ") or name IN ( " + m_TFBHTIF.ToUpper() + ")  or name IN ( " + m_TFBHIMG.ToUpper() + ")  or name IN ( " + m_TFBHIMG.ToLower() + ")";
			
			try
			{
				m_DataRowCollection=objectDataAccess.getDataRowsByQueryString(strSQL);
				if (m_DataRowCollection==null) return null;

				m_strImagesTemp=new string[ m_DataRowCollection.Count] ;
				if (m_DataRowCollection!=null && m_DataRowCollection.Count>0)
				{				

					for (int i=0;i<m_DataRowCollection.Count ;i++)
					{
						m_strImagesTemp[i]=m_DataRowCollection[i]["name"].ToString();
					}					
				}
				else 
				{
//					MessageBox.Show("��������û��Ӱ��");					
				}
				return m_strImagesTemp;
					
			}
			catch(Exception errs)
			{
				MessageBox.Show(this,errs.Message,"������ʾ",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			return null;
			
		}	

		
		/// �����ѯ����դ��Ӱ�����飩
		/// </summary>
		/// <param name="p_LayerNanes"></param>
		private void OpenImageFile(IRasterCatalog m_IRasterCatalog,string  p_LayerName)
		{			
			try
			{			
				string layerName = p_LayerName;				
				IRasterDataset m_IRasterDataset= mapFuntion.GetRasterDatasetFromRasterCatalog(m_IRasterCatalog,p_LayerName);
				if (m_IRasterDataset==null) return;				

				if(mapFuntion.haveRasterDataset(layerName)==false)					
				{
                    this.mapFuntion.AddRasterLayer(this.axMapControl, m_IRasterDataset, this.axMapControl.LayerCount, layerName);		
				}
				
			}
			catch (Exception e)
			{
				Debug.WriteLine("����Ӱ�����"+e.Message );
			}
		}

	
		/// <summary>
		/// mapcontrol��Χˢ���¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
		{
//			//���ݵ�ǰ���ڷ�Χ�ͱ����ߵ�����ӦӰ��ͼ��
//			//�ڸô���֮���Ӱ��ͼ������ʾ�����ݱ�����ѡ����ʾ��ȵ�Ӱ��ͼ������ʾ
			if (ShowImagesTimes>0)
			{
				ShowImagesTimes=ShowImagesTimes+1;
				ShowImages();
			}
			
		}

		/// <summary>
		/// mapcontrol ��С�ı��¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void axMapControl_SizeChanged(object sender, EventArgs e)
		{
			this.progressBar1.Top =this.Height-60 ;
			this.progressBar1.Left=0;
			this.progressBar1.Width=this.Width;
		}

		private void menuItemSetImageCatalog_Click(object sender, EventArgs e)
		{
			m_frmSetImageCatalog.objectDataAccess=this.objectDataAccess ;
			m_frmSetImageCatalog.strCurrentRasterCatalog= m_strCurrentRasterCatalog;
			m_frmSetImageCatalog.SelectChangeEvent+=new frmSetImageCatalog.SelectChangeEventHandler(m_frmSetImageCatalog_SelectChangeEvent);
			m_frmSetImageCatalog.ShowDialog();
		}

		public void m_frmSetImageCatalog_SelectChangeEvent(string p_strCurrentRasterCatalog)
		{
			this.m_strCurrentRasterCatalog =p_strCurrentRasterCatalog;
		}
		private void menuItemLoadImages_Click(object sender, EventArgs e)
		{
		
		}

		private void menuItemUnVisibleImages_Click(object sender, EventArgs e)
		{
		
		}

		private void menuItemRemoveImages_Click(object sender, EventArgs e)
		{
		
		}

		#endregion

        private void frmMapControl_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (this.MdiParent != null)
                {
                    //this.MdiParent.stat
                }
            }
        }
	}
}
