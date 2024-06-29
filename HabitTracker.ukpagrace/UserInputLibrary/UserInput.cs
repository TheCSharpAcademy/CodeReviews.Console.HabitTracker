using System.Globalization;
using System.Text.RegularExpressions;

namespace UserInputLibrary
{
    public class UserInput
    {
        public string GetMenuOption() {
            Console.WriteLine("Select an option");
            Console.WriteLine("1 - Create Habit");
            Console.WriteLine("2 - Insert");
            Console.WriteLine("3 - See all tracked habit");
            Console.WriteLine("4 - Update");
            Console.WriteLine("5 - Delete");
            Console.WriteLine("6 - List Habits");
            Console.WriteLine("7 - See Report");
            Console.WriteLine("8 - change habit table");
            Console.WriteLine("0 - or 0 to Exit");

            var userInput = Console.ReadLine();
            Console.Clear();

            return userInput;
        }
        public string GetDateInput()
        {
            Console.WriteLine("\nPlease Insert date with Format 'dd-MM-yy' or 9 to go back to menu");
            string dateInput = Console.ReadLine();
            if (dateInput == "9") return "menu";

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid Date format, format should be 'dd-MM-yy' or 9 to go back to menu ");
                dateInput = Console.ReadLine();
                if (dateInput == "9") return "menu";
            }
            return dateInput;
        }

        public int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            if (input == "9") return -1;
            while (!Int32.TryParse(input, out _) || Convert.ToInt32(input) < 0)
            {
                Console.WriteLine("Invalid input, input cannot be decimal or less than zero.Try again or 9 to see menu");
                input = Console.ReadLine();
                if(input == "9") return -1;
            }
            int numberInput = Convert.ToInt32(input);
            return numberInput;
        }

        public string GetHabitInput(string message)
        {
            Console.WriteLine(message);
            string habit = Console.ReadLine().Trim().ToLower();
            if (habit == "9") return "menu";
            habit = SanitizeName(habit);
            return habit;
        }


        public string GetUnitInput()
        {
            Console.WriteLine("Enter unit of measurement or 9 to go back to menu");
            string unitofmeasurement = Console.ReadLine().Trim();
            if (unitofmeasurement == "9") return "menu";
            unitofmeasurement = SanitizeName(unitofmeasurement);
            return unitofmeasurement;
        }

        public string SanitizeName(string name)
        {
            string result = Regex.Replace(name, @"[^a-z_\s]", string.Empty).Trim();
            result = Regex.Replace(result, @"\s", "_");
            return result;
        }
    }
}
