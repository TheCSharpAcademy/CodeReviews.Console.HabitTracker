using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Text.RegularExpressions;
namespace HabitTracker
{
    internal class DataBase
    {
        string? ConnectionString { get; set; }
        static string? readResult;

        public void GetConnectionString(string folderPath) { ConnectionString = $"Data Source={folderPath}HabitTracker.db"; }

        public void MainMenu()
        {
            bool closeApp = false;
            do
            {
                Console.Clear();
                DisplayText("Main Menu\n");
                Console.WriteLine();
                DisplayText("Choose an option:\n");
                Console.WriteLine();
                DisplayText("\t0. Close Application\n");
                DisplayText("\t1. Insert Record\n");
                DisplayText("\t2. Delete Record\n");
                DisplayText("\t3. Update Record\n");
                DisplayText("\t4. Print All Records\n");
                DisplayText("\t5. Print Record from Specific Table\n");
                DisplayText("\t6. Create a new table.\n");
                DisplayText("\t7. Show various reports.\n");

                bool validInput = false;
                while (validInput == false)
                {
                    readResult = Console.ReadLine();
                    switch (readResult)
                    {
                        case "0":
                            DisplayText("Thank for using Habit Tracker. The app will close now.\n",5,500);
                            closeApp = true;
                            validInput = true;
                            break;
                        case "1":
                            InsertData(ConnectionString);
                            validInput = true;
                            break;
                        case "2":
                            DeleteData(ConnectionString);
                            validInput = true;
                            break;
                        case "3":
                            UpdateData(ConnectionString);
                            validInput = true;
                            break;
                        case "4":
                            PrintData(ConnectionString);
                            validInput = true;
                            break;
                        case "5":
                            PrintTable(ConnectionString);
                            validInput = true;
                            break;
                        case "6":
                            CreateNewTable();
                            validInput = true;
                            break;
                        case "7":
                            ReportMenu();
                            validInput = true;
                            break;
                        default:
                            DisplayText("Invalid option, please choose some of the above", 5, 500);
                            ClearText();
                            break;
                    }
                }
            } while (closeApp == false);
        }

        private static void InsertData(string connectionString)
        {
            DisplayText("Choose a table of record:\n");
            string? table;
            table = GetTable();

            try {
                string unit = SetUnit(connectionString, table);

                DisplayText("Choose a quantity:\n");
                int quantity = GetNumericInput();

                string date = GetFormatedDate();
                string sqlCommand = $"INSERT INTO {table} (Date,Quantity,Unit) VALUES($date,$quantity,$unit)";

                ExecuteQuery(connectionString, sqlCommand,test: true,date: date,quantity:quantity,unit: unit);
            }
            catch (Exception ex)
            { 
                ClearText();
                DisplayText($"{ex.Message}", 5, 2000);
            }

            string redo = Redo("Do you want to insert another row? Yes(Y) No(N)\n");
            if (redo == "y") InsertData(connectionString);
        }

        private static void DeleteData(string connectionString)
        {
            DisplayText("What kind of data do you want to delete?\n");
            DisplayText("1. Table\n");
            DisplayText("2. Data row(s)\n");

            int option;
            bool isValid = false;
            do
            {
                string? readResult = Console.ReadLine();
                isValid = int.TryParse(readResult, out option);
                if (!isValid)
                {
                    DisplayText("Invalid option, please choose between 1 and 2.", 5, 500);
                    ClearText();
                }
            } while (!isValid || (option < 1 && option > 2));

            if (option == 1) { DeleteTable(connectionString); }
            else if (option == 2) { DeleteRow(connectionString); }

            string redo = Redo("Do you want to delete other records?\n");
            if (redo == "y") DeleteData(connectionString);
        }

        private static void DeleteTable(string connectionString)
        {
            DisplayText("Choose a table of record:\n");
            string? table = GetTable();

            DisplayText($"Are you sure you want to drop the table \"{table}\"?\n");
            DisplayText("Y(yes), N(no)\n");
            do { readResult = Console.ReadLine(); } while (readResult == null && readResult.ToLower() != "y" && readResult.ToLower() != "n");
            if (readResult.ToLower() == "y")
            {
                string sqlCommand = $"DROP TABLE {table}";
                ExecuteQuery(connectionString, sqlCommand);
            }

            DisplayText("Press Enter to continue.");
            Console.ReadLine();
        }

        private static void DeleteRow(string connectionString)
        {
            DisplayText("Choose a table of record:\n");
            string? table = GetTable();

            DisplayText("How to delete rows:\n");
            Console.WriteLine("1. Single row based on id");
            Console.WriteLine("2. Single row based on date");
            Console.WriteLine("3. Multiple rows in an id range");
            Console.WriteLine("4. Multiple rows in a date range");
            Console.WriteLine("5. Multiple individual rows based on id");
            Console.WriteLine("6. Miltiple individual rows based on date");

            int option;
            do
            {
                option = GetNumericInput();
                if (option <1 || option > 6)
                {
                    DisplayText("Please choose an option between 1 and 6",5,500);
                    ClearText();
                }
            } while (option < 1 || option > 6);

            string sqlCommand;
            int id;
            int endId;
            string date;
            string endDate;
            switch (option)
            {
                case 1:
                    DisplayText("Select a row id\n");
                    id = GetNumericInput();
                    sqlCommand = $"DELETE FROM {table} WHERE rowid = $id";
                    ExecuteQuery(connectionString, sqlCommand,id: id);
                    break;
                case 2:
                    DisplayText("Enter a date(yy-mm-dd):\n");
                    date = GetFormatedDate();
                    sqlCommand = $"DELETE FROM {table} WHERE Date = $date";
                    ExecuteQuery(connectionString, sqlCommand);
                    break;
                case 3:
                    DisplayText("Enter starting id (inculded in deletion):\n");
                    id = GetNumericInput();
                    DisplayText("Enter ending id (included in deletion):\n");
                    endId = GetNumericInput();
                    sqlCommand = $"DELETE FROM {table} WHERE rowid >= $id AND rowid <= $endId";
                    ExecuteQuery(connectionString, sqlCommand,id:id,endId:endId);
                    break;
                case 4:
                    DisplayText("Enter starting date(yy-mm-dd / included in deletion):\n");
                    date = GetFormatedDate();
                    DisplayText("Enter ending date(yy-mm-dd / included in deletion):\n");
                    endDate = GetFormatedDate();
                    sqlCommand = $"DELETE FROM {table} WHERE Date >= $date AND Date <= $endDate";
                    ExecuteQuery(connectionString, sqlCommand,date:date,endDate:endDate);
                    break;
                case 5:
                    sqlCommand = $"DELETE FROM {table} WHERE " + GetMultipleIds();
                    ExecuteQuery(connectionString, sqlCommand);
                    break;
                case 6:
                    sqlCommand = $"DELETE FROM {table} WHERE " + GetMultipleDates();
                    ExecuteQuery(connectionString, sqlCommand);
                    break;
            }

            string redo = Redo("Do you want to delete another row? Yes(Y) No(N)\n");
            if (redo == "y") DeleteRow(connectionString);
        }

        private static void UpdateData(string connectionString)
        {
            DisplayText("Choose a table to update:\n");
            string? table = GetTable();

            int option;
            DisplayText("Choose a way to select the row to update.\n");
            DisplayText("1. Id\n");
            DisplayText("2. Date\n");
            do
            {
                option = GetNumericInput();
                if (option < 1 || option > 2)
                {
                    DisplayText("Invalid option, please choose a valid option between 1 and 2", 5, 500);
                    ClearText();
                }
            } while (option < 1 || option > 2);

            int id = -1;
            string? updateDate = "";
            bool byId = false;
            if (option == 1)
            {
                DisplayText("Choose the id to update:\n");
                id = GetNumericInput();
                byId = true;
            }
            else 
            {
                updateDate = GetFormatedDate();
            }

            DisplayText("Choose the value to update:\n");
            DisplayText("1. Quantity\n");
            DisplayText("2. Date\n");
            DisplayText("3. Quantity and Date.\n");

            do
            {
                option = GetNumericInput();
                if (option <1 || option > 3)
                {
                    DisplayText("Invalid option, please choose a valid option between 1 and 3", 5, 500);
                    ClearText();
                }
            } while (option < 1 || option > 3);
            string sqlCommand;

            if (option == 1 || option ==3)
            {
                if (byId)
                {
                    DisplayText($"Choose a new quantity for {table} where id = {id}:\n");
                    int quantity = GetNumericInput();

                    sqlCommand = $"UPDATE {table} SET Quantity = $quantity WHERE rowid = $id";
                    ExecuteQuery(connectionString, sqlCommand, quantity: quantity, id: id);
                }
                else
                {
                    DisplayText($"Choose a new quantity for {table} where Date = {updateDate}: (yy-mm-dd)\n");
                    int quantity = GetNumericInput();

                    sqlCommand = $"UPDATE {table} SET Quantity = $quantity WHERE Date = $date";
                    ExecuteQuery(connectionString, sqlCommand, quantity: quantity, date: updateDate);
                }
            }
            if (option == 2 || option == 3)
            {
                if (byId)
                {
                    DisplayText($"Enter a new date for {table} where id = {id}: (Format yy-MM-dd)\n");
                    string date = GetFormatedDate();

                    sqlCommand = $"UPDATE {table} SET Date = $date WHERE rowid = $id";
                    ExecuteQuery(connectionString, sqlCommand, date: date, id: id);
                }
                else
                {
                    DisplayText($"Enter a new date for {table} where Date = {updateDate}: (yy-mm-dd)\n");
                    string date = GetFormatedDate();

                    sqlCommand = $"UPDATE {table} SET Date = $date WHERE Date = $endDate";
                    ExecuteQuery(connectionString, sqlCommand, date: date, endDate: updateDate);
                }
            }

            string redo = Redo("Do you want to update other records? Yes(Y) No(N)\n");
            if (redo == "y") UpdateData(connectionString);
        }

        private static void PrintData(string connectionString)
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            SqliteCommand tableCmd = new SqliteCommand($"SELECT name FROM sqlite_schema WHERE type='table' ORDER BY name", connection);

            connection.Open();


            SqliteDataReader tableReader = tableCmd.ExecuteReader();
            try
            {
                if (!tableReader.HasRows) { DisplayText("Data base is empty, sorry.\n"); }
                else {
                    while (tableReader.Read() && tableReader != null)
                    {
                        string table = tableReader["name"].ToString();
                        SqliteCommand tableCmdBis = new SqliteCommand($"SELECT rowid,* FROM {table} ORDER BY Date", connection);
                        SqliteDataReader reader = tableCmdBis.ExecuteReader();

                        List<string> currentTable = new List<string>();
                        if (!reader.HasRows) { Console.WriteLine($"The table {table} contains no data."); }
                        else if (reader != null)
                        {
                            Console.WriteLine($"{table}:");
                            Console.WriteLine($"Id\tDate:\t\t\tQuantity\tUnit:");
                            while (reader.Read())
                            {
                                currentTable.Add(reader[0].ToString());
                                string currentRowid = reader["rowid"].ToString();
                                string currentDate = reader["Date"].ToString();
                                string currentQuantity = reader["Quantity"].ToString();
                                string currentUnit = reader["Unit"].ToString();
                                Console.WriteLine($"{currentRowid}\t{currentDate}\t\t{currentQuantity}\t\t{currentUnit}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayText($"{ex.Message}\n");
            }

            connection.Close();
            DisplayText("Press Enter to continue\n");
            Console.ReadLine();
        }

        private static void PrintTable(string connectionString,bool report = false, string commandText = "",string aimedTable = "")
        {
            string? table;

            string? sqlCommandText;
            if (!report)
            {
                DisplayText("Choose a table of record:\n");
                table = GetTable();
                sqlCommandText = $"SELECT rowid,* FROM {table} ORDER BY Date";
            }
            else { sqlCommandText = commandText; table = aimedTable; }

            SqliteConnection connection = new SqliteConnection(connectionString);
            try
            {
                SqliteCommand tableCmd = new SqliteCommand(sqlCommandText, connection);

                connection.Open();

                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (!reader.HasRows) { Console.WriteLine($"The table {table} contains no data."); }
                else if (reader != null)
                {
                    Console.WriteLine($"{table}:");
                    Console.WriteLine($"Id\tDate:\t\t\tQuantity\tUnit:");
                    while (reader.Read())
                    {
                        string currentRowid = reader["rowid"].ToString();
                        string currentDate = reader["Date"].ToString();
                        string currentQuantity = reader["Quantity"].ToString();
                        string currentUnit = reader["Unit"].ToString();
                        Console.WriteLine($"{currentRowid}\t{currentDate}\t\t{currentQuantity}\t\t{currentUnit}");
                    }
                }
                else { DisplayText("Data base is empty, sorry."); }
            }
            catch (Exception ex) { DisplayText($"{ex.Message}\n"); }

            DisplayText("Press Enter to continue\n");
            Console.ReadLine();
            connection.Close();
        }

        public void CreateNewTable()
        {
            DisplayText("Enter a name for the new table.\n");
            string table = GetTable();
            CreateTable(table);
        }

        public void CreateTable(string table = "test_data")
        {
            // This function was made separatlly from the function above to be used to autoseed the database
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {table}(Date DATE,Quantity INTEGER,Unit TEXT);";
            ExecuteQuery(ConnectionString, sqlCommand);
        }

        private void ReportMenu()
        {
            Console.WriteLine("1. Sum of all quantity");
            Console.WriteLine("2. Count of all record");
            Console.WriteLine("3. Sum for specific month");
            Console.WriteLine("4. Count for specific month");
            Console.WriteLine("5. Sum for specific year");
            Console.WriteLine("6. Count for specific year");
            Console.WriteLine("7. Show specific month");
            Console.WriteLine("8. Show specific year");
            Console.WriteLine("9. Sum for two dates");
            Console.WriteLine("10. Count for two dates");
            Console.WriteLine("11. Show for two dates");

            int option;
            do
            {
                option = GetNumericInput();
                if (option < 1 || option > 11)
                {
                    DisplayText("Please choose an option between 1 and 11.\n", 5, 2000);
                    ClearText();
                }
            } while (option < 1 || option > 11);

            
            DisplayText("Choose a table:\n");
            string? table;
            do
            {
                table = GetTable();
                if(string.IsNullOrEmpty(table) || string.IsNullOrWhiteSpace(table))
                {
                    DisplayText("Please enter a valud name for a table");
                    ClearText();
                }
            } while (string.IsNullOrEmpty(table) || string.IsNullOrWhiteSpace(table));
            
            string month;
            string year;
            string sqlCommandText;
            switch (option)
            {
                case 1:
                    sqlCommandText = $"SELECT SUM(Quantity) as sum FROM {table}";
                    PrintSumCount(ConnectionString,sqlCommandText);
                    break;
                case 2:
                    sqlCommandText = $"SELECT COUNT(*) as count FROM {table}";
                    PrintSumCount(ConnectionString, sqlCommandText, true);
                    break;
                case 3:
                    month = GetMonthYear();
                    year = GetMonthYear(false);
                    sqlCommandText = $"SELECT SUM(Quantity) as sum,strftime('%Y-%m', Date) as month FROM {table} WHERE month = '{year}-{month}'";
                    PrintSumCount(ConnectionString, sqlCommandText);
                    break;
                case 4:
                    month = GetMonthYear();
                    year = GetMonthYear(false);
                    sqlCommandText = $"SELECT COUNT(*) as count,strftime('%Y-%m',Date) as month FROM {table} WHERE month = '{year}-{month}'";
                    PrintSumCount (ConnectionString, sqlCommandText,true);
                    break;
                case 5:
                    year = GetMonthYear(false);
                    sqlCommandText = $"SELECT SUM(Quantity) as sum,strftime('%Y',Date) as year FROM {table} WHERE year = '{year}'";
                    PrintSumCount(ConnectionString , sqlCommandText);
                    break;
                case 6:
                    year = GetMonthYear(true);
                    sqlCommandText = $"SELECT COUNT(*) as count,strftime('%Y',Date) as year FROM {table} WHERE year = '{year}'";
                    PrintSumCount(ConnectionString , sqlCommandText);
                    break;
                case 7:
                    month = GetMonthYear();
                    year = GetMonthYear(false);
                    sqlCommandText = $"SELECT strftime('%Y-%m',Date) as monthYear,rowid,* FROM {table} WHERE monthYear = '{year}-{month}' ORDER BY Date;";
                    PrintTable(ConnectionString,true,sqlCommandText);
                    break;
                case 8:
                    year = GetMonthYear(false);
                    sqlCommandText = $"SELECT strftime('%Y',Date) as year,rowid,* FROM {table} WHERE year = '{year}' ORDER BY Date";
                    PrintTable(ConnectionString,true,sqlCommandText);
                    break;
                case 9:
                    string startDate = GetFormatedDate();
                    string endDate = GetFormatedDate();
                    sqlCommandText = $"SELECT SUM(Quantity) as sum,strftime('%Y-%m-%d',Date) as date FROM {table} WHERE date >= '{startDate}' AND date <= '{endDate}'";
                    PrintSumCount(ConnectionString, sqlCommandText);
                    break;
                case 10:
                    startDate = GetFormatedDate();
                    endDate = GetFormatedDate();
                    sqlCommandText = $"SELECT COUNT(*) as count,strftime('%Y-%m-%d',Date) as date FROM {table} WHERE date >= '{startDate}' AND date <= '{endDate}'";
                    PrintSumCount(ConnectionString, sqlCommandText, true);
                    break;
                case 11:
                    startDate = GetFormatedDate();
                    endDate = GetFormatedDate();
                    sqlCommandText = $"SELECT *,rowid,strftime('%Y-%m-%d',Date) as date FROM {table} WHERE date >= '{startDate}' AND date <= '{endDate}'";
                    PrintTable(ConnectionString, true, sqlCommandText);
                    break;
                default:
                    break;
            }

            string redo = Redo("Do you want to print another report? Yes(Y) No(N)\n");
            if (redo == "y") ReportMenu();
        }

        public void PrintSumCount(string connectionString, string sqlCommandText,bool count = false)
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            try
            {
                SqliteCommand tableCmd = new SqliteCommand(sqlCommandText, connection);

                connection.Open();
                bool empty = false;
                try
                {
                    tableCmd.ExecuteScalar();
                }
                catch
                {
                    empty = true;
                }
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (empty) { Console.WriteLine($"The table contains no data for your request."); }
                else if (reader != null) {
                    if (count) { while (reader.Read()) Console.WriteLine($"{reader["count"].ToString()}"); }
                    else { while (reader.Read()) Console.WriteLine($"{reader["sum"].ToString()}"); }
                }
                else { DisplayText("Data base is empty, sorry."); }

                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                ClearText();
                DisplayText($"{ex.Message}\n",5,2000);
            }
        }

        private static string GetTable()
        {
            // Get table name only disallowing some characters to prevent injections
            // '_' is the only special char allowed, whitespaces are trimmed
            string? table;
            bool valid;
            do
            {
                table = Console.ReadLine().ToString().Trim();
                valid = table.IndexOfAny(new char[] {'/','-','\\','\'','"','(','[','{','?','!','&','>','<','='}) == -1;
                if (!valid)
                {
                    DisplayText("unvalid table name format, please enter a valid name.",5,2000);
                    ClearText();
                }
            } while (!valid || string.IsNullOrEmpty(table) || string.IsNullOrWhiteSpace(table));
            return table;
        }

        private static int GetNumericInput()
        {
            // Get only numeric integer type inputs to prevent injections
            int number;
            bool isValid = false;
            do
            {
                string? readResult = Console.ReadLine();
                isValid = int.TryParse(readResult, out number);
                if (!isValid)
                {
                    DisplayText("Invalid option, please choose a valid integer number", 5, 500);
                    ClearText();
                }
            } while (!isValid);
            return number;
        }

        private static string GetFormatedDate()
        {
            // Get dates in a specified format to prevent injections
            string? readResult;
            bool isValid;
            bool validDate;
            string resultDate;
            DisplayText("Choose a date(YY-MM-DD):\n");
            do
            {
                readResult = Console.ReadLine().ToString();
                DateTime dateTime;
                validDate = DateTime.TryParseExact(readResult, "yy-MM-dd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out dateTime);
                resultDate = dateTime.ToString("yyyy-MM-dd");
                isValid = Regex.IsMatch(resultDate, "([0-9][0-9][0-9][0-9])-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])");
                if (!isValid || !validDate)
                {
                    DisplayText("Invalid option, please enter date in the format above.", 5, 500);
                    ClearText();
                }
            } while (!isValid || !validDate);

            return resultDate;
        }

        private static string GetMultipleIds()
        {
            List<int> idList = new List<int>();
            string? readResult;
            DisplayText("Enter ids (Enter exit or s to stop):\n");
            do
            {
                int id;
                readResult = Console.ReadLine();
                bool valid = int.TryParse(readResult, out id);
                if (valid)
                {
                    idList.Add(id);
                }
            } while (readResult.ToLower() != "exit" && readResult.ToLower() != "s");

            int[] ids = idList.ToArray();
            string sqlCommand = "";
            string sqlCommandText;
            int len = ids.Length;
            int count = 1;
            foreach (int id in ids)
            {
                if (count == len) { sqlCommandText = $"rowid = {id}"; }
                else { sqlCommandText = $"rowid = {id} OR "; }
                foreach (char letter in sqlCommandText) { sqlCommand += letter; }
                count++;
            }
            return sqlCommand;
        }

        private static string GetMultipleDates()
        {
            List<string> dateList = new List<string>();
            string? readResult;
            string resultDate;
            bool isValid;
            bool validDate;
            DisplayText("Enter dates(yy-mm-dd)(Enter exit or s to stop):\n");
            do
            {
                readResult = Console.ReadLine().ToString();
                DateTime dateTime;
                validDate = DateTime.TryParseExact(readResult, "yy-MM-dd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out dateTime);
                resultDate = dateTime.ToString("yyyy-MM-dd");
                isValid = Regex.IsMatch(resultDate, "([0-9][0-9][0-9][0-9])-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])");
                if (isValid && validDate)
                {
                    dateList.Add(resultDate);
                }

            } while (readResult.ToLower() != "exit" && readResult.ToLower() != "s");

            string[] dates = dateList.ToArray();
            string sqlCommand = "";
            string sqlCommandText;
            int len = dates.Length;
            int count = 1;
            foreach (string date in dates)
            { 
                if (count == len) { sqlCommandText = $"Date = '{date}'"; }
                else { sqlCommandText = $"Date = '{date}' OR "; }
                foreach (char letter in sqlCommandText) { sqlCommand += letter;}
                count++;
            }

            return sqlCommand;
        }

        private static string GetMonthYear(bool getMonth = true)
        {
            int userInput;
            string inputMonth;
            if (getMonth)
            {
                DisplayText("Select a month between 1 and 12(mm):\n");
                string month;
                do
                {
                    userInput = GetNumericInput();
                    if (userInput < 1 || userInput > 12)
                    {
                        DisplayText("Please choose a number between 1 and 12 for the month", 5, 2000);
                        ClearText();
                    }
                    else
                    {
                        if (userInput < 10)
                        {
                            inputMonth = $"0{userInput}";
                        }
                        else
                        {
                            inputMonth = userInput.ToString();
                        }
                        DateTime dateTime;
                        DateTime.TryParseExact(inputMonth, "mm", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out dateTime);
                        month = dateTime.ToString("mm");
                        return month;
                    }
                } while (userInput < 1 || userInput > 12);
            }
            else
            {
                bool validYear;
                string year = "";
                DisplayText("Select a year(yyyy):\n");
                do
                {
                    userInput = GetNumericInput();
                    DateTime dateTime;
                    validYear = DateTime.TryParseExact(userInput.ToString(), "yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out dateTime);
                    if(validYear) year = dateTime.ToString("yyyy");
                    else
                    {
                        DisplayText("Please enter year as yyyy format (ex: 2024).", 5, 500);
                        ClearText();
                    }
                } while (!validYear);
                return year;
            }
            return "Error";
        }

        private static string SetUnit(string connectionString, string table)
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            SqliteCommand command = new($"SELECT Unit FROM {table}", connection);
            connection.Open();
            string? unit;
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                DisplayText("First time entering data for this record, please choose a unit:\n");
                do
                {
                    unit = GetTable();
                    if (string.IsNullOrEmpty(unit) || string.IsNullOrWhiteSpace(unit))
                    {
                        DisplayText("Please enter a valid unit for the record.", 10, 500);
                        ClearText();
                    }
                } while (string.IsNullOrEmpty(unit) || string.IsNullOrWhiteSpace(unit));
                connection.Close();
                return unit;
            }
            else
            {
                reader.Read();
                unit = reader["Unit"].ToString();
                connection.Close();
                return unit;
            }
        }

        public static void ExecuteQuery(string connectionString, string sqlCommand, bool test = false, string date = "", string endDate = "", string unit = "", int quantity = 0, int id = -1, int endId = -1)
        {
            try
            {
                SqliteConnection connection = new(connectionString);
                connection.Open();
                SqliteCommand? tableCmd = connection.CreateCommand();
                tableCmd.CommandText = sqlCommand;

                // Parametrized query are used to avoid injections
                tableCmd.Parameters.AddWithValue("$date", date);
                tableCmd.Parameters.AddWithValue("$quantity", quantity);
                tableCmd.Parameters.AddWithValue("$unit", unit);
                tableCmd.Parameters.AddWithValue("$id", id);
                tableCmd.Parameters.AddWithValue("$endDate", endDate);
                tableCmd.Parameters.AddWithValue("$endId", endId);
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                ClearText();
                DisplayText($"{ex.Message}\n", 5, 2000);
            }
        }

        private static string Redo(string message)
        {
            DisplayText(message);
            do
            {
                readResult = Console.ReadLine();
                if (readResult.ToLower() != "y" && readResult.ToLower() != "n")
                {
                    DisplayText("Please choose Between Yes (Y) or No (N).", 5, 500);
                    ClearText();
                }
                else if (readResult.ToLower() == "n")
                {
                    DisplayText("Press Enter to continue.");
                    Console.ReadLine();
                }
            } while (readResult.ToLower() != "n" && readResult.ToLower() != "y");
            return readResult;
        }

        private static void DisplayText(string message, int speed = 5, int sleep = 50)
        {
            string output = "\r";
            foreach (char letter in message)
            {
                while (Console.KeyAvailable)
                    Console.ReadKey(false);
                output += letter;
                Console.Write(output);
                Thread.Sleep(speed);
            }
            Thread.Sleep(sleep);
            while (Console.KeyAvailable)
                Console.ReadKey(false);
        }

        private static void ClearText()
        {
            Console.Write($"\r{new string(' ', Console.BufferWidth)}");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write($"\r{new string(' ', Console.BufferWidth)}");
            Console.SetCursorPosition(0, Console.CursorTop);
        }

    }

    internal class DataSeeder
    {
        public static List<DataSeeder> autoData = new List<DataSeeder>();
        string? Date { get; set; }
        int Quantity { get; set; }

        public static void AutoSeed(string connectionString)
        {
            // Verifies if database contains any table and fills it with test purposed dataseed if empty
            // didn't find a way to verify if it only exists
            bool dbExists = true;
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var command = new SqliteCommand($"SELECT * FROM sqlite_master WHERE type='table'", connection))
                {
                    connection.Open();
                    try
                    {
                        dbExists = command.ExecuteScalar() != null;
                    }
                    catch
                    {
                        dbExists = false;
                    }
                    connection.Close();
                }
            }

            if (!dbExists)
            {
                SqliteConnection connection = new(connectionString);
                connection.Open();
                string table1 = "CREATE TABLE IF NOT EXISTS test_data(Date DATE,Quantity INTEGER,Unit TEXT);";
                DataBase.ExecuteQuery(connectionString, table1);
                string table2 = "CREATE TABLE IF NOT EXISTS coffee_per_day(Date DATE,Quantity INTEGER,Unit TEXT);";
                DataBase.ExecuteQuery(connectionString, table2);
                string table3 = "CREATE TABLE IF NOT EXISTS fruits(Date DATE,Quantity INTEGER,Unit TEXT);";
                DataBase.ExecuteQuery(connectionString, table3);
                SqliteCommand sqlCommand = new("SELECT * FROM test_data", connection);
                SqliteDataReader reader = sqlCommand.ExecuteReader();
                connection.Close();


                if (!reader.HasRows)
                {
                    Random random = new();
                    DateTime start = DateTime.Today.AddDays(-100);
                    DateTime end = DateTime.Today;

                    List<string> dates = new List<string>();
                    for (DateTime dt = start; dt <= end; dt = dt.AddDays(1))
                    {
                        dates.Add(dt.ToString("yyyy-MM-dd"));
                    }

                    List<DataSeeder> autoData = new List<DataSeeder>();
                    foreach (string dateItem in dates)
                    {
                        autoData.Add(new DataSeeder
                        {
                            Quantity = random.Next(1, 20),
                            Date = dateItem,
                        });
                    }

                    int count = 0;
                    foreach (DataSeeder dataItem in autoData)
                    {
                        if (count < 50)
                        {
                            string sqlCommandText = $"INSERT INTO test_data (Date,Quantity,Unit) VALUES('{dataItem.Date}',{dataItem.Quantity},\"test_unit\");";
                            DataBase.ExecuteQuery(connectionString, sqlCommandText);
                            count++;
                        }
                        else if (count < 80)
                        {
                            string sqlCommandText = $"INSERT INTO coffee_per_day (Date,Quantity,Unit) VALUES('{dataItem.Date} ', {dataItem.Quantity},\"mug_of_coffee\");";
                            DataBase.ExecuteQuery(connectionString, sqlCommandText);
                            count++;
                        }
                        else
                        {
                            string sqlCommandText = $"INSERT INTO fruits (Date,Quantity,Unit) VALUES('{dataItem.Date} ', {dataItem.Quantity},\"fruit_per_day\");";
                            DataBase.ExecuteQuery(connectionString, sqlCommandText);
                        }
                    }
                }
            }
        }
    }
}