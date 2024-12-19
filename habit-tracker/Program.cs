using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO.Compression;
using Microsoft.Data.Sqlite;

namespace habit_tracker {

    class Program {

        static string connectionString = @"Data source=habit-tracker.db";
        static void Main(string[] args) {

            using (var connection = new SqliteConnection(connectionString)) {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER)";

                // SQLite has no date type. Different from SQL server which does.

                tableCmd.ExecuteNonQuery(); // non query means that we don't want DB to return any values
                // in this case, we're only creating a table which is why we're doing this

                connection.Close();
            }

            getUserInput();
        }

        static void getUserInput() {
            Console.Clear();
            bool closeApp = false; 

             

            while (!closeApp) {
                Console.WriteLine("\n\nMain Menu");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");

                string commandInput = Console.ReadLine(); 

                switch (commandInput) {
                    case "0":
                        Console.WriteLine("Closing.");
                        closeApp = true; 
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        Insert();
                        break; 
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break; 
                    default:
                        Console.WriteLine("Invalid operation.");
                        break;
                }
            }
        }

        private static void GetAllRecords() {
            using (var connection = new SqliteConnection(connectionString)) {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"SELECT * FROM drinking_water"; 

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        tableData.Add(new DrinkingWater 
                            {
                                Id = reader.GetInt32(0), 
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)

                                // reader.GetType(index of column)
                            }
                        );
                    }

                    foreach (var dw in tableData) {
                        Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
                    }
                } else {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();
            }

            return; 
        }

        private static void Update() {
            Console.Clear();
            GetAllRecords();
            
            using (var connection = new SqliteConnection(connectionString)) {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                int recordId = GetNumberInput("Type the Id of the record you want to update.");

                checkCmd.CommandText = $@"SELECT EXISTS(SELECT 1 from drinking_water where Id = {recordId})'"; 
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0) {
                    Console.WriteLine($"\nRecord with id {recordId} does not exist");
                    connection.Close();
                    getUserInput();
                }

                string date = GetDateInput();   
                int quantity = GetNumberInput("Insert number of glasses of water.");

                var tableCmd = connection.CreateCommand(); 
                tableCmd.CommandText = $"UPDATE drinking_water SET date = {date}, quantity = {quantity} WHERE Id = '{recordId}'";

                tableCmd.ExecuteNonQuery();

                Console.WriteLine($"Row with id {recordId} was updated");

                connection.Close();
            }

            return; 
        }

        private static void Delete() {
            Console.Clear();
            GetAllRecords();

            Console.Write("Choose a record to delete.");
            using (var connection = new SqliteConnection(connectionString)) {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    int recordId = GetNumberInput("Type the Id of the record you want to delete.");
                    tableCmd.CommandText = $@"DELETE from drinking_water where id = '{recordId}'"; 

                    int rowCount = tableCmd.ExecuteNonQuery(); // returns the number of rows affected by the command 
                    if (rowCount == 0) { // means that 0 rows were deleted or id not found
                        Console.WriteLine($"\nRecord with id ${recordId} does not exist.");
                        Delete();
                    }

                    Console.WriteLine($"Row with id {recordId} was deleted");

                    connection.Close();
                }

            return; 
        }



        private static void Insert() {
            string date = GetDateInput(); 

            int quantity = GetNumberInput("\nInsert number of glasses of water. Type 0 to return to the main menu.");

            using (var connection = new SqliteConnection(connectionString)) {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})"; 
                // note the single quotes around the date, and no single quotes around quantity

                tableCmd.ExecuteNonQuery(); // again, not returning anything from the DB

                connection.Close();
            }
        }

        internal static string GetDateInput() {
            Console.WriteLine("\n\nProvide date in (dd-mm-yy format). Type 0 to return to the main menu.");

            string dateInput = Console.ReadLine();
            if (dateInput == "0") {
                getUserInput();
            }

            while (!DateTime.TryParseExact(dateInput, "mm-DD-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _)) {
                Console.WriteLine("Try again. Date could not be parsed.");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message) {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();
            if (numberInput == "0") {
                getUserInput();
            }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}

public class DrinkingWater {

    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}