using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace JCZF.SubFrame
{
    public partial class uctDBFX : UserControl
    {
        private string m_strCurrentMapFileName_;
        public string m_strCurrentMapFileName
        {
            set
            {
                m_strCurrentMapFileName_ = value;
            }
        }
        frmDQBJTB m_frmDQBJTB;
        public delegate void uctTJFX_XCCQTJ_JGEventHandler(DataTable p_DataTable);
        public event uctTJFX_XCCQTJ_JGEventHandler uctTJFX_XCCQTJ_JGQueryEvent;
        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
              
                uctXZQTree_Dev_BTDQBJ.m_AxMapControl = value;
                uctXZQTree_Dev_QYNDDB.m_AxMapControl = value;
                uctXZQTree_Dev_TQLXBJ.m_AxMapControl = value;
                uctXZQTree_Dev_WFYDGJFX.m_AxMapControl = value;
                m_AxMapControl_ = value;
            }
        }

        private clsDataAccess.DataAccess m_DataAccess_SYS_;
        public clsDataAccess.DataAccess m_DataAccess_SYS
        {
            set
            {
                m_DataAccess_SYS_ = value;    
            }
        }
        private clsDataAccess.DataAccess m_DataAccess_SDE_;
        public clsDataAccess.DataAccess m_DataAccess_SDE
        {
            set
            {
                m_DataAccess_SDE_ = value;
            }
        }
        public uctDBFX()
        {
            InitializeComponent();
        }

        private void bntDBFXNDOK_Click(object sender, EventArgs e)
        {
            string NDSY1 = getNDSY(this.DBFXNDNDComB1);
            string NDSY2 = getNDSY(this.DBFXNDNDComB2);
            string HCLY = getHCLY(this.DBFXNDHCLYComB);
            string QHDM = uctXZQTree_Dev_QYNDDB.m_strXZQDM;

            if (NDSY1 == "" || NDSY2 == "")
            {
                MessageBox.Show("请选择年度范围");
                return;
            }
            string SQLString = getSQLString2(NDSY1, NDSY2, HCLY, QHDM);
            string str = "";
            string str1 = "";
            string zqm = uctXZQTree_Dev_QYNDDB.m_strXZQMC;

            if (QHDM.Trim().Length == 2)
            {
                if (SQLString != "")
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select sum(wfyd_hj_mj_zs),left(tbrq,4),sum(wfyd_hj_zds) from 违法用地数据汇总表 where " + SQLString + "group by substr(tbrq,1,4)";
                        //str = "select sum(wfyd_hj_mj_zs),left(tbrq,4),sum(wfyd_hj_zds),left(qhdm,4) from 违法用地数据汇总表 where" + SQLString + "group by left(tbrq,4),left(qhdm,4)";
                        str1 = "select sum(wfyd_hj_mj_zs) as mj,substr(tbrq,1,4) as tbrq,sum(wfyd_hj_zds) as zds,substr(qhdm,1,4) as qhdm from 违法用地数据汇总表 as table1 where" + SQLString + "group by substr(tbrq,1,4),substr(qhdm,1,4)";

                    }
                    else
                    {

                        str = "select sum(wfyd_hj_mj_zs),left(tbrq,4),sum(wfyd_hj_zds) from 违法用地数据汇总表 where " + SQLString + "group by left(tbrq,4)";
                        //str = "select sum(wfyd_hj_mj_zs),left(tbrq,4),sum(wfyd_hj_zds),left(qhdm,4) from 违法用地数据汇总表 where" + SQLString + "group by left(tbrq,4),left(qhdm,4)";
                        str1 = "select sum(wfyd_hj_mj_zs) as mj,left(tbrq,4) as tbrq,sum(wfyd_hj_zds) as zds,left(qhdm,4) as qhdm from 违法用地数据汇总表 as table1 where" + SQLString + "group by left(tbrq,4),left(qhdm,4)";
                    }
                }

            }
            if (QHDM.Trim().Length == 4)
            {
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    str = "select sum((wfyd_hj_mj_zs)),substr(tbrq,1,4),sum(wfyd_hj_zds),substr(qhdm,1,4) from 违法用地数据汇总表  where substr(qhdm,1,4)=" + QHDM + " and " + SQLString + "group by substr(tbrq,1,4),substr(qhdm,1,4)";
                    str1 = "select sum(wfyd_hj_mj_zs) as mj,substr(tbrq,1,4) as tbrq,sum(wfyd_hj_zds) as zds,qhdm as qhdm from 违法用地数据汇总表 where substr(qhdm,1,4)=" + QHDM + " and " + SQLString + "group by substr(tbrq,1,4),qhdm";
                }
                else
                {
                    str = "select sum((wfyd_hj_mj_zs)),left(tbrq,4),sum(wfyd_hj_zds),left(qhdm,4) from 违法用地数据汇总表  where left(qhdm,4)=" + QHDM + " and " + SQLString + "group by left(tbrq,4),left(qhdm,4)";
                    str1 = "select sum(wfyd_hj_mj_zs) as mj,left(tbrq,4) as tbrq,sum(wfyd_hj_zds) as zds,qhdm as qhdm from 违法用地数据汇总表 where left(qhdm,4)=" + QHDM + " and " + SQLString + "group by left(tbrq,4),qhdm";
                }
            }
            if (QHDM.Trim().Length == 6)
            {
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    str = "select sum(wfyd_hj_mj_zs),substr(tbrq,1,4),sum(wfyd_hj_zds),qhdm from 违法用地数据汇总表 where qhdm=" + QHDM + "and " + SQLString + "group by substr(tbrq,1,4),qhdm";

                    str1 = "select sum(wfyd_hj_mj_zs) as mj,substr(tbrq,1,4) as tbrq,sum(wfyd_hj_zds) as zds,qhdm as qhdm from 违法用地数据汇总表 where substr(qhdm,1,4)=" + QHDM.Substring(0, 4) + " and " + SQLString + "group by substr(tbrq,1,4),qhdm";

                }
                else
                {
                    str = "select sum(wfyd_hj_mj_zs),left(tbrq,4),sum(wfyd_hj_zds),qhdm from 违法用地数据汇总表 where qhdm=" + QHDM + "and " + SQLString + "group by left(tbrq,4),qhdm";

                    str1 = "select sum(wfyd_hj_mj_zs) as mj,left(tbrq,4) as tbrq,sum(wfyd_hj_zds) as zds,qhdm as qhdm from 违法用地数据汇总表 where left(qhdm,4)=" + QHDM.Substring(0, 4) + " and " + SQLString + "group by left(tbrq,4),qhdm";
                }
            }

            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(str);

            DataTable m_DataTable1 = m_DataAccess_SYS_.getDataTableByQueryString(str1);
            DataRowCollection m_DataRowCollection;
            if (QHDM.Trim().Length == 2)
            {
                m_DataRowCollection = m_DataAccess_SYS_.GetAllDSXZQY();
            }
            else
            {
                m_DataRowCollection = m_DataAccess_SYS_.GetQXXZQYofQHDM(QHDM.Substring(0, 4));
            }

            //listview
            //if (listView1 == null) listView1 = new DevComponents.DotNetBar.Controls.ListViewEx();
            //listView1.Clear();

            //listView1.Columns.Add("年度");


            ArrayList qhdmarr = new ArrayList();
            for (int i = 0; i < m_DataRowCollection.Count; i++)
            {
                if (m_DataRowCollection[i][0].ToString().Trim() != "")
                {

                    string XZQM = m_DataRowCollection[i][0].ToString();
                    string ColQHDM = m_DataRowCollection[i][1].ToString();
                    qhdmarr.Add(ColQHDM);
                    if (XZQM != "")
                    {
                        //listView1.Columns.Add(XZQM + "违法用地面积");
                        //listView1.Columns.Add(XZQM + "宗地数");
                        //for (int j = 0; j < m_DataTable.Rows.Count; j++)
                        //{
                        //    if (m_DataTable1.Rows[i][3].ToString() == ColQHDM)
                        //    {
                        //        listView1.Items[j].SubItems[1].Text = m_DataTable.Rows[i][0].ToString();
                        //        break; 
                        //    }
                        //}
                    }
                }
            }
            //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            for (int i = Convert.ToInt32(NDSY1); i <= Convert.ToInt32(NDSY2); i++)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems[0].Text = i.ToString();

                for (int j = 0; j < qhdmarr.Count; j++)
                {
                    int k;
                    for (k = 0; k < m_DataTable1.Rows.Count; k++)
                    {
                        if (m_DataTable1.Rows[k][3].ToString() == qhdmarr[j].ToString() && m_DataTable1.Rows[k][1].ToString() == i.ToString())
                        {
                            item.SubItems.Add(m_DataTable1.Rows[k][0].ToString());
                            item.SubItems.Add(m_DataTable1.Rows[k][2].ToString());
                            break;
                        }
                    }
                    if (k == m_DataTable1.Rows.Count)
                    {
                        item.SubItems.Add(" ");
                        item.SubItems.Add(" ");
                    }


                }

                //listView1.Items.Add(item);
            }

            System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();

            provider.PercentDecimalDigits = 2;//小数点保留几位数.
            provider.PercentPositivePattern = 1;//百分号出现在何处.
            double result = (double)1 / 3;//一定要用double类型.
            //Console.WriteLine(result.ToString("P", provider));//.Write(result.ToString("P", provider));


            for (int i = Convert.ToInt32(NDSY1); i < Convert.ToInt32(NDSY2); i++)
            {
                ListViewItem item = new ListViewItem();
                int ii = i + 1;
                item.SubItems[0].Text = i.ToString() + "->" + ii.ToString() + "变化率";

                int indexofk = -1;
                int indexofkk = -1;
                for (int j = 0; j < qhdmarr.Count; j++)
                {
                    for (int k = 0; k < m_DataTable1.Rows.Count; k++)
                    {
                        if (m_DataTable1.Rows[k][3].ToString() == qhdmarr[j].ToString() && m_DataTable1.Rows[k][1].ToString() == i.ToString())
                        {
                            indexofk = k;

                        }
                        if (m_DataTable1.Rows[k][3].ToString() == qhdmarr[j].ToString() && m_DataTable1.Rows[k][1].ToString() == ii.ToString())
                        {
                            indexofkk = k;
                        }
                    }

                    if (indexofk != -1 && indexofkk != -1)
                    {
                        double mjbhl = 0;
                        double zdsbhl = 0;
                        if (Convert.IsDBNull(m_DataTable1.Rows[indexofkk][0]) != true && Convert.IsDBNull(m_DataTable1.Rows[indexofk][0]) != true)// Convert.IsDBNull()!=true)
                        {
                            mjbhl = (Convert.ToDouble(m_DataTable1.Rows[indexofkk][0]) - Convert.ToDouble(m_DataTable1.Rows[indexofk][0])) / Convert.ToDouble(m_DataTable1.Rows[indexofk][0]);
                        }
                        if (Convert.IsDBNull(m_DataTable1.Rows[indexofkk][2]) != true && Convert.IsDBNull(m_DataTable1.Rows[indexofk][2]) != true)
                        {
                            zdsbhl = (Convert.ToDouble(m_DataTable1.Rows[indexofkk][2]) - Convert.ToDouble(m_DataTable1.Rows[indexofk][2])) / Convert.ToDouble(m_DataTable1.Rows[indexofk][2]);
                        }
                        item.SubItems.Add(mjbhl.ToString("P", provider));
                        item.SubItems.Add(zdsbhl.ToString("P", provider));
                    }
                    else
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                    }
                }

                //listView1.Items.Add(item);
            }


            if (m_DataTable == null)
            {
                MessageBox.Show("没有所选数据");
            }
            else
            {
                frmNDDBTB m_frmNDDBTB = new frmNDDBTB(m_DataTable, zqm, QHDM);
                //m_frmNDDBTB.m_DataAccess_SYS = this.m_DataAccess_SYS;
                m_frmNDDBTB.hcly = HCLY;
                m_frmNDDBTB.Text = zqm + "违法用地面积年度对比图";
                m_frmNDDBTB.MdiParent = this.ParentForm ;
                m_frmNDDBTB.Show();
            }
        }

        private void bntTQLXBJOK_Click(object sender, EventArgs e)
        {
            string NDSY = getNDSY(DBFXLXNDComB);
            string HCLY = getHCLY(DBFXLXHCLYComB);
            //string QHDM = getQHDM(this.DBFXLXDQSYComTree);
            string QHDM = uctXZQTree_Dev_TQLXBJ.m_strXZQDM;
            string SQLString = getSQLString(NDSY, HCLY, QHDM);
            string str = "";
            string zqm = uctXZQTree_Dev_TQLXBJ.m_strXZQMC;

            //if (QHDM == "21")
            //{
            //    if (SQLString != "")
            //    {
            //        //str = "select sum(zyd_xzjsyd_hj_zds),sum(zyd_xzjsyd_hj_mj_zs),sum(zyd_xzjsyd_hj_mj_gd),left(qhdm,4) from 执法检查数据汇总表 where " + SQLString + "group by left(qhdm,4)";
            //        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, LEFT(QHDM, 4) AS Expr4 FROM 土地核查 WHERE " + SQLString + " and ghyt = '建设用地'" + " group by LEFT(QHDM,4)";
            //    }
            //    else
            //    {
            //        //str = "select  sum(zyd_xzjsyd_hj_zds),sum(zyd_xzjsyd_hj_mj_zs),sum(zyd_xzjsyd_hj_mj_gd),left(qhdm,4) from 执法检查数据汇总表 group by left(qhdm,4)";
            //        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, LEFT(QHDM, 4) AS Expr4 FROM 土地核查 WHERE (ghyt = '建设用地') GROUP BY LEFT(QHDM, 4)";
            //    }

            //}
            //else
            //{
            //    if (SQLString != "")
            //    {
            //        //str = "select zyd_xzjsyd_hj_zds,zyd_xzjsyd_hj_mj_zs,zyd_xzjsyd_hj_mj_gd,city from 执法检查数据汇总表 where left(qhdm,4)='" + QHDM + "' and " + SQLString;
            //        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, QHDM AS Expr4 FROM 土地核查 where left(qhdm,4)='" + QHDM + "' and " + SQLString + " and ghyt = '建设用地'" + " group by qhdm";
            //    }
            //    else
            //    {
            //        //str = "select zyd_xzjsyd_hj_zds,zyd_xzjsyd_hj_mj_zs,zyd_xzjsyd_hj_mj_gd,city from 执法检查数据汇总表 where left(qhdm,4)='" + QHDM + "'";
            //        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, QHDM AS Expr4 FROM 土地核查 where left(qhdm,4)='" + QHDM + "'" + " and ghyt = '建设用地'" + " group by qhdm";
            //    }
            //}



            if (QHDM == "21")
            {
                if (SQLString != "")
                {
                    str = "select sum(wfyd_hj_mj_zs),sum(wfyd_ffpd_mj_zs),sum(wfyd_ffzd_pqpz_mj_zs),sum(wfyd_ffzd_czyd_mj_zs),sum(wfyd_ffzd_wpxy_wbjy_mj_zs),sum(wfyd_ffzd_wpxy_bbby_mj_zs),sum(wfyd_ffzd_wpxy_wgjy_mj_zs),sum(wfyd_szgbyt_mj_zs),sum(wfyd_qtwfyd_mj_zs) from 违法用地数据汇总表 where " + SQLString;
                }
                else
                {
                    str = "select sum(wfyd_hj_mj_zs),sum(wfyd_ffpd_mj_zs),sum(wfyd_ffzd_pqpz_mj_zs),sum(wfyd_ffzd_czyd_mj_zs),sum(wfyd_ffzd_wpxy_wbjy_mj_zs),sum(wfyd_ffzd_wpxy_bbby_mj_zs),sum(wfyd_ffzd_wpxy_wgjy_mj_zs),sum(wfyd_szgbyt_mj_zs),sum(wfyd_qtwfyd_mj_zs) from 违法用地数据汇总表";
                }

            }
            else if (QHDM.Length == 4)
            {
                if (SQLString != "")
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select sum(wfyd_hj_mj_zs),sum(wfyd_ffpd_mj_zs),sum(wfyd_ffzd_pqpz_mj_zs),sum(wfyd_ffzd_czyd_mj_zs),sum(wfyd_ffzd_wpxy_wbjy_mj_zs),sum(wfyd_ffzd_wpxy_bbby_mj_zs),sum(wfyd_ffzd_wpxy_wgjy_mj_zs),sum(wfyd_szgbyt_mj_zs),sum(wfyd_qtwfyd_mj_zs) from 违法用地数据汇总表 where " + SQLString + " AND substr(qhdm,1,4)='" + QHDM + "'";
                    }
                    else
                    {
                        str = "select sum(wfyd_hj_mj_zs),sum(wfyd_ffpd_mj_zs),sum(wfyd_ffzd_pqpz_mj_zs),sum(wfyd_ffzd_czyd_mj_zs),sum(wfyd_ffzd_wpxy_wbjy_mj_zs),sum(wfyd_ffzd_wpxy_bbby_mj_zs),sum(wfyd_ffzd_wpxy_wgjy_mj_zs),sum(wfyd_szgbyt_mj_zs),sum(wfyd_qtwfyd_mj_zs) from 违法用地数据汇总表 where " + SQLString + " AND left(qhdm,4)='" + QHDM + "'";
                    }
                }
                else
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select sum(wfyd_hj_mj_zs),sum(wfyd_ffpd_mj_zs),sum(wfyd_ffzd_pqpz_mj_zs),sum(wfyd_ffzd_czyd_mj_zs),sum(wfyd_ffzd_wpxy_wbjy_mj_zs),sum(wfyd_ffzd_wpxy_bbby_mj_zs),sum(wfyd_ffzd_wpxy_wgjy_mj_zs),sum(wfyd_szgbyt_mj_zs),sum(wfyd_qtwfyd_mj_zs) from 违法用地数据汇总表 where substr(qhdm,1,4)='" + QHDM + "'";
                    }
                    else
                    {
                        str = "select sum(wfyd_hj_mj_zs),sum(wfyd_ffpd_mj_zs),sum(wfyd_ffzd_pqpz_mj_zs),sum(wfyd_ffzd_czyd_mj_zs),sum(wfyd_ffzd_wpxy_wbjy_mj_zs),sum(wfyd_ffzd_wpxy_bbby_mj_zs),sum(wfyd_ffzd_wpxy_wgjy_mj_zs),sum(wfyd_szgbyt_mj_zs),sum(wfyd_qtwfyd_mj_zs) from 违法用地数据汇总表 where left(qhdm,4)='" + QHDM + "'";
                    }
                }

            }
            else
            {
                if (SQLString != "")
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select wfyd_hj_mj_zs,wfyd_ffpd_mj_zs,wfyd_ffzd_pqpz_mj_zs,wfyd_ffzd_czyd_mj_zs,wfyd_ffzd_wpxy_wbjy_mj_zs,wfyd_ffzd_wpxy_bbby_mj_zs,wfyd_ffzd_wpxy_wgjy_mj_zs,wfyd_szgbyt_mj_zs,wfyd_qtwfyd_mj_zs from 违法用地数据汇总表 where substr(qhdm,1,6)='" + QHDM + "' and " + SQLString;
                    }
                    else
                    {
                        str = "select wfyd_hj_mj_zs,wfyd_ffpd_mj_zs,wfyd_ffzd_pqpz_mj_zs,wfyd_ffzd_czyd_mj_zs,wfyd_ffzd_wpxy_wbjy_mj_zs,wfyd_ffzd_wpxy_bbby_mj_zs,wfyd_ffzd_wpxy_wgjy_mj_zs,wfyd_szgbyt_mj_zs,wfyd_qtwfyd_mj_zs from 违法用地数据汇总表 where left(qhdm,6)='" + QHDM + "' and " + SQLString;
                    }
                }
                else
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select wfyd_hj_mj_zs,wfyd_ffpd_mj_zs,wfyd_ffzd_pqpz_mj_zs,wfyd_ffzd_czyd_mj_zs,wfyd_ffzd_wpxy_wbjy_mj_zs,wfyd_ffzd_wpxy_bbby_mj_zs,wfyd_ffzd_wpxy_wgjy_mj_zs,wfyd_szgbyt_mj_zs,wfyd_qtwfyd_mj_zs from 违法用地数据汇总表 where substr(qhdm,1,6)='" + QHDM + "'";
                    }
                    else
                    {
                        str = "select wfyd_hj_mj_zs,wfyd_ffpd_mj_zs,wfyd_ffzd_pqpz_mj_zs,wfyd_ffzd_czyd_mj_zs,wfyd_ffzd_wpxy_wbjy_mj_zs,wfyd_ffzd_wpxy_bbby_mj_zs,wfyd_ffzd_wpxy_wgjy_mj_zs,wfyd_szgbyt_mj_zs,wfyd_qtwfyd_mj_zs from 违法用地数据汇总表 where left(qhdm,6)='" + QHDM + "'";
                    }
                }
            }

            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(str);

            frmTQLXBJ m_frmTQLXBJ = new frmTQLXBJ(m_DataTable, zqm, NDSY, QHDM);
            m_frmTQLXBJ.m_DataAccess_SYS = this.m_DataAccess_SYS_;
            m_frmTQLXBJ.hcly = HCLY;
            m_frmTQLXBJ.Text = zqm + "违法用地类型比较";
            m_frmTQLXBJ.MdiParent = this.ParentForm;
            m_frmTQLXBJ.Show();
        }

        private void btnDBFXDQOK_Click(object sender, EventArgs e)
        {
            string NDSY = getNDSY(DBFXDQNDComB);
            string HCLY = getHCLY(DBFXDQHCLYComB);
            string QHDM = uctXZQTree_Dev_BTDQBJ.m_strXZQDM;
            string SQLString = getSQLString(NDSY, HCLY, QHDM);
            string str = "";
            string zqm = uctXZQTree_Dev_BTDQBJ.m_strXZQMC;
            string m_strTempSQL = "";
            if (chkWFYD.Checked)
            {
                m_strTempSQL = " SDWBH='√' or WBJY='√' or BBBY='√' or PQPZ='√' or CZYD='√' or SZGBYT='√' or FFPD='√'"
                    + " or QTWFYD='√' or QTWFYD='√' or WFTDLYZTGH='√' or ZYJBNT='√' ";
                if (chkSDWBH.Checked)
                {
                    m_strTempSQL = m_strTempSQL + " or SDWBH='√'";
                }
                if (chkNYJGTZ.Checked)
                {
                    m_strTempSQL = m_strTempSQL + "  or NYJGTZ='√'";
                }

                if (SQLString != "") // 刘扬 修改 2011
                    m_strTempSQL = " and (" + m_strTempSQL + " )";
            }
            else if (chkSDWBH.Checked)
            {
                if (chkNYJGTZ.Checked)
                {
                    m_strTempSQL = " and ( SDWBH='√' or NYJGTZ='√')";
                }
                else
                {
                    m_strTempSQL = " and SDWBH='√'";
                }
            }
            else if (chkNYJGTZ.Checked)
            {
                m_strTempSQL = " and NYJGTZ='√'";
            }

            SQLString = SQLString + m_strTempSQL;

            if (QHDM == "21")
            {
                if (SQLString != "")
                {
                    if (m_DataAccess_SDE_.ProviderIsOraDB())
                    {
                        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, substr(QHDM, 1,4) AS Expr4 FROM 土地核查 WHERE " + SQLString + " group by substr(QHDM,1,4)";  // 刘扬 修改 2011
                    }
                    else
                    {
                        //str = "select sum(zyd_xzjsyd_hj_zds),sum(zyd_xzjsyd_hj_mj_zs),sum(zyd_xzjsyd_hj_mj_gd),left(qhdm,4) from 执法检查数据汇总表 where " + SQLString + "group by left(qhdm,4)";
                        //str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, LEFT(QHDM, 4) AS Expr4 FROM sde.土地核查 WHERE " + SQLString + " and ghyt = '建设用地'" + " group by LEFT(QHDM,4)";
                        //SQLString = SQLString.Substring(4, SQLString.Length - 4); // 刘扬 修改 2011
                        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, LEFT(QHDM, 4) AS Expr4 FROM 土地核查 WHERE " + SQLString + " group by LEFT(QHDM,4)";  // 刘扬 修改 2011
                    }
                }
                else
                {
                    if (m_DataAccess_SDE_.ProviderIsOraDB())
                    {
                        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, substr(QHDM,1, 4) AS Expr4 FROM 土地核查 WHERE (ghyt = '建设用地') GROUP BY substr(QHDM,1, 4)";
                    }
                    else
                    {
                        //str = "select  sum(zyd_xzjsyd_hj_zds),sum(zyd_xzjsyd_hj_mj_zs),sum(zyd_xzjsyd_hj_mj_gd),left(qhdm,4) from 执法检查数据汇总表 group by left(qhdm,4)";
                        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, LEFT(QHDM, 4) AS Expr4 FROM 土地核查 WHERE (ghyt = '建设用地') GROUP BY LEFT(QHDM, 4)";
                    }
                }
            }
            else
            {
                if (SQLString != "")
                {
                    if (m_DataAccess_SDE_.ProviderIsOraDB())
                    {
                        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, QHDM AS Expr4 FROM 土地核查 where substr(qhdm,1,4)='" + QHDM + "' and " + SQLString + " group by qhdm";  // 刘扬 修改 2011
                    }
                    else
                    {
                        //str = "select zyd_xzjsyd_hj_zds,zyd_xzjsyd_hj_mj_zs,zyd_xzjsyd_hj_mj_gd,city from 执法检查数据汇总表 where left(qhdm,4)='" + QHDM + "' and " + SQLString;
                        //str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, QHDM AS Expr4 FROM sde.土地核查 where left(qhdm,4)='" + QHDM + "' and " + SQLString + " and ghyt = '建设用地'" + " group by qhdm";

                        //str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, QHDM AS Expr4 FROM sde.土地核查 where left(qhdm,4)='" + QHDM + "' and " + SQLString + " group by qhdm"; // 原始 刘扬 修改 2011
                        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, QHDM AS Expr4 FROM 土地核查 where left(qhdm,4)='" + QHDM + "' and " + SQLString + " group by qhdm";  // 刘扬 修改 2011
                    }
                }
                else
                {
                    if (m_DataAccess_SDE_.ProviderIsOraDB())
                    {
                        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, QHDM AS Expr4 FROM 土地核查 where substr(qhdm,1,4)='" + QHDM + "'" + " and ghyt = '建设用地'" + " group by qhdm";
                    }
                    else
                    {
                        //str = "select zyd_xzjsyd_hj_zds,zyd_xzjsyd_hj_mj_zs,zyd_xzjsyd_hj_mj_gd,city from 执法检查数据汇总表 where left(qhdm,4)='" + QHDM + "'";
                        str = "SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, QHDM AS Expr4 FROM 土地核查 where left(qhdm,4)='" + QHDM + "'" + " and ghyt = '建设用地'" + " group by qhdm";
                    }
                }
            }
            //DataSet mm = m_DataAccess_SYS.GetDataBySQL("SELECT SUM(CAST(ydmjzs AS float)) AS Expr1, SUM(CAST(ydmjgd AS float)) AS Expr2,SUM(CAST(gdpzmjzs AS float)) AS Expr3, LEFT(QHDM, 4) AS Expr4 FROM 土地核查 WHERE (ghyt = '建设用地') GROUP BY LEFT(QHDM, 4)" );
            //DataSet m_DataSet = m_DataAccess_SYS.GetDataBySQL(str);
            DataTable m_DataTable = m_DataAccess_SDE_.getDataTableByQueryString(str);

            if (m_DataTable != null)
            {
                m_frmDQBJTB = new frmDQBJTB(m_DataTable, zqm, QHDM);
                m_frmDQBJTB.m_DataAccess_SYS = this.m_DataAccess_SDE_;
                m_frmDQBJTB.hcly = HCLY;
                m_frmDQBJTB.ndsy = NDSY;
                m_frmDQBJTB.Text = zqm + NDSY + "年新增建设用地违法用地不同地区比较";
                m_frmDQBJTB.MdiParent = this.ParentForm ;
                m_frmDQBJTB.Show();
            }
            else
            {
                MessageBox.Show("数据查询结果为空");
            }
        }


        private string getNDSY(ComboBox combox)
        {
            string NDSY = "";
            if (combox.Text != "年度索引" && combox.Text != "全部" && combox.Text != null)////lyh
            {
                NDSY = combox.Text.Trim().Substring(0, 4);
            }
            else
            {
                NDSY = "";
            }
            return NDSY;
        }

        private string getHCLY(ComboBox combox)
        {
            string HCLY = "";
            if (combox.Text != "核查来源" && combox.Text != "全部" && combox.Text != null)////lyh
            {
                HCLY = combox.Text;
            }
            else
            {
                HCLY = "";
            }
            return HCLY;
        }

        private string getBHLX(XZQTree.ComboBoxTreeView combtree)
        {
            string BHLX = "";
            if (combtree.Text != "变化类型")
            {
                BHLX = combtree.Text;
            }
            else
            {
                BHLX = "";
            }
            return BHLX;
        }

        private string getSQLString2(string NDSY1, string NDSY2, string HCLY, string QHDM)
        {
            string SQLString = "";
            ArrayList arr = new ArrayList();
            //年度索引
            if (NDSY1 != "" && NDSY2 != "")
            {

                if (NDSY1 != NDSY2)
                {
                    if (m_DataAccess_SDE_.ProviderIsOraDB())
                    {
                        arr.Add(" ((substr(tbrq,1,4) >= '" + NDSY1 + "') and (substr(tbrq,1,4) <= '" + NDSY2 + "'))");
                    }
                    else
                    {
                        arr.Add(" ((left(tbrq,4) >= '" + NDSY1 + "') and (left(tbrq,4) <= '" + NDSY2 + "'))");
                    }
                }
                else
                {
                    if (m_DataAccess_SDE_.ProviderIsOraDB())
                    {
                        arr.Add(" (substr(tbrq,1,4) = '" + NDSY1 + "')");
                    }
                    else
                    {
                        arr.Add(" (substr(tbrq,1,4) = '" + NDSY1 + "')");
                    }
                }
            }

            //核查来源
            if (HCLY != "")
            {
                if (HCLY == "国土资源部核查")
                {
                    arr.Add("ss='部'");
                }
                else if (HCLY == "黑龙江省核查")
                {
                    arr.Add("ss='省'");
                }
                else
                {
                }
            }

            for (int i = 0; i < arr.Count - 1; i++)
            {
                SQLString += "(" + arr[i].ToString() + ")";
                SQLString += "and ";
            }
            if (arr.Count != 0)
            {
                SQLString += "(" + arr[arr.Count - 1].ToString() + ")";
            }

            return SQLString;
        }

        //土地监察汇总表
        private string getSQLString(string NDSY, string HCLY, string QHDM)
        {
            string SQLString = "";
            ArrayList arr = new ArrayList();
            //年度索引
            if (NDSY != "")
            {
                if (m_DataAccess_SDE_.ProviderIsOraDB())
                {
                    arr.Add(" substr(tbrq,1,4)='" + NDSY + "'"); // 原始 刘扬 修改 201
                }
                else
                {
                    arr.Add(" left(tbrq,4)='" + NDSY + "'"); // 原始 刘扬 修改 201
                    //arr.Add(" left(hccs,4)='" + NDSY + "'"); // 刘扬 修改 2011
                }
            }
            //核查来源
            if (HCLY != "")
            {
                if (HCLY == "国土资源部核查")
                {
                    arr.Add("ss='部'");
                }
                else if (HCLY == "黑龙江省核查")
                {
                    arr.Add("ss='省'");
                }
                else
                {
                }
            }

            for (int i = 0; i < arr.Count - 1; i++)
            {
                SQLString += "(" + arr[i].ToString() + ")";
                SQLString += "and ";
            }
            if (arr.Count != 0)
            {
                SQLString += "(" + arr[arr.Count - 1].ToString() + ")";
            }



            return SQLString;

        }

        private void DBFXGJBtnOk_Click(object sender, EventArgs e)
        {
            string NDSY = getNDSY(this.DBFXGJNDComB);
            string QHDM = uctXZQTree_Dev_WFYDGJFX.m_strXZQDM ;
            string HCLY = "";

            string SQLString = getSQLString(NDSY, HCLY, QHDM);
            string str = "";
            string zqm = uctXZQTree_Dev_WFYDGJFX.m_strXZQMC ;
            if (QHDM == "21")
            {
                if (SQLString != "")
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select sum(zyd_hj_mj_zs),sum(zyd_xzjsyd_wfyd_wfydmj_zs),substr(qhdm,1,4) from 执法检查数据汇总表 where " + SQLString + "group by substr(qhdm,1,4)";
                    }
                    else
                    {
                        str = "select sum(zyd_hj_mj_zs),sum(zyd_xzjsyd_wfyd_wfydmj_zs),left(qhdm,4) from 执法检查数据汇总表 where " + SQLString + "group by left(qhdm,4)";
                    }
                }
                else
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select  sum(zyd_hj_mj_zs),sum(zyd_xzjsyd_wfyd_wfydmj_zs),substr(qhdm,1,4) from 执法检查数据汇总表 group by substr(qhdm,1,4)";
                    }
                    else
                    {
                        str = "select  sum(zyd_hj_mj_zs),sum(zyd_xzjsyd_wfyd_wfydmj_zs),left(qhdm,4) from 执法检查数据汇总表 group by left(qhdm,4)";
                    }
                }

            }
            else
            {
                if (SQLString != "")
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select zyd_hj_mj_zs,zyd_xzjsyd_wfyd_wfydmj_zs,city from 执法检查数据汇总表 where substr(qhdm,1,4)='" + QHDM + "' and " + SQLString;
                    }
                    else
                    {
                        str = "select zyd_hj_mj_zs,zyd_xzjsyd_wfyd_wfydmj_zs,city from 执法检查数据汇总表 where left(qhdm,4)='" + QHDM + "' and " + SQLString;
                    }
                }
                else
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        str = "select zyd_hj_mj_zs,zyd_xzjsyd_wfyd_wfydmj_zs,city from 执法检查数据汇总表 where substr(qhdm,1,4)='" + QHDM + "'";
                    }
                    else
                    {
                        str = "select zyd_hj_mj_zs,zyd_xzjsyd_wfyd_wfydmj_zs,city from 执法检查数据汇总表 where left(qhdm,4)='" + QHDM + "'";
                    }
                }
            }

            DataRowCollection m_DataRowCollection = m_DataAccess_SYS_.getDataRowsByQueryString(str);
            frmGJFX m_frmGJFX = new frmGJFX();
            m_frmGJFX.QHDM = QHDM;
            m_frmGJFX.ZQM = zqm;
            m_frmGJFX.m_DataAccess_SYS = m_DataAccess_SYS_;
            m_frmGJFX.m_DataRowCollection = m_DataRowCollection;


            m_frmGJFX.MdiParent = this.ParentForm ;
            m_frmGJFX.Show();
        }

        private void btnMapCompare_Click(object sender, EventArgs e)
        {
            string[] m_strMapName;
            if (combMap1.SelectedItem == null || combMap2.SelectedItem == null)
            {
                if (combMap1.SelectedItem == null) combMap1.Focus();
                if (combMap2.SelectedItem == null) combMap2.Focus();
                m_DataAccess_SYS_.MessageInforShow(this.ParentForm , "请先选择显示的地图");
                return;

            }
            frmMapViews m_frmMapViews = new frmMapViews((MainFrame.frmMain)this.ParentForm);
            if (radTwoMaps.Checked)
            {
                m_frmMapViews.m_intMapCount = 2;
                m_strMapName = new string[2];
                m_strMapName[0] = combMap1.SelectedItem.ToString();
                m_strMapName[1] = combMap2.SelectedItem.ToString();
            }
            else
            {
                m_frmMapViews.m_intMapCount = 4;

                if (combMap1.SelectedItem == null || combMap2.SelectedItem == null)
                {
                    if (combMap3.SelectedItem == null) combMap3.Focus();
                    if (combMap4.SelectedItem == null) combMap4.Focus();
                    m_DataAccess_SYS_.MessageInforShow(this.ParentForm , "请先选择显示的地图");
                    return;
                }
                m_strMapName = new string[4];

                m_strMapName[0] = combMap1.SelectedItem.ToString();
                m_strMapName[1] = combMap2.SelectedItem.ToString();
                m_strMapName[2] = combMap3.SelectedItem.ToString();
                m_strMapName[3] = combMap4.SelectedItem.ToString();
            }
            m_frmMapViews.m_strCurrentMapFileName = m_strCurrentMapFileName_;
            m_frmMapViews.MdiParent = this.ParentForm;
            m_frmMapViews.m_DataAccess_SYS = m_DataAccess_SYS_;
            m_frmMapViews.m_strMapName = m_strMapName;
            m_frmMapViews.Show();
        }

        private void navigationPane1_Load(object sender, EventArgs e)
        {
            combMap1.SelectedIndex = 0;
            combMap2.SelectedIndex = 1;
            combMap3.SelectedIndex = 2;
            combMap4.SelectedIndex = 3;
        }

        private void radFourMaps_CheckedChanged(object sender, EventArgs e)
        {
                combMap3.Enabled = radFourMaps.Checked;
                combMap4.Enabled = radFourMaps.Checked;
        }

       

      
    }
}
