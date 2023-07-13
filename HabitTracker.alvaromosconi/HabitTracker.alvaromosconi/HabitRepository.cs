using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTracker.alvaromosconi;

internal class HabitRepository
{
    private const string TABLE_NAME = "habits";
    private const string CONNECTION_STRING = @"Data Source=habit_tracker.db";
    private List<Habit> tableData = new();

    public HabitRepository()
    {
        CreateTable();
        SaveAllRecordsInCache();
    }

    internal void CreateTable()
    {
        using (var connection = new SqliteConnection(CONNECTION_STRING))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                $@"
                        CREATE table IF NOT EXISTS {TABLE_NAME} 
                            (id INTEGER PRIMARY KEY AUTOINCREMENT,
                             name TEXT,
                             date TEXT,
                             quantity INTEGER
                            )
                    ";

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    private void SaveAllRecordsInCache()
    {
        Console.Clear();


        using (var connection = new SqliteConnection(CONNECTION_STRING))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                $@"
                            SELECT * FROM {TABLE_NAME}
                        ";

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                tableData.Add(
                    new Habit
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(3),
                    });
            }

            connection.Close();
        }

    }

    internal void ExecuteOption(string userInput)
    {
        switch (userInput)
        {
            case "1":
                PrintAllRecords();
                break;
            case "2":
                InsertNewRecord();
                break;
            case "3":
                UpdateAnExistingRecord();
                break;
            case "4":
                DeleteAnExistingRecord();
                break;
        }

        PrintBackOption();
    }

    private void PrintBackOption()
    {

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }

    private void PrintAllRecords()
    {

        var groupedHabits = tableData.GroupBy(habit => habit.Name);

        foreach (var group in groupedHabits)
        {
            Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.DarkBlue; 
            Console.WriteLine($"\nGroup: {group.Key}\n");
            Console.ResetColor();
            Console.WriteLine("\nId     Date        Quantity");
            Console.WriteLine("---------------------------");
            foreach (Habit habit in group)
            {
                Console.WriteLine($"{habit.Id,-4} {habit.Date,-15:dd-MM-yy} {habit.Quantity}");
            }

            Console.WriteLine();
        }
    }


    internal void InsertNewRecord()
    {
        List<string> existingHabitNames = GetExistingHabitNames();

        if (existingHabitNames.Count == 0)
        {
            Insert(null);
        }
        else
        {
            Console.WriteLine("Please type:");
            Console.WriteLine("\n 0. To back. \n 1. To create a new habit. \n 2. To register a new record in an existing habit.");
            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "0":
                    break;
                case "1":
                    Console.Clear();
                    Insert(null);
                    break;
                case "2":
                    Console.Clear();
                    PrintAllExistingHabitNames();
                    SelectExistingHabitNameAndInsert(existingHabitNames);
                    break;
                default:
                    break;
            }

        }
    }

    private void SelectExistingHabitNameAndInsert(List<string> existingHabitNames)
    {
        
        string habitName = String.Empty;
        bool exists = false;
        do
        {
            habitName = GetNameInput("\nInsert the name of an existing habit. Type 0 to back.");

            if (habitName == "0")
                break;

            exists = existingHabitNames.Contains(habitName);

            if (!exists)
            {
                Console.Clear();
                PrintAllExistingHabitNames();
                Console.WriteLine("\n That name doesn't match with any record!");
            }
            else
                Insert(habitName);

        } while (!exists);
    }

    private List<string> GetExistingHabitNames()
    {
        Console.Clear();
        List<string> existingHabitNames = new List<string>();

        foreach (Habit habit in tableData)
        {
            existingHabitNames.Add(habit.Name);
        }

        return existingHabitNames;
    }

    private void Insert(string? name)
    {

        if (name == null)
        {
            name = GetNameInput(null);
        }

        string dateInput = GetDateInput(null);
        int quantityInput = GetQuantityInput(null);

        using (var connection = new SqliteConnection(CONNECTION_STRING))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                $@"
                        INSERT INTO {TABLE_NAME}(name, date, quantity) 
                        VALUES('{name}', '{dateInput}', {quantityInput});
                        SELECT last_insert_rowid();
                    ";

            int newRecordId = Convert.ToInt32(command.ExecuteScalar());
            command.ExecuteNonQuery();

            tableData.Add(
            new Habit
                {
                    Id = newRecordId,
                    Name = name,
                    Date = DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US")),
                    Quantity = quantityInput
                }
            );

            connection.Close();
        }
    }

    private string GetNameInput(string? message)
    {
        string userInput = String.Empty;
        string messageToPrint = message != null ? message : "\nPlease enter the name of the new habit.";

        do
        {
            Console.WriteLine(messageToPrint);
            userInput = Console.ReadLine();

        } while (String.IsNullOrEmpty(userInput));

        return userInput;
    }

    private string GetDateInput(string? message)
    {
        string userInput = string.Empty;
        bool isValidFormat = false;
        string messageToPrint = message != null ? message : "\nPlease enter the date: (Format: dd-mm-yy) for the new record. Type 0 to return to the main menu.";

        do
        {
            Console.WriteLine(messageToPrint);
            userInput = Console.ReadLine();

            if (userInput == "0")
            {
                Program.GetUserInput();
                return string.Empty;
            }

            string dateFormatPattern = @"^\d{2}-\d{2}-\d{2}$";
            if (Regex.IsMatch(userInput, dateFormatPattern))
            {
                isValidFormat = true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid date format. Please try again.");
            }
        }
        while (!isValidFormat);

        return userInput;
    }

    private int GetQuantityInput(string? message)
    {
        string userInput = String.Empty;
        int quantity;
        string messageToPrint = message != null ? message : "\nPlease enter the quantity for the new record.";
        do
        {
            Console.WriteLine(messageToPrint);
            userInput = Console.ReadLine();

        } while (!Int32.TryParse(userInput, out quantity));

        return quantity;
    }

    private void PrintAllExistingHabitNames()
    {
        Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("\nExisting Habit Names:\n");
        int counter = 0;
        foreach (Habit habit in tableData)
        {
            counter++;
            Console.WriteLine($"{counter}. {habit.Name}");
        }
        Console.ResetColor();
    }

    internal void DeleteAnExistingRecord()
    {
        int recordId = GetIdInput("\nPlease type the Id of the record that you want to delete or type 0 to go back to Main Menu\n");

        if (recordId != 0)
        {
            using (var connection = new SqliteConnection(@"Data Source=habit_tracker.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    $@"
                            DELETE from {TABLE_NAME} WHERE id = {recordId};       
                        ";

                int rowCount = command.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"\nRecord with Id {recordId} doesn't exist. \n ");
                    DeleteAnExistingRecord();
                }
                else
                { 
                    tableData.RemoveAll(habit => habit.Id == recordId);
                    Console.WriteLine($"\n Record with Id {recordId} was deleted. \n");
                }
            }

        }
    }

    private int GetIdInput(string message)
    {
        string userInput = String.Empty;
        int quantity;

        do
        {
            Console.WriteLine(message);
            userInput = Console.ReadLine();

        } while (!Int32.TryParse(userInput, out quantity));

        return quantity;
    }

    internal void UpdateAnExistingRecord()
    {
        int recordId = GetIdInput("\nPlease type the Id of the record that you want to update or type 0 to go back to Main Menu\n");

        if (recordId != 0)
        {
            using (var connection = new SqliteConnection(CONNECTION_STRING))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT EXISTS(SELECT 1 FROM {TABLE_NAME} WHERE id = {recordId})";
                int checkQuery = Convert.ToInt32(command.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n Record with Id {recordId} doesn't exist.\n");
                    connection.Close();
                    UpdateAnExistingRecord();
                }

                string dateInput = GetDateInput("\nPlease type the new date\n");
                int quantityInput = GetQuantityInput("\nPlease type the new quantity for this record");

                command = connection.CreateCommand();
                command.CommandText = $"UPDATE {TABLE_NAME} SET date = '{dateInput}', quantity = {quantityInput} WHERE id = {recordId}";
                command.ExecuteNonQuery();
                Habit habitToUpdate = tableData.FirstOrDefault(habit => habit.Id == recordId);
                if (habitToUpdate != null)
                {
                    habitToUpdate.Date = DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"));
                    habitToUpdate.Quantity = quantityInput;
                }

                connection.Close();
            }
        }
    }






   



}
