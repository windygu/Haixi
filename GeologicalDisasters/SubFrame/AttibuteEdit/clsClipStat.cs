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

        public static string m_strLayerNameOfStatics = ""; // Ҫ������ͼ������  // ���� �޸� 2011 ����

        private IEnumLayer m_Grouplayers; // �����޸� 201111

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
        /// ����ũ�����
        /// </summary>
        public double db_jbnt;
        /// <summary>
        /// �����õ����
        /// </summary>
        public double db_jsyd;
        /// <summary>
        /// �滮�������
        /// </summary>
        public double db_ghsj;
        /// <summary>
        /// �滮�������
        /// </summary>
        public double db_tdgy;
        /// <summary>
        /// ����滮���
        /// </summary>
        public double db_kczy;
        /// <summary>
        /// �ɿ�Ȩ���
        /// </summary>
        public double db_ckq;

        /// <summary>
        /// ʡ��֤���з�֤�����
        /// </summary>
        public double db_ckq_zmj;

        /// <summary>
        /// ̽��Ȩ���
        /// </summary>
        public double db_tkq;

        #endregion










        #region �仯ͼ�߿�˲�
        public void execute()
        {
            //�ж����ݿ����Ƿ��Ѿ��˲�������ֶ�CLIPRESULT�Ƿ�Ϊ��
            //�������ݿ�
            string sql = "select clipResult from ���غ˲� where objectid='" + _m_OID + "'";
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
                        if (pLayer.Name.Contains("_����ͼ��") && pLayer.Name.Contains(_zqm))
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;
                            break;
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();

                m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����

                if (m_inputFeatureLayer != null)
                {

                    m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����

                    string clipResult = executeClip(_pFeature, _m_selLayer, m_inputFeatureLayer);
                    _strHCJG = clipResult;
                    //�������ݿ�
                    string m_strUpdateRow = "update " + "���غ˲�" + " set clipResult='" + clipResult + "'";

                    m_strUpdateRow = m_strUpdateRow + " where Objectid = " + _m_OID;

                    this._pDataAcess.ExecuteSQLNoReturn(m_strUpdateRow);
                }
            }
        }

        // ��һ��ͼ�����һ���ֶΣ�ͼ�����ƣ�
        private void InsertTheLayerNameField(string theCatalogOfShpFiles, string theShpName, string theLayerName)
        {
            // ��ȡҪ����ֶε�ͼ��
            IFeatureLayer pFeatureLayer = GetOneLayerByFilePath(theCatalogOfShpFiles, theShpName) as IFeatureLayer;
            // �ֶ�����
            string fieldName = JCZF.Renderable.CGlobalVarable.m_strFieldNameOfLayerInTable;// ����ͼ�����ƣ���ϵͳ����ģ�����ͳ�Ƶ�ͼ����
            int length = 50;
            // ͼ������
            string theDefaultValue = "";
            bool isSuccess = MapFunction.AddLayerAttributeString(fieldName, fieldName, length, (string)theDefaultValue, true, pFeatureLayer);

            if (isSuccess) // ����ɹ��򱣴�ͼ������
            {
                // ����ͼ������
                MapFunction.SaveOneStringToOneField(pFeatureLayer.FeatureClass, fieldName, theLayerName);
            }
        }

        //��ѡͼ�������ͼ�߽��в�������
        private string executeClip(IFeature m_Feature, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa

            try
            {
                //���н��·��
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

                string text = "  ͳ�ƽ����" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    MeasureDltb(pDltbOutputFeatClass1);

                    double sumdltb = 0;
                    for (int i = 0; i < arrdlmc.Count; i++)
                    {
                        text += "  " + arrdlmc[i].ToString() + "���:  " + dbldltb[i].ToString() + " ƽ����" + "\r\n";
                        sumdltb += Convert.ToDouble(dbldltb[i]);
                    }
                    text += "  �ϼƣ�" + sumdltb + " ƽ����" + "\r\n";
                }
                return text;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        #endregion

        #region  ���� �޸� 2011���� �������÷���


        // �ҵ����ٰ�������һ���ؼ��־���ͼ����
        private bool FindItemOfContainOverOneKey(string theContent, ArrayList theList)
        {
            for (int i = 0; i < theList.Count; i++)
            {
                string theKey = theList[i].ToString().Trim(); // ��ȡһ���ؼ���
                if (theContent.Contains(theKey) == true)
                    return true;

            }

            return false;

        }


        // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ���������
        private IArray GetStaticsLayersByDistrictList(string layerNamePart, ArrayList districtNameList)
        {


            /*
              ILayer pLayer;
            IArray pArray;
            pArray = new ESRI.ArcGIS.esriSystem.Array() as IArray ;
             
            // ��ȡͼ������
            IEnumLayer theLayers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);         
            pLayer = theLayers.Next();
            
            // �ҵ�ĳ����������Ӧ�ĵ���ͼ�� 
            while (pLayer != null)
            {
                IFeatureLayer pFeatureLayer = (IFeatureLayer)pLayer; // ת��Ϊʸ��ͼ��
                // ����ͼ��(ͼ����ʸ��ͼ�㡢ͼ�������ݲ�Ϊ����ΪΪ��״ͼ��)
                if (pFeatureLayer != null && pFeatureLayer.FeatureClass != null && pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    // ͼ����������ж�(����ĳ���������Լ�����������)
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
            m_TDLYXZLayers = GetTDLYXZLayers();//�������������������ʸ��ͼ��
            if (m_TDLYXZLayers == null) return m_Array;
            for (int i = 0; i < m_TDLYXZLayers.Count; i++)
            {
                try
                {
                    m_Layer = (ILayer)m_TDLYXZLayers[i];
                    if (m_Layer != null && m_Layer is IFeatureLayer)
                    {
                        
                        IFeatureLayer m_FeatureLayer = (IFeatureLayer)m_Layer; // ת��Ϊʸ��ͼ��
                        // ����ͼ��(ͼ����ʸ��ͼ�㡢ͼ�������ݲ�Ϊ����ΪΪ��״ͼ��)
                        if (m_FeatureLayer != null && m_FeatureLayer.FeatureClass != null && m_FeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            // ͼ����������ж�(����ĳ���������Լ�����������)
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
        /// �������������������ʸ��ͼ��
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

                    if (pGroupLayer.Name == "����������״")
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
                MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return m_TDLYXZLayers;
       }
        /// <summary>
        /// ������µ������
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
                MessageBox.Show("����" + errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // ��ȡͼ������
            IEnumLayer theLayers = Functions.MapFunction.GetLayers(_m_axMapcontrol.Map);

            ILayer pLayer;
            IArray pArray;
            pArray = new ESRI.ArcGIS.esriSystem.Array() as IArray ;

            pLayer = theLayers.Next();
            // �ҵ�ĳ����������Ӧ�ĵ���ͼ�� 
            while (pLayer != null)
            {
                IFeatureLayer pFeatureLayer = (IFeatureLayer)pLayer; // ת��Ϊʸ��ͼ��
                // ����ͼ��(ͼ����ʸ��ͼ�㡢ͼ�������ݲ�Ϊ����ΪΪ��״ͼ��)
                if (pFeatureLayer != null && pFeatureLayer.FeatureClass != null && pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    // ͼ����������ж�(����ĳ���������Լ�����������)
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


        #region ѡ������˲�




        #region  ���� �޸� 2011���� �������͵�ͳ�Ʒ���

        // ִ��ͳ�Ʒ���
        public void BasicExecuteStatistics(IArray theStatisticsLayers)
        {
            _strHCJG = "";
            // û��ͳ��ͼ�㣬�򷵻�
            if (theStatisticsLayers==null || theStatisticsLayers.Count < 1)
                return;

            IFeatureLayer theInputFeatureLayer = theStatisticsLayers.get_Element(0) as IFeatureLayer;

            m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����


            if (theInputFeatureLayer != null)
            {
                m_strLayerNameOfStatics = theInputFeatureLayer.Name; // ���� �޸� 2011 ����

                string clipResult = GeoexecuteClip2(theStatisticsLayers);

                _strHCJG = clipResult;

            }
        }


        // �������÷���
        public void Geoexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {
                // ��ȡͼ����������������
                IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

                ArrayList theFeatureList = MapFunction.GetFeatureIncludeGeometry(m_xzqFeatureLayer as ILayer, _m_Geometry, this._m_axMapcontrol.SpatialReference);
                ArrayList theDistrictNameList = new ArrayList();
                for (int i = 0; i < theFeatureList.Count; i++)
                {
                    IFeature m_xzqFeature = theFeatureList[i] as IFeature;
                    int index = m_xzqFeature.Fields.FindField("xzqdm");
                    _zqm = m_xzqFeature.get_Value(index).ToString();
                    theDistrictNameList.Add(_zqm);
                }



                #region  ���� �޸� 2011���� �������÷���

                // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ��������б�
                IArray theStatisticsLayers = GetStaticsLayersByDistrictList("����ͼ��", theDistrictNameList);
                if (theStatisticsLayers == null || theStatisticsLayers.Count<1)
                {
                    theStatisticsLayers = GetStaticsLayersByDistrictList("DLTB_", theDistrictNameList);
                }
                //IArray theStatisticsLayers = GetStaticsLayers("_����ͼ��", _zqm);

                // ִ��ͳ�Ʒ���
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion

            }
        }



        // ����ũ�����
        public void JBNTexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  ���� �޸� 2011���� ����ũ�����

                // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ���������
                IArray theStatisticsLayers = GetStaticsLayers("����ũ��", "");

                // ִ��ͳ�Ʒ���
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }


        // �滮���ݷ���
        public void QHTexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  ���� �޸� 2011���� �滮���ݷ���

                // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ���������
                IArray theStatisticsLayers = GetStaticsLayers("�滮", "");

                // ִ��ͳ�Ʒ���
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }

        // ���ع�Ӧ����
        public void TDGYexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  ���� �޸� 2011���� ���ع�Ӧ����

                // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ���������
                IArray theStatisticsLayers = GetStaticsLayers("���ع�Ӧ", "");

                // ִ��ͳ�Ʒ���
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }


        // �����õ����ݷ���
        public void JSYDexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  ���� �޸� 2011���� �����õ����ݷ���


                // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ���������
                IArray theStatisticsLayers = GetStaticsLayers("�����õ�", "");

                // ִ��ͳ�Ʒ���
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }

        // �����Դ�滮����
        public void KCZYexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  ���� �޸� 2011���� �����Դ�滮����

                // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ���������
                IArray theStatisticsLayers = GetStaticsLayers("�����Դ", "");

                // ִ��ͳ�Ʒ���
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }




        // �ɿ�Ȩ����
        public void CKQexecute()
        {

            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  ���� �޸� 2011���� �ɿ�Ȩ����

                // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ���������
                IArray theStatisticsLayers = GetStaticsLayers("�ɿ�Ȩ", "");

                // ִ��ͳ�Ʒ���
                BasicExecuteStatistics(theStatisticsLayers);

                #endregion
            }

        }


        // ̽��Ȩ����
        public void TKQexecute()
        {
            _strHCJG = "";
            if (this._m_Geometry != null)
            {

                #region  ���� �޸� 2011���� ̽��Ȩ����

                // ��ȡͳ�Ƶ�ͼ�㣨����ͼ�����ƵĲ��ֺ���������
                IArray theStatisticsLayers = GetStaticsLayers("̽��Ȩ", "");

                // ִ��ͳ�Ʒ���
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
        //                if (pLayer.Name.Contains("����ũ��"))  //����ũ���
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����

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
        //                if (pLayer.Name.Contains("�滮ͼ"))  //����ũ���
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����

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

        //        ////��ȡzqm
        //        //IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

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
        //                if (pLayer.Name.Contains("���ع�Ӧ"))  //����ũ���
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����

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
        //            _m_selLayer = MapFunction.getFeatureLayerByName("���غ˲�", _m_axMapcontrol);
        //        }
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //��ȡzqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

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

        //            // ���� �޸� 2011���� ����
        //            if (pFeaturelay.FeatureClass != null && pFeaturelay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                if (pLayer.Name.Contains("�����õ�") && pLayer.Name.Contains(_zqm))  //�����õ�
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����

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

        //        //��ȡzqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

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
        //                if (pLayer.Name.Contains("�����Դ"))
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ���� 

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����

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

        //        //��ȡzqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

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
        //                if (pLayer.Name.Contains("�ɿ�Ȩ"))
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
        //        //        if (pLayer.Name.Contains("�з�֤"))
        //        //        {
        //        //            m_inputFeatureLayer2 = pLayer as IFeatureLayer;
        //        //            break;
        //        //        }

        //        //    }
        //        //    pLayer = m_Layers.Next();
        //        //}
        //        //m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ���� 

        //        string clipResult = "";
        //        string clipResult2 = "";
        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ���� 

        //            clipResult = CKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);

        //        }

        //        if (m_inputFeatureLayer2 != null) // ���� �޸� 2011 ����
        //        {

        //            m_strLayerNameOfStatics += String.Format(";{0}", m_inputFeatureLayer2.Name); // ���� �޸� 2011 ���� 

        //            clipResult2 = CKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer2);

        //        }
        //        _strHCJG = "";
        //        if (clipResult.IndexOf("����") >= 0)
        //        {
        //            _strHCJG = String.Format("\r\nʡ��֤-{0}", clipResult); // ���� �޸� 2011 ����
        //        }
        //        if (clipResult2.IndexOf("����") >= 0)
        //        {
        //            _strHCJG += String.Format("\r\n�з�֤-{0}", clipResult2); // ���� �޸� 2011 ����
        //        }
        //    }


        //}
        //public void CKQexecute1()
        //{
        //    if (this._m_Geometry != null)
        //    {

        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //��ȡzqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

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
        //                if (pLayer.Name.Contains("����ũ��"))
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
        //        //        if (pLayer.Name.Contains("�з�֤"))
        //        //        {
        //        //            m_inputFeatureLayer2 = pLayer as IFeatureLayer;
        //        //            break;
        //        //        }

        //        //    }
        //        //    pLayer = m_Layers.Next();
        //        //}
        //        //m_Layers.Reset();

        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ���� 

        //        string clipResult = "";
        //        string clipResult2 = "";
        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ���� 

        //            clipResult = CKQGeoexecuteClip1(_m_Geometry, _m_selLayer, m_inputFeatureLayer);

        //        }

        //        //if (m_inputFeatureLayer2 != null) // ���� �޸� 2011 ����
        //        //{

        //        //    m_strLayerNameOfStatics += String.Format(";{0}", m_inputFeatureLayer2.Name); // ���� �޸� 2011 ���� 

        //        //    clipResult2 = CKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer2);

        //        //}
        //        _strHCJG = "";
        //        if (clipResult.IndexOf("����") >= 0)
        //        {
        //            _strHCJG = String.Format("\r\nʡ��֤����ũ��-{0}", clipResult); // ���� �޸� 2011 ����
        //        }
        //        //if (clipResult2.IndexOf("����") >= 0)
        //        //{
        //        //    _strHCJG += String.Format("\r\n�з�֤-{0}", clipResult2); // ���� �޸� 2011 ����
        //        //}
        //    }


        //}



        //public void TKQexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //��ȡzqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

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
        //                if (pLayer.Name.Contains("̽��Ȩ"))  //̽��Ȩ
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();



        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����


        //            string clipResult = TKQGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
        //            _strHCJG = clipResult;
        //        }
        //    }


        //}



        //// ̽��Ȩ����
        //public void TKQexecute()
        //{
        //    if (this._m_Geometry != null)
        //    {
        //        IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

        //        //��ȡzqm
        //        IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

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
        //                if (pLayer.Name.Contains("̽��Ȩ"))  //̽��Ȩ
        //                {
        //                    m_inputFeatureLayer = pLayer as IFeatureLayer;
        //                    break;
        //                }

        //            }
        //            pLayer = m_Layers.Next();
        //        }
        //        m_Layers.Reset();



        //        m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����

        //        if (m_inputFeatureLayer != null)
        //        {
        //            m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����


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
                    _m_selLayer = MapFunction.getFeatureLayerByName("���غ˲�", _m_axMapcontrol);
                }
                IFeatureClass m_FeatureClass = (_m_selLayer as FeatureLayer).FeatureClass;

                //��ȡzqm
                IFeatureLayer m_xzqFeatureLayer = MapFunction.getFeatureLayerByName("��", this._m_axMapcontrol);

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
                        if (pLayer.Name.Contains("�����õ�") && pLayer.Name.Contains(_zqm))  //�����õ�
                        {
                            m_inputFeatureLayer = pLayer as IFeatureLayer;

                            m_strLayerNameOfStatics = ""; // ���� �޸� 2011 ����

                            if (m_inputFeatureLayer != null)
                            {
                                m_strLayerNameOfStatics = m_inputFeatureLayer.Name; // ���� �޸� 2011 ����

                                JSYDGeoexecuteClip(_m_Geometry, _m_selLayer, m_inputFeatureLayer);
                                m_dbJSYD = m_dbJSYD + db_jsyd;
                                //_strHCJG = clipResult;

                                AddtoTOC(pJsydOutputFeatClass, "�����õ�����clip");
                                //loadJSYDProp(pJsydOutputFeatClass); ���� �޸�201111
                            }
                        }

                    }
                    pLayer = m_Layers.Next();
                }
                m_Layers.Reset();
                _strHCJG = "���������õ�ͳ�ƽ����" + "\r\n\r\n";
                _strHCJG += "  ���������õ������  " + m_dbJSYD.ToString("#0") + "ƽ����" + "\r\n\r\n";

            }
        }



        #region ��ӵ�toc  ����20110814
        private void AddtoTOC(IFeatureClass outputfeatclass, string layername)
        {
            IFeatureLayer outlayer = new FeatureLayer() as IFeatureLayer;

            outlayer.SpatialReference = _m_axMapcontrol.SpatialReference;

            outlayer.FeatureClass = outputfeatclass;
            outlayer.Name = layername;

            IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
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
                hcjg1.Name = "�˲���";
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
        /// ����ĳ����������ũ��
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
                        if (pLayer.Name.Contains("����ũ��"))  //����ũ���
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
        /// �������ع滮����
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
                        if (pLayer.Name.Contains(str_dsmc + "�滮ͼ"))  //�滮ͼ
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
        /// ����ĳ�����������õ�
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
                        if (pLayer.Name.Contains("�����õ�"))  //�����õ�
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
        /// ����ĳ�����������Դ
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
                        if (pLayer.Name.Contains("�����Դ�滮"))  //�����õ�
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
        /// ����ĳ����ɿ�Ȩ���
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
                        if (pLayer.Name.Contains("ʡ��֤"))
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
                        if (pLayer.Name.Contains("�з�֤"))
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
                        if (pLayer.Name.Contains("̽��Ȩ"))
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

        /////����ͼ����ָ��ͼ����в��з���
        //private string GeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        //{
        //    string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
        //    string text = "";
        //    try
        //    {
        //        //���н��·��
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

        //        //����û�õ�һ��
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

        //        ESRI.ArcGIS.AnalysisTools.Clip clipTool2 = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath,m_ClipFeatureClass, outputfeatureclasspath);//��õķ�ʽ���������� ������·�� m_inputFeatureLayer.FeatureClass, m_ClipFeatureClass, _pDltbOutputFeatClass);


        //        IGeoProcessorResult result2 = (IGeoProcessorResult)gp2.Execute(clipTool2, null);

        //        string s = ReturnMessages(gp2);

        //        _pDltbOutputFeatClass = pFWS.OpenFeatureClass("clip");//.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


        //        //string text =  "����������״��"+"\r\n"+"  ͳ�ƽ����" + "\r\n";
        //        if (result2.Status == esriJobStatus.esriJobSucceeded)
        //        {
        //            MeasureDltb(_pDltbOutputFeatClass);

        //            double sumdltb = 0;
        //            for (int i = 0; i < arrdlmc.Count; i++)
        //            {
        //                double temp = Convert.ToDouble(dbldltb[i]) / 10000;
        //                text += "  " + arrdlmc[i].ToString() + "���:  " + temp.ToString("#0.000") + " ����" + "\r\n";
        //                sumdltb += Convert.ToDouble(dbldltb[i]) / 10000;
        //            }
        //            text += "  �ϼƣ�" + sumdltb.ToString("#0.000") + " ����" + "\r\n";
        //        }
        //        return text;
        //    }
        //    catch (Exception ex)
        //    {
        //        return text + "û��ͳ�ƽ��" + "\r\n";
        //    }

        //}

        // ��ȡͼ��(����·��)
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

        // �ϲ����ͼ�����ݵ�һ��ͼ��,
        private IFeatureClass MergeSomeLayersIntoOneLayer(string theCatalogOfSomeShpFile, ArrayList theLayerNameList, string theMergeShpName)
        {
            IFeatureClass m_IFeatureClass;
            try
            {

                //�ϲ�ͼ��ļ���
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


                //�ж�ͼ���Ƿ����2��
                if (pArray.Count < 2)
                {
                    m_IFeatureLayer = pArray.get_Element(0) as IFeatureLayer;
                    // �򿪲��к�����
                    //IFeatureWorkspace pFWS;
                    //IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                    //pFWS = pWorkspaceFactor.OpenFromFile(theCatalogOfSomeShpFile, 0) as IFeatureWorkspace;
                    //m_IFeatureClass = pFWS.OpenFeatureClass(theMergeShpName);
                    m_IFeatureClass = m_IFeatureLayer.FeatureClass;
                    return m_IFeatureLayer.FeatureClass; // Ӧ���޸�Ψһ��һ��ͼ������
                }

                //�������ͼ���fields��
                ITable pTable;
                pLayer = pArray.get_Element(0) as ILayer;
                pTable = (ITable)pLayer;


                //����ļ�����
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

                //���shapefile�����ƺ�λ��
                pNewWSName = new WorkspaceName() as IWorkspaceName;
                pNewWSName.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory";
                pNewWSName.PathName = theCatalogOfSomeShpFile;
                pDatasetName = (IDatasetName)pFeatureClassName;
                pDatasetName.Name = theMergeShpName;
                pDatasetName.WorkspaceName = pNewWSName;

                //�ϲ�ͼ��
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

        // �õ�ĳ������ͳ�ƵĽ��
        private string GetResultOfStatics(IFeatureClass p_theFeatureClass)
        {
            string text = "";
            _strHCJG = "";
            if (p_theFeatureClass != null) // �����ɹ�
            {
                // ��ǰ��ͳ������
                JCZF.Renderable.CGlobalVarable.Enum_AnalysisType theType = JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType;

                if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ) // ����������״
                {

                    _pDltbOutputFeatClass = p_theFeatureClass; // ��ȡ��������ͼ������

                    MeasureDltb(_pDltbOutputFeatClass); // ����ͼ�����

                    double sumdltb = 0;
                    for (int i = 0; i < arrdlmc.Count; i++)
                    {
                        double temp = Convert.ToDouble(dbldltb[i]) ;
                        sumdltb += temp;

                        text += "  " + arrdlmc[i].ToString() + "���:  " + temp.ToString("#0") + " ƽ����" + "\r\n\r\n";
                      
                    }
                    //db_t
                    text += "  �ϼƣ�" + sumdltb.ToString("#0") + " ƽ���ף�" + (sumdltb * 15 / 10000).ToString("#0.0") + "��Ķ��\r\n\r\n";

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH) // �滮����
                {

                    _pGHTOutputFeatClass = p_theFeatureClass; // ��ȡ�滮����ͼ������

                    MeasureGHT(_pGHTOutputFeatClass); // ����滮�������
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

                            text += "  " + arrghmc[i].ToString() + "���:  " + temp.ToString("#0") + " ƽ����" + "\r\n\r\n";
                           

                        }
                        db_ghsj = sumdltb;
                        text += "  �ϼƣ�" + (sumdltb).ToString("#0") + " ƽ���ף�" + (sumdltb * 15 / 10000).ToString("#0.0") + "��Ķ��\r\n\r\n";
                    }
                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY) // ���ع�Ӧ
                {
                    _pTDGYOutputFeatClass = p_theFeatureClass; // ��ȡ���ع�Ӧͼ������

                    MeasureTDGY(_pTDGYOutputFeatClass); // ����ͼ�����

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

                            text += "  " + arrghmc[i].ToString() + "���:  " + temp.ToString("#0") + " ƽ����" + "\r\n\r\n";
                            

                        }
                        db_tdgy = sumdltb;
                        text += "  �ϼƣ�" + sumdltb.ToString("#0") + " ƽ���ף�" + (sumdltb * 15 / 10000).ToString("#0.0") + "��Ķ��\r\n\r\n";
                    }
                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT) // ����ũ��
                {
                    _pJbntOutputFeatClass = p_theFeatureClass; // ��ȡ����ũ��ͼ������

                    double dblJbnt = MeasurePL(_pJbntOutputFeatClass); // ����ͼ�����
                    db_jbnt = dblJbnt;
                    //dblJbnt = dblJbnt / 10000;

                    text += "  ����ũ�������  " + dblJbnt.ToString("#0") + " ƽ���ף�" + (dblJbnt * 15 / 10000).ToString("#0.0") + "��Ķ��\r\n\r\n";

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD) // �����õ�����
                {
                    _pJsydOutputFeatClass = p_theFeatureClass; // ��ȡ�����õ�����ͼ������

                    double dblJsyd = MeasurePL(_pJsydOutputFeatClass); // ����ͼ�����
                    db_jsyd = dblJsyd;
                    //dblJsyd = dblJsyd / 10000;

                    text += "  ���������õ������  " + dblJsyd.ToString("#0") + " ƽ���ף�" + (dblJsyd * 15 / 10000).ToString("#0.0") + "��Ķ��\r\n\r\n"; ;

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH) // �����Դ�滮
                {
                    _pKczyOutputFeatClass = p_theFeatureClass; // ��ȡ�����Դ�滮ͼ������

                    MeasureKCZY(_pKczyOutputFeatClass); // ����ͼ�����

                    double sumdltb = 0;
                    for (int i = 0; i < arrkcmc.Count; i++)
                    {
                        double temp = Convert.ToDouble(dblkctb[i]);
                        sumdltb += temp;
                        text += "  " + arrkcmc[i].ToString() + "���:  " + temp.ToString("#0") + " ƽ����" + "\r\n";


                    }
                    db_kczy = sumdltb;
                    text += "  �ϼƣ�" + sumdltb.ToString("#0") + " ƽ���ף�" + (sumdltb * 15 / 10000).ToString("#0.0") + "��Ķ��\r\n";

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ) // �ɿ�Ȩ
                {
                    _pCkqOutputFeatClass = p_theFeatureClass; // ��ȡ�ɿ�Ȩͼ������

                    double dbCkq = MeasurePL(_pCkqOutputFeatClass);  // ����ͼ�����
                    db_ckq = dbCkq;
                    //dbCkq = dbCkq / 10000;


                    text += "  �ɿ�Ȩ�����  " + dbCkq.ToString("#0") + " ƽ���ף�" + (dbCkq * 15 / 10000).ToString("#0.0") + "��Ķ\r\n\r\n";

                }
                else if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ) // ̽��Ȩ
                {
                    _pTkqOutputFeatClass = p_theFeatureClass; // ��ȡ�ɿ�Ȩͼ������

                    double dbTkq = MeasurePL(_pTkqOutputFeatClass);
                    //double dbTkq =clsMapFunction.MapFunction.GetFeature_Shape_Area( p_theFeatureClass.GetFeature(0));
                    db_tkq = dbTkq;

                    //dbTkq = dbTkq / 10000;

                    //if (dbTkq >= 0.0001)
                    //{
                    text += "  ̽��Ȩ�����  " + dbTkq.ToString("#0") + " ƽ���ף�" + (dbTkq * 15 / 10000).ToString("#0.0") + "��Ķ\r\n\r\n";
                    //}
                    //else
                    //{

                    //    if (dbTkq * 15 >= 0.0001)
                    //    {
                    //        text += "  ̽��Ȩ�����  " + (dbTkq * 15).ToString("#0.0") + " Ķ" + "\r\n\r\n";
                    //    }
                    //    else
                    //    {
                    //        text += "  ̽��Ȩ�����  " + (dbTkq * 10000).ToString("#0") + " ƽ����" + "\r\n\r\n";
                    //    }
                    //}

                }



            }
            else // ���� �޸� 2011���� ���ط���
            {
                text += "����ʧ�ܣ����δ���㣡" + "\r\n\r\n";
            }

            return text;
        }


        #region  ���� �޸� 2011���� ���ط���

        // ��ȡ����·��(���������ݼ���)
        private string GetPathInDataSet(IFeatureLayer theInputFeatureLayer)
        {
            string inputfeatureclasspath ="";
            try
            {
                // ����������ͼ��·��
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

        // ��ȡ����·��(���ݲ������ݼ���)
        private string GetPathNotInDataSet(IFeatureLayer theInputFeatureLayer)
        {
            // ����������ͼ��·��
            //     
            IDataLayer pDatalayer = theInputFeatureLayer as IDataLayer;
            IDatasetName pDataSetname = pDatalayer.DataSourceName as IDatasetName; ;
            IWorkspaceName wsname = pDataSetname.WorkspaceName;
            string pFilePath = wsname.PathName;
            string pFileName = pDataSetname.Name;

            string inputfeatureclasspath = pFilePath + "\\" + pFileName;

            return inputfeatureclasspath;
        }

        // ����ʵ������жϻ�ȡ·���ķ�ʽ
        private string GetInputDataPath(IFeatureLayer theInputFeatureLayer)
        {
            string thePath = "";

            //thePath = GetPathInDataSet(theInputFeatureLayer);
            //if (thePath == null)
            //{
            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}

            // ��ǰ��ͳ������
            JCZF.Renderable.CGlobalVarable.Enum_AnalysisType theType = JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType;


            if (theType == JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ) // ����������״
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


            //if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[0]) // ����������״
            //{
            //    thePath = GetPathInDataSet(theInputFeatureLayer);
            //    if (thePath == null)
            //    {
            //        thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //    }
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[1]) // �滮����
            //{
            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[2]) // ���ع�Ӧ
            //{
            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[3]) // ����ũ��
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[4]) // �����õ�����
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[5]) // �����Դ�滮
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[6]) // �ɿ�Ȩ
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}
            //else if (theType == JCZF.Renderable.CGlobalVarable.m_listStaticsContents[7]) // ̽��Ȩ
            //{

            //    thePath = GetPathNotInDataSet(theInputFeatureLayer);
            //}

            return thePath;
        }


        // ִ�з�������(�����ͼ���Լ����к��ͼ����·��,�Լ����н����)
        private bool ExecuteClip(IFeatureLayer theInputFeatureLayer, string theResultFilePath, string theClipShpName)
        {

            try   // ���в���    
            {

                // ��ȡ���������ݵ�·��
                string inputfeatureclasspath = GetInputDataPath(theInputFeatureLayer);

                // ������н��·��
                string outputfeatureclasspath = System.IO.Path.Combine(theResultFilePath, theClipShpName);

                // ִ�в��в���
                //
                ESRI.ArcGIS.AnalysisTools.Clip clipTool2 = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);//��õķ�ʽ���������� ������·�� m_inputFeatureLayer.FeatureClass, m_ClipFeatureClass, _pDltbOutputFeatClass);
                Geoprocessor gp2 = new Geoprocessor();
                gp2.OverwriteOutput = true;
                // ִ�з��ز��к���
                IGeoProcessorResult result2 = (IGeoProcessorResult)gp2.Execute(clipTool2, null);

                // ����û����s
                // string s = ReturnMessages(gp2);

                //  �жϲ��в����Ƿ�ɹ�
                //
                if (result2 != null && result2.Status == esriJobStatus.esriJobSucceeded) // �����ɹ�
                    return true;
                else // ����
                    return false;
            }
            catch (Exception ex) //  �����쳣
            {
                System.Diagnostics.Debug.WriteLine(String.Format("�����쳣��ԭ��:{0}", ex));
                return false;
            }

        }

        // ִ�з�������
        private string GeoexecuteClip2(IArray theStatisticsLayers)
        {
            string m_strTemp = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[(int)JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType];
            string text = String.Format("{0}ͳ�ƽ����" + "\r\n\r\n", m_strTemp);
            string m_tempText = text;

            // ������н��·��
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa          
            string theResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
            // �����������ݷ���Ŀ¼
            if (!System.IO.Directory.Exists(theResultFilePath))
            {
                System.IO.Directory.CreateDirectory(theResultFilePath);
            }


            // ���в���      
            try
            {

                // ���н�����Ļ���
                string theClipTypeName = "clip";
                // ���к�Ľ��������
                IFeatureClass[] theResultFeatureClass = new IFeatureClass[theStatisticsLayers.Count];
                // ����ͼ����������
                //
                //if (theStatisticsLayers.Count == 1) // ���ֻ��һ��ͼ�㣬���ñ����ʽ
                //{

                //    // ��ȡҪ���е�ͼ��
                //    IFeatureLayer theFeatureLayer = theStatisticsLayers.get_Element(0) as IFeatureLayer;
                //    // ִ�в��з�������   
                //    if (ExecuteClip(theFeatureLayer, theResultFilePath, theClipTypeName))
                //    {
                //        // ���һ���ֶΣ�ͼ�����ƣ�
                //        InsertTheLayerNameField(theResultFilePath, theClipTypeName, theFeatureLayer.Name);

                //        // �򿪲��к�����
                //        IFeatureWorkspace pFWS;
                //        IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                //        pFWS = pWorkspaceFactor.OpenFromFile(theResultFilePath, 0) as IFeatureWorkspace;
                //        theResultFeatureClass[0] = pFWS.OpenFeatureClass(theClipTypeName);//.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                //   }

                //}
                //else // ���ͼ��
                //{
                    // �ϲ�ͼ�������б�
                    ArrayList theLayerNameList = new ArrayList();

                    for (int i = 0; i < theStatisticsLayers.Count; i++)
                    {
                        try
                        {
                            // ��ȡҪ���е�ͼ��
                            IFeatureLayer theFeatureLayer = theStatisticsLayers.get_Element(i) as IFeatureLayer;
                            // ������к�������
                            string theClipShpName = String.Format("{0}{1}", theClipTypeName, i + 1);

                           // IFeature m_ddd = theFeatureLayer.FeatureClass.GetFeature(0);

                            // ִ�в��з�������
                            //
                            if (ExecuteClip(theFeatureLayer, theResultFilePath, theClipShpName)) // ����ɹ������ͼ������
                            {
                                // ���һ���ֶΣ�ͼ�����ƣ�
                                InsertTheLayerNameField(theResultFilePath, theClipShpName, theFeatureLayer.Name);
                                theLayerNameList.Add(theClipShpName);

                                //        // �򿪲��к�����
                                IFeatureWorkspace pFWS;
                                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                                pFWS = pWorkspaceFactor.OpenFromFile(theResultFilePath, 0) as IFeatureWorkspace;
                                theResultFeatureClass[i] = pFWS.OpenFeatureClass(theClipShpName);//.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                                // �õ�ͳ�ƽ��
                                if (theResultFeatureClass != null)
                                {

                                    //��Shape_Lenght��Shape_Area���¸�ֵ




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
                                            text += "\n\n  δռ�ø����õأ�\r\n\r\n";
                                        }
                                    }
                                    catch(SystemException errs)
                                    {
                                         text += "\n\n  δռ�ø����õأ�\r\n\r\n";
                                    }

                                }
                                else
                                {
                                    text += "\n\n  δռ�ø����õأ�\r\n\r\n";
                                }


                            }
                        }
                        catch(SystemException errs )
                        {
                            clsFunction.Function.MessageBoxError(errs.Message);
                            continue;
                        }
                    //}

                    //// �����⣬���ֻ��һ���ļ������޷��ϲ�������Ҳ�Ĳ����������������shp�ļ�û���ҵ�����
                    //// �ϲ����ͼ�㵽һ��ͼ��
                    //if (theLayerNameList.Count > 0)
                    //{
                    //    theResultFeatureClass = MergeSomeLayersIntoOneLayer(theResultFilePath, theLayerNameList, theClipTypeName);
                    //}

                   
                }

                    if (m_tempText == text)
                    {
                        text += "\n\n  δռ�ø����õأ�\r\n\r\n";
                    }
              
                return text;
            }
            catch (Exception ex)
            {
                return text += ex.Message + "����������" + "\r\n\r\n";
            }
        }

        #endregion

        ///����ͼ�������ͼ�߽��в������� m_Geometry ����û���õ�
        private string GeoexecuteClip(IGeometry m_Geometry, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "����������״ͳ�ƽ����" + "\r\n\r\n";
            try
            {
                //���н��·��
                string m_strResultFilePath = _m_strFilePath + "\\overlay" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }

                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;


                // ���в���
                //����û�õ�һ��
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

                ESRI.ArcGIS.AnalysisTools.Clip clipTool2 = new ESRI.ArcGIS.AnalysisTools.Clip(inputfeatureclasspath, m_ClipFeatureClass, outputfeatureclasspath);//��õķ�ʽ���������� ������·�� m_inputFeatureLayer.FeatureClass, m_ClipFeatureClass, _pDltbOutputFeatClass);

                // ���ز��к���
                IGeoProcessorResult result2 = (IGeoProcessorResult)gp2.Execute(clipTool2, null);

                // ����û����s
                string s = ReturnMessages(gp2);
                // �򿪲��к�����
                _pDltbOutputFeatClass = pFWS.OpenFeatureClass("clip");//.CreateFeatureClass("clip", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


                //string text =  "����������״��"+"\r\n"+"  ͳ�ƽ����" + "\r\n";
                if (result2.Status == esriJobStatus.esriJobSucceeded) // �����ɹ�
                {
                    MeasureDltb(_pDltbOutputFeatClass); // ����ͼ�����

                    double sumdltb = 0;
                    for (int i = 0; i < arrdlmc.Count; i++)
                    {
                        //double temp = Convert.ToDouble(dbldltb[i]) / 10000;
                        double temp = Convert.ToDouble(dbldltb[i]);
                        text += "  " + arrdlmc[i].ToString() + "���:  " + temp.ToString("#0.000") + " ƽ����" + "\r\n\r\n";
                        sumdltb += Convert.ToDouble(dbldltb[i]) ;
                    }
                    text += "  �ϼƣ�" + sumdltb.ToString("#0") + " ƽ����" + (sumdltb * 15 / 10000).ToString("#0.0") + " Ķ\r\n\r\n";
                }
                else // ���� �޸� 2011���� ���ط���
                {
                    text += "���в���ʧ�ܣ��޽��";
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "û��ͳ�ƽ��" + "\r\n\r\n";
            }
        }


        //����ͼ�������ũ����в�������
        private string JBNTGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "����ũ��ͳ�ƽ����" + "\r\n\r\n";
            try
            {
                //���н��·��
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


                //string text = "����ũ�" + "\r\n" + "  ͳ�ƽ����" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    double dblJbnt = MeasurePL(_pJbntOutputFeatClass);
                    //dblJbnt = dblJbnt / 10000;
                    db_jbnt = dblJbnt;
                    //text = "  ͳ�ƽ����" + "\r\n";
                    text += "  ����ũ�������  " + dblJbnt.ToString("#0") + " ƽ����" + "\r\n\r\n";
                    //text += "  ����ũ�������  " + dblJbnt + " ƽ����" + "\r\n";                    
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "û��ͳ�ƽ��" + "\r\n\r\n";
            }

        }

        //����ͼ���뽨���õؽ��в�������
        private string JSYDGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "���������õ�ͳ�ƽ����" + "\r\n\r\n";
            try
            {
                //���н��·��
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

                //string text = "�����õأ�" + "\r\n" + "  ͳ�ƽ����" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    double dblJsyd = MeasurePL(_pJsydOutputFeatClass);
                    //dblJsyd = dblJsyd / 10000;

                    db_jsyd = dblJsyd;
                    //m_frmZhhcResult.txtJSYD.Text = "  �����õ������  " + dblJsyd + " ƽ����";
                    text += "  ���������õ������  " + dblJsyd.ToString("#0") + "ƽ����" + "\r\n\r\n"; ;
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "û��ͳ�ƽ��" + "\r\n\r\n";
            }

        }


        ///// <summary>
        ///// ����ͼ���뽨���õؽ��в�������
        ///// </summary>
        ///// <param name="m_Geometry"></param>
        ///// <param name="m_selLayer"></param>
        ///// <param name="m_inputFeatureLayer"></param>
        ///// <returns></returns>
        //private double  JSYDGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        //{
        //    double m_dblJsyd = 0;
        //    string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
        //    string text = "���������õ�ͳ�ƽ����" + "\r\n\r\n";
        //    try
        //    {
        //        //���н��·��
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

        //        //string text = "�����õأ�" + "\r\n" + "  ͳ�ƽ����" + "\r\n";
        //        if (result.Status == esriJobStatus.esriJobSucceeded)
        //        {
        //            m_dblJsyd = MeasurePL(_pJsydOutputFeatClass);
        //            m_dblJsyd = m_dblJsyd / 10000;

        //            db_jsyd = m_dblJsyd;
        //            //m_frmZhhcResult.txtJSYD.Text = "  �����õ������  " + dblJsyd + " ƽ����";
        //            text += "  ���������õ������  " + dblJsyd.ToString("#0.000") + "����" + "\r\n\r\n"; ;
        //        }
        //        return text;
        //    }
        //    catch (Exception ex)
        //    {
        //        return text + "û��ͳ�ƽ��" + "\r\n\r\n";
        //    }

        //}

        //����ͼ����̽��Ȩ���в�������
        private string TKQGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "̽��Ȩͳ�ƽ����" + "\r\n\r\n";
            try
            {
                //���н��·��
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
                //string text = "̽��Ȩ��" + "\r\n" + "  ͳ�ƽ����" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    double dblJsyd = MeasurePL(_pTkqOutputFeatClass);
                    //dblJsyd = dblJsyd / 10000;
                    db_tkq = dblJsyd;
                    //m_frmZhhcResult.txtJSYD.Text = "  �����õ������  " + dblJsyd + " ƽ����";
                    text += "  ̽��Ȩ�����  " + dblJsyd.ToString("#0") + " ƽ����" + "\r\n\r\n";
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "û��ͳ�ƽ��" + "\r\n\r\n";
            }

        }

        //����ͼ����ɿ�Ȩ���в�������
        private string CKQGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "�ɿ�Ȩͳ�ƽ����" + "\r\n\r\n";
            try
            {
                //���н��·��
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

                //string text = "�ɿ�Ȩ��" + "\r\n" + "  ͳ�ƽ����" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    double dblJsyd = MeasurePL(_pCkqOutputFeatClass);
                    //dblJsyd = dblJsyd / 10000;

                    db_ckq = dblJsyd;
                    //m_frmZhhcResult.txtJSYD.Text = "  �����õ������  " + dblJsyd + " ƽ����";
                    text += "  �ɿ�Ȩ�����  " + dblJsyd.ToString("#0") + " ƽ����" + "\r\n\r\n";
                }

                return text;
            }
            catch (Exception ex)
            {
                return text + "û��ͳ�ƽ��" + "\r\n\r\n";
            }

        }

        //����ͼ����ɿ�Ȩ���в�������//////����ũ��
        private string CKQGeoexecuteClip1(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "�ɿ�Ȩͳ�ƽ������ũ�" + "\r\n\r\n";
            try
            {
                //���н��·��
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

                    //string text = "�ɿ�Ȩ��" + "\r\n" + "  ͳ�ƽ����" + "\r\n";
                    if (result.Status == esriJobStatus.esriJobSucceeded)
                    {
                        double dblJsyd = MeasurePL(_pCkqOutputFeatClass);
                        //dblJsyd = dblJsyd / 10000;

                        db_ckq = dblJsyd;
                        //m_frmZhhcResult.txtJSYD.Text = "  �����õ������  " + dblJsyd + " ƽ����";
                        text += "  �ɿ�Ȩ�������ũ�  " + dblJsyd.ToString("#0") + " ƽ����" + "\r\n\r\n";
                    }

                }
                else
                {
                    text += "û��ͳ�ƽ��" + "\r\n\r\n";
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "û��ͳ�ƽ��" + "\r\n\r\n";
            }

        }


        //����ͼ����滮ͼ���в�������
        private string QHTGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString(); //aa
            string text = "�������ù滮����ͳ�ƽ����" + "\r\n\r\n";
            text += "\r\n\r";
            try
            {
                //���н��·��
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

                //string text = "�滮ͼ��" + "\r\n" + "  ͳ�ƽ����" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    MeasureGHT(_pGHTOutputFeatClass);

                    double sumdltb = 0;
                    for (int i = 0; i < arrghmc.Count; i++)
                    {
                        //double temp = Convert.ToDouble(dblghtb[i]) / 10000;
                        double temp = Convert.ToDouble(dblghtb[i]) ;
                        text += "  " + arrghmc[i].ToString() + "���:  " + temp.ToString("#0") + " ƽ����" + "\r\n\r\n";
                        //text += "\r\n\r\n";
                        sumdltb += Convert.ToDouble(dblghtb[i]);
                        //sumdltb = sumdltb / 10000;
                    }
                    db_ghsj = sumdltb;
                    text += "  �ϼƣ�" + sumdltb.ToString("#0") + " ƽ����" + (sumdltb * 15 / 10000).ToString("#0.0") + " Ķ\r\n\r\n";

                }
                return text;
            }
            catch (Exception ex)
            {
                text += "û��ͳ�ƽ��" + "\r\n\r\n";

                return text;
            }

        }


        /// <summary>
        /// ����ͼ�������ع�Ӧͼ���в�������
        /// </summary>
        /// <param name="m_Geometry"></param>
        /// <param name="m_selLayer"></param>
        /// <param name="m_inputFeatureLayer"></param>
        /// <returns></returns>
        private string TDGYGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString(); //aa
            string text = "���ع�Ӧ����ͳ�ƽ����" + "\r\n\r\n";
            text += "\r\n\r";
            try
            {
                //���н��·��
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

                //string text = "�滮ͼ��" + "\r\n" + "  ͳ�ƽ����" + "\r\n";
                if (result.Status == esriJobStatus.esriJobSucceeded)
                {
                    MeasureTDGY(_pTDGYOutputFeatClass);

                    double sumdltb = 0;
                    for (int i = 0; i < arrghmc.Count; i++)
                    {
                        //double temp = Convert.ToDouble(dblgdtb[i]) / 10000;
                        double temp = Convert.ToDouble(dblgdtb[i]) ;
                        text += "  " + arrghmc[i].ToString() + "���:  " + temp.ToString("#0") + " ƽ����" + "\r\n\r\n";
                        //text += "\r\n\r\n";
                        sumdltb += Convert.ToDouble(dblgdtb[i]);
                        //sumdltb = sumdltb / 10000;
                    }
                    db_tdgy = sumdltb;
                    text += "  �ϼƣ�" + sumdltb.ToString("#0") + " ƽ����" + (sumdltb * 15 / 10000).ToString("#0.0") + " Ķ\r\n\r\n";

                }
                return text;
            }
            catch (Exception ex)
            {
                text += "û��ͳ�ƽ��" + "\r\n\r\n";

                return text;
            }

        }






        //����ͼ��������Դͼ���в�������
        private string KCZYGeoexecuteClip(IGeometry m_Geometry, ILayer m_selLayer, IFeatureLayer m_inputFeatureLayer)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
            string text = "�����Դ�滮ͳ�ƽ����" + "\r\n";
            try
            {
                //���н��·��
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
                        text += "  " + arrkcmc[i].ToString() + "���:  " + temp.ToString("#0") + " ƽ����\r\n";
                        sumdltb += Convert.ToDouble(dblkctb[i]);
                        //sumdltb = sumdltb / 10000;
                    }
                    db_kczy = sumdltb;
                    text += "  �ϼƣ�" + sumdltb.ToString("#0") + " ƽ����" + (sumdltb * 15 / 10000).ToString("#0.0") + " Ķ\r\n";
                }
                return text;
            }
            catch (Exception ex)
            {
                return text + "û��ͳ�ƽ��" + "\r\n";
            }

        }


        //����ѡ��GeoFeature ������inputfeatureclass
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





        //����ѡ��feature ������inputfeatureclass
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

        //����ͼ���������
        public void MeasureDltb(IFeatureClass  outfeatureclass)
        {
            ////�ж����ֵ���ͼ��
            IFeatureCursor pCursor = null;
            ArrayList arr = new ArrayList();
            //ArrayList arrdlmc = new ArrayList();
            string m_strFieldName = "DLbM";
            int indexofDLDM = outfeatureclass.FindField(m_strFieldName);
            int indexofDLMC = outfeatureclass.FindField("DLMC");
            if (indexofDLDM == -1)
            {
                m_strFieldName = "DLDM";
                indexofDLDM = outfeatureclass.FindField(m_strFieldName);//���������仯

            }
            if (indexofDLDM == -1 || indexofDLMC == -1)
            {
                _strHCJG = "�ǳ���Ǹ������������״ͼ������ݽṹ������Ҫ���޷����м��㣡";
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
                    //ÿ�ֵ���ͼ�߽����������                
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

        //�滮ͼ�������
        public void MeasureGHT(IFeatureClass outfeatureclass)
        {
            ////�ж����ֹ滮����
            IFeatureCursor pCursor = null;
            ArrayList arr = new ArrayList();
            //ArrayList arrdlmc = new ArrayList();
            int indexofDLDM = outfeatureclass.FindField("FQMC");
            int indexofDLMC = outfeatureclass.FindField("FQMC");
            if (indexofDLDM == -1 || indexofDLMC == -1)
            {
                _strHCJG="�ǳ���Ǹ���滮ͼ������ݽṹ������Ҫ���޷����м��㣡";
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
                    //ÿ�ֵ���ͼ�߽����������                
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
        /// ���ع�Ӧ�������
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
                _strHCJG = "�ǳ���Ǹ�����ع�Ӧͼ������ݽṹ������Ҫ���޷����м��㣡";
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
                    //ÿ�ֵ���ͼ�߽����������                
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
        //�����Դͼ�������
        public void MeasureKCZY(IFeatureClass outfeatureclass)
        {
            ////�ж����ֵ���ͼ��
            IFeatureCursor pCursor = null;
            ArrayList arr = new ArrayList();
            //ArrayList arrdlmc = new ArrayList();
            int indexofDLDM = outfeatureclass.FindField("GHQLB1");
            int indexofDLMC = outfeatureclass.FindField("GHQMC1");
            if (indexofDLDM == -1 || indexofDLMC == -1)
            {
                _strHCJG = "�ǳ���Ǹ�������Դͼ������ݽṹ������Ҫ���޷����м��㣡";
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
                    //ÿ�ֵ���ͼ�߽����������                
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



        #region  �������

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
