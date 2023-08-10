using System;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using HabitTracker.AlainNshimirimana;

namespace HabitTracker
{
    public class Program
    {
        static string connectionString = @"Data Source=habitTracker.db";
        static void Main(string[] args)
        {
            //string connectionString = @"Data Source=habitTracker.db";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand(); // create a command to send to the database
                // create the database and table
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS WorkoutTracker (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Duration_hrs INTEGER
                    )";
                cmd.ExecuteNonQuery(); //execute the command w/ returning any values
                connection.Close(); // close the connection
            }
            //intro to the app and get user input
            Console.WriteLine("Hello! Welcome to the WorkoutTacker app where you will be able to log your workout.");
            UserInput();
        }

        static void UserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nWhat would you like to do (TYPE A NUMBER)");
                Console.ResetColor();
                Console.WriteLine("0. Close app");
                Console.WriteLine("1. View All Records");
                Console.WriteLine("2. New Record Entry");
                Console.WriteLine("3. Delete Record");
                Console.WriteLine("4. Update Record");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("> ");
                Console.ResetColor();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye");
                        closeApp = true;
                        break;

                    case "1":
                        GetRecords();
                        break;
                    case "2":
                        AddRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("\nPlease enter a valid integer (0-4)");
                        break;
                }
            }
        }

        static void GetRecords()  //retrieve records from the server
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM WorkoutTracker";
                List<Workout> workoutTracked = new();
                // read data from the DB
                SqliteDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        workoutTracked.Add(
                            new Workout
                            {
                                // tell reader what kind of date type (GetInt32) we're reading from the DB
                                // and what column (0,1, or 2) is located on the table
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "MM-dd-yy", new CultureInfo("en-US")),
                                Duration = reader.GetInt32(2)
                            }); ;
                    }
                }
                else
                {
                    Console.WriteLine("No Data Found");
                }
                connection.Close();
                // Now loop through the workoutTracked List and display all data
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" Id |  Date    | Duration ");
                Console.ResetColor();
                Console.WriteLine("+---+----------+---------+");
                foreach (var workout in workoutTracked)
                {
                    Console.WriteLine($" {workout.Id}  | {workout.Date.ToString("MM-dd-yy")} | {workout.Duration} hrs");
                }

                Console.WriteLine();
            }
        }

        // ADD RECORD
        static void AddRecord() //add new record
        {
            string date = DateInput();
            int hoursDuration = HoursInput();
            //now add the user inputs to the database
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    $"INSERT INTO WorkoutTracker (Date, Duration_hrs) VALUES('{date}', {hoursDuration})";
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        // DELETE RECORD
        static void DeleteRecord()
        {
            // Let's start by showing the user All data in the table
            Console.Clear();
            GetRecords();
            var recordId = GetIdInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    $"DELETE FROM WorkoutTracker WHERE Id = {recordId}";
                int rowCount = cmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with Id number of {recordId} doesn't exist.");
                    DeleteRecord();
                }
                Console.WriteLine($"\nThe record with the Id of {recordId} was successfully deleted");
                //UserInput();
            }
        }

        // UPDATE RECORD
        static void UpdateRecord() //Update record
        {
            // Let's start by showing the user All data in the table
            Console.Clear();
            GetRecords();
            var recordId = GetIdInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM WorkoutTracker WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(cmd.ExecuteScalar()); // returns 0 if false. 1 if true
                if (checkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with Id of {recordId} doesn't exist.\n");
                    UpdateRecord();
                }
                string date = DateInput();
                int hoursDuration = HoursInput();
                cmd.CommandText =
                    @$"UPDATE WorkoutTracker
                    SET Date = '{date}', Duration_hrs = {hoursDuration} 
                    WHERE Id = {recordId}";
                cmd.ExecuteNonQuery();
                Console.WriteLine($"\nThe record with the Id of {recordId} was successfully updated");
                connection.Close();
            }
        }

        // INTERNAL USER INPUT METHODS for getting Id, Date, and Duration inputs from user
        // Id
        internal static int GetIdInput()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nPlease enter the Id of the record you want to edit or Enter 0 to return to main menu\n> ");
            Console.ResetColor();
            string idInput = Console.ReadLine();
            if (idInput == "0") { UserInput(); }

            return Int32.Parse(idInput);
        }
        // Date
        internal static string DateInput()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nPlease enter workout date (Format: mm-dd-yy): Enter 0 to return to main menu \n> ");
            Console.ResetColor();
            string dateInput = Console.ReadLine();
            if (dateInput == "0") { UserInput(); }
            return dateInput;
        }
        // Workout Duration
        internal static int HoursInput()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nPlease enter workout duration (Format: Integer only. Round up to nearest whole number): Enter 0 to return to main menu\n> ");
            Console.ResetColor();
            string hoursInput = Console.ReadLine();
            if (hoursInput == "0") { UserInput(); }
            return Int32.Parse(hoursInput);
        }
    }
}
