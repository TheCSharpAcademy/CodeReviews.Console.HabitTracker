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

    private bool CheckForHabitNameInTable(string testTableName)
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

    public bool CheckForHabitByIndex(int index)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM " +
                tableName +
                $" WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            
            connection.Close();
            if (checkQuery == 0) return false;
            else return true;
        }
    }

    public void InsertNewHabit()
    {
        string habitName;
        string habitTableName;
        string habitUnit;
        bool showError = false;

        do
        {
            habitName = AskForString("Write you habit's name", showError);
            showError= true;
        }
        while (habitName == null);
        habitTableName = TransformToTableName(habitName);

        if (CheckForHabitNameInTable(habitTableName))
        {
            Console.WriteLine("Habit already exists. Press any key and ENTER to return to the menu");
            Console.ReadLine();
            return;
        }

        showError = false;
        do
        {
            habitUnit = AskForString("Write you habit's units", showError);
            showError = true;
        }
        while (habitUnit == null);

        dbCommands.Insert(tableName, habitTableName, habitUnit);
        dbCommands.Initialization(habitTableName);
    }

    public void InsertNewHabit(string habitName, string habitUnit)
    {
       string habitTableName = TransformToTableName(habitName);
       dbCommands.Insert(tableName, habitTableName, habitUnit);
       dbCommands.CreateHabitTable(habitTableName);
    }
    public void DeleteHabitByIndex(int index)
    {
        dbCommands.DeleteByIndex(index, tableName);
    }
    public void ViewAll()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM " + tableName;

            List<Habit> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new Habit
                    {
                        Id = reader.GetInt32(0),
                        HabitTableName = reader.GetString(1),
                        HabitUnit = reader.GetString(2)
                    }); ;
                }
            }
            else { Console.WriteLine("Empty"); }

            connection.Close();

            Console.WriteLine("-----------------------------\n");
            foreach (var dw in tableData)
            {
                string habitTableName_display = dw.HabitTableName.TrimEnd(']').TrimStart('[');
                Console.WriteLine($"{dw.Id} - {habitTableName_display} - Unit: {dw.HabitUnit}");
            }
            Console.WriteLine("\n-----------------------------");
        }
    }
}