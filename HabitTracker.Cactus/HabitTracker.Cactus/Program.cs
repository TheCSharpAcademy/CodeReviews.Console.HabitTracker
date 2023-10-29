
using Microsoft.Data.Sqlite;
using System.Globalization;

public class HabitTracker
{
    private const string CONNECTIONSTR = "Data Source=habit.db";

    static int Main(string[] args)
    {
        CreateWaterHabitTableIfNotExist();
        bool endApp = false;

        while (!endApp)
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Main Menu");
            Console.WriteLine("Type 0, exit app.");
            Console.WriteLine("Type 1, insert a water drinking habit record.");
            Console.WriteLine("-----------------------------------------");
            string input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    InsertWaterHabitRecord();
                    break;
                default:
                    break;
            }
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;
            Console.WriteLine("\n");
        }
        return 0;
    }

    public static void InsertWaterHabitRecord()
    {
        Console.Clear();
        DateTime date;
        int quantity;
        Console.WriteLine("Please type your date(dd-MM-yyyy):");
        string? dateStr = Console.ReadLine();
        while (!DateTime.TryParseExact(dateStr, "dd-MM-yyyy", new CultureInfo("en-US"),
                               DateTimeStyles.None, out date))
        {
            Console.WriteLine("Sorry, your date is invalid. Please type a valid date(dd-MM-yyyy):");
            dateStr = Console.ReadLine();
        }
        Console.WriteLine("Please type your quantity:");
        string? quantityStr = Console.ReadLine();
        while (!int.TryParse(quantityStr, out quantity))
        {
            Console.WriteLine("Sorry, your quantity is invalid. Please type a valid quantity:");
            quantityStr = Console.ReadLine();
        }
        WaterHabit habit = new();
        habit.Date = date;
        habit.Quantity = quantity;
        Insert(habit);
        Console.WriteLine("");
    }

    public static void CreateWaterHabitTableIfNotExist()
    {
        using (var connection = new SqliteConnection(CONNECTIONSTR))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS waterHabit (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    date Date NOT NULL,
                    quantity INT NOT NULL
                );
            ";
            command.ExecuteNonQuery();
        }
    }

    public static void Insert(WaterHabit habit)
    {
        using (var connection = new SqliteConnection(CONNECTIONSTR))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO waterHabit(date, quantity) 
                                     VALUES('{habit.Date}', '{habit.Quantity}')";
            command.ExecuteNonQuery();
        }
    }

}

public class WaterHabit
{
    public int Id { set; get; }
    public DateTime Date { set; get; }
    public int Quantity { set; get; }
}