namespace HabitLogger.View
{
    public partial class UpdateForm : Form
    {
        public int NewMetricValue { get; set; }
        public UpdateForm()
        {
            InitializeComponent();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {

            if (int.TryParse(textBox1.Text, out int value))
            {
                NewMetricValue = value;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }

        }
    }
}
