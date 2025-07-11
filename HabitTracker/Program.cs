using HabitTracker;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using static System.Int32;

string dataSource = "DataSource=habittracker.db";
Initialize(dataSource);

var userName = AnsiConsole.Ask<string>("What is your name?");

AnsiConsole.MarkupLine($"[green] Hello {userName}![/]");
AnsiConsole.MarkupLine($"Adding some example data to the habit tracker.");

var queries = new Queries(dataSource);
queries.InsertNewHabit(userName, "drinkingCoffee", 20, DateOnly.FromDateTime(DateTime.Now));
queries.InsertNewHabit(userName, "drinkingWater", 10, DateOnly.FromDateTime(DateTime.Now));
queries.InsertNewHabit(userName, "exercise", 10, DateOnly.FromDateTime(DateTime.Now));
queries.InsertNewHabit(userName, "drinkingMoreWater", 10, DateOnly.FromDateTime(DateTime.Now));
queries.RetrieveHabits(userName);
queries.UpdateHabit(1, 100);
queries.RetrieveHabits(userName);
queries.DeleteHabit(userName, "drinkingCoffee");
queries.RetrieveHabits(userName);

var menu = new UserInterface(dataSource, userName);
menu.MainMenu();

return;

static void Initialize(string dataSource)
{
    using var connection = new SqliteConnection(dataSource);
    connection.Open();
        
    var command = connection.CreateCommand();
    command.CommandText =
        """
        DROP TABLE IF EXISTS habit;

        CREATE TABLE habit (
            id INTEGER PRIMARY KEY AUTOINCREMENT ,
            user TEXT NOT NULL,
            habit TEXT NOT NULL,
            count INTEGER NULL,
            date DATETIME NULL
        );
        """;
    command.ExecuteNonQuery();
    connection.Close();
}


enum MenuOption
{
    InsertHabit,
    SeeHabits,
    UpdateHabit,
    RemoveHabit,
    ExitApplication,
}

public class HabitController(string connectionString, string userName)
{
    private readonly Queries _queries = new Queries(connectionString);
    
    public void InsertHabit()
    {
        var habit = AnsiConsole.Prompt(
            new TextPrompt<string>("What habit do you want to log?"));
        var count = AnsiConsole.Prompt(
            new TextPrompt<int>("How often did you do the habit?")
                .Validate((n) => n switch
                    {
                        < 1 => ValidationResult.Error("Please enter a number between 0 and 100."),
                        <= 100 => ValidationResult.Success(), 
                        > 100 => ValidationResult.Error("Please enter a number between 0 and 100."),
                    }
                ));
        var day = AnsiConsole.Prompt(
            new SelectionPrompt<DateOnly>()
                .Title("What day do you want to log?")
                .AddChoices(new DateOnly[]
                {
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-4)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-6)),
                }) // Considered making this programmatic, but the amount of complexity did not feel worth it.
                    );
        
        _queries.InsertNewHabit(userName, habit, count, day);
        AnsiConsole.MarkupLine($"[blue]Added new habit: {habit}[/]");
        AnsiConsole.WriteLine($"You did {habit}, {count} times on day {day}");
            
    }

    public void SeeHabits()
    {
        var habits = _queries.RetrieveHabits(userName);
        foreach (var habit in habits)
        {
            
            Console.WriteLine($"Id: {habit["id"]}, Habit: {habit["habit"]}, Count: {habit["count"]}, Date: {habit["date"]}");
        }
    }

    public void UpdateHabit()
    {
        var habits = _queries.RetrieveHabits(userName).ToList();
        Console.WriteLine(habits);
        var habitChoice = new SelectionPrompt<string>();
        habitChoice.Title = "What habit do you want to change?";
        foreach (var habit in habits)
        {
            habitChoice.AddChoice(
                $"Id: {habit["id"]}, Habit: {habit["habit"]}, Count: {habit["count"]}, Date: {habit["date"]}");
        }
        var selectedHabit = AnsiConsole.Prompt(habitChoice);
        var habitIndex = selectedHabit.Split(",")[0];
        var habitInt = Parse(habitIndex);

        var countChange = AnsiConsole.Prompt(
            new TextPrompt<int>("What new count do you want to give this habit?")
            );
        
        _queries.UpdateHabit(habitInt, countChange);
    }

    public void RemoveHabit()
    {
        var habits = _queries.RetrieveHabits(userName).ToList();
        Console.WriteLine(habits);
        var habitChoice = new SelectionPrompt<string>();
        habitChoice.Title = "What habit do you want to change?";
        foreach (var habit in habits)
        {
            habitChoice.AddChoice(
                $"{habit["id"]}, Habit: {habit["habit"]}, Count: {habit["count"]}, Date: {habit["date"]}");
        }
        var selectedHabit = AnsiConsole.Prompt(habitChoice);
        var habitIndex = selectedHabit.Split(",")[0];
        var habitInt = Parse(habitIndex);
        _queries.DeleteHabitById(habitInt);
    }
}

public class Queries(string connectionString)
{
    public SqliteConnection Connection = new SqliteConnection(connectionString); 
    public void InsertNewHabit(string user, string habit, int count, DateOnly day) {
        
        Connection.Open();
        using var command = Connection.CreateCommand();
        command.CommandText = $@"
                               INSERT INTO
                                   habit (user, habit, count, date)
                               values
                                   ('{user}', '{habit}', {count}, $date);
                               ";
        command.Parameters.AddWithValue("$date", day);
        command.ExecuteNonQuery();
        Connection.Close();
    }
   
    public List<Dictionary<string, object>> RetrieveHabits(string user)
    {
        var list = new List<Dictionary<string, object>>();
        string readQuery = $"select * from habit where user = '{user}' order by id;";
        using (SqliteCommand readCommand = new SqliteCommand(readQuery, Connection))
        {
            Connection.Open();
            
			using var reader = readCommand.ExecuteReader();
            
            {
                while (reader.Read())
                {
                    var  dict = new Dictionary<string, object>()
                    {
                        { "id", reader.GetInt32(0) },
                        { "user", reader.GetString(1) },
                        { "habit", reader.GetString(2) },
                        { "count", reader.GetInt32(3) },
                        { "date", reader.GetDateTime(4) },
                    };
                    list.Add(dict);
                }
            }
            Connection.Close();
            
        }
        return list;
    }

    public void UpdateHabit(int id,int count)
    {
        Connection.Open();
        string updateQuery = $"update habit set count = {count} where id = {id}; ";
        using var command = Connection.CreateCommand();
        command.CommandText = updateQuery;
        command.ExecuteNonQuery();
        Connection.Close();
    }

    public void DeleteHabit(string user, string habit)
    {
        Connection.Open();
        string updateQuery = $"delete from habit where user = '{user}' and habit = '{habit}'; ";
        using var command = Connection.CreateCommand();
        command.CommandText = updateQuery;
        command.ExecuteNonQuery();
        Connection.Close();
    }

    public void DeleteHabitById(int id)
    {
        {
            Connection.Open();
            string updateQuery = $"delete from habit where id = {id}; ";
            using var command = Connection.CreateCommand();
            command.CommandText = updateQuery;
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
