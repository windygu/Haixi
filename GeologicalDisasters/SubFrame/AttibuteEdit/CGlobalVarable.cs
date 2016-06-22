using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace JCZF.Renderable
{
    public class CGlobalVarable // 全局变量类
    {
        public static bool m_bShowCollapse = true;// 显示崩塌
        public static bool m_bShowCollapseHidden = true;//  显示崩塌隐患
        public static bool m_bShowLandslide = true;//  显示滑坡
        public static bool m_bShowLandslideHidden = true; //  显示滑坡隐患
        public static bool m_bShowGroundSubsidence = true;//  显示地面塌陷
        public static bool m_bShowGroundSubsidenceHidden = true; //  显示地面塌陷隐患
        public static bool m_bShowDebrisFlow = true;//  显示泥石流
        public static bool m_bShowDebrisFlowHidden = true; //  显示泥石流隐患
        public static bool m_bShowGroundFissure = true; //  显示地裂缝
        public static bool m_bShowMiningCollapse = true;//  显示采空塌陷
        public static bool m_bShowSlop = true; //  显示斜坡

        public static object m_renderableCollapse = null;// 崩塌对象
        public static object m_renderableCollapseHidden = null;//  崩塌隐患对象
        public static object m_renderableLandslide = null;//  滑坡对象
        public static object m_renderableLandslideHidden = null; //  滑坡隐患对象
        public static object m_renderableGroundSubsidence = null;//  地面塌陷对象
        public static object m_renderableGroundSubsidenceHidden = null; //  地面塌陷隐患对象
        public static object m_renderableDebrisFlow = null;//  泥石流对象
        public static object m_renderableDebrisFlowHidden = null; //  泥石流隐患对象
        public static object m_renderableGroundFissure = null; //  地裂缝对象
        public static object m_renderableMiningCollapse = null;//  采空塌陷对象
        public static object m_renderableSlop = null; // 显示斜坡
        public static object m_renderableAll = null; // 所有灾害


      public    enum Enum_AnalysisType
        {
             TDLYXZ =  0,TDLYGH = 1,TDGY = 2,JBNT = 3,JSYD =4, KCZYGH = 5,CKQ = 6,TKQ = 7
        }
      //public  enum Enum_AnalysisResultTable
      //  {
      //      TDLYXZ = "分析表_土地利用现状_最终结果", TDLYGH = "分析表_规划数据_最终结果", TDGY = "分析表_土地供应_最终结果",
      //      JBNT = "分析表_基本农田_最终结果", JSYD = "分析表_建设用地数据_最终结果", KCZYGH = "分析表_矿产资源规划_最终结果",
      //      CKQ = "分析表_采矿权_最终结果", TKQ = "分析表_探矿权_最终结果"
      //  };

      //public  enum Enum_AnalysisInformationTable
      //  {
      //      TDLYXZ = "分析表_土地利用现状_详细信息", TDLYGH = "分析表_规划数据_详细信息", TDGY = "分析表_土地供应_详细信息",
      //      JBNT = "分析表_基本农田_详细信息", JSYD = "分析表_建设用地数据_详细信息", KCZYGH = "分析表_矿产资源规划_详细信息",
      //      CKQ = "分析表_采矿权_详细信息", TKQ = "分析表_探矿权_详细信息"
      //  };


        // 地质灾害名称列表
        public static string[] m_strDisasterNameList = { "滑坡", "滑坡隐患", "崩塌", "崩塌隐患", "地面塌陷", "地面塌陷隐患", "泥石流", "泥石流隐患", "地裂缝", "采空塌陷", "斜坡" };


        #region // 刘扬 修改 2011后期 土地分析
        public static ArrayList m_listNameOfStaticsLayers = new ArrayList(); // 统计的图层名称

        public static string m_strSettingFileOfStaticsLayers = "统计图层名称.txt"; // 统计图层配置文件名

        // 获取统计图层名称
        public static void GetNameListOfStaticsLayers(string filePath)
        {
            ArrayList theLayerNameList = new ArrayList();// 图层名列表
            ArrayList theLayerTypeList = new ArrayList();// 图层类型名列表（1个类型对应多个图层名称）
            // 获取图层名称和类型
            ReadSettingOfNameOfStaticsLayers(filePath, ref theLayerNameList, ref theLayerTypeList);
            // 构造数据
            BuildDataOfNameOfStaticsLayers(theLayerNameList, theLayerTypeList);
        }




        // 构造数据
        public static void BuildDataOfNameOfStaticsLayers(ArrayList theLayerNameList, ArrayList theLayerTypeList)
        {
            m_listNameOfStaticsLayers.Clear(); // 清空

            for (int i = 0; i < m_listStaticsContents.Length; i++)
            {
                ArrayList therNameList = new ArrayList();// 图层名列表
                m_listNameOfStaticsLayers.Add(therNameList); // 添加图层

            }

            //string type = "";
            //string name = "";
            //int index = -1;
            //for (int i = 0; i < theLayerNameList.Count; i++)
            //{
            //    type = theLayerTypeList[i].ToString().Trim();
            //    name = theLayerNameList[i].ToString().Trim();
            //    index = GetStaticsIndex(type); // 获取类型索引
            //    if(index != -1) // 找到该类型；
            //    {
            //        ArrayList theList = m_listNameOfStaticsLayers[index] as ArrayList;
            //        theList.Add(name);

            //    }
            //}

        }


        // 读取数据
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

                    // 分为2个
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


        // 分离字符串,为2部分
        private static bool SplitToTwoPath(string theLine, ref string theOnePart, ref string theOtherPart)
        {
            int begin = theLine.IndexOf('<');
            int end = theLine.IndexOf('>');

            if (begin == -1 || end == -1) // 出错
                return false;


            theOnePart = theLine.Substring(0, begin).Trim();
            theOtherPart = theLine.Substring(begin + 1, end - begin - 1).Trim();
            return true;
        }

        public static JCZF.Renderable.CGlobalVarable.Enum_AnalysisType m_strCurrentStaticsType; // 当前的统计项目

        public const string m_strFieldNameOfLayerInSQLTable = "所属图层名称"; // 所属图层名称，由系统加入的，保存统计的图层名 在SQL表中

        public const string m_strFieldNameOfLayerInTable = "theLayerN"; // 所属图层名称，由系统加入的，保存统计的图层名 在图层中

        public static ArrayList m_theListOfLayerNamesInNonShowInStatistics = new ArrayList(); // 在统计中没有被显示的图层名称

        public static double[] m_listOfCurrentExtent = { 100.0, 100.0, 400.0, 400.0 }; // 统计前显示的范围，顺序 XMin,YMin,XMax,YMax


        #endregion

        #region // 刘扬 修改 2011 分析

   

        public static string[] m_listStaticsContents = { "土地利用现状", "规划数据", "土地供应", "基本农田", "建设用地数据", "矿产资源规划", "采矿权", "探矿权" }; // 分析内容 此项修改了注意 修改图层名称配置文件“统计图层名称.txt”

        public static string[] m_listTableNameOfStaticsResult = { "FXB_TDLYXZ_Result", "FXB_TDLYGH_Result", "FXB_TDGY_Result", "FXB_JBNT_Result", "FXB_JSYD_Result", "FXB_KCZYGH_Result", "FXB_CKQ_Result", "FXB_TKQ_Result" }; // 分析结果数据表名

        public static string[] m_listTableNameOfStaticsInfo = { "FXB_TDLYXZ_Result_detail", "FXB_TDLYGH_Result_detail", "FXB_TDGY_Result_detail", "FXB_JBNT_Result_detail", "FXB_JSYD_Result_detail", "FXB_KCZYGH_Result_detail", "FXB_CKQ_Result_detail", "FXB_TKQ_Result_detail" }; // 详细信息数据表名

        public static int[] m_listTabIndexOfStaticsInfo = { 0, 1, 2, 3, 4, 5, 6,7 };// 详细信息数据表名

        public static string[] m_listLayerNameOfStatics = { "", "", "", "", "", "", "" ,""}; // 分析图层名

        public static string[] m_listOutLayerNameOfStatics = { "", "", "", "", "", "", "","" }; // 分析图层结果名 刘丽20110814



        public static string[] m_listFeatureIDFieldNameOfStaticsLayer = { "OBJECTID", "OBJECTID", "GDBH", "OBJECTID", "DKID", "KAGHDY_ID", "许可证号", "申请序号" };// Feature的ID字段 添加例子

        public static string[] m_listColNameOfStaticsInfoTable = { "标识码", "标识码", "供地编号", "标识码", "标识码", "标识码", "许可证号", "申请序号" };// 详细信息中的ID字段名称
     
        public static string[] m_listColNameOfStaticsResult = { "DKID", "FXTCMC", "FXJG", "FXSJ" }; // 分析结果表插入列名

        public static string m_strStaticsrResultIDName = "DKID"; // 结果表ID名称

        public static string m_strStaticsrInfoIDName = "DKID"; // 信息表ID名称


        public static string[] m_listColNameOfStaticsInfoInTDLYTable_Chinese = {"土地利用类型名称", "土地利用类型编码", "权属单位名称", "权属单位代码", "坐落单位名称", "坐落单位代码", "所有权地籍号", "所在图幅号", "图斑面积", "坐标", "所属图层名称" };// 土地利用分析详细信息SQL数据库表列名(少编号)
        public static string[] m_listColNameOfStaticsInfoInTDLYTable = { "DLMC", "DLBM", "QSDWMC", "QSDWDM", "ZLDWMC", "ZLDWDM", "QSZDH", "SZTFH", "TBMJ", "ZB", "SSTCMC" };// 土地利用分析详细信息SQL数据库表列名(少编号)
        public static string[] m_listColNameOfStaticsInfoInTDLYLayer = {  "DLMC", "DLBM", "QSDWMC", "QSDWDM", "ZLDWMC", "ZLDWDM", "QSZDH", "SZTFH", "SHAPE_area","ZB", m_strFieldNameOfLayerInTable };// 土地利用分析详细信息图层中列名
        public static int[] m_listColDataTypeOfStaticsInfoInTDLYTable = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 };// 土地利用分析详细信息SQL数据库表的数据类型：1-字符串：2-数字：3-日期


        public static string[] m_listColNameOfStaticsInfoInGHTable_Chinese = { "所在规划区名称", "图斑面积", "坐标", "SSTCMC" };// 规划数据分析详细信息SQL数据库表列名(少编号)
        public static string[] m_listColNameOfStaticsInfoInGHTable = { "FQMC", "TBMJ", "ZB", "所属图层名称" };// 规划数据分析详细信息图层中列名
        public static string[] m_listColNameOfStaticsInfoInGHLayer = { "FQMC", "Shape_area", "ZB", m_strFieldNameOfLayerInTable };// 规划数据分析详细信息图层中列名

        public static string[] m_listColNameOfStaticsInfoInTDGYTable_Chinese = { "供地编号", "位置", "土地用途", "地块ID", "原地块编号", "地块名称", "地块面积", "项目名称", "出让面积", "划拨面积", "供地面积", "供地方式", "批准文号", "批准日期", "流水号", "地块类型", "电子监管号", "单位名称", "审批状态", "坐落行政区代码", "CJRQ", "坐标", "所属图层名称" };// 土地供应数据分析详细信息SQL数据库表列名(少编号)
         public static string[] m_listColNameOfStaticsInfoInTDGYTable = { "GDBH", "ZDWZ", "TDYT", "DKID", "DKBH", "DKMC", "DKMJ", "XMMC", "CRMJ", "HBMJ", "GDMJ", "GDFS", "PZWH", "PZRQ", "FLOWSN", "DKLX", "DZJGH", "DWMC", "SPZT", "ZLXZQDM", "CJRQ", "ZB", "SSTCMC" };// 土地供应数据分析详细信息图层中列名
        public static string[] m_listColNameOfStaticsInfoInTDGYLayer = { "GDBH", "ZDWZ", "TDYT", "DKID", "DKBH", "DKMC", "DKMJ", "XMMC", "CRMJ", "HBMJ", "GDMJ", "GDFS", "PZWH", "PZRQ", "FLOWSN", "DKLX", "DZJGH", "DWMC", "SPZT", "ZLXZQDM", "CJRQ", "ZB", m_strFieldNameOfLayerInTable };// 土地供应数据分析详细信息图层中列名

        public static string[] m_listColNameOfStaticsInfoInJBNTTable_Chinese = { "基本农田图斑编号", "所属地区", "图斑面积", "坐标", "所属图层名称" };// 基本农田分析详细信息SQL数据库表列名(少编号) 添加例子
        public static string[] m_listColNameOfStaticsInfoInJBNTTable = { "JBNTTBBH", "QSDWMC", "TBMJ", "ZB", "SSTCMC" };// 基本农田分析详细信息图层中列名 添加例子
        public static string[] m_listColNameOfStaticsInfoInJBNTLayer = { "JBNTTBBH", "QSDWMC", "Shape_area", "ZB", m_strFieldNameOfLayerInTable };// 基本农田分析详细信息图层中列名 添加例子

        public static string[] m_listColNameOfStaticsInfoInJSYDTable_Chinese = { "勘测名称", "地块名称", "省受理编号", "图幅号", "地块用途", "审批状态", "批准机关", "地块面积", "项目类型", "所属行政区", "项目名称", "省批复文号", "批准日期", "权属批文编号", "图斑面积", "坐标", "所属图层名称" };// 建设用地分析详细信息SQL数据库表列名
        public static string[] m_listColNameOfStaticsInfoInJSYDTable = { "KCMC", "DKMC", "SSLBH", "TFH", "DKYT", "SPZT", "PZJG", "DKMJ", "XMLX", "XZQMC", "XMMC", "SPFWH", "PZRQ", "QSWH", "TBMJ", "ZB", "SSTCMC" };// 建设用地分析详细信息SQL数据库表列名
        public static string[] m_listColNameOfStaticsInfoInJSYDLayer = { "KCMC", "DKMC", "SSLBH", "TFH", "DKYT", "SPZT", "PZJG", "DKMJ", "XMLX", "XZQMC", "XMMC", "SPFWH", "PZRQ", "QSWH", "Shape_area", "ZB", m_strFieldNameOfLayerInTable };// 建设用地分析详细信息图层中列名 


        public static string[] m_listColNameOfStaticsInfoInKCZYTable_Chinese = { "所属矿产规划区名称", "所属矿产规划区类别编号", "规划编制时间", "图斑面积", "坐标", };// 矿产资源分析详细信息SQL数据库表列名
        public static string[] m_listColNameOfStaticsInfoInKCZYTable = { "GHQMC1", "GHQLB1", "GHBZSJ", "TBMJ", "ZB","SSTCMC"};// 矿产资源分析详细信息图层中列名 
        public static string[] m_listColNameOfStaticsInfoInKCZYLayer = { "GHQMC1", "GHQLB1", "GHBZSJ", "Shape_area", "ZB", m_strFieldNameOfLayerInTable };// 矿产资源分析详细信息图层中列名 

        public static string[] m_listColNameOfStaticsInfoInCKQTable_Chinese = { "许可证号", "项目类型", "申请人", "矿山名称", "经济类型", "开采主矿种", "其它主矿种", "设计规模", "采深上限", "采深下限", "矿区面积", "有效期起", "有效期止", "矿体标识", "坐标", "所属图层名称" };// 采矿权分析详细信息SQL数据库表列名
        public static string[] m_listColNameOfStaticsInfoInCKQTable = { "XKZH", "XMLX", "SQR", "KSMC", "JJLX", "KCZKZ", "QTZKZ", "SJGM", "CSSX", "CSXX", "KQMJ", "YXQQ", "YXQZ", "KTBZ", "ZB", "SSTCMC" };// 采矿权分析详细信息SQL数据库表列名
        public static string[] m_listColNameOfStaticsInfoInCKQLayer = { "许可证号", "项目类型", "申请人", "矿山名称", "经济类型", "开采主矿种", "其它主矿种", "设计规模", "采深上限", "采深下限", "矿区面积", "有效期起", "有效期止", "矿体标识", "坐标", m_strFieldNameOfLayerInTable };// 采矿权分析详细信息图层中列名 

        public static string[] m_listColNameOfStaticsInfoInTKQTable_Chinese = { "申请序号", "许可证号", "项目名称", "项目类型", "申请人", "勘查单位", "经济类型", "项目性质", "地理位置", "勘查矿种", "总面积", "有效期起", "有效期止", "图斑面积", "坐标", "所属图层名称" };// 探矿权分析详细信息SQL数据库表列名
        public static string[] m_listColNameOfStaticsInfoInTKQTable = { "SQXH", "XKZH", "XMMC", "XMLX", "SQR", "KCDW", "JJLX", "XMXZ", "DLWZ", "KCKZ", "ZMJ", "YXQQ", "YXQZ", "TBMJ", "ZB", "SSTCMC" };// 探矿权分析详细信息SQL数据库表列名
        public static string[] m_listColNameOfStaticsInfoInTKQLayer = { "申请序号", "许可证号", "项目名称", "项目类型", "申请人", "勘查单位", "经济类型", "项目性质", "地理位置", "勘查矿种", "总面积", "有效期起", "有效期止", "Shape_Area", "坐标", m_strFieldNameOfLayerInTable };// 探矿权分析详细信息图层中列名 


        // 根据统计内容得到统计结果表名
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


        // 根据统计内容得到统计详细信息
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


        // 根据统计内容得到统计图层名称
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



        // 设置一个字符串数组中所有元素为空字符串
        public static void SetNullStringToList(string[] theList)
        {

            for (int i = 0; i < m_listStaticsContents.Length; i++)
            {
                theList[i] = "";
            }


        }

        // 设置统计图层名所有元素为空字符串
        public static void InitialLayerNameOfStatics(string[] theList)
        {

            SetNullStringToList(m_listLayerNameOfStatics);
            SetNullStringToList(m_listOutLayerNameOfStatics);



        }


        // 根据一个列名和值，找到另一个对应的值
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


        // 找到合适的列名
        public static string GetFitalbeColName(Enum_AnalysisType p_Enum_AnalysisType, string theColName)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // 分析类型：土地利用现状
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInTDLYTable, m_listColNameOfStaticsInfoInTDLYLayer, theColName);
                case Enum_AnalysisType.TDLYGH:  // 分析类型：规划数据
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInGHTable, m_listColNameOfStaticsInfoInGHLayer, theColName);
                case Enum_AnalysisType.TDGY:  // 分析类型：规划数据
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInTDGYTable, m_listColNameOfStaticsInfoInTDGYLayer, theColName);
                case Enum_AnalysisType.JBNT:   // 分析类型：基本农田
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInJBNTTable, m_listColNameOfStaticsInfoInJBNTLayer, theColName); // 添加例子
                case Enum_AnalysisType.JSYD:   // 分析类型：建设用地数据
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInJSYDTable, m_listColNameOfStaticsInfoInJSYDLayer, theColName);
                case Enum_AnalysisType.KCZYGH:   // 分析类型：矿产资源规划
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInKCZYTable, m_listColNameOfStaticsInfoInKCZYLayer, theColName);
                case Enum_AnalysisType.CKQ:   // 分析类型：采矿权
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInCKQTable, m_listColNameOfStaticsInfoInCKQLayer, theColName);
                case Enum_AnalysisType.TKQ:   // 分析类型：探矿权
                    return GetCorrespondingValueWithOneStringListAndValue(m_listColNameOfStaticsInfoInTKQTable, m_listColNameOfStaticsInfoInTKQLayer, theColName);
   
                
            }
            return null;
        }


        /// <summary>
        /// 获得数据库中统计详细信息的列名的集合
        /// </summary>
        /// <param name="p_Enum_AnalysisType"></param>
        /// <returns></returns>
        public static string[] GetTableColNameListOfStatisticsInfo(Enum_AnalysisType p_Enum_AnalysisType)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // 分析类型：土地利用现状
                    return m_listColNameOfStaticsInfoInTDLYTable;
                case  Enum_AnalysisType.TDLYGH:  // 分析类型：规划数据
                    return m_listColNameOfStaticsInfoInGHTable;
                case Enum_AnalysisType.TDGY:  // 分析类型：土地供应数据
                    return m_listColNameOfStaticsInfoInTDGYTable;
                case Enum_AnalysisType.JBNT:    // 分析类型：基本农田
                    return m_listColNameOfStaticsInfoInJBNTTable; // 
                case Enum_AnalysisType.JSYD:  // 分析类型：建设用地数据 
                    return m_listColNameOfStaticsInfoInJSYDTable;
                case Enum_AnalysisType.KCZYGH:     // 分析类型：矿产资源规划
                    return m_listColNameOfStaticsInfoInKCZYTable;
                case Enum_AnalysisType.CKQ:   // 分析类型：采矿权
                    return m_listColNameOfStaticsInfoInCKQTable;
                case Enum_AnalysisType.TKQ :   // 分析类型：探矿权
                    return m_listColNameOfStaticsInfoInTKQTable; 
            }
            return null;
        }

        /// <summary>
        /// 找到合适的统计详细信息的列名的中文解释集合
        /// </summary>
        /// <param name="p_Enum_AnalysisType"></param>
        /// <returns></returns>
        public static string[] GetTableColNameListOfStatisticsInfo_Chinese(Enum_AnalysisType p_Enum_AnalysisType)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // 分析类型：土地利用现状
                    return m_listColNameOfStaticsInfoInTDLYTable_Chinese;
                case Enum_AnalysisType.TDLYGH:  // 分析类型：规划数据
                    return m_listColNameOfStaticsInfoInGHTable_Chinese;
                case Enum_AnalysisType.TDGY:  // 分析类型：土地供应数据
                    return m_listColNameOfStaticsInfoInTDGYTable_Chinese;
                case Enum_AnalysisType.JBNT:    // 分析类型：基本农田
                    return m_listColNameOfStaticsInfoInJBNTTable_Chinese; // 
                case Enum_AnalysisType.JSYD:  // 分析类型：建设用地数据 
                    return m_listColNameOfStaticsInfoInJSYDTable_Chinese;
                case Enum_AnalysisType.KCZYGH:     // 分析类型：矿产资源规划
                    return m_listColNameOfStaticsInfoInKCZYTable_Chinese;
                case Enum_AnalysisType.CKQ:   // 分析类型：采矿权
                    return m_listColNameOfStaticsInfoInCKQTable_Chinese;
                case Enum_AnalysisType.TKQ:   // 分析类型：探矿权
                    return m_listColNameOfStaticsInfoInTKQTable_Chinese;
            }
            return null;
        }



        // 找到合适的统计详细信息的列名集合
        public static string[] GetFitableLayerColNameListOfStatisticsInfo(Enum_AnalysisType p_Enum_AnalysisType)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // 分析类型：土地利用现状
                    return m_listColNameOfStaticsInfoInTDLYLayer;
                case  Enum_AnalysisType.TDLYGH:  // 分析类型：规划数据
                    return m_listColNameOfStaticsInfoInGHLayer;
                case Enum_AnalysisType.TDGY:  // 分析类型：规划数据
                    return m_listColNameOfStaticsInfoInTDGYLayer;
                case Enum_AnalysisType.JBNT:    // 分析类型：基本农田
                    return m_listColNameOfStaticsInfoInJBNTLayer; // 添加例子
                case Enum_AnalysisType.JSYD:  // 分析类型：建设用地数据 
                    return m_listColNameOfStaticsInfoInJSYDLayer;
                case Enum_AnalysisType.KCZYGH:    // 分析类型：矿产资源规划
                    return m_listColNameOfStaticsInfoInKCZYLayer;
                case Enum_AnalysisType.CKQ:     // 分析类型：采矿权
                    return m_listColNameOfStaticsInfoInCKQLayer;
                case Enum_AnalysisType.TKQ:     // 分析类型：探矿权
                    return m_listColNameOfStaticsInfoInTKQLayer;
            }
            return null;
        }


        // 找到合适的统计详细信息的列的数据类型
        public static int[] GetFitalbeColDataTypeListOfStatisticsInfo(Enum_AnalysisType p_Enum_AnalysisType)
        {
            switch (p_Enum_AnalysisType)
            {
                case Enum_AnalysisType.TDLYXZ: // 分析类型：土地利用现状
                    return m_listColDataTypeOfStaticsInfoInTDLYTable;
                case  Enum_AnalysisType.TDLYGH:  // 分析类型：规划数据
                    //return m_listColDataTypeOfStaticsInfoInGHTable;
                case Enum_AnalysisType.JBNT:   // 分析类型：基本农田
                    break;
                case Enum_AnalysisType.JSYD:   // 分析类型：建设用地数据
                    break;
                case Enum_AnalysisType.KCZYGH:     // 分析类型：矿产资源规划
                    break;
                case Enum_AnalysisType.CKQ:     // 分析类型：采矿权
                    break;
                case Enum_AnalysisType.TKQ:    // 分析类型：探矿权
                    break;
            }
            return null;
        }


        #endregion // 刘扬 修改 2011 分析

        
        #region // 刘扬 修改 2011 信访

        public static int m_nWhichTypeOfStatistic = 1; // 统计类型：1，2：代表第1种类型，3：代表第2种类型 // 刘扬 修改  2011 信访

        public static JCZF.LetterAndCall.Query.uctLetterAndCallShowStaResult_1 m_theLetterAndCallStaForm1= null; // 统计类型前2项所用窗体

        public static JCZF.LetterAndCall.Query.uctLetterAndCallShowStaResult_2 m_theLetterAndCallStaForm2 = null; // 统计类型第3项所用窗体

        public static JCZF.SubFrame.frmLetterAndCallDetailInfo m_theLetterAndCallDetailInfoForm = null; // 显示投诉详细信息的窗体

        // 得到数组中的索引
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


        // 分离字符串,为3部分
        private static bool SplitToThreeOfListviewShow(string theLine, ref string theColName, ref int theWidth, ref string theFieldName)
        {
            string[] contents = theLine.Split(';');


            if (contents.Length != 3) // 出错
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

                    // 分为3个
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

        // 分离字符串,为2部分
        private static bool SplitToTwoOfListviewColWidth(string theLine, ref string theColName, ref int theWidth)
        {
            int begin = theLine.IndexOf('<');
            int end = theLine.IndexOf('>');

            if (begin == -1 || end == -1) // 出错
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

                    // 分为2个
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


        #endregion // 刘扬 修改 2011 信访

        // 地质灾害图标文件名的列表
        public static string[] m_strIconsNameList = { "iconLandslide.png", "iconLandslideHidden.png", "iconCollapse.png",
                                                        "iconCollapseHidden.png", "iconGroundSubsidence.png", "iconGroundSubsidenceHidden.png", 
                                                        "iconDebrisFlow.png", "iconDebrisFlowHidden.png", "iconGroundFissure.png", "iconMiningCollapse.png", "iconSlop.png" };

        public static System.Windows.Forms.ListView m_listView = null; //  listview

        public static System.Windows.Forms.RadioButton m_radioButtonDisplayNone = null; // radioButtonDisplayNone

        public static System.Windows.Forms.Form m_WorldWindow = null; //  主窗体

        public static JCZF.SubFrame.AttibuteEdit.FormMenuButton m_theSlideForm = null; // 滑动窗体

        public static string m_strExcelFileName = "江西地质灾害点_Geographic_xian80.xls"; // 灾害信息Excel文件名

        public static string m_strDataPath = ""; // 数据路径



        // 查看灾害是否被显示
        public static bool WhichDisasterShow(int which)
        {
            bool isShow = false;  // 默认为不显示

            switch(which)
            {
                case 0: isShow = m_bShowLandslide; break; //  显示滑坡
                case 1: isShow = m_bShowLandslideHidden; break; // 显示滑坡隐患
                case 2: isShow = m_bShowCollapse; break; // 显示崩塌
                case 3: isShow = m_bShowCollapseHidden; break; // 显示崩塌隐患
                case 4: isShow = m_bShowGroundSubsidence; break; // 显示地面塌陷
                case 5: isShow = m_bShowGroundSubsidenceHidden; break; // 显示地面塌陷隐患
                case 6: isShow = m_bShowDebrisFlow; break; // 显示泥石流
                case 7: isShow = m_bShowDebrisFlowHidden; break; // 显示泥石流隐患
                case 8: isShow = m_bShowGroundFissure; break; // 显示地裂缝
                case 9: isShow = m_bShowMiningCollapse; break;// 显示采空塌陷
                case 10: isShow = m_bShowSlop; break; // 显示斜坡
                default: break; // 其他灾害
            }

            return isShow;// 返回灾害是否被显示

        }

        // 更新listview
        public static void DoUpdateListviewOther()
        {
            m_listView.Items[11].Checked = false; // 其他灾害
        }

        // 更新listview
        public static void DoUpdateListview()
        {
            if (m_listView == null) // 如果listview为空
                return;

            //for (int i = 0; i < m_listView.Items.Count; i++)

            int length = m_strIconsNameList.Length;

            for (int i = 0; i < length; i++) // 更新灾害
            {
                m_listView.Items[i].Checked = WhichDisasterShow(i); // 崩塌
            }
          
            

            //m_listView.Items[0].Checked = m_bShowLandslide; // 崩塌
            //m_listView.Items[1].Checked = m_bShowLandslide; // 滑坡
            //m_listView.Items[2].Checked = m_bShowDebrisFlow; // 泥石流
            //m_listView.Items[3].Checked = m_bShowGroundSubsidence; // 地面塌陷
            //m_listView.Items[0].Checked = m_bShowCollapse; // 崩塌
            //m_listView.Items[1].Checked = m_bShowLandslide; // 滑坡
            //m_listView.Items[2].Checked = m_bShowDebrisFlow; // 泥石流
            //m_listView.Items[3].Checked = m_bShowGroundSubsidence; // 地面塌陷
        }


       

    }
}
