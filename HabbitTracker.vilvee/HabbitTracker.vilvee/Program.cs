using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabbitTracker.vilvee
{
    internal class Program
    {

        static string databaseConnection = @"Data Source=habitTracker.db";

        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS DrinkingWater(
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Quantity INTEGER
                            )";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            GetUserInput();
        }

        private static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                string menu = @"

MAIN MENU

[1] VIEW ALL RECORDS
[2] INSERT RECORD
[3] DELETE RECORD
[4] UPDATE RECORD
[0] CLOSE
-----------------------------------------";
                Console.WriteLine(menu);

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        InsertRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }

            }
        }

        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM DrinkingWater";
                List<DrinkingWater> records = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        records.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-mm-yyyy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("-----------------------------------------\n");

                foreach (var record in records)
                {
                    Console.WriteLine($"{record.Id} - {record.Date.ToString("dd-mm-yyy")} - Quantity: {record.Quantity}");
                }
                Console.WriteLine("-----------------------------------------\n");
            }

        }

        private static void InsertRecord()
        {
            string date = GetDateInput();

            int quantityOfWater = GetNumberInput("\n\nPlease insert number of glasses");

            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO DrinkingWater(date, quantity) VALUES('{date}', '{quantityOfWater}')";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert date (Format: dd-mm-yyyy.\nType 0 to return to Main Menu");
            string dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-mm-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid format. (dd-mm-yyyy)");
                dateInput = Console.ReadLine();
            }

            return dateInput;

        }
        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            int userInput = Convert.ToInt32(Console.ReadLine());
            if (userInput == 0) GetUserInput();
            return userInput;
        }


        private static void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();
            int recordId = GetNumberInput("\n\nWhich row do you want to delete? Enter the row number.\n\n");

            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"DELETE DrinkingWater WHERE Id = '{recordId}'";
                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    DeleteRecord();
                }

                Console.WriteLine($"\n\nRecord {recordId} was deleted.\n\n");
                connection.Close();
            }

            GetUserInput();

        }
        private static void UpdateRecord()
        {
            Console.Clear();
            GetAllRecords();
            int recordId = GetNumberInput("\n\nWhich row do you want to update? Enter the row number.\n\n");

            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();

                //check if record exists
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM DrinkingWater WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.\n\n");
                    connection.Close();
                    UpdateRecord();
                }

                //update record
                string date = GetDateInput();
                int quantity = GetNumberInput("\n\nPlease insert number of glasses");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"UPDATE DrinkingWater SET date = '{date}', quantity = {quantity} WHERE Id = {recordId} ";
                tableCmd.ExecuteNonQuery();
                Console.WriteLine($"\n\nRecord {recordId} was successfully updated.\n\n");
                connection.Close();
            }

            GetUserInput();
        }

    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }

}
