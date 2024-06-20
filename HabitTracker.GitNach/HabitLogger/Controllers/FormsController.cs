using HabitLogger.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger.Controllers
{
    public static class FormsController
    {
        private static MainForm? MainForm { get; set; }
        private static ViewForm? ActiveForm { get; set; }    


        public static void SetMainForm(MainForm mainform)
        {
            if (MainForm == null)
            {
                MainForm = mainform;
            }
            
        }

        public static void ChangeForm(ViewForm form) 
        {
            if (MainForm == null)
            {
                return;
            }
            MainForm.pnlMain.Controls.Clear();
            form.Dock = DockStyle.Fill;
            MainForm.pnlMain.Controls.Add(form);
            ActiveForm = form;
            form.Show();

        }


    }
}
