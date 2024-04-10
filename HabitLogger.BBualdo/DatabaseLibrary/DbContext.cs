using DatabaseLibrary.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace DatabaseLibrary;

public class DbContext
{
  private SqliteConnection _Connection { get; set; }

  public DbContext()
  {
    _Connection = new SqliteConnection("Data Source=habits.db");
    CreateTables();
    SeedHabitTable();
    SeedRecordTable();
  }

  public bool CreateHabit()
  {
    Console.Clear();
    Console.WriteLine("Enter a name of your habit (running, meditating, reading, swimming, etc.):\n");
    string? name = Console.ReadLine();

    while (string.IsNullOrEmpty(name))
    {
      Console.WriteLine("\nName can't be blank.\n");
      name = Console.ReadLine();
    }

    Console.WriteLine("\nEnter a measurement unit for your habit (kilometers, times, pages, minutes, etc.):\n");
    string? unit = Console.ReadLine();

    while (string.IsNullOrEmpty(unit))
    {
      Console.WriteLine("\nUnit can't be blank.\n");
      unit = Console.ReadLine();
    }

    using (_Connection)
    {
      _Connection.Open();

      string checkIfExistsQuery = $"SELECT COUNT(*) FROM habit WHERE name='{name}'";

      using (SqliteCommand checkCommand = new SqliteCommand(checkIfExistsQuery, _Connection))
      {
        int matchingHabits = Convert.ToInt32(checkCommand.ExecuteScalar());

        if (matchingHabits > 0)
        {
          Console.WriteLine("\nThat habit already exists. Press any key to return to Main Menu.\n");
          Console.ReadKey();
          _Connection.Close();
          return false;
        }
      }

      string insertHabitQuery = $"INSERT INTO habit(name, unit) VALUES('{name}', '{unit}')";

      using (SqliteCommand insertCommand = new SqliteCommand(insertHabitQuery, _Connection))
      {
        insertCommand.ExecuteNonQuery();
      }

      _Connection.Close();
      Console.Clear();
      Console.WriteLine($"\n{name} habit has been created. Press any key to return to Main Menu.\n");
      Console.ReadKey();
      return true;
    }
  }

  public bool GetAllRecords()
  {
    Console.Clear();
    List<Habit>? habits = GetAllHabits();

    if (habits == null) return false;

    Habit? choosenHabit = GetHabit(habits);

    if (choosenHabit == null) return false;

    List<Record>? records = GetRecordsForHabit(choosenHabit);

    if (records == null) return false;

    Console.WriteLine("---------------------------------------------------");
    foreach (Record record in records)
    {
      Console.WriteLine($"No.{record.Id}  - Date: {record.Date:dd-MMMM-yyyy}  - Quantity: {record.Quantity}");
      Console.WriteLine("---------------------------------------------------");
    }

    Console.WriteLine("\nPress any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool InsertRecord()
  {
    Console.Clear();
    List<Habit>? habits = GetAllHabits();

    if (habits == null) return false;

    Habit? choosenHabit = GetHabit(habits);

    if (choosenHabit == null) return false;

    string? date = GetDateInput();
    if (date == null) return false;
    int? quantity = GetQuantityInput(choosenHabit);
    if (quantity == null) return false;

    using (_Connection)
    {
      _Connection.Open();

      string insertRecordQuery = $"INSERT INTO record(habit_id, date, quantity) VALUES({choosenHabit.Id}, '{date}', {quantity})";

      using (SqliteCommand insertCommand = new SqliteCommand(insertRecordQuery, _Connection))
      {
        insertCommand.ExecuteNonQuery();
      }

      _Connection.Close();
    }

    Console.WriteLine($"\nRecord inserted into {choosenHabit.Name} successfully. Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool UpdateRecord()
  {
    Console.Clear();

    List<Habit>? habits = GetAllHabits();

    if (habits == null) return false;

    Habit? choosenHabit = GetHabit(habits);

    if (choosenHabit == null) return false;

    List<Record>? records = GetRecordsForHabit(choosenHabit);

    if (records == null) return false;

    Console.WriteLine("---------------------------------------------------");
    foreach (Record record in records)
    {
      Console.WriteLine($"No.{record.Id}  - Date: {record.Date:dd-MMMM-yyyy}  - Quantity: {record.Quantity}");
      Console.WriteLine("---------------------------------------------------");
    }

    int? recordId = GetRecordId();
    if (recordId == null) return false;

    UpdateRecordById(recordId, choosenHabit);

    Console.WriteLine($"\nUpdating record for {choosenHabit.Name} completed. Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool DeleteRecord()
  {
    Console.Clear();

    List<Habit>? habits = GetAllHabits();
    if (habits == null) return false;

    Habit? choosenHabit = GetHabit(habits);

    if (choosenHabit == null) return false;

    List<Record>? records = GetRecordsForHabit(choosenHabit);
    if (records == null) return false;

    Console.WriteLine("---------------------------------------------------");
    foreach (Record record in records)
    {
      Console.WriteLine($"No.{record.Id}  - Date: {record.Date:dd-MMMM-yyyy}  - Quantity: {record.Quantity}");
      Console.WriteLine("---------------------------------------------------");
    }

    int? recordId = GetRecordId();
    if (recordId == null) return false;

    DeleteRecordById(recordId, choosenHabit);

    Console.WriteLine($"\nDeleting record for {choosenHabit.Name} completed. Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool DeleteHabit()
  {
    Console.Clear();

    List<Habit>? habits = GetAllHabits();
    if (habits == null) return false;

    Habit? habitToDelete = GetHabit(habits);

    if (habitToDelete == null) return false;

    using (_Connection)
    {
      _Connection.Open();

      string deleteHabitQuery = $"DELETE FROM habit WHERE habit_id={habitToDelete.Id}";

      using (SqliteCommand deleteCommand = new SqliteCommand(deleteHabitQuery, _Connection))
      {
        deleteCommand.ExecuteNonQuery();
      }

      _Connection.Close();

      Console.WriteLine($"\nDeleting {habitToDelete.Name} habit completed. Press any key to return to Main Menu.");
      Console.ReadKey();
      return true;
    }
  }

  public bool GetSummary()
  {
    Console.Clear();

    List<Habit>? habits = GetAllHabits();
    if (habits == null) return false;
    Habit? choosenHabit = GetHabit(habits);
    if (choosenHabit == null) return false;

    using (_Connection)
    {
      _Connection.Open();

      string sumQuery = @$"
                    SELECT habit.name, habit.unit, SUM(record.quantity) AS total_quantity
                    FROM habit
                    JOIN record ON habit.habit_id=record.habit_id
                    WHERE habit.name='{choosenHabit.Name}'
                    GROUP BY habit.name, habit.unit";

      using (SqliteCommand command = new SqliteCommand(sumQuery, _Connection))
      {
        using (SqliteDataReader reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            string habitName = reader.GetString(0);
            string unit = reader.GetString(1);
            int totalQuantity = reader.GetInt32(2);

            Console.WriteLine($"\nYou were {habitName} {totalQuantity} {unit}.");
          }
          else
          {
            Console.WriteLine("\nNo records found for the specified habit.");
          }
        }
      }

      _Connection.Close();
    }

    Console.WriteLine("\nPress any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  #region Private Methods
  private void CreateTables()
  {
    using (_Connection)
    {
      _Connection.Open();

      string createTablesQuery = @"CREATE TABLE IF NOT EXISTS habit(
                                    habit_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    name TEXT,
                                    unit TEXT);
                                    CREATE TABLE IF NOT EXISTS record(
                                    record_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    habit_id INTEGER,
                                    date TEXT,
                                    quantity INTEGER,
                                    FOREIGN KEY(habit_id) REFERENCES habit(habit_id)
                                    )";

      using (SqliteCommand command = new SqliteCommand(createTablesQuery, _Connection))
      {
        command.ExecuteNonQuery();
      }

      _Connection.Close();
    }
  }

  private void SeedHabitTable()
  {
    using (_Connection)
    {
      _Connection.Open();

      string countHabitRowsQuery = @"SELECT COUNT(*) FROM habit";

      using (SqliteCommand countCommand = new SqliteCommand(countHabitRowsQuery, _Connection))
      {
        int rowsNumber = Convert.ToInt32(countCommand.ExecuteScalar());

        if (rowsNumber == 0)
        {
          Dictionary<string, string> defaultHabits = new Dictionary<string, string>() {
            { "running", "kilometers" },
            { "swimming", "minutes" },
            { "reading", "pages" },
            { "drinking_water", "glasses" },
            { "meditating", "times" }
          };

          foreach (KeyValuePair<string, string> entry in defaultHabits)
          {
            Console.Clear();
            Console.WriteLine("Please wait...");
            Console.WriteLine($"Loading {entry.Key} habit...");
            string insertHabitQuery = $"INSERT INTO habit(name, unit) VALUES('{entry.Key}', '{entry.Value}')";
            using (SqliteCommand insertCommand = new SqliteCommand(insertHabitQuery, _Connection))
            {
              insertCommand.ExecuteNonQuery();
            }
          }
        }
      }

      Console.Clear();
      _Connection.Close();
    }
  }

  private void SeedRecordTable()
  {
    using (_Connection)
    {
      _Connection.Open();

      string countRecordRowsQuery = "SELECT COUNT(*) FROM record";

      using (SqliteCommand countCommand = new SqliteCommand(countRecordRowsQuery, _Connection))
      {
        int rowsNumber = Convert.ToInt32(countCommand.ExecuteScalar());

        if (rowsNumber == 0)
        {
          Console.Clear();
          Console.WriteLine("Loading...");

          string getHabitIdQuery = "SELECT habit_id FROM habit";

          using (SqliteCommand getHabitIdCommand = new SqliteCommand(getHabitIdQuery, _Connection))
          {
            using (SqliteDataReader reader = getHabitIdCommand.ExecuteReader())
            {
              while (reader.Read())
              {
                int habitID = reader.GetInt32(0);

                for (int i = 0; i < 5; i++)
                {
                  Random random = new Random();
                  string date = DateTime.Now.AddDays(-random.Next(1, 365)).ToString("dd-MM-yy");
                  int quantity = random.Next(1, 10);

                  string insertRecordQuery = $"INSERT INTO record(habit_id, date, quantity) VALUES({habitID}, '{date}', {quantity})";

                  using (SqliteCommand insertCommand = new SqliteCommand(insertRecordQuery, _Connection))
                  {
                    insertCommand.ExecuteNonQuery();
                  }
                }
              }
            }
          }
        }
      }
      Console.Clear();
      _Connection.Close();
    }
  }

  private List<Habit>? GetAllHabits()
  {
    using (_Connection)
    {
      _Connection.Open();

      List<Habit> habits = new List<Habit>();

      string selectAllHabitsQuery = "SELECT * FROM habit";

      using (SqliteCommand selectAllCommand = new SqliteCommand(selectAllHabitsQuery, _Connection))
      {
        using (SqliteDataReader reader = selectAllCommand.ExecuteReader())
        {
          if (!reader.HasRows)
          {
            _Connection.Close();
            Console.WriteLine("Habits list is empty. Create one first. Press any key to return to Main Menu.\n");
            Console.ReadKey();
            return null;
          }

          while (reader.Read())
          {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string unit = reader.GetString(2);

            habits.Add(new Habit(id, name, unit));
          }
        }
      }

      _Connection.Close();

      Console.WriteLine("Choose a habit to interact with:\n");

      foreach (Habit habit in habits)
      {
        Console.WriteLine($"{habit.Id} - {habit.Name} ({habit.Unit})");
      }

      Console.WriteLine("");

      return habits;
    }
  }

  private Habit? GetHabit(List<Habit> habits)
  {
    int habitId;
    string? userInput = Console.ReadLine();

    while (!int.TryParse(userInput, out habitId))
    {
      Console.WriteLine("\nInvalid input.");
      userInput = Console.ReadLine();
    }

    Habit? choosenHabit = habits.FirstOrDefault(habit => habit.Id == habitId);

    while (choosenHabit == null)
    {
      Console.WriteLine("\nThere is no habit with given number. Press any key to return to Main Menu.");
      Console.ReadKey();
      return null;
    }

    return choosenHabit;
  }

  private List<Record>? GetRecordsForHabit(Habit habit)
  {
    Console.Clear();

    List<Record> records = new List<Record>();

    using (_Connection)
    {
      _Connection.Open();

      string selectRecordsQuery = $"SELECT * FROM record WHERE habit_id={habit.Id}";

      using (SqliteCommand selectCommand = new SqliteCommand(selectRecordsQuery, _Connection))
      {
        using (SqliteDataReader reader = selectCommand.ExecuteReader())
        {
          if (!reader.HasRows)
          {
            Console.WriteLine("There is no records yet. Press any key to return to Main Menu.\n");
            Console.ReadKey();
            return null;
          }

          while (reader.Read())
          {
            int record_id = reader.GetInt32(0);
            int habit_id = reader.GetInt32(1);
            DateTime date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US"));
            int quantity = reader.GetInt32(3);

            records.Add(new Record(record_id, habit_id, date, quantity));
          }
        }
      }

      _Connection.Close();
    }

    return records;
  }

  private string? GetDateInput()
  {
    Console.Clear();
    Console.WriteLine("Enter date for the record (Format: dd-mm-yy). Type 0 to return to Main Menu.\n");
    string? userInput = Console.ReadLine();

    while (!DateTime.TryParseExact(userInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
      if (userInput == "0") return null;

      Console.WriteLine("\nInvalid date. Please enter date in format (dd-mm-yy). Type 0 to return to Main Menu.\n");
      userInput = Console.ReadLine();
    }

    return userInput;
  }

  private int? GetQuantityInput(Habit choosenHabit)
  {
    Console.Clear();
    Console.WriteLine($"Enter quantity of {choosenHabit.Unit}. Type 0 to return to Main Menu.\n");
    int quantity;
    string? userInput = Console.ReadLine();

    while (!int.TryParse(userInput, out quantity) || quantity < 0)
    {
      Console.WriteLine("\nInvalid quantity. Try again or type 0 to return to Main Menu.\n");
      userInput = Console.ReadLine();
    }

    if (quantity == 0) return null;

    return quantity;
  }

  private int? GetRecordId()
  {
    int recordId;
    string? userInput = Console.ReadLine();

    while (!int.TryParse(userInput, out recordId) || recordId < 0)
    {
      Console.WriteLine("\nInvalid input. Try again or type 0 to return to Main Menu.\n");
      userInput = Console.ReadLine();
    }

    if (recordId == 0) return null;

    return recordId;
  }

  private bool UpdateRecordById(int? record_id, Habit habit)
  {
    using (_Connection)
    {
      _Connection.Open();

      string selectRecordQuery = $"SELECT EXISTS(SELECT 1 FROM record WHERE record_id={record_id} AND habit_id={habit.Id})";

      using (SqliteCommand selectCommand = new SqliteCommand(selectRecordQuery, _Connection))
      {
        int matchingRows = Convert.ToInt32(selectCommand.ExecuteScalar());

        if (matchingRows == 0)
        {
          Console.WriteLine("Record doesn't exist. Press any key to try again.\n");
          Console.ReadKey();
          UpdateRecord();
        }
      }

      string? date = GetDateInput();
      if (date == null) return false;
      int? quantity = GetQuantityInput(habit);
      if (quantity == null) return false;

      string updateRecordQuery = $"UPDATE record SET date='{date}', quantity={quantity} WHERE record_id={record_id} AND habit_id={habit.Id}";

      using (SqliteCommand updateCommand = new SqliteCommand(updateRecordQuery, _Connection))
      {
        updateCommand.ExecuteNonQuery();
      }

      _Connection.Close();
    }

    return true;
  }

  private bool DeleteRecordById(int? record_id, Habit habit)
  {
    using (_Connection)
    {
      _Connection.Open();

      string selectRecordQuery = $"SELECT EXISTS(SELECT 1 FROM record WHERE record_id={record_id} AND habit_id={habit.Id})";

      using (SqliteCommand selectCommand = new SqliteCommand(selectRecordQuery, _Connection))
      {
        int matchingRows = Convert.ToInt32(selectCommand.ExecuteScalar());

        if (matchingRows == 0)
        {
          Console.WriteLine("Record doesn't exist. Press any key to try again.\n");
          Console.ReadKey();
          DeleteRecord();
        }
      }

      string deleteRecordQuery = $"DELETE FROM record WHERE record_id={record_id} AND habit_id={habit.Id}";

      using (SqliteCommand updateCommand = new SqliteCommand(deleteRecordQuery, _Connection))
      {
        updateCommand.ExecuteNonQuery();
      }

      _Connection.Close();
    }

    return true;
  }
  #endregion
}