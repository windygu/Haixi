using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComprehensiveEvaluation
{
    public partial class new_task : Form
    {
        private string evaluation_type = null;
        public string land_db;
        public string risk_db;
        public string ecology_db;
        public new_task(string E_type)
        {
            InitializeComponent();
            this.evaluation_type = E_type;
        }

        private void new_task_Load(object sender, EventArgs e)
        {
            e_type.Text = evaluation_type;
            if (evaluation_type == "土地适宜性评价")
            {
                risk_choose.Enabled = ecology_choose.Enabled = false;
            }
            if (evaluation_type == "土地灾害风险评价")
                land_choose.Enabled = ecology_choose.Enabled = false;
            if(evaluation_type=="土地生态功能评价")
                land_choose.Enabled = risk_choose.Enabled = false;
            
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            land_db = land_data.Text;
            risk_db = risk_data.Text;
            ecology_db = ecology_data.Text;
            this.Close();
        }

    }
}
