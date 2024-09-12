using Microsoft.Data.Sqlite;
using System.Globalization;
namespace habit_tracker

{
    public static class Repository
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        public static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Menu.DisplayMenu();

                string? menuInput = Console.ReadLine();
                if (menuInput != null)
                {
                    int ChosenOption = int.Parse(menuInput);
                    switch (ChosenOption)
                    {
                        case 0:
                            Console.WriteLine("Bye Bye!");
                            Thread.Sleep(1000);
                            closeApp = true;
                            break;

                        case 1:
                            GetRecords();
                            Console.WriteLine("Press any key to return back to main menu");
                            Console.ReadKey();
                            break;

                        case 2:
                            InsertRecord();
                            break;

                        case 3:
                            DeleteRecord();
                            Console.Clear();
                            break;

                        case 4:
                            UpdateRecord();
                            break;

                        default:
                            Console.WriteLine("\nInvalid command. Please, enter number from 0 to 4\n");
                            Thread.Sleep(1500);
                            break;
                    }
                }
            }
        }
        public static string GetDateInput(string message)
        {
            Console.WriteLine(message);
            string? dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\n**Enter 0 to go to main menu**\nInvalid date. Format: dd-mm-yy.");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string? numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();
            while (numberInput == null || !(int.Parse(numberInput) > 0))
            {
                Console.WriteLine("\n**Enter 0 to go to main menu**\nInvalid number. Number must be integer and no decimals allowed. Enter correct number:");
                numberInput = Console.ReadLine();
            }
            int numberInputConverted = int.Parse(numberInput);
            return numberInputConverted;
        }
        public static void GetRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM csharp_lessons_learned ";
                List<CsharpLessons> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new CsharpLessons
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No records found");
                }

                connection.Close();
                Console.Clear();
                Console.WriteLine("\n------------------------------------------------\n\n");
                foreach (var cl in tableData)
                {
                    Console.WriteLine($"{cl.Id} - {cl.Date.ToString("dd-MMM-yyyy")} - Quantity: {cl.Quantity}");
                }
                Console.WriteLine("\n\n------------------------------------------------\n");



            }
        }
        public static void InsertRecord()
        {
            string date = GetDateInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the date: (format: dd-mm-yy).\n");
            int quantity = GetNumberInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the number of lessons: (number must be integer, no decimals allowed)\n");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO csharp_lessons_learned(date, quantity) VALUES('{date}','{quantity})')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        public static void DeleteRecord()
        {
            Console.Clear();
            GetRecords();
            var recordId = GetNumberInput("**Enter 0 to go to main menu**\nPlease, enter the Id of the record that you want to delete");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from csharp_lessons_learned WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.\n");
                    Thread.Sleep(1500);
                    connection.Close();
                    DeleteRecord();
                }
            }
            Console.WriteLine($"\nRecord with Id {recordId} was deleted succesfully.\n");
            Console.WriteLine("Press any key to return back to main menu");
            Console.ReadKey();

        }
        public static void UpdateRecord()
        {
            Console.Clear();
            GetRecords();
            var recordId = GetNumberInput("**Enter 0 to go to main menu**\nPlease, enter the Id of the record that you want to update");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCMD = connection.CreateCommand();
                checkCMD.CommandText = $"SELECT EXISTS(SELECT 1 FROM csharp_lessons_learned WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCMD.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"record with Id {recordId} doesn't exist.\n\n");
                    Thread.Sleep(1500);
                    connection.Close();
                    UpdateRecord();
                }

                string date = GetDateInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the date: (format: dd-mm-yy).\n");
                int quantity = GetNumberInput("\n\n**Enter 0 to go to main menu**\nPlease, insert the number of lessons: (number must be integer, no decimals allowed)\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE csharp_lessons_learned SET date = '{date}', quantity = '{quantity}' WHERE Id = '{recordId}'";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Console.WriteLine($"\nRecord with Id {recordId} was updated succesfully.\n");
            Console.WriteLine("Press any key to return back to main menu");
            Console.ReadKey();

        }
        public static void CreateTable()
        {
            string connectionString = @"Data Source=habit-Tracker.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"Create Table if not exists csharp_lessons_learned (
                                        id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                        Date TEXT, 
                                        Quantity INTEGER
                                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

    }
}