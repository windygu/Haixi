using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComprehensiveEvaluation.TargetSystem
{
    class targetManager
    {
        public static bool IsWeight = false;
        
        //土地适宜性评价指标
        public static string land_target = "土地适宜性评价指标";
        public static string land_t1 = "自然因素";
        public static string land_t11 = "高程";
        public static string land_t12 = "坡度";
        public static string land_t13= "坡向";
        public static string land_t14 = "地基承载力（t/m²）";
        public static string land_t15 = "河流缓冲区";
        public static string land_t16= "地形复杂度（%）";
        public static string land_t2= "社会经济因素";
        public static string land_t21= "土地利用现状";
        //public static string land_t211= "工矿居民地";
        //public static string land_t212= "旱地";
        //public static string land_t213= "草地";
        public static string land_t22= "建成区";
        public static string land_t23 = "国道、省道";
        public static string land_t24 = "县道、乡道";
        public static string land_t3 = "生态安全因素";
        public static string land_t31 = "基本农田保护区";
        public static string land_t32 = "自然保护区";
        public static string land_t33 = "水域保护区";
        public static string land_t34= "人文景观";

        //土地灾害风险评价指标
        public static string risk_target = "土地灾害风险评价指标";
        public static string risk_t1 = "危险性";
        public static string risk_t11 = "道路高程综合指标";
        public static string risk_t12 = "危险性城市距离";
        public static string risk_t13 = "坡度";
        public static string risk_t14 = "比值植被指数（RVI）";
        public static string risk_t2 = "易损性";
        public static string risk_t21 = "道路距离";
        public static string risk_t22 = "易损性城市距离";
        public static string risk_t23 = "人口密度";
        public static string risk_t24 = "道路密度";
        public static string risk_t25 = "城镇密度";
        //public static string risk_t1 = "";
        //public static string risk_t1 = "";
        //public static string risk_t1 = "";
        
        
        //土地生态功能评价指标
        public static string ecology_target = "土地生态功能评价指标";
        public static string ecology_t1 = "生态系统支持功能";
        public static string ecology_t11 = "土壤保持";
        public static string ecology_t12= "生物多样性";
        public static string ecology_t2 = "生态系统调节服务";
        public static string ecology_t21 = "气体调节";
        public static string ecology_t22 = "水文调节";
        public static string ecology_t3 = "供给服务";
        public static string ecology_t31 = "食物生产";
        public static string ecology_t32 = "原材料生产";
        public static string ecology_t4 = "文化娱乐服务";
        public static string ecology_t41 = "文化旅游";
        public static string ecology_t5 = "结构安全";
        public static string ecology_t51 = "垂直结构";
        public static string ecology_t52 = "水平结构";
        public static string ecology_t6 = "过程安全";
        public static string ecology_t61 = "物质循环";
        public static string ecology_t62= "能量循环";
        //public static string ecology_t5 = "结构安全";
        //public static string ecology_t5 = "结构安全";


        //综合评价指标
        public static string final_target = "综合评价指标";
        //public static string final_t1 = "";
        //public static string final_t1 = ""; 
        //public static string final_t1 = "";
        //public static string final_t1 = "";
        //public static string final_t1 = "";
        //public static string final_t1 = "";
        //public static string final_t1 = "";
        //public static string final_t1 = "";
        //public static string final_t1 = "";

        //专家名单
        public static string Pro_n1 = "Pro.Li";
        public static string Pro_n2  = "Pro.Deng";
        public static string Pro_n3 = "Pro.Yue";
        public static string Pro_n4 = "Pro.Gong";
        public static string Pro_n5 = "Pro.Chen";
        public static string Pro_n6 = "Pro.Guo";
        public static string Pro_n7 = "Pro.Zhou";
        public static string Pro_n8 = "Pro.Jiang";
        public static string Pro_n9 = "Pro.Zhang";
        public static string Pro_n10 = "Pro.Wang";

        public static void add_t(string t_name, System.Windows.Forms.ListView listview)
        {
            if (t_name == land_target)
            {
                addTarget(listview, land_t11);
                addTarget(listview, land_t12);
                addTarget(listview, land_t13);
                addTarget(listview, land_t14);
                addTarget(listview, land_t15);
                addTarget(listview, land_t16);
                addTarget(listview, land_t21);
                addTarget(listview, land_t22);
                addTarget(listview, land_t23);
                addTarget(listview, land_t24);
                addTarget(listview, land_t31);
                addTarget(listview, land_t32);
                addTarget(listview, land_t33);
                addTarget(listview, land_t34);
            }
            if (t_name == risk_target)
            {
                addTarget(listview, risk_t11);
                addTarget(listview, risk_t12);
                addTarget(listview, risk_t13);
                addTarget(listview, risk_t14);
                addTarget(listview, risk_t21);
                addTarget(listview, risk_t22);
                addTarget(listview, risk_t23);
                addTarget(listview, risk_t24);
                addTarget(listview, risk_t25);
            }
            if (t_name == ecology_target)
            {
                addTarget(listview, ecology_t11);
                addTarget(listview, ecology_t12);
                addTarget(listview, ecology_t21);
                addTarget(listview, ecology_t22);
                addTarget(listview, ecology_t31);
                addTarget(listview, ecology_t32);
                addTarget(listview, ecology_t41);
                addTarget(listview, ecology_t51);
                addTarget(listview, ecology_t52);
                addTarget(listview, ecology_t61);
                addTarget(listview, ecology_t62);
            }
            
        }
        public  static  void addTarget(System.Windows.Forms.ListView listview, string targetName)
        {
            ListViewItem lv = new ListViewItem();
            lv.ImageIndex = 0;
            lv.Text = targetName;
            lv.Checked = true;
            
            listview.Items.Add(lv);
        }
    }
}
