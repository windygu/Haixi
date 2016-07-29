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
        public string E_type="";
        public static  void setTask(string type)
        {
            task = new new_task(type);
            task.TopMost = true;
            task.StartPosition = FormStartPosition.CenterScreen;
            task.Show();
        }
    }
}
