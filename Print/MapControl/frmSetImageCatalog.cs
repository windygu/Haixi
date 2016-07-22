using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MainFrame.Form.MapControl
{
	/// <summary>
	/// frmSetImageCatalog ��ժҪ˵����
	/// </summary>
	public class frmSetImageCatalog : System.Windows.Forms.Form
	{
		public delegate void SelectChangeEventHandler(string p_strCurrentRasterCatalog);
		public event SelectChangeEventHandler SelectChangeEvent;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbImageRaster;
		public string strCurrentRasterCatalog;
		public clsDataAccess.DataAccess objectDataAccess;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSetImageCatalog()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.cmbImageRaster = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmbImageRaster
			// 
			this.cmbImageRaster.Location = new System.Drawing.Point(136, 40);
			this.cmbImageRaster.Name = "cmbImageRaster";
			this.cmbImageRaster.Size = new System.Drawing.Size(168, 20);
			this.cmbImageRaster.TabIndex = 0;
			this.cmbImageRaster.SelectedIndexChanged += new System.EventHandler(this.cmbImageRaster_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(48, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "ң��Ӱ��";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(80, 88);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "ȷ��";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(184, 88);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "ȡ��";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmSetImageCatalog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(360, 134);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbImageRaster);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSetImageCatalog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ң��Ӱ������";
			this.Load += new System.EventHandler(this.frmSetImageCatalog_Load);
			this.ResumeLayout(false);

		}
		#endregion


		private void InitcmbImageRaster()
		{
			this.cmbImageRaster.Items.Clear();
			int m_intSelectedIndex=0;
			if (this.objectDataAccess!=null)
			{
                string selectCMD1 = "select name from sysobjects where schema_ver ='144' and info='4'";
				System.Data.DataRowCollection m_DataRowCollection=objectDataAccess.getDataRowsByQueryString(selectCMD1);
				for(int i=0;i<m_DataRowCollection.Count ;i++)
				{					
					this.cmbImageRaster.Items.Add(m_DataRowCollection[i][0].ToString());
					if (strCurrentRasterCatalog==this.cmbImageRaster.Items[i].ToString())
					{
						m_intSelectedIndex=i;
					}
				}
				if(this.cmbImageRaster.Items.Count >0)
				{
					this.cmbImageRaster.SelectedIndex=m_intSelectedIndex;
				}
				
			}
		}

		private void cmbImageRaster_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
		}

		private void frmSetImageCatalog_Load(object sender, System.EventArgs e)
		{
			InitcmbImageRaster();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.SelectChangeEvent(this.cmbImageRaster.SelectedItem.ToString());
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
