using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JCZF.SubFrame
{
    public partial class uctTJFX_XCCQ_JG : UserControl
    {
        public delegate void uctTJFX_XCCQTJ_JGEventHandler(string p_strXZQDM);
        public event uctTJFX_XCCQTJ_JGEventHandler uctTJFX_XCCQTJ_JGEvent;
        public clsDataAccess.DataAccess m_DataAccess_SYS_;

        public uctTJFX_XCCQ_JG()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            panel_SendMessage.Visible = false;
            panel1.Visible = false;
        }

        private void m_DataGridView_Click(object sender, EventArgs e)
        {
            try
            {
                panel_SendMessage.Visible = false;
                MouseEventArgs m_MouseEventArgs = (MouseEventArgs)e;

                if (m_MouseEventArgs.Button == MouseButtons.Left)
                {
                    panel1.Visible = false;
                    if (uctTJFX_XCCQTJ_JGEvent != null && m_DataGridView.SelectedRows.Count > 0)
                    {

                        uctTJFX_XCCQTJ_JGEvent(m_DataGridView.SelectedRows[0].Cells["行政区代码"].Value.ToString());//将行政区代码传出去
                    }
                }

                if (m_MouseEventArgs.Button == MouseButtons.Right)
                {
                    if (m_DataGridView.SelectedRows.Count > 0)
                    {
                        btnSendMessage.Enabled = true;
                        //btnShowAllDCGJ.Enabled = true;
                    }
                    else
                    {
                        btnSendMessage.Enabled = false;
                        //btnShowAllDCGJ.Enabled = false;
                        if (m_DataGridView.SelectedCells.Count > 0)
                        {
                            if (m_DataGridView.Columns[m_DataGridView.SelectedCells[0].ColumnIndex].Name == "本级执法人员手机")
                            {
                                btnSendMessage.Enabled = true;
                                txtPhoneNumber.Text = (string)m_DataGridView.SelectedCells[0].Value;
                                txtName.Text = (string)m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["本级执法人员姓名"].Value;

                            }

                            else if (m_DataGridView.Columns[m_DataGridView.SelectedCells[0].ColumnIndex].Name == "上级执法人员手机")
                            {
                                btnSendMessage.Enabled = true;
                                txtPhoneNumber.Text = (string)m_DataGridView.SelectedCells[0].Value;
                                txtName.Text = (string)m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["上级执法人员姓名"].Value;

                            }
                        }

                    }
                    panel1.Left = m_MouseEventArgs.X;
                    panel1.Top = m_MouseEventArgs.Y;
                    panel1.Visible = true;
                    //panel1.Show();

                }
            }
            catch (SystemException errs)
            {
                m_DataAccess_SYS_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }

        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            //timer_ExportData.Enabled = true;

            panel_SendMessage.Visible = false;

            System.Windows.Forms.SaveFileDialog m_SaveFileDialog = new SaveFileDialog();
            m_SaveFileDialog.Filter = "Excel file(*.xls)|*.xls";
            if (m_SaveFileDialog.ShowDialog() == DialogResult.Cancel) return;

            if (m_SaveFileDialog.FileName == "") return;
            string m_strFileName = m_SaveFileDialog.FileName;

            m_DataAccess_SYS_.OutputExcel((DataGridView)m_DataGridView, "巡查超期统计", m_strFileName);

        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            panel_SendMessage.Top = panel1.Top;
            if ((panel1.Left + panel1.Width + panel_SendMessage.Width +270) > this.ParentForm.Width )
            {
                panel_SendMessage.Left = panel1.Left - panel_SendMessage.Width;
            }
            else
            {
                panel_SendMessage.Left = panel1.Left + panel1.Width;
            }
            panel_SendMessage.Visible = true;

            if (m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["最近巡查时间"].Value != null && m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["最近巡查时间"].Value.ToString() != "")
            {
                string m_strTimeSpan = (DateTime.Now - (DateTime)m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["最近巡查时间"].Value).Days.ToString();
                txtMessage.Text = txtName.Text  + "同志，您负责的" + (string)m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["行政区名称"].Value + "已经" + m_strTimeSpan + "天没有巡查，超过规定的时间，特此提醒！";
            }
            else
            {
                txtMessage.Text = txtName.Text + "同志，您负责的" + (string)m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["行政区名称"].Value + "已经超过规定的巡查间隔时间，特此提醒！";

            }
        }

        private void btnSendMessage1_Click(object sender, EventArgs e)
        {
            try{
            panel_SendMessage.Visible = false;
            m_DataAccess_SYS_.SendMessage("", txtPhoneNumber.Text, "1", txtMessage.Text,"");
            }
            catch (SystemException  ex)
            {


            }
        }

        private void timer_ExportData_Tick(object sender, EventArgs e)
        {
           
            timer_ExportData.Enabled = false ;
        }

       
    }
}
