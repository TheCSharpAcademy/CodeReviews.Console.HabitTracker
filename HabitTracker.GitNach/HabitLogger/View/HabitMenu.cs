using HabitLogger.Controllers;
using HabitLogger.Model;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            string newHabitName = textBox1.Text;
            string newHabitMetric = comboBox1.Text;

            HabitType newHabitType = new HabitType { Name =  newHabitName, Metric = newHabitMetric };

            DataBaseController.AddHabitType(newHabitType);
            FormsController.ChangeForm(new Menu());
        }
    }
}
