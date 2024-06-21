using HabitLogger.Controllers;
using HabitLogger.Model;
using MaterialSkin;
using System.Data;

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
            dataGridView1.DataSource = habits.Select(h => new
            {
                h.Id,
                Habit = h.Type.Name,
                h.Date,
                h.MetricValue,
                h.Type.Metric
            }).ToList();

        }



        private void AddHardcodedHabit()
        {
            var habitType = new HabitType { Id = 1, Name = "Running", Metric = "Km" };  //Use this for adding an Habit with a new HabitType (if the HabitType already exists, it adds the Habit with the existing HabitType)

            List<HabitType> habitTypes = DataBaseController.GetHabitTypes(); //For getting created habits and choosing by index



            var habit = new Habit
            {
                Date = DateTime.Now,
                MetricValue = 42,
                Type = habitType,
            };
            DataBaseController.AddHabitType(habitType);
            DataBaseController.AddHabit(habit);

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
    }
}
