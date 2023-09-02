using HabitTracker.w0lvesvvv;
using Microsoft.Data.Sqlite;

#region CreateDatabase
string connectionString = @"Data Source=habit-tracker.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var query = connection.CreateCommand();
    query.CommandText = @"CREATE TABLE IF NOT EXISTS habit (
                              habit_id_i INTEGER PRIMARY KEY AUTOINCREMENT,
                              habit_name_nv NVARCHAR
                          );

                          CREATE TABLE IF NOT EXISTS habit_record (
                              record_habit_id_i INTEGER,
                              record_date_dt DATETIME,
                              record_quantity_i INTEGER
                          );";

    query.ExecuteNonQuery();

    connection.Close();
}

#endregion

var option = string.Empty;
Dictionary<int, string> listHabits = new Dictionary<int, string>();
Dictionary<int, HabitRecord> listRecords = new Dictionary<int, HabitRecord>();

#region Menu
do
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("================ HABIT TRACKER ================");
    Console.WriteLine("     1 - View habits");
    Console.WriteLine("     2 - Add habit");
    Console.WriteLine("     3 - Delete habit");
    Console.WriteLine("     4 - View records");
    Console.WriteLine("     5 - Insert record");
    Console.WriteLine("     6 - Delete record");
    Console.WriteLine("     0 - Exit");
    Console.WriteLine("===============================================");

    Console.Write("Option selected: ");
    Console.ForegroundColor = ConsoleColor.White;
    option = Console.ReadLine();

    switch (option)
    {
        case "0":
            Environment.Exit(0);
            break;
        case "1":
            GetHabits();
            break;
        case "2":
            AddHabit();
            break;
        case "3":
            DeleteHabit();
            break;
        case "4":
            GetRecords();
            break;
        case "5":
            InsertRecord();
            break;
        case "6":
            DeleteRecord();
            break;
        default:
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Not valid!");
            Console.WriteLine();
            break;
    }
} while (true);

#endregion

#region HABITS
void GetHabits()
{
    listHabits.Clear();

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var query = connection.CreateCommand();
        query.CommandText = "SELECT habit_name_nv FROM habit";

        SqliteDataReader result = query.ExecuteReader();

        if (result.HasRows)
        {
            int habitCount = 1;

            while (result.Read())
            {
                listHabits.Add(habitCount, result.GetString(0));
                Console.WriteLine($"{habitCount} - {result.GetString(0)}");
                habitCount++;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Habit list is empty. Try creating one first");
        }

        connection.Close();
    }
}

void AddHabit()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("Habit name: ");
    Console.ForegroundColor = ConsoleColor.White;
    string habit = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(habit)) { return; }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var query = connection.CreateCommand();
        query.CommandText = $@"INSERT INTO habit (habit_name_nv) SELECT ('{habit}')
                                   WHERE NOT EXISTS (SELECT 1 FROM habit WHERE habit_name_nv = '{habit}')";

        int rows = query.ExecuteNonQuery();

        if (rows == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is already a habit with that name");
        }

        connection.Close();
    }
}

void DeleteHabit()
{
    string habitName = RequestHabitName();

    if (string.IsNullOrEmpty(habitName)) { return; }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var query = connection.CreateCommand();
        query.CommandText = $"DELETE FROM habit WHERE habit_name_nv = '{habitName}'";
        query.ExecuteNonQuery();

        connection.Close();
    }
}
#endregion

#region RECORDS
void GetRecords()
{
    listRecords.Clear();

    string habitName = RequestHabitName();

    if (string.IsNullOrEmpty(habitName)) { return; }

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var query = connection.CreateCommand();
        query.CommandText = $@"SELECT habit_name_nv
                                   , record_date_dt
                                   , record_quantity_i 
                              FROM habit
                              INNER JOIN habit_record ON habit_id_i = record_habit_id_i
                              WHERE habit_name_nv = '{habitName}'";

        List<HabitRecord> listHabitRecord = new();

        SqliteDataReader result = query.ExecuteReader();

        if (result.HasRows)
        {
            int recordCount = 1;

            while (result.Read())
            {
                HabitRecord record = new HabitRecord
                {
                    Record_habit_name_nv = result.GetString(0),
                    Record_date_dt = result.GetDateTime(1),
                    Record_quantity_i = result.GetInt32(2)
                };

                listHabitRecord.Add(record);
                listRecords.Add(recordCount, record);
                Console.WriteLine($"{recordCount} - {record.Record_habit_name_nv} - {record.Record_date_dt} - {record.Record_quantity_i}");
                recordCount++;
            }

        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{habitName} record list is empty. Try creating one first");
        }

        connection.Close();
    }
}

void InsertRecord()
{
    string habitName = RequestHabitName();

    if (string.IsNullOrEmpty(habitName)) { return; }
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("Insert quantity: ");
    Console.ForegroundColor = ConsoleColor.White;
    int? quantity = readNumber();

    if (quantity == null) return;

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var query = connection.CreateCommand();
        query.CommandText = $@"INSERT INTO habit_record (record_habit_id_i, record_date_dt, record_quantity_i)
                               SELECT habit_id_i, DATETIME('now'), {quantity}
                               FROM habit
                               WHERE habit_name_nv = '{habitName}'";

        int rows = query.ExecuteNonQuery();

        connection.Close();
    }
}

void DeleteRecord()
{
    int? recordNumber = RequestRecordNumber();

    if (recordNumber == null) { return; }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var query = connection.CreateCommand();
        //Not very good way to delete records but as it's impossible to have two records with same record_date_dt in this app i don't care
        query.CommandText = $@"DELETE FROM habit_record WHERE record_date_dt = '{listRecords.Where(x => x.Key == recordNumber.Value).First().Value.Record_date_dt.ToString("yyyy-MM-dd HH:mm:ss")}'";
        query.ExecuteNonQuery();

        connection.Close();
    }
}
#endregion

#region GENERIC METHODS
string RequestHabitName()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    GetHabits();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine();
    Console.Write("Introduce habit number: ");
    Console.ForegroundColor = ConsoleColor.White;
    int? habitNumber = readNumber();

    if (habitNumber == null) { return string.Empty; }

    listHabits.TryGetValue(habitNumber.Value, out string habitName);

    return habitName;
}

int? RequestRecordNumber()
{
    GetRecords();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine();
    Console.Write("Introduce record number: ");
    Console.ForegroundColor = ConsoleColor.White;
    return readNumber();
}

int? readNumber()
{
    string inputNumber = Console.ReadLine();
    if (!int.TryParse(inputNumber, out int habitNumber))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid input.");
        return null;
    }

    return habitNumber;
}
#endregion

