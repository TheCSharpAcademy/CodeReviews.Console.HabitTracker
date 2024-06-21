using HabitLogger.Controllers;

namespace HabitLogger.View
{
    public partial class DeleteForm : ViewForm
    {
        public DeleteForm()
        {
            InitializeComponent();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            FormsController.ChangeForm(new Menu());
        }
    }
}
