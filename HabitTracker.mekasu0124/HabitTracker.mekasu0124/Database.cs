using System.Data.SQLite;
using HabitTracker.Models;

namespace HabitTracker;

public class Database
{
    public static void CreateDatabase()
    {
        Console.WriteLine("Creating Database");
        SQLiteConnection.CreateFile("habits.db");

        using (var sqlite = new SQLiteConnection(@"Data Source=habits.db"))
        {
            sqlite.Open();
            string sql = @"CREATE TABLE IF NOT EXISTS 
                            [habits] (
                            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            [Name] VARCHAR(70) NULL, 
                            [Date] VARCHAR(50) NULL, 
                            [Count] INTEGER NULL, 
                            [Description] VARCHAR(2048))";
            SQLiteCommand command = new SQLiteCommand(sql, sqlite);
            command.ExecuteNonQuery();
        }
        
        Console.WriteLine("New Database Created");
    }

    public static void SaveEntry(Habit newHabit)
    {
        using (SQLiteConnection sqlite = new SQLiteConnection(@"Data Source=habits.db"))
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlite))
            {
                sqlite.Open();

                cmd.CommandText = "INSERT INTO habits(Name, Date, Count, Description) VALUES ($Name, $Date, $Count, $Description)";
                cmd.Parameters.Add(new SQLiteParameter("$Name", newHabit.Name));
                cmd.Parameters.Add(new SQLiteParameter("$Date", newHabit.Date.ToString()));
                cmd.Parameters.Add(new SQLiteParameter("$Count", newHabit.Count));
                cmd.Parameters.Add(new SQLiteParameter("$Description", newHabit.Description));

                cmd.ExecuteNonQuery();

                sqlite.Close();
            }
        }
    }

    public static void UpdateEntry(int selectedIndex, Habit newHabit)
    {
        using (SQLiteConnection sqlite = new SQLiteConnection(@"Data Source=habits.db"))
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlite))
            {
                using (SQLiteCommand cmd2 = new SQLiteCommand(sqlite))
                {
                    sqlite.Open();

                    cmd.CommandText = "SELECT * FROM habits WHERE Id=@CurrentId";
                    cmd.Parameters.Add(new SQLiteParameter("@CurrentId", selectedIndex));
                    cmd.ExecuteNonQuery();

                    cmd2.CommandText = @"UPDATE habits SET 
                                            Name = @NewName,
                                            Date = @NewDate,
                                            Count = @NewCount,
                                            Description = @NewDescription
                                            WHERE Id=@CurrentId";
                    cmd2.Parameters.Add(new SQLiteParameter("@CurrentId", selectedIndex));
                    cmd2.Parameters.Add(new SQLiteParameter("@NewName", newHabit.Name));
                    cmd2.Parameters.Add(new SQLiteParameter("@NewDate", newHabit.Date));
                    cmd2.Parameters.Add(new SQLiteParameter("@NewCount", newHabit.Count));
                    cmd2.Parameters.Add(new SQLiteParameter("@NewDescription", newHabit.Description));

                    cmd2.ExecuteNonQuery();

                    sqlite.Close();
                }
            }
        }
    }

    public static void DeleteEntry(int selectedId)
    {
        using (SQLiteConnection sqlite = new SQLiteConnection(@"Data Source=habits.db"))
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlite))
            {
                sqlite.Open();

                cmd.CommandText = "DELETE FROM habits WHERE Id=@selectedId";
                cmd.Parameters.Add(new SQLiteParameter("@selectedId", selectedId));
                cmd.ExecuteNonQuery();

                sqlite.Close();
            }
        }
    }

    public static List<Habit> GetHabits()
    {
        List<Habit> habits = new();

        using (SQLiteConnection sqlite = new SQLiteConnection(@"Data Source=habits.db"))
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlite))
            {
                SQLiteDataReader reader;

                sqlite.Open();

                cmd.CommandText = "SELECT * FROM habits";

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = int.Parse(reader["Id"].ToString());
                    string name = reader["Name"].ToString();
                    DateTime date = DateTime.Parse(reader["Date"].ToString());
                    int count = int.Parse(reader["Count"].ToString());
                    string description = reader["Description"].ToString();

                    Habit habit = new()
                    {
                        Id = id,
                        Name = name,
                        Date = date,
                        Count = count,
                        Description = description
                    };

                    habits.Add(habit);
                }

                sqlite.Close();
            }
        }
        return habits;
    }

    public static Habit GetSelectedHabit(int index)
    {
        Habit selectedHabit = new();

        using (SQLiteConnection sqlite = new SQLiteConnection(@"Data Source=habits.db"))
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlite))
            {
                SQLiteDataReader reader;

                sqlite.Open();

                cmd.CommandText = "SELECT * FROM habits WHERE Id=@SelectedID";
                cmd.Parameters.Add(new SQLiteParameter("@SelectedID", index));

                reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        int id = int.Parse(reader["Id"].ToString());
                        string name = reader["Name"].ToString();
                        DateTime date = DateTime.Parse(reader["Date"].ToString());
                        int count = int.Parse(reader["Count"].ToString());
                        string description = reader["Description"].ToString();

                        selectedHabit = new()
                        {
                            Id = id,
                            Name = name,
                            Date = date,
                            Count = count,
                            Description = description
                        };
                    }

                    sqlite.Close();
                }
                catch (SQLiteException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        return selectedHabit;
    }
}