using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame
{
    public partial class uctTJFX : UserControl
    {
        //public clsDataAccess.DataAccess m_DataAccess_SYS;
        private DevComponents.DotNetBar.ExpandablePanel m_ExpandablePanel_Main_;
        public DevComponents.DotNetBar.ExpandablePanel m_ExpandablePanel_Main
        {
            set
            {
                m_ExpandablePanel_Main_ = value;
                uctTJBB1.m_ExpandablePanel_Main = value;
                uctTJBB1.m_strTitleText = "调查结果统计";
                uctInspectOnlineStatistics1.m_ExpandablePanel_Main = value;
                uctInspectOnlineStatistics1.m_ExpandablePanel_Main.Text= "监控情况";
                uctInspectOnlineStatistics1.m_ExpandablePanel_Main.TitleText  = "监控情况";

            }
        }
        //public DevComponents.DotNetBar.TabControl m_TabControl_Main;
        //private IFeatureLayer m_IFeatureLayer;
        private Panel m_Panel_;
        public Panel m_Panel
        {
            set
            {
                m_Panel_ = value;
                uctTJBB1.m_Panel = value;
                uctInspectOnlineStatistics1.m_Panel = value;
            }
        }
        private JCZF.SubFrame.uctTJFX_XCCQ_JG m_uctTJFX_XCCQ_JG;
        private JCZF.SubFrame.uctTJFX_XCGJTJ_JG m_uctTJFX_XCGJTJ_JG;
private JCZF.SubFrame.uctTJFX_XCGJTJ_JG m_uctTJFX_XCGJFXTJ_JG;

        private JCZF.SubFrame.uctTJFX_ZFRWFX_JG m_uctTJFX_ZFRWFX_JG;
        private clsInspection.uctInspectStatisticsResult m_uctInspectStatisticsResult;


        //public DevComponents.DotNetBar.TabControl m_TabControl_Main;

        ////定义一个委托，用于将数据传送到地图窗口绘制轨迹
        public delegate void uctTJFX_XCGJTJ_JG_DrawGJEventHandler(string p_strGJID, string p_strGJZB);
        public event uctTJFX_XCGJTJ_JG_DrawGJEventHandler uctTJFX_XCGJTJ_JG_DrawGJEvent;


        ////定义一个委托，用于将数据传送到显示结果窗体中,在点击查询按钮时调用
        public delegate void uctTJFX_XCGJTJ_JGEventHandler(DataTable p_DataTable);
        public event uctTJFX_XCGJTJ_JGEventHandler uctTJFX_XCGJTJ_JGQueryEvent;

        public delegate void uctTJFX_ZFRWTJ_JGEventHandler(DataTable p_DataTable);
        public event uctTJFX_ZFRWTJ_JGEventHandler uctTJFX_ZFRWTJ_JGQueryEvent;

        public delegate void uctTJFX_XCCQTJ_JGEventHandler(DataTable p_DataTable);
        public event uctTJFX_XCCQTJ_JGEventHandler uctTJFX_XCCQTJ_JGQueryEvent;

        //public uctTJFX_XCGJTJ_JG m_uctTJFX_XCGJTJ_JG;

        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                uctTJFX_1_XCGJ.m_AxMapControl = value;
                uctXZQTree_Dev1.m_AxMapControl = value;
                uctTJFX_ZFRWTJ.m_AxMapControl = value;
                uctTJFX_1_XCCQ.m_AxMapControl = value;
                uctTJBB1.m_AxMapControl = value;
                uctInspectOnlineStatistics1.m_AxMapControl = value;
                
                m_AxMapControl_ = value;
            }
        }

        private clsDataAccess.DataAccess m_DataAccess_SYS_;
        public clsDataAccess.DataAccess m_DataAccess_SYS
        {
            set
            {
                m_DataAccess_SYS_ = value;
                uctTJFX_1_XCGJ.m_DataAccess_SYS = value;
                //m_uctTJFX_XCGJTJ_JG.m_DataAccess_SYS_ = value;
                uctTJFX_ZFRWTJ.m_DataAccess_SYS = value;
                uctTJFX_1_XCCQ.m_DataAccess_SYS = value;
                uctTJBB1.m_DataAccess_SYS = value;
                uctInspectOnlineStatistics1.m_DataAccess_SYS = value;
                
            }
        }    

        private clsDataAccess.DataAccess m_DataAccess_SDE_;
        public clsDataAccess.DataAccess m_DataAccess_SDE
        {
            set
            {
                m_DataAccess_SDE_ = value;
            }
        }

        private string bhtbReportQHDM = "";
        private string bhtbReportZQM = "";

        private DevComponents.DotNetBar.TabControl _TabControl;
        public DevComponents.DotNetBar.TabControl m_TabControl
        {
            set
            {
                _TabControl = value;
            }
        }

        public uctTJFX()
        {
            InitializeComponent();
        }

        private void navigationPane1_Load(object sender, EventArgs e)
        {
            //dateTimePicker_XCGJTJFX_End.Value = DateTime.Now;
            //dateTimePicker_XCGJTJFX_Start.Value = DateTime.Now;
            //dateTimePicker_XCGJTJFX_Start.Value.AddMonths(-1);
        }



        private void btnOK_XCGJTJFX_Click(object sender, EventArgs e)
        {
            try
            {
                string m_strSQL = "";

                if (m_Panel_ == null)
                {
                    m_Panel = (Panel)clsFunction.Function.GetControl(m_ExpandablePanel_Main_, "PanelInfo_Panel");
                    clsFunction.Function.ClearControls(m_Panel_);
                }


                if (chkGJFXTJ.Checked == true)
                {
                    //巡查轨迹分析统计

                    if (rdbXCGJTJ_AXZQFX.Checked == true)
                    {
                        #region 按行政区统计
                        m_strSQL = "SELECT XZQMC AS 行政区名称,XZQDM AS 行政区代码 ,rwid as 任务编号, GJID AS 轨迹标识, gjzb as 轨迹坐标,sj as 巡查时间,ZFRYXM AS 巡查人姓名, ";
                        m_strSQL = m_strSQL + " BGDH AS 办公电话, ZFRYSJ AS 手机,NBXH AS 内部小号,SGZH AS 上岗证号,ZFDWMC AS 所属单位,ZFDWDM AS 所属单位编号";
                        m_strSQL = m_strSQL + " FROM DCGJFXB_RW_ZFRY_ZFDW WHERE ( sj>='" + uctTJFX_1_XCGJ.m_dateStartTime.ToShortDateString() + "' or sj is null)";
                        m_strSQL = m_strSQL + " and (sj<='" + uctTJFX_1_XCGJ.m_dateEndTime.ToShortDateString() + "' or sj is null)";
                        if (chk_XCGJTJFX_FXKYDK.Checked && chk_XCGJTJFX_WFXKYDK.Checked)
                        {

                        }
                        else if (chk_XCGJTJFX_FXKYDK.Checked)
                        {
                            m_strSQL = m_strSQL + " and  rwid  in (select rwid from dcjgnr)";
                        }
                        else if (chk_XCGJTJFX_WFXKYDK.Checked)
                        {
                            m_strSQL = m_strSQL + " and  rwid not in (select rwid from dcjgnr)";
                        }

                        if (uctTJFX_1_XCGJ.chkXZQ.Checked == true)
                        {
                            if (m_DataAccess_SYS_.ProviderIsOraDB())
                            {
                                m_strSQL = m_strSQL + "  and  substr(XZQDM,1," + uctTJFX_1_XCGJ.m_strXZQDM.Length + ")='" + uctTJFX_1_XCGJ.m_strXZQDM + "'";
                            }
                            else
                            {
                                m_strSQL = m_strSQL + "  and  LEFT(XZQDM," + uctTJFX_1_XCGJ.m_strXZQDM.Length + ")='" + uctTJFX_1_XCGJ.m_strXZQDM + "'";
                            }
                        }

                        if (uctTJFX_1_XCGJ.chkDW.Checked == true)
                        {
                            m_strSQL = m_strSQL + " and  ZFDWDM='" + uctTJFX_1_XCGJ.m_strDWDM + "'";

                        }

                        if (uctTJFX_1_XCGJ.chkRY.Checked == true)
                        {
                            m_strSQL = m_strSQL + " and  ZFRYBH ='" + uctTJFX_1_XCGJ.m_strRYBH + "'";

                        }

                        //if (uctTJFX_XCGJTJ_JGQueryEvent != null)
                        //{
                        //    uctTJFX_XCGJTJ_JGQueryEvent(FillDataGridViewX(m_strSQL));
                        //}

                        m_strSQL = m_strSQL + " order by 行政区代码 desc";
                        #endregion
                    }
                    else
                    {
                        #region //按巡查轨迹统计分析
                        m_strSQL = "SELECT  rwid as 任务编号,GJID AS 轨迹标识,gjzb as 轨迹坐标,sj as 巡查时间,ZFRYXM AS 巡查人姓名, BGDH AS 办公电话, ZFRYSJ AS 手机,NBXH AS 内部小号,SGZH AS 上岗证号,ZFDWMC AS 所属单位,ZFDWDM AS 所属单位编号 FROM DCGJ_RW_ZFRY_ZFDW WHERE  sj>='" + uctTJFX_1_XCGJ.m_dateStartTime.ToShortDateString() + "' and sj<='" + uctTJFX_1_XCGJ.m_dateEndTime.ToShortDateString() + "'";
                        if (chk_XCGJTJFX_FXKYDK.Checked && chk_XCGJTJFX_WFXKYDK.Checked)
                        {

                        }
                        else if (chk_XCGJTJFX_FXKYDK.Checked)
                        {
                            m_strSQL = m_strSQL + " and  rwid  in (select rwid from dcjg)";
                        }
                        else if (chk_XCGJTJFX_WFXKYDK.Checked)
                        {
                            m_strSQL = m_strSQL + " and  rwid not in (select rwid from dcjg)";
                        }

                        if (uctTJFX_1_XCGJ.chkXZQ.Checked == true)
                        {
                            if (m_DataAccess_SYS_.ProviderIsOraDB())
                            {
                                m_strSQL = m_strSQL + "  and  GJID IN(SELECT GJID FROM DCGJFXB WHERE substr(XZQDM,1," + uctTJFX_1_XCGJ.m_strXZQDM.Length + ")='" + uctTJFX_1_XCGJ.m_strXZQDM + "')";
                            }
                            else
                            {
                                m_strSQL = m_strSQL + "  and  GJID IN(SELECT GJID FROM DCGJFXB WHERE LEFT(XZQDM," + uctTJFX_1_XCGJ.m_strXZQDM.Length + ")='" + uctTJFX_1_XCGJ.m_strXZQDM + "')";
                            }
                        }

                        if (uctTJFX_1_XCGJ.chkDW.Checked == true)
                        {
                            m_strSQL = m_strSQL + " and  ZFDWDM='" + uctTJFX_1_XCGJ.m_strDWDM + "'";

                        }

                        if (uctTJFX_1_XCGJ.chkRY.Checked == true)
                        {
                            m_strSQL = m_strSQL + " and  ZFRYBH ='" + uctTJFX_1_XCGJ.m_strRYBH + "'";

                        }

                        //if (uctTJFX_XCGJTJ_JGQueryEvent != null)
                        //{
                        //    uctTJFX_XCGJTJ_JGQueryEvent();
                        //}
                        #endregion

                    }

                    m_ExpandablePanel_Main_.Text = "巡查轨迹分析统计";
                    m_ExpandablePanel_Main_.TitleText = "巡查轨迹分析统计";


                    if (clsFunction.Function.HasControl(m_Panel_, "m_uctTJFX_XCGJFXTJ_JG") == false)
                    {
                        clsFunction.Function.ClearControls(m_Panel_);
                        m_uctTJFX_XCGJFXTJ_JG = new JCZF.SubFrame.uctTJFX_XCGJTJ_JG();
                        m_uctTJFX_XCGJFXTJ_JG.Name = "m_uctTJFX_XCGJFXTJ_JG";
                        m_uctTJFX_XCGJFXTJ_JG.Dock = DockStyle.Fill;
                        m_Panel_.Controls.Add(m_uctTJFX_XCGJFXTJ_JG);
                    }

                    m_uctTJFX_XCGJFXTJ_JG.m_DataGridView.DataSource = FillDataGridViewX(m_strSQL);
                    m_uctTJFX_XCGJFXTJ_JG.m_DataAccess_SYS_ = m_DataAccess_SYS_;

                }
                else
                {
                    //巡查轨迹统计
                    string m_strStart = uctTJFX_1_XCGJ.m_dateStartTime.ToShortDateString();
                    string m_strEnd = uctTJFX_1_XCGJ.m_dateEndTime.ToShortDateString();

                    if (m_strStart == m_strEnd)
                    {
                        m_strEnd = uctTJFX_1_XCGJ.m_dateEndTime.AddHours(24).ToShortDateString();
                    }

                    m_strSQL = "SELECT  GJID AS 轨迹标识,rwid as 任务编号,gjzb as 轨迹坐标,sj as 巡查时间,ZFRYXM AS 巡查人姓名, BGDH AS 办公电话, ZFRYSJ AS 手机,NBXH AS 内部小号,SGZH AS 上岗证号,ZFDWMC AS 所属单位,ZFDWDM AS 所属单位编号 FROM DCGJ_RW_ZFRY_ZFDW WHERE  sj>='" + m_strStart + "' and sj<='" + m_strEnd + "'";

                    if (uctTJFX_1_XCGJ.chkXZQ.Checked == true)
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strSQL = m_strSQL + "  and   substr(ZFRYBH,1," + uctTJFX_1_XCGJ.m_strXZQDM.Length + ")='" + uctTJFX_1_XCGJ.m_strXZQDM + "'";
                        }
                        else
                        {
                            m_strSQL = m_strSQL + "  and   LEFT(ZFRYBH," + uctTJFX_1_XCGJ.m_strXZQDM.Length + ")='" + uctTJFX_1_XCGJ.m_strXZQDM + "'";
                        }
                    }

                    if (uctTJFX_1_XCGJ.chkDW.Checked == true)
                    {
                        m_strSQL = m_strSQL + " and  ZFDWDM='" + uctTJFX_1_XCGJ.m_strDWDM + "'";

                    }

                    if (uctTJFX_1_XCGJ.chkRY.Checked == true)
                    {
                        m_strSQL = m_strSQL + " and  ZFRYBH ='" + uctTJFX_1_XCGJ.m_strRYBH + "'";

                    }



                    m_ExpandablePanel_Main_.Text = "巡查轨迹统计";
                    m_ExpandablePanel_Main_.TitleText = "巡查轨迹统计";



                    if (clsFunction.Function.HasControl(m_Panel_, "m_uctTJFX_XCGJTJ_JG") == false)
                    {
                        clsFunction.Function.ClearControls(m_Panel_);
                        m_uctTJFX_XCGJTJ_JG = new JCZF.SubFrame.uctTJFX_XCGJTJ_JG();
                        m_uctTJFX_XCGJTJ_JG.Name = "m_uctTJFX_XCGJTJ_JG";
                        m_uctTJFX_XCGJTJ_JG.Dock = DockStyle.Fill;
                        m_Panel_.Controls.Add(m_uctTJFX_XCGJTJ_JG);

                        m_uctTJFX_XCGJTJ_JG.uctTJFX_XCGJTJ_JGEvent += new uctTJFX_XCGJTJ_JG.uctTJFX_XCGJTJ_JGEventHandler(m_uctTJFX_XCGJTJ_JG_uctTJFX_XCGJTJ_JGEvent);
                    }

                    m_uctTJFX_XCGJTJ_JG.m_DataGridView.DataSource = FillDataGridViewX(m_strSQL);
                    m_uctTJFX_XCGJTJ_JG.m_DataAccess_SYS_ = m_DataAccess_SYS_;

                    m_ExpandablePanel_Main_.Visible = true;
                    m_ExpandablePanel_Main_.Expanded = true;
                }
              
                          

               
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void m_uctTJFX_XCGJTJ_JG_uctTJFX_XCGJTJ_JGEvent(string  p_strGJID,string p_strGJZB)
        {
            if (uctTJFX_XCGJTJ_JG_DrawGJEvent != null)
                uctTJFX_XCGJTJ_JG_DrawGJEvent(p_strGJID,p_strGJZB);
        }


        public DataTable  FillDataGridViewX(string p_stSQL)
        {
            try{
            if (m_DataAccess_SYS_ == null || p_stSQL == null) return null ;
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(p_stSQL);

            if (m_DataTable == null || m_DataTable.Rows.Count <1)
            {
                m_DataAccess_SYS_.MessageInforShow(this.ParentForm, "没有匹配的数据，请检查查询条件！");
                return null ;
            }

            Type m_Type = Type.GetType("System.String");
            m_DataTable.Columns.Add("经过行政村", m_Type);
           
            string m_strGJID = "";
            string m_strXZQDM = "";
            string m_strXZQMC = "";

            DataTable m_DataTableDCGJFXB;
            DataTable m_DataTableXZQ;
            DataTable m_DataTableRW_ZFRY_ZFDW;
            string m_strRWID="";
            for (int i = 0; i < m_DataTable.Rows.Count; i++)
            {
                m_strGJID = m_DataTable.Rows[i]["轨迹标识"].ToString();
                string m_strSQL = "select SYTC_TDLY.XZQMC from dcgjfxb LEFT JoiN SYTC_TDLY ON SYTC_TDLY.XZQDM=dcgjfxb.XZQDM where   dcgjfxb.gjid='" + m_strGJID + "'";
                m_DataTableDCGJFXB = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
                if (m_DataTableDCGJFXB != null && m_DataTableDCGJFXB.Rows.Count > 0)
                {
                    for (int j = 0; j < m_DataTableDCGJFXB.Rows.Count; j++)
                    {
                        m_strXZQMC = m_strXZQMC + m_DataTableDCGJFXB.Rows[j][0].ToString() + "，";
                    }
                    m_strXZQMC = m_strXZQMC.Substring(0, m_strXZQMC.Length - 1);
                }
                if (m_strXZQMC == "") m_strXZQMC = "  ";
                m_DataTable.Rows[i]["经过行政村"] = (object)m_strXZQMC;                

            }            

           
            return  m_DataTable;
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
               
            }
            return null;
        }



        private void FillListView(DataSet ds, ListView _listView)
        {
            _listView.Clear();

            string[] s1 = new string[ds.Tables[0].Columns.Count];

            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                _listView.Columns.Add(ds.Tables[0].Columns[i].ColumnName, 100, HorizontalAlignment.Left);
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < _listView.Columns.Count; j++)
                {
                    s1[j] = ds.Tables[0].Rows[i][j].ToString();

                }
                ListViewItem lvi = new ListViewItem(s1);
                _listView.Items.Add(lvi);
            }

        }

        private void btnTJBB_OK_Click(object sender, EventArgs e)
        {
            labHCBB_Info.Text = "";
            try{
            if (TJBXZComB.Text == "" || TJBXZComB.Text == "统计表选择")
            {
                MessageBox.Show("请选择统计表类型！"); return;
            }

            string ndsy = "";
            string hcly = "";
            string dqsy = "";
            string str_ndsy = "";
            string str_hcly = "";
            string str_dqsy = "";

            string QHDM = "";
            //QHDM = getQHDM(this.uctXZQTree_HCQKTJ.m_ComboBoxTreeView);
            QHDM = uctXZQTree_Dev1.m_strXZQDM;
            ndsy = this.TJBNDSYComB.Text;
            hcly = this.TJBHCLYComb.Text;
            dqsy = this.uctXZQTree_Dev1.m_strXZQMC;

            if (ndsy == "年度索引" || ndsy == "全部" || ndsy == "")
            {
                str_ndsy = "";
                m_DataAccess_SYS_.MessageInforShow(this.ParentForm, "请选择年度！");
                return;
            }
            else
            { str_ndsy = " WHERE tbrq = '" + ndsy.Substring(0, 4) + "'"; }

            if (hcly == "核查来源" || hcly == "全部")
            { str_hcly = ""; }
            else
            {
                if (str_ndsy != "")
                { str_hcly = " AND ss='" + hcly + "'"; }
                else
                { str_hcly = " WHERE ss='" + hcly + "'"; }
            }

            if (dqsy == "地区索引" || dqsy == "黑龙江省")
            { str_dqsy = ""; }
            else
            {
                //if (str_ndsy != "" || str_hcly != "")
                //{
                //    if (m_DataAccess_SYS.ProviderIsOraDB())
                //    {
                //        str_dqsy = " AND substr(XZQDM,1,4)='" + QHDM + "'";
                //    }
                //    else
                //    {
                //        str_dqsy = " AND left(XZQDM,4)='" + QHDM + "'";
                //    }
                //}
                //else
                //{
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str_dqsy = " WHERE substr(XZQDM,1,4)='" + QHDM + "'";
                    }
                    else
                    {
                        str_dqsy = " WHERE left(XZQDM,4)='" + QHDM + "'";
                    }
                //}
            }
            Crystal_Report.Report.DatabaseString.m_DataAccess_SYS = this.m_DataAccess_SYS_;
            Crystal_Report.Report.DatabaseString.m_DataAccess_SDE = this.m_DataAccess_SDE_;
            Crystal_Report.Report.SqlClass p_SqlClass = new Crystal_Report.Report.SqlClass();
            Crystal_Report.Report.DataSet1 pDataSet = new Crystal_Report.Report.DataSet1();
            string str_sql = "";
            Crystal_Report.Report.frm_tjbb pfrm_tjbb = new Crystal_Report.Report.frm_tjbb();

            switch (this.TJBXZComB.Text)
            {
                case "变化图斑情况统计表":

                    //str_sql = "SELECT ID as 序号, tbID as 图斑序号, zdh as 宗地号, tbwz AS 图斑位置, szzq AS 所占政区, qhdm AS 区号代码,yddwgr AS 用地单位, ydsj AS 用地时间, gdpzjgsjwh AS 供地批准机关时间和文号, zzspzjgsjwh AS 转征收批准机关时间和文号, gdpzmj_zs AS 供地批准面积_总数,gdpzmj_gd AS 供地批准面积_耕地, ydmj_zs AS 用地面积_总数,ydmj_gd AS 用地面积_耕地, ghyt AS 规划用途, pzyt AS 批准用途, sjyt AS 实际用途, gdfs AS 供地方式, sfwbjy AS 未报即用, sfbbby AS 边报边用, sfwgjy AS 未供即用,sfpqpz AS 骗取批准, sfczyd AS 超占用地, sfszgbyd AS 擅自改变用途,sfffpd AS 非法批准, sfwffsgd AS 违反方式供地, sfqtwfyd AS 其它违法用地,sfwftdlyztgh AS 违反土地利用总体规划, sfddxz AS 单独选址, sfgjhsjzdgc AS 国家和省级重点功能, sfzyjbnt AS 占用基本农田,sfnyjgtz AS 农业结构调整, sfsdwbh AS 实地未变化, sfdtxcyjfxwf AS 动态巡查已发现违法, bz AS 备注, tbjg AS 填表机关,tbrq AS 填表日期 FROM 变化图斑情况统计表" + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                   // str_sql = "SELECT OBJECTID AS 序号, tbbh AS 图斑序号, zdh AS 宗地号, tbwz AS 图斑位置,XZQM AS 所占政区, QHDM AS 区号代码, yddw AS 用地单位, ydsj AS 用地时间,gdpzjgwh AS 供地批准机关时间和文号, zzspzjgwh AS 转征收批准机关时间和文号,gdpzmjzs AS 供地批准面积_总数, gdpzmjgd AS 供地批准面积_耕地,ydmjzs AS 用地面积_总数, ydmjgd AS 用地面积_耕地, ghyt AS 规划用途,pzyt AS 批准用途, sjyt AS 实际用途, gdfs AS 供地方式, wbjy AS 未报即用,bbby AS 边报边用, wgjy AS 未供即用, pqpz AS 骗取批准, czyd AS 超占用地,szgbyt AS 擅自改变用途, ffpd AS 非法批准, wffsgd AS 违反方式供地,qtwfyd AS 其它违法用地, wftdlyztgh AS 违反土地利用总体规划, ddxz AS 单独选址,gjhsjzdgc AS 国家和省级重点功能, zyjbnt AS 占用基本农田, nyjgtz AS 农业结构调整,sdwbh AS 实地未变化, dtxcyfxwf AS 动态巡查已发现违法, bz AS 备注 FROM 土地核查 WHERE tbrq = '" + ndsy.Substring(0, 4) + "' " + str_hcly + str_dqsy;
                    //str_sql = "SELECT OBJECTID AS 编号, XZQDM  AS 行政区代码, XMC AS 区县名称, XZMC AS 乡镇名称,QSX AS 前时相, HSX AS 后时相, XZB AS X坐标, YZB AS Y坐标,JCBH AS 监察编号, JCMJ AS 监察面积,SJKDLBM AS 实际地类编码, QDLBM AS 前地类编码,HDLBM AS 后地类编码,ZYJBNT AS 占用基本农田, JBNTMJ AS 基本农田面积, SFBH AS 是否变化, XZDWKD AS 线状地物, WYHSZP AS 未用指标, BZ AS 备注,DKBH AS 地块编号, DKFL AS 地块分类, DKMJ AS 地块面积, NYDMJ AS 农用地面积,GDMJ AS 耕地面积, JBNTMJI AS 基本农田面积, WLYDMJ AS 未利用地面积,SJYT AS 实际用途, YDDW AS 用地单位, XMLX AS 项目类型,HFXSC AS 合法性审查, WFLX AS 违法类型 FROM " + "T" + ndsy.Substring(0, 4) + "WPJC" + " WHERE tbrq = '" + ndsy.Substring(0, 4) + "' " + str_hcly + str_dqsy;
                    str_sql = "SELECT OBJECTID AS 编号, XZQDM  AS 行政区代码, XMC AS 区县名称, XZMC AS 乡镇名称,QSX AS 前时相, HSX AS 后时相, XZB AS X坐标, YZB AS Y坐标,JCBH AS 监察编号, JCMJ AS 监察面积,SJKDLBM AS 实际地类编码, QDLBM AS 前地类编码,HDLBM AS 后地类编码,ZYJBNT AS 占用基本农田, JBNTMJ AS 基本农田面积, SFBH AS 是否变化, XZDWKD AS 线状地物, WYHSZP AS 未用指标, BZ AS 备注,DKBH AS 地块编号, DKFL AS 地块分类, DKMJ AS 地块面积, NYDMJ AS 农用地面积,GDMJ AS 耕地面积, JBNTMJI AS 基本农田面积, WLYDMJ AS 未利用地面积,SJYT AS 实际用途, YDDW AS 用地单位, XMLX AS 项目类型,HFXSC AS 合法性审查, WFLX AS 违法类型 FROM " + "T" + ndsy.Substring(0, 4) + "_WPJC_80_Temp" + " " + str_dqsy;

                   this.bhtbReportQHDM = QHDM;
                    this.bhtbReportZQM = uctXZQTree_Dev1.m_strXZQMC;
                    //p_SqlClass.GetDataSet(str_sql, "变化图斑情况统计表", pDataSet);
                    ////pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
                    //Crystal_Report.Report.Rpt_bhtbqktj mrpt1 = new Crystal_Report.Report.Rpt_bhtbqktj();
                    //mrpt1.SetDataSource(pDataSet.Tables["变化图斑情况统计表"]);
                    //pfrm_tjbb.Text = "变化图斑情况统计表";
                    //pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt1;

                    //pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
                    //pfrm_tjbb.crystalReportViewer1.Refresh();

                    //pfrm_tjbb.MdiParent = this;
                    //pfrm_tjbb.Show();
                    //DataSet ds = this.m_DataAccess_SYS.GetDataBySQL(str_sql); //this.GetDataBySQL(strConn, strQuery); // 原始 刘扬 修改 2011
                    DataSet ds = this.m_DataAccess_SDE_.GetDataSetBySQL(str_sql); // 刘扬 修改 2011

                    //DataSet ds = DBHelper.ToDataSet(strQuery);


                    if (ds != null )
                    {
                        JCZF.SubFrame.Query.utcDataQueryShow m_frmDataQueryShow = new JCZF.SubFrame.Query.utcDataQueryShow();
                        //m_frmDataQueryShow.MdiParent = this.ParentForm; ;
                        FillListView(ds, m_frmDataQueryShow.listView1);
                        //m_frmDataQueryShow.ZoomToFea += new JCZF.SubFrame.Query.frmDataQueryShow.ZoomToFeaEventHandler(m_frmDataQueryShow_ZoomToFea);
                        //m_frmDataQueryShow.Show();
                        //m_frmDataQueryShow.WindowState=FormWindowState.Maximized;
                        labHCBB_Info.Text = "符合查询条件的数据总共有 " + ds.Tables[0].Rows.Count.ToString() + " 条！";
                    }
                    else
                    {
                        MessageBox.Show("没有查询到数据！");
                    }


                    break;

                case "执法检查数据汇总表":

                    //str_sql = "select * from 执法检查数据汇总表 " + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    str_sql = "select * from 执法检查数据汇总表 " +  str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";

                    ////str_sql = "select * from 执法检查数据汇总表 WHERE qhdm ='000' ";      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    p_SqlClass.GetDataSet(str_sql, "执法检查数据汇总表", pDataSet);
                    //pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
                    Crystal_Report.Report.Rpt_zfjcsjhz mrpt2 = new Crystal_Report.Report.Rpt_zfjcsjhz();
                    mrpt2.SetDataSource(pDataSet.Tables["执法检查数据汇总表"]);
                    pfrm_tjbb.Text = "执法检查数据汇总表";
                    pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt2;

                    pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
                    pfrm_tjbb.crystalReportViewer1.Refresh();

                    pfrm_tjbb.crystalReportViewer2.Refresh();
                    //
                    pfrm_tjbb.MdiParent = this.ParentForm ;
                    pfrm_tjbb.Show();

                    break;

                case "违法用地数据汇总表":

                   // str_sql = "select * from 违法用地数据汇总表 " + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    str_sql = "select * from 违法用地数据汇总表 " +  str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    ////str_sql = "select * from 执法检查数据汇总表 WHERE qhdm ='000' ";      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    p_SqlClass.GetDataSet(str_sql, "违法用地数据汇总表", pDataSet);
                    //pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
                    Crystal_Report.Report.Rpt_wfydsjhz mrpt3 = new Crystal_Report.Report.Rpt_wfydsjhz();
                    mrpt3.SetDataSource(pDataSet.Tables["违法用地数据汇总表"]);
                    pfrm_tjbb.Text = "违法用地数据汇总表";
                    pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt3;

                    pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
                    pfrm_tjbb.crystalReportViewer1.Refresh();

                    pfrm_tjbb.crystalReportViewer2.Refresh();
                    //
                    pfrm_tjbb.MdiParent = this.ParentForm; ;
                    pfrm_tjbb.Show();

                    break;

                case "违法用地调查处理情况汇总表":

                    str_sql = "select * from 违法用地调查处理情况汇总表 " + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    ////str_sql = "select * from 执法检查数据汇总表 WHERE qhdm ='000' ";      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    p_SqlClass.GetDataSet(str_sql, "违法用地调查处理情况汇总表", pDataSet);
                    //pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
                    Crystal_Report.Report.Rpt_wfyddcclqkhz mrpt4 = new Crystal_Report.Report.Rpt_wfyddcclqkhz();
                    mrpt4.SetDataSource(pDataSet.Tables["违法用地调查处理情况汇总表"]);
                    pfrm_tjbb.Text = "违法用地调查处理情况汇总表";
                    pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt4;

                    pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
                    pfrm_tjbb.crystalReportViewer1.Refresh();

                    pfrm_tjbb.crystalReportViewer2.Refresh();
                    //
                    pfrm_tjbb.MdiParent = this.ParentForm; ;
                    pfrm_tjbb.Show();

                    break;

                case "违法用地分类统计表":

                   // str_sql = "select * from 违法用地分类统计表 " + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    str_sql = "select * from 违法用地分类统计表 " + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    ////str_sql = "select * from 执法检查数据汇总表 WHERE qhdm ='000' ";      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    p_SqlClass.GetDataSet(str_sql, "违法用地分类统计表", pDataSet);
                    //pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
                    Crystal_Report.Report.Rpt_wfydfltjb mrpt5 = new Crystal_Report.Report.Rpt_wfydfltjb();
                    mrpt5.SetDataSource(pDataSet.Tables["违法用地分类统计表"]);
                    pfrm_tjbb.Text = "违法用地分类统计表";
                    pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt5;

                    pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
                    pfrm_tjbb.crystalReportViewer1.Refresh();

                    pfrm_tjbb.crystalReportViewer2.Refresh();
                    //
                    pfrm_tjbb.MdiParent = this.ParentForm; ;
                    pfrm_tjbb.Show();

                    break;

                case "农村建设违法用地分类比例统计表":

                    str_sql = "select * from 农村建设违法用地分类比例统计表 " + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    ////str_sql = "select * from 执法检查数据汇总表 WHERE qhdm ='000' ";      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    p_SqlClass.GetDataSet(str_sql, "农村建设违法用地分类比例统计表", pDataSet);
                    //pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
                    Crystal_Report.Report.Rpt_ncjswfydflbltj mrpt6 = new Crystal_Report.Report.Rpt_ncjswfydflbltj();
                    mrpt6.SetDataSource(pDataSet.Tables["农村建设违法用地分类比例统计表"]);
                    pfrm_tjbb.Text = "农村建设违法用地分类比例统计表";
                    pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt6;

                    pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
                    pfrm_tjbb.crystalReportViewer1.Refresh();

                    pfrm_tjbb.crystalReportViewer2.Refresh();
                    //
                    pfrm_tjbb.MdiParent = this.ParentForm; ;
                    pfrm_tjbb.Show();

                    break;

                case "城市建设违法用地分类比例统计表":

                    str_sql = "select * from 城市建设违法用地分类比例统计表 " + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    ////str_sql = "select * from 执法检查数据汇总表 WHERE qhdm ='000' ";      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    p_SqlClass.GetDataSet(str_sql, "城市建设违法用地分类比例统计表", pDataSet);
                    //pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
                    Crystal_Report.Report.Rpt_csjswfydflbltj mrpt7 = new Crystal_Report.Report.Rpt_csjswfydflbltj();
                    mrpt7.SetDataSource(pDataSet.Tables["城市建设违法用地分类比例统计表"]);
                    pfrm_tjbb.Text = "城市建设违法用地分类比例统计表";
                    pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt7;

                    pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
                    pfrm_tjbb.crystalReportViewer1.Refresh();
                    //
                    ////Crystal_Report.Report.Rpt_wtdk_tb mrpt_tb = new Rpt_wtdk_tb();
                    ////mrpt_tb.SetDataSource(pDataSet.Tables["问题地块"]);
                    ////pfrm_tjbb.crystalReportViewer2.ReportSource = mrpt_tb;
                    pfrm_tjbb.crystalReportViewer2.Refresh();
                    //
                    pfrm_tjbb.MdiParent = this.ParentForm; ;
                    pfrm_tjbb.Show();

                    break;

                case "国家和省级重点工程违法用地分类比例统计表":

                    str_sql = "select * from 国家和省级重点工程违法用地分类比例统计表 " + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    ////str_sql = "select * from 执法检查数据汇总表 WHERE qhdm ='000' ";      //WHERE gdpzh = '" + this.textBox1.Text + "'";
                    p_SqlClass.GetDataSet(str_sql, "国家和省级重点工程违法用地分类比例统计表", pDataSet);
                    //pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
                    Crystal_Report.Report.Rpt_gjhsjzdgcwfydflbltj mrpt8 = new Crystal_Report.Report.Rpt_gjhsjzdgcwfydflbltj();
                    mrpt8.SetDataSource(pDataSet.Tables["国家和省级重点工程违法用地分类比例统计表"]);
                    pfrm_tjbb.Text = "国家和省级重点工程违法用地分类比例统计表";
                    pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt8;

                    pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
                    pfrm_tjbb.crystalReportViewer1.Refresh();
                    //
                    ////Crystal_Report.Report.Rpt_wtdk_tb mrpt_tb = new Rpt_wtdk_tb();
                    ////mrpt_tb.SetDataSource(pDataSet.Tables["问题地块"]);
                    ////pfrm_tjbb.crystalReportViewer2.ReportSource = mrpt_tb;
                    pfrm_tjbb.crystalReportViewer2.Refresh();
                    //
                    pfrm_tjbb.MdiParent = this.ParentForm; ;
                    pfrm_tjbb.Show();

                    break;

                case "统计表选择":

                    MessageBox.Show("请选择统计表！");
                    break;
            }
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);

            }
        }
        private void m_frmDataQueryShow_ZoomToFea(string OID)
        {
            //if (this.bhtbReportQHDM.Length == 4)
            //{
            //    openfrmBHTBView(this.bhtbReportQHDM, Convert.ToInt32(OID), this.bhtbReportZQM);
            //}
            //else
            //{
            //    openfrmBHTBView(this.bhtbReportQHDM, Convert.ToInt32(OID), this.bhtbReportZQM);
            //}

            //Functions.MapFunction.ZoomToSelFeaByFID("土地核查", "objectid", OID, m_frmBHTBView.axMapControl1);//this.parenForm.m_frmMapView.axMapControl1);
        }
        private void btnTJBB_ReCa_Click(object sender, EventArgs e)
        {
            labHCBB_Info.Text = "";
        }

        private void uctTJFX_Load(object sender, EventArgs e)
        {
            
           
        }

        private void btnOK_ZFRWFX_Click(object sender, EventArgs e)
        {
            try{
            string m_strSQL = "";

            m_strSQL = "SELECT rwid as 任务编号,FSSJ AS 登记时间,WCSX AS 完成时限,WCSJ AS 完成时间,RWDKID AS 任务地块标识, DCDKID AS 调查地块标识,";
            m_strSQL = m_strSQL + " dcjgzb as 地块坐标,dcjgnr as 调查结果,ZFRYXM AS 调查人姓名, BGDH AS 办公电话,";
            m_strSQL = m_strSQL + "ZFRYSJ AS 手机,NBXH AS 内部小号,SGZH AS 上岗证号,ZFDWMC AS 所属单位,";

            if (m_DataAccess_SYS_.ProviderIsMSSQLDB())
            {
                m_strSQL = m_strSQL + "ZFDWDM AS 所属单位编号 FROM RW_ZFRY_ZFDW_DCJG WHERE  wcsj between '" + uctTJFX_ZFRWTJ.m_dateStartTime.ToShortDateString() + "' and '" + uctTJFX_ZFRWTJ.m_dateEndTime.ToShortDateString() + "'";
            }
            else
            {
                m_strSQL = m_strSQL + "ZFDWDM AS 所属单位编号 FROM RW_ZFRY_ZFDW_DCJG WHERE  wcsj between to_date('" + uctTJFX_ZFRWTJ.m_dateStartTime.ToShortDateString() + "','YYYY-MM-DD HH24:MI:SS') and to_date('" + uctTJFX_ZFRWTJ.m_dateEndTime.ToShortDateString() + "','YYYY-MM-DD HH24:MI:SS')";
            }

            
            if (chk_ZFRWTJFX_FXKYDK.Checked && chk_ZFRWTJFX_WFXKYDK.Checked)
                {
                    //包括发现和未发现可疑地块
                }
                else if (chk_ZFRWTJFX_FXKYDK.Checked)
                {
                    m_strSQL = m_strSQL + " and  dcdkid is not null";
                }
                else if (chk_ZFRWTJFX_WFXKYDK.Checked)
                {
                    m_strSQL = m_strSQL + " and  dcdkid is null";
                }

                if (chk_ZFRWTJFX_CQ.Checked && chk_ZFRWTJFX_WCQ.Checked)
                {
                    //包括超期和未超期可疑地块
                }
                else if (chk_ZFRWTJFX_CQ.Checked)
                {
                    m_strSQL = m_strSQL + " and  (wcsj-fssj) > wcsx";
                }
                else if (chk_ZFRWTJFX_WCQ.Checked)
                {
                    m_strSQL = m_strSQL + " and  (wcsj-fssj) <= wcsx";
                }


                if (chk_ZFRWTJFX_WWC.Checked && chk_ZFRWTJFX_YWC.Checked)
                {
                    //包括已完成和未完成
                }
                else if (chk_ZFRWTJFX_YWC.Checked)
                {
                    m_strSQL = m_strSQL + " and  (dcjgnr is not null or dcjgnr <>'')";
                }
                else if (chk_ZFRWTJFX_WWC.Checked)
                {
                    m_strSQL = m_strSQL + " and  dcjgnr is null";
                }


                if (uctTJFX_ZFRWTJ.chkXZQ.Checked == true && uctTJFX_ZFRWTJ.m_strXZQDM!=null )
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strSQL + "  and  substr(ZFRYBH,1," + uctTJFX_ZFRWTJ.m_strXZQDM.Length + ")='" + uctTJFX_ZFRWTJ.m_strXZQDM + "'";
                    }
                    else
                    {
                        m_strSQL = m_strSQL + "  and  LEFT(ZFRYBH," + uctTJFX_ZFRWTJ.m_strXZQDM.Length + ")='" + uctTJFX_ZFRWTJ.m_strXZQDM + "'";
                    }
                }

                if (uctTJFX_ZFRWTJ.chkDW.Checked == true)
                {
                    m_strSQL = m_strSQL + " and  ZFDWDM='" + uctTJFX_ZFRWTJ.m_strDWDM + "'";

                }

                if (uctTJFX_ZFRWTJ.chkRY.Checked == true)
                {
                    m_strSQL = m_strSQL + " and  ZFRYBH ='" + uctTJFX_ZFRWTJ.m_strRYBH + "'";

                }
            


                DataTable m_DataTable;
                m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);

                //if (uctTJFX_ZFRWTJ_JGQueryEvent != null)
                //{
                //    uctTJFX_ZFRWTJ_JGQueryEvent(m_DataTable);
                //}

                if (m_Panel_ == null)
                {
                    m_Panel = (Panel)clsFunction.Function.GetControl(m_ExpandablePanel_Main_, "PanelInfo_Panel");
                    clsFunction.Function.ClearControls(m_Panel_);
                }
                m_ExpandablePanel_Main_.Text = "执法任务计";
                m_ExpandablePanel_Main_.TitleText = "执法任务计";

                if (clsFunction.Function.HasControl(m_Panel_, "m_uctTJFX_ZFRWFX_JG") == false)
                {
                    clsFunction.Function.ClearControls(m_Panel_);
                    m_uctTJFX_ZFRWFX_JG = new JCZF.SubFrame.uctTJFX_ZFRWFX_JG();
                    m_uctTJFX_ZFRWFX_JG.Name = "m_uctTJFX_ZFRWFX_JG";
                    m_uctTJFX_ZFRWFX_JG.Dock = DockStyle.Fill;
                    m_Panel_.Controls.Add(m_uctTJFX_ZFRWFX_JG);
                }

                m_uctTJFX_ZFRWFX_JG.m_DataGridView.DataSource = m_DataTable;
                m_ExpandablePanel_Main_.Visible = true;
                m_ExpandablePanel_Main_.Expanded = true;
                m_uctTJFX_ZFRWFX_JG.m_DataAccess_SYS_ = m_DataAccess_SYS_;
                
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);

            }
        }

        private void btnCQXC_OK_Click(object sender, EventArgs e)
        {
            
            //TimeSpan m_TimeSpan = new TimeSpan(integerInput_QXSZ.Value, 0, 0, 0);
            DateTime dtStar ;
            DateTime dtEnd ;

            

            string m_strXZQDM = "";
            m_strXZQDM = uctTJFX_1_XCCQ.m_strXZQDM;

            try
            {
                string m_strCommandText ="";
                if (chk_DQCQ.Checked)
                {
                    dtStar = DateTime.Now.AddDays(-integerInput_QXSZ.Value);
                    dtEnd = DateTime.Today;

                    m_strCommandText = "SELECT * from XZQ_CJ_ZFWG_ZFRY_ZFDW WHERE ( 最近巡查时间 < '" + dtStar.ToShortDateString() + "' or 最近巡查时间 is null )";
                }

                if (chk_LSCQ.Checked)
                {
                    dtStar = uctTJFX_1_XCCQ.m_dateStartTime;
                    dtEnd = uctTJFX_1_XCCQ.m_dateEndTime;
                    if (dtStar.Year < 1900 || dtEnd.Year < 1900 || dtStar.Year > DateTime.Now.Year)
                    {
                        return;
                    }
             //       dtStar = uctTJFX_1_XCCQ.m_strStartTime - m_TimeSpan;
             //dtEnd =uctTJFX_1_XCCQ.m_strEndTime;
             //       m_strCommandText =  "SELECT dcgjfxb.XCSJ, XZQ_CJ_ZFWG_ZFRY_ZFDW.XZQDM,XZQ_CJ_ZFWG_ZFRY_ZFDW.XZQMC,XZQ_CJ_ZFWG_ZFRY_ZFDW.ZFRYXM,";
             //        m_strCommandText =  m_strCommandText +"XZQ_CJ_ZFWG_ZFRY_ZFDW.ZFDWMC,XZQ_CJ_ZFWG_ZFRY_ZFDW.BGDH,XZQ_CJ_ZFWG_ZFRY_ZFDW.NBXH,";
             //       m_strCommandText =  m_strCommandText +"XZQ_CJ_ZFWG_ZFRY_ZFDW.SJ FROM XZQ_CJ_ZFWG_ZFRY_ZFDW LEFT JOIN DCGJFXB ON DCGJFXB.XZQDM=XZQ_CJ_ZFWG_ZFRY_ZFDW.XZQDM ";
             //       m_strCommandText =  m_strCommandText +"WHERE (XZQ_CJ_ZFWG_ZFRY_ZFDW.xzqdm NOT IN (SELECT xzqdm FROM dcgjfxb WHERE xcsj >= '"+dtStar.ToShortDateString()+"'  AND   xcsj <= '"+dtEnd.ToShortDateString()+"') ";
             //       m_strCommandText =  m_strCommandText +"ORDER BY XZQ_CJ_ZFWG_ZFRY_ZFDW.xzqdm";
                }

                if (uctTJFX_1_XCCQ.chkXZQ.Checked)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strCommandText = m_strCommandText + " and  substr(行政区代码,1," + uctTJFX_1_XCCQ.m_strXZQDM.Length + ")='" + uctTJFX_1_XCCQ.m_strXZQDM + "' ";
                    }
                    else
                    {
                        m_strCommandText = m_strCommandText + " and  left(行政区代码," + uctTJFX_1_XCCQ.m_strXZQDM.Length + ")='" + uctTJFX_1_XCCQ.m_strXZQDM + "' ";
                    }
                }

                if (uctTJFX_1_XCCQ.chkDW.Checked)
                {
                    m_strCommandText = m_strCommandText + " and ( 本级执法人员所在单位代码='" + uctTJFX_1_XCCQ.m_strDWDM + "' or 上级执法人员所在单位代码='" + uctTJFX_1_XCCQ.m_strDWDM+"' ) ";
                }
                if (uctTJFX_1_XCCQ.chkRY.Checked)
                {
                    m_strCommandText = m_strCommandText + " and ( 本级执法人员编号='" + uctTJFX_1_XCCQ.m_strRYBH + "' or 上级执法人员编号='" + uctTJFX_1_XCCQ.m_strRYBH + "') ";
                }
                //m_strCommandText = m_strCommandText + "

                DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strCommandText);
                DataRow m_DataRow;
                //if (m_DataTable == null) return;
                if (chk_DQCQ.Checked)
                {
                    //if (uctTJFX_XCCQTJ_JGQueryEvent != null)
                    //{
                    //    uctTJFX_XCCQTJ_JGQueryEvent(m_DataTable);
                    //}


                    if (m_Panel_ == null)
                    {
                        m_Panel = (Panel)clsFunction.Function.GetControl(m_ExpandablePanel_Main_, "PanelInfo_Panel");
                        //ClearControls(m_Panel);
                        clsFunction.Function.ClearControls(m_Panel_);
                    }
                    m_ExpandablePanel_Main_.Text = "巡查超期统计";
                    m_ExpandablePanel_Main_.TitleText = "巡查超期统计";
                    //m_TabControl_Main.Visible = false;

                    if (clsFunction.Function.HasControl(m_Panel_, "m_uctTJFX_XCCQ_JG") == false || m_uctTJFX_XCCQ_JG==null )
                    {
                        clsFunction.Function.ClearControls(m_Panel_);
                        m_uctTJFX_XCCQ_JG = new JCZF.SubFrame.uctTJFX_XCCQ_JG();
                        m_uctTJFX_XCCQ_JG.Name = "m_uctTJFX_XCCQ_JG";
                        m_uctTJFX_XCCQ_JG.Dock = DockStyle.Fill;
                        m_Panel_.Controls.Add(m_uctTJFX_XCCQ_JG);
                    }
                    //m_frmDataQueryShow.MdiParent = this.ParentForm; ;
                    m_uctTJFX_XCCQ_JG.m_DataGridView.DataSource = m_DataTable;
                    m_ExpandablePanel_Main_.Visible = true;
                    m_ExpandablePanel_Main_.Expanded = true;
                    m_uctTJFX_XCCQ_JG.m_DataAccess_SYS_ = m_DataAccess_SYS_;
                    //m_uctTJFX_XCCQ_JG.m_IFeatureLayer = m_IFeatureLayer;
                    //m_uctTJFX_XCCQ_JG.m_AxMapControl = m_AxMapControl_;        

                    return;
                }
                if(chk_LSCQ.Checked )
                {
                    clsFunction.Function.MessageBoxInformation("正在建设用地！");
                }

              
            }
            catch (SystemException  ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
            finally
            {
                //conn.Close();
            }
            
              
        }

        private void chk_DQCQ_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void chk_LSCQ_CheckedChanged(object sender, EventArgs e)
        {

        }

       
       
     
        
    }
}
