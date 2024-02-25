﻿using System;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;
using SQLitePCL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            bool end = false;
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

            userInput(end);
        }

        static void userInput(bool end)
        {
            Console.Clear();
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
                        Environment.Exit(0);
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
                        Console.WriteLine("\nInvalid command. Please type number 0 to 4.\n");
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
                    "SELECT * FROM StudyHours";

                List<string> table = new List<string>();

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        table.Add($"{reader.GetString(0)} - Date: {reader.GetString(1)} - Hours: {reader.GetString(2)}");
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                connection.Close();

                foreach (var row in table)
                {
                    Console.WriteLine(row);
                }

                Console.WriteLine();

            }
        }

        static void insert()
        {
            Console.Clear();
            string date = getDate();
            int quantity = getNumber("Please insert number of hours in whole number, or type 0 to go back to main menu.");

            using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = $"INSERT INTO StudyHours (Date, Quantity) Values('{date}','{quantity}')";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        static void delete()
        {
            Console.Clear();
            viewRecord();

            var recordID = getNumber("Please type the ID of the record you want to delete, or type 0 to go back to main menu.");

            using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM StudyHours where ID = '{recordID}'";

                int rowCount = command.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord ID {recordID} doesn't exist. Please any key to try again! \n");
                    Console.ReadLine();
                    delete();
                }
                else
                {
                    Console.WriteLine($"\nRecord ID {recordID} is deleted. \n");
                }

                connection.Close();

            }
        }

        static void update()
        {
            Console.Clear();
            viewRecord();

            var recordID = getNumber("Please type the ID of the record you want to update, or type 0 to go back to main menu.");

            using (var connection = new SqliteConnection("Data source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = $"SELECT * FROM StudyHours WHERE ID = '{recordID}'";

                var rows = command.ExecuteScalar();

                if (rows == null)
                {
                    Console.WriteLine($"\nRecord ID {recordID} doesn't exist. Please any key to try again! \n");
                    Console.ReadLine();
                    update();
                }
                else
                {
                    string date = getDate();
                    int quantity = getNumber("Please insert number of hours in whole number, or type 0 to go back to main menu.");

                    command.CommandText = $"UPDATE StudyHours SET Date='{date}',Quantity='{quantity}' WHERE ID = '{recordID}'";

                    command.ExecuteNonQuery();

                    Console.WriteLine($"\nRecord ID {recordID} is updated. \n");

                    connection.Close();
                }


            }

        }

        static string getDate()
        {
            string? date;
            bool result;
            do
            {
                Console.WriteLine("Please insert the date in format of YYYY-MM-DD. Type 0 to return to main menu.");
                date = Console.ReadLine();
                if (date == "0") userInput(false);
                result = DateTime.TryParseExact(date, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime product);

            } while ((date == null) || (!result));

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

            if (numberInput == "0") userInput(false);

            return num;
        }
    }
}