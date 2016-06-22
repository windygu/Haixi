using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame.AttibuteEdit
{
    public partial class FormPlayer : Form
    {
        public FormPlayer()
        {
            InitializeComponent();
        }


        // ²¥·Å
        public void PlayTheVideo(string theFilename)
        {
            try
            {
                //this.axMediaPlayer1.FileName = theFilename;
                //this.axMediaPlayer1.Play();
                axWindowsMediaPlayer1.URL = theFilename;


            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}