using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using clsDataAccess;
using clsClipStat=JCZF.SubFrame.clsClipStat;


using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Functions;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;



namespace JCZF.SubFrame
{
    public partial class frmhz : DevComponents.DotNetBar.Office2007Form
    { 
        /// <summary>
        /// 行政区名
        /// </summary>
        public string str_xzqm = null;
        /// <summary>
        /// 行政区代码
        /// </summary>
        public string str_xzqdm = null;
        /// <summary>
        /// 数据处理
        /// </summary>
        public DataAccess m_DataAccess_SYS_;
        /// <summary>
        /// 地图
        /// </summary>
        public AxMapControl m_axmapcontrol1 = new AxMapControl();

        clsClipStat m_clsClipStat;
        private IEnumLayer m_Grouplayers;

        /// <summary>
        /// 文本框显示结果
        /// </summary>
      public  string strTemp = null;

        public frmhz(string str_xzqm0, string str_xzqdm0, DataAccess p_dataaccess, AxMapControl p_axmapcontrol)
        {
            InitializeComponent();
            str_xzqm = str_xzqm0;
            str_xzqdm = str_xzqdm0;
            m_DataAccess_SYS_ = p_dataaccess;
            m_axmapcontrol1 = p_axmapcontrol;

        }

        /// <summary>
        /// 登陆窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmhz_Load(object sender, EventArgs e)
        {
            this.lab_dq.Text = str_xzqm + "汇总分析";


        }
        /// <summary>
        /// 开始汇总分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            strTemp = null;

            IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
            if (hcjg != null)
            {
                this.m_axmapcontrol1.Map.DeleteLayer((ILayer)hcjg);

            }

            this.progressBar1.Visible = true;
            this.progressBar1.Value = 10;


            if (m_clsClipStat == null)
            {
                m_clsClipStat = new clsClipStat();

            }
            m_clsClipStat.m_strFilePath = Application.StartupPath + "\\OverlayTemp";
            m_clsClipStat.m_axMapcontrol = this.m_axmapcontrol1;


            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();//所有表格名称



            try
            {

                #region  土地利用现状分析
                if (chkTdlyxz.Checked)
                {


                    try
                    {
                        //土地利用现状表
                        string str_TDLYXZ_table = "HZFX_" + str_xzqdm + "_TDLYXZ";
                        bool blif_TDLYXZ_table_exist = false;
                        bool blif_CalculateAgain = false;


                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_TDLYXZ_table == tablenames[i].Trim())
                            {
                                blif_TDLYXZ_table_exist = true;
                                break;
                            }
                        }
                        this.progressBar1.Value = 20;

                        #region  汇总分析表格存在
                        if (blif_TDLYXZ_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("土地利用现状分析记录已存在，是否重新计算", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  重新计算
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   不重新计算
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//不重新计算     

                                #region 在datagridview中显示结果

                                string str_sql = "SELECT * FROM  " + str_TDLYXZ_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_TDLYXZ.DataSource = dataset_gridview.Tables[0];
                                #endregion
                                this.progressBar1.Value = 100;
                            }
                            #endregion

                        }
                        #endregion


                        #region 汇总分析表格不存在  或重 新计算
                        if (blif_TDLYXZ_table_exist == false || blif_CalculateAgain == true)
                        {  //计算并输出到表格str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//黑龙江省
                            {

                                calculate_LNS(str_TDLYXZ_table);

                            }

                            else if (str_xzqdm.Length == 4)//地级市
                            {
                                calculate_ds(str_TDLYXZ_table, str_xzqm, str_xzqdm);

                            }

                            #region  区县土地利用现状汇总分析
                            else if (str_xzqdm.Length == 6)//区县
                            {
                                string str_dltb_tablename = null;//地类图斑表格名称
                                string str_lxdl_tablename = null;//零星地类
                                string str_xzdw_tablename = null;//线状地物

                                #region  设置区县级别土地利用现状地物表格
                                for (int i = 0; i < tablenames.Length; i++)
                                {

                                    if (tablenames[i].Contains(str_xzqdm) && tablenames[i].Contains("DLTB"))
                                    {
                                        str_dltb_tablename = tablenames[i].Trim();
                                        continue;

                                    }

                                    if (tablenames[i].Contains(str_xzqdm) && tablenames[i].Contains("LXDL"))
                                    {
                                        str_lxdl_tablename = tablenames[i].Trim();
                                        continue;
                                    }


                                    if (tablenames[i].Contains(str_xzqdm) && tablenames[i].Contains("XZDW"))
                                    {
                                        str_xzdw_tablename = tablenames[i].Trim();

                                    }

                                }
                                #endregion

                                calculate_qxdltb(str_TDLYXZ_table, str_dltb_tablename, str_lxdl_tablename, str_xzdw_tablename);

                            }
                            #endregion


                            this.progressBar1.Value = 100;
                            #region 在datagridview中显示结果
                            string str_sql = "SELECT * FROM  " + str_TDLYXZ_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_TDLYXZ.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //禁止点击列标题排序
                        for (int ii = 0; ii < dataGridView_TDLYXZ.ColumnCount; ii++)
                        {
                            dataGridView_TDLYXZ.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_TDLYXZ.Columns[1].DefaultCellStyle.Format = "F10";

                        #region  在文本框中显示结果
                        string str_infomation = "      土地利用现状分析结果：\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_TDLYXZ, this.txtResult);
                        #endregion
                    }

                    catch (Exception ex)
                    {
                        strTemp += "----------------------------------------------------------" + "\r\n" + "土地利用现状数据有错误，请检查数据" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }
                }

                #endregion


                #region  规划数据分析

                if (chkGhsj.Checked)
                {
                    this.progressBar1.Value = 10;
                    try
                    {
                        string str_GHSJ_table = "HZFX_" + str_xzqdm + "_GHSJ";
                        bool blif_GHSJ_table_exist = false;
                        bool blif_CalculateAgain = false;


                        #region  查找规划数据分析表格是否存在
                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_GHSJ_table == tablenames[i].Trim())
                            {
                                blif_GHSJ_table_exist = true;
                                break;
                            }
                        }

                        #endregion
                        this.progressBar1.Value = 20;
                        #region  规划数据汇总分析表格存在
                        if (blif_GHSJ_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("规划数据分析记录已存在，是否重新计算", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  重新计算
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   不重新计算
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//不重新计算     
                                this.progressBar1.Value = 100;
                                #region 在datagridview中显示结果

                                string str_sql = "SELECT * FROM  " + str_GHSJ_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_GHSJ.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region 汇总分析表格不存在  或重 新计算
                        if (blif_GHSJ_table_exist == false || blif_CalculateAgain == true)
                        {  //计算并输出到表格str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//黑龙江省
                            {

                                cal_GHSJ_LNS(str_GHSJ_table);

                            }

                            else if (str_xzqdm.Length == 4)//地级市
                            {

                                cal_DS_GHSJ(str_xzqdm, str_xzqm, str_GHSJ_table);

                            }

                            #region  区县规划数据汇总分析
                     else if (str_xzqdm.Length == 6)//区县
                       {   //获得区县行政区域
                                bool blif_cancalculate = false;
                                double db_qxgh = 0;
                                string str_dsdm = str_xzqdm.Substring(0,4);
                                string str_dsmc = null;
                                for (int ii = 0; ii < tablenames.Length; ii++)
                                { 
                                    
                                    if((tablenames[ii].ToString()).Contains("GHT")&&(tablenames[ii].ToString()).Contains(str_dsdm))
                                   {
                                       blif_cancalculate = true;
                                       break;
                                    }
                                
                                
                                }





                                DataRow datarow_xzqdm;//查询区域名称---符合查询条件的一条记录
                                DataRowCollection datarowcollection_xzqdm;//
                    
                                datarowcollection_xzqdm =m_DataAccess_SYS_.GetAllDSXZQY();//xzqm,qhdm
                                for (int i = 0; i < datarowcollection_xzqdm.Count; i++)
                                {
                                    datarow_xzqdm = datarowcollection_xzqdm[i];
                                    if (str_dsdm == datarow_xzqdm[1].ToString())
                                    {
                                        str_dsmc = datarow_xzqdm[0].ToString();
                                    
                                    }
                               
                                }
                        

                                if (blif_cancalculate == false)
                                {
                                    //没有数据，不能分析
                                    cal_qx_GHSJ2(str_GHSJ_table, db_qxgh);
                                    

                                }
                                else
                                { 
                                    //进行分析
                                    //IWorkspace m_workspace = SDE_FindWsByDefault();

                                    IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                    m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                    m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                                    for (int i = 0; i < 5; i++)
                                    {
                                        //m_clsClipStat.cal_JBNT();
                                        m_clsClipStat.cal_qxghsj(str_dsmc);

                                        this.progressBar1.Value = 50;
                                        if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                        {
                                            db_qxgh = m_clsClipStat.db_ghsj;
                                            this.progressBar1.Value = 60;
                                            break;
                                        }
                                    }


                                    cal_qx_GHSJ(str_GHSJ_table, db_qxgh);
                           

                              


                                }
                                /////////////////////////////


                            }


                        }
                            #endregion
                        this.progressBar1.Value = 100;
                        #region 在datagridview中显示结果

                        string str_sql0 = "SELECT * FROM  " + str_GHSJ_table;

                        DataSet dataset_gridview0 = new DataSet();
                        dataset_gridview0 = m_DataAccess_SYS_.GetDataSetBySQL(str_sql0);
                        this.dataGridView_GHSJ.DataSource = dataset_gridview0.Tables[0];
                        #endregion



                        #endregion

                        //禁止点击列标题排序
                        for (int ii = 0; ii < dataGridView_GHSJ.ColumnCount; ii++)
                        {
                            dataGridView_GHSJ.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_GHSJ.Columns[1].DefaultCellStyle.Format = "F10";

                        #region 在文本框中输出结果
                        string str_infomation = "      规划数据分析结果：\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_GHSJ, this.txtResult);
                        #endregion


                    }

                    catch (Exception ex)
                    {
                        strTemp += "------------------------------------------------------------" + "\r\n" + "土地利用规划数据有错误，请检查数据" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }

                }
                #endregion


                #region   基本农田分析

                if (chkJbnt.Checked)
                {
                    try
                    {
                        this.progressBar1.Value = 10;
                        string str_JBNT_table = "HZFX_" + str_xzqdm + "_JBNT";
                        bool blif_BNT_table_exist = false;
                        bool blif_CalculateAgain = false;
                        double db_jbntarea = 0;
                        //IFeature m_xzq_feat;//行政区feature

                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_JBNT_table == tablenames[i].Trim())
                            {
                                blif_BNT_table_exist = true;
                                break;
                            }
                        }

                        #region  基本农田分析表格存在
                        if (blif_BNT_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("基本农田分析记录已存在，是否重新计算", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  重新计算
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   不重新计算
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//不重新计算     
                                this.progressBar1.Value = 100;
                                #region 在datagridview中显示结果

                                string str_sql = "SELECT * FROM  " + str_JBNT_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_JBNT.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region   基本农田汇总分析表格不存在  或重 新计算
                        if (blif_BNT_table_exist == false || blif_CalculateAgain == true)
                        {
                            ////计算并输出到表格str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//黑龙江省
                            {

                                IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                            }

                            else if (str_xzqdm.Length == 4)//地级市
                            {

                                IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                                //strTemp += m_clsClipStat.strHCJG;
                                //strTemp += "--------------------------------------------------------------";
                                ////this.txtResult.Text = strTemp;
                                //this.txtResult.Text = strTemp;
                                //AddtoTOC(m_clsClipStat.pJbntOutputFeatClass);
                                //loadJBNTProp(m_clsClipStat.pJbntOutputFeatClass);
                                //this.progressBar1.Value = 60;





                            }


                            else if (str_xzqdm.Length == 6)//区县
                            {
                                IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }



                            //基本农田

                            for (int i = 0; i < 5; i++)
                            {
                                m_clsClipStat.cal_JBNT();
                                this.progressBar1.Value = 30;
                                if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                {
                                    db_jbntarea = m_clsClipStat.db_jbnt;
                                    this.progressBar1.Value = 50;
                                    break;
                                }
                            }


                            cal_LNS_JBNT(str_JBNT_table, db_jbntarea);

                            this.progressBar1.Value = 100;


                            #region 在datagridview中显示结果
                            string str_sql = "SELECT * FROM  " + str_JBNT_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_JBNT.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //禁止点击列标题排序
                        for (int ii = 0; ii < dataGridView_JBNT.ColumnCount; ii++)
                        {
                            dataGridView_JBNT.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_JBNT.Columns[1].DefaultCellStyle.Format = "F10";
                        this.progressBar1.Value = 100;
                        #region  在文本框中显示结果
                        string str_infomation = "      基本农田分析结果：\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_JBNT, this.txtResult);
                        #endregion


                    }
                    //
                    catch (Exception ex)
                    {
                        strTemp += "--------------------------------------------------------------" + "\r\n" + "基本农田数据有错误，请检查数据" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }
                }
                #endregion


                #region  建设用地分析

                if (chkJsyd.Checked)
                {
                    try
                    {
                        this.progressBar1.Value = 10;


                        string str_JSYD_table = "HZFX_" + str_xzqdm + "_JSYD";
                        bool blif_JSYD_table_exist = false;
                        bool blif_CalculateAgain = false;
                        double db_jsydarea = 0;
                        //IFeature m_xzq_feat;//行政区feature

                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_JSYD_table == tablenames[i].Trim())
                            {
                                blif_JSYD_table_exist = true;
                                break;
                            }
                        }

                        #region  建设用地分析表格存在
                        if (blif_JSYD_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("建设用地分析记录已存在，是否重新计算", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  重新计算
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   不重新计算
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//不重新计算     
                                this.progressBar1.Value = 100;
                                #region 在datagridview中显示结果

                                string str_sql = "SELECT * FROM  " + str_JSYD_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_JSYD.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region   建设用地汇总分析表格不存在  或重 新计算
                        if (blif_JSYD_table_exist == false || blif_CalculateAgain == true)
                        {
                            ////计算并输出到表格str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//黑龙江省
                            {

                                IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                            }

                            else if (str_xzqdm.Length == 4)//地级市
                            {

                                IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            else if (str_xzqdm.Length == 6)//区县
                            {
                                IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }

                            //建设用地

                            for (int i = 0; i < 5; i++)
                            {
                                //m_clsClipStat.cal_JBNT();
                                m_clsClipStat.cal_JSYD();
                                this.progressBar1.Value = 50;
                                if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                {
                                    db_jsydarea = m_clsClipStat.db_jsyd;
                                    this.progressBar1.Value = 60;
                                    break;
                                }
                            }



                            cal_LNS_JSYD(str_JSYD_table, db_jsydarea);

                            this.progressBar1.Value = 100;
                            #region 在datagridview中显示结果
                            string str_sql = "SELECT * FROM  " + str_JSYD_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_JSYD.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //禁止点击列标题排序
                        for (int ii = 0; ii < dataGridView_JSYD.ColumnCount; ii++)
                        {
                            dataGridView_JSYD.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_JSYD.Columns[1].DefaultCellStyle.Format = "F10";

                        #region  在文本框中显示结果
                        string str_infomation = "       建设用地分析结果：\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_JSYD, this.txtResult);
                        #endregion





                    }
                    catch (Exception ex)
                    {
                        strTemp += "--------------------------------------------------------------" + "\r\n" + "建设项目审批数据有错误，请检查数据" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }

                }
                #endregion

                #region  矿产资源规划分析
                if (chkKczygh.Checked)
                {
                    try
                    {
                        this.progressBar1.Value = 10;

                        string str_KCZY_table = "HZFX_" + str_xzqdm + "_KCZY";
                        bool blif_KCZY_table_exist = false;
                        bool blif_CalculateAgain = false;
                        double db_kczyarea = 0;
                        //IFeature m_xzq_feat;//行政区feature

                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_KCZY_table == tablenames[i].Trim())
                            {
                                blif_KCZY_table_exist = true;
                                break;
                            }
                        }

                        #region  矿产资源分析表格存在
                        if (blif_KCZY_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("矿产资源规划分析记录已存在，是否重新计算", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  重新计算
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   不重新计算
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//不重新计算     
                                this.progressBar1.Value = 100;
                                #region 在datagridview中显示结果

                                string str_sql = "SELECT * FROM  " + str_KCZY_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_KCGH.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region   矿产资源汇总分析表格不存在  或重 新计算
                        if (blif_KCZY_table_exist == false || blif_CalculateAgain == true)
                        {
                            ////计算并输出到表格str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//黑龙江省
                            {

                                IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                            }

                            else if (str_xzqdm.Length == 4)//地级市
                            {

                                IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            else if (str_xzqdm.Length == 6)//区县
                            {
                                IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            for (int i = 0; i < 5; i++)
                            {

                             
                                m_clsClipStat.cal_KCZY();
                                this.progressBar1.Value = 50;

                                if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                {
                                    db_kczyarea = m_clsClipStat.db_kczy;
                                    this.progressBar1.Value = 70;
                                    break;
                                }
                            }



                            cal_LNS_KCZY(str_KCZY_table, db_kczyarea);

                            this.progressBar1.Value = 100;
                            #region 在datagridview中显示结果
                            string str_sql = "SELECT * FROM  " + str_KCZY_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_KCGH.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //禁止点击列标题排序
                        for (int ii = 0; ii < dataGridView_JSYD.ColumnCount; ii++)
                        {
                            dataGridView_KCGH.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_KCGH.Columns[1].DefaultCellStyle.Format = "F10";

                        #region  在文本框中显示结果
                        string str_infomation = "       矿产资源规划分析结果：\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_KCGH, this.txtResult);
                        #endregion


                    }
                    catch (Exception ex)
                    {
                        strTemp += "--------------------------------------------------------------" + "\r\n" + "矿产资源规划数据有错误，请检查数据" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }

                }
                #endregion

                #region 采矿权分析
                ////采矿

                if (chkCkq.Checked)
                {

                    try
                    {
                        this.progressBar1.Value = 10;

                        string str_CKQ_table = "HZFX_" + str_xzqdm + "_CKQ";
                        bool blif_CKQ_table_exist = false;
                        bool blif_CalculateAgain = false;
                        double db_ckqarea = 0;
                        //IFeature m_xzq_feat;//行政区feature

                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_CKQ_table == tablenames[i].Trim())
                            {
                                blif_CKQ_table_exist = true;
                                break;
                            }
                        }

                        #region  采矿权分析表格存在
                        if (blif_CKQ_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("采矿权分析记录已存在，是否重新计算", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  重新计算
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   不重新计算
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//不重新计算     
                                this.progressBar1.Value = 100;
                                #region 在datagridview中显示结果

                                string str_sql = "SELECT * FROM  " + str_CKQ_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_CKQ.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region   采矿权汇总分析表格不存在  或重 新计算
                        if (blif_CKQ_table_exist == false || blif_CalculateAgain == true)
                        {
                            ////计算并输出到表格str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//黑龙江省
                            {

                                IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                            }

                            else if (str_xzqdm.Length == 4)//地级市
                            {

                                IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            else if (str_xzqdm.Length == 6)//区县
                            {
                                IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            for (int i = 0; i < 5; i++)
                            {
                                m_clsClipStat.cal_CKQ();
                                this.progressBar1.Value = 50;

                                if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                {
                                    db_ckqarea = m_clsClipStat.db_ckq_zmj;
                                    this.progressBar1.Value = 60;
                                    break;
                                }
                            }

                     
                            cal_LNS_CKQ(str_CKQ_table, db_ckqarea);
                            this.progressBar1.Value = 100;

                            #region 在datagridview中显示结果
                            string str_sql = "SELECT * FROM  " + str_CKQ_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_CKQ.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //禁止点击列标题排序
                        for (int ii = 0; ii < dataGridView_CKQ.ColumnCount; ii++)
                        {
                            dataGridView_CKQ.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_CKQ.Columns[1].DefaultCellStyle.Format = "F10";

                        #region  在文本框中显示结果
                        string str_infomation = "       采矿权分析结果：\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_CKQ, this.txtResult);
                        #endregion



                    }
                    catch (Exception ex)
                    {
                        strTemp += "--------------------------------------------------------------" + "\r\n" + "采矿权登记数据有错误，请检查数据" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }

                }
                #endregion


                #region 探矿权分析
                if (chkTkq.Checked)
                 {         
                     try
                     {
                         this.progressBar1.Value = 10;
                         string str_TKQ_table = "HZFX_" + str_xzqdm + "_TKQ";
                         bool blif_TKQ_table_exist = false;
                         bool blif_CalculateAgain = false;
                         double db_tkqyarea = 0;
                         //IFeature m_xzq_feat;//行政区feature

                         for (int i = 0; i < tablenames.Length; i++)
                         {
                             if (str_TKQ_table == tablenames[i].Trim())
                             {
                                 blif_TKQ_table_exist = true;
                                 break;
                             }
                         }

                         #region  矿产资源分析表格存在
                         if (blif_TKQ_table_exist == true)
                         {
                             DialogResult dialog;
                             dialog = MessageBox.Show("探矿权分析记录已存在，是否重新计算", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                             #region  重新计算
                             if (dialog == DialogResult.Yes)
                             {
                                 blif_CalculateAgain = true;

                             }
                             #endregion

                             #region   不重新计算
                             else if (dialog == DialogResult.No)
                             {
                                 blif_CalculateAgain = false;//不重新计算     
                                 this.progressBar1.Value = 100;
                                 #region 在datagridview中显示结果

                                 string str_sql = "SELECT * FROM  " + str_TKQ_table;

                                 DataSet dataset_gridview = new DataSet();
                                 dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                 this.dataGridView_TKQ.DataSource = dataset_gridview.Tables[0];
                                 #endregion

                             }
                             #endregion

                         }
                         #endregion


                         #region  探矿权汇总分析表格不存在  或重 新计算
                         if (blif_TKQ_table_exist == false || blif_CalculateAgain == true)
                         {
                             ////计算并输出到表格str_TDLYXZ_table
                             if (str_xzqdm.Length == 2)//黑龙江省
                             {

                                 IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                 m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                 m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                             }

                             else if (str_xzqdm.Length == 4)//地级市
                             {

                                 IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                 m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                 m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                             }


                             else if (str_xzqdm.Length == 6)//区县
                             {
                                 IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                 m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                 m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                             }


                             for (int i = 0; i < 5; i++)
                             {
                                 m_clsClipStat.cal_TKQ();

                                 this.progressBar1.Value = 50;

                                 if (m_clsClipStat.strHCJG.IndexOf("平方米") >= 0)
                                 {
                                     db_tkqyarea = m_clsClipStat.db_tkq;
                                     this.progressBar1.Value = 70;
                                     break;
                                 }
                             }

                             cal_LNS_TKQ(str_TKQ_table, db_tkqyarea);

                             //cal_LNS_KCZY(str_TKQ_table, db_tkqyarea);
                             this.progressBar1.Value = 100;

                             #region 在datagridview中显示结果
                             string str_sql = "SELECT * FROM  " + str_TKQ_table;

                             DataSet dataset_gridview = new DataSet();
                             dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                             this.dataGridView_TKQ.DataSource = dataset_gridview.Tables[0];

                             #endregion

                         }

                         #endregion

                         //禁止点击列标题排序
                         for (int ii = 0; ii < dataGridView_JSYD.ColumnCount; ii++)
                         {
                             dataGridView_TKQ.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                         }
                         //columns
                         dataGridView_TKQ.Columns[1].DefaultCellStyle.Format = "F10";

                         #region  在文本框中显示结果
                         string str_infomation = "       探矿权分析结果：\r\n";
                         strTemp = strTemp + str_infomation;
                         fdatagrieviewTOtextbox(dataGridView_TKQ, this.txtResult);
                         #endregion
                         this.progressBar1.Visible = false;
                
                     }
                     catch (Exception ex)
                     {
                         strTemp += "--------------------------------------------------------------" + "\r\n" + "探矿权登记数据有错误，请检查数据";
                         this.txtResult.Text = strTemp;
                     }


                 }
                     #endregion
            }
            catch (Exception o)
            { }
           this.progressBar1.Visible = false; 



        }

















        



        #region  详细窗口操作
        /// <summary>
        /// 展开详细窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetail_Click(object sender, EventArgs e)
        {
            this.Width =854;
            this.Height =516;
            this.btnNoDetail.Enabled = true;
            this.btnDetail.Enabled = false;
        }

        /// <summary>
        /// 收起详细窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNoDetail_Click(object sender, EventArgs e)
        {  
            this.Width = 427;
            this.Height = 516;
            this.btnNoDetail.Enabled = false;
            this.btnDetail.Enabled = true;
        }
        #endregion

        #region   退出关闭窗口
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
                if (hcjg != null)
                {
                    //this.m_frmBHTBView.axMapControl1.Map.DeleteLayer((ILayer)hcjg);
                    this.m_axmapcontrol1.Map.DeleteLayer((ILayer)hcjg);

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

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmhz_FormClosing(object sender, FormClosingEventArgs e)
        { 
            try
            {
                IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
                if (hcjg != null)
                {
                    //this.m_frmBHTBView.axMapControl1.Map.DeleteLayer((ILayer)hcjg);
                    this.m_axmapcontrol1.Map.DeleteLayer((ILayer)hcjg);

                }
                if (System.IO.Directory.Exists(Application.StartupPath + "\\OverlayTemp"))
                {
                    DeleteFolder(Application.StartupPath + "\\OverlayTemp");
                }
            }
            catch (Exception ex)
            { }

        }

        #endregion

        #region  获得行政区域

        /// <summary>
        /// 黑龙江省行政区
        /// </summary>
        /// <param name="str_dsdm"></param>
        /// <returns></returns>
        public IFeature get_LNS_geometry(string str_dsdm)
        {
            //获取黑龙江省行政区图层
            IFeatureLayer m_dsfeaturelayer = MapFunction.getFeatureLayerByName("省", this.m_axmapcontrol1);

            //获取黑龙江省行政区域
            IFeatureClass m_FeatureClass = (m_dsfeaturelayer as FeatureLayer).FeatureClass;

            IFields p_fields = m_FeatureClass.Fields;
            int index = 0;
            IField p_field;
            for (int i = 0; i < p_fields.FieldCount; i++)
            {
                p_field = p_fields.get_Field(i);
                if (p_field.Name == "QHDM")
                {
                    index = i;
                    break;
                }

            }

            IFeatureCursor pcursor;
            pcursor = m_FeatureClass.Search(null, false);
            IFeature pfeat;
            pfeat = pcursor.NextFeature();
            while (pfeat != null)
            {
                if (pfeat.get_Value(index).ToString() == str_dsdm)
                {
                    return pfeat as IFeature;
                    break;

                }
                pfeat = pcursor.NextFeature();

            }

            return pfeat;





        }

        /// <summary>
        /// 得到地级市行政区
        /// </summary>
        /// <returns></returns>
        public IFeature get_DS_geometry(string str_dsdm)
        {
            //获取地市行政区图层
            IFeatureLayer m_dsfeaturelayer = MapFunction.getFeatureLayerByName("市", this.m_axmapcontrol1);

            //获取地市行政区域
            IFeatureClass m_FeatureClass = (m_dsfeaturelayer as FeatureLayer).FeatureClass;

            IFields p_fields = m_FeatureClass.Fields;
            int index = 0;
            IField p_field;
            for (int i = 0; i < p_fields.FieldCount; i++)
            {
                p_field = p_fields.get_Field(i);
                if (p_field.Name == "QHDM")
                {
                    index = i;
                    break;
                }

            }

            IFeatureCursor pcursor;
            pcursor = m_FeatureClass.Search(null, false);
            IFeature pfeat;
            pfeat = pcursor.NextFeature();
            while (pfeat != null)
            {
                if (pfeat.get_Value(index).ToString() == str_dsdm)
                {
                    return pfeat as IFeature;
                    break;

                }
                pfeat = pcursor.NextFeature();

            }

            return pfeat;

        }
        /// <summary>
        /// 得到区县行政区
        /// </summary>
        /// <param name="str_dsdm"></param>
        /// <returns></returns>
        public IFeature get_QX_geometry(string str_dsdm)
        {
            //获取地市行政区图层
            IFeatureLayer m_dsfeaturelayer = MapFunction.getFeatureLayerByName("县", this.m_axmapcontrol1);

            //获取地市行政区域
            IFeatureClass m_FeatureClass = (m_dsfeaturelayer as FeatureLayer).FeatureClass;

            IFields p_fields = m_FeatureClass.Fields;
            int index = 0;
            IField p_field;
            for (int i = 0; i < p_fields.FieldCount; i++)
            {
                p_field = p_fields.get_Field(i);
                if (p_field.Name == "QHDM")
                {
                    index = i;
                    break;
                }

            }

            IFeatureCursor pcursor;
            pcursor = m_FeatureClass.Search(null, false);
            IFeature pfeat;
            pfeat = pcursor.NextFeature();
            while (pfeat != null)
            {
                if (pfeat.get_Value(index).ToString() == str_dsdm)
                {
                    return pfeat as IFeature;
                    break;

                }
                pfeat = pcursor.NextFeature();

            }

            return pfeat;





        }

        #endregion

        #region  土地利用现状分析方法
        //高啸峰 090627
        /// <summary>
        /// 计算区县地物信息,str_qxdm区县代码,str_dltb_tablename地类图斑表格名称
        /// </summary>
        /// <param name="str_qxdm"></param>
        public void calculate_qxdltb(string str_qxhzfx_table, string str_dltb_tablename, string str_lxdl_tablename, string str_xzdw_tablename)//(string str_dltb_tablename,string str_lxdl_tablename,str_xzdw_tablename)
        {
            //检验区县汇总分析表str_qxhzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();
            #region
            //获取所有表格名称

            bool if_qxhzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_qxhzfx_table == tablenames[i].Trim())
                {
                    if_qxhzfx_table_exist = true;
                    break;
                }
            }
            if (if_qxhzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_qxhzfx_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_qxhzfx_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_qxhzfx_table);
            }
            #endregion

            DataSet dataset_result = new DataSet();
            string str_sql = null;

            //计算地类图斑
            if (str_dltb_tablename != null)
            {
                str_sql = "Select DLMC AS 地物类型,SUM(TBMJ) AS 地物面积 " +
                                 " FROM  " + str_dltb_tablename +
                                  " GROUP BY DLMC   ORDER BY DLMC";
                dataset_result = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                //把数据存入表格str_qxhzfx_table
                m_DataAccess_SYS_.export_table(dataset_result, str_qxhzfx_table);

            }
         
            //计算零星地类
            if (str_lxdl_tablename != null)
            {
                str_sql = "Select DLMC AS 地物类型,SUM(LXMJ) AS 地物面积 " +
                                      " FROM  " + str_lxdl_tablename +
                                       " GROUP BY DLMC   ORDER BY DLMC";
                dataset_result = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                //把数据存入表格str_qxhzfx_table
                m_DataAccess_SYS_.export_table(dataset_result, str_qxhzfx_table);

            }
            this.progressBar1.Value = 70;
            //计算线状地物
            if (str_xzdw_tablename != null)
            {
                str_sql = "Select DLMC AS 地物类型,SUM(DWMJ) AS 地物面积 " +
                                    " FROM  " + str_xzdw_tablename +
                                     " GROUP BY DLMC   ORDER BY DLMC";
                dataset_result = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                //把数据存入表格str_qxhzfx_table
                m_DataAccess_SYS_.export_table(dataset_result, str_qxhzfx_table);

            }

            //重新整合表格中的数据
            if ((str_dltb_tablename == null) && (str_lxdl_tablename == null) && (str_xzdw_tablename == null))
            {
                return;
            }
            else
            {

                str_sql = "Select 地物类型, SUM(地物面积) AS 地物面积" +
                                         " FROM  " + str_qxhzfx_table +
                                          " GROUP BY 地物类型   ORDER BY 地物类型";
                dataset_result = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                m_DataAccess_SYS_.DeleteAll(str_qxhzfx_table);
                m_DataAccess_SYS_.export_table2(dataset_result, str_qxhzfx_table);//不需要单位转换

            }

        }


        /// <summary>
        /// 计算地市级地物信息，str_dshzfx_table地市汇总分析表，str_dsmc地市名称,str_dsdm地市代码
        /// </summary>
        public void calculate_ds(string str_dshzfx_table, string str_dsmc, string str_dsdm)
        {
            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();
            //检验地市汇总分析表str_dshzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region

            bool if_dshzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_dshzfx_table == tablenames[i].Trim())
                {
                    if_dshzfx_table_exist = true;
                    break;
                }
            }
            if (if_dshzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_dshzfx_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_dshzfx_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_dshzfx_table);
            }
            #endregion



            //查找其所包含的区县名称和代码
            DataRowCollection datarowcollection;
            datarowcollection = m_DataAccess_SYS_.GetQXXZQYofQHDM(str_dsdm);//第一列xzqm 行政区名,第二列qhdm行政区代码


            DataSet dataset_temp;//用来临时存储数据的数据集
            string str_sql = null;//查询语句
            //string str_ght_tablename = null;//地级市规划图

            //计算各县区的地物信息并输出到地级市汇总表中
            #region
            for (int i = 0; i < datarowcollection.Count; i++)
            {
                DataRow datarow_temp = datarowcollection[i];

                string str_qxhzfx_table = "HZFX_" + datarow_temp[1].ToString()+"_TDLYXZ";//区县汇总分析表格名称
                str_qxhzfx_table = str_qxhzfx_table.Trim();

                string str_qxdm = datarow_temp[1].ToString();//区县代码
                str_qxdm = str_qxdm.Trim();

                string str_dltb_tablename = null;//区县地类图斑
                string str_lxdl_tablename = null;//区县零星地类
                string str_xzdw_tablename = null;//区县线状地物



                //设置区县的三张表格和地级市的有关表格
                #region
                for (int ii = 0; ii < tablenames.Length; ii++)
                {
                    #region   区县地物信息表
                    if (tablenames[ii].Contains(str_qxdm) && (tablenames[ii].Contains("DLTB")||tablenames[ii].Contains("地类图斑")))
                    {
                        str_dltb_tablename = tablenames[ii].Trim();
                        continue;

                    }

                    if (tablenames[ii].Contains(str_qxdm) && (tablenames[ii].Contains("LXDL")||tablenames[ii].Contains("零星地物")))
                    {
                        str_lxdl_tablename = tablenames[ii].Trim();
                        continue;
                    }


                    if (tablenames[ii].Contains(str_qxdm) && (tablenames[ii].Contains("XZDW")||tablenames[ii].Contains("线状地物")))
                    {
                        str_xzdw_tablename = tablenames[ii].Trim();
                        continue;
                    }
                    #endregion





                }
                #endregion


                //计算各区县地物信息
                calculate_qxdltb(str_qxhzfx_table, str_dltb_tablename, str_lxdl_tablename, str_xzdw_tablename);

                if (i >= datarowcollection.Count/2)
                     this.progressBar1.Value = 60;

                //把区县汇总信息存入地市汇总分析表str_dshzfx_table
                str_sql = "SELECT * FROM " + str_qxhzfx_table;
                dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                m_DataAccess_SYS_.export_table2(dataset_temp, str_dshzfx_table);//不需要单位转换


            }

            #endregion

            #region  整理表格中的区县级地物信息
            str_sql = "Select 地物类型, SUM(地物面积) AS 地物面积" +
                                       " FROM  " + str_dshzfx_table + " GROUP BY 地物类型    ORDER BY 地物类型 ";
         

            dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);

            m_DataAccess_SYS_.DeleteAll(str_dshzfx_table);
            m_DataAccess_SYS_.export_table2(dataset_temp, str_dshzfx_table);

            #endregion

         


        }

        /// <summary>
        /// 计算黑龙江省土地利用现状
        /// </summary>
        /// <param name="str_LNShzfx_table"></param>
        public void calculate_LNS(string str_LNShzfx_table)
        {


            
            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region

            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_LNShzfx_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_LNShzfx_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_LNShzfx_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_LNShzfx_table);
            }
            #endregion

            //查找其所包含的地级市名称和代码
            DataRowCollection datarowcollection;
            datarowcollection = m_DataAccess_SYS_.GetAllDSXZQY();//第一列xzqm 行政区名,第二列qhdm行政区代码


            DataSet dataset_temp;//用来临时存储数据的数据集
            string str_sql = null;//查询语句
            //progressBar_cal.Value = 30;



            for (int i = 0; i < datarowcollection.Count; i++)
            {
                DataRow datarow_temp = datarowcollection[i];

                string str_dshzfx_table = "HZFX_" + datarow_temp[1]+"_TDLYXZ";
                string str_dsmc = datarow_temp[0].ToString();
                str_dsmc = str_dsmc.Trim();

                string str_dsdm = datarow_temp[1].ToString();
                str_dsdm = str_dsdm.Trim();

                calculate_ds(str_dshzfx_table, str_dsmc, str_dsdm);

                if (i >= datarowcollection.Count / 2)
                    this.progressBar1.Value = 50;//进度

                //把信息汇总后存入黑龙江省汇总分析表str_LNShzfx_table
                 str_sql = "SELECT * FROM " + str_dshzfx_table;
                dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                m_DataAccess_SYS_.export_table2(dataset_temp, str_LNShzfx_table);

            }
            //progressBar_cal.Value = 70;

            //表格整理
            str_sql = "Select 地物类型  AS 地物类型, SUM(地物面积) AS 地物面积" +
                                     " FROM  " + str_LNShzfx_table +
                                     "   GROUP BY 地物类型     ORDER BY 地物类型 ";


            dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);

            m_DataAccess_SYS_.dropatable(str_LNShzfx_table);
            m_DataAccess_SYS_.creatatable(str_LNShzfx_table);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_LNShzfx_table);


        }
        #endregion

        #region  规划数据分析方法
        /// <summary>
        /// 计算黑龙江省规划数据
        /// </summary>
        /// <param name="str_GHSJ_table"></param>
        private void cal_GHSJ_LNS(string str_GHSJ_table)
        {
            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region

            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_GHSJ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_GHSJ_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);
            }
            #endregion

            //查找其所包含的地级市名称和代码
            DataRowCollection datarowcollection;
            datarowcollection = m_DataAccess_SYS_.GetAllDSXZQY();//第一列xzqm 行政区名,第二列qhdm行政区代码
            DataRow datarow_temp;
            DataSet dataset_temp;
            string str_sql = null;

            #region
            for (int i = 0; i < datarowcollection.Count; i++)
            {
                datarow_temp = datarowcollection[i];

                string str_dsmc = datarow_temp[0].ToString();//地级市名称
                str_dsmc = str_dsmc.Trim();

                string str_dsdm = datarow_temp[1].ToString();//地级市代码
                str_dsdm = str_dsdm.Trim();

                //string str_ght_tablename = null;//地级市规划表名称
                string str_dshzfx_table = "HZFX_"+str_dsdm+"_GHSJ";

                cal_DS_GHSJ(str_dsdm, str_dsmc, str_dshzfx_table);

                str_sql = "SELECT * FROM " + str_dshzfx_table;
                dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);              
            }

            #endregion

            //在加入地级市信息后，整理黑龙江省汇总分析表各数据
            str_sql = "SELECT 地物类型 AS 地物类型 ,  SUM(地物面积)  AS  地物面积   FROM " + str_GHSJ_table
                + "  GROUP BY  地物类型  ORDER BY 地物类型";
            dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);

            m_DataAccess_SYS_.DeleteAll(str_GHSJ_table);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);

        

        
        
        
        }

        /// <summary>
        /// 计算地级市规划数据
        /// </summary>
        /// <param name="str_GHSJ_table"></param>
        private void cal_DS_GHSJ(string str_dsdm,string str_dsmc, string str_GHSJ_table)
        {

            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();
            string str_ght_tablename = null;//地级市规划表
            DataSet dataset_temp;
            string str_sql = null;
            //检验地市汇总分析表str_GHSJ_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region

            bool if_dshzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_GHSJ_table == tablenames[i].Trim())
                {
                    if_dshzfx_table_exist = true;
                   
                }
                if (tablenames[i].Contains(str_dsdm) && tablenames[i].Contains("GHT") && tablenames[i].Contains(str_dsmc))
                {
                    str_ght_tablename = tablenames[i].Trim();

                }
            }

            if (if_dshzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_GHSJ_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);
            }
            #endregion



            //在表格str_GHSJ_table中加入地级市的规划信息
            if (str_ght_tablename != null)
            {
                str_sql = "Select  FQMC AS  地物类型, SUM(AREA_) AS 地物面积" +
                                           " FROM  " + str_ght_tablename +
                                            "  GROUP BY FQMC   ORDER BY FQMC";
                dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);

                m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);
            }


        
        }

        private void cal_qx_GHSJ(string str_GHSJ_table, double db_ghsjarea)
        {
            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();



            DataSet dataset_temp = new DataSet();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_GHSJ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_GHSJ_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("地物类型", typeof(string));
            DataColumn dacolum1 = new DataColumn("地物面积", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);
            DataRow darow ;
            double db_temp=0;
            float fl_temp = 0;

            for (int ii = 0; ii < m_clsClipStat.arrghmc.Count; ii++)
            {
                darow = datable.NewRow();
                darow["地物类型"] = m_clsClipStat.arrghmc[ii].ToString() ;
                db_temp = Convert.ToDouble(m_clsClipStat.dblghtb[ii].ToString());

                fl_temp = (float)db_temp;

                darow["地物面积"] = fl_temp/10000;

                datable.Rows.Add(darow);
                
               
            }
            dataset_temp.Tables.Add(datable);
            m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);
        
        }

     

 private void cal_qx_GHSJ2(string str_GHSJ_table, double db_ghsjarea)
        {
            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            DataSet dataset_temp = new DataSet();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_GHSJ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_GHSJ_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("地物类型", typeof(string));
            DataColumn dacolum1 = new DataColumn("地物面积", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);
            DataRow darow=datable.NewRow();;


            darow["地物类型"] = "规划数据";
            darow["地物面积"] = db_ghsjarea;
            datable.Rows.Add(darow);
           
            dataset_temp.Tables.Add(datable);
            m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);
        
        
        
        
        
        }



        #endregion


    



        #region  基本农田计算方法
        /// <summary>
        /// 基本农田计算方法
        /// </summary>
        /// <param name="str_JBNT_table"></param>
        /// <param name="db_jbntarea"></param>
        private void cal_LNS_JBNT(string str_JBNT_table,double db_jbntarea )
        {

            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

       

            DataSet dataset_temp=new DataSet();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_JBNT_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_JBNT_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_JBNT_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_JBNT_table);
            }
            #endregion
            DataTable datable=new DataTable();
            DataColumn dacolum0 = new DataColumn("地物类型",  typeof( string));
            DataColumn dacolum1 = new DataColumn("地物面积",typeof (float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["地物类型"] = "基本农田";
            darow["地物面积"] = db_jbntarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);
           


            m_DataAccess_SYS_.export_table2(dataset_temp, str_JBNT_table);      
       
        }

        #endregion

        #region  建设用地计算方法
        /// <summary>
        /// 建设用地分析
        /// </summary>
        /// <param name="str_JSYD_table"></param>
        /// <param name="db_jsydarea"></param>
        private void cal_LNS_JSYD (string str_JSYD_table,double db_jsydarea )
        {

            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

      

            DataSet dataset_temp=new DataSet();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_JSYD_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_JSYD_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_JSYD_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_JSYD_table);
            }
            #endregion
            DataTable datable=new DataTable();
            DataColumn dacolum0 = new DataColumn("地物类型",  typeof( string));
            DataColumn dacolum1 = new DataColumn("地物面积",typeof (float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["地物类型"] = "新增建设用地";
            darow["地物面积"] = db_jsydarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);
       
            m_DataAccess_SYS_.export_table2(dataset_temp, str_JSYD_table);      
       
        }

        #endregion


        #region 矿产资源规划计算方法
        /// <summary>
        /// 矿产资源规划
        /// </summary>
        /// <param name="str_KCZY_table"></param>
        /// <param name="db_kczyarea"></param>
        private void cal_LNS_KCZY(string str_KCZY_table, double db_kczyarea)
        {
            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            DataSet dataset_temp = new DataSet();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_KCZY_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_KCZY_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_KCZY_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_KCZY_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("地物类型", typeof(string));
            DataColumn dacolum1 = new DataColumn("地物面积", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["地物类型"] = "矿产资源规划";
            darow["地物面积"] = db_kczyarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_KCZY_table);     




        }



        #endregion

        #region  采矿权分析
        /// <summary>
        /// 采矿权分析
        /// </summary>
        /// <param name="str_CKQ_table"></param>
        /// <param name="db_ckqarea"></param>
        private void cal_LNS_CKQ(string str_CKQ_table, double db_ckqarea)
        {             //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            DataSet dataset_temp = new DataSet();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_CKQ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_CKQ_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_CKQ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_CKQ_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("地物类型", typeof(string));
            DataColumn dacolum1 = new DataColumn("地物面积", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["地物类型"] = "采矿权";
            darow["地物面积"] = db_ckqarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_CKQ_table);  
        
        
        }
        #endregion

         
        #region  探矿权分析
        /// <summary>
        /// 计算某行政区探矿权
        /// </summary>
        /// <param name="str_TKQ_table"></param>
        /// <param name="db_tkqarea"></param>
        private void cal_LNS_TKQ(string str_TKQ_table, double db_tkqarea)
        {
            //获取所有表格名称
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            DataSet dataset_temp = new DataSet();

            //检验黑龙江省汇总分析表str_LNShzfx_table是否存在，若存在则删掉旧表创建新表,若不存在则直接创建新表
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_TKQ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //删掉原表
                m_DataAccess_SYS_.dropatable(str_TKQ_table);
                //创建新表
                m_DataAccess_SYS_.creatatable(str_TKQ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_TKQ_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("地物类型", typeof(string));
            DataColumn dacolum1 = new DataColumn("地物面积", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["地物类型"] = "探矿权";
            darow["地物面积"] = db_tkqarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_TKQ_table);  
        
                
        }



        #endregion




        /// <summary>
        /// datagridview输出到textbox中
        /// </summary>
        /// <param name="p_datagridview"></param>
        /// <param name="p_textbox"></param>
        public void fdatagrieviewTOtextbox(DataGridView p_datagridview, TextBox p_textbox)
        {

            string str_tdlx;//土地类型
            string str_tdmj;//土地面积
            double db_zmj = 0;//总面积
            string str_infomation = null;

            for (int ii = 0; ii < p_datagridview.RowCount - 1; ii++)
            {
                if (p_datagridview.Rows[ii].Cells[0].Value == null)
                    continue;
                str_tdlx = (p_datagridview.Rows[ii].Cells[0].Value.ToString()).Trim();

                if (p_datagridview.Rows[ii].Cells[1].Value == null)
                    p_datagridview.Rows[ii].Cells[1].Value = 0;

                str_tdmj = (p_datagridview.Rows[ii].Cells[1].Value.ToString()).Trim();

                str_infomation = str_tdlx + ":  " + str_tdmj + "平方米 " + "；\r\n";
                strTemp = strTemp + str_infomation;
                if (p_datagridview.Rows[ii].Cells[1].Value.ToString() != null)
                    db_zmj = db_zmj + Convert.ToDouble(p_datagridview.Rows[ii].Cells[1].Value);
            }

            str_tdlx = "总面积：";
            str_tdmj = db_zmj.ToString();
            str_infomation = str_tdlx + ":  " + str_tdmj + "平方米" + "。\r\n";
            strTemp = strTemp + str_infomation;
            strTemp = strTemp + "----------------------------------------------------------\r\n";

            p_textbox.Text = strTemp;
        
        
        
        }

        /// <summary>
        /// 得到核查结果图层
        /// </summary>
        /// <param name="grouplayername"></param>
        /// <returns></returns>
        private IGroupLayer GetHcjggrouplayer(string grouplayername)
        {
            IGroupLayer ResGrouplayer = null;
            this.m_Grouplayers = Functions.MapFunction.GetGroupLayers(this.m_axmapcontrol1.ActiveView.FocusMap);
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
        /// 删除文件夹
        /// </summary>
        /// <param name="dir"></param>
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



        private void AddtoTOC(IFeatureClass outputfeatclass)
        {
            IFeatureLayer outlayer = new FeatureLayer() as IFeatureLayer;
            outlayer.FeatureClass = outputfeatclass;
            outlayer.Name = outputfeatclass.AliasName;

            IGroupLayer hcjg = GetHcjggrouplayer("核查结果");
            IGroupLayer hcjg1 = new GroupLayer() as IGroupLayer;

            if (hcjg != null)
            {
                hcjg.Add((ILayer)outlayer);
                this.m_axmapcontrol1.Map.AddLayer(outlayer);
                this.m_axmapcontrol1.Map.DeleteLayer(outlayer);

            }
            else
            {
                //IGroupLayer hcjg = new GroupLayer() as IGroupLayer;
                hcjg1.SpatialReference =  this.m_axmapcontrol1.SpatialReference;
                hcjg1.Name = "核查结果";
                hcjg1.Add((ILayer)outlayer);
                this.m_axmapcontrol1.Map.AddLayer((ILayer)hcjg1);
            }
        }


       
        private void btnExport_Click(object sender, EventArgs e)
        {
            //m_DataAccess_SYS.OutputExcel(m_ResultDataGrid, "土地核查空间分析结果", "");
            if (chkTdlyxz.Checked && this.dataGridView_TDLYXZ!= null)
            {
                //m_DataAccess_SYS.OutputExcel(this.m_UserDLTBProperyShow.m_PropertyDataGrid
                this.m_DataAccess_SYS_.OutputExcel(this.dataGridView_TDLYXZ, "现状地物分析结果", "");

            }
            if (this.chkGhsj.Checked && this.dataGridView_GHSJ!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_GHSJ, "规划图分析结果", "");
            }
            if (chkCkq.Checked && this.dataGridView_CKQ!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_CKQ, "采矿权分析结果", "");
            }
            if (this.chkTkq.Checked && this.dataGridView_TKQ != null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_TKQ, "探矿权分析结果", "");
            }
            if (this.chkJsyd.Checked && this.dataGridView_JSYD!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_JSYD, "建设用地分析结果", "");
            }
            if (this.chkKczygh.Checked && this.dataGridView_KCGH!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_KCGH, "矿产资源分布图分析结果", "");

            }
            if (this.chkJbnt.Checked && this.dataGridView_JBNT!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_JBNT, "基本农田分析结果", "");
            }



        }

       







    }//class--end
}//namespace--end