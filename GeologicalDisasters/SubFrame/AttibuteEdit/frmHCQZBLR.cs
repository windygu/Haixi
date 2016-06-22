using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Carto;
using Functions;

namespace JCZF.SubFrame
{
    public partial class frmHCQZBLR : DevComponents.DotNetBar.Office2007Form
    {
        //private double[,] xy;

        //private IPointCollection pMultiPoint = new Multipoint() as IPointCollection ;
        private IPointCollection pGonColl=new Polygon() as IPointCollection ;

        private AxMapControl m_axmapcontrol;




        public frmHCQZBLR(frmMapView m_frmMapView)
        {
            this.m_axmapcontrol = m_frmMapView.axMapControl1;
            InitializeComponent();
        }


        private void txtX_Leave(object sender, EventArgs e)
        {
            if (!IsNumber(txtX.Text))
            {
                MessageBox.Show("请输入数值型数据");
            }

        }

        private void txtY_Leave(object sender, EventArgs e)
        {
            if (!IsNumber(this.txtY.Text))
            {
                MessageBox.Show("请输入数值型数据");
            }

        }

        private IRgbColor getRGB(int r, int g, int b)
        {
            IRgbColor pColor = new RgbColor() as IRgbColor;
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
        }




        #region

        //判断是否为数值型
        public bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                   !objTwoDotPattern.IsMatch(strNumber) &&
                   !objTwoMinusPattern.IsMatch(strNumber) &&
                   objNumberPattern.IsMatch(strNumber);
        }

        //添加一条记录
        private void Add()
        {
            int i = listView1.Items.Count;
            this.txtXH.Text = (i + 1).ToString();
            this.listView1.Items.Insert(i, (System.Convert.ToInt32(System.Convert.ToDouble(i + 1))).ToString("#"));
            this.listView1.Items[i].SubItems.Add(txtX.Text.ToString());
            this.listView1.Items[i].SubItems.Add(txtY.Text.ToString());  
        }
        //存储坐标
        //private void getxy()
        //{
        //    int i = listView1.Items.Count;
        //    xy = new double[i, 2];
        //    for (int j = 0; j < i; j++)
        //    {
        //        xy[j, 0] = Convert.ToDouble(listView1.Items[j].SubItems[1].Text);
        //        xy[j, 1] = Convert.ToDouble(listView1.Items[j].SubItems[2].Text);
        //    }
        //}

        //获取多边形个数
        private int GetPGCount()
        {
            if (listView1.Items == null || listView1.Items.Count <= 0)
            {
                return 0;
            }
            return System.Convert.ToInt32(listView1.Items[2].SubItems[2].Text);
        }


        private void Add(ArrayList arr)
        {
            for (int i = 0; i < arr.Count/2;i++ )
            {
                //int i = listView1.Items.Count;
                this.txtXH.Text = (i + 1).ToString();
                this.listView1.Items.Insert(i, (System.Convert.ToInt32(System.Convert.ToDouble(i + 1))).ToString("#"));
                this.listView1.Items[i].SubItems.Add(arr[2 * i].ToString());
                this.listView1.Items[i].SubItems.Add(arr[2 * i + 1].ToString());
            }
 
        }

        private ArrayList GetarrDD()
        {
            ArrayList arr = new ArrayList();
            int count= listView1.Items.Count;          
            for (int i = 0; i < count; i++)
            {
                if (listView1.Items[i].SubItems[1].Text == "顶点数")
                {
                    arr.Add(listView1.Items[i].SubItems[2].Text); 
                }                
            }
            return arr; 
        }


        private void Add2(ArrayList arr)
        {
            int flag = 2;
            for (int i = 0; i < arr.Count ; i++)
            {
                //int i = listView1.Items.Count;
                this.txtXH.Text = (i + 1).ToString();
                this.listView1.Items.Insert(i, (System.Convert.ToInt32(System.Convert.ToDouble(i + 1))).ToString("#"));
                if (i == 0)
                {
                    this.listView1.Items[i].SubItems.Add("文件识别码");
                    this.listView1.Items[i].SubItems.Add(arr[i].ToString());
                }
                else if (i == 1)
                {
                    this.listView1.Items[i].SubItems.Add("最后更新时间");
                    this.listView1.Items[i].SubItems.Add(arr[i].ToString()); 
                }
                else if (i == 2)
                {
                    this.listView1.Items[i].SubItems.Add("多边形个数");
                    this.listView1.Items[i].SubItems.Add(System.Convert.ToInt32(arr[i].ToString()).ToString());
                }
                else if (i == 3)
                {
                    this.listView1.Items[i].SubItems.Add("预留位");
                    this.listView1.Items[i].SubItems.Add(System.Convert.ToInt32(arr[i].ToString()).ToString());
                }                
                else
                {
                    if (arr[i].ToString().Contains(","))
                    {
                        string[] split = arr[i].ToString().Split(new char[] { ',' });
                        foreach (string s in split)
                        {
                            this.listView1.Items[i].SubItems.Add(s);
                        }
                    }
                    else
                    {
                        if (flag%2== 0)
                        {
                            this.listView1.Items[i].SubItems.Add("序号");
                            this.listView1.Items[i].SubItems.Add(System.Convert.ToInt32(arr[i].ToString()).ToString());
                            flag ++;
                        }
                        else
                        {
                            this.listView1.Items[i].SubItems.Add("顶点数");
                            this.listView1.Items[i].SubItems.Add(System.Convert.ToInt32(arr[i].ToString()).ToString());
                            flag ++;
                        }
 
                    }

                }




            }

        }

        //生成点，并增加点到multipoint
        private IPointCollection addtomulpt(double[,] xy)
        {
            IPointCollection pMultiPoint = new Multipoint() as IPointCollection ;
            for (int i = 0; i <xy.Length/2; i++)
            {
                IPoint point = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                point.PutCoords(xy[i, 0], xy[i, 1]);
                object missing = Type.Missing;

                pMultiPoint.AddPoint(point, ref missing, ref missing);
            }
            return pMultiPoint;           
        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            this.txtXH.Text=this.listView1.SelectedItems[0].SubItems[0].Text;
            this.txtX.Text = this.listView1.SelectedItems[0].SubItems[1].Text;
            this.txtY.Text = this.listView1.SelectedItems[0].SubItems[2].Text;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems == null || listView1.SelectedItems.Count == 0)
            {
                return;
            }


             int i = Convert.ToInt32(txtXH.Text.ToString());
             this.listView1.Items[i - 1].SubItems[1].Text = txtX.Text.ToString();
             this.listView1.Items[i - 1].SubItems[2].Text = txtY.Text.ToString();  
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems == null || listView1.SelectedItems.Count == 0)
            {
                return;
            }
            int i = Convert.ToInt32(txtXH.Text.ToString());
            this.listView1.Items[i - 1].Remove();
            for (int j = i - 1; j < listView1.Items.Count; j++)
            {
                listView1.Items[j].SubItems[0].Text = System.Convert.ToInt32(System.Convert.ToDouble(j+1)).ToString("#");
            }

        }
        
        //存储坐标  
        private double[,] getxyCoord(int i,int ddcount)
        { 
            double[,] xy = new double[ddcount, 2];
            
            
            int count = listView1.Items.Count;
            for (int k = 0; k < count; k++)
            {
                if (listView1.Items[k].SubItems[1].Text == "序号" && listView1.Items[k].SubItems[2].Text == (i+1).ToString())
                {
                    for (int j = 0; j < ddcount; j++)
                    {
                        xy[j, 0] = Convert.ToDouble(listView1.Items[k +2+ j].SubItems[1].Text);
                        xy[j, 1] = Convert.ToDouble(listView1.Items[k + 2+j].SubItems[2].Text);
                    }
                    break;
                }
            }
            
            return xy; 

        }

        private void RemoveOverviewLayer()
        {
           IEnumLayer m_EnumLayer=MapFunction.GetLayers(m_axmapcontrol.Map);
           int index=0;
           ILayer m_Layer=m_EnumLayer.Next();

            while(m_Layer!=null)
            {
                if(m_Layer.Name=="overview")
                {
                    m_axmapcontrol.DeleteLayer(index);
                }
                m_Layer=m_EnumLayer.Next();
            }
            m_EnumLayer.Reset();

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            
            RemoveOverviewLayer();
            try
            {

            int PGCount=GetPGCount();
            ArrayList arrDD = GetarrDD();
            for (int k = 0; k < PGCount; k++)
            {
                //getxy();
                double[,] xy = getxyCoord(k, System.Convert.ToInt32(arrDD[k]));

                IPointCollection pMultiPoint = addtomulpt(xy);
                if (pMultiPoint.PointCount < 3)
                {
                    MessageBox.Show("必须至少输入三组坐标");
                    return;
                }

                ISegmentCollection pSegCol;
                pSegCol = new Ring() as ISegmentCollection ;
                object Missing1 = Type.Missing;
                object Missing2 = Type.Missing;

                for (int i = 0; i < pMultiPoint.PointCount - 1; i++)
                {
                    ILine pLine = new Line() as ILine;
                    pLine.PutCoords(pMultiPoint.get_Point(i), pMultiPoint.get_Point(i + 1));
                    pSegCol.AddSegment(pLine as ISegment, ref  Missing1, ref Missing2);
                }

                IRing pRing;
                pRing = pSegCol as IRing;

                pRing.Close();
                 IGeometryCollection pPolygon;
                pPolygon = new Polygon() as IGeometryCollection;
                pPolygon.AddGeometry(pRing, ref Missing1, ref Missing2);

                #region
                IGeometry geometry = pPolygon as IGeometry;
                geometry.SpatialReference = m_axmapcontrol.SpatialReference;

                IEnumLayer m_layers = MapFunction.GetLayers(m_axmapcontrol.Map);
                IFeatureLayer pfeaturelayer = new FeatureLayer() as IFeatureLayer;
                ILayer player = m_layers.Next();
                while (player != null)
                {
                    if (player.Name == "土地核查")
                    {
                        pfeaturelayer = (IFeatureLayer)player;
                        break;
                    }
                    player = m_layers.Next();
                }
                m_layers.Reset();

                IFeatureClass pfeatureclass = pfeaturelayer.FeatureClass;

                IWorkspaceEdit workspaceEdit = (pfeaturelayer as IDataset).Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                IFeature feature = pfeatureclass.CreateFeature();
                feature.Shape = geometry;
                try
                {
                    feature.Store();
                }
                catch
                {
                    ITopologicalOperator topologicaloperator = pPolygon as ITopologicalOperator;
                    topologicaloperator.Simplify();
                    feature.Store();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                m_axmapcontrol.Extent = feature.Extent;

                //高亮显示
                IFeatureSelection featureSelection = pfeaturelayer as IFeatureSelection;
                featureSelection.Add(feature);

                IRgbColor m_color = new RgbColor() as IRgbColor;
                m_color.Red = 207;
                m_color.Green = 70;
                m_color.Blue = 215;
                featureSelection.SelectionColor = m_color;
                m_axmapcontrol.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, pfeaturelayer, null);
            }  
               
            } 
            catch(Exception ex)
            {
                MessageBox.Show("文件格式不对！");
            }

            //m_axmapcontrol.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, pfeaturelayer, null);
            m_axmapcontrol.ActiveView.Refresh();            
            #endregion





        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog pDlg = new OpenFileDialog();				//打开文件对话框
            pDlg.Title = "打开文本文件";

            pDlg.Filter = "文本文件(*.txt)|*.txt";

            if (pDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
              string filepath = System.IO.Path.GetDirectoryName(pDlg.FileName);          
            string filename = System.IO.Path.GetFileName(pDlg.FileName); 

            StreamReader sr = new StreamReader(filepath + "\\" + filename);
            ArrayList arr=new ArrayList(); 
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                string[] split = str.Split(new char[] { ';' });
                foreach (string s in split)
                {
                    if (s.Trim() != "")
                        arr.Add(s);
                }
            }
            Add2(arr);


        }


      public string ReadData(string name)
      {
          //创建 FileStream 的对象,说白了告诉程序,文件在那里,对文件如何处理,对文件内容采取的处理方式
          System.IO.FileStream fs = new System.IO.FileStream(name, FileMode.Open, FileAccess.Read);
       
         //仅 对文本 进行 读写操作
          string txt = "";
          StreamReader sr = new StreamReader(fs);
          //读一下，看看文件内有没有内容，为下一步循环 提供判断依据
          //sr.ReadLine() 这里是 StreamReader的方法 可不是 console 中的~ 
          string str = sr.ReadLine();                      //如果 文件有内容 
          while (str != null)
          {
           //输出字符串，str 在上面已经定义了 读入一行字符 
            //Console.WriteLine("{0}", str);
             txt+=str;
            //这里我的理解是 当输出一行后，指针移动到下一行~
            //下面这句话就是 判断 指针所指这行是否有内容~
            str = sr.ReadLine();                     
          }
           //关闭文件，注意顺序，先对文件内部进行关闭，然后才是文件~
          sr.Close();
          fs.Close();
          return txt; 
    }

        private void button1_Click(object sender, EventArgs e)
        {
            string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa

            string m_tempFilePath = Application.StartupPath + "\\temp\\overview" + time;  //aa            
            IFeatureClass m_OverViewFeatureclass = CreateFeatureClass(m_tempFilePath,m_axmapcontrol.SpatialReference);

            try
            {
                int PGCount = GetPGCount();
                ArrayList arrDD = GetarrDD();
                for (int k = 0; k < PGCount; k++)
                {
                    //getxy();
                    double[,] xy = getxyCoord(k, System.Convert.ToInt32(arrDD[k]));

                    IPointCollection pMultiPoint = addtomulpt(xy);
                    if (pMultiPoint.PointCount < 3)
                    {
                        MessageBox.Show("必须至少输入三组坐标");
                        return;
                    }

                    ISegmentCollection pSegCol;
                    pSegCol = new Ring() as ISegmentCollection ;
                    object Missing1 = Type.Missing;
                    object Missing2 = Type.Missing;

                    for (int i = 0; i < pMultiPoint.PointCount - 1; i++)
                    {
                        ILine pLine = new Line() as ILine;
                        pLine.PutCoords(pMultiPoint.get_Point(i), pMultiPoint.get_Point(i + 1));
                        pSegCol.AddSegment(pLine as ISegment, ref  Missing1, ref Missing2);
                    }

                    IRing pRing;
                    pRing = pSegCol as IRing;

                    pRing.Close();
                    IGeometryCollection pPolygon;
                    pPolygon = new Polygon() as IGeometryCollection;
                    pPolygon.AddGeometry(pRing, ref Missing1, ref Missing2);

                    IGeometry geometry = pPolygon as IGeometry;
                    geometry.SpatialReference = m_axmapcontrol.SpatialReference;

                    IWorkspaceEdit workspaceEdit = (m_OverViewFeatureclass as IDataset).Workspace as IWorkspaceEdit;
                    workspaceEdit.StartEditing(true);
                    workspaceEdit.StartEditOperation();
                    IFeature feature = m_OverViewFeatureclass.CreateFeature();
                    feature.Shape = geometry;
                    try
                    {
                        feature.Store();
                    }
                    catch
                    {
                        ITopologicalOperator topologicaloperator = pPolygon as ITopologicalOperator;
                        topologicaloperator.Simplify();
                        feature.Store();
                    }
                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);
                }

                //m_axmapcontrol.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, pfeaturelayer, null);
                m_axmapcontrol.AddShapeFile(m_tempFilePath, "overview");
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件格式不对！");
            }
            m_axmapcontrol.ActiveView.Refresh(); 

        }


        public IFeatureClass CreateFeatureClass(string m_tempFilePath, ISpatialReference SpatialReference)
        {

            IFeatureClass m_OverViewFeatureClass;

            if (!System.IO.Directory.Exists(m_tempFilePath))
            {
                System.IO.Directory.CreateDirectory(m_tempFilePath);
            }
            else
            {
                System.IO.Directory.Delete(m_tempFilePath, true);
                System.IO.Directory.CreateDirectory(m_tempFilePath);
            }

            //string clipFolder = this.m_txtBoxsavePath.Text;
            string overview = "overview";
            string shapeFieldName = "shape";//?

            //open the folder to contain the shapefile as a workspace
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
            IWorkspace pworkspace = pWorkspaceFactory.OpenFromFile(m_tempFilePath, 0);
            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pworkspace;

            //set a simple fields collection
            IFields pFields = new Fields() as IFields ;
            IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;

            //make the shape fied
            IField pField = new Field() as IField ;
            IFieldEdit pFieldEdit = (IFieldEdit)pField;   //edit the field properties

            IGeometryDef pGeomdef = new GeometryDef() as IGeometryDef ;   //reture information about the geometry definition
            IGeometryDefEdit pGeomdefEdit = (IGeometryDefEdit)pGeomdef;   //modify the geometry definition

            pGeomdefEdit.SpatialReference_2 = SpatialReference;   //the spacial reference of dataset, write only
            pGeomdefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;   //the geometry type, write only

            pFieldEdit.Name_2 = "shape";   //the name of the field, write only
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;//the type of the field, write only **
            pFieldEdit.GeometryDef_2 = pGeomdef;//the definition of geometry if IsGeometry is true,write only

            pFieldsEdit.AddField(pField);

            //create the shapefile
            m_OverViewFeatureClass = pFeatureWorkspace.CreateFeatureClass(overview, pFields, null, null, esriFeatureType.esriFTSimple, shapeFieldName, "");

            //IFeature pFea = m_ClipFeatureClass.CreateFeature();//wb
            //pFea.Shape = geo;//wb
            //pFea.Store();//wb

            return m_OverViewFeatureClass;
        }

        private void frmHCQZBLR_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveOverviewLayer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ZuobiaoDC())
                {
                    MessageBox.Show("导出成功！");

                }
                else
                {
                    MessageBox.Show("导出失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败！");
            }
        }

        private bool ZuobiaoDC()
        {
            try
            {               
                //导出路径          
                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                SaveFileDialog1.CreatePrompt = true;
                SaveFileDialog1.OverwritePrompt = true;
                SaveFileDialog1.FileName = "exportcoord";
                SaveFileDialog1.DefaultExt = "txt";
                SaveFileDialog1.Filter = "Text files (*.txt)|*.txt";
                SaveFileDialog1.InitialDirectory = "c:\\";
                DialogResult result = SaveFileDialog1.ShowDialog();
                System.IO.Stream fileStream;

                if (result == DialogResult.OK)
                {
                    fileStream = SaveFileDialog1.OpenFile();
                    StreamWriter sw = new StreamWriter(fileStream);

                    sw.Write("{0,-10}", "行号");
                    sw.Write("{0,-40}", "X坐标");
                    sw.Write("{0,-40}", "Y坐标");                    
                    sw.WriteLine();
                  
                    for (int j = 0; j < listView1.Items.Count; j++)
                    {
                        sw.Write("{0,-10}",listView1.Items[j].SubItems[0].Text.Trim());
                        sw.Write("{0,-40}", listView1.Items[j].SubItems[1].Text.Trim());
                        sw.Write("{0,-40}", listView1.Items[j].SubItems[2].Text.Trim());
                        sw.WriteLine();
                    }                   
                    sw.Close();
                    fileStream.Close();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }







    }
}