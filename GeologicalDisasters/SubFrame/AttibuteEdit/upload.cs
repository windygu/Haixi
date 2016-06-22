using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Runtime;

namespace JCZF.SubFrame
{
    public partial class upload : UserControl
    {
        //定义数据库连接
        JCZF.SubFrame.DatabaseString m_DatabaseString = new JCZF.SubFrame.DatabaseString();
        //定义辅助字符串处理
        JCZF.SubFrame.AssistantFunction m_AssistantFunction = new JCZF.SubFrame.AssistantFunction();

        public clsDataAccess.DataAccess m_DataAccess_SYS;

        private DataSet aa = new DataSet();
        public string imageFileLocation;
        OleDbConnection objConnection = null;
        OleDbCommand imageCommand = null;
        Point mousePos = new Point(0, 0);
        public string imageFileID = "";
        
        public string TableName = "";

        public string strDKID = "";
        public string strID = "";
        public string strName = "";
        public string strType = "";
        public string strTime = "";
        public string strWriter = "";
        public string strCompany = "";
        public string strNotice = "";
        public string strLj = "";
        public string strMutiFileType = "";

        int commandResult;

        public upload()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 打开连接函数
        /// </summary>
        public void openConnection()
        {

            try
            {
                //string strConnection = JCZF.SubFrame.DatabaseString.DBConnectString2();

                objConnection = m_DataAccess_SYS.DBConnection_OleDb;

                objConnection.Open();
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.Message);
            }

        }

        /// <summary>
        /// 关闭连接函数
        /// </summary>
        public void closeConnection()
        {
            //close connection
            objConnection.Close();
            Console.WriteLine("Connection Successfully Colsed!");
        }

        /// <summary>
        /// 创建数据库操作函数
        /// </summary>
        public void createCommand()
        {
            imageCommand = new OleDbCommand();
            imageCommand.Connection = objConnection;
        }

        /// <summary>
        /// 执行数据库操作函数
        /// </summary>
        /// <param name="commandText"></param>
        void ExecuteCommand(string commandText)
        {
            if (imageCommand.Connection.State != ConnectionState.Open)
            {
                imageCommand.Connection.Open();
            }
            commandResult = 0;
            imageCommand.CommandText = commandText;
            Console.WriteLine("Executing Command:");
            Console.WriteLine(imageCommand.CommandText);
            commandResult = imageCommand.ExecuteNonQuery();
            Console.WriteLine("ExecuteNonQuery returns {0}.",commandResult);
        }

        /// <summary>
        /// 打开图件函数
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="maxImageSize"></param>
        /// <returns></returns>
        byte[] loadImageFile(string fileLocation, int maxImageSize)
        {
            FileStream fs=null ;
            BinaryReader br = null;
            try
            {
            
            byte[] imagebytes = null;
            string fullpath = fileLocation;
            Console.WriteLine("Load File: ");
            Console.WriteLine(fileLocation);
             fs = new FileStream(fileLocation, FileMode.Open);
            br = new BinaryReader(fs);
            imagebytes = br.ReadBytes(maxImageSize);
            Console.WriteLine("Imagebytes has length {0} bytes.", imagebytes.GetLength(0));
            br.Close();
            fs.Close();

            return imagebytes;
            }
            catch (System.Exception ex)
            {
                if (br != null) br.Close();
                if (fs != null) fs.Close();
                m_DataAccess_SYS.MessageErrorInforShow(this.ParentForm, ex.Message);
                
            }
            return null;
        }

        /// <summary>
        /// 读取图件到byte[]数据结构中
        /// </summary>
        /// <param name="imageFileLocation"></param>
        /// <param name="maxImageSize"></param>
        public bool ExecuteInsertImages(string imageFileLocation, int maxImageSize)
        {
            //string imageFilwName = null;

            try
            {
                imageCommand = null;
                imageCommand = new OleDbCommand ();
                imageCommand.CommandType = CommandType.Text;

                imageCommand.Connection = m_DataAccess_SYS.DBConnection_OleDb ;

                byte[] imageImageData = null;
                imageImageData = loadImageFile(imageFileLocation, maxImageSize);

                //imageCommand.CommandText = "INSERT INTO DOC (ID) VALUES ('" + strID + "')";


                imageCommand.CommandText = "INSERT INTO DOC (ID,DKID,MC,DOC,WRITER,SHIJIAN,DANWEI,NOTE,TYPE) VALUES (?,?,?,?,?,?,?,?,?)";
                imageCommand.Parameters.Add("@ID", OleDbType.VarChar);
                imageCommand.Parameters.Add("@DKID", OleDbType.VarChar);
                imageCommand.Parameters.Add("@MC", OleDbType.VarChar);
                imageCommand.Parameters.Add("@DOC", OleDbType.LongVarBinary);
                imageCommand.Parameters.Add("@WRITER", OleDbType.VarChar);
                imageCommand.Parameters.Add("@SHIJIAN", OleDbType.VarChar);
                imageCommand.Parameters.Add("@DANWEI", OleDbType.VarChar);
                imageCommand.Parameters.Add("@NOTE", OleDbType.VarChar);
                imageCommand.Parameters.Add("@TYPE", OleDbType.VarChar);

                imageCommand.Parameters["@ID"].Value = strID;
                imageCommand.Parameters["@DKID"].Value = strDKID;
                imageCommand.Parameters["@MC"].Value = strName;
                imageCommand.Parameters["@DOC"].Value = imageImageData;
                imageCommand.Parameters["@WRITER"].Value = strWriter;
                imageCommand.Parameters["@SHIJIAN"].Value = strTime;
                imageCommand.Parameters["@DANWEI"].Value = strCompany;
                imageCommand.Parameters["@NOTE"].Value = strNotice;
                imageCommand.Parameters["@TYPE"].Value = strMutiFileType;

                
                //imageCommand.CommandText = "INSERT INTO DOC (ID,DKID,MC,DOC,WRITER,SHIJIAN,DANWEI,NOTE) VALUES (?,?,?,?,?,?,?,?)";
                //imageCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("?", System.Data.OleDb.OleDbType.VarChar)).Value = strID;
                //imageCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("?", System.Data.OleDb.OleDbType.VarChar, 50)).Value = strDKID;
                //imageCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("?", System.Data.OleDb.OleDbType.VarChar)).Value = strName;
                //imageCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("?", SqlDbType.Image)).Value = imageImageData;
                //imageCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("?", System.Data.OleDb.OleDbType.VarChar)).Value = strWriter;
                //imageCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("?", System.Data.OleDb.OleDbType.VarChar)).Value = strTime;
                //imageCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("?", System.Data.OleDb.OleDbType.VarChar)).Value = strCompany;
                //imageCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("?", System.Data.OleDb.OleDbType.VarChar)).Value = strNotice;

               


               // ececuteCommand(imageCommand.CommandText);

                if (imageCommand.Connection.State != ConnectionState.Open)
                {
                    imageCommand.Connection.Open();
                }
                commandResult = 0;               
               
                commandResult = imageCommand.ExecuteNonQuery();

                return true;
            }
            catch (SystemException errs )
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
            return false;
        }

        public void prepareInsertImages()
        {
            imageCommand.CommandText = "INSERT INTO DOC (ID,DKID,MC,DOC,WRITER,SHIJIAN,DANWEI,NOTE) VALUES (?,?,?,?,?,?,?,?)";
            imageCommand.Parameters.Add("@ID", OleDbType.VarChar);
            imageCommand.Parameters.Add("@DKID", OleDbType.VarChar);
            imageCommand.Parameters.Add("@MC", OleDbType.VarChar);
            imageCommand.Parameters.Add("@DOC", SqlDbType.Image);
            imageCommand.Parameters.Add("@WRITER", OleDbType.VarChar);
            imageCommand.Parameters.Add("@SHIJIAN", OleDbType.VarChar);
            imageCommand.Parameters.Add("@DANWEI", OleDbType.VarChar);
            imageCommand.Parameters.Add("@NOTE", OleDbType.VarChar);

        }

        public bool HasUpload()
        {
            string m_strSQL = "";
            m_strSQL = "SELECT DOCID FROM DOC WHERE MC='" +strName+ "' AND  dkid ='" + strDKID + "'";
            DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);
            if (m_DataTable == null || m_DataTable.Rows.Count < 1)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// listview编辑操作函数－记录鼠标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ltvGHCG_MouseMove(object sender, MouseEventArgs e)
        {
            ////record mouse position
            //mousePos.X = e.X;
            //mousePos.Y = e.Y;
        }

        private void ltvGHCG_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ////found clicked item
            //ListViewItem item = ltvGHCG.GetItemAt(mousePos.X, mousePos.Y); //获取鼠标所点击在哪一列上
            ////locate text box
            //Rectangle rect = item.GetBounds(ItemBoundsPortion.Entire);
            //int StartX = rect.Left;
            //ColumnIndex = 0;
            ////get ColumnIndex
            //foreach (ColumnHeader Column in ltvGHCG.Columns)
            //{
            //    if (mousePos.X >= StartX + Column.Width)
            //    {
            //        StartX += Column.Width;
            //        ColumnIndex += 1;
            //    }
            //}

            ////locate the txtinput and hide it. txtInput为TextBox
            //txtInput.Parent = ltvGHCG;
            ////begin edit
            //if (item != null)
            //{
            //    rect.X = StartX;
            //    rect.Width = ltvGHCG.Columns[ColumnIndex].Width; //得到长度和ListView的列的长度相同
            //    txtInput.Bounds = rect;
            //    //show textbox
            //    txtInput.Text = item.SubItems[ColumnIndex].Text;
            //    pub_item = item.SubItems[ColumnIndex];
            //    txtInput.Visible = true;
            //    txtInput.Focus();
            //}
        }

        /// <summary>
        /// listview编辑操作函数－编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ltvGHCG_MouseClick(object sender, MouseEventArgs e)
        {
            //if (txtInput.Text !="" && txtInput.Visible == true)
            //{
            //    pub_item.Text = this.txtInput.Text;

            //    this.ltvGHCG.Refresh();

            //    txtInput.Text = "";

            //    txtInput.Visible = false;
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (this.button2.Enabled == false)
            //{
            //    this.ltvGHCG.Items.Clear();
            //    this.button3.Text = "删　除";
            //    this.button3.Enabled = false;
            //    this.button2.Enabled = false;
            //    //this.openWksFileDialog.
            //}
            //else
            //{
            //    for (int i = 0; i < this.ltvGHCG.Items.Count; i++)
            //    {
            //        if (ltvGHCG.Items[i].Checked == true)
            //            this.ltvGHCG.Items[i].Remove();
            //    }
            //    ltvGHCG.Refresh();
            //    for (int j = 0; j< this.ltvGHCG.Items.Count; j++)
            //    {
            //        ltvGHCG.Items[j].SubItems[0].Text = (j+1).ToString();

            //    }
            //    ltvGHCG.Refresh();
            //}
        }

        private void upload_Load(object sender, EventArgs e)
        {
            //if (this.ltvGHCG.Items.Count == 0)
            //{
            //    this.button2.Enabled  = false;
            //    this.button3.Enabled = false;
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            //if (this.checkBox1.Checked == true)
            //{
            //    for (int i = 0; i < this.ltvGHCG.Items.Count; i++)
            //    {
            //        ltvGHCG.Items[i].Checked = true;
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < this.ltvGHCG.Items.Count; i++)
            //    {
            //        ltvGHCG.Items[i].Checked = false;
            //    }
            //}
        }

        private void InitializeComponent()
        {
            //this.SuspendLayout();
            //// 
            //// upload
            //// 
            //this.Name = "upload";
            //this.Load += new System.EventHandler(this.upload_Load_1);
            //this.ResumeLayout(false);

        }


    }
}
