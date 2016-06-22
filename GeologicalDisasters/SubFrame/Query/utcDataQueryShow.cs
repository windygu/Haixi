using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JCZF.MainFrame;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;


namespace JCZF.SubFrame.Query
{
    public partial class utcDataQueryShow : UserControl
    {
        public clsDataAccess.DataAccess m_DataAccess_SYS;
        private SubFrame.AttibuteEdit.frmAttributeEdit m_frmAttributeEdit;
        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                m_AxMapControl_ = value;
               
            }
        }
        private IFeatureLayer m_IFeatureLayer_;
        public IFeatureLayer m_IFeatureLayer
        {
            set
            {
                m_IFeatureLayer_ = value;
            }
        }
        //public delegate void ZoomToFeaEventHandler(string OID);
        //public event ZoomToFeaEventHandler ZoomToFea;

        //private frmMain parenForm;
        private string strIsOK = "1";
        public utcDataQueryShow()
        {
            InitializeComponent();
            //this.parenForm = parenForm;
        }

        private void frmDataQueryShow_DoubleClick(object sender, EventArgs e)
        {
           
            Functions.MapFunction.ZoomToSelFeaByFID(m_IFeatureLayer_.Name, "objectid", this.listView1.SelectedItems[0].SubItems[1].Text.ToString(), m_AxMapControl_);
        }

        private void frmDataQueryShow_Load(object sender, EventArgs e)
        {
            //this.MdiParent = this.parenForm;
            groupPanel1.Height = 36;
        }

        public void FillListView(DataTable p_DataTable)
        {
           listView1.Clear();

           string[] s1 = new string[p_DataTable.Columns.Count+1];

           listView1.Columns.Add("", 10, HorizontalAlignment.Left);
            for (int i = 1; i < p_DataTable.Columns.Count+1; i++)
            {
                listView1.Columns.Add(p_DataTable.Columns[i-1].ColumnName, 100, HorizontalAlignment.Left);
            }

            for (int i = 0; i < p_DataTable.Rows.Count; i++)
            {
                for (int j = 1; j < listView1.Columns.Count; j++)
                {
                    s1[j] = p_DataTable.Rows[i][j-1].ToString();

                }
                ListViewItem lvi = new ListViewItem(s1);
                listView1.Items.Add(lvi);
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog pdlg = new FolderBrowserDialog();
            saveFileDialog1.Filter = "EXCEL files (*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string m_FilePath = saveFileDialog1.FileName;
            txtPath.Text = m_FilePath;
            
        }

        //导出
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string excelpath = txtPath.Text;
                if (txtPath.Text == "")
                {
                    //MessageBox.Show("选择导出路径！");

                    saveFileDialog1.FileName = "Export.xls";
                    saveFileDialog1.DefaultExt = "xls";
                    saveFileDialog1.Filter = "EXCEL files (*.xls)|*.xls";
                    saveFileDialog1.InitialDirectory = "c:\\";
                    DialogResult result = saveFileDialog1.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        return;
                    }
                    excelpath = saveFileDialog1.FileName;

                }

                //SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                //SaveFileDialog1.CreatePrompt = true;
                //SaveFileDialog1.OverwritePrompt = true;


                // get files that contain  土地核查, then delet them

                string[] dirs = null;
                txtPath.Text = excelpath;
                if (File.Exists(excelpath))
                {
                    File.Delete(excelpath);
                }
                //dirs = Directory.GetFiles(excelpath);
                //foreach (string dir in dirs)
                //{
                //    File.Delete(dir);
                //}



                //string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\temp\aa2.xls" + ";Extended Properties=Excel 8.0;";
                string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelpath + ";Extended Properties=Excel 8.0;";
                System.Data.OleDb.OleDbConnection objConn = new System.Data.OleDb.OleDbConnection(connString);
                System.Data.OleDb.OleDbCommand objCmd = new System.Data.OleDb.OleDbCommand();

                objCmd.Connection = objConn;
                try
                {

                    //建立表结构
                    objCmd.CommandText = @"CREATE TABLE 统计结果(序号 VarChar(50),行政区代码 VarChar(50),区县名称 VarChar(50),乡镇名称 VarChar(50),
                                前时相 VarChar(50),后时相 VarChar(50),X坐标 VarChar(50),Y坐标 VarChar(50),监察编号 VarChar(50),实际地类编码 VarChar(50),前地类编码 VarChar(50),
                                占用基本农田 VarChar(50),基本农田面积 VarChar(50),后地类编码 VarChar(50),监察面积 VarChar(50),是否变化 VarChar(50),线状地物KD VarChar(50),WYHSZP VarChar(50),备注 VarChar(200),地块编号 VarChar(50),地块分类 VarChar(50),地块面积 VarChar(50),
                                农用地面积 VarChar(50),耕地面积 VarChar(50),基本农田面积1 VarChar(50),未利用地面积 VarChar(50),实际用途 VarChar(50),用地单位 VarChar(50),项目类型 VarChar(50),合法性审查 VarChar(50),
                               违法类型 VarChar(50),图斑编号x VarChar(50),乡镇代码 VarChar(50),地块标识 VarChar(50))";

                    try
                    {

                        objConn.Open();
                    }
                    catch (OleDbException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    objCmd.ExecuteNonQuery();

                    objCmd.Connection = objConn;

                    ////获取 数据集 
                    //string sql = tdjc.SubFrame.CGGL.DatabaseString.DBConnectString1();

                    //string select = "SELECT * FROM 土地核查";
                    //System.Data.OleDb.OleDbConnection mycon = new System.Data.OleDb.OleDbConnection(sql);
                    //System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(select, mycon);
                    //DataSet ds = new DataSet();
                    //da.Fill(ds, "土地核查");

                    //插入 数据
                    //建立插入动作的Command
                    objCmd.CommandText = @"INSERT INTO 统计结果(序号 ,行政区代码 ,区县名称 ,乡镇名称 ,前时相 ,后时相 ,X坐标 ,Y坐标 ,监察编号 ,实际地类编码 ,前地类编码 ,
                                占用基本农田 ,基本农田面积 ,后地类编码 ,监察面积,是否变化 ,线状地物KD ,WYHSZP ,备注 ,地块编号 ,地块分类 ,地块面积 ,
                                农用地面积,耕地面积,基本农田面积1,未利用地面积,实际用途,用地单位,项目类型,合法性审查,违法类型,图斑编号x,乡镇代码,地块标识)
                                VALUES(@objectid,@XZQDM,@XMC,@XZMC,@QSX,@HSX,@XZB,@YZB,@JCBH,@SJKDLBM,@QDLBM,@ZYJBNT,@JBNTMJ,@HDLBM,@JCMJ,@SFBH,
                                @XZDWKD,@WYHSZP,@BZ,@DKBH,@DKFL,@DKMJ,@NYDMJ,@GDMJ,@JBNTMJI,@WLYDMJ,@SJYT,@YDDW,@XMLX,@HFXSC,@WFLX,@TBBHX,@XZ,@DKID)";

                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@objectid", System.Data.OleDb.OleDbType.Integer));

                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@XZQDM", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@XMC", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@XZMC", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@QSX", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@HSX", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@XZB", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@YZB", System.Data.OleDb.OleDbType.VarChar));

                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@JCBH", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@SJKDLBM", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@QDLBM", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@ZYJBNT", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@JBNTMJ", System.Data.OleDb.OleDbType.VarChar));

                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@HDLBM", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@JCMJ", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@SFBH", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@XZDWKD", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@WYHSZP", System.Data.OleDb.OleDbType.VarChar));

                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@BZ", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@DKBH", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@DKFL", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@DKMJ", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@NYDMJ", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@GDMJ", System.Data.OleDb.OleDbType.VarChar));

                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@JBNTMJI", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@WLYDMJ", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@SJYT", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@YDDW", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@XMLX", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@HFXSC", System.Data.OleDb.OleDbType.VarChar));


                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@WFLX", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@TBBHX", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@XZ", System.Data.OleDb.OleDbType.VarChar));
                    objCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@DKID", System.Data.OleDb.OleDbType.VarChar));


                    try
                    {
                        for (int i = 0; i < this.listView1.Items.Count; i++)
                        {
                            for (int j = 0; j < objCmd.Parameters.Count; j++)
                            {
                                objCmd.Parameters[j].Value = this.listView1.Items[i].SubItems[j+1].Text;
                            }
                            objCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception errs)
                    {
                        //MessageBox.Show(ee.ToString());
                        clsFunction.Function.MessageBoxError(errs.Message);
                        
                    }


                    objConn.Close();

                    //if (strIsOK == "0")
                    //    MessageBox.Show("导出失败！", "导出失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //else
                    clsFunction.Function.MessageBoxInformation("导出成功！\n\n已导出至" + excelpath);
                        //MessageBox.Show("导出成功！\n\n已导出至" + excelpath, "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(SystemException errs)
                {
                    clsFunction.Function.MessageBoxError(errs.Message);
                    if (objConn.State == ConnectionState.Open)
                    {
                        objConn.Close();
                    }
                }
            }
            catch(SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message );
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this.Close();
        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            labelXInfo.Text = "";
            //this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            labelXInfo.Text = "";
            if (this.listView1.SelectedItems == null || this.listView1.SelectedItems.Count < 1)
            {
                clsFunction.Function.MessageBoxInformation("请选择一条记录！");
                return;
            }
            if (m_frmAttributeEdit == null || m_frmAttributeEdit.IsDisposed)
            {
                m_frmAttributeEdit = new AttibuteEdit.frmAttributeEdit();
            }

            m_frmAttributeEdit.m_featureClass = m_IFeatureLayer_.FeatureClass;
            m_frmAttributeEdit.m_strObjectID = this.listView1.SelectedItems[0].SubItems[1].Text.ToString();
            m_frmAttributeEdit.m_selectedFeature = GetSelectedFeature();
            m_frmAttributeEdit.m_strOID =System.Convert.ToString( m_frmAttributeEdit.m_selectedFeature.OID);
            m_frmAttributeEdit.m_DataAccess_SYS = m_DataAccess_SYS;
            m_frmAttributeEdit.ShowDialog();

        }

        private IFeature GetSelectedFeature()
        {
            IFeature m_IFeature;
            IFeatureCursor m_IFeatureCursor;
            if (m_IFeatureLayer_ == null) return null ;
            string m_strOBJECTID = this.listView1.SelectedItems[0].SubItems[1].Text.ToString();
            IQueryFilter m_QueryFilter = new QueryFilter() as IQueryFilter ;
            m_QueryFilter.WhereClause = " OBJECTID='" + m_strOBJECTID + "'";

            m_IFeatureCursor = m_IFeatureLayer_.Search(m_QueryFilter, false);

            m_IFeature = m_IFeatureCursor.NextFeature();
            return m_IFeature;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            labelXInfo.Text = "";
            try
            {
                IFeature m_IFeature;
                //IFeatureCursor m_IFeatureCursor;
                //if (m_IFeatureLayer_ == null) return;
                //string m_strOBJECTID = this.listView1.SelectedItems[0].SubItems[0].Text.ToString();
                //IQueryFilter m_QueryFilter = new QueryFilter() as IQueryFilter ;
                //m_QueryFilter.WhereClause = " OBJECTID='" + m_strOBJECTID + "'";

                //m_IFeatureCursor = m_IFeatureLayer_.Search(m_QueryFilter, false);

                //m_IFeature = m_IFeatureCursor.NextFeature();
                //if (m_IFeature == null) return;

                m_IFeature = GetSelectedFeature();

                IWorkspaceEdit workspaceEdit = ((IDataset)m_IFeatureLayer_.FeatureClass).Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                m_IFeature.Delete();

                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);

                this.listView1.Items.Remove(listView1.SelectedItems[0]);
                m_AxMapControl_.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                labelXInfo.Text = "删除成功！";
               

            }
            catch(SystemException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
        }
        
    }
}