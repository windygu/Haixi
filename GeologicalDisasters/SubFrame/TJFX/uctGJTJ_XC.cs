using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame.TJFX
{
    public partial class uctGJTJ_XC : UserControl
    {
        private JCZF.SubFrame.WGGL.clsZFDW m_clsZFDW;
        private JCZF.SubFrame.WGGL.clsZFRY m_clsZFRY;

        public string m_strAction;
        public DevComponents.DotNetBar.ExpandablePanel m_ExpandablePanel_Main;
        public Panel m_Panel;
        private GPS.uctGJShow m_uctGJShow;

        public clsDataAccess.DataAccess m_DataAccess_SYS;
        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                m_AxMapControl_ = value;
                uctXZQTree_Dev_XCCQ.m_AxMapControl = value;
                uctXZQTree1.m_AxMapControl = value;
            }
        }


        public uctGJTJ_XC()
        {
            InitializeComponent();
            uctXZQTree_Dev_XCCQ.uctXZQTree_DevSelectEvent += new XZQTree.uctXZQTree_Dev.uctXZQTree_DevSelectEventHandler(uctXZQTree_Dev_XCCQ_uctXZQTree_DevSelectEvent);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DateTime dtStar = dtInputStar.Value;
            DateTime dtEnd = dtInputEnd.Value;

            string m_strXZQDM = "";

            m_strXZQDM = uctXZQTree_Dev_XCCQ.m_strXZQDM;

            try
            {
                string m_strCommandText = "";

                m_strCommandText = CreatSQL();
                DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strCommandText);



                m_ExpandablePanel_Main.Text = "巡查轨迹管理";
                m_ExpandablePanel_Main.TitleText = "巡查轨迹管理";

                if (m_DataTable == null)
                {
                    m_ExpandablePanel_Main.Visible = false ;
                    return;
                }


                if (m_Panel == null)
                {
                    m_Panel = (Panel)clsFunction.Function.GetControl(m_ExpandablePanel_Main, "PanelInfo_Panel");
                    ////ClearControls(m_Panel);
                    //clsFunction.Function.ClearControls(m_Panel);
                }

                if (clsFunction.Function.HasControl(m_Panel, "m_uctGJShow") == false)
                {
                    clsFunction.Function.ClearControls(m_Panel);
                    m_uctGJShow = new GPS.uctGJShow();
                    m_uctGJShow.Name = "m_uctGJShow";
                   
                    m_uctGJShow.Dock = DockStyle.Fill;
                    m_Panel.Controls.Add(m_uctGJShow);
                }

                m_uctGJShow.m_strAction = m_strAction;
                
                m_uctGJShow.dataGridViewGuiJi.DataSource = m_DataTable;
                m_ExpandablePanel_Main.Visible = true;
                m_ExpandablePanel_Main.Expanded = true;
                m_uctGJShow.m_DataAccess_SYS = m_DataAccess_SYS;

            }
            catch (SystemException ex)
            {
                clsFunction.Function.MessageBoxError( ex.Message);
            }
            finally
            {
                //conn.Close();
            }
        }

        private string CreatSQL()
        {
            string m_strSQL0 = "";
            string m_strSQL1 = "";
            string m_strSQL2 = "";

            string m_strCommandText = "select * from DCGJ_RW_ZFRY where " + "  (时间 >'" + dtInputStar.Value.ToShortDateString() + "'  and  时间 <'" + dtInputEnd.Value.ToShortDateString() + "')";

            //if (chkXZQ.Checked)
            //{
            //    if (m_DataAccess_SYS.ProviderIsOraDB())
            //    {
            //        m_strSQL0 = "  (substr(行政区代码,1," + uctXZQTree_Dev_XCCQ.m_strXZQDM.Length + ")='" + uctXZQTree_Dev_XCCQ.m_strXZQDM + "') ";
            //    }
            //    else
            //    {
            //        m_strSQL0 = "   (LEFT(行政区代码," + uctXZQTree_Dev_XCCQ.m_strXZQDM.Length + ")='" + uctXZQTree_Dev_XCCQ.m_strXZQDM + "') ";
            //    }
            //}
            if (chkZFRY.Checked)
            {
                m_strSQL1 = " (执法人员编号='" + txtCQJGDXFS_RY.Text + "')";               
            }

            if (chkZFDW.Checked)
            {
                if (m_DataAccess_SYS.ProviderIsOraDB())
                {
                    m_strSQL2 =  "  ( substr(执法人员编号,1,8)='" + comCQJGDXFS_ZFDW.Items[comCQJGDXFS_ZFDW.SelectedIndex].ToString() + "' )";
                }
                else
                {
                    m_strSQL2 =  " ( left(执法人员编号,8)='" + comCQJGDXFS_ZFDW.Items[comCQJGDXFS_ZFDW.SelectedIndex].ToString() + "' )";
                }
            }

            if (m_strSQL0 != "")
            {
                m_strCommandText = m_strCommandText + " and " + m_strSQL0;
            }
            if (m_strSQL1 != "")
            {
                m_strCommandText = m_strCommandText + " and " + m_strSQL1;
            }
            if (m_strSQL2 != "")
            {
                m_strCommandText = m_strCommandText + " and " + m_strSQL2;
            }
            m_strCommandText = m_strCommandText + "  ORDER BY 时间";

            return m_strCommandText;
        }

        private void comCQJGDXFS_ZFDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comCQJGDXFS_ZFDW.SelectedValue != null)
            {
                FillComb_ZFRY(cmbZFRY, comCQJGDXFS_ZFDW.SelectedValue.ToString());
            }
            else
            {
                cmbZFRY.DataSource = null;
            }
        }

        private void FillComb_ZFRY(ComboBox p_ComboBox, string p_strZFDWDM)
        {
            try
            {
                if (m_clsZFRY == null) m_clsZFRY = new JCZF.SubFrame.WGGL.clsZFRY();
                m_clsZFRY.m_DataAccess_SYS_ = m_DataAccess_SYS;
                m_clsZFRY.m_strZFDW_ZFDWDM = p_strZFDWDM;

                DataTable m_DataTable = m_clsZFRY.GetZFRY();

                if (m_DataTable == null)
                {
                    p_ComboBox.DataSource = null;
                    return;
                }

                p_ComboBox.DataSource = m_DataTable.DefaultView;
                p_ComboBox.ValueMember = "ZFRYBH";
                p_ComboBox.DisplayMember = "ZFRYXM";

            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this.ParentForm, errs.Message);
            }
        }

        private void uctXZQTree_Dev_XCCQ_uctXZQTree_DevSelectEvent(string p_strXZQDM)
        {
            if (p_strXZQDM != "")
            {
                string m_strSQL = "";
                
                if (m_DataAccess_SYS.ProviderIsOraDB())
                {
                    m_strSQL = "SELECT zfdwmc,zfdwdm FROM ZFDW WHERE substr(ZFDWDM,1," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'";
                }
                else
                {
                    m_strSQL = "SELECT zfdwmc,zfdwdm FROM ZFDW WHERE LEFT(ZFDWDM," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'";
                }
                DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);

                comCQJGDXFS_ZFDW.DataSource = m_DataTable;
                comCQJGDXFS_ZFDW.DisplayMember = "zfdwmc";
                comCQJGDXFS_ZFDW.ValueMember = "zfdwdm";
                chkXZQ.Checked = true;
                chkZFDW.Checked = true;
            }
        }

        private void uctGJTJ_Load(object sender, EventArgs e)
        {
            dtInputStar.Value = DateTime.Now.AddMonths(-1);
            dtInputEnd.Value  = DateTime.Now;
        }

        private void uctXZQTree1_uctXZQTreeSelectEvent(string p_strXZQDM)
        {
            uctXZQTree_Dev_XCCQ_uctXZQTree_DevSelectEvent(p_strXZQDM);
        }

    }
}
