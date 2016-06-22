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


namespace JCZF.SubFrame.AttibuteEdit
{
    public partial class frmSelAnalysis : DevComponents.DotNetBar.Office2007Form
    {
        public string m_strAnalysisLayerName;
        public bool m_blIsDrawPolygon;

        private bool[] m_blHasAnalysis;
        public clsDataAccess.DataAccess m_DataAccess_SYS;

        public IFeature m_selFeature;

        public IGeometry m_SelectedGeometry;

        //private frmMapView m_frmMapView = null;

        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl;

        private IEnumLayer m_Grouplayers;

        private UserProperyShow m_UserJBNTProperyShow;
        private UserProperyShow m_UserJSYDProperyShow;
        private UserProperyShow m_UserDLTBProperyShow;
        private UserProperyShow m_UserGHTProperyShow;
        private UserProperyShow m_UserTDGYProperyShow;
        private UserProperyShow m_UserKCZYProperyShow;
        private UserProperyShow m_UserCKQProperyShow;
        private UserProperyShow m_UserTKQProperyShow;

        System.Collections.ArrayList m_theButtonList = new System.Collections.ArrayList(); // 保存7个按钮(原) 刘扬 修改 2011 分析


        //CSQLOperation m_theSqlOp = new CSQLOperation(); // Sql查询 刘扬 修改 2011 分析
        //public clsDataAccess.DataAccess m_DataAccess_SYS;

        public frmSelAnalysis(ESRI.ArcGIS.Controls.AxMapControl p_AxMapControl)
        {
            InitializeComponent();
            m_AxMapControl = p_AxMapControl;
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
            string m_strTBBH = "";
            m_strTBBH = GetOneFieldOftheFeatureWithFiledName(m_selFeature, "tbbhx");
            if (m_strTBBH == "")
            {
                m_strTBBH = GetOneFieldOftheFeatureWithFiledName(m_selFeature, "dkid");            
            }
            return m_strTBBH;
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
        private bool SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_type, string theResult, string theLayerNameOfStatics)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(p_type); // 表名
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // 表中关键字列名
            string theID = GetStaticsTBBH(); // 表中关键字的值

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
            return m_DataAccess_SYS.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);

        }

        // 根据一行数据构建一个列表
        private void BuildOneListWithOneRow(DataRow theRow,ref System.Collections.ArrayList  theList)
        {
            ////string [] theDataTypeList = JCZF.Renderable.CGlobalVarable.GetFitalbeColDataTypeListOfStatisticsInfo (type)

            for (int i = 0; i < theRow.Table.Columns.Count ; i++)
            {
                theList.Add(theRow[i]);  
            }       

        }


        // 保存分析详细信息到数据库中
        private bool SaveStaticsInfoToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_type , DataTable  theInfoTable)
        {


            try
            {
                string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsInfo(p_type); // 表名
                string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName; //编号 表中关键字列名
                string theTBBH = GetStaticsTBBH(); // 获取图斑编号

                System.Collections.ArrayList theColNameList = new System.Collections.ArrayList(); // 插入列名集合

                theColNameList.Add(theIDName); // 编号列

                string[] theColNameListString = JCZF.Renderable.CGlobalVarable.GetTableColNameListOfStatisticsInfo(p_type);

                foreach (string theColName in theColNameListString) // 构造列名集合
                {
                    theColNameList.Add(theColName);
                }

                System.Collections.ArrayList theColValList = new System.Collections.ArrayList();// 插入列数据集合


                //m_theSqlOp.DelAllCorrespondingRecordWithID(tableName, theIDName, theTBBH); // 删除所有相关ID的记录
                m_DataAccess_SYS.DeleteByColumnValue(theIDName, theTBBH, tableName);

                for (int i = 0; i < theInfoTable.Rows.Count; i++) // 按行存储
                {

                    int index = i + 1;// 设置编号的序号

                    theColValList.Clear();

                    string theID = String.Format("{0}_{1}", theTBBH, index); // 构造编号
                    theColValList.Add(theID); // 添加编号 
                    BuildOneListWithOneRow(theInfoTable.Rows[i], ref theColValList);   // 根据一行数据构建一个列表

                    //m_theSqlOp.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);   // 插入数据
                    m_DataAccess_SYS.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);

                }


                return true;
            }
            catch(Exception ee)
            { return false; }
        }


        
        // 判断是否已经被统计
        private bool HasBeenStatisticed(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_type )
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(p_type); // 表名
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // 表中关键字列名
            string theID = GetStaticsTBBH(); // 表中关键字的值
            if (theID == "")
            {
                return false;
            }

            return m_DataAccess_SYS.IsRecordExist(theIDName, theID, tableName);//yuejianwei 20120314

            //return m_theSqlOp.ExistKeyIntheTable(tableName, theIDName, theID);

        }

        #endregion

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.chkCkq.Checked = true;
            this.chkGhsj.Checked = true;
            this.chkJbnt.Checked = true;
            this.chkJsyd.Checked = true;
            this.chkKczygh.Checked = true;
            this.chkTdlyxz.Checked = true;
            this.chkTkq.Checked = true;   
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Text = "";

                IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
                if (hcjg != null)
                {
                    m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);

                }

                this.progressBar1.Value = 10;

                this.txtResult.Text = "开始分析计算！\r\n\r\n";

                //this.txtResult.Text = strTemp;

                clsClipStat m_clsClipStat = new clsClipStat();
                m_clsClipStat.m_strFilePath = Application.StartupPath + "\\OverlayTemp";
                m_clsClipStat.m_axMapcontrol = m_AxMapControl;
                //写死 土地核查图层 （因为通过属性表传来时，没有m_sellayer参数，即m_sellayer为null）
                if (m_strAnalysisLayerName == "")
                { m_strAnalysisLayerName = "土地核查"; }
            
                ILayer selLayer = MapFunction.getFeatureLayerByName(m_strAnalysisLayerName, m_AxMapControl);

                m_clsClipStat.m_selLayer = selLayer;
                m_clsClipStat.pDataAcess = m_DataAccess_SYS;
                if (m_SelectedGeometry == null)
                {
                    m_clsClipStat.m_Geometry = this.m_selFeature.ShapeCopy;
                    m_clsClipStat.CreateClipFeatureClass(this.m_selFeature.ShapeCopy);
                }
                else
                {
                    m_clsClipStat.m_Geometry = m_SelectedGeometry;
                    m_clsClipStat.CreateClipFeatureClass(m_SelectedGeometry);
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
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                    {
                                        break;
                                    }
                                }

                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;

                                AddtoTOC(m_clsClipStat.pDltbOutputFeatClass, "土地利用现状clip");
                                loadDLTBProp(m_clsClipStat.pDltbOutputFeatClass);
                                this.progressBar1.Value = 30;

                                #region // 刘扬 修改 2011 分析

                                string type = "土地利用现状"; // 分析类型
                                string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                // 获取图层名
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);
                                
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ, theResult, theLayerName);// 保存分析结果


                                #endregion

                                //txtResult.Text = m_clsClipStat.strHCJG;
                                //MessageBox.Show(m_clsClipStat.strHCJG);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "土地利用现状数据有错误，请检查数据" + "\r\n";
                        //this.txtResult.Text =strTemp;
                    }
                    try
                    {
                        //规划
                        if (chkGhsj.Checked)
                        {


                            if (m_blHasAnalysis[1] == true)
                            {
                                btnGH_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.QHTexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                    {
                                        break;
                                    }
                                }
                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;
                                AddtoTOC(m_clsClipStat.pGHTOutputFeatClass, "规划数据clip");
                                loadGHTProp(m_clsClipStat.pGHTOutputFeatClass);
                                this.progressBar1.Value = 50;

                                #region // 刘扬 修改 2011 分析

                                string type = "规划数据"; // 分析类型
                                string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                // 获取图层名
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH, theResult, theLayerName);// 保存分析结果


                                #endregion
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "土地利用规划数据有错误，请检查数据" + "\r\n";
                        //this.txtResult.Text = strTemp;
                    }

                    try
                    {
                        //土地供应
                        if (chkGDSJ.Checked)
                        {


                            if (m_blHasAnalysis[2] == true)
                            {
                                btnGH_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.TDGYexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                    {
                                        break;
                                    }
                                }
                                this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                this.txtResult.Text += "----------------------------------------------------";
                                //this.txtResult.Text = strTemp;
                                AddtoTOC(m_clsClipStat.pTDGYOutputFeatClass, "土地供应clip");
                                loadGHTProp(m_clsClipStat.pTDGYOutputFeatClass);
                                this.progressBar1.Value = 50;

                                #region // 岳建伟 修改 2011 分析

                                string type = "土地供应"; // 分析类型
                                string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                // 获取图层名
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY, theResult, theLayerName);// 保存分析结果


                                #endregion
                            }

                        }
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


                            if (m_blHasAnalysis[3] == true)
                            {
                                btnJBNT_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.JBNTexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                    {
                                        break;
                                    }
                                }

                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;
                                AddtoTOC(m_clsClipStat.pJbntOutputFeatClass, "基本农田clip");
                                loadJBNTProp(m_clsClipStat.pJbntOutputFeatClass);
                                this.progressBar1.Value = 60;

                                #region  刘扬 修改 2011 分析  添加例子

                                string type = "基本农田"; // 分析类型
                                string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                // 获取图层名
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT, theResult, theLayerName);// 保存分析结果


                                #endregion
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "基本农田数据有错误，请检查数据" + "\r\n";
                        //this.txtResult.Text = strTemp ;
                    }
                    try
                    {
                        //建设项目
                        if (chkJsyd.Checked)
                        {

                            if (m_blHasAnalysis[4] == true)
                            {
                                btnGH_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.JSYDexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                    {
                                        break;
                                    }
                                }
                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;
                                if (m_clsClipStat.pJsydOutputFeatClass != null)
                                {
                                    AddtoTOC(m_clsClipStat.pJsydOutputFeatClass, "建设用地数据clip");
                                    loadJSYDProp(m_clsClipStat.pJsydOutputFeatClass);
                                }
                               
                                this.progressBar1.Value = 70;

                                #region  刘扬 修改 2011 分析

                                string type = "建设用地数据"; // 分析类型
                                string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                // 获取图层名
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD, theResult, theLayerName);// 保存分析结果


                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "建设项目审批数据有错误，请检查数据" + "\r\n";
                        //this.txtResult.Text = strTemp ;
                    }
                    try
                    {
                        //矿产资源规划
                        if (chkKczygh.Checked)
                        {
                            if (m_blHasAnalysis[5] == true)
                            {
                                btnKCZY_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.KCZYexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                    {
                                        break;
                                    }
                                }

                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;

                                AddtoTOC(m_clsClipStat.pKczyOutputFeatClass, "矿产资源规划clip");
                                loadKczyProp(m_clsClipStat.pKczyOutputFeatClass);
                                this.progressBar1.Value = 80;


                                #region  刘扬 修改 2011 分析

                                string type = "矿产资源规划"; // 分析类型
                                string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                // 获取图层名
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH, theResult, theLayerName);// 保存分析结果


                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "矿产资源规划数据有错误，请检查数据" + "\r\n";
                        //this.txtResult.Text = strTemp ;
                    }
                    //采矿
                    try
                    {
                        if (chkCkq.Checked)
                        {
                            if (m_blHasAnalysis[6] == true)
                            {
                                btnCKQ_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.CKQexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                    {
                                        break;
                                    }
                                }

                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;

                                AddtoTOC(m_clsClipStat.pCkqOutputFeatClass, "采矿权clip");
                                loadCKQProp(m_clsClipStat.pCkqOutputFeatClass);
                                this.progressBar1.Value = 90;

                                #region  刘扬 修改 2011 分析

                                string type = "采矿权"; // 分析类型
                                string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                                // 获取图层名
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ );
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ, theResult, theLayerName);// 保存分析结果


                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "采矿权登记数据有错误，请检查数据" + "\r\n";
                        //this.txtResult.Text = strTemp;
                    }
                    try
                    {
                        //探矿
                        if (chkTkq.Checked)
                        {
                            if (m_blHasAnalysis[7] == true)
                            {
                                btnTKQ_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.TKQexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                    {
                                        break;
                                    }
                                }
                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;

                                AddtoTOC(m_clsClipStat.pTkqOutputFeatClass,"探矿权clip");
                                loadTKQProp(m_clsClipStat.pTkqOutputFeatClass);
                                this.progressBar1.Value = 100;
                            }

                            //this.txtResult.Text = strTemp;
                            this.progressBar1.Value = 100;

                            #region  刘扬 修改 2011 分析

                            string type = "探矿权"; // 分析类型
                            string theResult = m_clsClipStat.strHCJG; ;// 分析结果
                            string theLayerName = clsClipStat.m_strLayerNameOfStatics;// 图层名称
                            // 获取图层名
                            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ);
                            JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                            SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ, theResult, theLayerName);// 保存分析结果


                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "探矿权登记数据有错误，请检查数据";
                        //this.txtResult.Text = strTemp ;
                    }
                }
                catch (Exception o)
                { }
                this.progressBar1.Visible = false;
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
        private void AddtoTOC(IFeatureClass outputfeatclass, string layername)
        {
            IFeatureLayer outlayer = new FeatureLayer() as IFeatureLayer;
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
            m_UserJBNTProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserJBNTProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析 添加例子
            m_UserJBNTProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT; // 基本农田 添加例子
            m_UserJBNTProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析 添加例子

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "基本农田clip";

            // 获取图层名 刘丽20110814
            string type = "基本农田";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserJBNTProperyShow.SetData(m_layer as ILayer);

            m_UserJBNTProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            //this.gbJBNT.Controls.Add(m_UserJBNTProperyShow);
            m_UserJBNTProperyShow.Dock = DockStyle.Fill;
            this.tabPage3.Controls.Clear();
            this.tabPage3.Controls.Add(m_UserJBNTProperyShow);
        }
        void m_frmJBNTHCResult_FlashFeature(ESRI.ArcGIS.Geodatabase.IFeature pFeature)
        {
            m_AxMapControl.FlashShape(pFeature.Shape, 3, 300, null);
        }

        private void loadJSYDProp(IFeatureClass m_FeatureClass)
        {
            m_UserJSYDProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserJSYDProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserJSYDProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD; // 建设用地数据 
            m_UserJSYDProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析 

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "建设用地数据clip";

            // 获取图层名 刘丽20110814
            string type = "建设用地数据";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserJSYDProperyShow.SetData(m_layer as ILayer);
            m_UserJSYDProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserJSYDProperyShow.Dock = DockStyle.Fill;
            this.tabPage4.Controls.Clear();
            this.tabPage4.Controls.Add(m_UserJSYDProperyShow);
        }

        private void loadDLTBProp(IFeatureClass m_FeatureClass)
        {
            m_UserDLTBProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserDLTBProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserDLTBProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ; // 土地利用 刘扬 修改 2011 分析
            m_UserDLTBProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "土地利用现状clip";

            // 获取图层名 刘丽20110814
            string type = "土地利用现状";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;


            m_UserDLTBProperyShow.SetData(m_layer as ILayer);
            m_UserDLTBProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserDLTBProperyShow.Dock = DockStyle.Fill;
            this.tabPage1.Controls.Clear();
            this.tabPage1.Controls.Add(m_UserDLTBProperyShow);
        }

        private void loadGHTProp(IFeatureClass m_FeatureClass)
        {
            m_UserGHTProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);


            m_UserGHTProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserGHTProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH; // 规划数据 刘扬 修改 2011 分析
            m_UserGHTProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "规划数据clip";
            m_UserGHTProperyShow.SetData(m_layer as ILayer);
            m_UserGHTProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserGHTProperyShow.Dock = DockStyle.Fill;
            this.tabPage2.Controls.Clear();
            this.tabPage2.Controls.Add(m_UserGHTProperyShow);


        }

        private void loadTDGYProp(IFeatureClass m_FeatureClass)
        {
            m_UserTDGYProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);


            m_UserTDGYProperyShow.m_theDataAccess = m_DataAccess_SYS; // 岳建伟 修改 2011 分析
            m_UserTDGYProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY; // 土地供应数据 岳建伟修改 2011 分析
            m_UserTDGYProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "土地供应clip";
            m_UserTDGYProperyShow.SetData(m_layer as ILayer);
            m_UserTDGYProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserTDGYProperyShow.Dock = DockStyle.Fill;
            this.tabPage8.Controls.Clear();
            this.tabPage8.Controls.Add(m_UserTDGYProperyShow);


        }

        private void loadKczyProp(IFeatureClass m_FeatureClass)
        {
            m_UserKCZYProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);


            m_UserKCZYProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserKCZYProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH; // 矿产资源规划 
            m_UserKCZYProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析 


            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "矿产资源规划clip";
            m_UserKCZYProperyShow.SetData(m_layer as ILayer);
            m_UserKCZYProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserKCZYProperyShow.Dock = DockStyle.Fill;
            this.tabPage5.Controls.Clear();
            this.tabPage5.Controls.Add(m_UserKCZYProperyShow);
        }

        private void loadCKQProp(IFeatureClass m_FeatureClass)
        {
            m_UserCKQProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserCKQProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析
            m_UserCKQProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ; // 采矿权 
            m_UserCKQProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析 

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "采矿权clip";
            m_UserCKQProperyShow.SetData(m_layer as ILayer);
            m_UserCKQProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserCKQProperyShow.Dock = DockStyle.Fill;
            this.tabPage6.Controls.Clear();
            this.tabPage6.Controls.Add(m_UserCKQProperyShow);
        }
        private void loadTKQProp(IFeatureClass m_FeatureClass)
        {
            m_UserTKQProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserTKQProperyShow.m_theDataAccess = m_DataAccess_SYS; // 刘扬 修改 2011 分析 
            m_UserTKQProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ; // 探矿权 
            m_UserTKQProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // 保存统计的详细信息 刘扬 修改 2011 分析


            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "探矿权clip";
            m_UserTKQProperyShow.SetData(m_layer as ILayer);
            m_UserTKQProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserTKQProperyShow.Dock = DockStyle.Fill;
            this.tabPage7.Controls.Clear();
            this.tabPage7.Controls.Add(m_UserTKQProperyShow);
        }
        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //m_DataAccess_SYS.OutputExcel(m_ResultDataGrid, "土地核查空间分析结果", "");
                if (chkTdlyxz.Checked && this.m_UserDLTBProperyShow.m_PropertyDataGrid != null)
                {
                    //m_DataAccess_SYS.OutputExcel(this.m_UserDLTBProperyShow.m_PropertyDataGrid
                    this.m_DataAccess_SYS.OutputExcel(this.m_UserDLTBProperyShow.m_PropertyDataGrid, "现状地物分析结果", "");

                }
                if (this.chkGhsj.Checked && this.m_UserGHTProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserGHTProperyShow.m_PropertyDataGrid, "规划图分析结果", "");
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
                IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
                if (hcjg != null)
                {
                    m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);

                }
                if (System.IO.Directory.Exists(Application.StartupPath + "\\OverlayTemp"))
                {
                    DeleteFolder(Application.StartupPath + "\\OverlayTemp"); 
                }                
            }
            catch (Exception ex)
            { }
            this.Close();
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
                //ShowHasBeenStatisticedButton();
            }
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
               
        //         int index = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(theType);
        //         //Button theButton = m_theButtonList[index] as Button;
        //         bool bStatisticed = this.HasBeenStatisticed(theType); // 判断是否被统计过
        //         //theButton.Visible = bStatisticed; // 统计过则显示，否则不显示

        //         m_blHasAnalysis[i] = bStatisticed;

        //         //if (bStatisticed) // 为了判断有几项分析过
        //         //    i++;
        //    }

        //    //if (i == 7) // 标题提示
        //    //{
        //    //    this.Text = String.Format("图斑分析-全部项目已经分析过");
        //    //}
        //    //else
        //    //    this.Text = String.Format("图斑分析-已有{0}项分析结果", i);
        //}

        // 根据统计类型获取统计结果数据
        private DataRow GetStatisticResultData(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_type)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(p_type); // 表名
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // 表中关键字列名
            string theID = GetStaticsTBBH(); // 表中关键字的值

            return m_DataAccess_SYS.GetRowByColmn(theIDName, theID, tableName);
            //return this.m_theSqlOp.GetOneRow(tableName, theIDName, theID);
        }

        // 显示统计结果
        private void ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type)
        {
            DataRow theRow = GetStatisticResultData(type); // 根据统计类型获取统计结果数据
            string theTime = theRow[3].ToString(); // 分析时间
            string theResultShow = String.Format("已核查结果为：(分析时间({0}))", theTime);
            theResultShow = theResultShow + "\r\n";
            theResultShow = theResultShow + "\r\n";
            string theResult = ReTransforToFitableSQL(theRow[2].ToString().Trim()); // 分析结果
            theResultShow += theResult;
            theResultShow += "----------------------------------------------------------" + "\r\n";
            theResultShow = theResultShow + "\r\n";
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
            DataTable theData = m_DataAccess_SYS.getDataTableByQueryString(theSQL);

            theShowControl.SetDataAfterStatisticed(theData);
            theShowControl.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            theShowControl.Dock = DockStyle.Fill;
            
            int index = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type); // 获取索引
            int tabIndex = JCZF.Renderable.CGlobalVarable.m_listTabIndexOfStaticsInfo[index]; // 获取tab索引，目前是一致的与index
            this.tabControl1.TabPages[tabIndex].Controls.Clear();
            this.tabControl1.TabPages[tabIndex].Controls.Add(theShowControl);
            this.tabControl1.SelectedIndex = tabIndex;
       
        }

        // 按钮：原-土地利用
        private void btnTDLY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[0]; // 土地利用现状
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);   // 显示统计结果
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ, m_UserDLTBProperyShow);  // 显示统计详细信息
        }

        // 按钮：原-规划数据
        private void btnGH_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[1]; // 规划数据
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);   // 显示统计结果
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH, m_UserGHTProperyShow);  // 显示统计详细信息

        }



        // 按钮：原-基本农田
        private void btnJBNT_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[2]; // 基本农田 添加例子
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);   // 显示统计结果 添加例子
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT, m_UserJBNTProperyShow);  // 显示统计详细信息 添加例子
        }


        // 按钮：原-建设用地
        private void btnJSYD_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[3]; // 建设用地 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);   // 显示统计结果 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD, m_UserJSYDProperyShow);  // 显示统计详细信息 

        }


        // 按钮：原-矿产资源
        private void btnKCZY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[4]; // 矿产资源 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH);   // 显示统计结果 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH , m_UserKCZYProperyShow);  // 显示统计详细信息 
        }


        // 按钮：原-采矿权
        private void btnCKQ_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[5]; // 采矿权 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ );   // 显示统计结果 
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

    }
}