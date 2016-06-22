using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using clsDataAccess;

namespace JCZF.SubFrame.AttibuteEdit
{
    public partial class frmDTXCWFXWBGD : Form
    {
        public DataAccess m_DataAccess_SYS;

        private string m_strID_;
        public string m_strID
        {
            set
            {
                m_strID_ = value;
            }
        }

        private string m_strDKID_;
        public string m_strDKID
        {
            set
            {
                m_strDKID_ = value;
            }
        }

        

        public frmDTXCWFXWBGD()
        {
            InitializeComponent();
        }

        private void frmDTXCWFXWBGD_Load(object sender, EventArgs e)
        {
           
        }

        public  void ReadData()
        {
            if ((m_strID_.Trim() != "" || m_strDKID_.Trim() != "")&& m_DataAccess_SYS != null)
            {



                string m_strSQL = "SELECT * FROM DCJG WHERE DKID='" + m_strDKID_+"'";
                //string m_strSQL = "SELECT * FROM DCJG WHERE DKID='" + m_strDKID_ + "'";
                DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);

                if (m_DataTable != null && m_DataTable.Rows.Count > 0)
                {
                    try
                    {
                        txtMJ.Text = m_DataTable.Rows[0]["MJ"] == DBNull.Value ? "" : m_DataTable.Rows[0]["MJ"].ToString();
                        txtWFZT.Text = m_DataTable.Rows[0]["WFZT"] == DBNull.Value ? "" : m_DataTable.Rows[0]["WFZT"].ToString();

                        txtXMMC.Text = m_DataTable.Rows[0]["XMMC"] == DBNull.Value ? "" : m_DataTable.Rows[0]["XMMC"].ToString();
                        txtYDWZ.Text = m_DataTable.Rows[0]["YDWZ"] == DBNull.Value ? "" : m_DataTable.Rows[0]["YDWZ"].ToString();

                        rtb_DYZRRYJ.Text = m_DataTable.Rows[0]["DYZRRYJ"] == DBNull.Value ? "" : m_DataTable.Rows[0]["DYZRRYJ"].ToString();
                        rtb_SPHSGJZQK.Text = m_DataTable.Rows[0]["SPHSGJZQK"] == DBNull.Value ? "" : m_DataTable.Rows[0]["SPHSGJZQK"].ToString();
                        rtb_XCRYJ.Text = m_DataTable.Rows[0]["XCRYJ"] == DBNull.Value ? "" : m_DataTable.Rows[0]["XCRYJ"].ToString();

                        cmbWFXWLX.Text  = m_DataTable.Rows[0]["WFXWLX"] == DBNull.Value ? "" : m_DataTable.Rows[0]["WFXWLX"].ToString();
                    }
                    catch (Exception errs)
                    {
                        m_DataAccess_SYS.MessageInforShow(this, errs.Message);
                    }
                }

                else
                {
                    ClearText();
                }

            }
            else
            {
                m_DataAccess_SYS.MessageInforShow(this, "未传入数据！");
            }

        }

        private void ClearText()
        {
            txtMJ.Text = "";
            txtWFZT.Text = "";
            txtXMMC.Text = "";
            txtYDWZ.Text = "";
            cmbWFXWLX.Text = "";
             rtb_DYZRRYJ.Text ="";
            rtb_SPHSGJZQK.Text="";
            rtb_XCRYJ.Text="";
        }



        private void SaveData()
        {
            string m_strSQL = "update DCJG set ";
            string m_strSQLWhere = " WHERE dkid='" + m_strDKID_+"'";

            try
            {
                m_strSQL = m_strSQL + " wfzt='" + txtWFZT.Text.Trim() + "'";
               
                m_strSQL = m_strSQL + ", XMMC='" + txtXMMC.Text.Trim() + "'";
                m_strSQL = m_strSQL + ", YT='" + txtYT.Text.Trim() + "'";
                m_strSQL = m_strSQL + ", YDWZ='" + txtYDWZ.Text.Trim() + "'";
                m_strSQL = m_strSQL + ", mj=" + txtMJ.Text.Trim() + "";
               
                    m_strSQL = m_strSQL + ", WFXWLX='" + cmbWFXWLX.Text.Trim() + "'";               

                m_strSQL = m_strSQL + ", DYZRRYJ='" + rtb_DYZRRYJ.Text.Trim() + "'";

                m_strSQL = m_strSQL + ", SPHSGJZQK='" + rtb_SPHSGJZQK.Text.Trim() + "'";
                m_strSQL = m_strSQL + ", XCRYJ='" + rtb_XCRYJ.Text.Trim() + "'";

                m_strSQL = m_strSQL + m_strSQLWhere;

                m_DataAccess_SYS.ExecuteSQLNoReturn(m_strSQL);


            }
            catch (Exception errs)
            {
                m_DataAccess_SYS.MessageInforShow(this, errs.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if( (m_strID_.Trim() != "" || m_strDKID_.Trim() != "") && m_DataAccess_SYS != null)
            {
                SaveData();
            }
            else
            {
                m_DataAccess_SYS.MessageInforShow(this, "未获得数据库相关数据！");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            uctPicture1.Dock = DockStyle.Fill;
            uctPicture1.m_DataAccess_SYS = this.m_DataAccess_SYS;
            uctPicture1.m_strDKID = this.m_strDKID_;
            uctPicture1.ReadData();
            uctPicture1.Visible = true;

        }

        private void frmDTXCWFXWBGD_Shown(object sender, EventArgs e)
        {
           
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print.Print_DTXCWFXWBGD m_Print_DTXCWFXWBGD = new Print.Print_DTXCWFXWBGD();

            m_Print_DTXCWFXWBGD.m_intFontSize = 12;
            m_Print_DTXCWFXWBGD.m_intLineSpace = 36;

            m_Print_DTXCWFXWBGD.m_intTitleFontSize = 22;
            m_Print_DTXCWFXWBGD.m_intTitleLineSpace = 60;

            //m_Print_HC_Attribute.m_strFontName = "";
            //m_Print_DTXCWFXWBGD.m_PrintTextLines = m_PrintTextLines;

            //m_Print_HC_Attribute.OnFilePrintPreview();

            m_Print_DTXCWFXWBGD.m_strTitle = "动态巡查违法行为报告单";

            m_Print_DTXCWFXWBGD.m_strXCDW = txtXCDW.Text;
            m_Print_DTXCWFXWBGD.m_strSJ = txtSJ.Text;
            m_Print_DTXCWFXWBGD.m_strWFZT = txtWFZT.Text;
            m_Print_DTXCWFXWBGD.m_strXMMC = txtXMMC.Text;
            m_Print_DTXCWFXWBGD.m_strYDWZ = txtYDWZ.Text;
            m_Print_DTXCWFXWBGD.m_strWFXWLX = cmbWFXWLX.Text;
            m_Print_DTXCWFXWBGD.m_strMJ = txtMJ.Text;
            m_Print_DTXCWFXWBGD.m_strSPHSGJZQK = rtb_SPHSGJZQK.Text;
            m_Print_DTXCWFXWBGD.m_strXCRYJ = rtb_XCRYJ.Text;
            m_Print_DTXCWFXWBGD.m_strDYZRRYJ = rtb_DYZRRYJ.Text;
            m_Print_DTXCWFXWBGD.m_strYT = txtYT.Text;

            m_Print_DTXCWFXWBGD.OnFilePrint();
        }

       




    }
}
