using System.Diagnostics;
using System.Data.SQLite;

namespace ConsoleHabitTracker;

class Program
{
    static void Main(string[] args)
    {
        var endProgram = true;

        string connectionString = "Data Source = habitDatabase.db; Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            // Open the connection
            connection.Open();

            // Create a table
            string createTableQuery =
                "CREATE TABLE IF NOT EXISTS habits (Id INTEGER PRIMARY KEY AUTOINCREMENT, HabitName TEXT NOT NULL, Quantity INTEGER);";

            using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }


            while (endProgram)
            {
                DisplayMenu();
                var selection = Console.ReadKey().KeyChar;


                switch (selection)
                {
                    case '0':
                        Console.WriteLine("\n\nClosing application");
                        endProgram = false;
                        break;
                    case '1':
                        Console.WriteLine("\n\nView All Records");
                        Console.ReadLine();
                        break;
                    case '2':
                        Console.WriteLine("\n\nAdd a Record");
                        Console.ReadLine();
                        break;
                    case '3':
                        Console.WriteLine("\n\nDelete a Record");
                        Console.ReadLine();
                        break;
                    case '4':
                        Console.WriteLine("\n\nEdit a Record");
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("\n\nInvalid Selection press enter to try again");
                        Console.ReadLine();
                        // selection = Console.ReadKey().KeyChar;
                        break;
                }
            }

            connection.Close();
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("What do you want to do?");
        Console.WriteLine("Type 0 to Close Application");
        Console.WriteLine("Type 1 to View all Records");
        Console.WriteLine("Type 2 to Add a record");
        Console.WriteLine("Type 3 to Delete a record");
        Console.WriteLine("Type 4 to Edit a record");
    }
}