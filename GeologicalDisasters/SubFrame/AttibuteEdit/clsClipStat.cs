using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using clsDataAccess;
using Functions;
using System.Data;
using System.IO;
using System.Windows.Forms;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.AnalysisTools;

namespace JCZF.SubFrame
{
    class clsClipStat
    {
        private int _m_OID;

        public ArrayList arrdlmc = new ArrayList();
        public ArrayList dbldltb = new ArrayList();

        public ArrayList arrghmc = new ArrayList();
        public ArrayList dblghtb = new ArrayList();

        private ArrayList arrgdmc = new ArrayList();
        public ArrayList dblgdtb = new ArrayList();

        private ArrayList arrkcmc = new ArrayList();
        private ArrayList dblkctb = new ArrayList();
        private IFeatureClass m_ClipFeatureClass;

        private clsDataAccess.DataAccess _pDataAcess;
        private string _m_strFilePath = "";//Application.StartupPath + "\\OverlayTemp"

        private AxMapControl _m_axMapcontrol;
        private string _strHCJG = "";
        private IGeometry _m_Geometry;
        private IFeatureClass _pDltbOutputFeatClass;
        private IFeatureClass _pJbntOutputFeatClass;
        private IFeatureClass _pJsydOutputFeatClass;
        private IFeatureClass _pTkqOutputFeatClass;
        private IFeatureClass _pCkqOutputFeatClass;
        private IFeatureClass _pGHTOutputFeatClass;
        private IFeatureClass _pTDGYOutputFeatClass;
        private IFeatureClass _pKczyOutputFeatClass;

        public static string m_strLayerNameOfStatics = ""; // 要分析的图层名称  // 刘扬 修改 2011 分析

        private IEnumLayer m_Grouplayers; // 刘扬修改 201111

        public IFeatureClass pDltbOutputFeatClass
        {
            get { return _pDltbOutputFeatClass; }
        }
        public IFeatureClass pJbntOutputFeatClass
        {
            get { return _pJbntOutputFeatClass; }
        }
        public IFeatureClass pJsydOutputFeatClass
        {
            get { return _pJsydOutputFeatClass; }
        }
        public IFeatureClass pTkqOutputFeatClass
        {
            get { return _pTkqOutputFeatClass; }
        }
        public IFeatureClass pCkqOutputFeatClass
        {
            get { return _pCkqOutputFeatClass; }
        }
        public IFeatureClass pGHTOutputFeatClass
        {
            get { return _pGHTOutputFeatClass; }
        }
        public IFeatureClass pTDGYOutputFeatClass
        {
            get { return _pTDGYOutputFeatClass; }
        }
        public IFeatureClass pKczyOutputFeatClass
        {
            get { return _pKczyOutputFeatClass; }
        }
        public IGeometry m_Geometry
        {
            set
            {
                _m_Geometry = value;
            }
        }
        //IFeature _pFeature;

        ILayer _m_selLayer;
        string _zqm;

        public int m_OID
        {
            set
            {
                _m_OID = value;
            }
        }

        public clsDataAccess.DataAccess pDataAcess
        {
            set
            {
                _pDataAcess = value;
            }
        }
        public string m_strFilePath
        {
            set
            {
                _m_strFilePath = value;
            }
        }
        public AxMapControl m_axMapcontrol
        {
            set
            {
                _m_axMapcontrol = value;
            }

        }
        public string strHCJG
        {
            get
            {
                return _strHCJG;
            }

        }

        public ILayer m_selLayer
        {
            set
            {
                _m_selLayer = value;
            }
        }

        #region
        /// <summary>
        /// 基本农田面积
        /// </summary>
        public double db_jbnt;
        /// <summary>
        /// 建设用地面积
        /// </summary>
        public double db_jsyd;
        /// <summary>
        /// 规划数据面积
        /// </summary>
        public double db_ghsj;
        /// <summary>
        /// 规划数据面积
        /// </summary>
        public double db_tdgy;
        /// <summary>
        /// 矿产规划面积
        /// </summary>
        public double db_kczy;
        /// <summary>
        /// 采矿权面积
        /// </summary>
        public double db_ckq;

        /// <summary>
        /// 省发证和市发证总面积
        /// </summary>
        public double db_ckq_zmj;

        /// <summary>
        /// 探矿权面积
        /// </summary>
        public double db_tkq;

        #endregion










        #region 变化图斑块核查
        public void execute()
        {
            //判断数据库中是否已经核查过，即字段CLIPRESULT是否为空
            //更新数据库
            string sql = "select clipResult from 土地核查 where objectid='" + _m_OID + "'";
            DataRowCollection m_DataRowCollection = _pDataAcess.getDataRowsByQueryString(sql);
            if (m_DataRowCollection[0][0].ToString().Trim() != "")
            {
                _strHCJG = m_DataRowCollection[0][0].ToString();

            }
            else
            {
                IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;
                IFeature _pFeature = m_FeatureClass.GetFeature(_m_OID);

                int index = _pFeature.Fields.FindField("xzqm");
                _zqm = _pFeature.get_Value(index).ToString();
                IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

                IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains("_地类图斑") && pLayer.Name.Contains(_zqm))
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析

                if (m_inputFeatureLayer != null)
                {

                    m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析

                    string clipResult = executeClip(_pFeature, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                    //更新数据库
                    string m_strUpdateRow = "update " + "土地核查" + " set clipResult='" + clipResult + "'";

                    m_strUpdateRow = m_strUpdateRow + " where Objectid = " + _m_OID;

                    this._pDataAcess.ExecuteSQLNoReturn(m_strUpdateRow);
                }
            }
        }

        // 向一个图层添加一个字段（图层名称）
        private void InsertTheLayerNameField(string theCatalogOfShpFiles, string theShpName, string theLayerName)
        {
            // 获取要添加字段的图层
            IFeatureLayer pFeatureLayer = GetOneLayerByFilePath(theCatalogOfShpFiles, theShpName) as IFeatureLayer;
            // 字段名称
            string fieldName = JCZF.Renderable.CGlobalVarable.m_strFieldNameOfLayerInTable;// 所属图层名称，由系统加入的，保存统计的图层名
            int length = 50;
            // 图层名称
            string theDefaultValue = "";
            bool isSuccess = MapFunction.AddLayerAttributeString(fieldName, fieldName, length, (string)theDefaultValue, true, pFeatureLayer);

            if (isSuccess) // 如果成功则保存图层名称
            {
                // 保存图层名称
                MapFunction.SaveOneStringToOneField(pFeatureLayer.FeatureClass, fieldName, theLayerName);
            }
        }

        //所选图斑与地类图斑进行裁切运算
        private string executeClip(IFeature m_Feature, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa

            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }
                //overlayer
                IFeatureClass m_overlayFeatureClass = getFeatureClass(m_Feature, m_selLayer);
                //outputlayer 

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //IFeatureClass pDltbOutputFeatClass1 = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;

                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";

                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);

                //IFeatureClass pDltbOutputFeatClass1
                IFeatureClass pDltbOutputFeatClass1 = pFWS.OpenFeatureClass("clip");

                string text = "  统计结果：" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    MeasureDltb(pDltbOutputFeatClass1);

                    double sumdltb = 0;
                    for (int i = 0; i < arrdlmc.Count; i++)
                    {
                        text += "  " + arrdlmc[i].ToString() + "面积:  " + dbldltb[i].ToString() + " 平方米" + "\r\n";
                        sumdltb += Convert.ToDouble(dbldltb[i]);
                    }
                    text += "  合计：" + sumdltb + " 平方米" + "\r\n";
                }
                return text;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        #endregion

        #region  刘扬 修改 2011后期 土地利用分析


        // 找到至少包含其中一个关键字就是图层名
        private bool FindItemOfContainOverOneKey(string theContent, ArrayList theList)
        {
            for (int i = 0; i < theList.Count; i++)
            {
                string theKey = theList[i].ToString().Trim(); // 获取一个关键字
                if (theContent.Contains(theKey) == true)
                    return true;

            }

            return false;

        }


        // 获取统计的图层（根据图层名称的部分和行政区）
        private IArray GetStaticsLayersByDistrictList(string layerNamePart, ArrayList districtNameList)
        {


            /*
              ILayer pLayer;
            IArray pArray;
            pArray = new ESRI.ArcGIS.esriSystem.Array() as IArray ;
             
            // 获取图层索引
            IEnumLayer theLayers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);         
            pLayer = theLayers.Next();
            
            // 找到某个行政区对应的地类图斑 
            while (pLayer != null)
            {
                IFeatureLayer pFeatureLayer = (IFeatureLayer)pLayer; // 转化为矢量图层
                // 加入图层(图层是矢量图层、图层中数据不为空且为为面状图层)
                if (pFeatureLayer != null && pFeatureLayer.FeatureClass != null && pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    // 图层名称组成判断(包括某部分名称以及行政区名称)
                    if (pFeatureLayer.Name.Contains(layerNamePart))
                    {
                        if (FindItemOfContainOverOneKey(pFeatureLayer.Name, districtNameList))
                            pArray.Add(pFeatureLayer);
                    }
                }
                pLayer = theLayers.Next();
            }

            return pArray;
            */

            ILayer m_Layer;
            IArray m_Array;
            m_Array = new ESRI.ArcGIS.esriSystem.Array() as IArray ;

            ArrayList m_TDLYXZLayers = new ArrayList();
            m_TDLYXZLayers = GetTDLYXZLayers();//获得土地利用组下所有矢量图层
            if (m_TDLYXZLayers == null) return m_Array;
            for (int i = 0; i < m_TDLYXZLayers.Count; i++)
            {
                try
                {
                    m_Layer = (ILayer)m_TDLYXZLayers[i];
                    if (m_Layer != null && m_Layer is IFeatureLayer)
                    {
                        
                        IFeatureLayer m_FeatureLayer = (IFeatureLayer)m_Layer; // 转化为矢量图层
                        // 加入图层(图层是矢量图层、图层中数据不为空且为为面状图层)
                        if (m_FeatureLayer != null && m_FeatureLayer.FeatureClass != null && m_FeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            // 图层名称组成判断(包括某部分名称以及行政区名称)
                            if (m_FeatureLayer.Name.Contains(layerNamePart))
                            {
                                if (FindItemOfContainOverOneKey(m_FeatureLayer.Name, districtNameList))
                                    m_Array.Add(m_FeatureLayer);
                            }
                        }
                    }
                }
                catch(SystemException errs)
                {
                    clsFunction.Function.MessageBoxError(errs.Message);
                    continue;
                }
            }

            return m_Array;
        }

        /// <summary>
        /// 获得土地利用组下所有矢量图层
        /// </summary>
        /// <returns></returns>
        private ArrayList GetTDLYXZLayers()
        {
           ArrayList m_TDLYXZLayers = new ArrayList(); 
            try
            {
            
            ILayer m_Layer;

             ArrayList m_ArrayList = new ArrayList();
             ArrayList m_ArrayListLayers = new ArrayList();

                IEnumLayer m_grouplayer;
                m_grouplayer = Functions.MapFunction.GetGroupLayers(_m_axMapcontrol.Map);

                ILayer pglayer = m_grouplayer.Next();

                m_TDLYXZLayers = new ArrayList();
                while (pglayer != null)
                {
                    IGroupLayer pGroupLayer = (IGroupLayer)pglayer;

                    if (pGroupLayer.Name == "土地利用现状")
                    {
                        //m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(pGroupLayer);
                        //if (m_ArrayList != null && m_ArrayList.Count > 0)
                        //{
                           
                        //    m_Layer = (ILayer)m_ArrayList[GetLastDataIndex(m_ArrayList)];

                        //    //GetILayerArrayList(m_Layer, m_TDLYXZLayers);


                        //    m_TDLYXZLayers = Functions.MapFunction.GetLayerArrayList(m_Layer);
                        //}

                        Functions.MapFunction.GetAllLayersFromGroupLayer(m_TDLYXZLayers,pGroupLayer);
                        break;
                    }
                    pglayer = m_grouplayer.Next();
                }
                m_grouplayer.Reset();

                
            }
            catch (SystemException errs)
            {
                MessageBox.Show("错误：" + errs.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return m_TDLYXZLayers;
       }
        /// <summary>
        /// 获得最新的组序号
        /// </summary>
        /// <returns></returns>
        private int  GetLastDataIndex(ArrayList p_ArrayList)
        {
            int m_intIndex=0;
            ILayer m_Layer;
            int m_strName,m_strName1;
            try
            {
                m_Layer = (ILayer)p_ArrayList[0];
                m_strName =System.Convert.ToInt32( m_Layer.Name);
                m_intIndex = 0;

                for (int i = 1; i < p_ArrayList.Count; i++)
                {
                    m_Layer = (ILayer)p_ArrayList[i];
                    if (m_strName <System.Convert.ToInt32(m_Layer.Name))
                    {
                        m_strName = System.Convert.ToInt32(m_Layer.Name);
                        m_intIndex = i;
                    }
                }
            }
            catch (SystemException errs)
            {
                MessageBox.Show("错误：" + errs.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return m_intIndex;
        }
        //private ArrayList GetILayerArrayList(ILayer p_ILayer,ArrayList p_ArrayList)
        //{
        //    ArrayList m_ArrayList ;
        //    ILayer m_ILayer;
        //    if (Functions.MapFunction.IsCompositeLayer(p_ILayer))
        //    {
        //        m_ArrayList = Functions.MapFunction.GetLayerFromGroupLayer(p_ILayer);

        //        for (int i = 0; i < m_ArrayList.Count; i++)
        //        {
        //            m_ILayer = (ILayer)m_ArrayList[i];
        //            if (Functions.MapFunction.IsCompositeLayer(m_ILayer))
        //            {
        //                GetILayerArrayList(m_ILayer, p_ArrayList);
        //            }
        //            else
        //            {                        
        //                 p_ArrayList.Add(m_ArrayList[i]);                        
        //            }
        //        }                
        //    }
        //    else
        //    {                
        //            p_ArrayList.Add(p_ILayer);               
        //    }
        //    return p_ArrayList;
        //}


        private IArray GetStaticsLayers(string layerNamePart, string districtName)
        {
            // 获取图层索引
            IEnumLayer theLayers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

            ILayer pLayer;
            IArray pArray;
            pArray = new ESRI.ArcGIS.esriSystem.Array() as IArray ;

            pLayer = theLayers.Next();
            // 找到某个行政区对应的地类图斑 
            while (pLayer != null)
            {
                IFeatureLayer pFeatureLayer = (IFeatureLayer)pLayer; // 转化为矢量图层
                // 加入图层(图层是矢量图层、图层中数据不为空且为为面状图层)
                if (pFeatureLayer != null && pFeatureLayer.FeatureClass != null && pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    // 图层名称组成判断(包括某部分名称以及行政区名称)
                    if (pFeatureLayer.Name.Contains(layerNamePart) && pFeatureLayer.Name.Contains(districtName))
                    {
                        pArray.Add(pFeatureLayer);
                    }
                }
                pLayer = theLayers.Next();
            }

            return pArray;

        }

        #endregion


        #region 选择区域核查




        #region  刘扬 修改 2011后期 所有类型的统计分析

        // 执行统计分析
        public void BasicExecuteStatistics(IArray theStatisticsLayers)
        {
            _strHCJG = "";
            // 没有统计图层，则返回
            if (theStatisticsLayers==null || theStatisticsLayers.Count < 1)
                return;

            IFeatureLayer theInputFeatureLayer = theStatisticsLayers.get_Element(0) as IFeatureLayer;

            m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析


            if (theInputFeatureLayer != null)
            {
                m_strLayerNameOfStatics = theInputFeatureLayer.Name; // 刘扬 修改 2011 分析

                string clipResult = GeoexecuteClip2(theStatisticsLayers);

                _strHCJG = clipResult;

            }
        }


        // 土地利用分析
        public void Geoexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {
                // 获取图斑所在行政区名称
                IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("县", this._m_axMapcontrol);

                ArrayList theFeatureList = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
                ArrayList theDistrictNameList = new ArrayList();
                for (int i = 0; i < theFeatureList.Count; i++)
                {
                    IFeature m_xzqFeature = theFeatureList[i] as IFeature;
                    int index = m_xzqFeature.Fields.FindField("xzqdm");
                    _zqm = m_xzqFeature.get_Value(index).ToString();
                    theDistrictNameList.Add(_zqm);
                }



                #region  刘扬 修改 2011后期 土地利用分析

                // 获取统计的图层（根据图层名称的部分和行政区列表）
                IArray theStatisticsLayers = GetStaticsLayersByDistrictList("地类图斑", theDistrictNameList);
                if (theStatisticsLayers == null || theStatisticsLayers.Count<1)
                {
                    theStatisticsLayers = GetStaticsLayersByDistrictList("DLTB_", theDistrictNameList);
                }
                //IArray theStatisticsLayers = GetStaticsLayers("_地类图斑", _zqm);

                // 执行统计分析
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion

            }
        }



        // 基本农田分析
        public void JBNTexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  刘扬 修改 2011后期 基本农田分析

                // 获取统计的图层（根据图层名称的部分和行政区）
                IArray theStatisticsLayers = GetStaticsLayers("基本农田", "");

                // 执行统计分析
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }


        // 规划数据分析
        public void QHTexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  刘扬 修改 2011后期 规划数据分析

                // 获取统计的图层（根据图层名称的部分和行政区）
                IArray theStatisticsLayers = GetStaticsLayers("规划", "");

                // 执行统计分析
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }

        // 土地供应分析
        public void TDGYexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  刘扬 修改 2011后期 土地供应分析

                // 获取统计的图层（根据图层名称的部分和行政区）
                IArray theStatisticsLayers = GetStaticsLayers("土地供应", "");

                // 执行统计分析
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }


        // 建设用地数据分析
        public void JSYDexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  刘扬 修改 2011后期 建设用地数据分析


                // 获取统计的图层（根据图层名称的部分和行政区）
                IArray theStatisticsLayers = GetStaticsLayers("建设用地", "");

                // 执行统计分析
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }

        // 矿产资源规划分析
        public void KCZYexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  刘扬 修改 2011后期 矿产资源规划分析

                // 获取统计的图层（根据图层名称的部分和行政区）
                IArray theStatisticsLayers = GetStaticsLayers("矿产资源", "");

                // 执行统计分析
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }




        // 采矿权分析
        public void CKQexecute()
        {

            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  刘扬 修改 2011后期 采矿权分析

                // 获取统计的图层（根据图层名称的部分和行政区）
                IArray theStatisticsLayers = GetStaticsLayers("采矿权", "");

                // 执行统计分析
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }


        // 探矿权分析
        public void TKQexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  刘扬 修改 2011后期 探矿权分析

                // 获取统计的图层（根据图层名称的部分和行政区）
                IArray theStatisticsLayers = GetStaticsLayers("探矿权", "");

                // 执行统计分析
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }



        #endregion

        //public void JBNTexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {               
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;        

        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("基本农田"))  //基本农田层
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析

        //            string clipResult = JBNTGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
        //            _strHCJG = clipResult;

        //            //IGeometry[] m_IGeometry=new IGeometry[1];
        //            //m_IGeometry[0]=_m_Geometry;
        //            //Functions.SpatialAnalystClass.Clip(m_inputFeatureLayer.FeatureClass, m_IGeometry, _m_axMapcontrol);
        //        }
        //    }



        //}

        //public void QHTexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;


        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("规划图"))  //基本农田层
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析

        //            string clipResult = QHTGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
        //            _strHCJG = clipResult;
        //        }
        //    }


        //}


        //public void TDGYexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        ////获取zqm
        //        //IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("县", this._m_axMapcontrol);

        //        //IFeature m_xzqFeature = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);

        //        //int index = m_xzqFeature.Fields.FindField("xzqm");
        //        //_zqm = m_xzqFeature.get_Value(index).ToString();

        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("土地供应"))  //基本农田层
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析

        //            string clipResult = TDGYGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
        //            _strHCJG = clipResult;
        //        }
        //    }


        //}


        //public void JSYDexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        if (_m_selLayer == null)
        //        {
        //            _m_selLayer = MapFunction.getFeatureLayerByName("土地核查", _m_axMapcontrol);
        //        }
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //获取zqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("市", this._m_axMapcontrol);

        //        IFeature m_xzqFeature = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
        //        if (m_xzqFeature == null) return;
        //        int index = m_xzqFeature.Fields.FindField("xzqm");
        //        _zqm = m_xzqFeature.get_Value(index).ToString();

        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;

        //            // 刘扬 修改 2011后期 分析
        //            if (pFeaturelay.FeatureClass != null && pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("建设用地") && pLayer.Name.Contains(_zqm))  //建设用地
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析

        //            string clipResult = JSYDGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);

        //            _strHCJG = clipResult;
        //        }
        //    }


        //}


        //public void KCZYexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //获取zqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("县", this._m_axMapcontrol);

        //        IFeature m_xzqFeature = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
        //        int index = m_xzqFeature.Fields.FindField("xzqm");
        //        _zqm = m_xzqFeature.get_Value(index).ToString();



        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("矿产资源"))
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析 

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析

        //            string clipResult = KCZYGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
        //            _strHCJG = clipResult;
        //        }
        //    }


        //}



        //public void CKQexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //获取zqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("县", this._m_axMapcontrol);

        //        IFeature m_xzqFeature = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
        //        int index = m_xzqFeature.Fields.FindField("xzqm");
        //        _zqm = m_xzqFeature.get_Value(index).ToString();

        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;
        //        IFeatureLayer m_inputFeatureLayer2 = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();

        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("采矿权"))
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }
        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();
        //        //while (pLayer != null)
        //        //{
        //        //    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //        //    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //        //    {
        //        //        if (pLayer.Name.Contains("市发证"))
        //        //        {
        //        //            m_inputFeatureLayer2 = pLayer as IFeatureLayer;
        //        //            break;
        //        //        }

        //        //    }
        //        //    pLayer = m_Layers.Next();
        //        //}
        //        //m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析 

        //        string clipResult = "";
        //        string clipResult2 = "";
        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析 

        //            clipResult = CKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);

        //        }

        //        if (m_inputFeatureLayer2 != null) // 刘扬 修改 2011 分析
        //        {

        //            m_strLayerNameOfStatics += String.Format(";{0}", m_inputFeatureLayer2.Name); // 刘扬 修改 2011 分析 

        //            clipResult2 = CKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer2);

        //        }
        //        _strHCJG = "";
        //        if (clipResult.IndexOf("公顷") >= 0)
        //        {
        //            _strHCJG = String.Format("\r\n省发证-{0}", clipResult); // 刘扬 修改 2011 分析
        //        }
        //        if (clipResult2.IndexOf("公顷") >= 0)
        //        {
        //            _strHCJG += String.Format("\r\n市发证-{0}", clipResult2); // 刘扬 修改 2011 分析
        //        }
        //    }


        //}
        //public void CKQexecute1()
        //{
        //    if (this._m_Geometry != null)
        //    {

        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //获取zqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("县", this._m_axMapcontrol);

        //        IFeature m_xzqFeature = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
        //        int index = m_xzqFeature.Fields.FindField("xzqm");
        //        _zqm = m_xzqFeature.get_Value(index).ToString();

        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;
        //        IFeatureLayer m_inputFeatureLayer2 = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();


        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("基本农田"))
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();
        //        //while (pLayer != null)
        //        //{
        //        //    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //        //    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //        //    {
        //        //        if (pLayer.Name.Contains("市发证"))
        //        //        {
        //        //            m_inputFeatureLayer2 = pLayer as IFeatureLayer;
        //        //            break;
        //        //        }

        //        //    }
        //        //    pLayer = m_Layers.Next();
        //        //}
        //        //m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析 

        //        string clipResult = "";
        //        string clipResult2 = "";
        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析 

        //            clipResult = CKQGeoexecuteClip1(_m_Geometry, _m_selLayer, m_inputFeatureLayer);

        //        }

        //        //if (m_inputFeatureLayer2 != null) // 刘扬 修改 2011 分析
        //        //{

        //        //    m_strLayerNameOfStatics += String.Format(";{0}", m_inputFeatureLayer2.Name); // 刘扬 修改 2011 分析 

        //        //    clipResult2 = CKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer2);

        //        //}
        //        _strHCJG = "";
        //        if (clipResult.IndexOf("公顷") >= 0)
        //        {
        //            _strHCJG = String.Format("\r\n省发证基本农田-{0}", clipResult); // 刘扬 修改 2011 分析
        //        }
        //        //if (clipResult2.IndexOf("公顷") >= 0)
        //        //{
        //        //    _strHCJG += String.Format("\r\n市发证-{0}", clipResult2); // 刘扬 修改 2011 分析
        //        //}
        //    }


        //}



        //public void TKQexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //获取zqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("县", this._m_axMapcontrol);

        //        IFeature m_xzqFeature = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
        //        int index = m_xzqFeature.Fields.FindField("xzqm");
        //        _zqm = m_xzqFeature.get_Value(index).ToString();

        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("探矿权"))  //探矿权
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();



        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析


        //            string clipResult = TKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
        //            _strHCJG = clipResult;
        //        }
        //    }


        //}



        //// 探矿权分析
        //public void TKQexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //获取zqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("县", this._m_axMapcontrol);

        //        IFeature m_xzqFeature = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
        //        int index = m_xzqFeature.Fields.FindField("xzqm");
        //        _zqm = m_xzqFeature.get_Value(index).ToString();

        //        IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

        //        IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

        //        ILayer pLayer = m_Layers.Next();
        //        while (pLayer != null)
        //        {
        //            IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
        //            if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("探矿权"))  //探矿权
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();



        //        m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析


        //            string clipResult = TKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
        //            _strHCJG = clipResult;
        //        }
        //    }


        //}


        private string ReturnMessages(Geoprocessor gp)
        {
            StringBuilder sb = new StringBuilder();
            if (gp.MessageCount > 0)
            {
                for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                {
                    System.Diagnostics.Trace.WriteLine(gp.GetMessage(Count));
                    sb.AppendFormat("{0}\n", gp.GetMessage(Count));
                }
            }
            return sb.ToString();
        }




        public void JSYDexecute1()
        {
            double m_dbJSYD = 0;
            if (this._m_Geometry != null)
            {
                if (_m_selLayer == null)
                {
                    _m_selLayer = MapFunction.getFeatureLayerByName("土地核查", _m_axMapcontrol);
                }
                IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

                //获取zqm
                IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("市", this._m_axMapcontrol);

                IFeature m_xzqFeature = MapFunction.GetFeatureIncludeGeometry2(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
                if (m_xzqFeature == null) return;
                int index = m_xzqFeature.Fields.FindField("xzqm");
                _zqm = m_xzqFeature.get_Value(index).ToString();

                IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

                IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains("建设用地") && pLayer.Name.Contains(_zqm))  //建设用地
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;

                            m_strLayerNameOfStatics = ""; // 刘扬 修改 2011 分析

                            if (m_inputFeatureLayer != null)
                            {
                                m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // 刘扬 修改 2011 分析

                                JSYDGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                                m_dbJSYD = m_dbJSYD + db_jsyd;
                                //_strHCJG = clipResult;

                                AddtoTOC(pJsydOutputFeatClass, "建设用地数据clip");
                                //loadJSYDProp(pJsydOutputFeatClass); 刘扬 修改201111
                            }
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
                _strHCJG = "审批建设用地统计结果：" + "\r\n\r\n";
                _strHCJG += "  审批建设用地面积：  " + m_dbJSYD.ToString("#0") + "平方米" + "\r\n\r\n";

            }
        }



        #region 添加到toc  刘丽20110814
        private void AddtoTOC(IFeatureClass outputfeatclass, string layername)
        {
            IFeatureLayer outlayer = new FeatureLayer() as IFeatureLayer;

            outlayer.SpatialReference = _m_axMapcontrol.SpatialReference;

            outlayer.FeatureClass = outputfeatclass;
            outlayer.Name = layername;

            IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
            IGroupLayer hcjg1 = new GroupLayer() as IGroupLayer;

            if (hcjg != null)
            {
                hcjg.Add((ILayer)outlayer);
                _m_axMapcontrol.Map.AddLayer(outlayer);
                _m_axMapcontrol.Map.DeleteLayer(outlayer);

            }
            else
            {
                //IGroupLayer hcjg = new GroupLayer() as IGroupLayer;
                hcjg1.SpatialReference = _m_axMapcontrol.SpatialReference;
                hcjg1.Name = "核查结果";
                hcjg1.Add((ILayer)outlayer);
                _m_axMapcontrol.Map.AddLayer((ILayer)hcjg1);
            }
        }

        private IGroupLayer GetHcjggrouplayer(string grouplayername)
        {
            IGroupLayer ResGrouplayer = null;
            this.m_Grouplayers = Functions.MapFunction.GetGroupLayers(_m_axMapcontrol.ActiveView.FocusMap);
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
        #endregion








        /// <summary>
        /// 计算某行政区基本农田
        /// </summary>
        public void cal_JBNT()
        {
            if (this._m_Geometry != null)
            {
                IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

                IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains("基本农田"))  //基本农田层
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                if (m_inputFeatureLayer != null)
                {
                    string clipResult = JBNTGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                }



            }







        }

        /// <summary>
        /// 计算区县规划数据
        /// </summary>
        /// <returns></returns>
        public void cal_qxghsj(string str_dsmc)
        {
            if (this._m_Geometry != null)
            {
                IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

                IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains(str_dsmc + "规划图"))  //规划图
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                if (m_inputFeatureLayer != null)
                {
                    string clipResult = QHTGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                }

            }


        }

        /// <summary>
        /// 计算某行政区建设用地
        /// </summary>
        public void cal_JSYD()
        {

            if (this._m_Geometry != null)
            {
                IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

                IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains("建设用地"))  //建设用地
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                if (m_inputFeatureLayer != null)
                {
                    string clipResult = JSYDGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                }
            }


        }

        /// <summary>
        /// 计算某行政区矿产资源
        /// </summary>
        public void cal_KCZY()
        {

            if (this._m_Geometry != null)
            {
                IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

                IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains("矿产资源规划"))  //建设用地
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                if (m_inputFeatureLayer != null)
                {
                    string clipResult = KCZYGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                }
            }



        }
        /// <summary>
        /// 计算某区域采矿权面积
        /// </summary>
        public void cal_CKQ()
        {
            if (this._m_Geometry != null)
            {
                IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

                IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains("省发证"))
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                if (m_inputFeatureLayer != null)
                {
                    string clipResult = CKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                }

                db_ckq_zmj = db_ckq;

                pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains("市发证"))
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                if (m_inputFeatureLayer != null)
                {
                    string clipResult = CKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                }

                db_ckq_zmj = db_ckq_zmj + db_ckq;

            }


        }

        public void cal_TKQ()
        {
            if (this._m_Geometry != null)
            {
                IEnumLayer m_Layers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

                IFeatureLayer m_inputFeatureLayer = new FeatureLayer() as IFeatureLayer;

                ILayer pLayer = m_Layers.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeaturelay = (IFeatureLayer)pLayer;
                    if (pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        if (pLayer.Name.Contains("探矿权"))
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                if (m_inputFeatureLayer != null)
                {
                    string clipResult = TKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                }


            }


        }

        /////所画图形与指定图层进行裁切分析
        //private string GeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        //{
        //    string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
        //    string text = "";
        //    try
        //    {
        //        //裁切结果路径
        //        string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
        //        if (!System.IO.Directory.Exists(m_strResultFilePath))
        //        {
        //            System.IO.Directory.CreateDirectory(m_strResultFilePath);
        //        }
        //        //overlayer
        //        //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
        //        //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
        //        //outputlayer 

        //        IFeatureWorkspace pFWS;
        //        IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
        //        pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;

        //        //加了没用的一句
        //        IGeoProcessorResult result;
        //        Geoprocessor gp2 = new Geoprocessor();
        //        gp2.OverwriteOutput = true;

        //        IFeatureClass fc = (IFeatureClass)m_inputFeatureLayer.FeatureClass;
        //        IFeatureDataset fds = (IFeatureDataset)fc.FeatureDataset;
        //        IWorkspace ws = (IWorkspace)fds.Workspace;
        //        string s0 = ws.PathName.ToUpper(); //workpathname
        //        string ss3 = fds.Name;//datasetname
        //        string ss4 = fc.AliasName;//dataname

        //        string inputfeatureclasspath = s0 + "\\" + ss3 + "\\" + ss4;

        //        string outputfeatureclasspath = m_strResultFilePath + "\\clip";

        //        ESRI.ArcGIS.AnalysisTools.Clip clipTool2 = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath,m_ClipFeatureClass, outputfeatureclasspath);//最好的方式是三个参数 都采用路径 m_inputFeatureLayer.FeatureClass, m_ClipFeatureClass, _pDltbOutputFeatClass);


        //        IGeoProcessorResult result2 = (IGeoProcessorResult)gp2.Execute(clipTool2, null);

        //        string s = ReturnMessages(gp2);

        //        _pDltbOutputFeatClass = pFWS.OpenFeatureClass("clip");//.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


        //        //string text =  "土地利用现状："+"\r\n"+"  统计结果：" + "\r\n";
        //        if (result2.Status == esriJobStatus.esriJobSucceeded)
        //        {
        //            MeasureDltb(_pDltbOutputFeatClass);

        //            double sumdltb = 0;
        //            for (int i = 0; i < arrdlmc.Count; i++)
        //            {
        //                double temp = Convert.ToDouble(dbldltb[i]) / 10000;
        //                text += "  " + arrdlmc[i].ToString() + "面积:  " + temp.ToString("#0.000") + " 公顷" + "\r\n";
        //                sumdltb += Convert.ToDouble(dbldltb[i]) / 10000;
        //            }
        //            text += "  合计：" + sumdltb.ToString("#0.000") + " 公顷" + "\r\n";
        //        }
        //        return text;
        //    }
        //    catch (Exception ex)
        //    {
        //        return text + "没有统计结果" + "\r\n";
        //    }

        //}

        // 获取图层(根据路径)
        private ILayer GetOneLayerByFilePath(string theCatalogOfSomeShpFile, string p_theFeatureClass)
        {
            IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
            IFeatureWorkspace pFWS = pWorkspaceFactor.OpenFromFile(theCatalogOfSomeShpFile, 0) as IFeatureWorkspace;
            IFeatureClass pFC = pFWS.OpenFeatureClass(p_theFeatureClass);
            IFeatureLayer pFLayer = new FeatureLayer() as IFeatureLayer;
            pFLayer.FeatureClass = pFC;
            pFLayer.Name = pFC.AliasName;
            ILayer pLayer = pFLayer as ILayer;

            return pLayer;

        }

        // 合并多个图层数据到一个图层,
        private IFeatureClass MergeSomeLayersIntoOneLayer(string theCatalogOfSomeShpFile, ArrayList theLayerNameList, string theMergeShpName)
        {
            IFeatureClass m_IFeatureClass;
            try
            {

                //合并图层的集合
                ILayer pLayer;
                IArray pArray;
                IFeatureLayer m_IFeatureLayer;
                pArray = new ESRI.ArcGIS.esriSystem.Array() as IArray ;
                for (int i = 0; i < theLayerNameList.Count; i++)
                {
                    string theFeatureClassName = theLayerNameList[i].ToString().Trim();
                    pLayer = GetOneLayerByFilePath(theCatalogOfSomeShpFile, theFeatureClassName);
                    if (pLayer != null)
                        pArray.Add(pLayer);
                }


                //判断图层是否大于2个
                if (pArray.Count < 2)
                {
                    m_IFeatureLayer = pArray.get_Element(0) as IFeatureLayer;
                    // 打开裁切后数据
                    //IFeatureWorkspace pFWS;
                    //IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                    //pFWS = pWorkspaceFactor.OpenFromFile(theCatalogOfSomeShpFile, 0) as IFeatureWorkspace;
                    //m_IFeatureClass = pFWS.OpenFeatureClass(theMergeShpName);
                    m_IFeatureClass = m_IFeatureLayer.FeatureClass;
                    return m_IFeatureLayer.FeatureClass; // 应该修改唯一的一个图层名称
                }

                //定义输出图层的fields表
                ITable pTable;
                pLayer = pArray.get_Element(0) as ILayer;
                pTable = (ITable)pLayer;


                //输出文件类型
                IFeatureLayer pFeatureLayer;
                IFeatureClass pFeatureClass;
                pFeatureLayer = (IFeatureLayer)pLayer;
                pFeatureClass = pFeatureLayer.FeatureClass;

                IFeatureClassName pFeatureClassName;
                IDatasetName pDatasetName;
                IWorkspaceName pNewWSName;
                pFeatureClassName = new FeatureClassName() as IFeatureClassName;
                pFeatureClassName.FeatureType = esriFeatureType.esriFTSimple;
                pFeatureClassName.ShapeFieldName = "Shape";
                pFeatureClassName.ShapeType = pFeatureClass.ShapeType;

                //输出shapefile的名称和位置
                pNewWSName = new WorkspaceName() as IWorkspaceName;
                pNewWSName.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory";
                pNewWSName.PathName = theCatalogOfSomeShpFile;
                pDatasetName = (IDatasetName)pFeatureClassName;
                pDatasetName.Name = theMergeShpName;
                pDatasetName.WorkspaceName = pNewWSName;

                //合并图层
                IFeatureClass pOutputFeatClass;
                IBasicGeoprocessor pBasicGeop;
                pBasicGeop = new BasicGeoprocessor() as IBasicGeoprocessor ;
                pOutputFeatClass = pBasicGeop.Merge(pArray, pTable, pFeatureClassName);

                return pOutputFeatClass;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // 得到某个类型统计的结果
        private string GetResultOfStatics(IFeatureClass p_theFeatureClass)
        {
            string text = "";
            _strHCJG = "";
            if (p_theFeatureClass != null) // 操作成功
            {
                // 当前的统计类型
                JCZF.Renderable.CGlobalVarable.Enum_AnalysisType theType = JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType;

                if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ) // 土地利用现状
                {

                    _pDltbOutputFeatClass = p_theFeatureClass; // 获取土地利用图斑数据

                    MeasureDltb(_pDltbOutputFeatClass); // 计算图斑面积

                    double sumdltb = 0;
                    for (int i = 0; i < arrdlmc.Count; i++)
                    {
                        double temp = Convert.ToDouble(dbldltb[i]) ;
                        sumdltb += temp;

                        text += "  " + arrdlmc[i].ToString() + "面积:  " + temp.ToString("#0") + " 平方米" + "\r\n\r\n";
                      
                    }
                    //db_t
                    text += "  合计：" + sumdltb.ToString("#0") + " 平方米（" + (sumdltb * 15 / 10000).ToString("#0.0") + "　亩）\r\n\r\n";

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH) // 规划数据
                {

                    _pGHTOutputFeatClass = p_theFeatureClass; // 获取规划数据图斑数据

                    MeasureGHT(_pGHTOutputFeatClass); // 计算规划分区面积
                    if (_strHCJG != "")
                    {
                        text +=  _strHCJG+"\r\n\r\n" ;
                    }
                    else
                    {
                        double sumdltb = 0;

                        for (int i = 0; i < arrghmc.Count; i++)
                        {
                            double temp = Convert.ToDouble(dblghtb[i]) ;
                            sumdltb += temp;

                            text += "  " + arrghmc[i].ToString() + "面积:  " + temp.ToString("#0") + " 平方米" + "\r\n\r\n";
                           

                        }
                        db_ghsj = sumdltb;
                        text += "  合计：" + (sumdltb).ToString("#0") + " 平方米（" + (sumdltb * 15 / 10000).ToString("#0.0") + "　亩）\r\n\r\n";
                    }
                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY) // 土地供应
                {
                    _pTDGYOutputFeatClass = p_theFeatureClass; // 获取土地供应图斑数据

                    MeasureTDGY(_pTDGYOutputFeatClass); // 计算图斑面积

                    if (_strHCJG != "")
                    {
                        text += _strHCJG + "\r\n\r\n";
                    }
                    else
                    {
                        double sumdltb = 0;
                        for (int i = 0; i < arrghmc.Count; i++)
                        {
                            double temp = Convert.ToDouble(dblgdtb[i]) ;
                            sumdltb += temp;

                            text += "  " + arrghmc[i].ToString() + "面积:  " + temp.ToString("#0") + " 平方米" + "\r\n\r\n";
                            

                        }
                        db_tdgy = sumdltb;
                        text += "  合计：" + sumdltb.ToString("#0") + " 平方米（" + (sumdltb * 15 / 10000).ToString("#0.0") + "　亩）\r\n\r\n";
                    }
                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT) // 基本农田
                {
                    _pJbntOutputFeatClass = p_theFeatureClass; // 获取基本农田图斑数据

                    double dblJbnt = MeasurePL(_pJbntOutputFeatClass); // 计算图斑面积
                    db_jbnt = dblJbnt;
                    //dblJbnt = dblJbnt / 10000;

                    text += "  基本农田面积：  " + dblJbnt.ToString("#0") + " 平方米（" + (dblJbnt * 15 / 10000).ToString("#0.0") + "　亩）\r\n\r\n";

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD) // 建设用地数据
                {
                    _pJsydOutputFeatClass = p_theFeatureClass; // 获取建设用地数据图斑数据

                    double dblJsyd = MeasurePL(_pJsydOutputFeatClass); // 计算图斑面积
                    db_jsyd = dblJsyd;
                    //dblJsyd = dblJsyd / 10000;

                    text += "  审批建设用地面积：  " + dblJsyd.ToString("#0") + " 平方米（" + (dblJsyd * 15 / 10000).ToString("#0.0") + "　亩）\r\n\r\n"; ;

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH) // 矿产资源规划
                {
                    _pKczyOutputFeatClass = p_theFeatureClass; // 获取矿产资源规划图斑数据

                    MeasureKCZY(_pKczyOutputFeatClass); // 计算图斑面积

                    double sumdltb = 0;
                    for (int i = 0; i < arrkcmc.Count; i++)
                    {
                        double temp = Convert.ToDouble(dblkctb[i]);
                        sumdltb += temp;
                        text += "  " + arrkcmc[i].ToString() + "面积:  " + temp.ToString("#0") + " 平方米" + "\r\n";


                    }
                    db_kczy = sumdltb;
                    text += "  合计：" + sumdltb.ToString("#0") + " 平方米（" + (sumdltb * 15 / 10000).ToString("#0.0") + "　亩）\r\n";

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ) // 采矿权
                {
                    _pCkqOutputFeatClass = p_theFeatureClass; // 获取采矿权图斑数据

                    double dbCkq = MeasurePL(_pCkqOutputFeatClass);  // 计算图斑面积
                    db_ckq = dbCkq;
                    //dbCkq = dbCkq / 10000;


                    text += "  采矿权面积：  " + dbCkq.ToString("#0") + " 平方米（" + (dbCkq * 15 / 10000).ToString("#0.0") + "　亩\r\n\r\n";

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ) // 探矿权
                {
                    _pTkqOutputFeatClass = p_theFeatureClass; // 获取采矿权图斑数据

                    double dbTkq = MeasurePL(_pTkqOutputFeatClass);
                    //double dbTkq =clsMapFunction.MapFunction.GetFeature_Shape_Area( p_theFeatureClass.GetFeature(0));
                    db_tkq = dbTkq;

                    //dbTkq = dbTkq / 10000;

                    //if (dbTkq >= 0.0001)
                    //{
                    text += "  探矿权面积：  " + dbTkq.ToString("#0") + " 平方米（" + (dbTkq * 15 / 10000).ToString("#0.0") + "　亩\r\n\r\n";
                    //}
                    //else
                    //{

                    //    if (dbTkq * 15 >= 0.0001)
                    //    {
                    //        text += "  探矿权面积：  " + (dbTkq * 15).ToString("#0.0") + " 亩" + "\r\n\r\n";
                    //    }
                    //    else
                    //    {
                    //        text += "  探矿权面积：  " + (dbTkq * 10000).ToString("#0") + " 平方米" + "\r\n\r\n";
                    //    }
                    //}

                }



            }
            else // 刘扬 修改 2011后期 土地分析
            {
                text += "操作失败，面积未计算！" + "\r\n\r\n";
            }

            return text;
        }


        #region  刘扬 修改 2011后期 土地分析

        // 获取数据路径(数据在数据集中)
        private string GetPathInDataSet(IFeatureLayer theInputFeatureLayer)
        {
            string inputfeatureclasspath ="";
            try
            {
                // 构建被裁切图层路径
                //     
                IFeatureClass fc = (IFeatureClass)theInputFeatureLayer.FeatureClass;               
                IFeatureDataset fds = (IFeatureDataset)fc.FeatureDataset;
                if (fds == null) return null;
                IWorkspace ws = (IWorkspace)fds.Workspace;
                string s0 = ws.PathName.ToUpper(); //workpathname
                string ss3 = fds.Name;//datasetname
                string ss4 = fc.AliasName;//dataname
                inputfeatureclasspath = s0 + "\\" + ss3 + "\\" + ss4;
            }
            catch(SystemException errs)
            {
                throw errs;
            }

            return inputfeatureclasspath;
        }

        // 获取数据路径(数据不在数据集中)
        private string GetPathNotInDataSet(IFeatureLayer theInputFeatureLayer)
        {
            // 构建被裁切图层路径
            //     
            IDataLayer pDatalayer = theInputFeatureLayer as IDataLayer;
            IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;
            IWorkspaceName wsname = pDataSetname.WorkspaceName;
            string pFilePath = wsname.PathName;
            string pFileName = pDataSetname.Name;

            string inputfeatureclasspath = pFilePath + "\\" + pFileName;

            return inputfeatureclasspath;
        }

        // 根据实际情况判断获取路径的方式
        private string GetInputDataPath(IFeatureLayer theInputFeatureLayer)
        {
            string thePath = "";

            //thePath = GetPathInDataSet(theInputFeatureLayer);
            //if (thePath == null)
            //{
            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}

            // 当前的统计类型
            JCZF.Renderable.CGlobalVarable.Enum_AnalysisType theType = JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType;


            if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ) // 土地利用现状
            {
                thePath = GetPathInDataSet(theInputFeatureLayer);
                if (thePath == null)
                {
                    thePath = GetPathNotInDataSet(theInputFeatureLayer);
                }
            }
            else 
            {
                thePath = GetPathNotInDataSet(theInputFeatureLayer);
            }


            //if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[0]) // 土地利用现状
            //{
            //    thePath = GetPathInDataSet(theInputFeatureLayer);
            //    if (thePath == null)
            //    {
            //        thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //    }
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[1]) // 规划数据
            //{
            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[2]) // 土地供应
            //{
            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[3]) // 基本农田
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[4]) // 建设用地数据
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[5]) // 矿产资源规划
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[6]) // 采矿权
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[7]) // 探矿权
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}

            return thePath;
        }


        // 执行分析操作(输入的图层以及裁切后的图层存放路径,以及裁切结果名)
        private bool ExecuteClip(IFeatureLayer theInputFeatureLayer, string theResultFilePath, string theClipShpName)
        {

            try   // 进行裁切    
            {

                // 获取被裁切数据的路径
                string inputfeatureclasspath = GetInputDataPath(theInputFeatureLayer);

                // 构造裁切结果路径
                string outputfeatureclasspath = System.IO.Path.Combine(theResultFilePath, theClipShpName);

                // 执行裁切操作
                //
                ESRI.ArcGIS.AnalysisTools.Clip clipTool2 = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);//最好的方式是三个参数 都采用路径 m_inputFeatureLayer.FeatureClass, m_ClipFeatureClass, _pDltbOutputFeatClass);
                Geoprocessor gp2 = new Geoprocessor();
                gp2.OverwriteOutput = true;
                // 执行返回裁切后结果
                IGeoProcessorResult result2 = (IGeoProcessorResult)gp2.Execute(clipTool2, null);

                // 根本没有用s
                // string s = ReturnMessages(gp2);

                //  判断裁切操作是否成功
                //
                if (result2 != null && result2.Status == esriJobStatus.esriJobSucceeded) // 操作成功
                    return true;
                else // 否则
                    return false;
            }
            catch (Exception ex) //  裁切异常
            {
                System.Diagnostics.Debug.WriteLine(String.Format("裁切异常，原因:{0}", ex));
                return false;
            }

        }

        // 执行分析操作
        private string GeoexecuteClip2(IArray theStatisticsLayers)
        {
            string m_strTemp = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[(int)JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType];
            string text = String.Format("{0}统计结果：" + "\r\n\r\n", m_strTemp);
            string m_tempText = text;

            // 构造裁切结果路径
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa          
            string theResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
            // 建立裁切数据放置目录
            if (!System.IO.Directory.Exists(theResultFilePath))
            {
                System.IO.Directory.CreateDirectory(theResultFilePath);
            }


            // 进行裁切      
            try
            {

                // 裁切结果名的基础
                string theClipTypeName = "clip";
                // 裁切后的结果对象类
                IFeatureClass[] theResultFeatureClass = new IFeatureClass[theStatisticsLayers.Count];
                // 根据图层数量裁切
                //
                //if (theStatisticsLayers.Count == 1) // 如果只有一个图层，则不用编号形式
                //{

                //    // 获取要裁切的图层
                //    IFeatureLayer theFeatureLayer = theStatisticsLayers.get_Element(0) as IFeatureLayer;
                //    // 执行裁切分析操作   
                //    if (ExecuteClip(theFeatureLayer, theResultFilePath, theClipTypeName))
                //    {
                //        // 添加一个字段（图层名称）
                //        InsertTheLayerNameField(theResultFilePath, theClipTypeName, theFeatureLayer.Name);

                //        // 打开裁切后数据
                //        IFeatureWorkspace pFWS;
                //        IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                //        pFWS = pWorkspaceFactor.OpenFromFile(theResultFilePath, 0) as IFeatureWorkspace;
                //        theResultFeatureClass[0] = pFWS.OpenFeatureClass(theClipTypeName);//.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                //   }

                //}
                //else // 多个图层
                //{
                    // 合并图层名字列表
                    ArrayList theLayerNameList = new ArrayList();

                    for (int i = 0; i < theStatisticsLayers.Count; i++)
                    {
                        try
                        {
                            // 获取要裁切的图层
                            IFeatureLayer theFeatureLayer = theStatisticsLayers.get_Element(i) as IFeatureLayer;
                            // 构造裁切后结果名称
                            string theClipShpName = String.Format("{0}{1}", theClipTypeName, i + 1);

                           // IFeature m_ddd = theFeatureLayer.FeatureClass.GetFeature(0);

                            // 执行裁切分析操作
                            //
                            if (ExecuteClip(theFeatureLayer, theResultFilePath, theClipShpName)) // 如果成功则添加图层名字
                            {
                                // 添加一个字段（图层名称）
                                InsertTheLayerNameField(theResultFilePath, theClipShpName, theFeatureLayer.Name);
                                theLayerNameList.Add(theClipShpName);

                                //        // 打开裁切后数据
                                IFeatureWorkspace pFWS;
                                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                                pFWS = pWorkspaceFactor.OpenFromFile(theResultFilePath, 0) as IFeatureWorkspace;
                                theResultFeatureClass[i] = pFWS.OpenFeatureClass(theClipShpName);//.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                                // 得到统计结果
                                if (theResultFeatureClass != null)
                                {

                                    //对Shape_Lenght和Shape_Area重新赋值




                                    try
                                    {
                                        if (theResultFeatureClass[i].FeatureCount(null) > 0)
                                        {
                                            IFeature m_IFeature = theResultFeatureClass[i].GetFeature(0);

                                            string[,] m_strFields = new string[2, 2];
                                            m_strFields[0, 0] = "Shape_Area";
                                            m_strFields[1, 0] = "Shape_Lenght";

                                            m_strFields[0, 1] = clsMapFunction.MapFunction.MeasureArea(m_IFeature.Shape).ToString();
                                            m_strFields[1, 1] = clsMapFunction.MapFunction.MeasureLength(m_IFeature.Shape).ToString();
                                            clsMapFunction.clsSaveFeatureValue.SaveFeatureValueS(theResultFeatureClass[i], m_IFeature, m_strFields);


                                            text += GetResultOfStatics(theResultFeatureClass[i]);
                                        }
                                        else
                                        {
                                            text += "\n\n  未占用该类用地！\r\n\r\n";
                                        }
                                    }
                                    catch(SystemException errs)
                                    {
                                         text += "\n\n  未占用该类用地！\r\n\r\n";
                                    }

                                }
                                else
                                {
                                    text += "\n\n  未占用该类用地！\r\n\r\n";
                                }


                            }
                        }
                        catch(SystemException errs )
                        {
                            clsFunction.Function.MessageBoxError(errs.Message);
                            continue;
                        }
                    //}

                    //// 有问题，如果只有一个文件，则无法合并，名字也改不过来，如何重命名shp文件没有找到方法
                    //// 合并多个图层到一个图层
                    //if (theLayerNameList.Count > 0)
                    //{
                    //    theResultFeatureClass = MergeSomeLayersIntoOneLayer(theResultFilePath, theLayerNameList, theClipTypeName);
                    //}

                   
                }

                    if (m_tempText == text)
                    {
                        text += "\n\n  未占用该类用地！\r\n\r\n";
                    }
              
                return text;
            }
            catch (Exception ex)
            {
                return text += ex.Message + "，操作出错！" + "\r\n\r\n";
            }
        }

        #endregion

        ///所画图形与地类图斑进行裁切运算 m_Geometry 根本没有用到
        private string GeoexecuteClip(IGeometry m_Geometry, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "土地利用现状统计结果：" + "\r\n\r\n";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;


                // 进行裁切
                //加了没用的一句
                Geoprocessor gp2 = new Geoprocessor();
                gp2.OverwriteOutput = true;

                IFeatureClass fc = (IFeatureClass)m_inputFeatureLayer.FeatureClass;
                IFeatureDataset fds = (IFeatureDataset)fc.FeatureDataset;
                IWorkspace ws = (IWorkspace)fds.Workspace;
                string s0 = ws.PathName.ToUpper(); //workpathname
                string ss3 = fds.Name;//datasetname
                string ss4 = fc.AliasName;//dataname

                string inputfeatureclasspath = s0 + "\\" + ss3 + "\\" + ss4;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";

                ESRI.ArcGIS.AnalysisTools.Clip clipTool2 = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);//最好的方式是三个参数 都采用路径 m_inputFeatureLayer.FeatureClass, m_ClipFeatureClass, _pDltbOutputFeatClass);

                // 返回裁切后结果
                IGeoProcessorResult result2 = (IGeoProcessorResult)gp2.Execute(clipTool2, null);

                // 根本没有用s
                string s = ReturnMessages(gp2);
                // 打开裁切后数据
                _pDltbOutputFeatClass = pFWS.OpenFeatureClass("clip");//.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


                //string text =  "土地利用现状："+"\r\n"+"  统计结果：" + "\r\n";
                if (result2.Status == esriJobStatus.esriJobSucceeded) // 操作成功
                {
                    MeasureDltb(_pDltbOutputFeatClass); // 计算图斑面积

                    double sumdltb = 0;
                    for (int i = 0; i < arrdlmc.Count; i++)
                    {
                        //double temp = Convert.ToDouble(dbldltb[i]) / 10000;
                        double temp = Convert.ToDouble(dbldltb[i]);
                        text += "  " + arrdlmc[i].ToString() + "面积:  " + temp.ToString("#0.000") + " 平方米" + "\r\n\r\n";
                        sumdltb += Convert.ToDouble(dbldltb[i]) ;
                    }
                    text += "  合计：" + sumdltb.ToString("#0") + " 平方米" + (sumdltb * 15 / 10000).ToString("#0.0") + " 亩\r\n\r\n";
                }
                else // 刘扬 修改 2011后期 土地分析
                {
                    text += "裁切操作失败，无结果";
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "没有统计结果" + "\r\n\r\n";
            }
        }


        //所画图形与基本农田进行裁切运算
        private string JBNTGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "基本农田统计结果：" + "\r\n\r\n";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //_pJbntOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;

                //IFeatureClass fc = (IFeatureClass)m_inputFeatureLayer.FeatureClass;

                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;


                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";


                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);


                _pJbntOutputFeatClass = pFWS.OpenFeatureClass("clip");
                string temp = ReturnMessages(gp1);


                //string text = "基本农田：" + "\r\n" + "  统计结果：" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    double dblJbnt = MeasurePL(_pJbntOutputFeatClass);
                    //dblJbnt = dblJbnt / 10000;
                    db_jbnt = dblJbnt;
                    //text = "  统计结果：" + "\r\n";
                    text += "  基本农田面积：  " + dblJbnt.ToString("#0") + " 平方米" + "\r\n\r\n";
                    //text += "  基本农田面积：  " + dblJbnt + " 平方米" + "\r\n";                    
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "没有统计结果" + "\r\n\r\n";
            }

        }

        //所画图形与建设用地进行裁切运算
        private string JSYDGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "审批建设用地统计结果：" + "\r\n\r\n";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }
                //overlayer
                //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
                //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
                //outputlayer 

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //_pJsydOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;


                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";


                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);

                _pJsydOutputFeatClass = pFWS.OpenFeatureClass("clip");

                //string text = "建设用地：" + "\r\n" + "  统计结果：" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    double dblJsyd = MeasurePL(_pJsydOutputFeatClass);
                    //dblJsyd = dblJsyd / 10000;

                    db_jsyd = dblJsyd;
                    //m_frmZhhcResult.txtJSYD.Text = "  建设用地面积：  " + dblJsyd + " 平方米";
                    text += "  审批建设用地面积：  " + dblJsyd.ToString("#0") + "平方米" + "\r\n\r\n"; ;
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "没有统计结果" + "\r\n\r\n";
            }

        }


        ///// <summary>
        ///// 所画图形与建设用地进行裁切运算
        ///// </summary>
        ///// <param name="m_Geometry"></param>
        ///// <param name="m_selLayer"></param>
        ///// <param name="m_inputFeatureLayer"></param>
        ///// <returns></returns>
        //private double  JSYDGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        //{
        //    double m_dblJsyd = 0;
        //    string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
        //    string text = "审批建设用地统计结果：" + "\r\n\r\n";
        //    try
        //    {
        //        //裁切结果路径
        //        string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
        //        if (!System.IO.Directory.Exists(m_strResultFilePath))
        //        {
        //            System.IO.Directory.CreateDirectory(m_strResultFilePath);
        //        }
        //        //overlayer
        //        //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
        //        //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
        //        //outputlayer 

        //        IFeatureWorkspace pFWS;
        //        IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
        //        pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
        //        //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
        //        //_pJsydOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

        //        IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

        //        IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

        //        IWorkspaceName wsname = pDataSetname.WorkspaceName;

        //        string pFilePath = wsname.PathName;
        //        string pFileName = pDataSetname.Name;


        //        string inputfeatureclasspath = pFilePath + "\\" + pFileName;

        //        string outputfeatureclasspath = m_strResultFilePath + "\\clip";


        //        Geoprocessor gp1 = new Geoprocessor();
        //        gp1.OverwriteOutput = true;
        //        ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
        //        IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);

        //        _pJsydOutputFeatClass = pFWS.OpenFeatureClass("clip");

        //        //string text = "建设用地：" + "\r\n" + "  统计结果：" + "\r\n";
        //        if (result.Status == esriJobStatus.esriJobSucceeded)
        //        {
        //            m_dblJsyd = MeasurePL(_pJsydOutputFeatClass);
        //            m_dblJsyd = m_dblJsyd / 10000;

        //            db_jsyd = m_dblJsyd;
        //            //m_frmZhhcResult.txtJSYD.Text = "  建设用地面积：  " + dblJsyd + " 平方米";
        //            text += "  审批建设用地面积：  " + dblJsyd.ToString("#0.000") + "公顷" + "\r\n\r\n"; ;
        //        }
        //        return text;
        //    }
        //    catch (Exception ex)
        //    {
        //        return text + "没有统计结果" + "\r\n\r\n";
        //    }

        //}

        //所画图形与探矿权进行裁切运算
        private string TKQGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "探矿权统计结果：" + "\r\n\r\n";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }
                //overlayer
                //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
                //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
                //outputlayer 

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //_pTkqOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;

                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;


                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";


                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);


                _pTkqOutputFeatClass = pFWS.OpenFeatureClass("clip");
                //string text = "探矿权：" + "\r\n" + "  统计结果：" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    double dblJsyd = MeasurePL(_pTkqOutputFeatClass);
                    //dblJsyd = dblJsyd / 10000;
                    db_tkq = dblJsyd;
                    //m_frmZhhcResult.txtJSYD.Text = "  建设用地面积：  " + dblJsyd + " 平方米";
                    text += "  探矿权面积：  " + dblJsyd.ToString("#0") + " 平方米" + "\r\n\r\n";
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "没有统计结果" + "\r\n\r\n";
            }

        }

        //所画图形与采矿权进行裁切运算
        private string CKQGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "采矿权统计结果：" + "\r\n\r\n";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }
                //overlayer
                //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
                //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
                //outputlayer 

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //_pCkqOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;


                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";

                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);

                _pCkqOutputFeatClass = pFWS.OpenFeatureClass("clip");

                //string text = "采矿权：" + "\r\n" + "  统计结果：" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    double dblJsyd = MeasurePL(_pCkqOutputFeatClass);
                    //dblJsyd = dblJsyd / 10000;

                    db_ckq = dblJsyd;
                    //m_frmZhhcResult.txtJSYD.Text = "  建设用地面积：  " + dblJsyd + " 平方米";
                    text += "  采矿权面积：  " + dblJsyd.ToString("#0") + " 平方米" + "\r\n\r\n";
                }

                return text;
            }
            catch (Exception ex)
            {
                return text + "没有统计结果" + "\r\n\r\n";
            }

        }

        //所画图形与采矿权进行裁切运算//////基本农田
        private string CKQGeoexecuteClip1(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "采矿权统计结果基本农田：" + "\r\n\r\n";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }
                //overlayer
                //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
                //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
                //outputlayer 

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //_pCkqOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;


                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";

                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);

                if (result != null)
                {
                    _pCkqOutputFeatClass = pFWS.OpenFeatureClass("clip");

                    //string text = "采矿权：" + "\r\n" + "  统计结果：" + "\r\n";
                    if (result.Status == esriJobStatus.esriJobSucceeded)
                    {
                        double dblJsyd = MeasurePL(_pCkqOutputFeatClass);
                        //dblJsyd = dblJsyd / 10000;

                        db_ckq = dblJsyd;
                        //m_frmZhhcResult.txtJSYD.Text = "  建设用地面积：  " + dblJsyd + " 平方米";
                        text += "  采矿权面积基本农田：  " + dblJsyd.ToString("#0") + " 平方米" + "\r\n\r\n";
                    }

                }
                else
                {
                    text += "没有统计结果" + "\r\n\r\n";
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "没有统计结果" + "\r\n\r\n";
            }

        }


        //所画图形与规划图进行裁切运算
        private string QHTGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString(); //aa
            string text = "土地利用规划数据统计结果：" + "\r\n\r\n";
            text += "\r\n\r";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }
                //overlayer
                //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
                //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
                //outputlayer 

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //_pGHTOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;
                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;

                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";
                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);
                string s = ReturnMessages(gp1);
                _pGHTOutputFeatClass = pFWS.OpenFeatureClass("clip");

                //string text = "规划图：" + "\r\n" + "  统计结果：" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    MeasureGHT(_pGHTOutputFeatClass);

                    double sumdltb = 0;
                    for (int i = 0; i < arrghmc.Count; i++)
                    {
                        //double temp = Convert.ToDouble(dblghtb[i]) / 10000;
                        double temp = Convert.ToDouble(dblghtb[i]) ;
                        text += "  " + arrghmc[i].ToString() + "面积:  " + temp.ToString("#0") + " 平方米" + "\r\n\r\n";
                        //text += "\r\n\r\n";
                        sumdltb += Convert.ToDouble(dblghtb[i]);
                        //sumdltb = sumdltb / 10000;
                    }
                    db_ghsj = sumdltb;
                    text += "  合计：" + sumdltb.ToString("#0") + " 平方米" + (sumdltb * 15 / 10000).ToString("#0.0") + " 亩\r\n\r\n";

                }
                return text;
            }
            catch (Exception ex)
            {
                text += "没有统计结果" + "\r\n\r\n";

                return text;
            }

        }


        /// <summary>
        /// 所画图形与土地供应图进行裁切运算
        /// </summary>
        /// <param name="m_Geometry"></param>
        /// <param name="m_selLayer"></param>
        /// <param name="m_inputFeatureLayer"></param>
        /// <returns></returns>
        private string TDGYGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString(); //aa
            string text = "土地供应数据统计结果：" + "\r\n\r\n";
            text += "\r\n\r";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }
                //overlayer
                //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
                //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
                //outputlayer 

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //_pGHTOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;
                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;

                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";
                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);
                string s = ReturnMessages(gp1);
                _pTDGYOutputFeatClass = pFWS.OpenFeatureClass("clip");

                //string text = "规划图：" + "\r\n" + "  统计结果：" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    MeasureTDGY(_pTDGYOutputFeatClass);

                    double sumdltb = 0;
                    for (int i = 0; i < arrghmc.Count; i++)
                    {
                        //double temp = Convert.ToDouble(dblgdtb[i]) / 10000;
                        double temp = Convert.ToDouble(dblgdtb[i]) ;
                        text += "  " + arrghmc[i].ToString() + "面积:  " + temp.ToString("#0") + " 平方米" + "\r\n\r\n";
                        //text += "\r\n\r\n";
                        sumdltb += Convert.ToDouble(dblgdtb[i]);
                        //sumdltb = sumdltb / 10000;
                    }
                    db_tdgy = sumdltb;
                    text += "  合计：" + sumdltb.ToString("#0") + " 平方米" + (sumdltb * 15 / 10000).ToString("#0.0") + " 亩\r\n\r\n";

                }
                return text;
            }
            catch (Exception ex)
            {
                text += "没有统计结果" + "\r\n\r\n";

                return text;
            }

        }






        //所画图形与矿产资源图进行裁切运算
        private string KCZYGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "矿产资源规划统计结果：" + "\r\n";
            try
            {
                //裁切结果路径
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }
                //overlayer
                //IFeatureClass m_overlayFeatureClass = GeogetFeatureClass(m_Geometry, m_selLayer);
                //m_ClipFeatureClass = CreateClipFeatureClass(m_Geometry);
                //outputlayer 

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                //IFields outfields = m_inputFeatureLayer.FeatureClass.Fields;
                //_pKczyOutputFeatClass = pFWS.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                IDataLayer pDatalayer = m_inputFeatureLayer as IDataLayer;

                IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;

                IWorkspaceName wsname = pDataSetname.WorkspaceName;

                string pFilePath = wsname.PathName;
                string pFileName = pDataSetname.Name;

                string inputfeatureclasspath = pFilePath + "\\" + pFileName;

                string outputfeatureclasspath = m_strResultFilePath + "\\clip";

                Geoprocessor gp1 = new Geoprocessor();
                gp1.OverwriteOutput = true;

                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);
                IGeoProcessorResult result = (IGeoProcessorResult)gp1.Execute(clipTool, null);
                string s = ReturnMessages(gp1);

                _pKczyOutputFeatClass = pFWS.OpenFeatureClass("clip");

                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    MeasureKCZY(_pKczyOutputFeatClass);

                    double sumdltb = 0;
                    for (int i = 0; i < arrkcmc.Count; i++)
                    {
                        //double temp = Convert.ToDouble(dblkctb[i]) / 10000;
                        double temp = Convert.ToDouble(dblkctb[i]) ;
                        text += "  " + arrkcmc[i].ToString() + "面积:  " + temp.ToString("#0") + " 平方米\r\n";
                        sumdltb += Convert.ToDouble(dblkctb[i]);
                        //sumdltb = sumdltb / 10000;
                    }
                    db_kczy = sumdltb;
                    text += "  合计：" + sumdltb.ToString("#0") + " 平方米" + (sumdltb * 15 / 10000).ToString("#0.0") + " 亩\r\n";
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "没有统计结果" + "\r\n";
            }

        }


        //将所选的GeoFeature ，生成inputfeatureclass
        private IFeatureClass GeogetFeatureClass(IGeometry m_Geometry, ILayer m_selLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa

            string m_tempFilePath = _m_strFilePath + "\\feature" + time;  //aa            

            if (!System.IO.Directory.Exists(m_tempFilePath))
            {
                System.IO.Directory.CreateDirectory(m_tempFilePath);
            }
            else
            {
                System.IO.Directory.Delete(m_tempFilePath, true);
                System.IO.Directory.CreateDirectory(m_tempFilePath);
            }

            IFeatureWorkspace pFWS;
            IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
            pFWS = pWorkspaceFactor.OpenFromFile(m_tempFilePath, 0) as IFeatureWorkspace;
            IFeatureClass m_selFeatureClass = (m_selLayer as IFeatureLayer).FeatureClass;
            IFields outfields = m_selFeatureClass.Fields;

            IFeatureClass m_overlayFeatureClass = pFWS.CreateFeatureClass("tempfeatureclass", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

            IFeature saveFeature = m_overlayFeatureClass.CreateFeature();
            //IGeometry geo = m_Feature.ShapeCopy;
            saveFeature.Shape = m_Geometry;
            saveFeature.Store();

            return m_overlayFeatureClass;
        }


        public void CreateClipFeatureClass(IGeometry geo)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa

            string m_tempFilePath = _m_strFilePath + "\\feature" + time;  //aa            

            if (!System.IO.Directory.Exists(m_tempFilePath))
            {
                System.IO.Directory.CreateDirectory(m_tempFilePath);
            }
            else
            {
                System.IO.Directory.Delete(m_tempFilePath, true);
                System.IO.Directory.CreateDirectory(m_tempFilePath);
            }

            //string clipFolder = this.m_txtBoxsavePath.Text;
            string clipName = "clip";
            string shapeFieldName = "shape";//?

            //open the folder to contain the shapefile as a workspace
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
            IWorkspace pworkspace = pWorkspaceFactory.OpenFromFile(m_tempFilePath, 0);
            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pworkspace;

            //set a simple fields collection
            IFields pFields = new Fields() as IFields ;
            IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;

            //make the shape fied
            IField pField = new Field() as IField ;
            IFieldEdit pFieldEdit = (IFieldEdit)pField;   //edit the field properties

            IGeometryDef pGeomdef = new GeometryDef() as IGeometryDef ;   //reture information about the geometry definition
            IGeometryDefEdit pGeomdefEdit = (IGeometryDefEdit)pGeomdef;   //modify the geometry definition

            pGeomdefEdit.SpatialReference_2 = geo.SpatialReference;   //the spacial reference of dataset, write only
            pGeomdefEdit.GeometryType_2 = geo.GeometryType;   //the geometry type, write only

            pFieldEdit.Name_2 = "shape";   //the name of the field, write only
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;//the type of the field, write only **
            pFieldEdit.GeometryDef_2 = pGeomdef;//the definition of geometry if IsGeometry is true,write only

            pFieldsEdit.AddField(pField);

            //create the shapefile
            m_ClipFeatureClass = pFeatureWorkspace.CreateFeatureClass(clipName, pFields, null, null, esriFeatureType.esriFTSimple, shapeFieldName, "");

            IFeature pFea = m_ClipFeatureClass.CreateFeature();//wb
            pFea.Shape = geo;//wb
            pFea.Store();//wb
        }


        #endregion





        //将所选的feature ，生成inputfeatureclass
        private IFeatureClass getFeatureClass(IFeature m_Feature, ILayer m_selLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa

            string m_tempFilePath = _m_strFilePath + "\\feature" + time;  //aa            

            if (!System.IO.Directory.Exists(m_tempFilePath))
            {
                System.IO.Directory.CreateDirectory(m_tempFilePath);
            }
            else
            {
                System.IO.Directory.Delete(m_tempFilePath, true);
                System.IO.Directory.CreateDirectory(m_tempFilePath);
            }

            IFeatureWorkspace pFWS;
            IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
            pFWS = pWorkspaceFactor.OpenFromFile(m_tempFilePath, 0) as IFeatureWorkspace;
            IFeatureClass m_selFeatureClass = (m_selLayer as IFeatureLayer).FeatureClass;
            IFields outfields = m_selFeatureClass.Fields;

            IFeatureClass m_overlayFeatureClass = pFWS.CreateFeatureClass("tempfeatureclass", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

            IFeature saveFeature = m_overlayFeatureClass.CreateFeature();
            IGeometry geo = m_Feature.ShapeCopy;
            saveFeature.Shape = geo;
            saveFeature.Store();

            return m_overlayFeatureClass;
        }

        //地类图斑面积计算
        public void MeasureDltb(IFeatureClass  outfeatureclass)
        {
            ////有多少种地类图斑
            IFeatureCursor pCursor = null;
            ArrayList arr = new ArrayList();
            //ArrayList arrdlmc = new ArrayList();
            string m_strFieldName = "DLbM";
            int indexofDLDM = outfeatureclass.FindField(m_strFieldName);
            int indexofDLMC = outfeatureclass.FindField("DLMC");
            if (indexofDLDM == -1)
            {
                m_strFieldName = "DLDM";
                indexofDLDM = outfeatureclass.FindField(m_strFieldName);//二调后发生变化

            }
            if (indexofDLDM == -1 || indexofDLMC == -1)
            {
                _strHCJG = "非常抱歉，土地利用现状图层的数据结构不符合要求，无法进行计算！";
            }
            else
            {
                try
                {
                    ICursor cursor = (ICursor)outfeatureclass.Search(null, false);
                    IDataStatistics dataStatistics = new DataStatisticsClass();
                    dataStatistics.Field = m_strFieldName;
                    dataStatistics.Cursor = cursor;
                    System.Collections.IEnumerator enumerator = dataStatistics.UniqueValues;
                    enumerator.Reset();
                    while (enumerator.MoveNext())
                    {
                        object myObject = enumerator.Current;
                        arr.Add(myObject.ToString());

                    }
                    //每种地类图斑进行面积计算                
                    double dltbtemp;
                    int once;
                    IFeatureCursor pcursor1 = null;
                    IFeature ifeature;//= pcursor1.NextFeature();

                    for (int i = 0; i < arr.Count; i++)
                    {
                        dltbtemp = 0;
                        once = 1;
                        pcursor1 = outfeatureclass.Search(null, false);
                        ifeature = pcursor1.NextFeature();

                        while (ifeature != null)
                        {
                            if (ifeature.get_Value(indexofDLDM).ToString() == arr[i].ToString())
                            //if (Convert.ToInt32(ifeature.get_Value(indexofDLDM)) == Convert.ToInt32(arr[i]))
                            {
                                if (once == 1)
                                {
                                    arrdlmc.Add(ifeature.get_Value(indexofDLMC).ToString());
                                    once = 2;
                                }
                               
                                double dArea = Functions.MapFunction.MeasureArea(ifeature.Shape);
                                if (dArea < 0)
                                {
                                    dArea = 0 - dArea;
                                }
                                dltbtemp += dArea;

                            }
                            ifeature = pcursor1.NextFeature();
                        }
                        dbldltb.Add(dltbtemp.ToString());


                    }
                }

                catch (Exception ex)
                {

                }
            }

        }

        //规划图面积计算
        public void MeasureGHT(IFeatureClass outfeatureclass)
        {
            ////有多少种规划分区
            IFeatureCursor pCursor = null;
            ArrayList arr = new ArrayList();
            //ArrayList arrdlmc = new ArrayList();
            int indexofDLDM = outfeatureclass.FindField("FQMC");
            int indexofDLMC = outfeatureclass.FindField("FQMC");
            if (indexofDLDM == -1 || indexofDLMC == -1)
            {
                _strHCJG="非常抱歉，规划图层的数据结构不符合要求，无法进行计算！";
            }
            else
            {
                try
                {
                    ICursor cursor = (ICursor)outfeatureclass.Search(null, false);
                    IDataStatistics dataStatistics = new DataStatisticsClass();
                    dataStatistics.Field = "FQMC";
                    dataStatistics.Cursor = cursor;
                    System.Collections.IEnumerator enumerator = dataStatistics.UniqueValues;
                    enumerator.Reset();
                    while (enumerator.MoveNext())
                    {
                        object myObject = enumerator.Current;
                        arr.Add(myObject.ToString());

                    }
                    //每种地类图斑进行面积计算                
                    double dltbtemp;
                    int once;
                    IFeatureCursor pcursor1 = null;
                    IFeature ifeature;//= pcursor1.NextFeature();

                    for (int i = 0; i < arr.Count; i++)
                    {
                        dltbtemp = 0;
                        once = 1;
                        pcursor1 = outfeatureclass.Search(null, false);
                        ifeature = pcursor1.NextFeature();

                        while (ifeature != null)
                        {
                            if (ifeature.get_Value(indexofDLDM).ToString() == arr[i].ToString())
                            //if (Convert.ToInt32(ifeature.get_Value(indexofDLDM)) == Convert.ToInt32(arr[i]))
                            {
                                if (once == 1)
                                {
                                    arrghmc.Add(ifeature.get_Value(indexofDLMC).ToString());
                                    once = 2;
                                }
                                
                                double dArea = Functions.MapFunction.MeasureArea(ifeature.Shape);
                                if (dArea < 0)
                                {
                                    dArea = 0 - dArea;
                                }
                                dltbtemp += dArea;

                            }
                            ifeature = pcursor1.NextFeature();
                        }
                        dblghtb.Add(dltbtemp.ToString());


                    }
                }

                catch (Exception ex)
                {

                }
            }

        }


        /// <summary>
        /// 土地供应面积计算
        /// </summary>
        /// <param name="outfeatureclass"></param>
        public void MeasureTDGY(IFeatureClass outfeatureclass)
        {

            IFeatureCursor pCursor = null;
            ArrayList arr = new ArrayList();
            //ArrayList arrdlmc = new ArrayList();
            int indexofDLDM = outfeatureclass.FindField("dkmc");
            int indexofDLMC = outfeatureclass.FindField("dkmc");
            if (indexofDLDM == -1 || indexofDLMC == -1)
            {
                _strHCJG = "非常抱歉，土地供应图层的数据结构不符合要求，无法进行计算！";
            }
            else
            {
                try
                {
                    ICursor cursor = (ICursor)outfeatureclass.Search(null, false);
                    IDataStatistics dataStatistics = new DataStatisticsClass();
                    dataStatistics.Field = "dkmc";
                    dataStatistics.Cursor = cursor;
                    System.Collections.IEnumerator enumerator = dataStatistics.UniqueValues;
                    enumerator.Reset();
                    while (enumerator.MoveNext())
                    {
                        object myObject = enumerator.Current;
                        arr.Add(myObject.ToString());

                    }
                    //每种地类图斑进行面积计算                
                    double dltbtemp;
                    int once;
                    IFeatureCursor pcursor1 = null;
                    IFeature ifeature;//= pcursor1.NextFeature();

                    for (int i = 0; i < arr.Count; i++)
                    {
                        dltbtemp = 0;
                        once = 1;
                        pcursor1 = outfeatureclass.Search(null, false);
                        ifeature = pcursor1.NextFeature();

                        while (ifeature != null)
                        {
                            if (ifeature.get_Value(indexofDLDM).ToString() == arr[i].ToString())
                            //if (Convert.ToInt32(ifeature.get_Value(indexofDLDM)) == Convert.ToInt32(arr[i]))
                            {
                                if (once == 1)
                                {
                                    arrghmc.Add(ifeature.get_Value(indexofDLMC).ToString());
                                    once = 2;
                                }
                               
                                double dArea = Functions.MapFunction.MeasureArea(ifeature.Shape);
                                if (dArea < 0)
                                {
                                    dArea = 0 - dArea;
                                }
                                dltbtemp += dArea;

                            }
                            ifeature = pcursor1.NextFeature();
                        }
                        dblgdtb.Add(dltbtemp.ToString());


                    }
                }

                catch (Exception ex)
                {

                }
            }

        }
        //矿产资源图面积计算
        public void MeasureKCZY(IFeatureClass outfeatureclass)
        {
            ////有多少种地类图斑
            IFeatureCursor pCursor = null;
            ArrayList arr = new ArrayList();
            //ArrayList arrdlmc = new ArrayList();
            int indexofDLDM = outfeatureclass.FindField("GHQLB1");
            int indexofDLMC = outfeatureclass.FindField("GHQMC1");
            if (indexofDLDM == -1 || indexofDLMC == -1)
            {
                _strHCJG = "非常抱歉，矿产资源图层的数据结构不符合要求，无法进行计算！";
            }
            else
            {
                try
                {
                    ICursor cursor = (ICursor)outfeatureclass.Search(null, false);
                    IDataStatistics dataStatistics = new DataStatisticsClass();
                    dataStatistics.Field = "GHQLB1";
                    dataStatistics.Cursor = cursor;
                    System.Collections.IEnumerator enumerator = dataStatistics.UniqueValues;
                    enumerator.Reset();
                    while (enumerator.MoveNext())
                    {
                        object myObject = enumerator.Current;
                        arr.Add(myObject.ToString());

                    }
                    //每种地类图斑进行面积计算                
                    double dltbtemp;
                    int once;
                    IFeatureCursor pcursor1 = null;
                    IFeature ifeature;//= pcursor1.NextFeature();

                    for (int i = 0; i < arr.Count; i++)
                    {
                        dltbtemp = 0;
                        once = 1;
                        pcursor1 = outfeatureclass.Search(null, false);
                        ifeature = pcursor1.NextFeature();

                        while (ifeature != null)
                        {
                            if (ifeature.get_Value(indexofDLDM).ToString() == arr[i].ToString())
                            //if (Convert.ToInt32(ifeature.get_Value(indexofDLDM)) == Convert.ToInt32(arr[i]))
                            {
                                if (once == 1)
                                {
                                    arrkcmc.Add(ifeature.get_Value(indexofDLMC).ToString());
                                    once = 2;
                                }
                              
                                double dArea = Functions.MapFunction.MeasureArea(ifeature.Shape);
                                if (dArea < 0)
                                {
                                    dArea = 0 - dArea;
                                }
                                dltbtemp += dArea;

                            }
                            ifeature = pcursor1.NextFeature();
                        }
                        dblkctb.Add(dltbtemp.ToString());


                    }
                }

                catch (Exception ex)
                {

                }
            }

        }



        #region  面积计算

        public double MeasurePL(IFeatureClass outfeatureclass)
        {
            try
            {
                double pArea = 0;
                IFeatureCursor pCursor = null;
                pCursor = outfeatureclass.Search(null, false);
                IFeature pFeature = pCursor.NextFeature();
                while (pFeature != null)
                {
                  
                    double dArea =  Functions.MapFunction.MeasureArea(pFeature.Shape);
                    if (dArea < 0)
                    {
                        dArea = -dArea;
                    }
                    pArea = pArea + dArea;
                    pFeature = pCursor.NextFeature();
                }
                return pArea;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;

            }
        }


        #endregion
    }
}
