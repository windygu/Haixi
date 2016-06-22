using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace JCZF.SubFrame.WGGL
{
    
    class clsZFDW
    {
        /// <summary>
        /// 数据库访问对象
        /// </summary>
        public clsDataAccess.DataAccess m_DataAccess_SYS_;

        private  string m_strZFDW_TableName_;
        /// <summary>
        /// 执法单位表格名称
        /// </summary>
        public string m_strZFDW_TableName
        {
            set
            {
                m_strZFDW_TableName_ = value;
            }
            get
            {
               return  m_strZFDW_TableName_ ;
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


        /// <summary>
        ///执法单位所属行政区
        /// </summary>
        public string m_strXZQDM;

        public clsZFDW()
        {
            m_strZFDW_TableName_ = "ZFDW";
        }

        public string CreatNewZFDWDM()
        {
            return CreatNewZFDWDM(m_strXZQDM);
        }

        public int GetZFDWDM_Count(string p_strXZQDM)
        {
            string m_strSQL = "";
            int m_intCount = 0;

            if (p_strXZQDM.Length == 2)
            {
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    m_strSQL = "select count(zfdwdm) from " + m_strZFDW_TableName_ + " where substr(zfdwdm,1,6)='" + p_strXZQDM + "0000" + "'  ";
                }
                else
                {
                    m_strSQL = "select count(zfdwdm) from " + m_strZFDW_TableName_ + " where left(zfdwdm,6)='" + p_strXZQDM + "0000" + "'  ";
                }
            }
            else if (p_strXZQDM.Length == 4)
            {
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    m_strSQL = "select count(zfdwdm) from " + m_strZFDW_TableName_ + " where substr(zfdwdm,1,6)='" + p_strXZQDM + "00" + "'  ";
                }
                else
                {
                    m_strSQL = "select count(zfdwdm) from " + m_strZFDW_TableName_ + " where left(zfdwdm,6)='" + p_strXZQDM + "00" + "'  ";
                }
            }
            else if (p_strXZQDM.Length >= 6)
            {
                if (m_DataAccess_SYS_.ProviderIsOraDB())
                {
                    m_strSQL = "select count(zfdwdm) from " + m_strZFDW_TableName_ + " where substr(zfdwdm,1,6)='" + p_strXZQDM.Substring(0, 6) + "'  ";
                }
                else
                {
                    m_strSQL = "select count(zfdwdm) from " + m_strZFDW_TableName_ + " where left(zfdwdm,6)='" + p_strXZQDM.Substring(0, 6) + "'  ";
                }
            }
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

       public string CreatNewZFDWDM(string p_strXZQDM)
        {
            string m_strNewZFDWDM = "";
             int m_intCount = 0;
             string m_strCount = "";
             m_intCount = GetZFDWDM_Count(p_strXZQDM)+1;

             if (m_intCount < 10)
             {
                 m_strCount = "0" + m_intCount.ToString();
             }
             else
             {
                 m_strCount = m_intCount.ToString();
             }
             
             if (p_strXZQDM.Length == 2)
             {
                 m_strNewZFDWDM = p_strXZQDM + "0000" + m_strCount;
             }
             else if(p_strXZQDM.Length == 4)
             {
                 m_strNewZFDWDM = p_strXZQDM + "00" + m_strCount;
             }
             else if (p_strXZQDM.Length >= 6)
             {
                 m_strNewZFDWDM = p_strXZQDM.Substring(0,6)  + m_strCount;
             }
             
             return m_strNewZFDWDM;
        }

        public void  Delete()
        {
            string m_strSQL = "DELETE FROM ZFDW WHERE ZFDWDM='" + m_strZFDW_ZFDWDM_ + "'";
            m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);

        }


         public void  Delete(string p_strZFDWDM)
        {
            string m_strSQL = "DELETE FROM ZFDW WHERE ZFDWDM='" + p_strZFDWDM + "'";
            m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
        }

        public void  Add()
        {
            string m_strSQL = "insert into   ZFDW (zfdwdm,zfdwmc,djsj ) values('" + m_strZFDW_ZFDWDM_ + "','" + m_strZFDW_ZFDWMC_ + "','" + DateTime.Now.ToShortDateString() + "')";
            m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
        }

        public void  Add(string p_strZFDWDM, string p_strZFDWMC)
        {
            string m_strSQL = "insert into   ZFDW (zfdwdm,zfdwmc,djsj ) values('" + p_strZFDWDM + "','"+p_strZFDWMC+"','"+DateTime.Now.ToShortDateString()+"')";
            m_DataAccess_SYS_.ExecuteSQLNoReturn(m_strSQL);
        }

        //public string Update()
        //{

        //}

         public DataTable GetZFDW_By_XZQDM(string p_strXZQDM)
        {
            string m_strSQL;
            if (m_DataAccess_SYS_.ProviderIsOraDB())
            {
                m_strSQL = "SELECT *  FROM ZFDW where substr(zfdwdm,1," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'  ";
            }
            else
            {
                m_strSQL = "SELECT *  FROM ZFDW where left(zfdwdm," + p_strXZQDM.Length + ")='" + p_strXZQDM + "'  ";
            }
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            return m_DataTable;
        }

        public DataTable GetZFDW()
        {
            string m_strSQL ;
            if (m_DataAccess_SYS_.ProviderIsOraDB())
            {
                m_strSQL = "SELECT *  FROM ZFDW where substr(zfdwdm," + m_strXZQDM.Length + ")='" + m_strXZQDM + "'  ORDER BY ZFDWDM ";
            }
            else
            {
                m_strSQL = "SELECT *  FROM ZFDW where left(zfdwdm," + m_strXZQDM.Length + ")='" + m_strXZQDM + "'  ORDER BY ZFDWDM ";
            }
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            return m_DataTable;
        }


         public DataRow GetZFDW_By_ZFDWDM(string p_strZFDWDM)
        {
            string m_strSQL = "SELECT *  FROM ZFDW where ZFDWDM='" + p_strZFDWDM + "'  ";
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable == null || m_DataTable.Rows.Count<1)
            {
                return null;
            }
            
            return m_DataTable.Rows[0];
        }

        public string GetZFDWMC()
        {
            string m_strZFDWMC = "";
            string m_strSQL = "SELECT ZFDWMC FROM ZFDW where ZFDWDM='" + m_strZFDW_ZFDWDM_ + "'  ";
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                m_strZFDWMC =m_DataTable.Rows[0][0].ToString();
            }
            return m_strZFDWMC;
        }

        public string GetZFDWDM()
        {
            string m_strZFDWDM = "";
            string m_strSQL = "SELECT ZFDWDM FROM ZFDW where ZFDWMC='" + m_strZFDW_ZFDWMC_ + "'  ";
            DataTable m_DataTable = m_DataAccess_SYS_.getDataTableByQueryString(m_strSQL);
            if (m_DataTable != null && m_DataTable.Rows.Count > 0)
            {
                m_strZFDWDM = m_DataTable.Rows[0][0].ToString();
            }

            return m_strZFDWDM;
        }
    }
}
