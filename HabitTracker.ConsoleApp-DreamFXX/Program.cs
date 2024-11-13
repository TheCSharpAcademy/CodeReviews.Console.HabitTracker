using System.Data.SQLite;
using System.Globalization;

internal class Program
{
    static string connectionString = @"Data Source=HabitTracker-ConsoleApp.db";

    static void Main(string[] args)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = 
                @"CREATE TABLE IF NOT EXISTS cigarettes_smoked(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date DateTime,
                    Time Text,
                    CountOfCigs INTEGER
                    )"; 

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
        
        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();

        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("Welcome to Smoke Tracker!");
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("0 -> Save and Exit App\n");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("1 -> Show All records.");
            Console.WriteLine("2 -> Add a record.");
            Console.WriteLine("3 -> Delete a record");
            Console.WriteLine("4 -> Modify a record.");
            Console.WriteLine("------------------------------------------");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nHave a good day!\n");
                    closeApp = true;
                    Environment.Exit(1);
                    break;
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    AddRecord();
                    break;
                case "3":
                    DeleteRecod();
                    break;
                case "4":
                    ChangeRecord();
                    break;
                default:
                    Console.WriteLine("\n\nInvalid number of operation. Try Again. (0 - 4).\n\n");
                    break;
            }
        }
    }

    private static void ViewAllRecords()
    {
        Console.Clear();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM cigarettes_smoked";

            List<CigarettesSmoked> tableData = new();

            SQLiteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new CigarettesSmoked
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            Time = reader.GetString(2),
                            CountOfCigs = reader.GetInt32(3)
                        });
                }
            }
            else
            {
                Console.WriteLine("\n\nNo records added to show. Start noting your consumption!\n\n");
            }

            connection.Close();


            Console.WriteLine("------------CIGARETTES SMOKED LIST-----------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} -> {dw.Date} in {dw.Time}h // {dw.CountOfCigs} Cigarettes.");
            }
            Console.WriteLine("---------------------------------------------\n");
        }

        
    }

    private static void AddRecord()
    {
        string date = GetDate();
        string time = GetTime();
        
        int countOfCigs = CigsCountInput("\n\nEnter number of cigarettes you smoked.\n\n");

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open(); 
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO cigarettes_smoked(date, time, countOfCigs) VALUES('{date}', '{time}', {countOfCigs})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal static void ChangeRecord()
    {
        ViewAllRecords();

        var recordId = CigsCountInput("\n\nEnter ID number of record you want to modify.\n\n");

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM cigarettes_smoked WHERE Id = {recordId})";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with ID {recordId} does not exist. Enter ID number of an existing record.\n\n");
                connection.Close();
                ChangeRecord();
            }

            string date = GetDate();
            string time = GetTime();

            int countOfCigs = CigsCountInput("\n\nEnter how many cigarettes you smoked.\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE cigarettes_smoked SET date = '{date}', time = '{time}', CountOfCigs = {countOfCigs} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }


    }

    private static void DeleteRecod()
    {
        Console.Clear();
        ViewAllRecords();

        var recordId = CigsCountInput("Enter ID number of the record you want to DELETE.");


        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from cigarettes_smoked WHERE Id = {recordId}";

            int rowCount = tableCmd.ExecuteNonQuery();

            if(rowCount == 0)
            {
                Console.WriteLine($"Record with ID {recordId} does not exist. Try Again.");
                DeleteRecod();
            }
        }

        Console.WriteLine($"Record with {recordId} was succesfully deleted. Press ENTER to go back to the MENU.");
        Console.ReadLine();
        GetUserInput();
    }



    internal static string GetTime()
    {
        Console.WriteLine("\n\nEnter a time of the day you smoked. //\n0 -> menu\n");
        Console.Write("Please, Enter in this Format -> hh:mm - ");

        string timeinput = Console.ReadLine();

        if (timeinput == "0") GetUserInput();

       return timeinput;
    }

    internal static string GetDate()
    {
        Console.WriteLine("\n\nEnter a date.    //      Enter 0 to go back to the menu.\n\n");
        Console.Write("Type the date in this order -> DD-MM-YYYY - ");

        string dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nDate is typed in wrong format. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    internal static int CigsCountInput(string message)
    {
        Console.WriteLine(message);

        string countInput = Console.ReadLine();

        while (!Int32.TryParse(countInput, out _) || Convert.ToInt32(countInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            countInput = Console.ReadLine();
        }

        if (countInput == "0") GetUserInput();

        int intCountInput = Convert.ToInt32(countInput);

        return intCountInput;
    }
}

public class CigarettesSmoked
{
    public int Id { get; set; }
    public string Time { get; set; }
    public string Date { get; set; }
    public int CountOfCigs { get; set; }
}