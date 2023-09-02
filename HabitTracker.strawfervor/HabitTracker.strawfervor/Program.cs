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

                tableCmd.ExecuteNonQuery();//this is executing command without "output"

                connection.Close();
            }
        }

        public void main_menu()
        {
            Console.Clear();

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
                        Console.WriteLine("get_all_records");
                        break;
                    case 2:
                        Console.WriteLine("insert_record");
                        break;
                    case 3:
                        Console.WriteLine("");
                        break;
                    case 4:
                        Console.WriteLine("");
                        break;
                    default:
                        Console.WriteLine("Please choose correct option.");
                        break;
                }

            }
        }
        public void insert_record()
        {
            string currentQuery;
            Console.WriteLine("Please enter date in format dd-mm-yyyy: ");
            string 


            command_nonquery(currentQuery);
        }
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