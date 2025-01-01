using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using var connection = new SqliteConnection("Data Source=HabitTracker.db");
                connection.Open();

                using var command = new SqliteCommand(@"CREATE TABLE IF NOT EXISTS 
                                                        book_reading(id INTEGER PRIMARY KEY,
                                                        date TEXT,
                                                        pages_read INTEGER)", connection);
                command.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Console.WriteLine(e.Message);
            }

            string? option = null;
            do
            {
                PrintMenu();

                option = GetInputLowerCase("Chose an option to continue:");
                switch (option)
                {
                    case "v":
                        ViewLoggedHabit();
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadLine();
                        break;
                    case "i":
                        InsertRecord();
                        break;
                    case "u":
                        UpdateRecord();
                        break;
                    case "d":
                        DeleteRecord();
                        break;
                    case "e":
                        System.Environment.Exit(0);
                        break;
                    default:
                        break;
                }

            }
            while (option != null && option != "e");

            void PrintMenu()
            {
                Console.Clear();
                Console.WriteLine("HABIT TRACKER");
                Console.WriteLine("---------------------\n");

                Console.WriteLine("MAIN MENU");
                Console.WriteLine("'V' - view logged habit");
                Console.WriteLine("'I' - insert record");
                Console.WriteLine("'U' - update record");
                Console.WriteLine("'D' - delete record");
                Console.WriteLine("'E' - Exit the app");
            }

            void InsertRecord()
            {
                string date = GetDate();
                uint number = GetQuantity();

                using var connection = new SqliteConnection("Data Source=HabitTracker.db");
                connection.Open();

                using var command = new SqliteCommand(@"INSERT INTO book_reading (date, pages_read) VALUES (@date, @number)", connection);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@number", number);
                command.ExecuteNonQuery();
            }

            void UpdateRecord()
            {
                string? option;
                int recordID;

                ViewLoggedHabit();

                option = GetInputLowerCase("Select which record you want to update by typing the record's ID.\n" +
                                           "If you don't want to update anything, type 'x':");
                if (option == "x")
                {
                    PrintMenu();
                    return;
                }
                else if (!int.TryParse(option, out recordID))
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
                    return;
                }

                string[] validOptions = { "d", "q", "b" };
                do
                {
                    option = GetInputLowerCase("Select what you want to update:\n" +
                                               "'d' for date, 'q' for quantity, or 'b' for both:");
                }
                while (string.IsNullOrEmpty(option) || !validOptions.Contains(option));

                string? newDate = null;
                uint? newQuantity = null;

                using var connection = new SqliteConnection("Data Source=HabitTracker.db");
                connection.Open();
                using var command = new SqliteCommand();

                command.Connection = connection;

                switch (option)
                {
                    case "d":
                        Console.WriteLine("Insert new date (format: dd.mm.yyyy):");
                        newDate = GetDate();

                        command.CommandText = @"UPDATE book_reading 
                                    SET date = @newDate 
                                    WHERE id = @recordID";
                        command.Parameters.AddWithValue("@newDate", newDate);
                        command.Parameters.AddWithValue("@recordID", recordID);
                        break;

                    case "q":
                        Console.WriteLine("Insert new quantity:");
                        newQuantity = GetQuantity();

                        command.CommandText = @"UPDATE book_reading 
                                    SET pages_read = @newQuantity 
                                    WHERE id = @recordID";
                        command.Parameters.AddWithValue("@newQuantity", newQuantity);
                        command.Parameters.AddWithValue("@recordID", recordID);
                        break;

                    case "b":
                        Console.WriteLine("Insert new date (format: dd.mm.yyyy):");
                        newDate = GetDate();
                        Console.WriteLine("Insert new quantity:");
                        newQuantity = GetQuantity();

                        command.CommandText = @"UPDATE book_reading 
                                    SET date = @newDate, pages_read = @newQuantity 
                                    WHERE id = @recordID";
                        command.Parameters.AddWithValue("@newDate", newDate);
                        command.Parameters.AddWithValue("@newQuantity", newQuantity);
                        command.Parameters.AddWithValue("@recordID", recordID);
                        break;
                }

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Record updated successfully.");
                }
                else
                {
                    Console.WriteLine("No record found with the given ID.");
                }
                Console.ReadLine();
            }

            void DeleteRecord()
            {
                string? option;
                ViewLoggedHabit();

                option = GetInputLowerCase("Select which record you want to delete by typing record's ID\n" + 
                                           "If you dont want to delete anything type 'x'");
                if (option == "x")
                {
                    PrintMenu();
                }
                else if (int.TryParse(option, out int selection))
                {
                    using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                    {
                        connection.Open();
                        
                        var command = new SqliteCommand(@"DELETE FROM book_reading WHERE ID = @selection", connection);
                        command.Parameters.AddWithValue("@selection", selection);
                        command.ExecuteNonQuery();
                    }
                }
            }

            void ViewLoggedHabit()
            {
                using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                {
                    connection.Open();

                    var tableCommand = connection.CreateCommand();
                    tableCommand.CommandText =
                        $"SELECT * FROM book_reading";

                    List<BookReading> tableData = new();

                    using (SqliteDataReader reader = tableCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read()) // Advance the reader to the next row
                            {
                                tableData.Add(new BookReading
                                {
                                    Id = reader.GetInt32(0),
                                    Date = DateTime.ParseExact(reader.GetString(1), "dd.MM.yyyy", new CultureInfo("pl-PL")),
                                    Quantity = reader.GetInt32(2)
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found!");
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("ID\tDATE\t\tQuantity");
                    foreach (var item in tableData)
                    {
                        Console.WriteLine($"{item.Id}\t{item.Date.ToShortDateString()}\t{item.Quantity}");
                    }
                }
            }

            string GetDate()
            {
                DateTime thisDate = new DateTime();
                string date;
                string? input;
                input = GetInputLowerCase("Please enter a date in the format dd.MM.yyyy, or type 'T' for today's date:");
                if (input.Equals("t"))
                {
                    thisDate = DateTime.Today;
                }
                else
                {
                    while (!DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.CreateSpecificCulture("pl-PL"), DateTimeStyles.None, out thisDate) || thisDate > DateTime.Today)
                    {
                        input = GetInputLowerCase("Invalid date. Please enter a date in the format dd.MM.yyyy, or type 'T' for today's date:");
                        if (input.Equals("t"))
                        {
                            thisDate = DateTime.Today;
                            break;
                        }
                    }
                }

                date = thisDate.ToString("dd.MM.yyyy");
                return date;
            }

            uint GetQuantity()
            {
                uint number;
                string? temp;
                do
                {
                    temp = GetInputLowerCase("How many times habit has occured?"); ;
                } while (!uint.TryParse(temp, out number));
                Console.WriteLine(number);
                return number;
            }

            string? GetInputLowerCase(string message)
            {
                string? input;
                
                do
                {
                    Console.WriteLine(message);
                    input = Console.ReadLine();
                } while (input == null);
                return input.ToLower().Trim();
            }
        }
        public class BookReading()
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int Quantity { get; set; }
        }
    }
}     