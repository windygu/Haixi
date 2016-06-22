using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Functions;

namespace JCZF.SubFrame
{
    public partial class uctWPZBResult : UserControl
    {
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl;
        public DataTable m_DataSource
        {
            set
            {
                dataGridViewX1.DataSource = value.DefaultView;
                dataGridViewX1.Columns[0].Visible = false;
            }
        }
        public uctWPZBResult()
        {
            InitializeComponent();
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Functions.MapFunction m_MapFunction = new Functions.MapFunction();
            MapFunction.QueryAndLocateFeatureByID(dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(), "土地核查", "objectid", m_AxMapControl);
        }
    }
}