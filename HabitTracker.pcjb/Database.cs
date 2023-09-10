using Microsoft.Data.Sqlite;

namespace HabitTracker;

class Database
{
    private readonly string databaseFilename;

    public Database(string databaseFilename)
    {
        this.databaseFilename = databaseFilename;
    }

    public bool AddHabit(Habit habit)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            INSERT INTO habits
            (name, uom) 
            VALUES 
            ($name, $uom)
            ";
            command.Parameters.AddWithValue("$name", habit.Name);
            command.Parameters.AddWithValue("$uom", habit.UOM);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public List<Habit> GetHabits()
    {
        List<Habit> habits = new();

        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, name, uom 
            FROM habits
            ORDER BY id ASC
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt64(0);
                var name = reader.GetString(1);
                var uom = reader.GetString(2);
                habits.Add(new Habit(id, name, uom));
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return habits;
    }

    public Habit? GetHabit(long id)
    {
        Habit? habit = null;
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, name, uom 
            FROM habits
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var name = reader.GetString(1);
                var uom = reader.GetString(2);
                habit = new Habit(id, name, uom);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return habit;
    }

    public bool AddHabitLogRecord(HabitLogRecord record)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            INSERT INTO habitlog
            (habit_id, date, quantity) 
            VALUES 
            ($habit_id, $date, $quantity)
            ";
            command.Parameters.AddWithValue("$habit_id", record.HabitID);
            command.Parameters.AddWithValue("$date", record.Date);
            command.Parameters.AddWithValue("$quantity", record.Quantity);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool UpdateHabitLogRecord(HabitLogRecord record)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            UPDATE habitlog
            SET date=$date, quantity=$quantity
            WHERE id=$id
            ";
            command.Parameters.AddWithValue("$id", record.ID);
            command.Parameters.AddWithValue("$date", record.Date);
            command.Parameters.AddWithValue("$quantity", record.Quantity);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool DeleteHabitLogRecord(long id)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            DELETE FROM habitlog
            WHERE id=$id
            ";
            command.Parameters.AddWithValue("$id", id);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public List<HabitLogRecord> GetHabitLogRecords(long habitID)
    {
        List<HabitLogRecord> habitlog = new();

        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, habit_id, date, quantity 
            FROM habitlog
            WHERE habit_id = $habit_id
            ORDER BY id ASC
            ";
            command.Parameters.AddWithValue("$habit_id", habitID);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt64(0);
                var date = DateOnly.FromDateTime(reader.GetDateTime(2));
                var quantity = reader.GetInt32(3);
                habitlog.Add(new HabitLogRecord(id, habitID, date, quantity));
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return habitlog;
    }

    public bool CreateDatabaseIfNotPresent()
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();

            var createTableHabitsCmd = connection.CreateCommand();
            createTableHabitsCmd.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS habits (
                id INTEGER PRIMARY KEY,
                name TEXT NOT NULL,
                uom TEXT NOT NULL
            )
            ";
            createTableHabitsCmd.ExecuteNonQuery();

            var createTableHabitLogCmd = connection.CreateCommand();
            createTableHabitLogCmd.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS habitlog (
                id INTEGER PRIMARY KEY,
                habit_id INTEGER NOT NULL,
                date TEXT NOT NULL,
                quantity INTEGER NOT NULL,
                FOREIGN KEY(habit_id) REFERENCES habits(id)
            )
            ";
            createTableHabitLogCmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    private string GetConnectionString()
    {
        return String.Format("Data Source={0}", databaseFilename);
    }
}