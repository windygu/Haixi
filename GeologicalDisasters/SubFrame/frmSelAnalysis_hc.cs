using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using clsDataAccess;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Functions;
using System.IO;

using System.Collections;
using JCZF.Renderable;

namespace JCZF.SubFrame
{
    public partial class frmSelAnalysis_hc : DevComponents.DotNetBar.Office2007Form
    {
        ////地块选择方式
        //public bool b_hcselectDK;
        //public bool b_hcDrawDK;
        //public bool b_hcImportDK;

        private frmMapView m_frmMapView;
        public string m_strDKID;
        public bool m_blIsDrawPolygon;

        private bool[] m_blHasAnalysis;
        public clsDataAccess.DataAccess m_DataAccess_SYS;

         private  IFeature m_selFeature_;
         public IFeature m_selFeature
         {
             set
             {
                 m_selFeature_ = value;
                 FillHCListview(m_selFeature_.ShapeCopy);
             }
         }

        public IGeometry m_SelectedGeometry;

        //private frmMapView m_frmMapView = null;

        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl;

        private IEnumLayer m_Grouplayers;

        private UserProperyShow m_UserJBNTProperyShow;
        private UserProperyShow m_UserJSYDProperyShow;
        private UserProperyShow m_UserTDLYXZProperyShow;
        private UserProperyShow m_UserGHTProperyShow;
        private UserProperyShow m_UserTDGYProperyShow;
        private UserProperyShow m_UserKCZYProperyShow;
        private UserProperyShow m_UserCKQProperyShow;
        private UserProperyShow m_UserTKQProperyShow;

        System.Collections.ArrayList m_theButtonList = new System.Collections.ArrayList(); // 保存7个按钮(原) 刘扬 修改 2011 分析


        //CSQLOperation m_theSqlOp = new CSQLOperation(); // Sql查询 刘扬 修改 2011 分析
        //public clsDataAccess.DataAccess m_DataAccess_SYS;

        IFeature m_theOriginFeature = null; // 刘扬 修改 2011后期 分析 原始图像，为了保存原始图形

        public delegate void EventHandlerGetDkClick();
        public event EventHandlerGetDkClick GetDKClick;

        public frmSelAnalysis_hc(ESRI.ArcGIS.Controls.AxMapControl p_AxMapControl, frmMapView parentview)
        {
            InitializeComponent();
            m_AxMapControl = p_AxMapControl;
            m_frmMapView = parentview;
        }


        /// <summary>
        /// 读取以前分析的结果
        /// </summary>
        public void  ReadFromResult()
        {
            if (m_strDKID.Trim() != "" && m_strDKID.Trim() != null && m_DataAccess_SYS!=null )
            {
                //先将分析的checkbox 清空
                chkCkq.Checked = false; chkJbnt.Checked = false; chkJsyd.Checked = false; chkKczygh.Checked = false; 
                chkTDGY.Checked = false; chkTDLYGH.Checked = false; chkTdlyxz.Checked = false; chkTkq.Checked = false;

                ///////////////////////////

                bool m_blTDLYXZ = false;
                bool m_blTDLYGH = false;
                bool m_blTKQ = false;
                bool m_blCKQ = false;
                bool m_blKCZYGH = false;
                bool m_blJSYD = false;
                bool m_blJBNT = false;
                bool m_blTDGY = false;

                string m_strSQL="SELECT * FROM  ";
                
                //土地利用现状分析结果
                //ReadFromResult_TDLYXZ(m_strSQL);
                m_blTDLYXZ = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ, m_UserTDLYXZProperyShow, chkTdlyxz, tabItemTDXZ, tabControlPanelTDXZ);
               //基本农田
                m_blJBNT = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT, m_UserJBNTProperyShow, chkJbnt, tabItemJBNT, tabControlPanelJBNT);

                //建设用地
                m_blJSYD = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD, m_UserJSYDProperyShow, chkJsyd, tabItemJSYD, tabControlPanelJSYD);
                //土地供应
                m_blTDGY = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY, m_UserTDGYProperyShow, chkTDGY, tabItemTDGY, tabControlPanelTDGY);
                //土地利用规划
                m_blTDLYGH = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH, m_UserTDGYProperyShow, chkTDLYGH, tabItemTDGH, tabControlPanelTDGH);
                //采矿权
                m_blCKQ = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ, m_UserCKQProperyShow, chkCkq, tabItemCKQ, tabControlPanelCKQ);
                //探矿权
                m_blTKQ = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ, m_UserTKQProperyShow, chkTkq, tabItemTKQ, tabControlPanelTKQ);
                //矿产资源规划
                m_blKCZYGH = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH, m_UserKCZYProperyShow, chkKczygh, tabItemKCZYGH, tabControlPanelKCZYGH);

                if (m_blTDLYXZ == false && m_blJBNT == false && m_blJSYD == false && m_blTDGY == false && m_blTDLYGH == false && m_blCKQ == false && m_blTKQ == false && m_blKCZYGH == false )
                {
                    //没有一个从数据库中读出来，就将分析的check设置为默认状态
                    chkCkq.Checked = true; chkJbnt.Checked = true; chkJsyd.Checked = true; chkKczygh.Checked = false;
                    chkTDGY.Checked = true; chkTDLYGH.Checked = false; chkTdlyxz.Checked = false; chkTkq.Checked = true;
                }

                SetTabItemUnVisible();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strSQL"></param>
        /// <param name="p_Enum_AnalysisType"></param>
        /// <returns></returns>
        private bool ReadFromResult(string p_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_Enum_AnalysisType, UserProperyShow p_UserProperyShow, CheckBox p_CheckBox, DevComponents.DotNetBar.TabItem p_TabItem, DevComponents.DotNetBar.TabControlPanel p_TabControlPanel)
        {
            try
            {
                string m_strSQL = "";

                //读取分析结果
                m_strSQL = p_strSQL + JCZF.Renderable.CGlobalVarable.m_listTableNameOfStaticsResult[(int)p_Enum_AnalysisType];
                m_strSQL = m_strSQL + " WHERE " + JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName + "='" + m_strDKID + "'";
                DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);

                if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
                {
                    p_CheckBox.Checked = true;
                    txtResult.Text = txtResult.Text + "\r\n\r\n" + m_DataRowCollection[0]["FXJG"].ToString().Replace("WYHH", "\r\n");
                    this.txtResult.Text += "----------------------------------------------------";
                }
                else
                {
                    return false ;
                }

                //读取分析结果详细内容

                m_strSQL =  p_strSQL + JCZF.Renderable.CGlobalVarable.m_listTableNameOfStaticsInfo[(int)p_Enum_AnalysisType];
                m_strSQL = m_strSQL + " WHERE " + JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName + "='" + m_strDKID + "'";
                DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);

                if (m_DataTable != null && m_DataTable.Rows.Count > 0)
                {
                   TransColmnsNameToChinese(p_Enum_AnalysisType,m_DataTable);
                    p_CheckBox.Checked = true;

                    if (p_UserProperyShow == null)
                    {
                        p_UserProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
                    }
                    p_UserProperyShow.ShowDataFromDatabase(m_DataTable);


                    p_TabControlPanel.Controls.Clear();
                    p_TabControlPanel.Controls.Add(p_UserProperyShow);
                    p_UserProperyShow.Dock = DockStyle.Fill;
                    tabControl1.SelectedTab = p_TabItem;
                    tabControl1.Refresh();
                }
            }
            catch (SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
                return false;
            }

            return true;

        }

        private bool TransColmnsNameToChinese(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_Enum_AnalysisType, DataTable p_DataTable)
        {
            switch (p_Enum_AnalysisType)
            {
                case CGlobalVarable.Enum_AnalysisType.TDLYXZ: // 分析类型：土地利用现状
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.TDLYGH:  // 分析类型：规划数据
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.TDGY:  // 分析类型：土地供应数据
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.JBNT:   // 分析类型：基本农田
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInJBNTTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInJBNTTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.JSYD:   // 分析类型：建设用地数据
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInJSYDTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInJSYDTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.KCZYGH:   // 分析类型：矿产资源规划
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.CKQ:   // 分析类型：采矿权
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.TKQ:   // 分析类型：探矿权
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
            }
            return true;
        }

        private bool  TransColmnsNameToChinese(DataTable p_DataTable, string[] p_DataTableColmnsEnglish, string[] p_DataTableColmnsChinese)
        {
            for (int i = 0; i < p_DataTable.Columns.Count; i++)
            {
                if (p_DataTable.Columns[i].ColumnName.Trim().ToUpper() == "DKID" )
                {
                    p_DataTable.Columns[i].ColumnName ="地块编号";
                }
                else if (p_DataTable.Columns[i].ColumnName.Trim().ToUpper() == "BZ")
                {
                    p_DataTable.Columns[i].ColumnName = "备注";
                }
                else if (p_DataTable.Columns[i].ColumnName.Trim().ToUpper() == "SSTCMC")
                {
                    p_DataTable.Columns[i].ColumnName = "所属图层名称";
                }
                else {

                    for (int j = 0; j < p_DataTableColmnsEnglish.Length - 1; j++)
                    {
                        if (p_DataTable.Columns[i].ColumnName.Trim() == p_DataTableColmnsEnglish[j].Trim())
                        {
                            p_DataTable.Columns[i].ColumnName = p_DataTableColmnsChinese[j].Trim();
                        }
                    }

                }
                

            }

            return true;
        }

        private  void ReadFromResult_TDLYXZ(string p_strSQL)
        {
            //try
            //{
            //    string m_strSQL = "";

            //    //读取分析结果
            //    m_strSQL = p_strSQL + JCZF.Renderable.CGlobalVarable.m_listTableNameOfStaticsResult[(int)JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ];
            //    m_strSQL = m_strSQL + " WHERE " + JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName + "='" + m_strDKID + "'";
            //    DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);

            //    if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
            //    {
            //        chkTdlyxz.Checked = true;
            //        txtResult.Text = txtResult.Text + m_DataRowCollection[0]["FXJG"].ToString().Replace("WYHH", "\r\n");
            //        this.txtResult.Text += "----------------------------------------------------";
            //    }

            //    //读取分析结果详细内容

            //    m_strSQL = "set names gb2312 ;" + p_strSQL + JCZF.Renderable.CGlobalVarable.m_listTableNameOfStaticsInfo[(int)JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ];
            //    m_strSQL = m_strSQL + " WHERE " + JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName + "='" + m_strDKID + "'";
            //    DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);

            //    if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            //    {
            //        chkTdlyxz.Checked = true;

            //        if (m_UserTDLYXZProperyShow == null)
            //        {
            //            m_UserTDLYXZProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
            //        }
            //        m_UserTDLYXZProperyShow.ShowDataFromDatabase(m_DataTable);


            //        this.tabControlPanelTDXZ.Controls.Clear();
            //        this.tabControlPanelTDXZ.Controls.Add(m_UserTDLYXZProperyShow);

            //        tabControl1.SelectedTab = this.tabItemTDXZ;
            //        tabControl1.Refresh();

            //    }
            //}
            //catch (SystemException errs)
            //{
            //    clsFunction.Function.MessageBoxError(errs.Message);
            //    
            //}

           
        }
        private void ReadFromResult_TDLYGH()
        {
        }

        private void ReadFromResult_JSYDSP()
        {
        }
        private void ReadFromResult_TDGY()
        {
        }
        private void ReadFromResult_KCZYGH()
        {
        }
        private void ReadFromResult_TKQ()
        {
        }
        private void ReadFromResult_CKQ()
        {
        }
        private void ReadFromResult_JBNT()
        {
        }

        /// <summary>
        /// 填充 工具条按钮调用的窗体的listview 刘丽20110731
        /// </summary>
        /// <param name="geo"></param>
        private  void FillHCListview(IGeometry geo)
        {
            listViewEx1.Items.Clear();

            ListViewItem listviewitem;

            IPointCollection pcollection = geo as IPointCollection;

            for (int i = 0; i < pcollection.PointCount; i++)
            {
                IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                pcollection.QueryPoint(i, pt);

                listviewitem = new ListViewItem((i + 1).ToString());

                listviewitem.SubItems.Add(pt.X.ToString());

                listviewitem.SubItems.Add(pt.Y.ToString());
                listViewEx1.Items.Add(listviewitem);
            }

        }


        private void chkTdlyxz_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region 刘扬 修改 2011 分析
        // 根据字段获取feature的字段值
        private string GetOneFieldOftheFeatureWithFiledName(IFeature theFeature, string fieldName)
        {
            int index = theFeature.Fields.FindField(fieldName);

            if (index == -1)
                return null;

            return theFeature.get_Value(index).ToString();

        }



        // 获取土地核查的图斑编号
        private string GetStaticsTBBH()
        {
            return GetOneFieldOftheFeatureWithFiledName(m_selFeature_, "dkid");

        }

        // 将\r\n替换掉，否则不能存储
        private string TransforToFitableSQL(string theString)
        {

            return theString.Replace("\r\n", "WYHH");

        }



        // 将WYHH替换掉，恢复为\r\n
        private string ReTransforToFitableSQL(string theString)
        {
            return theString.Replace("WYHH", "\r\n");
        }



        // 保存分析结果到数据库中
        private bool SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_Enum_AnalysisType, string theResult, string theLayerNameOfStatics)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(p_Enum_AnalysisType); // 表名
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // 表中关键字列名
            string theID = m_strDKID;// GetStaticsTBBH(); // 表中关键字的值

            System.Collections.ArrayList theColNameList = new System.Collections.ArrayList(); // 插入列名集合

            foreach (string theColName in JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsResult) // 构造列名集合
            {
                theColNameList.Add(theColName);
            }

            System.Collections.ArrayList theColValList = new System.Collections.ArrayList();// 插入列数据集合
            theColValList.Add(theID); // 图斑编号
            theColValList.Add(theLayerNameOfStatics); // 图层名称
            theColValList.Add(TransforToFitableSQL(theResult)); // 分析结果
            theColValList.Add(DateTime.Now); // 分析时间


            //return m_theSqlOp.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);
            //删除以前的记录
            
            m_DataAccess_SYS.DeleteByColumnValue(theIDName, theID, tableName);
            return m_DataAccess_SYS.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);

        }

        // 根据一行数据构建一个列表
        private void BuildOneListWithOneRow(DataRow theRow, ref System.Collections.ArrayList theList)
        {
            ////string [] theDataTypeList = JCZF.Renderable.CGlobalVarable.GetFitalbeColDataTypeListOfStatisticsInfo (type)

            for (int i = 0; i < theRow.Table.Columns.Count; i++)
            {
                theList.Add(theRow[i]);
            }

        }


        /// <summary>
        /// 保存分析详细信息到数据库中
        /// </summary>
        /// <param name="type"></param>
        /// <param name="theInfoTable"></param>
        /// <returns></returns>
 
        private bool SaveStaticsInfoToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type, DataTable theInfoTable)
        {


            try
            {
                string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsInfo(type); // 表名
                string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName; //编号 表中关键字列名
                //string theTBBH = m_strDKID;// GetStaticsTBBH(); // 获取图斑编号

                System.Collections.ArrayList theColNameList = new System.Collections.ArrayList(); // 插入列名集合

                theColNameList.Add(theIDName); // 编号列

                string[] theColNameListString = JCZF.Renderable.CGlobalVarable.GetTableColNameListOfStatisticsInfo(type);

                foreach (string theColName in theColNameListString) // 构造列名集合
                {
                    theColNameList.Add(theColName);
                }

                System.Collections.ArrayList theColValList = new System.Collections.ArrayList();// 插入列数据集合


                //m_theSqlOp.DelAllCorrespondingRecordWithID(tableName, theIDName, m_strDKID); // 删除所有相关ID的记录
                m_DataAccess_SYS.DeleteByColumnValue(theIDName, m_strDKID, tableName);


                for (int i = 0; i < theInfoTable.Rows.Count; i++) // 按行存储
                {

                    int index = i + 1;// 设置编号的序号

                    theColValList.Clear();
                    theColValList.Add(m_strDKID);

                    //string theID = String.Format("{0}_{1}", m_strDKID, index); // 构造编号
                    //theColValList.Add(theTBBH); // 添加编号 
                    BuildOneListWithOneRow(theInfoTable.Rows[i], ref theColValList);   // 根据一行数据构建一个列表

                    //m_theSqlOp.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);   // 插入数据
                    m_DataAccess_SYS.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, m_strDKID);

                }


                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }



        // 判断是否已经被统计
        private bool HasBeenStatisticed(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(type); // 表名
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // 表中关键字列名
            string theID = GetStaticsTBBH(); // 表中关键字的值

            //return m_theSqlOp.ExistKeyIntheTable(tableName, theIDName, theID);
            return m_DataAccess_SYS.IsRecordExist(theIDName, theID,tableName);
        }

        #endregion

        private void btnSelectAll_Click(object sender, EventArgs e)
        {           
            SetCheckBoxChecked(true);
        }

        private void SetCheckBoxChecked(bool p_blChecked)
        {
            this.chkCkq.Checked = p_blChecked;
            this.chkTDLYGH.Checked = p_blChecked;
            this.chkJbnt.Checked = p_blChecked;
            this.chkJsyd.Checked = p_blChecked;
            this.chkKczygh.Checked = p_blChecked;
            this.chkTdlyxz.Checked = p_blChecked;
            this.chkTkq.Checked = p_blChecked;
            this.chkTDGY.Checked = p_blChecked;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                SetTabItemUnVisible();
                //tabControl2.SelectedTab = tabItem3;
                //for (int ii = 0; ii < 2; ii++)  // 刘扬 修改 2011后期  分析 注释
                {
                    //由于未知原因，必须进行两次，分析结果才正确，以后还要分析清楚！20111023，岳建伟
                    //if (ii == 1)  // 刘扬 修改 2011后期  分析 注释
                    {
                        tabControl2.SelectedTab = tabItem3;
                    }
                    this.progressBar1.Value = 10;

                    this.txtResult.Text = "开始分析计算！\r\n";


                    txtResult.Text = "";

                    // 清空临时图层“核查结果”
                    IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
                    if (hcjg != null)
                    {
                        m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);

                    }

               
                //this.txtResult.Text = strTemp;
                

                    clsClipStat m_clsClipStat = new clsClipStat();
                    m_clsClipStat.m_strFilePath = Application.StartupPath + "\\OverlayTemp";// 临时分析数据存放文件夹
                    m_clsClipStat.m_axMapcontrol = m_AxMapControl;
                    //写死 土地核查图层 （因为通过属性表传来时，没有m_sellayer参数，即m_sellayer为null）

                    // 获取图层“土地核查”
                    ILayer selLayer = MapFunction.getFeatureLayerByName("土地核查", m_AxMapControl);

                    m_clsClipStat.m_selLayer = selLayer;
                    m_clsClipStat.pDataAcess = m_DataAccess_SYS;
                    if (m_SelectedGeometry == null)
                    {
                        IPolygon m_IPolygon = (IPolygon)this.m_selFeature_.Shape;

                        // 刘扬 修改 2011后期 分析 注释
                        // 没有什么用
                        // IPointCollection m_IPointCollection = (IPointCollection)m_IPolygon;

                        // 肯定有用，翻转，不知道怎么用的
                        //m_IPolygon.ReverseOrientation();


                        m_clsClipStat.m_Geometry = (IGeometry)m_IPolygon;
                        m_clsClipStat.CreateClipFeatureClass(m_IPolygon); // 建立为一个图层
                        //m_clsClipStat.CreateClipFeatureClass(this.m_selFeature.ShapeCopy);

                    }
                    else
                    {
                        m_clsClipStat.m_Geometry = m_SelectedGeometry;
                        m_clsClipStat.CreateClipFeatureClass(m_SelectedGeometry); // 建立为一个图层
                    }
                    this.progressBar1.Visible = true;

                    try
                    {
                        try
                        {


                            // 现状
                            if (chkTdlyxz.Checked)
                            {
                                if (m_blHasAnalysis[0] == true)
                                {
                                    btnTDLY_Click(sender, e);
                                }
                                else
                                {

                                    // 刘扬 修改 2011后期 土地利用现状分析 注释
                                    //

                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "土地利用现状"; // 分析类型
                                    // 便于以后使用
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ;

                                    // 刘扬 修改 2011后期 分析
                                    // 进行Clip和Merge操作，最终获取数据
                                    m_clsClipStat.Geoexecute();

                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;

                                    // 将裁切后图层加载到map中，并命名“土地利用现状clip”
                                    AddtoTOC(m_clsClipStat.pDltbOutputFeatClass, "土地利用现状clip");
                                    loadDLTBProp(m_clsClipStat.pDltbOutputFeatClass);
                                    this.progressBar1.Value = 30;

                                    #region // 刘扬 修改 2011 分析

                                    
                                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                    // 获取图层名
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ,theResult, theLayerName);// 保存分析结果


                                    #endregion

                                 
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "土地利用现状数据有错误，请检查数据" + "\r\n";
                            //this.txtResult.Text =strTemp;
                        }
                        try
                        {
                            //规划
                            if (chkTDLYGH.Checked)
                            {


                                //if (m_blHasAnalysis[1] == true)
                                //{
                                //    btnGH_Click(sender, e);
                                //}
                                //else
                                //{
                                    // 刘扬 修改 2011后期 规划数据分析 注释
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.QHTexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "规划数据"; // 分析类型
                                    // 便于以后使用
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH;

                                    // 刘扬 修改 2011后期 分析
                                    // 进行Clip和Merge操作，最终获取数据

                                    m_clsClipStat.QHTexecute();

                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;
                                    AddtoTOC(m_clsClipStat.pGHTOutputFeatClass, "规划数据clip");
                                    loadGHTProp(m_clsClipStat.pGHTOutputFeatClass);
                                    this.progressBar1.Value = 50;

                                    #region // 刘扬 修改 2011 分析

                                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                    // 获取图层名
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH, theResult, theLayerName);// 保存分析结果


                                    #endregion
                                //}

                            }
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "土地利用规划数据有错误，请检查数据" + "\r\n";
                            //this.txtResult.Text = strTemp;
                        }

                        try
                        {
                            //土地供应
                            if (chkTDGY.Checked)
                            {
                                //if (m_blHasAnalysis[2] == true)
                                //{
                                //    btnTDGY_Click(sender, e);
                                //}
                                //else
                                //{

                                    // 刘扬 修改 2011后期 土地供应分析 注释
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.TDGYexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "土地供应"; // 分析类型
                                    // 便于以后使用
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY;

                                    // 刘扬 修改 2011后期 分析
                                    // 进行Clip和Merge操作，最终获取数据
                                    m_clsClipStat.TDGYexecute();

                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;
                                    AddtoTOC(m_clsClipStat.pTDGYOutputFeatClass, "土地供应clip");
                                    loadTDGYProp(m_clsClipStat.pTDGYOutputFeatClass);
                                    this.progressBar1.Value = 50;

                                    #region // 岳建伟 修改 2011 分析

                         
                                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                    // 获取图层名
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY, theResult, theLayerName);// 保存分析结果


                                    #endregion
                                }

                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "土地供应数据有错误，请检查数据" + "\r\n";
                            //this.txtResult.Text = strTemp;
                        }

                        try
                        {
                            //基本农田
                            if (chkJbnt.Checked)
                            {


                                //if (m_blHasAnalysis[3] == true)
                                //{
                                //    btnJBNT_Click(sender, e);
                                //}
                                //else
                                //{
                                    // 刘扬 修改 2011后期 基本农田分析 注释
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.JBNTexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}


                                    string type = "基本农田"; // 分析类型
                                    // 便于以后使用
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT ;

                                    // 刘扬 修改 2011后期 基本农田分析
                                    m_clsClipStat.JBNTexecute();//
                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;
                                    AddtoTOC(m_clsClipStat.pJbntOutputFeatClass, "基本农田clip");
                                    loadJBNTProp(m_clsClipStat.pJbntOutputFeatClass);
                                    this.progressBar1.Value = 60;

                                    #region  刘扬 修改 2011 分析  添加例子

                      
                                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                    // 获取图层名
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT, theResult, theLayerName);// 保存分析结果


                                    #endregion
                                }

                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "基本农田数据有错误，请检查数据" + "\r\n";
                            //this.txtResult.Text = strTemp ;
                        }
                        try
                        {
                            //建设项目
                            if (chkJsyd.Checked)
                            {

                                //if (m_blHasAnalysis[4] == true)
                                //{
                                //    btnGH_Click(sender, e);
                                //}
                                //else
                                //{
                                    // 刘扬 修改 2011后期 建设项目分析 注释
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.JSYDexecute1();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}
                                          
                                    string type = "建设用地数据"; // 分析类型
                                    // 便于以后使用
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD;

                                    // 刘扬 修改 2011后期 建设用地数据分析
                                    // 进行Clip和Merge操作，最终获取数据
                                    m_clsClipStat.JSYDexecute();


                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                

                                    AddtoTOC(m_clsClipStat.pJsydOutputFeatClass, "建设用地数据clip");
                                    loadJSYDProp(m_clsClipStat.pJsydOutputFeatClass);

                                    this.progressBar1.Value = 70;

                                    #region  刘扬 修改 2011 分析

                            
                                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                    // 获取图层名
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD , theResult, theLayerName);// 保存分析结果


                                    #endregion
                                }
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "建设项目审批数据有错误，请检查数据" + "\r\n";
                            //this.txtResult.Text = strTemp ;
                        }
                        try
                        {
                            //矿产资源规划
                            if (chkKczygh.Checked)
                            {
                                //if (m_blHasAnalysis[5] == true)
                                //{
                                //    btnKCZY_Click(sender, e);
                                //}
                                //else
                                //{

                                    // 刘扬 修改 2011后期 矿产资源规划分析 注释
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.KCZYexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "矿产资源规划"; // 分析类型
                                    // 便于以后使用
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH;

                                    // 刘扬 修改 2011后期 建设用地数据分析
                                    // 进行Clip和Merge操作，最终获取数据
                                    m_clsClipStat.KCZYexecute();


                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;

                                    AddtoTOC(m_clsClipStat.pKczyOutputFeatClass, "矿产资源规划clip");
                                    loadKczyProp(m_clsClipStat.pKczyOutputFeatClass);
                                    this.progressBar1.Value = 80;


                                    #region  刘扬 修改 2011 分析

                       
                                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                    // 获取图层名
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH, theResult, theLayerName);// 保存分析结果


                                    #endregion
                                }
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "矿产资源规划数据有错误，请检查数据" + "\r\n";
                            //this.txtResult.Text = strTemp ;
                        }
                        //采矿
                        try
                        {
                            if (chkCkq.Checked)
                            {
                                //if (m_blHasAnalysis[6] == true)
                                //{
                                //    btnCKQ_Click(sender, e);
                                //}
                                //else
                                //{
                                    // 刘扬 修改 2011后期 矿产资源规划分析 注释
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.CKQexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                      
                                    string type = "采矿权"; // 分析类型
                                    // 便于以后使用
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ;

                                    // 刘扬 修改 2011后期 采矿权分析
                                    // 进行Clip和Merge操作，最终获取数据
                                    m_clsClipStat.CKQexecute();


                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;

                                    AddtoTOC(m_clsClipStat.pCkqOutputFeatClass, "采矿权clip");
                                    loadCKQProp(m_clsClipStat.pCkqOutputFeatClass);
                                    this.progressBar1.Value = 90;

                                    #region  刘扬 修改 2011 分析

                                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                    // 获取图层名
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ, theResult, theLayerName);// 保存分析结果


                                    #endregion
                                }
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "采矿权登记数据有错误，请检查数据" + "\r\n";
                            //this.txtResult.Text = strTemp;
                        }


                        try
                        {
                            //探矿
                            if (chkTkq.Checked)
                            {
                                //if (m_blHasAnalysis[7] == true)
                                //{
                                //    btnTKQ_Click(sender, e);
                                //}
                                //else
                                //{

                                    // 刘扬 修改 2011后期 探矿权分析 注释
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.TKQexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "探矿权"; // 分析类型
                                    // 便于以后使用
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ;

                                    // 刘扬 修改 2011后期 采矿权分析
                                    // 进行Clip和Merge操作，最终获取数据
                                    m_clsClipStat.TKQexecute();

                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;

                                    AddtoTOC(m_clsClipStat.pTkqOutputFeatClass, "探矿权clip");
                                    loadTKQProp(m_clsClipStat.pTkqOutputFeatClass);

                                    this.progressBar1.Value = 100;


                                    //this.txtResult.Text = strTemp;
                                    this.progressBar1.Value = 100;

                                    #region  刘扬 修改 2011 分析


                                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                    // 获取图层名
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ, theResult, theLayerName);// 保存分析结果


                                    #endregion
                                }

                              
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "探矿权登记数据有错误，请检查数据";
                            //this.txtResult.Text = strTemp ;
                        }

                    }
                    catch (Exception o)
                    { }
                    this.progressBar1.Visible = false;
                }

                btnSaveResult.Enabled = true;
                btnExport.Enabled = true;


            }
            catch (Exception o)
            { }
        }


        //private void Analysis()
        //{
        //    txtResult.Text = "";

        //    IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
        //    if (hcjg != null)
        //    {
        //        m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);

        //    }

        //    this.progressBar1.Value = 10;

        //    this.txtResult.Text = "开始分析计算！\r\n";

        //    //this.txtResult.Text = strTemp;

        //    clsClipStat m_clsClipStat = new clsClipStat();
        //    m_clsClipStat.m_strFilePath = Application.StartupPath + "\\OverlayTemp";
        //    m_clsClipStat.m_axMapcontrol = m_AxMapControl;
        //    //写死 土地核查图层 （因为通过属性表传来时，没有m_sellayer参数，即m_sellayer为null）

        //    ILayer selLayer = MapFunction.getFeatureLayerByName("土地核查", m_AxMapControl);

        //    m_clsClipStat.m_selLayer = selLayer;
        //    m_clsClipStat.pDataAcess = m_DataAccess_SYS;
        //    m_clsClipStat.m_Geometry = this.m_selFeature.ShapeCopy;
        //    m_clsClipStat.CreateClipFeatureClass(this.m_selFeature.ShapeCopy);
        //    this.progressBar1.Visible = true;

        //    try
        //    {
        //        try
        //        {

        //            // 现状
        //            if (chkTdlyxz.Checked)
        //            {


        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnTDLY_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pDltbOutputFeatClass);
        //                    loadDLTBProp(m_clsClipStat.pDltbOutputFeatClass);
        //                    this.progressBar1.Value = 30;

        //                    #region // 刘扬 修改 2011 分析

        //                    string type = "土地利用现状"; // 分析类型
        //                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
        //                    // 获取图层名
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// 保存分析结果


        //                    #endregion

        //                    //txtResult.Text = m_clsClipStat.strHCJG;
        //                    //MessageBox.Show(m_clsClipStat.strHCJG);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "土地利用现状数据有错误，请检查数据" + "\r\n";
        //            //this.txtResult.Text =strTemp;
        //        }
        //        try
        //        {
        //            //规划
        //            if (chkGhsj.Checked)
        //            {


        //                if (m_blHasAnalysis[1] == true)
        //                {
        //                    btnGH_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.QHTexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;
        //                    AddtoTOC(m_clsClipStat.pGHTOutputFeatClass);
        //                    loadGHTProp(m_clsClipStat.pGHTOutputFeatClass);
        //                    this.progressBar1.Value = 50;

        //                    #region // 刘扬 修改 2011 分析

        //                    string type = "规划数据"; // 分析类型
        //                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
        //                    // 获取图层名
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// 保存分析结果


        //                    #endregion
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "土地利用规划数据有错误，请检查数据" + "\r\n";
        //            //this.txtResult.Text = strTemp;
        //        }
        //        try
        //        {
        //            //基本农田
        //            if (chkJbnt.Checked)
        //            {


        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnJBNT_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.JBNTexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;
        //                    AddtoTOC(m_clsClipStat.pJbntOutputFeatClass);
        //                    loadJBNTProp(m_clsClipStat.pJbntOutputFeatClass);
        //                    this.progressBar1.Value = 60;

        //                    #region  刘扬 修改 2011 分析  添加例子

        //                    string type = "基本农田"; // 分析类型
        //                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
        //                    // 获取图层名
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// 保存分析结果


        //                    #endregion
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "基本农田数据有错误，请检查数据" + "\r\n";
        //            //this.txtResult.Text = strTemp ;
        //        }
        //        try
        //        {
        //            //建设项目
        //            if (chkJsyd.Checked)
        //            {

        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnGH_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.JSYDexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pJsydOutputFeatClass);
        //                    loadJSYDProp(m_clsClipStat.pJsydOutputFeatClass);
        //                    this.progressBar1.Value = 70;

        //                    #region  刘扬 修改 2011 分析

        //                    string type = "建设用地数据"; // 分析类型
        //                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
        //                    // 获取图层名
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// 保存分析结果


        //                    #endregion
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "建设项目审批数据有错误，请检查数据" + "\r\n";
        //            //this.txtResult.Text = strTemp ;
        //        }
        //        try
        //        {
        //            //矿产资源规划
        //            if (chkKczygh.Checked)
        //            {
        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnKCZY_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.KCZYexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pKczyOutputFeatClass);
        //                    loadKczyProp(m_clsClipStat.pKczyOutputFeatClass);
        //                    this.progressBar1.Value = 80;


        //                    #region  刘扬 修改 2011 分析

        //                    string type = "矿产资源规划"; // 分析类型
        //                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
        //                    // 获取图层名
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// 保存分析结果


        //                    #endregion
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "矿产资源规划数据有错误，请检查数据" + "\r\n";
        //            //this.txtResult.Text = strTemp ;
        //        }
        //        //采矿
        //        try
        //        {
        //            if (chkCkq.Checked)
        //            {
        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnCKQ_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.CKQexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pCkqOutputFeatClass);
        //                    loadCKQProp(m_clsClipStat.pCkqOutputFeatClass);
        //                    this.progressBar1.Value = 90;

        //                    #region  刘扬 修改 2011 分析

        //                    string type = "采矿权"; // 分析类型
        //                    string theResult = m_clsClipStat.strHCJG; ;// 分析结果
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
        //                    // 获取图层名
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// 保存分析结果


        //                    #endregion
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "采矿权登记数据有错误，请检查数据" + "\r\n";
        //            //this.txtResult.Text = strTemp;
        //        }
        //        try
        //        {
        //            //探矿
        //            if (chkTkq.Checked)
        //            {
        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnTKQ_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.TKQexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("公顷") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pTkqOutputFeatClass);
        //                    loadTKQProp(m_clsClipStat.pTkqOutputFeatClass);
        //                    this.progressBar1.Value = 100;
        //                }

        //                //this.txtResult.Text = strTemp;
        //                this.progressBar1.Value = 100;

        //                #region  刘扬 修改 2011 分析

        //                string type = "探矿权"; // 分析类型
        //                string theResult = m_clsClipStat.strHCJG; ;// 分析结果
        //                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
        //                // 获取图层名
        //                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                SaveStaticsResultToSQL(type, theResult, theLayerName);// 保存分析结果


        //                #endregion
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "探矿权登记数据有错误，请检查数据";
        //            //this.txtResult.Text = strTemp ;
        //        }
        //    }
        //    catch (Exception o)
        //    { }
        //    this.progressBar1.Visible = false;
        //}


        #region 添加到toc  刘丽20110814
        private void AddtoTOC(IFeatureClass outputfeatclass,string layername)
        {
            // 刘扬 修改 2011后期 分析 
            if (outputfeatclass == null)
                return;

            IFeatureLayer outlayer = new FeatureLayer() as IFeatureLayer;

            outlayer.SpatialReference = m_AxMapControl.SpatialReference;
            
            outlayer.FeatureClass = outputfeatclass;
            outlayer.Name = layername;

            IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
            IGroupLayer hcjg1 = new GroupLayer() as IGroupLayer;

            if (hcjg != null)
            {
                hcjg.Add((ILayer)outlayer);
                m_AxMapControl.Map.AddLayer(outlayer);
                m_AxMapControl.Map.DeleteLayer(outlayer);

            }
            else
            {
                //IGroupLayer hcjg = new GroupLayer() as IGroupLayer;
                hcjg1.SpatialReference = m_AxMapControl.SpatialReference;
                hcjg1.Name = "核查结果";
                hcjg1.Add((ILayer)outlayer);
                m_AxMapControl.Map.AddLayer((ILayer)hcjg1);
            }
        }

        private IGroupLayer GetHcjggrouplayer(string grouplayername)
        {
            IGroupLayer ResGrouplayer = null;
            this.m_Grouplayers = Functions.MapFunction.GetGroupLayers(m_AxMapControl.ActiveView.FocusMap);
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

        #region 加载详细信息

        private void loadJBNTProp(IFeatureClass m_FeatureClass)
        {
            // 刘扬 修改 2011后期 分析 
            if (m_FeatureClass == null)
                return;

            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemJSYD.Visible = false ;
                return;
            }
            if (m_UserJBNTProperyShow == null)
            {
                m_UserJBNTProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
              m_UserJBNTProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
           m_UserJBNTProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析 添加例子
           }
            m_UserJBNTProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析 添加例子
            m_UserJBNTProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT; // 基本农田 添加例子

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "基本农田clip";

            // 获取图层名 刘丽20110814
            string type = "基本农田";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserJBNTProperyShow.SetData(m_layer as ILayer);

            //this.gbJBNT.Controls.Add(m_UserJBNTProperyShow);
            m_UserJBNTProperyShow.Dock = DockStyle.Fill;
            //this.tabJBNT.Controls.Clear();
            //this.tabJBNT.Controls.Add(m_UserJBNTProperyShow);

            this.tabControlPanelJBNT.Controls.Clear();
            this.tabControlPanelJBNT.Controls.Add(m_UserJBNTProperyShow);
            this.tabItemJBNT.Visible = true;
            tabControl1.SelectedTab = this.tabItemJBNT;
        }
        void m_frmJBNTHCResult_FlashFeature(ESRI.ArcGIS.Geodatabase.IFeature pFeature)
        {
            m_AxMapControl.FlashShape(pFeature.Shape, 3, 300, null);
        }

        private void loadJSYDProp(IFeatureClass m_FeatureClass)
        {
            // 刘扬 修改 2011后期 分析 
            if (m_FeatureClass == null)
                return;


            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemJSYD.Visible = false ;
                return;
            }
            if (m_UserJSYDProperyShow == null)
            {
                m_UserJSYDProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
                m_UserJSYDProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
                m_UserJSYDProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析 
            }
            m_UserJSYDProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserJSYDProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD ; // 建设用地数据 

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "建设用地数据clip";

            // 获取图层名 刘丽20110814
            string type = "建设用地数据";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserJSYDProperyShow.SetData(m_layer as ILayer);
            m_UserJSYDProperyShow.Dock = DockStyle.Fill;
            //this.tabJSYD.Controls.Clear();
            //this.tabJSYD.Controls.Add(m_UserJSYDProperyShow);

            this.tabControlPanelJSYD.Controls.Clear();
            this.tabControlPanelJSYD.Controls.Add(m_UserJSYDProperyShow);
            this.tabItemJSYD.Visible = true;
            tabControl1.SelectedTab = this.tabItemJSYD;
        }
        /// <summary>
        /// 将分析结果的每一块占地放入显示面板的listview中
        /// </summary>
        /// <param name="m_FeatureClass"></param>
        private void loadDLTBProp(IFeatureClass m_FeatureClass)
        {

            try
            {
                // 刘扬 修改 2011后期 分析 
                if (m_FeatureClass == null)
                    return;

                if (m_FeatureClass.FeatureCount(null) < 1)
                {
                    //this.tabItemTDXZ.Visible = false;
                    return;
                }
                this.tabItemTDXZ.Visible = true;

                if (m_UserTDLYXZProperyShow == null)
                {
                    m_UserTDLYXZProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
                    m_UserTDLYXZProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
                    m_UserTDLYXZProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析
                }

                m_UserTDLYXZProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
                m_UserTDLYXZProperyShow.m_strDKID = m_strDKID;
                m_UserTDLYXZProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ; // 土地利用 刘扬 修改 2011 分析

                IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
                m_layer.FeatureClass = m_FeatureClass;

                m_layer.Name = "土地利用现状clip";

                // 获取图层名 刘丽20110814
                string type = "土地利用现状";
                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);
                JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;


                m_UserTDLYXZProperyShow.SetData(m_layer as ILayer);
                m_UserTDLYXZProperyShow.Dock = DockStyle.Fill;
                //this.tabTDXZ.Controls.Clear();
                //this.tabTDXZ.Controls.Add(m_UserTDLYXZProperyShow);
                this.tabControlPanelTDXZ.Controls.Clear();
                this.tabControlPanelTDXZ.Controls.Add(m_UserTDLYXZProperyShow);

                tabControl1.SelectedTab = this.tabItemTDXZ;
                tabControl1.Refresh();
            }
            catch(SystemException errs)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this, "填入土地利用现状详细信息时发生错误：" + errs.Message);
            }
        }

        private void loadGHTProp(IFeatureClass m_FeatureClass)
        {
            // 刘扬 修改 2011后期 分析 
            if (m_FeatureClass == null)
                return;


            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemTDGH.Visible = false;
                return;
            }

            if (m_UserGHTProperyShow == null)
            {
                m_UserGHTProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
             m_UserGHTProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析
            m_UserGHTProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
           }

            m_UserGHTProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserGHTProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH; // 规划数据 刘扬 修改 2011 分析

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "规划数据clip";

            // 获取图层名 岳建伟20110920
            string type = "规划数据";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserGHTProperyShow.SetData(m_layer as ILayer);
            m_UserGHTProperyShow.Dock = DockStyle.Fill;
            //this.tabTDGH.Controls.Clear();
            //this.tabTDGH.Controls.Add(m_UserGHTProperyShow);

            this.tabControlPanelTDGH.Controls.Clear();
            this.tabControlPanelTDGH.Controls.Add(m_UserGHTProperyShow);
            this.tabItemTDGH.Visible = true;
            tabControl1.SelectedTab = this.tabItemTDGH;

        }
        private void loadTDGYProp(IFeatureClass m_FeatureClass)
        {
            // 刘扬 修改 2011后期 分析 
            if (m_FeatureClass == null)
                return;


            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemTDGY.Visible = false;
                return;
            }
            if (m_UserTDGYProperyShow == null)
            {
                m_UserTDGYProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
            m_UserTDGYProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
             m_UserTDGYProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析
           }

            m_UserTDGYProperyShow.m_theDataAccess = m_DataAccess_SYS; // 岳建伟 修改 2011 分析
            m_UserTDGYProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY; // 土地供应数据 岳建伟修改 2011 分析

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "土地供应clip";

            // 获取图层名 岳建伟20110920
            string type = "土地供应";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserTDGYProperyShow.SetData(m_layer as ILayer);
            m_UserTDGYProperyShow.Dock = DockStyle.Fill;
            //this.tabTDGY.Controls.Clear();
            //this.tabTDGY.Controls.Add(m_UserTDGYProperyShow);
            this.tabControlPanelTDGY.Controls.Clear();
            this.tabControlPanelTDGY.Controls.Add(m_UserTDGYProperyShow);
            this.tabItemTDGY.Visible = true;
            tabControl1.SelectedTab = this.tabItemTDGY;

        }
        private void loadKczyProp(IFeatureClass m_FeatureClass)
        {
            // 刘扬 修改 2011后期 分析 
            if (m_FeatureClass == null)
                return;


            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemKCZYGH.Visible = false;
                return;
            }

            if (m_UserKCZYProperyShow == null)
            {
                m_UserKCZYProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
             m_UserKCZYProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析 
             m_UserKCZYProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
          }

            m_UserKCZYProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserKCZYProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH; // 矿产资源规划 


            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "矿产资源规划clip";

            // 获取图层名 岳建伟20110920
            string type = "矿产资源规划";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserKCZYProperyShow.SetData(m_layer as ILayer);
            m_UserKCZYProperyShow.Dock = DockStyle.Fill;
            //this.tabKCGH.Controls.Clear();
            //this.tabKCGH.Controls.Add(m_UserKCZYProperyShow);

            this.tabControlPanelKCZYGH.Controls.Clear();
            this.tabControlPanelKCZYGH.Controls.Add(m_UserKCZYProperyShow);
            this.tabItemKCZYGH.Visible = true;
            tabControl1.SelectedTab = this.tabItemKCZYGH;
        }

        private void loadCKQProp(IFeatureClass m_FeatureClass)
        {
            // 刘扬 修改 2011后期 分析 
            if (m_FeatureClass == null)
                return;

            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemCKQ.Visible = false;
                return;
            }
            if (m_UserCKQProperyShow == null)
            {
                m_UserCKQProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
             m_UserCKQProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析 
            m_UserCKQProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
           }
            m_UserCKQProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserCKQProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ; // 采矿权 

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "采矿权clip";

            // 获取图层名 岳建伟20110920
            string type = "采矿权";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;


            m_UserCKQProperyShow.SetData(m_layer as ILayer);
            m_UserCKQProperyShow.Dock = DockStyle.Fill;
            //this.tabCKQ.Controls.Clear();
            //this.tabCKQ.Controls.Add(m_UserCKQProperyShow);

            this.tabControlPanelCKQ.Controls.Clear();
            this.tabControlPanelCKQ.Controls.Add(m_UserCKQProperyShow);
            this.tabItemCKQ.Visible = true;
            tabControl1.SelectedTab = this.tabItemCKQ;
        }
        private void loadTKQProp(IFeatureClass m_FeatureClass)
        {
            // 刘扬 修改 2011后期 分析 
            if (m_FeatureClass == null)
                return;

            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemTKQ.Visible = false;
                return;
            }
            if (m_UserTKQProperyShow == null)
            {
                m_UserTKQProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
            m_UserTKQProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析
            m_UserTKQProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            }
            m_UserTKQProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析 
            m_UserTKQProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ; // 探矿权 


            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "探矿权clip";

            // 获取图层名 岳建伟20110920
            string type = "探矿权";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ );
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserTKQProperyShow.SetData(m_layer as ILayer);
            m_UserTKQProperyShow.Dock = DockStyle.Fill;
            //this.tabTKQ.Controls.Clear();
            //this.tabTKQ.Controls.Add(m_UserTKQProperyShow);

            this.tabControlPanelTKQ.Controls.Clear();
            this.tabControlPanelTKQ.Controls.Add(m_UserTKQProperyShow);
            this.tabItemTKQ.Visible = true;
            tabControl1.SelectedTab = this.tabItemTKQ;
        }
        #endregion

        private void btnSaveResult_Click(object sender, EventArgs e)
        {
// 1）在随意绘制一个地块时，如果点击“保存结果”，则先将绘制的临时地块存入“土地核查”图层中，然
//后获得其ID号（oid 或者是OBJECTID）后，再将分析结果存入分析表中。首先还是要判断是否已经存入“土地核
//查”图层中，否则容易存入多条相同的记录

//2）对于选择地块进行分析，则不需要再保存地块了，而需要获得其ID号（oid 或者是OBJECTID）后，再将分析结果存入分析表中。
        }



        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //tabControl2.SelectedTab = tabItem3;

                //m_DataAccess_SYS.OutputExcel(m_ResultDataGrid, "土地核查空间分析结果", "");
                if (chkTdlyxz.Checked && this.m_UserTDLYXZProperyShow.m_PropertyDataGrid != null)
                {
                    //m_DataAccess_SYS.OutputExcel(this.m_UserTDLYXZProperyShow.m_PropertyDataGrid
                    this.m_DataAccess_SYS.OutputExcel(this.m_UserTDLYXZProperyShow.m_PropertyDataGrid, "现状地物分析结果", "");

                }
                if (this.chkTDLYGH.Checked && this.m_UserGHTProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserGHTProperyShow.m_PropertyDataGrid, "规划图分析结果", "");
                }
                if (this.chkTDGY.Checked && this.m_UserTDGYProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserTDGYProperyShow.m_PropertyDataGrid, "土地供应分析结果", "");
                }
                if (chkCkq.Checked && this.m_UserCKQProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserCKQProperyShow.m_PropertyDataGrid, "采矿权分析结果", "");
                }
                if (this.chkTkq.Checked && this.m_UserTKQProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserTKQProperyShow.m_PropertyDataGrid, "探矿权分析结果", "");
                }
                if (this.chkJsyd.Checked && this.m_UserJSYDProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserJSYDProperyShow.m_PropertyDataGrid, "建设用地分析结果", "");
                }
                if (this.chkKczygh.Checked && this.m_UserKCZYProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserKCZYProperyShow.m_PropertyDataGrid, "矿产资源分布图分析结果", "");

                }
                if (this.chkJbnt.Checked && this.m_UserJBNTProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserJBNTProperyShow.m_PropertyDataGrid, "基本农田分析结果", "");
                }
            }
            catch (Exception ex)
            { }

        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            this.Width = 839;
            this.Height = 502;
            this.btnNoDetail.Enabled = true;
            this.btnDetail.Enabled = false;
        }

        private void btnNoDetail_Click(object sender, EventArgs e)
        {
            this.Width = 422;
            this.Height = 502;
            this.btnDetail.Enabled = true;
            this.btnNoDetail.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
                //if (hcjg != null)
                //{
                //    m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);
                //}
                //if (System.IO.Directory.Exists(Application.StartupPath + "\\OverlayTemp"))
                //{
                //    DeleteFolder(Application.StartupPath + "\\OverlayTemp");
                //}

                if (true == RemoveTempLayer())
                {
                    //MessageBox.Show("删除成功");

                    DeleteTempData();
                }
                else
                {
                    //MessageBox.Show("对不起，删除失败");
                }
            }
            catch (Exception ex)
            { }
            this.Close();
        }

       

        /// <summary>
        /// 删除临时图层
        /// </summary>
        private bool RemoveTempLayer()
        {

            try
            {
              
                //m_EnumLayer.Reset();
                string[] m_strLayerNames = new string[1] {  "地块分析临时图层" };
                if (clsMapFunction.MapFunction.RemoveLayer(m_strLayerNames, this.m_AxMapControl))
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
                //IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
                //if (hcjg != null)
                //{
                //    axMapControl1.Map.DeleteLayer((ILayer)hcjg);

                //}
               clsMapFunction.MapFunction.RemoveLayerGroup("核查结果", this.m_AxMapControl);

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
                    File.Delete(d);//直接删除其中的文件   
                }
                else
                    DeleteFolder(d);//递归删除子文件夹   
            }
            Directory.Delete(dir);//删除已空文件夹   
        }
        private void frmSelAnalysis_Load(object sender, EventArgs e)
        {
            JCZF.SubFrame.DatabaseString.m_DataAccess_SYS = this.m_DataAccess_SYS; //  获取数据库连接属性 刘扬 修改 2011 分析

            // 获取统计图层列表 刘扬 修改 2011后期 土地分析
            string settingfilePath = System.IO.Path.Combine(Application.StartupPath, CGlobalVarable.m_strSettingFileOfStaticsLayers);
            CGlobalVarable.GetNameListOfStaticsLayers(settingfilePath);
            ArrayList theStatisticsLayerNameList = CGlobalVarable.m_listNameOfStaticsLayers;

            m_blHasAnalysis = new bool[8];
            //m_theButtonList.Clear();
            //m_theButtonList.Add(this.btnTDLY); // 添加“原”按钮：土地利用
            //m_theButtonList.Add(this.btnGH); // 添加“原”按钮：规划数据
            //m_theButtonList.Add(this.btnJBNT); // 添加“原”按钮：基本农田
            //m_theButtonList.Add(this.btnJSYD); // 添加“原”按钮：建设用地
            //m_theButtonList.Add(this.btnKCZY); // 添加“原”按钮：矿产资源
            //m_theButtonList.Add(this.btnCKQ); // 添加“原”按钮：采矿权
            //m_theButtonList.Add(this.btnTKQ); // 添加“原”按钮：探矿权

            if (m_blIsDrawPolygon == false)
            {
                // 显示被统计过的按钮（原）

                //刘丽注释掉
                //ShowHasBeenStatisticedButton();
            }

            //位置设置 岳建伟 20110731
            this.Left = 5;
            this.Top = 200;

        }

        #region 刘扬 修改 2011 分析

        // 设置详细信息窗体显示
        private void SetDetailFormShow()
        {
            this.Width = 839;
            this.Height = 502;
            this.btnNoDetail.Enabled = true;
            this.btnDetail.Enabled = false;
        }

        // 设置textbox滚动到最下面
        private void SetTextBoxScrollToBottom()
        {
            this.txtResult.SelectionStart = this.txtResult.Text.Length;
            this.txtResult.ScrollToCaret();

        }

        // 显示被统计过的按钮（原）
        //private void ShowHasBeenStatisticedButton()
        //{

        //    int i = 0;
        //    foreach (string theType in JCZF.Renderable.CGlobalVarable.m_listStaticsContents) // 8种分析类型循环
        //    {

        //        int index = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(theType);
        //        //Button theButton = m_theButtonList[index] as Button;
        //        bool bStatisticed = this.HasBeenStatisticed(theType); // 判断是否被统计过
        //        //theButton.Visible = bStatisticed; // 统计过则显示，否则不显示

        //        m_blHasAnalysis[i] = bStatisticed;

        //        //if (bStatisticed) // 为了判断有几项分析过
        //        //    i++;
        //    }

        //    //if (i == 7) // 标题提示
        //    //{
        //    //    this.Text = String.Format("图斑分析-全部项目已经分析过");
        //    //}
        //    //else
        //    //    this.Text = String.Format("图斑分析-已有{0}项分析结果", i);
        //}

        // 根据统计类型获取统计结果数据
        private DataRow GetStatisticResultData(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(type); // 表名
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // 表中关键字列名
            string theID = GetStaticsTBBH(); // 表中关键字的值
            //return this.m_theSqlOp.GetOneRow(tableName, theIDName, theID);
            return m_DataAccess_SYS.GetRowByColmn(theIDName, theID,tableName);
        }

        // 显示统计结果
        private void ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type)
        {
            DataRow theRow = GetStatisticResultData(type); // 根据统计类型获取统计结果数据
            string theTime = theRow[3].ToString(); // 分析时间
            string theResultShow = String.Format("已核查结果为：(分析时间({0}))", theTime);
            string theResult = ReTransforToFitableSQL(theRow[2].ToString().Trim()); // 分析结果
            theResultShow += theResult;
            theResultShow += "----------------------------------------------------------" + "\r\n";

            // 获取图层名称
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
            JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theRow[1].ToString().Trim(); // 获取图层名称

            this.txtResult.Text += theResultShow;

            SetTextBoxScrollToBottom();  // 设置显示结果的textbox滚动到最下面
            SetDetailFormShow(); // 设置显示详细窗体
        }

        // 显示统计详细信息
        private void ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type, UserProperyShow theShowControl)
        {
            theShowControl = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            theShowControl.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            theShowControl.m_theType = type; // 土地利用 刘扬 修改 2011 分析

            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsInfo(type); // 表名
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName; // 表中关键字列名
            string theID = GetStaticsTBBH(); // 表中关键字的值
            string theSQL = String.Format("Select * from {0} Where [{1}] like '{2}_%' ", tableName, theIDName, theID);
            //DataTable theData = this.m_theSqlOp.GetTableWithstrQuery(theSQL);// 获取数据
            DataTable theData = m_DataAccess_SYS.getDataTableByQueryString(theSQL);// 获取数据

            theShowControl.SetDataAfterStatisticed(theData);
            theShowControl.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            theShowControl.Dock = DockStyle.Fill;

            int index = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type); // 获取索引
            int tabIndex = JCZF.Renderable.CGlobalVarable.m_listTabIndexOfStaticsInfo[index]; // 获取tab索引，目前是一致的与index
            //this.tabTDFX.TabPages[tabIndex].Controls.Clear();
            //this.tabTDFX.TabPages[tabIndex].Controls.Add(theShowControl);
            //this.tabTDFX.SelectedIndex = tabIndex;

            this.tabControl1.Tabs[tabIndex].AttachedControl.Controls.Clear();
            this.tabControl1.Tabs[tabIndex].AttachedControl.Controls.Add(theShowControl);
            this.tabControl1.SelectedTab = this.tabControl1.Tabs[tabIndex];
        }

        // 按钮：原-土地利用
        private void btnTDLY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[0]; // 土地利用现状
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);   // 显示统计结果
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ, m_UserTDLYXZProperyShow);  // 显示统计详细信息
        }

        // 按钮：原-规划数据
        private void btnGH_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[1]; // 规划数据
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);   // 显示统计结果
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH , m_UserGHTProperyShow);  // 显示统计详细信息

        }


        // 按钮：土地供应
        private void btnTDGY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[2]; // 规划数据
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY);   // 显示统计结果
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY , m_UserTDGYProperyShow);  // 显示统计详细信息

        }




        // 按钮：原-基本农田
        private void btnJBNT_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[2]; // 基本农田 添加例子
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);   // 显示统计结果 添加例子
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT , m_UserJBNTProperyShow);  // 显示统计详细信息 添加例子
        }


        // 按钮：原-建设用地
        private void btnJSYD_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[3]; // 建设用地 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);   // 显示统计结果 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD , m_UserJSYDProperyShow);  // 显示统计详细信息 

        }


        // 按钮：原-矿产资源
        private void btnKCZY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[4]; // 矿产资源 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH);   // 显示统计结果 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH, m_UserKCZYProperyShow);  // 显示统计详细信息 
        }


        // 按钮：原-采矿权
        private void btnCKQ_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[5]; // 采矿权 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ);   // 显示统计结果 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ , m_UserCKQProperyShow);  // 显示统计详细信息 
        }



        // 按钮：原-探矿权
        private void btnTKQ_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[6]; // 探矿权 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ);   // 显示统计结果 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ , m_UserTKQProperyShow);  // 显示统计详细信息 
        }


        #endregion

        private void btnSelectDk_Click(object sender, EventArgs e)
        {
            m_frmMapView.axMapControl1.CurrentTool = null;
            m_frmMapView.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;

            m_frmMapView.b_hcselectDK = true;
            m_frmMapView.b_hcDrawDK = false;

            //if (this.GetDKClick!= null)
            //{
            //    this.GetDKClick();

            //}
        }

        private void btnDrawDk_Click(object sender, EventArgs e)
        {
            m_frmMapView.axMapControl1.CurrentTool = null;
            m_frmMapView.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;

            m_frmMapView.b_hcDrawDK = true;
            m_frmMapView.b_hcselectDK = false;
        }

        private void btnImportDk_Click(object sender, EventArgs e)
        {
            //m_frmMapView.b_hcImportDK = true;
            importDK();

        }


        private void importDK()
        {
            string file = ImportCoor();
            if (file == null || file == "") return;
            StreamReader sr = new StreamReader(file);
            ArrayList arr = new ArrayList();
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                string[] split = str.Split(new char[] { ';' });
                foreach (string s in split)
                {
                    if (s.Trim() != "")
                        arr.Add(s);
                }
            }

            try
            {
                double[,] xy = getxyCoord(arr, Convert.ToInt32(arr[5].ToString()));

                IPointCollection pMultiPoint = addtomulpt(xy);

                ISegmentCollection pSegCol;
                pSegCol = new Ring() as ISegmentCollection ;
                object Missing1 = Type.Missing;
                object Missing2 = Type.Missing;

                for (int i = 0; i < pMultiPoint.PointCount - 1; i++)
                {
                    ILine pLine = new Line() as ILine;
                    pLine.PutCoords(pMultiPoint.get_Point(i), pMultiPoint.get_Point(i + 1));
                    pSegCol.AddSegment(pLine as ISegment, ref  Missing1, ref Missing2);
                }

                IRing pRing;
                pRing = pSegCol as IRing;

                pRing.Close();
                IGeometryCollection pPolygon;
                pPolygon = new Polygon() as IGeometryCollection ;
                pPolygon.AddGeometry(pRing, ref Missing1, ref Missing2);

                IGeometry geometry = pPolygon as IGeometry;
                geometry.SpatialReference = this.m_AxMapControl.SpatialReference;             

                //把改图形加入到土地核查图层   修改 刘丽 添加到临时图层 （将地图操作工具栏内分析中绘制功能和导入数据所绘制图形修改为放在临时层上，当关闭分析对话框后删除该图形）

                string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
                //裁切结果路径
                string m_strResultFilePath = Application.StartupPath + "\\临时图层" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }


                IFeatureLayer m_hcfeaturelayer = MapFunction.getFeatureLayerByName("土地核查", m_AxMapControl);
                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                IFields outfields = m_hcfeaturelayer.FeatureClass.Fields;
                IFeatureClass m_FeatureClass = pFWS.CreateFeatureClass("土地核查", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


                IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
                m_layer.FeatureClass = m_FeatureClass;
                m_layer.Name = m_FeatureClass.AliasName;
                this.m_AxMapControl.Map.AddLayer(m_layer);

                IWorkspaceEdit workspaceEdit = pFWS as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                IFeature feature = m_FeatureClass.CreateFeature();
                feature.Shape = geometry;
                try
                {
                    feature.Store();
                }
                catch
                {
                    ITopologicalOperator topologicaloperator = pPolygon as ITopologicalOperator;
                    topologicaloperator.Simplify();
                    feature.Store();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                m_AxMapControl.Extent = feature.Extent;

                //高亮显示
                IFeatureSelection featureSelection = m_layer as IFeatureSelection;
                featureSelection.Add(feature);

                IRgbColor m_color = new RgbColor() as IRgbColor;
                m_color.Red = 207;
                m_color.Green = 70;
                m_color.Blue = 215;
                featureSelection.SelectionColor = m_color;
                this.m_AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, m_layer, null);

                this.m_selFeature = feature;
                FillHCListview(feature.ShapeCopy);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件格式不对！");
            }
            
            m_AxMapControl.ActiveView.Refresh();
        }


        //导入地块

        private string ImportCoor()
        {
            string s = "";
            OpenFileDialog pDlg = new OpenFileDialog();				//打开文件对话框
            pDlg.Title = "打开文本文件";

            pDlg.Filter = "文本文件(*.txt)|*.txt";

            if (pDlg.ShowDialog() != DialogResult.OK)
            {
                return s;
            }
            string filepath = System.IO.Path.GetDirectoryName(pDlg.FileName);
            string filename = System.IO.Path.GetFileName(pDlg.FileName);

            s = filepath + "\\" + filename;

            return s;
            //StreamReader sr = new StreamReader(filepath + "\\" + filename);


        }

        //public void FillHCListview(IGeometry geo)
        //{
        //    listViewEx1.Items.Clear();

        //    ListViewItem listviewitem;

        //    IPointCollection pcollection = geo as IPointCollection;

        //    for (int i = 0; i < pcollection.PointCount; i++)
        //    {
        //        IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
        //        pcollection.QueryPoint(i, pt);

        //        listviewitem = new ListViewItem(i.ToString());

        //        listviewitem.SubItems.Add(pt.X.ToString());

        //        listviewitem.SubItems.Add(pt.Y.ToString());
        //        listViewEx1.Items.Add(listviewitem);
        //    }

        //}



        //存储坐标  
        private double[,] getxyCoord(ArrayList arr, int ddcount)
        {
            double[,] xy = new double[ddcount, 2];
            string m_strX;
            double[] m_dbXY;
            for (int j = 0; j < ddcount; j++)
            {
                if (arr[j + 6].ToString().Contains(","))
                {
                    string[] split = arr[j + 6].ToString().Split(new char[] { ',' });

                    xy[j, 0] = Convert.ToDouble(split[0].ToString());
                    xy[j, 1] = Convert.ToDouble(split[1].ToString());

                    /////////////////////////20111023 岳建伟
                    m_strX=((int)xy[j, 0] ).ToString();
                    if (m_strX.Length == 8)//是否有带号
                    {
                        //转换成与所显示地图坐标系一致的坐标 
                        m_dbXY = clsMapFunction.clsCoordinateConvert.ZBDZH(xy[j, 0], xy[j, 1], m_AxMapControl);
                        xy[j, 0] = m_dbXY[0];
                        xy[j, 1] = m_dbXY[1];                       
                    }
                    /////////////////////
                }
            }

            return xy;

        }

        //生成点，并增加点到multipoint
        private IPointCollection addtomulpt(double[,] xy)
        {
            IPointCollection pMultiPoint = new Multipoint() as IPointCollection ;
            for (int i = 0; i < xy.Length / 2; i++)
            {
                IPoint point = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                point.PutCoords(xy[i, 0], xy[i, 1]);
                object missing = Type.Missing;

                pMultiPoint.AddPoint(point, ref missing, ref missing);
            }
            return pMultiPoint;
        }


        private void SetTabItemUnVisible()
        {

            if (chkCkq.Checked)
            {
                tabItemCKQ.Visible = true;
            }
            else
            {
                tabItemCKQ.Visible = false;
            }
            if (chkTDGY.Checked)
            {
                tabItemTDGY.Visible = true;
            }
            else
            {
                tabItemTDGY.Visible = false;
            }
            if (chkTDLYGH.Checked)
            {
                tabItemTDGH.Visible = true;
            }
            else
            {
                tabItemTDGH.Visible = false;
            }
            if (chkJbnt.Checked)
            {
                tabItemJBNT.Visible = true;
            }
            else
            {
                tabItemJBNT.Visible = false;
            }
            if (chkJsyd.Checked)
            {
                tabItemJSYD.Visible = true;
            }
            else
            {
                tabItemJSYD.Visible = false;
            }
            if (chkKczygh.Checked)
            {
                tabItemKCZYGH.Visible = true;
            }
            else
            {
                tabItemKCZYGH.Visible = false;
            }
            if (chkTdlyxz.Checked)
            {
                tabItemTDXZ.Visible = true;
            }
            else
            {
                tabItemTDXZ.Visible = false;
            }
            if (chkTkq.Checked)
            {
                tabItemTKQ.Visible = true;
            }
            else
            {
                tabItemTKQ.Visible = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SetCheckBoxChecked(false );
        }

        private void btnZBReverse_Click(object sender, EventArgs e)
        {
            if (listViewEx1.Items == null || listViewEx1.Items.Count < 1) return;
            string[,] m_strZB = new string[listViewEx1.Items.Count, 2];

            int m = 0;
            for (int i =  listViewEx1.Items.Count-1; i >=0 ; i--)
            {
                m_strZB[m, 0] = listViewEx1.Items[i].SubItems[1].Text;
                m_strZB[m, 1] = listViewEx1.Items[i].SubItems[2].Text;
                m++;
            }

           



            for (int i = 0; i < m_strZB.Length/2; i++)
            {
                listViewEx1.Items[i].SubItems[1].Text = m_strZB[i, 0];
                listViewEx1.Items[i].SubItems[2].Text=m_strZB[i, 1] ;
            }

            listViewEx1.Refresh();
        }

       
       
    }
}