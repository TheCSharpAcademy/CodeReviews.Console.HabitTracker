using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class Repository(SqliteConnection connection)
{
    public void ViewAllRecords(string viewAllRecordsCommand)
    {
        using var command = new SqliteCommand(viewAllRecordsCommand, connection);
        using var reader = command.ExecuteReader();
        
        Console.WriteLine("Displaying all habit records ...");

        if (!reader.HasRows)
        {
            Console.WriteLine("\nNo habit records found!");
            return;
        }

        Console.WriteLine($"\n{"Index", -5}\t{"Date", -12}\t{"Habit", -30}\t{"Quantity", 10}");
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            var date = reader.GetDateTime(1);
            string habit = reader.GetString(2);
            int quantity = reader.GetInt32(3);
            Console.WriteLine($"{id, -5}\t{date.ToShortDateString(), -12}\t{habit, -30}\t{quantity, 10}");
        }
    }

    public void InsertRecord(string insertRecordCommand)
    {
        using var command = new SqliteCommand(insertRecordCommand, connection);

        bool validDateEntered = false;
        bool validHabitEntered = false;
        bool validQuantityEntered = false;

        while (!validDateEntered)
        {
            Console.Write("Type the date when the habit was done in dd/mm/yyyy format: ");
            string? date = Console.ReadLine();

            if (date is null || Regex.IsMatch(date, "[1-31]/[1-12]/[1-9999]"))
            {
                Console.WriteLine("Error: date entered is not valid");
            }
            else
            {
                string[] dateElements = date.Split("/");
                int day = int.Parse(dateElements[0]);
                int month = int.Parse(dateElements[1]);
                int year = int.Parse(dateElements[2]);
                try
                {
                    var habitDate = new DateTime(year, month, day);
                    command.Parameters.AddWithValue("@date", habitDate);
                    validDateEntered = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Error: Number of days entered greater than number of days in the given month");
                }
            }
        }

        while (!validHabitEntered)
        {
            Console.Write("Type the name of the habit: ");
            string? habit = Console.ReadLine();

            if (habit is null || !Regex.IsMatch(habit, @"^[a-zA-Z ]+$"))
            {
                Console.WriteLine("Name of habit should contain only alphabetical letters");
            }
            else
            {
                command.Parameters.AddWithValue("@habit", habit.ToLower());
                validHabitEntered = true;
            }
        }

        while (!validQuantityEntered)
        {
            Console.Write("Type the quantity of the habit done: ");
            string? quantity = Console.ReadLine();

            if (quantity is null || int.TryParse(quantity, out int habitQuantity) == false)
            {
                Console.WriteLine("Quantity of the habit done must be an integer");
            }
            else
            {
                command.Parameters.AddWithValue("@quantity", habitQuantity);
                validQuantityEntered = true;
            }
        }
        
        command.ExecuteNonQuery();
    }
}