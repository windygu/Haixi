using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame
{
    public partial class uctWPZB : UserControl
    {
        private string m_strHCSJQueryTableNAME = "TDHC_2009_R";//核查数据表名称
        ESRI.ArcGIS.Controls.AxMapControl _m_AxMapControl;
        public clsDataAccess.DataAccess m_DataAccess_SYS;

        ////定义一个委托，用于将数据传送到显示结果窗体中,在点击查询按钮时调用
        public delegate void uctWPZBEventHandler(DataTable p_DataTable);
        public event uctWPZBEventHandler uctWPZBQueryEvent;

        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                _m_AxMapControl = value;
                uctXZQTree1.m_AxMapControl = _m_AxMapControl;
            }
        }
        public uctWPZB()
        {
            InitializeComponent();
            //m_strHCSJQueryTableNAME = "";
        }

        private void txtYDDW_KeyDown(object sender, KeyEventArgs e)
        {
            chkYDDW.Checked = true;
        }

        private void txtDKBH_KeyDown(object sender, KeyEventArgs e)
        {
            chkDKBH.Checked = true;
        }

        private void comDKFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkDKFL.Checked = true;
        }

        private void txtSJYT_KeyDown(object sender, KeyEventArgs e)
        {
            chkSJYT.Checked = true;
        }

        private void txtXMLX_KeyDown(object sender, KeyEventArgs e)
        {
            chkXMLX.Checked = true;
        }

        private void comHFXSC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comHFXSC.SelectedIndex >= 0)
            {
                chkHFXSC.Checked = true;
            }
        }

        private void comWFLX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comWFLX.SelectedIndex >= 0)
            {
                chkWFLX.Checked = true;
            }
        }

        private void btnHCSJQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //uctXZQTree1.Init();
                string m_strXZQDM = "";
                if (uctXZQTree1.TreeView.SelectedNode != null)
                {
                    m_strXZQDM = uctXZQTree1.m_strXZQDM;
                }
                string m_strWFLX, m_strHFXSC, m_strXMLX, m_strSJYT, m_strDKFL, m_strYDDW, m_strDKBH;
                m_strWFLX = m_strHFXSC = m_strXMLX = m_strSJYT = m_strDKFL = m_strYDDW = m_strDKBH = "";
                string m_strSQL = "";
                string m_strSQL_Temp = "";

                m_strSQL = "SELECT objectid , XMC AS 县区名, XZMC AS 乡镇名称,JCBH AS 监测编号,JCMJ AS 监测面积,BZ AS 备注, DKBH AS 地块编号,";
                m_strSQL = m_strSQL + "DKFL AS 	地块分类,DKMJ AS 	地块面积,NYDMJ AS 	农用地面积,GDMJ AS 	耕地面积,";
                m_strSQL = m_strSQL + "JBNTMJI	 AS 基本农田面积,WLYDMJ AS 	未利用地面积,SJYT AS 	实际用途,YDDW AS 	用地单位,";
                m_strSQL = m_strSQL + "XMLX	 AS 项目类型,HFXSC AS 合法性审查,WFLX AS 	违法类型 FROM " + m_strHCSJQueryTableNAME;
                m_strSQL_Temp = m_strSQL;
                if (chkYDDW.Checked == true)
                {
                    m_strYDDW = txtYDDW.Text;
                    m_strSQL = m_strSQL + " where YDDW='" + m_strYDDW + "'";
                }



                if (chkXMLX.Checked == true)
                {
                    m_strXMLX = txtXMLX.Text;

                    if (m_strSQL == m_strSQL_Temp)
                    {
                        m_strSQL = m_strSQL + " where XMLX='" + m_strXMLX + "'";
                    }
                    else
                    {
                        m_strSQL = m_strSQL + " and  XMLX='" + m_strXMLX + "'";
                    }

                }
                if (chkWFLX.Checked == true)
                {
                    m_strWFLX = comWFLX.SelectedItem.ToString();
                    if (m_strSQL == m_strSQL_Temp)
                    {
                        m_strSQL = m_strSQL + " where WFLX like '%" + m_strWFLX + "%'";
                    }
                    else
                    {
                        m_strSQL = m_strSQL + " and  WFLX='" + m_strWFLX + "'";
                    }
                }
                if (chkSJYT.Checked == true)
                {
                    m_strSJYT = txtSJYT.Text;
                    if (m_strSQL == m_strSQL_Temp)
                    {
                        m_strSQL = m_strSQL + " where SJYT='" + m_strSJYT + "'";
                    }
                    else
                    {
                        m_strSQL = m_strSQL + " and  SJYT='" + m_strSJYT + "'";
                    }
                }
                if (chkHFXSC.Checked == true)
                {
                    m_strHFXSC = comHFXSC.SelectedItem.ToString();
                    if (m_strSQL == m_strSQL_Temp)
                    {
                        if (m_strHFXSC != "未调查")
                        {
                            m_strSQL = m_strSQL + " where HFXSC like '%" + m_strHFXSC + "%'";
                        }
                        else
                        {
                            m_strSQL = m_strSQL + " where ( HFXSC ='0' or HFXSC ='')";
                        }
                    }
                    else
                    {
                        m_strSQL = m_strSQL + " and  HFXSC='" + m_strHFXSC + "'";
                    }
                }
                if (chkDKFL.Checked == true)
                {
                    m_strDKFL = comDKFL.SelectedItem.ToString();
                    if (m_strSQL == m_strSQL_Temp)
                    {
                        m_strSQL = m_strSQL + " where  DKFL like '%" + m_strDKFL + "%'";
                    }
                    else
                    {
                        m_strSQL = m_strSQL + " and  DKFL='" + m_strDKFL + "'";
                    }
                }
                if (chkDKBH.Checked == true)
                {
                    m_strDKBH = txtDKBH.Text;
                    if (m_strSQL == m_strSQL_Temp)
                    {
                        m_strSQL = m_strSQL + " where DKBH='" + m_strDKBH + "'";
                    }
                    else
                    {
                        m_strSQL = m_strSQL + " and  DKBH='" + m_strDKBH + "'";
                    }
                }

                if (m_strXZQDM == "")
                {
                    //全部数据
                }
                else
                {
                    if (m_strSQL == m_strSQL_Temp)
                    {
                        if (m_DataAccess_SYS.ProviderIsOraDB())
                        {
                            m_strSQL = m_strSQL + " where substr(xzqdm,1," + m_strXZQDM.Length.ToString() + ")='" + m_strXZQDM + "'";
                        }
                        else
                        {
                            m_strSQL = m_strSQL + " where left(xzqdm," + m_strXZQDM.Length.ToString() + ")='" + m_strXZQDM + "'";
                        }
                    }
                    else
                    {
                        if (m_DataAccess_SYS.ProviderIsOraDB())
                        {
                            m_strSQL = m_strSQL + " and  substr(xzqdm,1," + m_strXZQDM.Length.ToString() + ")='" + m_strXZQDM + "'";
                        }
                        else
                        {
                            m_strSQL = m_strSQL + " and  left(xzqdm," + m_strXZQDM.Length.ToString() + ")='" + m_strXZQDM + "'";
                        }
                    }
                }
                m_strSQL = m_strSQL + " order by xzqdm";
                DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);

                if (m_DataTable == null || m_DataTable.Rows.Count < 1)
                {
                    m_DataAccess_SYS.MessageInforShow(this.ParentForm, "未找到与查询条件符合的数据！\n 请检查条件设置情况！");
                }
                else
                {
                    uctWPZBQueryEvent(m_DataTable);
                }
            }catch(SystemException errs)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this.ParentForm, errs.Message);
            }

        }

        private void btnHCSJQuery_AD_Click(object sender, EventArgs e)
        {

        }

       
    }
}
