using System.Data.SQLite;
using System.Globalization;

namespace HabitTracker.kmakai;

public class Tracker
{
    private List<Habit> _habitsList { get; set; } = new List<Habit>();
    private string? _connectionString { get; set; } = @"Data Source=Habit_Tracker.db";

    public Tracker()
    {
        CreatedHabitsTable();
        _habitsList = this.GetListFromDb();
    }

    public void CreatedHabitsTable()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @$"CREATE TABLE IF NOT EXISTS habits_list (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Name TEXT,
                                            UnitOfMeasurement TEXT
                                        )";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void InsertToHabitsListTable(string habit, string unitOfMeasurement)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @$"INSERT INTO habits_list (Name, UnitOfMeasurement) VALUES ('{habit}', '{unitOfMeasurement}')";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void start()
    {
        CreatedHabitsTable();
        while (true)
        {

            string option = TrackerMenu.MainMenu();
            switch (option)
            {
                case "1":
                    AddHabit();
                    break;
                case "2":
                    RemoveHabit();
                    break;
                case "3":
                    ManageHabits();
                    break;
                case "4":
                    ViewHabitsList();
                    break;
                case "0":
                    Console.WriteLine("Thank you for using the Habit Tracker!\nGood Bye!");

                    return;
                default:
                    Console.WriteLine("Please enter a valid input!");
                    break;
            }

        }
    }

    private void ViewHabitsList()
    {
        if (_habitsList.Count == 0)
        {
            Console.WriteLine("No habits to show!");
            return;
        }
        Console.Clear();
        Console.WriteLine("Habits List:");
        for (int i = 0; i < _habitsList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_habitsList[i].Name}");
        }
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public List<Habit> GetListFromDb()
    {
        List<Habit> list = new List<Habit>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM habits_list";

            SQLiteDataReader reader = tableCommand.ExecuteReader();

            while (reader.Read())
            {
                Habit habit = new Habit(reader.GetString(1), reader.GetString(2));
                list.Add(habit);
            }

            connection.Close();

        }


        return list;
    }
    public void AddHabit()
    {
        string habit = TrackerMenu.GetOption("Please enter the habit you want to add use underscore ( _ ) for spaces: ");
        string UnitOfMeasurement = TrackerMenu.GetOption("Please enter the unit of measurement all will: ");


        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @$"CREATE TABLE IF NOT EXISTS {habit} (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Date TEXT,
                                            {UnitOfMeasurement} Double
                                        )";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }

        InsertToHabitsListTable(habit, UnitOfMeasurement);

        _habitsList.Add(new Habit(habit, UnitOfMeasurement));

        Console.WriteLine($"Habit {habit} added successfully!");
    }

    public void RemoveHabit()
    {
        TrackerMenu.ManageHabitsMenu(_habitsList);

        int index = int.Parse(TrackerMenu.GetOption()) - 1;

        Habit habit = _habitsList[index];

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @$"DROP TABLE IF EXISTS {habit.Name}";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }

        _habitsList.RemoveAt(index);

        Console.WriteLine($"Habit {habit} removed successfully!");
    }

    public void ManageHabits()
    {

        Console.WriteLine(_habitsList.Count);
        int index = int.Parse(TrackerMenu.ManageHabitsMenu(_habitsList)) - 1;
        if (index < 0) return;
        while (index > _habitsList.Count - 1)
        {
            Console.WriteLine("Please enter a valid input!");
            index = int.Parse(TrackerMenu.ManageHabitsMenu(_habitsList)) - 1;
        }

        Habit habit = _habitsList[index];
        while (true)
        {
            string option = TrackerMenu.HabitMenu(habit);
            switch (option)
            {
                case "1":
                    AddEntry(habit);
                    break;
                case "2":
                    EditEntry(habit);
                    break;
                case "3":
                    DeleteEntry(habit);
                    break;
                case "4":
                    ViewAllEntries(habit);
                    break;
                case "0":
                    TrackerMenu.ManageHabitsMenu(_habitsList);
                    return;
                default:
                    Console.WriteLine("Please enter a valid input!");
                    option = TrackerMenu.HabitMenu(habit);
                    break;
            }
        }
    }

    private void ViewAllEntries(Habit habit)
    {
        Console.Clear();
        Console.WriteLine($"All {habit.Name} entries:");
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @$"SELECT * FROM {habit.Name}";

            SQLiteDataReader reader = tableCommand.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"Id: {reader.GetInt32(0)} Date: {reader.GetString(1)} {habit.UnitOfMeasurement}: {reader.GetDouble(2)}");
            }

            connection.Close();
        }
        Console.WriteLine("-------------------------------------------\n");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        return;

    }

    private void DeleteEntry(Habit habit)
    {
        ViewAllEntries(habit);
        int id = int.Parse(TrackerMenu.GetOption("Please enter the Id of the entry to delete"));

        Console.WriteLine("Are you sure you want to delete this entry? (y/n)");
        string option = Console.ReadLine().ToLower();
        while (option != "y" && option != "n")
        {
            Console.WriteLine("Please enter a valid input!");
            option = Console.ReadLine().ToLower();
        }

        if (option == "n") TrackerMenu.HabitMenu(habit);

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @$"DELETE FROM {habit.Name} WHERE Id = {id}";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }

        Console.WriteLine($"Entry {id} deleted successfully!");

        TrackerMenu.HabitMenu(habit);
    }

    private void EditEntry(Habit habit)
    {
        ViewAllEntries(habit);
        int id = int.Parse(TrackerMenu.GetOption("Please enter the Id of the entry to edit"));

        string date = GetDateInput();
        double measurement = GetMeasurementInput(habit.UnitOfMeasurement);

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @$"UPDATE {habit.Name} SET Date = '{date}', {habit.UnitOfMeasurement} = '{measurement}' WHERE Id = {id}";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }

        Console.WriteLine($"Entry {id} edited successfully!");

        TrackerMenu.HabitMenu(habit);
    }

    private void AddEntry(Habit habit)
    {
        string entryDate = GetDateInput();
        double entryMeasurement = GetMeasurementInput(habit.UnitOfMeasurement);

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @$"INSERT INTO {habit.Name} (Date, {habit.UnitOfMeasurement}) VALUES ('{entryDate}', '{entryMeasurement}')";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }

        Console.WriteLine($"Entry added successfully!");
        TrackerMenu.HabitMenu(habit);
    }

    private string GetDateInput()
    {
        string? date = null;

        while (date == null || !DateTime.TryParse(date, out _))
        {
            Console.WriteLine("Please enter the date in formet dd-mm-yyyy: ");
            date = Console.ReadLine();
        }

        return date;
    }

    private double GetMeasurementInput(string unitOfmeasurement)
    {
        Console.WriteLine($"Please enter the {unitOfmeasurement} for the day: ");
        double number = Convert.ToDouble(Console.ReadLine());
        return number;
    }


}

public record Habit(string Name, string UnitOfMeasurement);