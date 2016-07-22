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
    public partial class Form2 : Form
    {
        

        FontDialog f = new FontDialog();
        ColorDialog color = new ColorDialog();

        private string filename = "Untitled";
        //打印文檔
        PrintDocument pdDocument = new PrintDocument();

        //打印格式設置頁面
        PageSetupDialog dlgPageSetup = new PageSetupDialog();

        //打印頁面
        PrintDialog dlgPrint = new PrintDialog();

        //1、實例化打印預覽
        PrintPreviewDialog dlgPrintPreview = new PrintPreviewDialog();


        private string[] lines;
        private int linesPrinted;

        public Form2()
        {
            InitializeComponent();
            pdDocument.PrintPage += new PrintPageEventHandler(OnPrintPage);
            pdDocument.BeginPrint += new PrintEventHandler(pdDocument_BeginPrint);
            pdDocument.EndPrint += new PrintEventHandler(pdDocument_EndPrint);

            //頁面設置的打印文檔設置為需要打印的文檔
            dlgPageSetup.Document = pdDocument;

            //打印界面的打印文檔設置為被打印文檔
            dlgPrint.Document = pdDocument;

            //2、打印預覽的打印文檔設置為被打印文檔
            dlgPrintPreview.Document = pdDocument;
        }

        /// <summary>
        /// 打印預覽按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilePrintPreview(object sender, EventArgs e)
        {
            //3、顯示打印預覽界面
            dlgPrintPreview.ShowDialog();
        }

        /// <summary>
        /// 頁面設置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilePageSetup(object sender, EventArgs e)
        {
            dlgPageSetup.ShowDialog();
        }

        private void OnExit(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 當按下打印時，此為界面中的打印按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilePrint(object sender, EventArgs e)
        {
            try
            {
                //判斷是否有選擇文本
                if (textBoxEdit.SelectedText != "")
                {
                    //如果有選擇文本，則可以選擇"打印選擇的範圍"
                    dlgPrint.AllowSelection = true;
                }
                else
                {
                    dlgPrint.AllowSelection = false;
                }
                //呼叫打印界面
                if (dlgPrint.ShowDialog()==DialogResult.OK)
                {

                    /*
                     * PrintDocument對象的Print()方法在PrintController類的幫助下，執行PrintPage事件。
                     */
                    pdDocument.Print();
                }
            }
            catch (InvalidPrinterException ex )
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
        void pdDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            char[] param ={ '\n' };
            //lines = textBoxEdit.Text.Split(param);
            //判斷是否選取 列印被選擇的範圍
            if (dlgPrint.PrinterSettings.PrintRange==PrintRange.Selection)
            {
                lines = textBoxEdit.SelectedText.Split(param);
            }
            else
            {
                lines = textBoxEdit.Text.Split(param);
            }

            int i = 0;
            char[] trimParam ={ '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }
        }




        /// <summary>
        /// printDocument的PrintPage事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            //string objString = this.textBoxEdit.Text;
            //获取用于绘制页面的Graphics的对象 
            Graphics g = e.Graphics;
            //向页面输出一行文字 
            //g.DrawString(objString, f.Font, new SolidBrush(color.Color), new RectangleF(0, 200, textBoxEdit.Width, textBoxEdit.Height));
           // g.DrawString(objString, f.Font, new SolidBrush(f.Color), new PointF(30, 30));
            //画一个矩形框
            g.DrawRectangle(new Pen(color.Color, 2), 200, 200, 200, 200);
            g.DrawLine(new Pen(color.Color, 2), 200, 600, 600, 600);
            g.DrawString("falfdallfjfdalfdllfdjads", f.Font, new SolidBrush(color.Color), new RectangleF(210, 600, textBoxEdit.Width, textBoxEdit.Height));
            g.DrawString("在国土资源法监察管理系统将综合运用GIS、GPS、RS、MIS、数据库、无线通讯技术、信息安全等成熟技术，以全省国土资源执法网格化责任区所各自负责的执法业务为基础，建立国土资源管理领域网格化管理模式，实现国土资源违法现象“发现、调查、处理、信息反馈”全程的信息管理，为执法工作的顺利开展提供技术手段；完善“发现及时、处置快速、解决有效、监督有力”的长效管理机制，提升国土资源执法管理的能力和水平。", f.Font, new SolidBrush(color.Color), new RectangleF(210, 600, textBoxEdit.Width, textBoxEdit.Height));

            //打印一张图
            Image temp = Image.FromFile(@"D:\小胡.png");
            Rectangle destRect = new Rectangle(450, 300, temp.Width, temp.Height);
             
            g.DrawImage(temp, destRect, 0, 0, temp.Width, temp.Height, System.Drawing.GraphicsUnit.Pixel);

            /*
             * 得到TextBox中每行的字符串數組
             * \n換行
             * \r回車
             */

            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            while (linesPrinted<lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++], new Font("Arial", 10), Brushes.Black, x, y);

                y += 55;

                //判斷超過一頁時，列印其它頁面
                if (y >= e.PageBounds.Height - 80)
                {
                    //多頁打印
                    e.HasMorePages = true;

                    /*
                     * PrintPageEventArgs類的HaeMorePages屬性為True時，通知控件器，必須再次調用OnPrintPage()方法，打印一個頁面。
                     * PrintLoopI()有一個用於每個要打印的頁面的序例。如果HasMorePages是False，PrintLoop()就會停止。
                     */
                    return;
                }
            }

            linesPrinted = 0;
            e.HasMorePages = false;

        }

        /// <summary>
        /// EndPrint事件釋放BeginPrint方法中佔用的資源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pdDocument_EndPrint(object sender, PrintEventArgs e)
        {
           //變量Lines占用和引用的字符串數組，現在釋放
            lines = null;
}

        private void button3_Click(object sender, EventArgs e)
        {
           
            string[] m_PrintTextLines=new string[22];
            m_PrintTextLines[0] = "行政区代码：";
            m_PrintTextLines[1] = "县 区 名：";
            m_PrintTextLines[2] = "乡镇名称：";
            m_PrintTextLines[3] = "前 时 相：";
            m_PrintTextLines[4] = "后 时 相：";
            m_PrintTextLines[5] = "X  坐 标：";
            m_PrintTextLines[6] = "Y  坐 标：";
            m_PrintTextLines[7] = "监测编号：";
            m_PrintTextLines[8] = "监测面积：";
            m_PrintTextLines[9] = "备    注：";
            m_PrintTextLines[10] = "地块编号：";
            m_PrintTextLines[11] = "地块分类：";
            m_PrintTextLines[12] = "地块面积：";
            m_PrintTextLines[13] = "耕地面积：";
            m_PrintTextLines[14] = "农用地面积：";
            m_PrintTextLines[15] = "基本农田面积：";
            m_PrintTextLines[16] = "未利用地面积：";
            m_PrintTextLines[17] = "实际用途：";
            m_PrintTextLines[18] = "用地单位：";
            m_PrintTextLines[19] = "项目类型：";
            m_PrintTextLines[20] = "合法性审查：";
            m_PrintTextLines[21] = "违法类型：";
            //m_PrintTextLines[22] = "";
           

            Print_DTXCWFXWBGD m_Print_HC_Attribute = new Print_DTXCWFXWBGD();

            m_Print_HC_Attribute.m_intFontSize = 16;
            m_Print_HC_Attribute.m_intLineSpace =40;
            //m_Print_HC_Attribute.m_strFontName = "";
            m_Print_HC_Attribute.m_PrintTextLines = m_PrintTextLines;

            //m_Print_HC_Attribute.OnFilePrintPreview();
            m_Print_HC_Attribute.OnFilePrint();
       
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

       
    }
}
