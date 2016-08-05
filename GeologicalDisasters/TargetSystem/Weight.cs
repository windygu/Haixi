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
        private ListViewItem lv_Pro;
        private ListViewItem lv_Tar;
        private ListViewItem lv_Wgt;

        private string W_Type="权重管理";

        public Weight(string weight)
        {
            InitializeComponent();
            setInitialize();
            this.W_Type = weight;
        }

        private void Weight_Load(object sender, EventArgs e)
        {
            //listView1.Columns.Add("专家名称",200);
            //listView2.Columns.Add("模块指标体系选择",200);
            //listView3.Columns.Add("因素", 100);
            //listView3.Columns.Add("打分", 100);
            //listView3.Columns.Add("权重", 100);

           



        }
        private void setInitialize()
        {
            listviewSet(Pro_weight);
            listviewSet(target_name);
            listviewSet(target_weight);

            

            addPro(Pro_weight, targetManager.Pro_n1, 1);
            addPro(Pro_weight, targetManager.Pro_n2, 1);
            addPro(Pro_weight, targetManager.Pro_n3, 1);
            addPro(Pro_weight, targetManager.Pro_n4, 1);
            addPro(Pro_weight, targetManager.Pro_n5, 1);
            addPro(Pro_weight, targetManager.Pro_n6, 1);
            addPro(Pro_weight, targetManager.Pro_n7, 1);
            addPro(Pro_weight, targetManager.Pro_n8, 1);
            addPro(Pro_weight, targetManager.Pro_n9, 1);
            addPro(Pro_weight, targetManager.Pro_n10, 1);

            addTar(target_name, targetManager.land_target, 0);
            addTar(target_name, targetManager.risk_target, 0);
            addTar(target_name, targetManager.ecology_target, 0);
            addTar(target_name, targetManager.final_target, 0);

            targetManager.add_t(targetManager.land_target, target_weight);
            targetManager.add_t(targetManager.risk_target, target_weight);
            targetManager.add_t(targetManager.ecology_target, target_weight);

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
            //targetManager.IsWeight = false;
            //this.Close();
            taskControl.setTarget(W_Type);
            this.Close();
        }
        /*
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
*/
        private void addPro(System.Windows.Forms.ListView listview, string targetName, int imageIndex)
        {
            lv_Pro = new ListViewItem();
            lv_Pro.ImageIndex = imageIndex;
            lv_Pro.Text = targetName;
            lv_Pro.Checked = true;
            listview.Items.Add(lv_Pro);
        }
        private void addTar(System.Windows.Forms.ListView listview, string targetName, int imageIndex)
        {
            lv_Tar = new ListViewItem();
            lv_Tar.ImageIndex = imageIndex;
            lv_Tar.Text = targetName;
            lv_Tar.Checked = true;
            listview.Items.Add(lv_Tar);
        }
        private void addWgt(System.Windows.Forms.ListView listview, string targetName, int imageIndex)
        {
            lv_Wgt = new ListViewItem();
            lv_Wgt.ImageIndex = imageIndex;
            lv_Wgt.Text = targetName;
            lv_Wgt.Checked = true;
            listview.Items.Add(lv_Wgt);
        }
        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.buttonX2.Text = "正在计算···";
            System.Threading.Thread.Sleep(3000);
            MessageBox.Show("指标权重参数初始化完成功！");
            this.buttonX2.Text = "参数计算";
            
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("保存成功！");
            this.Close();
        }
    }
}
