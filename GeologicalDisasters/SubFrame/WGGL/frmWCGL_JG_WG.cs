using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame.WGGL
{
    public partial class frmWCGL_JG_WG : Form
    {
        public   string m_strSQL;
        DataSet m_DataSet;
        System.Data.OleDb.OleDbDataAdapter m_OleDbDataAdapter ;
        //System.Data.OleDb.OleDbDataAdapter m_MySqlDbDataAdapter;
        private string m_strZFWGTableName = "zfwg";
        public string m_strZFDWDM;
        public string m_strZFDWMC;
        public string m_strXZQDM;
        public string m_strUserName;

        private string m_strZFDW_SQL = "select * from zfdw";
        private string m_strZFRY_SQL = "select * from zfry";

        //private WGGL.clsZFDW m_clsZFDW_;
        private WGGL.clsZFRY m_clsZFRY_;
        //public WGGL.clsZFDW m_clsZFDW
        //{
        //    set
        //    {
        //        m_clsZFDW_ = value;
        //    }
        //}

        //private string[] m_strSZGX_XZQDM;//省直管县行政区代码

        private clsDataAccess.DataAccess m_DataAccess_SYS_;
        public clsDataAccess.DataAccess m_DataAccess_SYS
        {
            set
            {
                m_DataAccess_SYS_ = value;
                //GetSZGX_XZQDM();
            }
        }

        bool m_blHasModified;
        public frmWCGL_JG_WG()
        {
            InitializeComponent();
            m_blHasModified = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string m_strSQL = "";
            string m_strBJZFRYBH = "";
            string m_strSJZFRYBH = "";
            string m_strXZQDM = "";
            string m_strXZQMC = "";

            string m_strID = "";
            int m_intSelectIndex =0;
            //int m_intPageIndex = dataGridViewX1.CurrentPageIndex;
            //dataGridViewX1.SelectedIndex = -1;
            int[] m_intSeleced = new int[dataGridViewX1.SelectedRows.Count ];
            try
            {
                //首先删除该行政区的以前的执法负责人
                //然后再添加现有执法负责人

                if (DropDownListBJZFRY.SelectedIndex < 0 && DropDownListBJZFRY.Enabled == true)
                {
                    m_DataAccess_SYS_.MessageInforShow(this, "请先选择本级执法人员！");
                    DropDownListBJZFRY.Focus();
                    return;
                }

                if (DropDownListSJZFRY.SelectedIndex < 0 && DropDownListSJZFRY.Enabled == true)
                {                  
                    //m_DataAccess_SYS_.MessageInforShow(this, "请先选择上级执法人员！");
                    //DropDownListSJZFRY.Focus();
                    //return;
                }

                int m_intIndex = 0;

                for (int i = 0; i < dataGridViewX1.SelectedRows.Count;i++ )
                {
                    m_intSeleced[i] = dataGridViewX1.SelectedRows[i].Index;
                    m_intIndex = m_intIndex + 1;
                   
                        m_strSQL = "";
                        m_strBJZFRYBH = "";
                        m_strSJZFRYBH = "";
                        m_strXZQDM = dataGridViewX1.SelectedRows[i].Cells["行政区代码"].Value.ToString() ;
                        m_strXZQMC = dataGridViewX1.SelectedRows[i].Cells["行政区名称"].Value.ToString();
                        m_strID = dataGridViewX1.SelectedRows[i].Cells["网格标识"].Value.ToString();

                        if (DropDownListBJZFRY.SelectedIndex >= 0)
                        {
                            m_strBJZFRYBH = this.DropDownListBJZFRY.SelectedValue.ToString();
                        }
                        if (DropDownListSJZFRY.SelectedIndex >= 0)
                        {
                            m_strSJZFRYBH = this.DropDownListSJZFRY.SelectedValue.ToString();
                        }

                       
                        if (m_strID != "")
                        {
                            m_strSQL = "UPDATE  " + m_strZFWGTableName + " SET jssj='" + DateTime.Now.ToShortDateString() + "' WHERE ID=" + m_strID;
                            //if (DropDownListSJZFRY.Enabled == true && DropDownListBJZFRY.Enabled == true)
                            //{
                            //    m_strSQL = "UPDATE  " + m_strZFWGTableName + " SET Sjzfrybh='" + m_strSJZFRYBH + "'";
                            //    m_strSQL = m_strSQL + ", bjzfryBZ='" + txtBJZFRYBZ.Text + "',SJZFRYBZ='" + txtSJZFRYBZ.Text + "' ";
                            //    m_strSQL = m_strSQL + ", bjzfrybh='" + m_strBJZFRYBH + "' WHERE XZQDM='" + m_strXZQDM + "' and ID=" + m_strID;
                            //}
                            //else if (DropDownListBJZFRY.Enabled == true && DropDownListSJZFRY.Enabled == false)
                            //{
                            //    m_strSQL = "UPDATE  " + m_strZFWGTableName + " SET bjzfrybh='" + m_strBJZFRYBH + "' ";
                            //    m_strSQL = m_strSQL + ", bjzfryBZ='" + txtBJZFRYBZ.Text + "' ";
                            //    m_strSQL = m_strSQL + " WHERE XZQDM='" + m_strXZQDM + "' and ID=" + m_strID;
                            //}
                            //else if (DropDownListSJZFRY.Enabled == true && DropDownListBJZFRY.Enabled == false)
                            //{
                            //    m_strSQL = "UPDATE  " + m_strZFWGTableName + " SET Sjzfrybh='" + m_strSJZFRYBH + "' ";
                            //    m_strSQL = m_strSQL + ",SJZFRYBZ='" + txtSJZFRYBZ.Text + "' ";
                            //    m_strSQL = m_strSQL + " WHERE XZQDM='" + m_strXZQDM + "' and ID=" + m_strID;
                            //}

                            m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                           
                        }
                        //else
                        //{
                            string m_strSQLAdd = "";
                            if (DropDownListSJZFRY.Enabled == true && DropDownListBJZFRY.Enabled == true)
                            {
                                m_strSQLAdd = "insert into zfwg (bjzfrybh,xzqdm,xzqmc,sjzfrybh,kssj,BJZFRYBZ,SJZFRYBZ) ";
                                m_strSQLAdd = m_strSQLAdd + " values('" + m_strBJZFRYBH + "','" + m_strXZQDM + "','" + m_strXZQMC + "','" + m_strSJZFRYBH + "','" + DateTime.Now.Date + "','" + txtBJZFRYBZ.Text + "',' " + txtSJZFRYBZ.Text + "')";
                                m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQLAdd);
                               
                               
                            }
                            else if (DropDownListBJZFRY.Enabled == true)
                            {
                                m_strSQLAdd = "insert into zfwg (bjzfrybh,xzqdm,xzqmc,kssj,BJZFRYBZ) values('" + m_strBJZFRYBH + "','" + m_strXZQDM + "','" + m_strXZQMC + "','" + DateTime.Now.Date + "','" + txtBJZFRYBZ.Text + "')";
                                m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQLAdd);
                               
                            }
                            else if (DropDownListSJZFRY.Enabled == true)
                            {
                                m_strSQLAdd = "insert into zfwg (xzqdm,xzqmc,sjzfrybh,kssj,SJZFRYBZ) values('" + m_strXZQDM + "','" + m_strXZQMC + "','" + m_strSJZFRYBH + "','" + DateTime.Now.Date + "',' " + txtSJZFRYBZ.Text + "')";
                                m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQLAdd);
                                
                            }

                        //}
                    }
                
                InitDataGridview();
                ResetSelectedRows(m_intSeleced);
                m_DataAccess_SYS_.MessageInforShow(this, "保存成功！");                
            }
            catch (SystemException errs)
            {
                //labInfo.Text = "保存失败：" + errs.Message;
                m_DataAccess_SYS_.MessageInforShow(this, "保存失败！" + errs.Message );
            }
        }

        /// <summary>
        /// 重新设定用户选择的行
        /// </summary>
        /// <param name="p_intSeleced">已选择行的index</param>
        private void ResetSelectedRows(int[] p_intSeleced)
        {
            dataGridViewX1.ClearSelection();
            for (int i = 0; i < p_intSeleced.Length; i++)
            {
                dataGridViewX1.Rows[p_intSeleced[i]].Selected = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridViewX1.SelectedRows.Count; i++)
            {
                dataGridViewX1.Rows.Remove(dataGridViewX1.SelectedRows[i]);
                //m_blHasModified = true;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                //if (m_DataSet.HasChanges())
                //{
                //    if (m_DataAccess_SYS_.MessageQuestionShow(this, "您修改了数据，是否需要保存？") == DialogResult.Yes)
                //    {
                //        btnSave_Click(sender, e);
                //    }                                     
                //}
                this.Close();
            }
            catch(SystemException errs)
            {
            }
            
        }

       

        private string CreatSQL()
        {

            string m_strSQL = "";
            //try
            //{
            //    if (m_strXZQDM == "") return "";

            //    if (m_strZFDWDM == "0" || m_strZFDWDM=="")
            //    {
            //        if (m_strXZQDM.Length <= 6)
            //        {
            //            m_strSQL = "SELECT id as 标识 ,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间  FROM ZFDW WHERE LEFT(ZFDWDM," + m_strXZQDM.Length + ")='" + m_strXZQDM + "'";
            //        }
            //        else
            //        {
            //            m_strSQL = "SELECT  id as 标识 ,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间 FROM ZFDW WHERE LEFT(ZFDWDM,6)='" + m_strXZQDM + "'";
            //        }
            //    }
            //    else
            //    {
            //        m_strSQL = "SELECT  id as 标识 ,zfdwmc as 执法单位名称 ,zfdwdm as 执法单位代码,djsj as 登记时间 FROM ZFDW WHERE ZFDWDM='" + m_strZFDWDM + "'";
            //    }              


            //}
            //catch (SystemException errs)
            //{
            //    m_DataAccess_SYS_.MessageErrorInforShow(this, errs.Message);
            //}

            return m_strSQL;
        }

        public  void InitDataGridview()
        {
            ////m_strSQL = CreatSQL();
            if (m_strSQL != "" && m_DataAccess_SYS_ != null)
            {
                DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);

                if (m_DataTable == null) return;
                m_DataSet = m_DataTable.DataSet;
                //if (m_DataTable == null || m_DataTable.Rows.Count < 1)
                //{
                //}
                
            //    //m_DataSet = m_DataTable.DataSet;
            //    m_DataSet = null;
            //    m_OleDbDataAdapter = null;

            //    m_DataSet = new DataSet();
            //    m_OleDbDataAdapter = new System.Data.OleDb.OleDbDataAdapter(m_strSQL, m_DataAccess_SYS_.con);
            //    m_OleDbDataAdapter.Fill(m_DataSet, "zfdw");
            //    dataGridViewX1.DataSource = m_DataSet.Tables[0];

            //    //为m_OleDbDataAdapter自动生成insert、Update、Delete命令

            //    System.Data.OleDb.OleDbCommandBuilder  m_MySqlCommandBuilder = new System.Data.OleDb.OleDbCommandBuilder (m_OleDbDataAdapter);
            //    //dataGridViewX1.Columns["标识"].ReadOnly = true;
            //    //dataGridViewX1.Columns["执法人员编号"].ReadOnly = true;
            //    //dataGridViewX1.Columns["执法单位名称"].ReadOnly = true;
            //    //dataGridViewX1.Columns["执法单位代码"].ReadOnly = true;
            //    m_clsZFRY_ = new clsZFRY();
            //    m_clsZFRY_.m_DataAccess_SYS_ = m_DataAccess_SYS_;
            //    m_clsZFRY_.m_strXZQDM = m_strXZQDM;
            //    m_clsZFRY_.m_strZFDW_ZFDWDM = m_strZFDWDM;

                //DataColumn m_DataColumn = new DataColumn("选中");
                //m_DataColumn.ReadOnly = false;
                //m_DataColumn.DataType = Type.GetType("System.Boolean");
                //m_DataColumn.DefaultValue = "1";
                //m_DataTable.Columns.Add(m_DataColumn);
                if (m_DataTable != null)
                {
                    dataGridViewX1.DataSource = m_DataTable.DefaultView;
                    //dataGridViewX1.Columns["选中"].ReadOnly = false;                    
                }
                else
                {
                    dataGridViewX1.DataSource = null;
                }
                RefreshZFDW();
            }


        }

        private void RefreshZFDW()
        {
            try
            {
                //this.labInfo.Visible = false;
                SetButton(false);
                ClearListbox();
                if (this.dataGridViewX1.Rows.Count  > 0)
                {
                    //if (this.dataGridViewX1.Rows.Count == 1)
                    //{
                    //    CheckBox m_CheckBox = (CheckBox)dataGridViewX1.Rows[0].FindControl("chk1");
                    //    m_CheckBox.Checked = true;
                    //}

                    string m_strXZQDM = "";//
                    string m_strXZQMC = "";
                    string m_strBJZFRYXM = "";//        
                    string m_strBJZFDWDM = "";
                    string m_strBJZFRYBH = "";
                    string m_strBJZFRYDH = "";
                    string m_strSJZFRYXM = "";//        
                    string m_strSJZFDWDM = "";
                    string m_strSJZFRYBH = "";
                    string m_strSJZFRYDH = "";
                    string m_strBJZFRYBZ = "";
                    string m_strSJZFRYBZ= "";

                    try
                    {
                        if (this.dataGridViewX1.RowCount < 1) return;
                        if (this.dataGridViewX1.SelectedRows.Count < 1) this.dataGridViewX1.Rows[0].Selected = true;
                        m_strBJZFRYBH = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员编号"].Value.ToString().Trim();
                        m_strBJZFRYXM = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员姓名"].Value.ToString().Trim();
                        m_strBJZFRYDH = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员办公电话"].Value.ToString().Trim();
                        m_strBJZFDWDM = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员所在单位代码"].Value.ToString().Trim();
                        m_strBJZFRYBZ = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员说明"].Value.ToString().Trim();
                        txtBJZFRYBZ.Text = m_strBJZFRYBZ;
                        txtBJZFFZR.Text = m_strBJZFRYXM;

                        m_strSJZFRYBH = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员编号"].Value.ToString().Trim();
                        m_strSJZFRYXM = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员姓名"].Value.ToString().Trim();
                        m_strSJZFRYDH = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员办公电话"].Value.ToString().Trim();
                        m_strSJZFDWDM = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员所在单位代码"].Value.ToString().Trim();
                        m_strSJZFRYBZ = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员说明"].Value.ToString().Trim();
                        txtSJZFRYBZ.Text = m_strSJZFRYBZ;
                        txtSJZFFZR.Text = m_strSJZFRYXM;

                        m_strXZQDM = this.dataGridViewX1.SelectedRows[0].Cells["行政区代码"].Value.ToString().Trim();
                        m_strXZQMC = this.dataGridViewX1.SelectedRows[0].Cells["行政区名称"].Value.ToString().Trim();
                        txtXZQMC.Text = m_strXZQMC;

                        //if (m_strBJZFRYXM == "&nbsp;") m_strBJZFRYXM = "";
                        //if (m_strBJZFDWDM == "&nbsp;") m_strBJZFDWDM = "";
                        //if (m_strBJZFRYDH == "&nbsp;") m_strBJZFRYDH = "";
                        //if (m_strBJZFRYBH == "&nbsp;") m_strBJZFRYBH = "";

                        //if (m_strSJZFRYXM == "&nbsp;") m_strSJZFRYXM = "";
                        //if (m_strSJZFDWDM == "&nbsp;") m_strSJZFDWDM = "";
                        //if (m_strSJZFRYDH == "&nbsp;") m_strSJZFRYDH = "";
                        //if (m_strSJZFRYBH == "&nbsp;") m_strSJZFRYBH = "";

                        if (m_strBJZFRYBH != "" || m_strSJZFRYBH  != "" )
                        {
                            btnRemove.Enabled = true;
                        }

                        //m_strZFRYXM.Replace("&nbsp;", "");

                        // m_strZFDWDM = this.dataGridViewX1.Items[0].Cells[10].Text.Trim();
                        // m_strXZQDM = this.dataGridViewX1.Items[0].Cells[11].Text.Trim();           
                        //m_strXZQMC = this.dataGridViewX1.Items[0].Cells[12].Text.Trim();



                        //for (int i = 0; i < DropDownListDSXZQ.Items.Count; i++)
                        //{
                        //    if (DropDownListDSXZQ.Items[i].Value == m_strXZQDM)
                        //    {
                        //        DropDownListDSXZQ.SelectedIndex = i;
                        //        InitDropDownListQXXZQ();
                        //        break;
                        //    }
                        //}

                        //for (int i = 0; i < DropDownListQXXZQ.Items.Count; i++)
                        //{
                        //    if (DropDownListQXXZQ.Items[i].Value == m_strXZQDM)
                        //    {
                        //        DropDownListQXXZQ.SelectedIndex = i;
                        //        InitDropDownListXZXZQ();
                        //        break;
                        //    }
                        //}

                        //for (int i = 0; i < DropDownListXZXZQ.Items.Count; i++)
                        //{
                        //    if (DropDownListXZXZQ.Items[i].Value == m_strXZQDM)
                        //    {
                        //        DropDownListXZXZQ.SelectedIndex = i;
                        //        break;
                        //    }
                        //}


                        string m_strSJXZQDM = ""; //上级行政区代码
                        string m_strSJXZQ_BJZFDWDM = "";
                        string m_strSJXZQ_BJZFRYBH = "";
                        string m_strSQL_SJXZQ_BJZFBH = "";

                        if (m_strXZQDM.Length == 12)
                        {
                            m_strSJXZQDM = m_strXZQDM.Substring(0, 9);
                        }

                        InitDropDownListBJZFDW(m_strXZQDM);
                        if (m_strBJZFDWDM != "")
                        {
                            for (int i = 0; i < DropDownListBJZFDW.Items.Count; i++)
                            {
                                ComboBox.ObjectCollection m_ObjectCollection;
                               
                                if (DropDownListBJZFDW.Items[i].ToString() == m_strBJZFDWDM)//you wen ti 
                                {
                                    DropDownListBJZFDW.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (m_strSJXZQDM != "")
                            {
                                //获得上级行政区单位的本级执法单位代码

                                m_strSQL_SJXZQ_BJZFBH = "SELECT BJZFRYBH FROM ZFWG WHERE XZQDM='" + m_strSJXZQDM + "' and jssj is null AND BJZFRYBH IS NOT NULL AND BJZFRYBH<>''";

                                DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL_SJXZQ_BJZFBH);
                                if (m_DataTable!=null && m_DataTable.Rows.Count > 0)
                                {
                                    try
                                    {
                                        m_strSJXZQ_BJZFRYBH = m_DataTable.Rows[0][0].ToString();
                                        m_strSJXZQ_BJZFDWDM = m_strSJXZQ_BJZFRYBH.Substring(0, 8);

                                        for (int i = 0; i < DropDownListBJZFDW.Items.Count; i++)
                                        {
                                            if (DropDownListBJZFDW.Items[i].ToString() == m_strSJXZQ_BJZFDWDM)//you wen ti
                                            {
                                                DropDownListBJZFDW.SelectedIndex = i;
                                                break;
                                            }
                                        }
                                    }
                                    catch (SystemException errs)
                                    { }

                                }
                            }


                        }
                        if (DropDownListBJZFDW.SelectedIndex >= 0)
                        {
                            InitDropDownListBJZFRY(DropDownListBJZFDW.SelectedValue.ToString());

                            if (m_strBJZFRYBH != "")
                            {
                                for (int i = 0; i < DropDownListBJZFRY.Items.Count; i++)
                                {
                                    if (DropDownListBJZFRY.Items[i].ToString() == m_strBJZFRYBH)//you wen ti
                                    {
                                        DropDownListBJZFRY.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (m_strSJXZQ_BJZFRYBH != "")
                                {
                                    for (int i = 0; i < DropDownListBJZFRY.Items.Count; i++)
                                    {
                                        if (DropDownListBJZFRY.Items[i].ToString() == m_strSJXZQ_BJZFRYBH)//you wen ti 
                                        {
                                            DropDownListBJZFRY.SelectedIndex = i;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        InitDropDownListSJZFDW(m_strXZQDM);

                        if (m_strSJZFDWDM != "")
                        {
                            for (int i = 0; i < DropDownListSJZFDW.Items.Count; i++)
                            {
                                if (DropDownListSJZFDW.Items[i].ToString() == m_strSJZFDWDM)//you wen ti
                                {
                                    DropDownListSJZFDW.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //if (m_strSJXZQDM != "")
                            //{
                            ////获得上级行政区单位的本级执法单位代码
                            //string m_strSJXZQ_BJZFDWDM = "";
                            //string m_strSQL_SJXZQ_BJZFBH = "";
                            //m_strSQL_SJXZQ_BJZFBH = "SELECT BJZFRYBH FROM ZFWG WHERE XZQDM='" + m_strSJXZQDM + "' and jssj is null AND BJZFRYBH IS NOT NULL AND BJZFRYBH<>''";


                            //DataTable m_DataTable = m_DataAccess_SYS_.GetDataTableByQueryString(m_strSQL_SJXZQ_BJZFBH);
                            if (m_strSJXZQ_BJZFDWDM != "")
                            {
                                try
                                {
                                    //m_strSJXZQ_BJZFDWDM = m_DataTable.Rows[0][0].ToString().Substring(0, 8);

                                    for (int i = 0; i < DropDownListSJZFDW.Items.Count; i++)
                                    {
                                        if (DropDownListSJZFDW.Items[i].ToString() == m_strSJXZQ_BJZFDWDM)//you wen ti
                                        {
                                            DropDownListSJZFDW.SelectedIndex = i;
                                            break;
                                        }
                                    }
                                }
                                catch (SystemException errs)
                                { }

                            }
                            //}


                        }
                        if (DropDownListSJZFDW.SelectedIndex >= 0)
                        {
                            InitDropDownListSJZFRY(DropDownListSJZFDW.SelectedValue.ToString());

                            if (m_strSJZFRYBH != "")
                            {
                                for (int i = 0; i < DropDownListSJZFRY.Items.Count; i++)
                                {
                                    if (DropDownListSJZFRY.Items[i].ToString() == m_strSJZFRYBH)//you wen ti
                                    {
                                        DropDownListSJZFRY.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (m_strSJXZQ_BJZFRYBH != "")
                                {
                                    for (int i = 0; i < DropDownListSJZFRY.Items.Count; i++)
                                    {
                                        if (DropDownListSJZFRY.Items[i].ToString() == m_strSJXZQ_BJZFRYBH)//you wen ti
                                        {
                                            DropDownListSJZFRY.SelectedIndex = i;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        this.txtXZQMC.Text = m_strXZQMC.Trim();
                        this.txtBJZFFZR.Text = m_strBJZFRYXM.Trim();
                        this.txtSJZFFZR.Text = m_strSJZFRYXM.Trim();

                        //SetButton(true);
                        if (m_strXZQDM.Length > m_strUserName.Length || m_strUserName.Length>=6)
                        {
                            if (m_strUserName.Length >= 6)
                            {
                                //DropDownListBJZFDW.Enabled = true;
                                //DropDownListBJZFRY.Enabled = true;
                            }
                            else
                            {
                                //DropDownListBJZFDW.Enabled = false;
                                //DropDownListBJZFRY.Enabled = false;
                            }
                            DropDownListSJZFDW.Enabled = true;
                            DropDownListSJZFRY.Enabled = true;
                        }
                        else if (m_strXZQDM.Length <= m_strUserName.Length)
                        {
                            //DropDownListBJZFDW.Enabled = true;
                            //DropDownListBJZFRY.Enabled = true;
                            //DropDownListSJZFDW.Enabled = false;
                            //DropDownListSJZFRY.Enabled = false;
                        }

                        //SetButton(true);
                        if (DropDownListBJZFRY.SelectedIndex >= 0)
                        {
                            btnSave.Enabled = true;
                        }
                    }
                    catch (System.Exception errs)
                    {
                        clsFunction.Function.MessageBoxError(errs.Message);
                    }
                }
            }
            catch (System.Exception errs)
            {
                throw errs;
            }
        }

        //private void GetSZGX_XZQDM()
        //{
        //    if (m_DataAccess_SYS_ != null)
        //    {
        //        string m_strSQL = "SELECT xzqdm FROM DSXZQ_TDLY WHERE (LEN(XZQDM)=6)";
        //        DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
        //        if (m_DataTable != null && m_DataTable.Rows.Count > 0)
        //        {
        //            m_strSZGX_XZQDM =null;
        //            m_strSZGX_XZQDM = new string[m_DataTable.Rows.Count];
        //            for (int i = 0; i < m_DataTable.Rows.Count; i++)
        //            {
        //                m_strSZGX_XZQDM[i] = m_DataTable.Rows[i][0].ToString();
        //            }
        //        }
        //    }
        //}

        //private bool IsSZGX(string p_strXZQDM)
        //{
        //    bool m_blIsSZGX = false;
        //    if (m_strSZGX_XZQDM == null) return m_blIsSZGX;
        //    for (int i = 0; i < m_strSZGX_XZQDM.Length; i++)
        //    {
        //        if (p_strXZQDM == m_strSZGX_XZQDM[i])
        //        {
        //            m_blIsSZGX = true ;
        //            break;
        //        }
        //    }
        //    return m_blIsSZGX;
        //}

        private void InitDropDownListBJZFDW(string p_strXZQDM)
        {
            string m_strSQL = "";

            if (m_strUserName.Length == 0)
            {
                if (p_strXZQDM.Length >= 6)
                {

                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strZFDW_SQL + "  WHERE  substr(ZFDWDM,1,4)='" + p_strXZQDM.Substring(0, 4) + "'  order by zfdwdm";
                    }
                    else
                    {
                        m_strSQL = m_strZFDW_SQL + "  WHERE  LEFT(ZFDWDM,4)='" + p_strXZQDM.Substring(0, 4) + "'  order by zfdwdm";
                    }

                }
                else
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strZFDW_SQL + " WHERE substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 4) + "00' ";
                        m_strSQL = m_strSQL + "  or  substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 2) + "0000'  order by zfdwdm";
                    }
                    else
                    {
                        m_strSQL = m_strZFDW_SQL + " WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 4) + "00' ";
                        m_strSQL = m_strSQL + "  or  LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 2) + "0000'  order by zfdwdm";
                    }
                }
            }
            else
            {
                if (p_strXZQDM.Length >= 6)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strZFDW_SQL + " WHERE substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 6) + "'";
                    }
                    else
                    {
                        m_strSQL = m_strZFDW_SQL + " WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 6) + "'";
                    }
                }
                else if (p_strXZQDM.Length == 4)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strZFDW_SQL + " WHERE substr(ZFDWDM,1,6)='" + p_strXZQDM + "00'";
                    }
                    else
                    {
                        //m_strSQL = m_strZFDW_SQL + " WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM + "00'";//20140527 
                        m_strSQL = m_strZFDW_SQL + " WHERE LEFT(ZFDWDM," + p_strXZQDM.Length + ")='" + p_strXZQDM + "' and len(zfdwdm)="+(p_strXZQDM.Length+2);
                    }
                }
                else if (p_strXZQDM.Length == 2)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strZFDW_SQL + " WHERE substr(ZFDWDM,1,6)='" + p_strXZQDM + "0000" + "'";
                    }
                    else
                    {
                        //m_strSQL = m_strZFDW_SQL + " WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM + "0000" + "'";//20140527 
                        m_strSQL = m_strZFDW_SQL + " WHERE LEFT(ZFDWDM," + p_strXZQDM.Length + ")='" + p_strXZQDM + "' and len(zfdwdm)=" + (p_strXZQDM.Length + 2);

                    }
                }

            }
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                DropDownListBJZFDW.DataSource = m_DataTable.DefaultView;
                DropDownListBJZFDW.DisplayMember = "zfdwmc";
                DropDownListBJZFDW.ValueMember = "zfdwdm";
                //DropDownListBJZFDW.DataBind();
            }
            else
            {
                DropDownListBJZFDW.DataSource = null;
            }
        }

        private void InitDropDownListSJZFDW(string p_strXZQDM)
        {
            string m_strSQL = "";

            if (p_strXZQDM == null || p_strXZQDM.Trim() == "")
            {
                return;
            }
            p_strXZQDM = p_strXZQDM.Trim();

            //if (m_strUserName.Length == 0)
            //{
                if (p_strXZQDM.Length > 6)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strZFDW_SQL + "  WHERE  substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 6) + "'  order by zfdwdm";
                    }
                    else
                    {
                        m_strSQL = m_strZFDW_SQL + "  WHERE  LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 6) + "'  order by zfdwdm";
                    }

                }
                else if (p_strXZQDM.Length == 6)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        if (m_DataAccess_SYS_.IsSZGX(p_strXZQDM))//是否是省直管县
                        {
                            m_strSQL = m_strZFDW_SQL + "  WHERE  substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 2) + "0000'  order by zfdwdm";
                        }
                        else
                        {
                            m_strSQL = m_strZFDW_SQL + "  WHERE  substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 4) + "00'  order by zfdwdm";
                        }
                    }
                    else
                    {
                        if (m_DataAccess_SYS_.IsSZGX(p_strXZQDM))//是否是省直管县
                        {
                            m_strSQL = m_strZFDW_SQL + "  WHERE  LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 2) + "0000'  order by zfdwdm";
                        }
                        else
                        {
                            m_strSQL = m_strZFDW_SQL + "  WHERE  LEFT(ZFDWDM,4)='" + p_strXZQDM.Substring(0, 4) + "' and len(zfdwdm)=6   order by zfdwdm";
                        }
                    }

                }
                else if (p_strXZQDM.Length == 4)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        //m_strSQL = m_strZFDW_SQL + " WHERE substr(ZFDWDM,1,6)='" + p_strXZQDM + "00' ";
                        //m_strSQL = m_strSQL + "  or  substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 2) + "0000'  order by zfdwdm";

                        m_strSQL = m_strZFDW_SQL + " WHERE  substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 2) + "0000'  order by zfdwdm";
                    }
                    else
                    {
                        //m_strSQL = m_strZFDW_SQL + " WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 4) + "00' ";
                        //m_strSQL = m_strSQL + "  or  LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 2) + "0000'  order by zfdwdm";

                        m_strSQL = m_strZFDW_SQL + " WHERE  LEFT(ZFDWDM,2)='" + p_strXZQDM.Substring(0, 2) + "' and len(zfdwdm)=4  order by zfdwdm";
                    }
                }
                else
                {
                    m_strSQL = "";
                }
            //}
            //else
            //{
            //    if (p_strXZQDM.Length >= 9)
            //    {
                //        if (m_DataAccess_SYS_.IsSZGX(p_strXZQDM.Substring(0, 6) ))//if (p_strXZQDM.Substring(0, 6) == "211421")
            //        {
            //            //是直管县

            //            if (m_DataAccess_SYS.ProviderIsOraDB())
            //            {
            //                m_strSQL = m_strZFDW_SQL + " WHERE (substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 6) + "' and substr(ZFDWDM,1,6)<>'211421') or substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 2) + "0000" + "' or substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 4) + "00" + "'";
            //            }
            //            else
            //            {
            //                m_strSQL = m_strZFDW_SQL + " WHERE (LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 6) + "' and LEFT(ZFDWDM,6)<>'211421') or LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 2) + "0000" + "' or LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 4) + "00" + "'";
            //            }
            //        }
            //        else
            //        {
            //            if (m_DataAccess_SYS.ProviderIsOraDB())
            //            {
            //                m_strSQL = m_strZFDW_SQL + " WHERE (substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 6) + "') or substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 2) + "0000" + "' or substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 4) + "00" + "'";
            //            }
            //            else
            //            {
            //                m_strSQL = m_strZFDW_SQL + " WHERE (LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 6) + "') or LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 2) + "0000" + "' or LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 4) + "00" + "'";
            //            }
            //        }
            //    }
                //    else if (p_strXZQDM.Length == 6 && m_DataAccess_SYS_.IsSZGX(p_strXZQDM) ==false)//else if (p_strXZQDM.Length == 6 && p_strXZQDM != "211421")
            //    {
            //        if (m_DataAccess_SYS.ProviderIsOraDB())
            //        {
            //            m_strSQL = m_strZFDW_SQL + " WHERE (substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 6) + "' and substr(ZFDWDM,1,6)<>'211421') or substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 2) + "0000" + "' or substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 4) + "00" + "'";
            //        }
            //        else
            //        {
            //            m_strSQL = m_strZFDW_SQL + " WHERE (LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 6) + "' and LEFT(ZFDWDM,6)<>'211421') or LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 2) + "0000" + "' or LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 4) + "00" + "'";
            //        }
            //    }
            //    else if (p_strXZQDM.Length == 4 || p_strXZQDM == "211421")
            //    {
            //        if (m_DataAccess_SYS.ProviderIsOraDB())
            //        {
            //            m_strSQL = m_strZFDW_SQL + " WHERE substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 2) + "0000" + "'";
            //        }
            //        else
            //        {
            //            m_strSQL = m_strZFDW_SQL + " WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 2) + "0000" + "'";
            //        }
            //    }

            //    m_strSQL = m_strSQL + " order by zfdwdm";
            //}

            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                DropDownListSJZFDW.DataSource = m_DataTable.DefaultView;
                DropDownListSJZFDW.ValueMember= "zfdwdm";
                DropDownListSJZFDW.DisplayMember = "zfdwmc";
                //DropDownListSJZFDW.DataBind();

                for (int i = 0; i < DropDownListSJZFDW.Items.Count; i++)
                {
                    if (p_strXZQDM.Length >= 6)
                    {
                        if (DropDownListSJZFDW.Items[i].ToString().Substring(0, 6) == p_strXZQDM.Substring(0, 6))//you wen ti
                        {
                            DropDownListSJZFDW.SelectedIndex = i;
                            break;
                        }
                    }
                    else
                    {
                        if (DropDownListSJZFDW.Items[i].ToString().Substring(0, p_strXZQDM.Length) == p_strXZQDM)//you wen ti
                        {
                            DropDownListSJZFDW.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                DropDownListSJZFDW.DataSource = null;
            }
        }

        private void InitDropDownListBJZFRY(string p_strZFDWDM)
        {
            string m_strSQL = "";

            if (p_strZFDWDM == null || p_strZFDWDM.Trim() == "")
            {
                return;
            }

            m_strSQL = m_strZFRY_SQL + " WHERE zfry.ZFDWDM='" + p_strZFDWDM + "'";

            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                DropDownListBJZFRY.DataSource = m_DataTable.DefaultView;
                DropDownListBJZFRY.ValueMember = "ZFRYBH";
                DropDownListBJZFRY.DisplayMember = "ZFRYXM";
                //DropDownListBJZFRY.DataBind();
                DropDownListBJZFRY.SelectedIndex = 0;
                btnSave.Enabled = true;
            }
            else
            {
                DropDownListBJZFRY.DataSource=null ;
            }

        }

        private void InitDropDownListSJZFRY(string p_strZFDWDM)
        {
            string m_strSQL = "";

            if (p_strZFDWDM == null || p_strZFDWDM.Trim() == "")
            {
                return;
            }

            m_strSQL = m_strZFRY_SQL + " WHERE zfry.ZFDWDM='" + p_strZFDWDM + "'";

            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                DropDownListSJZFRY.DataSource = m_DataTable.DefaultView;
                DropDownListSJZFRY.ValueMember = "ZFRYBH";
                DropDownListSJZFRY.DisplayMember = "ZFRYXM";

                DropDownListSJZFRY.SelectedIndex = 0;
                btnSave.Enabled = true;
            }
            else
            {
                DropDownListSJZFRY.DataSource=null ;
            }

        }

        private void DropDownListDataBind(ComboBox p_DropDownList, DataTable p_DataTable)
        {
            p_DropDownList.DataSource = p_DataTable.DefaultView;
            p_DropDownList.DisplayMember = "XZQMC";
            p_DropDownList.ValueMember = "XZQDM";
            //p_DropDownList.DataBind();
        }

        private void ClearListbox()
        {
            txtBJZFFZR.Text = "";
            txtSJZFFZR.Text = "";
            txtXZQMC.Text = "";
            DropDownListBJZFRY.DataSource=null ;
            DropDownListBJZFDW.DataSource = null;
            DropDownListSJZFRY.DataSource = null;
            DropDownListSJZFDW.DataSource = null;

        }
 

 


        public void SetButton(bool p_blIsEnable)
        {           
            btnSave.Enabled = p_blIsEnable;
            btnRemove.Enabled = p_blIsEnable;
            //btnExtend.Enabled = p_blIsEnable;
            //btnExtend.Visible = p_blIsEnable;

        }

        private void SetDisabled()
        {
            //if (m_strUserName.Length == 0)
            //{
                DropDownListBJZFDW.Enabled = true;
                DropDownListBJZFRY.Enabled = true;
                DropDownListSJZFDW.Enabled = true;
                DropDownListSJZFRY.Enabled = true;
            //}
            //else if (m_strUserName.Length == 2)
            //{
            //    DropDownListBJZFDW.Enabled = false;
            //    DropDownListBJZFRY.Enabled = false;
            //    DropDownListSJZFDW.Enabled = false;
            //    DropDownListSJZFRY.Enabled = false;
            //}
            //else if (m_strUserName.Length >= 4)
            //{
            //    DropDownListBJZFDW.Enabled = true;
            //    DropDownListBJZFRY.Enabled = true;
            //    DropDownListSJZFDW.Enabled = false;
            //    DropDownListSJZFRY.Enabled = false;
            //}
            //else if (m_strUserName.Length == 6)
            //{
            //}
        }

        private void dataGridViewX1_Click(object sender, EventArgs e)
        {
            int m_intTemp = -1;
          
            //SetButton(false);
            ClearListbox();
            if (this.dataGridViewX1.SelectedRows != null)
            {             

                //if (this.dataGridViewX1.SelectedIndex < 0)
                //{               
                //    SetButton(false);
                //}
                
                 //m_strXZQDM = "";//
                
                string m_strXZQMC = "";
                string m_strBJZFRYXM = "";//        
                string m_strBJZFDWDM = "";
                string m_strBJZFDWMC = "";
                string m_strBJZFRYBH = "";
                string m_strBJZFRYDH = "";
                string m_strSJZFRYXM = "";//        
                string m_strSJZFDWDM = "";
                string m_strSJZFRYBH = "";
                string m_strSJZFRYDH = "";
                string m_strSJZFRYBZ = "";
                string m_strBJZFRYBZ = "";
                string m_strSJZFDWMC = "";
                
                try
                {
                    m_strBJZFRYBH = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员编号"].Value.ToString().Trim();
                    m_strBJZFRYXM = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员姓名"].Value.ToString().Trim();
                    m_strBJZFRYDH = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员办公电话"].Value.ToString().Trim();
                    m_strBJZFDWDM = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员所在单位代码"].Value.ToString().Trim();
                    m_strBJZFDWDM = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员所在单位代码"].Value.ToString().Trim();
                    m_strBJZFRYBZ = this.dataGridViewX1.SelectedRows[0].Cells["本级执法人员说明"].Value.ToString().Trim();
                    txtBJZFRYBZ.Text = m_strBJZFRYBZ;
                    txtBJZFFZR.Text = m_strBJZFRYXM;

                    m_strSJZFRYBH = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员编号"].Value.ToString().Trim();
                    m_strSJZFRYXM = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员姓名"].Value.ToString().Trim();
                    m_strSJZFRYDH = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员办公电话"].Value.ToString().Trim();
                    m_strSJZFDWDM = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员所在单位代码"].Value.ToString().Trim();
                    m_strSJZFRYBZ = this.dataGridViewX1.SelectedRows[0].Cells["上级执法人员说明"].Value.ToString().Trim();
                    txtSJZFRYBZ.Text = m_strSJZFRYBZ;
                    txtSJZFFZR.Text = m_strSJZFRYXM;

                    m_strXZQDM = this.dataGridViewX1.SelectedRows[0].Cells["行政区代码"].Value.ToString().Trim();
                    m_strXZQMC = this.dataGridViewX1.SelectedRows[0].Cells["行政区名称"].Value.ToString().Trim();
                    txtXZQMC.Text = m_strXZQMC;

                    if (m_strBJZFRYBH != "" || m_strSJZFRYBH != "")
                    {
                        btnRemove.Enabled = true;
                    }

                    //m_strZFRYXM.Replace("&nbsp;", "");

                    // m_strZFDWDM = this.dataGridViewX1.SelectedItem.Cells[10].Text.Trim();
                    // m_strXZQDM = this.dataGridViewX1.SelectedItem.Cells[11].Text.Trim();           
                    //m_strXZQMC = this.dataGridViewX1.SelectedItem.Cells[12].Text.Trim();



                    JCZF.ComboBoxItem m_ComboBoxItem ;
                    DataRowView m_DataRow;

                    InitDropDownListBJZFDW(m_strXZQDM);
                    if (DropDownListBJZFDW.Items.Count > 0)
                    {
                        DropDownListBJZFDW.SelectedIndex = -1;
                    }
                    if (m_strBJZFDWDM != "")
                    {                       
                        for (int i = 0; i < DropDownListBJZFDW.Items.Count; i++)
                        {                           
                            m_DataRow = DropDownListBJZFDW.Items[i] as DataRowView;
                            if (m_DataRow.Row["zfdwdm"].ToString() == m_strBJZFDWDM)
                            {
                                DropDownListBJZFDW.SelectedIndex = i;
                                break;
                            }                           
                        }
                    }
                    if (DropDownListBJZFDW.SelectedIndex >= 0)
                    {
                        InitDropDownListBJZFRY(DropDownListBJZFDW.SelectedValue.ToString());
                        if (DropDownListBJZFRY.Items.Count > 0)
                        {
                            DropDownListBJZFRY.SelectedIndex = -1;
                        }
                        if (m_strBJZFRYBH != "")
                        {
                            for (int i = 0; i < DropDownListBJZFRY.Items.Count; i++)
                            {
                                m_DataRow = DropDownListBJZFRY.Items[i] as DataRowView;
                                if (m_DataRow.Row["zfrybh"].ToString() == m_strBJZFRYBH)
                                {
                                    DropDownListBJZFRY.SelectedIndex = i;
                                    break;
                                }
                            }
                        }

                    }
                    else
                    {
                        DropDownListBJZFRY.DataSource = null;
                    }



                    InitDropDownListSJZFDW(m_strXZQDM);
                    if (DropDownListSJZFDW.Items.Count > 0)
                    {
                        DropDownListSJZFDW.SelectedIndex = -1;
                    }
                    if (m_strSJZFDWDM != "")
                    {
                        for (int i = 0; i < DropDownListSJZFDW.Items.Count; i++)
                        {
                            m_DataRow = DropDownListSJZFDW.Items[i] as DataRowView;
                            if (m_DataRow.Row["zfdwdm"].ToString() == m_strSJZFDWDM)
                            {
                                DropDownListSJZFDW.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                      for (int i = 0; i < DropDownListSJZFDW.Items.Count; i++)
                        {
                            m_DataRow = DropDownListSJZFDW.Items[i] as DataRowView;
                            if (m_DataRow.Row["zfdwdm"].ToString().Substring(0, m_strUserName.Length) == m_strUserName)
                            {
                                DropDownListSJZFDW.SelectedIndex = i;
                                break;
                            }
                        }
                    }


                    if (DropDownListSJZFDW.SelectedIndex >= 0)
                    {
                        InitDropDownListSJZFRY(DropDownListSJZFDW.SelectedValue.ToString());
                        if (DropDownListSJZFRY.Items.Count>0)
                        {
                        DropDownListSJZFRY.SelectedIndex = -1;
                        }
                        if (m_strSJZFRYBH != "")
                        {
                            for (int i = 0; i < DropDownListSJZFRY.Items.Count; i++)
                            {
                                m_DataRow = DropDownListSJZFRY.Items[i] as DataRowView;
                                if (m_DataRow.Row["zfrybh"].ToString() == m_strSJZFRYBH)
                                {
                                    DropDownListSJZFRY.SelectedIndex = i;
                                    break;
                                }
                            }
                        }

                    }
                    else
                    {
                        DropDownListSJZFRY.DataSource = null;
                    }

                    if (m_strUserName.Length == 0 || m_strUserName == "admin" || m_strUserName.Length == 2)
                    {
                        DropDownListBJZFDW.Enabled = true;
                        DropDownListBJZFRY.Enabled = true;
                        DropDownListSJZFDW.Enabled = true;
                        DropDownListSJZFRY.Enabled = true;
                    }
                    else if (m_strXZQDM.Length > m_strUserName.Length || m_strUserName.Length>=6)
                    {
                        if (m_strUserName.Length >= 6)
                        {
                            DropDownListBJZFDW.Enabled = true;
                            DropDownListBJZFRY.Enabled = true;
                        }
                        else
                        {
                            //DropDownListBJZFDW.Enabled = false;
                            //DropDownListBJZFRY.Enabled = false;
                        }
                        DropDownListSJZFDW.Enabled = true;
                        DropDownListSJZFRY.Enabled = true;
                    }
                    else if (m_strXZQDM.Length == m_strUserName.Length)
                    {
                        //DropDownListBJZFDW.Enabled = true;
                        //DropDownListBJZFRY.Enabled = true;
                        //DropDownListSJZFDW.Enabled = false;
                        //DropDownListSJZFRY.Enabled = false;
                    }

                    //SetButton(true);
                    if (DropDownListBJZFRY.SelectedIndex >= 0)
                    {
                        btnSave.Enabled = true;
                    }

                }
                catch (System.Exception errs)
                {
                }
            }
        }

        private void DropDownListBJZFDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView m_DataRow;
                if (DropDownListBJZFDW.SelectedIndex >= 0)
                {
                    InitDropDownListBJZFRY(DropDownListBJZFDW.SelectedValue.ToString());
                    if (DropDownListBJZFDW.Items.Count > 0)
                    {
                        btnSave.Enabled = true;
                    }

                    if (dataGridViewX1.SelectedRows != null && dataGridViewX1.SelectedRows.Count>0 && dataGridViewX1.SelectedRows[0].Index >= 0)
                    {
                        string m_strZFRYBH = dataGridViewX1.SelectedRows[0].Cells["本级执法人员编号"].Value.ToString();

                        for (int i = 0; i < DropDownListBJZFRY.Items.Count; i++)
                        {
                            m_DataRow = DropDownListBJZFRY.Items[i] as DataRowView;
                            if (m_DataRow.Row["zfrybh"].ToString() == m_strZFRYBH)
                            {
                                DropDownListBJZFRY.SelectedIndex = i;
                            }

                            //if (DropDownListBJZFRY.Items[i].Value == m_strZFRYBH)
                            //{
                            //    DropDownListBJZFRY.SelectedIndex = i;

                            //    break;
                            //}
                        }
                    }
                }
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this, errs.Message);
            }
        }

        private void DropDownListSJZFDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView m_DataRow;
                if (DropDownListSJZFDW.SelectedIndex >= 0)
                {
                    InitDropDownListSJZFRY(DropDownListSJZFDW.SelectedValue.ToString());

                    //if (dataGridViewX1.SelectedRows != null && dataGridViewX1.SelectedRows[0].Index >= 0)
                    //{
                    //    string m_strZFRYBH = dataGridViewX1.SelectedRows[0].Cells["上级执法人员编号"].Value.ToString();

                    //    for (int i = 0; i < DropDownListSJZFRY.Items.Count; i++)
                    //    {
                    //        m_DataRow = DropDownListBJZFRY.Items[i] as DataRowView;
                    //        if (m_DataRow.Row["zfrybh"].ToString() == m_strZFRYBH)
                    //        {
                    //            DropDownListBJZFRY.SelectedIndex = i;
                    //        }                           
                    //    }
                    //}
                }
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this,errs.Message );
            }
        }

        private void DropDownListBJZFRY_SelectedIndexChanged(object sender, EventArgs e)
        {
            //labInfo.Text = "";
            if (DropDownListBJZFRY.SelectedIndex >= 0)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private void DropDownListSJZFRY_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txt.Text = "";
            if (DropDownListSJZFRY.SelectedIndex >= 0)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //this.labInfo.Visible = false;

            if (MessageBox.Show("将彻底网格关系，并且不保留历史网格关系。\n\n如果需要保留历史网格关系，请直接修改网格关系后点击“保存”按钮。\n\n是否继续删除？","删除提示", MessageBoxButtons.YesNo,MessageBoxIcon.Question ) == DialogResult.No)
            {
                return;
            }

            btnSave.Enabled = false;
            txtSJZFFZR.Text = "";
            txtBJZFFZR.Text = "";

            string m_strXZQDM = "";//
            string m_strXZQMC = "";
            string m_strBJZFRYBH = "";
            string m_strSJZFRYBH = "";
            string m_strID = "";
            string m_strSQL = "";
            int[] m_intSeleced = new int[dataGridViewX1.SelectedRows.Count];
            for (int i = 0; i < dataGridViewX1.SelectedRows.Count; i++)
            {
                //ArrayList m_Arrylist = new ArrayList();
                m_intSeleced[i] = dataGridViewX1.SelectedRows[i].Index;
                try
                {
                    m_strBJZFRYBH = this.dataGridViewX1.SelectedRows[i].Cells["本级执法人员编号"].Value.ToString().Trim();
                    m_strXZQMC = this.dataGridViewX1.SelectedRows[i].Cells["行政区名称"].Value.ToString().Trim();
                    m_strXZQDM = this.dataGridViewX1.SelectedRows[i].Cells["行政区代码"].Value.ToString().Trim();

                    m_strSJZFRYBH = this.dataGridViewX1.SelectedRows[i].Cells["上级执法人员编号"].Value.ToString().Trim();
                    m_strID = this.dataGridViewX1.SelectedRows[i].Cells["网格标识"].Value.ToString().Trim();



                    //m_strID = m_item.Cells[18].Text.Trim();

                    if (DropDownListBJZFRY.Enabled == true && m_strID != "")
                    {
                        if (m_strSJZFRYBH == "")
                        { 
                            //上级和本级执法人员都没有指定，那么开始时间也要置为空
                            m_strSQL = "update ZFWG set bjzfrybh=null,bjzfrybz=null,kssj=null WHERE  ID=" + m_strID; 
                        }
                        else
                        {
                            m_strSQL = "update ZFWG set bjzfrybh=null,bjzfrybz=null WHERE  ID=" + m_strID;
                        }
                        m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                    }
                    if (DropDownListSJZFRY.Enabled == true && m_strID != "&nbsp;")
                    {
                        if (m_strBJZFRYBH == "")
                        {
                            //上级和本级执法人员都没有指定，那么开始时间也要置为空
                            m_strSQL = "update ZFWG set sjzfrybh=null,sjzfrybz=null,kssj=null  WHERE  ID=" + m_strID;
                        }
                        else
                        {
                            m_strSQL = "update ZFWG set sjzfrybh=null,sjzfrybz=null WHERE  ID=" + m_strID;
                        }
                        m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                    }

                    if (DropDownListSJZFRY.Enabled == true && m_strID != "&nbsp;" && DropDownListBJZFRY.Enabled == true)
                    {
                        m_strSQL = "update ZFWG set sjzfrybh=null,bjzfrybh=null,bjzfrybz=null,sjzfrybz=null,kssj=null WHERE  ID=" + m_strID;
                        m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                    }
                        
                }
                catch (System.Exception errs)
                {                    
                    m_DataAccess_SYS_.MessageErrorInforShow(this, "移除失败！\n"+errs.Message );
                    continue ;
                }

            }          

            InitDataGridview();           
            ResetSelectedRows(m_intSeleced);
            m_DataAccess_SYS_.MessageInforShow(this, "移除成功！");
            btnSave.Enabled = true;
            //labInfo.Visible = true;
            //labInfo.Text = "移除成功！";
           
        }

        private void frmWCGL_JG_WG_SizeChanged(object sender, EventArgs e)
        {
            SetPanelPosition();
        }


        private void SetPanelPosition()
        {
            if (this.Width - panel1.Width > 0)
            {
                panel1.Left = (this.Width - panel1.Width) / 2 - 5;
            }
            else
            {
                panel1.Left = 0;
            }
            if (this.Width - panel2.Width > 0)
            {
                panel2.Left = (this.Width - panel2.Width) / 2 - 5;
            }
            else
            {
                panel2.Left = 0;
            }
        }

        private void frmWCGL_JG_WG_Load(object sender, EventArgs e)
        {
            SetPanelPosition();
        }

        private void btnExtend_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_strXZQDM == null || m_strXZQDM.Trim() == "") return;

                InitDropDownListBJZFDW_All(m_strXZQDM.Substring(0, 2));

                InitDropDownListSJZFDW_All(m_strXZQDM.Substring(0, 2));
            }
            catch
            {
            }
        }

        private void InitDropDownListBJZFDW_All(string p_strXZQDM)
        {
            string m_strSQL = "";

            

                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = m_strZFDW_SQL + "  WHERE  substr(ZFDWDM,1," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'  order by zfdwdm";
                    }
                    else
                    {
                        m_strSQL = m_strZFDW_SQL + "  WHERE  LEFT(ZFDWDM," + p_strXZQDM.Length + ")='" + p_strXZQDM+ "'  order by zfdwdm";
                    }

               
            
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                DropDownListBJZFDW.DataSource = m_DataTable.DefaultView;
                DropDownListBJZFDW.DisplayMember = "zfdwmc";
                DropDownListBJZFDW.ValueMember = "zfdwdm";
                //DropDownListBJZFDW.DataBind();
            }
            else
            {
                DropDownListBJZFDW.DataSource = null;
            }
        }

        private void InitDropDownListSJZFDW_All(string p_strXZQDM)
        {
            string m_strSQL = "";

            if (p_strXZQDM == null || p_strXZQDM.Trim() == "")
            {
                return;
            }
            p_strXZQDM = p_strXZQDM.Trim();

            //if (m_strUserName.Length == 0)
            //{
           
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    m_strSQL = m_strZFDW_SQL + "  WHERE  substr(ZFDWDM,1," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'  order by zfdwdm";
                }
                else
                {
                    m_strSQL = m_strZFDW_SQL + "  WHERE  LEFT(ZFDWDM," + p_strXZQDM.Length + ")='" + p_strXZQDM+ "'  order by zfdwdm";
                }

          

            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                DropDownListSJZFDW.DataSource = m_DataTable.DefaultView;
                DropDownListSJZFDW.ValueMember = "zfdwdm";
                DropDownListSJZFDW.DisplayMember = "zfdwmc";
                //DropDownListSJZFDW.DataBind();

                for (int i = 0; i < DropDownListSJZFDW.Items.Count; i++)
                {
                    if (p_strXZQDM.Length >= 6)
                    {
                        if (DropDownListSJZFDW.Items[i].ToString().Substring(0, 6) == p_strXZQDM.Substring(0, 6))//you wen ti
                        {
                            DropDownListSJZFDW.SelectedIndex = i;
                            break;
                        }
                    }
                    else
                    {
                        if (DropDownListSJZFDW.Items[i].ToString().Substring(0, p_strXZQDM.Length) == p_strXZQDM)//you wen ti
                        {
                            DropDownListSJZFDW.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                DropDownListSJZFDW.DataSource = null;
            }
        }
      

       

     
    }
}
