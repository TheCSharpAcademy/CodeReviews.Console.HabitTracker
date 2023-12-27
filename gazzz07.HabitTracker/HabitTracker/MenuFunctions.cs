using Microsoft.Data.Sqlite;
using System.Globalization;


namespace HabitTracker
{
    internal class MenuFunctions
    {
        internal static void UserMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("Habit Tracker Main Menu\n------------\nPlease select an option: ");
                Console.WriteLine("'1' - Create A New Record\n'2' - Read Records\n'3' - Update Records\n'4' - Delete Records\n'5' - Generate Habit Reports\n'0' - Exit Application");
                Console.WriteLine("-----\nYour choice: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        Create();
                        break;
                    case "2":
                        Read();
                        break;
                    case "3":
                        Update();
                        break;
                    case "4":
                        Delete();
                        break;
                    case "5":
                        GenerateReport();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\nInvalid, please select an number from the list.\n");
                        break;
                }
            }
        }
        internal static void Create()
        {
            Console.Clear();



            string date = Helpers.GetDateInput();

            int quantity = Helpers.GetNumberInput("\n\nPlease insert the number of miles (to the nearest mile) you ran today!");

            using (var connection = new SqliteConnection(Helpers.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO running(date, quantity) VALUES('{date}', {quantity})";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        internal static void Read()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(Helpers.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM running";
                List<RunningMiles> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new RunningMiles()
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            }); ;
                    }
                }
                else
                {
                    Console.WriteLine("Database has no rows, please create some rows!");
                }

                connection.Close();

                Console.WriteLine("------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("---------------\n");
            }

        }
        internal static void Update()
        {
            Console.Clear();
            Read();

            var recordId = Helpers.GetNumberInput("\n\nPlease type the ID of the record you want to update, or press 0 to return to the main menu.");

            using (var connection = new SqliteConnection(Helpers.connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM running WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    connection.Close();
                    Update();
                }

                string date = Helpers.GetDateInput();

                int quantity = Helpers.GetNumberInput("\n\nPlease insert the number of miles (to the nearest mile) you ran today!");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE running SET date = '{date}', quantity = {quantity} WHERE Id = '{recordId}'";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        internal static void Delete()
        {
            Console.Clear();
            Read();

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you want to delete, or press 0 to return to the main menu.");

            using (var connection = new SqliteConnection(Helpers.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM running WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    Delete();
                }

            }
            Console.WriteLine($"\n\nRecord {recordId} was deleted. \n\n");
        }
        internal static void GenerateReport()
        {
            Console.Clear();

            var reportInput = Helpers.GetNumberInput("Which type of report would you like to run?\n'1' - Total miles ran\n'2' - Number of times miles have been logged\n'0' - Return to Main Menu\n");

            switch (reportInput)
            {
                case 1:
                    Console.WriteLine("Total Miles");
                    TotalHoursReport();
                    break;
                case 2:
                    Console.WriteLine("Times Logged");
                    TimesLoggedReport();
                    break;
                default:
                    Console.WriteLine("Invalid");
                    break;
            }
        }

        internal static void TotalHoursReport()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(Helpers.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT SUM(quantity) FROM running";

                object result = tableCmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    int totalQuantity = Convert.ToInt32(result);
                    Console.WriteLine($"Total miles ran: {totalQuantity}");
                }
                else
                {
                    Console.WriteLine("You haven't added anything to the tracker!");
                }
                Console.WriteLine("Press any key to return to the main menu.");
                Console.WriteLine("--------------------");
                Console.ReadKey();
            }
        }
        internal static void TimesLoggedReport()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(Helpers.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT COUNT(DISTINCT date) FROM running";

                object result = tableCmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    int timesLogged = Convert.ToInt32(result);
                    Console.WriteLine($"You have added to your running tracker {timesLogged} times since it's creation.");
                }
                else
                {
                    Console.WriteLine("You haven't added to your running tracker yet!");
                }
                Console.WriteLine("Press any key to return to the main menu.");
                Console.WriteLine("--------------------");
                Console.ReadKey();
            }
        }
    }
}
