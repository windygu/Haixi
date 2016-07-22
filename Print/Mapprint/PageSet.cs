using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

namespace Print.Mapprint
{
	/// <summary>
	/// PageSet ��ժҪ˵����
	/// </summary>
	public class PageSet : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cboPageSize;
		private System.Windows.Forms.ComboBox cboPageToPrinterMapping;
		private System.Windows.Forms.RadioButton optLandscape;
		private System.Windows.Forms.RadioButton optPortrait;
		private System.Windows.Forms.TextBox txbOverlap;
		private System.Windows.Forms.TextBox txbStartPage;
		private System.Windows.Forms.TextBox txbEndPage;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Button button2;
		private AxPageLayoutControl axPageLayoutControl;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.TextBox txtWidth;
		private System.Windows.Forms.TextBox txtHeight;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		
		/// <summary>
		/// ����������������
		/// </summary>
		private IPoint m_Point;
		double width = 0;
		double height =0;
		private System.Windows.Forms.PrintDialog printDialog1;
		private System.Drawing.Printing.PrintDocument printDocument1;
		private System.Windows.Forms.Button btSetting;
		
		private System.ComponentModel.Container components = null;

		public PageSet(AxPageLayoutControl axPageLayoutControl)
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();


			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
			this.axPageLayoutControl = axPageLayoutControl;
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboPageSize = new System.Windows.Forms.ComboBox();
            this.cboPageToPrinterMapping = new System.Windows.Forms.ComboBox();
            this.optLandscape = new System.Windows.Forms.RadioButton();
            this.optPortrait = new System.Windows.Forms.RadioButton();
            this.txbOverlap = new System.Windows.Forms.TextBox();
            this.txbStartPage = new System.Windows.Forms.TextBox();
            this.txbEndPage = new System.Windows.Forms.TextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.btSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "ֽ�Ŵ�С��";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(8, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "ҳ���������и�";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(8, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "ҳ�淽��";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(8, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "��ӡ����";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(8, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 24);
            this.label6.TabIndex = 5;
            this.label6.Text = "��ӡҳ���";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(192, 224);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 24);
            this.label7.TabIndex = 6;
            this.label7.Text = "��";
            // 
            // cboPageSize
            // 
            this.cboPageSize.Location = new System.Drawing.Point(120, 16);
            this.cboPageSize.Name = "cboPageSize";
            this.cboPageSize.Size = new System.Drawing.Size(184, 20);
            this.cboPageSize.TabIndex = 1;
            this.cboPageSize.SelectedIndexChanged += new System.EventHandler(this.cboPageSize_SelectedIndexChanged);
            // 
            // cboPageToPrinterMapping
            // 
            this.cboPageToPrinterMapping.Location = new System.Drawing.Point(120, 112);
            this.cboPageToPrinterMapping.Name = "cboPageToPrinterMapping";
            this.cboPageToPrinterMapping.Size = new System.Drawing.Size(121, 20);
            this.cboPageToPrinterMapping.TabIndex = 4;
            this.cboPageToPrinterMapping.SelectedIndexChanged += new System.EventHandler(this.cboPageToPrinterMapping_SelectedIndexChanged);
            this.cboPageToPrinterMapping.Click += new System.EventHandler(this.cboPageToPrinterMapping_Click);
            // 
            // optLandscape
            // 
            this.optLandscape.Location = new System.Drawing.Point(232, 144);
            this.optLandscape.Name = "optLandscape";
            this.optLandscape.Size = new System.Drawing.Size(104, 24);
            this.optLandscape.TabIndex = 6;
            this.optLandscape.Text = "����";
            this.optLandscape.Click += new System.EventHandler(this.optLandscape_Click);
            // 
            // optPortrait
            // 
            this.optPortrait.Location = new System.Drawing.Point(120, 144);
            this.optPortrait.Name = "optPortrait";
            this.optPortrait.Size = new System.Drawing.Size(104, 24);
            this.optPortrait.TabIndex = 5;
            this.optPortrait.Text = "����";
            this.optPortrait.Click += new System.EventHandler(this.optPortrait_Click);
            // 
            // txbOverlap
            // 
            this.txbOverlap.Location = new System.Drawing.Point(120, 176);
            this.txbOverlap.Name = "txbOverlap";
            this.txbOverlap.Size = new System.Drawing.Size(100, 21);
            this.txbOverlap.TabIndex = 7;
            this.txbOverlap.Text = "0";
            this.txbOverlap.TextChanged += new System.EventHandler(this.txbOverlap_TextChanged);
            // 
            // txbStartPage
            // 
            this.txbStartPage.Location = new System.Drawing.Point(120, 224);
            this.txbStartPage.Name = "txbStartPage";
            this.txbStartPage.Size = new System.Drawing.Size(64, 21);
            this.txbStartPage.TabIndex = 8;
            this.txbStartPage.Text = "1";
            // 
            // txbEndPage
            // 
            this.txbEndPage.Location = new System.Drawing.Point(232, 224);
            this.txbEndPage.Name = "txbEndPage";
            this.txbEndPage.Size = new System.Drawing.Size(72, 21);
            this.txbEndPage.TabIndex = 9;
            this.txbEndPage.Text = "0";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(40, 320);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(112, 24);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "��ӡ";
            this.btnPrint.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(192, 320);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 24);
            this.button2.TabIndex = 13;
            this.button2.Text = "�ر�";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(40, 280);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 24);
            this.button4.TabIndex = 10;
            this.button4.Text = "��ӵ�ͼ����Ҫ��";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(120, 48);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(120, 21);
            this.txtWidth.TabIndex = 2;
            this.txtWidth.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
            this.txtWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWidth_KeyPress);
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(120, 80);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(120, 21);
            this.txtHeight.TabIndex = 3;
            this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
            this.txtHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHeight_KeyPress);
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(80, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 16);
            this.label8.TabIndex = 20;
            this.label8.Text = "���";
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(80, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "�߶�";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(256, 56);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 16);
            this.label10.TabIndex = 22;
            this.label10.Text = "����";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(256, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 16);
            this.label11.TabIndex = 23;
            this.label11.Text = "����";
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label12.Location = new System.Drawing.Point(80, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 23);
            this.label12.TabIndex = 24;
            this.label12.Text = "��׼";
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            // 
            // btSetting
            // 
            this.btSetting.Location = new System.Drawing.Point(192, 280);
            this.btSetting.Name = "btSetting";
            this.btSetting.Size = new System.Drawing.Size(112, 24);
            this.btSetting.TabIndex = 11;
            this.btSetting.Text = "��ӡ����";
            this.btSetting.Click += new System.EventHandler(this.btSetting_Click);
            // 
            // PageSet
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(392, 366);
            this.Controls.Add(this.btSetting);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.txbEndPage);
            this.Controls.Add(this.txbStartPage);
            this.Controls.Add(this.txbOverlap);
            this.Controls.Add(this.optPortrait);
            this.Controls.Add(this.optLandscape);
            this.Controls.Add(this.cboPageToPrinterMapping);
            this.Controls.Add(this.cboPageSize);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.Name = "PageSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ҳ������";
            this.Load += new System.EventHandler(this.PageSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void PageSet_Load(object sender, System.EventArgs e)
		{
			//Add esriPageFormID constants to drop down
			cboPageSize.Items.Add("Letter - 8.5in x 11in.");
			cboPageSize.Items.Add("Legal - 8.5in x 14in.");
			cboPageSize.Items.Add("Tabloid - 11in x 17in.");
			cboPageSize.Items.Add("C - 17in x 22in.");
			cboPageSize.Items.Add("D - 22in x 34in.");
			cboPageSize.Items.Add("E - 34in x 44in.");
			cboPageSize.Items.Add("A5 - 148mm x 210mm.");
			cboPageSize.Items.Add("A4 - 210mm x 297mm.");
			cboPageSize.Items.Add("A3 - 297mm x 420mm.");
			cboPageSize.Items.Add("A2 - 420mm x 594mm.");
			cboPageSize.Items.Add("A1 - 594mm x 841mm.");
			cboPageSize.Items.Add("A0 - 841mm x 1189mm.");
			cboPageSize.Items.Add("Custom Page Size.");
			cboPageSize.Items.Add("Same as Printer Form.");
			cboPageSize.SelectedIndex = 12;


			//Add esriPageToPrinterMapping constants to drop down
			cboPageToPrinterMapping.Items.Add("0: Crop");
			cboPageToPrinterMapping.Items.Add("1: Scale");
			cboPageToPrinterMapping.Items.Add("2: Tile");
			cboPageToPrinterMapping.SelectedIndex = 1;
			optPortrait.Checked = true;


			txbStartPage.Text = "1";
			txbEndPage.Text = "0";
			txbOverlap.Text = "1" ; 

			this.axPageLayoutControl.Page.QuerySize(out width,out height);
			this.txtWidth.Text = Convert.ToString(width);
			this.txtHeight.Text = Convert.ToString(height);

		}

		private void optPortrait_Click(object sender, System.EventArgs e)
		{
			if (optPortrait.Checked == true)
			{
				//Set the page orientation
//				if (axPageLayoutControl.Page.FormID != esriPageFormID.esriPageFormSameAsPrinter)
//				{
					axPageLayoutControl.Page.Orientation = 1;
					this.axPageLayoutControl.ActiveView.Refresh();
//				}
			}
		}

		private void optLandscape_Click(object sender, System.EventArgs e)
		{
			if (optLandscape.Checked == true)
			{
				//Set the page orientation
//				if (axPageLayoutControl.Page.FormID != esriPageFormID.esriPageFormSameAsPrinter)
//				{
					axPageLayoutControl.Page.Orientation = 2;

                // //Update printer page display  
                //UpdatePrintPageDisplay();  
                ////Update Printing display  
                //UpdatePrintingDisplay();  


					this.axPageLayoutControl.ActiveView.Refresh();


//				}
			}
		
		}


        //private void UpdatePrintPageDisplay()  
        //{  
        //    //�任��ӡҳ��
        ////    //Determine the number of pages  
        ////    short iPageCount = axPageLayoutControl.get_PrinterPageCount(Convert.ToDouble(txbOverlap.Text));  
        ////    lblPageCount.Text = iPageCount.ToString();  
 
        ////    //Validate start and end pages  
        ////    int iPageStart = Convert.ToInt32(txbStartPage.Text);  
        ////   int iPageEnd = Convert.ToInt32(txbEndPage.Text);  
        ////if ((iPageStart < 1) | (iPageStart > iPageCount))  
        ////    {  
        ////        txbStartPage.Text = "1";  
        ////    }  
        ////    if ((iPageEnd < 1) | (iPageEnd > iPageCount))  
        ////    {  
        ////        txbEndPage.Text = iPageCount.ToString();  
        ////    }  
        //}  
  
        //private void UpdatePrintingDisplay()  
        //{
        //    if (axPageLayoutControl.Printer != null)  
        //    {  
        //       //Get IPrinter interface through the PageLayoutControl's printer  
        //        IPrinter printer = axPageLayoutControl.Printer;  
  
        //        //Determine the orientation of the printer's paper  
        //        if (printer.Paper.Orientation == 1)  
        //        {  
        //            lblPrinterOrientation.Text = "����";  
        //        }  
        //        else  
        //        {  
        //            lblPrinterOrientation.Text = "����";  
        //        }  
  
        //        //Determine the printer name  
        //        lblPrinterName.Text = printer.Paper.PrinterName;  
  
        //        //Determine the printer's paper size  
        //        double dWidth;  
        //        double dheight;  
        //        printer.Paper.QueryPaperSize(out dWidth, out dheight);  
        //        //lblPrinterSize.Text = dWidth.ToString("###.000") + " by " + dheight.ToString("###.000") + " Inches";  
        //    }  
        //}  


		/// <summary>
		/// ��ť����ȷ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, System.EventArgs e)
		{
			//���ô�ӡ����
			
			/*MapPrint m_MapPrint;
			m_MapPrint = new MapPrint(this.axPageLayoutControl,this.txbStartPage.Text,this.txbEndPage.Text,this.txbOverlap.Text);
			m_MapPrint.Refresh();
			MessageBox.Show(this.txbStartPage.Text);
			PageSet.ActiveForm.Hide();*/


		    /*MapPrint m_MapPrint;
			m_MapPrint = n_MapPrint;
            m_MapPrint.txbEndPageText = txbEndPage.Text;
            m_MapPrint.txbStartPageText = txbStartPage.Text;            
			m_MapPrint.txbOverlapText = txbOverlap.Text;
			MessageBox.Show(this.txbStartPage.Text);
			PageSet.ActiveForm.Hide();*/

			try
			{
				
				if (axPageLayoutControl.Printer != null) 
				{
					//Set mouse pointer
					axPageLayoutControl.MousePointer = esriControlsMousePointer.esriPointerHourglass;

					//Get IPrinter interface through the PageLayoutControl's printer
					IPrinter printer = axPageLayoutControl.Printer;

					//���ô�ӡ��
					IPaper m_Paper = new PaperClass();
                    m_Paper.PrinterName = this.printDocument1.PrinterSettings.PrinterName;
                    printer.Paper = m_Paper;

					//Determine whether printer paper's orientation needs changing
					if (printer.Paper.Orientation != axPageLayoutControl.Page.Orientation)
					{
						printer.Paper.Orientation = axPageLayoutControl.Page.Orientation;
					}

					//Print the page range with the specified overlap
					axPageLayoutControl.PrintPageLayout(Convert.ToInt16(txbStartPage.Text), Convert.ToInt16(txbEndPage.Text), Convert.ToDouble(txbOverlap.Text));

					//Set the mouse pointer
					axPageLayoutControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
				}
			}
			catch(SystemException errs)
			{
                MessageBox.Show(errs.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}
        /// <summary>
        /// ��ť����ȡ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button2_Click(object sender, System.EventArgs e)
		{
            //cboPageToPrinterMapping.SelectedIndex = 1;
            //cboPageSize.SelectedIndex = 12;
            //optPortrait.Checked = true;
            //txbStartPage.Text = "1";
            //txbEndPage.Text = "0";
            //txbOverlap.Text = "1" ; 
			PageSet.ActiveForm.Close();
		}

		private void txbOverlap_TextChanged(object sender, System.EventArgs e)
		{
		
		}
        /// <summary>
        /// ҳ���������ӡ���壭����ť������ӡԤ���ͱ༭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button3_Click(object sender, System.EventArgs e)
		{
//			MapEdit m_MapEdit;
//			m_MapEdit = new MapEdit(this.axPageLayoutControl);
//			m_MapEdit.Show();
		}
        /// <summary>
        /// ҳ���������ӡ���壭����ť������ӵ�ͼ����Ҫ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void button4_Click(object sender, System.EventArgs e)
		{
			MapSurround m_MapSurround;
			m_MapSurround = new MapSurround(this.axPageLayoutControl);
			m_MapSurround.Show(); 
		}
   
        /// <summary>
        /// ҳ���������и�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cboPageToPrinterMapping_Click(object sender, System.EventArgs e)
		{
			//Set the printer to page mapping
			axPageLayoutControl.Page.PageToPrinterMapping = (esriPageToPrinterMapping) cboPageToPrinterMapping.SelectedIndex;
			
		}

		private void cboPageToPrinterMapping_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int index = cboPageToPrinterMapping.SelectedIndex;
			switch(index)
			{
				case (0):axPageLayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingCrop;break;
                case (1):axPageLayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingCrop;break;
				case (2):axPageLayoutControl.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingCrop;break;

			}
		        
		}

		private void cboPageSize_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(cboPageSize.SelectedIndex != 12)
			{
				this.axPageLayoutControl.Page.FormID = (esriPageFormID) cboPageSize.SelectedIndex;
				this.axPageLayoutControl.Page.QuerySize(out width,out height);
				this.txtWidth.Text = Convert.ToString(width);
				this.txtHeight.Text = Convert.ToString(height);
			}
			else
			{
				this.axPageLayoutControl.Page.QuerySize(out width,out height);
//				width = Convert.ToDouble(this.txtWidth.Text);
//				height = Convert.ToDouble(this.txtHeight.Text);
				this.axPageLayoutControl.Page.PutCustomSize(width,height);
			}
			
		}

		private void txtWidth_TextChanged(object sender, System.EventArgs e)
		{
//			cboPageSize.SelectedIndex = 12;
//			double width = 0;
//			double height = 0;
//            width = Convert.ToDouble(this.txtWidth.Text);
//			height = Convert.ToDouble(this.txtHeight.Text);
//		    this.axPageLayoutControl.Page.PutCustomSize(width,height);

		
		}

		private void txtHeight_TextChanged(object sender, System.EventArgs e)
		{
//			cboPageSize.SelectedIndex = 12;
//			double width = 0;
//			double height = 0;
//			width = Convert.ToDouble(this.txtWidth.Text);
//			height = Convert.ToDouble(this.txtHeight.Text);
//			this.axPageLayoutControl.Page.PutCustomSize(width,height);
		
		}

		private void txtWidth_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		    char press= e.KeyChar;
			if(press ==13)
			{
				cboPageSize.SelectedIndex = 12;
				width = Convert.ToDouble(this.txtWidth.Text);
				height = Convert.ToDouble(this.txtHeight.Text);
				this.axPageLayoutControl.Page.PutCustomSize(width,height);
			}
			
			

		}

		private void txtHeight_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			char press= e.KeyChar;
			if(press ==13)
			{
				cboPageSize.SelectedIndex = 12;
				width = Convert.ToDouble(this.txtWidth.Text);
				height = Convert.ToDouble(this.txtHeight.Text);
				this.axPageLayoutControl.Page.PutCustomSize(width,height);
			}
		
		}

		/// <summary>
		/// ��ť������ӡ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btSetting_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.printDialog1.ShowDialog();
            }
            catch(Exception errs)
            {
                MessageBox.Show("��ӡ��������װ����","��ӡ����");
            }

		}
	}
}
