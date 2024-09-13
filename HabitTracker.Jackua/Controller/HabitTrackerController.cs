using HabitTracker.Jackua.Model;
using HabitTracker.Jackua.View;
using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.Jackua.Controller;

public class HabitTrackerController
{
    static string connectionString = @"Data Source=habit-Tracker.db";

    public static void Run()
    {

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            @"
            PRAGMA foreign_keys = ON;

            CREATE TABLE IF NOT EXISTS habit (
                habitId INTEGER PRIMARY KEY AUTOINCREMENT,
                habitName TEXT
            );

            INSERT INTO habit
            SELECT *
            FROM (
                    VALUES (1, 'Drinking Water'),
                        (2, 'Eating Fruit'),
                        (3, 'Jogging')
            )
            WHERE NOT EXISTS (
                SELECT * FROM habit
            );

            CREATE TABLE IF NOT EXISTS record (
                recordId INTEGER PRIMARY KEY AUTOINCREMENT,
                recordHabit INTEGER,
                date TEXT,
                quantity INTEGER,
                FOREIGN KEY(recordHabit) REFERENCES habit(habitId)
            );

            INSERT INTO record
            SELECT *
            FROM (
                VALUES (1, 2, '05-05-24', 5),
                        (2, 3, '05-07-24', 23),
                        (3, 1, '05-08-24', 3),
                        (4, 3, '05-09-24', 2),
                        (5, 2, '05-10-24', 3),
                        (6, 2, '05-11-24', 2),
                        (7, 1, '05-12-24', 12)
            )
            WHERE NOT EXISTS (
                SELECT * FROM record
            );
            ";

        tableCmd.ExecuteNonQuery();

        connection.Close();

        GetUserInput();
    }

    private static void GetUserInput()
    {
        Console.Clear();
        while (true)
        {
            Console.Clear();

            MenuView.MainMenu();

            string command = Console.ReadLine();

            Console.Clear();

            switch (command)
            {
                case "0":
                    MenuView.GoodBye();
                    Environment.Exit(0);
                    break;
                case "1":
                    GetAllRecords();
                    MenuView.Continue();
                    Console.ReadLine();
                    break;
                case "2":
                    InsertRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "5":
                    GetAllHabits();
                    MenuView.Continue();
                    Console.ReadLine();
                    break;
                case "6":
                    InsertHabit();
                    break;
                case "7":
                    DeleteHabit();
                    break;
                case "8":
                    UpdateHabit();
                    break;
                case "9":
                    GetSummary();
                    MenuView.Continue();
                    Console.ReadLine();
                    break;
                default:
                    MenuView.InvalidCommand();
                    break;
            }
        }

    }

    private static void GetAllRecords()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var queryCmd = connection.CreateCommand();
        queryCmd.CommandText =
                "SELECT * FROM habit;";

        Dictionary<int, string> queryData = new();

        SqliteDataReader queryReader = queryCmd.ExecuteReader();

        if (queryReader.HasRows)
        {
            while (queryReader.Read())
            {
                queryData.Add(queryReader.GetInt32(0), queryReader.GetString(1));
            }
        }

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            "SELECT * FROM record;";

        List<RecordModel> tableData = new();

        SqliteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                    new RecordModel
                    {
                        RecordId = reader.GetInt32(0),
                        HabitName = queryData[reader.GetInt32(1)],
                        Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-us")),
                        Quantity = reader.GetInt32(3)
                    });
            }
        }
        else
        {
            MenuView.NoRows();
        }

        connection.Close();

        MenuView.DashLines();
        foreach (var rm in tableData)
        {
            MenuView.DisplayData(rm);
        }
        MenuView.DashLines();
    }

    private static void GetAllHabits()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            "SELECT * FROM habit;";

        List<HabitModel> tableData = new();

        SqliteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                    new HabitModel
                    {
                        HabitId = reader.GetInt32(0),
                        HabitName = reader.GetString(1)
                    });
            }
        }
        else
        {
            MenuView.NoRows();
        }

        connection.Close();

        MenuView.DashLines();
        foreach (var hm in tableData)
        {
            MenuView.DisplayData(hm);
        }
        MenuView.DashLines();
    }

    private static void GetSummary()
    {
        Console.Clear();
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @" 
            SELECT (
                    SELECT habitname 
                    FROM habit 
                    where recordHabit = habitId
                    ), 
                SUM(quantity) 
                FROM record 
                GROUP BY recordHabit
            ;";

        Dictionary<string, int> tableData = new();

        SqliteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                    reader.GetString(0),
                    reader.GetInt32(1)
                );
            }
        }
        else
        {
            MenuView.NoRows();
        }

        connection.Close();

        MenuView.DashLines();
        foreach (string key in tableData.Keys)
        {
            MenuView.DisplayData(key, tableData[key]);
        }
        MenuView.DashLines();
    }

    private static void InsertRecord()
    {
        Console.Clear();
        
        GetAllHabits();
        MenuView.HabitIdRequest();
        int habitId = GetNumberInput();
        if (habitId == 0) return;

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var checkCmd = connection.CreateCommand();

        checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM habit WHERE habitId = @habitId)";
        checkCmd.Parameters.Add(new SqliteParameter("@habitId", habitId));
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.Clear();
            MenuView.DoesNotExist(habitId, "Habit");
            MenuView.Continue();
            Console.ReadLine();
            connection.Close();
            return;
        }

        string date = GetDateInput();
        if (date == "0") return;

        MenuView.QuantityRequest();
        int quantity = GetNumberInput();
        if (quantity == 0) return;

        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            "INSERT INTO record(date, recordHabit, quantity) VALUES(@date, @habitId, @quantity)";
        tableCmd.Parameters.Add(new SqliteParameter("@date", date));
        tableCmd.Parameters.Add(new SqliteParameter("@habitId", habitId));
        tableCmd.Parameters.Add(new SqliteParameter("@quantity", quantity));
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    private static void InsertHabit()
    {
        Console.Clear();

        MenuView.HabitRequest();
        string habitName = Console.ReadLine();
        if (habitName == "0") return;

        using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                "INSERT INTO habit(habitName) VALUES(@habitName)";
            tableCmd.Parameters.Add(new SqliteParameter("@habitName", habitName));
            tableCmd.ExecuteNonQuery();

            connection.Close();
    }

    private static void DeleteRecord()
    {
        Console.Clear();
        GetAllRecords();

        MenuView.DeleteId("record");
        var recordId = GetNumberInput();
        if (recordId == 0) return;

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = "DELETE FROM record WHERE recordId = @recordId";
        tableCmd.Parameters.Add(new SqliteParameter("@recordId", recordId));

        int rowCount = tableCmd.ExecuteNonQuery();

        if (rowCount == 0)
        {
            MenuView.DoesNotExist(recordId, "Record");
        } 
        else
        {
            MenuView.Deleted(recordId, "Record");
        }

        connection.Close();
        MenuView.Continue();
        Console.ReadLine();
    }

    private static void DeleteHabit()
    {
        Console.Clear();
        GetAllHabits();

        MenuView.DeleteId("habit");
        var habitId = GetNumberInput();
        if (habitId == 0) return;

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = "DELETE FROM habit WHERE habitId = @habitId";
        tableCmd.Parameters.Add(new SqliteParameter("@habitId", habitId));

        try
        {
            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                MenuView.DoesNotExist(habitId, "Habit");
            }
            else
            {
                MenuView.Deleted(habitId, "Habit");
            }
        }
        catch (SqliteException e)
        {
            if (e.SqliteExtendedErrorCode == SQLitePCL.raw.SQLITE_CONSTRAINT_FOREIGNKEY)
            {
                MenuView.ForeignKey(habitId);
            }
        }
        connection.Close();
        MenuView.Continue();
        Console.ReadLine();
    }

    private static void UpdateRecord()
    {
        Console.Clear();
        GetAllRecords();
        GetAllHabits();

        MenuView.UpdateId("record");
        var recordId = GetNumberInput();
        if (recordId == 0) return;

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var checkCmd = connection.CreateCommand();

        checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM record WHERE recordId = @recordId)";
        checkCmd.Parameters.Add(new SqliteParameter("@recordId", recordId));

        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            MenuView.DoesNotExist(recordId, "Record");
            MenuView.Continue();
            Console.ReadLine();
            connection.Close();
            return;
        }

        MenuView.UpdateId("habit");
        int habitId = GetNumberInput();

        checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM habit WHERE habitId = @habitId)";
        checkCmd.Parameters.Add(new SqliteParameter("@habitId", habitId));
        checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (habitId == 0)
        {
            connection.Close();
            return;
        }
        else if (checkQuery == 0)
        {
            MenuView.DoesNotExist(habitId, "Habit");
            MenuView.Continue();
            Console.ReadLine();
            connection.Close();
            return;
        }

        string date = GetDateInput();
        if (date == "0") 
        {
            connection.Close();
            return; 
        }

        MenuView.QuantityRequest();
        int quantity = GetNumberInput();
        if (quantity == 0)
        {
            connection.Close();
            return;
        }

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "Update record SET recordhabit = @habitId, date = @date, quantity = @quantity WHERE recordId = @recordId";
        tableCmd.Parameters.Add(new SqliteParameter("@date", date));
        tableCmd.Parameters.Add(new SqliteParameter("@habitId", habitId));
        tableCmd.Parameters.Add(new SqliteParameter("@recordId", recordId));
        tableCmd.Parameters.Add(new SqliteParameter("@quantity", quantity));
        tableCmd.ExecuteNonQuery();

        connection.Close();
        MenuView.Updated(recordId, "record");
        MenuView.Continue();
        Console.ReadLine();
    }

    private static void UpdateHabit()
    {
        Console.Clear();
        GetAllHabits();

        MenuView.UpdateId("habit");
        var habitId = GetNumberInput();
        if (habitId == 0) return;

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var checkCmd = connection.CreateCommand();

        checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM habit WHERE habitId = @habitId)";
        checkCmd.Parameters.Add(new SqliteParameter("@habitId", habitId));

        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            MenuView.DoesNotExist(habitId, "Habit");
            MenuView.Continue();
            Console.ReadLine();
            connection.Close();
            return;
        }

        MenuView.HabitRequest();
        string habitName = Console.ReadLine();
        if (habitName == "0")
        {
            connection.Close();
            return;
        }

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "Update habit SET habitName = @habitName WHERE habitId = @habitId";
        tableCmd.Parameters.Add(new SqliteParameter("@habitName", habitName));
        tableCmd.Parameters.Add(new SqliteParameter("@habitId", habitId));
        tableCmd.ExecuteNonQuery();

        connection.Close();
        MenuView.Updated(habitId, "habit");
        MenuView.Continue();
        Console.ReadLine();
    }

    private static string GetDateInput()
    {
        MenuView.DateRequest();

        string dateInput = Console.ReadLine();

        if (dateInput == "0") return "0";

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            MenuView.InvalidDate();
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    private static int GetNumberInput()
    {
        string numberInput = Console.ReadLine();

        if (numberInput == "0") return 0;

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            MenuView.InvalidNumber();
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
}