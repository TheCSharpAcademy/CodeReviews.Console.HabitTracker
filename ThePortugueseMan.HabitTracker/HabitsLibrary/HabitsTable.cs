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
    private string? AskHabitName()
    {
        Console.WriteLine("Please write the name of your habit");
        string habitName = Console.ReadLine();

        return habitName;
    }

    private string? AskHabitUnit()
    {
        Console.WriteLine("Please write the units of your habit");
        string habitUnit = Console.ReadLine();

        return habitUnit;
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
                $" WHERE HabitTableName = {testTableName})";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }

    public void Insert()
    {
        string habitName = AskHabitName();
        string habitTableName = TransformToTableName(habitName);
        if (CheckForHabitNameInTable(habitTableName)) return;

        string habitUnit = AskHabitUnit();

        dbCommands.Insert(tableName, habitTableName, habitUnit);
        dbCommands.Initialization(habitTableName);
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
                Console.WriteLine($"{dw.Id} - {dw.HabitTableName} - Unit: {dw.HabitUnit}");
            }
            Console.WriteLine("\n-----------------------------");
            Console.ReadLine();
        }
    }

}