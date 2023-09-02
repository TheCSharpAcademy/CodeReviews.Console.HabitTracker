using Microsoft.Data.Sqlite;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace habit_tracker
{
    class Tracker
    {
        public bool CheckDate(String input)
        {
            Regex regex = new Regex("^[0-9][0-9]-[0-9][0-9]-[0-9][0-9][0-9][0-9]$", RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }

        string connectionString = @"Data Source=habit-Tracker.db";

        public void CommandNonQuery(string command)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = command;

                int rowCount = tableCmd.ExecuteNonQuery();//this is executing command without "output"

                if (rowCount > 0)
                { 
                    Console.Write($"Record updated!");
                }
                else
                {
                    Console.Write("No records has been updated.");
                }

                connection.Close();
            }
        }

        public void MainMenu()
        {
            bool menuLoop = true;
            while (menuLoop) 
            {
                Console.WriteLine("\n\nMain menu:");
                Console.WriteLine("\nPlease choose option:");
                Console.WriteLine("\t1 - View all records");
                Console.WriteLine("\t2 - Insert record");
                Console.WriteLine("\t3 - Delete record");
                Console.WriteLine("\t4 - Update record");
                Console.WriteLine("\t0 - Close app");
                Console.WriteLine("=============================================\n");

                string userInput = Console.ReadLine()!;
                int userCommand = 9999;
                try
                {
                    userCommand = int.Parse(userInput);
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Numbers only");
                }

                switch(userCommand)
                {
                    case 0:
                        Console.WriteLine("\nClosing app\n");
                        menuLoop = false;
                        Environment.Exit(0);
                        break;
                    case 1:
                        GetAllRecords();
                        break;
                    case 2:
                        InsertRecord();
                        break;
                    case 3:
                        Delete();
                        break;
                    case 4:
                        Update();
                        break;
                    default:
                        Console.WriteLine("Please choose correct option.");
                        break;
                }

            }
        }
        public void InsertRecord()
        {
            string currentQuery;
            int waterMl = 0;
            Console.WriteLine("Please enter date in format dd-mm-yyyy: ");
            string date = Console.ReadLine()!;
            while (!CheckDate(date))
            {
                Console.WriteLine("Please use correct format dd-mm-yyyy (ex. 12-01-2023): ");
                date = Console.ReadLine()!;
            }
            
            Console.WriteLine("Please enter amount of water drinked\n(just numbers in ml, 1 glass is about 250 ml):");
            while (true)
            {
                try
                {
                    waterMl = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Please enter numbers only!");
                }
            }
            currentQuery = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', '{waterMl}')";


            CommandNonQuery(currentQuery);
        }

        public void GetAllRecords()
        {
            List<DrinkingWater> tableData = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT * FROM drinking_water";

                SqliteDataReader reader = tableCmd.ExecuteReader(); //we are using this reader instead of ExecuteNonQuery because we want to 'feed' reader with data from DB
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new DrinkingWater
                        {
                            Id = reader.GetInt32(0),//0 indicate number of collumn
                            Date = reader.GetString(1),
                            Quantity = reader.GetInt32(2),
                        }) ;
                    }
                }
                else
                {
                    Console.WriteLine("No rows found!");
                }
                connection.Close();
            }

            foreach (var drinkingWater in tableData)
            {
                Console.WriteLine($"\t{drinkingWater.Id}) {drinkingWater.Quantity} ml of water has been drinked on {drinkingWater.Date}");
            }
        }

        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("==============================\n");
            GetAllRecords();
            string currentQuery;
            int recordToDelete = 0;
            Console.WriteLine("Please type the Id of the record you want to delete (0 or number that is not Id returns to main menu):");
            while (true)
            {
                try
                {
                    recordToDelete = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please use numbers only!");
                }
            }
            if ( recordToDelete > 0 )
            {
                currentQuery = $"DELETE FROM drinking_water WHERE Id = '{recordToDelete}'";
                CommandNonQuery(currentQuery);
            }
        }

        public void Update()
        {
            Console.Clear();
            Console.WriteLine("==============================\n");
            GetAllRecords();
            int recordToUpdate = 0;
            Console.WriteLine("Please type the Id of the record you want to delete (0 or number that is not Id returns to main menu):");
            while (true)
            {
                try
                {
                    recordToUpdate = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please use numbers only!");
                }
            }
            
            string currentQuery;
            int WaterMl = 0;
            Console.WriteLine("Please enter date in format dd-mm-yyyy: ");
            string date = Console.ReadLine()!;
            while (!CheckDate(date))
            {
                Console.WriteLine("Please use correct format dd-mm-yyyy (ex. 12-01-2023): ");
                date = Console.ReadLine()!;
            }
            Console.WriteLine("Please enter amount of water drinked\n(just numbers in ml, 1 glass is about 250 ml):");
            while (true)
            {
                try
                {
                    WaterMl = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Please enter numbers only!");
                }
            }
            currentQuery = $"UPDATE drinking_water SET date = '{date}', quantity = '{WaterMl}' WHERE Id = {recordToUpdate}";
            CommandNonQuery(currentQuery);
        }
    }

    public class DrinkingWater
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //create table command, if command doesnt exists, @ at beggining of command allows to use multiline strings
            string create_table_command = @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
                )";
            Tracker tr = new Tracker();
            tr.CommandNonQuery(create_table_command);

            tr.MainMenu();
        }
    }
}