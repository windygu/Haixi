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

        //����
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtPath.Text == "")
            {
                MessageBox.Show("ѡ�񵼳�·����");
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

            // get files that contain  ���غ˲�, then delet them
            string path = this.txtPath.Text;
            string[] dirs = null;
            dirs = Directory.GetFiles(this.txtPath.Text + "\\", "���غ˲�.xls");
            foreach (string dir in dirs)
            {
                File.Delete(dir);
            }
            string excelpath = path + "\\���غ˲�.xls";


            //string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\temp\aa2.xls" + ";Extended Properties=Excel 8.0;";
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelpath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);
            OleDbCommand objCmd = new OleDbCommand();

            objCmd.Connection = objConn;

            //������ṹ
            objCmd.CommandText = @"CREATE TABLE ���غ˲�(��� VarChar(50),ͼ�߱�� VarChar(50),�ڵغ� VarChar(50),ͼ��λ�� VarChar(50),
                                �������� VarChar(50),�������� VarChar(50),�õص�λ����� VarChar(50),�õ�ʱ�� VarChar(50),������׼����_��׼ʱ�����׼�ĺ� VarChar(50),ת������׼����_��׼ʱ�����׼�������ĺ� VarChar(50),������׼���_���� VarChar(50),
                                ������׼���_���� VarChar(50),�õ����_���� VarChar(50),�õ����_���� VarChar(50),�滮��; VarChar(50),��׼��; VarChar(50),ʵ����; VarChar(50),���ط�ʽ VarChar(50),�Ƿ�δ������ VarChar(50),�Ƿ�߱����� VarChar(50),�Ƿ�δ������ VarChar(50),
                                �Ƿ�ռ�õ� VarChar(50),�Ƿ����Ըı���; VarChar(50),�Ƿ�Ƿ����� VarChar(50),�Ƿ�Υ����ʽ���� VarChar(50),�Ƿ�����Υ���õ� VarChar(50),�Ƿ�Υ��������������滮 VarChar(50),�Ƿ񵥶�ѡַ VarChar(50),�Ƿ�����õ� VarChar(50),
                               �Ƿ���Һ�ʡ���ص㹤�� VarChar(50),�Ƿ�ռ�û���ũ�� VarChar(50),�Ƿ�ũҵ�ṹ���� VarChar(50),�Ƿ�ʵ��α�仯 VarChar(50),�Ƿ�̬Ѳ���Ѿ�����Υ�� VarChar(50),��ע VarChar(200))";
            
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

            ////��ȡ ���ݼ� 
            //string sql = tdjc.SubFrame.CGGL.DatabaseString.DBConnectString1();

            //string select = "SELECT * FROM ���غ˲�";
            //System.Data.OleDb.OleDbConnection mycon = new System.Data.OleDb.OleDbConnection(sql);
            //System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(select, mycon);
            //DataSet ds = new DataSet();
            //da.Fill(ds, "���غ˲�");

            //���� ����
            //�������붯����Command
            objCmd.CommandText = @"INSERT INTO ���غ˲�(���,ͼ�߱��,�ڵغ�,ͼ��λ��,��������,��������,
                                �õص�λ�����,�õ�ʱ��,������׼����_��׼ʱ�����׼�ĺ�,ת������׼����_��׼ʱ�����׼�������ĺ�,
                                ������׼���_����,������׼���_����,�õ����_����,�õ����_����,�滮��;,��׼��;,ʵ����;,
                                ���ط�ʽ,�Ƿ�δ������,�Ƿ�߱�����,�Ƿ�δ������,
                                �Ƿ�ռ�õ�,�Ƿ����Ըı���;,�Ƿ�Ƿ�����,�Ƿ�Υ����ʽ����,�Ƿ�����Υ���õ�,�Ƿ�Υ��������������滮,
                                �Ƿ񵥶�ѡַ,�Ƿ�����õ�,�Ƿ���Һ�ʡ���ص㹤��,�Ƿ�ռ�û���ũ��,�Ƿ�ũҵ�ṹ����,�Ƿ�ʵ��α�仯,�Ƿ�̬Ѳ���Ѿ�����Υ��,
                                ��ע)
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
                MessageBox.Show("����ʧ�ܣ�", "����ʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("�����ɹ���\n\n�ѵ�����" + this.txtPath.Text + "\\���غ˲�.xls", "�����ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }
    }
}