using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using System.Diagnostics;

namespace JCZF.SubFrame
{
    public class AssistantFunction
    {
        //定义数据库连接
        JCZF.SubFrame.DatabaseString m_DatabaseString = new JCZF.SubFrame.DatabaseString();

        /// <summary>
        /// 删除字符串中的空格
        /// </summary>
        /// <param name="motherString"></param>
        /// <returns></returns>
        public string EraseSpace(string motherString)
        {
            string result = motherString.Trim();
            return result;
        }

        /// <summary>
        /// 在带后缀名的文件名中取文件后缀名
        /// </summary>
        /// <param name="motherString"></param>
        /// <returns></returns>
        public string DocType(string motherString)
        {
            int position = motherString.LastIndexOf(".");
            if (position == -1)
                return "";
            else
            {
                string result = EraseSpace(motherString.Substring(position + 1).Trim());
                return result;
            }
        }



        /// <summary>
        /// 获取路径中的文件名（包含后缀名）
        /// </summary>
        /// <param name="motherString"></param>
        /// <returns></returns>
        public string GetFullFileName(string motherString)
        {
            int position = motherString.LastIndexOf("\\");
            string result = EraseSpace(motherString.Substring(position + 1).Trim());
            return result;
        }

        /// <summary>
        /// 获取路径中的文件名（不包含后缀名）
        /// </summary>
        /// <param name="motherString"></param>
        /// <returns></returns>
        public string NameWithoutType(string motherString)
        {
            int position = motherString.LastIndexOf(".");
            string[] r = motherString.Split('.');
            string result = EraseSpace(r[0]);
            return result;
        }

        /// <summary>
        /// 拆分字符串
        /// </summary>
        /// <param name="motherString"></param>
        /// <returns></returns>
        public string DocRecog(string motherString)
        {
            int position = motherString.LastIndexOf("\\");
            string result = motherString.Substring(position + 1);
            return result;
        }

        /// <summary>
        /// 从数据库中读取行政区信息
        /// </summary>
        public void XZQ_Load(System.Windows.Forms.ComboBox m_comboBox)
        {
            string sql = JCZF.SubFrame.DatabaseString.DBConnectString1();
            System.Data.OleDb.OleDbConnection mycon = new System.Data.OleDb.OleDbConnection(sql);

            string SQL = "SELECT JDMC FROM XZQ";

            System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(SQL, mycon);

            try
            {

                DataSet dataset1 = new DataSet();
                da.Fill(dataset1, "QXDM");

                for (int i = 0; i < dataset1.Tables[0].Rows.Count; i++)
                {
                    m_comboBox.Items.Add(dataset1.Tables[0].Rows[i][0]);
                }
            }
            catch (System.Exception errs)
            {
                MessageBox.Show(errs.ToString(), "错误提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                mycon.Close();
            }
        }


        /// <summary>
        ///  构建树的根节点
        /// </summary>
        /// <param name="XZQTree"></param>
        public void BuildTreeRoot(TreeView Tree, string strTableName)
        {
            string sql = JCZF.SubFrame.DatabaseString.DBConnectString1();
            System.Data.OleDb.OleDbConnection mycon = new System.Data.OleDb.OleDbConnection(sql);

            string SQL = " SELECT JDID,JDMC FROM " + strTableName + " WHERE FJDID = '0'";

            System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(SQL, mycon);
            try
            {

                DataSet dataset1 = new DataSet();
                da.Fill(dataset1, "CG");

                for (int i = 0; i < dataset1.Tables[0].Rows.Count; i++)
                {
                    string RootID = EraseSpace((string)dataset1.Tables[0].Rows[i][0]);
                    string RootName = EraseSpace((string)dataset1.Tables[0].Rows[i][1]);
                    TreeNode TreeRoot = new TreeNode(RootName);
                    TreeRoot.ImageIndex = 0;
                    TreeRoot.Tag = RootID;
                    Tree.Nodes.Add(TreeRoot);
                }

            }
            catch (System.Exception errs)
            {
                MessageBox.Show(errs.Message, "错误提示");
            }
            finally
            {
                mycon.Close();
            }
        }

        /// <summary>
        /// 构建树的其他节点
        /// </summary>
        /// <param name="tnc"></param>
        /// <param name="XZQTree"></param>
        /// <param name="strTableName"></param>
        public void BuildOtherTreeNode(TreeNodeCollection tnc, TreeView Tree, string strTableName)
        {
            foreach (TreeNode node in tnc)
            {
                string CurrentNodeID = node.Tag.ToString();

                string sql = JCZF.SubFrame.DatabaseString.DBConnectString1();
                System.Data.OleDb.OleDbConnection mycon = new System.Data.OleDb.OleDbConnection(sql);

                string SQL = " SELECT JDID,JDMC,FJDID FROM " + strTableName + " WHERE FJDID ='" + CurrentNodeID + "'";
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(SQL, mycon);

                try
                {

                    try
                    {
                        DataSet dataset1 = new DataSet();
                        da.Fill(dataset1, "CG");
                        if (dataset1.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dataset1.Tables[0].Rows.Count; i++)
                            {
                                string NodeID = EraseSpace((string)dataset1.Tables[0].Rows[i][0]);
                                string NodeName = EraseSpace((string)dataset1.Tables[0].Rows[i][1]);
                                string NodeParent = EraseSpace((string)dataset1.Tables[0].Rows[i][2]);

                                TreeNode m_ChildNode = new TreeNode(NodeName);
                                m_ChildNode.Tag = NodeID;
                                node.Nodes.Add(m_ChildNode);
                                funNodeType(NodeName,m_ChildNode);
                                //Icon(m_ChildNode);

                            }
                            BuildOtherTreeNode(node.Nodes, Tree, strTableName);
                        }
                    }
                    catch (System.Exception errs)
                    {
                        MessageBox.Show(errs.Message, "错误提示");
                    }
                }
                catch (System.Exception errs)
                {
                    MessageBox.Show(errs.Message, "错误提示");
                }
                finally
                {
                    mycon.Close();
                }

            }
        }

        /// <summary>
        /// 节点类型判断
        /// </summary>
        /// <param name="NodeName"></param>
        /// <param name="tmp"></param>
    
        public void funNodeType(string NodeName, TreeNode tmp)
        {
            switch (DocType(NodeName))
            {
                case "doc":
                    {
                        tmp.ImageIndex = 1;
                        break;
                    }
                case "DOC":
                    {
                        tmp.ImageIndex = 1;
                        break;
                    }
                case "jpg":
                    {
                        tmp.ImageIndex = 2;
                        break;
                    }
                case "JPG":
                    {
                        tmp.ImageIndex = 2;
                        break;
                    }
                case "bmp":
                    {
                        tmp.ImageIndex = 2;
                        break;
                    }
                case "BMP":
                    {
                        tmp.ImageIndex = 2;
                        break;
                    }
                case "xls":
                    {
                        tmp.ImageIndex = 5;
                        break;
                    }
                case "XLS":
                    {
                        tmp.ImageIndex = 5;
                        break;
                    }
                case "txt":
                    {
                        tmp.ImageIndex = 1;
                        break;
                    }
                case "TXT":
                    {
                        tmp.ImageIndex = 1;
                        break;
                    }
                case "mxd":
                    {
                        tmp.ImageIndex = 6;
                        break;
                    }
                case "MXD":
                    {
                        tmp.ImageIndex = 6;
                        break;
                    }
                case "mdb":
                    {
                        tmp.ImageIndex = 4;
                        break;
                    }
                case "MDB":
                    {
                        tmp.ImageIndex = 4;
                        break;
                    }
                default:
                    {
                        tmp.ImageIndex = 0;
                        break;
                    }
            }
        }

        public void GetSelectNode(TreeView m_treeView)
        {
            foreach (TreeNode tRoot in m_treeView.Nodes)
            {
                if (tRoot.IsExpanded == true)
                {
                    tRoot.ImageIndex = 7;
                }
                GetSelectNode(tRoot.TreeView);
                //else
                //{
                //    if (tRoot.ChildNodes != null)
                //    {
                //        //foreach (TreeNode tChild in tRoot.ChildNodes)
                //        //{
                //        //    if (tChild.Value == sNodeValue)
                //        //        tChild.Select();
                //        //}
                //        TreeNode tTmp = null;
                //        tTmp = FindNode(tRoot, sNodeValue);
                //        if (tTmp != null)
                //            tTmp.Select();
                //    }
                //}
            }

        }

    }
}
