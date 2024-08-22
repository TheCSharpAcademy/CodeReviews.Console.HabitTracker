using Microsoft.Data.Sqlite;
using System.Collections.Concurrent;
using System.Reflection;
using static System.Console;

namespace nikosnick13.HabitLogger
{
    internal class Program
    {
       static string  connectionString = @"Data Source=habit-Traker.db";
        static void Main(string[] args)
        {
 

            using (var conn = new SqliteConnection(connectionString)) 
            {
                conn.Open();

                var tableCmd = conn.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS  dring_water(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT, 
                        Quantity  INTEGER
                    )";

                tableCmd.ExecuteNonQuery();

                conn.Close();


                
            }

            userInput();

            ReadKey();

        }

        //USER INPUT MENY
        public static void userInput() 
        {
            Clear();
            ForegroundColor = ConsoleColor.Green;

            bool isAppRunning = true;

            while (isAppRunning) 
            {
                WriteLine("============================================");
                WriteLine("\n\nMAIN Menu");
                WriteLine("\nWhat would you like to do?");
                WriteLine("\n\tType 0 to Close Application.");
                WriteLine("\tType 1 to Inser a Record ");
                WriteLine("\tType 2 to View All Records");
                WriteLine("\tType 3 to Delete a Record");
                WriteLine("\tType 4 to Update a Record");
                WriteLine("============================================\n");

                string userInput = ReadLine();

                switch (userInput) 
                {
                    case "0":
                        isAppRunning = false;
                        break;

                    case "1":
                        Insert();
                        break;
                    case "2":
                        //TO DO: Call the ViewAll Method
                        break;

                    case "3":
                        //TO DO: Call the Delete Method
                        break;
                    case "4":
                        //TO DO: Call the Update Method
                        break;

                }

              
            }

        }

        #region Insert

        private static void Insert() 
        {
            string date = GetDateInput();

            int quantity = GetNumInput("\n\nPlaese Insert a number of glasses or another measures of your choice (no decimals is allowed \n\n) ");

            using (var conn = new SqliteConnection(connectionString)) 
            {
                conn.Open();

                var insertUserCmd = conn.CreateCommand();

                insertUserCmd.CommandText = $"INSERT INTO dring_water(date,quantity ) VALUES ('{date}',{quantity})";

                insertUserCmd.ExecuteNonQuery();

                conn.Close();
            

            }

        }

        //Get the input for Date as string
        private static string GetDateInput() 
        {
            WriteLine("Please insert tha date: DD-MM-YYYY. Type 0 to return to main menu.");

            string dateInput = ReadLine();

            if (dateInput == "0") userInput(); 


            return dateInput;
        }

        //Get the input for quanti as string
        private static int GetQuantityInput(string msq) 
        {
            WriteLine(msq);

            string quantityInput = ReadLine();

            if (quantityInput == "0") userInput();

            int finalQuantity = Convert.ToInt32(quantityInput);

            return finalQuantity;
        }

        #endregion

        #region ViewAll
        private static void VeiwAll() 
        {
            using(var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                var viewAllCmd = conn.CreateCommand();

                viewAllCmd.CommandText = 
                    $"SELECT * FROM dring_water";

                List<DringWater> tableData = new();


                viewAllCmd.ExecuteNonQuery();

                conn.Close();
                
                
            }
        }

        #endregion
    }

    public class DringWater 
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }

}
