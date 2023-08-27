using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HabitTracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main(string[] args)
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS eating_biscuits ( 
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER)";
                tableCmd.ExecuteNonQuery();
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
                Console.WriteLine("\n\nMain Menu");
                Console.WriteLine("\nWhat would you like to do");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("===============================\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye\n");
                        closeApp = true;
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
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }

            }

        }

        static void Update()
        {
            GetAllRecords();
            var recordId = GetNumberInput("\n\nPlease type the ID of the record you want to update. Type 0 to return to Main Menu\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM eating_biscuits WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nrecord with Id {recordId} doesnt exist.\n\n");
                    connection.Close();
                    Update();
                }
                string date = GetDateInput();
                int quantity = GetNumberInput("\n\nPlease insert number of biscuits (no decimals allowed)\n");
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE eating_biscuits SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        static void Delete()
        {
            Console.Clear();
            GetAllRecords();
            var recordId = GetNumberInput("\n\nPlease type the ID of the record you want to delete or return to Main Menu\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM eating_biscuits WHERE Id = '{recordId}'";
                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with ID {recordId} doesnt exist. \n\n");
                    Delete();
                }
            }
            Console.WriteLine($"\n\nRecord with ID {recordId} was deleted. \n\n");
            GetUserInput();
        }

        static void Insert()
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert number of biscuits (no decimals allowed)\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO eating_biscuits(date, quantity) VALUES ('{date}', {quantity})";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }

        static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date :(Format:dd-mm-yy). Type 0 to return to main menu");
            string dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }

        static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM eating_biscuits ";
                List<EatingBiscuits> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new EatingBiscuits
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-mm-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        }); ;
                    }
                }

                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();
                Console.WriteLine("-------------------------------------\n");

                foreach (var item in tableData)
                {
                    Console.WriteLine($"{item.Id} - {item.Date.ToString("dd-mm-yy")} - Quantity { item.Quantity}");
                }
                Console.WriteLine("-------------------------------------\n");
            }
        }
    }

    public class EatingBiscuits
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}
