using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

//using MainFrame.Function;

namespace Print.Mapprint
{
	/// <summary>
	/// MapPrint ��ժҪ˵����
	/// </summary>
	public class MapPrint : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private AxPageLayoutControl axPageLayoutControl;
		public string txbStartPageText;
		public string txbEndPageText;
		public string txbOverlapText;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MapPrint(AxPageLayoutControl axPageLayoutControl,string txbStartPageText,string txbEndPageText,string txbOverlapText)
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
			this.axPageLayoutControl = axPageLayoutControl;
			this.txbStartPageText = txbStartPageText;
			this.txbEndPageText = txbEndPageText;
			this.txbOverlapText = txbOverlapText;
            
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
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(304, 144);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 16;
            this.button6.Text = "ȡ��";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(16, 216);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(112, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "��ӡԤ���ͱ༭";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(152, 144);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "ҳ������";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(152, 216);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "��ӵ�ͼ����Ҫ��";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(24, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "�ļ����Ϊ��";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(224, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 24);
            this.button2.TabIndex = 10;
            this.button2.Text = "�������";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 24);
            this.button1.TabIndex = 9;
            this.button1.Text = "�Ӵ�ӡ�����";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(24, 72);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(176, 21);
            this.textBox1.TabIndex = 17;
            // 
            // MapPrint
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(432, 270);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.Name = "MapPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "��ͼ��ӡ";
            this.Load += new System.EventHandler(this.MapPrint_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        /// <summary>
        /// ��ӡ���壭����ť�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button2_Click(object sender, System.EventArgs e)
		{   
			try
			{
				this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();				//�����Ի���
				//Open a file dialog for saving map documents
				saveFileDialog1.Title = "��ͼ�ļ����Ϊ��";
				saveFileDialog1.Filter = "Map Documents (*.mxd)|*.mxd";
				saveFileDialog1.ShowDialog();

				//ѡ��Ĵ洢·��
				string sFilePath = saveFileDialog1.FileName;
				textBox1.Text = sFilePath;
				
				//���û��ѡ��洢·�����˳�
				if (sFilePath == "")
				{
					return;
				}
				//����MapDoc����
				//IMapDocument m_Map=(IMapDocument)axMapControl.ActiveView.FocusMap;
//				MapDoc.SaveAsDocument(sFilePath);
			}
			catch{}
		
		}
		
        /// <summary>
        /// ��ӡ���壭����ť����ҳ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button4_Click(object sender, System.EventArgs e)
		{
		    PageSet m_PageSet;
			m_PageSet = new PageSet(this.axPageLayoutControl);
			m_PageSet.Show();
   
		}
        /// <summary>
        /// ��ӡ���壭����ť������ӡԤ���ͱ༭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button5_Click(object sender, System.EventArgs e)
		{
//		    MapEdit m_MapEdit;
//			m_MapEdit = new MapEdit(this.axPageLayoutControl);
//			m_MapEdit.Show();
 
		}
        /// <summary>
        /// ��ӡ���壭����ť������ӵ�ͼ����Ҫ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button3_Click(object sender, System.EventArgs e)
		{
		    MapSurround m_MapSurround;
			m_MapSurround = new MapSurround(this.axPageLayoutControl);
			m_MapSurround.Show();
		}

		private void MapPrint_Load(object sender, System.EventArgs e)
		{
		      
		}


		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{

		    		    
		}
        /// <summary>
        /// ��ӡ���壭����ť�����Ӵ�ӡ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button1_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(txbStartPageText);

			if (axPageLayoutControl.Printer != null) 
			{
				//Set mouse pointer
				axPageLayoutControl.MousePointer = esriControlsMousePointer.esriPointerHourglass;

				//Get IPrinter interface through the PageLayoutControl's printer
				IPrinter printer = axPageLayoutControl.Printer;

				//Determine whether printer paper's orientation needs changing
				if (printer.Paper.Orientation != axPageLayoutControl.Page.Orientation)
				{
					printer.Paper.Orientation = axPageLayoutControl.Page.Orientation;
			
				}

				//Print the page range with the specified overlap
				axPageLayoutControl.PrintPageLayout(Convert.ToInt16(txbStartPageText), Convert.ToInt16(txbEndPageText), Convert.ToDouble(txbOverlapText));

				//Set the mouse pointer
				try
				  {
			    	axPageLayoutControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
             
				  }
				catch{}
				
			}
		}
	}
}
