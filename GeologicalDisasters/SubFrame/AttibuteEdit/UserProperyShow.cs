using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;




using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesGDB;

using Functions;

namespace JCZF.SubFrame
{
    public partial class UserProperyShow : UserControl
    {

        private ArrayList m_pArrayListOfFeatures;
        private IMap m_pCurrentMap;
        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl;
        //private frmMapView m_frmMapView;

        public delegate void FlashFeatureEventHandler(IFeature pFeature);
        public event FlashFeatureEventHandler FlashFeature;

        public delegate bool SaveStatisticsInfoEventHandler(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType  p_type, DataTable theInfoTable);
        public event SaveStatisticsInfoEventHandler Event_theSaveStatisticsInfo;

        public clsDataAccess.DataAccess m_theDataAccess;// ���� �޸� 2011 ����
        public string m_strDKID;

        public JCZF.Renderable.CGlobalVarable.Enum_AnalysisType  m_theType;// �������� ���� �޸� 2011 ����

        public string m_theCurrentLayerName = null;// ������ͼ�����ƣ����� �޸� 2011 ����

        public UserProperyShow(IMap pMap)
        {
            this.m_pCurrentMap = pMap;

            InitializeComponent();
        }

        public UserProperyShow(ESRI.ArcGIS.Controls.AxMapControl p_AxMapControl, string name)
        {
            InitializeComponent();
            m_AxMapControl = p_AxMapControl;
            this.m_pCurrentMap = p_AxMapControl.ActiveView.FocusMap;
            this.Text += ":" + name;
        }

        public UserProperyShow(ESRI.ArcGIS.Controls.AxMapControl p_AxMapControl, IMap pMap)
        {
            InitializeComponent();
            m_AxMapControl = p_AxMapControl;
            this.m_pCurrentMap = p_AxMapControl.ActiveView.FocusMap;            
        }
        // ���� �޸� 2011 ����
        // ���ݹؼ��ֵ�ֵ�õ�feature���Ŵ�
        private void GetFeatureAndZoom(string theKeyVal, string theLayerName)
        {
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);
            string theKeyName = JCZF.Renderable.CGlobalVarable.m_listFeatureIDFieldNameOfStaticsLayer[indexOfLayer];
            IFeature pFeature = this.GetOneFeatureWithID(theKeyName, theKeyVal.Trim(), theLayerName.Trim());
            if (pFeature == null)
            {
                if (theKeyName.ToUpper() == "OBJECTID")
                {
                    theKeyName = "FID";
                    pFeature = this.GetOneFeatureWithID(theKeyName, theKeyVal.Trim(), theLayerName.Trim());
                }
            }
            if (pFeature!= null)
                //MapFunction.ZoomAndFlashFeature(pFeature, m_AxMapControl);//������˸����Ҫ�ǵ���˸�����ݵ�����ϵ���ͼ����ϵ��һ��ʱ�����λ��ƫ�� 20110920����ΰ
                MapFunction.ZoomToFeature(pFeature, m_AxMapControl);      //����ѡ����GetOneFeatureWithID ��ѡ�С�
        }

        private void m_PropertyDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                #region yuejianwei �޸� 20130326 �������괮��λ

              string m_strCoordinate=  this.m_PropertyDataGrid.CurrentRow.Cells["����"].Value.ToString().Trim(); //��ȡѡ�м�¼������
                string[] m_strCoordinate2=m_strCoordinate.Split(new char[] { ';' });


                object missing = Type.Missing;
                
                IPoint pt1 = null;
                IPoint pt2 = null;
                Polygon m_IPolygon = new Polygon();

                for (int i = 0; i < m_strCoordinate2.Length ; i++)
                {
                    string[] split1 = m_strCoordinate2[i].ToString().Split(new char[] { ',' });
                   

                    double X1 = Convert.ToDouble(split1[0].ToString());
                    double Y1 = Convert.ToDouble(split1[1].ToString());
                   

                    pt1 = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                    pt1.PutCoords(X1, Y1);




                    m_IPolygon.AddPoint(pt1, ref missing, ref missing);
                    
                }

                MapFunction.DrawTempPolygonOnMap((IGeometry)m_IPolygon, m_AxMapControl);
                clsMapFunction.MapFunction.ZoomTOGeometry((IGeometry)m_IPolygon, m_AxMapControl);
                

                return;
                #endregion 

                //if (e.RowIndex < 0 || e.ColumnIndex != -1)
                //{
                //    return;
                //}

                //if (this.m_pArrayListOfFeatures == null || this.m_pArrayListOfFeatures.Count == 0)
                //{
                //    return;
                //}

                //if (e.RowIndex + 1 == this.m_PropertyDataGrid.RowCount)
                //{
                //    return;
                //}

                #region ���� �޸� 2011 ����

                //// ��ȡ�����ID����
                //int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);
                //string colName = JCZF.Renderable.CGlobalVarable.m_listColNameNameOfStaticsInfoTable[indexOfLayer];

                //DataGridViewCell pDGVC = this.m_PropertyDataGrid.CurrentRow.Cells[colName]; //��ȡѡ��ID��Ԫ��
                //GetFeatureAndZoom(pDGVC.Value.ToString()); // ����IDֵ�õ�feature���Ŵ�

                // ��ȡ�����ID����
                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);
                string colName = JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoTable[indexOfLayer];

                DataGridViewCell pDGVC = this.m_PropertyDataGrid.CurrentRow.Cells[colName]; //��ȡѡ��ID��Ԫ��

                // ���� �޸� 2011���� ����
                if (pDGVC == null) // û�������򷵻�
                    return;

                // ��ȡID
                string theID = pDGVC.Value.ToString().Trim();

                // ���� �޸� 2011���� ����
                colName = JCZF.Renderable.CGlobalVarable.m_strFieldNameOfLayerInSQLTable;// ����ͼ������

                pDGVC = this.m_PropertyDataGrid.CurrentRow.Cells[colName]; //��ȡ����ͼ������

                if (pDGVC == null) // û�������򷵻�
                    return;

                // ��ȡ����ͼ������
                string theLayerName = pDGVC.Value.ToString().Trim();

                GetFeatureAndZoom(theID, theLayerName); // ����IDֵ�õ�feature���Ŵ�


                #endregion

                #region  ԭʼ ���� �޸� 2011 ����

                //DataGridViewCell pDGVC;//= this.m_PropertyDataGrid.CurrentRow.Cells["OBJECTID"];

                //try
                //{
                //    pDGVC = this.m_PropertyDataGrid.CurrentRow.Cells["OBJECTID"];
                //}
                //catch
                //{
                //    pDGVC = this.m_PropertyDataGrid.CurrentRow.Cells["FID"];

                //}

                //int FeatureOID = Convert.ToInt32(pDGVC.Value);

                //IFeature pFeature = null;

                //for (int i = 0; i < this.m_pArrayListOfFeatures.Count; i++)
                //{
                //    pFeature = (IFeature)this.m_pArrayListOfFeatures[i];
                //    if (Convert.ToInt32(pFeature.get_Value(0)) == FeatureOID)
                //    {
                //        //this.ZoomAndFlashFeature(pFeature);
                //        //this.ZoomToFeature(pFeature);
                //        MapFunction.ZoomAndFlashFeature(pFeature, m_AxMapControl);
                //    }
                //}

                #endregion

            }
            catch(SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
        }

        #region // ���� �޸� 2011 ����

        //
        /// <summary>
        ///  ����ͼ��������ҵ����ݿ�����ʺϵ�����
        /// </summary>
        /// <param name="theCol"></param>
        /// <returns></returns>
        private string GetFitableColName(string theCol)
        {
            return JCZF.Renderable.CGlobalVarable.GetFitalbeColName(this.m_theType, theCol);
        }

        // �����ѯ���
        public string searchQueryFilter(string fldName, string fldValue, bool SearchVague, string p_esriFieldType)
        {
            string strWhere;
            
            if (SearchVague)
            {
                strWhere = string.Format("{0} like '%{1}%'", fldName, fldValue);
            }
            else
            {
                fldName = "\"" + fldName + "\"";
                if (p_esriFieldType == esriFieldType.esriFieldTypeString.ToString() )
                {
                    strWhere = string.Format("{0} = '{1}'", fldName, fldValue);
                }
                else
                {
                    strWhere = string.Format("{0} = {1}", fldName, fldValue);
                }
                //strWhere = '"' + fldName + '"' + "='" +fldValue+ "'";
               // strWhere = '"'+fldName'" = 'California'


              
            }
            return strWhere;
        }


        //�����ֶκͺ��ֶ�ֵѡ��Ҫ��
        public IFeature GetOneFeature(IFeatureLayer pFeatureLayer, string pFieldName, string pFieldValue)
        {
            IFeature pFeature = null;
            try
            {
                IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;

                IQueryFilter pQueryFilter = new QueryFilter() as IQueryFilter ;
                 
                string m_esriFieldType= pFeatureClass.Fields.get_Field(pFeatureClass.FindField(pFieldName)).Type.ToString();
                pQueryFilter.WhereClause = searchQueryFilter(pFieldName, pFieldValue, false, m_esriFieldType);
                IFeatureCursor pFeatureCursor = null;
                //����Ҫ�� ������Ϊѡ��
                
                pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);


                pFeature = pFeatureCursor.NextFeature();
            }
            catch(SystemException errs)
            {

            }
            return pFeature;
        }

        //���е�Featurelayer
        public IEnumLayer GetLayers(IMap pMap)
        {
            UID pUid = new UID();
            //pUid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";
            //pUid.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
            pUid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";

            return pMap.get_Layers(pUid, true);
        }


        /// <summary>
        /// ���ݲ���ͼ�����õ�ͼ��
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
       public FeatureLayer GetFeatureLayerByName(String partName, AxMapControl m_axMapControl)
        {


            FeatureLayer qryFeatLayer = null;

            IEnumLayer m_Layers = GetLayers(m_axMapControl.ActiveView.FocusMap);

            ILayer pLayer = m_Layers.Next();
            while (pLayer != null)
            {

                //�ж�qrylayer�Ƿ�ΪFeatureLayer����
                if ((pLayer is FeatureLayer) && pLayer.Name.Equals(partName))
                {
                    qryFeatLayer = (FeatureLayer)pLayer;
                    break;
                }

                pLayer = m_Layers.Next();
            }

            m_Layers.Reset();

            return qryFeatLayer;
        }

        // ����ID��ȡFeature 
       public IFeature GetOneFeatureWithID(string pFieldName, string pFieldValue, string theLayerName)
        {
           // int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);

           // //��ȡ���������ͼ�㣬Ӧ�û�ȡ���н����ͼ����ж�λ ����20110814
           // //m_theCurrentLayerName = JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer]; // ��ȡͼ������
            
           // m_theCurrentLayerName = JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer]; // ��ȡͼ������

           //IFeatureLayer theFLayer = GetFeatureLayerByName(m_theCurrentLayerName, this.m_AxMapControl); // ��ȡͼ��
           // IFeature m_IFeature=GetOneFeature(theFLayer, pFieldName, pFieldValue);// �ҵ�Feature

           // m_AxMapControl.Map.ClearSelection();
           // IFeatureSelection m_FeatureSelection = theFLayer as IFeatureSelection;
           // m_FeatureSelection.Add(m_IFeature);

           // //m_AxMapControl.ActiveView.Selection.Clear();
           // //m_AxMapControl.Map.SelectFeature(theFLayer ,m_IFeature);
            
           // return m_IFeature;

            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);

            //��ȡ���������ͼ�㣬Ӧ�û�ȡ���н����ͼ����ж�λ ����20110814
            //m_theCurrentLayerName = JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer]; // ��ȡͼ������

            m_theCurrentLayerName = JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer]; // ��ȡͼ������


            // ���� �޸� 2011���� ����
            if (m_theCurrentLayerName.Trim() == "")
            {
                if (theLayerName.Trim() == "")
                    return null;

            }

            IFeatureLayer theFLayer = GetFeatureLayerByName(m_theCurrentLayerName, this.m_AxMapControl); // ��ȡͼ��

            // ���� �޸� 2011���� ����
            if (theFLayer == null)
            {
                theFLayer = GetFeatureLayerByName(theLayerName, this.m_AxMapControl); // ��һ�λ�ȡͼ��

                if (theFLayer == null)
                    return null;
            }



            IFeature m_IFeature = GetOneFeature(theFLayer, pFieldName, pFieldValue);// �ҵ�Feature

            // ���� �޸� 2011���� ����
            if (m_IFeature == null)
                return null;

            // ���� �޸� 2011���� ���� Ϊ�˿���ͼ���ѡ��
            if (theFLayer.Visible == false)
            {
                // ����ͼ����ʾ
                theFLayer.Visible = true;

                // ��ȡ��ͳ����û�б���ʾ��ͼ�����ƣ��ȵ��رյ�ʱ��ȫ������ʾ
                JCZF.Renderable.CGlobalVarable.m_theListOfLayerNamesInNonShowInStatistics.Add(theFLayer.Name);
            }

            m_AxMapControl.Map.ClearSelection();
            IFeatureSelection m_FeatureSelection = theFLayer as IFeatureSelection;
            m_FeatureSelection.Add(m_IFeature);

            //m_AxMapControl.ActiveView.Selection.Clear();
            //m_AxMapControl.Map.SelectFeature(theFLayer ,m_IFeature);

            return m_IFeature;
        }

        #endregion


       private void DisplayPropertyForm(ILayer layer)
       {
           try
           {
               ITable pTable = (ITable)layer;

               ICursor pCursor = pTable.Search(null, true);

               IFields pFields = pCursor.Fields;

               IRow pRow = pCursor.NextRow();

               ArrayList FieldsName = new ArrayList();

               DataTable pDataTable = new DataTable("Property");

               DataColumn pDataColumn;
               DataRow pDataRow;

               // ѭ������  ���� �޸� 2011 ����
               string[] theLayerColList = JCZF.Renderable.CGlobalVarable.GetFitableLayerColNameListOfStatisticsInfo(this.m_theType); // ��ȡͼ�����������
               foreach (string theLayerColName in theLayerColList)
               {

                   string colName = GetFitableColName(theLayerColName); // ��ȡ���ݿ������� 

                   //ÿ���ֶ�Ϊһ��
                   pDataColumn = new DataColumn();

                   pDataColumn.ColumnName = colName;
                   pDataColumn.ReadOnly = true;
                   pDataTable.Columns.Add(pDataColumn);

                   //�ֶ�����
                   FieldsName.Add(theLayerColName);
               }



               // ����������
               //for (int i = 0; i < pFields.FieldCount; i++)
               //{

               //    IField pField = pFields.get_Field(i);

               //    if (pField.Type != esriFieldType.esriFieldTypeGeometry)
               //    {
               //        //ÿ���ֶ�Ϊһ��
               //        pDataColumn = new DataColumn();

               //        string colName = GetFitableColName(pField.Name); // ��ȡ���� ���� �޸� 2011 ����
               //        if (colName != "") // ���� �޸� 2011 ����
               //        {
               //            pDataColumn.ColumnName = colName;
               //            pDataColumn.ReadOnly = true;
               //            pDataTable.Columns.Add(pDataColumn);
               //            //�ֶ�����
               //            FieldsName.Add(pField.Name);
               //        }
               //    }

               //}

               while (pRow != null)
               {
                   if (pRow.HasOID == false) continue;
                   pDataRow = pDataTable.NewRow();
                   int index = 0;
                   //��ȡ��Ӧ�ֶε�ֵ
                   for (int i = 0; i < FieldsName.Count; i++)
                   {
                       string FieldName = (string)FieldsName[i];

                       index = pRow.Fields.FindField(FieldName);

                       string colName = GetFitableColName(FieldName); // ��ȡ���� ���� �޸� 2011 ����
                       if (FieldName.ToUpper() == "DKID" )
                       {
                           pDataRow[colName] = m_strDKID;
                       }
                       else
                       {
                           if (FieldName.ToUpper() == "ZB" || FieldName.Trim() == "����" )
                           {
                               pDataRow[colName] = GetCoordinate(pRow);
                           }
                           else if (FieldName.ToLower() == "shape_area" && index == -1)
                           {//9.2�汾��Ϊ��shape_area����9.3�汾��Ϊ��area��
                               FieldName = "area";
                               index = pRow.Fields.FindField(FieldName);
                           }
                           if (FieldName.ToLower() == "shape_area")
                           {
                               pDataRow[colName] = ((double)pRow.get_Value(index)).ToString("#0.00");
                           }
                           else
                           {
                               if (index == -1)
                               {
                                   continue;
                               }
                               pDataRow[colName] = pRow.get_Value(index).ToString();
                           }
                       }
                   }

                   pDataTable.Rows.Add(pDataRow);
                   pRow = pCursor.NextRow();
               }

               this.m_PropertyDataGrid.DataSource = pDataTable;

               TransToChinese(this.m_theType);//��Ӣ������תΪ��������


               Event_theSaveStatisticsInfo(this.m_theType, pDataTable); //����ͳ����Ϣ���� ���� �޸� 2011 ����

           }
           catch (Exception ee) // ���� �޸� 2011 ����
           { 
           }

       }

        /// <summary>
        /// ��dataGrid���б�ת��Ϊ��������
        /// </summary>
       private void TransToChinese(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_Enum_AnalysisType)
       {
           string[] m_strTemp_Chinese=null;
           m_strTemp_Chinese=JCZF.Renderable.CGlobalVarable.GetTableColNameListOfStatisticsInfo_Chinese(p_Enum_AnalysisType);
            string[] m_strTemp=null;
           m_strTemp=JCZF.Renderable.CGlobalVarable.GetTableColNameListOfStatisticsInfo(p_Enum_AnalysisType);

           for (int i = 0; i < m_PropertyDataGrid.Columns.Count; i++)
           {
               for (int j = 0; j < m_strTemp.Length; j++)
               {
                   if (this.m_PropertyDataGrid.Columns[i].Name == m_strTemp[j])
                   {
                       this.m_PropertyDataGrid.Columns[i].HeaderText = m_strTemp_Chinese[j];
                       this.m_PropertyDataGrid.Columns[i].Name  = m_strTemp_Chinese[j];
                   }
               }
           }
       }

       public void ShowDataFromDatabase(DataTable p_DataTable)
       {
           this.m_PropertyDataGrid.DataSource = p_DataTable;
       }

       private string GetCoordinate(IRow p_IRow)
        {
            string m_strCoordinate = "";
            try
            {
                IFeature m_IFeature = (IFeature)p_IRow;

                //IFeatureDataset m_IFeatureDataset=m_IFeatureLayer.FeatureClass..FeatureDataset;
                //IFeature m_IFeature = (IFeature)m_IFeatureLayer.FeatureClass.GetFeature(0);
                IGeometry m_IGeometry = m_IFeature.Shape;


                IPointCollection m_IPointCollection = m_IGeometry as IPointCollection;

                for (int i = 0; i < m_IPointCollection.PointCount; i++)
                {
                    IPoint m_IPoint = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                    m_IPointCollection.QueryPoint(i, m_IPoint);

                    m_strCoordinate = m_strCoordinate + m_IPoint.X.ToString() + ",";

                    m_strCoordinate = m_strCoordinate + m_IPoint.Y.ToString() + ";";

                }

                if (m_strCoordinate.Length > 0)
                {
                   m_strCoordinate= m_strCoordinate.Remove(m_strCoordinate.Length - 1);//ȥ�����ġ�;��
                }
            }
            catch(SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
            return m_strCoordinate;
        }

        private void DisplayPropertyForm(IFeatureCursor pFeatureCursor)
        {
            IFields pFields = pFeatureCursor.Fields;

            IFeature pFeature = pFeatureCursor.NextFeature();

            ArrayList pArrayOfFieldName = new ArrayList();
            DataTable pDataTable = new DataTable("Property");
            DataColumn pDataColumn;
            DataRow pDataRow;

            for (int i = 0; i < pFields.FieldCount; i++)
            {
                IField pField = pFields.get_Field(i);

                if (pField.Type != esriFieldType.esriFieldTypeGeometry)
                {
                    //ÿ���ֶ�Ϊһ��
                    pDataColumn = new DataColumn();
                    pDataColumn.ColumnName = pField.Name;
                    pDataColumn.ReadOnly = true;
                    pDataTable.Columns.Add(pDataColumn);
                    //�ֶ�����
                    pArrayOfFieldName.Add(pField.Name);
                }
            }

            while (pFeature != null)
            {
                pDataRow = pDataTable.NewRow();
                int index = 0;
                //��ȡ��Ӧ�ֶε�ֵ
                for (int j = 0; j < pArrayOfFieldName.Count; j++)
                {
                    string FieldName = (string)pArrayOfFieldName[j];
                    index = pFeature.Fields.FindField(FieldName);
                    pDataRow[FieldName] = pFeature.get_Value(index).ToString();
                }

                pDataTable.Rows.Add(pDataRow);
                pFeature = pFeatureCursor.NextFeature();
            }

            this.m_PropertyDataGrid.DataSource = pDataTable;
        }

        private void DisplayPropertyForm(ArrayList pArrayListOfFeatures)
        {

            if (pArrayListOfFeatures.Count == 0)
            {
                return;
            }

            IFeature pFeature = (IFeature)pArrayListOfFeatures[0];
            IFields pFields = pFeature.Fields;
            ArrayList pArrayOfFieldName = new ArrayList();
            DataTable pDataTable = new DataTable("Property");
            DataColumn pDataColumn;
            DataRow pDataRow;

            for (int i = 0; i < pFields.FieldCount; i++)
            {
                IField pField = pFields.get_Field(i);

                if (pField.Type != esriFieldType.esriFieldTypeGeometry)
                {
                    //ÿ���ֶ�Ϊһ��
                    pDataColumn = new DataColumn();
                    pDataColumn.ColumnName = pField.Name;
                    pDataColumn.ReadOnly = true;
                    pDataTable.Columns.Add(pDataColumn);
                    //�ֶ�����
                    pArrayOfFieldName.Add(pField.Name);
                }
            }

            pFeature = null;

            for (int i = 0; i < pArrayListOfFeatures.Count; i++)
            {
                pFeature = (IFeature)pArrayListOfFeatures[i];

                while (pFeature != null)
                {
                    pDataRow = pDataTable.NewRow();
                    int index = 0;
                    //��ȡ��Ӧ�ֶε�ֵ
                    for (int j = 0; j < pArrayOfFieldName.Count; j++)
                    {
                        string FieldName = (string)pArrayOfFieldName[j];
                        index = pFeature.Fields.FindField(FieldName);
                        pDataRow[FieldName] = pFeature.get_Value(index).ToString();
                    }

                    pDataTable.Rows.Add(pDataRow);
                    break;
                }

                this.m_PropertyDataGrid.DataSource = pDataTable;
            }
        }

        private ArrayList GetFeaturesFromLayer(ILayer pLayer)
        {

            IFeatureLayer pFeatureLayer = (IFeatureLayer)pLayer;

            IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

            return GetFeaturesFromFeatureCursor(pFeatureCursor);
        }

        private ArrayList GetFeaturesFromFeatureCursor(IFeatureCursor pFeatureCursor)
        {
            ArrayList pArrayList = new ArrayList();

            IFeature pFeature = pFeatureCursor.NextFeature();

            while (pFeature != null)
            {
                pArrayList.Add(pFeature);

                pFeature = pFeatureCursor.NextFeature();

            }

            return pArrayList;
        }

        private void ZoomAndFlashFeature(IFeature pFeature)
        {
            //this.ZoomToFeature(pFeature);
            this.FlashFeature(pFeature);
        }

        private void ZoomToFeature(IFeature pFeature)
        {
            if (pFeature.Shape.GeometryType != esriGeometryType.esriGeometryPoint)
            {
                IActiveView pActiview = (IActiveView)this.m_pCurrentMap;
                IEnvelope FeaExtent = pActiview.Extent;
                FeaExtent = pFeature.Extent;

                pActiview.Extent = FeaExtent;
                pActiview.Refresh();
            }
        }

        public void SetData(ArrayList pArrayList)
        {
            if (pArrayList == null)
            {
                return;
            }

            this.DisplayPropertyForm(pArrayList);
            this.m_pArrayListOfFeatures = pArrayList;
        }

        public void SetData(IFeatureCursor pFeatureCursor)
        {
            if (pFeatureCursor == null)
            {
                return;
            }

            this.DisplayPropertyForm(pFeatureCursor);
            this.m_pArrayListOfFeatures = this.GetFeaturesFromFeatureCursor(pFeatureCursor);
        }
      
        public void SetData(ILayer pLayer)
        {
            if (pLayer == null)
            {
                return;
            }

            this.DisplayPropertyForm(pLayer);
            this.m_pArrayListOfFeatures = this.GetFeaturesFromLayer(pLayer);
        }


        // ͳ�ƺ�ֵ
        public void SetDataAfterStatisticed(DataTable theTable)
        {
            this.m_PropertyDataGrid.DataSource = theTable;
            //this.m_pArrayListOfFeatures = this.GetFeaturesFromLayer(pLayer);
        }

        // ���� �޸� 2011 ����
        private void UserProperyShow_Load(object sender, EventArgs e)
        {
          
            
        }

    }
}
