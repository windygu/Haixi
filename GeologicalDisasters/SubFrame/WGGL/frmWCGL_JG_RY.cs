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
    public partial class frmWCGL_JG_RY : Form
    {
        public   string m_strSQL;
        DataSet m_DataSet;
        //System.Data.OleDb.OleDbDataAdapter  m_MySqlDbDataAdapter;
        System.Data.OleDb.OleDbDataAdapter  m_OleDbDataAdapter;


        public string m_strZFDWDM;
        public string m_strZFDWMC;
        public string m_strXZQDM;
        

        //private WGGL.clsZFDW m_clsZFDW_;
        private WGGL.clsZFRY m_clsZFRY_;
        //public WGGL.clsZFDW m_clsZFDW
        //{
        //    set
        //    {
        //        m_clsZFDW_ = value;
        //    }
        //}
       

        private clsDataAccess.DataAccess m_DataAccess_SYS_;
        public clsDataAccess.DataAccess m_DataAccess_SYS
        {
            set
            {
                m_DataAccess_SYS_ = value;
            }
        }

        bool m_blHasModified;
        public frmWCGL_JG_RY()
        {
            InitializeComponent();
            m_blHasModified = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            //dataGridViewX1.DataSource

            if (m_DataSet.HasChanges())
            {
                try
                {
                    if (m_DataAccess_SYS_.ProviderIsMySQLDB())
                    {
                        //m_MySqlDbDataAdapter.Update(m_DataSet.Tables[0]);
                    }
                    else
                    {
                        m_OleDbDataAdapter.Update(m_DataSet.Tables[0]);
                    }
                    m_DataSet.Tables[0].AcceptChanges();
                    m_DataAccess_SYS_.MessageInforShow(this, "保存成功！");
                }
                catch(SystemException errs)
                {
                    m_DataAccess_SYS_.MessageErrorInforShow(this, errs.Message );
                }
            }
            //m_blHasModified = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //clsFunction.Function.MessageBoxQuestion("")==DialogResult.Yes 
            string m_strZFRYBH = "";
            string m_strZFRYXM = "";
            clsZFRY m_clsZFRY=new clsZFRY();
            m_clsZFRY.m_DataAccess_SYS_=m_DataAccess_SYS_;

            for (int i = 0; i < dataGridViewX1.SelectedRows.Count; i++)
            {
                try
                {
                    m_strZFRYBH = System.Convert.ToString(dataGridViewX1.SelectedRows[i].Cells["执法人员编号"].Value);
                    m_strZFRYXM = System.Convert.ToString(dataGridViewX1.SelectedRows[i].Cells["执法人员姓名"].Value);
                    m_clsZFRY.m_strZFRY_ZFRYBH = m_strZFRYBH;
                    m_clsZFRY.m_strZFRY_ZFRYXM = m_strZFRYXM;
                    DataRow m_DataRow = m_clsZFRY.GetZFRY_By_ZFRYBH(m_strZFRYBH);
                    if (m_DataRow != null)
                    {
                        m_clsZFRY.Delete();

                        InitDataGridview();
                    }
                    else
                    {
                        dataGridViewX1.Rows.Remove(dataGridViewX1.SelectedRows[i]);
                    }
                    
                }
                catch
                {
                    continue;
                }
            }

           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_DataSet.HasChanges())
                {
                    if (m_DataAccess_SYS_.MessageQuestionShow(this, "您修改了数据，是否需要保存？") == DialogResult.Yes)
                    {
                        btnSave_Click(sender, e);
                    }                                     
                }
                this.Close();
            }
            catch(SystemException errs)
            {
            }
            
        }

        private void frmWCGL_JG_DW_Load(object sender, EventArgs e)
        {
           
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
            //m_strSQL = CreatSQL();
            if (m_strSQL != "" && m_DataAccess_SYS_ != null)
            {
                //DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
                //m_DataSet = m_DataTable.DataSet;
                m_DataSet = null;
                m_OleDbDataAdapter = null;
                //m_MySqlDbDataAdapter = null;

                m_DataSet = new DataSet();
                if (m_DataAccess_SYS_.ProviderIsMySQLDB())
                {
                    //m_MySqlDbDataAdapter = new System.Data.OleDb.OleDbDataAdapter(m_strSQL, m_DataAccess_SYS_.DBConnection_OleDb);
                    //m_MySqlDbDataAdapter.Fill(m_DataSet, "zfdw");
                }
                else
                {
                    m_OleDbDataAdapter = new System.Data.OleDb.OleDbDataAdapter (m_strSQL, m_DataAccess_SYS_.DBConnection_OleDb);
                    m_OleDbDataAdapter.Fill(m_DataSet, "zfdw");
                }
                dataGridViewX1.DataSource = m_DataSet.Tables[0];

                //为m_OleDbDataAdapter自动生成insert、Update、Delete命令

                 //System.Data.OleDb.OleDbCommandBuilder  m_MySqlCommandBuilder = new System.Data.OleDb.OleDbCommandBuilder (m_OleDbDataAdapter);

                if (m_DataAccess_SYS_.ProviderIsMySQLDB())
                {
                    //System.Data.OleDb.OleDbCommandBuilder m_MySqlCommandBuilder = new System.Data.OleDb.OleDbCommandBuilder(m_MySqlDbDataAdapter);
                }
                else
                {
                    System.Data.OleDb.OleDbCommandBuilder m_OleDbCommandBuilder = new System.Data.OleDb.OleDbCommandBuilder(m_OleDbDataAdapter);
                }

                dataGridViewX1.Columns["标识"].ReadOnly = true;
                dataGridViewX1.Columns["执法人员编号"].ReadOnly = true;
                //dataGridViewX1.Columns["执法单位名称"].ReadOnly = true;
                dataGridViewX1.Columns["执法单位代码"].ReadOnly = true;
                m_clsZFRY_ = new clsZFRY();
                m_clsZFRY_.m_DataAccess_SYS_ = m_DataAccess_SYS_;
                m_clsZFRY_.m_strXZQDM = m_strXZQDM;
                m_clsZFRY_.m_strZFDW_ZFDWDM = m_strZFDWDM;
            }
        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string m_strNewZFRYBH="";
            string m_strSQL="";
            if (dataGridViewX1.Columns[e.ColumnIndex].Name == "执法人员姓名")
            {
                if (dataGridViewX1.Rows[e.RowIndex].Cells["标识"].Value.ToString() != "") return;//不是新增数据

                if (m_clsZFRY_ != null)
                {
                    m_clsZFRY_.m_strXZQDM = this.m_strXZQDM;
                    m_clsZFRY_.m_strZFDW_ZFDWDM = this.m_strZFDWDM;
                }
                else 
                {
                    return ;
                }
                //m_strNewZFRYBH = m_clsZFRY_.CreatNewZFRYBH();//这是从数据库中获得最大，但是在这里如果连续添加多个人员后再保存入数据库，就编号可能出错

                m_strNewZFRYBH = CreatNewZFRYBH();            //，所以只有从界面的表中取最大值然后加1

                dataGridViewX1.Rows[e.RowIndex].Cells["执法人员编号"].Value = (object)m_strNewZFRYBH;
                dataGridViewX1.Rows[e.RowIndex].Cells["登记时间"].Value = (object)DateTime.Now.ToShortDateString();
                dataGridViewX1.Rows[e.RowIndex].Cells["在职状态"].Value = true;
                //dataGridViewX1.Rows[e.RowIndex].Cells["执法单位名称"].Value = m_strZFDWMC;
                dataGridViewX1.Rows[e.RowIndex].Cells["执法单位代码"].Value = m_strZFDWDM;
                //if(m_strZFDWDM.Length !=8)
                //{
                //   m_DataAccess_SYS_.MessageErrorInforShow(this,"执法单位编码为8位,编码规则为:\n本单位所属行政区编码(6位)+顺序号(2位)");
                //    return;
                //}
                //else 
                //{
                //m_strSQL="SELECT * FROM ";
                //}
            }
        }

        private string CreatNewZFRYBH()
        {
            string m_strNewZFRYBH = "";
            string m_strMaxZFRYBH = "0";
            string m_strTempZFRYBH = "0";

            try
            {
                for (int i = 0; i <= dataGridViewX1.Rows.Count-1; i++)
                {
                    m_strTempZFRYBH = System.Convert.ToString(dataGridViewX1.Rows[i].Cells["执法人员编号"].Value);
                    if (m_strTempZFRYBH == null || m_strTempZFRYBH == "") continue;

                    if (System.Convert.ToInt64(m_strTempZFRYBH) > System.Convert.ToInt64(m_strMaxZFRYBH))
                    {
                        m_strMaxZFRYBH = m_strTempZFRYBH;
                    }
                }
                if (m_strMaxZFRYBH == null || m_strMaxZFRYBH == "" || m_strMaxZFRYBH.Length <=1)
                {
                    m_strMaxZFRYBH = m_strZFDWDM + "001";                    
                }
                else
                {
                    m_strMaxZFRYBH = (System.Convert.ToInt64(m_strMaxZFRYBH) + 1).ToString();
                }
            }
            catch (SystemException errs)
            {
                //clsFunction.Function.MessageBoxError(errs.Message);
            }
            return m_strMaxZFRYBH;

        }

        private void dataGridViewX1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //string m_strZFDWDM = "";
            //string m_strSQL = "";
            //if (dataGridViewX1.Columns[e.ColumnIndex].Name == "执法单位名称")
            //{
            //    if (dataGridViewX1.Rows[e.RowIndex].Cells["标识"].Value.ToString() != "") return;//不是新增数据


            //    m_strZFDWDM = dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            //    if (m_strZFDWDM.Length != 8)
            //    {
            //        m_DataAccess_SYS_.MessageErrorInforShow(this, "执法单位编码为8位,编码规则为:\n本单位所属行政区编码(6位)+顺序号(2位)");
            //        return;
            //    }
            //    else
            //    {
            //        m_strSQL = "SELECT * FROM";
            //    }
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            InitDataGridview();
        }

       

     
    }
}
