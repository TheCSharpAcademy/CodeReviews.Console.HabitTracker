using HabitTracker.lordWalnuts.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.lordWalnuts;


internal static class Crud
{
    internal static void GetAllHabits()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();
            var sqlCommand = connection.CreateCommand();

            sqlCommand.CommandText = "SELECT * FROM habits";

            List<Habit> listOfHabits = new();

            SqliteDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                //advances to next row
                while (reader.Read())
                {
                    listOfHabits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),//0th column
                        HabitName = reader.GetString(1),
                        Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                        Unit = reader.GetString(3),
                        Quantity = reader.GetInt32(4),

                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }
            connection.Close();

            Console.WriteLine("------------------------------------------\n");
            foreach (var row in listOfHabits)
            {
                Console.WriteLine($"{row.Id} - {row.HabitName} - {row.Date.ToString("dd-MMM-yyyy")} - Unit: {row.Unit} - Quantity: {row.Quantity}");
            }
            Console.WriteLine("------------------------------------------\n");
        }
    }

    internal static void InsertHabit()
    {
        var habit = Helpers.GetHabitInput();
        var date = Helpers.GetDateInput();
        var unit = Helpers.GetUnitInput();
        var quantity = Helpers.GetQuantityInput();

        SqliteConnection sqlConnection = new SqliteConnection(connectionString: Program.connectionString);
        using (sqlConnection)
        {
            sqlConnection.Open();
            var sqlCommand = sqlConnection.CreateCommand();
            string commandText = @$"INSERT INTO habits(Habit, Date, Unit, Quantity)
                                    VALUES('{habit}', '{date}', '{unit}', '{quantity}')";

            sqlCommand.CommandText = commandText;
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }

    internal static void UpdateHabit()
    {

    }
    internal static void DeleteHabit()
    {

    }



}
