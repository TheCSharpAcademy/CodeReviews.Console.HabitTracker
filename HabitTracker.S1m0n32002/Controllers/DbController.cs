using HabitTracker.S1m0n32002.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Data;
using System.Data.Common;
using System.Globalization;
namespace HabitTracker.S1m0n32002.Controllers;

public class DbController
{
    /// <summary>
    /// Name of database file name
    /// </summary>
    const string DbName = "Db.sqlite";
    /// <summary>
    /// Database template file name
    /// </summary>
    const string DbTemplate = "SqliteTemplate.sql";
    /// <summary>
    /// DateTime format used in database
    /// </summary>
    const string DateTimeFormat = "yyyyMMddTHHmmssZ";

    public DbController()
    {
        CheckAndInitDB();
    }

    #region "Habit"
    /// <summary>
    /// Add habit to database
    /// </summary>
    /// <param name="name"> name of habit </param>
    /// <param name="periodicity"> Periodicities of habit </param>
    /// <returns> Habit object just created </returns>
    public Habit? SaveHabit(string name, Habit.Periodicities periodicity)
    {
        return SaveHabit(new Habit() { Name = name, Periodicity = periodicity });
    }

    /// <summary>
    /// SaveHabit habit to database
    /// </summary>
    /// <param name="habit"> habit to save </param>
    /// <returns> Habit object just created </returns>
    public Habit? SaveHabit(Habit habit)
    {
        string StrCmd;

        if (habit.Id < 0)
            StrCmd = @$"INSERT INTO [{Habit.TabName}] ({nameof(Habit.Id).ToLower()},
                                                       {nameof(Habit.Name).ToLower()},
                                                       {nameof(Habit.Periodicity).ToLower()}) 
                                               VALUES ((SELECT IFNULL(MAX({nameof(Habit.Id).ToLower()}), -1) + 1 FROM [{Habit.TabName}]),
                                                       @{nameof(Habit.Name).ToLower()},
                                                       @{nameof(Habit.Periodicity).ToLower()})
                                               RETURNING *;";
        else
            StrCmd = @$"UPDATE [{Habit.TabName}] SET {nameof(Habit.Name).ToLower()} = @{nameof(Habit.Name).ToLower()},
                                                     {nameof(Habit.Periodicity).ToLower()} = @{nameof(Habit.Periodicity).ToLower()} 
                                                 WHERE {nameof(Habit.Id).ToLower()} = @{nameof(Habit.Id).ToLower()}
                                                 RETURNING *;";
        

        using SqliteCommand cmd = new(StrCmd, Connect());
        var par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Habit.Id).ToLower()}";
        par.Value = habit.Id;
        cmd.Parameters.Add(par);

        par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Habit.Name).ToLower()}";
        par.Value = habit.Name;
        cmd.Parameters.Add(par);

        par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Habit.Periodicity).ToLower()}";
        par.Value = habit.Periodicity;
        cmd.Parameters.Add(par);

        var rdr = cmd.ExecuteReader();

        if (rdr.CanGetColumnSchema())
        { 
            var schema = rdr.GetColumnSchema();

            var colId = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Id), StringComparison.CurrentCultureIgnoreCase)).Single().Index;
            var colName = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Name), StringComparison.CurrentCultureIgnoreCase)).Single().Index;
            var colPeriodicity = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Periodicity), StringComparison.CurrentCultureIgnoreCase)).Single().Index;

            while (rdr.Read())
            {
                habit.Id = rdr.GetInt32(colId);
                habit.Name = rdr.GetString(colName);
                habit.Periodicity = (Habit.Periodicities)rdr.GetInt32(colPeriodicity);

                return habit;
            }
        }
        return null;
    }

    /// <summary>
    /// Delete a habit from the database
    /// </summary>
    /// <param name="habit"> habit to delete </param>
    public void DeleteHabit(Habit habit)
    {
        string StrCmd = @$"DELETE FROM [{Habit.TabName}] WHERE {nameof(Habit.Id).ToLower()} = @{nameof(Habit.Id).ToLower()}";

        using SqliteCommand cmd = new(StrCmd, Connect());
        var par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(habit.Id).ToLower()}";
        par.Value = habit.Id;
        cmd.Parameters.Add(par);

        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Load all habits from database
    /// </summary>
    public IEnumerable<Habit> LoadAllHabits()
    {
        string StrCmd = $"SELECT * FROM [{Habit.TabName}]";
        using SqliteCommand cmd = new(StrCmd, Connect());

        List<Habit> habits = [];

        using var reader = cmd.ExecuteReader();
        if (reader.CanGetColumnSchema())
        {
            var schema = reader.GetColumnSchema();

            var id = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Id), StringComparison.CurrentCultureIgnoreCase)).Single().Index;
            var name = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Name), StringComparison.CurrentCultureIgnoreCase)).Single().Index;
            var periodicity = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Periodicity), StringComparison.CurrentCultureIgnoreCase)).Single().Index;

            while (reader.Read())
            {
                Habit habit = new()
                {
                    Id = reader.GetInt32(id),
                    Name = reader.GetString(name),
                    Periodicity = (Habit.Periodicities)reader.GetInt32(periodicity),
                };

                habit.Occurrences = [.. LoadAllOccurrences(habit)];

                habits.Add(habit);
            }
        }

        return habits;
    }
    #endregion

    #region "Occurrence"
    /// <summary>
    /// Saves occurrence of a habit
    /// </summary>
    /// <param name="occurrence"> Occurrence to save </param>
    /// <returns> Occurrence object just created </returns>
    public Habit.Occurrence? SaveOccurrence(Habit.Occurrence occurrence)
    {
        string StrCmd;

        if (occurrence.Id < 0)
            StrCmd = @$"INSERT INTO [{Habit.Occurrence.TabName}] ({nameof(Habit.Occurrence.Id).ToLower()},
                                                                  {nameof(Habit.Occurrence.Date).ToLower()},
                                                                  {nameof(Habit.Occurrence.HabitId).ToLower()}) 
                                                           VALUES ((SELECT IFNULL(MAX({nameof(Habit.Occurrence.Id).ToLower()}), -1) + 1 FROM [{Habit.Occurrence.TabName}]),
                                                                  @{nameof(Habit.Occurrence.Date).ToLower()},
                                                                  @{nameof(Habit.Occurrence.HabitId).ToLower()})
                                                           RETURNING *;";
        else
            StrCmd = @$"UPDATE [{Habit.Occurrence.TabName}] SET {nameof(Habit.Occurrence.Date).ToLower()} = @{nameof(Habit.Occurrence.Date).ToLower()} 
                                                          WHERE {nameof(Habit.Occurrence.Id).ToLower()} = @{nameof(Habit.Occurrence.Id).ToLower()}
                                                      RETURNING *;";
        using SqliteCommand cmd = new(StrCmd, Connect());

        var par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Habit.Occurrence.Date).ToLower()}";
        par.Value = occurrence.Date.ToUniversalTime().ToString(DateTimeFormat);
        cmd.Parameters.Add(par);

        par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Habit.Occurrence.Id).ToLower()}";
        par.Value = occurrence.Id;
        cmd.Parameters.Add(par);

        par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Habit.Occurrence.HabitId).ToLower()}";
        par.Value = occurrence.HabitId;
        cmd.Parameters.Add(par);

        var reader = cmd.ExecuteReader();

        if (reader.CanGetColumnSchema())
        {
            var schema = reader.GetColumnSchema();

            var colId = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Occurrence.Id), StringComparison.CurrentCultureIgnoreCase)).Single().Index;
            var colDate = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Occurrence.Date), StringComparison.CurrentCultureIgnoreCase)).Single().Index;
            var colHabitId = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Occurrence.HabitId), StringComparison.CurrentCultureIgnoreCase)).Single().Index;

            while (reader.Read())
            {
                var datetime = DateTime.ParseExact(reader.GetString(colDate), DateTimeFormat, CultureInfo.InvariantCulture).ToLocalTime();
                
                occurrence.Id = reader.GetInt32(colId);
                occurrence.Date = datetime;
                occurrence.HabitId = reader.GetInt32(colHabitId);

                return occurrence;
            }
        }
        return null;
    }

    /// <summary>
    /// Adds occurrence to habit
    /// </summary>
    /// <param name="habit"> parent habit of the occurrence </param>
    /// <param name="date"> date of the occurrence </param>
    /// <returns> Occurrence object just created </returns>
    public Habit.Occurrence? SaveOccurrence(Habit habit, DateTime date)
    {
        return SaveOccurrence(new Habit.Occurrence() { Date = date, HabitId = habit.Id });
    }

    /// <summary>
    /// Delete occurrences from the database
    /// </summary>
    /// <param name="occurrences"> occurrences to delete </param>
    public void DeleteOccurrences(params Habit.Occurrence[] occurrences)
    {
        string StrCmd = @$"DELETE FROM [{Habit.Occurrence.TabName}] WHERE {nameof(Habit.Occurrence.Id).ToLower()} = @{nameof(Habit.Occurrence.Id).ToLower()}";

        foreach (Habit.Occurrence occurrence in occurrences)
        {
            using SqliteCommand cmd = new(StrCmd, Connect());
            var par = cmd.CreateParameter();
            par.ParameterName = $"@{nameof(occurrence.Id).ToLower()}";
            par.Value = occurrence.Id;
            cmd.Parameters.Add(par);

            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Delete all occurrences of a habit from the database
    /// </summary>
    /// <param name="habit"> parent habit of the occurrences to delete </param>
    public void DeleteAllOccurrences(Habit habit)
    {
        string StrCmd = @$"DELETE FROM [{Habit.Occurrence.TabName}] WHERE {nameof(Habit.Occurrence.HabitId).ToLower()} = @{nameof(Habit.Occurrence.HabitId).ToLower()}";

            using SqliteCommand cmd = new(StrCmd, Connect());
            var par = cmd.CreateParameter();
            par.ParameterName = $"@{nameof(Habit.Occurrence.HabitId).ToLower()}";
            par.Value = habit.Id;
            cmd.Parameters.Add(par);

            cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Load all occurrences of a habit from database
    /// </summary>
    /// <param name="habit"> habit to load occurrences </param>
    public IEnumerable<Habit.Occurrence> LoadAllOccurrences(Habit habit)
    {
        string StrCmd = $"SELECT * FROM [{Habit.Occurrence.TabName}] WHERE [{nameof(Habit.Occurrence.HabitId).ToLower()}] = @{nameof(Habit.Occurrence.HabitId).ToLower()}";
        using SqliteCommand cmd = new(StrCmd, Connect());

        var par = cmd.CreateParameter();
        par.ParameterName = $"@{nameof(Habit.Occurrence.HabitId).ToLower()}";
        par.Value = habit.Id;
        cmd.Parameters.Add(par);

        List<Habit.Occurrence> occurrences = [];

        using var reader = cmd.ExecuteReader();
        if (reader.CanGetColumnSchema())
        {
            // Get the column indexes
            var schema = reader.GetColumnSchema();
            var colId = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Occurrence.Id), StringComparison.CurrentCultureIgnoreCase)).Single().Index;
            var colDate = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Occurrence.Date), StringComparison.CurrentCultureIgnoreCase)).Single().Index;
            var colHabitId = schema.Index().Where(x => x.Item.ColumnName.Equals(nameof(Habit.Occurrence.HabitId), StringComparison.CurrentCultureIgnoreCase)).Single().Index;

            while (reader.Read())
            {
                var datetime = DateTime.ParseExact(reader.GetString(colDate), DateTimeFormat ,CultureInfo.InvariantCulture).ToLocalTime();
                Habit.Occurrence occurrence = new()
                {
                    Id = reader.GetInt32(colId),
                    Date = datetime,
                    HabitId = reader.GetInt32(colHabitId),
                };
                occurrences.Add(occurrence);
            }
        }

        return occurrences;
    }
    #endregion

    /// <summary>
    /// Check and initialize database against <see cref="DbTemplate"/>
    /// </summary>
    /// <returns> True if database is ready and initialized, false otherwise </returns>
    void CheckAndInitDB()
    {
        AnsiConsole.Status().Start("Checking database...", ctx =>
            {
                // Assumes the database was already created correctly if the file exists
                if (System.IO.File.Exists(DbName))
                    return;

                AnsiConsole.WriteLine("Database not found!");
                
                ctx.Spinner(Spinner.Known.Star); // not working :(
                ctx.Status("Initializing database...");
                AnsiConsole.WriteLine("Loading database template...");

                string Template;
                if (System.IO.File.Exists(DbTemplate))
                    Template = System.IO.File.ReadAllText(DbTemplate);
                else
                    throw new System.IO.FileNotFoundException($"Database initialization failed. Restore \"{DbTemplate}\" to continue");

                AnsiConsole.WriteLine("Database template loaded!");
                ctx.Status("Creating database...");

                using var connection = Connect();

                using SqliteCommand cmd = new(Template, connection);

                cmd.ExecuteNonQuery();

                AnsiConsole.WriteLine("Database created!");
                ctx.Status("Populating database...");

                Random random = new();

                for (int i = 0; i < 5; i++)
                {
                    var habit = SaveHabit($"DEMO Habit {i}", Habit.Periodicities.Daily);
                    if (habit != null)
                    {
                        for (int d = 0; d < random.Next(0, 20); d++)
                        {
                            for (int h = 0; h < random.Next(0, 20); h++)
                            {
                                SaveOccurrence(habit, DateTime.Now.AddDays(d).AddHours(h));
                            }
                        }
                    }
                }

                AnsiConsole.WriteLine("Database ready!");
            });

        Console.Clear();
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
