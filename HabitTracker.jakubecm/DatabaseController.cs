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
            loadQuery.CommandText = "SELECT HabitName, UnitName FROM Habits";

            using (var reader = loadQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tableName = reader.GetString(0);
                    string unitName = reader.GetString(1);

                    habits.Add((tableName, unitName));
                }
            }

            connection.Close();
        }

        /// <summary>
        /// Method for initializing the DB. Creates the database with some mock habits and seeds the first habit.
        /// </summary>
        public void InitializeDatabase(bool databaseExists)
        {
            if (!databaseExists)
            {
                CreateTables();
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

        internal void CreateTables()
        {
            connection.Open();
            using (var habitsTable = connection.CreateCommand())
            {
                habitsTable.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        HabitName TEXT, 
                        UnitName TEXT
                    )";
                habitsTable.ExecuteNonQuery();
            }

            using (var loggedTable = connection.CreateCommand())
            {
                loggedTable.CommandText = @"
                    CREATE TABLE IF NOT EXISTS HabitsLogged (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        HabitId INTEGER, 
                        Date TEXT, 
                        Quantity INTEGER,
                        FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                    )";
                loggedTable.ExecuteNonQuery();
            }
            connection.Close();
        }


        /// <summary>
        /// Method that gathers information to be able to insert a new record into a specified habit table.
        /// </summary>
        internal void PrepareInsert()
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");

            Console.WriteLine("Which habit would you like to log?");
            int habitId = SelectHabitFromDb();

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

            Console.WriteLine($"Enter amount of {FetchUnit(habitId)} below.");
            int quantity = Interface.ParseSelection();

            InsertRecord(habitId, date, quantity);
            Console.WriteLine("Insert successful. Press any key to continue.");
            Console.ReadKey();
        }

        internal string FetchUnit(int habitId)
        {
            string unitName = "units";

            connection.Open();

            using (var fetchQuery = connection.CreateCommand())
            {
                fetchQuery.CommandText = "SELECT UnitName FROM Habits WHERE Id = @habitId";
                fetchQuery.Parameters.AddWithValue("@habitId", habitId);

                using (var reader = fetchQuery.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        unitName = reader.GetString(0);
                    }
                }
            }
            connection.Close();

            return unitName;
        }

        /// <summary>
        /// CRUD method for creating a new record in the specified table
        /// </summary>
        /// <param name="habitId">The ID of the habit in the Habits table</param>
        /// <param name="date">The date of the record</param>
        /// <param name="quantity">Value of the unit that will be inserted</param>
        internal void InsertRecord(int habitId, string date, int quantity)
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = $"INSERT INTO HabitsLogged (HabitId, Date, Quantity) VALUES (@habitId, @date, @quantity)";
            insertCmd.Parameters.AddWithValue("@habitId", habitId);
            insertCmd.Parameters.AddWithValue("@date", date);
            insertCmd.Parameters.AddWithValue("@quantity", quantity);

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// CRUD method for reading all records of a specified habit
        /// </summary>
        internal void ViewRecords()
        {
            Console.WriteLine("Which habit records would you like to see?");
            int habitId = SelectHabitFromDb();
            string unitName = FetchUnit(habitId);

            var viewString = connection.CreateCommand();

            viewString.CommandText = @"SELECT hl.Id, hl.Date, hl.Quantity FROM HabitsLogged hl JOIN Habits h ON hl.HabitId = h.Id WHERE h.Id = @habitId";

            viewString.Parameters.AddWithValue("@habitId", habitId);

            connection.Open();
            using (var reader = viewString.ExecuteReader())
            {
                Console.Clear();

                if (reader.HasRows)
                {
                    Console.WriteLine("HABIT RECORDS");
                    Console.Write("------------------------------------------\n");
                    Console.WriteLine($"\tID\tDate\t\t{unitName}");

                    while (reader.Read())
                    {
                        var habitLogId = reader.GetInt32(0);
                        var date = reader.GetString(1);
                        var quantity = reader.GetInt32(2);
                        Console.WriteLine($"\t{habitLogId}\t{date}\t{quantity}");
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
            connection.Close();
        }

        /// <summary>
        /// CRUD method for updating a record of the specified habit
        /// </summary>
        internal void UpdateRecord()
        {
            Console.WriteLine("Which habit do you wish to update?");
            var habitId = SelectHabitFromDb();

            Console.WriteLine("What is the ID of the record you wish to update?");
            var selectedRecordId = Interface.ParseSelection();

            bool recordExists = DoesRecordExist(selectedRecordId);

            if (recordExists)
            {
                Console.Write("Insert the updated quantity for this record: ");
                var updatedMeasure = Interface.ParseSelection();

                try
                {
                    connection.Open();
                    var updateQuery = connection.CreateCommand();
                    updateQuery.CommandText = $"UPDATE HabitsLogged SET Quantity = @quantity WHERE Id = @id";
                    updateQuery.Parameters.AddWithValue($"@quantity", updatedMeasure);
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

            Console.WriteLine("What is the ID of the record you wish to delete?");
            var selectedRecordId = Interface.ParseSelection();

            bool recordExists = DoesRecordExist(selectedRecordId);

            if (recordExists)
            {
                try
                {
                    connection.Open();
                    var deleteQuery = connection.CreateCommand();
                    deleteQuery.CommandText = $"DELETE FROM HabitsLogged WHERE Id = @id";
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
            createCmd.CommandText = $"INSERT INTO Habits (HabitName, UnitName) VALUES (@habitName, @unitName)";
            createCmd.Parameters.AddWithValue("@habitName", habitName);
            createCmd.Parameters.AddWithValue("@unitName", unitName);
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
            connection.Open();
            var habitsQuery = connection.CreateCommand();
            habitsQuery.CommandText = "SELECT Id, HabitName FROM Habits";

            using (var reader = habitsQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    int habitId = reader.GetInt32(0);
                    string habitName = reader.GetString(1);
                    Console.WriteLine($"{habitId}: {habitName}");
                }
            }
            connection.Close();

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

                if (selectedOption > habits.Count || selectedOption <= 0)
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
        /// Helper method for checking if the record of the specified ID exists in the log table.
        /// </summary>
        /// <param name="id">ID of the record in form of an integer</param>
        /// <returns>True if the record does exist, false otherwise</returns>
        internal bool DoesRecordExist(int id)
        {
            connection.Open();

            var existanceQuery = connection.CreateCommand();
            existanceQuery.CommandText = "SELECT * FROM HabitsLogged WHERE Id = @id";
            existanceQuery.Parameters.AddWithValue("id", id);
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
                createCmd.Parameters.Clear();
                createCmd.CommandText = $"INSERT INTO Habits (HabitName, UnitName) VALUES (@habitName, @unitName)";
                createCmd.Parameters.AddWithValue("@habitName", habit.habitName);
                createCmd.Parameters.AddWithValue("@unitName", habit.unitName);
                createCmd.ExecuteNonQuery();
            }

            SeedTable();
            this.connection.Close();
        }

        /// <summary>
        /// Helper method for seeding the table with a 100 mock records
        /// </summary>
        internal void SeedTable()
        {
            int day = 01;
            int month = 06;

            for (int i = 0; i < 100; i++)
            {
                var randomGenerator = new Random();
                int habitId = randomGenerator.Next(1, 5);
                int measure = randomGenerator.Next(1, 100);
                string date = $"{day++}-{month}-2024";

                if (day >= 25)
                {
                    day = 1;
                    month++;
                }

                InsertRecord(habitId, date, measure);
            }
        }
    }
}
