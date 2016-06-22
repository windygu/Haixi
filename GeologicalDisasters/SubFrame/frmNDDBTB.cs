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
    public partial class frmNDDBTB : DevComponents.DotNetBar.Office2007Form
    {
        private DataTable m_DataTable;
        private string zqm;
        private string qhdm;

        public string hcly;
        public string ndsy;

        //public clsDataAccess.DataAccess m_DataAccess_SYS = new clsDataAccess.DataAccess();

        public frmNDDBTB(DataTable m_DataTable, string zqm, string qhdm)
        {
            InitializeComponent();
            this.m_DataTable = m_DataTable;
            this.zqm = zqm;
            this.qhdm = qhdm;
        }

        //----------------------------柱状图
        private void createBar(DataTable m_DataTable, string zqm, string qhdm)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zedGraphControl1.GraphPane;

            // Set the Titles
            myPane.Title.Text = zqm + "违法用地面积年度对比图";
            myPane.XAxis.Title.Text = "年度";
            myPane.YAxis.Title.Text = "面积（亩）";
            myPane.Legend.IsVisible = false;

            string[] labels = new string[m_DataTable.Rows.Count];
            double[] count = new double[m_DataTable.Rows.Count];
            PointPairList list = new PointPairList();

            for (int i = 0; i < m_DataTable.Rows.Count; i++)
            {
                if (Convert.IsDBNull(m_DataTable.Rows[i][0]) != true)
                {
                    list.Add((double)i, Convert.ToDouble(m_DataTable.Rows[i][0]));
                }
                else
                {
                    list.Add((double)i,0);
                }
                labels[i] = Convert.ToString(m_DataTable.Rows[i][1]);              
            }

            BarItem myCurve = myPane.AddBar("面积", list, Color.Blue);

            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White,
               Color.FromArgb(255, 255, 166), 45.0F);

            //myPane.XAxis.Scale.TextLabels = labels;

            myPane.XAxis.MajorTic.IsBetweenLabels = true;

            // Set the XAxis labels
            myPane.XAxis.Scale.TextLabels = labels;

            // Set the XAxis to Text type
            myPane.XAxis.Type = AxisType.Text;

            zedGraphControl1.AxisChange();

        }

        private void frmNDDBTB_Load(object sender, EventArgs e)
        {
            createBar(m_DataTable, zqm, qhdm);
        }

    }
}