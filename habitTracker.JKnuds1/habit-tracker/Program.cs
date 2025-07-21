using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace habit_tracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            CreateDatabase();
            GetDatabaseMenu();

        }
        private static void GetDatabaseMenu()
        {
            Console.Clear();
            bool run = true;
            while (run){
            Console.Write("Main Menu\n\n 1. - View Habit Log \n 2. - Insert Habit \n 3. - Delete Habit\n 4. - Update Habit \n 5. - Quit\n\n Input one of the numbers: ");
            string? userInput = Console.ReadLine();
            
            switch (userInput){
                case "1":
                    GetAllRecords();
                    Console.ReadLine();
                    Console.Clear();
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
                case "5":
                    Console.WriteLine("\n\nGoodbye...");
                    Console.ReadLine();
                    run = false;
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong Input. Please type a number from the menu.\n\n");
                    break;
            }

        }

        }
        private static void CreateDatabase(){

            using (var connection = new SqliteConnection(connectionString)) 
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Date TEXT,
                                            Quantity INTEGER
                                            )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        
        }

        private static void Insert()
        {
            Console.Clear();
            string? date = GetDateInput();
            int number = GetNumberInput("Input the number quantity.\n Or input 0 to get to main menu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand("INSERT INTO drinking_water (date, quantity) VALUES (@date,@quantity)",connection))
                {
                    command.Parameters.Add("@date", SqliteType.Text).Value = date;
                    command.Parameters.Add("@quantity", SqliteType.Integer).Value = number;
                    command.ExecuteNonQuery();
                }
                connection.Close();
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
                    $"SELECT * FROM drinking_water";

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-GB")),
                                Quantity = reader.GetInt32(2)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();

                Console.WriteLine("-----------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - {dw.Quantity}");
                }
                Console.WriteLine("\n-----------------------------------------\n");
           }
        }

        internal static string? GetDateInput()
        {
            
            Console.WriteLine("Input the date 'dd-mm-yyyy'.\nOr input 0 to get to main menu.\n\n");
            string? dateInput = Console.ReadLine();

            while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
            {
                if (dateInput == "0") GetDatabaseMenu();
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yyyy). Type 0 to return to main menu or try again:\n\n");
                dateInput = Console.ReadLine();
            }
            return dateInput;

        }

        internal static string? GetNameInput(string message)
        {   
            Console.WriteLine(message);
            string? nameInput = Console.ReadLine();

            while(String.IsNullOrEmpty(nameInput))
            { 
                Console.WriteLine("\n\n Invalid entry try again.\n\n");
                nameInput = Console.ReadLine();
            }
            if(nameInput == "0") GetDatabaseMenu();
            return nameInput;
        }

        internal static int GetNumberInput(string message)
        {
    
            Console.WriteLine(message);
            string? numberInput = Console.ReadLine();
            if (numberInput == "0") GetDatabaseMenu();
            
            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput)<0)
            {
                Console.WriteLine("\n\nInvalid number. Try again. \n\n");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);
            Console.Clear();
            return finalInput;

        }
        
        internal static void Delete()
        {
            Console.Clear();
            GetAllRecords();
            int deleteId = GetNumberInput("Input Id number of the element you wish to delete.\nOr input 0 to get to the main menu.\n\n");

            using (var connection = new SqliteConnection(connectionString)) 
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand("DELETE FROM drinking_water WHERE id=@Id",connection))
                {
                    command.Parameters.Add("@Id", SqliteType.Integer).Value = deleteId;
                    int rowCount = command.ExecuteNonQuery();
                    
                    if (rowCount == 0)
                    {
                    Console.WriteLine($"\n\nRecord with Id {deleteId} doesn't exist \n\n");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    connection.Close();
                    Delete();
                    }
                }

                connection.Close();
            }


        }

        internal static void Update()
        {
            Console.Clear();
            GetAllRecords();
            int updateId = GetNumberInput("Input Id number of row you wish to update.\nOr input 0 to get to the main menu.\n\n");
            
            using (var connection = new SqliteConnection(connectionString)) 
            {
                connection.Open();

                using(SqliteCommand command = new SqliteCommand("SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = @Id)", connection))
                {
                    command.Parameters.Add("@Id", SqliteType.Integer).Value = updateId;
                    
                    int checkQuery = Convert.ToInt32(command.ExecuteScalar());
                    if (checkQuery == 0)
                    {
                    Console.WriteLine($"\n\nRecord with Id {updateId} doesn't exist. \n\n");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    connection.Close();
                    GetDatabaseMenu();
                    }
                }

                string? updateDate = GetDateInput();
                int updateCount = GetNumberInput("Input new count.");
                
                using(SqliteCommand command = new SqliteCommand("UPDATE drinking_water SET Date=@Date, Quantity=@Count WHERE id=@Id", connection))
                {
                    command.Parameters.Add("@Id", SqliteType.Integer).Value = updateId;
                    command.Parameters.Add("@Date", SqliteType.Integer).Value = updateDate;
                    command.Parameters.Add("@Count", SqliteType.Integer).Value = updateCount;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

    }
}

public class DrinkingWater
{
    public int Id { get; set;}
    public DateTime Date {get; set;}

    public int Quantity {get; set;}
}