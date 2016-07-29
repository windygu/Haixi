using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GISHandler;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.EditorExt;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using System.Web.Http;
using _sdnMap;
using IdentifyTool;


namespace ComprehensiveEvaluation
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private bool isMeasureLong = false;
        private bool isMeasureArea = false;
        public  bool attribute = false;
        private bool polygonSt = false;
        private bool edit = false;
        private bool jian1 = false;
        private bool jian2 = false;
        private IdentifyDialog identifyDialog =new IdentifyDialog();
        //TOCControl控件变量
        private ITOCControl2 m_tocControl = null;
        //TOCControl中Map菜单
        private IToolbarMenu m_menuMap = null;
       //TOCControl中图层菜单
        private IToolbarMenu m_menuLayer = null;
        //pagelayout菜单
        private IToolbarMenu p_menuLayer = null;
     
        public Modules.ucFileNavPanel ucFileNavPanel = null;
        IHookHelper m_hookHelper = new HookHelperClass();
        int width;
        //构造函数
        public MainForm()
        {
            InitializeComponent();
            width = Width;
        }


        public  void Hook(IHookHelper m_hookHelper)
        {
            this.m_hookHelper = m_hookHelper;
            this.m_hookHelper.Hook = axMapControl1.Object;
        }
        private void setFormSize()
        {
            splitContainerControl1.SplitterPosition = Convert.ToInt32(0.2 * width);
            splitContainerControl2.SplitterPosition = Convert.ToInt32(0.6 * width);
        }
        //启动窗口加载
        private void MainForm_Load(object sender, EventArgs e)
        {
            setFormSize();
            //axMapControl1.LoadMxFile(SystemSet.Base_Map + "\\基础底图.mxd", 0, Type.Missing);
            axMapControl1.LoadMxFile(@"G:\MyProject\海西综合评价系统\数据库\邵武DEM.mxd", 0, Type.Missing);  
            //axMapControl1.AddShapeFile(@"I:\四平项目\实验数据", "东丰县行政区域");
            m_menuMap = new ToolbarMenuClass();
            m_menuLayer = new ToolbarMenuClass();
            p_menuLayer = new ToolbarMenuClass();

            p_menuLayer.AddItem(new AddName(axPageLayoutControl1), -1, 0, true, esriCommandStyles.esriCommandStyleTextOnly);

            p_menuLayer.AddItem(new AddLegend(), -1, 1, true, esriCommandStyles.esriCommandStyleTextOnly);

            p_menuLayer.AddItem(new AddNorthArrow(axPageLayoutControl1), -1, 2, true, esriCommandStyles.esriCommandStyleTextOnly);

            p_menuLayer.AddItem(new AddScal(axPageLayoutControl1), -1, 3, true, esriCommandStyles.esriCommandStyleTextOnly);
            //打开文档菜单
           // m_menuMap.AddItem(new GISTools(AddData), -1, 0, false, esriCommandStyles.esriCommandStyleIconAndText);
            //刷新
            m_menuMap.AddItem(new refresh(), -1, 0, false, esriCommandStyles.esriCommandStyleIconAndText);
            //

            //添加数据菜单
            //m_menuMap.AddItem(new ControlsAddDataCommandClass(), -1, 0, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_menuMap.AddItem(new addData(), -1, 1, false, esriCommandStyles.esriCommandStyleIconAndText);
            //全局显示
            //m_menuMap.AddItem(new ControlsMapFullExtentCommandClass(), -1, 1, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_menuMap.AddItem(new fullExtent(), -1, 2, false, esriCommandStyles.esriCommandStyleIconAndText);
            //移动
            //m_menuMap.AddItem(new ControlsMapPanToolClass(), 1, 2, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_menuMap.AddItem(new pan(), 2, 3, false, esriCommandStyles.esriCommandStyleIconAndText);
            //移除图层菜单
            m_menuLayer.AddItem(new RemoveLayer(), -1, 0, false, esriCommandStyles.esriCommandStyleTextOnly);
            //放大到整个图层
            m_menuLayer.AddItem(new ZoomToLayer(), -1, 1, true, esriCommandStyles.esriCommandStyleTextOnly);
            m_menuLayer.SetHook((IMapControl3)axMapControl1.Object);
            m_menuMap.SetHook((IMapControl3)axMapControl1.Object);
            axTOCControl1.SetBuddyControl(axMapControl1);
           // axTOCControl1.SetBuddyControl(axMapControl2);
        }

        #region GIS Map Tools
        #region GIS基本操作工具
        //
        //打开文件
        private void btn_OpenMapFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;    //单选
            ofd.Title = "选择地图文件";
            ofd.Filter = "mxd文件|*.mxd";
            ofd.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);
                if (fi.Exists)
                {
                    axMapControl1.LoadMxFile(fi.FullName);
                    axMapControl1.ActiveView.Refresh();
                   
                }
            }
        }
        //图层输出
        private void btn_ExportMapPic_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //输出当前活动窗口内容
            edit = true;
            if (edit)
            {
                GISHandler.GISTools.ExportImage(axPageLayoutControl1.ActiveView);
                edit = false;
            }

            else
                GISHandler.GISTools.ExportImage(axMapControl1.ActiveView);

           // 
        }
        //添加图层
        private void btn_AddMapLayer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          /*  OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;    //多选
            ofd.Title = "选择图层";
            ofd.Filter = "shp文件|*.shp";
            ofd.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);
                if (fi.Exists)
                {
                    string path = ofd.FileName;
                    this.axMapControl1.AddShapeFile(path,ofd.FileName);
                    this.axMapControl1.ActiveView.Refresh();
                }
            }*/
          GISHandler.GISTools.AddData_SHP(axMapControl1);
            
        //  axMapControl1.AddLayerFromFile(@"I:\四平项目\东丰县2000数据库", 1);
        }
        //移动图层
        private void btn_Pan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.Pan(axMapControl1);
        }
        //放大
        private void btn_ZoomIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.ZoomIn(axMapControl1);
        }
        //缩小
        private void btn_ZoomOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.ZoomOut(axMapControl1);
        }
        //渐大
        private void btn_ScaleIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.ZoomInFix(axMapControl1);
        }
        //渐小
        private void btn_ScaleOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.ZoomOutFix(axMapControl1);
        }
        //选择
        private void btn_Select_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.SelectFeature(axMapControl1);
        }
        //全局
        private void btn_FullExtent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.FullExtent(axMapControl1);
        }
        //前一视图
       private void btn_FrontView_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.MapForwardView(axMapControl1);
        }
       //后一视图
        #endregion
       #region GIS 辅助工具
       private void btn_NextView_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.MapNextView(axMapControl1);
        }
        //长度测量  完成
        private void btn_MapMeasureLength_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
       {
               isMeasureLong = true;
          
       }
        //面积测量  完成
        private void btn_MapMeasureArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            isMeasureArea = true;
        }
        //属性查看  引用控件
        private void btn_MapIdentifyInfo_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
        //axMapControl1.OnMouseDown += axMapControl1_OnMouseDown;
        attribute = true;
        //GISHandler.GISTools.Edit(this.axMapControl1);
        ShowIdentifyDialog();
        }
        private void ShowIdentifyDialog()
        {
            //新建属性查询对象
            identifyDialog = IdentifyDialog.CreateInstance(axMapControl1);
            identifyDialog.Owner = this;
            identifyDialog.Show();
        }
       
     
      
       #endregion
        //数据集管理（坐标数据）····未添加
        private void btn_ManageOfData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabPage_DataNav.Show();
            // LoadFiles("G:\\数据库\\坐标数据");
            GISHandler.GISTools.GainFile(@"G:\四平项目\数据库\坐标数据", listView1, imageList1);
            axMapControl1.ClearLayers();
            //axMapControl1.LoadMxFile(@"G:\数据库\地图数据\演示数据.mxd", 0, Type.Missing);
            axMapControl1.OnMouseDown += axMapControl1_OnMouseDown;
            edit = true;
           

        }
       
        //坐标信息的显示
        private void axMapControl1_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
       {
           //显示当前比例尺
           Coordinate.Text = "比例尺 1:" + ((long)axMapControl1.MapScale).ToString() + "  , 当前坐标X=" + e.mapX.ToString("0.000") + "°E,Y=" + e.mapY.ToString("0.000") + "°N,";
       identifyDialog.OnMouseMove(e.mapX, e.mapY);
            //显示当前坐标信息
         
        }


        #endregion

        #region //设置与帮助

        private Uri url;
        private void btn_HelpDocument_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            //url = new Uri("http://www.google.com"); //默认google
            //this.webBrowser1.Url = url;
            System.Diagnostics.Process.Start("iexplore.exe", "http://www.baidu.com");
        }

        private void btn_About_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("北京穆图科技有限公司", "关于我们");
        }
        #endregion 
        //mapcontrol和pagelayerout挂链；
        private void axMapControl1_OnMapReplaced(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMapReplacedEvent e)
        {
            GISHandler.GISTools.copyToPageLayerOut(axMapControl1, axPageLayoutControl1);
            GISHandler.GISTools.onMapReplace(axMapControl1, axMapControl2);
        }
        //mapcontrol和pagelayerout挂链；
        private void axMapControl1_OnAfterScreenDraw(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            GISHandler.GISTools.ScreenDraw(axMapControl1, axPageLayoutControl1);
            GISHandler.GISTools.copyToPageLayerOut(axMapControl1, axPageLayoutControl1);
        }
        //画线+测量
        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            //this.Cursor = Cursors.Default;
            if (e.button==1) //== MouseButtons.Left)
            {
                if (isMeasureLong)
                {
                    GISHandler.GISTools.CreatLine(axMapControl1);
                    isMeasureLong = false;
                }
                if (isMeasureArea)
                {
                   GISHandler.GISTools.MeasureArea(axMapControl1);
                    // 

                    isMeasureArea = false;
                }
                if(polygonSt)
                {
                    GISHandler.GISTools.FreePolygonSt(axMapControl1, SystemSet.Base_Map+"\\处理数据库\\图层数据");
                    polygonSt = false;
                }
                if (attribute)
                {
                   // GISHandler.GISTools.IdentifyTool(this.axMapControl1);
                    if (identifyDialog.IsDisposed)
                    {
                        ShowIdentifyDialog();
                    }
                    identifyDialog.OnMouseDown(e.button, e.mapX, e.mapY);
                    
                    //attribute = false;
                }
                //axMapControl1.Extent = axMapControl1.TrackRectangle();
                //axMapControl1.Refresh(esriViewDrawPhase.esriViewBackground, null, null);

               
            }
            
            if (e.button == 2)
            {
            //    axMapControl1.Pan(); 

            //    axMapControl1.Refresh(esriViewDrawPhase.esriViewBackground, null, null); 
                GISHandler.GISTools.setNull(axMapControl1);
                    //弹出右键菜单
                IMapControl3 m_mapControl = (IMapControl3)axMapControl1.Object;

                m_menuMap.PopupMenu(e.x, e.y, m_mapControl.hWnd);
              
            }
            
            
        }
        //保存
        private void btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //GISHandler.GISTools.Save(this.axMapControl1);
            GISHandler.GISTools.SaveDocument2(axMapControl1);

        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.refresh(axMapControl1);
           // GISHandler.GISTools.setNull(this.axMapControl1);
            attribute = false;
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button != 2) return;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null;
            ILayer layer = null;
            object other = null;
            object index = null;
            //判断所选菜单的类型
            axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            //m_tocControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            //确定选定的菜单类型，Map或是图层菜单
            if (item == esriTOCControlItem.esriTOCControlItemMap)

                axTOCControl1.SelectItem(map, null);
            else if (item == esriTOCControlItem.esriTOCControlItemLayer)

                axTOCControl1.SelectItem(layer, null);
            else
                return;

            //设置CustomProperty为layer (用于自定义的Layer命令)  
            IMapControl3 m_mapControl = (IMapControl3)axMapControl1.Object;
            m_mapControl.CustomProperty = layer;
            //弹出右键菜单
            if (item == esriTOCControlItem.esriTOCControlItemMap)

                m_menuMap.PopupMenu(e.x, e.y, axTOCControl1.hWnd);

            if (item == esriTOCControlItem.esriTOCControlItemLayer)

                m_menuLayer.PopupMenu(e.x, e.y, axTOCControl1.hWnd);

        }

        private void axPageLayoutControl1_OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 1)
            {
             //   if()
             //GISHandler.GISTools.addNorthArrow(this.axPageLayoutControl1, axPageLayoutControl1.ActiveView.FocusMap);
            }
            if (e.button == 2)
            {
                IPageLayoutControl p_mapControl = (IPageLayoutControl)axPageLayoutControl1.Object;

                p_menuLayer.PopupMenu(e.x, e.y, p_mapControl.hWnd);
            }
        }

        private void axPageLayoutControl1_OnMouseMove(object sender, IPageLayoutControlEvents_OnMouseMoveEvent e)
        {
            labelControl1.Text = "当前坐标:X=" + e.pageX.ToString() + ",Y=" + e.pageY.ToString() + ",";
        }
  
        private void axMapControl1_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            identifyDialog.OnMouseUp(e.mapX, e.mapY);
        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SystemSet set = new SystemSet();
            set.Show();
        }

       
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.clipRaster(axMapControl1);
            
        
        
        }
       
        //=======================================================================
        //从groupLayer中查找FeatureLayer
        public static IFeatureLayer getSubLayer(ILayer layers)
        {
            IFeatureLayer l = null;
            ICompositeLayer compositeLayer = layers as ICompositeLayer;
            for (int i = 0; i < compositeLayer.Count; i++)
            {
                ILayer layer = compositeLayer.Layer[i];   //递归
                if (layer is GroupLayer || layer is ICompositeLayer)
                {
                    //MessageBox.Show(layer.Name);
                    l = getSubLayer(layer);
                    
                    if (l != null)
                    {
                        continue;
                    }
                }
                else
                {
                    //while (layer.Name.Equals(layerName))
                    //{
                    try
                    {
                        l = layer as IFeatureLayer;
                        MessageBox.Show(l.Name );
                       
                    }
                    catch
                    {
                        try
                        {
                           IRasterLayer r = layer as IRasterLayer;
                           MessageBox.Show(r.Name);
                        }
                        catch
                        { }
                    }
                    //}
                }
            }
          
            return l;
        }
        public static IFeatureLayer getLayerFromName(AxMapControl mapControl)
        {
            IFeatureLayer layer = null;
            int s = 0;
            for (int i = 0; i < mapControl.LayerCount; i++)
            {
                ILayer layers = mapControl.get_Layer(i);
                if (layers is GroupLayer || layers is ICompositeLayer)   //判断是否是groupLayer
                {
                    MessageBox.Show(layers.Name);
                    //创建文件夹：slgc，ztdt，bhzy
                    if(layer.Name.Equals("水利工程"))
                    {
                        //创建文件夹：slgc

                        //将该文件路径传入函数中
                        layer = getSubLayer(layers);  //递归的思想
                    }
                    else if (layer.Name.Substring(0, 2).Equals("方案"))
                    {
                        s++;
                        //创建ztdu文件夹

                        //传入参数  ztdt和方案i

                        layer = getSubLayer(layers);  //递归的思想

                    }
                    else
                    {
                        //创建bhzy文件夹

                        //传入参数
                         layer = getSubLayer(layers);  //递归的思想
                    } 
                    
                    if (layer != null)
                    {
                        continue;
                    }
                }
                else
                {
                    //if (mapControl.get_Layer(i).Name.Equals(layerName))
                    //{
                        layer = layers as IFeatureLayer;
                       
                    //}
                }
            }
            //MessageBox.Show(layer.Name);
            return layer;
        }
        private string RasterDataSourse(string LayerName)
        {
            try
            {
                IRasterLayer pRasterLayer = GetRasterLayer(LayerName);
                return pRasterLayer.FilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private IRasterLayer GetRasterLayer(string layerName)
        {
            //get the layers from the maps
            IEnumLayer layers = GetLayers();
            layers.Reset();

            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    return layer as IRasterLayer;
            }

            return null;
        }
        private IFeatureLayer GetFeatureLayer(string layerName)
        {
            //get the layers from the maps
            IEnumLayer layers = GetLayers();
            layers.Reset();

            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    return layer as IFeatureLayer;
            }

            return null;
        }
        private IEnumLayer GetLayers()
        {

            UID uid = new UIDClass();
           uid.Value = "{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}";//获取所有图层
          //   uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";// 代表只获取矢量图层
            //问题在这个地方 解决！
           IEnumLayer layers = axMapControl1.ActiveView.FocusMap.get_Layers(uid,true);// .FocusMap.get_Layers(uid, true);
            return layers;
        }
        //=======================================================================
      

        private void axMapControl1_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            if (attribute)
            {
                attribute = false;
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabPage_MapLayers.Show();
            axMapControl1.LoadMxFile(@"G:\四平项目\数据库\地图数据\演示数据.mxd", 0, Type.Missing);
        }

        private void but_AddShp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.AddData_SHP(axMapControl1);
        }

        private void but_AddUDB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.AddData(axMapControl1);

        }

        private void but_AddRST_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.AddData_RST(axMapControl1);

        }

        private void but_AddCAD_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GISHandler.GISTools.AddData_CAD(axMapControl1);

        }

      

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            MessageBox.Show(listView1.FocusedItem.Text);
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(listView1.FocusedItem.Text);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(listView1.FocusedItem.Text);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("是否退出系统？", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
            else
                return;
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            polygonSt = true;
        }
        public void output_Info(string info)
        {
            try
            {
                Application.DoEvents();
                Info_Show.AppendText(info + "\r\n");
                Info_Show.Focus();
                Info_Show.SelectionStart = Info_Show.Text.Length;///焦点在最后
            }
            catch
            { }
        }
        /*窗体可用性
        public void ModelEnable(bool able)
        {
            Pae_LandExtent.Visible = able;
            Page_InfoQuery.Visible=able;
            Page_LandEcology.Visible = able;
            Page_LandRisk.Visible = able;
            Page_LandSuitability.Visible = able;
            //splitContainerControl1.Visible = able;
          
        }
 */
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tab_Zhibiao.Select();
            //Tab_Zhibiao.IsAccessible = true;
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Tab_Weight.IsAccessible = true;
           
        }

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            IEnvelope pEnv;
            pEnv = e.newEnvelope as IEnvelope;
            IGraphicsContainer pGraphicsContainer;
            IActiveView pActiveView;
            pGraphicsContainer = (IGraphicsContainer)axMapControl2.Map;
            pActiveView = (IActiveView)pGraphicsContainer;

            pGraphicsContainer.DeleteAllElements();
            IRectangleElement pRectangleElm=new RectangleElementClass();
            IElement pElm = (IElement)pRectangleElm;
            pElm.Geometry = pEnv;
            IRgbColor pColor = new RgbColorClass();
            pColor.RGB = 255;
            pColor.Transparency = 255;
            //产生一个线符号
            ILineSymbol pOutLine=new SimpleLineSymbolClass();
            pOutLine.Width = 2;
            pOutLine.Color = pColor;
            
            
            pColor = new RgbColorClass();
            pColor.RGB = 255;
            pColor.Transparency = 0;
            //设置填充符号
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutLine;
            IFillShapeElement pFillShapeElm=(IFillShapeElement)pElm;
            pFillShapeElm.Symbol = pFillSymbol;
            pElm = (IElement)pFillShapeElm;
            pGraphicsContainer.AddElement(pElm, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        }

        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 2)
                axMapControl2.Pan();
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(e.mapX, e.mapY);
            axMapControl1.CenterAt(pPoint);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            TargetSystem.Weight weight = new TargetSystem.Weight();
            GISHandler.GISTools.loadForm(weight);
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            TargetSystem.Target taget = new TargetSystem.Target("指标管理");
            GISHandler.GISTools.loadForm(taget);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            TargetSystem.Format format = new TargetSystem.Format();
            GISHandler.GISTools.loadForm(format);
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            TargetSystem.ModelManagement model = new TargetSystem.ModelManagement();
            GISHandler.GISTools.loadForm(model);
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExcuteWinform.OverLayer overLayer = new ExcuteWinform.OverLayer();
            GISHandler.GISTools.loadForm(overLayer);
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExcuteWinform.InsertValue insertValue = new ExcuteWinform.InsertValue();
            //insertValue.StartPosition = FormStartPosition.CenterScreen;
            //insertValue.Show();
            GISHandler.GISTools.loadForm(insertValue);
        }

        private void print_land_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Print.PageLayoutControl.frmPageLayoutControl frmpage = new Print.PageLayoutControl.frmPageLayoutControl();
            frmpage.Show();
        }

        private void land_new_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            taskControl.setTask("土地适宜性评价");
        }

        private void risk_new_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            taskControl.setTask("土地灾害风险评价");
        }

        private void ecology_new_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            taskControl.setTask("土地生态功能评价");
        }

        private void final_new_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            taskControl.setTask("空间拓展综合评价");
        }
       
    }
}
