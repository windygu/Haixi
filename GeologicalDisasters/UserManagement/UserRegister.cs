using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using ComprehensiveEvaluation.UserManagement;

namespace ComprehensiveEvaluation
{
    public partial class UserRegister : Form
    {
        private static string _srcConn;//数据库链接信息
        private OleDbConnection _oleDbConn;//access数据库链接对象
        private OleDbDataAdapter _oleDbAda;//数据库结果链接对象
        private string mdbpath;//数据库路径
        SystemSet set = new SystemSet();
        public UserRegister()
        {
            mdbpath = set.User_DB;
            InitializeComponent();
            _srcConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbpath;
        }
        //链接函数
       public DataTable SqlSearch(string strSql,string tableName)
        {
            try
            {
                _oleDbConn = new OleDbConnection(_srcConn);
                _oleDbConn.Open();
                _oleDbAda = new OleDbDataAdapter(strSql, _oleDbConn);
                if (!string.IsNullOrEmpty(strSql) && !string.IsNullOrEmpty(tableName))
                {
                    var dt = new DataSet();
                    _oleDbAda.Fill(dt, tableName);
                    _oleDbAda.Dispose();
                    _oleDbConn.Close();
                    return dt.Tables[tableName];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private bool txtbox(DevComponents.DotNetBar.Controls.TextBoxX txt)
        {
          bool tt=!string.IsNullOrEmpty(txt.Text);
          return tt;
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string selectSQL = "SELECT 用户信息表.用户名 FROM 用户信息表 ";//WHERE ((用户信息表.用户名)='" + textBoxX1.Text + "')";//"select * from 用户信息表 ";// + textBoxX1.Text;
            DataTable dt = SqlSearch(selectSQL,"用户信息表");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == textBoxX1.Text)
                {
                    MessageBox.Show("用户名已存在，请更换信息");
                    return;
                }
            }
            try
            {
                if (txtbox(textBoxX1) && txtbox(textBoxX2) && txtbox(textBoxX3) && txtbox(textBoxX4) && txtbox(textBoxX5) && txtbox(textBoxX6) && txtbox(textBoxX7) && checkBoxX1.Checked)
                {
                    if(textBoxX5.Text!="434485356")
                    {
                        MessageBox.Show("推荐码错误，请重试");
                            return;
                    }
                    if (textBoxX2.Text != textBoxX3.Text)
                    {
                        MessageBox.Show("两次输入密码不一致！请检查");
                        return;
                    }
                    string strInsert = " INSERT INTO 用户信息表 ( 用户名 , 密码 , 用户单位 , 推荐码 , 邮箱 , 联系方式 , 保密协议 , 注册日期 ) VALUES ( '";
                    strInsert += textBoxX1.Text + "','";
                    strInsert += textBoxX2.Text + "','";
                    strInsert += textBoxX4.Text + "','";
                    strInsert += textBoxX5.Text + "','";
                    strInsert += textBoxX6.Text + "','";
                    strInsert += textBoxX7.Text + "',";
                    strInsert += checkBoxX1.Checked + ",'";
                    strInsert += DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    OleDbConnection myConn = new OleDbConnection(_srcConn);
                    myConn.Open();
                    OleDbCommand inst = new OleDbCommand(strInsert, myConn);
                    inst.ExecuteNonQuery();
                    MessageBox.Show("恭喜你！" + "\r\n" + "用户：" + textBoxX1.Text + "\r\n" + "注册成功！", "注册成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    myConn.Close();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("以上内容不能为空!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Register_Load(object sender, EventArgs e)
        {
            checkBoxX1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DbConnectionMySQL.connection("localhost", "root", "noroot", this.listBoxAdv1);
        }

        private void listBoxAdv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DbConnectionMySQL.showTables(this.listBoxAdv1, this.listBoxAdv2);
        }
    }
}
