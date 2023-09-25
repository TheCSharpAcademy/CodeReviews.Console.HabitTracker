using System;
using System.Data;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.Paul_W_Saltzman
{
    internal static class Data
    {
        private static string connectionString = @"Data Source=habit-Tracker.db";

        internal static void Init()
        {


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit (
                habit_id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                unit_id INTEGER,
                FOREIGN KEY(unit_id) REFERENCES unit_type(unit_id)
                )";
                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS unit_type (
                unit_id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT
                )";
                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS logged_habit (
                logged_id INTEGER PRIMARY KEY AUTOINCREMENT,
                habit_id INTEGER,
                unit_id INTEGER,
                date STRING,
                quantity INTEGER,
                FOREIGN KEY(habit_id) REFERENCES habit(habit_id),
                FOREIGN KEY(unit_id) REFERENCES unit_type(unit_id)
                )";
                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                @"CREATE VIEW IF NOT EXISTS habit_view AS
                SELECT logged_habit.logged_id as id, 
                habit.name AS habit_name,
                habit.habit_id AS habit_id,
                logged_habit.quantity As quantity,
                unit_type.name AS unit_name,
                unit_type.unit_id As unit_id, 
                logged_habit.date AS date
                FROM logged_habit
                INNER JOIN habit ON logged_habit.habit_id = habit.habit_id
                INNER JOIN unit_type ON logged_habit.unit_id = unit_type.unit_id";
                tableCmd.ExecuteNonQuery();
                //I need to pre load some data so I need a way to keep track of the version state so i don't load data multiple times.
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS settings (
                setting_id INTEGER PRIMARY KEY AUTOINCREMENT,
                version INTEGER,
                test_mode BOOLEAN, 
                theme INTEGER)";

                tableCmd.ExecuteNonQuery();
                connection.Close();
                Settings settings = LoadData();
                Settings.SetTheme(settings);
            }

        }
        internal static Settings LoadData()
        {
            Settings settings = GetSettings();
            using (var connection = new SqliteConnection(connectionString))
            {

                if (settings.Version < 1)//So I can switch version and add more data if needed just add more if statements as needed.
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    try
                    {
                        //loading habit units
                        ExecuteCommand(connection, "INSERT INTO unit_type (name) VALUES ('Oz')");
                        ExecuteCommand(connection, "INSERT INTO unit_type (name) VALUES ('Minutes')");
                        ExecuteCommand(connection, "INSERT INTO unit_type (name) VALUES ('Times')");
                        ExecuteCommand(connection, "INSERT INTO unit_type (name) VALUES ('Miles')");
                        ExecuteCommand(connection, "INSERT INTO unit_type (name) VALUES ('Reps')");
                        //loading some good habits  Habits are not tied to units.  This is by design.  A user could mess up and enter 15 miles of drinking water, but they can always edit the record.
                        ExecuteCommand(connection, "INSERT INTO habit (name,unit_id) VALUES ('Drink Water','1')");
                        ExecuteCommand(connection, "INSERT INTO habit (name,unit_id) VALUES ('Walk','4')");
                        ExecuteCommand(connection, "INSERT INTO habit (name,unit_id) VALUES ('Run','4')");
                        ExecuteCommand(connection, "INSERT INTO habit (name,unit_id) VALUES ('Exercise','5')");
                        ExecuteCommand(connection, "INSERT INTO habit (name,unit_id) VALUES ('Brush Teeth','3')");
                        ExecuteCommand(connection, "INSERT INTO habit (name,unit_id) VALUES ('Learn Programming','2')");
                        ExecuteCommand(connection, "UPDATE settings SET version = 1 WHERE setting_id = 1");
                       
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }

        
            }
            return settings;
        }

        internal static void ExecuteCommand(SqliteConnection connection, string commandText)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} rows affected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                }
            }
        }


        internal static List<LoggedHabitView> LoadLoggedHabits()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM habit_view";
                List<LoggedHabitView> loggedHabitList = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        loggedHabitList.Add(
                        new LoggedHabitView
                        {
                            LoggedId = reader.GetInt32(0),
                            HabitName = reader.GetString(1),
                            HabitId = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            UnitName = reader.GetString(4),
                            UnitId = reader.GetInt32(5),
                            Date = DateOnly.ParseExact(reader.GetString(6), "MM-dd-yyyy")
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
                return loggedHabitList;
            }

        }

        internal static void LogHabbit(int habitID, int unitID, DateOnly dateInput, int numberInput)
        {
            string formattedDate = dateInput.ToString("MM-dd-yyyy");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"INSERT INTO logged_habit (habit_id, unit_id, date, quantity) VALUES ({habitID},{unitID},'{formattedDate}',{numberInput})";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows added.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($@"habitID: {habitID}  unitID: {unitID}  formattedDate:  {formattedDate}  numberInput: {numberInput}");
                    Console.WriteLine(exception);
                }
                connection.Close();
            }
        }
        internal static void UpdateLoggedHabit(int loggedID, int habitID, int unitID, DateOnly dateInput, int numberInput)
        {
            string formattedDate = dateInput.ToString("MM-dd-yyyy");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"Update logged_habit SET 
                                                        habit_id = {habitID}, 
                                                        unit_id = {unitID}, 
                                                        date = '{formattedDate}', 
                                                        quantity = {numberInput}
                                                        WHERE logged_id = '{loggedID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();
            }
        }
        internal static void DeleteLoggedHabit(int logedHabitID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"DELETE FROM logged_habit WHERE logged_id = '{logedHabitID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();

            }
        }


        internal static void UpdateHabit(int habitID, string habitName)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"Update habit SET name = '{habitName}' WHERE habit_id = '{habitID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();
            }

        }
        internal static void DeleteHabit(int HabitID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"DELETE FROM habit WHERE habit_id = '{HabitID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();

            }
        }
        internal static List<Habits> LoadHabits()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM habit";
                List<Habits> habitList = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        habitList.Add(
                        new Habits
                        {
                            HabitId = reader.GetInt32(0),
                            HabitName = reader.GetString(1),
                            UnitId = reader.GetInt32(2),
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
                return habitList;
            }
        }
        internal static void CreateHabit(string newHabit,int unitType)
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"INSERT INTO habit (name,unit_id) VALUES ('{newHabit}','{unitType}')";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows added.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }


                connection.Close();



            }
        }


        internal static void UpdateUnitType(int unitTypeID, string unitType)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"Update unit_type SET name = '{unitType}' WHERE unit_id = '{unitTypeID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();
            }

        }
        internal static void DeleteUnitType(int UnitTypeID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"DELETE FROM unit_type WHERE unit_id = '{UnitTypeID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();

            }
        }
        internal static List<UnitType> LoadUnits()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM unit_type";
                List<UnitType> unitList = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        unitList.Add(
                        new UnitType
                        {
                            UnitTypeId = reader.GetInt32(0),
                            UnitName = reader.GetString(1),
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
                return unitList;
            }
        }
        internal static void CreateUnitType(string newUnit)
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"INSERT INTO unit_type (name) VALUES ('{newUnit}')";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows added.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }


                connection.Close();



            }
        }
        internal static string GetUnitName(int unitId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {

                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $@"SELECT * FROM unit_type WHERE unit_id = '{unitId}'";
                UnitType unitType = new UnitType();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.Read())
                {
                    unitType.UnitTypeId = reader.GetInt32(0);
                    unitType.UnitName = reader.GetString(1);
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
                return unitType.UnitName;
            }
        }


        internal static Settings GetSettings()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                Settings settings = new Settings();
                bool loaded = false;
                while (!loaded)
                {
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"SELECT * FROM settings WHERE setting_id = 1";

                    SqliteDataReader reader = tableCmd.ExecuteReader();
                    if (reader.Read())// Check if there are rows to read
                    {
                        settings.ID = reader.GetInt32(0);
                        settings.Version = reader.GetInt32(1);
                        settings.TestMode = reader.GetBoolean(2);
                        settings.Theme = reader.GetInt32(3);
                        loaded = true;
                    }

                    else
                    {
                        tableCmd = connection.CreateCommand();
                        tableCmd.CommandText =
                        $"INSERT INTO settings (setting_id,version,test_mode,theme) VALUES (1,0,true,1)";
                        tableCmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
                return settings;

            }
        }
        internal static void ToggleTest(Settings settings)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                if (settings.TestMode) 
                { 
                    settings.TestMode = false;
                    tableCmd.CommandText = $@"Update settings SET test_mode = False WHERE setting_id = '1'";
                }
                else
                {
                    settings.TestMode = true;
                    tableCmd.CommandText = $@"Update settings SET test_mode = True WHERE setting_id = '1'";

                }                
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();
            }
        }
        
        internal static void SaveTheme(int theme)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"Update settings SET theme = '{theme}' WHERE setting_id = '1'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();
            }

        }

    }
}
