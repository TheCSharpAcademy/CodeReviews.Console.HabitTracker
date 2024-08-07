using Microsoft.Data.Sqlite;


public static class Dashboard
{
    public static void DisplayDashboard()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=habit_tracker.db"))
            {
                connection.Open();

                // Current year and month
                var currentYear = DateTime.Now.Year.ToString();
                var currentMonth = DateTime.Now.ToString("yyyy-MM");
                
                // Print the current year and month
                Console.WriteLine($"Current Year: {currentYear}");
                Console.WriteLine($"Current Month: {currentMonth}");

                // 1. Total glasses of water in the current year
                var waterCommand = new SqliteCommand();
                waterCommand.Connection = connection;
                waterCommand.CommandText = @"
                    SELECT COALESCE(SUM(Quantity), 0) AS TotalWater
                    FROM HabitLog
                    WHERE HabitId = 1 AND strftime('%Y', Date) = @year";
                waterCommand.Parameters.AddWithValue("@year", currentYear);
               
                var totalWater = waterCommand.ExecuteScalar();

                // 2. Highest number of glasses of water in a single day
                waterCommand.CommandText = @"
                    SELECT COALESCE(MAX(Quantity), 0) AS MaxWater
                    FROM HabitLog
                    WHERE HabitId = 1";

                var maxWater = waterCommand.ExecuteScalar();

                // 3. Total pages read in the current year
                var readingCommand = new SqliteCommand();
                readingCommand.Connection = connection;
                readingCommand.CommandText = @"
                    SELECT COALESCE(SUM(Quantity), 0) AS TotalPages
                    FROM HabitLog
                    WHERE HabitId = 3 AND strftime('%Y', Date) = @year";
                readingCommand.Parameters.AddWithValue("@year", currentYear);

                var totalPages = readingCommand.ExecuteScalar();

                // 4. Total steps walked in the current month
                var walkingCommand = new SqliteCommand();
                walkingCommand.Connection = connection;
                walkingCommand.CommandText = @"
                    SELECT COALESCE(SUM(Quantity), 0) AS TotalSteps
                    FROM HabitLog
                    WHERE HabitId = 2 AND strftime('%Y-%m', Date) = @month";
                walkingCommand.Parameters.AddWithValue("@month", currentMonth);

                var totalSteps = walkingCommand.ExecuteScalar();

                // 5. Average number of steps per day in the current month
                walkingCommand.CommandText = @"
                    SELECT COALESCE(AVG(Quantity), 0) AS AvgSteps
                    FROM HabitLog
                    WHERE HabitId = 2 AND strftime('%Y-%m', Date) = @month";

                var avgSteps = walkingCommand.ExecuteScalar();

                // Display results
                Console.WriteLine("Dashboard Report:");
                Console.WriteLine("-------------------------------");
                Console.WriteLine($"Total glasses of water this year: {totalWater}");
                Console.WriteLine($"Highest glasses of water in a day: {maxWater}");
                Console.WriteLine($"Total pages read this year: {totalPages}");
                Console.WriteLine($"Total steps walked this month: {totalSteps}");
                Console.WriteLine($"Average steps per day this month: {avgSteps:F2}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static void DisplayHabits()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=habit_tracker.db"))
            {
                connection.Open();
                var selectCommand = new SqliteCommand("SELECT Id, Name, Unit FROM Habits", connection);

                using (var reader = selectCommand.ExecuteReader())
                {
                    Console.WriteLine("Habit ID | Habit Name  | Unit");
                    Console.WriteLine("------------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Id"],-8} | {reader["Name"],-10} | {reader["Unit"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static void DisplayLogsForHabit(int habitId)
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=habit_tracker.db"))
            {
                connection.Open();
                var selectCommand = new SqliteCommand("SELECT Id, Date, Quantity FROM HabitLog WHERE HabitId = @habitId", connection);
                selectCommand.Parameters.AddWithValue("@habitId", habitId);

                using (var reader = selectCommand.ExecuteReader())
                {
                    Console.WriteLine("Log ID | Date       | Quantity");
                    Console.WriteLine("---------------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Id"],-6} | {reader["Date"],-10} | {reader["Quantity"],-8}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
