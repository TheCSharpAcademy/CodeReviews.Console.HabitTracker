using Microsoft.Data.Sqlite;
using System.Diagnostics.Metrics;

namespace HabitTracker.alvaromosconi
{
    internal class Program
    {
        private string tableName = String.Empty;
        static void Main(string[] args)
        {
            InitializeApplication();
        }

        static void InitializeApplication()
        {  
            HabitRepository habitRepository = new HabitRepository();
            string userInput = String.Empty;
            do
            {
                userInput = GetUserInput();
                Console.Clear();
                habitRepository.ExecuteOption(userInput);
            } while (userInput != "0");
        }

       
        internal static string GetUserInput()
        {
            Console.Clear();
            string userInput;
            bool isAValidInput = false;

            do
            {
                Console.WriteLine("\n==============Welcome to Habits Tracker!==============\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("0. Close App.\n");
                Console.WriteLine("1. View All Records.");
                Console.WriteLine("2. Insert A New Habit or Record.");
                Console.WriteLine("3. Update An Existing Record.");
                Console.WriteLine("4. Delete An Existing Record.");
                userInput = Console.ReadLine();
                isAValidInput = IsAValidInput(userInput);

                if (!isAValidInput)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input! Please enter a valid choice. Press any key to retry.");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (!isAValidInput);

            return userInput!;
        }

        private static bool IsAValidInput(string? choice)
        {
            if (string.IsNullOrWhiteSpace(choice))
                return false;

            if (!int.TryParse(choice, out int parsedChoice))
                return false;

            return IsChoiceInRange(parsedChoice);
        }

        private static bool IsChoiceInRange(int choice)
        {
            return choice >= 0 && choice <= 5;
        }

    }
}