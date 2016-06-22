using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

//using Crystal_Report;

using System.Data.OleDb;



namespace JCZF.SubFrame
{
    public partial class frmDQBJTB : DevComponents.DotNetBar.Office2007Form
    {
        private DataTable m_DataTable;
        private string zqm;
        private string qhdm;

        public string hcly;
        public string ndsy;

        public clsDataAccess.DataAccess m_DataAccess_SYS ;

        public frmDQBJTB(DataTable m_DataTable, string zqm, string qhdm)
        {
            InitializeComponent();
            this.m_DataTable = m_DataTable;
            this.zqm = zqm;
            this.qhdm=qhdm;

        }



        //----------------------------柱状图
        private void createBar(DataTable m_DataTable,string zqm,string qhdm)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zedGraphControl1.GraphPane;

            //FontSpec m_FontSpec = new FontSpec();
            //m_FontSpec.Size = 6;
            //zedGraphControl1.Font.Size  = 8;
            myPane.Title.FontSpec.Size = 12;
            // Set the Titles
            myPane.Title.Text=this.Text ;
            myPane.XAxis.Title.Text="城市";
            myPane.YAxis.Title.Text="数量";

            myPane.XAxis.Title.FontSpec.Size = 8;
            myPane.YAxis.Title.FontSpec.Size = 8;
            string[] labels = new string[m_DataTable.Rows.Count];

            int countofcol = m_DataTable.Columns.Count;
            if (qhdm =="21")
            {
                for (int j = 0; j < m_DataTable.Rows.Count; j++)
                {

                    labels[j] =getXZQM(m_DataTable.Rows[j][countofcol-1].ToString(),"市");
                }

                countofcol = countofcol - 1;
            }
            else
            {
                for (int j = 0; j < m_DataTable.Rows.Count; j++)
                {
                    labels[j] = getXZQM(m_DataTable.Rows[j][countofcol - 1].ToString(), "县");
                }

                countofcol = countofcol - 1;
            }
            double[] list = new double[m_DataTable.Rows.Count];

            for (int i = 0; i < countofcol; i++)
             {
                 for (int j = 0; j < m_DataTable.Rows.Count; j++)
                 {
                     if (m_DataTable.Rows[j][i].ToString() == "" || m_DataTable.Rows[j][i] == null || m_DataTable.Rows[j][i] is DBNull)
                     {
                         m_DataTable.Rows[j][i] = 0;
                     }
                     list[j] = Convert.ToDouble(m_DataTable.Rows[j][i]);
                 }
                 if (i == 0)
                 {
                     BarItem myBar = myPane.AddBar("用地面积总数", null, list, Color.Red);
                     myBar.Bar.Fill = new Fill(Color.Red, Color.White, Color.Red);
                     
                     
                 }
                 if (i == 1)
                 {
                     BarItem myBar = myPane.AddBar("占用耕地面积", null, list, Color.Blue);
                     myBar.Bar.Fill = new Fill(Color.Blue, Color.White,Color.Blue);
                 }
                 if (i == 2)
                 {
                     BarItem myBar = myPane.AddBar("供地批准面积总数", null, list, Color.Green);
                     myBar.Bar.Fill = new Fill(Color.Green, Color.White,Color.Green);
                 }
             }

            

             myPane.XAxis.MajorTic.IsBetweenLabels = true;

             // Set the XAxis labels
             myPane.XAxis.Scale.TextLabels = labels;
             myPane.XAxis.Scale.FontSpec.Size = 6;
             myPane.YAxis.Scale.FontSpec.Size = 6;
             myPane.Legend.FontSpec.Size = 6;

             // Set the XAxis to Text type
             myPane.XAxis.Type = AxisType.Text;

             zedGraphControl1.AxisChange();
                   
                  

        }

        private string getXZQM(string GHDM, string JB)
        {
            string XZQM = "";
            System.Data.DataRowCollection m_XZQMDataRowCollection;
            string strsqlXZQM = "";
            if (JB == "市")
            {
                //strsqlXZQM = "SELECT XZQM FROM lyk_dsxzqy WHERE QHDM='" + GHDM + "'"; // 原始 刘扬 修改 2011
                strsqlXZQM = "SELECT XZQMC FROM DSXZQ_TDLY WHERE XZQDM='" + GHDM + "'"; // 刘扬 修改 2011
            }
            if (JB == "县")
            {
                //strsqlXZQM = "SELECT XZQM FROM lyk_qxxzqy WHERE QHDM='" + GHDM + "'"; // 原始 刘扬 修改 2011
                strsqlXZQM = "SELECT XZQMC FROM XJZJ_TDLY WHERE XZQDM='" + GHDM + "'"; // 刘扬 修改 2011
            }

            m_XZQMDataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(strsqlXZQM);
            if (m_XZQMDataRowCollection != null && m_XZQMDataRowCollection.Count > 0)
            {
                XZQM = m_XZQMDataRowCollection[0][0].ToString();
            }
            return XZQM;
        }

        private void frmDQBJTB_Load(object sender, EventArgs e)
        {
            createBar(m_DataTable, zqm,qhdm);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {            
            string str_ndsy = "";
            string str_hcly = "";
            string str_dqsy = "";

            if (ndsy =="")
            { str_ndsy = ""; }
            else
            { str_ndsy = " WHERE tbrq = '" + ndsy.Substring(0, 4) + "'"; }

            if (hcly == "")
            { str_hcly = ""; }
            else
            {
                if (str_ndsy != "")
                { str_hcly = " AND hcly='" + hcly + "'"; }
                else
                { str_hcly = " WHERE hcly='" + hcly + "'"; }

            }

            if (qhdm=="21")
            { str_dqsy = ""; }
            else
            {
                if (str_ndsy != "" || str_hcly != "")
                {
                    if (m_DataAccess_SYS.ProviderIsOraDB())
                    {
                        str_dqsy = " AND substr(qhdm,1,4)='" + qhdm + "'";
                    }
                    else
                    {
                        str_dqsy = " AND left(qhdm,4)='" + qhdm + "'";
                    }
                }
                else
                {
                    if (m_DataAccess_SYS.ProviderIsOraDB())
                    { str_dqsy = " WHERE substr(qhdm,1,4)='" + qhdm + "'"; }
                    else
                    {
                        str_dqsy = " WHERE left(qhdm,4)='" + qhdm + "'";
                    }
                }
            }


            Crystal_Report.Report.DatabaseString.m_DataAccess_SYS = this.m_DataAccess_SYS;
            Crystal_Report.Report.SqlClass p_SqlClass = new Crystal_Report.Report.SqlClass();
            Crystal_Report.Report.DataSet1 pDataSet = new Crystal_Report.Report.DataSet1();            
            Crystal_Report.Report.frm_tjbb pfrm_tjbb = new Crystal_Report.Report.frm_tjbb();
            string str_sql = "select * from 执法检查数据汇总表 " + str_ndsy + str_hcly + str_dqsy;    
           
            p_SqlClass.GetDataSet(str_sql, "执法检查数据汇总表", pDataSet);            
           
            Crystal_Report.Report.Rpt_zfjcsjhz mrpt2 = new Crystal_Report.Report.Rpt_zfjcsjhz();
            mrpt2.SetDataSource(pDataSet.Tables["执法检查数据汇总表"]);
            pfrm_tjbb.Text = "执法检查数据汇总表";
            pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt2;

            pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
            pfrm_tjbb.crystalReportViewer1.Refresh();       
            pfrm_tjbb.crystalReportViewer2.Refresh();
            
            //pfrm_tjbb.MdiParent = this;
            pfrm_tjbb.Show();
        }
       
    }
}