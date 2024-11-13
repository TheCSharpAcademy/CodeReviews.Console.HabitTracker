namespace HabitTracker
{
    public class Report
    {
        enum Frequency
        {
            Daily,
            Weekly,
            Monthly
        }
        static public void GetReport()
        {
            var connection = Data.DbConnection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @$"
                SELECT Name, HabitId, COUNT(*) AS RecordCount
                FROM Records
                GROUP BY HabitId
                ORDER BY RecordCount DESC
                LIMIT 5;
                ";
            Console.WriteLine("The most reported habits:");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string habitName = reader.GetString(0);
                    int recordCount = reader.GetInt32(2);
                    Console.WriteLine($"{habitName} - performed {recordCount} Times");
                }
            }


            command.CommandText = @"
                SELECT Name, HabitId, COUNT(*) AS RecordCount
                FROM Records
                GROUP BY HabitId
                ORDER BY RecordCount ASC
                LIMIT 5;
                ";
            Console.WriteLine("\nThe least reported habits:");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string habitName = reader.GetString(0);
                    int recordCount = reader.GetInt32(2);
                    Console.WriteLine($"{habitName} - performed {recordCount} Times");
                }
            }


            command.CommandText =
            @$"
                SELECT strftime('%H',HabitDate) AS ActiveHour, COUNT(*) AS EntryCount
                FROM Records
                GROUP BY ActiveHour
                ORDER BY EntryCount DESC
                LIMIT 1;
            ";
            object? result = command.ExecuteScalar();
            int hour = result != null ? Convert.ToInt32(result) : 0;
            Console.WriteLine($"\nThe most active hour of the day is: {hour}:00");


            command.CommandText =
@$"
                SELECT strftime('%w',HabitDate) AS ActiveDay, COUNT(*) AS EntryCount
                FROM Records
                GROUP BY ActiveDay
                ORDER BY EntryCount DESC
                LIMIT 1;
            ";

            result = command.ExecuteScalar();
            DayOfWeek day = result != null ? (DayOfWeek)Convert.ToInt32(result) : 0;
            Console.WriteLine($"\nThe most active day of the week is: {day}");

            command.CommandText =
            @$"
                SELECT Name, HabitDate, HabitId, (JULIANDAY(HabitDate) - JULIANDAY(LAG (HabitDate) OVER (PARTITION BY HabitId ORDER BY HabitDate))) * 24 AS Break
                FROM Records
                ORDER BY Break DESC
                LIMIT 1;
            ";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string habitName = reader.GetString(0);
                    int recordCount = reader.GetInt32(3);
                    int id = reader.GetInt32(2);
                    Console.WriteLine($"\nThe longest break was taken for the {habitName} habit with an id of {id} - which was not performed for {recordCount} Hours");
                }
            }


            Console.WriteLine("\nComletion rate statistics:");
            Dictionary<int, float> habits = new Dictionary<int, float>();
            command.CommandText =
            @$"
                SELECT Id, Frequency, TimesPerPeriod, StartDate,
                (JULIANDAY('now') - JULIANDAY(StartDate)) * 24 * TimesPerPeriod /
                CASE 
                    WHEN Frequency = 'Daily' THEN 24
                    WHEN Frequency = 'Weekly' THEN 168
                    WHEN Frequency = 'Monthly' THEN 720
                END AS ExpectedNumberOfReps 
                FROM Habits;
            ";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    float expectedReps = reader.GetFloat(reader.GetOrdinal("ExpectedNumberOfReps"));
                    habits.Add(id, expectedReps);
                }
            }
            command.CommandText =
            $@"
                SELECT Name, HabitId, COUNT(*)
                FROM Records
                GROUP BY HabitId;
            ";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    int id = reader.GetInt32(1);
                    double completionRate = Math.Round((reader.GetInt32(2) / habits[id] * 100), 2);
                    Console.WriteLine($"The '{name}' habit with an id of {id} completion rate is {completionRate}%.");
                }
            }






        }


    }
}
