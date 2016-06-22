using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame.SystemManagement
{
    /// <summary>
    /// 系统管理员的用户名称为admin，不能删除
    /// 各地系统系统管理员的用户为本地区行政区代码
    /// </summary>
    public partial class frmUsers : Form
    {

       




        public string m_strZFDWDM;
        string m_strUserID = "";
        string m_strUserName = "";
        string m_strZFRYXM = "";
        string m_strZFRYBH = "";
        string m_strUserAliseName = "";

        public string m_strInitSQL;

        private clsDataAccess.DataAccess m_DataAccess_SYS_;
        public clsDataAccess.DataAccess m_DataAccess_SYS
        {
            set
            {
                m_DataAccess_SYS_ = value;
            }
        }  

        public frmUsers()
        {
            InitializeComponent();
        }

     public void  InitDataGridview()
      {
          DataTable m_DataTable;
          m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strInitSQL);
          if (m_DataTable != null)
          {
              dataGridViewX1.DataSource = m_DataTable.DefaultView;
          }
          else
          {
              dataGridViewX1.DataSource = null;
          }
      }

     private void dataGridViewX1_Click(object sender, EventArgs e)
     {
         if (dataGridViewX1.SelectedRows != null && dataGridViewX1.SelectedRows.Count > 0)
         {
             m_strUserID = dataGridViewX1.SelectedRows[0].Cells["用户标识"].Value.ToString();
             m_strUserName = dataGridViewX1.SelectedRows[0].Cells["用户名"].Value.ToString();
             m_strZFRYBH = dataGridViewX1.SelectedRows[0].Cells["执法人员编号"].Value.ToString();
             m_strZFRYXM = dataGridViewX1.SelectedRows[0].Cells["执法人员姓名"].Value.ToString();
             //m_strUserAliseName=dataGridViewX1.SelectedRows[0].Cells["用户别名"].Value.ToString();

             txtName.Text = m_strZFRYXM;
             txtUserName.Text = m_strUserName;
             //txtUserAliseName.Text  = m_strUserAliseName;
         }
     }

     private void btnAdd_Click(object sender, EventArgs e)
     {
         try
         {
             if (IsAvilable(txtUserName.Text.Trim()) == false)
             {
                 m_DataAccess_SYS_.MessageInforShow(this, "该用户名已经占用，请另选用户名！");
                 txtUserName.Focus();
                 txtUserName.SelectAll();
                 return;
             }
             InsertUser(m_strZFDWDM);           
         }
         catch(SystemException errs)
         {
             m_DataAccess_SYS_.MessageErrorInforShow(this, errs.Message );
         }
     }

     private void InsertUser(string p_strZFRYBH)
     {
         try
         {         

             //string m_strSQL = "";
             //m_strSQL = "insert into sys_users(zfrybh,username,password,UserAliseName) values('" + p_strZFRYBH + "','" + txtUserName.Text + "','" + txtPassword.Text + "','" + txtUserAliseName.Text + "')";
             //m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);

             //InitDataGridview();

             //m_DataAccess_SYS_.MessageInforShow(this, "添加成功！");
         }
         catch (SystemException errs)
         {
             m_DataAccess_SYS_.MessageErrorInforShow(this, errs.Message);
         }
     }

     private void btnUpdate_Click(object sender, EventArgs e)
     {
         int m_intSelectedIndex;
         try
         {
             if (dataGridViewX1.SelectedRows == null || dataGridViewX1.SelectedRows.Count < 1)
             {
                 m_DataAccess_SYS_.MessageInforShow(this, "请选择一个用户并修改后才能更新！");
                 return;
             }

             if (IsAvilable(txtUserName.Text.Trim()) == false)
             {
                 m_DataAccess_SYS_.MessageInforShow(this, "该用户名已经占用，请另选用户名！");
                 txtUserName.Focus();
                 txtUserName.SelectAll();
                 return;
             }
             m_intSelectedIndex = dataGridViewX1.SelectedRows[0].Index;
             string m_strSQL = "";
             if (m_strUserID.Trim() != "")
             {
                 m_strSQL = "UPDATE zfry SET  username='" + txtUserName.Text + "', password='" + txtPassword.Text + "'   WHERE id=" + m_strUserID;
                 m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                 InitDataGridview();
                 m_DataAccess_SYS_.MessageInforShow(this, "更新成功！");
             }
             else
             {
                 InsertUser(m_strZFRYBH);
             }
            
             dataGridViewX1.Rows[m_intSelectedIndex].Selected = true;            
         }
         catch (SystemException errs)
         {
             m_DataAccess_SYS_.MessageErrorInforShow(this, errs.Message);
         }
     }

        /// <summary>
        /// 用户名是否可用，没有重复的就可以用
        /// </summary>
        /// <param name="p_strUserName"></param>
        /// <returns></returns>
     private bool IsAvilable(string p_strUserName)
     {
         bool m_IsAvilable = false;
         string m_strSQL = "";
         m_strSQL = "select username from  zfry where username='" + p_strUserName + "'";
        DataRowCollection m_DataRowCollection= m_DataAccess_SYS_.getDataRowsByQueryString(m_strSQL);
        if (m_DataRowCollection == null || m_DataRowCollection.Count < 1)
        {
            //没有重复的
            m_IsAvilable = true;
        }
        return m_IsAvilable;
     }


     private void btnDelete_Click(object sender, EventArgs e)
     {
         int m_intSelectedIndex;
         try
         {
             if (dataGridViewX1.SelectedRows == null || dataGridViewX1.SelectedRows.Count < 1)
             {
                 m_DataAccess_SYS_.MessageInforShow(this, "请选择一个用户后才能删除！");
                 return;
             }
             if (m_strUserID=="")
             {
                 m_DataAccess_SYS_.MessageInforShow(this, "该执法人员尚未设置用户信息！");
                 return;
             }

             if (m_strUserName == "admin")
             {
                 m_DataAccess_SYS_.MessageInforShow(this, "管理员信息不能删除！");
                 return;
             }

             if (m_DataAccess_SYS_.MessageQuestionShow(this, "您确定要删除该用户吗？") == DialogResult.Yes)
             {
                 m_intSelectedIndex = dataGridViewX1.SelectedRows[0].Index;
                 string m_strSQL = "";
                 m_strSQL = "delete from sys_users  WHERE USERID=" + m_strUserID;
                 m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                 InitDataGridview();
                 dataGridViewX1.Rows[m_intSelectedIndex].Selected = true;
                 m_DataAccess_SYS_.MessageInforShow(this, "删除成功！");
             }        
         }
         catch (SystemException errs)
         {
             m_DataAccess_SYS_.MessageErrorInforShow(this, errs.Message);
         }
     }

     private void btnClose_Click(object sender, EventArgs e)
     {
         this.Close();
     }

     private void label4_Click(object sender, EventArgs e)
     {

     }

    
    }
}
