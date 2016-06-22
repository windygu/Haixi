using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame.Query
{
    public partial class UserCtrTufuResult : UserControl
    {
        public delegate void OpenFileEventHandler2(string TF);
        public event OpenFileEventHandler2 OpenFileEvent_Tufu;

        public UserCtrTufuResult()
        {
            InitializeComponent();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileEvent_Tufu(this.listView1.SelectedItems[0].SubItems[1].Text.Trim());
        }
    }
}
