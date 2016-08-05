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
        private ListViewItem lv;
        private string target_title = "指标管理";
        public Target(string target_title)
        {
            this.target_title = target_title;
            InitializeComponent();
        }

        private void Target_Load(object sender, EventArgs e)
        {
            listviewSet(target_list);
            listviewSet(target_list2);
            addTarget(target_list, targetManager.land_target);
            addTarget(target_list, targetManager.risk_target);
            addTarget(target_list, targetManager.ecology_target);
            addTarget(target_list, targetManager.final_target);
            if (this.target_title != "指标管理")
            {
                this.information.Visible = true;
            }
            else
                this.information.Visible = false;
                
            
            //addTarget(target_list2, targetManager.land_t1);
            /*ListViewItem lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = targetManager.land_target;
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = targetManager.risk_target;
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = targetManager.ecology_target;
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = targetManager.final_target;
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            //lv.ImageIndex = 0;
            lv.Text = "交通条件";
            listView2.Items.Add(lv);*/
        }
        private void addTarget(System.Windows.Forms.ListView listview, string targetName)
        {
            lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = targetName;
            listview.Items.Add(lv);
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

        private void target_list_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
           /* int t_count = target_list.Items.Count;
            target_list2.Items.Clear();
            for (int i = 0; i < t_count; i++)
            {
                if (target_list.Items[i].Checked && target_list.Items[i].Text == targetManager.land_target)
                {
                    //加载土地适宜性评价指标
                    targetManager.add_t(targetManager.land_target, target_list2);
                }else
                if (target_list.Items[i].Checked && target_list.Items[i].Text == targetManager.risk_target)
                {
                    //加载土地灾害风险评价指标
                    targetManager.add_t(targetManager.risk_target, target_list2);
                }else
                    if (target_list.Items[i].Checked && target_list.Items[i].Text == targetManager.ecology_target)
                    {
                        //加载土地生态功能评价指标
                        targetManager.add_t(targetManager.ecology_target, target_list2);
                    }
                    else
                    {
                        targetManager.add_t(targetManager.land_target, target_list2);
                        targetManager.add_t(targetManager.risk_target, target_list2);
                        targetManager.add_t(targetManager.ecology_target, target_list2);

                    }
                

            }*/
        }

        private void target_list_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            int t_count = target_list.Items.Count;
            target_list2.Items.Clear();
            for (int i = 0; i < t_count; i++)
            {
                if (target_list.Items[i].Checked && target_list.Items[i].Text == targetManager.land_target)
                {
                    //加载土地适宜性评价指标
                    targetManager.add_t(targetManager.land_target, target_list2);
                }

                if (target_list.Items[i].Checked && target_list.Items[i].Text == targetManager.risk_target)
                {
                    //加载土地灾害风险评价指标
                    targetManager.add_t(targetManager.risk_target, target_list2);
                }

                if (target_list.Items[i].Checked && target_list.Items[i].Text == targetManager.ecology_target)
                {
                    //加载土地生态功能评价指标
                    targetManager.add_t(targetManager.ecology_target, target_list2);
                }
                if (target_list.Items[i].Checked && target_list.Items[i].Text == targetManager.final_target)
                {
                    targetManager.add_t(targetManager.land_target, target_list2);
                    targetManager.add_t(targetManager.risk_target, target_list2);
                    targetManager.add_t(targetManager.ecology_target, target_list2);

                }


            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            taskControl.setTask(target_title);
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            taskControl.setWeight(target_title);
            this.Close();
        }
    }
}
