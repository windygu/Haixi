using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using JCZF.SubFrame;
using Functions;
using ESRI.ArcGIS.Geodatabase;
namespace JCZF.SubFrame.AttibuteEdit
{
    public partial class frmAttributeEdit : DevComponents.DotNetBar.Office2007Form
    {
        public string m_strTabelName;
        public clsDataAccess.DataAccess m_DataAccess_SYS;
        public string m_strObjectID;

        public string m_strOID;
        public IFeatureClass m_featureClass;
        public string m_strDKID;
        

        public IFeature m_selectedFeature;

        private string FileName = "";
        JCZF.SubFrame.AssistantFunction m_AssistantFunction = new JCZF.SubFrame.AssistantFunction();

        public frmAttributeEdit()
        {
            InitializeComponent();
        }

        private void frmAttributeEdit_Load(object sender, EventArgs e)
        {
            try
            {
                listViewEx1.Items.Clear();
                //JCZF.SubFrame.DatabaseString.m_DataAccess_SYS = this.m_DataAccess_SYS;

                ////cast the spatial filter to the IQueryFilter interface
                //IQueryFilter queryFilter = new QueryFilter() as IQueryFilter ;
                //queryFilter.WhereClause = "objectid = '"+m_strObjectID+"'";
                ////queryFilter.SubFields = "FID, Type";
                ////IFeatureClass m_featureClass=MapFunction.getFeatureLayerByName("���غ˲�",);

                ////preform the search on the supplied feature class; use a cursor to hold the results
                //IFeatureCursor featureCursor = m_featureClass.Search(queryFilter, false);

                //IFeature feature = featureCursor.NextFeature();
                IRow row = m_selectedFeature as IRow;

                string m_strSQL = "";

                if (row != null)
                {
                    ShowData(row);

                    //string m_strDKID = row.get_Value(row.Fields.FindField("dkid")).ToString();
                    if (m_strDKID != "" && m_strDKID != null)
                    {
                        m_strSQL = "select * from doc where dkid= '" + m_strDKID + "'";
                    }
                    
                    else
                    {
                        return;                       
                    }

                    DataRowCollection m_DataRowCollection = m_DataAccess_SYS.getDataRowsByQueryString(m_strSQL);
                    if (m_DataRowCollection != null && m_DataRowCollection.Count > 0)
                    {
                        if (m_DataRowCollection[0]["Note"] != null)
                        {
                            txtNote.Text = m_DataRowCollection[0]["Note"].ToString();
                        }
                        if (m_DataRowCollection[0]["danwei"] != null)
                        {
                            txtDW.Text = m_DataRowCollection[0]["danwei"].ToString();
                        }
                        if (m_DataRowCollection[0]["writer"] != null)
                        {
                            txtPSRY.Text = m_DataRowCollection[0]["writer"].ToString();
                        }
                        if (m_DataRowCollection[0]["shijian"] != null)
                        {
                            try
                            {
                                dateTimePicker1.Value = System.Convert.ToDateTime(m_DataRowCollection[0]["shijian"]);
                            }
                            catch (SystemException errs)
                            {
                                dateTimePicker1.Value = DateTime.Now;
                            }
                        }

                        //txtNote.Text = m_DataRowCollection[0]["Note"];

                        for (int i = 0; i < m_DataRowCollection.Count; i++)
                        {
                            listViewEx1.Items.Add((i + 1).ToString());
                            listViewEx1.Items[i].SubItems.Add(m_DataRowCollection[i]["MC"].ToString());
                        }
                    }
                    else
                    {
                        txtNote.Text = "";
                        txtDW.Text ="";
                        txtPSRY.Text = "";
                        dateTimePicker1.Value = DateTime.Now ;
                    }
                }

            }
            catch(System.Exception errs)
            {
                //
                MessageBox.Show(errs.Message);
            }
        }

        

        //private void ShowData(DataRow p_DataRow)
        private void ShowData(IRow p_DataRow)
        {

            this.txtXZQDM.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("XZQDM")).ToString();
            this.txtXMC.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("XMC")).ToString();
            this.txtXZMC.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("XZMC")).ToString();
            this.txtQSX.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("QSX")).ToString();

            this.txtHSX.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("HSX")).ToString();
            this.txtXZB.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("XZB")).ToString();
            this.txtYZB.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("YZB")).ToString();
            this.txtJCBH.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("JCBH")).ToString();
            try
            {
                this.txtJCMJ.Text = System.Convert.ToDouble(p_DataRow.get_Value(p_DataRow.Fields.FindField("JCMJ"))).ToString("#0.00");
            }
            catch
            {
                this.txtJCMJ.Text = "";
            }
            this.txtBZ.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("BZ")).ToString();

            this.txtDKBH.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("DKBH")).ToString();
            this.txtDKFL.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("DKFL")).ToString();
            try
            {
                this.txtDKMJ.Text = System.Convert.ToDouble(p_DataRow.get_Value(p_DataRow.Fields.FindField("DKMJ"))).ToString("0.00");
            }
            catch
            {
                this.txtDKMJ.Text = "";
            }
            try
            {
                this.txtGDMJ.Text = System.Convert.ToDouble(p_DataRow.get_Value(p_DataRow.Fields.FindField("GDMJ"))).ToString("0.00");
            }
            catch
            {
                this.txtGDMJ.Text = "";
            }
            try
            {
                this.txtJBNTMJ.Text = System.Convert.ToDouble(p_DataRow.get_Value(p_DataRow.Fields.FindField("JBNTMJ"))).ToString("0.00");
            }
            catch
            {
                this.txtJBNTMJ.Text = "";
            }
            try
            {
                this.txtWLYDMJ.Text = System.Convert.ToDouble(p_DataRow.get_Value(p_DataRow.Fields.FindField("WLYDMJ"))).ToString("0.00");
            }
            catch
            {
                this.txtWLYDMJ.Text = "";
            }
            try
            {
                this.txtNYDMJ.Text = System.Convert.ToDouble(p_DataRow.get_Value(p_DataRow.Fields.FindField("NYDMJ"))).ToString("0.00");
            }
            catch
            {
                this.txtNYDMJ.Text = "";
            }
            this.txtSJYT.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("SJYT")).ToString();
            this.txtYDDW.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("YDDW")).ToString();
            this.txtXMLX.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("XMLX")).ToString();

            this.txtHFXSC.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("HFXSC")).ToString();
            this.txtWFLX.Text = p_DataRow.get_Value(p_DataRow.Fields.FindField("WFLX")).ToString();

        }

        private void m_btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool m_blSaveZiliao = true;
                bool m_blSaveData = false;

                //���ȱ�������
                m_blSaveData = SaveData();

                if (m_blSaveData == false)
                {
                    return;
                }
               
                //�����������Ƭ
                    if (listViewEx1.Items.Count > 0)
                    {
                        m_blSaveZiliao = SaveZiLiao();
                    }
               

                
                if (m_blSaveZiliao == false)
                {
                    return;
                }

               

               
                    clsFunction.Function.MessageBoxInformation("��ϲ�ɹ�", "����ɹ���");
               

            }
            catch (Exception ex)
            {
            }
        }


        private bool  SaveData()
        {
            try
            {
                ////IQueryFilter queryFilter = new QueryFilter() as IQueryFilter ;

                ////queryFilter.WhereClause = "OBJECTID = '" + m_strObjectID + "'";//arcsde�����������
                ////queryFilter.WhereClause = "OBJECTID = " + m_strObjectID ;//filegeodatabase��ֻ��������
               
                ////IFeatureCursor featureCursor = m_featureClass.Search(queryFilter, false);
                ////IFeature feature = featureCursor.NextFeature();

                //IWorkspaceEdit workspaceEdit = (m_featureClass as IDataset).Workspace as IWorkspaceEdit;
                //workspaceEdit.StartEditing(true);
                //workspaceEdit.StartEditOperation();

                 
                // m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("DKID"),m_strDKID  );
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("BZ"), this.txtBZ.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("XZQDM"), this.txtXZQDM.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("XMC"), this.txtXMC.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("QSX"), this.txtQSX.Text);

                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("HSX"), this.txtHSX.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("XZB"), this.txtXZB.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("YZB"), this.txtYZB.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("JCBH"), this.txtJCBH.Text);

                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("JCMJ"), this.txtJCMJ.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("DKFL"), this.txtDKFL.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("DKMJ"), this.txtDKMJ.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("NYDMJ"), this.txtNYDMJ.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("GDMJ"), this.txtGDMJ.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("JBNTMJ"), this.txtJBNTMJ.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("WLYDMJ"), this.txtWLYDMJ.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("SJYT"), this.txtSJYT.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("YDDW"), this.txtYDDW.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("XMLX"), this.txtXMLX.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("HFXSC"), this.txtHFXSC.Text);
                //m_selectedFeature.set_Value(m_selectedFeature.Fields.FindField("WFLX"), this.txtWFLX.Text);
               
                //m_selectedFeature.Store();

               
               
                //workspaceEdit.StopEditOperation();
                //workspaceEdit.StopEditing(true);

                string[,] m_strFiledValues = new string[21, 2];

                  m_strFiledValues[0,0]="DKID";m_strFiledValues[0,1]=m_strDKID ;

                //m_strFiledValues[]={"BZ"), this.txtBZ.Text);
                 m_strFiledValues[1,0]="BZ";m_strFiledValues[1,1]=this.txtBZ.Text ;
                //m_strFiledValues[]={"XZQDM"), this.txtXZQDM.Text);
                 m_strFiledValues[2,0]="XZQDM";m_strFiledValues[2,1]=this.txtXZQDM.Text ;
                //m_strFiledValues[]={"XMC"), this.txtXMC.Text);
                 m_strFiledValues[3,0]="XMC";m_strFiledValues[3,1]=this.txtXMC.Text ;
                //m_strFiledValues[]={"QSX"), this.txtQSX.Text);
                 m_strFiledValues[4,0]="QSX";m_strFiledValues[4,1]=this.txtQSX.Text ;
                //m_strFiledValues[]={"HSX"), this.txtHSX.Text);
                 m_strFiledValues[5,0]="HSX";m_strFiledValues[5,1]=this.txtHSX.Text ;
                //m_strFiledValues[]={"XZB"), this.txtXZB.Text);
                 m_strFiledValues[6, 0] = "XZB"; m_strFiledValues[6, 1] = (this.txtXZB.Text.Trim() != "") ? this.txtXZB.Text.Trim() : "0";
                //m_strFiledValues[]={"YZB"), this.txtYZB.Text);
                 m_strFiledValues[7, 0] = "YZB"; m_strFiledValues[7, 1] = (this.txtYZB.Text.Trim() != "") ? this.txtXZB.Text.Trim() : "0"; 
                //m_strFiledValues[]={"JCBH"), this.txtJCBH.Text);
                 m_strFiledValues[8,0]="JCBH";m_strFiledValues[8,1]=this.txtJCBH.Text ;

                //m_strFiledValues[]={"JCMJ"), this.txtJCMJ.Text);
                 m_strFiledValues[9, 0] = "JCMJ"; m_strFiledValues[9, 1] = (this.txtJCMJ.Text.Trim() != "") ? this.txtXZB.Text.Trim() : "0"; 
                //m_strFiledValues[]={"DKFL"), this.txtDKFL.Text);
                 m_strFiledValues[10,0]="DKFL";m_strFiledValues[10,1]=this.txtDKFL.Text ;
                //m_strFiledValues[]={"DKMJ"), this.txtDKMJ.Text);
                 m_strFiledValues[11, 0] = "DKMJ"; m_strFiledValues[11, 1] = (this.txtDKMJ.Text.Trim() != "") ? this.txtXZB.Text.Trim() : "0"; 
                //m_strFiledValues[]={"NYDMJ"), this.txtNYDMJ.Text);
                 m_strFiledValues[12, 0] = "NYDMJ"; m_strFiledValues[12, 1] = (this.txtNYDMJ.Text.Trim() != "") ? this.txtXZB.Text.Trim() : "0"; 
                //m_strFiledValues[]={"GDMJ"), this.txtGDMJ.Text);
                 m_strFiledValues[13, 0] = "GDMJ"; m_strFiledValues[13, 1] = (this.txtGDMJ.Text.Trim() != "") ? this.txtXZB.Text.Trim() : "0"; 

                //m_strFiledValues[]={"JBNTMJ"), this.txtJBNTMJ.Text);
                 m_strFiledValues[14, 0] = "JBNTMJ"; m_strFiledValues[14, 1] = (this.txtJBNTMJ.Text.Trim() != "") ? this.txtXZB.Text.Trim() : "0"; 
                //m_strFiledValues[]={"WLYDMJ"), this.txtWLYDMJ.Text);
                 m_strFiledValues[15, 0] = "WLYDMJ"; m_strFiledValues[15, 1] = (this.txtWLYDMJ.Text.Trim() != "") ? this.txtXZB.Text.Trim() : "0"; 
                //m_strFiledValues[]={"SJYT"), this.txtSJYT.Text);
                 m_strFiledValues[16,0]="SJYT";m_strFiledValues[16,1]=this.txtSJYT.Text ;
                //m_strFiledValues[]={"YDDW"), this.txtYDDW.Text);
                 m_strFiledValues[17,0]="YDDW";m_strFiledValues[17,1]=this.txtYDDW.Text ;
                //m_strFiledValues[]={"XMLX"), this.txtXMLX.Text);
                 m_strFiledValues[18,0]="XMLX";m_strFiledValues[18,1]=this.txtXMLX.Text ;
                //m_strFiledValues[]={"HFXSC"), this.txtHFXSC.Text);
                 m_strFiledValues[19,0]="HFXSC";m_strFiledValues[19,1]=this.txtHFXSC.Text ;
                //m_strFiledValues[]={"WFLX"), this.txtWFLX.Text);
                 m_strFiledValues[20,0]="WFLX";m_strFiledValues[20,1]=this.txtWFLX.Text ;

                 clsMapFunction.clsSaveFeatureValue.SaveFeatureValueS(m_featureClass, m_selectedFeature, m_strFiledValues);

            }
            catch(System.Exception errs)
            {
                MessageBox.Show("������������ʧ�ܣ�"+errs.Message );
                return false;
            }

            return true;
        }

        private bool  SaveZiLiao()
        {
            string sql = JCZF.SubFrame.DatabaseString.DBConnectString1();
            
            //string sql = "user id=" + m_DataAccess_SYS.UserID + ";password=" + m_DataAccess_SYS.Password + ";";
            //sql += "initial catalog=" + m_DataAccess_SYS.DataBase + ";Server=" + m_DataAccess_SYS.Server + ";";
            //sql += "Connect Timeout=30";
            //System.Data.OleDb.OleDbConnection mycon = new System.Data.OleDb.OleDbConnection(sql);
            //mycon.Open();

            string m_strSQL;


            if (m_strDKID != "" && m_strDKID != null)
            {
                m_strSQL = "select * from doc where dkid= '" + m_strDKID + "'";
            }
           
            else
            {
                return false ; 
            }

            try
            {
                DataTable m_DataTable;
                m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);
                //System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(strSQL, mycon);
                //DataSet dataset = new DataSet();
                //da.Fill(dataset, "DOC");
                if (m_DataTable!=null && m_DataTable.Rows.Count > 0)
                {
                    //System.Data.OleDb.OleDbConnection mycon2 = new System.Data.OleDb.OleDbConnection(sql);
                    string strUpdate;
                    for (int i = 0; i < m_DataTable.Rows.Count; i++)
                    {
                        strUpdate = "UPDATE DOC SET WRITER ='" + txtPSRY.Text + "', SHIJIAN ='" + this.dateTimePicker1.Text + "', NOTE = '" + this.txtNote.Text + "', DANWEI = '" + this.txtDW.Text + "' WHERE dkid = '" + m_strDKID + "'";
                        try
                        {
                            m_DataAccess_SYS.ExecuteSQLNoReturn(strUpdate);
                        }
                        catch (Exception d)
                        {
                           

                            
                        clsFunction.Function.MessageBoxWarning("����ʧ�ܣ�������Ƭ����Ƶʧ�ܣ�"+d.ToString()+"��");
                      

                            return false;
                        }
                        //finally
                        //{
                        //    //mycon2.Close();
                        //}
                    }
                   // m_DataAccess_SYS.MessageInforShow(this, "ͼƬ���سɹ���");
                }
                
                //�ϴ������ӵ�ͼƬ��avicvg
                
                if( Upload()==false)
                {
                    return false; 
                }
                
                try
                {
                    //ɾ���û�Ҫɾ����ͼƬ��avi
                    DELETEPicORAvi();
                }
                catch (Exception d)
                {
                    MessageBox.Show(d.ToString());
                    return false;
                }
                        //finally
                        //{
                        //    //mycon2.Close();
                        //}
                
            }
            catch (Exception d)
            {
                MessageBox.Show(d.ToString());
                return false;
            }
            //finally
            //{
            //    //mycon.Close();
            //}

            return true;
 
        }

        private void m_btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            this.openFileDialog1.Multiselect=true;
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            //FileName = this.openFileDialog1.FileName.ToString();
             
             for (int i = 0; i < openFileDialog1.FileNames.Length; i++)
             {
                 listViewEx1.Items.Add((listViewEx1.Items.Count+1).ToString());
                 listViewEx1.Items[listViewEx1.Items.Count-1].SubItems.Add(openFileDialog1.FileNames[i]);
             }


        }

        /// <summary>
        /// �ϴ������ӵ�ͼƬ��avi
        /// </summary>
        private bool  Upload()
        {
            //if (FileName != "")
            //{
            try
            {

                JCZF.SubFrame.upload m_upload = new JCZF.SubFrame.upload();

                m_upload.m_DataAccess_SYS = this.m_DataAccess_SYS;
               if (this.m_DataAccess_SYS.DBConnection_OleDb.State!=ConnectionState.Open)
                {
                m_upload.openConnection();
                }
                

                for (int i = 0; i < listViewEx1.Items.Count; i++)
                {
                    string DocName1 = m_AssistantFunction.DocRecog(listViewEx1.Items[i].SubItems[1].Text);
                    string DocName = m_AssistantFunction.EraseSpace(DocName1);
                    string DocTypeName = m_AssistantFunction.DocType(listViewEx1.Items[i].SubItems[1].Text);//�ļ���׺

                     m_upload.strDKID = this.m_strDKID ;
                     m_upload.strName = DocName;   //�ļ�����

                    if (!m_upload.HasUpload())
                    {
                        m_upload.imageFileLocation = listViewEx1.Items[i].SubItems[1].Text;
                        
                        m_upload.strCompany = this.txtDW.Text;
                        m_upload.strNotice = this.txtNote.Text;
                        m_upload.strTime = this.dateTimePicker1.Value.ToShortDateString();
                        m_upload.strWriter = this.txtPSRY.Text;
                        m_upload.strLj = listViewEx1.Items[i].SubItems[1].Text;

                        DocTypeName = DocTypeName.ToLower();

                        if (DocTypeName == "bmp" || DocTypeName == "jpg" || DocTypeName == "ico" || DocTypeName == "gif" || DocTypeName == "tif" || DocTypeName == "tiff" || DocTypeName == "png")
                        {
                            m_upload.strMutiFileType ="p";                          
                        }
                        else if (DocTypeName.Contains( "mp" )|| DocTypeName == "avi" || DocTypeName == "rmvb" || DocTypeName == "3gp" || DocTypeName == "wma")
                        {
                            m_upload.strMutiFileType = "v";
                        }

                        //if (DocTypeName == "bmp" || DocTypeName == "jpg" || DocTypeName == "ico" || DocTypeName == "gif" || DocTypeName == "tif" || DocTypeName == "tiff" || DocTypeName == "png")
                        //{

                        //    m_upload.strID = this.m_strOID + "p";

                        //    m_upload.strDKID = this.m_strDKID + "p";
                        //}
                        //else if (DocTypeName == "mp3" || DocTypeName == "avi" || DocTypeName == "rmvb" || DocTypeName == "mpg" || DocTypeName == "wma")
                        //{
                            //m_upload.strID = this.m_strOID + "v";
                           
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ͼƬ����Ƶ����δ�ɹ�");
                        //    return false;
                        //}


                        m_upload.createCommand();
                        // m_upload.prepareInsertImages();

                        if (m_upload.imageFileLocation != "")
                        {
                            if (m_upload.ExecuteInsertImages(m_upload.imageFileLocation, 10000000) == false)
                            {

                                clsFunction.Function.MessageBoxWarning("������Ƭ����Ƶʧ�ܣ�");

                                return false;
                            }
                        }
                    }
                }
                //m_upload.closeConnection();
                //this.label5.Text = "����ɹ���";
                //}
            }
            catch (System.Exception ex)
            {

                clsFunction.Function.MessageBoxWarning("������Ƭ����Ƶʧ�ܣ�" + ex.ToString() + "��");

                return false;
            }
            return true;
        }

       
            /// <summary>
        /// ɾ���û�ѡ��Ҫɾ����ͼƬ��avi
        /// </summary>
        private void DELETEPicORAvi()
        {
            string m_strSQL;

            if (m_strDKID != "" && m_strDKID != null)
            {
                m_strSQL = "select * from doc where dkid= '" + m_strDKID + "'";
            }
           
            else
            {
                return;
            }


            try
            {
                DataTable m_DataTable;
                m_DataTable = m_DataAccess_SYS.getDataTableByQueryString(m_strSQL);
                //System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(strSQL, mycon);
                //DataSet dataset = new DataSet();
                //da.Fill(dataset, "DOC");

                if (m_DataTable == null) return;
                string m_strTempFile="";
                if (m_DataTable.Rows.Count > 0)
                {
                    //System.Data.OleDb.OleDbConnection mycon2 = new System.Data.OleDb.OleDbConnection(sql);
                    string strUpdate;
                    for (int i = 0; i < m_DataTable.Rows.Count; i++)
                    {
                        m_strTempFile=m_DataTable.Rows[i]["MC"].ToString();

                        if (IsInFileList(m_strTempFile) == false)
                        {
                            try
                            {
                                //if (m_strOID != "" && m_strOID != null )
                                //{
                                //    m_strSQL = "delete  from doc where (id = '" + m_strOID + "p' or id = '" + m_strOID + "v')";
                                //}
                                //else if (m_strDKID != "" && m_strDKID != null)
                                //{
                                    m_strSQL = "delete  from doc where (dkid= '" + m_strDKID + "')";
                                //}
                                //else if (m_strObjectID != "" && m_strObjectID != null)
                                //{
                                //    m_strSQL = "delete  from doc where (id = '" + m_strObjectID + "p' or id = '" + m_strObjectID + "v')";
                                //}

                                m_strSQL = m_strSQL + " and MC='" + m_strTempFile + "'";
                                m_DataAccess_SYS.ExecuteSQLNoReturn(m_strSQL);
                            }
                            catch (Exception d)
                            {
                                MessageBox.Show(d.ToString());
                            }
                            finally
                            {
                            }
                        }
                    }
                }             
            }
            catch (SyntaxErrorException errs)
            {
                clsFunction.Function.MessageBoxError(errs.Message);
            }
            finally
            {
                //mycon.Close();
            } 
        }

        bool IsInFileList(string p_strFileName)
        {
            string[] m_strFileList = GetFileList();
            bool m_blTemp = false;
            try
            {
                for (int i = 0; i < m_strFileList.Length ; i++)
                {
                   
                    if (p_strFileName.Trim().ToUpper()==m_strFileList[i].Trim().ToUpper())
                    {
                        m_blTemp = true;
                    }                   
                }
            }
            catch (SystemException errs)
            {
            }
            return m_blTemp;
        }
        string[] GetFileList()
        {
            string m_strTemp = "";
            string[] m_strFileList = new String[listViewEx1.Items.Count];
            try
            {
                for (int i = 0; i < listViewEx1.Items.Count; i++)
                {
                    m_strTemp = listViewEx1.Items[i].SubItems[1].Text;
                    if (m_strTemp.Contains("\\"))
                    {
                        m_strTemp = m_AssistantFunction.DocRecog(listViewEx1.Items[i].SubItems[1].Text);
                    }

                    m_strFileList[i] = m_strTemp;
                }
            }
            catch (SystemException errs)
            {
            }
            return m_strFileList;
        }
       

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedIndices[0] >= 0)
            {
                listViewEx1.Items.RemoveAt(listViewEx1.SelectedIndices[0]);
                for (int i = 0; i < listViewEx1.Items.Count;i++ )
                {
                    listViewEx1.Items[i].SubItems[0].Text = (i + 1).ToString();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string[] m_PrintTextLines = new string[22];
            m_PrintTextLines[0] = "���������룺 "+txtXZQDM.Text ;
            m_PrintTextLines[1] = "�� �� ���� " + txtXMC.Text;
            m_PrintTextLines[2] = "�������ƣ� " + txtXZMC.Text;
            m_PrintTextLines[3] = "ǰ ʱ �ࣺ " + txtQSX.Text;
            m_PrintTextLines[4] = "�� ʱ �ࣺ " + txtHSX.Text;
            m_PrintTextLines[5] = "X  �� �꣺ " + txtXZB.Text;
            m_PrintTextLines[6] = "Y  �� �꣺ " + txtYZB.Text;
            m_PrintTextLines[7] = "����ţ� " + txtJCBH.Text;
            m_PrintTextLines[8] = "�������� " + txtJCMJ.Text+" ƽ����";
            m_PrintTextLines[9] = "��    ע�� " + txtBZ.Text;
            m_PrintTextLines[10] = "�ؿ��ţ� " + txtDKBH.Text;
            m_PrintTextLines[11] = "�ؿ���ࣺ " + txtDKFL.Text;
            m_PrintTextLines[12] = "�ؿ������ " + txtDKMJ.Text + " ƽ����";
            m_PrintTextLines[13] = "��������� " + txtGDMJ.Text + " ƽ����";
            m_PrintTextLines[14] = "ũ�õ������ " + txtNYDMJ.Text + " ƽ����";
            m_PrintTextLines[15] = "����ũ������� " + txtJBNTMJ.Text + " ƽ����";
            m_PrintTextLines[16] = "δ���õ������ " + txtWLYDMJ.Text + " ƽ����";
            m_PrintTextLines[17] = "ʵ����;�� " + txtSJYT.Text;
            m_PrintTextLines[18] = "�õص�λ�� " + txtYDDW.Text;
            m_PrintTextLines[19] = "��Ŀ���ͣ� " + txtXMLX.Text;
            m_PrintTextLines[20] = "�Ϸ�����飺 " + txtHFXSC.Text;
            m_PrintTextLines[21] = "Υ�����ͣ� " + txtWFLX.Text;
            //m_PrintTextLines[22] = "";
           

            Print.Print_DTXCWFXWBGD m_Print_HC_Attribute = new Print.Print_DTXCWFXWBGD();

            m_Print_HC_Attribute.m_intFontSize = 16;
            m_Print_HC_Attribute.m_intLineSpace = 40;
            //m_Print_HC_Attribute.m_strFontName = "";
            m_Print_HC_Attribute.m_PrintTextLines = m_PrintTextLines;

            //m_Print_HC_Attribute.OnFilePrintPreview();
            m_Print_HC_Attribute.OnFilePrint();

        }

       

    }
}