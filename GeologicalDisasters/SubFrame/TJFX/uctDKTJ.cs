using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase ;
using System.Collections;

namespace JCZF.SubFrame.TJFX
{
    public partial class uctDKTJ : UserControl
    {

        public string m_strTitleText;
        public clsDataAccess.DataAccess m_DataAccess_SYS;
        public DevComponents.DotNetBar.ExpandablePanel m_ExpandablePanel_Main;
        //public DevComponents.DotNetBar.TabControl m_TabControl_Main;
        private IFeatureLayer m_IFeatureLayer;
        public  Panel m_Panel;
        private JCZF.SubFrame.Query.utcDataQueryShow m_frmDataQueryShow;
        public string m_strAction;
        
        //public DataAccess m_DataAccess_SDE;
        private string m_strTDXCLayerName = "土地核查";
        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {               
                m_AxMapControl_ = value;
                uctXZQTree_Dev1.m_AxMapControl = value;
                
            }
        }

        public uctDKTJ()
        {
            InitializeComponent();
        }

        private void btnTJBB_OK_Click(object sender, EventArgs e)
        {
            try
            {
                labInfo.Text = "没有符合条件的记录！";
                string m_strSQL = "";
                if (m_ExpandablePanel_Main == null) return;

                m_strSQL = CreatSQL();
                DataTable m_DataTable = GetDataTable(m_strSQL);

                if (m_Panel == null)
                {
                    m_Panel = (Panel)clsFunction.Function.GetControl(m_ExpandablePanel_Main, "PanelInfo_Panel");
                    //ClearControls(m_Panel);
                    clsFunction.Function.ClearControls(m_Panel);
                }
                if (m_strTitleText == null || m_strTitleText == "")
                {
                    if (m_strAction.Trim().ToLower() == "edit")
                    {
                        m_ExpandablePanel_Main.Text = "调查地块编辑";
                        m_ExpandablePanel_Main.TitleText = "调查地块编辑";
                    }
                    else
                    {
                        m_ExpandablePanel_Main.Text = "调查结果统计";
                        m_ExpandablePanel_Main.TitleText = "调查结果统计";
                    }
                }
                else
                {
                    m_ExpandablePanel_Main.Text = m_strTitleText;
                    m_ExpandablePanel_Main.TitleText = m_strTitleText;
                }
               

                if (m_DataTable == null)
                {
                    m_ExpandablePanel_Main.Visible = false ;
                    //m_ExpandablePanel_Main.Expanded = false;
                    clsFunction.Function.ClearControls(m_Panel);
                    //clsFunction.Function.MessageBoxInformation("没有符合条件的记录！");
                    labInfo.Text = "没有符合条件的记录！";
                    return;
                }

                if (clsFunction.Function.HasControl(m_Panel, "m_uctDataQueryShow") == false)
                {
                    clsFunction.Function.ClearControls(m_Panel);
                    m_frmDataQueryShow = new JCZF.SubFrame.Query.utcDataQueryShow();
                    m_frmDataQueryShow.Name = "m_uctDataQueryShow";
                    m_frmDataQueryShow.Dock = DockStyle.Fill;
                    m_Panel.Controls.Add(m_frmDataQueryShow);
                }

                labInfo.Text = "共有 "+m_DataTable.Rows.Count+" 个符合条件的地块！" ;
                m_frmDataQueryShow.FillListView(m_DataTable);               
                m_frmDataQueryShow.m_DataAccess_SYS = m_DataAccess_SYS;
                m_frmDataQueryShow.m_IFeatureLayer = m_IFeatureLayer;
                m_frmDataQueryShow.m_AxMapControl = m_AxMapControl_;

                if (m_strAction==null || m_strAction.Trim().ToLower() == "edit")
                {
                    m_frmDataQueryShow.groupPanel_browse.Visible = false;
                    m_frmDataQueryShow.groupPanel_Edit.Visible = true;
                }
                else
                {
                    m_frmDataQueryShow.groupPanel_browse.Visible = true;
                    m_frmDataQueryShow.groupPanel_Edit.Visible = false;
                }

                m_ExpandablePanel_Main.Visible = true;
                m_ExpandablePanel_Main.Expanded = true;
               
            }
            catch(SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
           
        }

        private string  CreatSQL()
        {
            string m_strSQL = "";
            string m_strSQL1 = "";
            string m_strSQL2 = "";
            //int m_intND1 = 0;
            //int m_intND2 = 0;
            DateTime m_DateTime1;
            DateTime m_DateTime2;

            m_DateTime1 = dateTimePicker1.Value;
            m_DateTime2 = dateTimePicker2.Value;

            if (radJG_HF.Checked)
            {
                m_strSQL = " (HFXSC LIKE '%合法%' or HFXSC='0') ";
            }
            else if (radJG_WF.Checked)
            {
                m_strSQL = " (HFXSC LIKE '%违法%')";
            }
            else if (radJG_QB.Checked)
            {
               
            }

            if (radLX_HC.Checked)
            {
                m_strSQL1 = " (QSX <>'') ";
                m_DateTime1 = m_DateTime1.AddYears(-1);//表中的hsx比实际的时间早一年
                //m_DateTime2 = m_DateTime2.AddYears(-1);//表中的hsx比实际的时间早一年
            }
            else if (radLX_XC.Checked)
            {
                m_strSQL1 = " (HSX <>'' and (QSX ='' or QSX is null) )";
            }
            else if (radLX_QB.Checked)
            {

            }
            if (m_strSQL != "" && m_strSQL1!= "")
            {
                m_strSQL =m_strSQL +" and "+ m_strSQL1;
            }
            else if (m_strSQL == "" && m_strSQL1 != "")
            {
                m_strSQL =  m_strSQL1;
            }
            
            m_strSQL2 = " ( hsx>='" + m_DateTime1.ToShortDateString() + "' and hsx<='" + m_DateTime2.ToShortDateString() + "' ) ";

            if (m_strSQL != "")
            {
                m_strSQL = m_strSQL + " and " + m_strSQL2;
            }
            else 
            {               
                m_strSQL = m_strSQL2;
           
            }

            string m_strXZQDM = "";
            m_strXZQDM = uctXZQTree_Dev1.m_strXZQDM;

            m_strSQL = " (left(xzqdm," + m_strXZQDM.Length + ")='" + m_strXZQDM + "' ) and  (" + m_strSQL2 + ")";


            return m_strSQL;
        }

        private DataTable GetDataTable(string p_strSQL)
        {
            ArrayList m_ArrayOfFieldName = new ArrayList();
            DataTable m_DataTable = new DataTable("Property");
            DataColumn m_DataColumn;
            DataRow m_DataRow;

            try
            {

                //string m_strFieldName;
                m_IFeatureLayer = clsMapFunction.MapFunction.getFeatureLayerByName(m_strTDXCLayerName, m_AxMapControl_);
                IField m_IField;
                IFeature m_Feature;
                IFeatureCursor m_IFeatureCursor;
                IQueryFilter m_IQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
                m_IQueryFilter.WhereClause = p_strSQL;

                m_IFeatureCursor = m_IFeatureLayer.Search(m_IQueryFilter, false);
                m_Feature = m_IFeatureCursor.NextFeature();

                if (m_Feature == null) return null;
                for (int i = 0; i < m_Feature.Fields.FieldCount; i++)
                {
                    m_IField = m_Feature.Fields.get_Field(i);
                    if (m_IField.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        //每个字段为一列
                        m_DataColumn = new DataColumn();
                        m_DataColumn.ColumnName = m_IField.Name;
                        m_DataColumn.ReadOnly = true;
                        m_DataTable.Columns.Add(m_DataColumn);
                        //字段数组
                        m_ArrayOfFieldName.Add(m_IField.Name);
                    }
                }

                while (m_Feature != null)
                {
                    m_DataRow = m_DataTable.NewRow();
                    int index = 0;
                    //获取对应字段的值
                    for (int j = 0; j < m_ArrayOfFieldName.Count; j++)
                    {
                        string FieldName = (string)m_ArrayOfFieldName[j];
                        index = m_Feature.Fields.FindField(FieldName);
                        m_DataRow[FieldName] = m_Feature.get_Value(index).ToString();
                    }

                    m_DataTable.Rows.Add(m_DataRow);
                    m_Feature = m_IFeatureCursor.NextFeature();
                }
            }
            catch (SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
            return m_DataTable;
        }

        //private void ClearControls(Panel p_Panel)
        //{
        //    if (p_Panel == null) return;
        //    for (int i = 0; i < p_Panel.Controls.Count; i++)
        //    {
        //        p_Panel.Controls.RemoveAt(i);
        //    }          
        //}
        //private bool HasAddControl(Panel p_Panel,string  p_strControlName)
        //{
        //    if (p_Panel == null || p_strControlName== null  || p_strControlName.Trim() == "") return false ;
        //    for (int i = 0; i < p_Panel.Controls.Count; i++)
        //    {
        //        if (p_Panel.Controls[i].Name == p_strControlName)
        //        {
        //            return true ;                    
        //        }
        //    }
        //    return false;
        //}
        //private Panel GetPanelInfo_Panel()
        //{
        //    if (m_ExpandablePanel_Main == null) return null ;
        //    Panel m_Panel = null;
        //    for (int i = 0; i < m_ExpandablePanel_Main.Controls.Count; i++)
        //    {
        //        if (m_ExpandablePanel_Main.Controls[i].Name == "PanelInfo_Panel")
        //        {
        //            m_Panel = (Panel)m_ExpandablePanel_Main.Controls[i];
        //            break;
        //        }
        //    }
        //    return m_Panel;
        //}
       

        private void uctTJBB_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
            dateTimePicker2.Value = DateTime.Now;
        }
    }
}
