using System.Globalization;
using System.Text.RegularExpressions;

namespace UserInputLibrary
{
    public class UserInput
    {
        public  string GetDateInput()
        {
            Console.WriteLine("\nPlease Insert date with Format 'dd-MM-yy'");
            string DateInput = Console.ReadLine();

            while (!DateTime.TryParseExact(DateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid Date format, format should be 'dd-MM-yy'");
                DateInput = Console.ReadLine();
            }
            return DateInput;
        }

        public int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            while (!Int32.TryParse(input, out _) || Convert.ToInt32(input) < 0)
            {
                Console.WriteLine("Invalid input, input cannot be decimal or less than zero.Try again");
                input = Console.ReadLine();
            }
            int numberInput = Convert.ToInt32(input);
            return numberInput;
        }

        public string GetHabitInput(string message)
        {
            Console.WriteLine(message);
            string habit = Console.ReadLine().Trim().ToLower();
            habit = SanitizeName(habit);
            return habit;
        }


        public string GetUnitInput()
        {
            Console.WriteLine("Enter unit of measurement");
            string unitofmeasurement = Console.ReadLine().Trim();
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
