using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HabitLogger.Controllers;
using HabitLogger.Model;
using MaterialSkin.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HabitLogger.View
{
    public partial class InsertForm : Form
    {
        private int userMetric;

        private HabitType userHabit;

        public InsertForm()
        {
            InitializeComponent();
            InitializeComboBox();
            
        }



        private void InitializeComboBox()
        {
            List<HabitType> habitTypes = DataBaseController.GetHabitTypes();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;


            comboBox1.DataSource = habitTypes;

            comboBox1.DisplayMember = "Name";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {   
            userHabit = comboBox1.SelectedItem as HabitType;

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            userMetric = int.Parse(textBox1.Text);

            if (userHabit == null)
            {
                MessageBox.Show("Please select a habit type.");
                return;
            }

            if (userMetric <= 0 )
            {
                MessageBox.Show("Please set a MetricValue greater than 0.");
                return;
            }

                List<HabitType> habitTypes = DataBaseController.GetHabitTypes();

            foreach (HabitType habitType in habitTypes)
            {
                if (habitType.Name == userHabit.Name)
                {
                    var habit = new Habit
                    {
                        Date = DateTime.Now,
                        MetricValue = userMetric,
                        Type = habitType
                    };
                    DataBaseController.AddHabitType(habitType);
                    DataBaseController.AddHabit(habit);
                    break;
                }
            }
            FormsController.ChangeForm(new Menu());   
            

            
        }
    }
}
