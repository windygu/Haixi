using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Diagnostics;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

using ESRI.ArcGIS.DataSourcesFile;

using ESRI.ArcGIS.esriSystem;
using JCZF.SubFrame.AttibuteEdit;
using JCZF.SubFrame;

namespace JCZF.SubFrame.AttibuteEdit
{
    public partial class FormMenuButton : Form
    {
        //public JCZF.MainFrame.frmMainFrame m_MainFrame;//����������
        public frmMapView m_frmMapView;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl;
        public IFeature m_selFeature;
        public IFeatureClass m_selFeatureclass;
        public int oid;

        public string m_strDKID;

        private JCZF.SubFrame.AttibuteEdit.frmDTXCWFXWBGD m_frmDTXCWFXWBGD;
      

        public string sellayname;

        public string m_strTabelName;
        public clsDataAccess.DataAccess m_DataAccess_SYS;
        public string m_strObjecgID; 

        private int m_intTime = 0;//
        public bool[] m_bEnableWhich; // ��ʾ�ĸ���ť
        public string m_fileContent; // �ֺ�����
        public string m_theAllData; // ��������
        public string m_theColName; // ��������
        private FormShowSmallPic m_theSlideForm1; // ��ʾ��Ƭ����ͼ
        private FormShowSmallPic m_theSlideForm2; // ��ʾƽ������ͼ    
        private FormShowSmallPic m_theSlideForm3; // ��ʾ��������ͼ
        private FormShowSmallPic m_theSlideForm4; // ��ʾ��Ƶ����ͼ

        bool m_bIsPop1; // ����״̬
        bool m_bIsPop2; // ����״̬
        bool m_bIsPop3; // ����״̬
        bool m_bIsPop4; // ����״̬

        public FormMenuButton()
        {
            InitializeComponent();

            // ��ʾ�ĸ���ť
            m_bEnableWhich = new bool[4];
            m_bEnableWhich[0] = true;  // ����
            m_bEnableWhich[1] = true;  // ƽ��ͼ
            m_bEnableWhich[2] = true;  // ����ͼ
            m_bEnableWhich[3] = true;  // ��Ƶ

            // ��������
            m_theSlideForm1 = new FormShowSmallPic(this, 0.1f);
            m_theSlideForm2 = new FormShowSmallPic(this, 0.1f);
            m_theSlideForm3 = new FormShowSmallPic(this, 0.1f);
            m_theSlideForm4 = new FormShowSmallPic(this, 0.1f);

            // ����״̬
            m_bIsPop1 = false;
            m_bIsPop2 = false;
            m_bIsPop3 = false;
            m_bIsPop4 = false;
            


        }

        // ��ʾ�ĸ���ť
        public void EnableButton()
        {
            this.btnPhotos.Enabled = m_bEnableWhich[0];  // ����         
            this.btnVideos.Enabled = m_bEnableWhich[3];  // ��Ƶ
        }

        private void FormMenuButton_Load(object sender, EventArgs e)
        {
            // ��ʾ�ĸ���ť
            EnableButton();
        }    
    
           // �õ��ļ��б�
        private void GetFile(string unicode, string folder, ref ArrayList theFileList)
        {
            theFileList.Clear();
            string path;
            if (folder == "Photos")
            {
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001��Ƭ_1.bmp");
                theFileList.Add(path);
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001��Ƭ_2.bmp");
                theFileList.Add(path);
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001��Ƭ_3.bmp");
                theFileList.Add(path);
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001��Ƭ_4.bmp");
                theFileList.Add(path);
            }
            else if (folder == "Plans")
            {
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001ƽ��ͼ_1.bmp");
                theFileList.Add(path);
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001ƽ��ͼ_2.bmp");
                theFileList.Add(path);

            }
            else if (folder == "Sections")
            {
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001����ͼ_1.bmp");
                theFileList.Add(path);
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001����ͼ_2.bmp");
                theFileList.Add(path);

            }
            else if (folder == "Videos")
            {
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001��Ƶ_1.bmp");
                theFileList.Add(path);
                path = System.IO.Path.Combine(Application.StartupPath, "zh1-001��Ƶ_2.bmp");
                theFileList.Add(path);

            }
        }

        private void GetFile(string m_strID, ref ArrayList theStreamList)
        {
            theStreamList.Clear();

            string m_strSQL = "select MC from DOC where ID LIKE '" + m_strID + "'";

            DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);
            if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
            {
                for (int i = 0; i < m_DataRowCollection.Count; i++)
                {
                    theStreamList.Add(m_DataRowCollection[i][0].ToString());

                    if (IsExist(m_DataRowCollection[i][0].ToString()) == false)
                        CreateFolder(m_strID);                    
                }
            }
        }

        private void GetdkFile(string m_strdkID, ref ArrayList theStreamList,string p_strType)
        {
            try
            {
                theStreamList.Clear();

                string m_strMC;
                string path = Application.StartupPath + "\\" + "HCPV";

                string m_strSQL = "select MC,DOC from DOC where DKID = '" + m_strdkID + "' and type='" + p_strType + "'";

                DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);


                if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
                {

                    
                    for (int i = 0; i < m_DataRowCollection.Count; i++)
                    {
                        theStreamList.Add(m_DataRowCollection[i][0].ToString());

                        m_strMC = m_DataRowCollection[i][0].ToString() ;
                        if (IsExist(m_strMC) == false)
                        //CreateFolder1(m_strdkID);
                        {
                            DownloadMultiFile(m_strMC, (byte[])m_DataRowCollection[i][1], m_strdkID);
                        }
                    }
                }
            }
            catch(SystemException errs)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }



        private bool IsExist(string mc)
        {
            string path = Application.StartupPath + "\\" + "HCPV"+"\\" + mc;

            if (System.IO.File.Exists(path))
                return true;
            else
                return false;

        }

        private void DownloadMultiFile(string p_strMC,byte[] p_DOC,string p_strdkID)
        {
           
            try{
                        //��������ļ���·��
                        string str_tempPath = Application.StartupPath + "\\" + "HCPV";
                        //string str_tempPath = Application.StartupPath+ "\\" + "HCPV";
                        DirectoryInfo m_DirectoryInfo = new DirectoryInfo(str_tempPath);
                        if (m_DirectoryInfo.Exists == false)
                            Directory.CreateDirectory(str_tempPath);

                        //if (p_strdkID.Length > 12) p_strdkID = p_strdkID.Substring(0, 12);//����̫������ʶ��
                        string Save_path = str_tempPath + "\\" +p_strMC;

                        if (System.IO.File.Exists(Save_path))
                        {
                           return ;
                        }

                        System.IO.FileStream fs = new System.IO.FileStream(Save_path, FileMode.CreateNew);
                        BinaryWriter bw = new BinaryWriter(fs);
                        //bw.Write(file1, 0, file1.Length);
                        bw.Write(p_DOC);
                        bw.Close();
                        fs.Close();

                    
            }
            catch (System.Exception errs)
            {
                System.Windows.Forms.MessageBox.Show(this, errs.Message, "������ʾ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }


        }

        private void CreateFolder(string m_strID)
        {
            byte[] file1 = null;
            string strMC = "";

            string m_strSQL = "SELECT * FROM DOC WHERE ID LIKE '" + m_strID + "'";

            DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);

            try
            {
                if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
                {
                    for (int i = 0; i < m_DataRowCollection.Count; i++)
                    {
                        DataRow m_DataRow = m_DataRowCollection[i];
                        strMC = (string)m_DataRow[2]; //m_AssistantFunction.EraseSpace()
                        file1 = (byte[])m_DataRow[3];

                        //��������ļ���·��
                        string str_tempPath = Directory.GetCurrentDirectory() + "\\" + "HCPV";
                        //string str_tempPath = Application.StartupPath+ "\\" + "HCPV";
                        DirectoryInfo m_DirectoryInfo = new DirectoryInfo(str_tempPath);
                        if (m_DirectoryInfo.Exists == false)
                            Directory.CreateDirectory(str_tempPath);

                        string Save_path = str_tempPath + "\\" + strMC;

                        if (System.IO.File.Exists(Save_path))
                        {
                            System.IO.File.Delete(Save_path);
                        }

                        System.IO.FileStream fs = new System.IO.FileStream(Save_path, FileMode.CreateNew);
                        BinaryWriter bw = new BinaryWriter(fs);
                        //bw.Write(file1, 0, file1.Length);
                        bw.Write(file1);
                        bw.Close();
                        fs.Close();

                    }

                }
            }
            catch (System.Exception errs)
            {
                System.Windows.Forms.MessageBox.Show(this, errs.Message, "������ʾ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

         
        }

        private void CreateFolder1(string m_strdkID)
        {
            byte[] file1 = null;
            string strMC = "";

            string m_strSQL = "SELECT MC,DOC FROM DOC WHERE m_strDKID = '" + m_strdkID + "'";

            DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);

            try
            {
                if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
                {
                    for (int i = 0; i < m_DataRowCollection.Count; i++)
                    {
                        DataRow m_DataRow = m_DataRowCollection[i];
                        strMC = (string)m_DataRow["MC"]; //m_AssistantFunction.EraseSpace()
                        file1 = (byte[])m_DataRow["DOC"];

                        //��������ļ���·��
                        string str_tempPath = Application.StartupPath + "\\" + "HCPV";
                        //string str_tempPath = Application.StartupPath+ "\\" + "HCPV";
                        DirectoryInfo m_DirectoryInfo = new DirectoryInfo(str_tempPath);
                        if (m_DirectoryInfo.Exists == false)
                            Directory.CreateDirectory(str_tempPath);

                        string Save_path = str_tempPath + "\\" + strMC;

                        if (System.IO.File.Exists(Save_path))
                        {
                            System.IO.File.Delete(Save_path);
                        }

                        System.IO.FileStream fs = new System.IO.FileStream(Save_path, FileMode.CreateNew);
                        BinaryWriter bw = new BinaryWriter(fs);
                        //bw.Write(file1, 0, file1.Length);
                        bw.Write(file1);
                        bw.Close();
                        fs.Close();

                    }

                }
            }
            catch (System.Exception errs)
            {
                System.Windows.Forms.MessageBox.Show(this, errs.Message, "������ʾ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }


        }


        /// <summary>
        /// ��ȡͼƬ����
        /// </summary>
        /// <param name="id"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        private string GetStrId(string id, string style)
        {
            string m_strID = "";
            if (this.sellayname.IndexOf("���غ˲�") > -1 && style == "p")
            { m_strID = "t" + id + "p"; }
            else if (this.sellayname.IndexOf("���غ˲�") > -1 && style == "v")
            { m_strID = "t" + id + "v"; }

            else if (this.sellayname.IndexOf("����˲�") > -1 && style == "p")
            { m_strID = "k" + id + "p"; }
            else if (this.sellayname.IndexOf("����˲�") > -1 && style == "v")
            { m_strID = "k" + id + "v"; }

            return m_strID;

        }


        private string GetStrdkId(string p_strDKID, string style)
        {
            string m_strdkID = "";
            if (this.sellayname.IndexOf("���غ˲�") > -1 && style == "p")
            { m_strdkID = p_strDKID + "p"; }
            else if (this.sellayname.IndexOf("���غ˲�") > -1 && style == "v")
            { m_strdkID = p_strDKID + "v"; }

            else if (this.sellayname.IndexOf("����˲�") > -1 && style == "p")
            { m_strdkID = p_strDKID + "p"; }
            else if (this.sellayname.IndexOf("����˲�") > -1 && style == "v")
            { m_strdkID = p_strDKID + "v"; }                  

            return m_strdkID;
        }

        // ��ť������
        private void btnProperties_Click(object sender, EventArgs e)
        {
            try
            {
                if (sellayname.IndexOf("���غ˲�") > -1)
                {
                    //JCZF.SubFrame.AttibuteEdit.frmAttributeEdit m_frmAttributeEdit = new JCZF.SubFrame.AttibuteEdit.frmAttributeEdit();
                    //m_frmAttributeEdit.m_DataAccess_SYS = this.m_DataAccess_SYS;
                    //m_frmAttributeEdit.m_strObjectID = this.oid.ToString();
                    //m_frmAttributeEdit.m_strOID = "t" + this.oid.ToString();
                    ////m_frmAttributeEdit.m_strTabelName = this.m_strTabelName;
                    //m_frmAttributeEdit.m_strTabelName = this.sellayname;
                    //m_frmAttributeEdit.m_featureClass = this.m_selFeatureclass;
                    //m_frmAttributeEdit.m_selectedFeature = this.m_selFeature;
                    ////m_frmAttributeEdit.m_Axmapcontrol=this
                    //m_frmAttributeEdit.m_strDKID = this.m_strDKID;

                    //m_frmAttributeEdit.ShowDialog();

                    DTXCWFXWBGD("", this.m_strDKID);
                }
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void DTXCWFXWBGD(string p_strID, string p_strDKID)
        {
            try
            {
                if (m_frmDTXCWFXWBGD == null || m_frmDTXCWFXWBGD.IsDisposed == true)
                {
                    m_frmDTXCWFXWBGD = new JCZF.SubFrame.AttibuteEdit.frmDTXCWFXWBGD();
                    m_frmDTXCWFXWBGD.m_DataAccess_SYS = this.m_DataAccess_SYS;
                }
                m_frmDTXCWFXWBGD.m_strID = p_strID;
                m_frmDTXCWFXWBGD.m_strDKID = p_strDKID;
                m_frmDTXCWFXWBGD.ReadData();

                //m_frmDTXCWFXWBGD.Left =-50;
                //m_frmDTXCWFXWBGD.Top = this.ParentForm.ParentForm.Top + 100;
                //m_frmDTXCWFXWBGD.Parent = this;
                m_frmDTXCWFXWBGD.ShowDialog();
                m_frmDTXCWFXWBGD.BringToFront();
            }
            catch (Exception errs)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this, errs.Message);
            }
        }


        // ��ť����Ƭ
        private void btnPhotos_Click(object sender, EventArgs e)
        {
            try
            {
          
            m_intTime = m_intTime + 1;
           ;
            ArrayList theList = new ArrayList();

            ArrayList thedkList = new ArrayList();

             string m_strID;
             string m_strdkID;
            // if (m_strDKID == "")
            //{
            //    m_strID = GetStrId(this.oid.ToString(), "p");

            //    GetFile(m_strID, ref theList);
            //}
            //else
            //{
                //m_strdkID = GetStrdkId(this.m_strDKID, "p");
             GetdkFile(m_strDKID, ref thedkList,"p");
            //}           

      
            m_theSlideForm1.m_type = "��Ƭ";
            m_theSlideForm1.m_theTitle = "�鿴�����Ƭ";
            m_theSlideForm1.m_theFileList.Clear();
            //m_theSlideForm1.m_theFileList.AddRange(theList); // ���
            m_theSlideForm1.m_theFileList.AddRange(thedkList); // ���

            m_theSlideForm1.DoInitial(); // ��ʼ����������յȣ�
            try
            {
                m_theSlideForm1.ShowIconImage();
            }
                catch (SystemException errs)
            {
                }
            m_theSlideForm1.SlideDirection = SlideDialog.SlideDialog.SLIDE_DIRECTION.RIGHT;

            if (m_bIsPop2)
            {
                m_theSlideForm2.Slide(); // ����
                m_bIsPop2 = false;

                //m_theSlideForm2.Visible = false;
            }
            if (m_bIsPop3)
            {
                m_theSlideForm3.Slide(); // ����
                m_bIsPop3 = false;
                //m_theSlideForm3.Visible = false;
            }
            if (m_bIsPop4)
            {
                m_theSlideForm4.Slide(); // ����
                m_bIsPop4 = false;
                //m_theSlideForm3.Visible = false;
            }

            m_theSlideForm1.Slide();
            m_bIsPop1 = !m_bIsPop1;
            //if (m_intTime == 1)
            //{
            //    m_theSlideForm1.Visible = true;
            //}
            //else
            //{
            //    m_theSlideForm1.Visible = false;
            //    m_intTime = 0;
            //}     
                  }
            catch (System.Exception ex)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this.ParentForm, ex.Message);
            }
        }

        // ��ť��ƽ��ͼ
        private void btnPlans_Click(object sender, EventArgs e)
        {
            ArrayList theList = new ArrayList();

            GetFile(m_fileContent, "Plans", ref theList); // �õ��ļ�
            m_theSlideForm2.m_type = "ƽ��ͼ";
            m_theSlideForm2.m_theTitle = "�鿴���ƽ��ͼ";
            m_theSlideForm2.m_theFileList.Clear();
            m_theSlideForm2.m_theFileList.AddRange(theList); // ���
            m_theSlideForm2.DoInitial(); // ��ʼ����������յȣ�
            m_theSlideForm2.SlideDirection = SlideDialog.SlideDialog.SLIDE_DIRECTION.RIGHT;

            if (m_bIsPop1)
            {
                m_theSlideForm1.Slide();
                m_bIsPop1 = false;
                //m_theSlideForm1.Visible = false;
            }
            if (m_bIsPop3)
            {
                m_theSlideForm3.Slide(); // ����
                m_bIsPop3 = false;
                //m_theSlideForm3.Visible = false;
            }
            if (m_bIsPop4)
            {
                m_theSlideForm4.Slide(); // ����
                m_bIsPop4 = false;
                //m_theSlideForm3.Visible = false;
            }

            m_theSlideForm2.Slide();
            m_bIsPop2 = !m_bIsPop2;
        }

        // ��ť������ͼ
        private void btnSections_Click(object sender, EventArgs e)
        {

            ArrayList theList = new ArrayList();

            GetFile(m_fileContent, "Sections", ref theList); // �õ��ļ�
            m_theSlideForm3.m_type = "����ͼ";
            m_theSlideForm3.m_theTitle = "�鿴�������ͼ";
            m_theSlideForm3.m_theFileList.Clear();
            m_theSlideForm3.m_theFileList.AddRange(theList); // ���
            m_theSlideForm3.DoInitial(); // ��ʼ����������յȣ�
            m_theSlideForm3.SlideDirection = SlideDialog.SlideDialog.SLIDE_DIRECTION.RIGHT;

            if (m_bIsPop1)
            {
                m_theSlideForm1.Slide();
                m_bIsPop1 = false;
                //m_theSlideForm1.Visible = false;
            }
            if (m_bIsPop2)
            {
                m_theSlideForm2.Slide();
                m_bIsPop2 = false;
                //m_theSlideForm2.Visible = false;
            }
            if (m_bIsPop4)
            {
                m_theSlideForm4.Slide(); // ����
                m_bIsPop4 = false;
                //m_theSlideForm3.Visible = false;
            }

            m_theSlideForm3.Slide();
            m_bIsPop3 = !m_bIsPop3;
        }

        // ��ť����Ƶ
        private void btnVideos_Click(object sender, EventArgs e)
        {
            m_intTime = m_intTime + 1;

            ArrayList theList = new ArrayList();

            //GetFile(m_fileContent, "Videos", ref theList); // �õ��ļ�
            //GetStream(this.oid.ToString(), "v", ref theList);

          

             string m_strID;
             string m_strdkID;
            // if (m_strDKID == "")
            //{
            //    m_strID = GetStrId(this.oid.ToString(), "v");

            //    GetFile(m_strID, ref theList);
            //}
            //else
            //{
                //m_strdkID = GetStrdkId(this.m_strDKID, "v");
                GetdkFile(m_strDKID, ref theList, "v");
            //}       




            m_theSlideForm4.m_type = "��Ƶ";
            m_theSlideForm4.m_theTitle = "�鿴�����Ƶ";
            m_theSlideForm4.m_theFileList.Clear();
            m_theSlideForm4.m_theFileList.AddRange(theList); // ���
            m_theSlideForm4.DoInitialVideos(); // ��ʼ����������յȣ�
            m_theSlideForm4.SlideDirection = SlideDialog.SlideDialog.SLIDE_DIRECTION.RIGHT;

            if (m_bIsPop1)
            {
                m_theSlideForm1.Slide();
                m_bIsPop1 = false;
                //m_theSlideForm1.Visible = false;
            }
            if (m_bIsPop2)
            {
                m_theSlideForm2.Slide();
                m_bIsPop2 = false;
                //m_theSlideForm2.Visible = false;
            }
            if (m_bIsPop3)
            {
                m_theSlideForm3.Slide(); // ����
                m_bIsPop3 = false;
                //m_theSlideForm3.Visible = false;
            }

            m_theSlideForm4.Slide();
            m_bIsPop4 = !m_bIsPop4;
        }

        //�˲�
        private void btnHC_Click(object sender, EventArgs e)
        {
           
            if (sellayname.Contains("���غ˲�")==true )
            {
                frmSelAnalysis_hc m_frmSelAnalysis_hc = new frmSelAnalysis_hc(m_AxMapControl,m_frmMapView);

                m_frmSelAnalysis_hc.m_DataAccess_SYS = this.m_DataAccess_SYS;
                m_frmSelAnalysis_hc.m_selFeature = this.m_selFeature;
                m_frmSelAnalysis_hc.m_strDKID = m_strDKID;
                //m_frmSelAnalysis_hc.m_strAnalysisLayerName = sellayname;
                m_frmSelAnalysis_hc.ReadFromResult();
                m_frmSelAnalysis_hc.Show();

                //frmSelAnalysis m_frmSelAnalysis = new frmSelAnalysis(m_AxMapControl);
                //m_frmSelAnalysis.m_DataAccess_SYS = this.m_DataAccess_SYS;
                //m_frmSelAnalysis.m_selFeature = this.m_selFeature;
                //m_frmSelAnalysis.m_strAnalysisLayerName = sellayname;
                //m_frmSelAnalysis.Show();
                //m_frmPGhc.m_selFeatureclass = this.m_selFeatureclass;
                //m_frmPGhc.oid = this.oid;
                //m_frmPGhc.sellayname = this.sellayname;
                //m_frmPGhc.ShowDialog();

                this.Visible = false;
            }
            //else if (sellayname == "����˲�")
            //{
            //    frmKChc m_frmKChc = new frmKChc();
            //    m_frmKChc.m_frmMapView = this.m_frmMapView;
            //    m_frmKChc.m_AxMapControl = this.m_frmMapView.m_axMapControl;
            //    m_frmKChc.m_selFeature = this.m_selFeature;
            //    m_frmKChc.m_selFeatureclass = this.m_selFeatureclass;
            //    m_frmKChc.oid = this.oid;
            //    m_frmKChc.sellayname = this.sellayname;
            //    m_frmKChc.ShowDialog();
            //}
            //}

        }
        //���굼��
        private void btnZbdc_Click(object sender, EventArgs e)
        {
            try
            {
                IFeature m_Feature = this.m_selFeature;

                if (ZuobiaoDC(m_Feature))
                {
                    MessageBox.Show("�����ɹ���","��ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information );

                }
                else
                {
                    //MessageBox.Show("����ʧ�ܣ�");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("����ʧ�ܣ�");
                MessageBox.Show("����ʧ�ܣ�"+ex.Message , "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private bool ZuobiaoDC(IFeature m_Feature)
        {
            try
            {
               
                //����·��          
                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                SaveFileDialog1.CreatePrompt = true;
                SaveFileDialog1.OverwritePrompt = true;
                SaveFileDialog1.FileName = "exportcoord";
                SaveFileDialog1.DefaultExt = "txt";
                SaveFileDialog1.Filter = "Text files (*.txt)|*.txt";
                SaveFileDialog1.InitialDirectory = "c:\\";
                DialogResult result = SaveFileDialog1.ShowDialog();
                System.IO.Stream fileStream;

                if (result == DialogResult.OK)
                {
                    IPolygon m_polygon = new Polygon() as IPolygon;
                    m_polygon = m_Feature.ShapeCopy as IPolygon;
                    IPointCollection m_PointCollection = new Polygon() as IPointCollection;
                    m_PointCollection = m_polygon as IPointCollection;

                    int iCount = m_PointCollection.PointCount;
                    double[,] xy = new double[iCount, 2];
                    for (int i = 0; i < iCount; i++)
                    {
                        //ArrayList arr = new ArrayList();              
                        xy[i, 0] = m_PointCollection.get_Point(i).X;
                        xy[i, 1] = m_PointCollection.get_Point(i).Y;
                    }

                    fileStream = SaveFileDialog1.OpenFile();
                    StreamWriter sw = new StreamWriter(fileStream);
                    sw.Write("6;");                    
                    sw.Write(DateTime.Now.ToString("yyyyMMdd")+";");
                    sw.Write("001;0000;001;");
                    sw.Write((iCount-1).ToString("000"));
                    sw.Write(";");
                    for (int j = 0; j < iCount - 1; j++)
                    {
                        sw.Write(xy[j, 0].ToString("00000000.000"));
                        sw.Write(',');
                        sw.Write(xy[j, 1].ToString("00000000.000"));
                        sw.Write(';');
                        //sw.WriteLine();
                    }
                    //sw.Write(xy[iCount - 2, 0].ToString());
                    //sw.Write(',');
                    //sw.Write(xy[iCount - 2, 1].ToString());

                    sw.Close();
                    fileStream.Close();
                    return true;

                }
                else
                {
                    return false; 
                }
            }
            catch (Exception ex)
            {
                return false;
            }
 
        }

       



    }
}