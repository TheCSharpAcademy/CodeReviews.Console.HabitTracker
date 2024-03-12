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
            Console.Clear();
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
                        GetUserInput();
                        break;

                }
            }
            
        }

        private static void Insert()
        {
            string date = GetDateInput();
            
            int quantity = GetNumberInput();
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

        }

        private static void Delete()
        {

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
                        tabledata.Add(new Burpees()
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1),"dd-mm-yy",new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                tableCMD.ExecuteNonQuery();
                connection.Close();
            }

        }

        private static string GetDateInput()
        {
            DateTime date = new DateTime();
            Console.WriteLine("\n\n Please insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
            string inputValue = Console.ReadLine();

            if (inputValue == "0")
            {
                GetUserInput();
                
            }
            else if(DateTime.TryParseExact(inputValue,"dd-MM-yy",new CultureInfo("en-US"),DateTimeStyles.None, out date))
            {
                
            }
            else
            {
                Console.WriteLine("Please input a valid date in the specified format (dd-mm-yy).");
                GetDateInput();
            }
            return date.ToString();

        }

        private static int GetNumberInput()
        {
            Console.WriteLine("\n\nPlease insert number of burpees(no decimals allowed).");
            string inputString = Console.ReadLine();
            if(int.TryParse(inputString, out int number))
            {
                return number;
            }
            else
            {
                Console.WriteLine("Please input a valid number (no decimals allowed).");
            }
            return 0;
        }
    }

    public class Burpees
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Quantity { get; set; }
    }
}
