using HabitTracker.Views;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.Controllers;

public class WaterController
{
    static string connectionString = @"Data Source=habit-Tracker.db";


    public static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("___________________________________");
            Console.WriteLine("Type 1 to view all records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            Console.WriteLine("___________________________________");
            Console.WriteLine("Type 0 to close");

            int commandInput;
            while (!int.TryParse(Console.ReadLine(), out commandInput))
            {
                Console.WriteLine("Enter a valid option");
            }

            

            switch (commandInput)
            {
                case 0:
                    Console.WriteLine("Bye!");
                    Environment.Exit(0);
                    break;
                case 1:
                    GetAllRecords();
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
                    Console.WriteLine("invalid command, type any number from 0 to 4");
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;
            }
        }
    }

    private static void Insert()
    {
        DateTime date = Program.GetDateInput();
        int quantity = Program.GetNumberInput("Enter the number of glasses or other measures (just integers)");


        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();


            connection.Close();
        }
        Console.WriteLine("Insert was a success");
        Thread.Sleep(1500);
        Console.Clear();
    }

    private static void Delete()
    {
        GetAllRecords();

        var recordId = Program.GetNumberInput("Enter the id of the record you want to delete or type 0 to go back");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";
            int rowCount = tableCmd.ExecuteNonQuery();


            if (rowCount == 0)
            {
                Console.WriteLine($"Record with the {recordId} id doesn't exist\n");
                Thread.Sleep(1000);
                Delete();
            }
            else
            {
                var resetCmd = connection.CreateCommand();
                resetCmd.CommandText = "DELETE FROM sqlite_sequence WHERE name = 'drinking_water';";
                resetCmd.ExecuteNonQuery();
                Thread.Sleep(1000) ;
                Console.WriteLine($"Record with id {recordId} was deleted \n");


            }
        }
        
        Console.Clear();

        GetUserInput();
    }


    private static void GetAllRecords()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            ResetIds();
            Console.Clear();
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.Parse(reader.GetString(1)),
                        Quantity = reader.GetInt32(2)
                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found...");
            }
            connection.Close();
            Console.WriteLine("Drinked water log");
            Console.WriteLine("-----------------------------------------");
            foreach (var x in tableData)
            {
                Console.WriteLine($"{x.Id} - {x.Date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)} - Quantity: {x.Quantity}");
            }
            Console.WriteLine("------------------------------------------\n");
        }
    }



    public static void ResetIds()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT Id FROM drinking_water ORDER BY Id";

            List<int> ids = new();
            SqliteDataReader reader = selectCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ids.Add(reader.GetInt32(0));
                }
            }
            connection.Close();

            connection.Open();
            var updateCmd = connection.CreateCommand();
            for (int i = 0; i < ids.Count; i++)
            {
                updateCmd.CommandText = $"UPDATE drinking_water SET Id = {i + 1} WHERE Id = {ids[i]}";
                updateCmd.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    private static void Update()
    {
        GetAllRecords();

        var recordId = Program.GetNumberInput("\nPlease enter the id of the record you would like to update, type 0 to return...\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n Record with Id {recordId} doesn't exist");
                Thread.Sleep(2000);
                connection.Close();
                Update();
            }

            DateTime date = Program.GetDateInput();
            int quantity = Program.GetNumberInput("Enter the number of glasses or cups");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
            tableCmd.ExecuteNonQuery();
        }
        Console.WriteLine("Update successful");
        Thread.Sleep(1500);
        Console.Clear();
    }
}




