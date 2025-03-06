using Erix101.HabitTracker.Services;
using HabitTracker.Models;
using Spectre.Console;
using System.Data.SQLite;
using System.Globalization;
using Color = Spectre.Console.Color;

namespace HabitTracker.DataHelpers
{

    internal class SQLite
    {
        private SQLiteConnection _connection;

        //Colour Palette
        private Color tableBorderColour = Color.White;
        private Color habitTableHeadingColour = Color.White;
        private Color habitListColour = Color.White;
        private Color habitSelectedColour = Color.SpringGreen2;

        private Color habitLogTableHeadingColour = Color.SpringGreen2;
        private Color habitLogListColour = Color.SpringGreen2;

        int timesUsed;


        public SQLite(string connectionString)
        {
            this._connection = new SQLiteConnection(connectionString);
        }
        public void SetUpDataBase(string connectionString)
        {
            string query = @"
                   CREATE TABLE IF NOT EXISTS times_Used (
                    count  INT
                    );

                   CREATE TABLE IF NOT EXISTS habits (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    habitName TEXT,
                    habitUnit TEXT
                   );

                   CREATE TABLE IF NOT EXISTS habit_log (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    habit_id INTEGER NOT NULL,
                    Date TEXT,
                    Quantity INTEGER,
                    FOREIGN KEY (habit_id) REFERENCES habits(id) ON DELETE RESTRICT
                   )";

            PerformNonQuery(query);
        }

        private void PerformNonQuery(string query)
        {
            _connection.Open();
            using (var tableCmd = _connection.CreateCommand())
            {
                tableCmd.CommandText = query;
                tableCmd.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void AddExampleDataIfEmpty(string connection)
        {
            long lastHabitRowId;

            _connection.Open();
            var tableCmd = _connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM times_Used";
            SQLiteDataReader reader = tableCmd.ExecuteReader();
            var addUsedLog = _connection.CreateCommand();

            if (!reader.HasRows)
            {
                var exampleHabitTableCmd = _connection.CreateCommand();
                var exampleHabitLogTableCmd = _connection.CreateCommand();


                foreach (var exampleHabit in InitialStarterData.ExampleHabits)
                {
                    exampleHabitTableCmd.CommandText =
                        $"INSERT INTO habits (habitName, habitUnit) VALUES (@habit,@habitUnit)";
                    exampleHabitTableCmd.Parameters.AddWithValue("@habit", exampleHabit.Habit);
                    exampleHabitTableCmd.Parameters.AddWithValue("@habitUnit", exampleHabit.Unit);
                    exampleHabitTableCmd.ExecuteNonQuery();
                    lastHabitRowId = _connection.LastInsertRowId;

                    foreach (var exampleHabitLog in InitialStarterData.ExampleHabitLogs)
                    {
                        exampleHabitLogTableCmd.CommandText =
                            $"INSERT INTO habit_log (habit_id, Date, Quantity) VALUES (@habit_id,@Date,@Quantity)";
                        exampleHabitLogTableCmd.Parameters.AddWithValue("@habit_id", lastHabitRowId);
                        exampleHabitLogTableCmd.Parameters.AddWithValue("@Date", exampleHabitLog.Date);
                        exampleHabitLogTableCmd.Parameters.AddWithValue("@Quantity", exampleHabitLog.Quantity);
                        exampleHabitLogTableCmd.ExecuteNonQuery();
                    }
                }

                addUsedLog.CommandText =
                    $"INSERT INTO times_Used (count) VALUES (1)";
                addUsedLog.ExecuteNonQuery();
            }
            else
            {
                addUsedLog.CommandText =
                    $"UPDATE times_Used SET count = count + 1 WHERE rowid = 1";
                addUsedLog.ExecuteNonQuery();
            }

            _connection.Close();
        }
        public Table GetAllHabits(int chosenRow = 0)
        {
            _connection.Open();

            var tableCmd = _connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM habits";
            List<Habit> tableData = new();
            SQLiteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new Habit
                    {
                        Id = reader.GetInt32(0),
                        HabitName = reader.GetString(1),
                        HabitUnit = reader.GetString(2),
                    });
                }
            }
            else
            {
                Console.WriteLine("No records to View");
            }
            _connection.Close();
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.BorderColor(tableBorderColour);
            table.AddColumn($"[{habitTableHeadingColour}]ID[/]");
            table.AddColumn($"[{habitTableHeadingColour}]Habit[/]");
            table.AddColumn($"[{habitTableHeadingColour}]Unit[/]");

            Color colour = habitListColour;
            foreach (Habit hab in tableData)
            {
                if (chosenRow != 0)
                {
                    colour = hab.Id == chosenRow ? habitSelectedColour : habitListColour;
                }

                table.AddRow(
                    $"[{colour} bold]{hab.Id.ToString()}[/]",
                    $"[{colour} bold]{hab.HabitName}[/]",
                    $"[{colour} bold]{hab.HabitUnit}[/]"
                    );
            }
            if (chosenRow == 0)
            {
                AnsiConsole.Write(table);
            }
            return table;
        }
        public void DeleteHabit()
        {
            Console.Clear();
            GetAllHabits();
            var habitId = UserInputServices.GetNumberInput("\nPlease enter the ID of the record you wish to delete");
            _connection.Open();

            //Check if the Habit has any associated logs
            var chkLogCmd = _connection.CreateCommand();
            chkLogCmd.CommandText = $"SELECT * FROM habit_log where habit_id = @habitId";
            chkLogCmd.Parameters.AddWithValue("@habitId", habitId);
            SQLiteDataReader reader = chkLogCmd.ExecuteReader();
            if (reader.HasRows)
            {
                Console.WriteLine($"\nYou cannot delete habit with id {habitId} because it has associated habit logs. Press any key to continue");
                Console.ReadLine();
                reader.Close();
                _connection.Close();
                Console.Clear();
                GetAllHabits();
                return;
            }

            // Delete the Habit if no logs exist
            var tableCmd = _connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from habits WHERE Id = @habitId";
            tableCmd.Parameters.AddWithValue("@habitId", habitId);

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {habitId} doesn't exist.  No records were deleted.");
                _connection.Close();
                return;
            }
            _connection.Close();
            Console.WriteLine($"\n\nHabit with Id {habitId} was deleted \n\nPress any key to continue\n\n");
            Console.ReadLine();
            Console.Clear();
            GetAllHabits();
        }
        public void UpdateHabit()
        {
            Console.Clear();
            GetAllHabits();
            var habitId = UserInputServices.GetNumberInput("\nPlease enter the Id of the record you'd like to update.");

            _connection.Open();

            var checkCmd = _connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE id = @habitId)";
            checkCmd.Parameters.AddWithValue("@habitId", habitId);
            int chkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (chkQuery == 0)
            {
                Console.WriteLine($"\nRecord with Id {habitId} doesn't exist.\nPress any key to continue");
                Console.ReadLine();
                _connection.Close();
                return;
            }

            string habitName = UserInputServices.GetStringInput("Enter new habit name");
            string habitUnit = UserInputServices.GetStringInput("Enter new habit Unit");

            var tableCmd = _connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE habits SET habitName=@habitName , habitUnit=@habitUnit WHERE id=@habitId";
            tableCmd.Parameters.AddWithValue("@habitName", habitName);
            tableCmd.Parameters.AddWithValue("@habitUnit", habitUnit);
            tableCmd.Parameters.AddWithValue("@habitId", habitId);

            tableCmd.ExecuteNonQuery();
            _connection.Close();
            Console.WriteLine($"\n\nHabit with Id {habitId} was updated \n\nPress any key to continue\n\n");
            Console.ReadLine();
            Console.Clear();
            GetAllHabits();
        }
        public void GetAllHabitLogs(string requestOrigin)
        {
            Console.Clear();
            GetAllHabits();

            string message = "";
            if (requestOrigin == "View")
            {
                message = "\nPlease enter the Id of the habit you'd like to view.";
            }
            else if (requestOrigin == "Delete")
            {
                message = "\nPlease enter the Id of the habit you'd like to delete from";
            }
            else if (requestOrigin == "Update")
            {
                message = "\nPlease enter the Id of the habit you'd like to update";
            }

            int habitId = UserInputServices.GetNumberInput(message);


            _connection.Open();

            var checkCmd = _connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE id = @habitId)";
            checkCmd.Parameters.AddWithValue("@habitId", habitId);
            int chkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (chkQuery == 0)
            {
                Console.WriteLine($"\nRecord with Id {habitId} doesn't exist.\nPress any key to continue");
                Console.ReadLine();
                _connection.Close();
                Console.Clear();
                GetAllHabits();
                return;
            }
            var habitNameCmd = _connection.CreateCommand();
            string habitName = "";
            habitNameCmd.CommandText = @"
                SELECT h.habitName
                FROM habits h
                WHERE h.id =@habitId";
            habitNameCmd.Parameters.AddWithValue("@habitId", habitId);

            SQLiteDataReader nameReader = habitNameCmd.ExecuteReader();
            if (nameReader.HasRows)
            {
                while (nameReader.Read())
                {

                    habitName = nameReader.GetString(0).ToUpper();
                }
            }
            else
            {
                Console.WriteLine("No records to View");
            }


            var tableCmd = _connection.CreateCommand();
            tableCmd.CommandText = @"
                SELECT l.id, l.Date, l.Quantity, h.habitName , h.habitUnit
                FROM habit_log l
                JOIN habits h ON l.habit_id = h.id 
                WHERE l.habit_id = @habitId";
            tableCmd.Parameters.AddWithValue("@habitId", habitId);

            List<HabitLog> tableData = new();
            SQLiteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new HabitLog
                    {
                        Id = reader.GetInt32(0),
                        HabitDate = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-GB")),
                        HabitQuantity = reader.GetInt32(2),
                        HabitName = reader.GetString(3),
                        HabitUnit = reader.GetString(4)
                    });
                }
            }
            else
            {
                Console.WriteLine("No records to View");
            }
            _connection.Close();

            Table habits = GetAllHabits(habitId);

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.BorderColor(tableBorderColour);
            table.Title($"Your Logs for : {habitName}");
            table.AddColumn($"[{habitLogTableHeadingColour}]ID[/]");
            table.AddColumn($"[{habitLogTableHeadingColour}]Date[/]");
            table.AddColumn($"[{habitLogTableHeadingColour}]Quantity[/]");
            table.AddColumn($"[{habitLogTableHeadingColour}]Unit[/]");

            foreach (HabitLog hab in tableData)
            {
                table.AddRow(
                   $"[{habitLogListColour}]{hab.Id.ToString()}[/]",
                   $"[{habitLogListColour}]{hab.HabitDate.ToString("dd-MM-yy")}[/]",
                   $"[{habitLogListColour}]{hab.HabitQuantity.ToString()}[/]",
                   $"[{habitLogListColour}]{hab.HabitUnit}[/]"
                   );
            }
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddRow(habits, table);
            Console.Clear();
            AnsiConsole.Write(grid);
        }
        public void InsertHabitLog()
        {
            Console.Clear();
            GetAllHabits();
            int habitId = UserInputServices.GetNumberInput("\nPlease enter the Id of the habit you'd like to log.");

            _connection.Open();

            using (var checkCmd = _connection.CreateCommand())
            {

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE id = @habitId)";
                checkCmd.Parameters.AddWithValue("@habitId", habitId);
                int chkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (chkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with Id {habitId} doesn't exist.\nPress any key to continue");
                    Console.ReadLine();
                    _connection.Close();
                    return;
                }
            }


            string logDate = UserInputServices.GetDateInput().ToString("dd-MM-yy");
            int Quantity = UserInputServices.GetNumberInput($"\nPlease enter the quantity logged on {logDate}");


            using (var tableCmd = _connection.CreateCommand())
            {
                tableCmd.CommandText =
                    $"INSERT INTO habit_log (habit_id, Date, Quantity) VALUES (@habit_id,@Date,@Quantity)";
                tableCmd.Parameters.AddWithValue("@habit_id", habitId);
                tableCmd.Parameters.AddWithValue("@Date", logDate);
                tableCmd.Parameters.AddWithValue("@Quantity", Quantity);
                tableCmd.ExecuteNonQuery();
                _connection.Close();
            }


            Console.WriteLine($"\n\nHabit log with Id {habitId} was inserted \n\nPress any key to continue\n\n");
            Console.ReadLine();
            Console.Clear();
            GetAllHabits();
        }
        public void UpdateHabitLog()
        {
            GetAllHabitLogs("Update");
            int habitId = UserInputServices.GetNumberInput("\n\nPlease enter the Id of the log you'd like to update");

            _connection.Open();
            var checkCmd = _connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_log WHERE id = @habitId)";
            checkCmd.Parameters.AddWithValue("@habitId", habitId);
            int chkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (chkQuery == 0)
            {
                Console.WriteLine($"\nRecord with Id {habitId} doesn't exist.\nPress any key to continue");
                Console.ReadLine();
                _connection.Close();
                return;
            }
            string date = UserInputServices.GetDateInput().ToString("dd-MM-yy");
            int quantity = UserInputServices.GetNumberInput($"\n\nPlease enter the new quantity logged at {date}");

            var tableCmd = _connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE habit_log SET Date=@habitDate , Quantity=@habitQuantity WHERE id=@id";
            tableCmd.Parameters.AddWithValue("@habitDate", date);
            tableCmd.Parameters.AddWithValue("@habitQuantity", quantity);
            tableCmd.Parameters.AddWithValue("@id", habitId);

            tableCmd.ExecuteNonQuery();
            _connection.Close();
        }
        public void DeleteHabitLog()
        {
            GetAllHabitLogs("Delete");

            int habitId = UserInputServices.GetNumberInput("\n\nPlease enter the Id of the log you'd like to delete");

            _connection.Open();

            var tableCmd = _connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from habit_log WHERE id = @habitId";
            tableCmd.Parameters.AddWithValue("@habitId", habitId);

            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {habitId} doesn't exist. \nPress any Key to continue\n");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            _connection.Close();
        }
        public void InsertHabit()
        {
            Console.Clear();
            GetAllHabits();
            string habit = UserInputServices.GetStringInput("\nPlease enter the habit you wish to track\n\n");
            string habitUnit = UserInputServices.GetStringInput("\n\nPlease enter the unit you wish to track by\n\n");

            _connection.Open();
            var tableCmd = _connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO habits (habitName, habitUnit) VALUES (@habit,@habitUnit)";
            tableCmd.Parameters.AddWithValue("@habit", habit);
            tableCmd.Parameters.AddWithValue("@habitUnit", habitUnit);
            tableCmd.ExecuteNonQuery();
            _connection.Close();
            Console.Clear();
            GetAllHabits();
        }
    }
}
