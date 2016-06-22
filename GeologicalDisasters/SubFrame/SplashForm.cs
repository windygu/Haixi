using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JCZF
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
          
        }

        private string _StatusInfo;
        private int _progress = 0;

        public string StatusInfo
        {
            set
            {
                _StatusInfo = value;
                ChangeStatusText();
            }
            get
            {
                return _StatusInfo;
            }
        }

        public void ChangeStatusText()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.ChangeStatusText));
                    return;
                }

                lStatusInfo.Text = _StatusInfo;
            }
            catch (Exception e)
            {
                //	do something here...
            }
        }

        public int progress
        {
            set
            {
                _progress = value;
                Changeprogress();
            }
            get
            {
                return _progress;
            }
        }
        public void Changeprogress()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.Changeprogress));
                    return;
                }

                this.progressBarX1.Value = _progress;
            }
            catch (Exception e)
            {
                //	do something here...
            }
        }

        private void lStatusInfo_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.progressBarX1.Value += 10;
        }


    }
}