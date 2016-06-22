using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JCZF.MainFrame;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace JCZF.SubFrame.DataEdit
{
    public partial class frmDataEdit : DevComponents.DotNetBar.Office2007Form
    {
        public delegate void ZoomToFeaEventHandler(string OID);
        public event ZoomToFeaEventHandler ZoomToFea;

        //private frmMain parenForm;
        private string strIsOK = "1";
        public frmDataEdit()
        {
            InitializeComponent();
            //this.parenForm = parenForm;
        }

        private void frmDataQueryShow_DoubleClick(object sender, EventArgs e)
        {
            ZoomToFea(this.listView1.SelectedItems[0].SubItems[0].Text.ToString());
        }

        private void frmDataQueryShow_Load(object sender, EventArgs e)
        {
            //this.MdiParent = this.parenForm;
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pdlg = new FolderBrowserDialog();
            if (pdlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string m_FilePath = pdlg.SelectedPath;
            txtPath.Text = m_FilePath;
            
        }

        //导出
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtPath.Text == "")
            {
                MessageBox.Show("选择导出路径！");
                return;
            }
            //SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            //SaveFileDialog1.CreatePrompt = true;
            //SaveFileDialog1.OverwritePrompt = true;
            //SaveFileDialog1.FileName = "exportcoord";
            //SaveFileDialog1.DefaultExt = "txt";
            //SaveFileDialog1.Filter = "EXCEL files (*.xls)|*.xls";
            //SaveFileDialog1.InitialDirectory = "c:\\";
            //DialogResult result = SaveFileDialog1.ShowDialog();
            //if (result != DialogResult.OK)
            //{
            //    result;
            //}

            // get files that contain  土地核查, then delet them
            string path = this.txtPath.Text;
            string[] dirs = null;
            dirs = Directory.GetFiles(this.txtPath.Text + "\\", "土地核查.xls");
            foreach (string dir in dirs)
            {
                File.Delete(dir);
            }
            string excelpath = path + "\\土地核查.xls";


            //string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\temp\aa2.xls" + ";Extended Properties=Excel 8.0;";
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelpath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);
            OleDbCommand objCmd = new OleDbCommand();

            objCmd.Connection = objConn;

            //建立表结构
            objCmd.CommandText = @"CREATE TABLE 土地核查(序号 VarChar(50),图斑编号 VarChar(50),宗地号 VarChar(50),图斑位置 VarChar(50),
                                所在政区 VarChar(50),区划代码 VarChar(50),用地单位或个人 VarChar(50),用地时间 VarChar(50),供地批准机关_批准时间和批准文号 VarChar(50),转征收批准机关_批准时间和批准或批次文号 VarChar(50),供地批准面积_总数 VarChar(50),
                                供地批准面积_耕地 VarChar(50),用地面积_总数 VarChar(50),用地面积_耕地 VarChar(50),规划用途 VarChar(50),批准用途 VarChar(50),实际用途 VarChar(50),供地方式 VarChar(50),是否未报即用 VarChar(50),是否边报边用 VarChar(50),是否未供即用 VarChar(50),
                                是否超占用地 VarChar(50),是否擅自改变用途 VarChar(50),是否非法批地 VarChar(50),是否违法方式供地 VarChar(50),是否其他违法用地 VarChar(50),是否违反土地利用总体规划 VarChar(50),是否单独选址 VarChar(50),是否紧急用地 VarChar(50),
                               是否国家和省级重点工程 VarChar(50),是否占用基本农田 VarChar(50),是否农业结构调整 VarChar(50),是否实地伪变化 VarChar(50),是否动态巡查已经发现违法 VarChar(50),备注 VarChar(200))";
            
            try
            {

                objConn.Open();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            objCmd.ExecuteNonQuery();

            objCmd.Connection = objConn;

            ////获取 数据集 
            //string sql = tdjc.SubFrame.CGGL.DatabaseString.DBConnectString1();

            //string select = "SELECT * FROM 土地核查";
            //System.Data.OleDb.OleDbConnection mycon = new System.Data.OleDb.OleDbConnection(sql);
            //System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(select, mycon);
            //DataSet ds = new DataSet();
            //da.Fill(ds, "土地核查");

            //插入 数据
            //建立插入动作的Command
            objCmd.CommandText = @"INSERT INTO 土地核查(序号,图斑编号,宗地号,图斑位置,所在政区,区划代码,
                                用地单位或个人,用地时间,供地批准机关_批准时间和批准文号,转征收批准机关_批准时间和批准或批次文号,
                                供地批准面积_总数,供地批准面积_耕地,用地面积_总数,用地面积_耕地,规划用途,批准用途,实际用途,
                                供地方式,是否未报即用,是否边报边用,是否未供即用,
                                是否超占用地,是否擅自改变用途,是否非法批地,是否违法方式供地,是否其他违法用地,是否违反土地利用总体规划,
                                是否单独选址,是否紧急用地,是否国家和省级重点工程,是否占用基本农田,是否农业结构调整,是否实地伪变化,是否动态巡查已经发现违法,
                                备注)
                                VALUES(@objectid,@TBBH,@ZDH,@TBWZ,@XZQM,@QHDM,@YDDW,@YDSJ,@GDPZJGWH,@ZZSPZJGWH,@GDPZMJZS,@GDPZMJGD,@YDMJZS,@YDMJGD,@GHYT,@PZYT,
                                @SJYT,@GDFS,@WBJY,@BBBY,@WGJY,@CZYD,@SZGBYT,@FFPD,@WFFSGD,@QTWFYD,@WFTDLYZTGH,@DDXZ,@JJYD,@GJHSJZDGC,@ZYJBNT,@NYJGTZ,@SDWBH,@DTXCYFXWF,@BZ)";

            objCmd.Parameters.Add(new OleDbParameter("@objectid", OleDbType.Integer ));

            objCmd.Parameters.Add(new OleDbParameter("@TBBH", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@YDDW", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@ZDH", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@TBWZ", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@XZQM", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@QHDM", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@YDDW", OleDbType.VarChar));

            objCmd.Parameters.Add(new OleDbParameter("@YDSJ", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@GDPZJGWH", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@ZZSPZJGWH", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@GDPZMJZS", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@GDPZMJGD", OleDbType.VarChar));

            objCmd.Parameters.Add(new OleDbParameter("@YDMJZS", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@YDMJGD", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@SJYT", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@PZYT", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@GDFS", OleDbType.VarChar));

            objCmd.Parameters.Add(new OleDbParameter("@WBJY", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@BBBY", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@WGJY", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@CZYD", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@SZGBYT", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@FFPD", OleDbType.VarChar));

            objCmd.Parameters.Add(new OleDbParameter("@WFFSGD", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@QTWFYD", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@WFTDLYZTGH", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@DDXZ", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@JJYD", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@GJHSJZDGC", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@ZYJBNT", OleDbType.VarChar));

            objCmd.Parameters.Add(new OleDbParameter("@NYJGTZ", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@SDWBH", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@DTXCYFXWF", OleDbType.VarChar));
            objCmd.Parameters.Add(new OleDbParameter("@BZ", OleDbType.VarChar));




            try
            {
                for (int i = 0; i < this.listView1.Items.Count; i++)
                {
                    for (int j = 0; j < this.listView1.Columns.Count; j++)
                    {
                        objCmd.Parameters[j].Value = this.listView1.Items[i].SubItems[j].Text;
                    }
                    objCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                strIsOK = "0";
            }


            objConn.Close();

            if (strIsOK == "0")
                MessageBox.Show("导出失败！", "导出失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("导出成功！\n\n已导出至" + this.txtPath.Text + "\\土地核查.xls", "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }
    }
}