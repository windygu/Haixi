using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComprehensiveEvaluation.TargetSystem
{
    public partial class Target : Form
    {
        public Target()
        {
            InitializeComponent();
        }

        private void Target_Load(object sender, EventArgs e)
        {
            listviewSet(listView1);
            listviewSet(listView2);
            ListViewItem lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = "土地适宜性指标";
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = "土地生态安全指标";
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = "土地灾害风险指标";
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = "综合评价指标";
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            //lv.ImageIndex = 0;
            lv.Text = "交通条件";
            listView2.Items.Add(lv);
        }
        private void listviewSet(System.Windows.Forms.ListView listview)
        {
            listview.SmallImageList = imageList1;
            listview.MultiSelect = false;
            listview.GridLines = true;
            listview.FullRowSelect = true;
            listview.View = View.Details;
            listview.CheckBoxes = true;
        }
    }
}
