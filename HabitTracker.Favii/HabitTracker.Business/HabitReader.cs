using System.Globalization;

namespace HabitTracker.Business
{
    public class HabitReader
    {
        //Methods used to read each property of the class

        public Habit IngressHabit()
        {
            Habit habit = new Habit();

            habit.Date = IngressDate();
            habit.Quantity = IngressQuantity();
            habit.Name = IngressName();
            habit.Unit = IngressUnit();

            return habit;
        }
        public DateTime IngressDate()
        {
            Console.WriteLine("Ingress the date in format dd/mm/yy");
            string dateInput = Console.ReadLine();
            try
            {
                var dateTime = DateTime.Parse(dateInput, new CultureInfo("es-ES"));
                return dateTime;
            }
            catch
            {
                Console.WriteLine("Please ingress a correct date format");
                return IngressDate();
            }
            
        }
        public string IngressName()
        {
            Console.WriteLine("Ingress the name of the habit:");
            string habitInput = Console.ReadLine();

            return habitInput;
        }
        public int IngressQuantity()
        {
            Console.WriteLine("Ingress the quantity:");
            string quantityInput = Console.ReadLine();
            try
            {
                int quantityInt = Convert.ToInt32(quantityInput);
                return quantityInt;

            }
            catch
            {
                Console.WriteLine("Please ingress a correct date format");
                return IngressQuantity();
            }
        }
        public string IngressUnit()
        {
            Console.WriteLine("Ingress the unit in wich the habit will be measured (kilometers, units, glasses, hours, etc)");
            string unitInput = Console.ReadLine();

            return unitInput;
        }


        //Methods used for the inital seeding
        public DateTime randomizeDate()
        {
            Random random = new Random();
            int day;
            int month = random.Next(1, 13);
            int year = random.Next(2020, 2026);

            switch (month)
            {
                case 2:
                    day = random.Next(1, 28);
                    break;
                case 4:
                case 5:
                case 9:
                case 11:
                    day = random.Next(1, 30);
                    break;
                default:
                    day = random.Next(1, 31);
                    break;

            }

            DateTime date = new DateTime(year, month, day);

            return date;
        }

        public void randomizeHabit(ref Habit habit)
        {
            Random random = new Random();

            int roll = random.Next(1, 6);

            switch (roll)
            {
                case 1:
                    habit.Name = "Drinking water";
                    habit.Quantity = random.Next(1, 4);
                    habit.Unit = habit.Quantity == 1 ? "Glass" : "Glasses";
                    break;
                case 2:
                    habit.Name = "Walking";
                    habit.Quantity = random.Next(1, 11);
                    habit.Unit = habit.Quantity == 1 ? "Kilometer" : "Kilometers";
                    break;
                case 3:
                    habit.Name = "Yoga";
                    habit.Quantity = random.Next(1, 3);
                    habit.Unit = habit.Quantity == 1 ? "Hour" : "Hours";
                    break;
                case 4:
                    habit.Name = "Eat a Fruit";
                    habit.Quantity = random.Next(1, 4);
                    habit.Unit = habit.Quantity == 1 ? "Unit" : "Units";
                    break;
                case 5:
                    habit.Name = "Reading";
                    habit.Quantity = random.Next(1, 5) * 10;
                    habit.Unit = "Pages";
                    break;

            }
        }
    }
}
