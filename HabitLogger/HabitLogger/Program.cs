using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ConsoleHabitLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseHelper.InitializeDatabase();

            while (true)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Create Habit");
                Console.WriteLine("2. Add Habit Log");
                Console.WriteLine("3. View Habit Logs");
                Console.WriteLine("4. Update Habit Log");
                Console.WriteLine("5. Delete Habit Log");
                Console.WriteLine("6. Dashboard"); // New option for dashboard
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateHabit();
                        break;
                    case "2":
                        AddHabitLog();
                        break;
                    case "3":
                        ViewHabitLog();
                        break;
                    case "4":
                        UpdateHabitLog();
                        break;
                    case "5":
                        DeleteHabitLog();
                        break;
                    case "6":
                        Dashboard.DisplayDashboard(); // Call the new dashboard method
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            }
        }

        private static void CreateHabit()
        {
            Console.Write("Enter habit name: ");
            var name = Console.ReadLine();

            Console.Write("Enter unit of measurement: ");
            var unit = Console.ReadLine();

            try
            {
                using (var connection = new SqliteConnection("Data Source=habit_tracker.db"))
                {
                    connection.Open();
                    var insertCommand = new SqliteCommand();
                    insertCommand.Connection = connection;

                    insertCommand.CommandText = "INSERT INTO Habits (Name, Unit) VALUES (@name, @unit)";
                    insertCommand.Parameters.AddWithValue("@name", name);
                    insertCommand.Parameters.AddWithValue("@unit", unit);

                    insertCommand.ExecuteNonQuery();
                    Console.WriteLine("Habit created successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void AddHabitLog()
        {
            Dashboard.DisplayHabits(); // Display habits before prompting for habit ID

            Console.Write("Enter habit ID: ");
            if (int.TryParse(Console.ReadLine(), out int habitId))
            {
                string? date;
                while (true)
                {
                    Console.Write("Enter Date (DD-MM-YYYY): ");
                    var inputDate = Console.ReadLine();
                    if (DateTime.TryParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        date = parsedDate.ToString("yyyy-MM-dd"); // Format date correctly for SQLite
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter the date in DD-MM-YYYY format.");
                    }
                }

                Console.Write("Enter quantity: ");
                if (int.TryParse(Console.ReadLine(), out int quantity))
                {
                    try
                    {
                        using (var connection = new SqliteConnection("Data Source=habit_tracker.db"))
                        {
                            connection.Open();
                            var insertCommand = new SqliteCommand();
                            insertCommand.Connection = connection;

                            insertCommand.CommandText = "INSERT INTO HabitLog (HabitId, Date, Quantity) VALUES (@habitId, @date, @quantity)";
                            insertCommand.Parameters.AddWithValue("@habitId", habitId);
                            insertCommand.Parameters.AddWithValue("@date", date);
                            insertCommand.Parameters.AddWithValue("@quantity", quantity);

                            insertCommand.ExecuteNonQuery();
                            Console.WriteLine("Habit log added successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid quantity. Please enter a number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid habit ID. Please enter a number.");
            }
        }

        private static void ViewHabitLog()
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=habit_tracker.db"))
                {
                    connection.Open();
                    var selectCommand = new SqliteCommand(
                        @"SELECT HabitLog.Id, Habits.Name, HabitLog.Date, HabitLog.Quantity, Habits.Unit 
                      FROM HabitLog 
                      JOIN Habits ON HabitLog.HabitId = Habits.Id", connection);

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        Console.WriteLine("Id  | Habit Name | Date       | Quantity | Unit");
                        Console.WriteLine("--------------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Id"],-3} | {reader["Name"],-10} | {reader["Date"],-10} | {reader["Quantity"],-8} | {reader["Unit"],-4}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        private static void UpdateHabitLog()
        {
            Dashboard.DisplayHabits(); // Display habits before prompting for habit ID

            Console.Write("Enter the Habit ID for which you want to update a log: ");
            if (int.TryParse(Console.ReadLine(), out int habitId))
            {
                Dashboard.DisplayLogsForHabit(habitId); // Display logs for the chosen habit

                Console.Write("Enter the Log ID of the habit log to update: ");
                if (int.TryParse(Console.ReadLine(), out int logId))
                {
                    string? date;
                    while (true)
                    {
                        Console.Write("Enter new Date (DD-MM-YYYY): ");
                        var inputDate = Console.ReadLine();
                        if (DateTime.TryParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                        {
                            date = parsedDate.ToString("yyyy-MM-dd"); // Format date correctly for SQLite
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format. Please enter the date in DD-MM-YYYY format.");
                        }
                    }

                    Console.Write("Enter new quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        try
                        {
                            using (var connection = new SqliteConnection("Data Source=habit_tracker.db"))
                            {
                                connection.Open();
                                var updateCommand = new SqliteCommand();
                                updateCommand.Connection = connection;

                                updateCommand.CommandText = "UPDATE HabitLog SET Date = @date, Quantity = @quantity WHERE Id = @logId";
                                updateCommand.Parameters.AddWithValue("@date", date);
                                updateCommand.Parameters.AddWithValue("@quantity", quantity);
                                updateCommand.Parameters.AddWithValue("@logId", logId);

                                int rowsAffected = updateCommand.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine("Habit log updated successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("No habit log found with the given Log ID.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity. Please enter a number.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Log ID. Please enter a number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Habit ID. Please enter a number.");
            }
        }


        private static void DeleteHabitLog()
        {
            Console.Write("Enter the Id of the habit log to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    using (var connection = new SqliteConnection("Data Source=habit_tracker.db"))
                    {
                        connection.Open();
                        var deleteCommand = new SqliteCommand();
                        deleteCommand.Connection = connection;

                        deleteCommand.CommandText = "DELETE FROM HabitLog WHERE Id = @id";
                        deleteCommand.Parameters.AddWithValue("@id", id);

                        int rowsAffected = deleteCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Habit log deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("No habit log found with the given Id.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Id. Please enter a number.");
            }

        }
        
        


    }
}