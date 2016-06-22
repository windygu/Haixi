using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using clsDataAccess;
using clsClipStat=JCZF.SubFrame.clsClipStat;


using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Functions;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;



namespace JCZF.SubFrame
{
    public partial class frmhz : DevComponents.DotNetBar.Office2007Form
    { 
        /// <summary>
        /// ��������
        /// </summary>
        public string str_xzqm = null;
        /// <summary>
        /// ����������
        /// </summary>
        public string str_xzqdm = null;
        /// <summary>
        /// ���ݴ���
        /// </summary>
        public DataAccess m_DataAccess_SYS_;
        /// <summary>
        /// ��ͼ
        /// </summary>
        public AxMapControl m_axmapcontrol1 = new AxMapControl();

        clsClipStat m_clsClipStat;
        private IEnumLayer m_Grouplayers;

        /// <summary>
        /// �ı�����ʾ���
        /// </summary>
      public  string strTemp = null;

        public frmhz(string str_xzqm0, string str_xzqdm0, DataAccess p_dataaccess, AxMapControl p_axmapcontrol)
        {
            InitializeComponent();
            str_xzqm = str_xzqm0;
            str_xzqdm = str_xzqdm0;
            m_DataAccess_SYS_ = p_dataaccess;
            m_axmapcontrol1 = p_axmapcontrol;

        }

        /// <summary>
        /// ��½����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmhz_Load(object sender, EventArgs e)
        {
            this.lab_dq.Text = str_xzqm + "���ܷ���";


        }
        /// <summary>
        /// ��ʼ���ܷ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            strTemp = null;

            IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
            if (hcjg != null)
            {
                this.m_axmapcontrol1.Map.DeleteLayer((ILayer)hcjg);

            }

            this.progressBar1.Visible = true;
            this.progressBar1.Value = 10;


            if (m_clsClipStat == null)
            {
                m_clsClipStat = new clsClipStat();

            }
            m_clsClipStat.m_strFilePath = Application.StartupPath + "\\OverlayTemp";
            m_clsClipStat.m_axMapcontrol = this.m_axmapcontrol1;


            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();//���б������



            try
            {

                #region  ����������״����
                if (chkTdlyxz.Checked)
                {


                    try
                    {
                        //����������״��
                        string str_TDLYXZ_table = "HZFX_" + str_xzqdm + "_TDLYXZ";
                        bool blif_TDLYXZ_table_exist = false;
                        bool blif_CalculateAgain = false;


                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_TDLYXZ_table == tablenames[i].Trim())
                            {
                                blif_TDLYXZ_table_exist = true;
                                break;
                            }
                        }
                        this.progressBar1.Value = 20;

                        #region  ���ܷ���������
                        if (blif_TDLYXZ_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("����������״������¼�Ѵ��ڣ��Ƿ����¼���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  ���¼���
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   �����¼���
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//�����¼���     

                                #region ��datagridview����ʾ���

                                string str_sql = "SELECT * FROM  " + str_TDLYXZ_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_TDLYXZ.DataSource = dataset_gridview.Tables[0];
                                #endregion
                                this.progressBar1.Value = 100;
                            }
                            #endregion

                        }
                        #endregion


                        #region ���ܷ�����񲻴���  ���� �¼���
                        if (blif_TDLYXZ_table_exist == false || blif_CalculateAgain == true)
                        {  //���㲢��������str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//������ʡ
                            {

                                calculate_LNS(str_TDLYXZ_table);

                            }

                            else if (str_xzqdm.Length == 4)//�ؼ���
                            {
                                calculate_ds(str_TDLYXZ_table, str_xzqm, str_xzqdm);

                            }

                            #region  ��������������״���ܷ���
                            else if (str_xzqdm.Length == 6)//����
                            {
                                string str_dltb_tablename = null;//����ͼ�߱������
                                string str_lxdl_tablename = null;//���ǵ���
                                string str_xzdw_tablename = null;//��״����

                                #region  �������ؼ�������������״������
                                for (int i = 0; i < tablenames.Length; i++)
                                {

                                    if (tablenames[i].Contains(str_xzqdm) && tablenames[i].Contains("DLTB"))
                                    {
                                        str_dltb_tablename = tablenames[i].Trim();
                                        continue;

                                    }

                                    if (tablenames[i].Contains(str_xzqdm) && tablenames[i].Contains("LXDL"))
                                    {
                                        str_lxdl_tablename = tablenames[i].Trim();
                                        continue;
                                    }


                                    if (tablenames[i].Contains(str_xzqdm) && tablenames[i].Contains("XZDW"))
                                    {
                                        str_xzdw_tablename = tablenames[i].Trim();

                                    }

                                }
                                #endregion

                                calculate_qxdltb(str_TDLYXZ_table, str_dltb_tablename, str_lxdl_tablename, str_xzdw_tablename);

                            }
                            #endregion


                            this.progressBar1.Value = 100;
                            #region ��datagridview����ʾ���
                            string str_sql = "SELECT * FROM  " + str_TDLYXZ_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_TDLYXZ.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //��ֹ����б�������
                        for (int ii = 0; ii < dataGridView_TDLYXZ.ColumnCount; ii++)
                        {
                            dataGridView_TDLYXZ.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_TDLYXZ.Columns[1].DefaultCellStyle.Format = "F10";

                        #region  ���ı�������ʾ���
                        string str_infomation = "      ����������״���������\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_TDLYXZ, this.txtResult);
                        #endregion
                    }

                    catch (Exception ex)
                    {
                        strTemp += "----------------------------------------------------------" + "\r\n" + "����������״�����д�����������" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }
                }

                #endregion


                #region  �滮���ݷ���

                if (chkGhsj.Checked)
                {
                    this.progressBar1.Value = 10;
                    try
                    {
                        string str_GHSJ_table = "HZFX_" + str_xzqdm + "_GHSJ";
                        bool blif_GHSJ_table_exist = false;
                        bool blif_CalculateAgain = false;


                        #region  ���ҹ滮���ݷ�������Ƿ����
                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_GHSJ_table == tablenames[i].Trim())
                            {
                                blif_GHSJ_table_exist = true;
                                break;
                            }
                        }

                        #endregion
                        this.progressBar1.Value = 20;
                        #region  �滮���ݻ��ܷ���������
                        if (blif_GHSJ_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("�滮���ݷ�����¼�Ѵ��ڣ��Ƿ����¼���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  ���¼���
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   �����¼���
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//�����¼���     
                                this.progressBar1.Value = 100;
                                #region ��datagridview����ʾ���

                                string str_sql = "SELECT * FROM  " + str_GHSJ_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_GHSJ.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region ���ܷ�����񲻴���  ���� �¼���
                        if (blif_GHSJ_table_exist == false || blif_CalculateAgain == true)
                        {  //���㲢��������str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//������ʡ
                            {

                                cal_GHSJ_LNS(str_GHSJ_table);

                            }

                            else if (str_xzqdm.Length == 4)//�ؼ���
                            {

                                cal_DS_GHSJ(str_xzqdm, str_xzqm, str_GHSJ_table);

                            }

                            #region  ���ع滮���ݻ��ܷ���
                     else if (str_xzqdm.Length == 6)//����
                       {   //���������������
                                bool blif_cancalculate = false;
                                double db_qxgh = 0;
                                string str_dsdm = str_xzqdm.Substring(0,4);
                                string str_dsmc = null;
                                for (int ii = 0; ii < tablenames.Length; ii++)
                                { 
                                    
                                    if((tablenames[ii].ToString()).Contains("GHT")&&(tablenames[ii].ToString()).Contains(str_dsdm))
                                   {
                                       blif_cancalculate = true;
                                       break;
                                    }
                                
                                
                                }





                                DataRow datarow_xzqdm;//��ѯ��������---���ϲ�ѯ������һ����¼
                                DataRowCollection datarowcollection_xzqdm;//
                    
                                datarowcollection_xzqdm =m_DataAccess_SYS_.GetAllDSXZQY();//xzqm,qhdm
                                for (int i = 0; i < datarowcollection_xzqdm.Count; i++)
                                {
                                    datarow_xzqdm = datarowcollection_xzqdm[i];
                                    if (str_dsdm == datarow_xzqdm[1].ToString())
                                    {
                                        str_dsmc = datarow_xzqdm[0].ToString();
                                    
                                    }
                               
                                }
                        

                                if (blif_cancalculate == false)
                                {
                                    //û�����ݣ����ܷ���
                                    cal_qx_GHSJ2(str_GHSJ_table, db_qxgh);
                                    

                                }
                                else
                                { 
                                    //���з���
                                    //IWorkspace m_workspace = SDE_FindWsByDefault();

                                    IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                    m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                    m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                                    for (int i = 0; i < 5; i++)
                                    {
                                        //m_clsClipStat.cal_JBNT();
                                        m_clsClipStat.cal_qxghsj(str_dsmc);

                                        this.progressBar1.Value = 50;
                                        if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                        {
                                            db_qxgh = m_clsClipStat.db_ghsj;
                                            this.progressBar1.Value = 60;
                                            break;
                                        }
                                    }


                                    cal_qx_GHSJ(str_GHSJ_table, db_qxgh);
                           

                              


                                }
                                /////////////////////////////


                            }


                        }
                            #endregion
                        this.progressBar1.Value = 100;
                        #region ��datagridview����ʾ���

                        string str_sql0 = "SELECT * FROM  " + str_GHSJ_table;

                        DataSet dataset_gridview0 = new DataSet();
                        dataset_gridview0 = m_DataAccess_SYS_.GetDataSetBySQL(str_sql0);
                        this.dataGridView_GHSJ.DataSource = dataset_gridview0.Tables[0];
                        #endregion



                        #endregion

                        //��ֹ����б�������
                        for (int ii = 0; ii < dataGridView_GHSJ.ColumnCount; ii++)
                        {
                            dataGridView_GHSJ.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_GHSJ.Columns[1].DefaultCellStyle.Format = "F10";

                        #region ���ı�����������
                        string str_infomation = "      �滮���ݷ��������\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_GHSJ, this.txtResult);
                        #endregion


                    }

                    catch (Exception ex)
                    {
                        strTemp += "------------------------------------------------------------" + "\r\n" + "�������ù滮�����д�����������" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }

                }
                #endregion


                #region   ����ũ�����

                if (chkJbnt.Checked)
                {
                    try
                    {
                        this.progressBar1.Value = 10;
                        string str_JBNT_table = "HZFX_" + str_xzqdm + "_JBNT";
                        bool blif_BNT_table_exist = false;
                        bool blif_CalculateAgain = false;
                        double db_jbntarea = 0;
                        //IFeature m_xzq_feat;//������feature

                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_JBNT_table == tablenames[i].Trim())
                            {
                                blif_BNT_table_exist = true;
                                break;
                            }
                        }

                        #region  ����ũ�����������
                        if (blif_BNT_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("����ũ�������¼�Ѵ��ڣ��Ƿ����¼���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  ���¼���
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   �����¼���
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//�����¼���     
                                this.progressBar1.Value = 100;
                                #region ��datagridview����ʾ���

                                string str_sql = "SELECT * FROM  " + str_JBNT_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_JBNT.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region   ����ũ����ܷ�����񲻴���  ���� �¼���
                        if (blif_BNT_table_exist == false || blif_CalculateAgain == true)
                        {
                            ////���㲢��������str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//������ʡ
                            {

                                IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                            }

                            else if (str_xzqdm.Length == 4)//�ؼ���
                            {

                                IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                                //strTemp += m_clsClipStat.strHCJG;
                                //strTemp += "--------------------------------------------------------------";
                                ////this.txtResult.Text = strTemp;
                                //this.txtResult.Text = strTemp;
                                //AddtoTOC(m_clsClipStat.pJbntOutputFeatClass);
                                //loadJBNTProp(m_clsClipStat.pJbntOutputFeatClass);
                                //this.progressBar1.Value = 60;





                            }


                            else if (str_xzqdm.Length == 6)//����
                            {
                                IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }



                            //����ũ��

                            for (int i = 0; i < 5; i++)
                            {
                                m_clsClipStat.cal_JBNT();
                                this.progressBar1.Value = 30;
                                if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                {
                                    db_jbntarea = m_clsClipStat.db_jbnt;
                                    this.progressBar1.Value = 50;
                                    break;
                                }
                            }


                            cal_LNS_JBNT(str_JBNT_table, db_jbntarea);

                            this.progressBar1.Value = 100;


                            #region ��datagridview����ʾ���
                            string str_sql = "SELECT * FROM  " + str_JBNT_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_JBNT.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //��ֹ����б�������
                        for (int ii = 0; ii < dataGridView_JBNT.ColumnCount; ii++)
                        {
                            dataGridView_JBNT.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_JBNT.Columns[1].DefaultCellStyle.Format = "F10";
                        this.progressBar1.Value = 100;
                        #region  ���ı�������ʾ���
                        string str_infomation = "      ����ũ����������\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_JBNT, this.txtResult);
                        #endregion


                    }
                    //
                    catch (Exception ex)
                    {
                        strTemp += "--------------------------------------------------------------" + "\r\n" + "����ũ�������д�����������" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }
                }
                #endregion


                #region  �����õط���

                if (chkJsyd.Checked)
                {
                    try
                    {
                        this.progressBar1.Value = 10;


                        string str_JSYD_table = "HZFX_" + str_xzqdm + "_JSYD";
                        bool blif_JSYD_table_exist = false;
                        bool blif_CalculateAgain = false;
                        double db_jsydarea = 0;
                        //IFeature m_xzq_feat;//������feature

                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_JSYD_table == tablenames[i].Trim())
                            {
                                blif_JSYD_table_exist = true;
                                break;
                            }
                        }

                        #region  �����õط���������
                        if (blif_JSYD_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("�����õط�����¼�Ѵ��ڣ��Ƿ����¼���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  ���¼���
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   �����¼���
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//�����¼���     
                                this.progressBar1.Value = 100;
                                #region ��datagridview����ʾ���

                                string str_sql = "SELECT * FROM  " + str_JSYD_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_JSYD.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region   �����õػ��ܷ�����񲻴���  ���� �¼���
                        if (blif_JSYD_table_exist == false || blif_CalculateAgain == true)
                        {
                            ////���㲢��������str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//������ʡ
                            {

                                IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                            }

                            else if (str_xzqdm.Length == 4)//�ؼ���
                            {

                                IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            else if (str_xzqdm.Length == 6)//����
                            {
                                IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }

                            //�����õ�

                            for (int i = 0; i < 5; i++)
                            {
                                //m_clsClipStat.cal_JBNT();
                                m_clsClipStat.cal_JSYD();
                                this.progressBar1.Value = 50;
                                if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                {
                                    db_jsydarea = m_clsClipStat.db_jsyd;
                                    this.progressBar1.Value = 60;
                                    break;
                                }
                            }



                            cal_LNS_JSYD(str_JSYD_table, db_jsydarea);

                            this.progressBar1.Value = 100;
                            #region ��datagridview����ʾ���
                            string str_sql = "SELECT * FROM  " + str_JSYD_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_JSYD.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //��ֹ����б�������
                        for (int ii = 0; ii < dataGridView_JSYD.ColumnCount; ii++)
                        {
                            dataGridView_JSYD.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_JSYD.Columns[1].DefaultCellStyle.Format = "F10";

                        #region  ���ı�������ʾ���
                        string str_infomation = "       �����õط��������\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_JSYD, this.txtResult);
                        #endregion





                    }
                    catch (Exception ex)
                    {
                        strTemp += "--------------------------------------------------------------" + "\r\n" + "������Ŀ���������д�����������" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }

                }
                #endregion

                #region  �����Դ�滮����
                if (chkKczygh.Checked)
                {
                    try
                    {
                        this.progressBar1.Value = 10;

                        string str_KCZY_table = "HZFX_" + str_xzqdm + "_KCZY";
                        bool blif_KCZY_table_exist = false;
                        bool blif_CalculateAgain = false;
                        double db_kczyarea = 0;
                        //IFeature m_xzq_feat;//������feature

                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_KCZY_table == tablenames[i].Trim())
                            {
                                blif_KCZY_table_exist = true;
                                break;
                            }
                        }

                        #region  �����Դ����������
                        if (blif_KCZY_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("�����Դ�滮������¼�Ѵ��ڣ��Ƿ����¼���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  ���¼���
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   �����¼���
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//�����¼���     
                                this.progressBar1.Value = 100;
                                #region ��datagridview����ʾ���

                                string str_sql = "SELECT * FROM  " + str_KCZY_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_KCGH.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region   �����Դ���ܷ�����񲻴���  ���� �¼���
                        if (blif_KCZY_table_exist == false || blif_CalculateAgain == true)
                        {
                            ////���㲢��������str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//������ʡ
                            {

                                IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                            }

                            else if (str_xzqdm.Length == 4)//�ؼ���
                            {

                                IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            else if (str_xzqdm.Length == 6)//����
                            {
                                IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            for (int i = 0; i < 5; i++)
                            {

                             
                                m_clsClipStat.cal_KCZY();
                                this.progressBar1.Value = 50;

                                if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                {
                                    db_kczyarea = m_clsClipStat.db_kczy;
                                    this.progressBar1.Value = 70;
                                    break;
                                }
                            }



                            cal_LNS_KCZY(str_KCZY_table, db_kczyarea);

                            this.progressBar1.Value = 100;
                            #region ��datagridview����ʾ���
                            string str_sql = "SELECT * FROM  " + str_KCZY_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_KCGH.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //��ֹ����б�������
                        for (int ii = 0; ii < dataGridView_JSYD.ColumnCount; ii++)
                        {
                            dataGridView_KCGH.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_KCGH.Columns[1].DefaultCellStyle.Format = "F10";

                        #region  ���ı�������ʾ���
                        string str_infomation = "       �����Դ�滮���������\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_KCGH, this.txtResult);
                        #endregion


                    }
                    catch (Exception ex)
                    {
                        strTemp += "--------------------------------------------------------------" + "\r\n" + "�����Դ�滮�����д�����������" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }

                }
                #endregion

                #region �ɿ�Ȩ����
                ////�ɿ�

                if (chkCkq.Checked)
                {

                    try
                    {
                        this.progressBar1.Value = 10;

                        string str_CKQ_table = "HZFX_" + str_xzqdm + "_CKQ";
                        bool blif_CKQ_table_exist = false;
                        bool blif_CalculateAgain = false;
                        double db_ckqarea = 0;
                        //IFeature m_xzq_feat;//������feature

                        for (int i = 0; i < tablenames.Length; i++)
                        {
                            if (str_CKQ_table == tablenames[i].Trim())
                            {
                                blif_CKQ_table_exist = true;
                                break;
                            }
                        }

                        #region  �ɿ�Ȩ����������
                        if (blif_CKQ_table_exist == true)
                        {
                            DialogResult dialog;
                            dialog = MessageBox.Show("�ɿ�Ȩ������¼�Ѵ��ڣ��Ƿ����¼���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            #region  ���¼���
                            if (dialog == DialogResult.Yes)
                            {
                                blif_CalculateAgain = true;

                            }
                            #endregion

                            #region   �����¼���
                            else if (dialog == DialogResult.No)
                            {
                                blif_CalculateAgain = false;//�����¼���     
                                this.progressBar1.Value = 100;
                                #region ��datagridview����ʾ���

                                string str_sql = "SELECT * FROM  " + str_CKQ_table;

                                DataSet dataset_gridview = new DataSet();
                                dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                this.dataGridView_CKQ.DataSource = dataset_gridview.Tables[0];
                                #endregion

                            }
                            #endregion

                        }
                        #endregion


                        #region   �ɿ�Ȩ���ܷ�����񲻴���  ���� �¼���
                        if (blif_CKQ_table_exist == false || blif_CalculateAgain == true)
                        {
                            ////���㲢��������str_TDLYXZ_table
                            if (str_xzqdm.Length == 2)//������ʡ
                            {

                                IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                            }

                            else if (str_xzqdm.Length == 4)//�ؼ���
                            {

                                IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            else if (str_xzqdm.Length == 6)//����
                            {
                                IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                            }


                            for (int i = 0; i < 5; i++)
                            {
                                m_clsClipStat.cal_CKQ();
                                this.progressBar1.Value = 50;

                                if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                {
                                    db_ckqarea = m_clsClipStat.db_ckq_zmj;
                                    this.progressBar1.Value = 60;
                                    break;
                                }
                            }

                     
                            cal_LNS_CKQ(str_CKQ_table, db_ckqarea);
                            this.progressBar1.Value = 100;

                            #region ��datagridview����ʾ���
                            string str_sql = "SELECT * FROM  " + str_CKQ_table;

                            DataSet dataset_gridview = new DataSet();
                            dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                            this.dataGridView_CKQ.DataSource = dataset_gridview.Tables[0];

                            #endregion

                        }

                        #endregion

                        //��ֹ����б�������
                        for (int ii = 0; ii < dataGridView_CKQ.ColumnCount; ii++)
                        {
                            dataGridView_CKQ.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                        }
                        //columns
                        dataGridView_CKQ.Columns[1].DefaultCellStyle.Format = "F10";

                        #region  ���ı�������ʾ���
                        string str_infomation = "       �ɿ�Ȩ���������\r\n";
                        strTemp = strTemp + str_infomation;
                        fdatagrieviewTOtextbox(dataGridView_CKQ, this.txtResult);
                        #endregion



                    }
                    catch (Exception ex)
                    {
                        strTemp += "--------------------------------------------------------------" + "\r\n" + "�ɿ�Ȩ�Ǽ������д�����������" + "\r\n";
                        this.txtResult.Text = strTemp;
                    }

                }
                #endregion


                #region ̽��Ȩ����
                if (chkTkq.Checked)
                 {         
                     try
                     {
                         this.progressBar1.Value = 10;
                         string str_TKQ_table = "HZFX_" + str_xzqdm + "_TKQ";
                         bool blif_TKQ_table_exist = false;
                         bool blif_CalculateAgain = false;
                         double db_tkqyarea = 0;
                         //IFeature m_xzq_feat;//������feature

                         for (int i = 0; i < tablenames.Length; i++)
                         {
                             if (str_TKQ_table == tablenames[i].Trim())
                             {
                                 blif_TKQ_table_exist = true;
                                 break;
                             }
                         }

                         #region  �����Դ����������
                         if (blif_TKQ_table_exist == true)
                         {
                             DialogResult dialog;
                             dialog = MessageBox.Show("̽��Ȩ������¼�Ѵ��ڣ��Ƿ����¼���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                             #region  ���¼���
                             if (dialog == DialogResult.Yes)
                             {
                                 blif_CalculateAgain = true;

                             }
                             #endregion

                             #region   �����¼���
                             else if (dialog == DialogResult.No)
                             {
                                 blif_CalculateAgain = false;//�����¼���     
                                 this.progressBar1.Value = 100;
                                 #region ��datagridview����ʾ���

                                 string str_sql = "SELECT * FROM  " + str_TKQ_table;

                                 DataSet dataset_gridview = new DataSet();
                                 dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                                 this.dataGridView_TKQ.DataSource = dataset_gridview.Tables[0];
                                 #endregion

                             }
                             #endregion

                         }
                         #endregion


                         #region  ̽��Ȩ���ܷ�����񲻴���  ���� �¼���
                         if (blif_TKQ_table_exist == false || blif_CalculateAgain == true)
                         {
                             ////���㲢��������str_TDLYXZ_table
                             if (str_xzqdm.Length == 2)//������ʡ
                             {

                                 IFeature m_xzq_feat = get_LNS_geometry(str_xzqdm);

                                 m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                 m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);
                             }

                             else if (str_xzqdm.Length == 4)//�ؼ���
                             {

                                 IFeature m_xzq_feat = get_DS_geometry(str_xzqdm);

                                 m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                 m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                             }


                             else if (str_xzqdm.Length == 6)//����
                             {
                                 IFeature m_xzq_feat = get_QX_geometry(str_xzqdm);

                                 m_clsClipStat.m_Geometry = m_xzq_feat.ShapeCopy;

                                 m_clsClipStat.CreateClipFeatureClass(m_xzq_feat.ShapeCopy);

                             }


                             for (int i = 0; i < 5; i++)
                             {
                                 m_clsClipStat.cal_TKQ();

                                 this.progressBar1.Value = 50;

                                 if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                 {
                                     db_tkqyarea = m_clsClipStat.db_tkq;
                                     this.progressBar1.Value = 70;
                                     break;
                                 }
                             }

                             cal_LNS_TKQ(str_TKQ_table, db_tkqyarea);

                             //cal_LNS_KCZY(str_TKQ_table, db_tkqyarea);
                             this.progressBar1.Value = 100;

                             #region ��datagridview����ʾ���
                             string str_sql = "SELECT * FROM  " + str_TKQ_table;

                             DataSet dataset_gridview = new DataSet();
                             dataset_gridview = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                             this.dataGridView_TKQ.DataSource = dataset_gridview.Tables[0];

                             #endregion

                         }

                         #endregion

                         //��ֹ����б�������
                         for (int ii = 0; ii < dataGridView_JSYD.ColumnCount; ii++)
                         {
                             dataGridView_TKQ.Columns[ii].SortMode = DataGridViewColumnSortMode.NotSortable;

                         }
                         //columns
                         dataGridView_TKQ.Columns[1].DefaultCellStyle.Format = "F10";

                         #region  ���ı�������ʾ���
                         string str_infomation = "       ̽��Ȩ���������\r\n";
                         strTemp = strTemp + str_infomation;
                         fdatagrieviewTOtextbox(dataGridView_TKQ, this.txtResult);
                         #endregion
                         this.progressBar1.Visible = false;
                
                     }
                     catch (Exception ex)
                     {
                         strTemp += "--------------------------------------------------------------" + "\r\n" + "̽��Ȩ�Ǽ������д�����������";
                         this.txtResult.Text = strTemp;
                     }


                 }
                     #endregion
            }
            catch (Exception o)
            { }
           this.progressBar1.Visible = false; 



        }

















        



        #region  ��ϸ���ڲ���
        /// <summary>
        /// չ����ϸ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetail_Click(object sender, EventArgs e)
        {
            this.Width =854;
            this.Height =516;
            this.btnNoDetail.Enabled = true;
            this.btnDetail.Enabled = false;
        }

        /// <summary>
        /// ������ϸ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNoDetail_Click(object sender, EventArgs e)
        {  
            this.Width = 427;
            this.Height = 516;
            this.btnNoDetail.Enabled = false;
            this.btnDetail.Enabled = true;
        }
        #endregion

        #region   �˳��رմ���
        /// <summary>
        /// �˳�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
                if (hcjg != null)
                {
                    //this.m_frmBHTBView.axMapControl1.Map.DeleteLayer((ILayer)hcjg);
                    this.m_axmapcontrol1.Map.DeleteLayer((ILayer)hcjg);

                }
                if (System.IO.Directory.Exists(Application.StartupPath + "\\OverlayTemp"))
                {
                    DeleteFolder(Application.StartupPath + "\\OverlayTemp");
                }
            }
            catch (Exception ex)
            { }
            this.Close();
        }

        /// <summary>
        /// �ر�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmhz_FormClosing(object sender, FormClosingEventArgs e)
        { 
            try
            {
                IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
                if (hcjg != null)
                {
                    //this.m_frmBHTBView.axMapControl1.Map.DeleteLayer((ILayer)hcjg);
                    this.m_axmapcontrol1.Map.DeleteLayer((ILayer)hcjg);

                }
                if (System.IO.Directory.Exists(Application.StartupPath + "\\OverlayTemp"))
                {
                    DeleteFolder(Application.StartupPath + "\\OverlayTemp");
                }
            }
            catch (Exception ex)
            { }

        }

        #endregion

        #region  �����������

        /// <summary>
        /// ������ʡ������
        /// </summary>
        /// <param name="str_dsdm"></param>
        /// <returns></returns>
        public IFeature get_LNS_geometry(string str_dsdm)
        {
            //��ȡ������ʡ������ͼ��
            IFeatureLayer m_dsfeaturelayer = MapFunction.getFeatureLayerByName("ʡ", this.m_axmapcontrol1);

            //��ȡ������ʡ��������
            IFeatureClass m_FeatureClass = (m_dsfeaturelayer as FeatureLayer).FeatureClass;

            IFields p_fields = m_FeatureClass.Fields;
            int index = 0;
            IField p_field;
            for (int i = 0; i < p_fields.FieldCount; i++)
            {
                p_field = p_fields.get_Field(i);
                if (p_field.Name == "QHDM")
                {
                    index = i;
                    break;
                }

            }

            IFeatureCursor pcursor;
            pcursor = m_FeatureClass.Search(null, false);
            IFeature pfeat;
            pfeat = pcursor.NextFeature();
            while (pfeat != null)
            {
                if (pfeat.get_Value(index).ToString() == str_dsdm)
                {
                    return pfeat as IFeature;
                    break;

                }
                pfeat = pcursor.NextFeature();

            }

            return pfeat;





        }

        /// <summary>
        /// �õ��ؼ���������
        /// </summary>
        /// <returns></returns>
        public IFeature get_DS_geometry(string str_dsdm)
        {
            //��ȡ����������ͼ��
            IFeatureLayer m_dsfeaturelayer = MapFunction.getFeatureLayerByName("��", this.m_axmapcontrol1);

            //��ȡ������������
            IFeatureClass m_FeatureClass = (m_dsfeaturelayer as FeatureLayer).FeatureClass;

            IFields p_fields = m_FeatureClass.Fields;
            int index = 0;
            IField p_field;
            for (int i = 0; i < p_fields.FieldCount; i++)
            {
                p_field = p_fields.get_Field(i);
                if (p_field.Name == "QHDM")
                {
                    index = i;
                    break;
                }

            }

            IFeatureCursor pcursor;
            pcursor = m_FeatureClass.Search(null, false);
            IFeature pfeat;
            pfeat = pcursor.NextFeature();
            while (pfeat != null)
            {
                if (pfeat.get_Value(index).ToString() == str_dsdm)
                {
                    return pfeat as IFeature;
                    break;

                }
                pfeat = pcursor.NextFeature();

            }

            return pfeat;

        }
        /// <summary>
        /// �õ�����������
        /// </summary>
        /// <param name="str_dsdm"></param>
        /// <returns></returns>
        public IFeature get_QX_geometry(string str_dsdm)
        {
            //��ȡ����������ͼ��
            IFeatureLayer m_dsfeaturelayer = MapFunction.getFeatureLayerByName("��", this.m_axmapcontrol1);

            //��ȡ������������
            IFeatureClass m_FeatureClass = (m_dsfeaturelayer as FeatureLayer).FeatureClass;

            IFields p_fields = m_FeatureClass.Fields;
            int index = 0;
            IField p_field;
            for (int i = 0; i < p_fields.FieldCount; i++)
            {
                p_field = p_fields.get_Field(i);
                if (p_field.Name == "QHDM")
                {
                    index = i;
                    break;
                }

            }

            IFeatureCursor pcursor;
            pcursor = m_FeatureClass.Search(null, false);
            IFeature pfeat;
            pfeat = pcursor.NextFeature();
            while (pfeat != null)
            {
                if (pfeat.get_Value(index).ToString() == str_dsdm)
                {
                    return pfeat as IFeature;
                    break;

                }
                pfeat = pcursor.NextFeature();

            }

            return pfeat;





        }

        #endregion

        #region  ����������״��������
        //��Х�� 090627
        /// <summary>
        /// �������ص�����Ϣ,str_qxdm���ش���,str_dltb_tablename����ͼ�߱������
        /// </summary>
        /// <param name="str_qxdm"></param>
        public void calculate_qxdltb(string str_qxhzfx_table, string str_dltb_tablename, string str_lxdl_tablename, string str_xzdw_tablename)//(string str_dltb_tablename,string str_lxdl_tablename,str_xzdw_tablename)
        {
            //�������ػ��ܷ�����str_qxhzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();
            #region
            //��ȡ���б������

            bool if_qxhzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_qxhzfx_table == tablenames[i].Trim())
                {
                    if_qxhzfx_table_exist = true;
                    break;
                }
            }
            if (if_qxhzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_qxhzfx_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_qxhzfx_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_qxhzfx_table);
            }
            #endregion

            DataSet dataset_result = new DataSet();
            string str_sql = null;

            //�������ͼ��
            if (str_dltb_tablename != null)
            {
                str_sql = "Select DLMC AS ��������,SUM(TBMJ) AS ������� " +
                                 " FROM  " + str_dltb_tablename +
                                  " GROUP BY DLMC   ORDER BY DLMC";
                dataset_result = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                //�����ݴ�����str_qxhzfx_table
                m_DataAccess_SYS_.export_table(dataset_result, str_qxhzfx_table);

            }
         
            //�������ǵ���
            if (str_lxdl_tablename != null)
            {
                str_sql = "Select DLMC AS ��������,SUM(LXMJ) AS ������� " +
                                      " FROM  " + str_lxdl_tablename +
                                       " GROUP BY DLMC   ORDER BY DLMC";
                dataset_result = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                //�����ݴ�����str_qxhzfx_table
                m_DataAccess_SYS_.export_table(dataset_result, str_qxhzfx_table);

            }
            this.progressBar1.Value = 70;
            //������״����
            if (str_xzdw_tablename != null)
            {
                str_sql = "Select DLMC AS ��������,SUM(DWMJ) AS ������� " +
                                    " FROM  " + str_xzdw_tablename +
                                     " GROUP BY DLMC   ORDER BY DLMC";
                dataset_result = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                //�����ݴ�����str_qxhzfx_table
                m_DataAccess_SYS_.export_table(dataset_result, str_qxhzfx_table);

            }

            //�������ϱ���е�����
            if ((str_dltb_tablename == null) && (str_lxdl_tablename == null) && (str_xzdw_tablename == null))
            {
                return;
            }
            else
            {

                str_sql = "Select ��������, SUM(�������) AS �������" +
                                         " FROM  " + str_qxhzfx_table +
                                          " GROUP BY ��������   ORDER BY ��������";
                dataset_result = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                m_DataAccess_SYS_.DeleteAll(str_qxhzfx_table);
                m_DataAccess_SYS_.export_table2(dataset_result, str_qxhzfx_table);//����Ҫ��λת��

            }

        }


        /// <summary>
        /// ������м�������Ϣ��str_dshzfx_table���л��ܷ�����str_dsmc��������,str_dsdm���д���
        /// </summary>
        public void calculate_ds(string str_dshzfx_table, string str_dsmc, string str_dsdm)
        {
            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();
            //������л��ܷ�����str_dshzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region

            bool if_dshzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_dshzfx_table == tablenames[i].Trim())
                {
                    if_dshzfx_table_exist = true;
                    break;
                }
            }
            if (if_dshzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_dshzfx_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_dshzfx_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_dshzfx_table);
            }
            #endregion



            //���������������������ƺʹ���
            DataRowCollection datarowcollection;
            datarowcollection = m_DataAccess_SYS_.GetQXXZQYofQHDM(str_dsdm);//��һ��xzqm ��������,�ڶ���qhdm����������


            DataSet dataset_temp;//������ʱ�洢���ݵ����ݼ�
            string str_sql = null;//��ѯ���
            //string str_ght_tablename = null;//�ؼ��й滮ͼ

            //����������ĵ�����Ϣ��������ؼ��л��ܱ���
            #region
            for (int i = 0; i < datarowcollection.Count; i++)
            {
                DataRow datarow_temp = datarowcollection[i];

                string str_qxhzfx_table = "HZFX_" + datarow_temp[1].ToString()+"_TDLYXZ";//���ػ��ܷ����������
                str_qxhzfx_table = str_qxhzfx_table.Trim();

                string str_qxdm = datarow_temp[1].ToString();//���ش���
                str_qxdm = str_qxdm.Trim();

                string str_dltb_tablename = null;//���ص���ͼ��
                string str_lxdl_tablename = null;//�������ǵ���
                string str_xzdw_tablename = null;//������״����



                //�������ص����ű��͵ؼ��е��йر��
                #region
                for (int ii = 0; ii < tablenames.Length; ii++)
                {
                    #region   ���ص�����Ϣ��
                    if (tablenames[ii].Contains(str_qxdm) && (tablenames[ii].Contains("DLTB")||tablenames[ii].Contains("����ͼ��")))
                    {
                        str_dltb_tablename = tablenames[ii].Trim();
                        continue;

                    }

                    if (tablenames[ii].Contains(str_qxdm) && (tablenames[ii].Contains("LXDL")||tablenames[ii].Contains("���ǵ���")))
                    {
                        str_lxdl_tablename = tablenames[ii].Trim();
                        continue;
                    }


                    if (tablenames[ii].Contains(str_qxdm) && (tablenames[ii].Contains("XZDW")||tablenames[ii].Contains("��״����")))
                    {
                        str_xzdw_tablename = tablenames[ii].Trim();
                        continue;
                    }
                    #endregion





                }
                #endregion


                //��������ص�����Ϣ
                calculate_qxdltb(str_qxhzfx_table, str_dltb_tablename, str_lxdl_tablename, str_xzdw_tablename);

                if (i >= datarowcollection.Count/2)
                     this.progressBar1.Value = 60;

                //�����ػ�����Ϣ������л��ܷ�����str_dshzfx_table
                str_sql = "SELECT * FROM " + str_qxhzfx_table;
                dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                m_DataAccess_SYS_.export_table2(dataset_temp, str_dshzfx_table);//����Ҫ��λת��


            }

            #endregion

            #region  �������е����ؼ�������Ϣ
            str_sql = "Select ��������, SUM(�������) AS �������" +
                                       " FROM  " + str_dshzfx_table + " GROUP BY ��������    ORDER BY �������� ";
         

            dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);

            m_DataAccess_SYS_.DeleteAll(str_dshzfx_table);
            m_DataAccess_SYS_.export_table2(dataset_temp, str_dshzfx_table);

            #endregion

         


        }

        /// <summary>
        /// ���������ʡ����������״
        /// </summary>
        /// <param name="str_LNShzfx_table"></param>
        public void calculate_LNS(string str_LNShzfx_table)
        {


            
            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region

            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_LNShzfx_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_LNShzfx_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_LNShzfx_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_LNShzfx_table);
            }
            #endregion

            //�������������ĵؼ������ƺʹ���
            DataRowCollection datarowcollection;
            datarowcollection = m_DataAccess_SYS_.GetAllDSXZQY();//��һ��xzqm ��������,�ڶ���qhdm����������


            DataSet dataset_temp;//������ʱ�洢���ݵ����ݼ�
            string str_sql = null;//��ѯ���
            //progressBar_cal.Value = 30;



            for (int i = 0; i < datarowcollection.Count; i++)
            {
                DataRow datarow_temp = datarowcollection[i];

                string str_dshzfx_table = "HZFX_" + datarow_temp[1]+"_TDLYXZ";
                string str_dsmc = datarow_temp[0].ToString();
                str_dsmc = str_dsmc.Trim();

                string str_dsdm = datarow_temp[1].ToString();
                str_dsdm = str_dsdm.Trim();

                calculate_ds(str_dshzfx_table, str_dsmc, str_dsdm);

                if (i >= datarowcollection.Count / 2)
                    this.progressBar1.Value = 50;//����

                //����Ϣ���ܺ���������ʡ���ܷ�����str_LNShzfx_table
                 str_sql = "SELECT * FROM " + str_dshzfx_table;
                dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                m_DataAccess_SYS_.export_table2(dataset_temp, str_LNShzfx_table);

            }
            //progressBar_cal.Value = 70;

            //�������
            str_sql = "Select ��������  AS ��������, SUM(�������) AS �������" +
                                     " FROM  " + str_LNShzfx_table +
                                     "   GROUP BY ��������     ORDER BY �������� ";


            dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);

            m_DataAccess_SYS_.dropatable(str_LNShzfx_table);
            m_DataAccess_SYS_.creatatable(str_LNShzfx_table);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_LNShzfx_table);


        }
        #endregion

        #region  �滮���ݷ�������
        /// <summary>
        /// ���������ʡ�滮����
        /// </summary>
        /// <param name="str_GHSJ_table"></param>
        private void cal_GHSJ_LNS(string str_GHSJ_table)
        {
            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region

            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_GHSJ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_GHSJ_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);
            }
            #endregion

            //�������������ĵؼ������ƺʹ���
            DataRowCollection datarowcollection;
            datarowcollection = m_DataAccess_SYS_.GetAllDSXZQY();//��һ��xzqm ��������,�ڶ���qhdm����������
            DataRow datarow_temp;
            DataSet dataset_temp;
            string str_sql = null;

            #region
            for (int i = 0; i < datarowcollection.Count; i++)
            {
                datarow_temp = datarowcollection[i];

                string str_dsmc = datarow_temp[0].ToString();//�ؼ�������
                str_dsmc = str_dsmc.Trim();

                string str_dsdm = datarow_temp[1].ToString();//�ؼ��д���
                str_dsdm = str_dsdm.Trim();

                //string str_ght_tablename = null;//�ؼ��й滮������
                string str_dshzfx_table = "HZFX_"+str_dsdm+"_GHSJ";

                cal_DS_GHSJ(str_dsdm, str_dsmc, str_dshzfx_table);

                str_sql = "SELECT * FROM " + str_dshzfx_table;
                dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);
                m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);              
            }

            #endregion

            //�ڼ���ؼ�����Ϣ�����������ʡ���ܷ����������
            str_sql = "SELECT �������� AS �������� ,  SUM(�������)  AS  �������   FROM " + str_GHSJ_table
                + "  GROUP BY  ��������  ORDER BY ��������";
            dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);

            m_DataAccess_SYS_.DeleteAll(str_GHSJ_table);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);

        

        
        
        
        }

        /// <summary>
        /// ����ؼ��й滮����
        /// </summary>
        /// <param name="str_GHSJ_table"></param>
        private void cal_DS_GHSJ(string str_dsdm,string str_dsmc, string str_GHSJ_table)
        {

            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();
            string str_ght_tablename = null;//�ؼ��й滮��
            DataSet dataset_temp;
            string str_sql = null;
            //������л��ܷ�����str_GHSJ_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region

            bool if_dshzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_GHSJ_table == tablenames[i].Trim())
                {
                    if_dshzfx_table_exist = true;
                   
                }
                if (tablenames[i].Contains(str_dsdm) && tablenames[i].Contains("GHT") && tablenames[i].Contains(str_dsmc))
                {
                    str_ght_tablename = tablenames[i].Trim();

                }
            }

            if (if_dshzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_GHSJ_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);
            }
            #endregion



            //�ڱ��str_GHSJ_table�м���ؼ��еĹ滮��Ϣ
            if (str_ght_tablename != null)
            {
                str_sql = "Select  FQMC AS  ��������, SUM(AREA_) AS �������" +
                                           " FROM  " + str_ght_tablename +
                                            "  GROUP BY FQMC   ORDER BY FQMC";
                dataset_temp = m_DataAccess_SYS_.GetDataSetBySQL(str_sql);

                m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);
            }


        
        }

        private void cal_qx_GHSJ(string str_GHSJ_table, double db_ghsjarea)
        {
            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();



            DataSet dataset_temp = new DataSet();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_GHSJ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_GHSJ_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("��������", typeof(string));
            DataColumn dacolum1 = new DataColumn("�������", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);
            DataRow darow ;
            double db_temp=0;
            float fl_temp = 0;

            for (int ii = 0; ii < m_clsClipStat.arrghmc.Count; ii++)
            {
                darow = datable.NewRow();
                darow["��������"] = m_clsClipStat.arrghmc[ii].ToString() ;
                db_temp = Convert.ToDouble(m_clsClipStat.dblghtb[ii].ToString());

                fl_temp = (float)db_temp;

                darow["�������"] = fl_temp/10000;

                datable.Rows.Add(darow);
                
               
            }
            dataset_temp.Tables.Add(datable);
            m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);
        
        }

     

 private void cal_qx_GHSJ2(string str_GHSJ_table, double db_ghsjarea)
        {
            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            DataSet dataset_temp = new DataSet();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_GHSJ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_GHSJ_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_GHSJ_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("��������", typeof(string));
            DataColumn dacolum1 = new DataColumn("�������", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);
            DataRow darow=datable.NewRow();;


            darow["��������"] = "�滮����";
            darow["�������"] = db_ghsjarea;
            datable.Rows.Add(darow);
           
            dataset_temp.Tables.Add(datable);
            m_DataAccess_SYS_.export_table2(dataset_temp, str_GHSJ_table);
        
        
        
        
        
        }



        #endregion


    



        #region  ����ũ����㷽��
        /// <summary>
        /// ����ũ����㷽��
        /// </summary>
        /// <param name="str_JBNT_table"></param>
        /// <param name="db_jbntarea"></param>
        private void cal_LNS_JBNT(string str_JBNT_table,double db_jbntarea )
        {

            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

       

            DataSet dataset_temp=new DataSet();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_JBNT_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_JBNT_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_JBNT_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_JBNT_table);
            }
            #endregion
            DataTable datable=new DataTable();
            DataColumn dacolum0 = new DataColumn("��������",  typeof( string));
            DataColumn dacolum1 = new DataColumn("�������",typeof (float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["��������"] = "����ũ��";
            darow["�������"] = db_jbntarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);
           


            m_DataAccess_SYS_.export_table2(dataset_temp, str_JBNT_table);      
       
        }

        #endregion

        #region  �����õؼ��㷽��
        /// <summary>
        /// �����õط���
        /// </summary>
        /// <param name="str_JSYD_table"></param>
        /// <param name="db_jsydarea"></param>
        private void cal_LNS_JSYD (string str_JSYD_table,double db_jsydarea )
        {

            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

      

            DataSet dataset_temp=new DataSet();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_JSYD_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_JSYD_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_JSYD_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_JSYD_table);
            }
            #endregion
            DataTable datable=new DataTable();
            DataColumn dacolum0 = new DataColumn("��������",  typeof( string));
            DataColumn dacolum1 = new DataColumn("�������",typeof (float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["��������"] = "���������õ�";
            darow["�������"] = db_jsydarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);
       
            m_DataAccess_SYS_.export_table2(dataset_temp, str_JSYD_table);      
       
        }

        #endregion


        #region �����Դ�滮���㷽��
        /// <summary>
        /// �����Դ�滮
        /// </summary>
        /// <param name="str_KCZY_table"></param>
        /// <param name="db_kczyarea"></param>
        private void cal_LNS_KCZY(string str_KCZY_table, double db_kczyarea)
        {
            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            DataSet dataset_temp = new DataSet();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_KCZY_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_KCZY_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_KCZY_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_KCZY_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("��������", typeof(string));
            DataColumn dacolum1 = new DataColumn("�������", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["��������"] = "�����Դ�滮";
            darow["�������"] = db_kczyarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_KCZY_table);     




        }



        #endregion

        #region  �ɿ�Ȩ����
        /// <summary>
        /// �ɿ�Ȩ����
        /// </summary>
        /// <param name="str_CKQ_table"></param>
        /// <param name="db_ckqarea"></param>
        private void cal_LNS_CKQ(string str_CKQ_table, double db_ckqarea)
        {             //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            DataSet dataset_temp = new DataSet();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_CKQ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_CKQ_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_CKQ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_CKQ_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("��������", typeof(string));
            DataColumn dacolum1 = new DataColumn("�������", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["��������"] = "�ɿ�Ȩ";
            darow["�������"] = db_ckqarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_CKQ_table);  
        
        
        }
        #endregion

         
        #region  ̽��Ȩ����
        /// <summary>
        /// ����ĳ������̽��Ȩ
        /// </summary>
        /// <param name="str_TKQ_table"></param>
        /// <param name="db_tkqarea"></param>
        private void cal_LNS_TKQ(string str_TKQ_table, double db_tkqarea)
        {
            //��ȡ���б������
            string[] tablenames = m_DataAccess_SYS_.GetAllDataTablesName();

            DataSet dataset_temp = new DataSet();

            //���������ʡ���ܷ�����str_LNShzfx_table�Ƿ���ڣ���������ɾ���ɱ����±�,����������ֱ�Ӵ����±�
            #region
            bool if_LNShzfx_table_exist = false;

            for (int i = 0; i < tablenames.Length; i++)
            {
                if (str_TKQ_table == tablenames[i].Trim())
                {
                    if_LNShzfx_table_exist = true;
                    break;
                }
            }
            if (if_LNShzfx_table_exist == true)
            {
                //ɾ��ԭ��
                m_DataAccess_SYS_.dropatable(str_TKQ_table);
                //�����±�
                m_DataAccess_SYS_.creatatable(str_TKQ_table);

            }
            else
            {
                m_DataAccess_SYS_.creatatable(str_TKQ_table);
            }
            #endregion
            DataTable datable = new DataTable();
            DataColumn dacolum0 = new DataColumn("��������", typeof(string));
            DataColumn dacolum1 = new DataColumn("�������", typeof(float));

            datable.Columns.Add(dacolum0);
            datable.Columns.Add(dacolum1);

            DataRow darow = datable.NewRow();
            darow["��������"] = "̽��Ȩ";
            darow["�������"] = db_tkqarea;
            //dataset_temp.Tables[0] = datable;
            datable.Rows.Add(darow);
            dataset_temp.Tables.Add(datable);

            m_DataAccess_SYS_.export_table2(dataset_temp, str_TKQ_table);  
        
                
        }



        #endregion




        /// <summary>
        /// datagridview�����textbox��
        /// </summary>
        /// <param name="p_datagridview"></param>
        /// <param name="p_textbox"></param>
        public void fdatagrieviewTOtextbox(DataGridView p_datagridview, TextBox p_textbox)
        {

            string str_tdlx;//��������
            string str_tdmj;//�������
            double db_zmj = 0;//�����
            string str_infomation = null;

            for (int ii = 0; ii < p_datagridview.RowCount - 1; ii++)
            {
                if (p_datagridview.Rows[ii].Cells[0].Value == null)
                    continue;
                str_tdlx = (p_datagridview.Rows[ii].Cells[0].Value.ToString()).Trim();

                if (p_datagridview.Rows[ii].Cells[1].Value == null)
                    p_datagridview.Rows[ii].Cells[1].Value = 0;

                str_tdmj = (p_datagridview.Rows[ii].Cells[1].Value.ToString()).Trim();

                str_infomation = str_tdlx + ":  " + str_tdmj + "ƽ���� " + "��\r\n";
                strTemp = strTemp + str_infomation;
                if (p_datagridview.Rows[ii].Cells[1].Value.ToString() != null)
                    db_zmj = db_zmj + Convert.ToDouble(p_datagridview.Rows[ii].Cells[1].Value);
            }

            str_tdlx = "�������";
            str_tdmj = db_zmj.ToString();
            str_infomation = str_tdlx + ":  " + str_tdmj + "ƽ����" + "��\r\n";
            strTemp = strTemp + str_infomation;
            strTemp = strTemp + "----------------------------------------------------------\r\n";

            p_textbox.Text = strTemp;
        
        
        
        }

        /// <summary>
        /// �õ��˲���ͼ��
        /// </summary>
        /// <param name="grouplayername"></param>
        /// <returns></returns>
        private IGroupLayer GetHcjggrouplayer(string grouplayername)
        {
            IGroupLayer ResGrouplayer = null;
            this.m_Grouplayers = Functions.MapFunction.GetGroupLayers(this.m_axmapcontrol1.ActiveView.FocusMap);
            ILayer pLayer = m_Grouplayers.Next();
            while (pLayer != null)
            {
                if (pLayer.Name == grouplayername)
                {
                    ResGrouplayer = (IGroupLayer)pLayer;
                    break;
                }
                pLayer = m_Grouplayers.Next();
            }

            m_Grouplayers.Reset();
            return ResGrouplayer;
        }

        /// <summary>
        /// ɾ���ļ���
        /// </summary>
        /// <param name="dir"></param>
        private void DeleteFolder(string dir)
        {
            foreach (string d in System.IO.Directory.GetFileSystemEntries(dir))
            {
                if (System.IO.File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//ֱ��ɾ�����е��ļ�   
                }
                else
                    DeleteFolder(d);//�ݹ�ɾ�����ļ���   
            }
            Directory.Delete(dir);//ɾ���ѿ��ļ���   
        }



        private void AddtoTOC(IFeatureClass outputfeatclass)
        {
            IFeatureLayer outlayer = new FeatureLayer() as IFeatureLayer;
            outlayer.FeatureClass = outputfeatclass;
            outlayer.Name = outputfeatclass.AliasName;

            IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
            IGroupLayer hcjg1 = new GroupLayer() as IGroupLayer;

            if (hcjg != null)
            {
                hcjg.Add((ILayer)outlayer);
                this.m_axmapcontrol1.Map.AddLayer(outlayer);
                this.m_axmapcontrol1.Map.DeleteLayer(outlayer);

            }
            else
            {
                //IGroupLayer hcjg = new GroupLayer() as IGroupLayer;
                hcjg1.SpatialReference =  this.m_axmapcontrol1.SpatialReference;
                hcjg1.Name = "�˲���";
                hcjg1.Add((ILayer)outlayer);
                this.m_axmapcontrol1.Map.AddLayer((ILayer)hcjg1);
            }
        }


       
        private void btnExport_Click(object sender, EventArgs e)
        {
            //m_DataAccess_SYS.OutputExcel(m_ResultDataGrid, "���غ˲�ռ�������", "");
            if (chkTdlyxz.Checked && this.dataGridView_TDLYXZ!= null)
            {
                //m_DataAccess_SYS.OutputExcel(this.m_UserDLTBProperyShow.m_PropertyDataGrid
                this.m_DataAccess_SYS_.OutputExcel(this.dataGridView_TDLYXZ, "��״����������", "");

            }
            if (this.chkGhsj.Checked && this.dataGridView_GHSJ!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_GHSJ, "�滮ͼ�������", "");
            }
            if (chkCkq.Checked && this.dataGridView_CKQ!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_CKQ, "�ɿ�Ȩ�������", "");
            }
            if (this.chkTkq.Checked && this.dataGridView_TKQ != null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_TKQ, "̽��Ȩ�������", "");
            }
            if (this.chkJsyd.Checked && this.dataGridView_JSYD!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_JSYD, "�����õط������", "");
            }
            if (this.chkKczygh.Checked && this.dataGridView_KCGH!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_KCGH, "�����Դ�ֲ�ͼ�������", "");

            }
            if (this.chkJbnt.Checked && this.dataGridView_JBNT!= null)
            {
                m_DataAccess_SYS_.OutputExcel(this.dataGridView_JBNT, "����ũ��������", "");
            }



        }

       







    }//class--end
}//namespace--end