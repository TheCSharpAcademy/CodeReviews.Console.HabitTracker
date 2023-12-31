using System;
using Habit_tracker.Ibrahim;
using Microsoft.Data.Sqlite;


namespace habit_tracker;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source= HabitTracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS daily_prayer (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT UNIQUE,
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

        Console.WriteLine("Welcome to your prayer tracker! \n");
        Console.WriteLine("press any key to go to the main menu");
        Console.ReadLine();

        while (!endApp)
        {
            Console.Clear();
            Console.WriteLine("Main Menu \n");
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("Type 0 to close Application");
            Console.WriteLine("Type 1 to View all records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            Console.WriteLine("Type 5 to view Reports\n");
            string userChoice = helper.ValidateChoice(Console.ReadLine());


            switch (userChoice)
            {
                case "0":
                    endApp = true;
                    break;

                case "1":
                    Console.Clear();
                    Console.WriteLine("your prayer history \n");
                    Console.WriteLine("Id\t Date\t\tPrayers \n");
                    database.Retrieve();
                    break;

                case "2":
                    Console.Clear();

                    Console.WriteLine(@"Type in your prayer count:
minimum = 0
maximum = 5" + "\n");

                    string input = helper.ValidateInt(Console.ReadLine());

                    int prayers = Convert.ToInt32(input);

                    Console.WriteLine("\nType in the date you wish to enter with the format: yyyy-mm-dd");

                    string date = helper.ValidateDate(Console.ReadLine());

                    database.Insert(date, prayers);
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine(@"Please enter the date you want to Delete in the format yyyy-mm-dd");
                    date = helper.ValidateDate(Console.ReadLine());

                    database.Delete(date);

                    Console.WriteLine($"\n {date} has been deleted");
                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine(@"please enter the date of the record which you want to change
Use the format: yyyy-mm-dd");
                    date = helper.ValidateDate(Console.ReadLine());
                    Console.WriteLine("\nplease enter the new number of prayers you prayed for the day");
                    int quantity = Convert.ToInt32(Console.ReadLine());

                    database.Update(date, quantity);
                    break;

                case "5":
                    Console.Clear();
                    Console.WriteLine("REPORTS: \n");
                    database.getReport();
                    // 
                    break;
                default:
                    Console.WriteLine("entered invalid input please try again");
                    break;

            }
            if (!endApp) 
            { 
                Console.WriteLine("\n press any key to go back to menu");
                Console.ReadLine();
            }
        }


    }

}