using System;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;
using SQLitePCL;

namespace HabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS StudyHours 
                        (ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
                        ,Date DATE
                        ,Quantity INTEGER                        
                        )";

                command.ExecuteNonQuery();

                connection.Close();
            }

            userInput();
        }

        static void userInput()
        { 
            Console.Clear();
            bool end = false;
            string? input;
            while (!end)
            {
                Console.WriteLine("MAIN MENU\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("------------------------------------------\n");

                input = Console.ReadLine();

                switch (input)
                {
                    case "0": 
                        end = true; 
                        break;
                    case "1":
                        viewRecord();
                        break;
                    case "2":
                        insert();
                        break;
                    case "3":
                        delete();
                        break;
                    case "4":
                        update();
                        break;
                    default:
                        Console.WriteLine("Invalid command. Please type number 0 to 4.");
                        break;
                }
            }

        }

        static void viewRecord()
        {
            Console.Clear();

            using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE StudyHours 
                        (ID int NOT NULL PRIMARY KEY AUTOINCREMENT
                        ,Date Datetime
                        ,Quantity Int                        
                        )";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        static void insert()
        {
            string date = getDate();
            int quantity = getNumber("Please insert number of hours in whole number");

            using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText =
                    $"INSERT INTO StudyHours (Date, Quantity) Values('{date}','{quantity}')";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        static void delete()
        { 
            Console.Clear(); 
        }
        
        static void update()
        { 
            Console.Clear(); 
        }

        static string getDate()
        {
            string? date;
            do
            {
                Console.WriteLine("Please insert the date in format of YYYY-MM-DD. Type 0 to return to main menu.");
                date = Console.ReadLine();

            } while (date == null);

            if (date == "0") userInput();

            return date;
        }

        static int getNumber(string message)
        {
            int num;
            string? numberInput;

            do
            {
                Console.WriteLine(message);
                numberInput = Console.ReadLine();

            } while (!int.TryParse(numberInput, out num));

            if (numberInput == "0") userInput();

            return num;
        }
    }
}