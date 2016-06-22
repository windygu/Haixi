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
    public partial class uctWGGL : UserControl
    {
        private string m_strDSXZQ_SQL = "select * from DSXZQ_TDLY";
        private string m_strQXXZQ_SQL = "select * from XJZJ_TDLY";
        private string m_strXZXZQ_SQL = "select * from ZJZJ_TDLY";
        private string m_strCJXZQ_SQL = "select * from SYTC_TDLY";

       

        public string m_strStaff_UserName;
        public int m_intSTAFF_ID;
        public string m_strUserName;
        private string m_strQHDM;

        private bool m_blIsAdmin;
        private WGGL.clsZFDW m_clsZFDW;
        private WGGL.clsZFRY m_clsZFRY;
        WGGL.frmWCGL_JG_DW m_frmWCGL_JG_DW;
        WGGL.frmWCGL_JG_RY m_frmWCGL_JG_RY;
        WGGL.frmWCGL_JG_WG m_frmWCGL_JG_WG;
        private clsDataAccess.DataAccess m_DataAccess_SYS_;
        public clsDataAccess.DataAccess m_DataAccess_SYS
        {
            set
            {
                m_DataAccess_SYS_ = value;            
            }
        }

        private clsDataAccess.DataAccess m_DataAccess_SDE_;
        public clsDataAccess.DataAccess m_DataAccess_SDE
        {
            set
            {
                m_DataAccess_SYS_ = value;
            }
        }

        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                uctXZQTree_Dev_ZFDW.m_AxMapControl = value;
                uctXZQTree_Dev_ZFRY.m_AxMapControl = value;
                //uctXZQTree_Dev_ZFWG.m_AxMapControl = value;
            }
        }

        private System.Data.DataTable m_DataTableSJXZQ;
        private System.Data.DataTable m_DataTableDSXZQ;
        private System.Data.DataTable m_DataTableQXXZQ;
        private System.Data.DataTable m_DataTableXZXZQ;
        private System.Data.DataTable m_DataTableCunXZQ;


        public uctWGGL()
        {
            m_strQHDM = "";
            InitializeComponent();
            m_clsZFDW = new JCZF.SubFrame.WGGL.clsZFDW();
        }

        private void uctXZQTree_Dev1_uctXZQTree_DevSelectEvent(string p_strXZQDM)
        {
            m_strQHDM = p_strXZQDM;
            FillComb_ZFDW(cmbDW_ZFDW, p_strXZQDM);
            clsFunction.Function.ComboBox_AdjustDropDownListWidth(cmbDW_ZFDW);
        }


        

        private void FillComb_ZFDW(ComboBox p_ComboBox, string p_strXZQDM)
        {    
            m_clsZFDW.m_DataAccess_SYS_ = m_DataAccess_SYS_;
            m_clsZFDW.m_strXZQDM = p_strXZQDM;

            DataTable m_DataTable=m_clsZFDW.GetZFDW();

            if (m_DataTable == null)
            {
                p_ComboBox.DataSource = null;
                return;
            }
            DataRow m_DataRow=m_DataTable.NewRow();
            m_DataRow["ZFDWDM"]="0";
            m_DataRow["ZFDWMC"]="全部";
            m_DataTable.Rows.InsertAt(m_DataRow, 0);
            p_ComboBox.DataSource = m_DataTable.DefaultView;
            p_ComboBox.ValueMember = "ZFDWDM";
            p_ComboBox.DisplayMember = "ZFDWMC";
            if (p_ComboBox.Items.Count > 0)
            {
                p_ComboBox.SelectedIndex = 0;
            }          
          
        }

        private void btnQuery_ZFDW_Click(object sender, EventArgs e)
        {
            if (cmbDW_ZFDW.Text == "")
            {
                m_DataAccess_SYS_.MessageInforShow(this.ParentForm, "没有数据！");
                return;
            }
            ShowfrmWCGL_JG_DW(true);
        }
        
        private void ShowfrmWCGL_JG_DW(bool p_blReadOnly)
        {
            string m_strSQL = "";
            try
            {
                if (cmbDW_ZFDW.Text == "全部" || cmbDW_ZFDW.Text != "")
                {
                    if (uctXZQTree_Dev_ZFDW.m_strXZQDM.Length <= 6)
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strSQL = "SELECT id as 标识 ,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间  FROM ZFDW WHERE substr(ZFDWDM,1," + uctXZQTree_Dev_ZFDW.m_strXZQDM.Length + ")='" + uctXZQTree_Dev_ZFDW.m_strXZQDM + "' order by zfdwdm";
                        }
                        else
                        {
                            m_strSQL = "SELECT id as 标识 ,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间  FROM ZFDW WHERE LEFT(ZFDWDM," + uctXZQTree_Dev_ZFDW.m_strXZQDM.Length + ")='" + uctXZQTree_Dev_ZFDW.m_strXZQDM + "' order by zfdwdm";
                        }
                    }
                    else
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strSQL = "SELECT  id as 标识 ,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间 FROM ZFDW WHERE substr(ZFDWDM,1,6)='" + uctXZQTree_Dev_ZFDW.m_strXZQDM + "'  order by zfdwdm";
                        }
                        else
                        {
                            m_strSQL = "SELECT  id as 标识 ,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间 FROM ZFDW WHERE LEFT(ZFDWDM,6)='" + uctXZQTree_Dev_ZFDW.m_strXZQDM + "'  order by zfdwdm";
                        }
                    }
                }
                else 
                {

                    m_strSQL = "SELECT  id as 标识 ,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间 FROM ZFDW WHERE ZFDWDM='" + cmbDW_ZFDW.SelectedValue + "'  order by zfdwdm";

                }                

                if (m_frmWCGL_JG_DW == null || m_frmWCGL_JG_DW.IsDisposed == true)
                {
                    //m_frmWCGL_JG_DW = null;
                    m_frmWCGL_JG_DW = new JCZF.SubFrame.WGGL.frmWCGL_JG_DW();
                    m_frmWCGL_JG_DW.m_DataAccess_SYS = this.m_DataAccess_SYS_;
                    m_frmWCGL_JG_DW.MdiParent = this.ParentForm;
                }
                m_frmWCGL_JG_DW.m_strXZQDM = uctXZQTree_Dev_ZFDW.m_strXZQDM;

                if (cmbDW_ZFDW.SelectedValue != null)
                {
                    m_frmWCGL_JG_DW.m_strZFDWDM = cmbDW_ZFDW.SelectedValue.ToString();
                }
                else
                {
                    m_frmWCGL_JG_DW.m_strZFDWDM = "";
                }
               

                 m_frmWCGL_JG_DW.dataGridViewX1.ReadOnly = p_blReadOnly;     
                 m_frmWCGL_JG_DW.panelEx_Button.Visible = !p_blReadOnly;          
                m_frmWCGL_JG_DW.InitDataGridview();
     
         
                m_frmWCGL_JG_DW.Show();
                m_frmWCGL_JG_DW.BringToFront();

            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void btnEDIT_ZFDW_Click(object sender, EventArgs e)
        {
            ShowfrmWCGL_JG_DW(false);
        }

        private void btnQuery_ZFRY_Click(object sender, EventArgs e)
        {
            ShowfrmWCGL_JG_RY(true );
        }

        private void uctXZQTree_Dev_ZFRY_uctXZQTree_DevSelectEvent(string p_strXZQDM)
        {
            m_strQHDM = p_strXZQDM;
            FillComb_ZFDW(cmbDW_ZFRY, p_strXZQDM);
            clsFunction.Function.ComboBox_AdjustDropDownListWidth(cmbDW_ZFRY);
        }

        private void cmbDW_ZFRY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDW_ZFRY.SelectedValue != null)
            {
                FillComb_ZFRY(cmbRY_ZFRY, cmbDW_ZFRY.SelectedValue.ToString());
            }
            else
            {
                cmbRY_ZFRY.Items.Clear();
            }
            if (cmbDW_ZFRY.SelectedItem == null || cmbDW_ZFRY.SelectedValue == null || cmbDW_ZFRY.SelectedValue == "0")
            {
                btnEDIT_ZFRY.Enabled = false;
            }
            else
            {
                btnEDIT_ZFRY.Enabled = true ;
            }
        }

        private void FillComb_ZFRY(ComboBox p_ComboBox, string p_strZFDWDM)
        {
            try
            {
                if (p_strZFDWDM == "0")
                {
                    p_strZFDWDM = m_strQHDM;
                }
                if (m_clsZFRY == null) m_clsZFRY = new JCZF.SubFrame.WGGL.clsZFRY();
                m_clsZFRY.m_DataAccess_SYS_ = m_DataAccess_SYS_;
                m_clsZFRY.m_strZFDW_ZFDWDM = p_strZFDWDM;

                DataTable m_DataTable = m_clsZFRY.GetZFRY();

                if (m_DataTable == null)
                {
                    p_ComboBox.DataSource = null;
                    return;
                }
                DataRow m_DataRow = m_DataTable.NewRow();
                m_DataRow["ZFRYBH"] = "0";
                m_DataRow["ZFRYXM"] = "全部";
                m_DataTable.Rows.InsertAt(m_DataRow, 0);
                p_ComboBox.DataSource = m_DataTable.DefaultView;
                p_ComboBox.ValueMember = "ZFRYBH";
                p_ComboBox.DisplayMember = "ZFRYXM";
                if (p_ComboBox.Items.Count > 0)
                {
                    p_ComboBox.SelectedIndex = 0;
                }
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_blReadOnly">是否是查询数据（只读，不编辑）</param>
        private void ShowfrmWCGL_JG_RY(bool p_blReadOnly)
        {
            string m_strSQL = "";
            string m_strSQLTemp = "";
            string m_strSQLTemp1 = "";
            try
            {
                if (p_blReadOnly == true)
                {
                    //查询
                    m_strSQL = "SELECT  id as 标识 ,zfryxm as 执法人员姓名,zfrybh as 执法人员编号,sj as 手机号码, bgdh as 办公电话, nbxh as 内部小号, sgzh as 上岗证号,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间,zzzt as 在职状态  FROM ZFRY_ZFDW";
                }
                else
                {
                    //编辑
                    m_strSQL = "SELECT  id as 标识 ,zfryxm as 执法人员姓名,zfrybh as 执法人员编号,sj as 手机号码, bgdh as 办公电话, nbxh as 内部小号, sgzh as 上岗证号,zfdwdm as 执法单位代码,djsj as 登记时间,zzzt as 在职状态  FROM ZFRY";
                }
                 if (chkZFDW.Checked)
                 {
                     if (cmbDW_ZFRY.Text == "全部")
                     {
                         if (m_DataAccess_SYS_.ProviderIsOraDB())
                         {
                             m_strSQLTemp = "  substr(ZFDWDM,1," + m_strQHDM.Length + ")='" + m_strQHDM + "'";
                         }
                         else
                         {
                             m_strSQLTemp = "  LEFT(ZFDWDM," + m_strQHDM.Length + ")='" + m_strQHDM + "'";
                         }
                     }
                     else
                     {
                         if (cmbDW_ZFRY.SelectedValue != null)
                         {
                             m_strSQLTemp = " zfdwdm='" + cmbDW_ZFRY.SelectedValue.ToString() + "'";
                         }
                     }
                    
                 }
                 if (chkZFRY.Checked)
                 {
                     if (cmbRY_ZFRY.Text == "全部")
                     {
                         //与上面条件重复，不再增加过滤条件

                         //if (m_strSQLTemp != "")
                         //{
                         //    //与上面条件重复，不再增加过滤条件
                         //}
                         //else
                         //{
                         //    m_strSQLTemp = "  ZFDWDM," + cmbDW_ZFRY.SelectedValue.ToString() + ")='" + uctXZQTree_Dev_ZFDW.m_strXZQDM + "'";
                         //}
                     }
                     else
                     {
                         if (cmbRY_ZFRY.SelectedValue != null)
                         {
                             m_strSQLTemp1 = " zfrybh='" + cmbRY_ZFRY.SelectedValue.ToString() + "'";
                         }
                         else if (cmbRY_ZFRY.Text != "")
                         {
                             m_strSQLTemp1 = " zfryxm like'%" + cmbRY_ZFRY.Text + "%'";
                         }
                         else
                         {
                             //不予处理
                         }
                         if (m_strSQLTemp1 != "")
                         {
                             if (m_strSQLTemp != "" )
                             {
                                 m_strSQLTemp = m_strSQLTemp + " and " + m_strSQLTemp1;
                             }
                             else if (m_strSQLTemp == "" )
                             {
                                 m_strSQLTemp = m_strSQLTemp1;
                             }
                         }
                        
                     }
                 }


                 if (chkSGZH.Checked)
                 {                     
                         if (m_strSQLTemp != "")
                         {
                             m_strSQLTemp =m_strSQLTemp+ " and  SGZH='" + txtSGZH.Text  + "'";
                         }
                         else
                         {
                             m_strSQLTemp = " SGZH='" + txtSGZH.Text + "'";
                         }                    
                 }

                 if (chkSJ.Checked)
                 {
                     if (m_strSQLTemp != "")
                     {
                         m_strSQLTemp =m_strSQLTemp+ " and  SJ='" + txtSJ.Text + "'";
                     }
                     else
                     {
                         m_strSQLTemp = "  SJ='" + txtSJ.Text + "'";
                     }
                 }

                 if (chkBGDH.Checked)
                 {
                     if (m_strSQLTemp != "")
                     {
                         m_strSQLTemp =m_strSQLTemp+ " and  BGDH='" + txtBGDH.Text + "'";
                     }
                     else
                     {
                         m_strSQLTemp = " BGDH='" + txtBGDH.Text + "'";
                     }
                 }

                 if (chkNBXH.Checked)
                 {
                     if (m_strSQLTemp != "")
                     {
                         m_strSQLTemp =m_strSQLTemp+ " and  NBXH='" + txtNBXH.Text + "'";
                     }
                     else
                     {
                         m_strSQLTemp = " NBXH='" + txtNBXH.Text + "'";
                     }
                 }
                 if (m_strSQLTemp.Trim() != "")
                 {
                     m_strSQL = m_strSQL + " where " + m_strSQLTemp + " ORDER BY ZFRYBH";
                 }
                 else
                 {
                     m_strSQL = m_strSQL +  " ORDER BY ZFRYBH";
                 }

                if (m_frmWCGL_JG_RY == null || m_frmWCGL_JG_RY.IsDisposed == true)
                {
                    m_frmWCGL_JG_RY = null;
                    m_frmWCGL_JG_RY = new JCZF.SubFrame.WGGL.frmWCGL_JG_RY();
                    m_frmWCGL_JG_RY.m_DataAccess_SYS = this.m_DataAccess_SYS_;
                    m_frmWCGL_JG_RY.MdiParent = this.ParentForm;
                }
                
                m_frmWCGL_JG_RY.m_strSQL = m_strSQL;
                m_frmWCGL_JG_RY.m_strXZQDM = uctXZQTree_Dev_ZFRY.m_strXZQDM;
                

                if (cmbDW_ZFRY.SelectedValue != null)
                {
                    m_frmWCGL_JG_RY.m_strZFDWDM = cmbDW_ZFRY.SelectedValue.ToString();
                    m_frmWCGL_JG_RY.m_strZFDWMC = cmbDW_ZFRY.Text ;
                }
                else
                {
                    m_frmWCGL_JG_RY.m_strZFDWDM = "";

                }

               
               
                m_frmWCGL_JG_RY.dataGridViewX1.ReadOnly = p_blReadOnly;
                m_frmWCGL_JG_RY.panelEx_Button.Visible = !p_blReadOnly;
                m_frmWCGL_JG_RY.InitDataGridview();


                //if (p_blReadOnly == false)
                //{
                    //if (cmbDW_ZFRY.SelectedValue == "0" || cmbDW_ZFRY.Text == "全部")
                    //{
                        //m_frmWCGL_JG_RY.dataGridViewX1.ReadOnly = true;//当没有单位时不能编辑
                //        m_frmWCGL_JG_RY.panelEx_Button.Visible = false;
                //    }
                //}
                //else
                //{
                //    m_frmWCGL_JG_RY.panelEx_Button.Visible = !p_blReadOnly;
                //}
                if (m_frmWCGL_JG_RY == null || m_frmWCGL_JG_RY.IsDisposed == true || m_frmWCGL_JG_RY.Visible==false )
                {
                    
                    m_frmWCGL_JG_RY.Show();
                }
                m_frmWCGL_JG_RY.BringToFront();

            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void btnEDIT_ZFRY_Click(object sender, EventArgs e)
        {
            ShowfrmWCGL_JG_RY(false  );
        }

        private void btnQuery_ZFWG_Click(object sender, EventArgs e)
        {
            ShowfrmWCGL_JG_WG(false);
        }

        private void btnEDIT_ZFWG_Click(object sender, EventArgs e)
        {
            ShowfrmWCGL_JG_WG(false );
        }

        private void uctXZQTree_Dev_ZFWG_uctXZQTree_DevSelectEvent(string p_strXZQDM)
        {
            //FillComb_ZFDW(cmbDW_ZFWG, p_strXZQDM);
        }

        private void cmbDW_ZFWG_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbDW_ZFWG.SelectedValue != null)
            //{
            //    FillComb_ZFRY(cmbRY_ZFWG, cmbDW_ZFWG.SelectedValue.ToString());
            //}
            //else
            //{
            //    cmbRY_ZFWG.Items.Clear();
            //}
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_blReadOnly">是否是查询数据（只读，不编辑）</param>
        private void ShowfrmWCGL_JG_WG(bool p_blReadOnly)
        {
            try{
             if (m_frmWCGL_JG_WG == null || m_frmWCGL_JG_WG.IsDisposed == true)
                {
                    m_frmWCGL_JG_WG = null;
                    m_frmWCGL_JG_WG = new JCZF.SubFrame.WGGL.frmWCGL_JG_WG();
                    m_frmWCGL_JG_WG.m_DataAccess_SYS = this.m_DataAccess_SYS_;
                    m_frmWCGL_JG_WG.MdiParent = this.ParentForm;
                }


             m_frmWCGL_JG_WG.SetButton(false);
          //m_frmWCGL_JG_WG.dataGridViewX1.se = -1;
               

                m_frmWCGL_JG_WG.m_strSQL = CreateSQL();
                //m_frmWCGL_JG_WG.m_strXZQDM = uctXZQTree_Dev_ZFRY.m_strXZQDM;


                //if (cmbDW_ZFWG.SelectedValue != null)
                //{
                //    m_frmWCGL_JG_WG.m_strZFDWDM = cmbDW_ZFWG.SelectedValue.ToString();
                //    m_frmWCGL_JG_WG.m_strZFDWMC = cmbDW_ZFWG.Text;
                //}
                //else
                //{
                //    m_frmWCGL_JG_WG.m_strZFDWDM = "";

                //}

                //m_frmWCGL_JG_WG.dataGridViewX1.ReadOnly = true ;
                //m_frmWCGL_JG_WG.panelEx_Button.Visible = !p_blReadOnly;
                m_frmWCGL_JG_WG.m_strUserName = this.m_strUserName;
                m_frmWCGL_JG_WG.InitDataGridview();


                //if (p_blReadOnly == false)
                //{
                //    if (cmbDW_ZFWG.SelectedValue == "0" || cmbDW_ZFWG.Text == "全部")
                //    {
                //        m_frmWCGL_JG_WG.dataGridViewX1.ReadOnly = true;//当没有单位时不能编辑
                //        m_frmWCGL_JG_WG.panelEx_Button.Visible = false;
                //    }
                //}

                m_frmWCGL_JG_WG.Show();
                m_frmWCGL_JG_WG.BringToFront();

            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void Query()
        {
            string m_strDataGrid_SQL = "";
            //this.labInfo.Visible = false;
            try
            {
                //SetHLinkUnVisible();
                m_strDataGrid_SQL = CreateSQL();
                if (m_strDataGrid_SQL != "")
                {
                    DataGridZFWGDataBind(m_strDataGrid_SQL);
                    //if (DataGridZFWG.Items.Count > 0 && DataGridZFWG.SelectedIndex < 0) DataGridZFWG.SelectedIndex = 0;                
                }
                else
                {

                }
            }
            catch (System.Exception errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void DataGridZFWGDataBind(string p_strQueryString)
        {
            //m_frmWCGL_JG_WG.dataGridViewX1.CurrentPageIndex = 0;
            m_frmWCGL_JG_WG.dataGridViewX1.DataSource = m_DataAccess_SYS_.getDataTableByQueryString(p_strQueryString).DefaultView;
            //m_frmWCGL_JG_WG.dataGridViewX1.DataBind();


        }

        private string CreateSQL()
        {
            string m_strZFWG_SQL_DSXZQ_ZFDW = "select * from XZQ_DS_ZFWG_ZFRY_ZFDW";
            string m_strZFWG_SQL_QXXZQ_ZFDW = "SELECT * FROM XZQ_XJ_ZFWG_ZFRY_ZFDW";
            string m_strZFWG_SQL_XZXZQ_ZFDW = "SELECT * FROM XZQ_ZJ_ZFWG_ZFRY_ZFDW";
            string m_strZFWG_SQL_CJXZQ_ZFDW = "SELECT * FROM XZQ_CJ_ZFWG_ZFRY_ZFDW";
            string m_strDataGrid_SQL = "";
            string m_strXZQDM_Temp = "";
            string m_strXZQMC_Temp = "";
            string m_strTableName = "";
            try
            {

                //if (this.labStaff_UserName.Text == "" || this.labStaff_UserName.Text == "21")
                //{
                //    m_strXZQDM_Temp = "";
                //    //return m_strXZQDM_Temp;
                //    //m_strXZQDM_Temp = this.DropDownListQXXZQ.SelectedItem.Value;
                //    //m_strDataGrid_SQL = " WHERE LEFT(XJZJ_TDLY.xzqdm," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                //    //m_strTableName = "XJZJ_TDLY"; 
                //    m_strDataGrid_SQL = " WHERE jssj is null ";
                //    m_strTableName = "DSXZQ_TDLY";   
                //}
                //else
                //{

                if (chkCun.Checked == true && DropDownListCun.SelectedIndex >= 0)
                {                    
                    m_strXZQDM_Temp = this.DropDownListCun.SelectedValue.ToString();
                    if (DropDownListCun.SelectedValue.ToString().Length == 9)
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                        else
                        {
                            m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }             
                    }
                    else
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                        else
                        {
                            m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                    }
                    m_strTableName = "SYTC_TDLY";
                }
                else if (chkXZ.Checked==true && DropDownListXZXZQ.SelectedIndex >= 0)
                {                    
                    m_strXZQDM_Temp = this.DropDownListXZXZQ.SelectedValue.ToString();
                    if (DropDownListXZXZQ.SelectedValue.ToString().Length == 9)
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                        else
                        {
                            m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }               
                    }
                    else                    
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                        else
                        {
                            m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                    }
                    m_strTableName = "ZJZJ_TDLY";
                }
                else if (chkQX.Checked==true  && DropDownListQXXZQ.SelectedIndex >= 0)
                {
                    m_strXZQDM_Temp = this.DropDownListQXXZQ.SelectedValue.ToString();

                    if (m_DataAccess_SYS_.IsSZGX(m_strUserName))
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                        else
                        {
                            m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                    }
                    else if (m_strXZQDM_Temp.Length == 4)
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                        else
                        {
                            m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                    }
                    else
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "' and  (行政区代码 not in (select xzqdm from dsxzq_tdly where length(xzqdm)=6))";
                        }
                        else
                        {
                            m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "' and  (行政区代码 not in (select xzqdm from dsxzq_tdly where len(xzqdm)=6))";
                        }
                    }
                    m_strTableName = "XJZJ_TDLY";
                }
                else if (DropDownListDSXZQ.SelectedIndex >= 0)
                {
                    if (m_strUserName.Length > 2)
                    {
                        m_strXZQDM_Temp = this.DropDownListDSXZQ.SelectedValue.ToString();

                        if (m_strUserName.Length ==6 &&  m_DataAccess_SYS_.IsSZGX(m_strUserName))
                        {
                            if (m_DataAccess_SYS_.ProviderIsOraDB())
                            {
                                m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                            }
                            else
                            {
                                m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                            }
                        }
                        else
                        {
                            if (m_DataAccess_SYS_.ProviderIsOraDB())
                            {
                                m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "' and  (行政区代码 not in (select xzqdm from dsxzq_tdly where length(xzqdm)=6))";
                            }
                            else
                            {
                                m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "' and  (行政区代码 not in (select xzqdm from dsxzq_tdly where len(xzqdm)=6))";
                            }
                        }
                    }
                    else
                    {
                        m_strXZQDM_Temp = this.DropDownListDSXZQ.SelectedValue.ToString().Substring(0, 2);
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strDataGrid_SQL = " WHERE substr(行政区代码,1," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                        else
                        {
                            m_strDataGrid_SQL = " WHERE LEFT(行政区代码," + m_strXZQDM_Temp.Length + ")='" + m_strXZQDM_Temp + "'";
                        }
                    }
                    m_strTableName = "DSXZQ_TDLY";
                }
                //}


                //if (DropDownListDSXZQ.SelectedValue.ToString() == "2114")
                //{省直管县
                //    if (m_strDataGrid_SQL == "")
                //    {
                //        m_strDataGrid_SQL = "  where 行政区代码<>'211421' ";
                //    }
                //    else
                //    {
                //        m_strDataGrid_SQL = m_strDataGrid_SQL + "  and 行政区代码<>'211421' ";
                //    }
                //}

                if (chkWZDFZR_ZFWG.Checked==false && chkZDFZR_ZFWG.Checked ==true )
                {
                    if (m_strDataGrid_SQL == "")
                    {
                        m_strDataGrid_SQL = "  where 本级执法人员编号 is not null and  上级执法人员编号 is not null and 本级执法人员编号<>'' and  上级执法人员编号<>''";
                    }
                    else
                    {
                        m_strDataGrid_SQL = m_strDataGrid_SQL + "  and (本级执法人员编号 is not null and 本级执法人员编号<>'' or 上级执法人员编号 is not null and  上级执法人员编号<>'')";
                    }
                }

                 if (chkWZDFZR_ZFWG.Checked==true  && chkZDFZR_ZFWG.Checked ==false  )
                {
                    if (m_strDataGrid_SQL == "")
                    {
                        m_strDataGrid_SQL = "  where (本级执法人员编号 is  null or 本级执法人员编号='') and ( 上级执法人员编号 is  null  or  上级执法人员编号='')";
                    }
                    else
                    {
                        m_strDataGrid_SQL = m_strDataGrid_SQL + "  and (本级执法人员编号 is  null or 本级执法人员编号='') and  (上级执法人员编号 is  null or  上级执法人员编号='')";
                    }
                }


                if (m_strDataGrid_SQL == "")
                {
                    m_strDataGrid_SQL = " where ";
                }
                else
                {
                    m_strDataGrid_SQL = m_strDataGrid_SQL + " and  ";
                }

                m_strDataGrid_SQL = m_strDataGrid_SQL + " 结束时间 is  null and  ";//只显示当前的执法网格

                if (m_strTableName == "DSXZQ_TDLY")
                {
                    m_strDataGrid_SQL = m_strZFWG_SQL_DSXZQ_ZFDW + m_strDataGrid_SQL + "   行政区代码<>'' and 结束时间 is null ORDER BY 行政区代码";
                }
                else if (m_strTableName == "XJZJ_TDLY")
                {
                    m_strDataGrid_SQL = m_strZFWG_SQL_QXXZQ_ZFDW + m_strDataGrid_SQL + "   行政区代码<>''  and 结束时间 is null ORDER BY 行政区代码";
                }
                else if (m_strTableName == "ZJZJ_TDLY")
                {
                    m_strDataGrid_SQL = m_strZFWG_SQL_XZXZQ_ZFDW + m_strDataGrid_SQL + "   行政区代码<>''  and 结束时间 is null ORDER BY 行政区代码";
                }
                else if (m_strTableName == "SYTC_TDLY")
                {
                    m_strDataGrid_SQL = m_strZFWG_SQL_CJXZQ_ZFDW + m_strDataGrid_SQL + "   行政区代码<>''  and 结束时间 is null  ORDER BY 行政区代码";
                }




            }
            catch (System.Exception errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
            //labSQL.Text = m_strDataGrid_SQL;
            return m_strDataGrid_SQL;
        }

        

        private void btiWGGL_Click(object sender, EventArgs e)
        {
            InitData();   
        }

        public void InitData()
        {
            InitDropDownListDSXZQ();
            InitDropDownListQXXZQ();
            InitDropDownListXZXZQ();     
        }


        private void InitXZQTree(ESRI.ArcGIS.Controls.AxMapControl p_AxMapControl)
        {
           

            if (p_AxMapControl != null)
            {
                XZQTree.XzqTreeView m_XzqTreeView = new XZQTree.XzqTreeView();               
             m_XzqTreeView.setAxMapControl(p_AxMapControl);

             m_DataTableSJXZQ = m_XzqTreeView.m_DataTableSJXZQ;
             m_DataTableDSXZQ = m_XzqTreeView.m_DataTableDSXZQ;
             m_DataTableQXXZQ = m_XzqTreeView.m_DataTableQXXZQ;
             m_DataTableXZXZQ = m_XzqTreeView.m_DataTableXZXZQ;
             m_DataTableCunXZQ = m_XzqTreeView.m_DataTableCunXZQ;

            }
        }

        

       
        private void InitDropDownListDSXZQ()
        {
            DropDownListDSXZQ.DataSource=null ;
            //if (DropDownListDSXZQ.Items.Count < 1)
            //{           
            string m_strSQL = "";
            string m_strSQL_Filt = "";
            try
            {
                if (m_strUserName != "")
                {
                    //if (this.m_strUserName == "211421")
                    //{
                    //    //if (this.m_strUserName == "211421")
                    //    //{
                    //    if (m_DataAccess_SYS.ProviderIsOraDB())
                    //    {
                    //        m_strSQL = m_strDSXZQ_SQL + " WHERE substr(xzqdm,1,6)='" + this.m_strUserName + "'  order by xzqdm";
                    //    }
                    //    else
                    //    {
                    //        m_strSQL = m_strDSXZQ_SQL + " WHERE LEFT(xzqdm,6)='" + this.m_strUserName + "'  order by xzqdm";
                    //    }
                    //    //}
                    //    //else if (this.m_strUserName.Length >= 4)
                    //    //{
                    //    //    m_strSQL = m_strDSXZQ_SQL + " WHERE LEFT(xzqdm,4)='" + this.m_strUserName.Substring(0, 4) + "' and xzqdm<>'211421'";
                    //    //}
                    //}
                    //else 
                    if (this.m_strUserName.Length >= 4)
                    {
                        if (m_DataAccess_SYS_.ProviderIsOraDB())
                        {
                            m_strSQL_Filt= " WHERE substr(xzqdm,1,4)='" + this.m_strUserName.Substring(0, 4) + "'  order by xzqdm";
                            m_strSQL = m_strDSXZQ_SQL +m_strSQL_Filt;

                        }
                        else
                        {
                            m_strSQL_Filt=" WHERE LEFT(xzqdm,4)='" + this.m_strUserName.Substring(0, 4) + "'  order by xzqdm";
                            m_strSQL = m_strDSXZQ_SQL + m_strSQL_Filt;
                        }
                    }
                    else if (this.m_strUserName.Length == 2)
                    {
                        m_strSQL = m_strDSXZQ_SQL;
                    }
                }
                else
                {
                    m_strSQL = m_strDSXZQ_SQL;
                }

                DataTable m_DataTable=m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);

                //DataRow[] m_DataRowS = m_DataTableDSXZQ.Select(m_strSQL_Filt);       

                if (m_strUserName == "" || this.m_strUserName.Length == 2)
                {
                    //if (m_DataTable != null && m_DataTable.Rows.Count > 0)
                    //{
                    //    DataRow m_DataRow = m_DataTable.NewRow();
                    //    m_DataRow["xzqmc"] = "黑河市国土资源局";
                    //    m_DataRow["xzqdm"] = "2311";
                    //    m_DataTable.Rows.InsertAt(m_DataRow, 0);
                    //}
                }

                DropDownListDataBind(this.DropDownListDSXZQ, m_DataTable);

                if (DropDownListDSXZQ.Items.Count > 0)
                {
                    DropDownListDSXZQ.SelectedIndex = 0;
                }

            }
            catch (System.Exception errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void DropDownListDataBind(ComboBox p_ComboBox, DataTable p_DataTable)
        {
            p_ComboBox.DataSource = p_DataTable.DefaultView;
            p_ComboBox.DisplayMember = "XZQMC";
            p_ComboBox.ValueMember = "XZQDM";
            
        }

        private void InitDropDownListQXXZQ()
        {
            try
            {
                //DropDownListDataBind(this.DropDownListQXXZQ, m_DataAccess_SYS_.GetDataTableByQueryString(m_strQXXZQ_SQL));

                if (this.DropDownListDSXZQ.Items.Count > 0 && this.DropDownListDSXZQ.SelectedIndex >= 0)
                {
                    string m_strSQL = "";

                        if (m_strUserName.Length == 0)
                        {
                            if (m_DataAccess_SYS_.ProviderIsOraDB())
                            {
                                m_strSQL = m_strQXXZQ_SQL + " WHERE substr(XZQDM,1," + this.DropDownListDSXZQ.SelectedValue.ToString().Length + ")='" + this.DropDownListDSXZQ.SelectedValue.ToString() + "'   order by xzqdm";
                            }
                            else
                            {
                                m_strSQL = m_strQXXZQ_SQL + " WHERE LEFT(XZQDM," + this.DropDownListDSXZQ.SelectedValue.ToString().Length + ")='" + this.DropDownListDSXZQ.SelectedValue.ToString() + "'  order by xzqdm";
                            }
                        }
                        else 
                        {
                            if (m_DataAccess_SYS_.ProviderIsOraDB())
                            {
                                m_strSQL = m_strQXXZQ_SQL + " WHERE substr(XZQDM,1,4)='" + this.DropDownListDSXZQ.SelectedValue.ToString() + "'   order by xzqdm";
                            }
                            else
                            {
                                m_strSQL = m_strQXXZQ_SQL + " WHERE LEFT(XZQDM,4)='" + this.DropDownListDSXZQ.SelectedValue.ToString() + "'   order by xzqdm";
                            }
                        }
                        //else if (m_strUserName.Length == 4)
                        //{
                        //    if (m_DataAccess_SYS.ProviderIsOraDB())
                        //    {
                        //        m_strSQL = m_strQXXZQ_SQL + " WHERE substr(XZQDM,1,4)='" + m_strUserName + "'  order by xzqdm";
                        //    }
                        //    else
                        //    {
                        //        m_strSQL = m_strQXXZQ_SQL + " WHERE LEFT(XZQDM,4)='" + m_strUserName + "'  order by xzqdm";
                        //    }
                        //}
                        //else
                        //{

                        //    m_strSQL = "";
                        //}
                    
                  

                    DataTable m_DataTable = null;
                    

                    if (m_strSQL != "")
                    {
                        m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
                        
                    }
                    if (m_DataTable == null)
                    {
                        DropDownListQXXZQ.Items.Clear();
                        return;
                    }
                    if (DropDownListDSXZQ.SelectedValue.ToString() != "2300" && DropDownListDSXZQ.SelectedValue.ToString().Length == 4 && m_DataTable.Rows.Count  > 1)
                    {                      
                       
                            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
                            {
                                DataRow m_DataRow = m_DataTable.NewRow();
                                m_DataRow["xzqmc"] = "------------------";
                                m_DataRow["xzqdm"] ="";
                                m_DataTable.Rows.InsertAt(m_DataRow, 0);

                                m_DataRow = m_DataTable.NewRow();
                                m_DataRow["xzqmc"] = "全部";
                                m_DataRow["xzqdm"] = DropDownListDSXZQ.SelectedValue.ToString();
                                m_DataTable.Rows.InsertAt(m_DataRow, 0);
                            }
                        
                    }
                    DropDownListDataBind(this.DropDownListQXXZQ, m_DataTable);
                }
            }
            catch (System.Exception errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }
        private void InitDropDownListXZXZQ()
        {
            try
            {
                //DropDownListDataBind(this.DropDownListQXXZQ, m_DataAccess_SYS_.GetDataTableByQueryString(m_strQXXZQ_SQL));

                if (this.DropDownListQXXZQ.Items.Count > 0 && this.DropDownListQXXZQ.SelectedIndex >= 0)
                {
                    string m_strSQL = "";

                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strXZXZQ_SQL + " where substr(xzqdm,1,6)='" + this.DropDownListQXXZQ.SelectedValue.ToString() + "'   order by xzqdm";
                    }
                    else
                    {
                        m_strSQL = m_strXZXZQ_SQL + " where left(xzqdm,6)='" + this.DropDownListQXXZQ.SelectedValue.ToString() + "'   order by xzqdm";
                    }

                    DataTable m_DataTable = null;
                    if (m_strSQL != "")
                    {
                        m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);

                    }
                    if (m_DataTable == null)
                    {
                        DropDownListXZXZQ.Items.Clear();
                        return;
                    }
                    if (m_DataTable.Rows.Count > 1)
                    {
                       
                            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
                            {
                                DataRow m_DataRow = m_DataTable.NewRow();
                                m_DataRow["xzqmc"] = "------------------";
                                m_DataRow["xzqdm"] = "";
                                m_DataTable.Rows.InsertAt(m_DataRow, 0);

                                 m_DataRow = m_DataTable.NewRow();
                                m_DataRow["xzqmc"] = "全部";
                                m_DataRow["xzqdm"] = DropDownListQXXZQ.SelectedValue.ToString();
                                m_DataTable.Rows.InsertAt(m_DataRow, 0);
                            }
                        
                    }
                    DropDownListDataBind(this.DropDownListXZXZQ, m_DataTable);              
                   
                }

            }
            catch (System.Exception errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void InitDropDownListCun()
        {
            try
            {
                //DropDownListDataBind(this.DropDownListQXXZQ, m_DataAccess_SYS_.GetDataTableByQueryString(m_strQXXZQ_SQL));

                if (this.DropDownListXZXZQ.Items.Count > 0 && this.DropDownListXZXZQ.SelectedIndex >= 0)
                {
                    string m_strSQL = "";

                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strCJXZQ_SQL + " where substr(xzqdm,1,9)='" + this.DropDownListXZXZQ.SelectedValue.ToString() + "' order by xzqdm";
                    }
                    else
                    {
                        m_strSQL = m_strCJXZQ_SQL + " where left(xzqdm,9)='" + this.DropDownListXZXZQ.SelectedValue.ToString() + "' order by xzqdm";
                    }
                    DataTable m_DataTable = null;
                    if (m_strSQL != "")
                    {
                        m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);

                    }
                    if (m_DataTable == null)
                    {
                        DropDownListCun.Items.Clear();
                        return;
                    }
                    if (m_DataTable.Rows.Count > 1)
                    {

                        if (m_DataTable != null && m_DataTable.Rows.Count > 0)
                        {
                            DataRow m_DataRow = m_DataTable.NewRow();
                            m_DataRow["xzqmc"] = "------------------";
                            m_DataRow["xzqdm"] = "";
                            m_DataTable.Rows.InsertAt(m_DataRow, 0);

                            m_DataRow = m_DataTable.NewRow();
                            m_DataRow["xzqmc"] = "全部";
                            m_DataRow["xzqdm"] = DropDownListXZXZQ.SelectedValue.ToString();
                            m_DataTable.Rows.InsertAt(m_DataRow, 0);
                        }

                    }
                    DropDownListDataBind(this.DropDownListCun, m_DataTable);

                }

            }
            catch (System.Exception errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private string GetUserName()
        {
            //string m_strStaffID = "";
            m_strUserName = "";
            //if (m_DataAccess_SYS_ == null)
            //{
            //    m_DataAccess_SYS_ = new myDataAccess();
            //}
            //string m_strSql = "";

            //string m_strRole_Name = "";
            //m_strStaffID = Request.QueryString["Staff_ID"].ToString();
            //m_strSql = "select role_name from wfd_role where role_id in(select role_id from wfd_roledept where roledept_id in(select roledept_id from wfd_staff_roledept where staff_id ='" + m_strStaffID + "'))";

            //DataTable m_DataTable = m_DataAccess_SYS_.GetDataTableByQueryString(m_strSql);
            //if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            //{
            //    m_strRole_Name = m_DataTable.Rows[0][0].ToString();
            //}

            //if (m_strRole_Name == "系统管理员")
            //{
            //    m_blIsAdmin = true;
            //}
            //if (m_blIsAdmin == false)
            //{
            //    m_DataTable = null;
            //    m_strSql = "select STAFF_USERNAME from WFD_STAFF where STAFF_ID='" + m_strStaffID + "'";

            //    m_DataTable = m_DataAccess_SYS_.GetDataTableByQueryString(m_strSql);
            //    if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            //    {
            //        m_strUserName = m_DataTable.Rows[0][0].ToString();
            //    }

            //}
            //else
            //{
            //    m_strUserName = "";
            //}
            //this.m_strUserName = m_strUserName;
            return m_strUserName;
        }

        private void DropDownListDSXZQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListDSXZQ.Text == "------------------")
            {
                DropDownListDSXZQ.SelectedIndex = 0;
            }

            this.DropDownListXZXZQ.DataSource=null ;
            this.DropDownListQXXZQ.DataSource=null ;
            //if (DropDownListDSXZQ.SelectedIndex == 0)//选中的是全部
            //{
            //    //InitDropDownListZFDW();
            //    return;
            //}
            try
            {
                InitDropDownListQXXZQ();
            }
            catch (System.Exception errs)
            {
                throw (errs);
            }
            finally
            {
                //this.LNKC_XZQH.SelectCommand = m_strTemp;
            }
        }

        private void DropDownListQXXZQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListQXXZQ.Text == "------------------")
            {
                DropDownListQXXZQ.SelectedIndex = 0;
            }

            this.DropDownListXZXZQ.DataSource=null ;
            if (DropDownListQXXZQ.SelectedIndex == 0)//选中的是全部
            {
                //InitDropDownListZFDW();
                return;
            }
            try
            {
                InitDropDownListXZXZQ();
            }
            catch (System.Exception errs)
            {
                throw (errs);
            }
            finally
            {
                //this.LNKC_XZQH.SelectCommand = m_strTemp;
            }
        }

        private void DropDownListXZXZQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListXZXZQ.Text == "------------------")
            {
                DropDownListXZXZQ.SelectedIndex = 0;
            }

            this.DropDownListCun.DataSource = null;
            if (DropDownListXZXZQ.SelectedIndex == 0)//选中的是全部
            {
                //InitDropDownListZFDW();
                return;
            }
            try
            {
                InitDropDownListCun();
            }
            catch (System.Exception errs)
            {
                throw (errs);
            }
            finally
            {
                //this.LNKC_XZQH.SelectCommand = m_strTemp;
            }
        }

        private void cmbDW_ZFDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDW_ZFDW.SelectedItem == null) return;
            if (cmbDW_ZFDW.SelectedItem.ToString() == "全部")
            {
                this.btnEDIT_ZFDW.Enabled = false;
            }
            else
            {
                this.btnEDIT_ZFDW.Enabled = true ;
            }
        }

        private void cmbDW_ZFDW_DrawItem(object sender, DrawItemEventArgs e)
        {
            clsFunction.Function.ComboBox_AdjustDrawMode_DrawItem((ComboBox)sender,e );
        }

        private void cmbDW_ZFRY_DrawItem(object sender, DrawItemEventArgs e)
        {
            clsFunction.Function.ComboBox_AdjustDrawMode_DrawItem((ComboBox)sender, e);
        }
      
    }
}
