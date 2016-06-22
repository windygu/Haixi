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
        public ArrayList m_theFileList = new ArrayList(); // �ļ��б�
        public string m_type; // �ֺ�ͼƬ����
        public string m_theTitle; // �������
        public bool m_bIsVideo; // ��Ƶ��


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



        // ��ʼ������-������Ƶ
        public void DoInitial()
        {
            // ��ʾ�������
            //   this.Text = m_theTitle;
            try
            {
                listView1.Clear(); // ���listview

                // ���ͼƬ
                int count = m_theFileList.Count;

                if (count == 0) // û���򷵻أ�������û�У�
                    return;

                // ���ͼƬ����
                //string str;
                for (int i = 0; i < count; i++)
                {
                    string filename = System.IO.Path.GetFileNameWithoutExtension((string)m_theFileList[i]);
                    //str = String.Format("{0}", filename);
                    ListViewItem item = new ListViewItem(filename, i);
                    listView1.Items.Add(item);
                }

                m_bIsVideo = false; // ������Ƶ
            }
            catch (InvalidVideoFileException ex)
            {
                MessageBox.Show(ex.Message, "Extraction failed");
            }

        }

        public void ShowIconImage()
        {
            ImageList imageListLarge = new ImageList();

            // ����ͼƬ��С
            imageListLarge.ImageSize = new System.Drawing.Size(108, 108);
            Image m_ImageTemp;
            // ����ͼƬ·����ȡͼƬ
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



        // ��ʼ������-��Ƶ
        public void DoInitialVideos()
        {

            DoInitial();

            m_bIsVideo = true; // ����Ƶ
            //try
            //{
            //    // ��ʾ�������
            //    //   this.Text = m_theTitle;

            //    listView1.Clear(); // ���listview

            //    // ���ͼƬ
            //    int count = m_theFileList.Count;

            //    if (count == 0) // û���򷵻أ�������û�У�
            //        return;

            //    // ���ͼƬ����
            //    string str;
            //    for (int i = 0; i < count; i++)
            //    {
            //        string filename = System.IO.Path.GetFileNameWithoutExtension((string)m_theFileList[i]);
            //        //str = String.Format("{0}", filename);
            //        ListViewItem item = new ListViewItem(filename, i);
            //        listView1.Items.Add(item);
            //    }

            //    ImageList imageListLarge = new ImageList();

            //    // ����ͼƬ��С
            //    imageListLarge.ImageSize = new System.Drawing.Size(108, 108);



            //    // ����ͼƬ·����ȡͼƬ
            //    for (int i = 0; i < count; i++)
            //    {
            //        str = (string)m_theFileList[i]; // ����ļ���
            //        Bitmap theImage = FrameGrabber.GetFrameFromVideo(Application.StartupPath+ "\\" + "HCPV\\"+str, 0.2d, new Size(200, 200)); // �õ�ͼƬ
            //        imageListLarge.Images.Add(theImage); // ���ͼƬ
            //    }

            //    listView1.View = View.LargeIcon;

            //    listView1.LargeImageList = imageListLarge;

            //    m_bIsVideo = true; // ����Ƶ
            //}
            //catch (InvalidVideoFileException ex)
            //{
            //    MessageBox.Show(ex.Message, "Extraction failed");
            //}

        }



        // ˫��ѡ��ͼƬ
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                if (listView1.SelectedItems.Count > 0)
                {
                    string str = (string)m_theFileList[listView1.SelectedItems[0].Index];
                    string str_tempPath = Application.StartupPath + "\\" + "HCPV";
                    string strLJ = str_tempPath + "\\" + str;


                    if (m_bIsVideo == false)// ͼƬ
                    {
                    //    frmPictureView1 m_frmPictureView1 = new frmPictureView1();
                    //    string str = (string)m_theFileList[listView1.SelectedItems[0].Index];
                    //    m_frmPictureView1.Text = str;
                    //    m_frmPictureView1.Show();
                    //    System.Threading.Thread.Sleep(500);

                    //    m_frmPictureView1.axAutoVueX1.SRC = strLJ;
                        //strLJ += ".JPG";

                    }
                    else // ��Ƶ
                    {
                    //    string str = (string)m_theFileList[listView1.SelectedItems[0].Index]; // ��ȡ�ļ���
                    //    //str = System.IO.Path.GetFileNameWithoutExtension(str) + ".AVI";
                    //    FormPlayer theForm = new FormPlayer();

                    //    theForm.Text = str;
                    //    theForm.Show();

                    //    string str_tempPath = Application.StartupPath + "\\" + "HCPV";
                    //    string strLJ = str_tempPath + "\\" + str;

                    //    theForm.PlayTheVideo(strLJ); // ������Ƶ
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

