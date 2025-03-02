using Microsoft.Data.Sqlite;
using System;

namespace HabitTracker.StressedBread
{
    internal class DatabaseInput
    {
        Helpers helpers = new();
        DatabaseService databaseService = new();
        internal void CreateTable()
        {
            string createHabit = @"CREATE TABLE IF NOT EXISTS habits ( 
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                HabitName TEXT,
                                Unit TEXT
                                )";

            string createHabitData = @"CREATE TABLE IF NOT EXISTS habit_data ( 
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                HabitID INTEGER,
                                Date TEXT,
                                Quantity INTEGER,
                                FOREIGN KEY (HabitID) REFERENCES habits(ID) ON DELETE CASCADE
                                )";

            databaseService.ExecuteCommand(createHabit);
            databaseService.ExecuteCommand(createHabitData);
        }
        internal void InsertHabit()
        {
            Console.Clear();

            string? name = helpers.ValidateString("Input the name of the habit.");
            string? unit = helpers.ValidateString("Input the unit of measurement.");

            string commandText = @"INSERT INTO habits (HabitName, Unit)
                       VALUES (@name, @unit)
                       ";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"name", name),
                new SqliteParameter(@"unit", unit)
            };

            databaseService.ExecuteCommand(commandText, parameters);
            Console.WriteLine("Succefully inserted! Press any key to return to menu.");
            Console.ReadKey();
        }
        internal void InsertHabitData()
        {
            Console.Clear();

            int index = helpers.ValidateInt("Choose which habit you want to insert data into by ID.");
            if (!HabitExists(index, "habits")) return;

            string? date = helpers.GetDateInput();
            int quantity = helpers.ValidateInt("Insert the quantity.");

            string commandText = @"INSERT INTO habit_data (HabitID, Date, Quantity)
                       VALUES (@index, @date, @quantity)
                       ";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"index", index),
                new SqliteParameter(@"date", date),
                new SqliteParameter(@"quantity", quantity)
            };

            databaseService.ExecuteCommand(commandText, parameters);
            Console.WriteLine("Succefully inserted! Press any key to return to menu.");
            Console.ReadKey();
        }
        internal void Update()
        {
            Console.Clear();

            Console.WriteLine("Press 1 to update habits or press 2 to edit habit data.");
            ConsoleKeyInfo input = Console.ReadKey();

            int index = helpers.ValidateInt("\nChoose which row you want to update."); 

            switch (input.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    UpdateHabit(index);
                    break;

                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    UpdateHabitData(index);
                    break;

                default:
                    Console.WriteLine("Invalid input! Press any key to return to menu.");
                    Console.ReadKey();
                    break;
            }                   
        }
        internal void Delete()
        {
            Console.Clear();

            int index = helpers.ValidateInt("Choose which row you want to update.");
            if (!HabitExists(index, "habits")) return;

            string deleteText = @"DELETE FROM drinking_water
                                  WHERE ID = @index";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"index", index),
            };

            databaseService.ExecuteCommand(deleteText, parameters);
            Console.WriteLine("Succefully deleted! Press any key to return to menu.");
            Console.ReadKey();
        }
        //Implement view and select filters
        internal void View()
        {
            Console.Clear();

            string commandText = @"SELECT * FROM drinking_water
                                   ";

            using (SqliteDataReader reader = databaseService.ExecuteRead(commandText))
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        var columnValue = reader.GetValue(i);

                        Console.Write($@"{columnName}: {columnValue} ");
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
        internal void UpdateHabit(int index)
        {
            if (!HabitExists(index, "habits")) return;

            string? name = helpers.ValidateString("Update the name of the habit.");
            string? unit = helpers.ValidateString("Update the unit of measurement.");

            string commandText = @"UPDATE habits
                                  SET HabitName = @name, Unit = @unit
                                  WHERE ID = @index
                                  ";

            List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"index", index),
                new SqliteParameter(@"name", name),
                new SqliteParameter(@"unit", unit)
            };

            databaseService.ExecuteCommand(commandText, parameters);
            Console.WriteLine("Succefully updated! Press any key to return to menu.");
            Console.ReadKey();
        }
        internal void UpdateHabitData(int index)
        {
            if (!HabitExists(index, "habit_data")) return;

            string? date = helpers.GetDateInput();
            string? quantity = helpers.ValidateString("Update the quantity of measurement.");

            string commandText = @"UPDATE habit_data
                                  SET Date = @date, Quantity = @quantity
                                  WHERE ID = @index
                                  ";

            List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"index", index),
                new SqliteParameter(@"date", date),
                new SqliteParameter(@"quantity", quantity)
            };

            databaseService.ExecuteCommand(commandText, parameters);
            Console.WriteLine("Succefully updated! Press any key to return to menu.");
            Console.ReadKey();
        }
        private bool HabitExists(int index, string tableName)
        {
            string selectText = $@"SELECT ID FROM {tableName}
                                  WHERE ID = @index";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"index", index),
            };

            using (SqliteDataReader reader = databaseService.ExecuteRead(selectText, parameters))
            {
                if (reader.Read())
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("Specified ID doesn't exist! Press any key to return to menu.");
                    Console.ReadKey();
                    return false;
                }

            }
        }
    }
}
