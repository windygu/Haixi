using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace JCZF.Renderable
{
    public class CGlobalVarable // ȫ�ֱ�����
    {
        public static bool m_bShowCollapse = true;// ��ʾ����
        public static bool m_bShowCollapseHidden = true;//  ��ʾ��������
        public static bool m_bShowLandslide = true;//  ��ʾ����
        public static bool m_bShowLandslideHidden = true; //  ��ʾ��������
        public static bool m_bShowGroundSubsidence = true;//  ��ʾ��������
        public static bool m_bShowGroundSubsidenceHidden = true; //  ��ʾ������������
        public static bool m_bShowDebrisFlow = true;//  ��ʾ��ʯ��
        public static bool m_bShowDebrisFlowHidden = true; //  ��ʾ��ʯ������
        public static bool m_bShowGroundFissure = true; //  ��ʾ���ѷ�
        public static bool m_bShowMiningCollapse = true;//  ��ʾ�ɿ�����
        public static bool m_bShowSlop = true; //  ��ʾб��

        public static object m_renderableCollapse = null;// ��������
        public static object m_renderableCollapseHidden = null;//  ������������
        public static object m_renderableLandslide = null;//  ���¶���
        public static object m_renderableLandslideHidden = null; //  ������������
        public static object m_renderableGroundSubsidence = null;//  �������ݶ���
        public static object m_renderableGroundSubsidenceHidden = null; //  ����������������
        public static object m_renderableDebrisFlow = null;//  ��ʯ������
        public static object m_renderableDebrisFlowHidden = null; //  ��ʯ����������
        public static object m_renderableGroundFissure = null; //  ���ѷ����
        public static object m_renderableMiningCollapse = null;//  �ɿ����ݶ���
        public static object m_renderableSlop = null; // ��ʾб��
        public static object m_renderableAll = null; // �����ֺ�


      public    enum Enum_AnalysisType
        {
             TDLYXZ =  0,TDLYGH = 1,TDGY = 2,JBNT = 3,JSYD =4, KCZYGH = 5,CKQ = 6,TKQ = 7
        }
      //public  enum Enum_AnalysisResultTable
      //  {
      //      TDLYXZ = "������_����������״_���ս��", TDLYGH = "������_�滮����_���ս��", TDGY = "������_���ع�Ӧ_���ս��",
      //      JBNT = "������_����ũ��_���ս��", JSYD = "������_�����õ�����_���ս��", KCZYGH = "������_�����Դ�滮_���ս��",
      //      CKQ = "������_�ɿ�Ȩ_���ս��", TKQ = "������_̽��Ȩ_���ս��"
      //  };

      //public  enum Enum_AnalysisInformationTable
      //  {
      //      TDLYXZ = "������_����������״_��ϸ��Ϣ", TDLYGH = "������_�滮����_��ϸ��Ϣ", TDGY = "������_���ع�Ӧ_��ϸ��Ϣ",
      //      JBNT = "������_����ũ��_��ϸ��Ϣ", JSYD = "������_�����õ�����_��ϸ��Ϣ", KCZYGH = "������_�����Դ�滮_��ϸ��Ϣ",
      //      CKQ = "������_�ɿ�Ȩ_��ϸ��Ϣ", TKQ = "������_̽��Ȩ_��ϸ��Ϣ"
      //  };


        // �����ֺ������б�
        public static string[] m_strDisasterNameList = { "����", "��������", "����", "��������", "��������", "������������", "��ʯ��", "��ʯ������", "���ѷ�", "�ɿ�����", "б��" };


        #region // ���� �޸� 2011���� ���ط���
        public static ArrayList m_listNameOfStaticsLayers = new ArrayList(); // ͳ�Ƶ�ͼ������

        public static string m_strSettingFileOfStaticsLayers = "ͳ��ͼ������.txt"; // ͳ��ͼ�������ļ���

        // ��ȡͳ��ͼ������
        public static void GetNameListOfStaticsLayers(string filePath)
        {
            ArrayList theLayerNameList = new ArrayList();// ͼ�����б�
            ArrayList theLayerTypeList = new ArrayList();// ͼ���������б�1�����Ͷ�Ӧ���ͼ�����ƣ�
            // ��ȡͼ�����ƺ�����
            ReadSettingOfNameOfStaticsLayers(filePath, ref theLayerNameList, ref theLayerTypeList);
            // ��������
            BuildDataOfNameOfStaticsLayers(theLayerNameList, theLayerTypeList);
        }




        // ��������
        public static void BuildDataOfNameOfStaticsLayers(ArrayList theLayerNameList, ArrayList theLayerTypeList)
        {
            m_listNameOfStaticsLayers.Clear(); // ���

            for (int i = 0; i < m_listStaticsContents.Length; i++)
            {
                ArrayList therNameList = new ArrayList();// ͼ�����б�
                m_listNameOfStaticsLayers.Add(therNameList); // ���ͼ��

            }

            //string type = "";
            //string name = "";
            //int index = -1;
            //for (int i = 0; i < theLayerNameList.Count; i++)
            //{
            //    type = theLayerTypeList[i].ToString().Trim();
            //    name = theLayerNameList[i].ToString().Trim();
            //    index = GetStaticsIndex(type); // ��ȡ��������
            //    if(index != -1) // �ҵ������ͣ�
            //    {
            //        ArrayList theList = m_listNameOfStaticsLayers[index] as ArrayList;
            //        theList.Add(name);

            //    }
            //}

        }


        // ��ȡ����
        public static void ReadSettingOfNameOfStaticsLayers(string filePath, ref ArrayList theColNameList, ref ArrayList theColWidthList)
        {
            StreamReader m_StreamReader;

            if (!File.Exists(filePath))
                return;

            theColNameList.Clear();
            theColWidthList.Clear();

            try
            {
                m_StreamReader = new StreamReader(filePath, Encoding.Default);

                string line;
                while ((line = m_StreamReader.ReadLine()) != null)
                {
                    string theColName = "";
                    string theColWidth = "";

                    // ��Ϊ2��
                    if (SplitToTwoPath(line, ref theColName, ref theColWidth))
                    {
                        theColNameList.Add(theColName);
                        theColWidthList.Add(theColWidth);
                    }
                }

            }
            catch (System.Exception ex)
            {

            }
        }


        // �����ַ���,Ϊ2����
        private static bool SplitToTwoPath(string theLine, ref string theOnePart, ref string theOtherPart)
        {
            int begin = theLine.IndexOf('<');
            int end = theLine.IndexOf('>');

            if (begin == -1 || end == -1) // ����
                return false;


            theOnePart = theLine.Substring(0, begin).Trim();
            theOtherPart = theLine.Substring(begin + 1, end - begin - 1).Trim();
            return true;
        }

        public static JCZF.Renderable.CGlobalVarable.Enum_AnalysisType m_strCurrentStaticsType; // ��ǰ��ͳ����Ŀ

        public const string m_strFieldNameOfLayerInSQLTable = "����ͼ������"; // ����ͼ�����ƣ���ϵͳ����ģ�����ͳ�Ƶ�ͼ���� ��SQL����

        public const string m_strFieldNameOfLayerInTable = "theLayerN"; // ����ͼ�����ƣ���ϵͳ����ģ�����ͳ�Ƶ�ͼ���� ��ͼ����

        public static ArrayList m_theListOfLayerNamesInNonShowInStatistics = new ArrayList(); // ��ͳ����û�б���ʾ��ͼ������

        public static double[] m_listOfCurrentExtent = { 100.0, 100.0, 400.0, 400.0 }; // ͳ��ǰ��ʾ�ķ�Χ��˳�� XMin,YMin,XMax,YMax


        #endregion

        #region // ���� �޸� 2011 ����

   

        public static string[] m_listStaticsContents = { "����������״", "�滮����", "���ع�Ӧ", "����ũ��", "�����õ�����", "�����Դ�滮", "�ɿ�Ȩ", "̽��Ȩ" }; // �������� �����޸���ע�� �޸�ͼ�����������ļ���ͳ��ͼ������.txt��

        public static string[] m_listTableNameOfStaticsResult = { "FXB_TDLYXZ_Result", "FXB_TDLYGH_Result", "FXB_TDGY_Result", "FXB_JBNT_Result", "FXB_JSYD_Result", "FXB_KCZYGH_Result", "FXB_CKQ_Result", "FXB_TKQ_Result" }; // ����������ݱ���

        public static string[] m_listTableNameOfStaticsInfo = { "FXB_TDLYXZ_Result_detail", "FXB_TDLYGH_Result_detail", "FXB_TDGY_Result_detail", "FXB_JBNT_Result_detail", "FXB_JSYD_Result_detail", "FXB_KCZYGH_Result_detail", "FXB_CKQ_Result_detail", "FXB_TKQ_Result_detail" }; // ��ϸ��Ϣ���ݱ���

        public static int[] m_listTabIndexOfStaticsInfo = { 0, 1, 2, 3, 4, 5, 6,7 };// ��ϸ��Ϣ���ݱ���

        public static string[] m_listLayerNameOfStatics = { "", "", "", "", "", "", "" ,""}; // ����ͼ����

        public static string[] m_listOutLayerNameOfStatics = { "", "", "", "", "", "", "","" }; // ����ͼ������ ����20110814



        public static string[] m_listFeatureIDFieldNameOfStaticsLayer = { "OBJECTID", "OBJECTID", "GDBH", "OBJECTID", "DKID", "KAGHDY_ID", "���֤��", "�������" };// Feature��ID�ֶ� �������

        public static string[] m_listColNameOfStaticsInfoTable = { "��ʶ��", "��ʶ��", "���ر��", "��ʶ��", "��ʶ��", "��ʶ��", "���֤��", "�������" };// ��ϸ��Ϣ�е�ID�ֶ�����
     
        public static string[] m_listColNameOfStaticsResult = { "DKID", "FXTCMC", "FXJG", "FXSJ" }; // ����������������

        public static string m_strStaticsrResultIDName = "DKID"; // �����ID����

        public static string m_strStaticsrInfoIDName = "DKID"; // ��Ϣ��ID����


        public static string[] m_listColNameOfStaticsInfoInTDLYTable_Chinese = {"����������������", "�����������ͱ���", "Ȩ����λ����", "Ȩ����λ����", "���䵥λ����", "���䵥λ����", "����Ȩ�ؼ���", "����ͼ����", "ͼ�����", "����", "����ͼ������" };// �������÷�����ϸ��ϢSQL���ݿ������(�ٱ��)
        public static string[] m_listColNameOfStaticsInfoInTDLYTable = { "DLMC", "DLBM", "QSDWMC", "QSDWDM", "ZLDWMC", "ZLDWDM", "QSZDH", "SZTFH", "TBMJ", "ZB", "SSTCMC" };// �������÷�����ϸ��ϢSQL���ݿ������(�ٱ��)
        public static string[] m_listColNameOfStaticsInfoInTDLYLayer = {  "DLMC", "DLBM", "QSDWMC", "QSDWDM", "ZLDWMC", "ZLDWDM", "QSZDH", "SZTFH", "SHAPE_area","ZB", m_strFieldNameOfLayerInTable };// �������÷�����ϸ��Ϣͼ��������
        public static int[] m_listColDataTypeOfStaticsInfoInTDLYTable = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 };// �������÷�����ϸ��ϢSQL���ݿ����������ͣ�1-�ַ�����2-���֣�3-����


        public static string[] m_listColNameOfStaticsInfoInGHTable_Chinese = { "���ڹ滮������", "ͼ�����", "����", "SSTCMC" };// �滮���ݷ�����ϸ��ϢSQL���ݿ������(�ٱ��)
        public static string[] m_listColNameOfStaticsInfoInGHTable = { "FQMC", "TBMJ", "ZB", "����ͼ������" };// �滮���ݷ�����ϸ��Ϣͼ��������
        public static string[] m_listColNameOfStaticsInfoInGHLayer = { "FQMC", "Shape_area", "ZB", m_strFieldNameOfLayerInTable };// �滮���ݷ�����ϸ��Ϣͼ��������

        public static string[] m_listColNameOfStaticsInfoInTDGYTable_Chinese = { "���ر��", "λ��", "������;", "�ؿ�ID", "ԭ�ؿ���", "�ؿ�����", "�ؿ����", "��Ŀ����", "�������", "�������", "�������", "���ط�ʽ", "��׼�ĺ�", "��׼����", "��ˮ��", "�ؿ�����", "���Ӽ�ܺ�", "��λ����", "����״̬", "��������������", "CJRQ", "����", "����ͼ������" };// ���ع�Ӧ���ݷ�����ϸ��ϢSQL���ݿ������(�ٱ��)
         public static string[] m_listColNameOfStaticsInfoInTDGYTable = { "GDBH", "ZDWZ", "TDYT", "DKID", "DKBH", "DKMC", "DKMJ", "XMMC", "CRMJ", "HBMJ", "GDMJ", "GDFS", "PZWH", "PZRQ", "FLOWSN", "DKLX", "DZJGH", "DWMC", "SPZT", "ZLXZQDM", "CJRQ", "ZB", "SSTCMC" };// ���ع�Ӧ���ݷ�����ϸ��Ϣͼ��������
        public static string[] m_listColNameOfStaticsInfoInTDGYLayer = { "GDBH", "ZDWZ", "TDYT", "DKID", "DKBH", "DKMC", "DKMJ", "XMMC", "CRMJ", "HBMJ", "GDMJ", "GDFS", "PZWH", "PZRQ", "FLOWSN", "DKLX", "DZJGH", "DWMC", "SPZT", "ZLXZQDM", "CJRQ", "ZB", m_strFieldNameOfLayerInTable };// ���ع�Ӧ���ݷ�����ϸ��Ϣͼ��������

        public static string[] m_listColNameOfStaticsInfoInJBNTTable_Chinese = { "����ũ��ͼ�߱��", "��������", "ͼ�����", "����", "����ͼ������" };// ����ũ�������ϸ��ϢSQL���ݿ������(�ٱ��) �������
        public static string[] m_listColNameOfStaticsInfoInJBNTTable = { "JBNTTBBH", "QSDWMC", "TBMJ", "ZB", "SSTCMC" };// ����ũ�������ϸ��Ϣͼ�������� �������
        public static string[] m_listColNameOfStaticsInfoInJBNTLayer = { "JBNTTBBH", "QSDWMC", "Shape_area", "ZB", m_strFieldNameOfLayerInTable };// ����ũ�������ϸ��Ϣͼ�������� �������

        public static string[] m_listColNameOfStaticsInfoInJSYDTable_Chinese = { "��������", "�ؿ�����", "ʡ������", "ͼ����", "�ؿ���;", "����״̬", "��׼����", "�ؿ����", "��Ŀ����", "����������", "��Ŀ����", "ʡ�����ĺ�", "��׼����", "Ȩ�����ı��", "ͼ�����", "����", "����ͼ������" };// �����õط�����ϸ��ϢSQL���ݿ������
        public static string[] m_listColNameOfStaticsInfoInJSYDTable = { "KCMC", "DKMC", "SSLBH", "TFH", "DKYT", "SPZT", "PZJG", "DKMJ", "XMLX", "XZQMC", "XMMC", "SPFWH", "PZRQ", "QSWH", "TBMJ", "ZB", "SSTCMC" };// �����õط�����ϸ��ϢSQL���ݿ������
        public static string[] m_listColNameOfStaticsInfoInJSYDLayer = { "KCMC", "DKMC", "SSLBH", "TFH", "DKYT", "SPZT", "PZJG", "DKMJ", "XMLX", "XZQMC", "XMMC", "SPFWH", "PZRQ", "QSWH", "Shape_area", "ZB", m_strFieldNameOfLayerInTable };// �����õط�����ϸ��Ϣͼ�������� 


        public static string[] m_listColNameOfStaticsInfoInKCZYTable_Chinese = { "��������滮������", "��������滮�������", "�滮����ʱ��", "ͼ�����", "����", };// �����Դ������ϸ��ϢSQL���ݿ������
        public static string[] m_listColNameOfStaticsInfoInKCZYTable = { "GHQMC1", "GHQLB1", "GHBZSJ", "TBMJ", "ZB","SSTCMC"};// �����Դ������ϸ��Ϣͼ�������� 
        public static string[] m_listColNameOfStaticsInfoInKCZYLayer = { "GHQMC1", "GHQLB1", "GHBZSJ", "Shape_area", "ZB", m_strFieldNameOfLayerInTable };// �����Դ������ϸ��Ϣͼ�������� 

        public static string[] m_listColNameOfStaticsInfoInCKQTable_Chinese = { "���֤��", "��Ŀ����", "������", "��ɽ����", "��������", "����������", "����������", "��ƹ�ģ", "��������", "��������", "�������", "��Ч����", "��Ч��ֹ", "�����ʶ", "����", "����ͼ������" };// �ɿ�Ȩ������ϸ��ϢSQL���ݿ������
        public static string[] m_listColNameOfStaticsInfoInCKQTable = { "XKZH", "XMLX", "SQR", "KSMC", "JJLX", "KCZKZ", "QTZKZ", "SJGM", "CSSX", "CSXX", "KQMJ", "YXQQ", "YXQZ", "KTBZ", "ZB", "SSTCMC" };// �ɿ�Ȩ������ϸ��ϢSQL���ݿ������
        public static string[] m_listColNameOfStaticsInfoInCKQLayer = { "���֤��", "��Ŀ����", "������", "��ɽ����", "��������", "����������", "����������", "��ƹ�ģ", "��������", "��������", "�������", "��Ч����", "��Ч��ֹ", "�����ʶ", "����", m_strFieldNameOfLayerInTable };// �ɿ�Ȩ������ϸ��Ϣͼ�������� 

        public static string[] m_listColNameOfStaticsInfoInTKQTable_Chinese = { "�������", "���֤��", "��Ŀ����", "��Ŀ����", "������", "���鵥λ", "��������", "��Ŀ����", "����λ��", "�������", "�����", "��Ч����", "��Ч��ֹ", "ͼ�����", "����", "����ͼ������" };// ̽��Ȩ������ϸ��ϢSQL���ݿ������
        public static string[] m_listColNameOfStaticsInfoInTKQTable = { "SQXH", "XKZH", "XMMC", "XMLX", "SQR", "KCDW", "JJLX", "XMXZ", "DLWZ", "KCKZ", "ZMJ", "YXQQ", "YXQZ", "TBMJ", "ZB", "SSTCMC" };// ̽��Ȩ������ϸ��ϢSQL���ݿ������
        public static string[] m_listColNameOfStaticsInfoInTKQLayer = { "�������", "���֤��", "��Ŀ����", "��Ŀ����", "������", "���鵥λ", "��������", "��Ŀ����", "����λ��", "�������", "�����", "��Ч����", "��Ч��ֹ", "Shape_Area", "����", m_strFieldNameOfLayerInTable };// ̽��Ȩ������ϸ��Ϣͼ�������� 


        // ����ͳ�����ݵõ�ͳ�ƽ������
        public static string GetTableNameOfStaticsResult(Enum_AnalysisType p_Enum_AnalysisType)
        {

            string tableName = "";

            //for (int i = 0; i < m_listStaticsContents.Length; i++)
            //{
            //    if (staticsContent == m_listStaticsContents[i])
            //        return m_listTableNameOfStaticsResult[i];
            //}

            tableName = m_listTableNameOfStaticsResult[(int)p_Enum_AnalysisType];
            return tableName;

        }


        // ����ͳ�����ݵõ�ͳ����ϸ��Ϣ
        public static string GetTableNameOfStaticsInfo(Enum_AnalysisType p_Enum_AnalysisType)
        {

            string tableName = "";

            //for (int i = 0; i < m_listStaticsContents.Length; i++)
            //{
            //    if (staticsContent == m_listStaticsContents[i])
            //        return m_listTableNameOfStaticsInfo[i];
            //}

            tableName = m_listTableNameOfStaticsInfo[(int)p_Enum_AnalysisType];
            return tableName;

        }


        // ����ͳ�����ݵõ�ͳ��ͼ������
        public static int GetStaticsIndex(Enum_AnalysisType p_Enum_AnalysisType)
        {

            int index = -1;

            //for (int i = 0; i < m_listStaticsContents.Length; i++)
            //{
            //    if (staticsContent == m_listStaticsContents[i])
            //        return i;
            //}

            index = (int)p_Enum_AnalysisType;

            return index;

        }



        // ����һ���ַ�������������Ԫ��Ϊ���ַ���
        public static void SetNullStringToList(string[] theList)
        {

            for (int i = 0; i < m_listStaticsContents.Length; i++)
            {
                theList[i] = "";
            }


        }

        // ����ͳ��ͼ��������Ԫ��Ϊ���ַ���
        public static void InitialLayerNameOfStatics(string[] theList)
        {

            SetNullStringToList(m_listLayerNameOfStatics);
            SetNullStringToList(m_listOutLayerNameOfStatics);



        }


        // ����һ��������ֵ���ҵ���һ����Ӧ��ֵ
        public static string GetCorrespondingValueWithOneStringListAndValue(string[] resultList,string[] resouceList,string resourceVal)
        {

            string theReusltVal = "";

            for (int i = 0; i < resouceList.Length; i++)
            {
                if (resourceVal == resouceList[i])
                    return resultList[i];
            }

            return theReusltVal;

        }


        // �ҵ����ʵ�����
        public static string GetFitalbeColName(Enum_AnalysisType p_Enum_AnalysisType, string theColName)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // �������ͣ�����������״
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInTDLYTable, m_listColNameOfStaticsInfoInTDLYLayer, theColName);
                case Enum_AnalysisType.TDLYGH:  // �������ͣ��滮����
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInGHTable, m_listColNameOfStaticsInfoInGHLayer, theColName);
                case Enum_AnalysisType.TDGY:  // �������ͣ��滮����
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInTDGYTable, m_listColNameOfStaticsInfoInTDGYLayer, theColName);
                case Enum_AnalysisType.JBNT:   // �������ͣ�����ũ��
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInJBNTTable, m_listColNameOfStaticsInfoInJBNTLayer, theColName); // �������
                case Enum_AnalysisType.JSYD:   // �������ͣ������õ�����
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInJSYDTable, m_listColNameOfStaticsInfoInJSYDLayer, theColName);
                case Enum_AnalysisType.KCZYGH:   // �������ͣ������Դ�滮
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInKCZYTable, m_listColNameOfStaticsInfoInKCZYLayer, theColName);
                case Enum_AnalysisType.CKQ:   // �������ͣ��ɿ�Ȩ
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInCKQTable, m_listColNameOfStaticsInfoInCKQLayer, theColName);
                case Enum_AnalysisType.TKQ:   // �������ͣ�̽��Ȩ
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInTKQTable, m_listColNameOfStaticsInfoInTKQLayer, theColName);
   
                
            }
            return null;
        }


        /// <summary>
        /// ������ݿ���ͳ����ϸ��Ϣ�������ļ���
        /// </summary>
        /// <param name="p_Enum_AnalysisType"></param>
        /// <returns></returns>
        public static string[] GetTableColNameListOfStatisticsInfo(Enum_AnalysisType p_Enum_AnalysisType)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // �������ͣ�����������״
                    return m_listColNameOfStaticsInfoInTDLYTable;
                case  Enum_AnalysisType.TDLYGH:  // �������ͣ��滮����
                    return m_listColNameOfStaticsInfoInGHTable;
                case Enum_AnalysisType.TDGY:  // �������ͣ����ع�Ӧ����
                    return m_listColNameOfStaticsInfoInTDGYTable;
                case Enum_AnalysisType.JBNT:    // �������ͣ�����ũ��
                    return m_listColNameOfStaticsInfoInJBNTTable; // 
                case Enum_AnalysisType.JSYD:  // �������ͣ������õ����� 
                    return m_listColNameOfStaticsInfoInJSYDTable;
                case Enum_AnalysisType.KCZYGH:     // �������ͣ������Դ�滮
                    return m_listColNameOfStaticsInfoInKCZYTable;
                case Enum_AnalysisType.CKQ:   // �������ͣ��ɿ�Ȩ
                    return m_listColNameOfStaticsInfoInCKQTable;
                case Enum_AnalysisType.TKQ :   // �������ͣ�̽��Ȩ
                    return m_listColNameOfStaticsInfoInTKQTable; 
            }
            return null;
        }

        /// <summary>
        /// �ҵ����ʵ�ͳ����ϸ��Ϣ�����������Ľ��ͼ���
        /// </summary>
        /// <param name="p_Enum_AnalysisType"></param>
        /// <returns></returns>
        public static string[] GetTableColNameListOfStatisticsInfo_Chinese(Enum_AnalysisType p_Enum_AnalysisType)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // �������ͣ�����������״
                    return m_listColNameOfStaticsInfoInTDLYTable_Chinese;
                case Enum_AnalysisType.TDLYGH:  // �������ͣ��滮����
                    return m_listColNameOfStaticsInfoInGHTable_Chinese;
                case Enum_AnalysisType.TDGY:  // �������ͣ����ع�Ӧ����
                    return m_listColNameOfStaticsInfoInTDGYTable_Chinese;
                case Enum_AnalysisType.JBNT:    // �������ͣ�����ũ��
                    return m_listColNameOfStaticsInfoInJBNTTable_Chinese; // 
                case Enum_AnalysisType.JSYD:  // �������ͣ������õ����� 
                    return m_listColNameOfStaticsInfoInJSYDTable_Chinese;
                case Enum_AnalysisType.KCZYGH:     // �������ͣ������Դ�滮
                    return m_listColNameOfStaticsInfoInKCZYTable_Chinese;
                case Enum_AnalysisType.CKQ:   // �������ͣ��ɿ�Ȩ
                    return m_listColNameOfStaticsInfoInCKQTable_Chinese;
                case Enum_AnalysisType.TKQ:   // �������ͣ�̽��Ȩ
                    return m_listColNameOfStaticsInfoInTKQTable_Chinese;
            }
            return null;
        }



        // �ҵ����ʵ�ͳ����ϸ��Ϣ����������
        public static string[] GetFitableLayerColNameListOfStatisticsInfo(Enum_AnalysisType p_Enum_AnalysisType)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // �������ͣ�����������״
                    return m_listColNameOfStaticsInfoInTDLYLayer;
                case  Enum_AnalysisType.TDLYGH:  // �������ͣ��滮����
                    return m_listColNameOfStaticsInfoInGHLayer;
                case Enum_AnalysisType.TDGY:  // �������ͣ��滮����
                    return m_listColNameOfStaticsInfoInTDGYLayer;
                case Enum_AnalysisType.JBNT:    // �������ͣ�����ũ��
                    return m_listColNameOfStaticsInfoInJBNTLayer; // �������
                case Enum_AnalysisType.JSYD:  // �������ͣ������õ����� 
                    return m_listColNameOfStaticsInfoInJSYDLayer;
                case Enum_AnalysisType.KCZYGH:    // �������ͣ������Դ�滮
                    return m_listColNameOfStaticsInfoInKCZYLayer;
                case Enum_AnalysisType.CKQ:     // �������ͣ��ɿ�Ȩ
                    return m_listColNameOfStaticsInfoInCKQLayer;
                case Enum_AnalysisType.TKQ:     // �������ͣ�̽��Ȩ
                    return m_listColNameOfStaticsInfoInTKQLayer;
            }
            return null;
        }


        // �ҵ����ʵ�ͳ����ϸ��Ϣ���е���������
        public static int[] GetFitalbeColDataTypeListOfStatisticsInfo(Enum_AnalysisType p_Enum_AnalysisType)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // �������ͣ�����������״
                    return m_listColDataTypeOfStaticsInfoInTDLYTable;
                case  Enum_AnalysisType.TDLYGH:  // �������ͣ��滮����
                    //return m_listColDataTypeOfStaticsInfoInGHTable;
                case Enum_AnalysisType.JBNT:   // �������ͣ�����ũ��
                    break;
                case Enum_AnalysisType.JSYD:   // �������ͣ������õ�����
                    break;
                case Enum_AnalysisType.KCZYGH:     // �������ͣ������Դ�滮
                    break;
                case Enum_AnalysisType.CKQ:     // �������ͣ��ɿ�Ȩ
                    break;
                case Enum_AnalysisType.TKQ:    // �������ͣ�̽��Ȩ
                    break;
            }
            return null;
        }


        #endregion // ���� �޸� 2011 ����

        
        #region // ���� �޸� 2011 �ŷ�

        public static int m_nWhichTypeOfStatistic = 1; // ͳ�����ͣ�1��2�������1�����ͣ�3�������2������ // ���� �޸�  2011 �ŷ�

        public static JCZF.LetterAndCall.Query.uctLetterAndCallShowStaResult_1 m_theLetterAndCallStaForm1= null; // ͳ������ǰ2�����ô���

        public static JCZF.LetterAndCall.Query.uctLetterAndCallShowStaResult_2 m_theLetterAndCallStaForm2 = null; // ͳ�����͵�3�����ô���

        public static JCZF.SubFrame.frmLetterAndCallDetailInfo m_theLetterAndCallDetailInfoForm = null; // ��ʾͶ����ϸ��Ϣ�Ĵ���

        // �õ������е�����
        public static int GetIndexOfList(string key, string[] list)
        {

            int index = -1;
            for (int i = 0; i < list.Length; i++)
            {
                if (key == list[i])
                {
                    index = i;
                    return index;
                }
            }

            return index;

        }


        // �����ַ���,Ϊ3����
        private static bool SplitToThreeOfListviewShow(string theLine, ref string theColName, ref int theWidth, ref string theFieldName)
        {
            string[] contents = theLine.Split(';');


            if (contents.Length != 3) // ����
                return false;


            theColName = contents[0].Trim();
            theWidth = int.Parse(contents[1].Trim());
            theFieldName = contents[2].Trim();

            return true;
        }


        public static void ReadSettingOfListviewColWidthOfFeature(string filePath, ref ArrayList theColNameList, ref ArrayList theColWidthList, ref ArrayList theFieldNameList)
        {
            StreamReader m_StreamReader;

            if (!File.Exists(filePath))
                return;

            theColNameList.Clear();
            theColWidthList.Clear();
            theFieldNameList.Clear();

            try
            {
                m_StreamReader = new StreamReader(filePath, Encoding.Default);

                string line;
                while ((line = m_StreamReader.ReadLine()) != null)
                {
                    string theColName = "";
                    int theColWidth = 0;
                    string theFieldName = "";

                    // ��Ϊ3��
                    if (SplitToThreeOfListviewShow(line, ref theColName, ref theColWidth, ref theFieldName))
                    {
                        theColNameList.Add(theColName);
                        theColWidthList.Add(theColWidth);
                        theFieldNameList.Add(theFieldName);
                    }
                }

            }
            catch (System.Exception ex)
            {

            }
        }

        // �����ַ���,Ϊ2����
        private static bool SplitToTwoOfListviewColWidth(string theLine, ref string theColName, ref int theWidth)
        {
            int begin = theLine.IndexOf('<');
            int end = theLine.IndexOf('>');

            if (begin == -1 || end == -1) // ����
                return false;


            theColName = theLine.Substring(0, begin).Trim();
            theWidth = int.Parse(theLine.Substring(begin + 1, end - begin - 1).Trim());
            return true;
        }

        public static void ReadSettingOfListviewColWidth(string filePath, ref ArrayList theColNameList, ref ArrayList theColWidthList)
        {
            StreamReader m_StreamReader;

            if (!File.Exists(filePath))
                return;

            theColNameList.Clear();
            theColWidthList.Clear();

            try
            {
                m_StreamReader = new StreamReader(filePath, Encoding.Default);

                string line;
                while ((line = m_StreamReader.ReadLine()) != null)
                {
                    string theColName = "";
                    int theColWidth = 0;

                    // ��Ϊ2��
                    if (SplitToTwoOfListviewColWidth(line, ref theColName, ref theColWidth))
                    {
                        theColNameList.Add(theColName);
                        theColWidthList.Add(theColWidth);
                    }
                }

            }
            catch (System.Exception ex)
            {

            }
        }


        #endregion // ���� �޸� 2011 �ŷ�

        // �����ֺ�ͼ���ļ������б�
        public static string[] m_strIconsNameList = { "iconLandslide.png", "iconLandslideHidden.png", "iconCollapse.png",
                                                        "iconCollapseHidden.png", "iconGroundSubsidence.png", "iconGroundSubsidenceHidden.png", 
                                                        "iconDebrisFlow.png", "iconDebrisFlowHidden.png", "iconGroundFissure.png", "iconMiningCollapse.png", "iconSlop.png" };

        public static System.Windows.Forms.ListView m_listView = null; //  listview

        public static System.Windows.Forms.RadioButton m_radioButtonDisplayNone = null; // radioButtonDisplayNone

        public static System.Windows.Forms.Form m_WorldWindow = null; //  ������

        public static JCZF.SubFrame.AttibuteEdit.FormMenuButton m_theSlideForm = null; // ��������

        public static string m_strExcelFileName = "���������ֺ���_Geographic_xian80.xls"; // �ֺ���ϢExcel�ļ���

        public static string m_strDataPath = ""; // ����·��



        // �鿴�ֺ��Ƿ���ʾ
        public static bool WhichDisasterShow(int which)
        {
            bool isShow = false;  // Ĭ��Ϊ����ʾ

            switch(which)
            {
                case 0: isShow = m_bShowLandslide; break; //  ��ʾ����
                case 1: isShow = m_bShowLandslideHidden; break; // ��ʾ��������
                case 2: isShow = m_bShowCollapse; break; // ��ʾ����
                case 3: isShow = m_bShowCollapseHidden; break; // ��ʾ��������
                case 4: isShow = m_bShowGroundSubsidence; break; // ��ʾ��������
                case 5: isShow = m_bShowGroundSubsidenceHidden; break; // ��ʾ������������
                case 6: isShow = m_bShowDebrisFlow; break; // ��ʾ��ʯ��
                case 7: isShow = m_bShowDebrisFlowHidden; break; // ��ʾ��ʯ������
                case 8: isShow = m_bShowGroundFissure; break; // ��ʾ���ѷ�
                case 9: isShow = m_bShowMiningCollapse; break;// ��ʾ�ɿ�����
                case 10: isShow = m_bShowSlop; break; // ��ʾб��
                default: break; // �����ֺ�
            }

            return isShow;// �����ֺ��Ƿ���ʾ

        }

        // ����listview
        public static void DoUpdateListviewOther()
        {
            m_listView.Items[11].Checked = false; // �����ֺ�
        }

        // ����listview
        public static void DoUpdateListview()
        {
            if (m_listView == null) // ���listviewΪ��
                return;

            //for (int i = 0; i < m_listView.Items.Count; i++)

            int length = m_strIconsNameList.Length;

            for (int i = 0; i < length; i++) // �����ֺ�
            {
                m_listView.Items[i].Checked = WhichDisasterShow(i); // ����
            }
          
            

            //m_listView.Items[0].Checked = m_bShowLandslide; // ����
            //m_listView.Items[1].Checked = m_bShowLandslide; // ����
            //m_listView.Items[2].Checked = m_bShowDebrisFlow; // ��ʯ��
            //m_listView.Items[3].Checked = m_bShowGroundSubsidence; // ��������
            //m_listView.Items[0].Checked = m_bShowCollapse; // ����
            //m_listView.Items[1].Checked = m_bShowLandslide; // ����
            //m_listView.Items[2].Checked = m_bShowDebrisFlow; // ��ʯ��
            //m_listView.Items[3].Checked = m_bShowGroundSubsidence; // ��������
        }


       

    }
}
