using System;
using System.Collections.Generic;
using System.Text;
using clsDataAccess;

namespace JCZF.SubFrame
{
    public class DatabaseString
    {
        static public DataAccess m_DataAccess_SYS;

        static public string DBConnectString1()
        {
            string SqlConnectString = "Server=" + m_DataAccess_SYS.Server + ";Database=" + m_DataAccess_SYS.DataBase + ";User ID=" + m_DataAccess_SYS.UserID + ";PassWord=" + m_DataAccess_SYS.Password + ";";

            return SqlConnectString;
        }

        static public string DBConnectString2()
        {
            string strConnection = "user id=" + m_DataAccess_SYS.UserID + ";password=" + m_DataAccess_SYS.Password + ";";
            strConnection += "initial catalog=" + m_DataAccess_SYS.DataBase + ";Server=" + m_DataAccess_SYS.Server + ";";
            strConnection += "Connect Timeout=30";

            return strConnection;
        }
        static public string server()
        {
            return m_DataAccess_SYS.Server;
        }

        static public string Instance()
        {
            return m_DataAccess_SYS.Instance;
        }

        static public string Service()
        {
            return m_DataAccess_SYS.Service;
        }

        static public string DataBase()
        {
            return m_DataAccess_SYS.DataBase;
        }

        static public string UserID()
        {
            return m_DataAccess_SYS.UserID;
        }

        static public string Password()
        {
            return m_DataAccess_SYS.Password;
        }



    }
    
}
