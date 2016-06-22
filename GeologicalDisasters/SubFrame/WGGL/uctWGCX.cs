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
    public partial class uctWGCX : UserControl
    {
        public DevComponents.DotNetBar.ExpandablePanel m_ExpandablePanel_Main;
        public Panel m_Panel;

        private uctWGCX_JG m_uctWGCX_JG;

        ////定义一个委托，用于将数据传送到显示结果窗体中,在点击查询按钮时调用
        public delegate void uctWGCX_EventHandler(DataTable p_DataTable);
        public event uctWGCX_EventHandler uctWGCX_EventQueryEvent;

        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                uctXZQTree_DevXZQ.m_AxMapControl = value;               
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

        public uctWGCX()
        {
            InitializeComponent();
        }

        private void btnXZQ_LS_Click(object sender, EventArgs e)
        {
            GetXZQData(true);
        }

        private void btnRY_LS_Click(object sender, EventArgs e)
        {
            GetRYData(true, txtRY.Text);
        }

        private void btnXZQ_Click(object sender, EventArgs e)
        {
            GetXZQData(false);
        }

        private void GetXZQData(bool p_blIsLS)
        {
            string m_strSQL = "";
            string m_strXZQDM = uctXZQTree_DevXZQ.m_strXZQDM;
            DataTable m_DataTable;
            if (chkBHXJXZQ.Checked == true)
            {
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    m_strSQL = "select * from zfwg_zfry_zfdw where substr(行政区代码,1," + m_strXZQDM.Length + ")='" + m_strXZQDM + "'";
                }
                else
                {
                    m_strSQL = "select * from zfwg_zfry_zfdw where left(行政区代码," + m_strXZQDM.Length + ")='" + m_strXZQDM + "'";
                }
            }
            else if (chkBHXJXZQ.Checked == false)
            {
                if (m_strXZQDM.Length == 2)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = "select * from zfwg_zfry_zfdw where length(行政区代码)=4";
                    }
                    else
                    {
                        m_strSQL = "select * from zfwg_zfry_zfdw where len(行政区代码)=4";
                    }
                }
                else if (m_strXZQDM.Length == 4)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = "select * from zfwg_zfry_zfdw where length(行政区代码)=6 and substr(行政区代码,1,4)='" + m_strXZQDM + "' or 行政区代码='" + m_strXZQDM + "'";
                    }
                    else
                    {
                        m_strSQL = "select * from zfwg_zfry_zfdw where len(行政区代码)=6 and left(行政区代码,4)='" + m_strXZQDM + "' or 行政区代码='" + m_strXZQDM + "'";
                    }
                }
                else if (m_strXZQDM.Length == 6)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = "select * from zfwg_zfry_zfdw where length(行政区代码)=9 and substr(行政区代码,1,6)='" + m_strXZQDM + "' or 行政区代码='" + m_strXZQDM + "'";
                    }
                    else
                    {
                        m_strSQL = "select * from zfwg_zfry_zfdw where len(行政区代码)=9 and left(行政区代码,6)='" + m_strXZQDM + "' or 行政区代码='" + m_strXZQDM + "'";
                    }
                }
                else if (m_strXZQDM.Length == 9)
                {
                    if (m_DataAccess_SYS_.ProviderIsOraDB())
                    {
                        m_strSQL = "select * from zfwg_zfry_zfdw where length(行政区代码)=12  and substr(行政区代码,1,9)='" + m_strXZQDM + "' or 行政区代码='" + m_strXZQDM + "'";
                    }
                    else
                    {
                        m_strSQL = "select * from zfwg_zfry_zfdw where len(行政区代码)=12  and left(行政区代码,9)='" + m_strXZQDM + "' or 行政区代码='" + m_strXZQDM + "'";
                    }
                }
                else
                {
                    m_strSQL = "select * from zfwg_zfry_zfdw where 行政区代码='" + m_strXZQDM + "'";
                }
            }
            if (p_blIsLS)
            {
                m_strSQL = m_strSQL + " and 结束时间 is not null";
            }
            else 
            {
                m_strSQL = m_strSQL + " and 结束时间 is null";
            }

            m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);

            uctWGCX_Query(m_DataTable);
            //if (uctWGCX_EventQueryEvent != null)
            //{
            //    uctWGCX_EventQueryEvent(m_DataTable);
            //}
        }

        private void uctWGCX_Query(DataTable p_DataTable)
        {
            if (m_Panel == null)
            {
                m_Panel = (Panel)clsFunction.Function.GetControl(m_ExpandablePanel_Main, "PanelInfo_Panel");
                ////ClearControls(m_Panel);
                clsFunction.Function.ClearControls(m_Panel);
            }

            m_ExpandablePanel_Main.Text = "执法网格查询结果";
            m_ExpandablePanel_Main.TitleText = "执法网格查询结果";
            //m_TabControl_Main.Visible = false;

            if (clsFunction.Function.HasControl(m_Panel, "m_uctWGCX_JG") == false)
            {
                clsFunction.Function.ClearControls(m_Panel);
                m_uctWGCX_JG = new uctWGCX_JG();
                m_uctWGCX_JG.Name = "m_uctWGCX_JG";
                m_uctWGCX_JG.Dock = DockStyle.Fill;
                m_Panel.Controls.Add(m_uctWGCX_JG);
            }

             m_uctWGCX_JG.m_AxMapControl = m_AxMapControl_;
           m_uctWGCX_JG.m_DataGridView.DataSource = p_DataTable;
            m_ExpandablePanel_Main.Visible = true;
            m_ExpandablePanel_Main.Expanded = true;
           
           

        }


        private void GetRYData(bool p_blIsLS, string p_strZFRYXM)
        {
            string m_strSQL = "";

            DataTable m_DataTable;

            if (p_strZFRYXM == "")
            {
                m_strSQL = "select * from zfwg_zfry_zfdw";
                if (p_blIsLS)
                {
                    m_strSQL = m_strSQL + " where 结束时间 <>null";
                }
            }
            else
            {
                if (rdbIndistinct.Checked)
                {
                    m_strSQL = "select * from zfwg_zfry_zfdw where (本级执法人员姓名 like'%" + p_strZFRYXM + "%' OR  上级执法人员姓名 like'%" + p_strZFRYXM + "%')";
                }
                else
                {
                    m_strSQL = "select * from zfwg_zfry_zfdw where (本级执法人员姓名='" + p_strZFRYXM + "' OR  上级执法人员姓名='" + p_strZFRYXM + "')";
                }
                if (p_blIsLS)
                {
                    m_strSQL = m_strSQL + " and 结束时间 <>null";
                }
            }



            m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (uctWGCX_EventQueryEvent != null)
            {
                uctWGCX_EventQueryEvent(m_DataTable);
            }
        }

        private void btnRY_Click(object sender, EventArgs e)
        {
            GetRYData(false, txtRY.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rdbDQWG.Checked == true)
            {
                if (radioXZQ.Checked)
                {
                    btnXZQ_Click(sender, e);
                }
                else
                {
                    btnRY_Click(sender, e);
                }
                
            }
            else
            {
                if (radioXZQ.Checked)
                {
                    btnXZQ_LS_Click(sender, e);
                }
                else
                {
                    btnRY_LS_Click(sender, e);
                }
            }
        }
      
    }
}
