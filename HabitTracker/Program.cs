using Microsoft.Data.Sqlite;
using Spectre.Console;

string dataSource = "DataSource=habittracker.db";
Initialize(dataSource);

var queries = new Queries(dataSource);
queries.InsertNewHabit("john", "drinkingCoffee", 20, DateOnly.FromDateTime(DateTime.Now));
queries.InsertNewHabit("john", "drinkingWater", 10, DateOnly.FromDateTime(DateTime.Now));
queries.InsertNewHabit("john", "exercise", 10, DateOnly.FromDateTime(DateTime.Now));
queries.InsertNewHabit("john", "drinkingMoreWater", 10, DateOnly.FromDateTime(DateTime.Now));
queries.RetrieveHabits("john");
queries.UpdateHabit("john", "drinkingCoffee", 100);
queries.RetrieveHabits("john");
queries.DeleteHabit("john", "drinkingCoffee");
queries.RetrieveHabits("john");

var menu = new UserInterface(dataSource);
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


internal class UserInterface(string connectionString)
{
    private HabitController HabitController = new HabitController(connectionString);
    internal void MainMenu()
    {
        while (true)
        {
            //Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                    .Title("What do you want to do next?")
                    .AddChoices(Enum.GetValues<MenuOption>()));

            switch (choice)
            {
                case MenuOption.InsertHabit:
                    HabitController.InsertHabit();
                    break;
                case MenuOption.SeeHabits:
                    HabitController.SeeHabits();
                    break;
                case MenuOption.UpdateHabit:
                    HabitController.UpdateHabit();
                    break;
                case MenuOption.RemoveHabit:
                    HabitController.RemoveHabit();
                    break;
            }

        }
    }
}

enum MenuOption
{
    InsertHabit,
    SeeHabits,
    UpdateHabit,
    RemoveHabit,
}

public class HabitController(string connectionString)
{
    //private readonly string _dataSource = dataSource;
    public Queries Queries = new Queries(connectionString);
    
    public void InsertHabit()
    {
        var name = AnsiConsole.Prompt(
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
                })
                    );
        
        Queries.InsertNewHabit("john", name, count, day);
        AnsiConsole.WriteLine($"You did {name}, {count} times on day {day}");
            
    }

    public void SeeHabits()
    {
        var habits = Queries.RetrieveHabits("john");
        foreach (var dict in habits)
        {
            
            Console.WriteLine($"Habit: {dict["habit"]} Count: {dict["count"]} Day: {dict["date"]}");
        }
    }

    public void UpdateHabit()
    {
        var habits = Queries.RetrieveHabits("john").ToList();
        Console.WriteLine(habits);
        var habitChoice = new SelectionPrompt<string>();
        habitChoice.Title = "What habit do you want to change?";
        foreach (var habit in habits)
        {
            habitChoice.AddChoice(habit.ToString());
        }
        var selectedHabit = AnsiConsole.Prompt(habitChoice);
        var user = "john";
        //var count = habits[]
        
    }

    public void RemoveHabit()
    {
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
        
        //int rowCount = GetRowCount("habit");
        var habits = RetrieveHabits(user);
    }

    public int GetRowCount(string tableName)
    {
        Connection.Open();
        string rowCountQuery = $"SELECT COUNT(*) FROM {tableName};";
        int rowCount = 0;
        using (SqliteCommand rowCountCommand = new SqliteCommand(rowCountQuery, Connection))
        {
            using var reader = rowCountCommand.ExecuteReader();
            while (reader.Read())
            {
                rowCount = reader.GetInt32(0);
            }
        }
        return rowCount;
        Connection.Close();

    }
    public List<Dictionary<string, object>> RetrieveHabits(string user)
    {
        var list = new List<Dictionary<string, object>>();
        var index = 0;
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
                    index++;
                    
                    
                }
            }
            Connection.Close();
            
        }
        return list;
    }

    public void UpdateHabit(string user, string habit, int count)
    {
        Connection.Open();
        string updateQuery = $"update habit set count = {count} where user = '{user}' and habit = '{habit}'; ";
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
}
