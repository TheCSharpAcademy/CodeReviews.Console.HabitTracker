﻿using DataBaseLibrary;
using Microsoft.Data.Sqlite;
using System;

namespace HabitsLibrary;


public class MainTable
{
    private static DataBaseCommands dbCommands = new();

    public string? tableName;
    public string? connectionString;
    public MainTable(string tableName, string connectionString) 
    {
        this.tableName = tableName;
        this.connectionString = connectionString;
    }
    private class Habit
    {
        public int Id { get; set; }
        public string? TableName { get; set; }
        public string? Unit { get; set; }

    }

    private string? TransformToTableName(string name) { return $"[{name}]"; }

    public bool CheckForHabitName(string testName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 2 FROM " +
                tableName +
                $" WHERE HabitTableName = '{TransformToTableName(testName)}')";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }

    public void InsertNew(string name, string unit)
    {
       string? tableName = TransformToTableName(name);
       dbCommands.Insert(this.tableName, tableName, unit);
       dbCommands.CreateSubTable(tableName);
    }
}