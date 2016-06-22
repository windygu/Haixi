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
    public partial class Weight : Form
    {
        public Weight()
        {
            InitializeComponent();
        }

        private void Weight_Load(object sender, EventArgs e)
        {
            //listView1.Columns.Add("专家名称",200);
            //listView2.Columns.Add("模块指标体系选择",200);
            //listView3.Columns.Add("因素", 100);
            //listView3.Columns.Add("打分", 100);
            //listView3.Columns.Add("权重", 100);
            listviewSet(listView1); 
            listviewSet(listView2); 
            listviewSet(listView3);

            ListViewItem lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = "Pro.Li";
            //lv.SubItems.Add("这是一个地图模板");
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 1;
            lv.Text = "土地适宜性指标";
            listView2.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 1;
            lv.Text = "土地生态安全指标";
            listView2.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 1;
            lv.Text = "土地灾害风险指标";
            listView2.Items.Add(lv);
            lv = new ListViewItem();
            lv.Text = "交通条件";
            lv.SubItems.Add("16.00");
            lv.SubItems.Add("0.16");
            listView3.Items.Add(lv);

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

        private void buttonX3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
