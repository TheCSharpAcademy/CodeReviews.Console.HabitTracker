using Microsoft.Data.Sqlite;
using System;
using System.Security.Cryptography.X509Certificates;

namespace habit_tracker
{
    class Tracker
    {
        string connectionString = @"Data Source=habit-Tracker.db";

        public void command_nonquery(string command)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = command;

                int row_count = tableCmd.ExecuteNonQuery();//this is executing command without "output"

                if (row_count > 0)
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

        public void main_menu()
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
                        break;
                    case 1:
                        get_all_records();
                        break;
                    case 2:
                        insert_record();
                        break;
                    case 3:
                        delete();
                        break;
                    case 4:
                        update();
                        break;
                    default:
                        Console.WriteLine("Please choose correct option.");
                        break;
                }

            }
        }
        public void insert_record()
        {
            string current_query;
            int water_ml = 0;
            Console.WriteLine("Please enter date in format dd-mm-yyyy: ");
            string date = Console.ReadLine()!;
            Console.WriteLine("Please enter amount of water drinked\n(just numbers in ml, 1 glass is about 250 ml):");
            while (true)
            {
                try
                {
                    water_ml = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Please enter numbers only!");
                }
            }
            current_query = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', '{water_ml}')";


            command_nonquery(current_query);
        }

        public void get_all_records()
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

            foreach (var drinking_water in tableData)
            {
                Console.WriteLine($"\t{drinking_water.Id}) {drinking_water.Quantity} ml of water has been drinked on {drinking_water.Date}");
            }
        }

        public void delete()
        {
            Console.Clear();
            Console.WriteLine("==============================\n");
            get_all_records();
            string current_query;
            int record_to_delete = 0;
            Console.WriteLine("Please type the Id of the record you want to delete (0 or number that is not Id returns to main menu):");
            while (true)
            {
                try
                {
                    record_to_delete = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please use numbers only!");
                }
            }
            if ( record_to_delete > 0 )
            {
                current_query = $"DELETE FROM drinking_water WHERE Id = '{record_to_delete}'";
                command_nonquery(current_query);
            }
        }

        public void update()
        {
            Console.Clear();
            Console.WriteLine("==============================\n");
            get_all_records();
            int record_to_update = 0;
            Console.WriteLine("Please type the Id of the record you want to delete (0 or number that is not Id returns to main menu):");
            while (true)
            {
                try
                {
                    record_to_update = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please use numbers only!");
                }
            }

            string current_query;
            int water_ml = 0;
            Console.WriteLine("Please enter date in format dd-mm-yyyy: ");
            string date = Console.ReadLine()!;
            Console.WriteLine("Please enter amount of water drinked\n(just numbers in ml, 1 glass is about 250 ml):");
            while (true)
            {
                try
                {
                    water_ml = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Please enter numbers only!");
                }
            }
            current_query = $"UPDATE drinking_water SET date = '{date}', quantity = '{water_ml}' WHERE Id = {record_to_update}";
            command_nonquery(current_query);
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
            tr.command_nonquery(create_table_command);

            tr.main_menu();
        }
    }
}