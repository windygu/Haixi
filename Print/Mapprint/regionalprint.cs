using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using Print.PageLayoutControl;
using Print.Function;
using Object = System.Object;
using Point = ESRI.ArcGIS.Geometry.Point;
using ESRI.ArcGIS.Controls;

namespace Print.Mapprint
{
	/// <summary>
	/// regionalprint ��ժҪ˵����
	/// </summary>
	public class regionalprint : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ComboBox comboScale;
		private System.Windows.Forms.ComboBox comboSHI;
		private System.Windows.Forms.ComboBox comboSHENG;
		private System.Windows.Forms.ComboBox comboQU;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtDistanceSX;
		private System.Windows.Forms.TextBox txtDistanceZY;
		private System.Windows.Forms.RadioButton radioTF;
		private System.Windows.Forms.TextBox txtTFName;
		private System.Windows.Forms.RadioButton radioRY;
		private System.Windows.Forms.Button button4;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		//����pagelayout
		private AxPageLayoutControl axPageLayoutControl;
		private AxMapControl axMapControl;
		private IMapDocument m_MapDocument;
		//��ͼ���ù��ܱ���
		private MapFunction mapFuntion;
		//��ӡ����
		private frmPageLayoutControl frmPageLayoutControl;

		private	ArrayList arryOfDishiValue = new ArrayList() ; 
		private ArrayList arryOfQuxianValue = new ArrayList() ;
		private System.Windows.Forms.Label label4;
		
		private IFeature selfeature;
		private double scale;
		private IElement m_PolygonElement;
		public  IGeometry m_Geometry;
		public  bool index = false;
		private System.Windows.Forms.RadioButton radioXZQ;
		public  IElement m_RECT = null;

		//������ͼ������
		private string m_strQXXZQY="����������";
		private string m_strDSXZQY="����������";
		private string m_strSJXZQY="ʡ��������";
		////////////////////////////////////////////

		public regionalprint(AxPageLayoutControl axPageLayoutControl,AxMapControl axMapControl,IMapDocument m_MapDocument,MapFunction  mapFuntion)
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
			this.m_MapDocument = m_MapDocument;
			this.axPageLayoutControl = axPageLayoutControl;
			this.axPageLayoutControl.PageLayout = m_MapDocument.PageLayout;
			this.axMapControl = axMapControl;
		    this.mapFuntion = mapFuntion;
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
			this.comboScale = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.comboQU = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.comboSHI = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboSHENG = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtDistanceZY = new System.Windows.Forms.TextBox();
			this.txtDistanceSX = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.radioTF = new System.Windows.Forms.RadioButton();
			this.txtTFName = new System.Windows.Forms.TextBox();
			this.radioRY = new System.Windows.Forms.RadioButton();
			this.button4 = new System.Windows.Forms.Button();
			this.radioXZQ = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboScale
			// 
			this.comboScale.Items.AddRange(new object[] {
															"2500",
															"5000",
															"10000",
															"50000",
															"100000",
															"250000"});
			this.comboScale.Location = new System.Drawing.Point(176, 245);
			this.comboScale.Name = "comboScale";
			this.comboScale.Size = new System.Drawing.Size(121, 20);
			this.comboScale.TabIndex = 23;
			this.comboScale.SelectedIndexChanged += new System.EventHandler(this.comboScale_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(37, 246);
			this.label1.Name = "label1";
			this.label1.TabIndex = 22;
			this.label1.Text = "���ô�ӡ������";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioXZQ);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.comboQU);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.comboSHI);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.comboSHENG);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 128);
			this.groupBox1.TabIndex = 33;
			this.groupBox1.TabStop = false;
			// 
			// label6
			// 
			this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label6.Location = new System.Drawing.Point(16, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 23);
			this.label6.TabIndex = 41;
			// 
			// comboQU
			// 
			this.comboQU.Location = new System.Drawing.Point(88, 88);
			this.comboQU.Name = "comboQU";
			this.comboQU.Size = new System.Drawing.Size(192, 20);
			this.comboQU.TabIndex = 38;
			this.comboQU.SelectedIndexChanged += new System.EventHandler(this.comboQU_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 37;
			this.label3.Text = "����������";
			// 
			// comboSHI
			// 
			this.comboSHI.Location = new System.Drawing.Point(88, 56);
			this.comboSHI.Name = "comboSHI";
			this.comboSHI.Size = new System.Drawing.Size(192, 20);
			this.comboSHI.TabIndex = 36;
			this.comboSHI.SelectedIndexChanged += new System.EventHandler(this.comboSHI_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 35;
			this.label2.Text = "�е�������";
			// 
			// comboSHENG
			// 
			this.comboSHENG.Location = new System.Drawing.Point(88, 24);
			this.comboSHENG.Name = "comboSHENG";
			this.comboSHENG.Size = new System.Drawing.Size(192, 20);
			this.comboSHENG.TabIndex = 34;
			this.comboSHENG.SelectedIndexChanged += new System.EventHandler(this.comboSHENG_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 16);
			this.label5.TabIndex = 33;
			this.label5.Text = "ʡ��������";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(40, 376);
			this.button1.Name = "button1";
			this.button1.TabIndex = 34;
			this.button1.Text = "��ӡ";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(176, 376);
			this.button2.Name = "button2";
			this.button2.TabIndex = 35;
			this.button2.Text = "ȡ��";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(149, 248);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(24, 23);
			this.label4.TabIndex = 37;
			this.label4.Text = "1��";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txtDistanceZY);
			this.groupBox2.Controls.Add(this.txtDistanceSX);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Location = new System.Drawing.Point(16, 280);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(288, 72);
			this.groupBox2.TabIndex = 38;
			this.groupBox2.TabStop = false;
			// 
			// txtDistanceZY
			// 
			this.txtDistanceZY.Location = new System.Drawing.Point(232, 32);
			this.txtDistanceZY.Name = "txtDistanceZY";
			this.txtDistanceZY.Size = new System.Drawing.Size(32, 21);
			this.txtDistanceZY.TabIndex = 4;
			this.txtDistanceZY.Text = "2";
			// 
			// txtDistanceSX
			// 
			this.txtDistanceSX.Location = new System.Drawing.Point(96, 32);
			this.txtDistanceSX.Name = "txtDistanceSX";
			this.txtDistanceSX.Size = new System.Drawing.Size(32, 21);
			this.txtDistanceSX.TabIndex = 3;
			this.txtDistanceSX.Text = "2";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 36);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 16);
			this.label9.TabIndex = 2;
			this.label9.Text = "ͼ�����±߿�";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(150, 38);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(80, 16);
			this.label8.TabIndex = 1;
			this.label8.Text = "ͼ�����ұ߿�";
			// 
			// label7
			// 
			this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label7.Location = new System.Drawing.Point(21, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 23);
			this.label7.TabIndex = 0;
			this.label7.Text = "���ô�ӡ����";
			// 
			// radioTF
			// 
			this.radioTF.Location = new System.Drawing.Point(40, 160);
			this.radioTF.Name = "radioTF";
			this.radioTF.Size = new System.Drawing.Size(72, 24);
			this.radioTF.TabIndex = 39;
			this.radioTF.Text = "����ͼ��";
			this.radioTF.Click += new System.EventHandler(this.radioTF_Click);
			// 
			// txtTFName
			// 
			this.txtTFName.Location = new System.Drawing.Point(152, 160);
			this.txtTFName.Name = "txtTFName";
			this.txtTFName.Size = new System.Drawing.Size(144, 21);
			this.txtTFName.TabIndex = 40;
			this.txtTFName.Text = "";
			this.txtTFName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTFName_KeyPress);
			// 
			// radioRY
			// 
			this.radioRY.Location = new System.Drawing.Point(40, 200);
			this.radioRY.Name = "radioRY";
			this.radioRY.Size = new System.Drawing.Size(96, 24);
			this.radioRY.TabIndex = 41;
			this.radioRY.Text = "�Զ�������";
			this.radioRY.Click += new System.EventHandler(this.radioRY_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(152, 200);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(144, 23);
			this.button4.TabIndex = 42;
			this.button4.Text = "���ƴ�ӡ����";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// radioXZQ
			// 
			this.radioXZQ.Location = new System.Drawing.Point(16, 0);
			this.radioXZQ.Name = "radioXZQ";
			this.radioXZQ.TabIndex = 42;
			this.radioXZQ.Text = "��������";
			this.radioXZQ.Click += new System.EventHandler(this.radioXZQ_Click);
			// 
			// regionalprint
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(328, 422);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.radioRY);
			this.Controls.Add(this.txtTFName);
			this.Controls.Add(this.radioTF);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.comboScale);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.Name = "regionalprint";
			this.Text = "�������ӡ";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.regionalprint_Closing);
			this.Load += new System.EventHandler(this.regionalprint_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region ���������ر��¼�

		private void regionalprint_Load(object sender, EventArgs e)
		{
			//			//���comboboxѡ��
			//			comboScale.Items.Add("1:2500");
			//			comboScale.Items.Add("1:5000");
			//			comboScale.Items.Add("1:10000");
			//            comboScale.Items.Add("1:50000");
			//			comboScale.Items.Add("1:100000");
			//			comboScale.Items.Add("1:250000");

			//��ʼ�����Ͽ�
			if (comboScale.Items.Count >0 )
				comboScale.SelectedIndex = 0;			

			this.axMapControl.Extent = this.axMapControl.FullExtent;
			SelectByName(m_strSJXZQY,"XZQM","����ʡ");
			this.axMapControl.ActiveView.FocusMap.ClearSelection();

			//			this.axMapControl.ActiveView.Selection.Clear();



		}
		

		/// <summary>
		/// ����ر��¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void regionalprint_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//�ر�ʱ��ɾ�����Ƕ����
			DelPolygon();
			this.axMapControl.Extent = this.axMapControl.FullExtent;
			this.axMapControl.ActiveView.FocusMap.ClearSelection();
			this.axMapControl.ActiveView.Refresh();
		
		}

		#endregion

		#region   ���Ͽ�ı��¼�

		private void comboScale_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//			IMapFrame m_MapFrame;
			//			IGraphicsContainer m_GraphicsContainer;
			//			m_GraphicsContainer = this.axPageLayoutControl.PageLayout as IGraphicsContainer;
			//			IMap  m_Map = this.axPageLayoutControl.ActiveView.FocusMap;
			//			m_MapFrame = m_GraphicsContainer.FindFrame(m_Map) as IMapFrame;
			//			int index = this.comboScale.SelectedIndex;
			//			switch(index)
			//			{
			//				case 0:m_MapFrame.MapScale = 2500;break;
			//				case 1:m_MapFrame.MapScale = 5000;break;
			//				case 2:m_MapFrame.MapScale = 10000;break;
			//				case 3:m_MapFrame.MapScale = 50000;break;
			//				case 4:m_MapFrame.MapScale = 100000;break;
			//				case 5:m_MapFrame.MapScale = 250000;break;			
			//			}
		}

		/// <summary>
		/// ʡ���Ͽ򵥻��ı��¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboSHENG_SelectedIndexChanged(object sender, EventArgs e)
		{
			
			this.axMapControl.Extent = this.axMapControl.FullExtent;
			SelectByName(m_strSJXZQY,"XZQM","����ʡ");
			this.comboSHENG.ForeColor = System.Drawing.Color.RoyalBlue;
			this.comboSHI.ForeColor=System.Drawing.Color.Black ;
			this.comboQU.ForeColor=System.Drawing.Color.Black ;
		
		}
        
		/// <summary>
		/// �и��Ͽ򵥻��ı��¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboSHI_SelectedIndexChanged(object sender, EventArgs e)
		{
			string name = comboSHI.SelectedItem.ToString();
			this.comboQU.Items.Clear();
			GetQUbasedonSHI(name);
			SelectByName(m_strDSXZQY,"XZQM",name);
			this.comboSHENG.ForeColor = System.Drawing.Color.Black ;
			this.comboSHI.ForeColor=System.Drawing.Color.RoyalBlue;
			this.comboQU.ForeColor=System.Drawing.Color.Black ;
			
		}

		/// <summary>
		/// ���ظ��Ͽ򵥻��ı��¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboQU_SelectedIndexChanged(object sender, EventArgs e)
		{
			string Name = comboQU.SelectedItem.ToString();
			SelectByName(m_strQXXZQY,"XZQM",Name);
			this.comboSHENG.ForeColor = System.Drawing.Color.Black ;
			this.comboSHI.ForeColor=System.Drawing.Color.Black ;
			this.comboQU.ForeColor=System.Drawing.Color.RoyalBlue;
		}

		private void txtTFName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(this.radioTF.Checked)
			{
				char keychar = e.KeyChar;
				if(keychar == 13)
				{
					//				ILayer iLayerOfTF = this.getFeatureLayerByName("SDE.FFTC10000");
					//				IFeatureLayer m_FeatureLayer = iLayerOfTF as IFeatureLayer;
					//
					//				//�����ѡ
					//				this.axMapControl.ActiveView.FocusMap.ClearSelection();
					//				axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,iLayerOfTF, null);
					//
					//				IFeatureCursor pCursor = null;
					//				IQueryFilter m_QueryFilter = new QueryFilter() as IQueryFilter ;
					//				string TFName = this.txtTFName.Text;
					//				m_QueryFilter.WhereClause = "TFBH = "+"'"+TFName+"'";
					//				pCursor =m_FeatureLayer.Search(m_QueryFilter,true);
					//
					//				this.selfeature = pCursor.NextFeature();
					//				if(this.selfeature == null)
					//				{
					//					MessageBox.Show("û�и�ͼ��������������");
					//				
					//				}
					string TFName = this.txtTFName.Text;
                    SelectByName("10000�ַ�ͼ��", "TFBH", TFName);
							
				}
			}
		}

		/// <summary>
		/// ͨ��ѡ�����ֶ�λ
		/// </summary>
		/// <param name="layerName"></param>
		/// <param name="field"></param>
		/// <param name="Name"></param>
		private void SelectByName(string layerName,string field,string Name)
		{
			try
			{
				//�õ�ͼ��
				ILayer iLayer = this.getFeatureLayerByName(layerName);
				IFeatureLayer FeatureLayer = (FeatureLayer)iLayer;

				//����ϴβ�ѯҪ��
				this.axMapControl.ActiveView.FocusMap.ClearSelection();
				axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,iLayer, null);

				IActiveView activeView = (IActiveView) this.axMapControl.ActiveView.FocusMap;

				//��ѯ���       
				QueryFilter queryFilter = new QueryFilter();
				queryFilter.WhereClause =  field + " = '" + Name + "'" ;
				
				//��ǰѡ���Ҫ��
				IFeatureSelection featureSelection =FeatureLayer as IFeatureSelection;

				//Ҫ��ѡ��
				IFeatureCursor pCursor = null;
				pCursor =FeatureLayer.Search(queryFilter,false);
				selfeature=pCursor.NextFeature();

				if(selfeature!=null)
				{
					featureSelection.Add(selfeature) ;
				}	
				else
				{
					MessageBox.Show("û�и���������"); 
					if(this.radioTF.Checked)
					{
						MessageBox.Show("û�и�ͼ�������������룡"); 
					}
				}
					

				//CopyTOPageLayout();

				//������ʾ
				this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,iLayer, null);

				//��ͼ��λ����ǰ�����Ҫ��
				IEnvelope m_Envelope = this.axMapControl.ActiveView.Extent;
				m_Envelope = selfeature.Extent;
				this.axMapControl.ActiveView.Extent = m_Envelope;
				this.axMapControl.ActiveView.Refresh();
				


			}
			catch(Exception ee)
			{
				Debug.WriteLine("---locatedByXzqName has errors---:"+ee.Message);
			}	
			
		}

		private void CopyTOPageLayout()
		{
			try
			{ 
				//Get IObjectCopy interface
				IObjectCopy objectCopy = new ObjectCopy() as IObjectCopy;

				//Get IUnknown interface (map to copy)
				object toCopyMap = this.axMapControl.Map;

				//Get IUnknown interface (copied map)
				object copiedMap = objectCopy.Copy(toCopyMap);

				//Get IUnknown interface (map to overwrite)
				object toOverwriteMap = this.axPageLayoutControl.ActiveView.FocusMap;

				//Overwrite the MapControl's map
				objectCopy.Overwrite(copiedMap, ref toOverwriteMap);

				//SetMapExtent();

				//				IEnvelope m_pEnvelope = this.axMapControl.Extent;
				//				this.m_MapDocument.ActiveView.Extent = m_pEnvelope;
				this.axPageLayoutControl.ActiveView.Refresh();
				
			}
			catch (Exception e)
			{
				Debug.WriteLine("----CopyAndOverwriteMap has errors----"+e.Message);
			}
			
		}

		
		/// <summary>
		/// MapControl��PageLayoutControl���������õ�ͼ��Χ
		/// </summary>
		private void SetMapExtent()
		{
			//Get IActiveView interface
			IActiveView activeView = (IActiveView) this.axPageLayoutControl.ActiveView.FocusMap;
			//Set the control's extent
			this.axMapControl.Extent = activeView.Extent;
			//Refresh the display
			axMapControl.ActiveView.Refresh();
			this.axPageLayoutControl.Refresh();
		}

		#endregion

		#region ��ť����

		#region ��ӡ

		/// <summary>
		/// ��ť������ӡ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			if(this.m_Geometry != null)
			{
				selfeature.Shape = m_Geometry;
			}
			
			//�õ����ǵĶ����
			if(selfeature!= null)
			{
				//ͨ����ֵõ����Ƕ����
				DifferFeature(selfeature);
			}
			//�����趨�ı��������ŵ�ͼ
			ScaleMap();
			
			//ɾ�������ӡ������
			IGraphicsContainer m_GraphicsContainer = this.axMapControl.ActiveView as IGraphicsContainer;
			if(m_RECT != null)
			{
				m_GraphicsContainer.DeleteElement(m_RECT);
                m_RECT = null;
			}
			this.axMapControl.ActiveView.Refresh();


			//��õ�ǰMapControl�򿪵ĵ�ͼ�ĵ���·��
			string sFilePath  = this.axMapControl.DocumentFilename; 
			//Create a new map document
			m_MapDocument = new MapDocument() as IMapDocument;
			//Open the map document selected
			m_MapDocument.Open(sFilePath,"");

			if (frmPageLayoutControl == null || frmPageLayoutControl.IsDisposed == true)
			{
				frmPageLayoutControl = new frmPageLayoutControl();
				frmPageLayoutControl.Owner = this;//����ProfileGraphDrawForm�����OwnerΪForm1

			}
			this.axPageLayoutControl =frmPageLayoutControl.axPageLayoutControl;
			//frmPageLayoutControl.axPageLayoutControl.PageLayout = m_MapDocument.PageLayout;
			//��page���ݲ���
			PassToPage();
			//ͨ��copy����������ͼ����һ��
			this.CopyAndOverwritePage();
			this.frmPageLayoutControl.Show();
			//ʹmapframe�븲�Ƕ���α���һ��
			FixFrame();
			this.DelPolygon();
		}

		/// <summary>
		/// ��page���崫�����
		/// </summary>
		private void PassToPage()
		{
			this.mapFuntion = this.InitMapFunction(this.axMapControl,this.axPageLayoutControl);
			frmPageLayoutControl.mapFuntion = this.mapFuntion;
			frmPageLayoutControl.axMapControl = this.axMapControl;
			frmPageLayoutControl.m_MapDocument = this.m_MapDocument;
		}

		/// <summary>
		/// ����������ʼ��MapFunction
		/// </summary>
		/// <param name="p_axMapControl"></param>
		/// <param name="p_axPageLayoutControl"></param>
		/// <returns></returns>
		private MapFunction InitMapFunction(AxMapControl p_axMapControl,AxPageLayoutControl p_axPageLayoutControl)
		{
		 MapFunction m_mapFuntion;
			m_mapFuntion= new MapFunction(this.axMapControl,this.axPageLayoutControl);
			return m_mapFuntion;
		}


		/// <summary>
		/// MapControl��PageLayoutControl������������ͼ
		/// </summary>
		private void CopyAndOverwritePage()
		{
			try
			{ //Get IObjectCopy interface
				IObjectCopy objectCopy = new ObjectCopy() as IObjectCopy;

				//Get IUnknown interface (map to copy)
				object toCopyMap = this.axMapControl.ActiveView.FocusMap;

				//Get IUnknown interface (copied map)
				object copiedMap = objectCopy.Copy(toCopyMap);

				//Get IUnknown interface (map to overwrite)
				object toOverwriteMap = this.axPageLayoutControl.ActiveView.FocusMap;

				//Overwrite the MapControl's map
				objectCopy.Overwrite(copiedMap, ref toOverwriteMap);

				//Refresh the display
				this.axPageLayoutControl.ActiveView.Refresh();

				//SetMapExtent();
			}
			catch (Exception e)
			{
				Debug.WriteLine("----CopyAndOverwriteMap has errors----"+e.Message);
			}
		}

		#endregion

		#region ȡ��

		/// <summary>
		/// ��ť����ȡ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			//			comboScale.SelectedIndex = 0;
			//			comboSHENG.SelectedIndex = 0;
			//			comboSHI.SelectedIndex = 0;
			//ɾ�����Ƕ���λ��߾��ο�
			DelPolygon();
			//ȫͼ��ʾ
			this.axMapControl.Extent = this.axMapControl.FullExtent;
			//Form.Mapprint.regionalprint.ActiveForm.Close();
		}

		/// <summary>
		/// ��������ɾ�������
		/// </summary>
		private void DelPolygon()
		{
			//			this.axPageLayoutControl.GraphicsContainer.DeleteElement(m_PolygonElement);
			//			this.axPageLayoutControl.ActiveView.Refresh();
			try
			{
				IGraphicsContainer m_GraphicsContainer = this.axMapControl.ActiveView as IGraphicsContainer;
				if(m_PolygonElement != null)
				{
					m_GraphicsContainer.DeleteElement(m_PolygonElement);
				
				}

				if(m_RECT != null)
				{
					m_GraphicsContainer.DeleteElement(m_RECT);
				}
				this.axMapControl.ActiveView.Refresh();
			}
			catch (Exception e)
			{
				Debug.WriteLine("----DelPolygon has errors----"+e.Message);
			}
//			CopyTOPageLayout();
//			this.axPageLayoutControl.ActiveView.Refresh();
		}

		#endregion

		#region ȷ��

		/// <summary>
		/// ��ť����ȷ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			if(this.m_Geometry != null)
			{
				selfeature.Shape = m_Geometry;
			}
			
			//�õ����ǵĶ����
			if(selfeature!= null)
			{
				//ͨ����ֵõ����Ƕ����
				DifferFeature(selfeature);
			}
		    //�����趨�ı��������ŵ�ͼ
			ScaleMap();
		}

		/// <summary>
		/// �����������ŵ�ͼ����
		/// </summary>
		private void ScaleMap()
		{
			string combotext = this.comboScale.Text;
			//            float temp  = float.Parse(combotext);
			scale= Convert.ToDouble(combotext);
			this.axMapControl.Map.MapScale = scale;
			this.axMapControl.ActiveView.Refresh();
			//MessageBox.Show(this.axMapControl.MapScale.ToString());
		}

		/// <summary>
		/// ��������ʹMapFrame��ͼ�α�������һ�£���ͼ�������һ��
		/// </summary>
		private void FixFrame()
		{
			IFrameElement m_Frame;
			IElement m_Element;
			IGraphicsContainer m_GraphicsContainer;
			m_GraphicsContainer = this.axPageLayoutControl.PageLayout as IGraphicsContainer;
			IMap  m_Map = this.axPageLayoutControl.ActiveView.FocusMap;
			m_Frame = m_GraphicsContainer.FindFrame(m_Map);

			IMapFrame m_MapFrame = m_Frame as IMapFrame;
			m_MapFrame = m_Frame as IMapFrame;
			m_MapFrame.Container  = m_GraphicsContainer;
			m_MapFrame.Map = m_Map;
			m_MapFrame.MapScale = scale;
			//this.axPageLayoutControl.ActiveView.FocusMap.MapScale = scale;
			//			m_MapFrame.MapBounds.
			//����ͼ��ķ�Χ����
			m_MapFrame.ExtentType = esriExtentTypeEnum.esriExtentScale;
			m_MapFrame.Map.IsFramed = true;


			m_Element = m_MapFrame as IElement;

			IEnvelope m_Envelope = this.axPageLayoutControl.ActiveView.Extent;
//			IPoint m_Point = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
//			m_Point.X = (this.axPageLayoutControl.ActiveView.Extent.XMax+this.axPageLayoutControl.ActiveView.Extent.XMin)/2;
//			m_Point.Y = (this.axPageLayoutControl.ActiveView.Extent.YMax+this.axPageLayoutControl.ActiveView.Extent.YMin)/2;
			//            m_Envelope.CenterAt(m_Point);

			//			m_Envelope.XMin = 2;
			//			m_Envelope.XMax = (this.m_MapDocument.ActiveView.Extent.XMax-this.m_MapDocument.ActiveView.Extent.XMin)*100/scale +m_Envelope.XMin;
			//			m_Envelope.YMin = 2;
			//			m_Envelope.YMax = (this.m_MapDocument.ActiveView.Extent.YMax-this.m_MapDocument.ActiveView.Extent.YMin)*100/scale + m_Envelope.YMin;
   
			int distanceSX =  Convert.ToInt32(this.txtDistanceSX.Text);
			int distanceZY =  Convert.ToInt32(this.txtDistanceZY.Text);
			if(selfeature != null)
			{
////				m_Envelope.XMin = 2;
////				m_Envelope.XMax = (this.selfeature.Extent.XMax-this.selfeature.Extent.XMin)*100/scale +m_Envelope.XMin + distanceZY;
////				m_Envelope.YMin = 2;
////				m_Envelope.YMax = (this.selfeature.Extent.YMax-this.selfeature.Extent.YMin)*100/scale + m_Envelope.YMin + distanceSX;
				SpatialReferenceEnvironment pSpatRefFact =new SpatialReferenceEnvironment();
				ISpatialReference pProjCoordSystem = pSpatRefFact.CreateProjectedCoordinateSystem(21480);//21476
				//esriSRProjCS_Beijing1954_3_Degree_GK_CM_102E
				IGeometry m_selGeometry = this.selfeature.Shape;
				m_selGeometry.Project(pProjCoordSystem);
				m_Envelope.XMin = 2;
				m_Envelope.XMax = (m_selGeometry.Envelope.Width )*100/scale*1.37 +2 ;
				m_Envelope.YMin = 2;
				m_Envelope.YMax = (m_selGeometry.Envelope.Height)*100/scale*1.37 +2;
//
			}
//			if(this.m_Geometry != null)
//			{
//				SpatialReferenceEnvironment pSpatRefFact =new SpatialReferenceEnvironment();
//				ISpatialReference pProjCoordSystem = pSpatRefFact.CreateProjectedCoordinateSystem(21480);//21476
//				IGeometry m_selGeometry  = this.m_Geometry;
//				m_selGeometry.Project(pProjCoordSystem);
//				m_Envelope.XMin = 2;
//				m_Envelope.XMax = (m_selGeometry.Envelope.Width )*100/scale +2 ;
//				m_Envelope.YMin = 2;
//				m_Envelope.YMax = (m_selGeometry.Envelope.Height)*100/scale +2;
////				m_Envelope.XMin = 2;
////				m_Envelope.XMax = (this.m_Geometry.Envelope.XMax-this.m_Geometry.Envelope.XMin)*100/scale +m_Envelope.XMin ;
////				m_Envelope.YMin = 2;
////				m_Envelope.YMax = (this.m_Geometry.Envelope.YMax-this.m_Geometry.Envelope.YMin)*100/scale + m_Envelope.YMin ;
//			}


			m_Element.Geometry = m_Envelope;
//			m_Element.Geometry = m_selGeometry.Envelope;
//			m_Envelope = m_selGeometry.Envelope;

			//            m_MapFrame.MapBounds = m_Envelope;
			//			m_Element = m_MapFrame as IElement;


			m_GraphicsContainer.UpdateElement(m_Element);
			// m_GraphicsContainer.Reset();
			m_Frame = m_Element as IFrameElement;

			//this.axPageLayoutControl.Page.PutCustomSize(m_Envelope.XMax+2.0,m_Envelope.YMax+2.0);
            this.axPageLayoutControl.Page.PutCustomSize(m_Envelope.XMax +distanceZY ,m_Envelope.YMax +distanceSX );
			this.axPageLayoutControl.ActiveView.Refresh();
      
			//			double m_dbHeight,m_dbWidth;
			//			this.axPageLayoutControl.PageLayout.Page.QuerySize(out m_dbWidth,out m_dbHeight);
			//
			//			this.axPageLayoutControl.PageLayout.Page.PutCustomSize(m_dbWidth/2,m_dbHeight/2);

			
			//          MessageBox.Show(m_MapFrame.MapScale.ToString()) ;
			//			m_GraphicsContainer.UpdateElement(m_Element);
        
			//          IEnvelope m_Envelope = this.selfeature.Shape.Envelope;
			//			this.m_MapDocument.ActiveView.Extent = m_Envelope;
			//			this.axPageLayoutControl.ActiveView.Refresh();

			//			MessageBox.Show(height.ToString());

		}


		/// <summary>
		/// �õ����Ƕ����
		/// </summary>
		/// <param name="selfeature"></param>
		private void DifferFeature(IFeature selfeature)
		{
			
			IEnvelope m_Envelope = this.m_MapDocument.ActiveView.Extent;
			//			IRectangleElement m_RectangleElement = new RectangleElementClass();
			//			IElement m_Element = m_RectangleElement as IElement;
			//            m_Element.Geometry = m_Envelope;
			IGeometry m_Source  = m_Envelope;
			Polygon polygon = new Polygon();
			Point pt1 = new Point();
			Point pt2 = new Point();
			Point pt3 = new Point();
			Point pt4 = new Point();
			pt1.PutCoords(m_Envelope.XMin,m_Envelope.YMax);
			pt2.PutCoords(m_Envelope.XMin,m_Envelope.YMin);
			pt3.PutCoords(m_Envelope.XMax,m_Envelope.YMin);
			pt4.PutCoords(m_Envelope.XMax,m_Envelope.YMax);
			Object before = Type.Missing;
			polygon.AddPoint(pt1,ref before ,ref before);
			polygon.AddPoint(pt2,ref before ,ref before);
			polygon.AddPoint(pt3,ref before ,ref before);
			polygon.AddPoint(pt4,ref before ,ref before);
			
            
			IGeometry m_Other = selfeature.Shape;
			IGeometry m_Result = null;
			ITopologicalOperator m_TopoOp = polygon as ITopologicalOperator;
			m_Result = m_TopoOp.Difference(m_Other);

			//������mapcontrol����
			IRgbColor RgbColor = new RgbColor() as IRgbColor;
			RgbColor.Red = 255;
			RgbColor.Green = 255;
			RgbColor.Blue = 255;

			//�½�һ��������
			ISimpleFillSymbol m_SimpleFillSymbol = new SimpleFillSymbol() as ISimpleFillSymbol;
			m_SimpleFillSymbol.Color = RgbColor;
			m_SimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
			//			ISymbol m_Symbol = m_SimpleFillSymbol as ISymbol;
			//			Object m_Object = m_Symbol as Object;
			//			this.axMapControl.DrawShape(m_Result, ref m_Object);

			//���polygonҪ��
			m_PolygonElement = new PolygonElement() as IElement ;
			m_PolygonElement.Geometry = m_Result;
			IFillShapeElement m_FillEle = m_PolygonElement as IFillShapeElement;
			m_FillEle.Symbol = m_SimpleFillSymbol;
			m_PolygonElement = m_FillEle as IElement;

			IElementProperties m_ElementProperties;
			m_ElementProperties = m_PolygonElement as  IElementProperties;
			m_ElementProperties.Name = "NewPolygon";

			IGraphicsContainer m_GraphicsContainer = this.axMapControl.ActiveView as IGraphicsContainer;
			m_GraphicsContainer.AddElement(m_PolygonElement,0);
			this.axMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics,null,null);
			this.axMapControl.ActiveView.Refresh();
//			CopyTOPageLayout();
		}

		#endregion

		#region ���������ӡ����

		/// <summary>
		/// ��ť�������ƾ���
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button4_Click(object sender, EventArgs e)
		{
			
			if(this.radioRY.Checked)
			{
				this.index = true;
			}
		}

		#endregion

		
		#endregion

		#region ��ѡ���¼�

		/// <summary>
		/// ��ѡ�򡪡�������������ӡʱ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioXZQ_Click(object sender, EventArgs e)
		{

			//���comboSHENGѡ��
			comboSHENG.Items.Add("����ʡ");

			comboSHENG.SelectedIndex = 0;

			//���comboSHIѡ��
			InitializecoboSHI();
		
			//���comboQU
			InitializecoboQU();
		
		}

		/// <summary>
		/// ��ѡ�򡪡������ӡ�����¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioRY_Click(object sender, System.EventArgs e)
		{
			//����ϴβ�ѯҪ��,ȥ��������ʾ
			this.axMapControl.ActiveView.FocusMap.ClearSelection();
			this.axMapControl.Extent = this.axMapControl.FullExtent;
			this.axMapControl.ActiveView.Refresh();

		}

		private void radioTF_Click(object sender, System.EventArgs e)
		{
			//����ϴβ�ѯҪ��,ȥ��������ʾ
			this.axMapControl.ActiveView.FocusMap.ClearSelection();
			this.axMapControl.Extent = this.axMapControl.FullExtent;
			this.axMapControl.ActiveView.Refresh();
		
		}

		/// <summary>
		/// ����������ʼ���м����������Ͽ�
		/// </summary>
		private void InitializecoboSHI()
		{
			
			//���м�
			ILayer iLayerOfDishi = this.getFeatureLayerByName(m_strDSXZQY);
			if(iLayerOfDishi != null && iLayerOfDishi is FeatureLayer)
			{
				FeatureLayer fLayer = (FeatureLayer)iLayerOfDishi;	
				arryOfDishiValue = this.getArrayOfXzqmQhdm(fLayer,"QHDM","XZQM") ;
				for(int i=0;i<arryOfDishiValue.Count;i++)
				{
					ArrayList arryOfDishi = (ArrayList)arryOfDishiValue[i] ;
					//��õ��м����ƺʹ���
					string nameOfDishi = arryOfDishi[1].ToString();
					string daimaOfDishi = arryOfDishi[0].ToString();
					//�������м�combobox
					this.comboSHI.Items.Add(nameOfDishi);
				}
			}
			this.comboSHI.SelectedIndex = 0;

		}
		/// <summary>
		/// ����������ʼ�����ظ��Ͽ�
		/// </summary>
		private void InitializecoboQU()
		{
			//�õ�����
			string curnameOfDishi = this.comboSHI.SelectedItem.ToString();
			//���ؼ�
			ILayer iLayerOfQuxian = this.getFeatureLayerByName(m_strQXXZQY);
			if(iLayerOfQuxian !=null && iLayerOfQuxian is FeatureLayer)
			{
				FeatureLayer fLayer = (FeatureLayer)iLayerOfQuxian;	
				arryOfQuxianValue = this.getArrayOfXzqmQhdm(fLayer,"QHDM","XZQM") ;
			}
			//���������õ���
			if(curnameOfDishi != null)
			{
				GetQUbasedonSHI(curnameOfDishi);
			}
			
			
			
		}

		/// <summary>
		/// �����������õ�����
		/// </summary>
		/// <param name="curnameOfDishi"></param>
		/// <returns></returns>
		private void GetQUbasedonSHI(string curnameOfDishi)
		{
			string curdaimaOfDishi = null;
			//������������д���
			for(int i= 0;i< arryOfDishiValue.Count;i++)
			{
				ArrayList arryOfDishi = (ArrayList)arryOfDishiValue[i] ;
				//��õ��м����ƺʹ���
				string nameOfDishi = arryOfDishi[1].ToString();
				string daimaOfDishi = arryOfDishi[0].ToString();
				if(curnameOfDishi== nameOfDishi)
				{
					curdaimaOfDishi = daimaOfDishi;
					break;
				}
			}

			
			
			//			//��������
			//			IQueryFilter QueryFilter = new QueryFilter() as IQueryFilter ;
			//            QueryFilter.WhereClause = "XZQM like 'curdaimaOfDishi*'";


			//�����д������м���Ͻ���е�����
			for(int j=0; j<arryOfQuxianValue.Count;j++)
			{
				ArrayList arryOfQuxian = (ArrayList)arryOfQuxianValue[j] ;
				//��ȡ���ؼ����ƺʹ���
				string nameOfQuxian = arryOfQuxian[1].ToString();
				string daimaOfQuxian = arryOfQuxian[0].ToString();
				//������ش����ǰ��λ����м�������ͬ���������м�
				if( getQiansiwei(daimaOfQuxian) == getQiansiwei(curdaimaOfDishi))
				{
					this.comboQU.Items.Add(nameOfQuxian);
				}		
			}	

			if (this.comboQU.Items.Count>0) this.comboQU.SelectedIndex=0;

		}

        
		/// <summary>
		/// ������������ͼ�����õ�ͼ��
		/// </summary>
		/// <param name="layerName"></param>
		/// <returns></returns>
		private  IFeatureLayer getFeatureLayerByName(string layerName)
		{
			IFeatureLayer fLayer=null;
			IFeatureLayer resOfLayer=null;
			try 
			{
				ArrayList arrayOflayers=this.getFeatureLayers();				//�õ�ͼ������
		
				IEnumerator myEnumerator = arrayOflayers.GetEnumerator();		//����ͼ������
				while ( myEnumerator.MoveNext() )
				{
					fLayer=(IFeatureLayer)myEnumerator.Current;
					if(fLayer!= null)
					{
						if(fLayer.Name.Equals(layerName))
						{
							resOfLayer= fLayer;
							break;
						}	
					}
				}		
			} 		
			catch (Exception e) 
			{
				Debug.WriteLine("���� �õ���ѡ��layer������"+e.Message );
			}
			return resOfLayer;	
		}

		/// <summary>
		///  ��ȡ���е�ͼ������
		/// </summary>
		/// <returns></returns>
		private ArrayList getFeatureLayers()
		{
			ILayer qrylayer = null;
			FeatureLayer qryFeatLayer = null;
			ArrayList arrOfFeatlayer=new ArrayList();
			int layerCount;													//ͼ����
			try 
			{
				layerCount = this.axMapControl.Map.LayerCount;
				for(int i=0; i<layerCount; i++)
				{
					qrylayer = this.axMapControl.Map.get_Layer(i);
					//�ж�qrylayer�Ƿ�ΪFeatureLayer����
					if(!qrylayer.Name.Equals(null) && (qrylayer is FeatureLayer))
					{
						qryFeatLayer = (FeatureLayer)qrylayer;
						arrOfFeatlayer.Add(qryFeatLayer) ;						
					}
				}		
			}
			catch (Exception e) 
			{
				Debug.WriteLine("������ȡ���е�ͼ�������������"+e.Message );
			}
			return arrOfFeatlayer;
		}

		/// <summary>
		/// ����ͼ�������������롢�����������ֶ������õ��������롢��������������
		/// </summary>
		/// <param name="fLayer"></param>
		/// <param name="QhdmFieldName"></param>
		/// <param name="XzqmFieldName"></param>
		/// <returns></returns>
		private ArrayList getArrayOfXzqmQhdm(FeatureLayer fLayer, string QhdmFieldName, string XzqmFieldName)
		{
			ArrayList arryOfXzdmValue = new ArrayList() ;
			try
			{
				//���Ҫ����
				IFeatureClass ifeaturclass = fLayer.FeatureClass;

				//�ҵ��������롢�����������ֶ����ڵı��
				int numOfQhdmField = ifeaturclass.FindField(QhdmFieldName);
				int numOfXzqmField = ifeaturclass.FindField(XzqmFieldName);

				if(numOfQhdmField == -1 || numOfXzqmField ==-1)
				{
					MessageBox.Show("�Բ��𣬸�ͼ��û�д��ֶ�");
				}
					//				else
					//				{
					//					int id = Convert.ToInt32(ifeaturclass.GetFeature(0).get_Value(0));
					//					//�����������		
					//					for(int i=id;i<=ifeaturclass.FeatureCount(null); i++)
					//					{
					//						//���������ֵ
					//						string qhdm = ifeaturclass.GetFeature(i).get_Value(numOfQhdmField).ToString();
					//						//����������ֵ
					//						string xzqm = ifeaturclass.GetFeature(i).get_Value(numOfXzqmField).ToString();
					//						//������������������������Ӧ�Ķ�ά����
					//						ArrayList myarry = new ArrayList() ;
					//						myarry.Add(qhdm);
					//						myarry.Add(xzqm); 
					//						arryOfXzdmValue.Add(myarry); 
					//					}	
					//				}
				else
				{
					IFeatureCursor pCursor = null;
					pCursor =ifeaturclass.Search(null,true);
					IFeature ifeature = pCursor.NextFeature();
					while(ifeature != null)
					{
						//���������ֵ
						string qhdm = ifeature.get_Value(numOfQhdmField).ToString();
						//����������ֵ
						string xzqm = ifeature.get_Value(numOfXzqmField).ToString();
						//������������������������Ӧ�Ķ�ά����
						ArrayList myarry = new ArrayList() ;
						myarry.Add(qhdm);
						myarry.Add(xzqm); 
						arryOfXzdmValue.Add(myarry); 
						ifeature = pCursor.NextFeature();
					}
				}
				
			}
			catch (Exception e) 
			{
				// TODO �Զ����� catch ��
				Debug.WriteLine("---getArrayOfXzqmQhdm has error ---:"+e.Message) ;
			}
			return arryOfXzdmValue;
		}

		/// <summary>
		/// �����ַ�����ǰ4λ
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private string getQiansiwei(string name)
		{
			string res = null;
			try
			{
				res = name.Substring(0,4); 
			}
			catch (Exception e) 
			{
				// TODO �Զ����� catch ��
				Debug.WriteLine("getQiansiwei has an error:"+e.Message) ;
			}	
			return res;
		}

		#endregion

		
	}
}

