using HabitTracker.S1m0n32002.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using static HabitTracker.S1m0n32002.Models.Habit;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace HabitTracker.S1m0n32002.Controllers;

public class DbController
{
    const string DbName = "Db.sqlite";
    const string DbTemplate = "SqliteTemplate.sql";

    public DbController()
    {
        CheckAndInit();
    }

    /// <param name="habit"> Habit to save </param>
    /// <inheritdoc cref="SaveHabit(string,DateTime)"/>
    public bool SaveHabit(Habit habit) => SaveHabit(habit.Name,habit.LastOccurrance);

    /// <summary>
    /// Save habit to database
    /// </summary>
    /// <param name="Name"> Name of habit </param>
    /// <param name="Times"> Times of habit </param>
    /// <param name="LastOccurrance"> LastOccurrance of habit </param>
    public bool SaveHabit(string Name, DateTime LastOccurrance)
    {
        string StrCmd;
        
        bool FlagUpdate = false;

        if (FlagUpdate)
        {
            StrCmd = @$"UPDATE [Habits] SET {nameof(LastOccurrance)} = @{nameof(LastOccurrance)} 
                                            WHERE {nameof(Name)} = @{nameof(Name)}";
        }
        else
        {
            StrCmd = @$"INSERT INTO [Habits] ({nameof(Name)},
                                                  {nameof(LastOccurrance)}) 
                                         VALUES (@{nameof(Name)},
                                                 @{nameof(LastOccurrance)})";
        }

        using SqliteCommand cmd = new(StrCmd, Connect());
        var par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Name)}";
        par.Value = Name;
        cmd.Parameters.Add(par);

        par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(LastOccurrance)}";
        par.Value = new DateTimeOffset(LastOccurrance.ToUniversalTime()).ToUnixTimeSeconds();
        cmd.Parameters.Add(par);

        cmd.ExecuteNonQuery();

        return true;
    }

    /// <summary>
    /// Load habit from database
    /// </summary>
    public Habit DeleteHabit(string Name)
    {
        string StrCmd = $"DELETE FROM [Habits] WHERE {nameof(Name)} = @{nameof(Name)}";
        using SqliteCommand cmd = new(StrCmd, Connect());

        var par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Name)}";
        par.Value = Name;
        cmd.Parameters.Add(par);

        Habit habit = new();
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) // Only one habit should be returned
        {
            habit.Name = reader.GetString(0);
            habit.LastOccurrance = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(1)).ToLocalTime().DateTime;
        }
        return habit;
    }

    /// <summary>
    /// Load habit from database
    /// </summary>
    public Habit LoadHabit(string Name)
    {
        string StrCmd = $"SELECT * FROM [Habits] WHERE {nameof(Name)} = @{nameof(Name)}";
        using SqliteCommand cmd = new(StrCmd, Connect());

        var par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Name)}";
        par.Value = Name;
        cmd.Parameters.Add(par);

        Habit habit = new();
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) // Only one habit should be returned
        {
            habit.Name = reader.GetString(0);
            habit.LastOccurrance = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(1)).ToLocalTime().DateTime;
        }
        return habit;
    }

    /// <summary>
    /// Load all habits from database
    /// </summary>
    public IEnumerable<Habit> LoadAllHabits()
    {
        string StrCmd = $"SELECT * FROM [Habits]";
        using SqliteCommand cmd = new(StrCmd, Connect());

        List<Habit> habits = [];

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Habit habit = new()
            {
                Name = reader.GetString(0),
                LastOccurrance = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(1)).ToLocalTime().DateTime,
            };

            habits.Add(habit);
        }
        
        return habits;
    }

    /// <summary>
    /// Check and initialize database against <see cref="DbTemplate"/>
    /// </summary>
    /// <returns> True if database is ready and initialized, false otherwise </returns>
    bool CheckAndInit()
    {
        AnsiConsole.Progress()
            .AutoClear(true)
            .HideCompleted(false)
            .Columns(
                new TaskDescriptionColumn(),
                new PercentageColumn(),
                new ProgressBarColumn(),
                new SpinnerColumn(),
                new RemainingTimeColumn()
                )
            .Start(ctx =>
            {
                var taskInit = ctx.AddTask("[green]Initializing database[/]");
                
                var step1 = ctx.AddTask("[green]Checking database[/]");
                if (File.Exists(DbName))
                {
                    taskInit.Value = 100;
                    step1.Value = 100;
                    return;
                }
                step1.Value = 100;
                taskInit.Value = 10;

                var step2 = ctx.AddTask("[green] Creating database[/]");
                string Template;
                if (System.IO.File.Exists(DbTemplate))
                    Template = System.IO.File.ReadAllText(DbTemplate);
                else
                    throw new System.IO.FileNotFoundException($"Database initialization failed. Restore \"{DbTemplate}\" to continue");
                
                step2.Value = 50;
                taskInit.Value = 20;

                using var connection = Connect();

                step2.Value = 75;
                taskInit.Value = 30;

                using SqliteCommand cmd = new(Template, connection);

                cmd.ExecuteNonQuery();

                step2.Value = 100;
                taskInit.Value = 0b101010; 

                var step3 = ctx.AddTask("[green]Generating mock data[/]");
                
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        SaveHabit($"Habit {i}", DateTime.Now.AddDays(-j));
                        step3.Increment((double)100 / (5 * 5));
                        taskInit.Increment((double)(100-41)/(5 * 5));
                    }
                }
            });

        return true;
    }

    /// <summary>
    /// Connect to database
    /// </summary>
    SqliteConnection Connect()
    {
        SqliteConnectionStringBuilder builder = [];
        builder.DataSource = DbName;
        builder.Mode = SqliteOpenMode.ReadWriteCreate;

        SqliteConnection connection = new ($"Data Source={DbName}");
        connection.Open();
        return connection;
    }
}
