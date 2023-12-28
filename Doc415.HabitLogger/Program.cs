using Microsoft.Data.Sqlite;
namespace Doc415.HabitLogger
{
    internal class Program
    {
        static string habitName;
        static string habitUnit;
        static string tableName;
        static string connectionString = @"Data Source=habit-Logger.db";
        static SqliteConnection connection = new(connectionString);
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (!AnyPreviousSavedHabit())
            {
                GetHabit();
                CreateDb();
            }
            StringTyper($"Welcome. \nToday is {DateTime.Now}\n", 0, 0);
            UserMenu();
        }
        static bool AnyPreviousSavedHabit()
        {
            try
            {
                long? tableCount;
                using (connection)
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table'";
                    tableCount = (long)tableCmd.ExecuteScalar();
                    connection.Close();
                }
                if (tableCount == 0)
                {
                    return false;
                }
                else
                {
                    StreamReader sr = new StreamReader("habitNameData");
                    habitName = sr.ReadLine();
                    tableName = sr.ReadLine();
                    habitUnit = sr.ReadLine();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        static void UserMenu(bool clrscreen = false)
        {
            bool validEntry = false;
            do
            {
                Console.WriteLine(@"
What would you like to to? 
---------------------------------
a---Add a new record for today
b---Add a new custom record
d---Delete a record
u---Update a record
r---Look at all records
t---Look at todays report
m---Look at monthly report
q---Quit program");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.A:
                        AddNewRecord();
                        Console.Clear();
                        break;
                    case ConsoleKey.B:
                        AddNewCustomRecord();
                        Console.Clear();
                        break;
                    case ConsoleKey.D:
                        DeleteRecord();
                        Console.Clear();
                        break;
                    case ConsoleKey.U:
                        UpdateRecord();
                        Console.Clear();
                        break;
                    case ConsoleKey.R:
                        DisplayAllRecords();
                        Console.Clear();
                        break;
                    case ConsoleKey.T:
                        DisplayTodaysReport();
                        Console.Clear();
                        break;
                    case ConsoleKey.M:
                        DisplayMonthlyReport();
                        Console.Clear();
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                    default:
                        validEntry = false;
                        Console.Clear();
                        break;
                }
            } while (!validEntry);
        }
        static void AddNewRecord()
        {
            Console.Clear();
            StringTyper($"Date: {DateTime.Now.ToShortDateString()} ", 0, 0);
            StringTyper($"Enter quantity for {habitName}:      {habitUnit} ", 0, 1);
            Console.SetCursorPosition(20 + habitName.Length, 1);
            bool validInputEntry = false;
            int quantity;
            do
            {
                validInputEntry = int.TryParse(Console.ReadLine(), out quantity);
            } while (!validInputEntry);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"INSERT INTO {tableName} (Date,Quantity) VALUES (date(), {quantity})";
            tableCmd.ExecuteNonQuery();
            connection.Close();
            StringTyper($"Record added, Good Job! \nPress Enter to continue...", Console.CursorLeft, Console.CursorTop);
            Console.ReadLine();
        }
        static void AddNewCustomRecord()
        {
            Console.Clear();
            StringTyper($"Enter the date (yyyy-mm-dd):  ", Console.CursorLeft, Console.CursorTop);
            string? inputDate = "";
            bool validInputEntry = false;
            do
            {
                inputDate = Console.ReadLine();
                string[] parsed = inputDate.Split('-');
                if (parsed[0].Length > 4 || parsed[0].Length < 4 || !int.TryParse(parsed[0], out int dump) || parsed[0].StartsWith('0'))
                    validInputEntry = false;
                else
                {
                    if (!int.TryParse(parsed[1], out dump) || int.Parse(parsed[1]) > 12 || int.Parse(parsed[1]) < 1)
                    {
                        validInputEntry = false;
                    }
                    else
                    {
                        if (!int.TryParse(parsed[2], out dump) || int.Parse(parsed[2]) > 31 || int.Parse(parsed[2]) < 1)
                        {
                            validInputEntry = false;
                        }
                        else
                            validInputEntry = true;
                    }
                }
                if (!validInputEntry)
                    Console.WriteLine("Enter a valid date (yyyy-mm-dd)");
            } while (!validInputEntry);
            StringTyper($"Enter the quantity:       {habitUnit} ", Console.CursorLeft, Console.CursorTop);
            long newQuantity = 0;
            validInputEntry = false;
            do
            {
                Console.SetCursorPosition(21, Console.CursorTop);
                string inputQuantity = Console.ReadLine();
                validInputEntry = long.TryParse(inputQuantity, out newQuantity);
            } while (!validInputEntry);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"INSERT INTO {tableName} (Date,Quantity) VALUES ('{inputDate}', {newQuantity})";
            tableCmd.ExecuteNonQuery();
            connection.Close();
            StringTyper("Record added. Welldone!\nPress Enter to continue...", Console.CursorLeft, Console.CursorTop);
            Console.ReadLine();
        }
        static void DeleteRecord()
        {
            Console.Clear();
            DisplayAllRecords();
            StringTyper("Enter the Id of record to remove: ", Console.CursorLeft, Console.CursorTop);
            bool validInputEntry = false;
            int removingId = 0;
            do
            {
                string? input = Console.ReadLine();
                validInputEntry = int.TryParse(input, out removingId);
                if (!validInputEntry)
                    Console.WriteLine("Enter a valid Id (integer)");
            } while (!validInputEntry);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM {tableName} WHERE Id={removingId}";
            tableCmd.ExecuteNonQuery();
            connection.Close();
            StringTyper("Record deleted. Press Enter to continue...", Console.CursorLeft, Console.CursorTop);
            Console.ReadLine();
        }
        static void UpdateRecord()
        {
            Console.Clear();
            DisplayAllRecords();
            StringTyper("Enter the Id of record to update: ", Console.CursorLeft, Console.CursorTop);
            bool validInputEntry = false;
            int updatingId = 0;
            do
            {
                string? userInput = Console.ReadLine();
                validInputEntry = int.TryParse(userInput, out updatingId);
                if (!validInputEntry)
                    Console.WriteLine("Enter a valid Id (integer)");
            } while (!validInputEntry);
            StringTyper($"Enter the date  (yyyy-mm-dd) for Id-{updatingId}:  ", Console.CursorLeft, Console.CursorTop);
            string? inputDate = "";
            validInputEntry = false;
            do
            {
                inputDate = Console.ReadLine();
                string[] parsed = inputDate.Split('-');
                if (parsed[0].Length > 4 || parsed[0].Length < 4 || !int.TryParse(parsed[0], out int dump) || parsed[0].StartsWith('0'))
                    validInputEntry = false;
                else
                {
                    if (!int.TryParse(parsed[1], out dump) || int.Parse(parsed[1]) > 12 || int.Parse(parsed[1]) < 1)
                    {
                        validInputEntry = false;
                    }
                    else
                    {
                        if (!int.TryParse(parsed[2], out dump) || int.Parse(parsed[2]) > 31 || int.Parse(parsed[2]) < 1)
                        {
                            validInputEntry = false;
                        }
                        else
                            validInputEntry = true;
                    }
                }
                if (!validInputEntry)
                    Console.WriteLine("Enter a valid date (yyyy-mm-dd)");
            } while (!validInputEntry);
            StringTyper($"Enter the quantity for Id-{updatingId}:  ", Console.CursorLeft, Console.CursorTop);
            long updatedQuantity = 0;
            validInputEntry = false;
            do
            {
                string inputQuantity = Console.ReadLine();
                validInputEntry = long.TryParse(inputQuantity, out updatedQuantity);
            } while (!validInputEntry);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE {tableName} SET Date='{inputDate}', Quantity={updatedQuantity} WHERE Id={updatingId}";
            tableCmd.ExecuteNonQuery();
            connection.Close();
            StringTyper("Record updated. Press Enter to continue...", Console.CursorLeft, Console.CursorTop);
            Console.ReadLine();
        }
        static void DisplayAllRecords()
        {
            connection.Open();
            Console.WriteLine("Id".PadRight(10) + "Date".PadRight(20) + $"Quantity({habitUnit})");
            Console.WriteLine("---       ----------          -----------------");
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM {tableName}";
            tableCmd.CommandType = System.Data.CommandType.Text;
            SqliteDataReader myReader = tableCmd.ExecuteReader();
            int counter = 0;
            while (myReader.Read())
            {
                if (counter != 0 && counter % 20 == 0)
                {
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                Console.WriteLine(myReader["Id"].ToString().PadRight(10) + myReader["Date"].ToString().PadRight(20) + myReader["Quantity"]);
                counter++;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"-------------------------------------------------\nTotal: {counter} entries");
            Console.ForegroundColor = ConsoleColor.White;
            StringTyper("\nPress Enter To Continue ", Console.CursorLeft, Console.CursorTop);
            Console.ReadLine();
        }
        static void DisplayTodaysReport()
        {
            connection.Open();
            Console.WriteLine("Id".PadRight(10) + "Date".PadRight(20) + $"Quantity({habitUnit})");
            Console.WriteLine("---       ----------          -----------------");
            var tableCmd = connection.CreateCommand();
            string thisDay = FormatDateForQuery(DateTime.Today);
            tableCmd.CommandText = $"SELECT * FROM {tableName} WHERE Date = '{thisDay}'";
            tableCmd.CommandType = System.Data.CommandType.Text;
            SqliteDataReader myReader = tableCmd.ExecuteReader();
            var todaysQuantities = new List<long>();
            long total = 0;
            while (myReader.Read())
            {
                Console.WriteLine(myReader["Id"].ToString().PadRight(10) + myReader["Date"].ToString().PadRight(20) + myReader["Quantity"]);
                todaysQuantities.Add((long)myReader["Quantity"]);
                total += (long)myReader["Quantity"];
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Todays best:    {todaysQuantities.Max()} {habitUnit}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Todays avarage: {todaysQuantities.Average():N2} {habitUnit}");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Todays total:   {total} {habitUnit}\n");
            Console.ForegroundColor = ConsoleColor.White;
            StringTyper("Press Enter To Continue ", Console.CursorLeft, Console.CursorTop);
            Console.ReadLine();
        }
        static string FormatDateForQuery(DateTime date)
        {
            string thisDay = date.ToShortDateString();
            string[] parsedDay = thisDay.Split('.');
            Array.Reverse(parsedDay);
            thisDay = string.Join("-", parsedDay);
            return thisDay;
        }
        static void DisplayMonthlyReport()
        {
            connection.Open();
            Console.WriteLine("Id".PadRight(10) + "Date".PadRight(20) + $"Quantity({habitUnit})");
            Console.WriteLine("---       ----------          -----------------");
            var tableCmd = connection.CreateCommand();
            string thisDay = FormatDateForQuery(DateTime.Today);
            tableCmd.CommandText = $"SELECT * FROM {tableName} WHERE strftime('%Y',Date)=strftime('%Y',date('now')) AND  strftime('%m',Date) = strftime('%m',date('now'))";
            tableCmd.CommandType = System.Data.CommandType.Text;
            SqliteDataReader myReader = tableCmd.ExecuteReader();
            var thisMonthsQuantities = new List<long>();
            long total = 0;
            while (myReader.Read())
            {
                Console.WriteLine(myReader["Id"].ToString().PadRight(10) + myReader["Date"].ToString().PadRight(20) + myReader["Quantity"]);
                thisMonthsQuantities.Add((long)myReader["Quantity"]);
                total += (long)myReader["Quantity"];
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"This months best:    {thisMonthsQuantities.Max()} {habitUnit}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"This months avarage: {thisMonthsQuantities.Average():N2} {habitUnit}");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"This months total:    {total} {habitUnit}\n");
            Console.ForegroundColor = ConsoleColor.White;
            StringTyper("Press Enter To Continue ", Console.CursorLeft, Console.CursorTop);
            Console.ReadLine();
            Console.Clear();
        }
        static void GetHabit()
        {
            Console.Clear();
            Console.WindowHeight = 100;
            Console.WindowWidth = 100;
            string welcomeMesage = "Welcome to Habit Logger, your humble assistant.\nLet's start to configure.\nPlease enter the name of habit you want me to keep track:  ";
            StringTyper(welcomeMesage, 0, 0);
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            bool notValidEntry = true;
            do
            {
                Console.SetCursorPosition(cursorX, cursorY);
                habitName = Console.ReadLine();
                if (habitName == null || habitName == "")
                    notValidEntry = true;
                else notValidEntry = false;
            } while (notValidEntry);
            StreamWriter sw = new StreamWriter("habitNameData", false);
            sw.WriteLine(habitName);
            string[] parsedName = habitName.Split(" ");
            tableName = string.Join("_", parsedName);
            sw.WriteLine(tableName);
            StringTyper("Please enter the measure unit for your habit:  ", Console.CursorLeft, Console.CursorTop);
            cursorX = Console.CursorLeft;
            cursorY = Console.CursorTop;
            do
            {
                Console.SetCursorPosition(cursorX, cursorY);
                habitUnit = Console.ReadLine();
                if (habitUnit == null || habitUnit == "")
                    notValidEntry = true;
                else notValidEntry = false;
            } while (notValidEntry);
            sw.WriteLine(habitUnit);
            string[] parsedUnit = habitUnit.Split(" ");
            habitUnit = string.Join("_", parsedUnit);
            sw.WriteLine(habitUnit);
            sw.Close();
            StringTyper("Configuration completed. Let's begin! ", Console.CursorLeft, Console.CursorTop);
            Thread.Sleep(500);
            Console.Clear();
        }
        static void StringTyper(string input, int cursorX, int cursorY)
        {
            for (int i = 0; i < input.Length; i++)
            {
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write(input.Substring(0, i));
                Thread.Sleep(40);
            }
        }
        static void CreateDb()
        {
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {tableName} (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER
                    )";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
