using System;
using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace HabitTracker_obitom67
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main()
        {
            

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCMD = connection.CreateCommand();
                tableCMD.CommandText =
                    @"CREATE TABLE IF NOT EXISTS burpees (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

                tableCMD.ExecuteNonQuery();
                connection.Close();
            }
            GetUserInput();
        }

        static void GetUserInput()
        {
            //Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                int userCase;
                Console.WriteLine("\n\nHabit Tracker");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record\n");
                int.TryParse(Console.ReadLine(), out userCase);

                switch (userCase)
                {
                    case 0:
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        break;
                    case 1:
                        ViewRecords();
                        break;
                    case 2:
                        Insert();
                        break;
                    case 3:
                        Delete();
                        break;
                    case 4:
                        Update();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\n\nInvalid command please type one of the commands on the menu.");
                        GetUserInput();
                        break;

                }
            }
            
        }

        private static void Insert()
        {
            string date = GetDateInput();
            
            int quantity = GetNumberInput("\n\nPlease insert number of burpees(no decimals allowed).");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCMD = connection.CreateCommand();
                tableCMD.CommandText =
                    $"INSERT INTO burpees(date,quantity) VALUES('{date}', {quantity})";

                tableCMD.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void Update()
        {
            Console.Clear();
            ViewRecords();

            var tableId = GetNumberInput("\n\nPlease input the Id of the record that you would like to update or type 0 to go to the Main Menu\n\n");
            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCMD = connection.CreateCommand();
                checkCMD.CommandText = $"SELECT EXISTS(SELECT 1 FROM burpees WHERE Id = {tableId})";
                int checkQuery = Convert.ToInt32(checkCMD.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine("\n\nThere is no record with that Id");
                    connection.Close();
                    Update();
                }
                string date = GetDateInput();
                int quantity = GetNumberInput("\n\nPlease insert number of burpees (no decimals allowed)");



                var tableCMD = connection.CreateCommand();
                tableCMD.CommandText = $"UPDATE burpees SET date = '{date}', quantity = {quantity} WHERE Id = {tableId}";
                tableCMD.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void Delete()
        {
            Console.Clear();
            ViewRecords();

            var tableId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go to the Main Menu");

             

            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCMD = connection.CreateCommand();
                tableCMD.CommandText = $"DELETE from burpees WHERE Id = '{tableId}'";

                int rowCount = tableCMD.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {tableId} doesn't exist. \n\n");
                }
                GetUserInput();
            }


        }
        private static void ViewRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCMD = connection.CreateCommand();
                tableCMD.CommandText =
                    $"SELECT * FROM burpees";

                List<Burpees> tabledata = new();
                SqliteDataReader reader = tableCMD.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string readerString = reader.GetString(1);
                        tabledata.Add(new Burpees
                        {
                            
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(readerString,"dd-MM-yy",new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                        }); ;
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                
                connection.Close();
                Console.WriteLine("-------------------------------\n");
                foreach (var dw in tabledata)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("-------------------------------\n");
            }

        }

        private static string GetDateInput()
        {
            DateTime date = new DateTime();
            string dateString = "";
            Console.WriteLine("\n\n Please insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
            string inputValue = Console.ReadLine();

            if (inputValue == "0")
            {
                GetUserInput();
                
            }
            
            

            while (!DateTime.TryParseExact(inputValue, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out date))
            {
                Console.WriteLine("\n\nInvalid date. (Format:dd-mm-yy). Type 0 to return to main menu or try again.\n\n");
                inputValue = Console.ReadLine();
            }

            return inputValue;

        }

        private static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string inputString = Console.ReadLine();

            if (inputString == "0")
            {
                GetUserInput();
            }

            while (!Int32.TryParse(inputString, out int number)|| Convert.ToInt32(inputString)<0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                inputString = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(inputString);

            return finalInput;
        }

     

    }

    public class Burpees
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Quantity { get; set; }
    }
}
