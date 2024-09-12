using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;
namespace habit_tracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main(string[] args)
        {

            Repository.CreateTable();
            Repository.GetUserInput();
        }

    }
}

public class CsharpLessons
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}