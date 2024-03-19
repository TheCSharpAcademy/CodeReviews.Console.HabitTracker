using Microsoft.Data.Sqlite;

namespace HabbitLogger;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = @"Data Source=habitLogger.db";
        bool keepRunning = true;

        EnsureTableExists(connectionString);
        SeedData(connectionString);

        while (keepRunning)
        {
            ShowMenu();
            keepRunning = ProcessInput(connectionString);
        }
    }

    static void EnsureTableExists(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS habit (
                id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                quantity INT NOT NULL,
                unitOfMeassure TEXT NOT NULL
            );
        ";
        command.ExecuteNonQuery();
    }

    static void SeedData(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT count(*)
            FROM habit
        ";

        long count = (long)command.ExecuteScalar()!;

        if (count == 0)
        {
            command.CommandText =
            @"
                INSERT INTO habit
                VALUES (1, 'Drink water', 15, 'cup(s)'),
                       (2, 'Wake up', 0, 'times');
            ";
            command.ExecuteNonQuery();
        }
    }

    static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("MAIN MENU\n");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("Type 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");

        Console.Write("\nSelect option: ");
    }

    static bool ProcessInput(string connectionString)
    {
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int number))
        {
            switch (number)
            {
                case 0:
                    return false;
                case 1:
                    ShowAllRecords(connectionString);
                    Console.WriteLine("\nPress enter to return to the menu...");
                    Console.ReadLine();
                    return true;
                case 2:
                    InsertRecord(connectionString);
                    return true;
                case 3:
                    DeleteRecord(connectionString);
                    return true;
                case 4:
                    UpdateRecord(connectionString);
                    return true;
                default:
                    Console.WriteLine("\nUnknown action, press enter to try again...");
                    Console.ReadLine();
                    return true;
            }
        }
        else
        {
            Console.WriteLine("\nPlease enter a number, press enter to try again...");
            Console.ReadLine();
            return true;
        }
    }

    static void ShowAllRecords(string connectionString)
    {
        Console.Clear();
        Console.WriteLine("Your habits:\n");
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT name, quantity, unitOfMeassure
            FROM habit
        ";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader.GetString(0)}: {reader.GetString(1)} {reader.GetString(2)}.");
        }
    }

    static void InsertRecord(string connectionString)
    {
        Console.Clear();
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();

        Console.Write("New habit: ");
        string? name = Console.ReadLine();
        Console.Write("Unit of meassure: ");
        string? unitOfMeassure = Console.ReadLine();

        Console.Write("Quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity))
        {
            Console.WriteLine("\nInvalid quantity... Press enter to return to the menu...");
            Console.ReadLine();
            return;
        }

        command.CommandText =
        @"
            INSERT INTO habit (name, quantity, unitOfMeassure)
            VALUES ($name, $quantity, $unit)
        ";
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$quantity", quantity);
        command.Parameters.AddWithValue("$unit", unitOfMeassure);

        command.ExecuteNonQuery();

        Console.WriteLine("\nHabit added! Press enter to return to the menu...");
        Console.ReadLine();
    }

    static void DeleteRecord(string connectionString)
    {
        ShowAllRecords(connectionString);
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();

        Console.Write("\nName of habit to delete: ");
        var name = Console.ReadLine();

        command.CommandText =
        @"
            DELETE FROM habit 
            WHERE name = $name
        ";
        command.Parameters.AddWithValue("$name", name);

        int success = command.ExecuteNonQuery();

        string message = success == 1 ? "Habit deleted" : "Failed to delete";

        Console.WriteLine($"\n{message}! Press enter to return to the menu...");
        Console.ReadLine();
    }

    static void UpdateRecord(string connectionString)
    {
        ShowAllRecords(connectionString);
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();

        Console.Write("\nName of habit to update: ");
        string? name = Console.ReadLine();

        bool habitExists = CheckIfRecordExists(connectionString, name);

        if (!habitExists)
        {
            Console.WriteLine("\nCan't find habit... Press enter to return to the menu...");
            Console.ReadLine();
            return;
        }

        Console.Write("\nEnter quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity))
        {
            Console.WriteLine("\nInvalid quantity... Press enter to return to the menu...");
            Console.ReadLine();
            return;
        }

        command.CommandText =
        @"
            UPDATE habit 
            SET quantity = $value
            WHERE name = $name
        ";
        command.Parameters.AddWithValue("$value", quantity);
        command.Parameters.AddWithValue("$name", name);

        int success = command.ExecuteNonQuery();

        string message = success == 1 ? "Habit updated" : "Failed to update";

        Console.WriteLine($"\n{message}! Press enter to return to the menu...");
        Console.ReadLine();
    }

    static bool CheckIfRecordExists(string connectionString, string? name)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT COUNT(*) 
            FROM habit
            WHERE name = $name
        ";
        command.Parameters.AddWithValue("$name", name);

        int count = Convert.ToInt32(command.ExecuteScalar());

        return count > 0;
    }
}