using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame.Query
{
    public partial class UserCtrDizhiResult : UserControl
    {
        public delegate void OpenFileEventHandler(string mID);
        public event OpenFileEventHandler OpenFileEvent;


        public UserCtrDizhiResult()
        {
            InitializeComponent();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                OpenFileEvent(this.listView1.SelectedItems[0].SubItems[2].Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
