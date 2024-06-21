using HabitLogger.Controllers;
using HabitLogger.Model;

namespace HabitLogger.View
{
    public partial class HabitMenu : ViewForm
    {

        public HabitMenu() : base()
        {

            InitializeComponent();
            comboBox1.Items.Add("Km");
            comboBox1.Items.Add("Kcal");
            comboBox1.Items.Add("Hs");


        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormsController.ChangeForm(new Menu());
        }



        private void materialButton2_Click(object sender, EventArgs e)
        {
            string newHabitName = textBox1.Text;
            string newHabitMetric = comboBox1.Text;

            HabitType newHabitType = new HabitType { Name = newHabitName, Metric = newHabitMetric };

            DataBaseController.AddHabitType(newHabitType);
            FormsController.ChangeForm(new Menu());
        }
    }
}
