using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace JCZF.SubFrame
{
    class LabelUserDef
    {
        //private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        //{

        //    if (i >= 3)//测试目的
        //        return;
        //    i++;
        //    ITextElement te = createTextElement(e.mapX, e.mapY, "魁x");
        //    axMapControl1.ActiveView.GraphicsContainer.AddElement(te as IElement, 1);
        //    axMapControl1.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);

        //}
        //int i;
        public IBalloonCallout createBalloonCallout(double x, double y)
        {
            IRgbColor rgb = new RgbColor() as IRgbColor;
            {
                rgb.Red = 202;
                rgb.Green = 202;
                rgb.Blue = 202;
            }
            ISimpleFillSymbol sfs = new SimpleFillSymbol() as ISimpleFillSymbol;
            {
                sfs.Color = rgb;
                sfs.Style = esriSimpleFillStyle.esriSFSSolid;
            }
            IPoint p = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
            {
                p.PutCoords(x, y);
            }
            IBalloonCallout bc = new BalloonCallout() as IBalloonCallout;
            {
                bc.Style = esriBalloonCalloutStyle.esriBCSRoundedRectangle;
                //
                bc.Symbol = sfs;
                //
                bc.LeaderTolerance = 10;
                //1
                bc.AnchorPoint = p;
            }
            return bc;
        }
        public ITextElement createTextElement(IPoint point, string text)
        {
            IBalloonCallout bc = createBalloonCallout(point.X, point.Y);

            IRgbColor rgb = new RgbColor() as IRgbColor;
            {
                rgb.Red = 255;
                //rgb.Green = 255;
            }
            ITextSymbol ts = new TextSymbol() as ITextSymbol;
            {
                ts.Color = rgb;


                ts.Font.Name = "Courier New";


                ts.Size = 24;

            }
            IFormattedTextSymbol fts = ts as IFormattedTextSymbol;
            {
                fts.Background = bc as ITextBackground;
            }
            //fts.Size = 8;
            ts.Size = 8;
            //IPoint point = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
            //{
            //    double width = axMapControl1.Extent.Width / 13;
            //    double height = axMapControl1.Extent.Height / 20;
            //    point.PutCoords(x + width, y + height);
            //}
            ITextElement te = new TextElement() as ITextElement ;
            //IMarkerElement me = new MarkerElement() as  IMarkerElement;
            {
                te.Symbol = ts;
                //ts.Text = text;
                te.Text = text;
            }
            IElement e = te as IElement;
            {
                e.Geometry = point;
            }
            return te;
        }
    }
}
