using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame.SystemManagement
{
    public partial class uctSystemManagement : UserControl
    {
        public string m_strSystemUserName_;
        public string m_strSystemUserName
        {
            set
            {
                txtUserName.Text  = value;
            }
        }  
        private frmUsers m_frmUsers;
        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {               
                uctXZQTree_Dev1.m_AxMapControl = value;               
                m_AxMapControl_ = value;
            }
        }

        private clsDataAccess.DataAccess m_DataAccess_SYS_;
        public clsDataAccess.DataAccess m_DataAccess_SYS
        {
            set
            {
                m_DataAccess_SYS_ = value;           
            }
        }  

        public uctSystemManagement()
        {
            InitializeComponent();
        }

        private void uctXZQTree_Dev1_uctXZQTree_DevSelectEvent(string p_strXZQDM)
        {
            FillComb_ZFDW(cmbDW, p_strXZQDM);
        }

        private void FillComb_ZFDW(ComboBox p_ComboBox, string p_strXZQDM)
        {
            string m_strSQL;
            //m_DataAccess_SYS_ = m_DataAccess_SYS_;
            //string m_strSQL = "SELECT * FROM ZFRY_SYS_USERS WHERE LEFT(ZFRYBH," + p_strXZQDM.Length + ")='" + p_strXZQDM+"' ";
            if (p_strXZQDM.Length  <= 6)
            {
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    m_strSQL = "SELECT * FROM ZFDW WHERE substr(ZFDWDM,1," + p_strXZQDM.Length + ")='" + p_strXZQDM + "' ";
                }
                else
                {
                    m_strSQL = "SELECT * FROM ZFDW WHERE LEFT(ZFDWDM," + p_strXZQDM.Length + ")='" + p_strXZQDM + "' ";
                }
            }
            else
            {
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    m_strSQL = "SELECT * FROM ZFDW WHERE substr(ZFDWDM,1,6)='" + p_strXZQDM.Substring(0, 6) + "' ";
                }
                else
                {
                    m_strSQL = "SELECT * FROM ZFDW WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM.Substring(0, 6) + "' ";
                }
            }

            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL); ;

            if (m_DataTable == null)
            {
                p_ComboBox.DataSource = null;
                return;
            }
            DataRow m_DataRow = m_DataTable.NewRow();
            m_DataRow["ZFDWDM"] = "0";
            m_DataRow["ZFDWMC"] = "全部";
            m_DataTable.Rows.InsertAt(m_DataRow, 0);
            p_ComboBox.DataSource = m_DataTable.DefaultView;
            p_ComboBox.ValueMember = "ZFDWDM";
            p_ComboBox.DisplayMember = "ZFDWMC";
            if (p_ComboBox.Items.Count > 0)
            {
                p_ComboBox.SelectedIndex = 0;
            }
        }

        private void btnUserOK_Click(object sender, EventArgs e)
        {
            try
            {


                string m_strSQL = "";
                if (cmbDW.SelectedIndex > 0)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = "SELECT id as 用户标识,zfryxm as 执法人员姓名 ,zfrybh as 执法人员编号 ,username as 用户名,password as 密码 FROM ZFRY WHERE substr(zfrybh,1,8)='" + cmbDW.SelectedValue.ToString() + "' ";
                    }
                    else
                    {
                        m_strSQL = "SELECT id as 用户标识,zfryxm as 执法人员姓名 ,zfrybh as 执法人员编号 ,username as 用户名,password as 密码 FROM ZFRY WHERE LEFT(zfrybh,8)='" + cmbDW.SelectedValue.ToString() + "' ";
                    }
                }
                else
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = "SELECT id as 用户标识,zfryxm as 执法人员姓名 ,zfrybh as 执法人员编号 ,username as 用户名,password as 密码 FROM ZFRY WHERE substr(zfrybh,1,8)='" + cmbDW.SelectedValue.ToString() + "' ";
                    }
                    else
                    {
                        m_strSQL = "SELECT id as 用户标识,zfryxm as 执法人员姓名 ,zfrybh as 执法人员编号  ,username as 用户名,password as 密码 FROM ZFRY  WHERE LEFT(zfrybh," + uctXZQTree_Dev1.m_strXZQDM.Length + ")='" + uctXZQTree_Dev1.m_strXZQDM + "' ";
                    }
                }

                if (m_frmUsers == null || m_frmUsers.IsDisposed == true)
                {
                    m_frmUsers = new frmUsers();
                    m_frmUsers.m_DataAccess_SYS = m_DataAccess_SYS_;
                }
                if (cmbDW.SelectedIndex > 0)
                {
                    m_frmUsers.m_strZFDWDM = cmbDW.SelectedValue.ToString();
                }
                else
                {
                    m_frmUsers.m_strZFDWDM = uctXZQTree_Dev1.m_strXZQDM;
                }
                m_frmUsers.m_strInitSQL = m_strSQL;
                m_frmUsers.InitDataGridview();
                m_frmUsers.MdiParent = this.ParentForm;                
                m_frmUsers.Show();
                m_frmUsers.Focus();
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void btnUserPasswordSave_Click(object sender, EventArgs e)
        {
            try
            {
            if (txtPasswordNew1.Text.Trim() != txtPasswordNew2.Text.Trim())
            {
                m_DataAccess_SYS_.MessageInforShow(this.ParentForm, "两次填写的密码不一致！");
                txtPasswordNew2.SelectAll();
                txtPasswordNew2.Focus();
                return;
            }

            
                string m_strSQL = "update zfry set PASSWORD='" + txtPasswordNew1.Text + "' WHERE USERNAME='" + txtUserName.Text + "' AND PASSWORD='" + txtPasswordOld.Text + "'";

                int m_intUpdate = m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                if (m_intUpdate < 1)
                {
                    m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, "原密码错误！");
                }
            }
            catch(SystemException errs )
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message );
            }

        }
    }
}
