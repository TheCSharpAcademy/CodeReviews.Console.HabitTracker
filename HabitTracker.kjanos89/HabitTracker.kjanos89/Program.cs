using Microsoft.Data.Sqlite;
class Program
{
    static void Main(string[] args)
    {
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        string connectionString = @"Data Source=habit-Tracker.db";
        using (var sqlConnection = new SqliteConnection(connectionString))
        {
            sqlConnection.Open();
            var tableCommand = sqlConnection.CreateCommand();
            tableCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS coding (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                IssuesSolved INTEGER
            )";
            tableCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        DisplayMainMenu();
    }

    static void DisplayMainMenu()
    {
        Console.WriteLine("________________________________");
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("Choose from the options below:");
        Console.WriteLine("Press 1 to View Records");
        Console.WriteLine("Press 2 to Delete Record");
        Console.WriteLine("Press 3 to Update Record");
        Console.WriteLine("Press 4 to Insert Record");
        Console.WriteLine("Press 0 to Quit the application");
        Console.WriteLine("________________________________");

        string input = Console.ReadLine();
        MenuChoice(input[0]);
    }

    public static void MenuChoice(char message)
    {
        switch (message)
        {
            case '1':
                ViewRecords();
                break;
            case '2':
                DeleteRecord();
                break;
            case '3':
                UpdateRecord();
                break;
            case '4':
                InsertRecord();
                break;
            case '0':
                Console.WriteLine("Quit the application...");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input, try again!");
                DisplayMainMenu();
                break;
        }
    }

    public static void ViewRecords()
    {
        string connectionString = @"Data Source=habit-Tracker.db";
        using (var sqlConnection = new SqliteConnection(connectionString))
        {
            sqlConnection.Open();
            var selectCommand = sqlConnection.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM coding";

            using (var reader = selectCommand.ExecuteReader())
            {
                bool hasRows = false;
                while (reader.Read())
                {
                    hasRows = true;
                    var id = reader.GetInt32(0);
                    var date = reader.GetString(1);
                    var issuesSolved = reader.GetInt32(2);
                    Console.WriteLine($"Id: {id}, Date: {date}, Issues solved: {issuesSolved}");
                }
                if (!hasRows)
                {
                    Console.WriteLine("No records found.");
                }
            }

            sqlConnection.Close();
        }

        DisplayMainMenu();
    }
    public static void InsertRecord()
    {
        Console.Write("Enter the date: ");
        string date = Console.ReadLine();
        Console.Write("Enter the number of issues solved: ");
        if (int.TryParse(Console.ReadLine(), out int issuesSolved))
        {
            string connectionString = @"Data Source=habit-Tracker.db";
            using (var sqlConnection = new SqliteConnection(connectionString))
            {
                sqlConnection.Open();
                var insertCommand = sqlConnection.CreateCommand();
                insertCommand.CommandText = @"
                    INSERT INTO coding (Date, IssuesSolved)
                    VALUES (@Date, @IssuesSolved)";
                insertCommand.Parameters.AddWithValue("@Date", date);
                insertCommand.Parameters.AddWithValue("@IssuesSolved", issuesSolved);
                insertCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }

            Console.WriteLine("Record inserted.");
        }
        else
        {
            Console.WriteLine("Invalid number for issues solved.");
        }

        DisplayMainMenu();
    }
    public static void UpdateRecord()
    {
        Console.Write("Enter the Id of the record you want to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            string connectionString = @"Data Source=habit-Tracker.db";
            using (var sqlConnection = new SqliteConnection(connectionString))
            {
                sqlConnection.Open();
                var selectCommand = sqlConnection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM coding WHERE Id = @Id";
                selectCommand.Parameters.AddWithValue("@Id", id);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"Current Date: {reader.GetString(1)}");
                        Console.WriteLine($"Current Issues Solved: {reader.GetInt32(2)}");

                        Console.Write("Enter date: ");
                        string newDate = Console.ReadLine();
                        Console.Write("Enter the number of solved issues: ");
                        if (int.TryParse(Console.ReadLine(), out int newIssuesSolved))
                        {
                            var updateCommand = sqlConnection.CreateCommand();
                            updateCommand.CommandText = @"
                                UPDATE coding
                                SET Date = @NewDate, IssuesSolved = @NewIssuesSolved
                                WHERE Id = @Id";
                            updateCommand.Parameters.AddWithValue("@NewDate", newDate);
                            updateCommand.Parameters.AddWithValue("@NewIssuesSolved", newIssuesSolved);
                            updateCommand.Parameters.AddWithValue("@Id", id);
                            updateCommand.ExecuteNonQuery();

                            Console.WriteLine("Record updated.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for solved issues.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Record not found.");
                    }
                }
                sqlConnection.Close();
            }
        }
        else
        {
            Console.WriteLine("Invalid Id.");
        }

        DisplayMainMenu();
    }
    public static void DeleteRecord()
    {
        Console.Write("Enter the Id of the record to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            string connectionString = @"Data Source=habit-Tracker.db";
            using (var sqlConnection = new SqliteConnection(connectionString))
            {
                sqlConnection.Open();
                var deleteCommand = sqlConnection.CreateCommand();
                deleteCommand.CommandText = "DELETE FROM coding WHERE Id = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", id);
                int rowsAffected = deleteCommand.ExecuteNonQuery();
                sqlConnection.Close();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Record deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Record not found.");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid Id.");
        }
        DisplayMainMenu();
    }

}