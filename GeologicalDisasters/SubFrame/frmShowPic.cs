using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace JCZF.SubFrame
{
    public partial class frmShowPic : DevComponents.DotNetBar.Office2007Form
    {

        public ArrayList m_theFileList = new ArrayList(); // �ļ��б�

        public ArrayList m_theFileList1 = new ArrayList(); // �ļ��б�
        private frmPictureView m_frmPictureView = new frmPictureView();
        public clsDataAccess.DataAccess m_DataAccess_SYS;

        private string FileName = "";
        public int m_Oid;

        public string strdkid;

        public frmShowPic()
        {
            InitializeComponent();
            this.panelEx1.Controls.Add(m_frmPictureView);
            m_frmPictureView.Dock = DockStyle.Fill;
        }

        // ��ʼ������-������Ƶ
        public void DoInitial()
        {
            // ��ʾ�������
            //   this.Text = m_theTitle;
            try
            {
                listView1.Clear(); // ���listview

                // ���ͼƬ
                int count = m_theFileList.Count;

                if (count == 0) // û���򷵻أ�������û�У�
                    return;

                // ���ͼƬ����
                //string str;
                for (int i = 0; i < count; i++)
                {
                    string filename = System.IO.Path.GetFileNameWithoutExtension((string)m_theFileList[i]);
                    //str = String.Format("{0}", filename);
                    ListViewItem item = new ListViewItem(filename, i);
                    listView1.Items.Add(item);
                }

                //m_bIsVideo = false; // ������Ƶ
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Extraction failed");
            }

        }


        // ��ʼ������-��Ƶ
        public void DoInitialVideos()
        {         
            try
            {
                this.listView2 .Clear(); // ���listview

                // ���ͼƬ
                int count = m_theFileList1.Count;

                if (count == 0) // û���򷵻أ�������û�У�
                    return;

                // ���ͼƬ����
                //string str;
                for (int i = 0; i < count; i++)
                {
                    string filename = System.IO.Path.GetFileNameWithoutExtension((string)m_theFileList1[i]);
                    //str = String.Format("{0}", filename);
                    ListViewItem item = new ListViewItem(filename, i);
                    listView2.Items.Add(item);
                }          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Extraction failed");
            }
            //m_bIsVideo = true; // ����Ƶ         
        }



        public void ShowIconImage()
        {
            ImageList imageListLarge = new ImageList();

            // ����ͼƬ��С
            imageListLarge.ImageSize = new System.Drawing.Size(108, 108);

            // ����ͼƬ·����ȡͼƬ
            for (int i = 0; i < m_theFileList.Count; i++)
            {
                string str_tempPath = System.IO.Directory.GetCurrentDirectory() + "\\" + "HCPV";
                string strMC = (string)m_theFileList[i];
                string strLJ = str_tempPath + "\\" + strMC;
                imageListLarge.Images.Add(Image.FromFile(@strLJ));
            }

            listView1.View = View.LargeIcon;

            listView1.LargeImageList = imageListLarge;
        }



        private void listView1_DoubleClick(object sender, EventArgs e)
        {
             if (listView1.SelectedItems.Count > 0)
            {
                 string str = (string)m_theFileList[listView1.SelectedItems[0].Index];
                 string str_tempPath = System.IO.Directory.GetCurrentDirectory() + "\\" + "HCPV";
                 //string str_tempPath = Application.StartupPath +"\\" + "HCPV";
                 string strLJ = str_tempPath + "\\" + str;
                 this.m_frmPictureView.axAutoVueX1.SRC = strLJ;
                 
             }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            FileName = this.openFileDialog1.FileName.ToString();
            this.textBox1.Text = FileName;
        }

        private void m_btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string DocName1 = DocRecog(FileName);
                string DocName = EraseSpace(DocName1);
                string DocTypeName = DocType(FileName);//�ļ���׺

                string strID = "";
                string strName= DocName;   //�ļ�����
                string  strWriter= this.textBox2 .Text;
                string strTime= this.dateTimePicker1.Text;
                string  strCompany = this.textBox3 .Text;
                string  strNotice=this.textBox4 .Text;
                //m_upload.strLj = this.textBox1.Text;

                if (DocTypeName == "bmp" || DocTypeName == "JPG" || DocTypeName == "ico" || DocTypeName == "gif" || DocTypeName == "jpg")
                    strID = "t"+m_Oid + "p";
                else
                    if (DocTypeName == "mp3" || DocTypeName == "avi" || DocTypeName == "rmvb" || DocTypeName == "AVI" || DocTypeName=="MPG"||DocTypeName=="mpg")
                        strID = "t" + this.m_Oid + "v";
               if( Upload(strID, strName, strWriter, strTime, strCompany, strNotice))
               {
                    MessageBox.Show("����ɹ���");
               }                
                      
            }
            catch (Exception ex)
            {
                MessageBox.Show("����Ϊ�ɹ���");
            }
        }

      

        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        private bool Upload(string strID, string strName, string strWriter, string strTime, string strCompany, string strNotice)
        {
            byte[] imageImageData = null;
            imageImageData = loadImageFile(this.textBox1.Text);
            //imageImageData = loadImageFile("d:\\", 10000000);

            string strConnection = "user id=" + m_DataAccess_SYS.UserID + ";password=" + m_DataAccess_SYS.Password + ";";
            strConnection += "initial catalog=" + m_DataAccess_SYS.DataBase + ";Server=" + m_DataAccess_SYS.Server + ";";
            strConnection += "Connect Timeout=30";

            MySql.Data.MySqlClient.MySqlConnection sqlCon = new MySql.Data.MySqlClient.MySqlConnection(strConnection);
            sqlCon.Open();
            MySql.Data.MySqlClient.MySqlCommand sqlCom = new MySql.Data.MySqlClient.MySqlCommand();
            sqlCom.Connection = sqlCon;
            //sqlCom.CommandText = "INSERT INTO DOC VALUES (@ID,@MC,@DOC,@WRITER,@SHIJIAN,@DANWEI,@NOTE)";

            //sqlCom.Parameters.Add("@ID", MySql.Data.MySqlClient.MySqlDbType.VarChar);
            //sqlCom.Parameters.Add("@MC", MySql.Data.MySqlClient.MySqlDbType.VarChar);
            //sqlCom.Parameters.Add("@DOC", SqlDbType.Image);;
            //sqlCom.Parameters.Add("@WRITER", MySql.Data.MySqlClient.MySqlDbType.VarChar);
            //sqlCom.Parameters.Add("@SHIJIAN",MySql.Data.MySqlClient.MySqlDbType.VarChar);
            //sqlCom.Parameters.Add("@DANWEI", MySql.Data.MySqlClient.MySqlDbType.VarChar);
            //sqlCom.Parameters.Add("@NOTE",MySql.Data.MySqlClient.MySqlDbType.VarChar);           

            //sqlCom.Parameters["@ID"].Value = strID; 
            //sqlCom.Parameters["@MC"].Value = strName;
            //sqlCom.Parameters["@DOC"].Value = imageImageData;
            //sqlCom.Parameters["@WRITER"].Value = strWriter;
            //sqlCom.Parameters["@SHIJIAN"].Value = strTime;
            //sqlCom.Parameters["@DANWEI"].Value = strCompany;
            //sqlCom.Parameters["@NOTE"].Value = strNotice;

            //sqlCom.CommandText = "INSERT INTO DOC VALUES (@ID,@MC,@DOC,@WRITER,@SHIJIAN,@DANWEI,@NOTE)";
            sqlCom.CommandText = "INSERT INTO DOC VALUES (?,?,?,?,?,?,?)";
            sqlCom.Parameters.Add("?", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = strID;
            sqlCom.Parameters.Add("?", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = strName;
            sqlCom.Parameters.Add("?", SqlDbType.Image).Value = imageImageData;
            sqlCom.Parameters.Add("?", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = strWriter;
            sqlCom.Parameters.Add("?", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = strTime;
            sqlCom.Parameters.Add("?", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = strCompany;
            sqlCom.Parameters.Add("?", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = strNotice;          

            sqlCom.ExecuteNonQuery();
            sqlCon.Close();
            return true;
        }

        /// <summary>
        /// ��ͼ������
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="maxImageSize"></param>
        /// <returns></returns>
        byte[] loadImageFile(string fileLocation)
        {
            byte[] imagebytes = null;
            string fullpath = fileLocation;
            Console.WriteLine("Load File: ");
            Console.WriteLine(fileLocation);
            FileStream fs = new FileStream(fileLocation, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            imagebytes = br.ReadBytes(Convert.ToInt32(fs.Length));
            Console.WriteLine("Imagebytes has length {0} bytes.", imagebytes.GetLength(0));
            return imagebytes;
        }


        /// <summary>
        /// ����ַ���
        /// </summary>
        /// <param name="motherString"></param>
        /// <returns></returns>
        public string DocRecog(string motherString)
        {
            int position = motherString.LastIndexOf("\\");
            string result = motherString.Substring(position + 1);
            return result;
        }
        /// <summary>
        /// ɾ���ַ����еĿո�
        /// </summary>
        /// <param name="motherString"></param>
        /// <returns></returns>
        public string EraseSpace(string motherString)
        {
            string result = motherString.Trim();
            return result;
        }

        /// <summary>
        /// �ڴ���׺�����ļ�����ȡ�ļ���׺��
        /// </summary>
        /// <param name="motherString"></param>
        /// <returns></returns>
        public string DocType(string motherString)
        {
            int position = motherString.LastIndexOf(".");
            if (position == -1)
                return "";
            else
            {
                string result = EraseSpace(motherString.Substring(position + 1).Trim());
                return result;
            }
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            string str = (string)m_theFileList1[listView2.SelectedItems[0].Index]; // ��ȡ�ļ���
            //str = System.IO.Path.GetFileNameWithoutExtension(str) + ".AVI";
            AttibuteEdit.FormPlayer theForm = new AttibuteEdit.FormPlayer();

            theForm.Text = str;
            theForm.Show();

            string str_tempPath = Directory.GetCurrentDirectory() + "\\" + "HCPV";
            string strLJ = str_tempPath + "\\" + str;

            theForm.PlayTheVideo(strLJ); // ������Ƶ
        }


        

    }
}