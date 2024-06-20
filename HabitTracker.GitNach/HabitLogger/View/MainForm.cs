using HabitLogger.Controllers;
using HabitLogger.View;

namespace HabitLogger
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            FormsController.SetMainForm(this);
            FormsController.ChangeForm(new Menu());
            Refresh();

        }
    }
}
