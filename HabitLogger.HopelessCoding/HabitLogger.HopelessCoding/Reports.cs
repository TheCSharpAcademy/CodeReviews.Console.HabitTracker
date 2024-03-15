using DatabaseHelpers.HopelessCoding;
using System.Data.SQLite;

namespace GenerateReports.HopelessCoding
{
    internal class Reports
    {
        internal static void TimelineReports(int days)
        {
            Console.WriteLine($"Report for last {days} days\n");

            using (var connection = new SQLiteConnection(DbHelpers.connectionString))
            {
                connection.Open();
                string viewQuery = @$"SELECT * 
                                    FROM daily_calories 
                                    WHERE Date > Date('now', '-' || @days || ' days') 
                                    ORDER BY Date DESC";

                using (var command = new SQLiteCommand(viewQuery, connection))
                {
                    command.Parameters.AddWithValue("@days", days);

                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("ID\t\tDate\t\t\tCalories");
                        Console.WriteLine("================================================");

                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string day = reader["Date"].ToString();
                            int calories = Convert.ToInt32(reader["Quantity"]);

                            Console.WriteLine($"{id}\t\t{day}\t\t{calories}");
                        }
                        Console.WriteLine("\n----------------------------");
                    }
                }
            }
        }

        internal static void AverageReports()
        {
            Console.Write("Enter the number of days to calculate the average calories: ");
            string inputString = Console.ReadLine();
            int days;

            while(!int.TryParse(inputString, out days) || days <= 0)
            {
                Console.Write("\nInvalid input. Please enter a valid positive integer value: ");
                inputString = Console.ReadLine();
            }

            using (var connection = new SQLiteConnection(DbHelpers.connectionString))
            {
                connection.Open();
                string viewQuery = @$"SELECT avg(Quantity) 
                                    FROM daily_calories 
                                    WHERE Date > Date('now', '-' || @days || ' days')";

                using (var command = new SQLiteCommand(viewQuery, connection))
                {
                    command.Parameters.AddWithValue("@days", days);
                    int avgCalories = Convert.ToInt32(command.ExecuteScalar());

                    Console.WriteLine($"\nAverage calories from last {days} days are {avgCalories}.");
                }
                Console.WriteLine("\n----------------------------");
            }
        }
    }
}
