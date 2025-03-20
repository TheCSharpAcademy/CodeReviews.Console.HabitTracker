using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        
        static void Main(string[] args)
        {
            Console.WriteLine("Current Directory: " + System.IO.Directory.GetCurrentDirectory());


            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS drinking_water (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Quantity INTEGER)";

                    tableCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            GetUserInput();

        }
        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                System.Console.WriteLine("\nWhat would you like to do?");
                System.Console.WriteLine("\nType 0 to Close Application");
                System.Console.WriteLine("Type 1 to View All Records");
                System.Console.WriteLine("Type 2 to Insert Record");
                System.Console.WriteLine("Type 3 to Update Record");
                System.Console.WriteLine("Type 4 to Delete Record");
                System.Console.WriteLine("---------------------------------");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        System.Console.WriteLine("\nBye now\n");
                        closeApp = true;
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        Insert();
                        break;
                    case "3":
                        Update();    
                        break;
                    case "4":
                        Delete();
                        break;
                    default:
                        System.Console.WriteLine("\n Invalid Command");
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
                        @"SELECT * FROM drinking_water";

                    List<DrinkingWater> tableData = new List<DrinkingWater>();

                    SqliteDataReader reader = tableCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            });
                        }
                    } 
                    else
                    {
                        System.Console.WriteLine("no rows found");
                    }

                    connection.Close();

                    System.Console.WriteLine("-------------------------------------\n");
                    foreach (var dw in tableData)
                    {
                        System.Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                    }

                    System.Console.WriteLine("-------------------------------------\n");

                }
        }

        private static void Insert()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice.\n\n");

            using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        $"INSERT INTO drinking_water (Date, Quantity) VALUES('{date}', {quantity})";

                    tableCmd.ExecuteNonQuery();
                }
        }

        private static int GetNumberInput(string mes)
        {
            System.Console.WriteLine(mes);

            string numberInput = Console.ReadLine();

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }

        internal static string GetDateInput()
        {
            System.Console.WriteLine("\n\nPlease insert the date (dd-mm-yy)");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();
            

            return dateInput;
        }

        
        private static void Update()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to update.\n\n");

            using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var checkCmd = connection.CreateCommand();
                    checkCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM drinking_water WHERE Id = {recordId})";
                    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (checkQuery == 0)   
                    {
                        System.Console.WriteLine($@"\n\nRecord with Id {recordId} doesn't exists.\n\n");
                        connection.Close();
                        Update();
                    }

                    string date = GetDateInput();

                    int quantity = GetNumberInput("\n\nPlease insert number of glass.\n\n");

                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @$"UPDATE drinking_water SET Date = '{date}', Quantity = {quantity} WHERE Id = {recordId}";

                    int rowCount = tableCmd.ExecuteNonQuery();
                    connection.Close();
                }

        }

        private static void Delete()
        {
            Console.Clear();
            // GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete.\n\n");

            using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @$"DELETE from drinking_water WHERE Id = {recordId}";

                    int rowCount = tableCmd.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        System.Console.WriteLine($@"\n\nRecord with Id {recordId} doesn't exists.\n\n");
                        Delete();
                    }
                }
        }


    }

    public class DrinkingWater
    {
        public int Id { get; set; } 
        public DateTime Date { get; set; }      
        public int Quantity { get; set; }   
    }
}