using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace JCZF.SubFrame.WGGL
{
    class clsZFRY
    {
        /// <summary>
        /// 数据库访问对象
        /// </summary>
        public clsDataAccess.DataAccess m_DataAccess_SYS_;

        private  string m_strZFRY_TableName_;
        /// <summary>
        /// 执法单位表格名称
        /// </summary>
        public string m_strZFRY_TableName
        {
            set
            {
                m_strZFRY_TableName_ = value;
            }
            get
            {
               return  m_strZFRY_TableName_ ;
            }
        }

        private string m_strZFDW_ZFDWDM_;
        /// <summary>
        /// 执法单位代码
        /// </summary>
        public string m_strZFDW_ZFDWDM
        {
            set
            {
                m_strZFDW_ZFDWDM_ = value;
            }
            get
            {
                return m_strZFDW_ZFDWDM_;
            }
        }

        private string m_strZFDW_ZFDWMC_;
        /// <summary>
        /// 执法单位名称
        /// </summary>
        public string m_strZFDW_ZFDWMC
        {
            set
            {
                m_strZFDW_ZFDWMC_ = value;
            }
            get
            {
                return m_strZFDW_ZFDWMC_;
            }
        }

        private string m_strZFRY_ZFRYXM_;
        /// <summary>
        /// 执法人员姓名
        /// </summary>
        public string m_strZFRY_ZFRYXM
        {
            set
            {
                m_strZFRY_ZFRYXM_ = value;
            }
            get
            {
                return m_strZFRY_ZFRYXM_;
            }
        }

        private string m_strZFRY_ZFRYBH_;
        /// <summary>
        /// 执法人员编号
        /// </summary>
        public string m_strZFRY_ZFRYBH
        {
            set
            {
                m_strZFRY_ZFRYBH_ = value;
            }
            get
            {
                return m_strZFRY_ZFRYBH_;
            }
        }

        private string m_strZFRY_SJ_;
        /// <summary>
        /// 执法人员手机号
        /// </summary>
        public string m_strZFRY_SJ
        {
            set
            {
                m_strZFRY_SJ = value;
            }
            get
            {
                return m_strZFRY_SJ_;
            }
        }



        /// <summary>
        ///执法单位所属行政区
        /// </summary>
        public string m_strXZQDM;

        public clsZFRY()
        {
            m_strZFRY_TableName_ = "ZFRY";
        }

        public string CreatNewZFRYBH()
        {
            return CreatNewZFRYBH(m_strZFDW_ZFDWDM_);
        }

        public int GetZFRYBH_Count(string p_strZFDWDM)
        {
            string m_strSQL = "";
            int m_intCount = 0;
           
            m_strSQL = "select count(zfrybh) from " + m_strZFRY_TableName_ + " where zfdwdm='" + p_strZFDWDM + "'  ";

            if (m_strSQL != null)
            {
                DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
                if (m_DataTable != null && m_DataTable.Rows.Count>0)
                {
                    m_intCount = System.Convert.ToInt32(m_DataTable.Rows[0][0].ToString());
                }
            }
            //m_intMax = m_intMax + 1;
            return m_intCount;
        }


        /// <summary>
        /// 获得某单位执法人员最大编号
        /// </summary>
        /// <param name="p_strZFDWDM"></param>
        /// <returns></returns>
        public Int64   GetZFRYBH_Max(string p_strZFDWDM)
        {
            string m_strSQL = "";
            string  m_strZFRYBH_MAX = "0";

            long  m_intZFRYBH_MAX = 0;

            m_strSQL = "select zfrybh  from " + m_strZFRY_TableName_ + " where zfdwdm='" + p_strZFDWDM + "' order by zfrybh desc ";

            
                DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
                if (m_DataTable != null && m_DataTable.Rows.Count > 0)
                {
                    m_strZFRYBH_MAX =m_DataTable.Rows[0][0].ToString();
                }

                return System.Convert.ToInt64(m_strZFRYBH_MAX);
                
        }

        public string CreatNewZFRYBH(string p_strZFDWDM)
        {
            string m_strNewZFDWDM = "";
            Int64 m_intNewZFDWDM = 0;
             string m_strCount = "";
             m_intNewZFDWDM = GetZFRYBH_Max(p_strZFDWDM) + 1;

             //if (m_intCount < 10)
             //{
             //    m_strCount = "00" + m_intCount.ToString();
             //}
             //else if (m_intCount < 100)
             //{
             //    m_strCount = "0" + m_intCount.ToString();
             //}
             //else {
             //    m_strCount = m_intCount.ToString();
             //}

             //m_strNewZFDWDM = p_strZFDWDM + m_strCount;
             m_strNewZFDWDM = m_intNewZFDWDM.ToString();
             return m_strNewZFDWDM;
        }

        public void  Delete()
        {
            Delete(m_strZFRY_ZFRYBH_);
        }


        public void Delete(string p_strZFRY_ZFRYBH)
        {
            if (p_strZFRY_ZFRYBH == null)
            {
                clsFunction.Function.MessageBoxWarning("执法人员编号不能为空！");
                return;
            }
            DataTable m_DataTable = GetZFWG_By_ZFRYBH();

            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                //判断是否拥有执法网格

                if (IsLZ() == true)
                {
                    //如果已经离职，则提示不能删除！
                    clsFunction.Function.MessageBoxWarning("执法人员 "+m_strZFRY_ZFRYXM_ +" 负责相关行政区的执法工作，不能删除！");
                }
                else
                {
                    //如果没有离职，提示是否将状态改为离职
                    if (clsFunction.Function.MessageBoxQuestion("执法人员 " + m_strZFRY_ZFRYXM_ + " 负责相关行政区的执法工作，不能删除！\r\n\r\n是否将该执法人员变为离职状态？") == System.Windows.Forms.DialogResult.Yes)
                    {
                        SetZFRY_ZZZT_LZ();
                    }
                }
            }
            else
            {
                Delete_By_ZFRYBH(m_strZFRY_ZFRYBH_);
            }
           
        }

        private  bool  Delete_By_ZFRYBH(string p_strZFRYBH)
        {
            if (p_strZFRYBH == null)
            {
                clsFunction.Function.MessageBoxWarning("执法人员编号不能为空！");
                return false  ;
            }
            try
            {
                if (clsFunction.Function.MessageBoxQuestion("将删除执法人员 " + m_strZFRY_ZFRYXM_ + " 的所有信息！\r\n\r\n是否继续删除？") == System.Windows.Forms.DialogResult.Yes)
                {
                    string m_strSQL = "DELETE FROM ZFRY WHERE ZFRYBH='" + m_strZFRY_ZFRYBH_ + "'";
                    m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                }
              
            }
            catch
            {
                return false;
            }
            return true;
        }


        public DataTable GetZFWG_By_ZFRYBH()
        {
            string m_strSQL = "";

            m_strSQL = "SELECT *  FROM ZFWG where SJZFRYBH='" + m_strZFRY_ZFRYBH_ + "' OR BJZFRYBH='" + m_strZFRY_ZFRYBH_ + "'";
           
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            return m_DataTable;
    }

        public int GetZFRY_ZZZT()
        {

           return  GetZFRY_ZZZT_By_ZFRYBH(m_strZFRY_ZFRYBH_);
        }


        public int GetZFRY_ZZZT_By_ZFRYBH(string p_strZFRYBH)
        {
            if (p_strZFRYBH == null)
            {
                clsFunction.Function.MessageBoxWarning("执法人员编号不能为空！");
                return -1;
            }

            string m_strSQL = "";

            m_strSQL = "SELECT zzzt  FROM ZFry where ZFRYBH='" + p_strZFRYBH + "'";

            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);

            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                int m_intZZZT = System.Convert.ToInt32(m_DataTable.Rows[0]["ZZZT"]);

                return m_intZZZT;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 判断执法人员是否已离职
        /// </summary>
        /// <returns></returns>
        public bool IsLZ()
        {
            return IsLZ(m_strZFRY_ZFRYBH_);
        }

        /// <summary>
        /// 判断执法人员是否已离职
        /// </summary>
        /// <param name="p_strZFRYBH"></param>
        /// <returns></returns>
        public bool  IsLZ(string p_strZFRYBH)
        {
            if (p_strZFRYBH == null)
            {
                clsFunction.Function.MessageBoxWarning("执法人员编号不能为空！");
                return false ;
            }
            int m_intZZZT = 0;
            m_intZZZT = GetZFRY_ZZZT_By_ZFRYBH(p_strZFRYBH);
            if (m_intZZZT == 1)
            {
                return false ;
            }
            return true ;
        }


        /// <summary>
        /// 设执法人员为离职状态
        /// </summary>
        /// <returns></returns>
        public bool SetZFRY_ZZZT_LZ()
        {
            return (SetZFRY_ZZZT_LZ_By_ZFRYBH(m_strZFRY_ZFRYBH_));
        }

        /// <summary>
        /// 设执法人员为离职状态
        /// </summary>
        /// <returns></returns>
        public bool SetZFRY_ZZZT_LZ_By_ZFRYBH(string p_strZFRYBH)
        {
            if (p_strZFRYBH == null)
            {
                clsFunction.Function.MessageBoxWarning("执法人员编号不能为空！");
                return false ;
            }
           return (SetZFRY_ZZZT_By_ZFRYBH(p_strZFRYBH,0));
        }


       /// <summary>
        /// 设执法人员在职状态
       /// </summary>
       /// <param name="p_strZFRYBH"></param>
       /// <param name="p_intZZZT">在职状态，0为离职，1为在职</param>
       /// <returns></returns>
        public bool SetZFRY_ZZZT_By_ZFRYBH(string p_strZFRYBH, int p_intZZZT)
        {
            if (p_strZFRYBH == null)
            {
                clsFunction.Function.MessageBoxWarning("执法人员编号不能为空！");
                return false ;
            }
            try
            {
                string m_strZFRYXM = "";
                string m_strSQL = "UPDATE   ZFRY SET  ZZZT=" + p_intZZZT + " where zfrybh='" + p_strZFRYBH + "'";
                m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
                return true ;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Add()
        {
            try
            {

                string m_strSQL = "insert into   ZFRY (zfdwdm,zfdwmc,djsj ) values('" + m_strZFDW_ZFDWDM_ + "','" + m_strZFDW_ZFDWMC_ + "','" + DateTime.Now.ToShortDateString() + "')";
                m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
            }
            catch
            {
            }
        }

        public void  Add(string p_strZFDWDM, string p_strZFDWMC)
        {
            try
            {
                string m_strSQL = "insert into   ZFRY (zfdwdm,zfdwmc,djsj ) values('" + p_strZFDWDM + "','" + p_strZFDWMC + "','" + DateTime.Now.ToShortDateString() + "')";
                m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
            }
            catch
            {
            }
        }

        //public string Update()
        //{

        //}

        public DataTable GetZFRY_By_ZFDWDM(string p_strZFDWDM)
        {
            string m_strSQL = "SELECT *  FROM ZFRY where zfdwdm='" + p_strZFDWDM + "'  ";
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            return m_DataTable;
        }

        public DataTable GetZFRY()
        {
            string m_strSQL = "";
            if (m_DataAccess_SYS_.ProviderIsOraDB())
            {
                m_strSQL = "SELECT *  FROM ZFRY where substr(zfdwdm,"+m_strZFDW_ZFDWDM_.Length +")='" + m_strZFDW_ZFDWDM_ + "'";
            }
            else
            {
                m_strSQL = "SELECT *  FROM ZFRY where left(zfdwdm," + m_strZFDW_ZFDWDM_.Length + ")='" + m_strZFDW_ZFDWDM_ + "'";
            }
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            return m_DataTable;
        }

        public DataTable GetZFRY_By_XZQDM(string p_strXZQDM)
        {
            string m_strSQL ;
            if (m_DataAccess_SYS_.ProviderIsOraDB())
            {
                m_strSQL = "SELECT *  FROM ZFRY where substr(zfdwdm,1," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'  ";
            }
            else
            {
                m_strSQL = "SELECT *  FROM ZFRY where left(zfdwdm," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'  ";
            }
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            return m_DataTable;
        }


         public DataRow GetZFRY_By_ZFRYBH(string p_strZFRYBH)
        {
            string m_strSQL = "SELECT *  FROM ZFRY where ZFRYBH='" + p_strZFRYBH + "'  ";
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable == null || m_DataTable.Rows.Count<1)
            {
                return null;
            }
            
            return m_DataTable.Rows[0];
        }

        public string GetZFRYXM()
        {
            string m_strZFRYXM = "";
            string m_strSQL = "SELECT ZFRYXM FROM ZFRY where ZFRYBH='" + m_strZFRY_ZFRYBH_ + "'  ";
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                m_strZFRYXM = m_DataTable.Rows[0][0].ToString();
            }
            return m_strZFRYXM;
        }

        public DataRowCollection GetZFRYBH()
        {
            //string m_strZFRYBH = "";
            string m_strSQL = "SELECT ZFRYBH FROM ZFRY where ZFRYXM='" + m_strZFRY_ZFRYXM_ + "'  ";
            DataRowCollection m_DataRowCollection = m_DataAccess_SYS_.getDataRowsByQueryString(m_strSQL);

            return m_DataRowCollection;
        }

        public string GetSJ_By_ZFRYBH(string p_strZFRYBH)
        {  string m_strSJ = "";
            try
            {
              
                string m_strSQL = "SELECT SJ FROM ZFRY where ZFRYBH='" + p_strZFRYBH + "'  ";
                DataRowCollection m_DataRowCollection = m_DataAccess_SYS_.getDataRowsByQueryString(m_strSQL);
                if (m_DataRowCollection != null)
                {
                    if (m_DataRowCollection[0]["SJ"] != null && m_DataRowCollection[0]["SJ"] != System.DBNull.Value)
                    {
                        m_strSJ = m_DataRowCollection[0]["SJ"].ToString();
                    }
                }
            }
            catch(SystemException errs)
            {
            }

            return m_strSJ;
        }
    }
}
