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
    public partial class Waitform : Form
    {
        public Waitform()
        {
            InitializeComponent();
        }

        private void Waitform_Load(object sender, EventArgs e)
        {
            
            //wait_info.Text = "正在计算参数值···";
            //System.Threading.Thread.Sleep(3000);
            //wait_info.Text = "剔除限制区域···";
            //System.Threading.Thread.Sleep(3000);
            //wait_info.Text = "叠置分析···";
            //System.Threading.Thread.Sleep(2000);
            //wait_info.Text = "计算评价结果···"; 
            //System.Threading.Thread.Sleep(3000);
            //wait_info.Text = "正在分类统计···";
            //System.Threading.Thread.Sleep(3000);
            wait_info.Text = "加载数据···";
           
        }
    }
}
