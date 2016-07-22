using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Print
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        PrintDocument objDocument;
        DialogResult resultP;
        FontDialog f = new FontDialog();
        ColorDialog color = new ColorDialog();

        private void button1_Click(object sender, EventArgs e)
        {
            //初始化打印机设置窗口
            PrintDialog priD = new PrintDialog();
            //设置当前页按钮为真
            priD.AllowCurrentPage = true;
            //是否显示帮助按钮
            priD.ShowHelp = true;
            //是否启用打印到文件按钮
            priD.AllowPrintToFile = true;
            //是否启用选择按钮
            priD.AllowSelection = true;
            //是否启用页选项按钮
            priD.AllowSomePages = true;
            resultP = priD.ShowDialog();
            if (resultP == DialogResult.OK)
            {
                object[] obj = new object[]
                {
                    //获取打印机是否支持双面打印
                    priD.PrinterSettings.CanDuplex,
                    //打印文档是否逐份打印
                    priD.PrinterSettings.Collate,
                    //获取打印机允许用户打印的最大份数
                    priD.PrinterSettings.MaximumCopies,
                    //获取打印文档的份数
                    priD.PrinterSettings.Copies,
                    //获取此打印机默认页设置
                    priD.PrinterSettings.DefaultPageSettings,
                    //获取双面打印的打印机设置
                    priD.PrinterSettings.Duplex,
                    //获取打印第一页的页码
                    priD.PrinterSettings.FromPage,
                    //获取该打印机支持的纸张大小
                    priD.PrinterSettings.PaperSizes,
                    //获取用户已指定要打印的页码
                    priD.PrinterSettings.PrintRange,
                    //获取要打印最后一页的页码
                    priD.PrinterSettings.ToPage
                };
                listBox1.Items.AddRange(obj);
            }

            //初始化要显示的设置窗口 
            PageSetupDialog objPageSetupDialog = new PageSetupDialog();
            objPageSetupDialog.PageSettings = new PageSettings();
            objPageSetupDialog.ShowNetwork = false;
            //获取文档 
            objPageSetupDialog.Document = this.objDocument;
            //显示窗口 
            //objPageSetupDialog.ShowDialog();
            resultP = objPageSetupDialog.ShowDialog();
            if (resultP == DialogResult.OK)
            {
                object[] resultsP = new object[]{ 
                    //获取该页的边距
                    objPageSetupDialog.PageSettings.Margins, 
                    //获取该页纸张的大小
                    objPageSetupDialog.PageSettings.PaperSize, 
                    //获取是横向还是纵向打印True为横向
                    objPageSetupDialog.PageSettings.Landscape, 
                };
                listBox1.Items.AddRange(resultsP);
            }

            //初始化要预览窗口 
            PrintPreviewDialog ppvw = new PrintPreviewDialog();
            //获取要预览的文档 
            ppvw.Document = objDocument;
            ppvw.Width = 800;
            ppvw.Height = 600;
            //显示预览窗口 
            ppvw.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            objDocument = new PrintDocument();
            //当需要为当前页打印的输出时发生 
            objDocument.PrintPage += new PrintPageEventHandler(objDocument_PrintPage);
        }

        void objDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            string objString = this.textBox1.Text;
            //获取用于绘制页面的Graphics的对象 
            Graphics g = e.Graphics;
            //向页面输出一行文字 
            g.DrawString(objString, f.Font, new SolidBrush(color.Color), new RectangleF(0, 200, textBox1.Width, textBox1.Height));
            g.DrawString(objString, f.Font, new SolidBrush(f.Color), new PointF(30,30));
            //画一个矩形框
            g.DrawRectangle(new Pen(color.Color,2),200,200,200,200);
            //打印一张图
            Image temp = Image.FromFile(@"D:\网络.png");
            Rectangle destRect = new Rectangle(450, 300, temp.Width,temp.Height);
            g.DrawImage(temp, destRect, 0, 0, temp.Width, temp.Height, System.Drawing.GraphicsUnit.Pixel);
        }

        private void btncolor_Click(object sender, EventArgs e)
        {
            resultP = color.ShowDialog();
            if (resultP == DialogResult.OK)
            {
                object[] obj1 = new object[]
                {
                    color.Color
                };
                listBox1.Items.AddRange(obj1);
            }
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            //对话框中是否显示颜色选择
            f.ShowColor = true;
            resultP = f.ShowDialog();
            if (resultP == DialogResult.OK)
            {
                object[] obj2 = new object[]
                {
                    f.Font,
                    f.ShowColor
                };
                listBox1.Items.AddRange(obj2);
            }
        }

       
    }
}

