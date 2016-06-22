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

        public clsDataAccess.DataAccess m_theDataAccess;// 刘扬 修改 2011 分析
        public string m_strDKID;

        public JCZF.Renderable.CGlobalVarable.Enum_AnalysisType  m_theType;// 分析类型 刘扬 修改 2011 分析

        public string m_theCurrentLayerName = null;// 分析的图层名称，刘扬 修改 2011 分析

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
        // 刘扬 修改 2011 分析
        // 根据关键字的值得到feature并放大
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
                //MapFunction.ZoomAndFlashFeature(pFeature, m_AxMapControl);//不用闪烁，主要是当闪烁的内容的坐标系与地图坐标系不一致时会出现位置偏差 20110920岳建伟
                MapFunction.ZoomToFeature(pFeature, m_AxMapControl);      //改用选择，在GetOneFeatureWithID 中选中。
        }

        private void m_PropertyDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                #region yuejianwei 修改 20130326 根据坐标串定位

              string m_strCoordinate=  this.m_PropertyDataGrid.CurrentRow.Cells["坐标"].Value.ToString().Trim(); //获取选中记录的坐标
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

                #region 刘扬 修改 2011 分析

                //// 获取表格中ID名称
                //int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);
                //string colName = JCZF.Renderable.CGlobalVarable.m_listColNameNameOfStaticsInfoTable[indexOfLayer];

                //DataGridViewCell pDGVC = this.m_PropertyDataGrid.CurrentRow.Cells[colName]; //获取选中ID单元格
                //GetFeatureAndZoom(pDGVC.Value.ToString()); // 根据ID值得到feature并放大

                // 获取表格中ID名称
                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);
                string colName = JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoTable[indexOfLayer];

                DataGridViewCell pDGVC = this.m_PropertyDataGrid.CurrentRow.Cells[colName]; //获取选中ID单元格

                // 刘扬 修改 2011后期 分析
                if (pDGVC == null) // 没有数据则返回
                    return;

                // 获取ID
                string theID = pDGVC.Value.ToString().Trim();

                // 刘扬 修改 2011后期 分析
                colName = JCZF.Renderable.CGlobalVarable.m_strFieldNameOfLayerInSQLTable;// 所属图层名称

                pDGVC = this.m_PropertyDataGrid.CurrentRow.Cells[colName]; //获取所属图层名称

                if (pDGVC == null) // 没有数据则返回
                    return;

                // 获取所属图层名称
                string theLayerName = pDGVC.Value.ToString().Trim();

                GetFeatureAndZoom(theID, theLayerName); // 根据ID值得到feature并放大


                #endregion

                #region  原始 刘扬 修改 2011 分析

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

        #region // 刘扬 修改 2011 分析

        //
        /// <summary>
        ///  根据图层的列名找到数据库表中适合的列名
        /// </summary>
        /// <param name="theCol"></param>
        /// <returns></returns>
        private string GetFitableColName(string theCol)
        {
            return JCZF.Renderable.CGlobalVarable.GetFitalbeColName(this.m_theType, theCol);
        }

        // 构造查询语句
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


        //根据字段和和字段值选择要素
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
                //查找要素 并设置为选择
                
                pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);


                pFeature = pFeatureCursor.NextFeature();
            }
            catch(SystemException errs)
            {

            }
            return pFeature;
        }

        //所有的Featurelayer
        public IEnumLayer GetLayers(IMap pMap)
        {
            UID pUid = new UID();
            //pUid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";
            //pUid.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
            pUid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";

            return pMap.get_Layers(pUid, true);
        }


        /// <summary>
        /// 根据部分图层名得到图层
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

                //判断qrylayer是否为FeatureLayer类型
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

        // 根据ID获取Feature 
       public IFeature GetOneFeatureWithID(string pFieldName, string pFieldValue, string theLayerName)
        {
           // int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);

           // //获取的是输入的图层，应该获取裁切结果的图层进行定位 刘丽20110814
           // //m_theCurrentLayerName = JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer]; // 获取图层名称
            
           // m_theCurrentLayerName = JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer]; // 获取图层名称

           //IFeatureLayer theFLayer = GetFeatureLayerByName(m_theCurrentLayerName, this.m_AxMapControl); // 获取图层
           // IFeature m_IFeature=GetOneFeature(theFLayer, pFieldName, pFieldValue);// 找到Feature

           // m_AxMapControl.Map.ClearSelection();
           // IFeatureSelection m_FeatureSelection = theFLayer as IFeatureSelection;
           // m_FeatureSelection.Add(m_IFeature);

           // //m_AxMapControl.ActiveView.Selection.Clear();
           // //m_AxMapControl.Map.SelectFeature(theFLayer ,m_IFeature);
            
           // return m_IFeature;

            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(this.m_theType);

            //获取的是输入的图层，应该获取裁切结果的图层进行定位 刘丽20110814
            //m_theCurrentLayerName = JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer]; // 获取图层名称

            m_theCurrentLayerName = JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer]; // 获取图层名称


            // 刘扬 修改 2011后期 分析
            if (m_theCurrentLayerName.Trim() == "")
            {
                if (theLayerName.Trim() == "")
                    return null;

            }

            IFeatureLayer theFLayer = GetFeatureLayerByName(m_theCurrentLayerName, this.m_AxMapControl); // 获取图层

            // 刘扬 修改 2011后期 分析
            if (theFLayer == null)
            {
                theFLayer = GetFeatureLayerByName(theLayerName, this.m_AxMapControl); // 再一次获取图层

                if (theFLayer == null)
                    return null;
            }



            IFeature m_IFeature = GetOneFeature(theFLayer, pFieldName, pFieldValue);// 找到Feature

            // 刘扬 修改 2011后期 分析
            if (m_IFeature == null)
                return null;

            // 刘扬 修改 2011后期 分析 为了看到图层的选中
            if (theFLayer.Visible == false)
            {
                // 设置图层显示
                theFLayer.Visible = true;

                // 获取在统计中没有被显示的图层名称，等到关闭的时候全部不显示
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

               // 循环列名  刘扬 修改 2011 分析
               string[] theLayerColList = JCZF.Renderable.CGlobalVarable.GetFitableLayerColNameListOfStatisticsInfo(this.m_theType); // 获取图层的列名集合
               foreach (string theLayerColName in theLayerColList)
               {

                   string colName = GetFitableColName(theLayerColName); // 获取数据库表的列名 

                   //每个字段为一列
                   pDataColumn = new DataColumn();

                   pDataColumn.ColumnName = colName;
                   pDataColumn.ReadOnly = true;
                   pDataTable.Columns.Add(pDataColumn);

                   //字段数组
                   FieldsName.Add(theLayerColName);
               }



               // 这里有问题
               //for (int i = 0; i < pFields.FieldCount; i++)
               //{

               //    IField pField = pFields.get_Field(i);

               //    if (pField.Type != esriFieldType.esriFieldTypeGeometry)
               //    {
               //        //每个字段为一列
               //        pDataColumn = new DataColumn();

               //        string colName = GetFitableColName(pField.Name); // 获取列名 刘扬 修改 2011 分析
               //        if (colName != "") // 刘扬 修改 2011 分析
               //        {
               //            pDataColumn.ColumnName = colName;
               //            pDataColumn.ReadOnly = true;
               //            pDataTable.Columns.Add(pDataColumn);
               //            //字段数组
               //            FieldsName.Add(pField.Name);
               //        }
               //    }

               //}

               while (pRow != null)
               {
                   if (pRow.HasOID == false) continue;
                   pDataRow = pDataTable.NewRow();
                   int index = 0;
                   //获取对应字段的值
                   for (int i = 0; i < FieldsName.Count; i++)
                   {
                       string FieldName = (string)FieldsName[i];

                       index = pRow.Fields.FindField(FieldName);

                       string colName = GetFitableColName(FieldName); // 获取列名 刘扬 修改 2011 分析
                       if (FieldName.ToUpper() == "DKID" )
                       {
                           pDataRow[colName] = m_strDKID;
                       }
                       else
                       {
                           if (FieldName.ToUpper() == "ZB" || FieldName.Trim() == "坐标" )
                           {
                               pDataRow[colName] = GetCoordinate(pRow);
                           }
                           else if (FieldName.ToLower() == "shape_area" && index == -1)
                           {//9.2版本中为“shape_area”，9.3版本中为“area”
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

               TransToChinese(this.m_theType);//将英文列名转为中文名称


               Event_theSaveStatisticsInfo(this.m_theType, pDataTable); //保存统计信息数据 刘扬 修改 2011 分析

           }
           catch (Exception ee) // 刘扬 修改 2011 分析
           { 
           }

       }

        /// <summary>
        /// 将dataGrid的列表转换为中文名称
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
                   m_strCoordinate= m_strCoordinate.Remove(m_strCoordinate.Length - 1);//去掉最后的“;”
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
                    //每个字段为一列
                    pDataColumn = new DataColumn();
                    pDataColumn.ColumnName = pField.Name;
                    pDataColumn.ReadOnly = true;
                    pDataTable.Columns.Add(pDataColumn);
                    //字段数组
                    pArrayOfFieldName.Add(pField.Name);
                }
            }

            while (pFeature != null)
            {
                pDataRow = pDataTable.NewRow();
                int index = 0;
                //获取对应字段的值
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
                    //每个字段为一列
                    pDataColumn = new DataColumn();
                    pDataColumn.ColumnName = pField.Name;
                    pDataColumn.ReadOnly = true;
                    pDataTable.Columns.Add(pDataColumn);
                    //字段数组
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
                    //获取对应字段的值
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


        // 统计后赋值
        public void SetDataAfterStatisticed(DataTable theTable)
        {
            this.m_PropertyDataGrid.DataSource = theTable;
            //this.m_pArrayListOfFeatures = this.GetFeaturesFromLayer(pLayer);
        }

        // 刘扬 修改 2011 分析
        private void UserProperyShow_Load(object sender, EventArgs e)
        {
          
            
        }

    }
}
