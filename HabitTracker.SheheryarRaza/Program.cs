using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using HabitTracker.SheheryarRaza;

class Program
{
    // Represents a single habit record
    public class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    static void Main(string[] args)
    {
        DBContext.InitializeDatabase();

        while (true)
        {
            Console.WriteLine("\n--- Habit Tracker Menu ---");
            Console.WriteLine("1. View All Habits");
            Console.WriteLine("2. Add New Habit Record");
            Console.WriteLine("3. Update Habit Record");
            Console.WriteLine("4. Delete Habit Record");
            Console.WriteLine("5. Show Reports");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1": ViewHabits(); break;
                case "2": AddHabit(); break;
                case "3": UpdateHabit(); break;
                case "4": DeleteHabit(); break;
                case "5": ShowReports(); break;
                case "6":
                    Console.WriteLine("Exiting Habit Tracker. Goodbye!");
                    return;
                default: Console.WriteLine("Invalid input. Please choose a number from the menu."); break;
            }
        }
    }

    static List<Habit> DisplayAllHabits(string title = "Habit Logs")
    {
        var habits = new List<Habit>();
        using (var connection = new SqliteConnection(DBContext.GetConnectionString()))
        {
            try
            {
                connection.Open();
                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT Id, Name, Quantity, Unit, CreatedAt FROM Habits ORDER BY CreatedAt ASC;";

                using (var reader = selectCommand.ExecuteReader())
                {
                    Console.WriteLine($"\n--- {title} ---");

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No habits found.\n");
                        return null; // Return null to indicate no habits were found
                    }

                    // Print header for better readability
                    Console.WriteLine("----------------------------------------------------------------------------------");
                    Console.WriteLine($"{"ID",-5} | {"Name",-20} | {"Quantity",-20} | {"Logged At",-30}");
                    Console.WriteLine("----------------------------------------------------------------------------------");


                    while (reader.Read())
                    {
                        var habit = new Habit
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Quantity = reader.GetInt32(2),
                            Unit = reader.GetString(3),
                            CreatedAt = reader.GetDateTime(4)
                        };
                        habits.Add(habit);
                        // Improved formatting using string padding
                        Console.WriteLine($"{habit.Id,-5} | {habit.Name,-20} | {habit.Quantity} {habit.Unit,-17} | {habit.CreatedAt:yyyy-MM-dd HH:mm,-30}");
                    }
                    Console.WriteLine("----------------------------------------------------------------------------------");
                    Console.WriteLine(); // extra line spacing
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"An error occurred while retrieving habits: {ex.Message}");
                return null;
            }
        }
        return habits;
    }


    static DateTime? GetDateInput(string prompt)
    {
        DateTime date;
        while (true)
        {
            Console.Write($"{prompt} (Type 'today' for current date, or YYYY-MM-DD HH:MM): ");
            string input = Console.ReadLine().Trim().ToLower();

            if (input == "today")
            {
                return DateTime.Now;
            }
            else if (DateTime.TryParse(input, out date))
            {
                return date;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please use YYYY-MM-DD HH:MM or type 'today'.");
            }
        }
    }


    static int GetIntInput(string prompt, int minValue = 1)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value) && value >= minValue)
            {
                return value;
            }
            else
            {
                Console.WriteLine($"Invalid input. Please enter a number greater than or equal to {minValue}.");
            }
        }
    }


    static void AddHabit()
    {
        Console.WriteLine("\n--- Add New Habit Record ---");

        Console.Write("Enter The Name of Habit: ");
        string name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Habit name cannot be empty.");
            return;
        }

        int quantity = GetIntInput("Enter quantity (number): ");

        Console.Write("Enter the unit of measurement (e.g., 'glasses', 'km', 'pages'): ");
        string unit = Console.ReadLine();
        if (string.IsNullOrEmpty(unit))
        {
            Console.WriteLine("Unit of measurement cannot be empty.");
            return;
        }

        DateTime? createdAt = GetDateInput("Enter the date and time of occurrence");
        if (!createdAt.HasValue)
        {
            Console.WriteLine("Invalid date entered. Habit not added.");
            return;
        }

        try
        {
            using (var connection = new SqliteConnection(DBContext.GetConnectionString()))
            {
                connection.Open();
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
                    INSERT INTO Habits (Name, Quantity, Unit, CreatedAt)
                    VALUES (@name, @quantity, @unit, @createdAt);
                ";
                insertCommand.Parameters.AddWithValue("@name", name);
                insertCommand.Parameters.AddWithValue("@quantity", quantity);
                insertCommand.Parameters.AddWithValue("@unit", unit);
                insertCommand.Parameters.AddWithValue("@createdAt", createdAt.Value);

                insertCommand.ExecuteNonQuery();
                Console.WriteLine("Habit record added successfully.\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while adding the habit: {ex.Message}");
        }
    }


    static void ViewHabits()
    {
        DisplayAllHabits();
    }


    static void UpdateHabit()
    {
        Console.WriteLine("\n--- Update Habit Record ---");
        var habits = DisplayAllHabits("Existing Habits to Update");
        if (habits == null || habits.Count == 0)
        {
            return; // No habits to update
        }

        int habitId = GetIntInput("Enter the ID of the habit you want to update: ");

        // Check if the habit ID exists
        var existingHabit = habits.Find(h => h.Id == habitId);
        if (existingHabit == null)
        {
            Console.WriteLine("No habit found with that ID.");
            return;
        }

        Console.WriteLine($"Updating habit: ID: {existingHabit.Id} | Name: {existingHabit.Name} | Quantity: {existingHabit.Quantity} {existingHabit.Unit} | Logged At: {existingHabit.CreatedAt:yyyy-MM-dd HH:mm}");

        Console.Write($"Enter new name (current: {existingHabit.Name}, leave blank to keep current): ");
        string newName = Console.ReadLine();

        Console.Write($"Enter new quantity (current: {existingHabit.Quantity}, leave blank to keep current): ");
        string qtyInput = Console.ReadLine();
        int? newQty = null;
        if (int.TryParse(qtyInput, out int parsedQty) && parsedQty >= 1)
        {
            newQty = parsedQty;
        }
        else if (!string.IsNullOrWhiteSpace(qtyInput))
        {
            Console.WriteLine("Invalid quantity input. Keeping current quantity.");
        }

        Console.Write($"Enter new unit (current: {existingHabit.Unit}, leave blank to keep current): ");
        string newUnit = Console.ReadLine();

        DateTime? newCreatedAt = GetDateInput($"Enter new date and time (current: {existingHabit.CreatedAt:yyyy-MM-dd HH:mm}, leave blank to keep current)");

        try
        {
            using (var connection = new SqliteConnection(DBContext.GetConnectionString()))
            {
                connection.Open();
                string updateQuery = "UPDATE Habits SET ";
                bool hasSet = false;
                var updateCmd = connection.CreateCommand();

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    updateQuery += "Name = @newName";
                    updateCmd.Parameters.AddWithValue("@newName", newName);
                    hasSet = true;
                }

                if (newQty.HasValue)
                {
                    if (hasSet) updateQuery += ", ";
                    updateQuery += "Quantity = @newQty";
                    updateCmd.Parameters.AddWithValue("@newQty", newQty.Value);
                    hasSet = true;
                }

                if (!string.IsNullOrWhiteSpace(newUnit))
                {
                    if (hasSet) updateQuery += ", ";
                    updateQuery += "Unit = @newUnit";
                    updateCmd.Parameters.AddWithValue("@newUnit", newUnit);
                    hasSet = true;
                }

                if (newCreatedAt.HasValue)
                {
                    if (hasSet) updateQuery += ", ";
                    updateQuery += "CreatedAt = @newCreatedAt";
                    updateCmd.Parameters.AddWithValue("@newCreatedAt", newCreatedAt.Value);
                    hasSet = true;
                }

                if (!hasSet)
                {
                    Console.WriteLine("Nothing to update. No changes made.");
                    return;
                }

                updateQuery += " WHERE Id = @id;";
                updateCmd.Parameters.AddWithValue("@id", habitId);
                updateCmd.CommandText = updateQuery;

                int rowsAffected = updateCmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0
                    ? "Habit record updated successfully.\n"
                    : "No habit found with that ID.\n");
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"An error occurred while updating the database: {ex.Message}");
        }
    }


    static void DeleteHabit()
    {
        Console.WriteLine("\n--- Delete Habit Record ---");
        var habits = DisplayAllHabits("Existing Habits to Delete");
        if (habits == null || habits.Count == 0)
        {
            return; // No habits to delete
        }

        int idToDelete = GetIntInput("Enter the ID of the habit you want to delete: ");

        // Optional: Confirm deletion
        Console.Write($"Are you sure you want to delete habit ID {idToDelete}? (yes/no): ");
        string confirmation = Console.ReadLine().Trim().ToLower();
        if (confirmation != "yes")
        {
            Console.WriteLine("Deletion cancelled.");
            return;
        }

        try
        {
            using (var connection = new SqliteConnection(DBContext.GetConnectionString()))
            {
                connection.Open();
                var deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = "DELETE FROM Habits WHERE Id = @id;";
                deleteCmd.Parameters.AddWithValue("@id", idToDelete);

                int rowsAffected = deleteCmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0
                    ? "Habit record deleted successfully.\n"
                    : "No habit found with that ID.\n");
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"An error occurred while deleting the habit: {ex.Message}");
        }
    }


    static void ShowReports()
    {
        while (true)
        {
            Console.WriteLine("\n--- Reports Menu ---");
            Console.WriteLine("1. Total Quantity for a Specific Habit");
            Console.WriteLine("2. Total Quantity for a Specific Habit in a Given Year");
            Console.WriteLine("3. Total Quantity for a Specific Habit in a Given Month");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("Choose a report option: ");

            string reportInput = Console.ReadLine();
            switch (reportInput)
            {
                case "1": ReportTotalQuantityForHabit(); break;
                case "2": ReportTotalQuantityForHabitInYear(); break;
                case "3": ReportTotalQuantityForHabitInMonth(); break;
                case "4": return; // Go back to main menu
                default: Console.WriteLine("Invalid report option."); break;
            }
        }
    }


    static void ReportTotalQuantityForHabit()
    {
        Console.WriteLine("\n--- Total Quantity for a Specific Habit ---");
        var habits = DisplayAllHabits("Available Habits for Report");
        if (habits == null || habits.Count == 0)
        {
            return;
        }

        Console.Write("Enter the Name of the habit for the report: ");
        string habitName = Console.ReadLine().Trim();

        if (string.IsNullOrEmpty(habitName))
        {
            Console.WriteLine("Habit name cannot be empty.");
            return;
        }

        try
        {
            using (var connection = new SqliteConnection(DBContext.GetConnectionString()))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT SUM(Quantity), Unit
                    FROM Habits
                    WHERE Name = @name
                    GROUP BY Unit;
                ";
                command.Parameters.AddWithValue("@name", habitName);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"\nReport for '{habitName}':");
                        while (reader.Read())
                        {
                            long totalQuantity = reader.GetInt64(0); // SUM returns long
                            string unit = reader.GetString(1);
                            Console.WriteLine($"Total: {totalQuantity} {unit}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No records found for habit '{habitName}'.");
                    }
                    Console.WriteLine();
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"An error occurred while generating the report: {ex.Message}");
        }
    }


    static void ReportTotalQuantityForHabitInYear()
    {
        Console.WriteLine("\n--- Total Quantity for a Specific Habit in a Given Year ---");
        var habits = DisplayAllHabits("Available Habits for Report");
        if (habits == null || habits.Count == 0)
        {
            return;
        }

        Console.Write("Enter the Name of the habit for the report: ");
        string habitName = Console.ReadLine().Trim();

        if (string.IsNullOrEmpty(habitName))
        {
            Console.WriteLine("Habit name cannot be empty.");
            return;
        }

        int year = GetIntInput("Enter the year (e.g., 2023): ", 1900); // Assuming years from 1900 onwards

        try
        {
            using (var connection = new SqliteConnection(DBContext.GetConnectionString()))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT SUM(Quantity), Unit
                    FROM Habits
                    WHERE Name = @name AND STRFTIME('%Y', CreatedAt) = @year
                    GROUP BY Unit;
                ";
                command.Parameters.AddWithValue("@name", habitName);
                command.Parameters.AddWithValue("@year", year.ToString());

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"\nReport for '{habitName}' in {year}:");
                        while (reader.Read())
                        {
                            long totalQuantity = reader.GetInt64(0);
                            string unit = reader.GetString(1);
                            Console.WriteLine($"Total: {totalQuantity} {unit}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No records found for habit '{habitName}' in {year}.");
                    }
                    Console.WriteLine();
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"An error occurred while generating the report: {ex.Message}");
        }
    }


    static void ReportTotalQuantityForHabitInMonth()
    {
        Console.WriteLine("\n--- Total Quantity for a Specific Habit in a Given Month ---");
        var habits = DisplayAllHabits("Available Habits for Report");
        if (habits == null || habits.Count == 0)
        {
            return;
        }

        Console.Write("Enter the Name of the habit for the report: ");
        string habitName = Console.ReadLine().Trim();

        if (string.IsNullOrEmpty(habitName))
        {
            Console.WriteLine("Habit name cannot be empty.");
            return;
        }

        int year = GetIntInput("Enter the year (e.g., 2023): ", 1900);
        int month = GetIntInput("Enter the month number (1-12): ", 1);
        if (month < 1 || month > 12)
        {
            Console.WriteLine("Invalid month number. Please enter a number between 1 and 12.");
            return;
        }

        try
        {
            using (var connection = new SqliteConnection(DBContext.GetConnectionString()))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT SUM(Quantity), Unit
                    FROM Habits
                    WHERE Name = @name AND STRFTIME('%Y', CreatedAt) = @year AND STRFTIME('%m', CreatedAt) = @month
                    GROUP BY Unit;
                ";
                command.Parameters.AddWithValue("@name", habitName);
                command.Parameters.AddWithValue("@year", year.ToString());
                command.Parameters.AddWithValue("@month", month.ToString("D2")); // "D2" for two digits (e.g., 01, 02)

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"\nReport for '{habitName}' in {new DateTime(year, month, 1):MMMM yyyy}:");
                        while (reader.Read())
                        {
                            long totalQuantity = reader.GetInt64(0);
                            string unit = reader.GetString(1);
                            Console.WriteLine($"Total: {totalQuantity} {unit}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No records found for habit '{habitName}' in {new DateTime(year, month, 1):MMMM yyyy}.");
                    }
                    Console.WriteLine();
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"An error occurred while generating the report: {ex.Message}");
        }
    }
}
