using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Mateusz_Platek;

public static class Database
{
    private static string connectionString = @"Data Source=..\..\..\database.db";
    private static SqliteConnection sqliteConnection = new SqliteConnection(connectionString);
    
    public static List<HabitLog> GetHabitLogs()
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = @"SELECT logID, date, quantity, habit, unit FROM logs 
                                    JOIN habits ON logs.habitID = habits.habitID 
                                    JOIN units ON habits.unitID = units.unitID";

        List<HabitLog> habitLogs = new List<HabitLog>();
        
        SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
        if (sqliteDataReader.HasRows)
        {
            while (sqliteDataReader.Read())
            {
                HabitLog habitLog = new HabitLog(
                    sqliteDataReader.GetInt32(0), 
                    DateTime.ParseExact(sqliteDataReader.GetString(1), "dd-MM-yyyy", CultureInfo.CurrentCulture),
                    sqliteDataReader.GetInt32(2),
                    sqliteDataReader.GetString(3),
                    sqliteDataReader.GetString(4)
                );
                habitLogs.Add(habitLog);
            }
        }
        sqliteConnection.Close();

        return habitLogs;
    }
    
    public static List<Habit> GetHabits()
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = @"SELECT habitID, habit, unit FROM habits 
                                    JOIN units ON habits.unitID = units.unitID";

        List<Habit> habits = new List<Habit>();
        
        SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
        if (sqliteDataReader.HasRows)
        {
            while (sqliteDataReader.Read())
            {
                Habit habit = new Habit(
                    sqliteDataReader.GetInt32(0), 
                    sqliteDataReader.GetString(1),
                    sqliteDataReader.GetString(2)
                );
                habits.Add(habit);
            }
        }
        sqliteConnection.Close();

        return habits;
    }

    public static List<Unit> GetUnits()
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = "SELECT unitID, unit FROM units";

        List<Unit> units = new List<Unit>();
        
        SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
        if (sqliteDataReader.HasRows)
        {
            while (sqliteDataReader.Read())
            {
                Unit unit = new Unit(
                    sqliteDataReader.GetInt32(0), 
                    sqliteDataReader.GetString(1)
                );
                units.Add(unit);
            }
        }
        sqliteConnection.Close();

        return units;
    }

    public static void AddUnit(string unit)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"INSERT INTO units (unit) VALUES ('{unit}')";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void AddHabit(string habit, int unitId)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"INSERT INTO habits (habit, unitID) VALUES ('{habit}', {unitId})";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void AddHabitLog(DateTime date, int quantity, int habitId)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        string day = date.Day.ToString("00");
        string month = date.Month.ToString("00");
        string year = date.Year.ToString("0000");
        string formattedDate = $"{day}-{month}-{year}";
        sqliteCommand.CommandText = $"INSERT INTO logs (date, quantity, habitID) VALUES ('{formattedDate}', {quantity}, {habitId})";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void DeleteUnit(int unitId)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"DELETE FROM units WHERE unitID = {unitId}";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void DeleteHabit(int habitId)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"DELETE FROM habits WHERE habitID = {habitId}";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void DeleteHabitLog(int habitLogId)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"DELETE FROM logs WHERE logID = {habitLogId}";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void UpdateUnit(int unitId, string unit)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"UPDATE units SET unit = '{unit}' WHERE unitID = {unitId}";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void UpdateHabit(int habitId, string habit, int unitId)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"UPDATE habits SET habit = '{habit}', unitID = {unitId} WHERE habitID = {habitId}";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void UpdateHabitLog(int habitLogId, DateTime date, int quantity, int habitId)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        string formattedDate = $"{date.Day}-{date.Month}-{date.Year}";
        sqliteCommand.CommandText = $"UPDATE logs SET date = '{formattedDate}', quantity = {quantity}, habitID = {habitId} WHERE logID = {habitLogId}";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }
}