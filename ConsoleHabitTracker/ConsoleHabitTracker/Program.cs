using System.Data;
using System.Diagnostics;
using System.Data.SQLite;

namespace ConsoleHabitTracker;

class Program
{
    static void Main(string[] args)
    {
        var endProgram = true;

        string connectionString = "Data Source = habits.db; Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            // Open the connection
            connection.Open();
            if (connection.State != ConnectionState.Open)
            {
                Console.WriteLine("Failed to connect to the database.");
                return;
            }
            else
            {
                Console.WriteLine("Connected to the database.");
            }

            string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='habitsTable';"; 
            var tableExists = false;
            
            using (SQLiteCommand command = new SQLiteCommand(checkTableQuery, connection)) 
            { 
                using (SQLiteDataReader reader = command.ExecuteReader()) 
                { 
                    tableExists = reader.HasRows; 
                } 
            }
            
            // if tabe doesn't exit Create a table

            if (!tableExists)
            {
                string createTableQuery =
                    "CREATE TABLE habitsTable (Id INTEGER PRIMARY KEY AUTOINCREMENT, HabitName TEXT NOT NULL, Quantity INTEGER, Units TEXT);";
            
                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            
                // prepoulated data
                string insertDataQuery = $"INSERT INTO habitsTable (HabitName, Quantity, Units) VALUES ('jumping', 27, 'minutes'),('swimming', 15, 'miles'), ('drink water', 7, 'glasses'), ('biking', 4, 'miles');";
           
                using (SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
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
                        ViewRecords(connection);
                        Console.WriteLine("Press enter to continue");
                        Console.ReadLine();
                        break;
                    case '2':
                        Console.WriteLine("\n\nAdd a Record");
                        AddNewHabbit(connection);
                        Console.WriteLine("New entry added, press enter to continue");
                        Console.ReadLine();
                        break;
                    case '3':
                        Console.WriteLine("\n\nDelete a Record");
                        ViewRecords(connection);
                        
                        Console.WriteLine("Which record ID would you like to delete");
                        string? entry = Console.ReadLine().ToLower();

                        while (entry == null)
                        {
                            Console.WriteLine("invalid entry please try again or press E to exit");
                            entry = Console.ReadLine().ToLower();
                        }

                        if (entry == "e")
                        {
                                Console.WriteLine("Exiting program");
                                return;
                        }
                        else
                        {
                            Console.WriteLine($"Deleting record ID {entry}");
                            string deleteRecord = $"DELETE FROM habitsTable WHERE Id == {entry};";
                            
                            using (SQLiteCommand command = new SQLiteCommand(deleteRecord, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        
                        break;
                    case '4':
                        Console.WriteLine("\n\nEdit a Record");
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("\n\nInvalid Selection press enter to try again");
                        Console.ReadLine();
                        break;
                }
            }

            connection.Close();
        }
    }

    private static void ViewRecords(SQLiteConnection connection)
    {
        string selectDataQuery = "SELECT * FROM habitsTable;";

        using (SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection))
        {
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader["Id"], -4} HabitName: {reader["HabitName"], -20} Quantity: {reader["Quantity"], -10} Units: {reader["Units"],-10}");
                }
            }
        }
    }

    private static void AddNewHabbit(SQLiteConnection connection)
    {
        string insertDataQuery;
        Console.WriteLine("Enter Habit Name");
        string? habitName = Console.ReadLine();
                        
        Console.WriteLine("Enter Quantity complete");
        string? entry = Console.ReadLine();
        int quantity=-1;
        bool validEntry = false;
        while (!validEntry)
        {
            if (int.TryParse(entry, out int validQuantity))
            {
                quantity = validQuantity;
                validEntry = true;
            }
            else
            {
                Console.WriteLine("Invalid Entry please enter a numerical quantity");
                entry = Console.ReadLine();
            }
        }

        Console.WriteLine("Enter type of Units tracked");
        string? units = Console.ReadLine();
                        
        insertDataQuery = $"INSERT INTO habitsTable (HabitName, Quantity, Units) VALUES ('{habitName}', {quantity}, '{units}');";
        using (SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection))
        {
            command.ExecuteNonQuery();
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