using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace ComprehensiveEvaluation
{
    public partial class SystemSet : Form
    {
        public static string Base_Map = @"G:\四平项目\国情应用数据";
        //private ExeConfigurationFileMap file;
        //private Configuration config1;
        //private ConfigSectionData data1;\
        public string User_DB = @"G:\用户数据库.accdb";
        public SystemSet()
        {
            //file = new ExeConfigurationFileMap();
            InitializeComponent();
            //file.ExeConfigFilename = "setting.config";
            //set_Value();
        }
        //private void set_Value()
        //{
        //    config1 = ConfigurationManager.OpenMappedExeConfiguration(file, ConfigurationUserLevel.None);
        //    data1 = config1.Sections["add"] as ConfigSectionData;
        //    Base_Map = textBoxX1.Text = System.IO.Path.Combine(data1.Base_Map);

        //}

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SystemSet_Load(object sender, EventArgs e)
        {
            //textBoxX1.Text = System.IO.Path.Combine(@"G:\数据库\坐标数据");
            //textBoxX2.Text = System.IO.Path.Combine(@"G:\数据库\图层数据");
            //textBoxX3.Text = System.IO.Path.Combine(@"G:\数据库\地图数据");
            //textBoxX4.Text = System.IO.Path.Combine(@"G:\数据库\统计数据");
        }
        private void save(string type,string name,string title,TextBox textBox)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = type;// "shp文件 (*.shp)|*.shp";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = title;
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = name;

            DialogResult dr = saveDlg.ShowDialog();
            if (dr == DialogResult.OK)
                textBox.Text = saveDlg.FileName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            save("文本文件 (*.txt)|*.txt", DateTime.Now.ToLongDateString(), "坐标输出", textBoxX1);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            save("shp文件 (*.shp)|*.shp", DateTime.Now.ToLongDateString(), "图层输出", textBoxX2);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            save("地图文件 (*.mxd)|*.mxd", DateTime.Now.ToLongDateString(), "地图输出", textBoxX3);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            save("表文件 (*.exl)|*.exl", DateTime.Now.ToLongDateString(), "统计输出", textBoxX4);
        }
    }
    //class ConfigSectionData : ConfigurationSection
    //{

    //    [ConfigurationProperty("Base_Map")]

    //    public string Base_Map
    //    {

    //        get { return (string)this["Base_Map"]; }

    //        set { this["Base_Map"] = value; }

    //    }



        //[ConfigurationProperty("Database")]

        //public string Deal_path
        //{

        //    get { return (string)this["Database"]; }

        //    set { this["Database"] = value; }

        //}

        //[ConfigurationProperty("StoreBase")]

        //public string mod
        //{

        //    get { return (string)this["StoreBase"]; }

        //    set { this["StoreBase"] = value; }

        //}


        //[ConfigurationProperty("report")]

        //public string report
        //{

        //    get { return (string)this["report"]; }

        //    set { this["report"] = value; }

        //}
        //[ConfigurationProperty("Coor_ref")]

        //public string Coor_ref
        //{

        //    get { return (string)this["Coor_ref"]; }

        //    set { this["Coor_ref"] = value; }

        //}
    //}
}
