using HabitTracker.Jackua.Model;
using HabitTracker.Jackua.View;
using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.Jackua.Controller;

public class HabitTrackerController
{
    static string connectionString = @"Data Source=habit-Tracker.db";

    public static void Run()
    {

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
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
        while (true)
        {
            MenuView.MainMenu();

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    MenuView.GoodBye();
                    Environment.Exit(0);
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
                    MenuView.InvalidCommand();
                    break;
            }
        }

    }

    private static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM drinking_water ";

            List<DrinkingWaterModel> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new DrinkingWaterModel
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-us")),
                            Quantity = reader.GetInt32(2)
                        });
                }
            }
            else
            {
                MenuView.NoRows();
            }

            connection.Close();

            MenuView.DashLines();
            foreach (var dw in tableData)
            {
                MenuView.DisplayDrinkingWater(dw);
            }
            MenuView.DashLines();
        }
    }

    private static void Insert()
    {
        Console.Clear();
        string date = GetDateInput();
        if (date == "0") return;

        MenuView.QuantityRequest();
        int quantity = GetNumberInput();
        if (quantity == 0) return;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    private static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        MenuView.DeleteId();
        var recordId = GetNumberInput();
        if (recordId == 0) return;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                MenuView.DoesNotExist(recordId);
            } 
            else
            {
                MenuView.Deleted(recordId);
            }

            connection.Close();
        }
    }

    private static void Update()
    {
        GetAllRecords();

        MenuView.UpdateId();
        var recordId = GetNumberInput();
        if (recordId == 0) return;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                MenuView.DoesNotExist(recordId);
                connection.Close();
                return;
            }

            string date = GetDateInput();
            if (date == "0") 
            {
                connection.Close();
                return; 
            }

            MenuView.QuantityRequest();
            int quantity = GetNumberInput();
            if (quantity == 0)
            {
                connection.Close();
                return;
            }

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"Update drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    private static string GetDateInput()
    {
        MenuView.DateRequest();

        string dateInput = Console.ReadLine();

        if (dateInput == "0") return "0";

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            MenuView.InvalidDate();
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    private static int GetNumberInput()
    {
        string numberInput = Console.ReadLine();

        if (numberInput == "0") return 0;

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            MenuView.InvalidNumber();
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
}