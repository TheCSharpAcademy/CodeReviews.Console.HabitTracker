using Microsoft.Data.Sqlite;
using STUDY.ConsoleApp.HabitLogger.Models;
using System.Globalization;

namespace STUDY.ConsoleApp.HabitLogger;

internal class CrudOperations
{
    internal static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT * FROM drinking_water ";

            List<DrinkingWater> tableData = new();

            SqliteDataReader tableDataReader = tableCmd.ExecuteReader();
            if (tableDataReader.HasRows)
            {
                while (tableDataReader.Read())
                {
                    tableData.Add(new DrinkingWater
                    {
                        ID = tableDataReader.GetInt32(0),
                        Date = DateTime.ParseExact(tableDataReader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = tableDataReader.GetInt32(2)
                    });
                }
            }
            else Console.WriteLine("No rows found.");

            connection.Close();

            Console.WriteLine("------------------------------------------\n");
            foreach (DrinkingWater drink in tableData)
            {
                Console.WriteLine($"{drink.ID} - {drink.Date.ToString("dd-MMM-yy")} - Quantity: {drink.Quantity}");
            }
            Console.WriteLine("------------------------------------------\n");

        }
    }
    internal static void Insert()
    {
        string date = Helpers.GetDateInput();
        int quantity = Helpers.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO drinking_water(Date, QUANTITY) VALUES ('{date}',{quantity}) ";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordID = Helpers.GetNumberInput
            ("\n\nEnter row ID of the recourd you want to delete or typr 0 to go back to main menu.\n\n");

        using(SqliteConnection connection = new (Program.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DELETE FROM drinking_water WHERE ID = '{recordID}' ";

            var rawCount = tableCmd.ExecuteNonQuery();
            if (rawCount == 0) 
            {
                Console.Write($"\n\nRecord with ID {recordID} doesn't exist. Press Enter to try again: ");
                connection.Close();
                Console.ReadLine();
                Delete();
            }

            Console.WriteLine($"\n\nRecord with ID {recordID} was deleted.\n\n");
            Menu.GetUserInput();
        }
    }

    internal static void Update()
    {
        Console.Clear();
        GetAllRecords();
        var recordID = Helpers.GetNumberInput
            ("\n\nEnter row ID of the recourd you want to update or typr 0 to go back to main menu.\n\n");
        
        using (SqliteConnection connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordID})";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());     
            if (checkQuery == 0)
            {
                Console.Write($"\n\nRecord with the ID {recordID} is not exist. Press Enter to try again: ");
                connection.Close();
                Console.ReadLine();
                Update();
            }
            var date = Helpers.GetDateInput();
            int quantity = Helpers.GetNumberInput
                ("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
            
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = 
                $"UPDATE drinking_water SET Date = '{date}' ,Quantity = {quantity} WHERE Id = {recordID}";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
