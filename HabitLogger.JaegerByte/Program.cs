using Microsoft.Data.Sqlite;
using System.Globalization;
namespace HabitLogger.JaegerByte
{
    internal class Program
    {
        static SqliteConnection connection = new SqliteConnection(@"Data source=database.db");

        static void Main()
        {
            CreateTable();

            while (true)
            {
                PrintMenu();
                GetUserInput();
            }
        }

        static void CreateTable()
        {
            connection.Open();

            SqliteCommand tableCommand = connection.CreateCommand();
            tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS pushups
                        (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";
            tableCommand.ExecuteNonQuery();
            connection.Close();
        }

        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("press number to select option");
            Console.WriteLine("1 - insert log");
            Console.WriteLine("2 - delete log");
            Console.WriteLine("3 - update log");
            Console.WriteLine("4 - view all logs");
            Console.WriteLine("0 - close application");
        }

        static void GetUserInput()
        {
            ConsoleKey userInput;
            while (true)
            {
                userInput = Console.ReadKey(true).Key;
                switch (userInput)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        InsertLog();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        DeleteLog();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        UpdateLog();
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        GetLog();
                        break;
                    case ConsoleKey.D0:
                        System.Environment.Exit(0);
                        break;
                }
                break;
            }
        }

        static void InsertLog()
        {
            Console.WriteLine("New log:");
            Console.WriteLine("Please insert the date (dd-mm-yyyy) and confirm with ENTER");
            string inputDate = Console.ReadLine();
            Console.WriteLine("Please insert the quantity als whole number and confirm with ENTER");
            int inputQuantity;
            Int32.TryParse(Console.ReadLine(), out inputQuantity);
            DateTime result; // not used
            if (DateTime.TryParseExact(inputDate, "dd-MM-yyyy", new CultureInfo("de-DE"), DateTimeStyles.None, out result) && inputQuantity>0)
            {
                CommandInsertLog(inputDate, inputQuantity);
                Console.WriteLine("log inserted successfully!");
                Console.WriteLine("press ANY key to get back to the menu");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("invalid input!");
                Console.WriteLine("press ANY key to get back to the menu");
                Console.ReadKey(true);
            }
        }

        static void CommandInsertLog(string date, int quantity)
        {
            SqliteCommand insertCommand = connection.CreateCommand();
            insertCommand.CommandText = $"INSERT INTO pushups(Date, Quantity) VALUES('{date}', {quantity})";
            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();
        }

        static void DeleteLog()
        {
            Console.WriteLine("Delete log:");
            Console.WriteLine("Please insert Id and confirm with ENTER");
            int inputIndex;
            if (Int32.TryParse(Console.ReadLine(), out inputIndex))
            {
                CommandDeleteLog(inputIndex);
                Console.WriteLine("log deleted successfully!");
                Console.WriteLine("press ANY key to get back to the menu");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("invalid input!");
                Console.WriteLine("press ANY key to get back to the menu");
                Console.ReadKey(true);
            }
        }

        static void CommandDeleteLog(int index)
        {
            SqliteCommand deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = $"DELETE FROM pushups WHERE Id='{index}'";
            connection.Open();
            deleteCommand.ExecuteNonQuery();
            connection.Close();
        }

        static void UpdateLog()
        {
            Console.WriteLine("Update log:");
            Console.WriteLine("Please insert Id and confirm with ENTER");
            int inputIndex;
            Int32.TryParse(Console.ReadLine(), out inputIndex);

            Console.WriteLine("Please insert the date (dd-mm-yyyy) and confirm with ENTER");
            string inputDate = Console.ReadLine();
            DateTime result; // not used
            Console.WriteLine("Please insert the quantity als whole number and confirm with ENTER");
            int inputQuantity;
            Int32.TryParse(Console.ReadLine(), out inputQuantity);
            if (DateTime.TryParseExact(inputDate, "dd-MM-yyyy", new CultureInfo("de-DE"), DateTimeStyles.None, out result) && inputQuantity > 0)
            {
                CommandUpdateLog(inputIndex, inputDate, inputQuantity);
                Console.WriteLine("log updated successfully!");
                Console.WriteLine("press ANY key to get back to the menu");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("invalid input!");
                Console.WriteLine("press ANY key to get back to the menu");
                Console.ReadKey(true);
            }
        }

        static void CommandUpdateLog(int index, string date, int quantity)
        {
            SqliteCommand updateCommand = connection.CreateCommand();
            updateCommand.CommandText = $"UPDATE pushups SET Date = '{date}', Quantity = {quantity} WHERE Id = {index}";
            connection.Open();
            updateCommand.ExecuteNonQuery();
            connection.Close();
        }

        static void GetLog()
        {
            string query = "SELECT * FROM pushups";
            SqliteCommand selectCommand = connection.CreateCommand();
            selectCommand.CommandText = query;

            List<DatabaseEntry> entries = new List<DatabaseEntry>();

            connection.Open();
            SqliteDataReader reader = selectCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    entries.Add(
                    new DatabaseEntry
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("de-DE")),
                        Quantity = reader.GetInt32(2)
                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found");
                Console.WriteLine("press ANY key to get back to the menu");
                Console.ReadKey(true);
                return;
            }

            connection.Close();

            foreach (DatabaseEntry item in entries)
            {
                Console.Write($"{item.Id}. ");
                Console.Write($"{item.Date}");
                Console.WriteLine($"{item.Quantity}");
            }

            Console.WriteLine("press ANY key to get back to the menu");
            Console.ReadKey(true);
        }
    }
}
