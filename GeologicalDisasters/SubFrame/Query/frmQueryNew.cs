using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DevComponents.DotNetBar;

using System.Data.OleDb;
using System.Configuration;
//using DbManager;
using clsDataAccess;
using JCZF.MainFrame;
using System.Diagnostics;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;

namespace JCZF.SubFrame.Query
{
    public partial class frmQueryNew : DevComponents.DotNetBar.Office2007Form
    {
        //public string strConn = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sde;Initial Catalog=sde;Data Source=sde;PassWord=sde;";
        //public string strConn = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

       

        public clsDataAccess.DataAccess m_DataAccess;
        private frmDataQueryShow m_frmDataQueryShow;

        private frmMain parenForm;
        //private frmBHTBView m_frmBHTBView;

        public string subString = "";
        public string qhdm = "";
        public string zqm = "";

        private ArrayList arrdlmc = new ArrayList();
        private ArrayList dbldltb = new ArrayList();

        public frmQueryNew(frmMain parentForm)
        {
            InitializeComponent();
            this.parenForm = parentForm;
        }

        private void frmQuery_Load(object sender, EventArgs e)
        {
            this.cmbXZYDLX.Items.Add("�Ϸ��õ�");
            this.cmbXZYDLX.Items.Add("Υ���õ�");

            this.cmbYDMJLX.Items.Add("�õ�����");
            this.cmbYDMJLX.Items.Add("��������");

            this.cmbYDFH.Items.Add("����");
            this.cmbYDFH.Items.Add("С��");

            this.cmbGHYT.Items.Add("ũ�õ�");
            this.cmbGHYT.Items.Add("�����õ�");
            this.cmbGHYT.Items.Add("δ���õ�");

            this.cmbGDFS.Items.Add("����");
            this.cmbGDFS.Items.Add("Э�����");
            this.cmbGDFS.Items.Add("�б��������Ƴ���");
            this.cmbGDFS.Items.Add("����");

            this.cmbXZYDLX.Visible = false;

            DateTime dt = DateTime.Now;

            this.txtToDate.Text = dt.ToShortDateString();

            this.SetckbWFEnabledFalse();

            //this.Size = new System.Drawing.Size(434,315);

        }

        private void rdbXZYD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbXZYD.Checked == true)
            {
                this.rdbNYD.Checked = false;
                this.rdbWLYD.Checked = false;
                this.cmbXZYDLX.Visible = true;
            }
        }

        private void rdbNYD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbNYD.Checked == true)
            {
                this.rdbXZYD.Checked = false;
                this.rdbWLYD.Checked = false;

                this.cmbXZYDLX.Text = "";
                this.cmbXZYDLX.Visible = false;

                this.SetckbWFEnabledFalse();
                
            }

        }

        private void rdbWLYD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbWLYD.Checked == true)
            {
                this.rdbXZYD.Checked = false;
                this.rdbNYD.Checked = false;

                this.cmbXZYDLX.Text = "";
                this.cmbXZYDLX.Visible = false;

                this.SetckbWFEnabledFalse();
            }

        }

        //�Ƿ�����
        private void ckbFFPD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbFFPD.Checked == true)
                {
                    this.ckbFFZD.Enabled = false;
                    this.ckbCZYD.Enabled = false;
                    this.ckbPQPZ.Enabled = false;
                    this.ckbWBJY.Enabled = false;
                    this.ckbBBBY.Enabled = false;
                    this.ckbWPXY.Enabled = false;
                    this.ckbWGJY.Enabled = false;
                    this.ckbSZGBYT.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;
                }
                else
                {
                    this.ckbFFZD.Enabled = true;
                    this.ckbCZYD.Enabled = true;
                    this.ckbPQPZ.Enabled = true;
                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWPXY.Enabled = true;
                    this.ckbWGJY.Enabled = true;
                    this.ckbSZGBYT.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }
        }

        //�Ƿ�ռ��
        private void ckbFFZD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbFFZD.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;
                    //this.ckbCZYD.Enabled = false;
                    //this.ckbPQPZ.Enabled = false;
                    //this.ckbWBJY.Enabled = false;
                    //this.ckbBBBY.Enabled = false;
                    //this.ckbWPXY.Enabled = false;
                    //this.ckbWGJY.Enabled = false;
                    this.ckbSZGBYT.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;
                }
                else
                {
                    this.ckbFFPD.Enabled = true;
                    this.ckbCZYD.Enabled = true;
                    this.ckbPQPZ.Enabled = true;
                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWPXY.Enabled = true;
                    this.ckbWGJY.Enabled = true;
                    this.ckbSZGBYT.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }
        }

        //��ռ�õ�
        private void ckbCZYD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbCZYD.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;
                    this.ckbFFZD.Checked = true;
                    this.ckbPQPZ.Enabled = false;
                    this.ckbWBJY.Enabled = false;
                    this.ckbBBBY.Enabled = false;
                    this.ckbWPXY.Enabled = false;
                    this.ckbWGJY.Enabled = false;
                    this.ckbSZGBYT.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;
                }
                else
                {
                    this.ckbFFPD.Enabled = true;
                    this.ckbFFZD.Checked = false;
                    this.ckbPQPZ.Enabled = true;
                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWPXY.Enabled = true;
                    this.ckbWGJY.Enabled = true;
                    this.ckbSZGBYT.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }
        }

        //ƭȡ��׼
        private void ckbPQPZ_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbPQPZ.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;
                    this.ckbFFZD.Checked = true;
                    this.ckbCZYD.Enabled = false;
                    this.ckbWBJY.Enabled = false;
                    this.ckbBBBY.Enabled = false;
                    this.ckbWPXY.Enabled = false;
                    this.ckbWGJY.Enabled = false;
                    this.ckbSZGBYT.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;
                }
                else
                {
                    this.ckbFFPD.Enabled = true;
                    this.ckbFFZD.Checked = false;
                    this.ckbCZYD.Enabled = true;
                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWPXY.Enabled = true;
                    this.ckbWGJY.Enabled = true;
                    this.ckbSZGBYT.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }
        }

        //δ������
        private void ckbWBJY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbWBJY.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;

                    this.ckbFFZD.Checked = true;

                    this.ckbCZYD.Enabled = false;
                    this.ckbPQPZ.Enabled = false;

                    this.ckbWPXY.Checked = true;
                    this.ckbBBBY.Enabled = false;
                    this.ckbWGJY.Enabled = false;

                    this.ckbSZGBYT.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;
                }
                else
                {
                    this.ckbFFPD.Enabled = true;

                    this.ckbFFZD.Checked = false;

                    this.ckbCZYD.Enabled = true;
                    this.ckbPQPZ.Enabled = true;

                    this.ckbWPXY.Checked = false;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWGJY.Enabled = true;

                    this.ckbSZGBYT.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }
        }

        //�߱�����
        private void ckbBBBY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbBBBY.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;

                    this.ckbFFZD.Checked = true;

                    this.ckbCZYD.Enabled = false;
                    this.ckbPQPZ.Enabled = false;

                    this.ckbWPXY.Checked = true;
                    this.ckbWBJY.Enabled = false;
                    this.ckbWGJY.Enabled = false;

                    this.ckbSZGBYT.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;
                }
                else
                {
                    this.ckbFFPD.Enabled = true;

                    this.ckbFFZD.Checked = false;

                    this.ckbCZYD.Enabled = true;
                    this.ckbPQPZ.Enabled = true;

                    this.ckbWPXY.Checked = false;
                    this.ckbWBJY.Enabled = true;
                    this.ckbWGJY.Enabled = true;

                    this.ckbSZGBYT.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }
        }

        //δ������
        private void ckbWPXY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbWPXY.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;

                    this.ckbFFZD.Checked = true;

                    this.ckbCZYD.Enabled = false;
                    this.ckbPQPZ.Enabled = false;

                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWGJY.Enabled = true;

                    this.ckbSZGBYT.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;
                }
                else
                {
                    this.ckbFFPD.Enabled = true;

                    this.ckbFFZD.Checked = false;

                    this.ckbCZYD.Enabled = true;
                    this.ckbPQPZ.Enabled = true;

                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWGJY.Enabled = true;

                    this.ckbSZGBYT.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }
        }

        //δ������
        private void ckbWGJY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbWGJY.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;

                    this.ckbFFZD.Checked = true;

                    this.ckbCZYD.Enabled = false;
                    this.ckbPQPZ.Enabled = false;

                    this.ckbWPXY.Checked = true;
                    this.ckbWBJY.Enabled = false;
                    this.ckbBBBY.Enabled = false;

                    this.ckbSZGBYT.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;
                }
                else
                {
                    this.ckbFFPD.Enabled = true;

                    this.ckbFFZD.Checked = false;

                    this.ckbCZYD.Enabled = true;
                    this.ckbPQPZ.Enabled = true;

                    this.ckbWPXY.Checked = false;
                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;

                    this.ckbSZGBYT.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }
        }

        //���Ըı���;
        private void ckbSZGBYT_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbSZGBYT.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;
                    this.ckbFFZD.Enabled = false;
                    this.ckbQTWFYT.Enabled = false;

                    this.ckbPQPZ.Enabled = false;
                    this.ckbCZYD.Enabled = false;

                    this.ckbWPXY.Enabled = false;
                    this.ckbWBJY.Enabled = false;
                    this.ckbBBBY.Enabled = false;
                    this.ckbWGJY.Enabled = false;

                }
                else
                {
                    this.ckbFFPD.Enabled = true;
                    this.ckbFFZD.Enabled = true;
                    this.ckbCZYD.Enabled = true;
                    this.ckbPQPZ.Enabled = true;
                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWPXY.Enabled = true;
                    this.ckbWGJY.Enabled = true;
                    this.ckbQTWFYT.Enabled = true;

                }
            }

        }

        //����Υ���õ�
        private void ckbQTWFYT_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "Υ���õ�")
            {
                if (this.ckbQTWFYT.Checked == true)
                {
                    this.ckbFFPD.Enabled = false;
                    this.ckbFFZD.Enabled = false;
                    this.ckbCZYD.Enabled = false;
                    this.ckbPQPZ.Enabled = false;
                    this.ckbWBJY.Enabled = false;
                    this.ckbBBBY.Enabled = false;
                    this.ckbWPXY.Enabled = false;
                    this.ckbWGJY.Enabled = false;
                    this.ckbSZGBYT.Enabled = false;
                }
                else
                {
                    this.ckbFFPD.Enabled = true;
                    this.ckbFFZD.Enabled = true;
                    this.ckbCZYD.Enabled = true;
                    this.ckbPQPZ.Enabled = true;
                    this.ckbWBJY.Enabled = true;
                    this.ckbBBBY.Enabled = true;
                    this.ckbWPXY.Enabled = true;
                    this.ckbWGJY.Enabled = true;
                    this.ckbSZGBYT.Enabled = true;

                }
            }
        }

        private void cmbXZYDLX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.SelectedIndex == 0)
            {
                this.SetckbWFEnabledFalse();
            }
            if (this.cmbXZYDLX.SelectedIndex == 1)
            {
                this.SetckbWFEnabledTrue();
            }
        }

        private string CreateQueSQL()
        {
            ArrayList arr = new ArrayList();

            arr.Add(((this.rdbWLYD.Checked == true) ? (" SDWBH != '' ") : ("")));
            arr.Add(((this.rdbNYD.Checked == true) ? (" NYJGTZ != '' ") : ("")));

            if (this.txtFromDate.Text != "" && this.txtToDate.Text != "")
            {
                if (this.txtFromDate.Text != this.txtToDate.Text)
                    arr.Add(" ((left(YDSJ,4) >= '" + this.txtFromDate.Text.ToString() + "') and (left(YDSJ,4) <= '" + this.txtToDate.Text.ToString() + "'))");
                else
                    arr.Add(" (left(YDSJ,4) = '" + this.txtFromDate.Text.ToString() + "')");
            }

            if (this.cmbYDMJLX.Text != "" && this.cmbYDFH.Text != "" && this.txtYDSL.Text != "")
            {
                if (this.cmbYDMJLX.Text == "�õ�����" && this.cmbYDFH.Text == "����")
                    arr.Add(" CAST(YDMJZS as float) > " + this.txtYDSL.Text.ToString());
                if (this.cmbYDMJLX.Text == "�õ�����" && this.cmbYDFH.Text == "С��")
                    arr.Add(" CAST(YDMJZS as float) < " + this.txtYDSL.Text.ToString());
                if (this.cmbYDMJLX.Text == "��������" && this.cmbYDFH.Text == "����")
                    arr.Add(" CAST(YDMJGD as float) > " + this.txtYDSL.Text.ToString());
                if (this.cmbYDMJLX.Text == "��������" && this.cmbYDFH.Text == "С��")
                    arr.Add(" CAST(YDMJGD as float) < " + this.txtYDSL.Text.ToString());

            }

            arr.Add(((this.cmbGHYT.Text != "") ? (" GHYT = '" + this.cmbGHYT.Text + "'") : ("")));
            arr.Add(((this.cmbGDFS.Text != "") ? (" GDFS = '" + this.cmbGDFS.Text + "'") : ("")));
            arr.Add(((this.txtPZYT.Text != "") ? (" PZYT LIKE  '%" + this.txtPZYT.Text + "%'") : ("")));
            arr.Add(((this.txtSJYT.Text != "") ? (" SJYT LIKE '%" + this.txtSJYT.Text + "%'") : ("")));

            arr.Add(((this.ckbFFPD.Checked == true) ? (" FFPD != '' ") : ("")));

            //arr.Add(((this.ckbPQPZ.Checked == true) ? (" PQPZ = " + ((this.ckbPQPZ.Checked == true) ? 1 : 0)) : ("")));
            arr.Add(((this.ckbCZYD.Checked == true) ? (" CZYD != '' ") : ("")));
            //arr.Add(((this.ckbWPXY.Checked == true) ? (" WPXY = " + ((this.ckbWPXY.Checked == true) ? 1 : 0)) : ("")));
            arr.Add(((this.ckbWBJY.Checked == true) ? (" WBJY != '' ") : ("")));
            arr.Add(((this.ckbBBBY.Checked == true) ? (" BBBY != '' ") : ("")));
            arr.Add(((this.ckbWGJY.Checked == true) ? (" WGJY != '' ") : ("")));

            arr.Add(((this.ckbDDXZ.Checked == true) ? (" DDXZ != '' ") : ("")));
            arr.Add(((this.ckbJJYD.Checked == true) ? (" JJYD != '' ") : ("")));
            arr.Add(((this.ckbGJSJZDGC.Checked == true) ? (" GJHSJZDGC != '' ") : ("")));
            arr.Add(((this.ckbZYJBNT.Checked == true) ? (" ZYJBNT != '' ") : ("")));
            arr.Add(((this.ckbDTXCYFX.Checked == true) ? (" DTXCYFXWF != '' ") : ("")));
            arr.Add(((this.ckbWFGH.Checked == true) ? (" WFTDLYZTGH != '' ") : ("")));


            arr.Add(((this.ckbSZGBYT.Checked == true) ? (" SZGBYT != '' ") : ("")));
            arr.Add(((this.ckbQTWFYT.Checked == true) ? (" QTWFYD != '' ") : ("")));

            string strCondition = "";

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].ToString() != ""&& strCondition !="")
                {
                    strCondition += " and " + arr[i].ToString();
                }
                else
                    strCondition += arr[i].ToString();
 
            }

            return strCondition;

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string strQuery = "";

            string strCondition = this.CreateQueSQL();

            if (strCondition != "")
            {
                if (subString == "")
                {
                    strQuery = "select objectid as ���,TBBH as ͼ�߱��,ZDH as �ڵغ�,TBWZ as ͼ��λ��,YDDW as �õص�λ�����,YDSJ as �õ�ʱ��,GDPZJGWH as ������׼����_��׼ʱ�����׼�ĺ�,ZZSPZJGWH as ת������׼����_��׼ʱ�����׼�������ĺ�,GDPZMJZS as ������׼���_����,GDPZMJGD as ������׼���_����,YDMJZS as �õ����_����,YDMJGD as �õ����_����,GHYT as �滮��;,PZYT as ��׼��;,SJYT as ʵ����;,GDFS as ���ط�ʽ,WBJY as �Ƿ�δ������,BBBY as �Ƿ�߱�����,WGJY as �Ƿ�δ������,CZYD as �Ƿ�ռ�õ�,SZGBYT as �Ƿ����Ըı���;,FFPD as �Ƿ�Ƿ�����,WFFSGD as �Ƿ�Υ����ʽ����,QTWFYD as �Ƿ�����Υ���õ�,WFTDLYZTGH as �Ƿ�Υ��������������滮,DDXZ as �Ƿ񵥶�ѡַ,JJYD as �Ƿ�����õ�,GJHSJZDGC as �Ƿ���Һ�ʡ���ص㹤��,ZYJBNT as �Ƿ�ռ�û���ũ��,NYJGTZ as �Ƿ�ũҵ�ṹ����,SDWBH as �Ƿ�ʵ��δ�仯,DTXCYFXWF as �Ƿ�̬Ѳ���Ѿ�����Υ��,BZ as ��ע,XZQM as �������� from ���غ˲� where " + strCondition;

                }
                else
                {
                    strQuery = "select objectid as ���,TBBH as ͼ�߱��,ZDH as �ڵغ�,TBWZ as ͼ��λ��,YDDW as �õص�λ�����,YDSJ as �õ�ʱ��,GDPZJGWH as ������׼����_��׼ʱ�����׼�ĺ�,ZZSPZJGWH as ת������׼����_��׼ʱ�����׼�������ĺ�,GDPZMJZS as ������׼���_����,GDPZMJGD as ������׼���_����,YDMJZS as �õ����_����,YDMJGD as �õ����_����,GHYT as �滮��;,PZYT as ��׼��;,SJYT as ʵ����;,GDFS as ���ط�ʽ,WBJY as �Ƿ�δ������,BBBY as �Ƿ�߱�����,WGJY as �Ƿ�δ������,CZYD as �Ƿ�ռ�õ�,SZGBYT as �Ƿ����Ըı���;,FFPD as �Ƿ�Ƿ�����,WFFSGD as �Ƿ�Υ����ʽ����,QTWFYD as �Ƿ�����Υ���õ�,WFTDLYZTGH as �Ƿ�Υ��������������滮,DDXZ as �Ƿ񵥶�ѡַ,JJYD as �Ƿ�����õ�,GJHSJZDGC as �Ƿ���Һ�ʡ���ص㹤��,ZYJBNT as �Ƿ�ռ�û���ũ��,NYJGTZ as �Ƿ�ũҵ�ṹ����,SDWBH as �Ƿ�ʵ��δ�仯,DTXCYFXWF as �Ƿ�̬Ѳ���Ѿ�����Υ��,BZ as ��ע,XZQM as �������� from ���غ˲� where " + this.subString + "and " + strCondition;
                }
            }
            else
                //strQuery = "select TBWZ as ͼ��λ��,YDDW as �õص�λ�����,YDSJ as �õ�ʱ��,GDPZJGWH as ������׼����_��׼ʱ�����׼�ĺ�,ZZSPZJGWH as ת������׼����_��׼ʱ�����׼�������ĺ�,GDPZMJZS as ������׼���_����,GDPZMJGD as ������׼���_����,YDMJZS as �õ����_����,YDMJGD as �õ����_����,GHYT as �滮��;,PZYT as ��׼��;,SJYT as ʵ����;,GDFS as ���ط�ʽ,WBJY as �Ƿ�δ������,BBBY as �Ƿ�߱�����,WGJY as �Ƿ�δ������,CZYD as �Ƿ�ռ�õ�,SZGBYT as �Ƿ����Ըı���;,FFPD as �Ƿ�Ƿ�����,DDXZ as �Ƿ񵥶�ѡַ,JJYD as �Ƿ�����õ�,GJHSJZDGC as �Ƿ���Һ�ʡ���ص㹤��,ZYJBNT as �Ƿ�ռ�û���ũ��,NYJGTZ as �Ƿ�ũҵ�ṹ����,SDWBH as �Ƿ�ʵ��δ�仯,DTXCYFXWF as �Ƿ�̬Ѳ���Ѿ�����Υ��,BZ as ��ע,XZQM as �������� from ���غ˲�";         
                MessageBox.Show("�������ѯ����");

            DataSet ds = m_DataAccess.GetDataBySQL(strQuery); //this.GetDataBySQL(strConn, strQuery);

            //DataSet ds = DBHelper.ToDataSet(strQuery);


            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                m_frmDataQueryShow = new frmDataQueryShow();
                m_frmDataQueryShow.MdiParent = this.parenForm;
                this.FillListView(ds, m_frmDataQueryShow.listView1);
                m_frmDataQueryShow.ZoomToFea+=new frmDataQueryShow.ZoomToFeaEventHandler(m_frmDataQueryShow_ZoomToFea);
                m_frmDataQueryShow.Show();
                //m_frmDataQueryShow.WindowState=FormWindowState.Maximized;
                this.Close();
            }
            else
            {
                MessageBox.Show("û�в�ѯ�����ݣ�");
            }
            
        }

        private void m_frmDataQueryShow_ZoomToFea(string OID)
        {
            if (qhdm.Length == 4)
            {
                openfrmBHTBView(this.qhdm, Convert.ToInt32(OID), this.zqm);
            }
            else
            {
                openfrmBHTBView(this.qhdm, Convert.ToInt32(OID), this.zqm);
            }

            Functions.MapFunction.ZoomToSelFeaByFID("���غ˲�", "objectid", OID, m_frmBHTBView.axMapControl1);//this.parenForm.m_frmMapView.axMapControl1);
        }

        //private void openfrmBHTBView(string qhdm, int m_OID, string m_FeatureLayerName,string zqm)
        //{
        //    string path = Application.StartupPath + @"\" + qhdm + ".mxd";

        //    m_frmBHTBView = new frmBHTBView();
        //    //m_frmBHTBView.WindowState = FormWindowState.Minimized;
        //    m_frmBHTBView.Text = zqm;
        //    m_frmBHTBView.path = path;
        //    m_frmBHTBView.m_OID = m_OID;
        //    m_frmBHTBView.m_FeatureLayerName = m_FeatureLayerName;
        //    m_frmBHTBView.pDataAccess = this.m_DataAccess;

        //    m_frmBHTBView.setImageBtni(m_frmBHTBView.bar1);

        //    m_frmBHTBView.Show();

        //}

        private void openfrmBHTBView(string qhdm, int m_OID, string zqm)
        {

            string path = Application.StartupPath + @"\" + qhdm + ".mxd";
            m_frmBHTBView = new frmBHTBView();
            string m_FeatureLayerName = "���غ˲�";
            m_frmBHTBView.WindowState = FormWindowState.Minimized;
            m_frmBHTBView.Text = zqm;
            m_frmBHTBView.path = path;
            m_frmBHTBView.m_OID = m_OID;
            m_frmBHTBView.m_FeatureLayerName = m_FeatureLayerName;
            m_frmBHTBView.pDataAccess = this.m_DataAccess;

            m_frmBHTBView.setImageBtni(m_frmBHTBView.bar1);
            m_frmBHTBView.Show(); 
        }
       

        private void FillListView(DataSet ds,ListView _listView)
        {
            _listView.Clear();

            string[] s1 = new string[ds.Tables[0].Columns.Count];

            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                _listView.Columns.Add(ds.Tables[0].Columns[i].ColumnName, 100, HorizontalAlignment.Left);
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < _listView.Columns.Count; j++)
                {
                    s1[j] = ds.Tables[0].Rows[i][j].ToString();
 
                }
                ListViewItem lvi = new ListViewItem(s1);
                _listView.Items.Add(lvi);
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.rdbXZYD.Checked = false;
            this.cmbXZYDLX.Text = "";
            this.rdbWLYD.Checked = false;
            this.rdbNYD.Checked = false;

            this.txtFromDate.Text = "";
            this.txtToDate.Text = DateTime.Now.ToShortDateString();

            this.cmbYDMJLX.Text = "";
            this.cmbYDFH.Text = "";
            this.txtYDSL.Text = "";

            this.cmbGHYT.Text = "";
            this.cmbGDFS.Text = "";
            this.txtPZYT.Text = "";
            this.txtSJYT.Text = "";

            this.SetckbWFEnabledFalse();

        }

        private void SetckbWFEnabledFalse()
        {
            if(this.ckbBBBY.Checked == true)
                this.ckbBBBY.Checked = false;
                this.ckbBBBY.Enabled = false;
            if(this.ckbCZYD.Checked ==true)
                this.ckbCZYD.Checked = false;
                this.ckbCZYD.Enabled = false;
            if(this.ckbDDXZ.Checked == true)
                this.ckbDDXZ.Checked = false;
                this.ckbDDXZ.Enabled = false;
            if(this.ckbDTXCYFX.Checked == true)
                this.ckbDTXCYFX.Checked = false;
                this.ckbDTXCYFX.Enabled = false;
            if(this.ckbFFPD.Checked ==true)
                this.ckbFFPD.Checked = false;
                this.ckbFFPD.Enabled = false;
            if(this.ckbFFZD.Checked ==true)
                this.ckbFFZD.Checked = false;
                this.ckbFFZD.Enabled = false;
            if(this.ckbGJSJZDGC.Checked ==true)
                this.ckbGJSJZDGC.Checked = false;
                this.ckbGJSJZDGC.Enabled = false;
            if(this.ckbJJYD.Checked == true)
                this.ckbJJYD.Checked = false;
                this.ckbJJYD.Enabled = false;
            if(this.ckbPQPZ.Checked == true)
                this.ckbPQPZ.Checked = false;
                this.ckbPQPZ.Enabled = false;
            if(this.ckbQTWFYT.Checked == true)
                this.ckbQTWFYT.Checked = false;
                this.ckbQTWFYT.Enabled = false;
            if(this.ckbSZGBYT.Checked == true)
                this.ckbSZGBYT.Checked = false;
                this.ckbSZGBYT.Enabled = false;
            if(this.ckbWBJY.Checked == true)
                this.ckbWBJY.Checked = false;
                this.ckbWBJY.Enabled = false;
            if(this.ckbWFGH.Checked ==true)
                this.ckbWFGH.Checked = false;
                this.ckbWFGH.Enabled = false;
            if(this.ckbWGJY.Checked ==true)
                this.ckbWGJY.Checked = false;
                this.ckbWGJY.Enabled = false;
            if(this.ckbWPXY.Checked ==true)
                this.ckbWPXY.Checked = false;
                this.ckbWPXY.Enabled = false;
            if(this.ckbZYJBNT.Checked == true)
                this.ckbZYJBNT.Checked = false;
                this.ckbZYJBNT.Enabled = false;
        }

        private void SetckbWFEnabledTrue()
        {
            this.ckbBBBY.Checked = false;
            this.ckbBBBY.Enabled = true;
            this.ckbCZYD.Checked = false;
            this.ckbCZYD.Enabled = true;
            this.ckbDDXZ.Checked = false;
            this.ckbDDXZ.Enabled = true;
            this.ckbDTXCYFX.Checked = false;
            this.ckbDTXCYFX.Enabled = true;
            this.ckbFFPD.Checked = false;
            this.ckbFFPD.Enabled = true;
            this.ckbFFZD.Checked = false;
            this.ckbFFZD.Enabled = true;
            this.ckbGJSJZDGC.Checked = false;
            this.ckbGJSJZDGC.Enabled = true;
            this.ckbJJYD.Checked = false;
            this.ckbJJYD.Enabled = true;
            this.ckbPQPZ.Checked = false;
            this.ckbPQPZ.Enabled = true;
            this.ckbQTWFYT.Checked = false;
            this.ckbQTWFYT.Enabled = true;
            this.ckbSZGBYT.Checked = false;
            this.ckbSZGBYT.Enabled = true;
            this.ckbWBJY.Checked = false;
            this.ckbWBJY.Enabled = true;
            this.ckbWFGH.Checked = false;
            this.ckbWFGH.Enabled = true;
            this.ckbWGJY.Checked = false;
            this.ckbWGJY.Enabled = true;
            this.ckbWPXY.Checked = false;
            this.ckbWPXY.Enabled = true;
            this.ckbZYJBNT.Checked = false;
            this.ckbZYJBNT.Enabled = true;
        }

       









    }
}