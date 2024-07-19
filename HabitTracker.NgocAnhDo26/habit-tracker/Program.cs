using System.Globalization;
using Microsoft.Data.Sqlite;

public class Program
{
    static readonly string ConnectionString = "Data Source=habit-Tracker.db";
    public static void Main(string[] args)
    {
        CreateTable();

        if (TableIsEmpty())
            SeedData();

        var running = true;
        while (running)
        {
            Console.Clear();
            PrintMenu();

            running = MenuController();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

    }

    static void PrintMenu()
    {
        Console.WriteLine("".PadLeft(30, '-'));
        Console.WriteLine("\t MAIN MENU\n");
        Console.WriteLine("1. View all tracked habits.");
        Console.WriteLine("2. View all records.");
        Console.WriteLine();
        Console.WriteLine("3. Insert a new log.");
        Console.WriteLine("4. Delete a log.");
        Console.WriteLine("5. Update a log.");
        Console.WriteLine();
        Console.WriteLine("6. Insert a new habit.");
        Console.WriteLine("7. Delete a tracked habit.");
        Console.WriteLine("8. Update a tracked habit.");
        Console.WriteLine();
        Console.WriteLine("9. Habits Report");
        Console.WriteLine();
        Console.WriteLine("0. Close application.");
        Console.WriteLine("".PadLeft(30, '-'));
        Console.WriteLine();
    }

    static bool MenuController()
    {
        Console.Write("<*> Your choice is: ");
        var option = Console.ReadLine();
        Console.Clear();

        switch (option)
        {
            case "1":
                RetrieveAllRecords(true);
                break;
            case "2":
                RetrieveAllRecords();
                break;
            case "3":
                Insert();
                break;
            case "4":
                Delete();
                break;
            case "5":
                Update();
                break;
            case "6":
                Insert(true);
                break;
            case "7":
                Delete(true);
                break;
            case "8":
                Update(true);
                break;
            case "9":
                Report();
                break;
            case "0":
                Console.Write("Exiting the application...");
                return false;
            default:
                Console.WriteLine("Invalid option!");
                break;
        }

        return true;
    }

    static bool TableIsEmpty()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Habits";

        var rows = Convert.ToInt32(command.ExecuteScalar());

        return rows == 0;
    }

    static void CreateTable()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Habits (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT,
            Unit TEXT
            )";

            command.ExecuteNonQuery();

            command.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Habits_Logs (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            Habit_ID INTEGER,
            Date TEXT,
            Quantity INTEGER,
            FOREIGN KEY(Habit_ID) REFERENCES Habits(ID)
            )";

            command.ExecuteNonQuery();
        }
    }

    static void SeedData()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Habits (Name, Unit) VALUES
            ('Drinking Water', 'Cup(s)'),
            ('Coding', 'Minute(s)'),
            ('Reading books', 'Page(s)')";

            command.ExecuteNonQuery();

            Random random = new Random();
            for (int i = 0; i < 100; ++i)
            {
                var habitID = random.Next(1, 4);
                var date = new DateOnly(2024, random.Next(1, 13), random.Next(1, 29));
                var quantity = (habitID == 1) ? random.Next(1, 10) : ((habitID == 2) ? random.Next(15, 120) : random.Next(2, 50));

                command.CommandText = @"INSERT INTO Habits_Logs 
                (Habit_ID, Date, Quantity) VALUES 
                (@habitID, @date, @quantity)";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@habitID", habitID);
                command.Parameters.AddWithValue("@date", date.ToString("MM-dd-yyyy"));
                command.Parameters.AddWithValue("@quantity", quantity);

                command.ExecuteNonQuery();
            }
        }
    }

    static DateOnly GetDateValidation()
    {
        Console.Write("Enter date (format: MM-dd-yyyy): ");

        DateOnly date;
        while (!DateOnly.TryParseExact(Console.ReadLine(), "MM-dd-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out date))
        {
            Console.WriteLine("Invalid date format! Please try again!");
            Console.Write("\nEnter date (format: MM-dd-yyyy): ");
        }

        return date;
    }

    static int GetNumberValidation(string message)
    {
        int number;
        Console.Write(message);

        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Invalid number! Please try again!");
                Console.Write(message);
                continue;
            }
            else if (number < 0)
            {
                Console.WriteLine("Must be a positive number! Please try again!");
                Console.Write(message);
                continue;
            }

            break;
        }

        return number;
    }

    static void RetrieveAllRecords(bool habit = false)
    {
        Console.WriteLine("\tALL RECORDS\n");

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();

            if (habit)
            {
                command.CommandText = "SELECT * FROM Habits";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var habitName = reader.GetString(1);
                    var unit = reader.GetString(2);

                    Console.WriteLine($"ID: {id} - {habitName} - Unit: {unit}");
                }
            }
            else
            {
                command.CommandText =
                @"SELECT Habits.Name, Habits.Unit, Habits_Logs.ID, Habits_Logs.Date, Habits_Logs.Quantity 
            FROM Habits_Logs JOIN Habits ON (Habits_Logs.Habit_ID = Habits.ID)";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var habitName = reader.GetString(0);
                    var unit = reader.GetString(1);
                    var id = reader.GetInt32(2);
                    var date = reader.GetDateTime(3);
                    var quantity = reader.GetInt32(4);

                    Console.WriteLine($"ID: {id} - {habitName} - {date.ToString("MM-dd-yyy")} - {quantity} {unit}");
                }
            }
        }
    }

    static void Insert(bool habit = false)
    {
        Console.WriteLine("\tINSERT\n");

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();

            if (habit)
            {
                Console.Write("Enter new habit's name: ");
                var habitName = Console.ReadLine();

                Console.Write("Enter measurement unit: ");
                var unit = Console.ReadLine();

                command.CommandText = $"INSERT INTO Habits (Name, Unit) VALUES ('{habitName}', '{unit}')";
            }
            else
            {
                RetrieveAllRecords(true);

                var id = GetNumberValidation("Enter habit's ID: ");
                var date = GetDateValidation();
                var quantity = GetNumberValidation("Enter quantity (must be a number): ");

                command.CommandText = $"INSERT INTO Habits_Logs (Habit_ID, Date, Quantity) VALUES ({id}, '{date.ToString("MM-dd-yyyy")}', {quantity})";
            }

            command.ExecuteNonQuery();

        }

        Console.WriteLine("Insert new record successfully!");
    }

    static void Delete(bool habit = false)
    {
        RetrieveAllRecords(habit);

        Console.WriteLine("\nDELETE A RECORD\n");

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            var id = GetNumberValidation("Enter ID: ");

            if (habit)
            {
                command.CommandText = $"DELETE FROM Habits_Logs WHERE Habit_ID = {id}";
                command.ExecuteNonQuery();
            }

            command.CommandText = $"DELETE FROM {(habit ? "Habits" : "Habits_Logs")} WHERE ID={id}";

            if (command.ExecuteNonQuery() == 0)
            {
                Console.WriteLine($"There is no record matched ID={id}");
                return;
            }

            Console.WriteLine($"Delete record ID={id} successfully!");
        }
    }

    static void Update(bool habit = false)
    {
        RetrieveAllRecords(habit);

        Console.WriteLine("\nUPDATE A RECORD\n");

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            var id = GetNumberValidation("Enter ID: ");

            if (habit)
            {
                Console.Write("Enter new habit's name: ");
                var habitName = Console.ReadLine();

                Console.Write("Enter new habit's unit: ");
                var unit = Console.ReadLine();

                command.CommandText = @$"
                UPDATE Habits
                SET Name = '{habitName}',
                    Unit = '{unit}'
                WHERE ID={id}";
            }
            else
            {
                var date = GetDateValidation();
                var quantity = GetNumberValidation("Enter new quantity (must be a number): ");

                command.CommandText = @$"
                UPDATE Habits_Logs
                SET Date = '{date.ToString("MM-dd-yyyy")}',
                    Quantity = {quantity}
                WHERE ID={id}";
            }

            if (command.ExecuteNonQuery() == 0)
            {
                Console.WriteLine($"There is no record matched ID={id}");
                return;
            }

            Console.WriteLine($"Update record ID={id} successfully!");
        }
    }

    static void Report()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"
            SELECT Habits.ID, Habits.Name, Habits.Unit, SUM(Habits_Logs.Quantity)  
            FROM Habits JOIN Habits_Logs ON (Habits_Logs.Habit_ID = Habits.ID)
            GROUP BY Habits.ID";

            Console.WriteLine("\tALL TIME FIGURES\n");

            try 
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var habitName = reader.GetString(1);
                    var unit = reader.GetString(2);
                    var sum = reader.GetInt32(3);

                    Console.WriteLine($"ID: {id} - {habitName} - Total: {sum} {unit}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}


