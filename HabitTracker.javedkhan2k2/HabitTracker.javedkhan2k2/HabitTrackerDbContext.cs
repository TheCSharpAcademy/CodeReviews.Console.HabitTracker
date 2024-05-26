using System.Data;
using System.Globalization;
using HabitTracker.Models;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

internal class HabitTrackerDbContext
{
    private string ConnectionString { get; init; } = default!;

    public HabitTrackerDbContext(string DatabasePath)
    {
        ConnectionString = $"Data Source = {DatabasePath}";
        InitDatabase(DatabasePath);
    }

    private void InitDatabase(string DbPath)
    {
        // Create Database if one does'nt exits
        if (!File.Exists(DbPath))
        {
            Console.WriteLine($"Creating {DbPath}.");
            // Create the Table
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            var sql = @"
                        Create Table Habits(
                            Id INTEGER PRIMARY KEY, 
                            Habit TEXT NOT NULL,
                            Unit Text NOT NULL
                        );
                        Create Table Habitlogs(
                            Id INTEGER PRIMARY KEY, 
                            Date Text NOT NULL,
                            Quantity INTEGER NOT NULL, 
                            HabitId INTEGER,
                            FOREIGN KEY(HabitId) REFERENCES Habits(Id)
                        );";
            using var cmd = new SqliteCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
                // Seed Data
                SeedInitialData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
        }

    }

    // internal void ViewAllRecords()
    // {
    //     using var conn = new SqliteConnection(ConnectionString);
    //     conn.Open();
    //     var sql = @"select 
    //                 h.id, h.habitDesc, t.desc 
    //                 from 
    //                 Habits h
    //                 inner join HabitTypes t on h.habitTypeId = t.id;";
    //     using var cmd = new SqliteCommand(sql, conn);
    //     using var reader = cmd.ExecuteReader();
    //     Console.Clear();
    //     Console.WriteLine("List of Habits");
    //     Console.WriteLine("-----------------------------------------------------------------");
    //     Console.WriteLine("{0,-5} | {1, -15} | {2, -10}", "ID", "Type", "Habit");
    //     Console.WriteLine("-----------------------------------------------------------------");
    //     while (reader.Read())
    //     {
    //         Console.WriteLine("{0,-5} | {1, -15} | {2, -10}", reader["id"], reader["desc"], reader["habitDesc"]);
    //         Console.WriteLine("-----------------------------------------------------------------");
    //     }

    //     Console.WriteLine("Press any key to continue.");
    //     Console.ReadLine();
    // }

    internal void SeedInitialData()
    {
        var random = new Random();

        InsertHabit("Video Game", "Hour"); // Id = 1
        InsertHabit("Reading Books", "Hour"); // Id = 2
        InsertHabit("Drinking Water", "Glass"); // Id = 3
        int habitId;
        int habitQuantity;
        string? date;
        DateTime startDate = new DateTime(2022,1,1);
        int range = (DateTime.Today - startDate).Days; 
        for(int i=0; i<1000; i++)
        {
            habitId = random.Next(1,4);
            habitQuantity = random.Next(1, 13);
            //date = $"{random.Next(2022,2025)}-{random.Next(1,13)}-{random.Next(1, 29)}";
            date = startDate.AddDays(random.Next(range)).ToString("yyyy-MM-dd");
            InsertHabitLog(date, habitQuantity, habitId);
        }
    }

    internal void InsertHabit(string habit, string unit)
    {
        var sql = @"INSERT INTO Habits (Habit, Unit) Values (@habit, @unit)";
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqliteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@habit", habit);
        cmd.Parameters.AddWithValue("@unit", unit);
        cmd.ExecuteNonQuery();
    }

    internal void InsertHabitLog(string date, int quantity, int habitId)
    {
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();

        var sql = @"Insert into Habitlogs (Date, Quantity, HabitId) 
            Values
            (@Date, @Quantity, @HabitId)
        ";
        using var cmd = new SqliteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Date", date);
        cmd.Parameters.AddWithValue("@Quantity", quantity);
        cmd.Parameters.AddWithValue("@HabitId", habitId);
        cmd.ExecuteNonQuery();
    }

    internal List<Habit>? GetAllHabits()
    {
        List<Habit>? habits = new List<Habit>();
        var sql = "select * from Habits";
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqliteCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            habits.Add(new Habit
            {
                Id = Convert.ToInt32(reader["Id"]),
                HabitDescription = reader["Habit"].ToString(),
                Unit = reader["Unit"].ToString()
            });
        }
        return habits;
    }

    internal List<HabitLog>? GetAllHabitLogs()
    {
        List<HabitLog>? habitlogs = new List<HabitLog>();
        var sql = @"select 
                    h.Id, h.Date, h.Quantity, t.Habit, t.Unit 
                    from 
                    Habitlogs h
                    inner join Habits t on h.HabitId = t.Id;";
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqliteCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            habitlogs.Add(new HabitLog
            {
                Id = Convert.ToInt32(reader["Id"]),
                Date = DateTime.ParseExact(reader["Date"].ToString(), "yyyy-MM-dd", new CultureInfo("en-US")),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                HabitDescription = reader["Habit"].ToString(),
                HabitUnit = reader["Unit"].ToString()
            });
        }
        return habitlogs;
    }

    internal int DeleteHabitLog(int id)
    {
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        var sql = @"delete from Habitlogs where id = @id";
        using var cmd = new SqliteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        var result = cmd.ExecuteNonQuery();
        return result;
    }

    internal int UpdateHabitLog(int id, string date, int quantity)
    {
        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        var sql = @"Update Habitlogs 
                    set
                        Date = @Date,
                        Quantity = @Quantity
                    where id = @id";
        using var cmd = new SqliteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@Date", date);
        cmd.Parameters.AddWithValue("@Quantity", quantity);
        var result = cmd.ExecuteNonQuery();
        return result;
    }

    internal List<HabitReport> GenerateYearlyReport()
    {
        List<HabitReport>? report = new List<HabitReport>();
        DateTime today = DateTime.UtcNow;
        DateTime startYear = today.AddMonths(-today.Month+1).AddDays(-today.Day+1);

        using var conn = new SqliteConnection(ConnectionString);
        conn.Open();
        var sql = @"select 
                    t.Habit, t.Unit, SUM(h.Quantity) as Sum
                    from 
                    Habits t
                    inner join Habitlogs h on h.HabitId = t.Id
                    where
                        h.Date >= @StartDate and h.Date
                         <= @EndDate
                    group by t.Id
                    ;";
        using var cmd = new SqliteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@StartDate", startYear.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@EndDate", today.ToString("yyyy-MM-dd"));
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            report.Add(new HabitReport
            {
                HabitDescription = reader["Habit"].ToString(),
                Sum = reader["Sum"].ToString(),
                Unit = reader["Unit"].ToString()
            });
        }
        return report;
    }
}