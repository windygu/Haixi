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
    public partial class uctTJFX_1 : UserControl
    {
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                uctXZQTree.m_AxMapControl = value;
            }
        }

        //public bool  m_blTJSJFW
        //{
        //    get
        //    {
        //        return chkTJSJFW.Checked ;
        //    }
        //}
        private clsDataAccess.DataAccess m_clsDataAccess_;

        public clsDataAccess.DataAccess m_DataAccess_SYS
        {
            set
            {
                m_clsDataAccess_=value ;
            }
        }

        public string m_strXZQDM
        {
            get
            {             
                 return uctXZQTree.m_strXZQDM;                
            }
        }

        public string m_strDWDM
        {
            get
            {
                if (cmbDW.SelectedValue == null  )
                {
                    return "";
                }
                else
                {
                    return cmbDW.SelectedValue.ToString();
                }
            }
        }

        public string m_strRYBH
        {
            get
            {
                if (cmbRY.SelectedValue == null  )
                {
                    return "";
                }
                else
                {
                    return cmbRY.SelectedValue.ToString();
                }
            }
        }

        public DateTime  m_dateStartTime
        {
            get
            {
                return dateTimePicker_Start.Value;
            }
        }

        public DateTime m_dateEndTime
        {
            get
            {
                return dateTimePicker_End.Value;
            }
        }



        public uctTJFX_1()
        {
            InitializeComponent();
        }

        private void uctTJFX_1_Load(object sender, EventArgs e)
        {
            dateTimePicker_End.Value = DateTime.Now;
            //dateTimePicker_Start.Value = DateTime.Now;
            dateTimePicker_Start.Value = dateTimePicker_End.Value.AddMonths(-1);

            //uctXZQTree.uctXZQTree_DevSelectEvent+=new XZQTree.uctXZQTree_Dev.uctXZQTree_DevSelectEventHandler(uctXZQTree_uctXZQTreeSelectEvent);
        }

        //private void uctXZQTree_uctXZQTreeSelectEvent(string p_strXZQDM)
        //{
        //    string m_strSQL="";
        //    if (p_strXZQDM.Length<=6)
        //    {
        //    m_strSQL = "SELECT ZFDWDM,ZFDWMC FROM ZFDW WHERE LEFT(ZFDWDM," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'";
        //    }
        //    else 
        //    {

        //     m_strSQL = "SELECT ZFDWDM,ZFDWMC FROM ZFDW WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM + "'";
        //    }
        //    DataTable m_DataTable = m_clsDataAccess_.getDataTableByQueryString(m_strSQL);

        //    if (m_DataTable == null)
        //    {
        //        cmbDW.DataSource=null;
        //        cmbRY.DataSource=null;
        //        return;
        //    }
        //    cmbDW.DataSource = m_DataTable.DefaultView  ;
        //    cmbDW.ValueMember = "ZFDWDM";
        //    cmbDW.DisplayMember = "ZFDWMC";
        //}

        private void chkDW_CheckedChanged(object sender, EventArgs e)
        {
            cmbDW.Enabled = chkDW.Checked;
        }

        private void chkRY_CheckedChanged(object sender, EventArgs e)
        {
            cmbRY.Enabled = chkRY.Checked;
        }

        private void cmbDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDW.SelectedValue == null || cmbDW.SelectedValue.ToString() == "System.Data.DataRowView") return;
            string m_strSQL="";
            if (m_clsDataAccess_.ProviderIsOraDB())
            {
                m_strSQL = "SELECT ZFRYBH,ZFRYXM FROM ZFRY WHERE substr(ZFDWDM,1," + cmbDW.SelectedValue.ToString() + ")='" + cmbDW.SelectedValue.ToString() + "'";
            }
            else
            {
                m_strSQL = "SELECT ZFRYBH,ZFRYXM FROM ZFRY WHERE LEFT(ZFDWDM," + cmbDW.SelectedValue.ToString() + ")='" + cmbDW.SelectedValue.ToString() + "'";
            }
           
            DataTable m_DataTable = m_clsDataAccess_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable == null)
            {
                cmbRY.DataSource = null;
                return;
            }
            cmbRY.DataSource = m_DataTable.DefaultView ;
            cmbRY.ValueMember = "ZFRYBH";
            cmbRY.DisplayMember = "ZFRYXM";
        }

        private void uctXZQTree_uctXZQTree_DevSelectEvent(string p_strXZQDM)
        {
            string m_strSQL = "";
            if (p_strXZQDM.Length <= 6)
            {
                if (m_clsDataAccess_.ProviderIsOraDB())
                {
                    m_strSQL = "SELECT ZFDWDM,ZFDWMC FROM ZFDW WHERE substr(ZFDWDM,1," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'";
                }
                else
                {
                    m_strSQL = "SELECT ZFDWDM,ZFDWMC FROM ZFDW WHERE LEFT(ZFDWDM," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'";
                }
            }
            else
            {
                if (m_clsDataAccess_.ProviderIsOraDB())
                {
                    m_strSQL = "SELECT ZFDWDM,ZFDWMC FROM ZFDW WHERE substr(ZFDWDM,1,6)='" + p_strXZQDM + "'";
                }
                else
                {
                    m_strSQL = "SELECT ZFDWDM,ZFDWMC FROM ZFDW WHERE LEFT(ZFDWDM,6)='" + p_strXZQDM + "'";
                }
            }
            DataTable m_DataTable = m_clsDataAccess_.getDataTableByQueryString(m_strSQL);

            if (m_DataTable == null)
            {
                cmbDW.DataSource = null;
                cmbRY.DataSource = null;
                return;
            }
            cmbDW.DataSource = m_DataTable.DefaultView;
            cmbDW.ValueMember = "ZFDWDM";
            cmbDW.DisplayMember = "ZFDWMC";
        }

       

       

       
    }
}
