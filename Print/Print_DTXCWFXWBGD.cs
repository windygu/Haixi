using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Print
{

   public  class Print_DTXCWFXWBGD
    {
        FontDialog f = new FontDialog();
        ColorDialog color = new ColorDialog();

        private string filename = "Untitled";
        //打印文檔
        PrintDocument m_PrintDocument = new PrintDocument();

        //打印格式設置頁面
        PageSetupDialog m_PageSetupDialog = new PageSetupDialog();

        //打印頁面
        PrintDialog m_PrintDialog = new PrintDialog();

        //1、實例化打印預覽
        PrintPreviewDialog m_PrintDialogPreview = new PrintPreviewDialog();


        public  string[] m_PrintTextLines;
        private int m_intLinesPrinted;

        private int m_intFontSize_;
        public int m_intFontSize
        {
            set
            {
                m_intFontSize_ = value;
            }
            get
            {
                if (m_intFontSize_ == 0)
                {
                    return 55;
                }
                    else 
                    {
                        return m_intFontSize_;
                    }
            }
        }
        public string m_strFontName;
        public int m_intLineSpace;

        public string m_strTitle;
        public int m_intTitleFontSize;
        public string m_strTitleFontName;
        public int m_intTitleLineSpace;

        public string m_strXCDW;
        public string m_strSJ;
        public string m_strWFZT;
        public string m_strXMMC;
        public string m_strYDWZ;
        public string m_strYT;
        public string m_strWFXWLX;
        public string m_strMJ;
        public string m_strSPHSGJZQK;
        public string m_strXCRYJ;
        public string m_strDYZRRYJ;


   



        public Print_DTXCWFXWBGD()
        {
            m_intFontSize = 0;
            m_strFontName = "";

            m_PrintDocument.PrintPage += new PrintPageEventHandler(OnPrintPage);
            m_PrintDocument.BeginPrint += new PrintEventHandler(PrintDocument_BeginPrint);
            m_PrintDocument.EndPrint += new PrintEventHandler(PrintDocument_EndPrint);

            //頁面設置的打印文檔設置為需要打印的文檔
            m_PageSetupDialog.Document = m_PrintDocument;

            //打印界面的打印文檔設置為被打印文檔
            m_PrintDialog.Document = m_PrintDocument;

            //2、打印預覽的打印文檔設置為被打印文檔
            m_PrintDialogPreview.Document = m_PrintDocument;
        }
        /// <summary>
        /// 打印預覽按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFilePrintPreview()
        {
            //3、顯示打印預覽界面
            m_PrintDialogPreview.ShowDialog();
        }

        /// <summary>
        /// 頁面設置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFilePageSetup()
        {
            m_PageSetupDialog.ShowDialog();
        }

        /// <summary>
        /// 當按下打印時，此為界面中的打印按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFilePrint()
        {
            try
            {
                ////判斷是否有選擇文本
                //if (textBoxEdit.SelectedText != "")
                //{
                //    //如果有選擇文本，則可以選擇"打印選擇的範圍"
                //    m_PrintDialog.AllowSelection = true;
                //}
                //else
                //{
                //    m_PrintDialog.AllowSelection = false;
                //}
                //呼叫打印界面
                if (m_PrintDialog.ShowDialog() == DialogResult.OK)
                {

                    /*
                     * PrintDocument對象的Print()方法在PrintController類的幫助下，執行PrintPage事件。
                     */
                    m_PrintDocument.Print();
                }
            }
            catch (InvalidPrinterException ex)
            {
                MessageBox.Show(ex.Message, "Simple Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// 每個打印任務衹調用OnBeginPrint()一次。
        /// 所有要打印的內容都在此設置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        { 
            char[] param = { '\n' };
            //lines = textBoxEdit.Text.Split(param);
            //判斷是否選取 列印被選擇的範圍
            //if (dlgPrint.PrinterSettings.PrintRange == PrintRange.Selection)
            //{
            //    lines = textBoxEdit.SelectedText.Split(param);
            //}
            //else
            //{
            //    lines = textBoxEdit.Text.Split(param);
            //}

            int i = 0;
            char[] trimParam = { '\r' };
            //foreach (string s in lines)
            //{
            //    lines[i++] = s.TrimEnd(trimParam);
            //}
        }

       


        /// <summary>
        /// printDocument的PrintPage事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      public    void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            //string objString = this.textBoxEdit.Text;
            //获取用于绘制页面的Graphics的对象 
            Graphics m_Graphics = e.Graphics;
            ////向页面输出一行文字 
            ////g.DrawString(objString, f.Font, new SolidBrush(color.Color), new RectangleF(0, 200, textBoxEdit.Width, textBoxEdit.Height));
            //// g.DrawString(objString, f.Font, new SolidBrush(f.Color), new PointF(30, 30));
            ////画一个矩形框
            //g.DrawRectangle(new Pen(color.Color, 2), 200, 200, 200, 200);
            //g.DrawLine(new Pen(color.Color, 2), 200, 600, 600, 600);
            ////g.DrawString("falfdallfjfdalfdllfdjads", f.Font, new SolidBrush(color.Color), new RectangleF(210, 600, textBoxEdit.Width, textBoxEdit.Height));

            ////打印一张图
            //Image temp = Image.FromFile(@"C:\WINDOWS\Zapotec.bmp");
            //Rectangle destRect = new Rectangle(450, 300, temp.Width, temp.Height);

            //g.DrawImage(temp, destRect, 0, 0, temp.Width, temp.Height, System.Drawing.GraphicsUnit.Pixel);

            /*
             * 得到TextBox中每行的字符串數組
             * \n換行
             * \r回車
             */

            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;

            int m_intX1, m_intY1, m_intX2, m_intY2, m_intX3, m_intX4, m_intX5, m_intX6, m_intX7,m_intY7;

          int m_intTemp=y ;
          string m_strTempText="";//标记是否需要换行
          int m_intTextWidth ;

            m_intX1 = x;
            m_intY1 = y;
           m_intX7 = e.PageBounds.Width - (e.PageBounds.Width - e.MarginBounds.Right);
            m_intY7 = e.PageBounds.Height - (e.PageBounds.Height - e.MarginBounds.Bottom);

          //打印报表抬头
            if (m_strTitle.Trim() != "")
            {
                //计算字符串的长度
                int m_intSize =(int) MeasureString(m_strTitle, m_strTitleFontName, m_intTitleFontSize,m_Graphics);
                int m_intStart = (m_intX1 + m_intX7) / 2 - m_intSize / 2;
                PrintText(m_strTitle, m_strTitleFontName, m_intTitleFontSize, m_intStart, m_intY1, m_Graphics);
                y = setY(y, m_intTitleLineSpace);
               
            }

            ////画表格
            //m_intX1 = x;
            //m_intY1 = y - m_intLineSpace/2+10;
            //m_intX2 = e.PageBounds.Width - (e.PageBounds.Width - e.MarginBounds.Right);
            //m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY1, m_intX2, m_intY1);   


            PrintText("巡查单位： " + m_strXCDW, m_strFontName, m_intFontSize, x, y, m_Graphics);
            PrintText("时间： " + m_strSJ, m_strFontName, m_intFontSize, x + 400, y, m_Graphics);
            //画表格横线
          
            m_intY1 = y + m_intLineSpace / 2 + 10;           
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY1, m_intX7, m_intY1);    

            y = setY(y, m_intLineSpace);



            PrintText("违法主体 ", m_strFontName, m_intFontSize, m_intX1, y, m_Graphics);

            m_intX2 = m_intX1 + 100;
            m_intX3 = m_intX1 + 300;
            m_intX4 = m_intX1 + 360;
            m_intX5 = m_intX1 + 460;
            m_intX6 = m_intX1 + 560;

            PrintText(m_strWFZT, m_strFontName, m_intFontSize,m_intX2, y, m_Graphics);
            PrintText("用途 ", m_strFontName, m_intFontSize, m_intX3, y, m_Graphics);
            PrintText(m_strYT, m_strFontName, m_intFontSize, m_intX4, y, m_Graphics);
            PrintText("面积（亩） ", m_strFontName, m_intFontSize,m_intX5, y, m_Graphics);
            PrintText(m_strMJ, m_strFontName, m_intFontSize, m_intX6, y, m_Graphics);
            //画表格横线
            m_intY2 = y + m_intLineSpace / 2 + 10;
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY2, m_intX7, m_intY2);
            //画表格竖线
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX6, m_intY1, m_intX6, m_intY2);
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX4, m_intY1, m_intX4, m_intY2);

            y = setY(y, m_intLineSpace);

            PrintText("项目名称 ", m_strFontName, m_intFontSize, x , y, m_Graphics);
            PrintText(m_strXMMC, m_strFontName, m_intFontSize,m_intX2, y, m_Graphics);
            //画表格横线
            m_intY2 = y + m_intLineSpace / 2 + 10;
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY2, m_intX3, m_intY2);

           m_intTemp=y;
            y = setY(y, m_intLineSpace);

            PrintText("用地位置 ", m_strFontName, m_intFontSize, x , y, m_Graphics);
            PrintText(m_strYDWZ, m_strFontName, m_intFontSize,m_intX2, y, m_Graphics);
            PrintText("违法行为类型 ", m_strFontName, m_intFontSize, m_intX3, (m_intTemp + y) / 2, m_Graphics);
            PrintText(m_strWFXWLX, m_strFontName, m_intFontSize, m_intX5, (m_intTemp + y) / 2, m_Graphics);
            //画表格横线
            m_intY2 = y + m_intLineSpace / 2 + 10;
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY2, m_intX7, m_intY2);
            //画表格竖线
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX3, m_intY1, m_intX3, m_intY2);
           
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX5, m_intY1, m_intX5, m_intY2);

            y = setY(y, m_intLineSpace);

            PrintText("审批和施工 ", m_strFontName, m_intFontSize, x , y, m_Graphics);
            m_intTextWidth =(int) MeasureString(m_strSPHSGJZQK, m_strFontName, m_intFontSize, m_Graphics);
            if (m_intTextWidth <= (m_intX7 - m_intX2))
            {
                PrintText(m_strSPHSGJZQK, m_strFontName, m_intFontSize, m_intX2, y + m_intLineSpace / 2, m_Graphics);
            }
            else
            {
                m_strTempText = GetSubString(m_strSPHSGJZQK, m_strFontName, m_intFontSize,(m_intX7 - m_intX2), m_Graphics);
                PrintText(m_strTempText, m_strFontName, m_intFontSize, m_intX2, y, m_Graphics);
            }
            y = setY(y, m_intLineSpace);
            PrintText("进展情况 ", m_strFontName, m_intFontSize, x, y, m_Graphics);
            if (m_strTempText != "")
            {
                m_strTempText = m_strSPHSGJZQK.Substring(m_strTempText.Length -1);
                PrintText(m_strTempText, m_strFontName, m_intFontSize, m_intX2, y, m_Graphics);//没有处理超过两行的
            }
            m_strTempText = "";

            //画表格横线
            m_intY2 = y + m_intLineSpace / 2 + 10;
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY2, m_intX7, m_intY2);
            y = setY(y, m_intLineSpace);

            PrintText("巡查负责人 ", m_strFontName, m_intFontSize, x, y, m_Graphics);
            m_intTextWidth = (int)MeasureString(m_strXCRYJ, m_strFontName, m_intFontSize, m_Graphics);
            if (m_intTextWidth <= (m_intX7 - m_intX2))
            {
                PrintText(m_strXCRYJ, m_strFontName, m_intFontSize, m_intX2, y + m_intLineSpace / 2, m_Graphics);
            }
            else
            {
                m_strTempText = GetSubString(m_strXCRYJ, m_strFontName, m_intFontSize, (m_intX7 - m_intX2), m_Graphics);
                PrintText(m_strTempText, m_strFontName, m_intFontSize, m_intX2, y, m_Graphics);
            }

            //PrintText(m_strXCRYJ, m_strFontName, m_intFontSize, m_intX2, y, m_Graphics);//换行没有处理

            y = setY(y, m_intLineSpace);

            PrintText("意见 ", m_strFontName, m_intFontSize, x, y, m_Graphics);
            if (m_strTempText != "")
            {
                m_strTempText = m_strXCRYJ.Substring(m_strTempText.Length - 1);
                PrintText(m_strTempText, m_strFontName, m_intFontSize, m_intX2, y, m_Graphics);//没有处理超过两行的
            }
            m_strTempText = "";
            //画表格横线
            m_intY2 = y + m_intLineSpace / 2 + 10;
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY2, m_intX7, m_intY2);
            y = setY(y, m_intLineSpace);

            PrintText("第一责任人 ", m_strFontName, m_intFontSize, x,y, m_Graphics);
            m_intTextWidth = (int)MeasureString(m_strDYZRRYJ, m_strFontName, m_intFontSize, m_Graphics);
            if (m_intTextWidth <= (m_intX7 - m_intX2))
            {
                PrintText(m_strDYZRRYJ, m_strFontName, m_intFontSize, m_intX2, y + m_intLineSpace / 2, m_Graphics);
            }
            else
            {
                m_strTempText = GetSubString(m_strDYZRRYJ, m_strFontName, m_intFontSize, (m_intX7 - m_intX2), m_Graphics);
                PrintText(m_strTempText, m_strFontName, m_intFontSize, m_intX2, y, m_Graphics);
            }
            //PrintText(m_strDYZRRYJ, m_strFontName, m_intFontSize, m_intX2, y, m_Graphics);//换行没有处理

            y = setY(y, m_intLineSpace);

            PrintText("意见 ", m_strFontName, m_intFontSize, x, y, m_Graphics);
            if (m_strTempText != "")
            {
                m_strTempText = m_strDYZRRYJ.Substring(m_strTempText.Length - 1);
                PrintText(m_strTempText, m_strFontName, m_intFontSize, m_intX2, y, m_Graphics);//没有处理超过两行的
            }
            m_strTempText = "";
            //画表格横线
            m_intY2 = y + m_intLineSpace / 2 + 10;
            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY2, m_intX7, m_intY2);
                       y = setY(y, m_intLineSpace);

                       //画表格横线
                       m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY1, m_intX1, m_intY7);//第一条竖线
                       m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX7, m_intY1, m_intX7, m_intY7);//最后一条竖线

                       m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX2, m_intY1, m_intX2, m_intY2);//第二条竖线

            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX3, m_intY2, m_intX3, m_intY7);//下面中间的竖线

            m_Graphics.DrawLine(new Pen(color.Color, 2), m_intX1, m_intY7, m_intX7, m_intY7);//最后一条横线

           

         


            
           

            m_intLinesPrinted = 0;
            e.HasMorePages = false;

        }

      private int setY(int p_intY,int p_intLineSpace)
       {
           ////if (p_intLineSpace == 0)
           ////{
           ////    p_intY += 55;
           ////}
           ////else
           ////{
           p_intY += p_intLineSpace;
           ////}

           ////判斷超過一頁時，列印其它頁面
           //if (p_intY >= e.PageBounds.Height - 80)
           //{
           //    //多頁打印
           //    e.HasMorePages = true;

           //    /*
           //     * PrintPageEventArgs類的HaeMorePages屬性為True時，通知控件器，必須再次調用OnPrintPage()方法，打印一個頁面。
           //     * PrintLoopI()有一個用於每個要打印的頁面的序例。如果HasMorePages是False，PrintLoop()就會停止。
           //     */
           //    //return p_intY;
           //}
           return p_intY;
       }

        /// <summary>
        /// EndPrint事件釋放BeginPrint方法中佔用的資源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      public void PrintDocument_EndPrint(object sender, PrintEventArgs e)
        {
            //變量Lines占用和引用的字符串數組，現在釋放
            m_PrintTextLines = null;
        }

       /// <summary>
       /// 测量字符串的长度
       /// </summary>
       /// <param name="p_strPrintText"></param>
       /// <param name="p_strFontName"></param>
       /// <param name="p_strFontSize"></param>
       /// <param name="p_Graphics"></param>
       /// <returns></returns>
      private double  MeasureString(string p_strPrintText, string p_strFontName, int p_strFontSize, Graphics p_Graphics)
      {
          if (p_strFontSize == 0 )
          {
              return 0;
          }
          else 
          {
              if (p_strFontName == "")
              {
                  p_strFontName = "宋体";
              }
           SizeF m_SizeF=  p_Graphics.MeasureString(p_strPrintText, new Font(p_strFontName, p_strFontSize));
           return m_SizeF.Width;
          }
      }

       /// <summary>
       /// 获得站指定长度的子字符串
       /// </summary>
       /// <param name="p_strPrintText"></param>
       /// <param name="p_strFontName"></param>
       /// <param name="p_strFontSize"></param>
       /// <param name="p_Graphics"></param>
       /// <returns></returns>
      private string GetSubString(string p_strText, string p_strFontName, int p_strFontSize,int p_intLength, Graphics p_Graphics)
      {
          string m_strSubString = "";

          for (int i = 0; i < p_strText.Length; i++)
          {
              m_strSubString = p_strText.Substring(0, i);
              if (MeasureString(m_strSubString, p_strFontName, p_strFontSize, p_Graphics) >= p_intLength)
              {
                  m_strSubString = p_strText.Substring(0, i-1);
                  return m_strSubString;
              }
          }

              return m_strSubString;
      }
      private void PrintText(string p_strPrintText, string p_strFontName, int p_strFontSize, int p_intX, int p_intY, Graphics p_Graphics)
      {
          if (p_strFontSize == 0 && p_strFontName == "")
          {
              p_Graphics.DrawString(p_strPrintText, new Font("宋体", 10), Brushes.Black, p_intX, p_intY);
          }
          else if (p_strFontName != "" && p_strFontSize == 0)
          {
              p_Graphics.DrawString(p_strPrintText, new Font(p_strFontName, 10), Brushes.Black, p_intX, p_intY);
          }
          else if (p_strFontSize > 0 && p_strFontName == "")
          {
              p_Graphics.DrawString(p_strPrintText, new Font("宋体", p_strFontSize), Brushes.Black, p_intX, p_intY);
          }
          else if (p_strFontSize > 0 && p_strFontName != "")
          {
              p_Graphics.DrawString(p_strPrintText, new Font(p_strFontName, p_strFontSize), Brushes.Black, p_intX, p_intY);
          }

         
      }
    }
}
