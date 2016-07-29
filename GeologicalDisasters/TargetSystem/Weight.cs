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
            listviewSet(Pro_weight); 
            listviewSet(target_name); 
            listviewSet(target_weight);
            addTarget(target_name, targetManager.land_target);
            addTarget(target_name, targetManager.risk_target);
            addTarget(target_name, targetManager.ecology_target);
            addTarget(target_name, targetManager.final_target);
          
            addTarget(Pro_weight, targetManager.Pro_n1 );
            addTarget(Pro_weight, targetManager.Pro_n2 );
            addTarget(Pro_weight, targetManager.Pro_n3 );
            addTarget(Pro_weight, targetManager.Pro_n4 );
            addTarget(Pro_weight, targetManager.Pro_n5 );
            addTarget(Pro_weight, targetManager.Pro_n6 );
            addTarget(Pro_weight, targetManager.Pro_n7 );
            addTarget(Pro_weight, targetManager.Pro_n8 );
            addTarget(Pro_weight, targetManager.Pro_n9 );
            addTarget(Pro_weight, targetManager.Pro_n10);

           

            /*ListViewItem lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = "Pro.Li";
            //lv.SubItems.Add("这是一个地图模板");
            Pro_weight.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 1;
            lv.Text = "土地适宜性指标";
            target_name.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 1;
            lv.Text = "土地生态安全指标";
            target_name.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 1;
            lv.Text = "土地灾害风险指标";
            target_name.Items.Add(lv);
            lv = new ListViewItem();
            lv.Text = "交通条件";
            lv.SubItems.Add("16.00");
            lv.SubItems.Add("0.16");
            target_weight.Items.Add(lv);
*/
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
            targetManager.IsWeight = false;
            this.Close();
        }

        private void target_name_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            int t_count = target_name.Items.Count;
            target_name.Items.Clear();
            for (int i = 0; i < t_count; i++)
            {
                if (target_name.Items[i].Checked && target_name.Items[i].Text == targetManager.land_target)
                {
                    //加载土地适宜性评价指标
                    targetManager.add_t(targetManager.land_target, target_weight);
                }

                if (target_name.Items[i].Checked && target_name.Items[i].Text == targetManager.risk_target)
                {
                    //加载土地灾害风险评价指标
                    targetManager.add_t(targetManager.risk_target, target_weight);
                }

                if (target_name.Items[i].Checked && target_name.Items[i].Text == targetManager.ecology_target)
                {
                    //加载土地生态功能评价指标
                    targetManager.add_t(targetManager.ecology_target, target_weight);
                }
                if (target_name.Items[i].Checked && target_name.Items[i].Text == targetManager.final_target)
                {
                    targetManager.add_t(targetManager.land_target, target_weight);
                    targetManager.add_t(targetManager.risk_target, target_weight);
                    targetManager.add_t(targetManager.ecology_target, target_weight);

                }


            }
        }

        private void addTarget(System.Windows.Forms.ListView listview, string targetName)
        {
            ListViewItem lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = targetName;
            if (targetManager.IsWeight)
            {
                lv.SubItems.Add("16.00");
                lv.SubItems.Add("0.16");
            }
            listview.Items.Add(lv);
        }
    }
}
