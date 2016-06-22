using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using clsDataAccess;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Functions;
using System.IO;


namespace JCZF.SubFrame.AttibuteEdit
{
    public partial class frmSelAnalysis : DevComponents.DotNetBar.Office2007Form
    {
        public string m_strAnalysisLayerName;
        public bool m_blIsDrawPolygon;

        private bool[] m_blHasAnalysis;
        public clsDataAccess.DataAccess m_DataAccess_SYS;

        public IFeature m_selFeature;

        public IGeometry m_SelectedGeometry;

        //private frmMapView m_frmMapView = null;

        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl;

        private IEnumLayer m_Grouplayers;

        private UserProperyShow m_UserJBNTProperyShow;
        private UserProperyShow m_UserJSYDProperyShow;
        private UserProperyShow m_UserDLTBProperyShow;
        private UserProperyShow m_UserGHTProperyShow;
        private UserProperyShow m_UserTDGYProperyShow;
        private UserProperyShow m_UserKCZYProperyShow;
        private UserProperyShow m_UserCKQProperyShow;
        private UserProperyShow m_UserTKQProperyShow;

        System.Collections.ArrayList m_theButtonList = new System.Collections.ArrayList(); // ����7����ť(ԭ) ���� �޸� 2011 ����


        //CSQLOperation m_theSqlOp = new CSQLOperation(); // Sql��ѯ ���� �޸� 2011 ����
        //public clsDataAccess.DataAccess m_DataAccess_SYS;

        public frmSelAnalysis(ESRI.ArcGIS.Controls.AxMapControl p_AxMapControl)
        {
            InitializeComponent();
            m_AxMapControl = p_AxMapControl;
        }

        private void chkTdlyxz_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region ���� �޸� 2011 ����
        // �����ֶλ�ȡfeature���ֶ�ֵ
        private string GetOneFieldOftheFeatureWithFiledName(IFeature theFeature, string fieldName)
        {

            int index = theFeature.Fields.FindField(fieldName);

            if (index == -1)
                return null;

            return theFeature.get_Value(index).ToString();

        }


      
        // ��ȡ���غ˲��ͼ�߱��
        private string GetStaticsTBBH()
        {
            string m_strTBBH = "";
            m_strTBBH = GetOneFieldOftheFeatureWithFiledName(m_selFeature, "tbbhx");
            if (m_strTBBH == "")
            {
                m_strTBBH = GetOneFieldOftheFeatureWithFiledName(m_selFeature, "dkid");            
            }
            return m_strTBBH;
        }

        // ��\r\n�滻���������ܴ洢
        private string TransforToFitableSQL(string theString)
        {

            return theString.Replace("\r\n", "WYHH");

        }



        // ��WYHH�滻�����ָ�Ϊ\r\n
        private string ReTransforToFitableSQL(string theString)
        {

            return theString.Replace("WYHH", "\r\n");

        }



        // ���������������ݿ���
        private bool SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_type, string theResult, string theLayerNameOfStatics)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(p_type); // ����
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // ���йؼ�������
            string theID = GetStaticsTBBH(); // ���йؼ��ֵ�ֵ

            System.Collections.ArrayList theColNameList = new System.Collections.ArrayList(); // ������������

            foreach (string theColName in JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsResult) // ������������
            {
                theColNameList.Add(theColName);
            }

            System.Collections.ArrayList theColValList = new System.Collections.ArrayList();// ���������ݼ���
            theColValList.Add(theID); // ͼ�߱��
            theColValList.Add(theLayerNameOfStatics); // ͼ������
            theColValList.Add(TransforToFitableSQL(theResult)); // �������
            theColValList.Add(DateTime.Now); // ����ʱ��


            //return m_theSqlOp.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);
            return m_DataAccess_SYS.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);

        }

        // ����һ�����ݹ���һ���б�
        private void BuildOneListWithOneRow(DataRow theRow,ref System.Collections.ArrayList  theList)
        {
            ////string [] theDataTypeList = JCZF.Renderable.CGlobalVarable.GetFitalbeColDataTypeListOfStatisticsInfo (type)

            for (int i = 0; i < theRow.Table.Columns.Count ; i++)
            {
                theList.Add(theRow[i]);  
            }       

        }


        // ���������ϸ��Ϣ�����ݿ���
        private bool SaveStaticsInfoToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_type , DataTable  theInfoTable)
        {


            try
            {
                string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsInfo(p_type); // ����
                string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName; //��� ���йؼ�������
                string theTBBH = GetStaticsTBBH(); // ��ȡͼ�߱��

                System.Collections.ArrayList theColNameList = new System.Collections.ArrayList(); // ������������

                theColNameList.Add(theIDName); // �����

                string[] theColNameListString = JCZF.Renderable.CGlobalVarable.GetTableColNameListOfStatisticsInfo(p_type);

                foreach (string theColName in theColNameListString) // ������������
                {
                    theColNameList.Add(theColName);
                }

                System.Collections.ArrayList theColValList = new System.Collections.ArrayList();// ���������ݼ���


                //m_theSqlOp.DelAllCorrespondingRecordWithID(tableName, theIDName, theTBBH); // ɾ���������ID�ļ�¼
                m_DataAccess_SYS.DeleteByColumnValue(theIDName, theTBBH, tableName);

                for (int i = 0; i < theInfoTable.Rows.Count; i++) // ���д洢
                {

                    int index = i + 1;// ���ñ�ŵ����

                    theColValList.Clear();

                    string theID = String.Format("{0}_{1}", theTBBH, index); // ������
                    theColValList.Add(theID); // ��ӱ�� 
                    BuildOneListWithOneRow(theInfoTable.Rows[i], ref theColValList);   // ����һ�����ݹ���һ���б�

                    //m_theSqlOp.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);   // ��������
                    m_DataAccess_SYS.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);

                }


                return true;
            }
            catch(Exception ee)
            { return false; }
        }


        
        // �ж��Ƿ��Ѿ���ͳ��
        private bool HasBeenStatisticed(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_type )
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(p_type); // ����
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // ���йؼ�������
            string theID = GetStaticsTBBH(); // ���йؼ��ֵ�ֵ
            if (theID == "")
            {
                return false;
            }

            return m_DataAccess_SYS.IsRecordExist(theIDName, theID, tableName);//yuejianwei 20120314

            //return m_theSqlOp.ExistKeyIntheTable(tableName, theIDName, theID);

        }

        #endregion

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.chkCkq.Checked = true;
            this.chkGhsj.Checked = true;
            this.chkJbnt.Checked = true;
            this.chkJsyd.Checked = true;
            this.chkKczygh.Checked = true;
            this.chkTdlyxz.Checked = true;
            this.chkTkq.Checked = true;   
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Text = "";

                IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
                if (hcjg != null)
                {
                    m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);

                }

                this.progressBar1.Value = 10;

                this.txtResult.Text = "��ʼ�������㣡\r\n\r\n";

                //this.txtResult.Text = strTemp;

                clsClipStat m_clsClipStat = new clsClipStat();
                m_clsClipStat.m_strFilePath = Application.StartupPath + "\\OverlayTemp";
                m_clsClipStat.m_axMapcontrol = m_AxMapControl;
                //д�� ���غ˲�ͼ�� ����Ϊͨ�����Ա���ʱ��û��m_sellayer��������m_sellayerΪnull��
                if (m_strAnalysisLayerName == "")
                { m_strAnalysisLayerName = "���غ˲�"; }
            
                ILayer selLayer = MapFunction.getFeatureLayerByName(m_strAnalysisLayerName, m_AxMapControl);

                m_clsClipStat.m_selLayer = selLayer;
                m_clsClipStat.pDataAcess = m_DataAccess_SYS;
                if (m_SelectedGeometry == null)
                {
                    m_clsClipStat.m_Geometry = this.m_selFeature.ShapeCopy;
                    m_clsClipStat.CreateClipFeatureClass(this.m_selFeature.ShapeCopy);
                }
                else
                {
                    m_clsClipStat.m_Geometry = m_SelectedGeometry;
                    m_clsClipStat.CreateClipFeatureClass(m_SelectedGeometry);
                }
                this.progressBar1.Visible = true;

                try
                {
                    try
                    {

                        // ��״
                        if (chkTdlyxz.Checked)
                        {


                            if (m_blHasAnalysis[0] == true)
                            {
                                btnTDLY_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                    {
                                        break;
                                    }
                                }

                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;

                                AddtoTOC(m_clsClipStat.pDltbOutputFeatClass, "����������״clip");
                                loadDLTBProp(m_clsClipStat.pDltbOutputFeatClass);
                                this.progressBar1.Value = 30;

                                #region // ���� �޸� 2011 ����

                                string type = "����������״"; // ��������
                                string theResult = m_clsClipStat.strHCJG; ;// �������
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                // ��ȡͼ����
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);
                                
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ, theResult, theLayerName);// ����������


                                #endregion

                                //txtResult.Text = m_clsClipStat.strHCJG;
                                //MessageBox.Show(m_clsClipStat.strHCJG);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "����������״�����д�����������" + "\r\n";
                        //this.txtResult.Text =strTemp;
                    }
                    try
                    {
                        //�滮
                        if (chkGhsj.Checked)
                        {


                            if (m_blHasAnalysis[1] == true)
                            {
                                btnGH_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.QHTexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                    {
                                        break;
                                    }
                                }
                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;
                                AddtoTOC(m_clsClipStat.pGHTOutputFeatClass, "�滮����clip");
                                loadGHTProp(m_clsClipStat.pGHTOutputFeatClass);
                                this.progressBar1.Value = 50;

                                #region // ���� �޸� 2011 ����

                                string type = "�滮����"; // ��������
                                string theResult = m_clsClipStat.strHCJG; ;// �������
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                // ��ȡͼ����
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH, theResult, theLayerName);// ����������


                                #endregion
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "�������ù滮�����д�����������" + "\r\n";
                        //this.txtResult.Text = strTemp;
                    }

                    try
                    {
                        //���ع�Ӧ
                        if (chkGDSJ.Checked)
                        {


                            if (m_blHasAnalysis[2] == true)
                            {
                                btnGH_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.TDGYexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                    {
                                        break;
                                    }
                                }
                                this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                this.txtResult.Text += "----------------------------------------------------";
                                //this.txtResult.Text = strTemp;
                                AddtoTOC(m_clsClipStat.pTDGYOutputFeatClass, "���ع�Ӧclip");
                                loadGHTProp(m_clsClipStat.pTDGYOutputFeatClass);
                                this.progressBar1.Value = 50;

                                #region // ����ΰ �޸� 2011 ����

                                string type = "���ع�Ӧ"; // ��������
                                string theResult = m_clsClipStat.strHCJG; ;// �������
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                // ��ȡͼ����
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY, theResult, theLayerName);// ����������


                                #endregion
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "���ع�Ӧ�����д�����������" + "\r\n";
                        //this.txtResult.Text = strTemp;
                    }

                    try
                    {
                        //����ũ��
                        if (chkJbnt.Checked)
                        {


                            if (m_blHasAnalysis[3] == true)
                            {
                                btnJBNT_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.JBNTexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                    {
                                        break;
                                    }
                                }

                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;
                                AddtoTOC(m_clsClipStat.pJbntOutputFeatClass, "����ũ��clip");
                                loadJBNTProp(m_clsClipStat.pJbntOutputFeatClass);
                                this.progressBar1.Value = 60;

                                #region  ���� �޸� 2011 ����  �������

                                string type = "����ũ��"; // ��������
                                string theResult = m_clsClipStat.strHCJG; ;// �������
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                // ��ȡͼ����
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT, theResult, theLayerName);// ����������


                                #endregion
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "����ũ�������д�����������" + "\r\n";
                        //this.txtResult.Text = strTemp ;
                    }
                    try
                    {
                        //������Ŀ
                        if (chkJsyd.Checked)
                        {

                            if (m_blHasAnalysis[4] == true)
                            {
                                btnGH_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.JSYDexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                    {
                                        break;
                                    }
                                }
                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;
                                if (m_clsClipStat.pJsydOutputFeatClass != null)
                                {
                                    AddtoTOC(m_clsClipStat.pJsydOutputFeatClass, "�����õ�����clip");
                                    loadJSYDProp(m_clsClipStat.pJsydOutputFeatClass);
                                }
                               
                                this.progressBar1.Value = 70;

                                #region  ���� �޸� 2011 ����

                                string type = "�����õ�����"; // ��������
                                string theResult = m_clsClipStat.strHCJG; ;// �������
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                // ��ȡͼ����
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD, theResult, theLayerName);// ����������


                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "������Ŀ���������д�����������" + "\r\n";
                        //this.txtResult.Text = strTemp ;
                    }
                    try
                    {
                        //�����Դ�滮
                        if (chkKczygh.Checked)
                        {
                            if (m_blHasAnalysis[5] == true)
                            {
                                btnKCZY_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.KCZYexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                    {
                                        break;
                                    }
                                }

                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;

                                AddtoTOC(m_clsClipStat.pKczyOutputFeatClass, "�����Դ�滮clip");
                                loadKczyProp(m_clsClipStat.pKczyOutputFeatClass);
                                this.progressBar1.Value = 80;


                                #region  ���� �޸� 2011 ����

                                string type = "�����Դ�滮"; // ��������
                                string theResult = m_clsClipStat.strHCJG; ;// �������
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                // ��ȡͼ����
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH);
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH, theResult, theLayerName);// ����������


                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "�����Դ�滮�����д�����������" + "\r\n";
                        //this.txtResult.Text = strTemp ;
                    }
                    //�ɿ�
                    try
                    {
                        if (chkCkq.Checked)
                        {
                            if (m_blHasAnalysis[6] == true)
                            {
                                btnCKQ_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.CKQexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                    {
                                        break;
                                    }
                                }

                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;

                                AddtoTOC(m_clsClipStat.pCkqOutputFeatClass, "�ɿ�Ȩclip");
                                loadCKQProp(m_clsClipStat.pCkqOutputFeatClass);
                                this.progressBar1.Value = 90;

                                #region  ���� �޸� 2011 ����

                                string type = "�ɿ�Ȩ"; // ��������
                                string theResult = m_clsClipStat.strHCJG; ;// �������
                                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                // ��ȡͼ����
                                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ );
                                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ, theResult, theLayerName);// ����������


                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "�ɿ�Ȩ�Ǽ������д�����������" + "\r\n";
                        //this.txtResult.Text = strTemp;
                    }
                    try
                    {
                        //̽��
                        if (chkTkq.Checked)
                        {
                            if (m_blHasAnalysis[7] == true)
                            {
                                btnTKQ_Click(sender, e);
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    m_clsClipStat.TKQexecute();// Geoexecute();
                                    if (m_clsClipStat.strHCJG.IndexOf("ƽ����") >= 0)
                                    {
                                        break;
                                    }
                                }
                                this.txtResult.Text += m_clsClipStat.strHCJG;
                                this.txtResult.Text += "--------------------------------------------------------------";
                                this.txtResult.Text += "\r\n\r\n";
                                //this.txtResult.Text = strTemp;

                                AddtoTOC(m_clsClipStat.pTkqOutputFeatClass,"̽��Ȩclip");
                                loadTKQProp(m_clsClipStat.pTkqOutputFeatClass);
                                this.progressBar1.Value = 100;
                            }

                            //this.txtResult.Text = strTemp;
                            this.progressBar1.Value = 100;

                            #region  ���� �޸� 2011 ����

                            string type = "̽��Ȩ"; // ��������
                            string theResult = m_clsClipStat.strHCJG; ;// �������
                            string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                            // ��ȡͼ����
                            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ);
                            JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                            SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ, theResult, theLayerName);// ����������


                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtResult.Text += "\r\n" + "--------------------------------------------------------------" + "\r\n" + "̽��Ȩ�Ǽ������д�����������";
                        //this.txtResult.Text = strTemp ;
                    }
                }
                catch (Exception o)
                { }
                this.progressBar1.Visible = false;
            }
            catch (Exception o)
            { }
        }


        //private void Analysis()
        //{
        //    txtResult.Text = "";

        //    IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
        //    if (hcjg != null)
        //    {
        //        m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);

        //    }

        //    this.progressBar1.Value = 10;

        //    this.txtResult.Text = "��ʼ�������㣡\r\n";

        //    //this.txtResult.Text = strTemp;

        //    clsClipStat m_clsClipStat = new clsClipStat();
        //    m_clsClipStat.m_strFilePath = Application.StartupPath + "\\OverlayTemp";
        //    m_clsClipStat.m_axMapcontrol = m_AxMapControl;
        //    //д�� ���غ˲�ͼ�� ����Ϊͨ�����Ա���ʱ��û��m_sellayer��������m_sellayerΪnull��

        //    ILayer selLayer = MapFunction.getFeatureLayerByName("���غ˲�", m_AxMapControl);

        //    m_clsClipStat.m_selLayer = selLayer;
        //    m_clsClipStat.pDataAcess = m_DataAccess_SYS;
        //    m_clsClipStat.m_Geometry = this.m_selFeature.ShapeCopy;
        //    m_clsClipStat.CreateClipFeatureClass(this.m_selFeature.ShapeCopy);
        //    this.progressBar1.Visible = true;

        //    try
        //    {
        //        try
        //        {

        //            // ��״
        //            if (chkTdlyxz.Checked)
        //            {


        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnTDLY_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pDltbOutputFeatClass);
        //                    loadDLTBProp(m_clsClipStat.pDltbOutputFeatClass);
        //                    this.progressBar1.Value = 30;

        //                    #region // ���� �޸� 2011 ����

        //                    string type = "����������״"; // ��������
        //                    string theResult = m_clsClipStat.strHCJG; ;// �������
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
        //                    // ��ȡͼ����
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// ����������


        //                    #endregion

        //                    //txtResult.Text = m_clsClipStat.strHCJG;
        //                    //MessageBox.Show(m_clsClipStat.strHCJG);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "����������״�����д�����������" + "\r\n";
        //            //this.txtResult.Text =strTemp;
        //        }
        //        try
        //        {
        //            //�滮
        //            if (chkGhsj.Checked)
        //            {


        //                if (m_blHasAnalysis[1] == true)
        //                {
        //                    btnGH_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.QHTexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;
        //                    AddtoTOC(m_clsClipStat.pGHTOutputFeatClass);
        //                    loadGHTProp(m_clsClipStat.pGHTOutputFeatClass);
        //                    this.progressBar1.Value = 50;

        //                    #region // ���� �޸� 2011 ����

        //                    string type = "�滮����"; // ��������
        //                    string theResult = m_clsClipStat.strHCJG; ;// �������
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
        //                    // ��ȡͼ����
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// ����������


        //                    #endregion
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "�������ù滮�����д�����������" + "\r\n";
        //            //this.txtResult.Text = strTemp;
        //        }
        //        try
        //        {
        //            //����ũ��
        //            if (chkJbnt.Checked)
        //            {


        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnJBNT_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.JBNTexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;
        //                    AddtoTOC(m_clsClipStat.pJbntOutputFeatClass);
        //                    loadJBNTProp(m_clsClipStat.pJbntOutputFeatClass);
        //                    this.progressBar1.Value = 60;

        //                    #region  ���� �޸� 2011 ����  �������

        //                    string type = "����ũ��"; // ��������
        //                    string theResult = m_clsClipStat.strHCJG; ;// �������
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
        //                    // ��ȡͼ����
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// ����������


        //                    #endregion
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "����ũ�������д�����������" + "\r\n";
        //            //this.txtResult.Text = strTemp ;
        //        }
        //        try
        //        {
        //            //������Ŀ
        //            if (chkJsyd.Checked)
        //            {

        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnGH_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.JSYDexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pJsydOutputFeatClass);
        //                    loadJSYDProp(m_clsClipStat.pJsydOutputFeatClass);
        //                    this.progressBar1.Value = 70;

        //                    #region  ���� �޸� 2011 ����

        //                    string type = "�����õ�����"; // ��������
        //                    string theResult = m_clsClipStat.strHCJG; ;// �������
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
        //                    // ��ȡͼ����
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// ����������


        //                    #endregion
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "������Ŀ���������д�����������" + "\r\n";
        //            //this.txtResult.Text = strTemp ;
        //        }
        //        try
        //        {
        //            //�����Դ�滮
        //            if (chkKczygh.Checked)
        //            {
        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnKCZY_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.KCZYexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pKczyOutputFeatClass);
        //                    loadKczyProp(m_clsClipStat.pKczyOutputFeatClass);
        //                    this.progressBar1.Value = 80;


        //                    #region  ���� �޸� 2011 ����

        //                    string type = "�����Դ�滮"; // ��������
        //                    string theResult = m_clsClipStat.strHCJG; ;// �������
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
        //                    // ��ȡͼ����
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// ����������


        //                    #endregion
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "�����Դ�滮�����д�����������" + "\r\n";
        //            //this.txtResult.Text = strTemp ;
        //        }
        //        //�ɿ�
        //        try
        //        {
        //            if (chkCkq.Checked)
        //            {
        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnCKQ_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.CKQexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pCkqOutputFeatClass);
        //                    loadCKQProp(m_clsClipStat.pCkqOutputFeatClass);
        //                    this.progressBar1.Value = 90;

        //                    #region  ���� �޸� 2011 ����

        //                    string type = "�ɿ�Ȩ"; // ��������
        //                    string theResult = m_clsClipStat.strHCJG; ;// �������
        //                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
        //                    // ��ȡͼ����
        //                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                    SaveStaticsResultToSQL(type, theResult, theLayerName);// ����������


        //                    #endregion
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "�ɿ�Ȩ�Ǽ������д�����������" + "\r\n";
        //            //this.txtResult.Text = strTemp;
        //        }
        //        try
        //        {
        //            //̽��
        //            if (chkTkq.Checked)
        //            {
        //                if (m_blHasAnalysis[0] == true)
        //                {
        //                    btnTKQ_Click(sender, e);
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        m_clsClipStat.TKQexecute();// Geoexecute();
        //                        if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    this.txtResult.Text += m_clsClipStat.strHCJG;
        //                    this.txtResult.Text += "--------------------------------------------------------------";
        //                    //this.txtResult.Text = strTemp;

        //                    AddtoTOC(m_clsClipStat.pTkqOutputFeatClass);
        //                    loadTKQProp(m_clsClipStat.pTkqOutputFeatClass);
        //                    this.progressBar1.Value = 100;
        //                }

        //                //this.txtResult.Text = strTemp;
        //                this.progressBar1.Value = 100;

        //                #region  ���� �޸� 2011 ����

        //                string type = "̽��Ȩ"; // ��������
        //                string theResult = m_clsClipStat.strHCJG; ;// �������
        //                string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
        //                // ��ȡͼ����
        //                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
        //                JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

        //                SaveStaticsResultToSQL(type, theResult, theLayerName);// ����������


        //                #endregion
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.txtResult.Text += "--------------------------------------------------------------" + "\r\n" + "̽��Ȩ�Ǽ������д�����������";
        //            //this.txtResult.Text = strTemp ;
        //        }
        //    }
        //    catch (Exception o)
        //    { }
        //    this.progressBar1.Visible = false;
        //}


        #region ��ӵ�toc  ����20110814
        private void AddtoTOC(IFeatureClass outputfeatclass, string layername)
        {
            IFeatureLayer outlayer = new FeatureLayer() as IFeatureLayer;
            outlayer.FeatureClass = outputfeatclass;
            outlayer.Name = layername;

            IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
            IGroupLayer hcjg1 = new GroupLayer() as IGroupLayer;

            if (hcjg != null)
            {
                hcjg.Add((ILayer)outlayer);
                m_AxMapControl.Map.AddLayer(outlayer);
                m_AxMapControl.Map.DeleteLayer(outlayer);

            }
            else
            {
                //IGroupLayer hcjg = new GroupLayer() as IGroupLayer;
                hcjg1.SpatialReference = m_AxMapControl.SpatialReference;
                hcjg1.Name = "�˲���";
                hcjg1.Add((ILayer)outlayer);
                m_AxMapControl.Map.AddLayer((ILayer)hcjg1);
            }
        }

        private IGroupLayer GetHcjggrouplayer(string grouplayername)
        {
            IGroupLayer ResGrouplayer = null;
            this.m_Grouplayers = Functions.MapFunction.GetGroupLayers(m_AxMapControl.ActiveView.FocusMap);
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
        #endregion

        #region ������ϸ��Ϣ

        private void loadJBNTProp(IFeatureClass m_FeatureClass)
        {
            m_UserJBNTProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserJBNTProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ���� �������
            m_UserJBNTProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT; // ����ũ�� �������
            m_UserJBNTProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ���� �������

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "����ũ��clip";

            // ��ȡͼ���� ����20110814
            string type = "����ũ��";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserJBNTProperyShow.SetData(m_layer as ILayer);

            m_UserJBNTProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            //this.gbJBNT.Controls.Add(m_UserJBNTProperyShow);
            m_UserJBNTProperyShow.Dock = DockStyle.Fill;
            this.tabPage3.Controls.Clear();
            this.tabPage3.Controls.Add(m_UserJBNTProperyShow);
        }
        void m_frmJBNTHCResult_FlashFeature(ESRI.ArcGIS.Geodatabase.IFeature pFeature)
        {
            m_AxMapControl.FlashShape(pFeature.Shape, 3, 300, null);
        }

        private void loadJSYDProp(IFeatureClass m_FeatureClass)
        {
            m_UserJSYDProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserJSYDProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserJSYDProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD; // �����õ����� 
            m_UserJSYDProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ���� 

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "�����õ�����clip";

            // ��ȡͼ���� ����20110814
            string type = "�����õ�����";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserJSYDProperyShow.SetData(m_layer as ILayer);
            m_UserJSYDProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserJSYDProperyShow.Dock = DockStyle.Fill;
            this.tabPage4.Controls.Clear();
            this.tabPage4.Controls.Add(m_UserJSYDProperyShow);
        }

        private void loadDLTBProp(IFeatureClass m_FeatureClass)
        {
            m_UserDLTBProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserDLTBProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserDLTBProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ; // �������� ���� �޸� 2011 ����
            m_UserDLTBProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ����

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "����������״clip";

            // ��ȡͼ���� ����20110814
            string type = "����������״";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;


            m_UserDLTBProperyShow.SetData(m_layer as ILayer);
            m_UserDLTBProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserDLTBProperyShow.Dock = DockStyle.Fill;
            this.tabPage1.Controls.Clear();
            this.tabPage1.Controls.Add(m_UserDLTBProperyShow);
        }

        private void loadGHTProp(IFeatureClass m_FeatureClass)
        {
            m_UserGHTProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);


            m_UserGHTProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserGHTProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH; // �滮���� ���� �޸� 2011 ����
            m_UserGHTProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ����

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "�滮����clip";
            m_UserGHTProperyShow.SetData(m_layer as ILayer);
            m_UserGHTProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserGHTProperyShow.Dock = DockStyle.Fill;
            this.tabPage2.Controls.Clear();
            this.tabPage2.Controls.Add(m_UserGHTProperyShow);


        }

        private void loadTDGYProp(IFeatureClass m_FeatureClass)
        {
            m_UserTDGYProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);


            m_UserTDGYProperyShow.m_theDataAccess = m_DataAccess_SYS; // ����ΰ �޸� 2011 ����
            m_UserTDGYProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY; // ���ع�Ӧ���� ����ΰ�޸� 2011 ����
            m_UserTDGYProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ����

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "���ع�Ӧclip";
            m_UserTDGYProperyShow.SetData(m_layer as ILayer);
            m_UserTDGYProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserTDGYProperyShow.Dock = DockStyle.Fill;
            this.tabPage8.Controls.Clear();
            this.tabPage8.Controls.Add(m_UserTDGYProperyShow);


        }

        private void loadKczyProp(IFeatureClass m_FeatureClass)
        {
            m_UserKCZYProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);


            m_UserKCZYProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserKCZYProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH; // �����Դ�滮 
            m_UserKCZYProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ���� 


            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "�����Դ�滮clip";
            m_UserKCZYProperyShow.SetData(m_layer as ILayer);
            m_UserKCZYProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserKCZYProperyShow.Dock = DockStyle.Fill;
            this.tabPage5.Controls.Clear();
            this.tabPage5.Controls.Add(m_UserKCZYProperyShow);
        }

        private void loadCKQProp(IFeatureClass m_FeatureClass)
        {
            m_UserCKQProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserCKQProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserCKQProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ; // �ɿ�Ȩ 
            m_UserCKQProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ���� 

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "�ɿ�Ȩclip";
            m_UserCKQProperyShow.SetData(m_layer as ILayer);
            m_UserCKQProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserCKQProperyShow.Dock = DockStyle.Fill;
            this.tabPage6.Controls.Clear();
            this.tabPage6.Controls.Add(m_UserCKQProperyShow);
        }
        private void loadTKQProp(IFeatureClass m_FeatureClass)
        {
            m_UserTKQProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            m_UserTKQProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ���� 
            m_UserTKQProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ; // ̽��Ȩ 
            m_UserTKQProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ����


            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "̽��Ȩclip";
            m_UserTKQProperyShow.SetData(m_layer as ILayer);
            m_UserTKQProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            m_UserTKQProperyShow.Dock = DockStyle.Fill;
            this.tabPage7.Controls.Clear();
            this.tabPage7.Controls.Add(m_UserTKQProperyShow);
        }
        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //m_DataAccess_SYS.OutputExcel(m_ResultDataGrid, "���غ˲�ռ�������", "");
                if (chkTdlyxz.Checked && this.m_UserDLTBProperyShow.m_PropertyDataGrid != null)
                {
                    //m_DataAccess_SYS.OutputExcel(this.m_UserDLTBProperyShow.m_PropertyDataGrid
                    this.m_DataAccess_SYS.OutputExcel(this.m_UserDLTBProperyShow.m_PropertyDataGrid, "��״����������", "");

                }
                if (this.chkGhsj.Checked && this.m_UserGHTProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserGHTProperyShow.m_PropertyDataGrid, "�滮ͼ�������", "");
                }
                if (chkCkq.Checked && this.m_UserCKQProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserCKQProperyShow.m_PropertyDataGrid, "�ɿ�Ȩ�������", "");
                }
                if (this.chkTkq.Checked && this.m_UserTKQProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserTKQProperyShow.m_PropertyDataGrid, "̽��Ȩ�������", "");
                }
                if (this.chkJsyd.Checked && this.m_UserJSYDProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserJSYDProperyShow.m_PropertyDataGrid, "�����õط������", "");
                }
                if (this.chkKczygh.Checked && this.m_UserKCZYProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserKCZYProperyShow.m_PropertyDataGrid, "�����Դ�ֲ�ͼ�������", "");

                }
                if (this.chkJbnt.Checked && this.m_UserJBNTProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserJBNTProperyShow.m_PropertyDataGrid, "����ũ��������", "");
                }
            }
            catch (Exception ex)
            { }
           
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            this.Width = 839;
            this.Height = 502;
            this.btnNoDetail.Enabled = true;
            this.btnDetail.Enabled = false;
        }

        private void btnNoDetail_Click(object sender, EventArgs e)
        {
            this.Width = 422;
            this.Height = 502;
            this.btnDetail.Enabled = true;
            this.btnNoDetail.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
                if (hcjg != null)
                {
                    m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);

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

        private void frmSelAnalysis_Load(object sender, EventArgs e)
        {
            JCZF.SubFrame.DatabaseString.m_DataAccess_SYS = this.m_DataAccess_SYS; //  ��ȡ���ݿ��������� ���� �޸� 2011 ����

            m_blHasAnalysis = new bool[8];
            //m_theButtonList.Clear();
            //m_theButtonList.Add(this.btnTDLY); // ��ӡ�ԭ����ť����������
            //m_theButtonList.Add(this.btnGH); // ��ӡ�ԭ����ť���滮����
            //m_theButtonList.Add(this.btnJBNT); // ��ӡ�ԭ����ť������ũ��
            //m_theButtonList.Add(this.btnJSYD); // ��ӡ�ԭ����ť�������õ�
            //m_theButtonList.Add(this.btnKCZY); // ��ӡ�ԭ����ť�������Դ
            //m_theButtonList.Add(this.btnCKQ); // ��ӡ�ԭ����ť���ɿ�Ȩ
            //m_theButtonList.Add(this.btnTKQ); // ��ӡ�ԭ����ť��̽��Ȩ

            if (m_blIsDrawPolygon == false)
            {
                // ��ʾ��ͳ�ƹ��İ�ť��ԭ��
                //ShowHasBeenStatisticedButton();
            }
        }

        #region ���� �޸� 2011 ����

        // ������ϸ��Ϣ������ʾ
        private void SetDetailFormShow()
        {
            this.Width = 839;
            this.Height = 502;
            this.btnNoDetail.Enabled = true;
            this.btnDetail.Enabled = false;
        }

        // ����textbox������������
        private void SetTextBoxScrollToBottom()
        {
            this.txtResult.SelectionStart = this.txtResult.Text.Length;
            this.txtResult.ScrollToCaret();

        }

        // ��ʾ��ͳ�ƹ��İ�ť��ԭ��
        //private void ShowHasBeenStatisticedButton()
        //{

        //    int i = 0;
        //    foreach (string theType in JCZF.Renderable.CGlobalVarable.m_listStaticsContents) // 8�ַ�������ѭ��
        //    {
               
        //         int index = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(theType);
        //         //Button theButton = m_theButtonList[index] as Button;
        //         bool bStatisticed = this.HasBeenStatisticed(theType); // �ж��Ƿ�ͳ�ƹ�
        //         //theButton.Visible = bStatisticed; // ͳ�ƹ�����ʾ��������ʾ

        //         m_blHasAnalysis[i] = bStatisticed;

        //         //if (bStatisticed) // Ϊ���ж��м��������
        //         //    i++;
        //    }

        //    //if (i == 7) // ������ʾ
        //    //{
        //    //    this.Text = String.Format("ͼ�߷���-ȫ����Ŀ�Ѿ�������");
        //    //}
        //    //else
        //    //    this.Text = String.Format("ͼ�߷���-����{0}��������", i);
        //}

        // ����ͳ�����ͻ�ȡͳ�ƽ������
        private DataRow GetStatisticResultData(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_type)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(p_type); // ����
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // ���йؼ�������
            string theID = GetStaticsTBBH(); // ���йؼ��ֵ�ֵ

            return m_DataAccess_SYS.GetRowByColmn(theIDName, theID, tableName);
            //return this.m_theSqlOp.GetOneRow(tableName, theIDName, theID);
        }

        // ��ʾͳ�ƽ��
        private void ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type)
        {
            DataRow theRow = GetStatisticResultData(type); // ����ͳ�����ͻ�ȡͳ�ƽ������
            string theTime = theRow[3].ToString(); // ����ʱ��
            string theResultShow = String.Format("�Ѻ˲���Ϊ��(����ʱ��({0}))", theTime);
            theResultShow = theResultShow + "\r\n";
            theResultShow = theResultShow + "\r\n";
            string theResult = ReTransforToFitableSQL(theRow[2].ToString().Trim()); // �������
            theResultShow += theResult;
            theResultShow += "----------------------------------------------------------" + "\r\n";
            theResultShow = theResultShow + "\r\n";
            // ��ȡͼ������
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type);
            JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theRow[1].ToString().Trim(); // ��ȡͼ������
                        
            this.txtResult.Text += theResultShow;

            SetTextBoxScrollToBottom();  // ������ʾ�����textbox������������
            SetDetailFormShow(); // ������ʾ��ϸ����
        }

        // ��ʾͳ����ϸ��Ϣ
        private void ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type, UserProperyShow theShowControl)
        {
            theShowControl = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);

            theShowControl.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            theShowControl.m_theType = type; // �������� ���� �޸� 2011 ����

            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsInfo(type); // ����
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName; // ���йؼ�������
            string theID = GetStaticsTBBH(); // ���йؼ��ֵ�ֵ
            string theSQL = String.Format("Select * from {0} Where [{1}] like '{2}_%' ", tableName, theIDName, theID);
            //DataTable theData = this.m_theSqlOp.GetTableWithstrQuery(theSQL);// ��ȡ����
            DataTable theData = m_DataAccess_SYS.getDataTableByQueryString(theSQL);

            theShowControl.SetDataAfterStatisticed(theData);
            theShowControl.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            theShowControl.Dock = DockStyle.Fill;
            
            int index = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type); // ��ȡ����
            int tabIndex = JCZF.Renderable.CGlobalVarable.m_listTabIndexOfStaticsInfo[index]; // ��ȡtab������Ŀǰ��һ�µ���index
            this.tabControl1.TabPages[tabIndex].Controls.Clear();
            this.tabControl1.TabPages[tabIndex].Controls.Add(theShowControl);
            this.tabControl1.SelectedIndex = tabIndex;
       
        }

        // ��ť��ԭ-��������
        private void btnTDLY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[0]; // ����������״
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);   // ��ʾͳ�ƽ��
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ, m_UserDLTBProperyShow);  // ��ʾͳ����ϸ��Ϣ
        }

        // ��ť��ԭ-�滮����
        private void btnGH_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[1]; // �滮����
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);   // ��ʾͳ�ƽ��
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH, m_UserGHTProperyShow);  // ��ʾͳ����ϸ��Ϣ

        }



        // ��ť��ԭ-����ũ��
        private void btnJBNT_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[2]; // ����ũ�� �������
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);   // ��ʾͳ�ƽ�� �������
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT, m_UserJBNTProperyShow);  // ��ʾͳ����ϸ��Ϣ �������
        }


        // ��ť��ԭ-�����õ�
        private void btnJSYD_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[3]; // �����õ� 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);   // ��ʾͳ�ƽ�� 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD, m_UserJSYDProperyShow);  // ��ʾͳ����ϸ��Ϣ 

        }


        // ��ť��ԭ-�����Դ
        private void btnKCZY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[4]; // �����Դ 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH);   // ��ʾͳ�ƽ�� 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH , m_UserKCZYProperyShow);  // ��ʾͳ����ϸ��Ϣ 
        }


        // ��ť��ԭ-�ɿ�Ȩ
        private void btnCKQ_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[5]; // �ɿ�Ȩ 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ );   // ��ʾͳ�ƽ�� 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ , m_UserCKQProperyShow);  // ��ʾͳ����ϸ��Ϣ 
        }



        // ��ť��ԭ-̽��Ȩ
        private void btnTKQ_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[6]; // ̽��Ȩ 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ);   // ��ʾͳ�ƽ�� 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ , m_UserTKQProperyShow);  // ��ʾͳ����ϸ��Ϣ 
        }


        #endregion

    }
}