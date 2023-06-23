using System.Globalization;
using Microsoft.Data.Sqlite;

namespace Habit_Logger
{
    public class DatabaseManagement
    {
        static readonly string connectionString = $"Data Source=habit-logger.db";
        private string _name;
        private string _userValue;

        public DatabaseManagement(string name, string value)
        {
            _name = name;
            _userValue = value;
            CheckDatabaseExists(name, value);
        }
        public DatabaseManagement() : this("running", "distance") { }
        public string Name
        {
            set
            {
                _name = value;
            }
            get
            {
                return _name;
            }
        }
        public void CheckDatabaseExists(string name, string _userValue)
        {
            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS {name} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    {_userValue} INTEGER
                    );";
            ExecuteUserQuery(tableCmd);            
        }
        internal void ExecuteUserQuery(SqliteCommand a)
        {
            try
            {
                a.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh no! An exception occurred.\n - Details: " + e.Message);
                Console.ReadLine();
            }
        }
        public string CreateNewRecord()
        {
            Console.Clear();
            Console.Write("Name of habit you want to track: ");
            string? newHabit = Console.ReadLine();

            while (newHabit == null || newHabit == "")
            {
                Console.Write("Invalid input please try again: ");
                newHabit = Console.ReadLine();
            }

            newHabit = newHabit.Replace(" ", "_");
            

            Console.WriteLine("Habit can't be tracked by time.");
            Console.WriteLine("Only in quantity (Distance, Quantity, Calories, Laps, e.g.)");
            Console.Write("Name of the the value you want to track: ");

            string? newUserValue = Console.ReadLine();

            while (newUserValue == null || newUserValue == "")
            {
                Console.Write("Invalid input please try again: ");
                newUserValue = Console.ReadLine();
            }

            newUserValue = newUserValue.Replace(" ", "_");
            _userValue = newUserValue;
            DatabaseManagement _ = new(newHabit, _userValue);

            Console.WriteLine($"New record created!<<{newHabit}>>");
            Thread.Sleep(1000);

            return newHabit;
        }
        public void Insert(string record)
        {
            string date = GetDateInput();
            int valueInsert = GetNumberInput($"Your {_userValue.ToLower()} is: ");

            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO {record}(date, {_userValue}) VALUES('{date}', {valueInsert})";
            ExecuteUserQuery(tableCmd);

        }
        public void GetAllRecords(string record)
        {
            Console.Clear();
            using var connection = new SqliteConnection(connectionString);

            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM {record}";           
            List<Records> tableData = new();
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new Records
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Value = reader.GetInt32(2)
                        }); ;
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            Console.Clear();
            Console.WriteLine("--------------------------------------\n");
            Console.WriteLine($"\t{record.ToUpper()} RECORD");
            Console.WriteLine("--------------------------------------\n");

            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - {_userValue}:{dw.Value}");
            }

            Console.WriteLine("--------------------------------------\n");
            Console.WriteLine("Press ENTER to continue . . .");

            Console.ReadLine();
        }

        public void Delete(string record)
        {
            Console.Clear();
            GetAllRecords(record);

            if (CheckEmptyTable(record)) return;
            var recordId = GetNumberInput("Please type the Id of the record you want to delete or 0 back to the Menu:");

            if (recordId == 0) return;
            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from {record} WHERE Id = '{recordId}'";
            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                Delete(record);
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");   
            
            Console.ReadKey();
        }
        public void Update(string record)
        {
            Console.Clear();
            GetAllRecords(record);

            if (CheckEmptyTable(record)) return;
            var recordId = GetNumberInput("Please type the Id of the record you want to Update or 0 back to the Menu:");
            if (recordId == 0) return;

            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {record} WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");

                connection.Close();
                Update(record);
            }

            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert value or other measure of your choice (no decimals allowed)\n\n");
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE {record} SET date = '{date}', {_userValue} = {quantity} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();            
        }
        public void Report(string record)
        {
            Console.Clear();

            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();
            if (CheckEmptyTable(record)) return;
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT COUNT(*) FROM {record}";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery < 3)
            {
                Console.WriteLine($"\n\nInsert at least {3 - checkQuery} more entries for the report !\n\n");

                connection.Close();
            }
            else
            {
                GetAllRecords(record);
                string value = _userValue.ToLower();
                int avgValue = GetAverage(record);
                int minValue = GetMin(record);
                int maxValue = GetMax(record);
                int totalValue = GetTotal(record);
                Console.WriteLine("\n-------------------------------");
                Console.WriteLine($"Average {value}: {avgValue} ");
                Console.WriteLine($"Min {value}: {minValue}\nMax value: {maxValue}");
                Console.WriteLine($"Total of {value} : {totalValue}");
                Console.WriteLine("-------------------------------");
            }

            Console.ReadLine();                
        }
        internal int GetAverage(string record)
        {
            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT AVG({_userValue}) FROM {record}";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            return checkQuery;            
        }
        internal int GetMin(string record)
        {
            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT MIN({_userValue}) FROM {record}";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            return checkQuery;            
        }
        internal int GetMax(string record)
        {
            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT MAX({_userValue}) FROM {record}";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            return checkQuery;
            
        }
        internal int GetTotal(string record)
        {
            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT SUM({_userValue}) FROM {record}";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());                  

            return checkQuery;            
        }
        internal bool CheckEmptyTable(string record)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT COUNT(*) FROM {record}";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"Table {record} has nothing inside. Press ENTER to go back . . .");
                Console.ReadLine();
                return true;
            }

            Console.ReadLine();

            return false;
        }
        internal string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy)\n");
            string? dateInput = Console.ReadLine();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Try again:\n\n");
                dateInput = Console.ReadLine();                
            }

            return dateInput;
        }
        internal int GetNumberInput(string input)
        {
            Console.Write(input);

            string? valueInsert = Console.ReadLine();

            while (!Int32.TryParse(valueInsert, out _) || Convert.ToInt32(valueInsert) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                valueInsert = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(valueInsert);

            return finalInput;
        }
    }
}