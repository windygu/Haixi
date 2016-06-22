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
            this.cmbXZYDLX.Items.Add("合法用地");
            this.cmbXZYDLX.Items.Add("违法用地");

            this.cmbYDMJLX.Items.Add("用地总数");
            this.cmbYDMJLX.Items.Add("耕地数量");

            this.cmbYDFH.Items.Add("大于");
            this.cmbYDFH.Items.Add("小于");

            this.cmbGHYT.Items.Add("农用地");
            this.cmbGHYT.Items.Add("建设用地");
            this.cmbGHYT.Items.Add("未利用地");

            this.cmbGDFS.Items.Add("划拨");
            this.cmbGDFS.Items.Add("协议出让");
            this.cmbGDFS.Items.Add("招标拍卖挂牌出让");
            this.cmbGDFS.Items.Add("其他");

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

        //非法批地
        private void ckbFFPD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //非法占地
        private void ckbFFZD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //超占用地
        private void ckbCZYD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //骗取批准
        private void ckbPQPZ_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //未报即用
        private void ckbWBJY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //边报边用
        private void ckbBBBY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //未批先用
        private void ckbWPXY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //未供即用
        private void ckbWGJY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //擅自改变用途
        private void ckbSZGBYT_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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

        //其他违法用地
        private void ckbQTWFYT_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cmbXZYDLX.Text == "违法用地")
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
                if (this.cmbYDMJLX.Text == "用地总数" && this.cmbYDFH.Text == "大于")
                    arr.Add(" CAST(YDMJZS as float) > " + this.txtYDSL.Text.ToString());
                if (this.cmbYDMJLX.Text == "用地总数" && this.cmbYDFH.Text == "小于")
                    arr.Add(" CAST(YDMJZS as float) < " + this.txtYDSL.Text.ToString());
                if (this.cmbYDMJLX.Text == "耕地数量" && this.cmbYDFH.Text == "大于")
                    arr.Add(" CAST(YDMJGD as float) > " + this.txtYDSL.Text.ToString());
                if (this.cmbYDMJLX.Text == "耕地数量" && this.cmbYDFH.Text == "小于")
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
                    strQuery = "select objectid as 序号,TBBH as 图斑编号,ZDH as 宗地号,TBWZ as 图斑位置,YDDW as 用地单位或个人,YDSJ as 用地时间,GDPZJGWH as 供地批准机关_批准时间和批准文号,ZZSPZJGWH as 转征收批准机关_批准时间和批准或批次文号,GDPZMJZS as 供地批准面积_总数,GDPZMJGD as 供地批准面积_耕地,YDMJZS as 用地面积_总数,YDMJGD as 用地面积_耕地,GHYT as 规划用途,PZYT as 批准用途,SJYT as 实际用途,GDFS as 供地方式,WBJY as 是否未报即用,BBBY as 是否边报边用,WGJY as 是否未供即用,CZYD as 是否超占用地,SZGBYT as 是否擅自改变用途,FFPD as 是否非法批地,WFFSGD as 是否违法方式供地,QTWFYD as 是否其他违法用地,WFTDLYZTGH as 是否违反土地利用总体规划,DDXZ as 是否单独选址,JJYD as 是否紧急用地,GJHSJZDGC as 是否国家和省级重点工程,ZYJBNT as 是否占用基本农田,NYJGTZ as 是否农业结构调整,SDWBH as 是否实地未变化,DTXCYFXWF as 是否动态巡查已经发现违法,BZ as 备注,XZQM as 所在政区 from 土地核查 where " + strCondition;

                }
                else
                {
                    strQuery = "select objectid as 序号,TBBH as 图斑编号,ZDH as 宗地号,TBWZ as 图斑位置,YDDW as 用地单位或个人,YDSJ as 用地时间,GDPZJGWH as 供地批准机关_批准时间和批准文号,ZZSPZJGWH as 转征收批准机关_批准时间和批准或批次文号,GDPZMJZS as 供地批准面积_总数,GDPZMJGD as 供地批准面积_耕地,YDMJZS as 用地面积_总数,YDMJGD as 用地面积_耕地,GHYT as 规划用途,PZYT as 批准用途,SJYT as 实际用途,GDFS as 供地方式,WBJY as 是否未报即用,BBBY as 是否边报边用,WGJY as 是否未供即用,CZYD as 是否超占用地,SZGBYT as 是否擅自改变用途,FFPD as 是否非法批地,WFFSGD as 是否违法方式供地,QTWFYD as 是否其他违法用地,WFTDLYZTGH as 是否违反土地利用总体规划,DDXZ as 是否单独选址,JJYD as 是否紧急用地,GJHSJZDGC as 是否国家和省级重点工程,ZYJBNT as 是否占用基本农田,NYJGTZ as 是否农业结构调整,SDWBH as 是否实地未变化,DTXCYFXWF as 是否动态巡查已经发现违法,BZ as 备注,XZQM as 所在政区 from 土地核查 where " + this.subString + "and " + strCondition;
                }
            }
            else
                //strQuery = "select TBWZ as 图斑位置,YDDW as 用地单位或个人,YDSJ as 用地时间,GDPZJGWH as 供地批准机关_批准时间和批准文号,ZZSPZJGWH as 转征收批准机关_批准时间和批准或批次文号,GDPZMJZS as 供地批准面积_总数,GDPZMJGD as 供地批准面积_耕地,YDMJZS as 用地面积_总数,YDMJGD as 用地面积_耕地,GHYT as 规划用途,PZYT as 批准用途,SJYT as 实际用途,GDFS as 供地方式,WBJY as 是否未报即用,BBBY as 是否边报边用,WGJY as 是否未供即用,CZYD as 是否超占用地,SZGBYT as 是否擅自改变用途,FFPD as 是否非法批地,DDXZ as 是否单独选址,JJYD as 是否紧急用地,GJHSJZDGC as 是否国家和省级重点工程,ZYJBNT as 是否占用基本农田,NYJGTZ as 是否农业结构调整,SDWBH as 是否实地未变化,DTXCYFXWF as 是否动态巡查已经发现违法,BZ as 备注,XZQM as 所在政区 from 土地核查";         
                MessageBox.Show("请输入查询条件");

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
                MessageBox.Show("没有查询到数据！");
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

            Functions.MapFunction.ZoomToSelFeaByFID("土地核查", "objectid", OID, m_frmBHTBView.axMapControl1);//this.parenForm.m_frmMapView.axMapControl1);
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
            string m_FeatureLayerName = "土地核查";
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