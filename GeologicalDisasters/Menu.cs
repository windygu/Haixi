using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesFile;


using System.ComponentModel;
using System.Data;
using System.Drawing;

using ESRI.ArcGIS.Display;


using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Output;

using ESRI.ArcGIS.Geometry;


namespace _sdnMap
{
    /// <summary>

    /// 删除图层

    /// </summary>
    public sealed class RemoveLayer : BaseCommand
    {

        private IMapControl3 m_mapControl;
        public RemoveLayer()
        {

            base.m_caption = "删除图层";

        }



        public override void OnClick()
        {

            ILayer layer = (ILayer)m_mapControl.CustomProperty;

            m_mapControl.Map.DeleteLayer(layer);

        }



        public override void OnCreate(object hook)
        {

            m_mapControl = (IMapControl3)hook;

        }
    }
    /// <summary>

    /// 放大至整个图层

    /// </summary>

    public sealed class ZoomToLayer : BaseCommand
    {

        private IMapControl3 m_mapControl;



        public ZoomToLayer()
        {

            base.m_caption = "图层全显";

        }



        public override void OnClick()
        {

            ILayer layer = (ILayer)m_mapControl.CustomProperty;

            m_mapControl.Extent = layer.AreaOfInterest;

        }



        public override void OnCreate(object hook)
        {

            m_mapControl = (IMapControl3)hook;

        }

    }
    //移动
    public sealed class pan : BaseCommand
    {

        private IMapControl3 m_mapControl;



        public pan()
        {

            base.m_caption = "移动图层";

        }



        public override void OnClick()
        {

            ILayer layer = (ILayer)m_mapControl.CustomProperty;

            //m_mapControl.Extent = layer.AreaOfInterest;
            ICommand pCommand;
            pCommand = new ControlsMapPanToolClass();
            pCommand.OnCreate(m_mapControl.Object);
            m_mapControl.CurrentTool = (ITool)pCommand;

        }



        public override void OnCreate(object hook)
        {

            m_mapControl = (IMapControl3)hook;

        }

    }
    //添加图层
    public sealed class addData : BaseCommand
    {

        private IMapControl3 m_mapControl;



        public addData()
        {

            base.m_caption = "添加图层";
            

        }



        public override void OnClick()
        {

            ILayer layer = (ILayer)m_mapControl.CustomProperty;

            //m_mapControl.Extent = layer.AreaOfInterest;
            ICommand pCommand;
            pCommand = new ControlsAddDataCommandClass();
            pCommand.OnCreate(m_mapControl.Object);
            pCommand.OnClick();

        }



        public override void OnCreate(object hook)
        {

            m_mapControl = (IMapControl3)hook;

        }

    }

    //刷新图层
    public sealed class refresh : BaseCommand
    {

        private IMapControl3 m_mapControl;



        public refresh()
        {

            base.m_caption = "刷新图层";

        }



        public override void OnClick()
        {
            ILayer layer = (ILayer)m_mapControl.CustomProperty;
            //AxMapControl axmap = (AxMapControl)m_mapControl;
          IGraphicsContainer pDeleteElements = m_mapControl.ActiveView.FocusMap as IGraphicsContainer;
            pDeleteElements.DeleteAllElements();
           m_mapControl.ActiveView.Refresh();
           // GISHandler.GISTools.refresh(axmap);
           m_mapControl.CurrentTool = null;
           m_mapControl.ActiveView.FocusMap.ClearSelection();
          
        }
        public override void OnCreate(object hook)
        {

            m_mapControl = (IMapControl3)hook;

        }
    }
    //恢复全显
    public sealed class fullExtent : BaseCommand
    {

        private IMapControl3 m_mapControl;



        public fullExtent()
        {

            base.m_caption = "全局显示";

        }



        public override void OnClick()
        {
            ILayer layer = (ILayer)m_mapControl.CustomProperty;
            ICommand pCommand;
            pCommand = new ControlsMapFullExtentCommandClass();
            pCommand.OnCreate(m_mapControl.Object);
            pCommand.OnClick();
        }
        public override void OnCreate(object hook)
        {

            m_mapControl = (IMapControl3)hook;

        }
    }
    //添加图名
    public sealed class AddName : BaseCommand
    {

        private AxPageLayoutControl pagelayercontrol;



        public AddName(AxPageLayoutControl app)
        {
            this.pagelayercontrol = app;
            base.m_caption = "添加图名";

        }



        public override void OnClick()
        {

            UID uid = new UIDClass();
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";// 代表只获取矢量图层
            //问题在这个地方 解决！
            IEnumLayer layers = pagelayercontrol.ActiveView.FocusMap.get_Layers(uid, true);
            layers.Reset();
            ILayer layer=layers.Next();
            GISHandler.GISTools.AddTextElement(pagelayercontrol, 10, 25, layer.Name);
            
        }
        public override void OnCreate(object hook)
        {

            pagelayercontrol = (AxPageLayoutControl)hook;

        }
    }
    //添加图例
    public sealed class AddLegend : BaseCommand
    {

        private IPageLayoutControl3 p_PageLayoutControl;


        public AddLegend()
        {

            base.m_caption = "添加图例";

        }



        public override void OnClick()
        {
           
            
        }
        public override void OnCreate(object hook)
        {

            p_PageLayoutControl = (IPageLayoutControl3)hook;

        }
    }
    //添加指北针
    public sealed class AddNorthArrow : BaseCommand
    {


       private AxPageLayoutControl pagelayercontrol;

       public AddNorthArrow(AxPageLayoutControl pagelayercontrol)
       {
           this.pagelayercontrol = pagelayercontrol;
           base.m_caption = "添加指北针";
       }

       
      
       public override void OnClick()
       {
           GISHandler.GISTools.AddNorthArrow(pagelayercontrol, pagelayercontrol.ActiveView.FocusMap);
       }
       public override void OnCreate(object hook)
       {
           pagelayercontrol = (AxPageLayoutControl)hook;

       }

    }

    //添加比例尺
    public sealed class AddScal : BaseCommand
    {

        
        private AxPageLayoutControl pagelayercontrol;
     
        
        public AddScal(AxPageLayoutControl pagelayercontrol)
        {
            this.pagelayercontrol = pagelayercontrol;
            base.m_caption = "添加比例尺";

        }



        public override void OnClick()
        {
            GISHandler.GISTools.AddScalebar(pagelayercontrol, pagelayercontrol.ActiveView.FocusMap);
            
        }
        public override void OnCreate(object hook)
        {

            pagelayercontrol = (AxPageLayoutControl)hook;

        }
    }
   
   
#region  图层输出要素 比例尺 指北针
    /*class addPageLayoutName : BaseTool
    {
        //public  formTemp;
        TextBox textbox;
        AxPageLayoutControl axLayoutControl;
        IPoint pPoint;
        //double xMap, yMap;
        public static double xMap;
        public static double yMap;

        public override void OnMouseDown(int Button, int Shift, int X, int Y,AxPageLayoutControl axPageLayoutControl)
        {
            if (Button == 1)
            {
                pPoint = axPageLayoutControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                xMap = pPoint.X;
                yMap = pPoint.Y;
                formTemp.returnTextbox1().Location = new System.Drawing.Point(X, Y);
                formTemp.returnTextbox1().Visible = true;
                formTemp.returnTextbox1().Focus();
                formTemp.returnTextbox1().Text = "请在此输入图名";


            }

        }


        public override void OnCreate(object hook)
        {
            axLayoutControl = hook as AxPageLayoutControl;

        }

        public void AddTextElement(AxPageLayoutControl PageLayoutControl, double x, double y, string textName)
        {
            IPageLayout pPageLayout;
            IActiveView pAV;
            IGraphicsContainer pGraphicsContainer;
            IPoint pPoint;
            ITextElement pTextElement;
            IElement pElement;
            ITextSymbol pTextSymbol;
            IRgbColor pColor;
            pPageLayout = PageLayoutControl.PageLayout;
            pAV = (IActiveView)pPageLayout;
            pGraphicsContainer = (IGraphicsContainer)pPageLayout;
            pTextElement = new TextElementClass();

            IFontDisp pFont = new StdFontClass() as IFontDisp;
            pFont.Bold = true;
            pFont.Name = "宋体";
            pFont.Size = 13;

            pColor = new RgbColorClass();
            pColor.Red = 255;

            pTextSymbol = new TextSymbolClass();
            pTextSymbol.Color = (IColor)pColor;
            pTextSymbol.Font = pFont;

            pTextElement.Text = textName;
            pTextElement.Symbol = pTextSymbol;

            pPoint = new PointClass();
            pPoint.X = x;
            pPoint.Y = y;

            pElement = (IElement)pTextElement;
            pElement.Geometry = (IGeometry)pPoint;
            pGraphicsContainer.AddElement(pElement, 0);

            pAV.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        }

    }
    sealed class addNorthArrow : BaseTool
    {
        AxPageLayoutControl axPageLayout = null;
        IPoint pPoint;
        bool bInuse;
        INewEnvelopeFeedback pNewEnvelopeFeedback = null;

        public addNorthArrow()
        {
            base.m_caption = "添加指北针";
            base.m_toolTip = "添加指北针";
            base.m_category = "customCommands";
            base.m_message = "添加指北针";
            base.m_deactivate = true;

        }

        public override void OnCreate(object hook)
        {
            axPageLayout = (AxPageLayoutControl)hook;

        }
        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            pPoint = axPageLayout.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            bInuse = true;
        }
        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            if (bInuse == false)
            {
                return;
            }

            if (pNewEnvelopeFeedback == null)
            {
                pNewEnvelopeFeedback = new NewEnvelopeFeedbackClass();
                pNewEnvelopeFeedback.Display = axPageLayout.ActiveView.ScreenDisplay;
                pNewEnvelopeFeedback.Start(pPoint);
            }
            pNewEnvelopeFeedback.MoveTo(axPageLayout.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y));


        }
        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (bInuse == false)
            {
                return;
            }
            if (pNewEnvelopeFeedback == null)
            {
                pNewEnvelopeFeedback = null;
                bInuse = false;
                return;
            }
            IEnvelope pEnvelope = pNewEnvelopeFeedback.Stop();
            if ((pEnvelope.IsEmpty) || (pEnvelope.Width == 0) || (pEnvelope.Height == 0))
            {
                pNewEnvelopeFeedback = null;
                bInuse = false;
                return;
            }

            addNorthArrowForm northArrow = new addNorthArrowForm();
            IStyleGalleryItem pStyleGalleryItemTemp = Form1.pStyleGalleryItem;
            if (pStyleGalleryItemTemp == null)
            {
                return;
            }

            IMapFrame pMapframe = axPageLayout.ActiveView.GraphicsContainer.FindFrame(axPageLayout.ActiveView.FocusMap) as IMapFrame;
            IMapSurroundFrame pMapSurroundFrame = new MapSurroundFrameClass();
            pMapSurroundFrame.MapFrame = pMapframe;
            pMapSurroundFrame.MapSurround = (IMapSurround)pStyleGalleryItemTemp.Item;
            //在pageLayout中根据名称查要Element，找到之后删除已经存在的指北针
            IElement pElement = axPageLayout.FindElementByName("NorthArrows");
            if (pElement != null)
            {
                axPageLayout.ActiveView.GraphicsContainer.DeleteElement(pElement);  //删除已经存在的指北针
            }


            pElement = (IElement)pMapSurroundFrame;
            pElement.Geometry = (IGeometry)pEnvelope;

            axPageLayout.ActiveView.GraphicsContainer.AddElement(pElement, 0);
            axPageLayout.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            pNewEnvelopeFeedback = null;
            bInuse = false;
        }


    }*/
#endregion

}


namespace topo.com
{
/// <summary>
/// 导出缓冲分析之后出现JointCount>1的点位图层
/// </summary>
public  class ExportToShp
{
public string shpPath;
/// <summary>
/// 保存输出JointCount>1的点位图层
/// </summary>
/// <param name="apFeatureClass"></param>
public void ExportFeatureClassToShp(IFeatureClass apFeatureClass)
{
if (apFeatureClass == null)
{
MessageBox.Show("请选择", "系统提示");
return;
}
//调用保存文件函数
SaveFileDialog sa = new SaveFileDialog();
sa.Filter = "SHP文件(.shp)|*.shp";
sa.ShowDialog();
sa.CreatePrompt = true;
string ExportShapeFileName = sa.FileName;
// string StrFilter = "SHP文件(.shp)|*.shp";
// string ExportShapeFileName = SaveFileDialog(StrFilter);
if (ExportShapeFileName == "")
return;
string ExportFileShortName = System.IO.Path.GetFileNameWithoutExtension(ExportShapeFileName);
string ExportFilePath = System.IO.Path.GetDirectoryName(ExportShapeFileName);
shpPath = ExportFilePath + "\\" + ExportFileShortName + "\\" + ExportFileShortName + ".shp";
//设置导出要素类的参数
IFeatureClassName pOutFeatureClassName = new FeatureClassNameClass();
IDataset pOutDataset = (IDataset)apFeatureClass;
pOutFeatureClassName = (IFeatureClassName)pOutDataset.FullName;
//创建一个输出shp文件的工作空间
IWorkspaceFactory pShpWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
IWorkspaceName pInWorkspaceName = new WorkspaceNameClass();
pInWorkspaceName = pShpWorkspaceFactory.Create(ExportFilePath, ExportFileShortName, null, 0);
//创建一个要素集合
IFeatureDatasetName pInFeatureDatasetName = null;
//创建一个要素类
IFeatureClassName pInFeatureClassName = new FeatureClassNameClass();
IDatasetName pInDatasetClassName;
pInDatasetClassName = (IDatasetName)pInFeatureClassName;
pInDatasetClassName.Name = ExportFileShortName;//作为输出参数
pInDatasetClassName.WorkspaceName = pInWorkspaceName;
//通过FIELDCHECKER检查字段的合法性，为输出SHP获得字段集合
long iCounter;
IFields pOutFields, pInFields;
IFieldChecker pFieldChecker;
IField pGeoField;
IEnumFieldError pEnumFieldError = null;
pInFields = apFeatureClass.Fields;
pFieldChecker = new FieldChecker();
pFieldChecker.Validate(pInFields, out pEnumFieldError, out pOutFields);
//通过循环查找几何字段
pGeoField = null;
for (iCounter = 0; iCounter < pOutFields.FieldCount; iCounter++)
{
if (pOutFields.get_Field((int)iCounter).Type == esriFieldType.esriFieldTypeGeometry)
{
pGeoField = pOutFields.get_Field((int)iCounter);
break;
}
}
//得到几何字段的几何定义
IGeometryDef pOutGeometryDef;
IGeometryDefEdit pOutGeometryDefEdit;
pOutGeometryDef = pGeoField.GeometryDef;
//设置几何字段的空间参考和网格
pOutGeometryDefEdit = (IGeometryDefEdit)pOutGeometryDef;
pOutGeometryDefEdit.GridCount_2 = 1;
pOutGeometryDefEdit.set_GridSize(0, 1500000);
try
{
//开始导入
IFeatureDataConverter pShpToClsConverter = new FeatureDataConverterClass();
pShpToClsConverter.ConvertFeatureClass(pOutFeatureClassName, null, pInFeatureDatasetName, pInFeatureClassName, pOutGeometryDef, pOutFields, "", 1000, 0);
MessageBox.Show("导出成功", "系统提示");
}
catch (Exception ex)
{
MessageBox.Show("the following exception occurred:" + ex.ToString());
}
}
}
}
