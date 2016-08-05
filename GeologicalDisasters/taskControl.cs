using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ComprehensiveEvaluation
{
    class taskControl
    {
        public static new_task task;
        public static TargetSystem.Target target;
        public static TargetSystem.Weight weight;

        public string E_type="";
        public static  void setTask(string type)
        {
            task = new new_task(type);
            task.TopMost = true;
            task.StartPosition = FormStartPosition.CenterScreen;
            task.Show();
        }
        public static void setTarget(string targetName)
        {
            target = new TargetSystem.Target(targetName);
            target.TopMost = true;
            target.StartPosition = FormStartPosition.CenterScreen;
            target.Show();
        }
        public static void setWeight(string weight_name)
        {
            weight = new TargetSystem.Weight(weight_name);
            weight.TopMost = true;
            weight.StartPosition = FormStartPosition.CenterScreen;
            weight.Show();
        }
        public static void setWait()
        {
            Waitform wait = new Waitform() ;
            wait.TopMost = true;
            wait.StartPosition = FormStartPosition.CenterScreen;
            wait.Show();
        }
    }
}
