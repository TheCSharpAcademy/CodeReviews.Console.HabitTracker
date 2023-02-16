using DataBaseLibrary;
using Microsoft.Data.Sqlite;
using System;

namespace HabitsLibrary;


public class HabitsTable
{
    private static DataBaseCommands dbCommands = new();

    public string? tableName;
    public string? connectionString;
    public HabitsTable(string tableName, string connectionString) 
    {
        this.tableName = tableName;
        this.connectionString = connectionString;
    }
    private class Habit
    {
        public int Id { get; set; }
        public string? HabitTableName { get; set; }
        public string? HabitUnit { get; set; }

    }

    private string? AskForString(string message, bool showError)
    {
        if (showError) Console.WriteLine("Invalid Input.");
        else Console.WriteLine(message);

        Console.WriteLine("Use only letters, numbers and spaces");

        char c = default;
        string returnString = Console.ReadLine();
        if (returnString == null) return returnString;

        if ((returnString.All(c => Char.IsLetterOrDigit(c)) || c == ' ') && returnString != "")
            return returnString;
        else
            return null;
    }

    private string? TransformToTableName(string habitName) { return $"[{habitName}]"; }

    public bool CheckForHabitNameInTable(string testTableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 2 FROM " +
                tableName +
                $" WHERE HabitTableName = '{testTableName}')";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }

    public void InsertNewHabit(string habitName, string habitUnit)
    {
       string habitTableName = TransformToTableName(habitName);
       dbCommands.Insert(tableName, habitTableName, habitUnit);
       dbCommands.CreateHabitTable(habitTableName);
    }
}