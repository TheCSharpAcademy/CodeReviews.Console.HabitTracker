using System;
using Microsoft.Data.Sqlite;
using Repository;

namespace habit_tracker;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source= HabitTracker.db";

        using(var connection = new SqliteConnection(connectionString)){
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = 
                @"CREATE TABLE IF NOT EXISTS daily_prayer (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
                )";

            tableCmd.ExecuteNonQuery();
            connection.Close();

            DatabaseAccessLayer database = new DatabaseAccessLayer(connectionString);

            UserInput(connectionString, database);
        }
    }

    public static void UserInput(string connectionString, DatabaseAccessLayer database)
    {
        
        bool endApp = false;

        while (!endApp)
        {
            Console.Clear();
            Console.WriteLine("Welcome to your prayer tracker!");
            Console.WriteLine("Main Menu \n");
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("Type 0 to close Application");
            Console.WriteLine("Type 1 to View all records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            string userChoice= Console.ReadLine();
            

            switch (userChoice)
            {
                case "0":
                    endApp = true;
                    break;

                case "1":
                    Console.Clear();
                    Console.WriteLine("your prayer history \n");
                    Console.WriteLine("Id\t Date\t\t\t Prayers \n");
                    database.Retrieve();
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine(@"Type in your prayer count for today:
minimum = 0
maximum = 5");
                    int prayers = Convert.ToInt32(Console.ReadLine());

                    database.Insert(DateTime.UtcNow.Date.ToString(), prayers);
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("Please enter the records' ID number which you want to Delete");
                    int id = Convert.ToInt32(Console.ReadLine());

                    database.Delete(id);

                    Console.WriteLine($"record {id} has been deleted");
                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine("please enter the ID of the record which you want to change");
                    id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("please enter the new number of prayers you prayed for the day");
                    int quantity = Convert.ToInt32(Console.ReadLine());

                    database.Update(id, quantity);
                    break;
                default:
                    Console.WriteLine("entered invalid input please try again");
                    break;

            }
            Console.WriteLine("press any key to go back to menu");
            Console.ReadLine();
        }


    }

}