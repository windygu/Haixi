using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using System.IO;
using ESRI.ArcGIS.SystemUI;

namespace JCZF.SubFrame.DataEdit
{
    public partial class frmTDHCEdit : UserControl  
    {
        public frmTDHCEdit()
        {
            InitializeComponent();
        }

        private ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl_;
        public ESRI.ArcGIS.Controls.AxMapControl m_AxMapControl
        {
            set
            {
                m_AxMapControl_ = value;
                //uctXZQTree_Dev1.m_AxMapControl = value;

            }
        }


        private clsDataAccess.DataAccess m_DataAccess_;

        public clsDataAccess.DataAccess m_DataAccess_SYS_
        {
            set
            {
                m_DataAccess_ = value;
            }
        }



        private clsDataAccess.DataAccess m_DataAccess_SDE;

        public clsDataAccess.DataAccess m_DataAccess_SDE_
        {
            set
            {
                m_DataAccess_SDE = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{

              

            //    string sql = "";
            //    sql = "SELECT OBJECTID , XZQDM AS 行政区代码, XMC AS 行政区名, QSX AS 前时相, HSX AS 后时相, JCBH AS 监察编号, SJKDLBM AS 实际地类编码, QDLBM AS 前地类编码, ZYJBNT AS 占用基本农田, JBNTMJ AS 基本农田面积, HDLBM AS 后地类编码, JCMJ AS 监察面积, SFBH AS 是否变化,"
            //        + "  XZMC AS 街道名, XZDWKD AS 线状地物, WYHSZP AS 未用指标, BZ AS 备注, DKBH AS 地块变化, DKFL AS 地块分类, DKMJ AS 地块面积, NYDMJ AS 农用地面积, GDMJ AS 耕地面积, "
            //        + "JBNTMJI AS 基本农田面积, WLYDMJ AS 违法用地面积, SJYT AS 实际用途, YDDW AS 用地单位, XMLX AS 项目类型, HFXSC AS 合法性审查, WFLX AS 违法类型,"
            //        + "CASE xffx when 0 then CAST('0' AS bit) when 1 then CAST('1' as bit) end AS 信访发现, CASE xcfx when 0 then CAST('0' AS bit) when 1 then CAST('1' as bit) end AS 巡查发现,CASE wpfx when 0 then CAST('0' AS bit) when 1 then CAST('1' as bit) end AS 卫片发现, CASE tdorkcwf when 0 then CAST('0' AS bit) when 1 then CAST('1' as bit) end AS 土地违法,  case ajzt when 0 then CAST('0' AS bit) when 1 then CAST('1' as bit) end AS 是否立案," +
            //        "lasj AS 立案时间, xzcf_chaichu AS 拆除, xzcf_moshou AS 没收, xzcf_tuihuantd AS 退还土地或复耕, fk_yingshou AS 罚款应收, fk_shishou AS 罚款实收, CASE shqfayuanzx WHEN 0 THEN CAST('0' AS bit) WHEN 1 THEN CAST('1' AS bit) END AS 申请法院强制执行, CASE fla_zltg WHEN 0 THEN CAST('0' AS bit) WHEN 1 THEN CAST('1' AS bit) END AS 责令停工或限改, CASE fla_zxql WHEN 0 THEN CAST('0' AS bit) WHEN 1 THEN CAST('1' AS bit) END AS 自行清理或整治, CASE fla_zdtt WHEN 0 THEN CAST('0' AS bit) WHEN 1 THEN CAST('1' AS bit) " +
            //        " END AS 主动腾退土地或矿坑, CASE fla_zqtbm WHEN 0 THEN CAST('0' AS bit) WHEN 1 THEN CAST('1' AS bit) END AS 转其他部门处理, CASE fla_qt WHEN 0 THEN CAST('0' AS bit) WHEN 1 THEN CAST('1' AS bit) END AS 非立案方式处理情况其他, CASE sftccf WHEN 0 THEN CAST('0' AS bit) " +
            //        " WHEN 1 THEN CAST('1' AS bit) END AS 是否提出处分建议, tccfrs AS 提出处分建议人数, CASE yysong WHEN 0 THEN CAST('0' AS bit) WHEN 1 THEN CAST('1' AS bit) END AS 已移送处分或刑事案件 " + " FROM T2009_WPJC_80_Temp ";

            //    int bz = 0;

            //    if (uctXZQTree_Dev1.TreeView.SelectedNode.Text != "" || uctXZQTree_Dev1.TreeView.SelectedNode.Text != null)
            //    {
            //        string p_strXZQDM = uctXZQTree_Dev1.m_strXZQDM;
            //        if (bz == 0)
            //        {
            //            sql = sql + "WHERE LEFT(XZQDM ," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'";
            //            bz = bz + 1;
            //        }
            //        else
            //        {
            //            sql = sql + "and LEFT(XZQDM ," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'";

            //        }
            //    }



            //    DateTime dtStar = this.dateTimeInput2.Value;
            //    DateTime dtEnd = this.dateTimeInput1.Value.AddDays(1);


            //    if (bz == 0)
            //    {
            //        if (dtStar.Year < 1800 || dtEnd.Year < 1800)
            //        {
            //            //return;
            //        }
            //        else
            //        {
            //            if (m_DataAccess_.ProviderIsMSSQLDB())
            //            {
            //                sql = sql + " where lasj between '" + dtStar.ToString() + "' and  '" + dtEnd.ToString() + "'";
            //            }
            //            else
            //            {
            //                sql = sql + " where lasj between to_date('" + dtStar.ToString() + "','YYYY-MM-DD HH24:MI:SS') and  to_date('" + dtEnd.ToString() + "','YYYY-MM-DD HH24:MI:SS') ";
            //            }
            //        }

            //    }
            //    else
            //    {
            //        if (dtStar.Year < 1800 || dtEnd.Year < 1800)
            //        {
            //            //return;
            //        }
            //        else
            //        {
            //            if (m_DataAccess_.ProviderIsMSSQLDB())
            //            {
            //                sql = sql + " and lasj between '" + dtStar.ToString() + "' and  '" + dtEnd.ToString() + "'";
            //            }
            //            else
            //            {
            //                sql = sql + " and lasj between to_date('" + dtStar.ToString() + "','YYYY-MM-DD HH24:MI:SS') and  to_date('" + dtEnd.ToString() + "','YYYY-MM-DD HH24:MI:SS') ";
            //            }
            //        }
            //    }

            //    try
            //    {
            //        //dataGridView1.Columns[0].ReadOnly = false;
            //        DataTable m_DataTable = m_DataAccess_SDE.getDataTableByQueryString(sql);

            //        if (m_DataTable != null)
            //        {
            //            //dataGridView1.Columns[0].ReadOnly = false;
            //            dataGridView1.DataSource = m_DataTable;
            //            dataGridView1.Columns[0].ReadOnly = true;
            //        }
            //        else
            //        {
            //            dataGridView1.DataSource = null;
            //            MessageBox.Show("没有满足条件的数据，请检查查询条件！");
            //        }

            //        //dataGridView1.Columns[0].ReadOnly = true;
            //    }
            //    catch (SystemException errs)
            //    { clsFunction.Function.MessageBoxError(errs.Message); }
            //}
            //catch (SystemException  errs)
            //{
            //    clsFunction.Function.MessageBoxError(errs.Message);
            //}

        }

        private void button2_Click(object sender, EventArgs e)
        {          
            try
            {//dataGridView2.Columns[0].ReadOnly == true;
                string m_strSql = "";
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    try
                    {
                        int objectid = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                        string XZQDM = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        string XMC = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string QSX = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        string HSX = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        string JCBH = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        string SJKDLBM = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        string QDLBM = dataGridView1.Rows[i].Cells[7].Value.ToString();
                        string ZYJBNT = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        double JBNTMJ = Convert.ToDouble(dataGridView1.Rows[i].Cells[9].Value);

                        string HDLBM = dataGridView1.Rows[i].Cells[10].Value.ToString();
                        double JCMJ = Convert.ToDouble(dataGridView1.Rows[i].Cells[11].Value);
                        string SFBH = dataGridView1.Rows[i].Cells[12].Value.ToString();
                        //string QDLBM = dataGridView1.Rows[i].Cells[13].Value.ToString();
                        //string HDLBM = dataGridView1.Rows[i].Cells[14].Value.ToString();
                        string XZMC = dataGridView1.Rows[i].Cells[13].Value.ToString();
                        double XZDWKD = Convert.ToDouble(dataGridView1.Rows[i].Cells[14].Value);

                        string WYHSZP = dataGridView1.Rows[i].Cells[15].Value.ToString();
                        string BZ = dataGridView1.Rows[i].Cells[16].Value.ToString();
                        string DKBH = dataGridView1.Rows[i].Cells[17].Value.ToString();
                        string DKFL = dataGridView1.Rows[i].Cells[18].Value.ToString();
                        string DKMJ = dataGridView1.Rows[i].Cells[19].Value.ToString();
                        string NYDMJ = dataGridView1.Rows[i].Cells[20].Value.ToString();
                        string GDMJ = dataGridView1.Rows[i].Cells[21].Value.ToString();
                        string JBNTMJI = dataGridView1.Rows[i].Cells[22].Value.ToString();
                        string WLYDMJ = dataGridView1.Rows[i].Cells[23].Value.ToString();
                        string SJYT = dataGridView1.Rows[i].Cells[24].Value.ToString();

                        string YDDW = dataGridView1.Rows[i].Cells[25].Value.ToString();
                        string XMLX = dataGridView1.Rows[i].Cells[26].Value.ToString();
                        string HFXSC = dataGridView1.Rows[i].Cells[27].Value.ToString();
                        string WFLX = dataGridView1.Rows[i].Cells[28].Value.ToString();
                        // int xffx = 0;
                        // if (dataGridView1.Rows[i].Cells[29].Value == DBNull.Value)
                        // {  xffx = 0; }
                        // else
                        // {
                        //      xffx = Convert.ToInt32(dataGridView1.Rows[i].Cells[29].Value);
                        // }

                        // int xcfx = 0;
                        // if (dataGridView1.Rows[i].Cells[30].Value == DBNull.Value)
                        // {  xcfx = 0; }
                        // else
                        // {
                        //      xcfx = Convert.ToInt32(dataGridView1.Rows[i].Cells[30].Value);
                        // }

                        // int wpfx = 0;
                        // if (dataGridView1.Rows[i].Cells[31].Value == DBNull.Value)
                        // {  wpfx = 0; }
                        // else
                        // {
                        //      wpfx = Convert.ToInt32(dataGridView1.Rows[i].Cells[31].Value);
                        // }
                        // int tdorkcwf = 0;
                        // if (dataGridView1.Rows[i].Cells[32].Value == DBNull.Value)
                        // {
                        //     tdorkcwf = 0;
                        // }
                        // else
                        // {

                        //     tdorkcwf = Convert.ToInt32(dataGridView1.Rows[i].Cells[32].Value);
                        // }

                        // int ajzt=0;
                        // if (dataGridView1.Rows[i].Cells[33].Value == DBNull.Value)
                        // {
                        //     ajzt = 0; 
                        // }

                        // else
                        // {
                        //     ajzt = Convert.ToInt32(dataGridView1.Rows[i].Cells[33].Value);
                        // }
                        // string lasj =null ;
                        // if (dataGridView1.Rows[i].Cells[34].Value == DBNull.Value)
                        // {
                        //     lasj = null;
                        // }
                        // else
                        // {
                        //      lasj = dataGridView1.Rows[i].Cells[34].Value.ToString();
                        // }





                        //double xzcf_chaichu=0;
                        //if (dataGridView1.Rows[i].Cells[35].Value == DBNull.Value)
                        //{
                        //   // xzcf_chaichu = 0;
                        //}
                        //else
                        //{
                        //    xzcf_chaichu = Convert.ToDouble(dataGridView1.Rows[i].Cells[35].Value);
                        //}
                        //double xzcf_moshou = 0;
                        //    if (dataGridView1.Rows[i].Cells[36].Value == DBNull.Value)
                        // {; }
                        // else
                        // {xzcf_moshou =Convert .ToDouble ( dataGridView1.Rows[i].Cells[36].Value);
                        //    }
                        //double xzcf_tuihuantd = 0;
                        //if (dataGridView1.Rows[i].Cells[37].Value == DBNull.Value)
                        //{ ; }
                        //else
                        //{
                        //    xzcf_tuihuantd = Convert.ToDouble(dataGridView1.Rows[i].Cells[37].Value);
                        //}

                        // double fk_yingshou= 0;
                        //if (dataGridView1.Rows[i].Cells[38].Value == DBNull.Value)
                        //{ ; }
                        //else
                        //{
                        //    fk_yingshou=Convert.ToDouble(dataGridView1.Rows[i].Cells[38].Value);
                        //}
                        // double fk_shishou= 0;
                        // if (dataGridView1.Rows[i].Cells[39].Value == DBNull.Value)
                        // { ; }
                        // else
                        // {
                        //     fk_shishou = Convert.ToDouble(dataGridView1.Rows[i].Cells[39].Value);
                        // }

                        // int shqfayuanzx=0;                     
                        // if (dataGridView1.Rows[i].Cells[40].Value == DBNull.Value)
                        // {; }
                        // else
                        // {
                        //     shqfayuanzx = Convert.ToInt32(dataGridView1.Rows[i].Cells[40].Value);
                        // }




                        // int fla_zltg=0;
                        // if (dataGridView1.Rows[i].Cells[41].Value == DBNull.Value)
                        // {; }
                        // else
                        // {
                        //     fla_zltg = Convert.ToInt32(dataGridView1.Rows[i].Cells[41].Value);
                        // }

                        // int fla_zxql=0;
                        // if (dataGridView1.Rows[i].Cells[42].Value == DBNull.Value)
                        // {; }
                        // else
                        // {
                        //     fla_zxql = Convert.ToInt32(dataGridView1.Rows[i].Cells[42].Value);
                        // }

                        // int fla_zdtt=0;
                        // if (dataGridView1.Rows[i].Cells[43].Value == DBNull.Value)
                        // {; }
                        // else
                        // {
                        //     fla_zdtt = Convert.ToInt32(dataGridView1.Rows[i].Cells[43].Value);
                        // }

                        // int fla_zqtbm=0;
                        // if (dataGridView1.Rows[i].Cells[44].Value == DBNull.Value)
                        // {; }
                        // else
                        // {
                        //     fla_zqtbm = Convert.ToInt32(dataGridView1.Rows[i].Cells[44].Value);
                        // }

                        // int fla_qt=0;
                        // if (dataGridView1.Rows[i].Cells[45].Value == DBNull.Value)
                        // {; }
                        // else
                        // {
                        //     fla_qt = Convert.ToInt32(dataGridView1.Rows[i].Cells[45].Value);
                        // }

                        // int sftccf=0;
                        // if (dataGridView1.Rows[i].Cells[46].Value == DBNull.Value)
                        // {; }
                        // else
                        // {
                        //     sftccf = Convert.ToInt32(dataGridView1.Rows[i].Cells[46].Value);
                        // }
                        //int tccfrs=0;
                        //if (dataGridView1.Rows[i].Cells[47].Value == DBNull.Value)
                        //{ ; }
                        //else
                        //{
                        //    tccfrs=Convert.ToInt32(dataGridView1.Rows[i].Cells[47].Value);
                        //}
                        //int yysong = 0;
                        //if (dataGridView1.Rows[i].Cells[48].Value == DBNull.Value)
                        //{ ; }
                        //else
                        //{
                        //    yysong = Convert.ToInt32(dataGridView1.Rows[i].Cells[48].Value);
                        //}


                        //string sql = "update T2009_WPJC_80_Temp  set XZQDM='"+XZQDM+"',XMC='"+XMC+"' ,QSX='"+QSX+"' ,HSX='"+HSX +"', JCBH='"+JCBH +"', SJKDLBM='"+SJKDLBM +"', QDLBM='"+QDLBM+"', ZYJBNT='"+ZYJBNT+"', JBNTMJ="+JBNTMJ 
                        //          + " ,HDLBM='"+HDLBM +"', JCMJ="+JCMJ +", SFBH='"+SFBH+"', XZMC='"+XZMC+"', XZDWKD="+XZDWKD
                        //          + ",WYHSZP='"+WYHSZP+"', BZ='"+BZ +"', DKBH='"+DKBH +"', DKFL='"+DKFL +"', DKMJ="+DKMJ +", NYDMJ='"+NYDMJ +"', GDMJ="+GDMJ +", JBNTMJI="+JBNTMJ +", WLYDMJ='"+WLYDMJ+"', SJYT= '"+SJYT
                        //          + "',YDDW='"+YDDW +"', XMLX='"+XMLX +"', HFXSC='"+HFXSC +"', WFLX='"+WFLX +"', xffx="+xffx +", xcfx="+xcfx +", wpfx="+wpfx +", tdorkcwf="+tdorkcwf +", ajzt="+ajzt +", lasj='"+lasj
                        //          + "',xzcf_chaichu='"+xzcf_chaichu+"', xzcf_moshou='"+xzcf_moshou+"', xzcf_tuihuantd='"+xzcf_tuihuantd+"',fk_yingshou='"+fk_yingshou+"', fk_shishou='"+fk_shishou+"',shqfayuanzx='"+shqfayuanzx+
                        //            "',fla_zltg='"+ fla_zltg+"',fla_zxql='"+fla_zxql+"', fla_zdtt='"+fla_zdtt+"', fla_zqtbm='"+fla_zqtbm+"', fla_qt='"+
                        //            "',sftccf='"+sftccf+"',tccfrs='"+tccfrs+"', yysong='"+yysong +"' where objectid="+objectid ;

                        
                         m_strSql = "update T2009_WPJC_80_Temp  set XZQDM='" + XZQDM + "',XMC='" + XMC + "' ,QSX='" + QSX + "' ,HSX='" + HSX + "', JCBH='" + JCBH + "', SJKDLBM='" + SJKDLBM + "', QDLBM='" + QDLBM + "', ZYJBNT='" + ZYJBNT + "', JBNTMJ=" + JBNTMJ
                                 + " ,HDLBM='" + HDLBM + "', JCMJ=" + JCMJ + ", SFBH='" + SFBH + "', XZMC='" + XZMC + "', XZDWKD=" + XZDWKD
                                 + ",WYHSZP='" + WYHSZP + "', BZ='" + BZ + "', DKBH='" + DKBH + "', DKFL='" + DKFL + "', DKMJ=" + DKMJ + ", NYDMJ='" + NYDMJ + "', GDMJ='" + GDMJ + "', JBNTMJI='" + JBNTMJ + "', WLYDMJ='" + WLYDMJ + "', SJYT= '" + SJYT
                                 + "',YDDW='" + YDDW + "', XMLX='" + XMLX + "', HFXSC='" + HFXSC + "', WFLX='" + WFLX + "'  where objectid=" + objectid;


                        m_DataAccess_SDE.ExecuteSQLNoReturn(m_strSql);

                         
                    }
                    catch (Exception ex)
                    {
                        clsFunction.Function.MessageBoxError(ex.Message+ i.ToString()  );
                        continue;
                    }
                }
                clsFunction.Function.MessageBoxInformation("更新成功！");
            }
            catch (Exception ex)
            {
                clsFunction.Function.MessageBoxError(ex.Message);
              }





        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count < 1) return;
            if (dataGridView1.SelectedRows[0].Cells[0].Value == DBNull.Value  || dataGridView1.SelectedRows[0].Cells[0].Value == null) return;

            string m_strOBJECTID = "";


            m_strOBJECTID = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            clsMapFunction.MapFunction.ZoomToSelFeaByFID("土地核查", "objectid", m_strOBJECTID, m_AxMapControl_);

        }






    }

     


}
