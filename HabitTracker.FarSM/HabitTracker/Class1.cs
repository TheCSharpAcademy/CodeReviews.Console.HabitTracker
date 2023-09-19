using Microsoft.Data.Sqlite;
using System.Data;
using System.Globalization;

namespace HabitTracker
{
    class Program
    {
        static string connectionstring = @"Data Source=Habit-Tracker.db";

        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();  
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS media_time(
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER
                                        )";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
            GetUserInput();
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool doLoop = true;
            while (doLoop)
            {
                Console.WriteLine(@"    
    MAIN MENU
    ---------
    What do you want to do?
    Type 0 to exit app
    Type 1 to view the data
    Type 2 to insert data
    Type 3 to delete data
    Type 4 to update data
    Type 5 to retrieve specific data");
                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        doLoop= false;
                        Environment.Exit(0);
                        break;
                    case "1":
                        ViewData();
                        break;
                    case "2":
                        Insert();
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    case "5":
                        RetrieveSpecificData();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter the correct input");
                        break;
                }
            }
        }

        private static void RetrieveSpecificData()
        {
            Console.Clear();
            bool doLoop = true;
            while (doLoop)
            {
                Console.WriteLine(@"    
    
    What do you want to do?
    Type 0 to exit app
    Type 1 to view the data from a particular month
    Type 2 to view data in between specific dates
    Type 3 to view data for the last 10 days
    Type 4 to view data sorted by media view time
    Type 5 to view data for when media view time is more than an hour
    Type 6 to return to the main menu");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        doLoop = false;
                        Environment.Exit(0);
                        break;
                    case "1":
                        MonthData();
                        break;
                    case "2":
                        InBetweenDatesData();
                        break;
                    case "3":
                        TenDaysData();
                        break;
                    case "4":
                        SortedData();
                        break;
                    case "5":
                        HourPlusData();
                        break;
                    case "6":
                        GetUserInput();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter the correct input");
                        break;
                }
            }
        }

        private static void HourPlusData()
        {
            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM media_time WHERE (Quantity/60) >= 1 ";
                List<MediaTime> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new MediaTime
                        {
                            Id = reader.GetInt32(0),
                            date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                            viewTime = TimeSpan.FromMinutes(reader.GetInt32(2))
                        });
                    }
                }
                else Console.WriteLine("No data found");
                connection.Close();

                Console.Clear();
                Console.WriteLine("\n  ENTRIES WITH AN HOUR PLUS MEDIA VIEW TIME");
                PrintData(tableData);
            }
        }

        private static void SortedData()
        {
            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM media_time";
                List<MediaTime> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new MediaTime
                        {
                            Id = reader.GetInt32(0),
                            date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                            viewTime = TimeSpan.FromMinutes(reader.GetInt32(2))
                        });
                    }
                }
                else Console.WriteLine("No data found");
                connection.Close();

                tableData = tableData.OrderBy(x => x.viewTime).ToList();
                Console.Clear();
                Console.WriteLine("\n  ENTRIES ORDERED BY VIEW TIME");
                PrintData(tableData);
            }
        }

        private static void TenDaysData()
        {
            using (var connection = new SqliteConnection(connectionstring))
            {
                DateTime currentDate = DateTime.Now;
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM media_time";
                List<MediaTime> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")) >=
                           currentDate.AddMonths(-1) &&
                           DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")) <=
                           currentDate)
                        {
                            tableData.Add(new MediaTime
                            {
                                Id = reader.GetInt32(0),
                                date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                                viewTime = TimeSpan.FromMinutes(reader.GetInt32(2))
                            });
                        }
                    }
                }
                else Console.WriteLine("No data found");
                connection.Close();
                if (tableData.Count == 0) { Console.WriteLine("\n No data found matching the dates"); }
                else
                {
                    Console.Clear();
                    Console.WriteLine("  LAST MONTH ENTRY");
                    PrintData(tableData);
                }
            }
        }

        private static void InBetweenDatesData()
        {
            Console.Clear();
            Console.Write("\nStart date");
            string startDate = GetDateInput();
            Console.Write("\nEnd date");
            string endDate = GetDateInput();

            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM media_time";
                List<MediaTime> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if(DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")) >=
                           DateTime.ParseExact(startDate, "dd-MM-yyyy", new CultureInfo("en-US")) &&
                           DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")) <=
                           DateTime.ParseExact(endDate, "dd-MM-yyyy", new CultureInfo("en-US")))
                        {
                            tableData.Add(new MediaTime
                            {
                                Id = reader.GetInt32(0),
                                date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                                viewTime = TimeSpan.FromMinutes(reader.GetInt32(2))
                            });
                        }
                    }
                }
                else Console.WriteLine("No data found");
                connection.Close();
                if (tableData.Count == 0) { Console.WriteLine("\n No data found matching the dates"); }
                else {
                    Console.Clear();
                    Console.WriteLine($"\n  ENTRIES IN BETWEEN {startDate} & {endDate} ");
                    PrintData(tableData); 
                }
            }
        }

        private static void MonthData()
        {
            string monthYear = GetMonthYear();
            
            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM media_time WHERE substr(Date, 4, 7) = '{monthYear}'";
                List<MediaTime> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new MediaTime
                        {
                            Id = reader.GetInt32(0),
                            date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                            viewTime = TimeSpan.FromMinutes(reader.GetInt32(2))
                        });
                    }
                }
                else Console.WriteLine("No data found");
                connection.Close();

                Console.Clear();
                Console.WriteLine($"\n  ENTRIES FOR {monthYear}");
                PrintData(tableData);
            }
        }

        private static string GetMonthYear()
        {
            Console.WriteLine("\nEnter the month and year (Format: mm-yyyy).");
            string input = Console.ReadLine();

            while (!DateTime.TryParseExact(input, "MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid input. Enter the date (Format: mm-yyyy)");
                input = Console.ReadLine();
            }
            return input;
        }

        private static void PrintData(List<MediaTime> table)
        {
            Console.WriteLine("  ------------------------------------------------");
            Console.WriteLine("{0,10}{1,14}{2,25}", "Entry No", "Date", "View Time");
            Console.WriteLine("  ------------------------------------------------");
            foreach (var data in table)
            {
                Console.WriteLine($"{data.Id,6}: {data.date.ToString("dd-MM-yyyy"),19} {data.viewTime.ToString(),20}");
            }
            Console.WriteLine("  ------------------------------------------------");
        }

        private static void Update()
        {
            //Console.Clear();
            ViewData();
            int rowToUpdate = GetNumberInput("Type the row number that has to be updated.");

            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM media_time WHERE Id={rowToUpdate})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (checkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with Id {rowToUpdate} doesn't exist");
                    connection.Close();
                    Update();
                }
                else
                {
                    string date = GetDateInput();
                    int viewTime = GetNumberInput("\nEnter the time used for media viewing in minutes: ");

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"UPDATE media_time SET Date='{date}', Quantity={viewTime} WHERE Id={rowToUpdate}";
                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine($"Row {rowToUpdate} is updated.");
                }
            }
        }

        private static void Delete()
        {
            ViewData();
            int rowToDelete = GetNumberInput("Type the row number that has to be deleted.");

            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM media_time WHERE Id='{rowToDelete}'";
                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.Write($"\nRecord with Id {rowToDelete} doesn't exist. Press any key to re-enter");
                    Console.ReadLine(); 
                    Console.Clear();
                    Delete();
                }
                else {
                    connection.Close();
                    Console.WriteLine($"\nThe record with the Id {rowToDelete} is deleted. Press any key to go to the main menu");
                    Console.ReadLine();
                    GetUserInput();
                }
            }
        }

        private static void ViewData()
        {
            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM media_time";
                List<MediaTime> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new MediaTime
                        {
                            Id = reader.GetInt32(0),
                            date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                            viewTime = TimeSpan.FromMinutes(reader.GetInt32(2))
                        }) ; 
                    }
                }
                else Console.WriteLine("No data found");
                connection.Close();

                PrintData(tableData);
            }
        }

        private static void Insert()
        {
            string Date = GetDateInput();
            int Number = GetNumberInput("\nEnter the time used for media viewing in minutes: ");
            using (var connection = new SqliteConnection(connectionstring))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO media_time (Date,Quantity) VALUES('{Date}',{Number})";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            if (input == "0") GetUserInput();
            while (!int.TryParse(input, out _) || (Convert.ToInt32(input) < 0))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                input = Console.ReadLine();

            }
            int clearInput = Convert.ToInt32(input);
            return clearInput;
        }

        private static string GetDateInput()
        {
            Console.WriteLine("\nEnter the date (Format: dd-mm-yyyy).");
            string dateInput = Console.ReadLine();  
            if(dateInput == "0") GetUserInput();
            while(!DateTime.TryParseExact(dateInput,"dd-MM-yyyy",new CultureInfo("en-US"),DateTimeStyles.None,out _))
            {
                Console.WriteLine("Invalid input. Enter the date (Format: dd-mm-yyyy)");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
    }

    class MediaTime
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public TimeSpan viewTime { get; set; }

    }
}
