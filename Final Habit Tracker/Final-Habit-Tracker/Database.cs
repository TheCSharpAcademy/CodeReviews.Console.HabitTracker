using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitLogger;

public class Database
{
    static string connectionString = @"Data Source=habit-tracker.db";

    public static void CreateDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS logged_habits(
                        Id INTEGER PRIMARY_KEY,
                        Date TEXT,
                        Habit TEXT,
                        Quantity INTEGER,
                        Measurement TEXT)";

            try
            {
                tableCmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static void SeedDatabase()
    {
        bool hasRows = CheckDatabase();

        if (!hasRows)
        {
            Random random = new Random();

            DateTime start = new DateTime(2024, 6, 20);
            int range = (DateTime.Today - start).Days;
            start.AddDays(random.Next(range));

            for (int i = 1; i < 101; i++)
            {
                int randomQuantity = random.Next(1, 6);
                string randomDate = start.AddDays(random.Next(range)).ToString("dd-MM-yy");
                int randomHabitNumber = random.Next(0, 3);
                string[,] randomHabits = { { "reading", "running", "drinking water" }, { "books", "miles", "glasses" } };

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = $@"INSERT INTO logged_habits (Id, Date, Habit, Quantity, Measurement)
                                        VALUES ({i}, '{randomDate}', '{randomHabits[0, randomHabitNumber]}', {randomQuantity}, '{randomHabits[1, randomHabitNumber]}')";

                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            Console.WriteLine("Database seeded.");
        }
    }

    internal static void Insert()
    {
        int id = Convert.ToInt32(User.GetNumber("\nEnter Id"));
        string? date = User.GetDate();
        string? habit = User.GetString("\nEnter habit.");
        int quantity = Convert.ToInt32(User.GetNumber("\nEnter quantity."));
        string? measurement = User.GetString("\nEnter measurement.");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"INSERT INTO logged_habits (Id, Date, Habit, Quantity, Measurement)
                                    VALUES ({id}, '{date}', '{habit}', {quantity}, '{measurement}')";

            try
            {
                tableCmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        Console.Clear();
        User.PrintMainMenu();
        Console.WriteLine("Record was added.");
    }

    internal static void Delete()
    {
        ViewRecords();

        string? userInput = User.GetNumber("\nSelect record Id to delete.");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"DELETE FROM logged_habits WHERE Id = '{userInput}'";

            try
            {
                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord {userInput} doesn't exist.\n");
                    Delete();
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                connection.Close();
            }
        }

        Console.Clear();
        User.PrintMainMenu();
        Console.WriteLine("Record was deleted.");
    }

    internal static void DeleteDatabase()
    {
        Console.WriteLine("\nDelete database?");
        string? userInput = User.GetString("Select Y or N.");
        bool validInput = false;

        while (!validInput)
        {
            if (userInput == "y")
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = "DELETE FROM logged_habits";

                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                Console.Clear();
                User.PrintMainMenu();
                Console.WriteLine("Database cleared.\n");
                validInput = true;
            }
            else if (userInput == "n")
            {
                Console.Clear();
                User.PrintMainMenu();
                Console.WriteLine("Action canceled.\n");
                validInput = true;
            }
            else
            {
                userInput = User.GetString("Invalid input. Please select Y or N.");
            }
        }
    }

    internal static void Update()
    {
        bool closeMenu = false;
        string? selectedColumn = "";
        string? newValue = "";
        int newNumberValue = 1;

        ViewRecords();

        string? idSelected = User.GetNumber("\nSelect record Id to update.");

        while (!closeMenu)
        {
            Console.WriteLine("Press 0 to return to Main Menu.");
            Console.WriteLine("\nSelect column to update:");
            Console.WriteLine("1. Date");
            Console.WriteLine("2. Habit");
            Console.WriteLine("3. Quantity");
            Console.WriteLine("4. Measurement");

            string? switchInput = User.GetNumber(" ");

            switch (switchInput)
            {
                case "0":
                    Console.WriteLine("\nExiting to main menu. Press any key to continue.");
                    closeMenu = true;
                    break;
                case "1":
                    newValue = User.GetDate();
                    selectedColumn = "Date";
                    CommenceUpdate(idSelected, selectedColumn, newValue, newNumberValue);
                    break;
                case "2":
                    newValue = User.GetString("\nEnter habit.");
                    selectedColumn = "Habit";
                    CommenceUpdate(idSelected, selectedColumn, newValue, newNumberValue);
                    break;
                case "3":
                    newNumberValue = Convert.ToInt32(User.GetNumber("\nEnter quantity."));
                    selectedColumn = "Quantity";
                    CommenceUpdate(idSelected, selectedColumn, newValue, newNumberValue);
                    break;
                case "4":
                    newValue = User.GetString("\nEnter measurement.");
                    selectedColumn = "Measurement";
                    CommenceUpdate(idSelected, selectedColumn, newValue, newNumberValue);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please select an option from the menu.");
                    break;
            }
        }

        Console.Clear();
        User.PrintMainMenu();
    }

    internal static void ViewRecords()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"SELECT * FROM logged_habits";
            List<Data> tableData = new();

            try
            {
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new Data
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Habit = reader.GetString(2),
                                Quantity = reader.GetInt32(3),
                                Measurement = reader.GetString(4)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            Console.WriteLine("----------------------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - {dw.Habit} - {dw.Quantity} {dw.Measurement}");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }

        User.PrintMainMenu();
    }

    internal static void SearchRecords()
    {
        bool closeMenu = false;
        string? selectedColumn = "";
        string? searchTerm = "";

        while (!closeMenu)
        {
            Console.WriteLine("Press 0 to return to Main Menu.");
            Console.WriteLine("\nSelect column you'd like to search by:");
            Console.WriteLine("1. Date");
            Console.WriteLine("2. Habit");
            Console.WriteLine("3. Quantity");
            Console.WriteLine("4. Measurement");

            string? switchInput = User.GetNumber(" ");

            switch (switchInput)
            {
                case "0":
                    Console.WriteLine("\nExiting to main menu. Press any key to continue.");
                    closeMenu = true;
                    break;
                case "1":
                    selectedColumn = "Date";
                    searchTerm = User.GetString("\nInput date: (dd-mm-yy)");
                    CommenceSearch(selectedColumn, searchTerm);
                    break;
                case "2":
                    selectedColumn = "Habit";
                    searchTerm = User.GetString("\nInput habit: ");
                    CommenceSearch(selectedColumn, searchTerm);
                    break;
                case "3":
                    selectedColumn = "Quantity";
                    searchTerm = User.GetNumber("\nInput quantity: ");
                    CommenceSearch(selectedColumn, searchTerm);
                    break;
                case "4":
                    selectedColumn = "Measurement";
                    searchTerm = User.GetString("\nInput measurement: ");
                    CommenceSearch(selectedColumn, searchTerm);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please select an option from the menu.");
                    break;
            }
        }

        Console.Clear();
        User.PrintMainMenu();
    }

    internal static void CommenceUpdate(string id, string column, string value, int numberValue)
    {
        if (column == "Quantity")
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $@"SELECT Id FROM logged_habits where Id = {id}";

                try
                {
                    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (checkQuery == 0)
                    {
                        Console.WriteLine($"\nRecord {id} doesn't exist.\n");
                        connection.Close();
                    }
                    else
                    {
                        var tableCmd = connection.CreateCommand();
                        tableCmd.CommandText = $@"UPDATE logged_habits 
                                    SET {column} = {numberValue}
                                    WHERE Id = {id}";
                        tableCmd.ExecuteNonQuery();
                        Console.Clear();
                        Console.WriteLine($"Record {id} updated.");
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        else
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $@"SELECT Id FROM logged_habits where Id = {id}";

                try
                {
                    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (checkQuery == 0)
                    {
                        Console.WriteLine($"\nRecord {id} doesn't exist.\n");
                        connection.Close();
                    }
                    else
                    {
                        var tableCmd = connection.CreateCommand();
                        tableCmd.CommandText = $@"UPDATE logged_habits 
                                    SET {column} = '{value}'
                                    WHERE Id = {id}";
                        tableCmd.ExecuteNonQuery();
                        Console.Clear();
                        Console.WriteLine($"Record {id} updated.");
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }

    internal static void CommenceSearch(string column, string search)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT * FROM logged_habits WHERE {column} LIKE '{search}%'";
            List<Data> tableData = new();

            try
            {
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new Data
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Habit = reader.GetString(2),
                                Quantity = reader.GetInt32(3),
                                Measurement = reader.GetString(4)
                            });
                    }

                    Console.WriteLine("\nId - Date - Habit - Quantity - Measurement");
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            Console.WriteLine("----------------------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - {dw.Habit} - {dw.Quantity} {dw.Measurement}");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }
    }

    internal static bool CheckDatabase()
    {
        bool hasRows = false;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT * FROM logged_habits";

            try
            {
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    hasRows = true;
                }
                else
                {
                    hasRows = false;
                }

            }            
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return hasRows;
    }
}

public class Data
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public string? Habit { get; set; }

    public string? Measurement { get; set; }
}