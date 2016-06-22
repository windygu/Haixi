using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using JCZF.SubFrame;
namespace JCZF.SubFrame.AttibuteEdit
{
    // liuyang 
    public partial class FormShowSmallPic : SlideDialog.SlideDialog
    {
        public ArrayList m_theFileList = new ArrayList(); // 文件列表
        public string m_type; // 灾害图片类型
        public string m_theTitle; // 窗体标题
        public bool m_bIsVideo; // 视频否


        public FormShowSmallPic(Form poOwner, float pfStep)
            : base(poOwner, pfStep) 
        {
            InitializeComponent();
            m_theTitle = "";
            m_type = "";
            m_bIsVideo = false;
        }


        private void FormShowSmallPic_Load(object sender, EventArgs e)
        {

            
        }



        // 初始化工作-除了视频
        public void DoInitial()
        {
            // 显示窗体标题
            //   this.Text = m_theTitle;
            try
            {
                listView1.Clear(); // 清空listview

                // 添加图片
                int count = m_theFileList.Count;

                if (count == 0) // 没有则返回（不可能没有）
                    return;

                // 添加图片名称
                //string str;
                for (int i = 0; i < count; i++)
                {
                    string filename = System.IO.Path.GetFileNameWithoutExtension((string)m_theFileList[i]);
                    //str = String.Format("{0}", filename);
                    ListViewItem item = new ListViewItem(filename, i);
                    listView1.Items.Add(item);
                }

                m_bIsVideo = false; // 不是视频
            }
            catch (InvalidVideoFileException ex)
            {
                MessageBox.Show(ex.Message, "Extraction failed");
            }

        }

        public void ShowIconImage()
        {
            ImageList imageListLarge = new ImageList();

            // 设置图片大小
            imageListLarge.ImageSize = new System.Drawing.Size(108, 108);
            Image m_ImageTemp;
            // 按照图片路径获取图片
            for (int i = 0; i < m_theFileList.Count; i++)
            {
                string str_tempPath =Application.StartupPath + "\\" + "HCPV";
                string strMC = (string)m_theFileList[i];
                string strLJ = str_tempPath + "\\" + strMC;
            //    if(!File.Exists(strLJ))
            //    {
            //        //strLJ = strLJ + ".JPG";
            //        if (!File.Exists(strLJ))
            //        {
            //            continue;
            //        }
            //}
                //FileStream  m_FileStream = File.OpenRead(@strLJ);
                
                //m_ImageTemp = Image.FromStream(m_FileStream);
                //m_FileStream.Close();
                //imageListLarge.Images.Add(m_ImageTemp);
                if (File.Exists(strLJ))
                {
                    imageListLarge.Images.Add(Image.FromFile(@strLJ));
                }
            }

            listView1.View = View.LargeIcon;

            listView1.LargeImageList = imageListLarge;
        }



        // 初始化工作-视频
        public void DoInitialVideos()
        {

            DoInitial();

            m_bIsVideo = true; // 是视频
            //try
            //{
            //    // 显示窗体标题
            //    //   this.Text = m_theTitle;

            //    listView1.Clear(); // 清空listview

            //    // 添加图片
            //    int count = m_theFileList.Count;

            //    if (count == 0) // 没有则返回（不可能没有）
            //        return;

            //    // 添加图片名称
            //    string str;
            //    for (int i = 0; i < count; i++)
            //    {
            //        string filename = System.IO.Path.GetFileNameWithoutExtension((string)m_theFileList[i]);
            //        //str = String.Format("{0}", filename);
            //        ListViewItem item = new ListViewItem(filename, i);
            //        listView1.Items.Add(item);
            //    }

            //    ImageList imageListLarge = new ImageList();

            //    // 设置图片大小
            //    imageListLarge.ImageSize = new System.Drawing.Size(108, 108);



            //    // 按照图片路径获取图片
            //    for (int i = 0; i < count; i++)
            //    {
            //        str = (string)m_theFileList[i]; // 获得文件名
            //        Bitmap theImage = FrameGrabber.GetFrameFromVideo(Application.StartupPath+ "\\" + "HCPV\\"+str, 0.2d, new Size(200, 200)); // 得到图片
            //        imageListLarge.Images.Add(theImage); // 添加图片
            //    }

            //    listView1.View = View.LargeIcon;

            //    listView1.LargeImageList = imageListLarge;

            //    m_bIsVideo = true; // 是视频
            //}
            //catch (InvalidVideoFileException ex)
            //{
            //    MessageBox.Show(ex.Message, "Extraction failed");
            //}

        }



        // 双击选择图片
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                if (listView1.SelectedItems.Count > 0)
                {
                    string str = (string)m_theFileList[listView1.SelectedItems[0].Index];
                    string str_tempPath = Application.StartupPath + "\\" + "HCPV";
                    string strLJ = str_tempPath + "\\" + str;


                    if (m_bIsVideo == false)// 图片
                    {
                    //    frmPictureView1 m_frmPictureView1 = new frmPictureView1();
                    //    string str = (string)m_theFileList[listView1.SelectedItems[0].Index];
                    //    m_frmPictureView1.Text = str;
                    //    m_frmPictureView1.Show();
                    //    System.Threading.Thread.Sleep(500);

                    //    m_frmPictureView1.axAutoVueX1.SRC = strLJ;
                        //strLJ += ".JPG";

                    }
                    else // 视频
                    {
                    //    string str = (string)m_theFileList[listView1.SelectedItems[0].Index]; // 获取文件名
                    //    //str = System.IO.Path.GetFileNameWithoutExtension(str) + ".AVI";
                    //    FormPlayer theForm = new FormPlayer();

                    //    theForm.Text = str;
                    //    theForm.Show();

                    //    string str_tempPath = Application.StartupPath + "\\" + "HCPV";
                    //    string strLJ = str_tempPath + "\\" + str;

                    //    theForm.PlayTheVideo(strLJ); // 播放视频
                        //strLJ += ".avi";
                    }
                    System.Diagnostics.Process.Start(strLJ);
                    return;

                }
            }
            catch(SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }

        }
    }
}

