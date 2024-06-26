using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker
{
    public class DatabaseController
    {
        /// <value>The connection string used to create the Sqlite connection</value>
        private string connectionString;

        /// <value>Instance of the connection (for reusability)</value>
        private SqliteConnection connection;

        /// <value>A list containing habits in the form of pairs name-unit</value>
        private List<(string habitName, string unitName)> habits;

        /// <summary>
        /// Constructor for the DB controller, sets up the connection instance.
        /// </summary>
        /// <param name="connectionString">The connection string used to make the connection.</param>
        public DatabaseController(string connectionString)
        {
            this.connectionString = connectionString;
            this.connection = new SqliteConnection(connectionString);
            this.habits = new();
        }

        /// <summary>
        /// Method that pulls the table and unit names and loads them into the list for the program.
        /// </summary>
        internal void LoadInfoFromDatabase()
        {
            connection.Open();

            var loadQuery = connection.CreateCommand();
            loadQuery.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name <> 'sqlite_sequence'";

            using (var reader = loadQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tableName = reader.GetString(0); // Get table name from reader

                    // Retrieve the name of the third column in the table
                    string unitName = GetHabitUnitName(connection, tableName);

                    habits.Add((tableName, unitName));
                }
            }

            connection.Close();
        }

        /// <summary>
        /// Helper method that gets the name of the unit for the habit.
        /// </summary>
        /// <param name="connection">DB connection instance</param>
        /// <param name="tableName">Name of the habit table</param>
        /// <returns>Name of the measuring unit for the habit</returns>
        /// <exception cref="Exception">Gets thrown in case the table has fewer than 3 cols.</exception>
        internal string GetHabitUnitName(SqliteConnection connection, string tableName)
        {
            string? columnName = null;

            // Query to get information about columns in the specified table
            string query = $"PRAGMA table_info({tableName});";

            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    // Move to the third row in the result set (index 2)
                    for (int i = 0; i < 3; i++)
                    {
                        if (!reader.Read())
                        {
                            // Handle cases where the table has fewer than 3 columns
                            throw new Exception($"Table '{tableName}' does not have at least 3 columns.");
                        }
                    }

                    // Read the third column's name (second column in the result set)
                    if (reader.HasRows)
                    {
                        columnName = reader.GetString(1); // Second column (name)
                    }
                }
            }

            return columnName!;
        }

        /// <summary>
        /// Method for initializing the DB. Creates the database with some mock habits and seeds the first habit.
        /// </summary>
        public void InitializeDatabase(bool seedRequired)
        {
            if (seedRequired)
            {
                Console.WriteLine("Database created. Seeding...");
                SeedDatabase();
            }
            else
            {
                Console.WriteLine("Database found. Loading tables...");
            }

            LoadInfoFromDatabase();
            ViewHabits();
            Console.WriteLine("Database ready. Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Method that gathers information to be able to insert a new record into a specified habit table.
        /// </summary>
        internal void PrepareInsert()
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");

            Console.WriteLine("Which habit would you like to log?");
            int selectedOption = SelectHabitFromDb();

            Console.Write("Do you want to use today's date? (y/n):   ");
            var userResponse = Console.ReadLine();

            while (userResponse != "y" && userResponse != "n")
            {
                Console.WriteLine("Invalid input, please reenter:");
                Console.Write("Do you want to use today's date? (y/n):  ");
                userResponse = Console.ReadLine();
            }

            if (userResponse == "n")
            {
                bool isValid = false;

                while (!isValid)
                {
                    Console.Write("Enter your own date in the format DD-MM-YYYY:  ");
                    date = Console.ReadLine()!;
                    isValid = IsValidDate(date!);
                }
            }

            Console.WriteLine($"Enter amount of {this.habits[selectedOption].unitName} below.");
            int measure = Interface.ParseSelection();

            InsertRecord(this.habits[selectedOption], date, measure);
            Console.WriteLine("Insert successful. Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// CRUD method for creating a new record in the specified table
        /// </summary>
        /// <param name="habit">The habit tuple in the form of table and unit names</param>
        /// <param name="date">The date of the record</param>
        /// <param name="measure">Value of the unit that will be inserted</param>
        internal void InsertRecord((string tableName, string unitsName) habit, string date, int measure)
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = $"INSERT INTO {habit.tableName} (Date, {habit.unitsName}) VALUES (@date, @measure)";
            insertCmd.Parameters.AddWithValue("@date", date);
            insertCmd.Parameters.AddWithValue("@measure", measure);

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// CRUD method for reading all records of a specified habit
        /// </summary>
        internal void ViewRecords()
        {
            Console.WriteLine("Which habit records would you like to see?");
            int selectedOption = SelectHabitFromDb();
            string tableName = this.habits[selectedOption].habitName;
            string unitName = this.habits[selectedOption].unitName;

            var viewString = $"SELECT * FROM {tableName}";

            try
            {
                connection.Open();
                using var command = new SqliteCommand(viewString, connection);
                using var reader = command.ExecuteReader();

                Console.Clear();
                Console.WriteLine($"HABIT RECORDS :  {tableName}");
                Console.Write("------------------------------------------\n");
                Console.WriteLine($"\tID\tDate\t\t{unitName}");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var date = reader.GetString(1);
                        var unit = reader.GetString(2);
                        Console.WriteLine($"\t{id}\t{date}\t{unit}");
                    }

                    Console.Write("------------------------------------------\n");
                    Console.WriteLine("Press any key to return.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("No records found. Press any key to return.");
                    Console.ReadKey();
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// CRUD method for updating a record of the specified habit
        /// </summary>
        internal void UpdateRecord()
        {
            Console.WriteLine("Which habit table do you wish to update?");
            var selectedHabitIndex = SelectHabitFromDb();
            var table = this.habits[selectedHabitIndex].habitName;
            var unit = this.habits[selectedHabitIndex].unitName;

            Console.WriteLine("What is the ID of the record you wish to update?");
            var selectedRecordId = Interface.ParseSelection();

            bool recordExists = DoesRecordExist(table, selectedRecordId);

            if (recordExists)
            {
                Console.Write("Insert the updated measure for this record: ");
                var updatedMeasure = Interface.ParseSelection();

                try
                {
                    connection.Open();
                    var updateQuery = connection.CreateCommand();
                    updateQuery.CommandText = $"UPDATE {table} SET {unit} = @{unit} WHERE Id = @id";
                    updateQuery.Parameters.AddWithValue($"@{unit}", updatedMeasure);
                    updateQuery.Parameters.AddWithValue($"@id", selectedRecordId);

                    updateQuery.ExecuteNonQuery();
                    Console.WriteLine($"Row updated sucessfuly. Press any key to continue.");
                    Console.ReadKey();
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("There is no record of the entered ID in the selected habit table. Press any key to return.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// CRUD method for deleting the record of a specified habit
        /// </summary>
        internal void DeleteRecord()
        {
            Console.WriteLine("Which habit table do you wish to delete from?");
            var selectedHabitIndex = SelectHabitFromDb();
            var table = this.habits[selectedHabitIndex].habitName;
            var unit = this.habits[selectedHabitIndex].unitName;

            Console.WriteLine("What is the ID of the record you wish to delete?");
            var selectedRecordId = Interface.ParseSelection();

            bool recordExists = DoesRecordExist(table, selectedRecordId);

            if (recordExists)
            {
                try
                {
                    connection.Open();
                    var deleteQuery = connection.CreateCommand();
                    deleteQuery.CommandText = $"DELETE FROM {table} WHERE Id = @id";
                    deleteQuery.Parameters.AddWithValue($"@id", selectedRecordId);

                    deleteQuery.ExecuteNonQuery();
                    Console.WriteLine($"Row deleted sucessfuly. Press any key to continue.");
                    Console.ReadKey();
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("There is no record of the entered ID in the selected habit table. Press any key to return.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Method for creating a new habit in case the user wants to.
        /// </summary>
        internal void CreateHabit()
        {
            Console.Write("Insert a name for the new habit: ");
            var habitName = Console.ReadLine();

            Console.Write("\nInsert the unit of measurement of this habit (eg. 'Glasses' (of water each day)):  ");
            var unitName = Console.ReadLine();

            this.connection.Open();
            var createCmd = connection.CreateCommand();
            createCmd.CommandText = $"CREATE TABLE IF NOT EXISTS {habitName} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, {unitName} INTEGER)";
            createCmd.ExecuteNonQuery();
            this.connection.Close();

            habits.Add((habitName!, unitName!));
            Console.WriteLine("Habit created sucessfully. Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Helper method for viewing all habits
        /// </summary>
        internal void ViewHabits()
        {
            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"{i}: {habits[i].Item1}");

            }
            Console.WriteLine("-----------------------------");
        }

        /// <summary>
        /// Helper method for retreving specified habit from the user.
        /// </summary>
        /// <returns>Index of the habit in the habit list</returns>
        internal int SelectHabitFromDb()
        {
            bool validChoice = false;
            int selectedOption = 0;
            ViewHabits();

            while (!validChoice)
            {
                selectedOption = Interface.ParseSelection();

                if (selectedOption >= habits.Count || selectedOption < 0)
                {
                    Console.WriteLine("This is not a valid choice.");
                    continue;
                }

                validChoice = true;
            }

            return selectedOption;
        }

        /// <summary>
        /// Helper method for checking if the inserted date from the user is valid.
        /// </summary>
        /// <param name="date">Date that will be checked in form of a string</param>
        /// <returns>True if the date is valid, false otherwise.</returns>
        internal static bool IsValidDate(string date)
        {
            DateTime tempDate;
            string format = "dd-MM-yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;

            return DateTime.TryParseExact(date, format, provider, DateTimeStyles.None, out tempDate);
        }

        /// <summary>
        /// Helper method for checking if the record of the specified ID exists in the habit table.
        /// </summary>
        /// <param name="table">Name of the table to search in</param>
        /// <param name="id">ID of the record in form of an integer</param>
        /// <returns>True if the record does exist, false otherwise</returns>
        internal bool DoesRecordExist(string table, int id)
        {
            connection.Open();

            var existanceQuery = connection.CreateCommand();
            existanceQuery.CommandText = $"SELECT * FROM {table} WHERE Id = {id}";
            var foundRecord = existanceQuery.ExecuteScalar();

            connection.Close();

            if (foundRecord != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Helper method for seeding the initial table with some mock data
        /// </summary>
        internal void SeedDatabase()
        {
            List<(string habitName, string unitName)> seedHabits = new();

            seedHabits.AddRange(new List<(string habitName, string unitName)>
            {
                ("Drinking_Water", "Liters"),
                ("Running", "Kilometers"),
                ("Reading", "Pages"),
                ("Sleeping", "Hours")
            });

            this.connection.Open();
            var createCmd = connection.CreateCommand();

            foreach (var habit in seedHabits)
            {
                createCmd.CommandText = $"CREATE TABLE IF NOT EXISTS {habit.habitName} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, {habit.unitName} INTEGER)";
                createCmd.ExecuteNonQuery();
            }

            SeedTable(seedHabits[0]);
            this.connection.Close();
        }

        /// <summary>
        /// Helper method for seeding a concrete table with a 100 mock records
        /// </summary>
        /// <param name="habit"></param>
        internal void SeedTable((string habitName, string unitName) habit)
        {
            int day = 01;
            int month = 06;

            for (int i = 0; i < 100; i++)
            {
                var randomGenerator = new Random();
                int measure = randomGenerator.Next(1, 100);
                string date = $"{day++}-{month}-2024";

                if (day >= 25)
                {
                    day = 1;
                    month++;
                }

                InsertRecord(habit, date, measure);
            }
        }
    }
}
