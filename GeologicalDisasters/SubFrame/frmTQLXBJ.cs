using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

using System.Data.OleDb;


namespace JCZF.SubFrame
{
    public partial class frmTQLXBJ : DevComponents.DotNetBar.Office2007Form
    {
        private DataTable m_DataTable;
        private string zqm;
        private string qhdm;
        private string NDSY;
        public string hcly;

        public clsDataAccess.DataAccess m_DataAccess_SYS ;

        public frmTQLXBJ(DataTable m_DataTable, string zqm, string NDSY, string qhdm)
        {
            InitializeComponent();
            this.m_DataTable = m_DataTable;
            this.zqm = zqm;
            this.qhdm = qhdm;
            this.NDSY = NDSY;

        }



        //----------------------------饼状图
        private void createPie(DataTable m_DataTable, string zqm, string NDSY, string qhdm)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zedGraphControl1.GraphPane;

            // Set the Titles
            if (NDSY == "全部") NDSY = "";

            this.Text = zqm + NDSY + "年度违法用地类型比较";
            myPane.Title.Text =  this.Text;
            myPane.Title.FontSpec.Size = 12;
            ////myPane.XAxis.Title.Text="城市";
            ////myPane.YAxis.Title.Text="数量";
            myPane.XAxis.IsVisible = false;
            myPane.YAxis.IsVisible = false;
            myPane.Legend.Position = LegendPos.Right;
            //myPane.Legend.IsVisible = false;

            

            
            ////myPane.Legend.Location = new Location(1.0f, 0.15f, CoordType.ChartFraction, AlignH.Right, AlignV.Top);
            ////string[] labels = new string[m_DataTable.Rows.Count];

            if (m_DataTable == null) { MessageBox.Show("无符合条件数据！"); return; }
            int ColumnsCount = m_DataTable.Columns.Count;
            double[] Num = new double[ColumnsCount-1];
            string[] Str = new string[ColumnsCount - 1];
            string[] m_strLabel = new string[ColumnsCount - 1];
            int k = 0;
            double m_Sum=0;
            for (int i = 0; i < ColumnsCount; i++)
            {
                if (m_DataTable.Rows[0][i].ToString() == "" || m_DataTable.Rows[0][i] == null || m_DataTable.Rows[0][i] is DBNull)
                {
                    m_DataTable.Rows[0][i] = 0;
                    k++;
                }
                m_Sum = m_Sum + Convert.ToDouble(m_DataTable.Rows[0][i]);
            }
            if (k ==9) { MessageBox.Show("无符合条件数据！"); return; }
            int j=0;

            m_strLabel[0] = "非法批地";
            m_strLabel[1] = "骗取批准";
            m_strLabel[2] = "超占用地";
            m_strLabel[3] = "未报即用";
            m_strLabel[4] = "边报边用";
            m_strLabel[5] = "未供即用";
            m_strLabel[6] = "擅自改变用途";
            m_strLabel[7] = "其他违法用地";
            myPane.Legend.Label = m_strLabel;
            for (int i = 1; i < ColumnsCount; i++)
            {

                Num[j] = Convert.ToDouble(m_DataTable.Rows[0][i]);
                if (m_Sum > 0)
                {
                    Str[j] = ((Num[j] * 100) / m_Sum).ToString("0.00") + "%";
                }
                else
                {
                    Str[j] = "";
                }
            j++;
            
            }
           

            PieItem[] myPie = myPane.AddPieSlices(Num, Str);
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
            myPane.Legend.FontSpec.Size = 8;
            //myPane.Legend.
            ////if (qhdm =="21")
                ////{
                ////    for (int j = 0; j < m_DataTable.Rows.Count; j++)
                ////    {

                ////        labels[j] =getXZQM(m_DataTable.Rows[j][countofcol-1].ToString(),"市");
                ////    }

                ////    countofcol = countofcol - 1;
                ////}
                ////else
                ////{
                ////    for (int j = 0; j < m_DataTable.Rows.Count; j++)
                ////    {
                ////        labels[j] = m_DataTable.Rows[j][countofcol-1].ToString();
                ////    }

                ////    countofcol = countofcol - 1;
                ////}


                ////for (int i = 0; i < countofcol; i++)
                //// {
                ////     for (int j = 0; j < m_DataTable.Rows.Count; j++)
                ////     {
                ////         if (m_DataTable.Rows[j][i].ToString() == "" || m_DataTable.Rows[j][i] == null || m_DataTable.Rows[j][i] is DBNull)
                ////         {
                ////             m_DataTable.Rows[j][i] = 0;
                ////         }
                ////         list[j] = Convert.ToDouble(m_DataTable.Rows[j][i]);
                ////     }
                ////     if (i == 0)
                ////     {
                ////         BarItem myBar = myPane.AddBar("宗地数", null, list, Color.Red);
                ////         myBar.Bar.Fill = new Fill(Color.Red, Color.White, Color.Red);
                ////     }
                ////     if (i == 1)
                ////     {
                ////         BarItem myBar = myPane.AddBar("面积", null, list, Color.Blue);
                ////         myBar.Bar.Fill = new Fill(Color.Blue, Color.White,Color.Blue);
                ////     }
                ////     if (i == 2)
                ////     {
                ////         BarItem myBar = myPane.AddBar("耕地", null, list, Color.Green);
                ////         myBar.Bar.Fill = new Fill(Color.Green, Color.White,Color.Green);
                ////     }
                //// }

                ////myPane.XAxis.MajorTic.IsBetweenLabels = true;

             // Set the XAxis labels
             ////myPane.XAxis.Scale.TextLabels = labels;

             // Set the XAxis to Text type
             ////myPane.XAxis.Type = AxisType.Text;

             zedGraphControl1.AxisChange();
                   
                  

        }

        ////private string getXZQM(string GHDM, string JB)
        ////{
        ////    string XZQM = "";
        ////    System.Data.DataRowCollection m_XZQMDataRowCollection;
        ////    string strsqlXZQM = "";
        ////    if (JB == "市")
        ////    {
        ////        strsqlXZQM = "SELECT XZQM FROM lyk_dsxzqy WHERE QHDM='" + GHDM + "'";
        ////    }
        ////    if (JB == "县")
        ////    {
        ////        strsqlXZQM = "SELECT XZQM FROM lyk_qxxzqy WHERE QHDM='" + GHDM + "'";
        ////    }

        ////    m_XZQMDataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(strsqlXZQM);
        ////    if (m_XZQMDataRowCollection != null && m_XZQMDataRowCollection.Count > 0)
        ////    {
        ////        XZQM = m_XZQMDataRowCollection[0][0].ToString();
        ////    }
        ////    return XZQM;
        ////}

        private void frmDQBJTB_Load(object sender, EventArgs e)
        {
            createPie(m_DataTable, zqm, NDSY, qhdm);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string str_ndsy = "";
            string str_hcly = "";
            string str_dqsy = "";

            if (NDSY == "")
            { str_ndsy = ""; }
            else
            { str_ndsy = " WHERE tbrq = '" + NDSY.Substring(0, 4) + "'"; }

            if (hcly == "")
            { str_hcly = ""; }
            else
            {
                if (str_ndsy != "")
                { str_hcly = " AND hcly='" + hcly + "'"; }
                else
                { str_hcly = " WHERE hcly='" + hcly + "'"; }
            }

            if (qhdm == "21")
            { str_dqsy = ""; }
            else
            {
                //if (str_ndsy != "" || str_hcly != "")
                //{ str_dqsy = " AND left(qhdm,4)='" + qhdm + "'"; }
                //else
                //{ str_dqsy = " WHERE left(qhdm,4)='" + qhdm + "'"; }


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
            string str_sql = "select * from 违法用地数据汇总表 " + str_ndsy + str_hcly + str_dqsy;      //WHERE gdpzh = '" + this.textBox1.Text + "'";
            ////str_sql = "select * from 执法检查数据汇总表 WHERE qhdm ='000' ";      //WHERE gdpzh = '" + this.textBox1.Text + "'";
            p_SqlClass.GetDataSet(str_sql, "违法用地数据汇总表", pDataSet);
            //pDataSet = p_SqlClass.GetDataSet(str_sql, "问题地块");
            Crystal_Report.Report.Rpt_wfydsjhz mrpt3 = new Crystal_Report.Report.Rpt_wfydsjhz();
            mrpt3.SetDataSource(pDataSet.Tables["违法用地数据汇总表"]);
            pfrm_tjbb.Text = "违法用地数据汇总表";
            pfrm_tjbb.crystalReportViewer1.ReportSource = mrpt3;

            pfrm_tjbb.crystalReportViewer1.DisplayToolbar = true;
            pfrm_tjbb.crystalReportViewer1.Refresh();

            pfrm_tjbb.crystalReportViewer2.Refresh();
            //            
            pfrm_tjbb.Show();

        }
       
    }
}