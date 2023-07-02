using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HabitTracker.alvaromosconi
{
    internal class HabitRepository
    {
        private const string TABLE_NAME = "habits";
        private List<Habit> tableData = new();

        public HabitRepository() 
        {
            SaveAllRecordsInCache();
        }

        internal void CreateTable()
        {
            using (var connection = new SqliteConnection(@"Data Source=habit-tracker.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    $@"
                        CREATE table IF NOT EXISTS {TABLE_NAME} 
                            (id INTEGER PRIMARY KEY AUTOINCREMENT,
                             name TEXT,
                             date TEXT,
                             quantity INTEGER
                            )
                    ";

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        internal void UpdateCache()
        {

        }

        internal void ExecuteOption(string userInput)
        {
            switch (userInput)
            {
                case "1":
                    PrintAllRecords();
                    break;
                case "2":
                    InsertNewRecord();
                    break;
                case "3":
                    UpdateAnExistingRecord();
                    break;
                case "4":
                    DeleteAnExistingRecord();
                    break;
            }

            PrintBackOption();
        }

        private void PrintBackOption()
        {
            
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
        }
        private void SaveAllRecordsInCache()
        {
            Console.Clear();

          
                using (var connection = new SqliteConnection(@"Data Source=habit-tracker.db"))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                        $@"
                            SELECT * FROM {TABLE_NAME}
                        ";

                    SqliteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableData.Add(
                            new Habit
                            {
                                Name = reader.GetString(1),
                                Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(3),
                            });
                    }

                    connection.Close();
                }
            
        }

        private void PrintAllRecords()
        {
            Console.WriteLine("\nDate           Name         Quantity");
            Console.WriteLine("---------------------------------------");

            var groupedHabits = tableData.GroupBy(habit => habit.Name);

            foreach (var group in groupedHabits)
            {
                foreach (Habit habit in group)
                {
                    Console.WriteLine($"{habit.Date,-15:dd-MM-yy}{habit.Name,-12}{habit.Quantity,10}");
                }
            }

        }

        internal void DeleteAnExistingRecord()
        {
            throw new NotImplementedException();
        }

        internal void UpdateAnExistingRecord()
        {
            throw new NotImplementedException();
        }

        internal void InsertNewRecord()
        {
            List<string> existingHabitNames = GetExistingHabitNames();

            if (existingHabitNames.Count == 0)
            {
                Insert(null);
            }
            else
            {
                Console.WriteLine("\nPlease type\n 1. For creating a new habit. \n 2. For choose an existing habit.");
                string userChoice = Console.ReadLine();
                switch (userChoice)
                {
                    case "1":
                        Console.Clear();
                        Insert(null);
                        break;
                    case "2":
                        Console.Clear();
                        PrintAllExistingHabitNames(existingHabitNames);
                        string habitName = String.Empty;

                        do
                        {
                            habitName = Console.ReadLine();
                           

                        } while (!existingHabitNames.Contains(habitName));

                        Insert(habitName);
                        break;
                    default:
                        break;
                }

            }
        }

        private void Insert(string? name)
        {

            if (name == null)
            {
                name = GetNameInput();
            }

            string dateInput = GetDateInput();
            int quantityInput = GetQuantityInput();

            using (var connection = new SqliteConnection(@"Data Source=habit-tracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    $"INSERT INTO {TABLE_NAME}(name, date, quantity) VALUES('{name}', '{dateInput}', {quantityInput})";

                command.ExecuteNonQuery();

                tableData.Add(
                new Habit {
                    Name = name,
                    Date = DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US")),
                    Quantity = quantityInput
                });

                connection.Close();
            }
        }

        private int GetQuantityInput()
        {
            string userInput = String.Empty;
            int quantity;
            do
            {
                Console.WriteLine("\nPlease enter the quantity for this record.");
                userInput = Console.ReadLine();
                
            } while (!Int32.TryParse(userInput, out quantity));
        
            return quantity;
        }

        private string GetNameInput()
        {
            string userInput = String.Empty;

            do
            {
                Console.WriteLine("Please enter the name of the new habit.");
                userInput = Console.ReadLine();

            } while (String.IsNullOrEmpty(userInput));

            return userInput;
        }

        private string GetDateInput()
        {
            string userInput = string.Empty;
            bool isValidFormat = false;

            do
            {
                Console.WriteLine("\nPlease enter the date: (Format: dd-mm-yy). Type 0 to return to the main menu.");
                userInput = Console.ReadLine();

                if (userInput == "0")
                {
                    Program.GetUserInput();
                    return string.Empty;
                }

                string dateFormatPattern = @"^\d{2}-\d{2}-\d{2}$";
                if (Regex.IsMatch(userInput, dateFormatPattern))
                {
                    isValidFormat = true;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                }
            }
            while (!isValidFormat);

            return userInput;
        }



        private List<string> GetExistingHabitNames()
        {
            Console.Clear();
            List<string> existingHabitNames = new List<string>();

            foreach (Habit habit in tableData)
            { 
                existingHabitNames.Add( habit.Name );
            }

            return existingHabitNames;
        }

        private void PrintAllExistingHabitNames(List<string> habits)
        {
            int counter = 0;
            foreach (Habit habit in tableData)
            {
                counter++;
                Console.WriteLine($"{counter}. {habit.Name}");
            }
        }
    }
}
