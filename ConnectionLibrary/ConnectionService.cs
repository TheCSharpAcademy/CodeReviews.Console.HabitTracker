using System.Data;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;

namespace ConnectionLibrary;

public class ConnectionService
{
    public string DbPath { get; set; }
    SqliteConnectionStringBuilder connectionStringBuilder;
    string connectionString;

    public ConnectionService(string dbPath = "HabitTracker.db")
    {
        // Constructor
        DbPath = dbPath;
        connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = DbPath
        };
        connectionString = connectionStringBuilder.ConnectionString;
    }

    public void Init()
    {
        // Initialize the database
        if (!File.Exists(DbPath))
        {
            Console.Clear();
            Console.WriteLine("Database file does not exist. Creating a new database file...");
            Console.WriteLine("Please wait...");
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string createTable = @"
                CREATE TABLE HabitTracker 
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Habit TEXT NOT NULL, 
                    CreationDate TEXT NOT NULL,
                    DeadlineDate TEXT,
                    GoalProgress INTEGER,
                    Goal INTEGER,
                    GoalType TEXT,
                    GoalCompletion INTEGER,
                    GoalMeasurement TEXT
                )";
                SqliteCommand command = new SqliteCommand(createTable, connection);
                command.ExecuteNonQuery();
            }
            if (DbPath == "SeededDb.db")
            {
                Random random = new Random();
                string randomHabit;
                DateTime randomDate;
                string randomDateStr;
                string randomDeadline;
                int randomGoalProgress;
                int randomGoal;
                string randomGoalType;
                string[] goalTypes = { "Daily", "Weekly", "Monthly", "Yearly" };
                int randomGoalCompletion;
                string randomGoalMeasurement;

                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    for (int i = 0; i < 100; i++)
                    {
                        randomHabit = "Random Habit " + i;
                        randomDate = DateTime.Now.AddDays(random.Next(1, 30));
                        randomDateStr = randomDate.ToString("yyyy-MM-dd HH:mm:ss");
                        randomDeadline = randomDate.AddDays(random.Next(1, 30)).ToString("yyyy-MM-dd HH:mm:ss");
                        randomGoalProgress = random.Next(1, 100);
                        randomGoal = random.Next(1, 100);
                        while (randomGoalProgress > randomGoal)
                        {
                            randomGoalProgress = random.Next(1, 100);
                        }
                        randomGoalType = goalTypes[random.Next(0, 4)];
                        if (randomGoal == randomGoalProgress)
                        {
                            randomGoalCompletion = 1;
                        }
                        else
                        {
                            randomGoalCompletion = 0;
                        }
                        randomGoalMeasurement = "Random Goal Measurement" + i;

                        string addSeededData = @$"
                            INSERT INTO HabitTracker 
                            (
                                Habit, 
                                CreationDate,
                                DeadlineDate,
                                GoalProgress,
                                Goal,
                                GoalType,
                                GoalCompletion,
                                GoalMeasurement
                            )
                            VALUES
                            (
                                '{randomHabit}',
                                '{randomDateStr}', 
                                '{randomDeadline}', 
                                '{randomGoalProgress}', 
                                '{randomGoal}', 
                                '{randomGoalType}', 
                                '{randomGoalCompletion}', 
                                '{randomGoalMeasurement}'
                            )";
                        SqliteCommand command = new SqliteCommand(addSeededData, connection);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    public DataTable ExecuteDbCommand
    (
        string commandText,
        long id = 0,
        string dbColumn = "",
        string habit = "",
        string date = "",
        string deadline = "",
        long? goal = 0,
        string goalType = "",
        long? goalCompletion = 0,
        string goalMeasurement = ""
    )
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string query = "";
            switch (commandText)
            {
                case "InsertHabit":
                    query = @$"
                    INSERT INTO HabitTracker 
                    (
                        Habit, CreationDate, DeadlineDate, Goal, GoalType, GoalCompletion, GoalMeasurement
                    ) 
                    VALUES
                    (
                        '{habit}', '{date}', '{deadline}', @Goal, '{goalType}', @GoalCompletion, '{goalMeasurement}'
                    )";
                    break;
                case "DeleteHabit":
                    query = $"DELETE FROM HabitTracker WHERE Id = '{id}'";
                    break;
                case "UpdateHabit":
                    query = $"UPDATE HabitTracker SET '{dbColumn}' = '{habit}' WHERE Id = '{id}'";
                    break;
                case "ViewHabits":
                    query = "SELECT * FROM HabitTracker";
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                if (commandText == "ViewHabits")
                {
                    return GetDataAsTable(query);
                }
                else
                {
                    if (goal.HasValue)
                    {
                        command.Parameters.AddWithValue("@Goal", goal);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Goal", DBNull.Value);
                    }
                    if (goalCompletion.HasValue)
                    {
                        command.Parameters.AddWithValue("@GoalCompletion", goalCompletion);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@GoalCompletion", DBNull.Value);
                    }
                    command.ExecuteNonQuery();
                }
            }
            return new DataTable();
        }
    }

    private DataTable GetDataAsTable(string query)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }
    }

    public DataTable GetFilteredResults(string? habit = null, string? date = null, string? goalType = null, string? goalCompletion = null)
    {
        string query = "SELECT * FROM HabitTracker WHERE 1=1";

        if (!string.IsNullOrEmpty(habit))
        {
            query += $" AND Habit = '{habit}'";
        }
        if (!string.IsNullOrEmpty(date))
        {
            query += $" AND CreationDate LIKE '{date}%'";
        }
        if (!string.IsNullOrEmpty(goalType))
        {
            query += $" AND GoalType = '{goalType}'";
        }
        if (!string.IsNullOrEmpty(goalCompletion))
        {
            query += $" AND GoalCompletion = '{goalCompletion}'";
        }

        return GetDataAsTable(query);
    }
}





