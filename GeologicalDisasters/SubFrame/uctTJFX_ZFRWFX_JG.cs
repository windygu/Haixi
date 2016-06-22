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
    public partial class uctTJFX_ZFRWFX_JG : UserControl
    {
        public delegate void uctTJFX_XCGJTJ_JGEventHandler(string p_strGJID, string p_strGJZB);
        public event uctTJFX_XCGJTJ_JGEventHandler uctTJFX_XCGJTJ_JGEvent;

        private clsDataAccess.DataAccess m_DataAccess_;
        public clsDataAccess.DataAccess m_DataAccess_SYS_
        {
            set
            {
                m_DataAccess_ = value;
            }
        }
        private string m_strSQL_;
        public string m_strSQL
        {
            set
            {
                m_strSQL_ = value;
            }
        }

        public uctTJFX_ZFRWFX_JG()
        {
            InitializeComponent();
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
                    if (uctTJFX_XCGJTJ_JGEvent != null && m_DataGridView.SelectedRows.Count > 0)
                    {

                        uctTJFX_XCGJTJ_JGEvent(m_DataGridView.SelectedRows[0].Cells["轨迹标识"].Value.ToString(), m_DataGridView.SelectedRows[0].Cells["轨迹坐标"].Value.ToString());//将轨迹id传出去
                    }
                }

                if (m_MouseEventArgs.Button == MouseButtons.Right)
                {
                    if (m_DataGridView.SelectedRows.Count > 0)
                    {
                        btnSendMessage.Enabled = true;
                        btnShowAllDCGJ.Enabled = true;
                    }
                    else
                    {
                        btnSendMessage.Enabled = false;
                        btnShowAllDCGJ.Enabled = false;
                        if (m_DataGridView.SelectedCells.Count > 0)
                        {
                            if (m_DataGridView.Columns[m_DataGridView.SelectedCells[0].ColumnIndex].Name == "手机")
                            {
                                btnSendMessage.Enabled = true;
                                if (m_DataGridView.SelectedCells[0].Value != System.DBNull.Value)
                                {
                                    txtPhoneNumber.Text = (string)m_DataGridView.SelectedCells[0].Value;
                                }
                                if (m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["调查人姓名"].Value != System.DBNull.Value)
                                {
                                    txtName.Text = (string)m_DataGridView.Rows[m_DataGridView.SelectedCells[0].RowIndex].Cells["调查人姓名"].Value;
                                }
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
                m_DataAccess_.MessageErrorInforShow(this.ParentForm, errs.Message);
            }


        }
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            panel_SendMessage.Top = panel1.Top;
            panel_SendMessage.Left = panel1.Left + panel1.Width;
            panel_SendMessage.Visible = true;
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            panel_SendMessage.Visible = false;

            System.Windows.Forms.SaveFileDialog m_SaveFileDialog = new SaveFileDialog();
            m_SaveFileDialog.Filter = "Excel file(*.xls)|*.xls";
            if (m_SaveFileDialog.ShowDialog() == DialogResult.Cancel) return;

            if (m_SaveFileDialog.FileName == "") return;
            string m_strFileName = m_SaveFileDialog.FileName;

            m_DataAccess_.OutputExcel((DataGridView)m_DataGridView, "任务完情况", m_strFileName);
        }
        
        private void btnShowAllDCGJ_Click(object sender, EventArgs e)
        {
            panel_SendMessage.Visible = false;
            panel1.Visible = false;
        }

        private void btnSendMessage1_Click(object sender, EventArgs e)
        {
            try
            {
            panel_SendMessage.Visible = false;
            m_DataAccess_.SendMessage("", txtPhoneNumber.Text, "1", txtMessage.Text,"");
            }
            catch (SystemException ex)
            {


            }
        }
    }
}
