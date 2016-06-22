using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using clsDataAccess;
using Functions;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using System.IO;
using ESRI.ArcGIS.SystemUI;

using JCZF.MainFrame;

namespace JCZF.SubFrame
{
    public partial class frmMapView : Form
    {
        // ******����������       
        
        private bool m_blStartComputeDistance;//��ʼ�������
        private bool m_blStartComputeDistance_Check;//���¾��������ť
        private bool m_blStartComputeArea;//��ʼ�������
        private bool m_blStartComputeArea_Check;//�������������ť
        //IPolyline m_IPolyline_Survey;
        //IGeometryCollection m_IPolyline_Survey;
        IPolyline m_IPolyline_Survey;
        IPolygon m_IPolygon_Survey;
        IPointCollection m_IPointCollection ;
        INewLineFeedback m_INewLineFeedback;
        IPoint m_IPoint_LastDraw;
        double m_dbMesureResult;
        IElement m_IElement_Survey;
       // ********

        //��ӡ
        public AxPageLayoutControl m_axPageLayoutControl;
        public Print.PageLayoutControl.frmPageLayoutControl m_frmPageLayoutControl;
        //public Print.Function.MapFunction m_MapFunction_Print;

        private string m_strCurrentMapFileName_;
        public string m_strCurrentMapFileName
        {
            set
            {
                m_strCurrentMapFileName_ = value;
            }
        }

        private string m_FeatureLayerName = "";
        //private frmShowPic m_frmShowPic;
        public int m_OID = -1;

        public string dkid = "";

        private DataTable m_DataTableBJZFRY;
        private DataTable m_DataTableSJZFRY;

        //�鿴���Դ���
        public  clsMapFunction.IDentifyDialog  m_frmIDentifyDialog;
        //ѡ��Ҫ������
        public ArrayList arryOfSelFea = new ArrayList();
        //��ǰѡ���ͼ��
        public FeatureLayer curLayer;
        public  clsMapFunction.MapFunction m_MapFunction = new clsMapFunction.MapFunction();

        //�ж��Ƿ���˲鿴����
        public bool isAttribute = false;

        //�ж��������,�������
        public bool isMeasureLength = false;
        public bool isMeasureArea = false;


        #region ������

        public frmMain m_pFrmMain = null;

        //public MapFunction m_MapFunction = new MapFunction();


        #endregion

        public delegate void frmMapView_ActivateEventHandler(bool p_IsActivated);
        public event         frmMapView_ActivateEventHandler frmMapView_Activate;

        //public delegate void DeleteSelectFeatureEventHandler();
        //public event DeleteSelectFeatureEventHandler DeleteSelectFeatureEvent;
       


        #region �����·���ѡ�� �ؿ��¼� ����0731
        public delegate void EventHandlerPostDkClick(string[] m_DkId, int[] m_DkNum, string[] m_DkZb);
        public event EventHandlerPostDkClick PostDKClick;
        public delegate void EventHandlerPostDrawDkClick(string[] m_DkId, int[] m_DkNum, string[] m_DkZb);
        public event EventHandlerPostDrawDkClick PostDrawDKClick;
        #endregion

        public clsDataAccess.DataAccess m_DataAccess_SYS;


        public IActiveView m_ActiveView;
        //�򿪵�mxd�ļ�
        public IMapDocument m_MapDocument;

        #region ��ͼ����

        private esriUnits m_MapUnits;
        private string m_sMapUnits;

        private double m_tempX = 0;
        private double m_tempY = 0;


        #endregion

        private bool m_bShowTBDS = false;
        private string m_strXZQDM="";

        //private string m_strDM1 = "";

        

        private bool m_bSelTB = false;
        private IFeature m_CurFeature;

        private ArrayList arrdlmc = new ArrayList();
        private ArrayList dbldltb = new ArrayList();

        bool Isshowpanel = false ;


        public IFeature m_hcselfeature;

        private bool b_showLabel;

        //�ؿ�
        public bool b_GetDK = false;
        public bool b_GetDrawDK = false;

        public string[] m_DkId = new string[1];
        public  int[]  m_DkNum=new int[1];
        public string[]  m_DkZb=new string[1];


        //��ť ����  �ؿ�ѡ��ʽ ����0731
        public bool b_hcselectDK;
        public bool b_hcDrawDK;
        public bool b_hcImportDK;

        public frmSelAnalysis_hc m_frmSelAnalysis_hc;


        private DevComponents.DotNetBar.ButtonItem m_ImageShow;




        ILayer m_ILayerImage;
        ILayer m_ILayerTDLY;
        ILayer m_ILayerCKQ;
        ILayer m_ILayerTKQ;
        ILayer m_ILayerTDGY;
        ILayer m_ILayerJSYD;
        ILayer m_ILayerTDGH;
        ILayer m_ILayerJBNT;


        public frmMapView(frmMain parentForm)
        {
            m_blStartComputeDistance = false;
            m_blStartComputeArea = false;  
            m_blStartComputeDistance_Check = false;
            m_blStartComputeArea_Check = false;  


            InitializeComponent();
            this.m_pFrmMain = parentForm;
            this.m_pFrmMain.m_bIsMapViewFormOpen = true;
            this.m_pFrmMain.m_MapFuction.axMapControl = this.axMapControl1;

            m_MapFunction.axMapControl = this.axMapControl1;
            b_showLabel = false;
           
        }

        private void frmMapView_Load(object sender, EventArgs e)
        {
            
            if (this.m_pFrmMain.m_bIsFirstStart)
            {
                //this.LoadFile(Application.StartupPath + @"\work.mxd");
                //LoadDefaultMap();

                //setImageBtni();
                //setTDLYXZBtni();
            }
            this.WindowState = FormWindowState.Maximized;
        }

        private void LoadDefaultMap()
        {

            this.LoadFile(Application.StartupPath + @"\work\" +m_strCurrentMapFileName_ );
          
        }

        public void LoadFile(string filepath)
        {
            if (this.axMapControl1.CheckMxFile(filepath))
            {
                this.axMapControl1.LoadMxFile(filepath);

                GetBtiLayers(this.axMapControl1);

                this.InitMap();
                //Open document
                OpenDocument(filepath);

                setImageBtni();
                setTDLYXZBtni();
                setJBNTBtni();  //����ũ��
                SetSelectedMapBtiOption();

              
                uctXZQTree1.m_AxMapControl = axMapControl1;
            }
        }

        /// <summary>
        /// ����������ʼ����ͼ��ز���
        /// </summary>
        private void InitMap()
        {
            this.m_ActiveView = this.axMapControl1.ActiveView;
            this.m_MapUnits = this.axMapControl1.MapUnits;
            this.m_sMapUnits = MapFunction.getMapUnits(this.m_MapUnits);
        }

        /// <summary>
        /// ��mapdocument�ļ�
        /// </summary>
        /// <param name="sFilePath"></param>
        private void OpenDocument(string sFilePath)
        {
            //Create a new map document
            m_MapDocument = new MapDocument() as IMapDocument;
            //Open the map document selected
            m_MapDocument.Open(sFilePath, "");
        }

        #region ���������
        /// <summary>
        /// ��������ͣ��
        /// </summary>
        private void SetFalse(bool p_blIsEnable)
        {
            m_bSelTB = false;
            b_hcselectDK = p_blIsEnable;
            b_hcDrawDK = p_blIsEnable;
            
        }
        private void bubbleButton1_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false );
            SetBubbleButtonNormal();
            this.m_MapFunction.Pan();
            //Isshowpanel = false ;
            //panelEx1.Visible = false ;

            bubbleButton1.Image = global::JCZF.Properties.Resources._32x32_ƽ��_hot;

            SetbubleHot();
        }

        private void SetBubbleButtonNormal()
        {
            bubbleButton1.Image = global::JCZF.Properties.Resources._32x32_ƽ��;
            bubbleButton2.Image = global::JCZF.Properties.Resources._32x32_�Ŵ�;
            bubbleButton3.Image = global::JCZF.Properties.Resources._32x32_��С;
            bubbleButton4.Image = global::JCZF.Properties.Resources._32x32_ȫ��;
            bubbleButton5.Image = global::JCZF.Properties.Resources._32x32_ǰһ��;
            bubbleButton6.Image = global::JCZF.Properties.Resources._32x32_��һ��;
            bubbleButton8.Image = global::JCZF.Properties.Resources._32x32_ѡ��;
            bubbleButton9.Image = global::JCZF.Properties.Resources._32x32_��Ǹ�����;
            bubbleButton10.Image = global::JCZF.Properties.Resources._32x32_ĳ������������;
            bubbleButton13.Image = global::JCZF.Properties.Resources._32x32_ͼ�η���;
            bubbleButton_Delete.Image = global::JCZF.Properties.Resources._32x32_ɾ��;
            bubbleButton_ComputeArea.Image = global::JCZF.Properties.Resources._32X32_�������;
            bubbleButton_ComputeDistance.Image = global::JCZF.Properties.Resources._32x32_��������;

            bubbleBar1.Refresh();

            SetComputeDisiable();
            
        }

        private void SetComputeDisiable()
        {
            panelEx_Compute.Visible = false;
            m_blStartComputeArea = false;
            m_blStartComputeArea_Check = false;
            m_blStartComputeDistance = false;
            m_blStartComputeDistance_Check = false;
        }
            

        private void bubbleButton2_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            SetBubbleButtonNormal();

            this.m_MapFunction.ZoomIn();
            //Isshowpanel = false;
            //panelEx1.Visible = false;

            bubbleButton2.Image = global::JCZF.Properties.Resources._32x32_�Ŵ�_hot;

            SetbubleHot();
        }

        private void bubbleButton3_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            SetBubbleButtonNormal();

            this.m_MapFunction.ZoomOut();
            //Isshowpanel = false;
            //panelEx1.Visible = false;
            SetbubleHot();
        }

        private void bubbleButton4_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
        //    SetBubbleButtonNormal();
            this.m_MapFunction.FullExtent();
            //Isshowpanel = false;
            //panelEx1.Visible = false;

            //bubbleButton1.Image = global::JCZF.Properties.Resources._32x32_ȫ��_hot;

            SetbubleHot();

        }

        private void bubbleButton5_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            this.m_MapFunction.BringToFront();
            //Isshowpanel = false;
            //panelEx1.Visible = false;

        }

        private void bubbleButton6_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            this.m_MapFunction.BringForward();
            //Isshowpanel = false;
            //panelEx1.Visible = false;

        }

        private void bubbleButton7_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            SetBubbleButtonNormal();

            m_bSelTB = true;
            this.axMapControl1.CurrentTool = null;
            this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
            //this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
            //Isshowpanel = false;
            //panelEx1.Visible = false;

            bubbleButton8.Image = global::JCZF.Properties.Resources._32x32_ѡ��_hot;

            SetbubleHot();
        }

        private void bubbleButton9_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            SetBubbleButtonNormal();

            //if (b_showLabel == false)
            //{
            b_showLabel = !b_showLabel;
            panelEx1.Visible = false;
            Isshowpanel = false;
            //}

            //else
            //{
            //    b_showLabel = false;
            //}


            if (g != null)
            {
                g.DeleteAllElements();
                this.axMapControl1.Refresh();
            }
            SetbubleHot();

        }

        private void SetbubleHot()
        {
            if (b_showLabel == true)
            {
                bubbleButton9.Image = global::JCZF.Properties.Resources._32x32_ĳ������������_hot;
            }
            if (Isshowpanel == true)
            {
                bubbleButton10.Image = global::JCZF.Properties.Resources._32x32_��Ǹ�����_hot;
            }

            bubbleBar1.Refresh();
        }


        //���Ҫ��
        private void bubbleButton10_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            SetBubbleButtonNormal();

            this.m_MapFunction.Pan();
            Isshowpanel = !Isshowpanel;
            panelEx1.Visible = false;

            if (Isshowpanel == true)
            {
                bubbleButton10.Image = global::JCZF.Properties.Resources._32x32_ĳ������������_hot;
            }
            bubbleButton1.Image = global::JCZF.Properties.Resources._32x32_ƽ��_hot;
            bubbleBar1.Refresh();
        }

        #endregion


        private JCZF.SubFrame.AttibuteEdit.FormMenuButton theForm;

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            panelEx_ZFRY_Info.Visible = false;
            if (this.theForm != null && this.theForm.Disposing == false)
            {
                theForm.Close();
            }

            IActiveView pActiveView = this.axMapControl1.ActiveView;

            #region

            if (b_showLabel == true)
            {

                //���м���������ʾʱ��
                if (this.axMapControl1.MapScale < 8000000 && axMapControl1.MapScale > 1500000)
                {
                    //��ȡ��ǰfeature
                    IFeature m_curXZQFeature = GetCurrentf(e.mapX, e.mapY, "��");

                    MapFunction.FlashFeature1(m_curXZQFeature, axMapControl1);



                    m_strXZQDM = m_curXZQFeature.get_Value(m_curXZQFeature.Fields.FindField("QHDM")).ToString();


                    //�Ŵ�feature
                    axMapControl1.MapScale = 800000;

                    //MapFunction.ZoomToFeature(m_curXZQFeature, axMapControl1);

                    //��ע

                    showZFRY(1, m_strXZQDM);
                    //MessageBox.Show("DDDD");  
                    this.axMapControl1.ActiveView.Refresh();
                    return;
                }


                //���ؼ���������ʾʱ��
                if (axMapControl1.MapScale < 1500000 && axMapControl1.MapScale > 400000)
                { //��ȡ��ǰfeature
                    IFeature m_curXZQFeature = GetCurrentf(e.mapX, e.mapY, "��");

                    MapFunction.FlashFeature1(m_curXZQFeature, axMapControl1);

                     m_strXZQDM = m_curXZQFeature.get_Value(m_curXZQFeature.Fields.FindField("QHDM")).ToString();


                    //�Ŵ�feature

                    //MapFunction.ZoomToFeature(m_curXZQFeature, axMapControl1);
                    axMapControl1.MapScale = 236769;

                    //��ע

                    showZFRY(2, m_strXZQDM);
                    //MessageBox.Show("eeee");
                    this.axMapControl1.ActiveView.Refresh();
                    return;
                }

                //��������������ʾʱ��
                if (axMapControl1.MapScale < 400001 && axMapControl1.MapScale > 80000)
                {
                    IFeature m_curXZQFeature = GetCurrentf(e.mapX, e.mapY, "����");
                    MapFunction.FlashFeature1(m_curXZQFeature, axMapControl1);
                    string m_strXZQDM = m_curXZQFeature.get_Value(m_curXZQFeature.Fields.FindField("dwdm")).ToString();


                    //�Ŵ�feature
                    axMapControl1.MapScale = 56186;
                    //MapFunction.ZoomToFeature(m_curXZQFeature, axMapControl1);

                    //��ע

                    showZFRY(3, m_strXZQDM);
                    //MessageBox.Show("ffff");
                    this.axMapControl1.ActiveView.Refresh();
                    return;
                }

            }
            #endregion



            #region   ѡ��ؿ� ���͵ؿ�����

            if (b_GetDK == true)
            {
                m_DkId[0] = "";
                m_DkNum[0] = 0;
                m_DkZb[0] = "";

                ArrayList arr = new ArrayList();
                IEnumLayer pLayers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
                ILayer pLayer = pLayers.Next();

                while (pLayer != null)
                {
                    if (pLayer.Visible == true)
                    {
                        if (pLayer.Name.IndexOf("���غ˲�") > -1 || pLayer.Name.IndexOf("����˲�") > -1)
                        {
                            arr.Add(pLayer);
                        }

                    }
                    pLayer = pLayers.Next();
                }

                pLayers.Reset();


                IIdentify pIdentify = null;
                IArray pIdentifyArray = null;
                IFeatureIdentifyObj pFeaIdObj;
                IRowIdentifyObject pRowIdObj;
                IIdentifyObj pIdObj;

                IEnvelope pEnvelope = new Envelope() as IEnvelope;
                tagRECT r;

                r.left = e.x - 10;
                r.top = e.y - 10;
                r.right = e.x + 10;
                r.bottom = e.y + 10;

                this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvelope, ref r, 4);
                pEnvelope.SpatialReference = this.axMapControl1.Map.SpatialReference;

                m_DkZb[0] = "";
                for (int i = 0; i < arr.Count; i++)
                {
                    if (pIdentifyArray == null)
                    {
                        pIdentify = (IIdentify)((ILayer)arr[i]);
                        IFeatureLayer selLayer = arr[i] as IFeatureLayer;
                        pIdentifyArray = pIdentify.Identify(pEnvelope);

                        if (pIdentifyArray == null)
                        {
                            continue;
                        }

                        pFeaIdObj = pIdentifyArray.get_Element(0) as IFeatureIdentifyObj;
                        pRowIdObj = (IRowIdentifyObject)pFeaIdObj;

                        IRow pRow = pRowIdObj.Row;

                        IFeature pFeature = (IFeature)pRow;
                        pIdObj = (IIdentifyObj)pFeaIdObj; //dkoid
                        pIdObj.Flash(this.axMapControl1.ActiveView.ScreenDisplay);
                        int j = pRow.OID;

                        int ifield = pFeature.Fields.FindField("dkid");
                        string strdkid = pFeature.get_Value(ifield).ToString(); //dkid

                        //m_DkId[0] = strdkid;

                        m_DkId[0] = j.ToString();
                        IGeometry geo = pFeature.ShapeCopy;

                        if (geo.GeometryType == esriGeometryType.esriGeometryPolygon)
                        {
                            IPointCollection pcollection = geo as IPointCollection;
                            m_DkNum[0] = pcollection.PointCount - 1;

                            for (int count = 0; count < pcollection.PointCount - 1; count++)
                            {
                                IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                                pcollection.QueryPoint(count, pt);

                                m_DkZb[0] += pt.X.ToString("#0.00") + ",";
                                m_DkZb[0] += pt.Y.ToString("#0.00") + ";";
                            }
                        }

                    }
                }

                b_GetDK = false;
                if (this.PostDKClick != null)
                {

                    this.PostDKClick(this.m_DkId, this.m_DkNum, this.m_DkZb);

                }

            }

            #endregion

            //����20110709
            #region �����´���Ƶؿ�

            IGraphicsContainer graphicsContainer = (IGraphicsContainer)this.m_ActiveView.FocusMap;
            if (b_GetDrawDK == true)
            {
                m_DkId[0] = "";
                m_DkNum[0] = 0;
                m_DkZb[0] = "";

                //����element
                IGeometry selectedGeometry = this.axMapControl1.TrackPolygon();

                IElement polygonElement = new PolygonElement() as IElement;
                polygonElement.Geometry = selectedGeometry;
                IFillShapeElement fillShapeElement = (IFillShapeElement)polygonElement;
                IFillSymbol fillSymbol = Functions.MapFunction.GetStaticSymbol();
                fillShapeElement.Symbol = fillSymbol;
                graphicsContainer.AddElement(polygonElement, 0);

                //�Ѹ�ͼ�μ��뵽���غ˲�����ͼ��

                IEnumLayer m_layers = MapFunction.GetLayers(this.axMapControl1.Map);
                IFeatureLayer pfeaturelayer = new FeatureLayer() as IFeatureLayer;
                ILayer player = m_layers.Next();
                while (player != null)
                {
                    if (player.Name == "���غ˲�")
                    {
                        pfeaturelayer = (IFeatureLayer)player;
                        break;
                    }
                    player = m_layers.Next();
                }
                m_layers.Reset();

                IFeatureClass pfeatureclass = pfeaturelayer.FeatureClass;

                IWorkspaceEdit workspaceEdit = (pfeaturelayer as IDataset).Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                IFeature feature = pfeatureclass.CreateFeature();
                feature.Shape = selectedGeometry;

                try
                {
                    feature.Store();
                }
                catch
                {
                    ITopologicalOperator topologicaloperator = selectedGeometry as ITopologicalOperator;
                    topologicaloperator.Simplify();
                    feature.Store();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);

                int j = feature.OID;

                m_DkId[0] = j.ToString();
                if (selectedGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    IPointCollection pcollection = selectedGeometry as IPointCollection;
                    m_DkNum[0] = pcollection.PointCount - 1;

                    for (int count = 0; count < pcollection.PointCount - 1; count++)
                    {
                        IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                        pcollection.QueryPoint(count, pt);

                        m_DkZb[0] += pt.X + ",";
                        m_DkZb[0] += pt.Y + ";";
                    }
                }



                this.axMapControl1.ActiveView.Refresh();
                b_GetDrawDK = false;
                if (this.PostDrawDKClick != null)
                {

                    this.PostDrawDKClick(this.m_DkId, this.m_DkNum, this.m_DkZb);

                }


            }

            #endregion

            #region �鿴����
            if (e.button == 1 && m_bSelTB == true)
            {
                ShowAttribute(e.mapX, e.mapY);
            }
            #endregion
            #region  �Ҽ� ��ݲ˵�
            if (e.button == 2 && m_blStartComputeArea != true && m_blStartComputeDistance != true)
            {
                axMapControl1.CurrentTool = null;
                this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;

                ArrayList arr = new ArrayList();
                IEnumLayer pLayers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
                ILayer pLayer = pLayers.Next();


                while (pLayer != null)
                {
                    if (pLayer.Visible == true)
                    {
                        if (pLayer.Name.IndexOf("���غ˲�") > -1 || pLayer.Name.IndexOf("����˲�") > -1)
                        {
                            arr.Add(pLayer);
                            //if (pLayer.Name == "���غ˲�")
                            //{
                            //    pfeaturelayer = (IFeatureLayer)pLayer;
                            //}                             
                        }

                    }
                    pLayer = pLayers.Next();
                }

                pLayers.Reset();


                IIdentify pIdentify = null;
                IArray pIdentifyArray = null;
                IFeatureIdentifyObj pFeaIdObj;
                IRowIdentifyObject pRowIdObj;
                IIdentifyObj pIdObj;

                IEnvelope pEnvelope = new Envelope() as IEnvelope;
                tagRECT r;

                r.left = e.x - 10;
                r.top = e.y - 10;
                r.right = e.x + 10;
                r.bottom = e.y + 10;

                this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvelope, ref r, 4);
                pEnvelope.SpatialReference = this.axMapControl1.Map.SpatialReference;

                for (int i = 0; i < arr.Count; i++)
                {
                    if (pIdentifyArray == null)
                    {
                        pIdentify = (IIdentify)((ILayer)arr[i]);
                        IFeatureLayer selLayer = arr[i] as IFeatureLayer;
                        pIdentifyArray = pIdentify.Identify(pEnvelope);

                        if (pIdentifyArray == null)
                        {
                            continue;
                        }

                        pFeaIdObj =pIdentifyArray.get_Element(0) as IFeatureIdentifyObj;
                        pRowIdObj = (IRowIdentifyObject)pFeaIdObj;

                        IRow pRow = pRowIdObj.Row;

                        IFeature pFeature = (IFeature)pRow;
                        this.m_hcselfeature = pFeature;
                        pIdObj = (IIdentifyObj)pFeaIdObj;
                        //pIdObj.Flash(this.axMapControl1.ActiveView.ScreenDisplay);//20140527
                        int j = pRow.OID;

                        int ifield = pFeature.Fields.FindField("dkid");

                        object m_objDKID  = pFeature.get_Value(ifield);

                        //���ɶ�һ�޶��ĵؿ�id��������������ʱ��

                        string strdkid = (Microsoft.VisualBasic.Information.IsDBNull(m_objDKID) ? "" : m_objDKID.ToString());

                        if (strdkid.Trim() == "")
                        {
                            strdkid = m_strXZQDM + System.DateTime.Now.ToString().Replace(":", "").Replace("-", "").Replace(@"/", "").Replace(" ", "");

                            clsMapFunction.clsSaveFeatureValue.SaveFeatureValue( selLayer.FeatureClass,pFeature,"DKID", strdkid);
                        }



                        theForm = new JCZF.SubFrame.AttibuteEdit.FormMenuButton();
                        //theForm.m_fileContent = "good"; // �ֺ�����                            

                        theForm.Location = new System.Drawing.Point(e.x, e.y);
                        System.Drawing.Point MouseClickPoint = new System.Drawing.Point(e.x, e.y);
                        //Convert from Tree coordinates to Screen   
                        System.Drawing.Point ScreenPoint = this.axMapControl1.PointToScreen(MouseClickPoint);
                        theForm.Left = ScreenPoint.X;
                        theForm.Top = ScreenPoint.Y;

                        theForm.oid = j;

                        //���ӵؿ�id
                        //theForm.m_strXZQDM = m_strXZQDM;
                        theForm.m_strDKID = strdkid;
                        theForm.sellayname = selLayer.Name;
                        theForm.m_strTabelName = selLayer.Name;
                        theForm.m_strObjecgID = j.ToString();
                        theForm.m_AxMapControl = axMapControl1;
                        theForm.m_frmMapView = this;
                        theForm.m_selFeature = pFeature;
                        theForm.m_selFeatureclass = selLayer.FeatureClass;
                        theForm.m_DataAccess_SYS = this.m_DataAccess_SYS;
                        m_FeatureLayerName = selLayer.Name;

                        JCZF.Renderable.CGlobalVarable.m_theSlideForm = theForm;

                        //theForm. = this;
                        theForm.Show(); // ��ʾ

                    }

                }

            }
            #endregion


            #region ������ť ��ѡ��| ����ͼ��  ����20110731
            #region ѡ��| ����20110731
            if (b_hcselectDK == true)
            {
                ArrayList arr = new ArrayList();
                IEnumLayer pLayers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
                ILayer pLayer = pLayers.Next();

                while (pLayer != null)
                {
                    if (pLayer.Visible == true)
                    {
                        if (pLayer.Name.IndexOf("���غ˲�") > -1 || pLayer.Name.IndexOf("����˲�") > -1)
                        {
                            arr.Add(pLayer);
                        }

                    }
                    pLayer = pLayers.Next();
                }

                pLayers.Reset();

                IIdentify pIdentify = null;
                IArray pIdentifyArray = null;
                IFeatureIdentifyObj pFeaIdObj;
                IRowIdentifyObject pRowIdObj;
                IIdentifyObj pIdObj;

                IEnvelope pEnvelope = new Envelope() as IEnvelope;
                tagRECT r;

                r.left = e.x - 10;
                r.top = e.y - 10;
                r.right = e.x + 10;
                r.bottom = e.y + 10;

                this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvelope, ref r, 4);
                pEnvelope.SpatialReference = this.axMapControl1.Map.SpatialReference;

                for (int i = 0; i < arr.Count; i++)
                {
                    if (pIdentifyArray == null)
                    {
                        pIdentify = (IIdentify)((ILayer)arr[i]);
                        IFeatureLayer selLayer = arr[i] as IFeatureLayer;
                        pIdentifyArray = pIdentify.Identify(pEnvelope);

                        if (pIdentifyArray == null)
                        {
                            continue;
                        }

                        pFeaIdObj =pIdentifyArray.get_Element(0) as IFeatureIdentifyObj;
                        pRowIdObj = (IRowIdentifyObject)pFeaIdObj;

                        IRow pRow = pRowIdObj.Row;

                        IFeature pFeature = (IFeature)pRow;
                        pIdObj = (IIdentifyObj)pFeaIdObj; //dkoid
                        pIdObj.Flash(this.axMapControl1.ActiveView.ScreenDisplay);

                        m_frmSelAnalysis_hc.m_selFeature = pFeature;
                        //FillHCListview(pFeature.ShapeCopy);
                        b_hcselectDK = false;

                    }
                }




            }
            #endregion
            #region ����ͼ��
            if (b_hcDrawDK == true)
            {
                //IGraphicsContainer graphicsContainer = (IGraphicsContainer)this.m_ActiveView.FocusMap;

                //����element
                IGeometry selectedGeometry = this.axMapControl1.TrackPolygon();

                IElement polygonElement = new PolygonElement() as IElement;
                polygonElement.Geometry = selectedGeometry;
                IFillShapeElement fillShapeElement = (IFillShapeElement)polygonElement;
                IFillSymbol fillSymbol = Functions.MapFunction.GetStaticSymbol();
                fillShapeElement.Symbol = fillSymbol;
                graphicsContainer.AddElement(polygonElement, 0);


                //�Ѹ�ͼ�μ��뵽���غ˲�ͼ��   �޸� ���� ��ӵ���ʱͼ�� ������ͼ�����������ڷ����л��ƹ��ܺ͵�������������ͼ���޸�Ϊ������ʱ���ϣ����رշ����Ի����ɾ����ͼ�Σ�

                string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
                //���н��·��
                string m_strResultFilePath = Application.StartupPath + "\\OverlayTemp\\��ʱͼ��" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }


                IFeatureLayer m_hcfeaturelayer = MapFunction.getFeatureLayerByName("���غ˲�", axMapControl1);

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                IFields outfields = m_hcfeaturelayer.FeatureClass.Fields;
                IFeatureClass m_FeatureClass = pFWS.CreateFeatureClass("�ؿ������ʱͼ��", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


                IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
                m_layer.FeatureClass = m_FeatureClass;
                m_layer.Name = m_FeatureClass.AliasName;

                ///��Ⱦ��ͼ��,���Ϊ�п�,20110918,����ΰ
                ISimpleRenderer m_SimpleRenderer = new SimpleRenderer() as ISimpleRenderer;
                IGeoFeatureLayer m_GeoFeatureLayer = (IGeoFeatureLayer)m_layer;
                ISimpleFillSymbol m_SimpleFillSymbol = new SimpleFillSymbol();
                IRgbColor m_rgbColor = new RgbColor() as IRgbColor;
                m_rgbColor.Blue = 10;
                m_rgbColor.Red = 255;
                m_rgbColor.Green = 10;

                m_SimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSHollow;

                m_SimpleFillSymbol.Color = m_rgbColor;
                m_SimpleFillSymbol.Outline.Color = m_rgbColor;
                m_SimpleFillSymbol.Outline.Width = 5;


                m_SimpleRenderer.Symbol = (ISymbol)m_SimpleFillSymbol;
                m_GeoFeatureLayer.Renderer = m_SimpleRenderer as IFeatureRenderer;
                //////////////////////////////////////////////////////////////////////////////////

                this.axMapControl1.Map.AddLayer(m_layer);

                IWorkspaceEdit workspaceEdit = pFWS as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                IFeature feature = m_FeatureClass.CreateFeature();
                feature.Shape = selectedGeometry;

                try
                {
                    feature.Store();
                }
                catch
                {
                    ITopologicalOperator topologicaloperator = selectedGeometry as ITopologicalOperator;
                    topologicaloperator.Simplify();
                    feature.Store();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);

                graphicsContainer.DeleteAllElements();

                this.axMapControl1.ActiveView.Refresh();

                m_frmSelAnalysis_hc.m_selFeature = feature;
                //FillHCListview(feature.ShapeCopy);
                b_hcDrawDK = false;


            }
            #endregion
            #endregion
            #region �������⡢������� ����ΰ20130108
            if (m_blStartComputeDistance_Check == true)
            {
                ComputeDistance_MouseDown(e.mapX, e.mapY);


                /* �����ø��ٲ㣬����ֻ�ܵ������ܵõ�����

                IGraphicsContainer m_IGraphicsContainer = axMapControl1.ActiveView.FocusMap as IGraphicsContainer;
                IGeometry selectedGeometry = this.axMapControl1.TrackLine();

                ILineElement m_ILineElement = new LineElement() as ILineElement;
                m_ILineElement.Symbol =MapFunction.getLineSymbol(0, 255, 0);
                IElement elementLine = null;
                elementLine = m_ILineElement as IElement;
                elementLine.Geometry = selectedGeometry as IGeometry;
                m_IGraphicsContainer.AddElement(elementLine, 0);

                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                */
            }

            if (m_blStartComputeArea_Check == true)
            {
                ComputeArea_MouseDown(e.mapX, e.mapY);
            }

            if (e.button == 2 && (m_blStartComputeArea == true || m_blStartComputeDistance == true))
            {
                if (m_blStartComputeArea == true)
                {
                    ComputeArea_MouseDown(e.mapX, e.mapY);
                }

                if (m_blStartComputeDistance == true)
                {
                    ComputeDistance_DoubleClick(e.mapX, e.mapY);
                }

            }

            #endregion
        }


        /// <summary>
        /// ������ѡ���ͼ�İ�ť״̬
        /// </summary>
        /// <param name="p_IMap"></param>
        /// <param name="p_intIndex"></param>
        private void SetSelectedMapBtiOption()
        {
            //IEnumLayer m_IEnumLayer;
            //m_IEnumLayer=clsMapFunction.MapFunction.GetGroupLayers(p_AxMapControl.Map );
            //if(m_IEnumLayer==null ) return ;
            //ILayer m_ILayer, m_ILayerTemp;
            //ArrayList m_ArrayListILayer;

            //m_ILayer = m_IEnumLayer.Next();
            //while (m_ILayer != null)
            //{
            //    if (m_ILayer.Name.Contains("��������") && m_ILayer.Visible == true)
            //    {
            //        m_ArrayListILayer=clsMapFunction.MapFunction.GetLayerFromGroupLayer(m_ILayer);

            //        if (m_ILayer.Name.Contains("����ͼ��") && clsMapFunction.MapFunction.IslayerVisible(m_ILayer, p_AxMapControl))
            //        {

            //        }
            //    }
            //    else if (m_ILayer.Name.Contains("��������") && m_ILayer.Visible == true)
            //    {
            //        if (m_ILayer.Name.Contains("����ͼ��") && clsMapFunction.MapFunction.IslayerVisible(m_ILayer, p_AxMapControl))
            //        {

            //        }
            //    }
            //}
            //m_IEnumLayer.Reset();
            DevComponents.DotNetBar.ButtonItem m_ButtonItemTemp;
            bool m_blChecked = false;
            ILayer m_ILayer, m_ILayerTemp;
            ArrayList m_ArrayList;
            int m_intTempItemsChecked = 0;

            try
            {

                #region ң��Ӱ�񹤾�������
                if (m_ILayerImage != null)
                {
                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = m_ILayerImage;
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            for (int j = 0; j < m_ImageShow.SubItems.Count; j++)
                            {
                                if (m_ImageShow.SubItems[j].Text == m_ILayer.Name)
                                {
                                    m_ButtonItemTemp = (DevComponents.DotNetBar.ButtonItem)m_ImageShow.SubItems[j];
                                    m_ButtonItemTemp.Checked = m_ILayer.Visible ;
                                    if (m_ButtonItemTemp.Checked)
                                    {
                                        m_intTempItemsChecked++;
                                    }
                                }
                            }
                        }
                    }
                    if (m_ILayerTemp.Visible = true && m_intTempItemsChecked == 0)
                    {
                        m_ImageShow.Checked = false;
                    }
                    else if (m_ILayerTemp.Visible = true && m_intTempItemsChecked > 0)
                    {
                        m_ImageShow.Checked = true;
                    }
                    else if (m_ILayerTemp.Visible = false)
                    {
                        m_ImageShow.Checked = false;
                    }

                }
                #endregion
                
                #region ����������״����������
                if (m_ILayerTDLY != null )
                {
                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayerTDLY;
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];
                            for (int j = 0; j < btnTDLYXZ.SubItems.Count; j++)
                            {
                                if (btnTDLYXZ.SubItems[j].Text == m_ILayer.Name)
                                {
                                    m_ButtonItemTemp = (DevComponents.DotNetBar.ButtonItem)btnTDLYXZ.SubItems[j];
                                    m_ButtonItemTemp.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer, axMapControl1);
                                    if (m_ButtonItemTemp.Checked)
                                    {
                                        m_intTempItemsChecked++;
                                    }
                                }
                            }
                        }
                    }

                    if (m_ILayerTemp.Visible = true && m_intTempItemsChecked == 0)
                    {
                        btnTDLYXZ.Checked = false;
                    }
                    else if (m_ILayerTemp.Visible = true && m_intTempItemsChecked > 0)
                    {
                        btnTDLYXZ.Checked = true;
                    }
                    else if (m_ILayerTemp.Visible = false)
                    {
                        btnTDLYXZ.Checked = false;
                    }
                }
                #endregion
                #region �������ù滮����������
                if (m_ILayerTDGH != null )
                {
                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayerTDGH;
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnTDLYGH.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer, axMapControl1 );
                            if (btnTDLYGH.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btnTDLYGH.Checked = m_blChecked;
                }
                #endregion
                #region �������ù�Ӧ����������
                if (m_ILayerTDGY != null)
                {


                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayerTDGY;
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btiTDGY.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer,axMapControl1 );
                            if (btiTDGY.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btiTDGY.Checked = m_blChecked;
                }
                #endregion
                #region �ɿ�Ȩ����������
                if (m_ILayerCKQ != null)
                {


                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayerCKQ;
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnCKQFZ.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer, axMapControl1 );
                            if (btnCKQFZ.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btnCKQFZ.Checked = m_blChecked;
                }
                #endregion
                #region ̽��Ȩ����������
                if (m_ILayerTKQ != null )
                {

                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayerTKQ;
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnTKQFZ.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer, axMapControl1 );
                            if (btnTKQFZ.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btnTKQFZ.Checked = m_blChecked;
                }
                #endregion
                #region ����ũ�﹤��������
                if (m_ILayerJBNT != null)
                {

                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayerJBNT;
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnJBNTBH.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer, axMapControl1);
                            if (btnJBNTBH.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btnJBNTBH.Checked = m_blChecked;
                }
                #endregion

            }
            catch (SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
            bar2.Refresh();

            //IGroupLayer pGroupLayer = (IGroupLayer)pglayer;



            //m_ImageShows = new DevComponents.DotNetBar.ButtonItem[m_ArrayList.Count];
            //for (int i = 0; i < m_ArrayList.Count; i++)
            //{
            //    m_ImageShows[i] = new DevComponents.DotNetBar.ButtonItem();
            //    m_ImageShows[i].Text = ((ILayer)m_ArrayList[i]).Name;
            //    m_ImageShow.SubItems.Add(m_ImageShows[i] as DevComponents.DotNetBar.BaseItem);
            //    m_ImageShows[i].Click += new System.EventHandler(ImageButton_Click);
            //}
            //bar2.Refresh();

        }
        /// <summary>
        /// ��õ�ͼ�п��ƹ�������Ӧ��ͼ���飨grouplayer�������ڿ��ƹ�������ѡ�����
        /// </summary>
        /// <param name="p_IMap"></param>
        /// <param name="p_intIndex"></param>
        private void GetBtiLayers(AxMapControl p_AxMapControl)
        {
            try
            {
                //if (m_ILayerImage == null) m_ILayersImage = new ArrayList();
                //if (m_ILayersTDLY == null) m_ILayersTDLY = new ArrayList();
                //if (m_ILayersTDGY == null) m_ILayersTDGY = new ArrayList();
                //if (m_ILayersJBNT == null) m_ILayersJBNT = new ArrayList();
                //if (m_ILayersTKQ == null) m_ILayersTKQ = new ArrayList();
                //if (m_ILayersCKQ == null) m_ILayersCKQ = new ArrayList();
                //if (m_ILayersJSYD == null) m_ILayersJSYD = new ArrayList();
                //if (m_ILayersTDGH == null) m_ILayersTDGH = new ArrayList();

                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(p_AxMapControl.Map);

                ILayer pglayer = m_grouplayer.Next();
                while (pglayer != null)
                {
                    if (pglayer.Name.Contains("ң��Ӱ��"))
                    {
                        m_ILayerImage=pglayer;
                        
                    }
                    else if (pglayer.Name.Contains("����������״"))
                    {
                        m_ILayerTDLY=pglayer;
                    }
                    else if (pglayer.Name.Contains("�������ù滮"))
                    {
                        m_ILayerTDGH=pglayer;
                    }
                    else if (pglayer.Name.Contains("���ع�Ӧ"))
                    {
                        m_ILayerTDGY=pglayer;
                    }
                    else if (pglayer.Name.Contains("�����õ�"))
                    {
                        m_ILayerJSYD = pglayer;
                    }
                    else if (pglayer.Name.Contains("�ɿ�Ȩ"))
                    {
                        m_ILayerCKQ=pglayer;
                    }
                    else if (pglayer.Name.Contains("̽��Ȩ"))
                    {
                        m_ILayerTKQ=pglayer;
                    }
                    else if (pglayer.Name.Contains("����ũ��"))
                    {
                        m_ILayerJBNT=pglayer;
                    }
                    pglayer = m_grouplayer.Next();
                }
                m_grouplayer.Reset();
            }
            catch (SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
        }


        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            //����
            string[] m_strXZQDM_MC = new string[2];
            string m_strWZ = "";

            panelEx_ZFRY_Info.Visible = false;

            this.labCoordinate.Text = "" + "X:" + e.mapX.ToString(".00") + "; Y:" + e.mapY.ToString(".00") + " " + m_sMapUnits;
            //txtX.Text = e.mapX.ToString(".00");
            //txtY.Text = e.mapY.ToString(".00");
            this.labCoordinate.Refresh();
            if (Math.Abs(m_tempX - e.mapX) > 500 || Math.Abs(m_tempY - e.mapY) > 500)
            {
                //if (axMapControl1.MapScale < 1000000)
                //{
                //    //�������߽ϴ�ʱ��ʾ��ǰλ��
                //    string m_strWZ = "�ں���";
                //    m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY);
                //    this.tssPosition.Text = m_strWZ;

                //    m_tempX = e.mapX;
                //    m_tempY = e.mapY;
                //}
                //else
                //{
                //    this.tssPosition.Text = "�ں���";
                //    this.panelEx1.Visible = false;
                //}
                //���м���������ʾʱ��
                if (this.axMapControl1.MapScale < 8000000 && axMapControl1.MapScale > 1500000)
                {
                    //if (GetCurrentXZQYName(e.mapX, e.mapY) != "")
                    //    ShowPanel((int)e.x, (int)e.y, "����");


                    try
                    {                   

                        m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "XZQDM", "XZQMC");
                        m_strXZQDM = m_strXZQDM_MC[0];
                        m_strWZ = m_strXZQDM_MC[1];

                    }
                    catch
                    {
                        try
                        {
                            //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "QHDM");
                            m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "QHDM", "XZQM");
                            m_strXZQDM = m_strXZQDM_MC[0];
                            m_strWZ = m_strXZQDM_MC[1];
                        }
                        catch
                        {
                            try
                            {
                                //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "DM");
                                m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "DM", "MC");
                                m_strXZQDM = m_strXZQDM_MC[0];
                                m_strWZ = m_strXZQDM_MC[1];
                            }
                            catch (SystemException errs)
                            {
                                clsFunction.Function.MessageBoxError(errs.Message);
                            }
                        }
                    }

                    
                    //m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY, "��");

                  

                    if (m_strXZQDM != "")
                    {
                        if (Isshowpanel == true)
                        {
                            ShowPanel((int)e.x, (int)e.y, m_strXZQDM);
                        }
                        //m_strDM1 = m_strXZQDM;
                    }
                }

                //���ؼ���������ʾʱ��
                if (axMapControl1.MapScale < 1500000 && axMapControl1.MapScale > 400000)
                {
                    //try
                    //{
                    //    m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "XZQDM");
                    //}
                    //catch
                    //{
                    //    try
                    //    {
                    //        m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "QHDM");
                    //    }
                    //    catch
                    //    {
                    //        try
                    //        {
                    //            m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "DM");
                    //        }
                    //        catch( SystemException errs)
                    //        {
                    //            clsFunction.Function.MessageBoxError(errs.Message);
                    //        }
                    //    }
                    //}

                    //string m_strWZ = "";
                    //m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY, "��");

                    try
                    {

                        m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "XZQDM", "XZQMC");
                        m_strXZQDM = m_strXZQDM_MC[0];
                        m_strWZ = m_strXZQDM_MC[1];

                    }
                    catch
                    {
                        try
                        {
                            //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "QHDM");
                            m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "QHDM", "XZQM");
                            m_strXZQDM = m_strXZQDM_MC[0];
                            m_strWZ = m_strXZQDM_MC[1];
                        }
                        catch
                        {
                            try
                            {
                                //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "DM");
                                m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "DM", "MC");
                                m_strXZQDM = m_strXZQDM_MC[0];
                                m_strWZ = m_strXZQDM_MC[1];
                            }
                            catch (SystemException errs)
                            {
                                clsFunction.Function.MessageBoxError(errs.Message);
                            }
                        }
                    }

                  

                   

                    if (m_strXZQDM != "" )
                    {
                        if (Isshowpanel == true)
                        {
                            ShowPanel((int)e.x, (int)e.y, m_strXZQDM);
                        }
                        //m_strDM1 = m_strXZQDM;
                    }
                }

                //��������������ʾʱ��
                if (axMapControl1.MapScale < 400001 && axMapControl1.MapScale > 80000)
                {
                    //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "QHDM");


                    //try
                    //{
                    //    m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "XZQDM");
                    //}
                    //catch
                    //{
                    //    try
                    //    {
                    //        m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "QHDM");
                    //    }
                    //    catch
                    //    {
                    //        try
                    //        {
                    //            m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "DM");
                    //        }
                    //        catch (SystemException errs)
                    //        {
                    //            clsFunction.Function.MessageBoxError(errs.Message);
                    //        }
                    //    }
                    //}

                    //string m_strWZ = "";
                    //m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY, "��");

                    try
                    {

                        m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "XZQDM", "XZQMC");
                        m_strXZQDM = m_strXZQDM_MC[0];
                        m_strWZ = m_strXZQDM_MC[1];

                    }
                    catch
                    {
                        try
                        {
                            //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "QHDM");
                            m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "QHDM", "XZQM");
                            m_strXZQDM = m_strXZQDM_MC[0];
                            m_strWZ = m_strXZQDM_MC[1];
                        }
                        catch
                        {
                            try
                            {
                                //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "DM");
                                m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "DM", "MC");
                                m_strXZQDM = m_strXZQDM_MC[0];
                                m_strWZ = m_strXZQDM_MC[1];
                            }
                            catch (SystemException errs)
                            {
                                clsFunction.Function.MessageBoxError(errs.Message);
                            }
                        }
                    }

                       
                    
                    if (m_strXZQDM != "" )
                    {
                        if (Isshowpanel == true)
                        {
                            ShowPanel((int)e.x, (int)e.y, m_strXZQDM);
                        }
                        //m_strDM1 = m_strXZQDM;
                    }
                }

                //������������ʾʱ��
                if (axMapControl1.MapScale < 80001)
                {
                    //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "XZQDM");


                    //try
                    //{
                    //    m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "XZQDM");
                    //}
                    //catch
                    //{
                    //    try
                    //    {
                    //        m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "QHDM");
                    //    }
                    //    catch
                    //    {
                    //        try
                    //        {
                    //            m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "DM");
                    //        }
                    //        catch (SystemException errs)
                    //        {
                    //            clsFunction.Function.MessageBoxError(errs.Message);
                    //        }
                    //    }
                    //}

                    //string m_strWZ = "";
                    //m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY, "��");

                    try
                    {

                        m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "XZQDM", "XZQMC");
                        m_strXZQDM = m_strXZQDM_MC[0];
                        m_strWZ = m_strXZQDM_MC[1];

                    }
                    catch
                    {
                        try
                        {
                            //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "�弶������", "QHDM");
                            m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "QHDM", "XZQM");
                            m_strXZQDM = m_strXZQDM_MC[0];
                            m_strWZ = m_strXZQDM_MC[1];
                        }
                        catch
                        {
                            try
                            {
                                //m_strXZQDM = GetCurrentXZQYDM(e.mapX, e.mapY, "��", "DM");
                                m_strXZQDM_MC = GetCurrentXZQYDM_MC(e.mapX, e.mapY, "��", "DM", "MC");
                                m_strXZQDM = m_strXZQDM_MC[0];
                                m_strWZ = m_strXZQDM_MC[1];
                            }
                            catch (SystemException errs)
                            {
                                clsFunction.Function.MessageBoxError(errs.Message);
                            }
                        }
                    }

                                     

                    //if (m_strXZQDM != "" && m_strXZQDM != m_strDM1)
                    if (m_strXZQDM != "")
                    {
                        if (Isshowpanel == true)
                        {
                            ShowPanel((int)e.x, (int)e.y, m_strXZQDM);
                        }
                     
                    }
                }

            }

            this.labeldetailDZ.Text = m_strWZ;   
            this.tssPosition.Text = m_strWZ;  

            tssPosition.Refresh();

            //uctXZQTree1.m_strXZQDM = m_strXZQDM;

            #region  ���롢������� ����ΰ 20130108
            if (m_blStartComputeDistance == true)
            {
                ComputeDistance_MouseMove(e.mapX, e.mapY);
            }

            //if (m_blStartComputeDistance == true)
            //{
            //    ComputeDistance_MouseMove(e.mapX, e.mapY);
            //}
           #endregion
        }



        private void ShowPanel(int p_intX, int p_intY, string QHDM)
        {
            string BJRY = "";
            string SJRY = "";

            this.panelEx1.Visible = true;
            this.panelEx1.Left = p_intX +5;
            this.panelEx1.Top = p_intY+5;

            IPoint m_Point = this.m_ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(p_intX, p_intY);

            if (QHDM != "")
            {
                getBJRYfromQHDM(QHDM);
                if (m_DataTableBJZFRY != null)
                {
                    for (int i = 0; i < m_DataTableBJZFRY.Rows.Count; i++)
                    {
                        if (m_DataTableBJZFRY.Rows[i][0] != null)
                        {
                            if (i == 0)
                            {
                                BJRY =  m_DataTableBJZFRY.Rows[i][0].ToString();
                            }
                            else
                            {
                                BJRY = BJRY+"��" + m_DataTableBJZFRY.Rows[i][0].ToString();
                            }
                        }
                    }
                }

                getSJRYfromQHDM(QHDM);
                if (m_DataTableSJZFRY != null)
                {
                    for (int i = 0; i < m_DataTableSJZFRY.Rows.Count; i++)
                    {
                        if (m_DataTableSJZFRY.Rows[i][0] != null)
                        {
                            if (i == 0)
                            {
                                SJRY = m_DataTableSJZFRY.Rows[i][0].ToString();
                            }
                            else
                            {
                                SJRY = SJRY+"��" + m_DataTableSJZFRY.Rows[i][0].ToString();
                            }
                        }
                    }

                }

                this.labelX5.Text = SJRY.ToString();
                this.labelX6.Text = BJRY.ToString();
                if (labelX6.Text == "")
                {
                    linkLabel1.Enabled = false;
                }
                if (labelX5.Text == "")
                {
                    linkLabel2.Enabled = false;
                }
            }


        }

        /// <summary>
        ///���������� ��ȡ����ִ����Ա����
        /// </summary>
        /// <param name="XZQM"></param>
        /// <returns></returns>
        private DataTable  getBJRYfromXZQM(string XZQM)
        {
            //string BJRY = "";
           
             //m_strBJZFRY = null;
             //m_strBJZFRY = new string[1, 3];

            string sql = "SELECT ZFRY.zfryxm,ZFRY.BGDH,ZFRY.SJ FROM ZFWG INNER JOIN XZQH ON ZFWG.xzqdm = XZQH.QHDM INNER JOIN ZFRY ON ZFWG.BJZFRYbh = ZFRY.zfrybh WHERE (XZQH.XZQM = '" + XZQM + "')  ";
            m_DataTableBJZFRY= m_DataAccess_SYS.getDataTableByQueryString(sql);

            //if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
            //{
            //    BJRY = m_DataRowCollection[0][0].ToString();
            //}

            return m_DataTableBJZFRY;
        }

        /// <summary>
        ///������������ ��ȡ����ִ����Ա����
        /// </summary>
        /// <param name="XZQM"></param>
        /// <returns></returns>
        private DataTable getBJRYfromQHDM(string QHDM)
        {
             //m_strBJZFRY = null;
            

            //string BJRY = "";
            string sql = "SELECT ZFRY.zfryxm,ZFRY.BGDH,ZFRY.SJ FROM ZFWG INNER JOIN ZFRY ON ZFWG.BJZFRYbh = ZFRY.zfrybh  WHERE (ZFWG.xzqdm = '" + QHDM + "') and JSSJ IS  NULL";
            m_DataTableBJZFRY = m_DataAccess_SYS.getDataTableByQueryString(sql);

            

            //if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
            //{
            //    m_strBJZFRY = new string[m_DataRowCollection.Count, 3];
            //    for (int i = 0; i < m_DataRowCollection.Count; i++)
            //    {
            //        //BJRY += m_DataRowCollection[i][0].ToString() + ' ' ;

            //        m_strBJZFRY[i, 0] = m_DataRowCollection[i][0].ToString();
            //        m_strBJZFRY[i, 1] = m_DataRowCollection[i][1].ToString();
            //        m_strBJZFRY[i, 2] = m_DataRowCollection[i][2].ToString();
            //    }
            //}

            return m_DataTableBJZFRY;
        }

        /// <summary>
        ///������������ ��ȡ�ϼ�ִ����Ա����
        /// </summary>
        /// <param name="XZQM"></param>
        /// <returns></returns>
        private DataTable   getSJRYfromQHDM(string QHDM)
        {
            //m_strSJZFRY = null;
            
            //string SJRY = "";
            string sql = "SELECT ZFRY.zfryxm,ZFRY.BGDH,ZFRY.SJ FROM ZFWG INNER JOIN ZFRY ON ZFWG.SJZFRYbh = ZFRY.zfrybh WHERE (ZFWG.xzqdm = '" + QHDM + "') and JSSJ IS  NULL";
            m_DataTableSJZFRY = m_DataAccess_SYS.getDataTableByQueryString(sql);


            //if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
            //{
            //    m_strSJZFRY = new string[m_DataRowCollection.Count, 3];
            //    for (int i = 0; i < m_DataRowCollection.Count; i++)
            //    {
            //        //SJRY += m_DataRowCollection[i][0].ToString()+' ';

            //        m_strSJZFRY[i, 0] = m_DataRowCollection[i][0].ToString();
            //        m_strSJZFRY[i, 1] = m_DataRowCollection[i][1].ToString();
            //        m_strSJZFRY[i, 2] = m_DataRowCollection[i][2].ToString();
            //    }
            //}

            return m_DataTableSJZFRY;
        }



        /// <summary>
        /// ��ȡ��ǰ������������������
        /// </summary>
        /// <param name="p_dbX">x����</param>
        /// <param name="p_dbY">y����</param>
        /// <param name="layername"></param>
        /// <returns></returns>
        private string GetCurrentXZQYDM(double p_dbX, double p_dbY, string layername, string strField)
        {
            string m_strXZQDM = "";

            try
            {
                IEnumLayer m_Layers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
                ILayer m_Layer = m_Layers.Next();

                while (m_Layer != null)
                {
                    if (m_Layer.Name == layername)
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        if (m_Feature == null) return "";
                        m_strXZQDM = m_Feature.get_Value(m_Feature.Fields.FindField(strField)).ToString();
                        //MessageBox.Show(m_strXZQDM.ToString());
                        //m_pFrmMain.textEvents.Text = m_pFrmMain.textEvents.Text + "  " + m_strXZQDM.ToString();

                        break;
                    }
                    m_Layer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
            catch (SystemException errs)
            {
                throw errs;
            }

            return m_strXZQDM.ToString();

        }


        /// <summary>
        /// ��ȡ��ǰ�����������������������
        /// </summary>
        /// <param name="p_dbX">x����</param>
        /// <param name="p_dbY">y����</param>
       /// <param name="layername"></param>
       /// <param name="strFieldName_XZQDM"></param>
       /// <param name="strFieldName_XZQMC"></param>
       /// <returns>�������������������</returns>
        private string[] GetCurrentXZQYDM_MC(double p_dbX, double p_dbY, string layername, string strFieldName_XZQDM,string strFieldName_XZQMC)
        {
            string[] m_strXZQYDM_MC = new string[2];
            string m_strXZQDM = "";
            string m_strMC = "";

            m_strXZQYDM_MC[0] = "";//����������
            m_strXZQYDM_MC[1] = "";
            try
            {
                IEnumLayer m_Layers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
                ILayer m_Layer = m_Layers.Next();

                while (m_Layer != null)
                {
                    if (m_Layer.Name == layername)
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        if (m_Feature == null) return m_strXZQYDM_MC;
                        m_strXZQDM = m_Feature.get_Value(m_Feature.Fields.FindField(strFieldName_XZQDM)).ToString();
                        m_strMC = m_Feature.get_Value(m_Feature.Fields.FindField(strFieldName_XZQMC)).ToString();
                        m_strXZQYDM_MC[0] = m_strXZQDM;
                        m_strXZQYDM_MC[1] = m_strMC;

                        break;
                    }
                    m_Layer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
            catch (SystemException errs)
            {
                throw errs;
            }

            return m_strXZQYDM_MC;

        }




        /// <summary>
        /// ���������������
        /// </summary>
        /// <param name="m_Feature"></param>
        /// <returns></returns>
        private string m_strGetMC(IFeature m_Feature)
        {
            if (m_Feature == null) return "";
            string m_strName = "";
            try
            {

                m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("XZQMC")).ToString();
            }
            catch
            {
                try
                {

                    m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("XZQM")).ToString();
                }
                catch
                {
                    try
                    {

                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("MC")).ToString();
                    }
                    catch
                    {

                    }
                }
            }
            return m_strName;
        }

        /// <summary>
        /// ��õ�ǰ����������������
        /// </summary>
        /// <param name="p_dbX"></param>
        /// <param name="p_dbY"></param>
        /// <returns></returns>
        private string GetCurrentXZQYName(double p_dbX, double p_dbY,string p_strXZJB)
        {
            string m_strName = "";
            //string m_strName_Sheng = "";
            string m_strName_Shi = "";
            string m_strName_Xian = "";
            string m_strName_Xiang = "";
            string m_strName_Cun = "";
            this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;

            IEnumLayer m_Layers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
            ILayer m_Layer = m_Layers.Next();

            while (m_Layer != null)
            {
                ///workͼ����Ų�Ҫ�䶯
                ///
                if (m_Layer.Name == "��")
                {
                    IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                    m_strName_Shi = m_strGetMC(m_Feature);
                }
                if (p_strXZJB == "��")
                {
                    m_strName = m_strName_Shi;
                    break;
                }

                if (m_Layer.Name == "��")
                {
                    IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                    m_strName_Xian = m_strGetMC(m_Feature);
                }
                if (p_strXZJB == "��")
                {
                    m_strName = m_strName_Shi + m_strName_Xian;
                    break;
                }

                if (m_Layer.Name == "��")
                {
                    IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                    m_strName_Xiang = m_strGetMC(m_Feature);
                }
                if (p_strXZJB == "��")
                {
                    m_strName = m_strName_Shi + m_strName_Xian + m_strName_Xiang;
                    break;
                }

                if (m_Layer.Name == "��")
                {
                    IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                    m_strName_Cun = m_strGetMC(m_Feature);
                }
                if (p_strXZJB == "��")
                {
                    m_strName = m_strName_Shi + m_strName_Xian + m_strName_Xiang + m_strName_Cun;
                    break;
                }


                //if (str == "��")
                //{
                //    if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName_Sheng=m_strGetMC(m_Feature);
                //        //try
                //        //{

                //        //    m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("XZQMC")).ToString();
                //        //}
                //        //catch
                //        //{
                //        //    try
                //        //    {

                //        //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("XZQM")).ToString();
                //        //    }
                //        //    catch
                //        //    {
                //        //        try
                //        //        {

                //        //            m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("MC")).ToString();
                //        //        }
                //        //        catch
                //        //        {
                                    
                //        //        }
                //        //    }
                //        //}
                //        break;
                //    }
                //}
                //else if (str == "��")
                //{
                //    if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("MC")).ToString();
                //        //break;
                //    }
                //    if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("MC")).ToString();
                //        break;
                //    }
                //}

                //else if (str == "��")
                //{
                //    if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("MC")).ToString();
                //        //break;
                //    }
                //    if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("MC")).ToString();
                //        //break;
                //    }

                //    if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("XZQM")).ToString();
                //        //break;
                //    }
                //}

                //else if (str == "��")
                //{
                //    if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("MC")).ToString();
                //        //break;
                //    }
                //    else if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("MC")).ToString();
                //        //break;
                //    }

                //    else  if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("XZQM")).ToString();
                //        //break;
                //    }
                //    else  if (m_Layer.Name == "��")
                //    {
                //        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                //        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("XZQMC")).ToString();
                //        break;
                //    }
                //}
              

                m_Layer = m_Layers.Next();
            }
            m_Layers.Reset();

            return m_strName;
        }

        private void axMapControl1_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            //��ʾ����
            double m_dScale = this.axMapControl1.MapScale;
            int m_intScale = (int)m_dScale;
            string m_strScale = m_intScale.ToString();

            this.labScale.Text = m_strScale;
            //this.txtScale.Text = m_strScale;
        }

        private void frmMapView_Activated(object sender, EventArgs e)
        {
            frmMapView_Activate(true);
        }

        private void frmMapView_Deactivate(object sender, EventArgs e)
        {
            frmMapView_Activate(false );
        }

        #region ��ͼ�л�
        //����������״ͼ
        private void btnTDLYXZ_Click(object sender, EventArgs e)
        {
            if (btnTDLYXZ.Checked == false)
            {
                btnTDLYXZ.Checked = true;
            }
            else
            {
                btnTDLYXZ.Checked = false;
            }
            //ILayer rootGroupLayer = MapFunction.GetGroupLayerByName(axMapControl1.Map, "����������״");
            if (m_ILayerTDLY == null)
            {
                return;
            }
            if (btnTDLYXZ.Checked == true)
            {
                m_ILayerTDLY.Visible = true;
                
                //������ͼ������ʾ
                ArrayList m_ArrayList = new ArrayList();
                IGroupLayer m_GroupLayerTDLY = (IGroupLayer)m_ILayerTDLY;
                m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_GroupLayerTDLY);
                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    ILayer layer = m_ArrayList[i] as ILayer;
                    layer.Visible = true;
                }
                ////this.TLControl.SelectedTab = this.TDLYti;
                //ArrayList arr;
                //IGeometry geo = axMapControl1.Extent as IGeometry;
                //IFeatureLayer featurelayer = (IFeatureLayer)MapFunction.getFeatureLayerByName("��", axMapControl1);
                //arr = MapFunction.Overlay(geo, featurelayer, "XZQM");
                //IEnumLayer m_Layers;
                //m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
                //ILayer pLayer = m_Layers.Next();
                //while (pLayer != null)
                //{
                //    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                //    for (int i = 0; i < arr.Count; i++)
                //    {
                //        if (pFeaturelay.Name.Contains(arr[i].ToString()))
                //        {
                //            if (pFeaturelay.Name.Contains("�����߽�") || pFeaturelay.Name.Contains("��������") || pFeaturelay.Name.Contains("����ͼ��") || pFeaturelay.Name.Contains("��״����") || pFeaturelay.Name.Contains("���ǵ���"))
                //            {
                //                pFeaturelay.Visible = true;
                //            }
                //            break;
                //        }
                //    }
                //    pLayer = m_Layers.Next();
                //}
                //m_Layers.Reset();
            }
            else
            {
                m_ILayerTDLY.Visible = false;
                //������ͼ��������
                ArrayList m_ArrayList = new ArrayList();
                IGroupLayer m_GroupLayerTDLY = (IGroupLayer)m_ILayerTDLY;
                m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_GroupLayerTDLY);
                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    ILayer layer = m_ArrayList[i] as ILayer;
                    layer.Visible = false;
                }
                //IEnumLayer m_Layers;
                //m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                //ILayer pLayer = m_Layers.Next();
                //IFeatureLayer pFeaturelay = new FeatureLayer() as IFeatureLayer;
                //while (pLayer != null)
                //{
                //    pFeaturelay = (IFeatureLayer)pLayer;
                //    if (pFeaturelay.Name.Contains("����ͼ��") || pFeaturelay.Name.Contains("��״����") || pFeaturelay.Name.Contains("���ǵ���"))
                //    {
                //        pFeaturelay.Visible = false;

                //    }
                //    pLayer = m_Layers.Next();
                //}
                //m_Layers.Reset();

            }
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        private void SetFeatureLayerVisible()
        {
            IEnumLayer m_Layers;
            m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            ILayer pLayer = m_Layers.Next();
            while (pLayer != null)
            {
                IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                if (pFeaturelay.Name.Contains("�����߽�") || pFeaturelay.Name.Contains("��������"))
                {
                    pFeaturelay.Visible = true;
                }

                pLayer = m_Layers.Next();
            }
            m_Layers.Reset();
        }

        private void setDefaultVisible()
        {
            IEnumLayer m_grouplayer;
            m_grouplayer = Functions.MapFunction.GetGroupLayers(axMapControl1.ActiveView.FocusMap);
            ILayer pglayer = m_grouplayer.Next();
            while (pglayer != null)
            {
                IGroupLayer pGroupLayer = (IGroupLayer)pglayer;
                if (pGroupLayer.Name == "������")  //������������ʼ�տɼ�
                {
                    pGroupLayer.Visible = true;
                    ArrayList m_ArrayList;
                    ILayer m_Layer;
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
                    for (int i = 0; i < m_ArrayList.Count; i++)
                    {
                        m_Layer = (ILayer)m_ArrayList[i];
                        m_Layer.Visible = true;
                    }

                }
                else
                {
                    if (pGroupLayer.Name == "2003" || pGroupLayer.Name == "2005" || pGroupLayer.Name == "2007")
                    {
                        //pGroupLayer.Visible = true;
                    }
                    else
                    {
                        pGroupLayer.Visible = true;
                    }
                }

                pglayer = m_grouplayer.Next();

            }
            m_grouplayer.Reset();


        }


        private void btnTDLYGH_Click(object sender, EventArgs e)
        {

            if (this.btnTDLYGH.Checked == false)
            {
                this.btnTDLYGH.Checked = true;
            }
            else
            {
                btnTDLYGH.Checked = false;
            }

            
         
            if (m_ILayerTDGH == null) return;
            m_ILayerTDGH.Visible = this.btnTDLYGH.Checked;

            //if (this.btnTDLYGH.Checked == false)
            //{
            //    this.btnTDLYGH.Checked = true;
            //}
            //else
            //{
            //    btnTDLYGH.Checked = false;
            //}

            //if (this.btnTDLYGH.Checked == true)
            //{
            //    //this.TLControl.SelectedTab = this.TDLYGHti;
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        //if (pFeaturelay.Name == "��״������Ŀ" || pFeaturelay.Name == "��״������Ŀ" || pFeaturelay.Name.Contains("�滮ͼ"))
            //        if (pFeaturelay.Name.Contains("�滮ͼ"))
            //        {
            //            pFeaturelay.Visible = true;
            //        }

            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();

            //}
            //else
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("�滮ͼ"))
            //        {
            //            pFeaturelay.Visible = false;
            //        }

            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();

            //}
            axMapControl1.ActiveView.Refresh();

        }



        //����ũ�ﱣ��ͼ
        private void btnJBNTBH_Click(object sender, EventArgs e)
        {
            if (this.btnJBNTBH.Checked == false)
            {
                this.btnJBNTBH.Checked = true;
            }
            else
            {
                btnJBNTBH.Checked = false;
            }

            //if (m_ILayerJBNT == null) return;
            //m_ILayerJBNT.Visible = this.btnJBNTBH.Checked;

            //ILayer rootGroupLayer = MapFunction.GetGroupLayerByName(axMapControl1.Map, "����ũ��");
            if (m_ILayerJBNT == null)
            {
                return;
            }
            if (btnJBNTBH.Checked == true)
            {
                m_ILayerJBNT.Visible = true;

                //������ͼ������ʾ
                ArrayList m_ArrayList = new ArrayList();
                IGroupLayer m_GroupLayerTDLY = (IGroupLayer)m_ILayerJBNT;
                m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_GroupLayerTDLY);
                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    ILayer layer = m_ArrayList[i] as ILayer;
                    layer.Visible = true;
                }
            }
            else
            {
                m_ILayerJBNT.Visible = false;
                //������ͼ������ʾ
                ArrayList m_ArrayList = new ArrayList();
                IGroupLayer m_GroupLayerTDLY = (IGroupLayer)m_ILayerJBNT;
                m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_GroupLayerTDLY);
                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    ILayer layer = m_ArrayList[i] as ILayer;
                    layer.Visible = false;
                }
            }
            
            axMapControl1.ActiveView.Refresh();
        }





        private void btnJSXM_Click(object sender, EventArgs e)
        {
            if (this.btnJSXM.Checked == false)
            {
                this.btnJSXM.Checked = true;
            }
            else
            {
                btnJSXM.Checked = false;
            }
            if (m_ILayerJSYD == null) return;
            m_ILayerJSYD.Visible = this.btnJSXM.Checked;
            //if (this.btnJSXM.Checked == true)
            //{
            //    //this.TLControl.SelectedTab = this.JSXMti;

            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("�����õ�"))
            //        {
            //            pFeaturelay.Visible = true;
            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}
            //else
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("�����õ�"))
            //        {
            //            pFeaturelay.Visible = false;
            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}
            axMapControl1.ActiveView.Refresh();
        }

        private void btnKCZYGH_Click(object sender, EventArgs e)
        {
            if (this.btnKCZYGH.Checked == false)
            {
                this.btnKCZYGH.Checked = true;
            }
            else
            {
                btnKCZYGH.Checked = false;
            }

            //if (m_ILayersJSYD == null) return;
            //m_ILayersJSYD.Visible = this.btnKCZYGH.Checked;

            //if (this.btnKCZYGH.Checked == true)
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("�����Դ"))
            //        {
            //            pFeaturelay.Visible = true;
            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}
            //else
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("�����Դ"))
            //        {
            //            pFeaturelay.Visible = false;
            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}

          
            axMapControl1.ActiveView.Refresh();
        }

        private void btnCKQFZ_Click(object sender, EventArgs e)
        {
            if (this.btnCKQFZ.Checked == false)
            {
                this.btnCKQFZ.Checked = true;
            }
            else
            {
                btnCKQFZ.Checked = false;
            }
            if (m_ILayerCKQ == null) return;
            m_ILayerCKQ.Visible = this.btnCKQFZ.Checked;

            //if (this.btnCKQFZ.Checked == true)
            //{
            //    //this.TLControl.SelectedTab = this.CKQti;
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name == "����˲�" || pFeaturelay.Name == "ȫʡ�ɿ�Ȩʡ��֤")
            //        {
            //            pFeaturelay.Visible = true;

            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}
            //else
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name == "����˲�" || pFeaturelay.Name == "ȫʡ�ɿ�Ȩʡ��֤")
            //        {
            //            pFeaturelay.Visible = false;

            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();

            //}
            axMapControl1.ActiveView.Refresh();
        }

        private void btnTKQFZ_Click(object sender, EventArgs e)
        {
            if (this.btnTKQFZ.Checked == false)
            {
                this.btnTKQFZ.Checked = true;
            }
            else
            {
                btnTKQFZ.Checked = false;
            }

            if (m_ILayerTKQ == null) return;
            m_ILayerTKQ.Visible = this.btnTKQFZ.Checked;
            //if (btnTKQFZ.Checked == true)
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name == "����˲�" || pFeaturelay.Name == "ȫʡ̽��Ȩ")
            //        {
            //            pFeaturelay.Visible = true;

            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}
            //else
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name == "����˲�" || pFeaturelay.Name == "ȫʡ̽��Ȩ")
            //        {
            //            pFeaturelay.Visible = false;

            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();

            //}
            axMapControl1.ActiveView.Refresh();
        }

        private void btiTDGY_Click(object sender, EventArgs e)
        {
            if (this.btiTDGY.Checked == false)
            {
                this.btiTDGY.Checked = true;
            }
            else
            {
                btiTDGY.Checked = false;
            }

            if (m_ILayerTDGY == null) return;
            m_ILayerTDGY.Visible = this.btiTDGY.Checked;

            //if (this.btiTDGY.Checked == true)
            //{
            //    //this.TLControl.SelectedTab = this.TDLYGHti;
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        //if (pFeaturelay.Name == "��״������Ŀ" || pFeaturelay.Name == "��״������Ŀ" || pFeaturelay.Name.Contains("�滮ͼ"))
            //        if (pFeaturelay.Name.Contains("���ع�Ӧ"))
            //        {
            //            pFeaturelay.Visible = true;
            //        }

            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();

            //}
            //else
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("���ع�Ӧ"))
            //        {
            //            pFeaturelay.Visible = false;
            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}
            axMapControl1.ActiveView.Refresh();
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ButtonItem m_ButtonItem=(DevComponents.DotNetBar.ButtonItem)sender;
            m_ButtonItem.Checked=!m_ButtonItem.Checked ;
            SetImageVisible(m_ButtonItem.Text, m_ButtonItem.Checked);

            SetButtonItemCheckStation(m_ButtonItem);
        }

        /// <summary>
        /// ���ò˵��Ƿ�Ӧ�ñ�ѡ��
        /// </summary>
        /// <param name="p_ButtonItem"></param>
        private void SetButtonItemCheckStation(DevComponents.DotNetBar.ButtonItem p_ButtonItem)
        {
            bool m_blTemp = false;
            DevComponents.DotNetBar.ButtonItem m_ButtonItemTemp, m_ButtonItemParent;
            m_ButtonItemParent = (DevComponents.DotNetBar.ButtonItem)p_ButtonItem.Parent;
            for (int i = 0; i < m_ButtonItemParent.SubItems.Count; i++)
            {
                //�ж��Ƿ������Ӳ˵����Ѿ�
                m_ButtonItemTemp = (DevComponents.DotNetBar.ButtonItem)m_ButtonItemParent.SubItems[i];
                if (m_ButtonItemTemp.Checked)
                {
                    m_blTemp = true;
                    break;
                }
            }
            m_ButtonItemParent.Checked = m_blTemp;
        }

        public void setImageBtni()
        {
            //try
            //{
            //    DevComponents.DotNetBar.ButtonItem m_ImageShow = new DevComponents.DotNetBar.ButtonItem();
            //    DevComponents.DotNetBar.ButtonItem[] m_ImageShows;

            //    bar2.Items.Insert(bar2.Items.Count, m_ImageShow);

            //    m_ImageShow.Text = "ң��Ӱ��";

            //    ArrayList m_ArrayList = new ArrayList();
            //    IEnumLayer m_grouplayer;
            //    m_grouplayer = Functions.MapFunction.GetGroupLayers(this.axMapControl1.Map);

            //    ILayer pglayer = m_grouplayer.Next();
            //    while (pglayer != null)
            //    {
            //        IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

            //        if (pGroupLayer.Name == "ң��Ӱ��")
            //        {
            //            m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
            //            break;
            //        }
            //        pglayer = m_grouplayer.Next();
            //    }
            //    m_grouplayer.Reset();

            //    ILayer m_layer;
            //    DevComponents.DotNetBar.ButtonItem m_ButtonItem;

            //    m_ImageShows = new DevComponents.DotNetBar.ButtonItem[m_ArrayList.Count];
            //    for (int i = 0; i < m_ArrayList.Count; i++)
            //    {
            //        m_ImageShows[i] = new DevComponents.DotNetBar.ButtonItem();
            //        m_layer = (ILayer)m_ArrayList[i];
            //        m_ImageShows[i].Text = m_layer.Name;
            //        m_ImageShow.SubItems.Add(m_ImageShows[i] as DevComponents.DotNetBar.BaseItem);
            //        m_ButtonItem = (DevComponents.DotNetBar.ButtonItem)m_ImageShow.SubItems[i];
            //        m_ButtonItem.Checked = m_layer.Visible;
            //        if (m_ButtonItem.Checked)
            //        {
            //            //����������״ͼ��ť���ɫ
            //            m_ImageShow.Checked = true;
            //        }

            //        m_ImageShows[i].Click += new System.EventHandler(ImageButton_Click);
            //    }
            //    bar2.Refresh();
            //}
            //catch (SystemException errs)
            //{
            //    MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}

            try
            {
                ILayer m_ILayer, m_ILayerImageGroup;
                ArrayList m_ArrayList;

DevComponents.DotNetBar.ButtonItem[] m_ImageShows;

if (m_ImageShow == null)
{
    m_ImageShow = new DevComponents.DotNetBar.ButtonItem();



    bar2.Items.Insert(bar2.Items.Count, m_ImageShow);

    m_ImageShow.Text = "ң��Ӱ��";
}
else
{
    m_ImageShow.SubItems.Clear();
}

                //ArrayList m_ArrayList = new ArrayList();
                //IEnumLayer m_grouplayer;
                //m_grouplayer = Functions.MapFunction.GetGroupLayers(this.m_AxMapControl_Selected.Map);

                //ILayer pglayer = m_grouplayer.Next();
                //while (pglayer != null)
                //{
                //    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                //    if (pGroupLayer.Name == "ң��Ӱ��")
                //    {
                //        m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
                //    }
                //    pglayer = m_grouplayer.Next();
                //}
                //m_grouplayer.Reset();

                m_ILayerImageGroup = (ILayer)m_ILayerImage;
                if (m_ILayerImageGroup == null) return;


                m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerImageGroup);

                m_ImageShows = new DevComponents.DotNetBar.ButtonItem[m_ArrayList.Count];
                //int m_intTempItemsChecked = 0;

                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    m_ImageShows[i] = new DevComponents.DotNetBar.ButtonItem();
                    m_ILayer = (ILayer)m_ArrayList[i];
                    m_ImageShows[i].Text = m_ILayer.Name;
                    m_ImageShow.SubItems.Add(m_ImageShows[i] as DevComponents.DotNetBar.BaseItem);
                    m_ImageShows[i].Click += new System.EventHandler(ImageButton_Click);

                    //m_ImageShows[i].Checked = m_ILayer.Visible;
                    //if (m_ImageShows[i].Checked = true)
                    //{
                    //    m_intTempItemsChecked++;
                    //}         
                }
                //if (m_ILayerImageGroup.Visible = true && m_intTempItemsChecked == 0)
                //{
                //    m_ImageShow.Checked = false;
                //}
                //else if (m_ILayerImageGroup.Visible = true && m_intTempItemsChecked > 0)
                //{
                //    m_ImageShow.Checked = true;
                //}
                //else if (m_ILayerImageGroup.Visible = false)
                //{
                //    m_ImageShow.Checked = false;
                //}
                bar2.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void SetImageVisible(string p_strRasterCatalogName, bool m_bool)
        {
            try
            {
                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(this.axMapControl1.ActiveView.FocusMap);

                ILayer pglayer = m_grouplayer.Next();
                while (pglayer != null)
                {
                    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                    if (pGroupLayer.Name == "ң��Ӱ��")
                    {
                        pGroupLayer.Visible = true;

                        ArrayList m_ArrayList;
                        ILayer m_Layer;
                        m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_Layer = (ILayer)m_ArrayList[i];
                            if (m_Layer.Name == p_strRasterCatalogName)
                            {
                                m_Layer.Visible = m_bool;
                            }
                        }

                    }
                    pglayer = m_grouplayer.Next();
                }
                m_grouplayer.Reset();

                this.axMapControl1.ActiveView.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void setTDLYXZBtni()
        {
            //try
            //{
            //    DevComponents.DotNetBar.ButtonItem m_TDLYXZShow=null ;
            //    DevComponents.DotNetBar.ButtonItem[] m_JBNTShows=null;

               
            //    for (int i = 0; i < bar2.Items.Count; i++)
            //    {
            //        if (bar2.Items[i].Text.Trim() == "����������״ͼ")
            //        {
            //            m_TDLYXZShow = bar2.Items[i] as DevComponents.DotNetBar.ButtonItem;
            //            break;
            //        }
            //    }

            //    if (m_TDLYXZShow == null) return;

            //    ArrayList m_ArrayList = new ArrayList();
            //    IEnumLayer m_grouplayer;
            //    m_grouplayer = Functions.MapFunction.GetGroupLayers(this.axMapControl1.Map);

            //    ILayer pglayer = m_grouplayer.Next();
            //    while (pglayer != null)
            //    {
            //        IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

            //        if (pGroupLayer.Name == "����������״")
            //        {
            //            m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
            //            break;
            //        }
            //        pglayer = m_grouplayer.Next();
            //    }
            //    m_grouplayer.Reset();

            //    m_JBNTShows = new DevComponents.DotNetBar.ButtonItem[m_ArrayList.Count];
            //    ILayer m_layer;
            //    DevComponents.DotNetBar.ButtonItem m_ButtonItem;
            //    for (int i = 0; i < m_ArrayList.Count; i++)
            //    {
            //        m_JBNTShows[i] = new DevComponents.DotNetBar.ButtonItem();
            //        m_layer=(ILayer)m_ArrayList[i];
            //        m_JBNTShows[i].Text = m_layer.Name;
            //        m_TDLYXZShow.SubItems.Add(m_JBNTShows[i] as DevComponents.DotNetBar.BaseItem);
            //        m_ButtonItem = (DevComponents.DotNetBar.ButtonItem)m_TDLYXZShow.SubItems[i];
            //        m_ButtonItem.Checked = m_layer.Visible;
            //        if (m_ButtonItem.Checked)
            //        {
            //            //����������״ͼ��ť���ɫ
            //            btnTDLYXZ.Checked = true;
            //        }
                    
            //        m_JBNTShows[i].Click += new System.EventHandler(TDLYXZButton_Click);
            //    }
            //    bar2.Refresh();
            //}
            //catch (SystemException errs)
            //{
            //    MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}

            try
            {
                DevComponents.DotNetBar.ButtonItem m_TDLYXZShow = null;
                DevComponents.DotNetBar.ButtonItem[] m_TDLYXZShows = null;

                IGroupLayer m_GroupLayerTDLY = null;
                ArrayList m_ArrayList = new ArrayList();
                ILayer m_ILayer;
                //IEnumLayer m_grouplayer;
                //m_grouplayer = Functions.MapFunction.GetGroupLayers(this.m_AxMapControl_Selected.Map);

                //ILayer pglayer = m_grouplayer.Next();
                //int m_intMapIndex = System.Convert.ToInt32(this.m_AxMapControl_Selected.Name.Substring(this.m_AxMapControl_Selected.Name.Length - 1, 1)) - 1;

               
                if (m_ILayerTDLY == null) return;

                //while (pglayer != null)
                //{
                m_GroupLayerTDLY = (IGroupLayer)m_ILayerTDLY;

                m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_GroupLayerTDLY);
                //}
                //m_grouplayer.Reset();

                m_TDLYXZShows = new DevComponents.DotNetBar.ButtonItem[m_ArrayList.Count];
                ILayer m_layer;
                int m_intTempItemsChecked = 0;
                DevComponents.DotNetBar.ButtonItem m_ButtonItem;
                btnTDLYXZ.SubItems.Clear();
                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    m_TDLYXZShows[i] = new DevComponents.DotNetBar.ButtonItem();
                    m_layer = (ILayer)m_ArrayList[i];
                    m_TDLYXZShows[i].Text = m_layer.Name;
                    //m_JBNTShows[i].Tag = 
                    btnTDLYXZ.SubItems.Add(m_TDLYXZShows[i] as DevComponents.DotNetBar.BaseItem);
                    m_ButtonItem = (DevComponents.DotNetBar.ButtonItem)btnTDLYXZ.SubItems[i];
                    //m_ButtonItem.Checked = m_layer.Visible;
                    //if (m_ButtonItem.Checked=true  )
                    //{
                    //    m_intTempItemsChecked++;                                        
                    //}                    

                    m_TDLYXZShows[i].Click += new System.EventHandler(TDLYXZButton_Click);
                }
                //if (m_GroupLayerJBNT.Visible= true && m_intTempItemsChecked == 0)
                //{
                //    btnTDLYXZ.Checked = false;
                //}
                //else if (m_GroupLayerJBNT.Visible = true && m_intTempItemsChecked > 0)
                //{
                //    btnTDLYXZ.Checked = true;
                //}
                //else if (m_GroupLayerJBNT.Visible = false )
                //{
                //    btnTDLYXZ.Checked = false ;
                //}
                bar2.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        

        private void TDLYXZButton_Click(object sender, EventArgs e)
        {
            //DevComponents.DotNetBar.ButtonItem m_ButtonItem = (DevComponents.DotNetBar.ButtonItem)sender;
            //m_ButtonItem.Checked = !m_ButtonItem.Checked;
            ////SetImageVisible(m_ButtonItem.Text, m_ButtonItem.Checked);

            //try
            //{
            //    IEnumLayer m_grouplayer;
            //    m_grouplayer = Functions.MapFunction.GetGroupLayers(this.axMapControl1.ActiveView.FocusMap);

            //    ILayer pglayer = m_grouplayer.Next();
            //    while (pglayer != null)
            //    {
            //        IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

            //        if (pGroupLayer.Name == "����������״")
            //        {
            //            pGroupLayer.Visible = true;

            //            ArrayList m_ArrayList;
            //            ILayer m_Layer;
            //            m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
            //            for (int i = 0; i < m_ArrayList.Count; i++)
            //            {
            //                m_Layer = (ILayer)m_ArrayList[i];
            //                if (m_Layer.Name == m_ButtonItem.Text )
            //                {
            //                    m_Layer.Visible = m_ButtonItem.Checked;
            //                }
            //            }
            //            break;
            //        }
            //        pglayer = m_grouplayer.Next();
            //    }
            //    m_grouplayer.Reset();
            //    SetButtonItemCheckStation(m_ButtonItem);
            //    this.axMapControl1.ActiveView.Refresh();
            //}
            //catch (SystemException errs)
            //{
            //    MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}

            DevComponents.DotNetBar.ButtonItem m_ButtonItem = (DevComponents.DotNetBar.ButtonItem)sender;
            m_ButtonItem.Checked = !m_ButtonItem.Checked;
            //SetImageVisible(m_ButtonItem.Text, m_ButtonItem.Checked);

            try
            {
                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(this.axMapControl1.ActiveView.FocusMap);

                ILayer pglayer = m_grouplayer.Next();
                while (pglayer != null)
                {
                    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                    if (pGroupLayer.Name == "����������״")
                    {
                        pGroupLayer.Visible = true;

                        ArrayList m_ArrayList;
                        ILayer m_Layer;
                        m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_Layer = (ILayer)m_ArrayList[i];
                            if (m_Layer.Name == m_ButtonItem.Text)
                            {
                                m_Layer.Visible = m_ButtonItem.Checked;
                            }
                        }
                        break;
                    }
                    pglayer = m_grouplayer.Next();
                }
                m_grouplayer.Reset();
                SetButtonItemCheckStation(m_ButtonItem);
                this.axMapControl1.ActiveView.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        //��ʼ��������ũ����Ӱ�ť
        public void setJBNTBtni()
        {
            try
            {
                //DevComponents.DotNetBar.ButtonItem m_TDLYXZShow = null;
                DevComponents.DotNetBar.ButtonItem[] m_JBNTShows = null;

                IGroupLayer m_GroupLayerJBNT = null;
                ArrayList m_ArrayList = new ArrayList();
                ILayer m_ILayer;

                if (m_ILayerTDLY == null) return;

                m_GroupLayerJBNT = (IGroupLayer)m_ILayerJBNT;

                m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_GroupLayerJBNT);

                m_JBNTShows = new DevComponents.DotNetBar.ButtonItem[m_ArrayList.Count];
                ILayer m_layer;
                int m_intTempItemsChecked = 0;
                DevComponents.DotNetBar.ButtonItem m_ButtonItem;
                btnJBNTBH.SubItems.Clear();
                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    m_JBNTShows[i] = new DevComponents.DotNetBar.ButtonItem();
                    m_layer = (ILayer)m_ArrayList[i];
                    m_JBNTShows[i].Text = m_layer.Name;
                    //m_JBNTShows[i].Tag = 
                    btnJBNTBH.SubItems.Add(m_JBNTShows[i] as DevComponents.DotNetBar.BaseItem);
                    m_ButtonItem = (DevComponents.DotNetBar.ButtonItem)btnJBNTBH.SubItems[i];
                    //m_ButtonItem.Checked = m_layer.Visible;
                    //if (m_ButtonItem.Checked=true  )
                    //{
                    //    m_intTempItemsChecked++;                                        
                    //}                    

                    m_JBNTShows[i].Click += new System.EventHandler(JBNTButton_Click);
                }
                //if (m_GroupLayerJBNT.Visible= true && m_intTempItemsChecked == 0)
                //{
                //    btnTDLYXZ.Checked = false;
                //}
                //else if (m_GroupLayerJBNT.Visible = true && m_intTempItemsChecked > 0)
                //{
                //    btnTDLYXZ.Checked = true;
                //}
                //else if (m_GroupLayerJBNT.Visible = false )
                //{
                //    btnTDLYXZ.Checked = false ;
                //}
                bar2.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void JBNTButton_Click(object sender, EventArgs e)
        {            
            DevComponents.DotNetBar.ButtonItem m_ButtonItem = (DevComponents.DotNetBar.ButtonItem)sender;
            m_ButtonItem.Checked = !m_ButtonItem.Checked;
            //SetImageVisible(m_ButtonItem.Text, m_ButtonItem.Checked);

            try
            {
                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(this.axMapControl1.ActiveView.FocusMap);

                ILayer pglayer = m_grouplayer.Next();
                while (pglayer != null)
                {
                    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                    if (pGroupLayer.Name == "����ũ��")
                    {
                        pGroupLayer.Visible = true;

                        ArrayList m_ArrayList;
                        ILayer m_Layer;
                        m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_Layer = (ILayer)m_ArrayList[i];
                            if (m_Layer.Name == m_ButtonItem.Text)
                            {
                                m_Layer.Visible = m_ButtonItem.Checked;
                            }
                        }
                        break;
                    }
                    pglayer = m_grouplayer.Next();
                }
                m_grouplayer.Reset();
                SetButtonItemCheckStation(m_ButtonItem);
                this.axMapControl1.ActiveView.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        #endregion

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            //m_frmShowPic = new frmShowPic();
            //m_frmShowPic.m_Oid = this.m_OID;


            ////����dkid�ж� ����
            //m_frmShowPic.strdkid = this.dkid;




            //m_frmShowPic.m_DataAccess_SYS = this.m_DataAccess_SYS;

            //ArrayList theList = new ArrayList();
            //string m_strID = GetStrId(m_OID.ToString(), "p");


            //GetFile(m_strID, ref theList);
            //m_frmShowPic.m_theFileList.Clear();
            //m_frmShowPic.m_theFileList.AddRange(theList); // ���
            //m_frmShowPic.DoInitial();
            //m_frmShowPic.ShowIconImage();


            //ArrayList theList1 = new ArrayList();

            //string m_strID1 = GetStrId(this.m_OID.ToString(), "v");

            //GetFile(m_strID1, ref theList1);

            //m_frmShowPic.m_theFileList1.Clear();
            //m_frmShowPic.m_theFileList1.AddRange(theList1); // ���
            //m_frmShowPic.DoInitialVideos(); // ��ʼ����������յȣ�

            //m_frmShowPic.Show();

        }

        private string GetStrId(string id, string style)
        {
            string m_strID = "";
            //yuejianwei,��Ҫ�޸�20110412,ȷ��m_FeatureLayerName�Ǵ�������
            if (this.m_FeatureLayerName == "���غ˲�" && style == "p")
                m_strID = "t" + id + "p";
            else
                if (this.m_FeatureLayerName == "���غ˲�" && style == "v")
                    m_strID = "t" + id + "v";

            if (this.m_FeatureLayerName == "����˲�" && style == "p")
                m_strID = "k" + id + "p";
            else
                if (this.m_FeatureLayerName == "����˲�" && style == "v")
                    m_strID = "k" + id + "v";

            return m_strID;

        }


        private void GetFile(string m_strID, ref ArrayList theStreamList)
        {
            theStreamList.Clear();

            string m_strSQL = "select MC from DOC where ID LIKE '" + m_strID + "'";

            DataRowCollection m_DataRowCollection = this.m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);
            if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
            {
                for (int i = 0; i < m_DataRowCollection.Count; i++)
                {
                    theStreamList.Add(m_DataRowCollection[i][0].ToString());

                    if (IsExist(m_DataRowCollection[i][0].ToString()) == false)
                        CreateFolder(m_strID);
                }
            }
        }

        private bool IsExist(string mc)
        {
            string path =Directory.GetCurrentDirectory() + "\\" + "HCPV" + "\\" + mc;

            if (System.IO.File.Exists(path))
                return true;
            else
                return false;

        }

        private void CreateFolder(string m_strID)
        {
            byte[] file1 = null;
            string strMC = "";

            string m_strSQL = "SELECT * FROM DOC WHERE ID LIKE '" + m_strID + "'";

            DataRowCollection m_DataRowCollection = this.m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);

            try
            {
                if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
                {
                    for (int i = 0; i < m_DataRowCollection.Count; i++)
                    {
                        DataRow m_DataRow = m_DataRowCollection[i];
                        strMC = (string)m_DataRow[1]; //m_AssistantFunction.EraseSpace()
                        file1 = (byte[])m_DataRow[2];

                        //��������ļ���·��
                        string str_tempPath = Directory.GetCurrentDirectory() + "\\" + "HCPV";
                        DirectoryInfo m_DirectoryInfo = new DirectoryInfo(str_tempPath);
                        if (m_DirectoryInfo.Exists == false)
                            Directory.CreateDirectory(str_tempPath);

                        string Save_path = str_tempPath + "\\" + strMC;

                        if (System.IO.File.Exists(Save_path))
                        {
                            System.IO.File.Delete(Save_path);
                        }

                        System.IO.FileStream fs = new System.IO.FileStream(Save_path, FileMode.CreateNew);
                        BinaryWriter bw = new BinaryWriter(fs);
                        //bw.Write(file1, 0, file1.Length);
                        bw.Write(file1);
                        bw.Close();
                        fs.Close();

                    }

                }
            }
            catch (System.Exception errs)
            {
                System.Windows.Forms.MessageBox.Show(this, errs.Message, "������ʾ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            Fill_listViewEx_ZFRYInfo(m_DataTableBJZFRY);
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            Fill_listViewEx_ZFRYInfo(m_DataTableSJZFRY);           
        }

        private void Fill_listViewEx_ZFRYInfo(DataTable p_DataTable)
        {
            listViewEx_ZFRYInfo.Items.Clear();
            if (p_DataTable != null && p_DataTable.Rows.Count > 0)
            {
                panelEx_ZFRY_Info.Top=panelEx1.Bottom;
                panelEx_ZFRY_Info.Left = panelEx1.Left;

                panelEx_ZFRY_Info.Visible = true;
                ListViewItem m_ListViewItem;
                for (int i=0;i<p_DataTable.Rows.Count;i++)
                {                    
                    m_ListViewItem = new ListViewItem(p_DataTable.Rows[i][0].ToString());
                    if (p_DataTable.Rows[i][1] != null)
                    {
                        m_ListViewItem.SubItems.Add(p_DataTable.Rows[i][1].ToString());
                    }
                    else
                    {
                        m_ListViewItem.SubItems.Add("");
                    }
                    if (p_DataTable.Rows[i][2] != null)
                    {
                        m_ListViewItem.SubItems.Add(p_DataTable.Rows[i][2].ToString());
                    }
                    else
                    {
                        m_ListViewItem.SubItems.Add("");
                    }
                    listViewEx_ZFRYInfo.Items.Add(m_ListViewItem);
                }
            }
        }

        

        private void labelX6_MouseHover(object sender, EventArgs e)
        {
            Fill_listViewEx_ZFRYInfo(m_DataTableBJZFRY);
        }

        private void labelX5_MouseHover(object sender, EventArgs e)
        {
            Fill_listViewEx_ZFRYInfo(m_DataTableSJZFRY);   
        }




        IGraphicsContainer g = null;
        private void showZFRY(int level, string sjqhdm)
        {
            IGroupElement m_Elements = new GroupElement() as IGroupElement ;
            IGroupElement m_SJElement = new GroupElement() as IGroupElement ;
            IGroupElement m_QXElement = new GroupElement() as IGroupElement ;
            IGroupElement m_XZElement = new GroupElement() as IGroupElement ;
            IGroupElement m_CUNElement = new GroupElement() as IGroupElement ;

            g = this.axMapControl1.ActiveView.GraphicsContainer;

            //g.AddElement(m_Elements as IElement, 0);
            //g.AddElement(m_SJElement as IElement, 0);
            //g.AddElement(m_QXElement as IElement, 0);
            //g.AddElement(m_XZElement as IElement, 0);
            //g.AddElement(m_CUNElement as IElement, 0);

            ////��ӵ�m_Elements��
            //g.MoveElementToGroup(m_SJElement as IElement, m_Elements);
            //g.MoveElementToGroup(m_QXElement as IElement, m_Elements);
            //g.MoveElementToGroup(m_XZElement as IElement, m_Elements);
            //g.MoveElementToGroup(m_CUNElement as IElement, m_Elements);

            if (g != null)
            { g.DeleteAllElements(); }

            //�м�
            IArea area;
            IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;

            //if (level == 0)
            {
                //if (m_SJElement.ElementCount > 0)
                //{
                //    m_SJElement.ClearElements();
                //    MessageBox.Show("���0");
                //}
                //if (m_QXElement.ElementCount >0)
                //{
                //    m_QXElement.ClearElements();
                //}
                //if (m_XZElement.ElementCount > 0)
                //{
                //    m_XZElement.ClearElements();
                //}
                //if (m_CUNElement.ElementCount >0)
                //{
                //    m_CUNElement.ClearElements();
                //}



                IFeatureLayer m_IFeatureLayer = MapFunction.getFeatureLayerByName("��", axMapControl1);


                LabelUserDef m_LabelUserDef = new LabelUserDef();

                IFeatureCursor m_FeatureCursor = m_IFeatureLayer.FeatureClass.Search(null, false);

                IFeature m_Feature = m_FeatureCursor.NextFeature();

                while (m_Feature != null)
                {
                    //λ�ã�ȡ���ĵ�
                    area = m_Feature.Shape as IArea;
                    pt = area.Centroid;

                    //��ȡ��ע�ı�
                    string qhdm = m_Feature.get_Value(m_Feature.Fields.FindField("qhdm")).ToString();

                    DataTable m_DataTableBJZFRY1 = getBJRYfromQHDM(qhdm);
                    string BJRY = "";

                    if (m_DataTableBJZFRY1 != null)
                    {
                        for (int i = 0; i < m_DataTableBJZFRY1.Rows.Count; i++)
                        {
                            if (m_DataTableBJZFRY1.Rows[i][0] != null)
                            {
                                BJRY = BJRY + m_DataTableBJZFRY1.Rows[i][0].ToString();
                            }
                        }
                    }
                    if (BJRY != "")
                    {

                        //��ӱ�ע
                        ITextElement te = m_LabelUserDef.createTextElement(pt, BJRY);

                        m_SJElement.AddElement(te as IElement);
                        //this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te as IElement, 1);
                    }
                    m_Feature = m_FeatureCursor.NextFeature();

                }
                g.AddElement(m_SJElement as IElement, 0);

                this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            // ����
            if (level == 1)
            {

                //if (m_QXElement.ElementCount > 0)
                //{
                //    g.DeleteElement(m_QXElement as IElement);
                //    m_QXElement=null;
                //    MessageBox.Show("���1");
                //}


                IFeatureLayer m_IFeatureLayer = MapFunction.getFeatureLayerByName("��", axMapControl1);

                LabelUserDef m_LabelUserDef = new LabelUserDef();

                ISpatialFilter spatialFilter = new SpatialFilter() as ISpatialFilter;
                //spatialFilter.WhereClause = "left(qhdm,4)='" + sjqhdm+"'";

                spatialFilter.WhereClause ="qhdm LIKE '"+sjqhdm+"__'";  // "left(qhdm,4)='" + sjqhdm + "'";_
                IFeatureCursor m_FeatureCursor = m_IFeatureLayer.FeatureClass.Search(spatialFilter, false);

                IFeature m_Feature = m_FeatureCursor.NextFeature();

                while (m_Feature != null)
                {
                    //λ�ã�ȡ���ĵ�
                    area = m_Feature.Shape as IArea;
                    pt = area.Centroid;

                    //��ȡ��ע�ı�
                    string qhdm = m_Feature.get_Value(m_Feature.Fields.FindField("qhdm")).ToString();

                    DataTable m_DataTableBJZFRY1 = getBJRYfromQHDM(qhdm);
                    string BJRY = "";

                    if (m_DataTableBJZFRY1 != null)
                    {
                        for (int i = 0; i < m_DataTableBJZFRY1.Rows.Count; i++)
                        {
                            if (m_DataTableBJZFRY1.Rows[i][0] != null)
                            {
                                BJRY = BJRY + m_DataTableBJZFRY1.Rows[i][0].ToString();
                            }
                        }
                    }
                    if (BJRY != "")
                    {

                        //��ӱ�ע
                        ITextElement te = m_LabelUserDef.createTextElement(pt, BJRY);

                        m_QXElement.AddElement(te as IElement);
                        //this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te as IElement, 1);
                    }
                    m_Feature = m_FeatureCursor.NextFeature();

                }
                g.AddElement(m_QXElement as IElement, 0);
                this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            }
            //����
            if (level == 2)
            {
                ////if (m_XZElement.ElementCount > 0)
                //{
                //    m_XZElement.ClearElements();
                //    MessageBox.Show("���2");
                //}                

                IFeatureLayer m_IFeatureLayer = MapFunction.getFeatureLayerByName("����", axMapControl1);

                LabelUserDef m_LabelUserDef = new LabelUserDef();
                ISpatialFilter spatialFilter = new SpatialFilter() as ISpatialFilter;
                spatialFilter.WhereClause = "left(DWDM,6)='" + sjqhdm+"'";
                IFeatureCursor m_FeatureCursor = m_IFeatureLayer.FeatureClass.Search(spatialFilter, false);

                IFeature m_Feature = m_FeatureCursor.NextFeature();

                while (m_Feature != null)
                {
                    //λ�ã�ȡ���ĵ�
                    area = m_Feature.Shape as IArea;
                    pt = area.Centroid;

                    //��ȡ��ע�ı�
                    string qhdm = m_Feature.get_Value(m_Feature.Fields.FindField("dwdm")).ToString();

                    DataTable m_DataTableBJZFRY1 = getBJRYfromQHDM(qhdm);
                    string BJRY = "";

                    if (m_DataTableBJZFRY1 != null)
                    {
                        for (int i = 0; i < m_DataTableBJZFRY1.Rows.Count; i++)
                        {
                            if (m_DataTableBJZFRY1.Rows[i][0] != null)
                            {
                                BJRY = BJRY + m_DataTableBJZFRY1.Rows[i][0].ToString();
                            }
                        }
                    }
                    if (BJRY != "")
                    {
                        //��ӱ�ע
                        ITextElement te = m_LabelUserDef.createTextElement(pt, BJRY);

                        m_XZElement.AddElement(te as IElement);
                        //this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te as IElement, 1);
                    }
                    m_Feature = m_FeatureCursor.NextFeature();

                }
                g.AddElement(m_XZElement as IElement, 0);
                this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            //��
            if (level == 3)
            {
                //if (m_CUNElement.ElementCount > 0)
                //{
                //    m_CUNElement.ClearElements();
                //    MessageBox.Show("���3");
                //}

                IFeatureLayer m_IFeatureLayer = MapFunction.getFeatureLayerByName("��", axMapControl1);

                LabelUserDef m_LabelUserDef = new LabelUserDef();
                ISpatialFilter spatialFilter = new SpatialFilter() as ISpatialFilter;
                spatialFilter.WhereClause = "left(DWDM,9)='" + sjqhdm+"'";
                IFeatureCursor m_FeatureCursor = m_IFeatureLayer.FeatureClass.Search(spatialFilter, false);


                IFeature m_Feature = m_FeatureCursor.NextFeature();

                while (m_Feature != null)
                {
                    //λ�ã�ȡ���ĵ�
                    area = m_Feature.Shape as IArea;
                    pt = area.Centroid;

                    //��ȡ��ע�ı�
                    string qhdm = m_Feature.get_Value(m_Feature.Fields.FindField("dwdm")).ToString();

                    DataTable m_DataTableBJZFRY1 = getBJRYfromQHDM(qhdm);
                    string BJRY = "";

                    if (m_DataTableBJZFRY1 != null)
                    {
                        for (int i = 0; i < m_DataTableBJZFRY1.Rows.Count; i++)
                        {
                            if (m_DataTableBJZFRY1.Rows[i][0] != null)
                            {
                                BJRY = BJRY + m_DataTableBJZFRY1.Rows[i][0].ToString();
                            }
                        }
                    }

                    //��ӱ�ע

                    if (BJRY != "")
                    {
                        ITextElement te = m_LabelUserDef.createTextElement(pt, BJRY);

                        m_CUNElement.AddElement(te as IElement);
                    }
                    //this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te as IElement, 1);

                    m_Feature = m_FeatureCursor.NextFeature();

                }
                g.AddElement(m_CUNElement as IElement, 0);
                this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
        }

        //private void showZFRY(string sjqhdm)
        //{
        //    IGroupElement m_Elements = new GroupElement() as IGroupElement ;
        //    IGroupElement m_SJElement = new GroupElement() as IGroupElement ;
        //    IGroupElement m_QXElement = new GroupElement() as IGroupElement ;
        //    IGroupElement m_XZElement = new GroupElement() as IGroupElement ;
        //    IGroupElement m_CUNElement = new GroupElement() as IGroupElement ;

        //    IGraphicsContainer g = this.axMapControl1.ActiveView.GraphicsContainer;

        //    //g.AddElement(m_Elements as IElement, 0);
        //    g.AddElement(m_SJElement as IElement, 0);
        //    g.AddElement(m_QXElement as IElement, 0);
        //    g.AddElement(m_XZElement as IElement, 0);
        //    g.AddElement(m_CUNElement as IElement, 0);

        //    ////��ӵ�m_Elements��
        //    //g.MoveElementToGroup(m_SJElement as IElement, m_Elements);
        //    //g.MoveElementToGroup(m_QXElement as IElement, m_Elements);
        //    //g.MoveElementToGroup(m_XZElement as IElement, m_Elements);
        //    //g.MoveElementToGroup(m_CUNElement as IElement, m_Elements);



        //    //�м�
        //    IArea area;
        //    IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;

        //    IFeatureLayer m_DSFeatureLayer = MapFunction.getFeatureLayerByName("��", axMapControl1);
        //    IFeatureLayer m_QXFeatureLayer = MapFunction.getFeatureLayerByName("��", axMapControl1);
        //    IFeatureLayer m_XZFeatureLayer = MapFunction.getFeatureLayerByName("��", axMapControl1);
        //    IFeatureLayer m_CUNFeatureLayer = MapFunction.getFeatureLayerByName("����", axMapControl1);

        //    if (m_DSFeatureLayer.Visible == true && m_QXFeatureLayer.Visible == false && m_XZFeatureLayer.Visible == false && m_CUNFeatureLayer.Visible == false)
        //    {
        //        LabelUserDef m_LabelUserDef = new LabelUserDef();
        //        IFeatureCursor m_FeatureCursor = m_DSFeatureLayer.FeatureClass.Search(null, false);

        //        IFeature m_Feature = m_FeatureCursor.NextFeature();

        //        while (m_Feature != null)
        //        {
        //            //λ�ã�ȡ���ĵ�
        //            area = m_Feature.Shape as IArea;
        //            pt = area.Centroid;

        //            //��ȡ��ע�ı�
        //            string qhdm = m_Feature.get_Value(m_Feature.Fields.FindField("qhdm")).ToString();

        //            DataTable m_DataTableBJZFRY1 = getBJRYfromQHDM(qhdm);
        //            string BJRY = "";

        //            if (m_DataTableBJZFRY1 != null)
        //            {
        //                for (int i = 0; i < m_DataTableBJZFRY1.Rows.Count; i++)
        //                {
        //                    if (m_DataTableBJZFRY1.Rows[i][0] != null)
        //                    {
        //                        BJRY = BJRY + m_DataTableBJZFRY1.Rows[i][0].ToString();
        //                    }
        //                }
        //            }
        //            if (BJRY != "")
        //            {

        //                //��ӱ�ע
        //                ITextElement te = m_LabelUserDef.createTextElement(pt, BJRY);

        //                m_SJElement.AddElement(te as IElement);
        //                //this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te as IElement, 1);
        //            }
        //            m_Feature = m_FeatureCursor.NextFeature();

        //        }

        //        this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        //    }


        //    // ����
        //    if (m_DSFeatureLayer.Visible == false && m_QXFeatureLayer.Visible == true && m_XZFeatureLayer.Visible == false && m_CUNFeatureLayer.Visible == false)
        //    {
        //        LabelUserDef m_LabelUserDef = new LabelUserDef();

        //        ISpatialFilter spatialFilter = new SpatialFilter() as ISpatialFilter;
        //        spatialFilter.WhereClause = "left(qhdm,4)=" + sjqhdm;
        //        IFeatureCursor m_FeatureCursor = m_QXFeatureLayer.FeatureClass.Search(spatialFilter, false);

        //        IFeature m_Feature = m_FeatureCursor.NextFeature();

        //        while (m_Feature != null)
        //        {
        //            //λ�ã�ȡ���ĵ�
        //            area = m_Feature.Shape as IArea;
        //            pt = area.Centroid;

        //            //��ȡ��ע�ı�
        //            string qhdm = m_Feature.get_Value(m_Feature.Fields.FindField("qhdm")).ToString();

        //            DataTable m_DataTableBJZFRY1 = getBJRYfromQHDM(qhdm);
        //            string BJRY = "";

        //            if (m_DataTableBJZFRY1 != null)
        //            {
        //                for (int i = 0; i < m_DataTableBJZFRY1.Rows.Count; i++)
        //                {
        //                    if (m_DataTableBJZFRY1.Rows[i][0] != null)
        //                    {
        //                        BJRY = BJRY + m_DataTableBJZFRY1.Rows[i][0].ToString();
        //                    }
        //                }
        //            }
        //            if (BJRY != "")
        //            {

        //                //��ӱ�ע
        //                ITextElement te = m_LabelUserDef.createTextElement(pt, BJRY);

        //                m_QXElement.AddElement(te as IElement);
        //                //this.axMapControl1.ActiveView.GraphicsContainer.AddElement(te as IElement, 1);
        //            }
        //            m_Feature = m_FeatureCursor.NextFeature();

        //        }

        //        this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        //    }
        //    //����

        //    if (m_DSFeatureLayer.Visible == false && m_QXFeatureLayer.Visible == false && m_XZFeatureLayer.Visible == true && m_CUNFeatureLayer.Visible == false)
        //    {
        //        LabelUserDef m_LabelUserDef = new LabelUserDef();
        //        ISpatialFilter spatialFilter = new SpatialFilter() as ISpatialFilter;
        //        spatialFilter.WhereClause = "left(DWDM,6)=" + sjqhdm;
        //        IFeatureCursor m_FeatureCursor = m_XZFeatureLayer.FeatureClass.Search(spatialFilter, false);

        //        IFeature m_Feature = m_FeatureCursor.NextFeature();

        //        while (m_Feature != null)
        //        {
        //            //λ�ã�ȡ���ĵ�
        //            area = m_Feature.Shape as IArea;
        //            pt = area.Centroid;

        //            //��ȡ��ע�ı�
        //            string qhdm = m_Feature.get_Value(m_Feature.Fields.FindField("dwdm")).ToString();

        //            DataTable m_DataTableBJZFRY1 = getBJRYfromQHDM(qhdm);
        //            string BJRY = "";

        //            if (m_DataTableBJZFRY1 != null)
        //            {
        //                for (int i = 0; i < m_DataTableBJZFRY1.Rows.Count; i++)
        //                {
        //                    if (m_DataTableBJZFRY1.Rows[i][0] != null)
        //                    {
        //                        BJRY = BJRY + m_DataTableBJZFRY1.Rows[i][0].ToString();
        //                    }
        //                }
        //            }
        //            if (BJRY != "")
        //            {
        //                //��ӱ�ע
        //                ITextElement te = m_LabelUserDef.createTextElement(pt, BJRY);
        //                m_XZElement.AddElement(te as IElement);
        //            }
        //            m_Feature = m_FeatureCursor.NextFeature();

        //        }

        //        this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        //    }
        //    //��

        //    if (m_DSFeatureLayer.Visible == false && m_QXFeatureLayer.Visible == false && m_XZFeatureLayer.Visible == false && m_CUNFeatureLayer.Visible == true)
        //    {
        //        LabelUserDef m_LabelUserDef = new LabelUserDef();
        //        ISpatialFilter spatialFilter = new SpatialFilter() as ISpatialFilter;
        //        spatialFilter.WhereClause = "left(DWDM,9)=" + sjqhdm;
        //        IFeatureCursor m_FeatureCursor = m_CUNFeatureLayer.FeatureClass.Search(spatialFilter, false);


        //        IFeature m_Feature = m_FeatureCursor.NextFeature();

        //        while (m_Feature != null)
        //        {
        //            //λ�ã�ȡ���ĵ�
        //            area = m_Feature.Shape as IArea;
        //            pt = area.Centroid;

        //            //��ȡ��ע�ı�
        //            string qhdm = m_Feature.get_Value(m_Feature.Fields.FindField("dwdm")).ToString();

        //            DataTable m_DataTableBJZFRY1 = getBJRYfromQHDM(qhdm);
        //            string BJRY = "";

        //            if (m_DataTableBJZFRY1 != null)
        //            {
        //                for (int i = 0; i < m_DataTableBJZFRY1.Rows.Count; i++)
        //                {
        //                    if (m_DataTableBJZFRY1.Rows[i][0] != null)
        //                    {
        //                        BJRY = BJRY + m_DataTableBJZFRY1.Rows[i][0].ToString();
        //                    }
        //                }
        //            }

        //            //��ӱ�ע

        //            if (BJRY != "")
        //            {
        //                ITextElement te = m_LabelUserDef.createTextElement(pt, BJRY);

        //                m_CUNElement.AddElement(te as IElement);
        //            }
        //            m_Feature = m_FeatureCursor.NextFeature();

        //        }

        //        this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        //    }
        //}



        /// <summary>
        /// ��ȡ���ĵ�
        /// </summary>
        /// <param name="m_FeatureClass"></param>
        /// <returns></returns>
        private ArrayList getAreaPointArray(IFeatureClass m_FeatureClass)
        {
            ArrayList arrpoint = new ArrayList();
            IArea area;
            IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
            IFeatureCursor m_FeatureCursor = m_FeatureClass.Search(null, false);

            IFeature m_Feature = m_FeatureCursor.NextFeature();

            while (m_Feature != null)
            {
                area = m_Feature.Shape as IArea;
                pt = area.LabelPoint;
                arrpoint.Add(pt);

                m_Feature = m_FeatureCursor.NextFeature();

            }
            return arrpoint;

        }


        /// <summary>
        /// ��ȡ��ǰ����������feature
        /// </summary>
        /// <param name="p_dbX">x����</param>
        /// <param name="p_dbY">y����</param>
        /// <param name="layername"></param>
        /// <returns></returns>
        private IFeature GetCurrentf(double p_dbX, double p_dbY, string layername)
        {
            IFeature m_Feature = null;

            IEnumLayer m_Layers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
            ILayer m_Layer = m_Layers.Next();

            while (m_Layer != null)
            {
                if (m_Layer.Name == layername)
                {
                    m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);

                    break;
                }
                m_Layer = m_Layers.Next();
            }
            m_Layers.Reset();

            return m_Feature;

        }

        private void bubbleButton11_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            JCZF.MainFrame.Form1 frm = new JCZF.MainFrame.Form1();
            frm.Show();
            //if (this.tbtnEdit.Checked == false)
            //{
            //    this.tbtnEdit.Checked = true;
            //    EditBar.Visible = true;
            //    this.m_axToolbarControlMapEdit.Visible = true;
            //    this.m_axToolbarControlMapEdit.Enabled = true;
            //}
            //else
            //{
            //    this.tbtnEdit.Checked = false;
            //    EditBar.Visible = false;
            //    this.m_axToolbarControlMapEdit.Visible = false;
            //    this.m_axToolbarControlMapEdit.Enabled = false;
            //}
            m_axToolbarControlMapEdit.Visible = true;
        }

        private void frmMapView_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width < 840)
            {
                panelExLocation.Visible = false;
            }
            else
            {
                panelExLocation.Visible = true ;
            }
            //if (this.Width < 640)
            //{
            //    panelEx4.Visible = false;
            //}
            //else 
            //{
            //    panelEx4.Visible = true ;
            //}
            if (Panel_LayerControl.Visible == true)
            {
                Panel_LayerControl.Top = bubbleBar1.Bottom + 1;
                Panel_LayerControl.Height = axMapControl1.Height - bubbleBar1.Bottom-1;
            }
            this.Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Panel_LayerControl.Visible = false;
        }

        private void bubbleButton12_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            Panel_LayerControl.Visible = !Panel_LayerControl.Visible;
        }

        private void frmMapView_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        /// <summary>
        /// ��������ť ����  ����0731
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton13_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            if (m_frmSelAnalysis_hc==null || m_frmSelAnalysis_hc.IsDisposed)
            {
                m_frmSelAnalysis_hc = new frmSelAnalysis_hc(axMapControl1, this);
                m_frmSelAnalysis_hc.m_DataAccess_SYS = this.m_DataAccess_SYS;
            }
            b_hcselectDK = false;
            b_hcDrawDK = false;
            b_hcImportDK = false;
            b_GetDrawDK = false;

            //bubbleButton13.Image = global::JCZF.Properties.Resources._32x32_ͼ�η���_hot;

            m_frmSelAnalysis_hc.Show();
            m_frmSelAnalysis_hc.Focus();
        }

        /// <summary>
        /// ɾ����ʱͼ�㣬 ����20110813
        /// ɾ��ͼ�������а��������غ˲顱�򡰿���˲顱
        /// �������������غ˲�ԭ���ݡ��򡰿���˲�ԭ���ݡ���ѡ�еĶ���
        /// ������ɾ��ȷ�϶Ի�����ʹ�û�ȷ��Ҫɾ���ö������������ 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void bubbleButton14_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetFalse(false);
            try
            {
                DialogResult result = MessageBox.Show("ȷ��ɾ����ʱ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    //��ղ�ѯ��ѡҪ��
                    this.axMapControl1.Map.ClearSelection();
                   
                    arryOfSelFea.Clear();

                    IGraphicsContainer m_graphicsContainer = (IGraphicsContainer)this.axMapControl1.Map;
                   
                    m_graphicsContainer.DeleteAllElements();//ɾ��������ʱͼ��
                    //this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    this.axMapControl1.ActiveView.Refresh();
                    if (true == RemoveTempLayer())
                    {
                        //MessageBox.Show("ɾ���ɹ�");

                        DeleteTempData();
                    }
                    else
                    {
                        //MessageBox.Show("�Բ���ɾ��ʧ��");
                    }
                }
                else
                {
                    return;
                }
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this, errs.Message);
            }

        }

        /// <summary>
        /// ɾ����ʱͼ��
        /// </summary>
        private bool RemoveTempLayer()
        {

            try
            {
                ////ɾ������Ҫ��?
                //if (g != null)
                //{
                //    g.DeleteAllElements();
                //}

                //IEnumLayer m_EnumLayer = MapFunction.GetLayers(this.axMapControl1.Map);
                //int index = 0;
                //ILayer m_Layer = m_EnumLayer.Next();

                //while (m_Layer != null)
                //{

                //    if (m_Layer.Name.Contains("���غ˲�") || m_Layer.Name.Contains("�ؿ������ʱͼ��"))
                //    {
                //        axMapControl1.Map.DeleteLayer(m_Layer);
                        
                //    }

                //    m_Layer = m_EnumLayer.Next();
                   
                //}
                //m_EnumLayer.Reset();
                string[] m_strLayerNames=new string[1] {"�˲���"};
                if (clsMapFunction.MapFunction.RemoveLayer(m_strLayerNames, this.axMapControl1))
                {
                return true;
                }
                else 
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void DeleteTempData()
        {
            try
            {
                //IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
                //if (hcjg != null)
                //{
                //    axMapControl1.Map.DeleteLayer((ILayer)hcjg);

                //}
                clsMapFunction.MapFunction.RemoveLayerGroup("�˲���", this.axMapControl1);

                if (System.IO.Directory.Exists(Application.StartupPath + "\\OverlayTemp"))
                {
                    DeleteFolder(Application.StartupPath + "\\OverlayTemp");
                }
            }
            catch (Exception ex)
            { }
        }

        private void DeleteFolder(string dir)
        {
            foreach (string d in System.IO.Directory.GetFileSystemEntries(dir))
            {
                if (System.IO.File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//ֱ��ɾ�����е��ļ�   
                }
                else
                    DeleteFolder(d);//�ݹ�ɾ�����ļ���   
            }
            Directory.Delete(dir);//ɾ���ѿ��ļ���   
        }

        private IGroupLayer GetHcjggrouplayer(string grouplayername)
        {
            IGroupLayer ResGrouplayer = null;
            IEnumLayer m_Grouplayers = Functions.MapFunction.GetGroupLayers(axMapControl1.ActiveView.FocusMap);
            ILayer pLayer = m_Grouplayers.Next();
            while (pLayer != null)
            {
                if (pLayer.Name == grouplayername)
                {
                    ResGrouplayer = (IGroupLayer)pLayer;
                    break;
                }
                pLayer = m_Grouplayers.Next();
            }

            m_Grouplayers.Reset();
            return ResGrouplayer;
        }


        /// <summary>
        ///  �鿴����
        /// </summary>

        private void ShowAttribute(double p_dbX, double p_dbY)
        {
            //try
            //{
            //    this.axMapControl1.Map.ClearSelection();
            //    this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
            //    //��õ�ǰ�������Ĺ��ߣ�ʹ����ʱ��Ч
            //    ITool iTool = axMapControl1.CurrentTool;
            //    if (iTool != null)
            //        this.axMapControl1.CurrentTool = (ITool)new Controls3DAnalystContourTool();

            //    if (this.m_frmIDentifyDialog == null || m_frmIDentifyDialog.IsDisposed == true)
            //    {
            //        m_frmIDentifyDialog = new clsMapFunction.IDentifyDialog(this.axMapControl1, arryOfSelFea);
            //        m_frmIDentifyDialog.CloseIdentifyFormEvent += new clsMapFunction.IDentifyDialog.CloseIdentifyFormEventHandler(identifyForm_CloseIdentifyFormEvent);
            //        m_frmIDentifyDialog.CurrentLayerChangeEvent += new clsMapFunction.IDentifyDialog.CurrentLayerChangeEventHandler(identifyForm_CurrentLayerChangeFormEvent);
            //        m_frmIDentifyDialog.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1
            //    }
            //    this.m_frmIDentifyDialog.Show();

            //    GetarryOfSelFea(p_dbX, p_dbY);
            //    curLayer = this.m_frmIDentifyDialog.curLayer;
            //    ////�������Բ�ѯ����
            //    //this.m_frmIDentifyDialog.setSelFeature(arryOfSelFea, curLayer);
            //    //�����ʽ
            //   // this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerIdentify;

            //}
            //catch (Exception e1)
            //{
            //    Debug.WriteLine("�鿴���Գ���" + e1.Message);
            //}


            try
            {
                this.axMapControl1.Map.ClearSelection();
                this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                //��õ�ǰ�������Ĺ��ߣ�ʹ����ʱ��Ч
                ITool iTool = axMapControl1.CurrentTool;
                if (iTool != null)
                    this.axMapControl1.CurrentTool = (ITool)new Controls3DAnalystContourTool();

                if (this.m_frmIDentifyDialog == null || m_frmIDentifyDialog.IsDisposed == true)
                {
                    m_frmIDentifyDialog = new clsMapFunction.IDentifyDialog(this.axMapControl1, arryOfSelFea);
                    m_frmIDentifyDialog.CloseIdentifyFormEvent += new clsMapFunction.IDentifyDialog.CloseIdentifyFormEventHandler(identifyForm_CloseIdentifyFormEvent);
                    m_frmIDentifyDialog.CurrentLayerChangeEvent += new clsMapFunction.IDentifyDialog.CurrentLayerChangeEventHandler(identifyForm_CurrentLayerChangeFormEvent);
                    m_frmIDentifyDialog.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1
                }
                this.m_frmIDentifyDialog.Show();

                this.m_frmIDentifyDialog.m_dbPoint_X = p_dbX;
                this.m_frmIDentifyDialog.m_dbPoint_Y = p_dbY;
                this.m_frmIDentifyDialog.InitSelectedData();

                //GetarryOfSelFea(p_dbX, p_dbY);
                curLayer = this.m_frmIDentifyDialog.curLayer;
                ////�������Բ�ѯ����
                //this.m_frmIDentifyDialog.setSelFeature(arryOfSelFea, curLayer);
                //�����ʽ
                // this.axMapControl.MousePointer = esriControlsMousePointer.esriPointerIdentify;

            }
            catch (Exception e1)
            {
                Debug.WriteLine("�鿴���Գ���" + e1.Message);
            }

        }
        /// <summary>
        /// ���Դ���ر��¼�,�����ʽ�ص�ȱʡֵ 
        /// </summary>
        public void identifyForm_CloseIdentifyFormEvent()
        {
            //Debug.Write("�ر�") ;
            //this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            this.axMapControl1.Map.ClearSelection();
            this.axMapControl1.ActiveView.Refresh();
            //this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            //axMapControl1.ActiveView.ScreenDisplay.UpdateWindow();//�������ǹؼ�
        }

        public void identifyForm_CurrentLayerChangeFormEvent(FeatureLayer p_curLayer)
        {
            curLayer=p_curLayer;
        }

        //private void GetarryOfSelFea(double p_dbX,double p_dbY)
        //{

        //    IPoint point = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
        //    //Set points coordinates
        //    point.PutCoords(p_dbX, p_dbY);

        //    //QI for ITopologicalOperator interface through IPoint interface
        //    ITopologicalOperator topologicalOperator = (ITopologicalOperator)point;
        //    //Create a polygon by buffering the point and get the IPolygon interface
        //    double distance;
        //    distance = ComputeDistance(4);
        //    IPolygon polygon = (IPolygon)topologicalOperator.Buffer(distance);
        //    //QI for IRelationalOperator interface through IPolygon interface
        //    //IRelationalOperator relationalOperator = (IRelationalOperator) polygon;
        //    //��ȡ��ѡҪ��
        //    //curLayer=this.mapFuntion.getCurrentFeatureLayer(1);

        //    //arryOfSelFea =this.mapFuntion.selectFeatures(polygon, curLayer);	
        //    arryOfSelFea = GetClosestFeature(polygon, curLayer);
        //    //�������Բ�ѯ����
        //    if (this.m_frmIDentifyDialog != null && this.m_frmIDentifyDialog.IsDisposed == false)
        //    {
        //        this.m_frmIDentifyDialog.setSelFeature(arryOfSelFea, curLayer);
        //    }


        //}
        //private ArrayList GetClosestFeature(IGeometry selectGeometry, FeatureLayer fLayer)
        //{
        //    int m_intTemp = 0;
        //    this.axMapControl1.ActiveView.FocusMap.ClearSelection();
        //    IFeatureSelection featureSelection;
        //    featureSelection = fLayer as IFeatureSelection;

        //    //this.arryOfSelFea.Clear();
        //    string[] layerNames = null;
        //    //int num = this.axMapControl.ActiveView.FocusMap.LayerCount;
        //    layerNames = m_MapFunction.getVisibleLayerNames();
        //    int num = layerNames.Length;
        //    string selName = null;

        //    ArrayList pSelected = new ArrayList();
        //    FeatureLayer curSelLayer = null;
        //    IFeatureLayer templayer = null;

        //    IEnvelope pSrchEnv = selectGeometry.Envelope;
        //    UID pUID = new ESRI.ArcGIS.esriSystem.UIDClass();
        //    pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";

        //    for (int i = 0; i < num; i++)
        //    {
        //        try
        //        {
        //            selName = layerNames[i];
        //            if (selName == "") continue;
        //            templayer = m_MapFunction.getFeatureLayerByName(selName);
        //            //templayer =(IFeatureLayer)this.axMapControl.ActiveView.FocusMap.get_Layer(i);
        //            curSelLayer = templayer as FeatureLayer;
        //            IGeoFeatureLayer pGeoLayer = curSelLayer as IGeoFeatureLayer;
        //            if (pGeoLayer.Selectable && pGeoLayer.FeatureClass != null && pGeoLayer.Visible == true)
        //            {
        //                IIdentify2 pID;
        //                pID = pGeoLayer as IIdentify2;
        //                IArray pIDArray;
        //                pIDArray = pID.Identify(pSrchEnv, null);

        //                IFeatureIdentifyObj ipFeatIdObj;
        //                IRowIdentifyObject pRowObj;
        //                IFeature pFeature;
        //                if (pIDArray != null)
        //                {
        //                    m_intTemp = m_intTemp + 1;
        //                    curLayer = curSelLayer;
        //                    ArrayList arrayofFeaInOneLayer = new ArrayList();
        //                    arrayofFeaInOneLayer.Add(layerNames[i]);
        //                    for (int j = 0; j < pIDArray.Count; j++)
        //                    {
        //                        if (pIDArray.get_Element(j) is IFeatureIdentifyObj)
        //                        {
        //                            ipFeatIdObj = (IFeatureIdentifyObj)pIDArray.get_Element(j);
        //                            pRowObj = (IRowIdentifyObject)ipFeatIdObj;
        //                            pFeature = (IFeature)pRowObj.Row;

        //                            if (fLayer != null && pGeoLayer.Name == fLayer.Name)
        //                            {
        //                                featureSelection.Add(pFeature);
        //                            }
        //                            else if (fLayer == null)
        //                            {
        //                                //featureSelection.Add(pFeature);
        //                                this.axMapControl1.Map.SelectFeature(templayer, pFeature);
        //                            }
        //                            arrayofFeaInOneLayer.Add(pFeature);
        //                        }
        //                    }
        //                    pSelected.Add(arrayofFeaInOneLayer);
        //                    pIDArray.RemoveAll();
        //                }
        //            }
        //        }
        //        catch(SystemException errs)
        //        {
        //            Debug.WriteLine("�鿴���Գ���" + errs.Message);
        //            continue;
        //        }
        //    }
        //    this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, fLayer, null);
        //    return pSelected;
        //}


        ///// <summary>
        ///// ��������������ѻ������
        ///// </summary>
        ///// <returns></returns>
        //private double ComputeDistance(double pixelunits)
        //{
        //    double distance = 0;
        //    try
        //    {
        //        double realWorldDisplayExtent;
        //        double sizeofOnePixel;
        //        //			tagRECT DeviceFrame = this.axMapControl.ActiveView.ScreenDisplay.DisplayTransformation.DeviceFrame;
        //        //            pixelExtent = DeviceFrame.right-DeviceFrame.left;
        //        IPoint p1 = this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperLeft;
        //        IPoint p2 = this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperRight;
        //        int x1, x2, y1, y2;
        //        this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(p1, out x1, out y1);
        //        this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(p2, out x2, out y2);
        //        double pixelExtent = x2 - x1;
        //        realWorldDisplayExtent = this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
        //        sizeofOnePixel = realWorldDisplayExtent / pixelExtent;
        //         distance = pixelunits * sizeofOnePixel;
                
        //    }
        //    catch (SyntaxErrorException errs)
        //    {
        //        clsFunction.Function.MessageBoxError(errs.Message);
        //    }
        //    return distance;
        //}

        private void bubbleButton_Delete_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            try
            {
                bubbleButton_Delete.Image = global::JCZF.Properties.Resources._32x32_ɾ��_hot;
                SetFalse(false);
                SetBubbleButtonNormal();

                m_bSelTB = true;
                //this.axMapControl1.CurrentTool = null;
                //this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
                //this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
                //Isshowpanel = false;
                //panelEx1.Visible = false;         

                bubbleButton_Delete.Image = global::JCZF.Properties.Resources._32x32_ɾ��_hot;

                if (axMapControl1.ActiveView.FocusMap.SelectionCount > 0)
                {
                    if (curLayer.Name.Contains("���غ˲�") && (!curLayer.Name.Contains("ԭʼ")))
                    {
                        if (MessageBox.Show("ɾ���󽫲��ָܻ���\n\n�Ƿ����ɾ����", "ɾ����ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (clsMapFunction.MapFunction.DeleteSelectedFeature(curLayer, axMapControl1))
                            {
                                //clsFunction.Function.MessageBoxInformation("ɾ���ɹ���");
                                //if (DeleteSelectFeatureEvent != null)
                                //{
                                //    DeleteSelectFeatureEvent(sender, e);
                                //}
                                if (this.m_frmIDentifyDialog != null)
                                {
                                    this.m_frmIDentifyDialog.InitSelectedData();
                                }
                            }

                            
                        }
                    }
                }

                bubbleButton_Delete.Image = global::JCZF.Properties.Resources._32x32_ɾ��;

                SetbubleHot();
            }
            catch(SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
        }

        private void bubbleButton_ComputeDistance_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetBubbleButtonNormal();
            this.axMapControl1.CurrentTool = null;

            m_blStartComputeDistance_Check = !m_blStartComputeDistance_Check;
           
            if (m_blStartComputeDistance_Check == true)
            {
                labelCompute.Text = "���ȣ�";
                panelEx_Compute.Visible = true;
                m_IPolyline_Survey = null;                
                m_IPolyline_Survey = new Polyline() as IPolyline ;
                m_dbMesureResult = 0;
                m_IPoint_LastDraw = null;

                m_blStartComputeArea = false;
                m_blStartComputeArea_Check = false;

                // �ı�������㰴ť����ɫ               
                bubbleButton_ComputeDistance.Image = global::JCZF.Properties.Resources._32x32_��������_hot;
                bubbleBar1.Refresh();
            }
            else
            {
                m_blStartComputeDistance = false ;
                panelEx_Compute.Visible = false ;
                m_IPolyline_Survey = null;              
            }
            m_dbMesureResult = 0;
        
        }
        private void ComputeDistance_MouseDown(double p_dbX,double p_dbY )
        {
            
            IPoint m_IPoint = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
            m_IPoint.X = p_dbX;
            m_IPoint.Y = p_dbY;
            ComputeDistance_MouseDown(m_IPoint);

        }
        private void ComputeDistance_MouseDown(IPoint p_IPoint)
        {
            if (m_blStartComputeDistance_Check == false )
            {
                return;
            }
            m_blStartComputeDistance = true;
            labelCompute.Text = "����Ϊ�� ";

            IGeometry m_SurveyGeometry = this.axMapControl1.TrackLine();         


            IGraphicsContainer m_IGraphicsContainer = axMapControl1.ActiveView.FocusMap as IGraphicsContainer;
            m_IGraphicsContainer.DeleteAllElements();

            m_IPolyline_Survey = (IPolyline)m_SurveyGeometry;
            //MapFunction.DrawTempPolyLineOnMap(m_IPolyline_Survey, axMapControl1);

            //axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            m_dbMesureResult = MapFunction.MeasureLength(m_SurveyGeometry);
            labelCompute.Text = "����Ϊ�� " + m_dbMesureResult.ToString("#.00") + " ��";
            clsFunction.Function.MessageBoxInformation("����Ϊ�� " + m_dbMesureResult.ToString("#.00") + " ��");
           
           

            return;

            /*   //���治�õģ�������ܹ�ʵ��ʵʱ��ʾ�ߵĳ��ȣ������ٶ�̫����

                        ILine m_ILine_Temp = new Line() as ILine;

                        try
                        {               
                            object m_objMissing = Type.Missing;
               
                
                            if (m_IPoint_LastDraw != null)
                            {
                                //���ǵ�һ����
                                m_ILine_Temp.PutCoords(m_IPoint_LastDraw, p_IPoint);                    
                    
                                m_dbMesureResult = m_dbMesureResult + m_ILine_Temp.Length;
                                labelCompute.Text = "����Ϊ�� " + m_dbMesureResult.ToString("#.00") + " ��";

                                MapFunction.DrawTempLineOnMap(m_ILine_Temp, axMapControl1);
                                m_IPoint_LastDraw = p_IPoint;

                                //�������������//////////////////////////////////
                                ISegment m_ISegment = m_ILine_Temp as ISegment;
                                ISegmentCollection m_Path = new  ESRI.ArcGIS.Geometry.Path() as ISegmentCollection;

                                m_Path.AddSegment(m_ISegment, ref m_objMissing, ref m_objMissing);
                                if (m_IPolyline_Survey == null) m_IPolyline_Survey = new Polyline() as IPolyline ;
                                m_IPolyline_Survey.AddGeometry(m_Path as IGeometry, ref m_objMissing, ref m_objMissing);
                                /////////////////////////////////////////////////////
                            }
                            else
                            {
                                m_IPoint_LastDraw = p_IPoint;
                                return;
                            }
               
                        }
                        catch (SyntaxErrorException errs)
                        {
                            clsFunction.Function.MessageBoxError(errs.Message);
                        }
             */
        }

        private void ComputeDistance_MouseMove(double p_dbX, double p_dbY)
        {
            IPoint m_IPoint = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
            m_IPoint.X = p_dbX;
            m_IPoint.Y = p_dbY;
            ComputeDistance_MouseMove(m_IPoint);
        }
        private void ComputeDistance_MouseMove(IPoint p_IPoint)
        {
            return;

             /*   //���治�õģ�������ܹ�ʵ��ʵʱ��ʾ�ߵĳ��Ⱥͻ����ߣ������ٶ�̫����
            try
            {                 
               
                if (m_IPoint_LastDraw == null) return;
                
                
                ILine m_ILine = new Line() as ILine;
                m_ILine.PutCoords(m_IPoint_LastDraw, p_IPoint);
                double m_db = m_dbMesureResult + m_ILine.Length;
                labelCompute.Text = "����Ϊ�� " + m_db.ToString("#.00") + " ��";

                IGraphicsContainer graphicsContainer = (IGraphicsContainer)this.m_ActiveView.FocusMap;

                if (m_IElement_Survey != null)
                {
                    graphicsContainer.DeleteElement(m_IElement_Survey);
                }
                m_IElement_Survey=MapFunction.DrawTempLineOnMap(m_ILine, axMapControl1);
            }
            catch (SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
              */ 
        }


        private void ComputeDistance_DoubleClick(double p_dbX, double p_dbY)
        {
            IPoint m_IPoint = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
            m_IPoint.X = p_dbX;
            m_IPoint.Y = p_dbY;
            ComputeDistance_DoubleClick(m_IPoint);           
        }
        private void ComputeDistance_DoubleClick(IPoint p_IPoint)
        {         
            ComputeDistance_MouseDown(p_IPoint);          

            clsFunction.Function.MessageBoxInformation("����Ϊ " + m_dbMesureResult.ToString("##.00") + "   �ף�");

            panelEx_Compute.Visible = false;
            m_blStartComputeDistance_Check = false;

            ////�任�����İ�ť��ɫ
            //bubbleButton_ComputeArea.Image = global::JCZF.Properties.Resources._32x32_�������;
           

            //ɾ����������
            IGraphicsContainer graphicsContainer = (IGraphicsContainer)this.m_ActiveView.FocusMap;
            graphicsContainer.DeleteAllElements();
        }
        private void bubbleButton_ComputeArea_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetBubbleButtonNormal();
            this.axMapControl1.CurrentTool = null;
            m_blStartComputeArea_Check = !m_blStartComputeArea_Check;

            if (m_blStartComputeArea_Check == true)
            {
                labelCompute.Text = "�����";
                panelEx_Compute.Visible = true  ;
                m_IPolygon_Survey = null;
                m_IPolygon_Survey = new Polygon() as IPolygon ;

                m_blStartComputeDistance = false;
                m_blStartComputeDistance_Check = false;

                // �ı�������㰴ť����ɫ

                bubbleButton_ComputeArea.Image = global::JCZF.Properties.Resources._32x32_�������_hot;

                bubbleBar1.Refresh();
            }
            else
            {
                m_blStartComputeArea = false;
                panelEx_Compute.Visible = false;
                m_IPolygon_Survey = null;
            }
            m_dbMesureResult = 0;
        }

        private void ComputeArea_MouseDown(double p_dbX, double p_dbY)
        {
            IPoint m_IPoint = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
            m_IPoint.X = p_dbX;
            m_IPoint.Y = p_dbY;
            ComputeArea_MouseDown(m_IPoint);  
        }
        private void ComputeArea_MouseDown(IPoint p_IPoint)
        {
            m_blStartComputeArea = true;

            //* �����ø��ٲ㣬����ֻ�ܵ������ܵõ�����

            IGraphicsContainer m_IGraphicsContainer = axMapControl1.ActiveView.FocusMap as IGraphicsContainer;
            IGeometry m_SurveyGeometry = this.axMapControl1.TrackPolygon();

            //�Ѹ��ٲ��϶�����Ƶ���ʱ����

            //IPolygonElement m_IPolygonElement = new PolygonElement() as IPolygonElement;

            //IFillShapeElement fillShapeElement = (IFillShapeElement)m_IPolygonElement;
            //IFillSymbol fillSymbol = Functions.MapFunction.GetStaticSymbol();
            //fillShapeElement.Symbol = fillSymbol;

            //IElement m_elementPolygonElement = null;
            //m_elementPolygonElement = m_IPolygonElement as IElement;
            //m_elementPolygonElement.Geometry = m_SurveyGeometry as IGeometry;
            //m_IGraphicsContainer.DeleteAllElements();
            //m_IGraphicsContainer.AddElement(m_elementPolygonElement, 0);

            //axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);


            m_dbMesureResult = MapFunction.MeasureArea(m_SurveyGeometry);
            labelCompute.Text = "���Ϊ�� " + (m_dbMesureResult / (1000 * 2 / 3)).ToString("#0.00") + " Ķ";
            clsFunction.Function.MessageBoxInformation("���Ϊ�� " + (m_dbMesureResult / 1000000).ToString("#0.00") + " ƽ������\n\n         " + (m_dbMesureResult / (1000 * 2 / 3)).ToString("#0.00") + " Ķ\n\n         " + m_dbMesureResult.ToString("#0.0") + " ƽ����");


            

            ////////////////////////////////

        }

        private void ComputeArea_MouseMove(double p_dbX, double p_dbY)
        {
        }
        private void ComputeArea_MouseMove(IPoint p_IPoint)
        {
        }

        private void ComputeArea_DoubleClick(double p_dbX, double p_dbY)
        {
        }
        private void ComputeArea_DoubleClick(IPoint p_IPoint)
        {
        }

        private void axMapControl1_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            if (m_blStartComputeArea == true)
            {
                ComputeArea_DoubleClick(e.mapX, e.mapY);
            }
            if (m_blStartComputeDistance == true)
            {
                ComputeDistance_DoubleClick(e.mapX, e.mapY);
            }

        }

        private void btnGoToPosition_Click(object sender, EventArgs e)
        {

            if (txtX.Text.Trim() == "")
            {
                clsFunction.Function.MessageBoxError("����X�������������Ҫ��λ������ֵ��");
                txtX.Focus();
                return;
            }
            if (txtX.Text.Trim() != "")
            {
                if (clsMapFunction.MapFunction.PointXIsInMapExtent(System.Convert.ToDouble(txtX.Text), axMapControl1) == false)
                {
                    clsFunction.Function.MessageBoxError("X����ֵ��ͼ��ͼ��Χ��");

                    txtX.Focus();
                    return;
                }
               
            }            
        
            if (txtY.Text.Trim() == "")
            {
                clsFunction.Function.MessageBoxError("����Y�������������Ҫ��λ������ֵ��");
                txtY.Focus();
                return;
            }

            if (txtY.Text.Trim() != "")
            {
                if (clsMapFunction.MapFunction.PointYIsInMapExtent(System.Convert.ToDouble(txtY.Text), axMapControl1) == false)
                {
                    clsFunction.Function.MessageBoxError("Y����ֵ��ͼ��ͼ��Χ��");

                    txtY.Focus();
                    return;
                }
               
            }   

            clsMapFunction.MapFunction.GoToPoint(System.Convert.ToDouble(txtX.Text), System.Convert.ToDouble(txtY.Text), axMapControl1);
            //axMapControl1.Refresh();
        }

        private void txtX_TextChanged(object sender, EventArgs e)
        {
            if (clsFunction.Function.IsNumber(txtX.Text)==false )
            {
                if (txtX.Text.Trim() != "")
                {
                    txtX.Focus();
                    txtX.SelectAll();
                    clsFunction.Function.MessageBoxError("��������������֣�");
                }
            }
        }

        private void txtY_TextChanged(object sender, EventArgs e)
        {
            if (clsFunction.Function.IsNumber(txtY.Text) == false)
            {
                if (txtY.Text.Trim() != "")
                {
                    txtY.Focus();
                    txtY.SelectAll();
                    clsFunction.Function.MessageBoxError("��������������֣�");
                }
            }
        }

        private void bubbleButton_GoToPosition_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            SetBubbleButtonNormal();
            this.axMapControl1.CurrentTool = null;

            panelEx_Compute.Visible = false;

            if (panelEx_GoToPosition.Visible == false)
            {
                panelExLocation.Visible = false ;
                panelEx_GoToPosition.Visible = true;
                IPoint m_IPoint = clsMapFunction.MapFunction.GetMapCenterPoint(axMapControl1);

                txtX.Text = m_IPoint.X.ToString("0.00");
                txtY.Text = m_IPoint.Y.ToString("0.00");

                // �ı�������㰴ť����ɫ               
                bubbleButton_GoToPosition.Image = global::JCZF.Properties.Resources._32x32_��λ_hot2;
                bubbleBar1.Refresh();
            }
            else
            {
                panelExLocation.Visible = true ;
                panelEx_GoToPosition.Visible = false;
                // �ı�������㰴ť����ɫ               
                bubbleButton_GoToPosition.Image = global::JCZF.Properties.Resources._32x32_��λ2;
                bubbleBar1.Refresh();
            }
        }

        /// <summary>
        /// print
        /// </summary>
        public void PassToPage()
        {
            this.m_MapFunction = this.InitMapFunction(this.axMapControl1, this.m_axPageLayoutControl);
            m_frmPageLayoutControl.mapFuntion = this.m_MapFunction;
            m_frmPageLayoutControl.axMapControl  = this.axMapControl1 ;

            m_frmPageLayoutControl.m_MapDocument  = this.m_MapDocument;

        }
        private clsMapFunction.MapFunction InitMapFunction(AxMapControl p_axMapControl, AxPageLayoutControl p_aAxPageLayoutControl)
        {
            clsMapFunction.MapFunction m_MapFunction;
            m_MapFunction = new clsMapFunction.MapFunction(this.axMapControl1 , m_axPageLayoutControl);
            return m_MapFunction;
        }

        public void CopyAndOverwritePage()
        {
            try
            {
                IObjectCopy m_ObjectCopy = new ObjectCopy() as IObjectCopy;

                object m_toCopyMap = this.axMapControl1.ActiveView.FocusMap;

                object m_CopiedMap = m_ObjectCopy.Copy(m_toCopyMap);

                object m_toOverwriteMap = this.m_axPageLayoutControl.ActiveView.FocusMap;

                m_ObjectCopy.Overwrite(m_CopiedMap, ref m_toOverwriteMap);

                this.m_axPageLayoutControl.ActiveView.Refresh();

            }
            catch (SystemException errs)
            {
            }
        }
        
    }
}