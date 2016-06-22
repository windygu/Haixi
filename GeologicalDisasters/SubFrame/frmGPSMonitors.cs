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
    public partial class frmMapView : Form
    {
        private string m_FeatureLayerName = "";
        private frmShowPic m_frmShowPic;
        public int m_OID = -1;

        private DataTable m_DataTableBJZFRY;
        private DataTable m_DataTableSJZFRY;


        #region 父窗体

        public frmMain m_pFrmMain = null;

        public MapFunction m_MapFunction = new MapFunction();


        #endregion

        public delegate void frmMapView_ActivateEventHandler(bool p_IsActivated);
        public event frmMapView_ActivateEventHandler frmMapView_Activate;

        public clsDataAccess.DataAccess m_DataAccess;


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


        public IFeature m_hcselfeature;

        public frmMapView(frmMain parentForm)
        {
            InitializeComponent();
            this.m_pFrmMain = parentForm;
            this.m_pFrmMain.m_bIsMapViewFormOpen = true;
            this.m_pFrmMain.m_MapFuction.axMapControl = this.axMapControl1;

            m_MapFunction.axMapControl = this.axMapControl1;
           
        }

        private void frmMapView_Load(object sender, EventArgs e)
        {
            if (this.m_pFrmMain.m_bIsFirstStart)
            {
                this.LoadFile(Application.StartupPath + @"\work.mxd");

                setImageBtni();
            }
            this.WindowState = FormWindowState.Maximized;
        }

        public void LoadFile(string filepath)
        {
            if (this.axMapControl1.CheckMxFile(filepath))
            {
                this.axMapControl1.LoadMxFile(filepath);
                this.axMapControl2.LoadMxFile(filepath);
                this.axMapControl3.LoadMxFile(filepath);
                this.axMapControl4.LoadMxFile(filepath);
                this.axMapControl5.LoadMxFile(filepath);
                this.axMapControl6.LoadMxFile(filepath);
                this.axMapControl7.LoadMxFile(filepath);
                this.axMapControl8.LoadMxFile(filepath);
                this.axMapControl9.LoadMxFile(filepath);
                //this.axMapControl1.LoadMxFile(filepath);
                this.InitMap();                
                OpenDocument(filepath);
            }
        }

        /// <summary>
        /// 函数——初始化地图相关参数
        /// </summary>
        private void InitMap()
        {
            this.m_ActiveView = this.axMapControl1.ActiveView;
            this.m_MapUnits = this.axMapControl1.MapUnits;
            this.m_sMapUnits = MapFunction.getMapUnits(this.m_MapUnits);
        }

        /// <summary>
        /// 打开mapdocument文件
        /// </summary>
        /// <param name="sFilePath"></param>
        private void OpenDocument(string sFilePath)
        {
            //Create a new map document
            m_MapDocument = new MapDocumentClass();
            //Open the map document selected
            m_MapDocument.Open(sFilePath, "");
        }

        #region 浏览工具条
        private void bubbleButton1_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.Pan();
            Isshowpanel = false ;
            panelEx1.Visible = false ;
        }

        private void bubbleButton2_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.ZoomIn();
            Isshowpanel = false;
            panelEx1.Visible = false;
        }

        private void bubbleButton3_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.ZoomOut();
            Isshowpanel = false;
            panelEx1.Visible = false;
        }

        private void bubbleButton4_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.FullExtent();
            Isshowpanel = false;
            panelEx1.Visible = false;

        }

        private void bubbleButton5_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.BringToFront();
            Isshowpanel = false;
            panelEx1.Visible = false;
        }

        private void bubbleButton6_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.BringForward();
            Isshowpanel = false;
            panelEx1.Visible = false;
        }

        private void bubbleButton7_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            m_bSelTB = true;
            this.axMapControl1.CurrentTool = null;
            this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
            //this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
            Isshowpanel = false;
            panelEx1.Visible = false;
        }

        private void bubbleButton9_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            this.m_MapFunction.Pan();
            Isshowpanel = true ;
            panelEx1.Visible = true;
        }

        #endregion


        private JCZF.FormMenuButton theForm;

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            panelEx_ZFRY_Info.Visible = false ;
            if (this.theForm != null && this.theForm.Disposing == false)
            {
                theForm.Close();
            }

            IActiveView pActiveView = this.axMapControl1.ActiveView;
           #region 
            if (e.button == 2)
            {
                 this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;

                 ArrayList arr = new ArrayList();
                 IEnumLayer pLayers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
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

                    IEnvelope pEnvelope = new EnvelopeClass();
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

                            pFeaIdObj = (IFeatureIdentifyObj)pIdentifyArray.get_Element(0);
                            pRowIdObj = (IRowIdentifyObject)pFeaIdObj;

                            IRow pRow = pRowIdObj.Row;

                            IFeature pFeature = (IFeature)pRow;
                            this.m_hcselfeature = pFeature;
                            pIdObj = (IIdentifyObj)pFeaIdObj;
                            pIdObj.Flash(this.axMapControl1.ActiveView.ScreenDisplay);
                            int j = pRow.OID;                           

                            theForm = new FormMenuButton();
                            theForm.m_fileContent = "good"; // 灾害类型                            

                            theForm.Location = new System.Drawing.Point(e.x, e.y);
                            System.Drawing.Point MouseClickPoint = new System.Drawing.Point(e.x, e.y);
                            //Convert from Tree coordinates to Screen   
                            System.Drawing.Point ScreenPoint = this.axMapControl1.PointToScreen(MouseClickPoint);
                            theForm.Left = ScreenPoint.X;
                            theForm.Top = ScreenPoint.Y;

                            theForm.oid = j;
                            theForm.sellayname = selLayer.Name;
                            theForm.m_strTabelName = selLayer.Name;
                            theForm.m_strObjecgID = j.ToString();
                            theForm.m_frmMapView=this;
                            theForm.m_selFeature=pFeature;
                            theForm.m_selFeatureclass=selLayer.FeatureClass;
                            theForm.m_DataAccess=this.m_DataAccess;
                            m_FeatureLayerName = selLayer.Name;

                            JCZF.Renderable.CGlobalVarable.m_theSlideForm = theForm;

                            theForm.Show(); // 显示

                    }

                }

            }
    #endregion

               
        } 

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            //坐标

            panelEx_ZFRY_Info.Visible = false;

            this.labCoordinate.Text = "" + "X:" + e.mapX.ToString(".00") + "; Y:" + e.mapY.ToString(".00") + " " + m_sMapUnits;

            if (Math.Abs(m_tempX - e.mapX) > 500 || Math.Abs(m_tempY - e.mapY) > 500)
            {
                //if (axMapControl1.MapScale < 1000000)
                //{
                //    //当比例尺较大时显示当前位置
                //    string m_strWZ = "辽宁省";
                //    m_strWZ = GetCurrentXZQYName(e.mapX, e.mapY);
                //    this.tssPosition.Text = m_strWZ;

                //    m_tempX = e.mapX;
                //    m_tempY = e.mapY;
                //}
                //else
                //{
                //    this.tssPosition.Text = "辽宁省";
                //    this.panelEx1.Visible = false;
                //}
                ////当市级行政区显示时，
                //if (this.axMapControl1.MapScale < 8000000 && axMapControl1.MapScale > 1500000)
                //{
                //    if (GetCurrentXZQYName(e.mapX, e.mapY) != "")
                //        ShowPanel((int)e.x, (int)e.y, "地市");
                //}

                //当县级行政区显示时，
                if (axMapControl1.MapScale < 1500000 && axMapControl1.MapScale > 400000)
                {
                    m_strDM = GetCurrentXZQYDM(e.mapX, e.mapY, "区县行政区", "QHDM");

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
                if (axMapControl1.MapScale < 400001 && axMapControl1.MapScale > 80000)
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
                if (axMapControl1.MapScale < 80001)
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
            m_DataTableBJZFRY= m_DataAccess.getDataTableByQueryString(sql);

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
            m_DataTableBJZFRY = m_DataAccess.getDataTableByQueryString(sql);

            

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
            m_DataTableSJZFRY = m_DataAccess.getDataTableByQueryString(sql);


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

            IEnumLayer m_Layers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
            ILayer m_Layer = m_Layers.Next();

            while (m_Layer != null)
            {
                if (m_Layer.Name == layername)
                {
                    IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
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
            this.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;

            IEnumLayer m_Layers = MapFunction.GetLayers(this.axMapControl1.ActiveView.FocusMap);
            ILayer m_Layer = m_Layers.Next();

            while (m_Layer != null)
            {
                ///work图层编排不要变动

                if (str == "地市")
                {
                    if (m_Layer.Name == "地市行政区")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        break;
                    }
                }
                if (str == "区县")
                {
                    if (m_Layer.Name == "地市行政区")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }
                    if (m_Layer.Name == "区县行政区")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        break;
                    }
                }
               
                if (str == "乡镇")
                {
                    if (m_Layer.Name == "地市行政区")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }
                    if (m_Layer.Name == "区县行政区")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }

                    if (m_Layer.Name == "乡镇")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzmc")).ToString();
                        //break;
                    }
                }

                if (str == "村")
                {
                    if (m_Layer.Name == "地市行政区")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }
                    if (m_Layer.Name == "区县行政区")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzqm")).ToString();
                        //break;
                    }

                    if (m_Layer.Name == "乡镇")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
                        m_strName += m_Feature.get_Value(m_Feature.Fields.FindField("xzmc")).ToString();
                        //break;
                    }
                    if (m_Layer.Name == "村")
                    {
                        IFeature m_Feature = MapFunction.GetFeatureIncludePoint(m_Layer, p_dbX, p_dbY, this.axMapControl1.SpatialReference);
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
            //显示比例
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
                IGeometry geo = axMapControl1.Extent as IGeometry;
                IFeatureLayer featurelayer = (IFeatureLayer)MapFunction.getFeatureLayerByName("区县行政区", axMapControl1);
                arr = MapFunction.Overlay(geo, featurelayer, "XZQM");
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
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
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                IFeatureLayer pFeaturelay = new FeatureLayerClass();
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
            m_grouplayer = Functions.MapFunction.GetGroupLayers(axMapControl1.ActiveView.FocusMap);
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

            if (this.btnTDLYGH.Checked == true)
            {
                //this.TLControl.SelectedTab = this.TDLYGHti;
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    //if (pFeaturelay.Name == "线状建设项目" || pFeaturelay.Name == "面状建设项目" || pFeaturelay.Name.Contains("规划图"))
                    if (pFeaturelay.Name.Contains("规划图"))
                    {
                        pFeaturelay.Visible = true;
                    }

                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

            }
            else
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name.Contains("规划图"))
                    {
                        pFeaturelay.Visible = false;
                    }

                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

            }
            axMapControl1.ActiveView.Refresh();

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

            if (this.btnJBNTBH.Checked == true)
            {
                //this.TLControl.SelectedTab = this.JBNT;
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name.Contains("基本农田"))
                    {
                        pFeaturelay.Visible = true;

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
            else
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name.Contains("基本农田"))
                    {
                        pFeaturelay.Visible = false;

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

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

            if (this.btnJSXM.Checked == true)
            {
                //this.TLControl.SelectedTab = this.JSXMti;

                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name.Contains("建设用地"))
                    {
                        pFeaturelay.Visible = true;
                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
            else
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name.Contains("建设用地"))
                    {
                        pFeaturelay.Visible = false;
                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
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

            if (this.btnKCZYGH.Checked == true)
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name.Contains("矿产资源"))
                    {
                        pFeaturelay.Visible = true;
                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
            else
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);
                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name.Contains("矿产资源"))
                    {
                        pFeaturelay.Visible = false;
                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
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
            if (this.btnCKQFZ.Checked == true)
            {
                //this.TLControl.SelectedTab = this.CKQti;
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name == "矿产核查" || pFeaturelay.Name == "全省采矿权省发证")
                    {
                        pFeaturelay.Visible = true;

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
            else
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name == "矿产核查" || pFeaturelay.Name == "全省采矿权省发证")
                    {
                        pFeaturelay.Visible = false;

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

            }
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
            if (btnTKQFZ.Checked == true)
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name == "矿产核查" || pFeaturelay.Name == "全省探矿权")
                    {
                        pFeaturelay.Visible = true;

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
            }
            else
            {
                IEnumLayer m_Layers;
                m_Layers = Functions.MapFunction.GetLayers(axMapControl1.ActiveView.FocusMap);

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.Name == "矿产核查" || pFeaturelay.Name == "全省探矿权")
                    {
                        pFeaturelay.Visible = false;

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

            }
            axMapControl1.ActiveView.Refresh();
        }

        private void 土地供应_Click(object sender, EventArgs e)
        {

        }

        private void ImageButton_Click(object sender, EventArgs e)
        {

        }

        public void setImageBtni()
        {
            try
            {
                DevComponents.DotNetBar.ButtonItem m_ImageShow = new DevComponents.DotNetBar.ButtonItem();
                DevComponents.DotNetBar.ButtonItem[] m_ImageShows;

                bar2.Items.Insert(bar2.Items.Count, m_ImageShow);

                m_ImageShow.Text = "遥感影像";

                ArrayList m_ArrayList = new ArrayList();
                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(this.axMapControl1.Map);

                ILayer pglayer = m_grouplayer.Next();
                while (pglayer != null)
                {
                    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                    if (pGroupLayer.Name == "遥感影像")
                    {
                        m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
                    }
                    pglayer = m_grouplayer.Next();
                }
                m_grouplayer.Reset();

                m_ImageShows = new DevComponents.DotNetBar.ButtonItem[m_ArrayList.Count];
                for (int i = 0; i < m_ArrayList.Count; i++)
                {
                    m_ImageShows[i] = new DevComponents.DotNetBar.ButtonItem();
                    m_ImageShows[i].Text = ((ILayer)m_ArrayList[i]).Name;
                    m_ImageShow.SubItems.Add(m_ImageShows[i] as DevComponents.DotNetBar.BaseItem);
                    m_ImageShows[i].Click += new System.EventHandler(ImageButton_Click);
                }
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
                m_grouplayer = Functions.MapFunction.GetGroupLayers(this.axMapControl1.ActiveView.FocusMap);

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

                this.axMapControl1.ActiveView.Refresh();
            }
            catch (SystemException errs)
            {
                MessageBox.Show("错误：" + errs.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        #endregion

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            m_frmShowPic = new frmShowPic();
            m_frmShowPic.m_Oid = this.m_OID;
            m_frmShowPic.m_DataAccess = this.m_DataAccess;

            ArrayList theList = new ArrayList();
            string m_strID = GetStrId(m_OID.ToString(), "p");
            GetFile(m_strID, ref theList);
            m_frmShowPic.m_theFileList.Clear();
            m_frmShowPic.m_theFileList.AddRange(theList); // 添加
            m_frmShowPic.DoInitial();
            m_frmShowPic.ShowIconImage();


            ArrayList theList1 = new ArrayList();

            string m_strID1 = GetStrId(this.m_OID.ToString(), "v");

            GetFile(m_strID1, ref theList1);

            m_frmShowPic.m_theFileList1.Clear();
            m_frmShowPic.m_theFileList1.AddRange(theList1); // 添加
            m_frmShowPic.DoInitialVideos(); // 初始化工作（清空等）

            m_frmShowPic.Show();

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

            DataRowCollection m_DataRowCollection = this.m_DataAccess.getDataRowsByQueryString(m_strSQL);
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

            DataRowCollection m_DataRowCollection = this.m_DataAccess.getDataRowsByQueryString(m_strSQL);

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


    }
}