using HabitTrackerLibrary.sadklouds;

namespace ConsoleUI.Helpers
{
    internal static class UserInputsHelper
    {
        public static string GetHabitDateInput()
        {
            bool validFormat = false;
            string userDate = "";
            do
            {
                Console.Write("Enter the day for habit in the format {dd/MM/yyyy}: ");
                userDate = Console.ReadLine();
                validFormat = InputValidator.DateValidator(userDate);

                if (validFormat == false) Console.WriteLine("Invalid Date format");

            } while (validFormat == false);
            return userDate;
        }

        public static string GetStringInput(string message)
        {
            Console.Write(message);
            string output = Console.ReadLine();
            return output;
        }

        public static double GetDoubleInput(string message)
        {
            bool parse = false;
            double output = 0;
            do
            {
                Console.Write(message);
                string input = Console.ReadLine();
                parse = double.TryParse(input, out output);

            } while (parse == false);
            return output;
        }
    }
}
