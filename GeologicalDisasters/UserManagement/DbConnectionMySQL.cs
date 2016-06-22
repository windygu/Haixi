using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
namespace ComprehensiveEvaluation.UserManagement
{

    public class DbConnectionMySQL
    {

        private static MySqlConnection conn;
        private static DataTable dt;
        private static MySqlDataAdapter da;
        private static MySqlCommandBuilder cb;
        private static DataGrid dataGrid;
        //建立数据库链接
        public MySqlConnection getMySqlCon()
        {
            string MysqlCon = "server=localhost;user id=root;password=noroot;database=haixi";
            MySqlConnection myCon = new MySqlConnection(MysqlCon);
            return myCon;
        }
        //执行数据库操作
        public void getMysqlCmd(string sqlWord)
        {
            MySqlConnection mysqlcon = this.getMySqlCon();
            mysqlcon.Open();
            MySqlCommand mysqlcmd = new MySqlCommand(sqlWord, mysqlcon);
            mysqlcmd.ExecuteNonQuery();
            mysqlcmd.Dispose();
            mysqlcon.Close();
            mysqlcon.Dispose();
        }
        public MySqlDataReader getMysqlRead(string sqlWord)
        {
            MySqlConnection mysqlcon = this.getMySqlCon();
            MySqlCommand mysqlcmd = new MySqlCommand(sqlWord, mysqlcon);
            mysqlcon.Open();
            MySqlDataReader mysqlread = mysqlcmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return mysqlread;
        }
        public static void connection(string server, string userid, string password, DevComponents.DotNetBar.ListBoxAdv databaseList)
        {
            if (conn != null)
            {
                conn.Close();
            }
            string constr = string.Format("server={0};user id={1}; password={2}; port={3}; database=haixi; pooling=false; charset=utf8", server, userid, password, 3306);
            try
            {
                conn = new MySqlConnection(constr);
    
                  conn.Open(); 
              GetDatabases(databaseList);
                MessageBox.Show("连接数据库成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void GetDatabases(DevComponents.DotNetBar.ListBoxAdv databaseList)
        {
            
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand("SHOW DATABASES", conn);
            try
            {
                reader = cmd.ExecuteReader();
                databaseList.Items.Clear();
                while (reader.Read())
                {
                    databaseList.Items.Add(reader.GetString(0));

                }

            }

            catch (MySqlException ex)
            {

                MessageBox.Show("Failed to populate database list: " + ex.Message);

            }

            finally
            {

                if (reader != null)
                    reader.Close();

            }

        }

        public static void showTables(DevComponents.DotNetBar.ListBoxAdv databaseList, DevComponents.DotNetBar.ListBoxAdv tablesList)
        {
            MySqlDataReader reader = null;
            conn.ChangeDatabase(databaseList.SelectedItem.ToString());
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES",conn);
            try
            {
                reader = cmd.ExecuteReader();
                tablesList.Items.Clear();
                while (reader.Read())
                {
                    tablesList.Items.Add(reader.GetString(0));
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }

}
