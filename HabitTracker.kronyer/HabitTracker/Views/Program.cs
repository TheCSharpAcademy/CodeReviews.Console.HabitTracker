using HabitTracker.Controllers;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Views
{

    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";

        static void Main(string[] args)
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Quantity INTEGER)";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            WaterController.GetUserInput();
        }

        public static DateTime GetDateInput()
        {
            DateTime dateInput;
            Console.WriteLine("Insert the date: (dd--mm---yy) or type 0 to return");

            while (true)
            {
                string userInput = Console.ReadLine();

                if (userInput == "0") {
                    Console.Clear();
                    WaterController.GetUserInput();
                }
                if (DateTime.TryParse(userInput, out dateInput))
                {
                    break;
                }
                Console.WriteLine("Insert a valid DateTime:");
                
            }
            return dateInput;
        }


        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            if (numberInput == "0")
            {
                WaterController.GetUserInput();
            }
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }




    }
}
