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

using System.Collections;
using JCZF.Renderable;

namespace JCZF.SubFrame
{
    public partial class frmSelAnalysis_hc : DevComponents.DotNetBar.Office2007Form
    {
        ////�ؿ�ѡ��ʽ
        //public bool b_hcselectDK;
        //public bool b_hcDrawDK;
        //public bool b_hcImportDK;

        private frmMapView m_frmMapView;
        public string m_strDKID;
        public bool m_blIsDrawPolygon;

        private bool[] m_blHasAnalysis;
        public clsDataAccess.DataAccess m_DataAccess_SYS;

         private  IFeature m_selFeature_;
         public IFeature m_selFeature
         {
             set
             {
                 m_selFeature_ = value;
                 FillHCListview(m_selFeature_.ShapeCopy);
             }
         }

        public IGeometry m_SelectedGeometry;

        //private frmMapView m_frmMapView = null;

        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl;

        private IEnumLayer m_Grouplayers;

        private UserProperyShow m_UserJBNTProperyShow;
        private UserProperyShow m_UserJSYDProperyShow;
        private UserProperyShow m_UserTDLYXZProperyShow;
        private UserProperyShow m_UserGHTProperyShow;
        private UserProperyShow m_UserTDGYProperyShow;
        private UserProperyShow m_UserKCZYProperyShow;
        private UserProperyShow m_UserCKQProperyShow;
        private UserProperyShow m_UserTKQProperyShow;

        System.Collections.ArrayList m_theButtonList = new System.Collections.ArrayList(); // ����7����ť(ԭ) ���� �޸� 2011 ����


        //CSQLOperation m_theSqlOp = new CSQLOperation(); // Sql��ѯ ���� �޸� 2011 ����
        //public clsDataAccess.DataAccess m_DataAccess_SYS;

        IFeature m_theOriginFeature = null; // ���� �޸� 2011���� ���� ԭʼͼ��Ϊ�˱���ԭʼͼ��

        public delegate void EventHandlerGetDkClick();
        public event EventHandlerGetDkClick GetDKClick;

        public frmSelAnalysis_hc(ESRI.ArcGIS.Controls.AxMapControl p_AxMapControl, frmMapView parentview)
        {
            InitializeComponent();
            m_AxMapControl = p_AxMapControl;
            m_frmMapView = parentview;
        }


        /// <summary>
        /// ��ȡ��ǰ�����Ľ��
        /// </summary>
        public void  ReadFromResult()
        {
            if (m_strDKID.Trim() != "" && m_strDKID.Trim() != null && m_DataAccess_SYS!=null )
            {
                //�Ƚ�������checkbox ���
                chkCkq.Checked = false; chkJbnt.Checked = false; chkJsyd.Checked = false; chkKczygh.Checked = false; 
                chkTDGY.Checked = false; chkTDLYGH.Checked = false; chkTdlyxz.Checked = false; chkTkq.Checked = false;

                ///////////////////////////

                bool m_blTDLYXZ = false;
                bool m_blTDLYGH = false;
                bool m_blTKQ = false;
                bool m_blCKQ = false;
                bool m_blKCZYGH = false;
                bool m_blJSYD = false;
                bool m_blJBNT = false;
                bool m_blTDGY = false;

                string m_strSQL="SELECT * FROM  ";
                
                //����������״�������
                //ReadFromResult_TDLYXZ(m_strSQL);
                m_blTDLYXZ = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ, m_UserTDLYXZProperyShow, chkTdlyxz, tabItemTDXZ, tabControlPanelTDXZ);
               //����ũ��
                m_blJBNT = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT, m_UserJBNTProperyShow, chkJbnt, tabItemJBNT, tabControlPanelJBNT);

                //�����õ�
                m_blJSYD = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD, m_UserJSYDProperyShow, chkJsyd, tabItemJSYD, tabControlPanelJSYD);
                //���ع�Ӧ
                m_blTDGY = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY, m_UserTDGYProperyShow, chkTDGY, tabItemTDGY, tabControlPanelTDGY);
                //�������ù滮
                m_blTDLYGH = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH, m_UserTDGYProperyShow, chkTDLYGH, tabItemTDGH, tabControlPanelTDGH);
                //�ɿ�Ȩ
                m_blCKQ = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ, m_UserCKQProperyShow, chkCkq, tabItemCKQ, tabControlPanelCKQ);
                //̽��Ȩ
                m_blTKQ = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ, m_UserTKQProperyShow, chkTkq, tabItemTKQ, tabControlPanelTKQ);
                //�����Դ�滮
                m_blKCZYGH = ReadFromResult(m_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH, m_UserKCZYProperyShow, chkKczygh, tabItemKCZYGH, tabControlPanelKCZYGH);

                if (m_blTDLYXZ == false && m_blJBNT == false && m_blJSYD == false && m_blTDGY == false && m_blTDLYGH == false && m_blCKQ == false && m_blTKQ == false && m_blKCZYGH == false )
                {
                    //û��һ�������ݿ��ж��������ͽ�������check����ΪĬ��״̬
                    chkCkq.Checked = true; chkJbnt.Checked = true; chkJsyd.Checked = true; chkKczygh.Checked = false;
                    chkTDGY.Checked = true; chkTDLYGH.Checked = false; chkTdlyxz.Checked = false; chkTkq.Checked = true;
                }

                SetTabItemUnVisible();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strSQL"></param>
        /// <param name="p_Enum_AnalysisType"></param>
        /// <returns></returns>
        private bool ReadFromResult(string p_strSQL, JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_Enum_AnalysisType, UserProperyShow p_UserProperyShow, CheckBox p_CheckBox, DevComponents.DotNetBar.TabItem p_TabItem, DevComponents.DotNetBar.TabControlPanel p_TabControlPanel)
        {
            try
            {
                string m_strSQL = "";

                //��ȡ�������
                m_strSQL = p_strSQL + JCZF.Renderable.CGlobalVarable.m_listTableNameOfStaticsResult[(int)p_Enum_AnalysisType];
                m_strSQL = m_strSQL + " WHERE " + JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName + "='" + m_strDKID + "'";
                DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);

                if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
                {
                    p_CheckBox.Checked = true;
                    txtResult.Text = txtResult.Text + "\r\n\r\n" + m_DataRowCollection[0]["FXJG"].ToString().Replace("WYHH", "\r\n");
                    this.txtResult.Text += "----------------------------------------------------";
                }
                else
                {
                    return false ;
                }

                //��ȡ���������ϸ����

                m_strSQL =  p_strSQL + JCZF.Renderable.CGlobalVarable.m_listTableNameOfStaticsInfo[(int)p_Enum_AnalysisType];
                m_strSQL = m_strSQL + " WHERE " + JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName + "='" + m_strDKID + "'";
                DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);

                if (m_DataTable != null && m_DataTable.Rows.Count > 0)
                {
                   TransColmnsNameToChinese(p_Enum_AnalysisType,m_DataTable);
                    p_CheckBox.Checked = true;

                    if (p_UserProperyShow == null)
                    {
                        p_UserProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
                    }
                    p_UserProperyShow.ShowDataFromDatabase(m_DataTable);


                    p_TabControlPanel.Controls.Clear();
                    p_TabControlPanel.Controls.Add(p_UserProperyShow);
                    p_UserProperyShow.Dock = DockStyle.Fill;
                    tabControl1.SelectedTab = p_TabItem;
                    tabControl1.Refresh();
                }
            }
            catch (SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
                return false;
            }

            return true;

        }

        private bool TransColmnsNameToChinese(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_Enum_AnalysisType, DataTable p_DataTable)
        {
            switch (p_Enum_AnalysisType)
            {
                case CGlobalVarable.Enum_AnalysisType.TDLYXZ: // �������ͣ�����������״
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.TDLYGH:  // �������ͣ��滮����
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.TDGY:  // �������ͣ����ع�Ӧ����
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.JBNT:   // �������ͣ�����ũ��
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInJBNTTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInJBNTTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.JSYD:   // �������ͣ������õ�����
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInJSYDTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInJSYDTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.KCZYGH:   // �������ͣ������Դ�滮
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.CKQ:   // �������ͣ��ɿ�Ȩ
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
                case CGlobalVarable.Enum_AnalysisType.TKQ:   // �������ͣ�̽��Ȩ
                    return TransColmnsNameToChinese(p_DataTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable, JCZF.Renderable.CGlobalVarable.m_listColNameOfStaticsInfoInTDLYTable_Chinese);
            }
            return true;
        }

        private bool  TransColmnsNameToChinese(DataTable p_DataTable, string[] p_DataTableColmnsEnglish, string[] p_DataTableColmnsChinese)
        {
            for (int i = 0; i < p_DataTable.Columns.Count; i++)
            {
                if (p_DataTable.Columns[i].ColumnName.Trim().ToUpper() == "DKID" )
                {
                    p_DataTable.Columns[i].ColumnName ="�ؿ���";
                }
                else if (p_DataTable.Columns[i].ColumnName.Trim().ToUpper() == "BZ")
                {
                    p_DataTable.Columns[i].ColumnName = "��ע";
                }
                else if (p_DataTable.Columns[i].ColumnName.Trim().ToUpper() == "SSTCMC")
                {
                    p_DataTable.Columns[i].ColumnName = "����ͼ������";
                }
                else {

                    for (int j = 0; j < p_DataTableColmnsEnglish.Length - 1; j++)
                    {
                        if (p_DataTable.Columns[i].ColumnName.Trim() == p_DataTableColmnsEnglish[j].Trim())
                        {
                            p_DataTable.Columns[i].ColumnName = p_DataTableColmnsChinese[j].Trim();
                        }
                    }

                }
                

            }

            return true;
        }

        private  void ReadFromResult_TDLYXZ(string p_strSQL)
        {
            //try
            //{
            //    string m_strSQL = "";

            //    //��ȡ�������
            //    m_strSQL = p_strSQL + JCZF.Renderable.CGlobalVarable.m_listTableNameOfStaticsResult[(int)JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ];
            //    m_strSQL = m_strSQL + " WHERE " + JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName + "='" + m_strDKID + "'";
            //    DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);

            //    if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
            //    {
            //        chkTdlyxz.Checked = true;
            //        txtResult.Text = txtResult.Text + m_DataRowCollection[0]["FXJG"].ToString().Replace("WYHH", "\r\n");
            //        this.txtResult.Text += "----------------------------------------------------";
            //    }

            //    //��ȡ���������ϸ����

            //    m_strSQL = "set names gb2312 ;" + p_strSQL + JCZF.Renderable.CGlobalVarable.m_listTableNameOfStaticsInfo[(int)JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ];
            //    m_strSQL = m_strSQL + " WHERE " + JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName + "='" + m_strDKID + "'";
            //    DataTable m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);

            //    if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            //    {
            //        chkTdlyxz.Checked = true;

            //        if (m_UserTDLYXZProperyShow == null)
            //        {
            //            m_UserTDLYXZProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
            //        }
            //        m_UserTDLYXZProperyShow.ShowDataFromDatabase(m_DataTable);


            //        this.tabControlPanelTDXZ.Controls.Clear();
            //        this.tabControlPanelTDXZ.Controls.Add(m_UserTDLYXZProperyShow);

            //        tabControl1.SelectedTab = this.tabItemTDXZ;
            //        tabControl1.Refresh();

            //    }
            //}
            //catch (SystemException errs)
            //{
            //    clsFunction.Function.MessageBoxError(errs.Message);
            //    
            //}

           
        }
        private void ReadFromResult_TDLYGH()
        {
        }

        private void ReadFromResult_JSYDSP()
        {
        }
        private void ReadFromResult_TDGY()
        {
        }
        private void ReadFromResult_KCZYGH()
        {
        }
        private void ReadFromResult_TKQ()
        {
        }
        private void ReadFromResult_CKQ()
        {
        }
        private void ReadFromResult_JBNT()
        {
        }

        /// <summary>
        /// ��� ��������ť���õĴ����listview ����20110731
        /// </summary>
        /// <param name="geo"></param>
        private  void FillHCListview(IGeometry geo)
        {
            listViewEx1.Items.Clear();

            ListViewItem listviewitem;

            IPointCollection pcollection = geo as IPointCollection;

            for (int i = 0; i < pcollection.PointCount; i++)
            {
                IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                pcollection.QueryPoint(i, pt);

                listviewitem = new ListViewItem((i + 1).ToString());

                listviewitem.SubItems.Add(pt.X.ToString());

                listviewitem.SubItems.Add(pt.Y.ToString());
                listViewEx1.Items.Add(listviewitem);
            }

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
            return GetOneFieldOftheFeatureWithFiledName(m_selFeature_, "dkid");

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
        private bool SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType p_Enum_AnalysisType, string theResult, string theLayerNameOfStatics)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(p_Enum_AnalysisType); // ����
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // ���йؼ�������
            string theID = m_strDKID;// GetStaticsTBBH(); // ���йؼ��ֵ�ֵ

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
            //ɾ����ǰ�ļ�¼
            
            m_DataAccess_SYS.DeleteByColumnValue(theIDName, theID, tableName);
            return m_DataAccess_SYS.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);

        }

        // ����һ�����ݹ���һ���б�
        private void BuildOneListWithOneRow(DataRow theRow, ref System.Collections.ArrayList theList)
        {
            ////string [] theDataTypeList = JCZF.Renderable.CGlobalVarable.GetFitalbeColDataTypeListOfStatisticsInfo (type)

            for (int i = 0; i < theRow.Table.Columns.Count; i++)
            {
                theList.Add(theRow[i]);
            }

        }


        /// <summary>
        /// ���������ϸ��Ϣ�����ݿ���
        /// </summary>
        /// <param name="type"></param>
        /// <param name="theInfoTable"></param>
        /// <returns></returns>
 
        private bool SaveStaticsInfoToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type, DataTable theInfoTable)
        {


            try
            {
                string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsInfo(type); // ����
                string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrInfoIDName; //��� ���йؼ�������
                //string theTBBH = m_strDKID;// GetStaticsTBBH(); // ��ȡͼ�߱��

                System.Collections.ArrayList theColNameList = new System.Collections.ArrayList(); // ������������

                theColNameList.Add(theIDName); // �����

                string[] theColNameListString = JCZF.Renderable.CGlobalVarable.GetTableColNameListOfStatisticsInfo(type);

                foreach (string theColName in theColNameListString) // ������������
                {
                    theColNameList.Add(theColName);
                }

                System.Collections.ArrayList theColValList = new System.Collections.ArrayList();// ���������ݼ���


                //m_theSqlOp.DelAllCorrespondingRecordWithID(tableName, theIDName, m_strDKID); // ɾ���������ID�ļ�¼
                m_DataAccess_SYS.DeleteByColumnValue(theIDName, m_strDKID, tableName);


                for (int i = 0; i < theInfoTable.Rows.Count; i++) // ���д洢
                {

                    int index = i + 1;// ���ñ�ŵ����

                    theColValList.Clear();
                    theColValList.Add(m_strDKID);

                    //string theID = String.Format("{0}_{1}", m_strDKID, index); // ������
                    //theColValList.Add(theTBBH); // ��ӱ�� 
                    BuildOneListWithOneRow(theInfoTable.Rows[i], ref theColValList);   // ����һ�����ݹ���һ���б�

                    //m_theSqlOp.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, theID);   // ��������
                    m_DataAccess_SYS.InsertOneRecord(tableName, theColNameList, theColValList, theIDName, m_strDKID);

                }


                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }



        // �ж��Ƿ��Ѿ���ͳ��
        private bool HasBeenStatisticed(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(type); // ����
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // ���йؼ�������
            string theID = GetStaticsTBBH(); // ���йؼ��ֵ�ֵ

            //return m_theSqlOp.ExistKeyIntheTable(tableName, theIDName, theID);
            return m_DataAccess_SYS.IsRecordExist(theIDName, theID,tableName);
        }

        #endregion

        private void btnSelectAll_Click(object sender, EventArgs e)
        {           
            SetCheckBoxChecked(true);
        }

        private void SetCheckBoxChecked(bool p_blChecked)
        {
            this.chkCkq.Checked = p_blChecked;
            this.chkTDLYGH.Checked = p_blChecked;
            this.chkJbnt.Checked = p_blChecked;
            this.chkJsyd.Checked = p_blChecked;
            this.chkKczygh.Checked = p_blChecked;
            this.chkTdlyxz.Checked = p_blChecked;
            this.chkTkq.Checked = p_blChecked;
            this.chkTDGY.Checked = p_blChecked;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                SetTabItemUnVisible();
                //tabControl2.SelectedTab = tabItem3;
                //for (int ii = 0; ii < 2; ii++)  // ���� �޸� 2011����  ���� ע��
                {
                    //����δ֪ԭ�򣬱���������Σ������������ȷ���Ժ�Ҫ���������20111023������ΰ
                    //if (ii == 1)  // ���� �޸� 2011����  ���� ע��
                    {
                        tabControl2.SelectedTab = tabItem3;
                    }
                    this.progressBar1.Value = 10;

                    this.txtResult.Text = "��ʼ�������㣡\r\n";


                    txtResult.Text = "";

                    // �����ʱͼ�㡰�˲�����
                    IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
                    if (hcjg != null)
                    {
                        m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);

                    }

               
                //this.txtResult.Text = strTemp;
                

                    clsClipStat m_clsClipStat = new clsClipStat();
                    m_clsClipStat.m_strFilePath = Application.StartupPath + "\\OverlayTemp";// ��ʱ�������ݴ���ļ���
                    m_clsClipStat.m_axMapcontrol = m_AxMapControl;
                    //д�� ���غ˲�ͼ�� ����Ϊͨ�����Ա���ʱ��û��m_sellayer��������m_sellayerΪnull��

                    // ��ȡͼ�㡰���غ˲顱
                    ILayer selLayer = MapFunction.getFeatureLayerByName("���غ˲�", m_AxMapControl);

                    m_clsClipStat.m_selLayer = selLayer;
                    m_clsClipStat.pDataAcess = m_DataAccess_SYS;
                    if (m_SelectedGeometry == null)
                    {
                        IPolygon m_IPolygon = (IPolygon)this.m_selFeature_.Shape;

                        // ���� �޸� 2011���� ���� ע��
                        // û��ʲô��
                        // IPointCollection m_IPointCollection = (IPointCollection)m_IPolygon;

                        // �϶����ã���ת����֪����ô�õ�
                        //m_IPolygon.ReverseOrientation();


                        m_clsClipStat.m_Geometry = (IGeometry)m_IPolygon;
                        m_clsClipStat.CreateClipFeatureClass(m_IPolygon); // ����Ϊһ��ͼ��
                        //m_clsClipStat.CreateClipFeatureClass(this.m_selFeature.ShapeCopy);

                    }
                    else
                    {
                        m_clsClipStat.m_Geometry = m_SelectedGeometry;
                        m_clsClipStat.CreateClipFeatureClass(m_SelectedGeometry); // ����Ϊһ��ͼ��
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

                                    // ���� �޸� 2011���� ����������״���� ע��
                                    //

                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "����������״"; // ��������
                                    // �����Ժ�ʹ��
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ;

                                    // ���� �޸� 2011���� ����
                                    // ����Clip��Merge���������ջ�ȡ����
                                    m_clsClipStat.Geoexecute();

                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;

                                    // �����к�ͼ����ص�map�У�������������������״clip��
                                    AddtoTOC(m_clsClipStat.pDltbOutputFeatClass, "����������״clip");
                                    loadDLTBProp(m_clsClipStat.pDltbOutputFeatClass);
                                    this.progressBar1.Value = 30;

                                    #region // ���� �޸� 2011 ����

                                    
                                    string theResult = m_clsClipStat.strHCJG; ;// �������
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                    // ��ȡͼ����
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ,theResult, theLayerName);// ����������


                                    #endregion

                                 
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "����������״�����д�����������" + "\r\n";
                            //this.txtResult.Text =strTemp;
                        }
                        try
                        {
                            //�滮
                            if (chkTDLYGH.Checked)
                            {


                                //if (m_blHasAnalysis[1] == true)
                                //{
                                //    btnGH_Click(sender, e);
                                //}
                                //else
                                //{
                                    // ���� �޸� 2011���� �滮���ݷ��� ע��
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.QHTexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "�滮����"; // ��������
                                    // �����Ժ�ʹ��
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH;

                                    // ���� �޸� 2011���� ����
                                    // ����Clip��Merge���������ջ�ȡ����

                                    m_clsClipStat.QHTexecute();

                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;
                                    AddtoTOC(m_clsClipStat.pGHTOutputFeatClass, "�滮����clip");
                                    loadGHTProp(m_clsClipStat.pGHTOutputFeatClass);
                                    this.progressBar1.Value = 50;

                                    #region // ���� �޸� 2011 ����

                                    string theResult = m_clsClipStat.strHCJG; ;// �������
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                    // ��ȡͼ����
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH, theResult, theLayerName);// ����������


                                    #endregion
                                //}

                            }
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "�������ù滮�����д�����������" + "\r\n";
                            //this.txtResult.Text = strTemp;
                        }

                        try
                        {
                            //���ع�Ӧ
                            if (chkTDGY.Checked)
                            {
                                //if (m_blHasAnalysis[2] == true)
                                //{
                                //    btnTDGY_Click(sender, e);
                                //}
                                //else
                                //{

                                    // ���� �޸� 2011���� ���ع�Ӧ���� ע��
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.TDGYexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "���ع�Ӧ"; // ��������
                                    // �����Ժ�ʹ��
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY;

                                    // ���� �޸� 2011���� ����
                                    // ����Clip��Merge���������ջ�ȡ����
                                    m_clsClipStat.TDGYexecute();

                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;
                                    AddtoTOC(m_clsClipStat.pTDGYOutputFeatClass, "���ع�Ӧclip");
                                    loadTDGYProp(m_clsClipStat.pTDGYOutputFeatClass);
                                    this.progressBar1.Value = 50;

                                    #region // ����ΰ �޸� 2011 ����

                         
                                    string theResult = m_clsClipStat.strHCJG; ;// �������
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                    // ��ȡͼ����
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY, theResult, theLayerName);// ����������


                                    #endregion
                                }

                            //}
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


                                //if (m_blHasAnalysis[3] == true)
                                //{
                                //    btnJBNT_Click(sender, e);
                                //}
                                //else
                                //{
                                    // ���� �޸� 2011���� ����ũ����� ע��
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.JBNTexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}


                                    string type = "����ũ��"; // ��������
                                    // �����Ժ�ʹ��
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT ;

                                    // ���� �޸� 2011���� ����ũ�����
                                    m_clsClipStat.JBNTexecute();//
                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;
                                    AddtoTOC(m_clsClipStat.pJbntOutputFeatClass, "����ũ��clip");
                                    loadJBNTProp(m_clsClipStat.pJbntOutputFeatClass);
                                    this.progressBar1.Value = 60;

                                    #region  ���� �޸� 2011 ����  �������

                      
                                    string theResult = m_clsClipStat.strHCJG; ;// �������
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                    // ��ȡͼ����
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT, theResult, theLayerName);// ����������


                                    #endregion
                                }

                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "����ũ�������д�����������" + "\r\n";
                            //this.txtResult.Text = strTemp ;
                        }
                        try
                        {
                            //������Ŀ
                            if (chkJsyd.Checked)
                            {

                                //if (m_blHasAnalysis[4] == true)
                                //{
                                //    btnGH_Click(sender, e);
                                //}
                                //else
                                //{
                                    // ���� �޸� 2011���� ������Ŀ���� ע��
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.JSYDexecute1();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}
                                          
                                    string type = "�����õ�����"; // ��������
                                    // �����Ժ�ʹ��
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD;

                                    // ���� �޸� 2011���� �����õ����ݷ���
                                    // ����Clip��Merge���������ջ�ȡ����
                                    m_clsClipStat.JSYDexecute();


                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                

                                    AddtoTOC(m_clsClipStat.pJsydOutputFeatClass, "�����õ�����clip");
                                    loadJSYDProp(m_clsClipStat.pJsydOutputFeatClass);

                                    this.progressBar1.Value = 70;

                                    #region  ���� �޸� 2011 ����

                            
                                    string theResult = m_clsClipStat.strHCJG; ;// �������
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                    // ��ȡͼ����
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD , theResult, theLayerName);// ����������


                                    #endregion
                                }
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "������Ŀ���������д�����������" + "\r\n";
                            //this.txtResult.Text = strTemp ;
                        }
                        try
                        {
                            //�����Դ�滮
                            if (chkKczygh.Checked)
                            {
                                //if (m_blHasAnalysis[5] == true)
                                //{
                                //    btnKCZY_Click(sender, e);
                                //}
                                //else
                                //{

                                    // ���� �޸� 2011���� �����Դ�滮���� ע��
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.KCZYexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "�����Դ�滮"; // ��������
                                    // �����Ժ�ʹ��
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH;

                                    // ���� �޸� 2011���� �����õ����ݷ���
                                    // ����Clip��Merge���������ջ�ȡ����
                                    m_clsClipStat.KCZYexecute();


                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;

                                    AddtoTOC(m_clsClipStat.pKczyOutputFeatClass, "�����Դ�滮clip");
                                    loadKczyProp(m_clsClipStat.pKczyOutputFeatClass);
                                    this.progressBar1.Value = 80;


                                    #region  ���� �޸� 2011 ����

                       
                                    string theResult = m_clsClipStat.strHCJG; ;// �������
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                    // ��ȡͼ����
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH, theResult, theLayerName);// ����������


                                    #endregion
                                }
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "�����Դ�滮�����д�����������" + "\r\n";
                            //this.txtResult.Text = strTemp ;
                        }
                        //�ɿ�
                        try
                        {
                            if (chkCkq.Checked)
                            {
                                //if (m_blHasAnalysis[6] == true)
                                //{
                                //    btnCKQ_Click(sender, e);
                                //}
                                //else
                                //{
                                    // ���� �޸� 2011���� �����Դ�滮���� ע��
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.CKQexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                      
                                    string type = "�ɿ�Ȩ"; // ��������
                                    // �����Ժ�ʹ��
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ;

                                    // ���� �޸� 2011���� �ɿ�Ȩ����
                                    // ����Clip��Merge���������ջ�ȡ����
                                    m_clsClipStat.CKQexecute();


                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;

                                    AddtoTOC(m_clsClipStat.pCkqOutputFeatClass, "�ɿ�Ȩclip");
                                    loadCKQProp(m_clsClipStat.pCkqOutputFeatClass);
                                    this.progressBar1.Value = 90;

                                    #region  ���� �޸� 2011 ����

                                    string theResult = m_clsClipStat.strHCJG; ;// �������
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                    // ��ȡͼ����
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ, theResult, theLayerName);// ����������


                                    #endregion
                                }
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "�ɿ�Ȩ�Ǽ������д�����������" + "\r\n";
                            //this.txtResult.Text = strTemp;
                        }


                        try
                        {
                            //̽��
                            if (chkTkq.Checked)
                            {
                                //if (m_blHasAnalysis[7] == true)
                                //{
                                //    btnTKQ_Click(sender, e);
                                //}
                                //else
                                //{

                                    // ���� �޸� 2011���� ̽��Ȩ���� ע��
                                    //
                                    //for (int i = 0; i < 5; i++)
                                    //{
                                    //    m_clsClipStat.TKQexecute();// Geoexecute();
                                    //    if (m_clsClipStat.strHCJG.IndexOf("����") >= 0)
                                    //    {
                                    //        break;
                                    //    }
                                    //}

                                    string type = "̽��Ȩ"; // ��������
                                    // �����Ժ�ʹ��
                                    JCZF.Renderable.CGlobalVarable.m_strCurrentStaticsType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ;

                                    // ���� �޸� 2011���� �ɿ�Ȩ����
                                    // ����Clip��Merge���������ջ�ȡ����
                                    m_clsClipStat.TKQexecute();

                                    this.txtResult.Text += "\r\n" + m_clsClipStat.strHCJG;
                                    this.txtResult.Text += "----------------------------------------------------";
                                    //this.txtResult.Text = strTemp;

                                    AddtoTOC(m_clsClipStat.pTkqOutputFeatClass, "̽��Ȩclip");
                                    loadTKQProp(m_clsClipStat.pTkqOutputFeatClass);

                                    this.progressBar1.Value = 100;


                                    //this.txtResult.Text = strTemp;
                                    this.progressBar1.Value = 100;

                                    #region  ���� �޸� 2011 ����


                                    string theResult = m_clsClipStat.strHCJG; ;// �������
                                    string theLayerName = clsClipStat.m_strLayerNameOfStatics;// ͼ������
                                    // ��ȡͼ����
                                    int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ );
                                    JCZF.Renderable.CGlobalVarable.m_listLayerNameOfStatics[indexOfLayer] = theLayerName;

                                    SaveStaticsResultToSQL(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ, theResult, theLayerName);// ����������


                                    #endregion
                                }

                              
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.txtResult.Text += "\r\n" + "----------------------------------------------------" + "\r\n" + "̽��Ȩ�Ǽ������д�����������";
                            //this.txtResult.Text = strTemp ;
                        }

                    }
                    catch (Exception o)
                    { }
                    this.progressBar1.Visible = false;
                }

                btnSaveResult.Enabled = true;
                btnExport.Enabled = true;


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
        private void AddtoTOC(IFeatureClass outputfeatclass,string layername)
        {
            // ���� �޸� 2011���� ���� 
            if (outputfeatclass == null)
                return;

            IFeatureLayer outlayer = new FeatureLayer() as IFeatureLayer;

            outlayer.SpatialReference = m_AxMapControl.SpatialReference;
            
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
            // ���� �޸� 2011���� ���� 
            if (m_FeatureClass == null)
                return;

            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemJSYD.Visible = false ;
                return;
            }
            if (m_UserJBNTProperyShow == null)
            {
                m_UserJBNTProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
              m_UserJBNTProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
           m_UserJBNTProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ���� �������
           }
            m_UserJBNTProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ���� �������
            m_UserJBNTProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT; // ����ũ�� �������

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "����ũ��clip";

            // ��ȡͼ���� ����20110814
            string type = "����ũ��";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserJBNTProperyShow.SetData(m_layer as ILayer);

            //this.gbJBNT.Controls.Add(m_UserJBNTProperyShow);
            m_UserJBNTProperyShow.Dock = DockStyle.Fill;
            //this.tabJBNT.Controls.Clear();
            //this.tabJBNT.Controls.Add(m_UserJBNTProperyShow);

            this.tabControlPanelJBNT.Controls.Clear();
            this.tabControlPanelJBNT.Controls.Add(m_UserJBNTProperyShow);
            this.tabItemJBNT.Visible = true;
            tabControl1.SelectedTab = this.tabItemJBNT;
        }
        void m_frmJBNTHCResult_FlashFeature(ESRI.ArcGIS.Geodatabase.IFeature pFeature)
        {
            m_AxMapControl.FlashShape(pFeature.Shape, 3, 300, null);
        }

        private void loadJSYDProp(IFeatureClass m_FeatureClass)
        {
            // ���� �޸� 2011���� ���� 
            if (m_FeatureClass == null)
                return;


            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemJSYD.Visible = false ;
                return;
            }
            if (m_UserJSYDProperyShow == null)
            {
                m_UserJSYDProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
                m_UserJSYDProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
                m_UserJSYDProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ���� 
            }
            m_UserJSYDProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserJSYDProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD ; // �����õ����� 

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "�����õ�����clip";

            // ��ȡͼ���� ����20110814
            string type = "�����õ�����";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserJSYDProperyShow.SetData(m_layer as ILayer);
            m_UserJSYDProperyShow.Dock = DockStyle.Fill;
            //this.tabJSYD.Controls.Clear();
            //this.tabJSYD.Controls.Add(m_UserJSYDProperyShow);

            this.tabControlPanelJSYD.Controls.Clear();
            this.tabControlPanelJSYD.Controls.Add(m_UserJSYDProperyShow);
            this.tabItemJSYD.Visible = true;
            tabControl1.SelectedTab = this.tabItemJSYD;
        }
        /// <summary>
        /// �����������ÿһ��ռ�ط�����ʾ����listview��
        /// </summary>
        /// <param name="m_FeatureClass"></param>
        private void loadDLTBProp(IFeatureClass m_FeatureClass)
        {

            try
            {
                // ���� �޸� 2011���� ���� 
                if (m_FeatureClass == null)
                    return;

                if (m_FeatureClass.FeatureCount(null) < 1)
                {
                    //this.tabItemTDXZ.Visible = false;
                    return;
                }
                this.tabItemTDXZ.Visible = true;

                if (m_UserTDLYXZProperyShow == null)
                {
                    m_UserTDLYXZProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
                    m_UserTDLYXZProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
                    m_UserTDLYXZProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ����
                }

                m_UserTDLYXZProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
                m_UserTDLYXZProperyShow.m_strDKID = m_strDKID;
                m_UserTDLYXZProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ; // �������� ���� �޸� 2011 ����

                IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
                m_layer.FeatureClass = m_FeatureClass;

                m_layer.Name = "����������״clip";

                // ��ȡͼ���� ����20110814
                string type = "����������״";
                int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);
                JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;


                m_UserTDLYXZProperyShow.SetData(m_layer as ILayer);
                m_UserTDLYXZProperyShow.Dock = DockStyle.Fill;
                //this.tabTDXZ.Controls.Clear();
                //this.tabTDXZ.Controls.Add(m_UserTDLYXZProperyShow);
                this.tabControlPanelTDXZ.Controls.Clear();
                this.tabControlPanelTDXZ.Controls.Add(m_UserTDLYXZProperyShow);

                tabControl1.SelectedTab = this.tabItemTDXZ;
                tabControl1.Refresh();
            }
            catch(SystemException errs)
            {
                m_DataAccess_SYS.MessageErrorInforShow(this, "��������������״��ϸ��Ϣʱ��������" + errs.Message);
            }
        }

        private void loadGHTProp(IFeatureClass m_FeatureClass)
        {
            // ���� �޸� 2011���� ���� 
            if (m_FeatureClass == null)
                return;


            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemTDGH.Visible = false;
                return;
            }

            if (m_UserGHTProperyShow == null)
            {
                m_UserGHTProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
             m_UserGHTProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ����
            m_UserGHTProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
           }

            m_UserGHTProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserGHTProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH; // �滮���� ���� �޸� 2011 ����

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "�滮����clip";

            // ��ȡͼ���� ����ΰ20110920
            string type = "�滮����";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserGHTProperyShow.SetData(m_layer as ILayer);
            m_UserGHTProperyShow.Dock = DockStyle.Fill;
            //this.tabTDGH.Controls.Clear();
            //this.tabTDGH.Controls.Add(m_UserGHTProperyShow);

            this.tabControlPanelTDGH.Controls.Clear();
            this.tabControlPanelTDGH.Controls.Add(m_UserGHTProperyShow);
            this.tabItemTDGH.Visible = true;
            tabControl1.SelectedTab = this.tabItemTDGH;

        }
        private void loadTDGYProp(IFeatureClass m_FeatureClass)
        {
            // ���� �޸� 2011���� ���� 
            if (m_FeatureClass == null)
                return;


            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemTDGY.Visible = false;
                return;
            }
            if (m_UserTDGYProperyShow == null)
            {
                m_UserTDGYProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
            m_UserTDGYProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
             m_UserTDGYProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ����
           }

            m_UserTDGYProperyShow.m_theDataAccess = m_DataAccess_SYS; // ����ΰ �޸� 2011 ����
            m_UserTDGYProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY; // ���ع�Ӧ���� ����ΰ�޸� 2011 ����

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "���ع�Ӧclip";

            // ��ȡͼ���� ����ΰ20110920
            string type = "���ع�Ӧ";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserTDGYProperyShow.SetData(m_layer as ILayer);
            m_UserTDGYProperyShow.Dock = DockStyle.Fill;
            //this.tabTDGY.Controls.Clear();
            //this.tabTDGY.Controls.Add(m_UserTDGYProperyShow);
            this.tabControlPanelTDGY.Controls.Clear();
            this.tabControlPanelTDGY.Controls.Add(m_UserTDGYProperyShow);
            this.tabItemTDGY.Visible = true;
            tabControl1.SelectedTab = this.tabItemTDGY;

        }
        private void loadKczyProp(IFeatureClass m_FeatureClass)
        {
            // ���� �޸� 2011���� ���� 
            if (m_FeatureClass == null)
                return;


            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemKCZYGH.Visible = false;
                return;
            }

            if (m_UserKCZYProperyShow == null)
            {
                m_UserKCZYProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
             m_UserKCZYProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ���� 
             m_UserKCZYProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
          }

            m_UserKCZYProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserKCZYProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH; // �����Դ�滮 


            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "�����Դ�滮clip";

            // ��ȡͼ���� ����ΰ20110920
            string type = "�����Դ�滮";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserKCZYProperyShow.SetData(m_layer as ILayer);
            m_UserKCZYProperyShow.Dock = DockStyle.Fill;
            //this.tabKCGH.Controls.Clear();
            //this.tabKCGH.Controls.Add(m_UserKCZYProperyShow);

            this.tabControlPanelKCZYGH.Controls.Clear();
            this.tabControlPanelKCZYGH.Controls.Add(m_UserKCZYProperyShow);
            this.tabItemKCZYGH.Visible = true;
            tabControl1.SelectedTab = this.tabItemKCZYGH;
        }

        private void loadCKQProp(IFeatureClass m_FeatureClass)
        {
            // ���� �޸� 2011���� ���� 
            if (m_FeatureClass == null)
                return;

            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemCKQ.Visible = false;
                return;
            }
            if (m_UserCKQProperyShow == null)
            {
                m_UserCKQProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
             m_UserCKQProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ���� 
            m_UserCKQProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
           }
            m_UserCKQProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ����
            m_UserCKQProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ; // �ɿ�Ȩ 

            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "�ɿ�Ȩclip";

            // ��ȡͼ���� ����ΰ20110920
            string type = "�ɿ�Ȩ";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ);
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;


            m_UserCKQProperyShow.SetData(m_layer as ILayer);
            m_UserCKQProperyShow.Dock = DockStyle.Fill;
            //this.tabCKQ.Controls.Clear();
            //this.tabCKQ.Controls.Add(m_UserCKQProperyShow);

            this.tabControlPanelCKQ.Controls.Clear();
            this.tabControlPanelCKQ.Controls.Add(m_UserCKQProperyShow);
            this.tabItemCKQ.Visible = true;
            tabControl1.SelectedTab = this.tabItemCKQ;
        }
        private void loadTKQProp(IFeatureClass m_FeatureClass)
        {
            // ���� �޸� 2011���� ���� 
            if (m_FeatureClass == null)
                return;

            if (m_FeatureClass.FeatureCount(null) < 1)
            {
                //this.tabItemTKQ.Visible = false;
                return;
            }
            if (m_UserTKQProperyShow == null)
            {
                m_UserTKQProperyShow = new UserProperyShow(m_AxMapControl, m_AxMapControl.Map);
            m_UserTKQProperyShow.Event_theSaveStatisticsInfo += new UserProperyShow.SaveStatisticsInfoEventHandler(SaveStaticsInfoToSQL);  // ����ͳ�Ƶ���ϸ��Ϣ ���� �޸� 2011 ����
            m_UserTKQProperyShow.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            }
            m_UserTKQProperyShow.m_theDataAccess = m_DataAccess_SYS; // ���� �޸� 2011 ���� 
            m_UserTKQProperyShow.m_theType = JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ; // ̽��Ȩ 


            IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
            m_layer.FeatureClass = m_FeatureClass;
            m_layer.Name = "̽��Ȩclip";

            // ��ȡͼ���� ����ΰ20110920
            string type = "̽��Ȩ";
            int indexOfLayer = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TKQ );
            JCZF.Renderable.CGlobalVarable.m_listOutLayerNameOfStatics[indexOfLayer] = m_layer.Name;

            m_UserTKQProperyShow.SetData(m_layer as ILayer);
            m_UserTKQProperyShow.Dock = DockStyle.Fill;
            //this.tabTKQ.Controls.Clear();
            //this.tabTKQ.Controls.Add(m_UserTKQProperyShow);

            this.tabControlPanelTKQ.Controls.Clear();
            this.tabControlPanelTKQ.Controls.Add(m_UserTKQProperyShow);
            this.tabItemTKQ.Visible = true;
            tabControl1.SelectedTab = this.tabItemTKQ;
        }
        #endregion

        private void btnSaveResult_Click(object sender, EventArgs e)
        {
// 1�����������һ���ؿ�ʱ�����������������������Ƚ����Ƶ���ʱ�ؿ���롰���غ˲顱ͼ���У�Ȼ
//������ID�ţ�oid ������OBJECTID�����ٽ������������������С����Ȼ���Ҫ�ж��Ƿ��Ѿ����롰���غ�
//�顱ͼ���У��������״��������ͬ�ļ�¼

//2������ѡ��ؿ���з���������Ҫ�ٱ���ؿ��ˣ�����Ҫ�����ID�ţ�oid ������OBJECTID�����ٽ������������������С�
        }



        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //tabControl2.SelectedTab = tabItem3;

                //m_DataAccess_SYS.OutputExcel(m_ResultDataGrid, "���غ˲�ռ�������", "");
                if (chkTdlyxz.Checked && this.m_UserTDLYXZProperyShow.m_PropertyDataGrid != null)
                {
                    //m_DataAccess_SYS.OutputExcel(this.m_UserTDLYXZProperyShow.m_PropertyDataGrid
                    this.m_DataAccess_SYS.OutputExcel(this.m_UserTDLYXZProperyShow.m_PropertyDataGrid, "��״����������", "");

                }
                if (this.chkTDLYGH.Checked && this.m_UserGHTProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserGHTProperyShow.m_PropertyDataGrid, "�滮ͼ�������", "");
                }
                if (this.chkTDGY.Checked && this.m_UserTDGYProperyShow.m_PropertyDataGrid != null)
                {
                    m_DataAccess_SYS.OutputExcel(this.m_UserTDGYProperyShow.m_PropertyDataGrid, "���ع�Ӧ�������", "");
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
                //IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
                //if (hcjg != null)
                //{
                //    m_AxMapControl.Map.DeleteLayer((ILayer)hcjg);
                //}
                //if (System.IO.Directory.Exists(Application.StartupPath + "\\OverlayTemp"))
                //{
                //    DeleteFolder(Application.StartupPath + "\\OverlayTemp");
                //}

                if (true == RemoveTempLayer())
                {
                    //MessageBox.Show("ɾ���ɹ�");

                    DeleteTempData();
                }
                else
                {
                    //MessageBox.Show("�Բ���ɾ��ʧ��");
                }
            }
            catch (Exception ex)
            { }
            this.Close();
        }

       

        /// <summary>
        /// ɾ����ʱͼ��
        /// </summary>
        private bool RemoveTempLayer()
        {

            try
            {
              
                //m_EnumLayer.Reset();
                string[] m_strLayerNames = new string[1] {  "�ؿ������ʱͼ��" };
                if (clsMapFunction.MapFunction.RemoveLayer(m_strLayerNames, this.m_AxMapControl))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void DeleteTempData()
        {
            try
            {
                //IGroupLayer hcjg = GetHcjggrouplayer("�˲���");
                //if (hcjg != null)
                //{
                //    axMapControl1.Map.DeleteLayer((ILayer)hcjg);

                //}
               clsMapFunction.MapFunction.RemoveLayerGroup("�˲���", this.m_AxMapControl);

                if (System.IO.Directory.Exists(Application.StartupPath + "\\OverlayTemp"))
                {
                    DeleteFolder(Application.StartupPath + "\\OverlayTemp");
                }
            }
            catch (Exception ex)
            { }
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

            // ��ȡͳ��ͼ���б� ���� �޸� 2011���� ���ط���
            string settingfilePath = System.IO.Path.Combine(Application.StartupPath, CGlobalVarable.m_strSettingFileOfStaticsLayers);
            CGlobalVarable.GetNameListOfStaticsLayers(settingfilePath);
            ArrayList theStatisticsLayerNameList = CGlobalVarable.m_listNameOfStaticsLayers;

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

                //����ע�͵�
                //ShowHasBeenStatisticedButton();
            }

            //λ������ ����ΰ 20110731
            this.Left = 5;
            this.Top = 200;

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

        //        int index = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(theType);
        //        //Button theButton = m_theButtonList[index] as Button;
        //        bool bStatisticed = this.HasBeenStatisticed(theType); // �ж��Ƿ�ͳ�ƹ�
        //        //theButton.Visible = bStatisticed; // ͳ�ƹ�����ʾ��������ʾ

        //        m_blHasAnalysis[i] = bStatisticed;

        //        //if (bStatisticed) // Ϊ���ж��м��������
        //        //    i++;
        //    }

        //    //if (i == 7) // ������ʾ
        //    //{
        //    //    this.Text = String.Format("ͼ�߷���-ȫ����Ŀ�Ѿ�������");
        //    //}
        //    //else
        //    //    this.Text = String.Format("ͼ�߷���-����{0}��������", i);
        //}

        // ����ͳ�����ͻ�ȡͳ�ƽ������
        private DataRow GetStatisticResultData(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type)
        {
            string tableName = JCZF.Renderable.CGlobalVarable.GetTableNameOfStaticsResult(type); // ����
            string theIDName = JCZF.Renderable.CGlobalVarable.m_strStaticsrResultIDName; // ���йؼ�������
            string theID = GetStaticsTBBH(); // ���йؼ��ֵ�ֵ
            //return this.m_theSqlOp.GetOneRow(tableName, theIDName, theID);
            return m_DataAccess_SYS.GetRowByColmn(theIDName, theID,tableName);
        }

        // ��ʾͳ�ƽ��
        private void ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType type)
        {
            DataRow theRow = GetStatisticResultData(type); // ����ͳ�����ͻ�ȡͳ�ƽ������
            string theTime = theRow[3].ToString(); // ����ʱ��
            string theResultShow = String.Format("�Ѻ˲���Ϊ��(����ʱ��({0}))", theTime);
            string theResult = ReTransforToFitableSQL(theRow[2].ToString().Trim()); // �������
            theResultShow += theResult;
            theResultShow += "----------------------------------------------------------" + "\r\n";

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
            DataTable theData = m_DataAccess_SYS.getDataTableByQueryString(theSQL);// ��ȡ����

            theShowControl.SetDataAfterStatisticed(theData);
            theShowControl.FlashFeature += new UserProperyShow.FlashFeatureEventHandler(m_frmJBNTHCResult_FlashFeature);
            theShowControl.Dock = DockStyle.Fill;

            int index = JCZF.Renderable.CGlobalVarable.GetStaticsIndex(type); // ��ȡ����
            int tabIndex = JCZF.Renderable.CGlobalVarable.m_listTabIndexOfStaticsInfo[index]; // ��ȡtab������Ŀǰ��һ�µ���index
            //this.tabTDFX.TabPages[tabIndex].Controls.Clear();
            //this.tabTDFX.TabPages[tabIndex].Controls.Add(theShowControl);
            //this.tabTDFX.SelectedIndex = tabIndex;

            this.tabControl1.Tabs[tabIndex].AttachedControl.Controls.Clear();
            this.tabControl1.Tabs[tabIndex].AttachedControl.Controls.Add(theShowControl);
            this.tabControl1.SelectedTab = this.tabControl1.Tabs[tabIndex];
        }

        // ��ť��ԭ-��������
        private void btnTDLY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[0]; // ����������״
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ);   // ��ʾͳ�ƽ��
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYXZ, m_UserTDLYXZProperyShow);  // ��ʾͳ����ϸ��Ϣ
        }

        // ��ť��ԭ-�滮����
        private void btnGH_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[1]; // �滮����
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH);   // ��ʾͳ�ƽ��
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDLYGH , m_UserGHTProperyShow);  // ��ʾͳ����ϸ��Ϣ

        }


        // ��ť�����ع�Ӧ
        private void btnTDGY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[2]; // �滮����
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY);   // ��ʾͳ�ƽ��
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.TDGY , m_UserTDGYProperyShow);  // ��ʾͳ����ϸ��Ϣ

        }




        // ��ť��ԭ-����ũ��
        private void btnJBNT_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[2]; // ����ũ�� �������
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT);   // ��ʾͳ�ƽ�� �������
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JBNT , m_UserJBNTProperyShow);  // ��ʾͳ����ϸ��Ϣ �������
        }


        // ��ť��ԭ-�����õ�
        private void btnJSYD_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[3]; // �����õ� 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD);   // ��ʾͳ�ƽ�� 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.JSYD , m_UserJSYDProperyShow);  // ��ʾͳ����ϸ��Ϣ 

        }


        // ��ť��ԭ-�����Դ
        private void btnKCZY_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[4]; // �����Դ 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH);   // ��ʾͳ�ƽ�� 
            ShowHasStatisticedInfo(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.KCZYGH, m_UserKCZYProperyShow);  // ��ʾͳ����ϸ��Ϣ 
        }


        // ��ť��ԭ-�ɿ�Ȩ
        private void btnCKQ_Click(object sender, EventArgs e)
        {
            string type = JCZF.Renderable.CGlobalVarable.m_listStaticsContents[5]; // �ɿ�Ȩ 
            ShowHasStatisticedResult(JCZF.Renderable.CGlobalVarable.Enum_AnalysisType.CKQ);   // ��ʾͳ�ƽ�� 
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

        private void btnSelectDk_Click(object sender, EventArgs e)
        {
            m_frmMapView.axMapControl1.CurrentTool = null;
            m_frmMapView.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;

            m_frmMapView.b_hcselectDK = true;
            m_frmMapView.b_hcDrawDK = false;

            //if (this.GetDKClick!= null)
            //{
            //    this.GetDKClick();

            //}
        }

        private void btnDrawDk_Click(object sender, EventArgs e)
        {
            m_frmMapView.axMapControl1.CurrentTool = null;
            m_frmMapView.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;

            m_frmMapView.b_hcDrawDK = true;
            m_frmMapView.b_hcselectDK = false;
        }

        private void btnImportDk_Click(object sender, EventArgs e)
        {
            //m_frmMapView.b_hcImportDK = true;
            importDK();

        }


        private void importDK()
        {
            string file = ImportCoor();
            if (file == null || file == "") return;
            StreamReader sr = new StreamReader(file);
            ArrayList arr = new ArrayList();
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                string[] split = str.Split(new char[] { ';' });
                foreach (string s in split)
                {
                    if (s.Trim() != "")
                        arr.Add(s);
                }
            }

            try
            {
                double[,] xy = getxyCoord(arr, Convert.ToInt32(arr[5].ToString()));

                IPointCollection pMultiPoint = addtomulpt(xy);

                ISegmentCollection pSegCol;
                pSegCol = new Ring() as ISegmentCollection ;
                object Missing1 = Type.Missing;
                object Missing2 = Type.Missing;

                for (int i = 0; i < pMultiPoint.PointCount - 1; i++)
                {
                    ILine pLine = new Line() as ILine;
                    pLine.PutCoords(pMultiPoint.get_Point(i), pMultiPoint.get_Point(i + 1));
                    pSegCol.AddSegment(pLine as ISegment, ref  Missing1, ref Missing2);
                }

                IRing pRing;
                pRing = pSegCol as IRing;

                pRing.Close();
                IGeometryCollection pPolygon;
                pPolygon = new Polygon() as IGeometryCollection ;
                pPolygon.AddGeometry(pRing, ref Missing1, ref Missing2);

                IGeometry geometry = pPolygon as IGeometry;
                geometry.SpatialReference = this.m_AxMapControl.SpatialReference;             

                //�Ѹ�ͼ�μ��뵽���غ˲�ͼ��   �޸� ���� ��ӵ���ʱͼ�� ������ͼ�����������ڷ����л��ƹ��ܺ͵�������������ͼ���޸�Ϊ������ʱ���ϣ����رշ����Ի����ɾ����ͼ�Σ�

                string time = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString(); //aa
                //���н��·��
                string m_strResultFilePath = Application.StartupPath + "\\��ʱͼ��" + time;  //aa
                if (!System.IO.Directory.Exists(m_strResultFilePath))
                {
                    System.IO.Directory.CreateDirectory(m_strResultFilePath);
                }


                IFeatureLayer m_hcfeaturelayer = MapFunction.getFeatureLayerByName("���غ˲�", m_AxMapControl);
                IFeatureWorkspace pFWS;
                IWorkspaceFactory pWorkspaceFactor = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                pFWS = pWorkspaceFactor.OpenFromFile(m_strResultFilePath, 0) as IFeatureWorkspace;
                IFields outfields = m_hcfeaturelayer.FeatureClass.Fields;
                IFeatureClass m_FeatureClass = pFWS.CreateFeatureClass("���غ˲�", outfields, null, null, esriFeatureType.esriFTSimple, "Shape", "");


                IFeatureLayer m_layer = new FeatureLayer() as IFeatureLayer;
                m_layer.FeatureClass = m_FeatureClass;
                m_layer.Name = m_FeatureClass.AliasName;
                this.m_AxMapControl.Map.AddLayer(m_layer);

                IWorkspaceEdit workspaceEdit = pFWS as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                IFeature feature = m_FeatureClass.CreateFeature();
                feature.Shape = geometry;
                try
                {
                    feature.Store();
                }
                catch
                {
                    ITopologicalOperator topologicaloperator = pPolygon as ITopologicalOperator;
                    topologicaloperator.Simplify();
                    feature.Store();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                m_AxMapControl.Extent = feature.Extent;

                //������ʾ
                IFeatureSelection featureSelection = m_layer as IFeatureSelection;
                featureSelection.Add(feature);

                IRgbColor m_color = new RgbColor() as IRgbColor;
                m_color.Red = 207;
                m_color.Green = 70;
                m_color.Blue = 215;
                featureSelection.SelectionColor = m_color;
                this.m_AxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, m_layer, null);

                this.m_selFeature = feature;
                FillHCListview(feature.ShapeCopy);
            }
            catch (Exception ex)
            {
                MessageBox.Show("�ļ���ʽ���ԣ�");
            }
            
            m_AxMapControl.ActiveView.Refresh();
        }


        //����ؿ�

        private string ImportCoor()
        {
            string s = "";
            OpenFileDialog pDlg = new OpenFileDialog();				//���ļ��Ի���
            pDlg.Title = "���ı��ļ�";

            pDlg.Filter = "�ı��ļ�(*.txt)|*.txt";

            if (pDlg.ShowDialog() != DialogResult.OK)
            {
                return s;
            }
            string filepath = System.IO.Path.GetDirectoryName(pDlg.FileName);
            string filename = System.IO.Path.GetFileName(pDlg.FileName);

            s = filepath + "\\" + filename;

            return s;
            //StreamReader sr = new StreamReader(filepath + "\\" + filename);


        }

        //public void FillHCListview(IGeometry geo)
        //{
        //    listViewEx1.Items.Clear();

        //    ListViewItem listviewitem;

        //    IPointCollection pcollection = geo as IPointCollection;

        //    for (int i = 0; i < pcollection.PointCount; i++)
        //    {
        //        IPoint pt = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
        //        pcollection.QueryPoint(i, pt);

        //        listviewitem = new ListViewItem(i.ToString());

        //        listviewitem.SubItems.Add(pt.X.ToString());

        //        listviewitem.SubItems.Add(pt.Y.ToString());
        //        listViewEx1.Items.Add(listviewitem);
        //    }

        //}



        //�洢����  
        private double[,] getxyCoord(ArrayList arr, int ddcount)
        {
            double[,] xy = new double[ddcount, 2];
            string m_strX;
            double[] m_dbXY;
            for (int j = 0; j < ddcount; j++)
            {
                if (arr[j + 6].ToString().Contains(","))
                {
                    string[] split = arr[j + 6].ToString().Split(new char[] { ',' });

                    xy[j, 0] = Convert.ToDouble(split[0].ToString());
                    xy[j, 1] = Convert.ToDouble(split[1].ToString());

                    /////////////////////////20111023 ����ΰ
                    m_strX=((int)xy[j, 0] ).ToString();
                    if (m_strX.Length == 8)//�Ƿ��д���
                    {
                        //ת����������ʾ��ͼ����ϵһ�µ����� 
                        m_dbXY = clsMapFunction.clsCoordinateConvert.ZBDZH(xy[j, 0], xy[j, 1], m_AxMapControl);
                        xy[j, 0] = m_dbXY[0];
                        xy[j, 1] = m_dbXY[1];                       
                    }
                    /////////////////////
                }
            }

            return xy;

        }

        //���ɵ㣬�����ӵ㵽multipoint
        private IPointCollection addtomulpt(double[,] xy)
        {
            IPointCollection pMultiPoint = new Multipoint() as IPointCollection ;
            for (int i = 0; i < xy.Length / 2; i++)
            {
                IPoint point = new ESRI.ArcGIS.Geometry.Point() as IPoint ;
                point.PutCoords(xy[i, 0], xy[i, 1]);
                object missing = Type.Missing;

                pMultiPoint.AddPoint(point, ref missing, ref missing);
            }
            return pMultiPoint;
        }


        private void SetTabItemUnVisible()
        {

            if (chkCkq.Checked)
            {
                tabItemCKQ.Visible = true;
            }
            else
            {
                tabItemCKQ.Visible = false;
            }
            if (chkTDGY.Checked)
            {
                tabItemTDGY.Visible = true;
            }
            else
            {
                tabItemTDGY.Visible = false;
            }
            if (chkTDLYGH.Checked)
            {
                tabItemTDGH.Visible = true;
            }
            else
            {
                tabItemTDGH.Visible = false;
            }
            if (chkJbnt.Checked)
            {
                tabItemJBNT.Visible = true;
            }
            else
            {
                tabItemJBNT.Visible = false;
            }
            if (chkJsyd.Checked)
            {
                tabItemJSYD.Visible = true;
            }
            else
            {
                tabItemJSYD.Visible = false;
            }
            if (chkKczygh.Checked)
            {
                tabItemKCZYGH.Visible = true;
            }
            else
            {
                tabItemKCZYGH.Visible = false;
            }
            if (chkTdlyxz.Checked)
            {
                tabItemTDXZ.Visible = true;
            }
            else
            {
                tabItemTDXZ.Visible = false;
            }
            if (chkTkq.Checked)
            {
                tabItemTKQ.Visible = true;
            }
            else
            {
                tabItemTKQ.Visible = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SetCheckBoxChecked(false );
        }

        private void btnZBReverse_Click(object sender, EventArgs e)
        {
            if (listViewEx1.Items == null || listViewEx1.Items.Count < 1) return;
            string[,] m_strZB = new string[listViewEx1.Items.Count, 2];

            int m = 0;
            for (int i =  listViewEx1.Items.Count-1; i >=0 ; i--)
            {
                m_strZB[m, 0] = listViewEx1.Items[i].SubItems[1].Text;
                m_strZB[m, 1] = listViewEx1.Items[i].SubItems[2].Text;
                m++;
            }

           



            for (int i = 0; i < m_strZB.Length/2; i++)
            {
                listViewEx1.Items[i].SubItems[1].Text = m_strZB[i, 0];
                listViewEx1.Items[i].SubItems[2].Text=m_strZB[i, 1] ;
            }

            listViewEx1.Refresh();
        }

       
       
    }
}