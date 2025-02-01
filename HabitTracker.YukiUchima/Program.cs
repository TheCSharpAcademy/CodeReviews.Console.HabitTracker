using System.Globalization;
using HabitTracker.YukiUchima.Model;
using Microsoft.Data.Sqlite;

namespace HabitTracker;
class Program
{
    static private string _connectionString = @"Data Source=habit-tracker.db";
    static void Main(string[] args)
    {

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
               CREATE TABLE IF NOT EXISTS drinking_water(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
                )
                ";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();

        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application");
            Console.WriteLine("1 - View All Records");
            Console.WriteLine("2 - Insert Record");
            Console.WriteLine("3 - Delete Record");
            Console.WriteLine("4 - Update Record");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    closeApp = CloseApplication();
                    break;
                case "1":
                    ViewAll();
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
                    break;
            };
        }

    }
    static bool CloseApplication()
    {
        Console.Clear();
        Console.WriteLine("Thank you, we are ending the application");
        return true;
    }

    static void ViewAll()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            List<DrinkingWater> tableData = new();
            using (SqliteCommand queryCmd = new SqliteCommand("SELECT * FROM drinking_water", connection))
            {
                using (SqliteDataReader reader = queryCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.Clear();
                        Console.WriteLine("No rows found");
                    }
                    else
                    {
                        Console.WriteLine("----------------------------------------------------\n");
                        while (reader.Read())
                        {
                            tableData.Add(new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "MM-dd-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
            connection.Close();
            Console.WriteLine("----------------------------------------------------\n");
            foreach (var dataEntry in tableData)
            {
                Console.WriteLine($"{dataEntry.Id} - {dataEntry.Date.ToString("MM-dd-yyyy")} - Quantity: {dataEntry.Quantity}");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }
    }

    private static void InsertRecord()
    {
        string? date = GetDateInput();
        if (date == null) return;
        int quantity = GetNumberInput("Please enter number of glasses... (0 to return)");

        if (date != null && quantity > 0)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO drinking_water(Date,Quantity) VALUES(@NewDate, @NewQuantity)";
                using (SqliteCommand insertCmd = new SqliteCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.Add("@NewDate", SqliteType.Text).Value = date;
                    insertCmd.Parameters.Add("@NewQuantity", SqliteType.Integer).Value = quantity;
                    insertCmd.ExecuteNonQuery();
                }
                Console.WriteLine("Successfully inserted new record!");
                connection.Close();
            }
        }
        else
        {
            Console.WriteLine("Cancelling insert... Returning to main menu");
        }
    }

    static void DeleteRecord()
    {
        Console.Clear();
        ViewAll();
        int removeId;
        removeId = GetNumberInput("Enter the record ID you would like to delete...");
        if (removeId <= 0)
        {
            Console.Clear();
            Console.WriteLine("Cancelling Deletion process...");
            return;
        }
        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM drinking_water WHERE Id = @RemoveId";
            using (SqliteCommand queryCmd = new SqliteCommand(query, connection))
            {
                queryCmd.Parameters.Add("@RemoveId", SqliteType.Integer).Value = removeId;

                using (SqliteDataReader reader = queryCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine($"No record found with Id {removeId}");
                        return;
                    }
                    string deleteQuery = "DELETE FROM drinking_water WHERE Id = @RemoveId";
                    using (SqliteCommand deleteCmd = new SqliteCommand(deleteQuery, connection))
                    {
                        deleteCmd.Parameters.Add("@RemoveId", SqliteType.Integer).Value = removeId;
                        deleteCmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("Successfully deleted record!");
                }
            }
            connection.Close();
        }
    }
    static void UpdateRecord()
    {
        ViewAll();
        int recordId = GetNumberInput("\n\nWhich record would you like to update?");
        string query = "SELECT * FROM drinking_water WHERE Id = @RecordId";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (SqliteCommand queryRecords = new SqliteCommand(query, connection))
            {
                queryRecords.Parameters.Add("@RecordId", SqliteType.Integer).Value = recordId;
                using (SqliteDataReader reader = queryRecords.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine($"No record exists with id {recordId}");
                    }
                    else
                    {
                        string? newDate = GetDateInput();
                        int newQuantity = GetNumberInput("Enter new quantity...");
                        string updateQuery;

                        if (newDate != null && newQuantity > 0)
                        {
                            updateQuery = "UPDATE drinking_water SET date = @newDate, quantity = @newQuantity WHERE Id = @UpdateRecord";
                        }
                        else if (newDate != null)
                        {
                            updateQuery = "UPDATE drinking_water SET date = @newDate WHERE Id = @UpdateRecord";
                        }
                        else
                        {
                            updateQuery = "UPDATE drinking_water SET quantity = @newQuantity WHERE Id = @UpdateRecord";
                        }
                        using (SqliteCommand updateQueryRecords = new SqliteCommand(updateQuery, connection))
                        {
                            updateQueryRecords.Parameters.Add("@newDate", SqliteType.Text).Value = newDate;
                            updateQueryRecords.Parameters.Add("@newQuantity", SqliteType.Integer).Value = newQuantity;
                            updateQueryRecords.Parameters.Add("@UpdateRecord", SqliteType.Integer).Value = recordId;

                            updateQueryRecords.ExecuteNonQuery();
                        }
                        Console.WriteLine("Successfully updated record");
                    }
                }
            }
            connection.Close();
        }
    }

    private static string? GetDateInput()
    {
        Console.WriteLine("\n\n Please insert date: (Format: mm-dd-yy). Type 0 to return to main menu...");
        string? dateInput = Console.ReadLine();
        while (!DateTime.TryParseExact(dateInput, "mm-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            if (dateInput == null || dateInput == "0") return null;
            Console.WriteLine("\n\nInvalid date format. Try again...");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    private static int GetNumberInput(string msg)
    {
        Console.Clear();
        Console.WriteLine($"\n\n{msg}");
        int number;
        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Invalid number, try again or enter 0 to cancel");
        }
        return number;
    }
}