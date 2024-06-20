using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace HabitLogger.View
{
    public partial class ViewForm : MaterialForm
    {
        public ViewForm() 
        {
            InitializeComponent();
            this.TopLevel = false;
        }
    }
}
