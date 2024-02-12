using System.Data.SQLite;
using System.Globalization;

namespace HabitTracker;

class Program
{
    static void Main(string[] args)
    {
        string separator = "-----------------------------------------";
        bool appRunning = true;

        string connectionString = "Data Source=HabitDatabase.db;Version=3";

        SQLiteConnection connection = new(connectionString);

        CreateTable();

        MainMenu();

        void CreateTable()
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS situps (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                date TEXT, quantity INTEGER)";
            tableCommand.ExecuteNonQuery();
            connection.Close();
        }

        void MainMenu()
        {
            do
            {
                DisplayMenuOptions();

                GetMenuInput();
            } while (appRunning);
        }

        void DisplayMenuOptions()
        {
            DisplayHeader();

            Console.WriteLine($@"
        Main Menu

{separator}
Type 0 to close the application.
Type 1 to view all records.
Type 2 to insert a record.
Type 3 to delete a record.
Type 4 to update a record.
{separator}");
        }

        void DisplayHeader()
        {
            Console.Clear();
            Console.WriteLine($"Habit Tracker - Situps\n{separator}");
        }

        void GetMenuInput()
        {
            bool validInput;

            do
            {
                validInput = true;
                string? userInput = Console.ReadLine();

                if (userInput != null)
                {
                    switch (userInput)
                    {
                        case "0":
                            appRunning = false;
                            break;
                        case "1":
                            ViewRecords();
                            break;
                        case "2":
                            InsertRecord();
                            break;
                        case "3":
                            DeleteRecord();
                            break;
                        case "4":
                            UpdateRecord();
                            break;
                        default:
                            validInput = false;
                            Console.WriteLine("Invalid input. Enter a number from 1-4.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Enter a number from 1-4.");
                }
            } while (!validInput);
        }

        void ViewRecords()
        {
            DisplayHeader();

            Console.WriteLine($"\n\tView Records\n\n{separator}");

            GetRecords();

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
        }

        void GetRecords()
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText =
                @"SELECT * FROM situps";

            List<Situps> tableData = [];
            SQLiteDataReader reader = tableCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new Situps
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }

            connection.Close();

            foreach (var i in tableData)
            {
                Console.WriteLine($"{i.Id} - {i.Date:dd-MMM-yyyy} - Quantity: {i.Quantity}");
            }
        }

        void InsertRecord()
        {
            DisplayInsertOptions(1);

            string date = GetDateInput();

            if (date != "0")
            {
                DisplayInsertOptions(2);

                int quantity = GetHabitQuantity();

                if (quantity != 0)
                {
                    if (ConfirmInsert(date, quantity))
                    {
                        InsertInTable(date, quantity);
                    }
                }
            }
        }

        void DisplayInsertOptions(int step)
        {
            DisplayHeader();

            Console.WriteLine($"\n\tInsert Record\n\n{separator}\n(Type 0 to return to menu.)");

            if (step == 1)
            {
                Console.Write("Enter date (Format: dd-mm-yy): ");
            }
            else if (step == 2)
            {
                Console.Write("Enter number of situps: ");
            }
        }

        string GetDateInput()
        {
            string? dateInput = Console.ReadLine();

            if (dateInput != "0")
            {
                while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-AU"), DateTimeStyles.None, out _))
                {
                    Console.WriteLine("Invalid input.\nEnter date (Format: dd-mm-yy): ");
                    dateInput = Console.ReadLine();
                }
            }

            return dateInput;
        }

        int GetHabitQuantity()
        {
            string? situpsInput = Console.ReadLine();
            int validSitupsInput = 0;

            if (situpsInput != "0")
            {
                while (!int.TryParse(situpsInput, out validSitupsInput))
                {
                    Console.WriteLine("Invalid input. Only enter positive numbers.");
                    situpsInput = Console.ReadLine();
                }
            }

            return validSitupsInput;
        }

        bool ConfirmInsert(string date, int quantity)
        {
            DisplayHeader();

            Console.WriteLine($"\n\tInsert Record\n\n{separator}");
            Console.WriteLine($"Adding record: {quantity} situps on {date} date.");
            Console.WriteLine("Press Enter to confirm or type 0 to discard.");

            string? confirmInput = Console.ReadLine();

            if (confirmInput == "0")
            {
                return false;
            }

            return true;
        }

        void InsertInTable(string date, int quantity)
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText =
                @$"INSERT INTO situps(date, quantity) VALUES('{date}', {quantity})";
            tableCommand.ExecuteNonQuery();
            connection.Close();
        }

        void DeleteRecord()
        {
            DisplayHeader();

            DisplayDeleteOptions();

            DeleteSelectedRecords();
        }

        void DisplayDeleteOptions()
        {
            Console.WriteLine($"\n\tDelete Record\n\n{separator}");

            GetRecords();

            Console.Write("(Type 0 to return to menu.)\nType the id of the record you want to delete: ");
        }

        void DeleteSelectedRecords()
        {
            var idInput = Console.ReadLine();

            if (idInput != "0")
            {
                connection.Open();
                var deleteCommand = connection.CreateCommand();
                deleteCommand.CommandText = $"DELETE from situps WHERE Id = '{idInput}'";
                int rowCount = deleteCommand.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with Id:{idInput} doesn't exist.");
                    connection.Close();

                    DeleteRecord();
                }

                connection.Close();

                Console.WriteLine($"Record with id: {idInput} was deleted. Press Enter to return to menu.");
                Console.ReadLine();
            }
        }

        void UpdateRecord()
        {
            DisplayHeader();

            DisplayUpdateOptions();

            UpdateSelectedRecord();
        }

        void DisplayUpdateOptions()
        {
            Console.WriteLine($"\n\tUpdate Record\n\n{separator}");

            GetRecords();

            Console.Write("(Type 0 to return to menu.)\nType the id of the record you want to update: ");
        }

        void UpdateSelectedRecord()
        {
            bool running = true;

            do
            {
                string? idInput = Console.ReadLine();
                
                if (idInput != "0" && Int32.TryParse(idInput, out _))
                {
                    connection.Open();
                    var updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM situps WHERE Id = {idInput})";

                    int checkQuery = Convert.ToInt32(updateCommand.ExecuteScalar());

                    if (checkQuery == 0)
                    {
                        Console.WriteLine($"Record with Id {idInput} doesn't exist. Press enter to try again.");
                        Console.ReadLine();

                        connection.Close();
                        break;
                    }

                    Console.Write("Enter new date: ");
                    string date = GetDateInput();

                    if (date != "0")
                    {
                        Console.Write("Enter new quantity: ");

                        int quantity = GetHabitQuantity();

                        if (quantity != 0)
                        {
                            updateCommand = connection.CreateCommand();
                            updateCommand.CommandText =
                                $@"UPDATE situps SET date = '{date}', quantity = {quantity} WHERE Id = {idInput}";
                            updateCommand.ExecuteNonQuery();

                            Console.WriteLine("Record updated. Press Enter to return to menu.");
                            Console.ReadLine();

                            running = false;
                        }
                        else
                        {
                            running = false;
                        }
                    }
                    else
                    {
                        running = false;
                    }

                    connection.Close();
                }
                else if(idInput == "0")
                {
                    running = false;
                    connection.Close();
                }
                else
                {
                    connection.Close();
                    Console.WriteLine("Invalid input. Try again.");
                }
            } while (running);
        }
    }
}

public class Situps
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }
}