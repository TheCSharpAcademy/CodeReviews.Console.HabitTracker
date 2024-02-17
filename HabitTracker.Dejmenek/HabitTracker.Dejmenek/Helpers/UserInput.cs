using HabitTracker.Dejmenek.Enums;

namespace HabitTracker.Dejmenek.Helpers
{
    internal class UserInput
    {
        public static MenuOptions GetMenuOption()
        {
            MenuOptions selectedOption;
            string? userInput = Console.ReadLine();

            while (!Validation.IsValidMenuOption(userInput))
            {
                Console.WriteLine("Invalid menu option.");
                userInput = Console.ReadLine();
            }
            selectedOption = (MenuOptions)Enum.Parse(typeof(MenuOptions), userInput);

            return selectedOption;
        }

        public static string GetHabitDate()
        {
            string? habitDate = Console.ReadLine();

            while (!Validation.IsValidDateFormat(habitDate))
            {
                Console.WriteLine("Invalid date format. Please enter a date in YYYY-MM-DD format.");
                habitDate = Console.ReadLine();
            }

            return habitDate;
        }

        public static int GetNumber()
        {
            string? userInput = Console.ReadLine();
            int userNumber;

            while (!Validation.IsValidNumber(userInput))
            {
                Console.WriteLine("Invalid number. Please enter a positive number.");
                userInput = Console.ReadLine();
            }
            userNumber = int.Parse(userInput);

            return userNumber;
        }

        public static string GetString()
        {
            string? userInput = Console.ReadLine();

            while (Validation.IsValidString(userInput))
            {
                Console.WriteLine("Invalid string format. Please try again.");
                userInput = Console.ReadLine();
            }

            return userInput;
        }
    }
}
