using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Paul_W_Saltzman
{
    internal static class Helpers
    {
        public static DateOnly GetDate()
        {
            Console.Clear();
            Console.WriteLine("Type T to log a habit for today. Or insert the date: (Format: MM-DD-YY).");
            bool dateValid = false;
            DateOnly dateInput = new DateOnly(1900, 01, 01);
            while (!dateValid)
            {
                string userInput = Console.ReadLine();
                userInput = userInput.ToLower().Trim();
                string dateFormat = "MM-dd-yy";

                switch (userInput)
                {
                    case "t":

                        dateInput = DateOnly.FromDateTime(DateTime.Now);
                        dateValid = true;

                        break;

                    default:

                        if (DateOnly.TryParseExact(userInput, dateFormat, null, System.Globalization.DateTimeStyles.None, out dateInput))
                        {
                            dateValid = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Date Format. Please try again.");
                        }
                        break;
                }
            }
            return dateInput;

        }
        internal static int NumberOfHabits()
        {
            List<Habits> habitList = Data.LoadHabits();
            int numberOfHabits = habitList.Count;
            return numberOfHabits;
        }

        internal static int NumberOfUnits()
        {
            List<UnitType> unitList = Data.LoadUnits();
            int numberOfUnits = unitList.Count;
            return numberOfUnits;
        }

        internal static string Sanitize(string toSanitize)
        {
            //the assignment is to work with sql direct with the database this is my very humble attempt to stop the most obvious sql injections attacks 
            // This will not capture everything.
            toSanitize = toSanitize.Trim();
            toSanitize = toSanitize.ToLower();
            int originalLength = toSanitize.Length;
            toSanitize = toSanitize.Replace("'", "''");
            toSanitize = toSanitize.Trim();
            string sanitized = toSanitize;
            int finalLength = sanitized.Length;
            if (originalLength != finalLength)
            { Console.WriteLine("Your input has been sanitized."); }
            else { }
            return sanitized;
        }

        internal static string EnforceFormatting(string preformatted)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            string formatted = textInfo.ToTitleCase(preformatted);
            return formatted;
        }

        internal static bool CheckForDuplicateHabits(string newHabit)
        {
            bool dupsFound = false;
            List<Habits> habits = Data.LoadHabits();
            foreach (var habit in habits)
            {
                if (habit.HabitName == newHabit)
                {
                    dupsFound = true;
                }
                else { }
            }
                return dupsFound;
        }

        internal static bool CheckForDuplicateUnits(string newHabit)
        {
            bool dupsFound = false;
            List<UnitType> units = Data.LoadUnits();
            foreach (var unit in units)
            {
                if (unit.UnitName == newHabit)
                {
                    dupsFound = true;
                }
                else { }
            }
            return dupsFound;
        }
        internal static string CheckSize(string toCheck)
        {
            int maxLength = 19;

            toCheck = new string(toCheck.Take(maxLength).ToArray());

            return toCheck;
        }

        public static int GetNumber(int unitID)
        {

            string unitName = Data.GetUnitName(unitID);
            int numberInput = 0;

            while (numberInput == 0)
            {
                Console.Clear();
                Console.WriteLine($@"Please enter a number of {unitName}(s).");
                string numberInputString = Console.ReadLine();
                if (int.TryParse(numberInputString, out int number))
                {
                    numberInput = number;
                }
                else
                {
                    Console.WriteLine("Invalid Input Press Enter to Continue");
                    Console.ReadLine();
                }
            }
            return numberInput;
        }
    }
}
