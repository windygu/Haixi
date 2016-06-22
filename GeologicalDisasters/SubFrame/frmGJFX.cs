using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame
{
    public partial class frmGJFX : DevComponents.DotNetBar.Office2007Form
    {
        public DataRowCollection m_DataRowCollection;
        public clsDataAccess.DataAccess m_DataAccess_SYS;
        public string QHDM;
        public string ZQM;
        public frmGJFX()
        {
            InitializeComponent();
        }

        private void frmGJFX_Load(object sender, EventArgs e)
        {
            this.labelX1.Text = ZQM+"告警分析结果";
            addData();
        }
        private void addData()
        {
            if (m_DataRowCollection != null)
            {
                if (QHDM == "21")
                {
                    for (int i = 0; i < m_DataRowCollection.Count; i++)
                    {

                        if (m_DataRowCollection[i][2].ToString().Trim() != "")
                        {
                            string XZQM = "";
                            XZQM = getXZQM(m_DataRowCollection[i][2].ToString(), "市");
                            if (XZQM != "")
                            {
                                listViewEx1.Columns.Add(XZQM + "总用地");
                                listViewEx1.Columns.Add(XZQM + "违法用地");
                                listViewEx1.Columns.Add(XZQM + "比例");
                            }
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < m_DataRowCollection.Count; i++)
                    {
                        if (m_DataRowCollection[i][0].ToString() != "")
                        {
                            string XZQM = m_DataRowCollection[i][2].ToString();
                            listViewEx1.Columns.Add(XZQM + "总用地");
                            listViewEx1.Columns.Add(XZQM + "违法用地");
                            listViewEx1.Columns.Add(XZQM + "比例");
                        }

                    }

                }
           

            listViewEx1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();

            provider.PercentDecimalDigits = 2;//小数点保留几位数.
            provider.PercentPositivePattern = 1;//百分号出现在何处.

            //listview数据填充
            int colunms = listViewEx1.Columns.Count;
            String[] subitems = new String[colunms];
            double bili = 0;
            //int j = 1;
            for (int i = 0; i < colunms; i++)
            {
                if (i % 3 == 0)
                {
                    if (m_DataRowCollection[i / 3][0].ToString() != "")
                    {
                        subitems[i] = m_DataRowCollection[i / 3][0].ToString();
                    }
                    else
                    {
                        subitems[i] = "";
                    }
                }
                if (i % 3 == 1)
                {
                    if (m_DataRowCollection[i / 3][1].ToString() != "")
                    {
                        subitems[i] = m_DataRowCollection[i / 3][1].ToString();
                    }
                    else
                    {
                        subitems[i] = "";
                    }
                }
                if (i % 3 == 2)
                {
                    if (m_DataRowCollection[i / 3][0].ToString() != "" && m_DataRowCollection[i / 3][1].ToString() != "")
                    {
                        bili = Convert.ToDouble(m_DataRowCollection[i / 3][1]) / Convert.ToDouble(m_DataRowCollection[i / 3][0]);
                        subitems[i] = bili.ToString("P", provider);
                    }
                }
            }
          
            ListViewItem item = new ListViewItem(subitems, -1);
            listViewEx1.Items.Add(item);
            }
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
                //strsqlXZQM = "SELECT XZQM FROM lyk_qxxzqy WHERE QHDM='" + GHDM + "'";// 原始 刘扬 修改 2011
                strsqlXZQM = "SELECT XZQMC FROM XJZJ_TDLY WHERE XZQDM='" + GHDM + "'"; // 刘扬 修改 2011

            }

     
            m_XZQMDataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(strsqlXZQM);
            if (m_XZQMDataRowCollection != null && m_XZQMDataRowCollection.Count > 0)
            {
                XZQM = m_XZQMDataRowCollection[0][0].ToString();
            }
            return XZQM;
        }

    }
}