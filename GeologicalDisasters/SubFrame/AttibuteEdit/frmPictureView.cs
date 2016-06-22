using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame
{
    public partial class frmPictureView : UserControl
    {
        public frmPictureView()
        {
            InitializeComponent();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            this.axAutoVueX1.ZoomByFactor(0.8);
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {           
            this.axAutoVueX1.ZoomByFactor(1.4);
        }

        private void btnPan_Click(object sender, EventArgs e)
        {
            this.axAutoVueX1.EnablePanMode(true);
        }

        private void frmPictureView_Load(object sender, EventArgs e)
        {
        }

        private void btnFullView_Click(object sender, EventArgs e)
        {
            this.axAutoVueX1.ZoomWidth();

        }
    }
}