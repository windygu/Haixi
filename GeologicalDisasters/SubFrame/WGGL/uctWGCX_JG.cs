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
    public partial class uctWGCX_JG : UserControl
    {
        //public clsDataAccess.DataAccess m_DataAccess_SYS;
        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                m_AxMapControl_ = value;

            }
        }

        //public delegate void uctWGCX_JGEventHandler(string  p_strXZQDM);
        //public event uctWGCX_JGEventHandler uctWGCX_EventJGClickEvent;

        public uctWGCX_JG()
        {
            InitializeComponent();
        }

        private void dataGridViewX2_Click(object sender, EventArgs e)
        {
            if (m_DataGridView.SelectedRows.Count > 0)
            {
                GoToSelectItem(m_DataGridView.SelectedRows[0].Cells["行政区代码"].Value.ToString());//将行政区代码传出去
            }
        }

        private void GoToSelectItem(string p_strXZQDM)
        {
            try
            {
                if (p_strXZQDM.Length == 2)
                {
                    clsMapFunction.MapFunction.QueryAndLocateFeatureByID(p_strXZQDM, "省", "dm", m_AxMapControl_);

                }
                else if (p_strXZQDM.Length == 4)
                {
                    clsMapFunction.MapFunction.QueryAndLocateFeatureByID(p_strXZQDM, "市", "dm", m_AxMapControl_);
                }
                else if (p_strXZQDM.Length == 6)
                {
                    clsMapFunction.MapFunction.QueryAndLocateFeatureByID(p_strXZQDM, "县", "dm", m_AxMapControl_);
                }
                else if (p_strXZQDM.Length == 9)
                {
                    clsMapFunction.MapFunction.QueryAndLocateFeatureByID(p_strXZQDM, "乡", "qhdm", m_AxMapControl_);
                }
                else if (p_strXZQDM.Length == 12)
                {
                    clsMapFunction.MapFunction.QueryAndLocateFeatureByID(p_strXZQDM, "村", "xzqdm", m_AxMapControl_);
                }
            }
            catch (SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
        }
    }
}
