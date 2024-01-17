using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.frockett;

internal class dbOperations
{
    Helpers helpers = new Helpers();

    string connectionString = @"Data Source=habit-Tracker.db";
    List<string> tableName = new List<string>() { "drinking_water", "running_in_km", "doing_push_ups" };
    List<string> unitName = new List<string>() { "glasses", "kilometer", "reps" };
  
    public void InsertHabit()
    {
        Console.Clear();
        Console.WriteLine("\nEnter the name of your habit: \n");
        string? habitName = Console.ReadLine();

        if (habitName == null)
        {
            Console.WriteLine("\nInvalid, please enter the name of your habit\n");
            habitName = Console.ReadLine();
        }

        Console.Clear();
        Console.WriteLine($"\nWhat is the unit of measurement for {habitName}?");
        Console.WriteLine("E.g. glasses, miles, reps, etc.\n");
        string? habitUnit = Console.ReadLine();

        if (habitUnit == null)
        {
            Console.WriteLine("\nInvalid, please enter the unit of measurement\n");
            habitUnit = Console.ReadLine();
        }

        AddToList(habitName, habitUnit);

    }

    public void AddToList(string? habitName, string? habitUnit)
    {
        tableName.Add(habitName);
        unitName.Add(habitUnit);

        if(CheckForTable(habitName)) //this if-condition looks a bit weird because CheckForTable actually returns false when there is a table due to phrasing of the bool in SeedTestData
        {
            CreateSQLTable(habitName, connectionString);
            Console.WriteLine($"Table for {habitName} created!");
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO unit_table(unit_name) VALUES('{habitUnit}')";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO {habitName}(unit_id) VALUES({tableName.Count})";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

    }

    public void SeedTestData()
    {
        CreateUnitTable("unit_table", connectionString);
        
        for (int i = 0; i < tableName.Count; i++)
        {
            bool shouldSeedData = CheckForTable(tableName.ElementAt(i));

            CreateSQLTable(tableName.ElementAt(i), connectionString);

            if (shouldSeedData)
            {
                SeedRandomData(tableName.ElementAt(i), connectionString, i+1);
            }
        }
    }

    // This method checks to see if the table exists at application startup. It returns a bool which is used to determine whether or not to generate seed data
    public bool CheckForTable(string tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'";

            var result = checkCmd.ExecuteScalar();
            int resultCount = Convert.ToInt32(result);

            if (resultCount != 0)
            {
                return false;
            }
            connection.Close();
        }
        return true;
    }

    public void CreateUnitTable(string tableName, string connectionString)
    {
        bool shouldSeedUnits = CheckForTable(tableName);

        if(shouldSeedUnits)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"CREATE TABLE IF NOT EXISTS unit_table (
                                        unit_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        unit_name TEXT
                                        )";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }


            for (int i = 0; i < unitName.Count; i++)
            {
                Console.WriteLine("Current unit name: " + unitName.ElementAt(i));
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"INSERT INTO unit_table(unit_name) VALUES('{unitName.ElementAt(i)}')";
                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }

    public void CreateSQLTable(string tableName, string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {tableName} (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER,
                                        unit_id INTEGER
                                        )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void UpdateRecord()
    {
        Console.Clear();
        PrintAllRecords();

        var recordId = helpers.GetNumberInput("\nPlease enter the ID of the record you want to update, or enter 0 to return to the main menu\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 from drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\nRecord with ID {recordId} does not exist.\n");
                connection.Close();
                UpdateRecord();
            }

            string date = helpers.GetDateInput();
            int quantity = helpers.GetNumberInput("\nPlease enter number of glasses or other metric (number must be an integer), or enter 0 to return to main menu\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

            int rowCount = tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void DeleteRecord()
    {
        Console.Clear();
        PrintAllRecords();

        int recordId = helpers.GetNumberInput("\nPlease enter the ID of the record you want to delete, or enter 0 to return to the main menu\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with ID {recordId} doesn't exist. \n\n");
                DeleteRecord();
            }
            else
            {
                Console.WriteLine($"\n\nRecord with ID {recordId} deleted successfully. \n\n");
            }
            connection.Close();
        }

    }

    public void PrintAllRecords()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM drinking_water";

            List<HabitDataModel> tableData = new List<HabitDataModel>();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new HabitDataModel
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2),
                        UnitId = reader.GetInt32(3)
                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();
            Console.WriteLine("-----------------------------------------------\n");
            foreach (var entry in tableData)
            {
                Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MMM-yy")} - Quantity: {entry.Quantity}");
            }
            Console.WriteLine("-----------------------------------------------\n");
            Console.ReadLine();
        }
    }

    public void InsertRecord()
    {
        string date = helpers.GetDateInput();
        int quantity = helpers.GetNumberInput("\n\nPlease enter number of glasses or other metric (number must be an integer)\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void SeedRandomData(string tableName, string connectionString, int unitId)
    {
        for (int i = 0; i < 100; i++)
        {
            string date = RandomGenerators.GetRandomDate();
            int quantity = RandomGenerators.GetRandomQuantity();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO {tableName}(date, quantity, unit_id) VALUES('{date}', {quantity}, {unitId})";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        Console.WriteLine("data seeded successfully");
    }
}
