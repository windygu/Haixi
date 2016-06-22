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

using JCZF.MainFrame;

namespace JCZF.SubFrame
{
    public partial class frmMapViews : Form
    {

        private string m_strCurrentMapFileName_;
        public string m_strCurrentMapFileName
        {
            set
            {
                m_strCurrentMapFileName_ = value;
            }
        }

        public int m_intMapCount;
        ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_Selected ;
        int m_intMapControl_SelectedIndex;

        private string m_FeatureLayerName = "";
        //private frmShowPic m_frmShowPic;
        public int m_OID = -1;

        private DataTable m_DataTableBJZFRY;
        private DataTable m_DataTableSJZFRY;

        public string[] m_strMapName;


        #region 父窗体

        public frmMain m_pFrmMain = null;

        public MapFunction m_MapFunction = new MapFunction();


        #endregion

        public delegate void frmMapView_ActivateEventHandler(bool p_IsActivated);
        public event frmMapView_ActivateEventHandler frmMapView_Activate;

        public clsDataAccess.DataAccess m_DataAccess_SYS;


        public IActiveView m_ActiveView;
        //打开的mxd文件
        public IMapDocument m_MapDocument;

        #region 地图常数

        private esriUnits m_MapUnits;
        private string m_sMapUnits;

        private double m_tempX = 0;
        private double m_tempY = 0;


        #endregion

        private bool m_bShowTBDS = false;
        private string m_strDM="";

        private string m_strDM1 = "";

        

        private bool m_bSelTB = false;
        private IFeature m_CurFeature;

        private ArrayList arrdlmc = new ArrayList();
        private ArrayList dbldltb = new ArrayList();

        bool Isshowpanel = false ;

        bool[] m_blBtiTDLYXZChecked;
        bool[] m_blBtiTDLYGHChecked;
        bool[] m_blBtiJSYDSPChecked;
        bool[] m_blBtiCKQChecked;
        bool[] m_blBtiTKQChecked;
        bool[] m_blBtiYGYXChecked;
        bool[] m_blBtiJBNTChecked;
        bool[] m_blBtiKCZYGHChecked;

        private DevComponents.DotNetBar.ButtonItem m_ImageShow;

        ArrayList m_ILayersImage;
        ArrayList m_ILayersTDLY;
        ArrayList m_ILayersCKQ;
        ArrayList m_ILayersTKQ;
        ArrayList m_ILayersTDGY;
        ArrayList m_ILayersJSYD;
        ArrayList m_ILayersTDGH;
        ArrayList m_ILayersJBNT;

        public IFeature m_hcselfeature;

        public frmMapViews(frmMain parentForm)
        {
            InitializeComponent();
            //this.m_pFrmMain = parentForm;
            //this.m_pFrmMain.m_bIsMapViewFormOpen = true;
            //this.m_pFrmMain.m_MapFuction.axMapControl = this.m_axMapControl1;

            m_MapFunction.axMapControl = this.m_axMapControl1;
            m_AxMapControl_Selected = this.m_axMapControl1;
           
        }

        private void LoadDefaultMap()
        {
            this.LoadFile(Application.StartupPath + @"\work\" + m_strCurrentMapFileName_);
        }

        private void frmGPSMointors_Load(object sender, EventArgs e)
        {
            //if (this.m_pFrmMain.m_bIsFirstStart)
            //{
            LoadDefaultMap();

            setImageBtni();
            setTDLYXZBtni();
            //}
            this.WindowState = FormWindowState.Maximized;

            //ResizeMapSzie();

            m_blBtiCKQChecked = new bool[4];
            m_blBtiJBNTChecked = new bool[4];
            m_blBtiJSYDSPChecked = new bool[4];
            m_blBtiTDLYGHChecked = new bool[4];
            m_blBtiTDLYXZChecked = new bool[4];
            m_blBtiTKQChecked = new bool[4];
            m_blBtiYGYXChecked = new bool[4];
            m_blBtiKCZYGHChecked = new bool[4];

            //ResizeMap();
            //InitMaps();

            m_AxMapControl_Selected = m_axMapControl1;
            SetSelectedMapBtiOption();
           // m_intMapControl_SelectedIndex = System.Convert.ToInt32(this.m_AxMapControl_Selected.Name.Substring(this.m_AxMapControl_Selected.Name.Length - 1, 1)) - 1;


        }

        public  void  InitMaps()
        {
           for(int i=0;i<m_strMapName.Length ;i++)
           {
               if (i == 0)
               {
                   m_AxMapControl_Selected = m_axMapControl1;
               }
               else if (i == 1)
               {
                   m_AxMapControl_Selected = m_axMapControl2;
               }
               else if (i == 2)
               {
                   m_AxMapControl_Selected = m_axMapControl3;
               }
               else if (i == 3)
               {
                   m_AxMapControl_Selected = m_axMapControl4;
               }
               
               
               switch (m_strMapName[i])
               {
                   case "土地利用现状图":
                       btnTDLYXZ_Click(null, null);
                       break;
                   case "土地利用规划图":
                       btnTDLYGH_Click(null, null);
                       break;
                   case "基本农田图":
                       btnJBNTBH_Click(null, null);
                       break;
                   case "建设用地审批图":
                       btnJSXM_Click(null, null);
                       break;
                   case "采矿权登记图":
                       btnCKQFZ_Click(null, null);
                       break;
                   case "探矿权登记图":
                       btnTKQFZ_Click(null, null);
                       break;
                   case "遥感图像":
                       break;

               }
           }

           m_AxMapControl_Selected = m_axMapControl1;

           SetSelectedMapBtiOption();
        }

        public void LoadFile(string filepath)
        {
            if (this.m_AxMapControl_Selected.CheckMxFile(filepath))
            {

                this.m_axMapControl1.LoadMxFile(filepath);
                GetBtiLayers(this.m_axMapControl1);

                this.m_axMapControl2.LoadMxFile(filepath);
                GetBtiLayers(this.m_axMapControl2);

                this.m_AxMapControl_Selected = m_axMapControl1;

                this.InitMap();
                //Open document
                OpenDocument(filepath);

                if (m_intMapCount == 4)
                {
                    panel2.Visible = true;
                    this.m_axMapControl3.Visible = true;
                    this.m_axMapControl4.Visible = true;

                    this.m_axMapControl3.LoadMxFile(filepath);
                    GetBtiLayers(this.m_axMapControl3);

                    this.m_axMapControl4.LoadMxFile(filepath);
                    GetBtiLayers(this.m_axMapControl4);


                }
                else
                {
                    expandableSplitter3.Visible = false;
                    panel2.Visible = false;
                    this.m_axMapControl3.Visible = false;
                    this.m_axMapControl4.Visible = false;
                }
            }

            if (m_strMapName != null)
            {
                if (m_strMapName.Length == 2 || m_strMapName.Length == 4)
                {
                    btnMapName1.Text = m_strMapName[0];
                    btnMapName2.Text = m_strMapName[1];


                }
                if (m_strMapName.Length == 4)
                {
                    btnMapName3.Text = m_strMapName[2];
                    btnMapName4.Text = m_strMapName[3];
                }

            }
        }

        /// <summary>
        /// 函数——初始化地图相关参数
        /// </summary>
        private void InitMap()
        {
            this.m_ActiveView = this.m_AxMapControl_Selected.ActiveView;
            this.m_MapUnits = this.m_AxMapControl_Selected.MapUnits;
            this.m_sMapUnits = MapFunction.getMapUnits(this.m_MapUnits);
        }

        /// <summary>
        /// 打开mapdocument文件
        /// </summary>
        /// <param name="sFilePath"></param>
        private void OpenDocument(string sFilePath)
        {
            //Create a new map document
            m_MapDocument = new MapDocument() as IMapDocument;
            //Open the map document selected
            m_MapDocument.Open(sFilePath, "");
        }

        #region 浏览工具条
        private void bubbleButton1_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.Pan();
            Isshowpanel = false ;
            panelEx1.Visible = false ;

            if (btiActionLinkage.Checked)
            {
                this.m_MapFunction.Pan(m_axMapControl1);
                this.m_MapFunction.Pan(m_axMapControl2);
                this.m_MapFunction.Pan(m_axMapControl3);
                this.m_MapFunction.Pan(m_axMapControl4);
            }
        }

        private void bubbleButton2_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.ZoomIn();
            Isshowpanel = false;
            panelEx1.Visible = false;

            if (btiActionLinkage.Checked)
            {
                this.m_MapFunction.ZoomIn(m_axMapControl1);
                this.m_MapFunction.ZoomIn(m_axMapControl2);
                this.m_MapFunction.ZoomIn(m_axMapControl3);
                this.m_MapFunction.ZoomIn(m_axMapControl4);
            }
        }

        private void bubbleButton3_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.ZoomOut();
            Isshowpanel = false;
            panelEx1.Visible = false;

            if (btiActionLinkage.Checked)
            {
                this.m_MapFunction.ZoomOut(m_axMapControl1);
                this.m_MapFunction.ZoomOut(m_axMapControl2);
                this.m_MapFunction.ZoomOut(m_axMapControl3);
                this.m_MapFunction.ZoomOut(m_axMapControl4);
            }
        }

        private void bubbleButton4_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.FullExtent();
            Isshowpanel = false;
            panelEx1.Visible = false;

            if (btiActionLinkage.Checked)
            {
                this.m_MapFunction.FullExtent(m_axMapControl1);
                this.m_MapFunction.FullExtent(m_axMapControl2);
                this.m_MapFunction.FullExtent(m_axMapControl3);
                this.m_MapFunction.FullExtent(m_axMapControl4);
            }

        }

        private void bubbleButton5_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.BringToFront();
            Isshowpanel = false;
            panelEx1.Visible = false;

            if (btiActionLinkage.Checked)
            {
                this.m_MapFunction.BringToFront(m_axMapControl1);
                this.m_MapFunction.BringToFront(m_axMapControl2);
                this.m_MapFunction.BringToFront(m_axMapControl3);
                this.m_MapFunction.BringToFront(m_axMapControl4);
            }
        }

        private void bubbleButton6_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.BringForward();
            Isshowpanel = false;
            panelEx1.Visible = false;

            if (btiActionLinkage.Checked)
            {
                this.m_MapFunction.BringForward(m_axMapControl1);
                this.m_MapFunction.BringForward(m_axMapControl2);
                this.m_MapFunction.BringForward(m_axMapControl3);
                this.m_MapFunction.BringForward(m_axMapControl4);
            }
        }

        private void bubbleButton7_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            m_bSelTB = true;
            this.m_AxMapControl_Selected.CurrentTool = null;
            this.m_AxMapControl_Selected.MousePointer = esriControlsMousePointer.esriPointerIdentify;
            //this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
            Isshowpanel = false;
            panelEx1.Visible = false;

            if (btiActionLinkage.Checked)
            {
                this.m_axMapControl1.CurrentTool = null;
                this.m_axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
                this.m_axMapControl2.CurrentTool = null;
                this.m_axMapControl2.MousePointer = esriControlsMousePointer.esriPointerIdentify;
                this.m_axMapControl3.CurrentTool = null;
                this.m_axMapControl3.MousePointer = esriControlsMousePointer.esriPointerIdentify;
                this.m_axMapControl4.CurrentTool = null;
                this.m_axMapControl4.MousePointer = esriControlsMousePointer.esriPointerIdentify;
            }
        }

        private void bubbleButton9_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.Pan();
            Isshowpanel = true ;
            panelEx1.Visible = true;

            if (btiActionLinkage.Checked)
            {
                this.m_MapFunction.Pan(m_axMapControl1);
                this.m_MapFunction.Pan(m_axMapControl2);
                this.m_MapFunction.Pan(m_axMapControl3);
                this.m_MapFunction.Pan(m_axMapControl4);
            }
        }

        #endregion


        private JCZF.SubFrame.AttibuteEdit.FormMenuButton theForm;

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {

            AxMapControl m_AxMapControlTemp;
            m_AxMapControlTemp = (ESRI.ArcGIS.Controls.AxMapControl)sender;

            if (m_AxMapControlTemp.Name != m_AxMapControl_Selected.Name)
            {
                m_AxMapControl_Selected = m_AxMapControlTemp;
                SetSelectedMapBtiOption();
            }
            m_intMapControl_SelectedIndex = System.Convert.ToInt32(this.m_AxMapControl_Selected.Name.Substring(this.m_AxMapControl_Selected.Name.Length - 1, 1)) - 1;


            m_MapFunction.axMapControl = (ESRI.ArcGIS.Controls.AxMapControl)sender;

            this.m_ActiveView = m_AxMapControl_Selected.ActiveView;
            this.m_MapUnits = m_AxMapControl_Selected.MapUnits;
            this.m_sMapUnits = MapFunction.getMapUnits(this.m_MapUnits);

            panelEx_ZFRY_Info.Visible = false ;
            if (this.theForm != null && this.theForm.Disposing == false)
            {
                theForm.Close();
            }

            IActiveView pActiveView = m_AxMapControl_Selected.ActiveView;
           #region 
            if (e.button == 2)
            {
                m_AxMapControl_Selected.MousePointer = esriControlsMousePointer.esriPointerIdentify;

                 ArrayList arr = new ArrayList();
                 IEnumLayer pLayers = MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);
                 ILayer pLayer = pLayers.Next();


                 while (pLayer != null)
                 {
                     if (pLayer.Visible == true)
                     {                    
                         if (pLayer.Name == "土地核查" || pLayer.Name == "矿产核查")
                         {
                             arr.Add(pLayer);
                             //if (pLayer.Name == "土地核查")
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

                    m_AxMapControl_Selected.ActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnvelope, ref r, 4);
                    pEnvelope.SpatialReference = m_AxMapControl_Selected.Map.SpatialReference;

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
                            this.m_hcselfeature = pFeature;
                            pIdObj = (IIdentifyObj)pFeaIdObj;
                            pIdObj.Flash(m_AxMapControl_Selected.ActiveView.ScreenDisplay);
                            int j = pRow.OID;

                            theForm = new JCZF.SubFrame.AttibuteEdit.FormMenuButton();
                            theForm.m_fileContent = "good"; // 灾害类型                            

                            theForm.Location = new System.Drawing.Point(e.x, e.y);
                            System.Drawing.Point MouseClickPoint = new System.Drawing.Point(e.x, e.y);
                            //Convert from Tree coordinates to Screen   
                            System.Drawing.Point ScreenPoint = m_AxMapControl_Selected.PointToScreen(MouseClickPoint);
                            theForm.Left = ScreenPoint.X;
                            theForm.Top = ScreenPoint.Y;

                            theForm.oid = j;
                            theForm.sellayname = selLayer.Name;
                            theForm.m_strTabelName = selLayer.Name;
                            theForm.m_strObjecgID = j.ToString();
                            //theForm.m_frmMapView=this;
                            theForm.m_selFeature=pFeature;
                            theForm.m_selFeatureclass=selLayer.FeatureClass;
                            theForm.m_DataAccess_SYS=this.m_DataAccess_SYS;
                            m_FeatureLayerName = selLayer.Name;

                            JCZF.Renderable.CGlobalVarable.m_theSlideForm = theForm;

                            theForm.Show(); // 显示

                    }

                }

            }
    #endregion

            SetSelectedMapBtiOption();

               
        }

        /// <summary>
        /// 根据选中的地图窗口更新下部Bar的状态
        /// </summary>
        private void SetSelectedMapBtiOption()
        {
            //MessageBox.Show(m_AxMapControl_Selected.Name);
            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                SetSelectedMapBtiOption(0);
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                SetSelectedMapBtiOption(1);
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                SetSelectedMapBtiOption(2);
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                SetSelectedMapBtiOption(3);
            }

            //bar2.Refresh();
        }

        //private void SetSelectedMapBtiOption(int p_intMapControlIndex)
        //{
        //    btnTDLYXZ.Checked = m_blBtiTDLYXZChecked[p_intMapControlIndex];
        //    btnTDLYGH.Checked = m_blBtiTDLYGHChecked[p_intMapControlIndex];
        //    btnTKQFZ.Checked = m_blBtiTKQChecked[p_intMapControlIndex];
        //    btnCKQFZ.Checked = m_blBtiCKQChecked[p_intMapControlIndex];
        //    btnJBNTBH.Checked = m_blBtiJBNTChecked[p_intMapControlIndex];
        //    btnJSXM.Checked = m_blBtiJSYDSPChecked[p_intMapControlIndex];
        //    btnKCZYGH.Checked = m_blBtiKCZYGHChecked[p_intMapControlIndex];
        //}

        /// <summary>
        /// 设置所选择地图的按钮状态
        /// </summary>
        /// <param name="p_IMap"></param>
        /// <param name="p_intIndex"></param>
        private void SetSelectedMapBtiOption(int p_intMapControlIndex)
        {
            //IEnumLayer m_IEnumLayer;
            //m_IEnumLayer=clsMapFunction.MapFunction.GetGroupLayers(p_AxMapControl.Map );
            //if(m_IEnumLayer==null ) return ;
            //ILayer m_ILayer, m_ILayerTemp;
            //ArrayList m_ArrayListILayer;

            //m_ILayer = m_IEnumLayer.Next();
            //while (m_ILayer != null)
            //{
            //    if (m_ILayer.Name.Contains("土地利用") && m_ILayer.Visible == true)
            //    {
            //        m_ArrayListILayer=clsMapFunction.MapFunction.GetLayerFromGroupLayer(m_ILayer);

            //        if (m_ILayer.Name.Contains("地类图斑") && clsMapFunction.MapFunction.IslayerVisible(m_ILayer, p_AxMapControl))
            //        {

            //        }
            //    }
            //    else if (m_ILayer.Name.Contains("土地利用") && m_ILayer.Visible == true)
            //    {
            //        if (m_ILayer.Name.Contains("地类图斑") && clsMapFunction.MapFunction.IslayerVisible(m_ILayer, p_AxMapControl))
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

                #region 遥感影像工具栏控制
                if (m_ILayersImage != null && m_ILayersImage.Count > 0)
                {
                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayersImage[p_intMapControlIndex];
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
                                    m_ButtonItemTemp.Checked = m_ILayer.Visible;//Functions.MapFunction.IslayerVisible(m_ILayer, m_AxMapControl_Selected);
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
                #region 土地利用现状工具栏控制
                if (m_ILayersTDLY != null && m_ILayersTDLY.Count > 0)
                {

                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayersTDLY[p_intMapControlIndex];
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
                                    m_ButtonItemTemp.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer, m_AxMapControl_Selected);
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
                #region 土地利用规划工具栏控制
                if (m_ILayersTDGH != null && m_ILayersTDGH.Count > 0)
                {
                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayersTDGH[p_intMapControlIndex];
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnTDLYGH.Checked = m_ILayer.Visible;//Functions.MapFunction.IslayerVisible(m_ILayer, m_AxMapControl_Selected);
                            if (btnTDLYGH.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btnTDLYGH.Checked = m_blChecked;
                }
                #endregion
                #region 土地利用供应工具栏控制
                if (m_ILayersTDGY != null && m_ILayersTDGY.Count > 0)
                {


                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayersTDGY[p_intMapControlIndex];
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnTDGY.Checked = m_ILayer.Visible;//Functions.MapFunction.IslayerVisible(m_ILayer, m_AxMapControl_Selected);
                            if (btnTDGY.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btnTDGY.Checked = m_blChecked;
                }
                #endregion
                #region 采矿权工具栏控制
                if (m_ILayersCKQ != null && m_ILayersCKQ.Count > 0)
                {


                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayersCKQ[p_intMapControlIndex];
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnCKQFZ.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer, m_AxMapControl_Selected);
                            if (btnCKQFZ.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btnCKQFZ.Checked = m_blChecked;
                }
                #endregion
                #region 探矿权工具栏控制
                if (m_ILayersTKQ != null && m_ILayersTKQ.Count > 0)
                {

                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayersTKQ[p_intMapControlIndex];
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnTKQFZ.Checked = m_ILayer.Visible;// Functions.MapFunction.IslayerVisible(m_ILayer, m_AxMapControl_Selected);
                            if (btnTKQFZ.Checked)
                            {
                                m_intTempItemsChecked++;
                            }
                        }
                    }
                    btnTKQFZ.Checked = m_blChecked;
                }
                #endregion
                #region 基本农田工具栏控制
                if (m_ILayersJBNT != null && m_ILayersJBNT.Count > 0)
                {

                    m_intTempItemsChecked = 0;
                    m_ILayerTemp = (ILayer)m_ILayersJBNT[p_intMapControlIndex];
                    m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayerTemp);
                    if (m_ArrayList != null)
                    {
                        for (int i = 0; i < m_ArrayList.Count; i++)
                        {
                            m_ILayer = (ILayer)m_ArrayList[i];

                            btnJBNTBH.Checked = m_ILayer.Visible;//Functions.MapFunction.IslayerVisible(m_ILayer, m_AxMapControl_Selected);
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
        /// 获得地图中控制工具栏对应的图层组（grouplayer），用于控制工具栏的选中与否
        /// </summary>
        /// <param name="p_IMap"></param>
        /// <param name="p_intIndex"></param>
        private void GetBtiLayers(AxMapControl p_AxMapControl)
        {
            try
            {
                if (m_ILayersImage == null) m_ILayersImage = new ArrayList();
                if (m_ILayersTDLY == null) m_ILayersTDLY = new ArrayList();
                if (m_ILayersTDGY == null) m_ILayersTDGY = new ArrayList();
                if (m_ILayersJBNT == null) m_ILayersJBNT = new ArrayList();
                if (m_ILayersTKQ == null) m_ILayersTKQ = new ArrayList();
                if (m_ILayersCKQ == null) m_ILayersCKQ = new ArrayList();
                if (m_ILayersJSYD == null) m_ILayersJSYD = new ArrayList();
                if (m_ILayersTDGH == null) m_ILayersTDGH = new ArrayList();

                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(p_AxMapControl.Map);

                ILayer pglayer = m_grouplayer.Next();
                while (pglayer != null)
                {
                    if (pglayer.Name.Contains("遥感影像"))
                    {
                        m_ILayersImage.Add(pglayer);
                    }
                    else if (pglayer.Name.Contains("土地利用现状"))
                    {
                        m_ILayersTDLY.Add(pglayer);
                    }
                    else if (pglayer.Name.Contains("土地利用规划"))
                    {
                        m_ILayersTDGH.Add(pglayer);
                    }
                    else if (pglayer.Name.Contains("建设用地"))
                    {
                        m_ILayersJSYD.Add(pglayer);
                    }
                    else if (pglayer.Name.Contains("土地供应"))
                    {
                        m_ILayersTDGY.Add(pglayer);
                    }
                    else if (pglayer.Name.Contains("采矿权"))
                    {
                        m_ILayersCKQ.Add(pglayer);
                    }
                    else if (pglayer.Name.Contains("探矿权"))
                    {
                        m_ILayersTKQ.Add(pglayer);
                    }
                    else if (pglayer.Name.Contains("基本农田"))
                    {
                        m_ILayersJBNT.Add(pglayer);
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
            //坐标
         // m_AxMapControl_Selected = (ESRI.ArcGIS.Controls.AxMapControl)sender;
            panelEx_ZFRY_Info.Visible = false;

            this.labCoordinate.Text = "" + "X:" + e.mapX.ToString(".00") + "; Y:" + e.mapY.ToString(".00") + " " + m_sMapUnits;

            if (Math.Abs(m_tempX - e.mapX) > 500 || Math.Abs(m_tempY - e.mapY) > 500)
            {
                //if (axMapControl1.MapScale < 1000000)
                //{
                //    //当比例尺较大时显示当前位置
                //    string m_strWZ = "黑河市";
                //    m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY);
                //    this.tssPosition.Text = m_strWZ;

                //    m_tempX = e.mapX;
                //    m_tempY = e.mapY;
                //}
                //else
                //{
                //    this.tssPosition.Text = "黑河市";
                //    this.panelEx1.Visible = false;
                //}
                ////当市级行政区显示时，
                //if (this.axMapControl1.MapScale < 8000000 && axMapControl1.MapScale > 1500000)
                //{
                //    if (GetCurrentXZQYName(e.mapX, e.mapY) != "")
                //        ShowPanel((int)e.x, (int)e.y, "地市");
                //}

                //当县级行政区显示时，
                if (m_AxMapControl_Selected.MapScale < 1500000 && m_AxMapControl_Selected.MapScale > 400000)
                {
                    m_strDM = GetCurrentXZQYDM(e.mapX, e.mapY, "县", "QHDM");

                    string m_strWZ = "";
                    m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY, "区县");

                    this.labeldetailDZ.Text = m_strWZ ;
                    this.tssPosition.Text = m_strWZ;  

                    if (m_strDM != "" )
                    {
                        if (Isshowpanel == true)
                        {
                            ShowPanel((int)e.x, (int)e.y, m_strDM);
                        }
                        //m_strDM1 = m_strDM;
                    }
                }

                //当乡镇行政区显示时，
                if (m_AxMapControl_Selected.MapScale < 400001 && m_AxMapControl_Selected.MapScale > 80000)
                {
                    m_strDM = GetCurrentXZQYDM(e.mapX, e.mapY, "乡镇", "DWDM");

                    string m_strWZ = "";
                    m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY, "乡镇");

                    this.labeldetailDZ.Text = m_strWZ;    
                    
                    if (m_strDM != "" )
                    {
                        if (Isshowpanel == true)
                        {
                            ShowPanel((int)e.x, (int)e.y, m_strDM);
                        }
                        //m_strDM1 = m_strDM;
                    }
                }

                //当村行政区显示时，
                if (m_AxMapControl_Selected.MapScale < 80001)
                {
                    m_strDM = GetCurrentXZQYDM(e.mapX, e.mapY, "村", "DWDM");

                    string m_strWZ = "";
                    m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY,"村");

                    this.labeldetailDZ.Text = m_strWZ;                    

                    //if (m_strDM != "" && m_strDM != m_strDM1)
                    if (m_strDM != "")
                    {
                        if (Isshowpanel == true)
                        {
                            ShowPanel((int)e.x, (int)e.y, m_strDM);
                        }
                     
                    }
                }

            }        
           
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
                            BJRY = BJRY + m_DataTableBJZFRY.Rows[i][0].ToString();
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
                            SJRY = SJRY + m_DataTableSJZFRY.Rows[i][0].ToString();
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
        ///由行政区名 获取本级执法人员姓名
        /// </summary>
        /// <param name="XZQM"></param>
        /// <returns></returns>
        private DataTable  getBJRYfromXZQM(string XZQM)
        {
            //string BJRY = "";
           
             //m_strBJZFRY = null;
             //m_strBJZFRY = new string[1, 3];

            string sql = "SELECT ZFRY.zfryxm,ZFRY.BGDH,ZFRY.SJ FROM ZFWG INNER JOIN XZQH ON ZFWG.xzqdm = XZQH.QHDM INNER JOIN ZFRY ON ZFWG.BJZFRYbh = ZFRY.zfrybh WHERE (XZQH.XZQM = '" + XZQM + "')";
            m_DataTableBJZFRY= m_DataAccess_SYS.getDataTableByQueryString(sql);

            //if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
            //{
            //    BJRY = m_DataRowCollection[0][0].ToString();
            //}

            return m_DataTableBJZFRY;
        }

        /// <summary>
        ///由行政区代码 获取本级执法人员姓名
        /// </summary>
        /// <param name="XZQM"></param>
        /// <returns></returns>
        private DataTable getBJRYfromQHDM(string QHDM)
        {
             //m_strBJZFRY = null;
            

            //string BJRY = "";
            string sql = "SELECT ZFRY.zfryxm,ZFRY.BGDH,ZFRY.SJ FROM ZFWG INNER JOIN ZFRY ON ZFWG.BJZFRYbh = ZFRY.zfrybh  WHERE (ZFWG.xzqdm = '" + QHDM + "')";
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
        ///由行政区代码 获取上级执法人员姓名
        /// </summary>
        /// <param name="XZQM"></param>
        /// <returns></returns>
        private DataTable   getSJRYfromQHDM(string QHDM)
        {
            //m_strSJZFRY = null;
            
            //string SJRY = "";
            string sql = "SELECT ZFRY.zfryxm,ZFRY.BGDH,ZFRY.SJ FROM ZFWG INNER JOIN ZFRY ON ZFWG.SJZFRYbh = ZFRY.zfrybh WHERE (ZFWG.xzqdm = '" + QHDM + "')";
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
        /// 获取当前的行政区的区划代码
        /// </summary>
        /// <param name="p_dbX">x坐标</param>
        /// <param name="p_dbY">y坐标</param>
        /// <param name="layername"></param>
        /// <returns></returns>
        private string GetCurrentXZQYDM(double p_dbX, double p_dbY, string layername, string strField)
        {
            string m_strDM = "";


            IEnumLayer m_Layers = MapFunction.GetLayers( this.m_AxMapControl_Selected.ActiveView.FocusMap);
            ILayer m_Layer = m_Layers.Next();

            while (m_Layer != null)
            {
                if (m_Layer.Name == layername)
                {
                    IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                    m_strDM = m_Feature.get_Value(m_Feature.Fields.FindField(strField)).ToString();
                    //MessageBox.Show(m_strDM.ToString());
                    //m_pFrmMain.textEvents.Text = m_pFrmMain.textEvents.Text + "  " + m_strDM.ToString();

                    break;
                }
                m_Layer = m_Layers.Next();
            }
            m_Layers.Reset();

            return m_strDM.ToString();

        }






        /// <summary>
        /// 获得当前坐标所在行政区域
        /// </summary>
        /// <param name="p_dbX"></param>
        /// <param name="p_dbY"></param>
        /// <returns></returns>
        private string GetCurrentXZQYName(double p_dbX, double p_dbY,string str)
        {
            string m_strName = "";
            this.m_AxMapControl_Selected.MousePointer = esriControlsMousePointer.esriPointerArrow;

            IEnumLayer m_Layers = MapFunction.GetLayers(this.m_AxMapControl_Selected.ActiveView.FocusMap);
            ILayer m_Layer = m_Layers.Next();

            while (m_Layer != null)
            {
                ///work图层编排不要变动

                if (str == "地市")
                {
                    if (m_Layer.Name == "市")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        break;
                    }
                }
                if (str == "区县")
                {
                    if (m_Layer.Name == "市")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }
                    if (m_Layer.Name == "县")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        break;
                    }
                }
               
                if (str == "乡镇")
                {
                    if (m_Layer.Name == "市")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }
                    if (m_Layer.Name == "县")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }

                    if (m_Layer.Name == "乡镇")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzmc")).ToString();
                        //break;
                    }
                }

                if (str == "村")
                {
                    if (m_Layer.Name == "市")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }
                    if (m_Layer.Name == "县")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }

                    if (m_Layer.Name == "乡镇")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzmc")).ToString();
                        //break;
                    }
                    if (m_Layer.Name == "村")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.m_AxMapControl_Selected.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("cm")).ToString();
                        break;
                    }
                }
              

                m_Layer = m_Layers.Next();
            }
            m_Layers.Reset();

            return m_strName;
        }

        private void axMapControl1_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            ////显示比例
            //m_AxMapControl_Selected = (ESRI.ArcGIS.Controls.AxMapControl)sender;
            //double m_dScale = this.m_AxMapControl_Selected.MapScale;
            //int m_intScale = (int)m_dScale;
            //string m_strScale = m_intScale.ToString();

            //this.labScale.Text = m_strScale;
            ////this.txtScale.Text = m_strScale;

            //if (btiActionLinkage.Checked && m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            //{
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
            //}

            //if (btiActionLinkage.Checked && m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            //{
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
            //}

            //if (btiActionLinkage.Checked && m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            //{
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
            //}

            //if (btiActionLinkage.Checked && m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            //{
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
            //    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
            //}
            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                double m_dScale = this.m_AxMapControl_Selected.MapScale;
                int m_intScale = (int)m_dScale;
                string m_strScale = m_intScale.ToString();

                this.labScale.Text = m_strScale;
                //this.txtScale.Text = m_strScale;          

                if (btiActionLinkage.Checked)
                {
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
                }
            }

           
        }

        private void SetMapExtend(ESRI.ArcGIS.Geometry.IEnvelope p_Envelope, ESRI.ArcGIS.Controls.AxMapControl p_AxMapControl)
        {
            p_AxMapControl.ActiveView.Extent  = p_Envelope;
            p_AxMapControl.ActiveView.Refresh();

        }

        private void frmMapView_Activated(object sender, EventArgs e)
        {
            //frmMapView_Activate(true);
        }

        private void frmMapView_Deactivate(object sender, EventArgs e)
        {
            //frmMapView_Activate(false );
        }

        #region 地图切换
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

            if (btnTDLYXZ.Checked == true)
            {
                //this.TLControl.SelectedTab = this.TDLYti;
                ArrayList arr;
                IGeometry geo = m_AxMapControl_Selected.Extent as IGeometry;
                IFeatureLayer featurelayer = (IFeatureLayer)MapFunction.getFeatureLayerByName("县", m_AxMapControl_Selected);
                arr = MapFunction.Overlay(geo, featurelayer, "XZQM");
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);
                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (pFeaturelay.Name.Contains(arr[i].ToString()))
                        {
                            if (pFeaturelay.Name.Contains("乡镇线界") || pFeaturelay.Name.Contains("行政区域") || pFeaturelay.Name.Contains("地类图斑") || pFeaturelay.Name.Contains("线状地物") || pFeaturelay.Name.Contains("零星地类"))
                            {
                                pFeaturelay.Visible = true;

                            }
                            break;
                        }
                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
            else
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                IFeatureLayer pFeaturelay = new FeatureLayer() as IFeatureLayer;
                while (pLayer != null)
                {
                    pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name.Contains("地类图斑") || pFeaturelay.Name.Contains("线状地物") || pFeaturelay.Name.Contains("零星地类"))
                    {
                        pFeaturelay.Visible = false;

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

            }
            m_AxMapControl_Selected.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                m_blBtiTDLYXZChecked[0] = btnTDLYXZ.Checked;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                m_blBtiTDLYXZChecked[1] = btnTDLYXZ.Checked;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                m_blBtiTDLYXZChecked[2] = btnTDLYXZ.Checked;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                m_blBtiTDLYXZChecked[3] = btnTDLYXZ.Checked;
            }
        }

        private void SetFeatureLayerVisible()
        {
            IEnumLayer m_Layers;
            m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);

            ILayer pLayer = m_Layers.Next();
            while (pLayer != null)
            {
                IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                if (pFeaturelay.Name.Contains("乡镇线界") || pFeaturelay.Name.Contains("行政区域"))
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
            m_grouplayer = Functions.MapFunction.GetGroupLayers(m_AxMapControl_Selected.ActiveView.FocusMap);
            ILayer pglayer = m_grouplayer.Next();
            while (pglayer != null)
            {
                IGroupLayer pGroupLayer = (IGroupLayer)pglayer;
                if (pGroupLayer.Name == "行政区")  //基础地理数据始终可见
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

            if (m_ILayersTDGH == null || m_ILayersTDGH.Count < m_intMapControl_SelectedIndex + 1) return;

            ILayer m_ILayer;
            m_ILayer = (ILayer)m_ILayersTDGH[m_intMapControl_SelectedIndex];
            if (m_ILayer == null) return;
            m_ILayer.Visible = this.btnTDLYGH.Checked;
            //if (this.btnTDLYGH.Checked == true)
            //{
            //    m_ILayer = m_ILayersTDGH[m_intMapControl_SelectedIndex];
            //    //this.TLControl.SelectedTab = this.TDLYGHti;
            //    //ArrayList m_ArrayListLayers;
            //    //m_ArrayListLayers = Functions.MapFunction.GetLayerFromGroupLayer(m_ILayersTDGH[m_intMapControl_SelectedIndex]);

            //    //for(int i=0;i<m_ArrayListLayers.Count ;i++)
            //    //{
            //    //    IFeatureLayer pFeaturelay = (IFeatureLayer)m_ArrayListLayers[i];
            //    //    //if (pFeaturelay.Name == "线状建设项目" || pFeaturelay.Name == "面状建设项目" || pFeaturelay.Name.Contains("规划图"))
            //    //    if (pFeaturelay.Name.Contains("规划图"))
            //    //    {
            //    //        pFeaturelay.Visible = true;
            //    //    }

            //    //    pLayer = m_Layers.Next();
            //    //}
            //    //m_Layers.Reset();
            //}
            //else
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("规划图"))
            //        {
            //            pFeaturelay.Visible = false;
            //        }

            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();

            //}
            m_AxMapControl_Selected.ActiveView.Refresh();

            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                m_blBtiTDLYGHChecked[0] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                m_blBtiTDLYGHChecked[1] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                m_blBtiTDLYGHChecked[2] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                m_blBtiTDLYGHChecked[3] = true;
            }
        }

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

            if (m_ILayersJBNT == null || m_ILayersJBNT.Count < m_intMapControl_SelectedIndex + 1) return;

            ILayer m_ILayer;
            m_ILayer = (ILayer)m_ILayersJBNT[m_intMapControl_SelectedIndex];
            if (m_ILayer == null) return;
            m_ILayer.Visible = this.btnJBNTBH.Checked;

            //if (this.btnJBNTBH.Checked == true)
            //{
            //    //this.TLControl.SelectedTab = this.JBNT;
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("基本农田"))
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
            //    m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("基本农田"))
            //        {
            //            pFeaturelay.Visible = false;

            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();

            //}
            m_AxMapControl_Selected.ActiveView.Refresh();
            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                m_blBtiJBNTChecked[0] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                m_blBtiJBNTChecked[1] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                m_blBtiJBNTChecked[2] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                m_blBtiJBNTChecked[3] = true;
            }
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

            if (m_ILayersJSYD == null || m_ILayersJSYD.Count < m_intMapControl_SelectedIndex + 1) return;

            ILayer m_ILayer;
            m_ILayer = (ILayer)m_ILayersJSYD[m_intMapControl_SelectedIndex];
            if (m_ILayer == null) return;
            m_ILayer.Visible = this.btnJSXM.Checked;

            //if (this.btnJSXM.Checked == true)
            //{
            //    //this.TLControl.SelectedTab = this.JSXMti;

            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);
            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("建设用地"))
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
            //    m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);
            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("建设用地"))
            //        {
            //            pFeaturelay.Visible = false;
            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}
            m_AxMapControl_Selected.ActiveView.Refresh();

            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                m_blBtiJSYDSPChecked[0] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                m_blBtiJSYDSPChecked[1] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                m_blBtiJSYDSPChecked[2] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                m_blBtiJSYDSPChecked[3] = true;
            }
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

            //if (this.btnKCZYGH.Checked == true)
            //{
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);
            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("矿产资源"))
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
            //    m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);
            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name.Contains("矿产资源"))
            //        {
            //            pFeaturelay.Visible = false;
            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();
            //}
            m_AxMapControl_Selected.ActiveView.Refresh();

            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                m_blBtiKCZYGHChecked[0] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                m_blBtiKCZYGHChecked[1] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                m_blBtiKCZYGHChecked[2] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                m_blBtiKCZYGHChecked[3] = true;
            }
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

            if (m_ILayersCKQ == null || m_ILayersCKQ.Count < m_intMapControl_SelectedIndex + 1) return;

            ILayer m_ILayer;
            m_ILayer = (ILayer)m_ILayersCKQ[m_intMapControl_SelectedIndex];
            if (m_ILayer == null) return;
            m_ILayer.Visible = this.btnCKQFZ.Checked;
            //if (this.btnCKQFZ.Checked == true)
            //{
            //    //this.TLControl.SelectedTab = this.CKQti;
            //    IEnumLayer m_Layers;
            //    m_Layers = Functions.MapFunction.GetLayerFromGroupLayer((m_AxMapControl_Selected.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name == "矿产核查" || pFeaturelay.Name == "全省采矿权省发证")
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
            //    m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);

            //    ILayer pLayer = m_Layers.Next();
            //    while (pLayer != null)
            //    {
            //        IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
            //        if (pFeaturelay.Name == "矿产核查" || pFeaturelay.Name == "全省采矿权省发证")
            //        {
            //            pFeaturelay.Visible = false;
            //        }
            //        pLayer = m_Layers.Next();
            //    }
            //    m_Layers.Reset();

            //}
            m_AxMapControl_Selected.ActiveView.Refresh();

            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                m_blBtiCKQChecked[0] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                m_blBtiCKQChecked[1] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                m_blBtiCKQChecked[2] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                m_blBtiCKQChecked[3] = true;
            }
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

            if (m_ILayersTKQ == null || m_ILayersTKQ.Count < m_intMapControl_SelectedIndex + 1) return;

            ILayer m_ILayer;
            m_ILayer = (ILayer)m_ILayersTKQ[m_intMapControl_SelectedIndex];
            if (m_ILayer == null) return;
            m_ILayer.Visible = this.btnTKQFZ.Checked;
        //    if (btnTKQFZ.Checked == true)
        //    {
        //        IEnumLayer m_Layers;
        //        m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.Name == "矿产核查" || pFeaturelay.Name == "全省探矿权")
        //            {
        //                pFeaturelay.Visible = true;

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();
        //    }
        //    else
        //    {
        //        IEnumLayer m_Layers;
        //        m_Layers = Functions.MapFunction.GetLayers(m_AxMapControl_Selected.ActiveView.FocusMap);

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.Name == "矿产核查" || pFeaturelay.Name == "全省探矿权")
        //            {
        //                pFeaturelay.Visible = false;

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //    }
            m_AxMapControl_Selected.ActiveView.Refresh();

            if (m_AxMapControl_Selected.Name == m_axMapControl1.Name)
            {
                m_blBtiTKQChecked[0] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                m_blBtiTKQChecked[1] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                m_blBtiTKQChecked[2] = true;
            }
            else if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                m_blBtiTKQChecked[3] = true;
            }
        }

        private void btnTDGY_Click(object sender, EventArgs e)
        {
            if (this.btnTDGY.Checked == false)
            {
                this.btnTDGY.Checked = true;
            }
            else
            {
                btnTDGY.Checked = false;
            }

            if (m_ILayersTDGY == null || m_ILayersTDGY.Count < m_intMapControl_SelectedIndex + 1) return;

            ILayer m_ILayer;
            m_ILayer = (ILayer)m_ILayersTDGY[m_intMapControl_SelectedIndex];
            if (m_ILayer == null) return;
            m_ILayer.Visible = this.btnTDGY.Checked;
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ButtonItem m_ButtonItem=(DevComponents.DotNetBar.ButtonItem)sender;
            m_ButtonItem.Checked=!m_ButtonItem.Checked ;
            SetImageVisible(m_ButtonItem.Text, m_ButtonItem.Checked);
        }

        /// <summary>
        /// 设置菜单是否应该被选中
        /// </summary>
        /// <param name="p_ButtonItem"></param>
        private void SetButtonItemCheckStation(DevComponents.DotNetBar.ButtonItem p_ButtonItem)
        {
            bool m_blTemp = false;
            DevComponents.DotNetBar.ButtonItem m_ButtonItemTemp, m_ButtonItemParent;
            m_ButtonItemParent = (DevComponents.DotNetBar.ButtonItem)p_ButtonItem.Parent;
            for (int i = 0; i < m_ButtonItemParent.SubItems.Count; i++)
            {
                //判断是否所有子菜单都已经
                m_ButtonItemTemp = (DevComponents.DotNetBar.ButtonItem)m_ButtonItemParent.SubItems[i];
                if (m_ButtonItemTemp.Checked)
                {
                    m_blTemp = true;
                    break;
                }
            }
            m_ButtonItemParent.Checked = m_blTemp;
        }

        public void setTDLYXZBtni()
        {
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

                m_ILayer = (ILayer)m_ILayersTDLY[m_intMapControl_SelectedIndex];
                if (m_ILayer == null) return;
                
                //while (pglayer != null)
                //{
                m_GroupLayerTDLY = (IGroupLayer)m_ILayer;

                m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(m_GroupLayerTDLY);
                //}
                //m_grouplayer.Reset();

                m_TDLYXZShows = new DevComponents.DotNetBar.ButtonItem[m_ArrayList.Count];
                ILayer m_layer;
                int m_intTempItemsChecked = 0;
                DevComponents.DotNetBar.ButtonItem m_ButtonItem;
                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    m_TDLYXZShows[i] = new DevComponents.DotNetBar.ButtonItem();
                    m_layer = (ILayer)m_ArrayList[i];
                    m_TDLYXZShows[i].Text = m_layer.Name;
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
                MessageBox.Show("错误：" + errs.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void TDLYXZButton_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ButtonItem m_ButtonItem = (DevComponents.DotNetBar.ButtonItem)sender;
            m_ButtonItem.Checked = !m_ButtonItem.Checked;
            //SetImageVisible(m_ButtonItem.Text, m_ButtonItem.Checked);

            try
            {
                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(this.m_AxMapControl_Selected.ActiveView.FocusMap);

                ILayer pglayer = m_grouplayer.Next();
                while (pglayer != null)
                {
                    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                    if (pGroupLayer.Name == "土地利用现状")
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
                this.m_AxMapControl_Selected.ActiveView.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("错误：" + errs.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void setImageBtni()
        {
            try
            {
                ILayer m_ILayer, m_ILayerImageGroup;
                ArrayList m_ArrayList;
                m_ImageShow = new DevComponents.DotNetBar.ButtonItem();
                DevComponents.DotNetBar.ButtonItem[] m_ImageShows;

                bar2.Items.Insert(bar2.Items.Count, m_ImageShow);

                m_ImageShow.Text = "遥感影像";

                //ArrayList m_ArrayList = new ArrayList();
                //IEnumLayer m_grouplayer;
                //m_grouplayer = Functions.MapFunction.GetGroupLayers(this.m_AxMapControl_Selected.Map);

                //ILayer pglayer = m_grouplayer.Next();
                //while (pglayer != null)
                //{
                //    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                //    if (pGroupLayer.Name == "遥感影像")
                //    {
                //        m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
                //    }
                //    pglayer = m_grouplayer.Next();
                //}
                //m_grouplayer.Reset();

                m_ILayerImageGroup = (ILayer)m_ILayersImage[m_intMapControl_SelectedIndex];
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
                MessageBox.Show("错误：" + errs.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void SetImageVisible(string p_strRasterCatalogName, bool m_bool)
        {
            try
            {
                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(this.m_AxMapControl_Selected.ActiveView.FocusMap);

                ILayer pglayer = m_grouplayer.Next();
                while (pglayer != null)
                {
                    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                    if (pGroupLayer.Name == "遥感影像")
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

                this.m_AxMapControl_Selected.ActiveView.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("错误：" + errs.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }



        #endregion

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            //m_frmShowPic = new frmShowPic();
            //m_frmShowPic.m_Oid = this.m_OID;
            //m_frmShowPic.m_DataAccess_SYS = this.m_DataAccess_SYS;

            //ArrayList theList = new ArrayList();
            //string m_strID = GetStrId(m_OID.ToString(), "p");
            //GetFile(m_strID, ref theList);
            //m_frmShowPic.m_theFileList.Clear();
            //m_frmShowPic.m_theFileList.AddRange(theList); // 添加
            //m_frmShowPic.DoInitial();
            //m_frmShowPic.ShowIconImage();


            //ArrayList theList1 = new ArrayList();

            //string m_strID1 = GetStrId(this.m_OID.ToString(), "v");

            //GetFile(m_strID1, ref theList1);

            //m_frmShowPic.m_theFileList1.Clear();
            //m_frmShowPic.m_theFileList1.AddRange(theList1); // 添加
            //m_frmShowPic.DoInitialVideos(); // 初始化工作（清空等）

            //m_frmShowPic.Show();

        }

        private string GetStrId(string id, string style)
        {
            string m_strID = "";
            //yuejianwei,需要修改20110412,确定m_FeatureLayerName是从哪里来
            if (this.m_FeatureLayerName == "土地核查" && style == "p")
                m_strID = "t" + id + "p";
            else
                if (this.m_FeatureLayerName == "土地核查" && style == "v")
                    m_strID = "t" + id + "v";

            if (this.m_FeatureLayerName == "矿产核查" && style == "p")
                m_strID = "k" + id + "p";
            else
                if (this.m_FeatureLayerName == "矿产核查" && style == "v")
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

                        //创建存放文件的路径
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
                System.Windows.Forms.MessageBox.Show(this, errs.Message, "错误提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

       

        private void frmGPSMointors_ResizeEnd(object sender, EventArgs e)
        {
            //ResizeMapSzie();
            ResizeMap();
        }

        public  void  ResizeMap()
        {
             if (m_intMapCount == 4)
            {
                panel1.Dock = DockStyle.Top ;
                panel1.Height = this.Height / 2 - 15;

                m_axMapControl1.Width = this.Width / 2 - 5;
                m_axMapControl2.Width = m_axMapControl1.Width;                

                m_axMapControl3.Width = m_axMapControl1.Width;
                m_axMapControl4.Width = m_axMapControl1.Width;
            }
            else
            {
                panel2.Visible = false;
                panel1.Visible = true;
                panel1.Dock = DockStyle.Fill;

                m_axMapControl1.Width = this.Width / 2 - 5;
                m_axMapControl2.Width = m_axMapControl1.Width;
            }
        }

        private void ReLocationBti()
        {
            if (m_intMapCount == 4)
            {              
                btnMapName1.Left = m_axMapControl1.Left;
                btnMapName1.Top = m_axMapControl1.Height / 2;

                btnMapName2.Left = m_axMapControl2.Right - btnMapName2.Width;
                btnMapName2.Top = btnMapName1.Top;
              

                btnMapName3.Left = m_axMapControl3.Left;
                btnMapName3.Top = m_axMapControl3.Top + m_axMapControl3.Height / 2;

                btnMapName4.Left = m_axMapControl4.Right - btnMapName4.Width;
                btnMapName4.Top = btnMapName3.Top;
            }
            else
            {
                btnMapName1.Left = m_axMapControl1.Left;
                btnMapName1.Top = m_axMapControl1.Height / 2;

                btnMapName2.Left = m_axMapControl2.Right - btnMapName2.Width;
                btnMapName2.Top = btnMapName1.Top;
            }


        }
       

        private void axMapControl_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {

            /*
            m_AxMapControl_Selected = (ESRI.ArcGIS.Controls.AxMapControl)sender;


           
           
            if (panel1.Dock == DockStyle.Fill)
            {
                panel1.Visible = true;
                panel1.Dock = DockStyle.Top;   

                m_axMapControl1.Visible = true;                               
                m_axMapControl2.Visible = true;           
               


                if (m_intMapCount == 4)
                {             
                    panel2.Visible = true;
                    m_axMapControl3.Visible = true;
                    m_axMapControl4.Visible = true;
                }
                else
                {
                    m_axMapControl1.Width = this.Width ; m_axMapControl1.Dock = DockStyle.Left;
                }
            }
            else
            {
                m_axMapControl1.Visible = true;
                m_axMapControl2.Visible = false;
                if (m_intMapCount == 4)
                {
                    m_axMapControl3.Visible = false;
                    m_axMapControl4.Visible = false;
                    panel2.Visible = false;
                }

            
          

            m_axMapControl1.Dock = DockStyle.Fill;

            //DevComponents.DotNetBar.PanelEx m_PanelEx = (DevComponents.DotNetBar.PanelEx)m_AxMapControl_Selected.Container;
            panel1.Visible = true;
            panel1.Dock = DockStyle.Fill;    
            }

           
            */
           
        }

        private void axMapControl1_SizeChanged(object sender, EventArgs e)
        {
            //ResizeMapSzie();
            ReLocationBti();
        }

        private void btiActionLinkage_Click(object sender, EventArgs e)
        {
            btiActionLinkage.Checked = !btiActionLinkage.Checked;
        }

        //private void m_axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        //{            
            
        //}

        //private void m_axMapControl2_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        //{
        //    if (btiActionLinkage.Checked && m_AxMapControl_Selected.Name == m_axMapControl2.Name)
        //    {
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl1);
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
        //    }
        //}

        //private void m_axMapControl3_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        //{
        //    if (btiActionLinkage.Checked && m_AxMapControl_Selected.Name == m_axMapControl3.Name)
        //    {
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl1);
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
        //    }
        //}

        //private void m_axMapControl4_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        //{
        //    if (btiActionLinkage.Checked && m_AxMapControl_Selected.Name == m_axMapControl4.Name)
        //    {
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
        //        SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl1);
        //    }
        //}

        private void axMapControl2_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            if (m_AxMapControl_Selected.Name == m_axMapControl2.Name)
            {
                double m_dScale = this.m_AxMapControl_Selected.MapScale;
                int m_intScale = (int)m_dScale;
                string m_strScale = m_intScale.ToString();

                this.labScale.Text = m_strScale;
                //this.txtScale.Text = m_strScale;          

                if (btiActionLinkage.Checked)
                {
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl1);
                }
            }

        }

        private void axMapControl3_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            if (m_AxMapControl_Selected.Name == m_axMapControl3.Name)
            {
                double m_dScale = this.m_AxMapControl_Selected.MapScale;
                int m_intScale = (int)m_dScale;
                string m_strScale = m_intScale.ToString();

                this.labScale.Text = m_strScale;
                //this.txtScale.Text = m_strScale;          

                if (btiActionLinkage.Checked)
                {
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl4);
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl1);
                }
            }

        }

        private void axMapControl4_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            if (m_AxMapControl_Selected == null)
            {
                return;
            }
            //显示比例
            //m_AxMapControl_Selected = (ESRI.ArcGIS.Controls.AxMapControl)sender;
            if (m_AxMapControl_Selected.Name == m_axMapControl4.Name)
            {
                double m_dScale = this.m_AxMapControl_Selected.MapScale;
                int m_intScale = (int)m_dScale;
                string m_strScale = m_intScale.ToString();

                this.labScale.Text = m_strScale;
                //this.txtScale.Text = m_strScale;          

                if (btiActionLinkage.Checked)
                {
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl2);
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl3);
                    SetMapExtend(m_AxMapControl_Selected.ActiveView.Extent.Envelope, m_axMapControl1);
                }
            }

        }

        private void axMapContro2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            axMapControl1_OnMouseDown(sender, e);
        }

        private void axMapControl3_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            axMapControl1_OnMouseDown(sender, e);
        }

        private void axMapControl4_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            axMapControl1_OnMouseDown(sender, e);
        }



    }
}