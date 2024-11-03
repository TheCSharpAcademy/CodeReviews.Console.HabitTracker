using System.Globalization;
using Microsoft.Data.Sqlite;

const string ConnectionString = @"Data Source=habit-tracker.db";

using (var connection = new SqliteConnection(ConnectionString))
{
  connection.Open();

  var tableCmd = connection.CreateCommand();

  tableCmd.CommandText =
      @"CREATE TABLE IF NOT EXISTS Habits (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Unit TEXT NOT NULL
        )";
  tableCmd.ExecuteNonQuery();

  tableCmd.CommandText =
      @"CREATE TABLE IF NOT EXISTS HabitRecords (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            HabitId INTEGER,
            Date TEXT,
            Quantity INTEGER,
            FOREIGN KEY (HabitId) REFERENCES Habits(Id)
        )";
  tableCmd.ExecuteNonQuery();

  connection.Close();
}
SeedDatabase();
GetUserInput();

static void GetUserInput()
{
  Console.Clear();
  bool closeApp = false;
  while (closeApp == false)
  {
    Console.WriteLine("MAIN MENU \n");
    Console.WriteLine("What would you like to do? \n");
    Console.WriteLine("Enter 0 to Close Application. \n");
    Console.WriteLine("Enter 1 to View All Records.");
    Console.WriteLine("Enter 2 to Insert Record.");
    Console.WriteLine("Enter 3 to Delete Record.");
    Console.WriteLine("Enter 4 to Update Record.");
    Console.WriteLine("Enter 5 to Add New Habit.");
    Console.WriteLine("------------------------------------------\n");


    string? userInput = Console.ReadLine();

    switch (userInput)
    {
      case "0":
        Console.WriteLine("\nGoodbye!\n");
        closeApp = true;
        Environment.Exit(0);
        break;
      case "1":
        GetAllRecords();
        break;
      case "2":
        Insert();
        break;
      case "3":
        Delete();
        break;
      case "4":
        Update();
        break;
      case "5":
        AddHabit();
        break;
      default:
        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
        break;
    }
  }
}

static void Insert()
{
  Console.WriteLine("Choose a habit to track by entering its ID:");
  ViewHabits();

  int habitId = GetNumberInput("Enter the Habit ID:");
  string date = GetDateInput();
  int quantity = GetNumberInput("\nPlease enter the quantity:");

  using (var connection = new SqliteConnection(ConnectionString))
  {
    connection.Open();
    var command = connection.CreateCommand();
    command.CommandText = $"INSERT INTO HabitRecords (HabitId, Date, Quantity) VALUES ({habitId}, '{date}', {quantity})";
    command.ExecuteNonQuery();
    connection.Close();
  }

  Console.WriteLine("\nRecord added successfully!");

}

static void Delete()
{
  Console.Clear();
  GetAllRecords();

  var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n");

  using (var connection = new SqliteConnection(ConnectionString))
  {
    connection.Open();
    var tableCmd = connection.CreateCommand();
    tableCmd.CommandText = $"DELETE FROM HabitRecords WHERE Id = {recordId}";

    int rowCount = tableCmd.ExecuteNonQuery();
    if (rowCount == 0)
    {
      Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.\n");
    }
    else
    {
      Console.WriteLine($"\nRecord with Id {recordId} was deleted.\n");
    }
    connection.Close();
  }

  Console.WriteLine($"\nRecord with Id {recordId} was deleted. \n");

  GetUserInput();
}

static void Update()
{
  GetAllRecords();

  var recordId = GetNumberInput("\nPlease type Id of the record you would like to update. Type 0 to return to main menu.\n");

  using (var connection = new SqliteConnection(ConnectionString))
  {
    connection.Open();
    var checkCmd = connection.CreateCommand();
    checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM HabitRecords WHERE Id = {recordId})";

    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
    if (checkQuery == 0)
    {
      Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
      connection.Close();
      Update();
    }

    string date = GetDateInput();
    int quantity = GetNumberInput("\n\nPlease enter the quantity:\n\n");

    var tableCmd = connection.CreateCommand();
    tableCmd.CommandText = $"UPDATE HabitRecords SET Date = '{date}', Quantity = {quantity} WHERE Id = {recordId}";
    tableCmd.ExecuteNonQuery();
    connection.Close();
  }
}

static void GetAllRecords()
{
  Console.Clear();
  using (var connection = new SqliteConnection(ConnectionString))
  {
    connection.Open();
    var tableCmd = connection.CreateCommand();
    tableCmd.CommandText = $"SELECT * FROM HabitRecords";

    List<HabitRecord> tableData = new List<HabitRecord>();
    SqliteDataReader reader = tableCmd.ExecuteReader();

    if (reader.HasRows)
    {
      while (reader.Read())
      {
        tableData.Add(new HabitRecord
        {
          Id = reader.GetInt32(0),
          HabitId = reader.GetInt32(1),
          Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
          Quantity = reader.GetInt32(3)
        });
      }
    }
    else
    {
      Console.WriteLine("No records found!");
    }
    connection.Close();

    Console.WriteLine("----------------------------\n");
    foreach (var record in tableData)
    {
      Console.WriteLine($"{record.Id} - {record.Date.ToString("dd-MMM-yyyy")} - Quantity: {record.Quantity}");
    }
    Console.WriteLine("-----------------------\n");
  }

}

static string GetDateInput()
{
  Console.WriteLine("\nPlease insert the date: (Format: dd-mm-yy).\n Type 00 to set date as today \n Type 0 to return to main manu.\n");
  string? dateInput = Console.ReadLine();
  if (dateInput == "0") GetUserInput();

  if (dateInput == "00")
  {
    var today = DateTime.Today;
    dateInput = today.ToString("dd-MM-yy");
  }
  else
  {
    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
      Console.WriteLine("\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n");
      dateInput = Console.ReadLine();
    }
  }
  return dateInput;
}

static int GetNumberInput(string message)
{
  Console.WriteLine(message);

  string? numberInput = Console.ReadLine();

  if (numberInput == "0") GetUserInput();

  while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
  {
    Console.WriteLine("\nInvalid number. Try again.\n");
    numberInput = Console.ReadLine();
  }

  int finalInput = Convert.ToInt32(numberInput);

  return finalInput;
}

static void SeedDatabase()
{
  using (var connection = new SqliteConnection(ConnectionString))
  {
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText = "SELECT Id FROM Habits LIMIT 1";
    var habitIdObj = tableCmd.ExecuteScalar();

    int habitId;
    if (habitIdObj == null)
    {
      tableCmd.CommandText = "INSERT INTO Habits (Name, Unit) VALUES ('Cycling', 'kilometers')";
      tableCmd.ExecuteNonQuery();

      tableCmd.CommandText = "SELECT Id FROM Habits LIMIT 1";
      habitId = Convert.ToInt32(tableCmd.ExecuteScalar());
    }
    else
    {
      habitId = Convert.ToInt32(habitIdObj);
    }
    var random = new Random();
    for (int i = 0; i < 100; i++)
    {
      var date = DateTime.Today.AddDays(-random.Next(0, 365)).ToString("dd-MM-yy");
      int quantity = random.Next(1, 100);
      tableCmd.CommandText = $"INSERT INTO HabitRecords (HabitId, Date, Quantity) VALUES({habitId}, '{date}', {quantity})";
      tableCmd.ExecuteNonQuery();
    }
    connection.Close();
  }
  Console.WriteLine("100 records have been added to the db.");
}

static void AddHabit()
{
  Console.WriteLine("\nEnter the name of the habit you want to track:");
  string? habitName = Console.ReadLine();

  Console.WriteLine("\nEnter the unit of measurement (e.g: reps, minutes, kilometers):");
  string? habitUnit = Console.ReadLine();

  using (var connection = new SqliteConnection(ConnectionString))
  {
    connection.Open();
    var command = connection.CreateCommand();
    command.CommandText = $"INSERT INTO Habits (Name, Unit) VALUES ('{habitName}', '{habitUnit}')";
    command.ExecuteNonQuery();
    connection.Close();
  }

  Console.WriteLine($"\nHabit '{habitName}' added successfully!");

}

static void ViewHabits()
{
  using (var connection = new SqliteConnection(ConnectionString))
  {
    connection.Open();
    var command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM Habits";

    using (var reader = command.ExecuteReader())
    {
      Console.WriteLine("\nAvailable Habits:");
      while (reader.Read())
      {
        int id = reader.GetInt32(0);
        string name = reader.GetString(1);
        string unit = reader.GetString(2);
        Console.WriteLine($"{id}. {name} ({unit})");
      }
    }
    connection.Close();
  }
}



public class HabitRecord
{
  public int Id { get; set; }
  public int HabitId { get; set; }
  public DateTime Date { get; set; }
  public int Quantity { get; set; }
}