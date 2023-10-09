using HabitTracker.Matija87;
using Microsoft.Data.Sqlite;

class Program
{
    static readonly string connectionString = @"Data source=habit-Tracker.db";

    public static void Main ()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS cigs_smoked (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                QUANTITY INTEGER
                )";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        MainMenu();
    }

    internal static void MainMenu()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("Type 0 to close application");
            Console.WriteLine("Type 1 to View All Records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            Console.Write("\nYour selection: ");

            var selection = Console.ReadKey();

            switch (selection.KeyChar.ToString())
            {
                case "0":
                    Console.WriteLine("\n\nThank you for using this program! Goodbye!\n\n");
                    Environment.Exit(0);
                    break;
                case "1":
                    MenuCommands.GetAllRecords();
                    break;
                case "2":
                    MenuCommands.InsertRecord();
                    break;
                case "3":
                    MenuCommands.DeleteRecord();
                    break;
                case "4":
                    MenuCommands.UpdateRecord();
                    break;
                default:
                    Console.WriteLine("Invalid command! Type 0-4!\n");
                    break;
            }
        }

    }

}