﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComprehensiveEvaluation.TargetSystem
{
    public partial class Format : Form
    {
        public Format()
        {
            InitializeComponent();
        }

        private void Format_Load(object sender, EventArgs e)
        {
            listviewSet(listView1);
            ListViewItem lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = "最小阻力模型公式";
            listView1.Items.Add(lv);
            lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = "Logistics";
            listView1.Items.Add(lv);
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
