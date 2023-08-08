using Microsoft.Data.Sqlite;

namespace HabitTracker.lordWalnuts;

public class Program
{
    public static readonly string connectionString = @"Data Source=habit-tracker.db";

    public static void Main(string[] args)
    {
        CreateDatabase();
        ShowMenu();
    }

    private static void CreateDatabase()
    {

        var connection = new SqliteConnection(connectionString);

        using (connection)
        {
            connection.Open();
            var sqlCommand = connection.CreateCommand();

            string commandText =
                @"CREATE TABLE IF NOT EXISTS habits(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Habit TEXT,
                Date TEXT,
                Unit TEXT,
                Quantity INTEGER )";

            sqlCommand.CommandText = commandText;
            sqlCommand.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine("DB created");
        }

    }
    internal static void ShowMenu()
    {
        Console.Clear();
        bool appOn = true;
        while (appOn)
        {
            Console.WriteLine("Welcome to HabitTracker");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("------------------------------------------\n");

            string userChoice = Console.ReadLine();

            //TODO: null check here

            switch (int.Parse(userChoice))
            {
                case 0:
                    Console.WriteLine("\nGoodBye\n");
                    appOn = false;
                    Environment.Exit(0);
                    break;
                case 1:
                    Crud.GetAllHabits();
                    break;
                case 2:
                    Crud.InsertHabit();
                    break;
                case 3:
                    Crud.DeleteHabit();
                    break;
                case 4:
                    Crud.UpdateHabit();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

}