using HabitLogger.Controllers;
using HabitLogger.Model;
using MaterialSkin;

namespace HabitLogger.View
{
    public partial class Menu : ViewForm
    {

        private Habit? SelectedHabit => dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0].DataBoundItem as Habit : null;

        public Menu() : base()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Green900, Primary.Green900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);

            LoadHabits();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormsController.ChangeForm(new HabitMenu());
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            InsertForm insertForm = new InsertForm();
            insertForm.ShowDialog(this);
        }
        private void LoadHabits()
        {
            List<Habit> habits = DataBaseController.GetHabits();
            dataGridView1.DataSource = habits;

        }



        private void materialButton4_Click(object sender, EventArgs e)
        {
            if (SelectedHabit == null)
            {
                return;
            }
            DataBaseController.DeleteHabit(SelectedHabit.Id);
            LoadHabits();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            if (SelectedHabit == null)
            {
                return;
            }
            UpdateForm updateForm = new UpdateForm();
            updateForm.ShowDialog(this);
            DataBaseController.UpdateItem(SelectedHabit.Id, updateForm.NewMetricValue);
            LoadHabits();
        }
    }
}
