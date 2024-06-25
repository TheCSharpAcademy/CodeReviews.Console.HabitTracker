﻿using DatabaseLibrary;
using UserInputLibrary;
class HabitTracker
{
    static DateTime start;
    static Random gen;
    static int range;
    static string habit = "none";
    static Database database = new();
    static UserInput userInput = new();
    static List<Table> tableData = database.tableData;
    static List<string> table = database.table;

    public static void Main()
    {
        Console.WriteLine("Welcome, Tracking your habit is a benefical way to grow and be accountable\n");
        SeedDatabase();
        UseTable(); // created for when SeedDatabase cannot be used eg prod
        GetUsersInput();
    }

    public static void ShowMenu()
    {
        Console.WriteLine("Select an option");
        Console.WriteLine("1 - Create Habit");
        Console.WriteLine("2 - Insert");
        Console.WriteLine("3 - See all tracked habit");
        Console.WriteLine("4 - Update");
        Console.WriteLine("5 - Delete");
        Console.WriteLine("6 - List Habits");
        Console.WriteLine("7 - See Report");
        Console.WriteLine("8 - change habit table");
        Console.WriteLine("0 - or 0 to Exit");
    }

    public static void GetUsersInput()
    {
        bool exit = false;

        while (!exit)
        {
            ShowMenu();
            var userInput = Console.ReadLine();
            Console.Clear();
            switch (userInput)
            {
                case "1":
                    CreateHabit();
                    break;
                case "2":
                    InsertHabit();
                    break;
                case "3":
                    DisplayRecords();
                    break;
                case "4":
                    UpdateHabit();
                    break;
                case "5":
                    DeleteHabit();
                    break;
                case "6":
                    ShowHabits();
                    break;
                case "7":
                    ViewReport();
                    break;
                case "8":
                    ChangeTable();
                    break;
                case "0":
                    Console.WriteLine("GoodBye");
                    exit = true;
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input selected choose from the menu");
                    break;
            }
        }
    }

    static void RandomDate()
    {
        start = new DateTime(2023, 1, 1);
        gen = new Random();
        range = (DateTime.Today - start).Days;
    }

    static string GetRandomDate()
    {
        string date = start.AddDays(gen.Next(range)).ToString("dd/MM/yy");
        return date.Replace("/", "-");
    }

    public static void SeedDatabase()
    {
        database.Create("walking", "km");
        database.Create("drinking_water", "litres");


        if (!database.TableHasRows("walking_km"))
        {
            Random random = new Random();
            RandomDate();
            for (int i = 0; i < 100; i++)
            {
                database.Insert("walking_km", GetRandomDate(), random.Next(1, 15));
            }
        }
        if (!database.TableHasRows("drinking_water_litres"))
        {
            Random random = new Random();
            RandomDate();
            for (int i = 0; i < 100; i++)
            {
                database.Insert("drinking_water_litres", GetRandomDate(), random.Next(1, 4));
            }
        }
    }

    static void CreateHabit()
    {
        string newHabit = userInput.GetHabitInput("Enter Habit to Track, Note This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)");
        string unit = userInput.GetUnitInput();
        database.Create(newHabit, unit);
        Console.WriteLine("Created Sucessfully, Choose another option\n");
    }

    static void InsertHabit()
    {
        string date = userInput.GetDateInput();
        int quantity = userInput.GetNumberInput("Enter only numeric quantity in any measurement(No decimal allowed)");
        database.Insert(habit, date, quantity);
        Console.Clear();
        Console.WriteLine("Record Inserted Sucessfully, Choose another option\n");
    }

    static void DisplayRecords()
    {
        DisplayTable(habit);
    }
    static void UpdateHabit()
    {
        DisplayTable(habit);

        if (tableData.Count > 0)
        {
            int id = userInput.GetNumberInput("Enter the Id you want to update from the list");

            if (!IdExists(id))
            {
                Console.WriteLine("No item with the following id, please select an id from the table");
                UpdateHabit();
                return;
            }

            string date = userInput.GetDateInput();
            int quantity = userInput.GetNumberInput("Enter only numeric quantity in any measurement(No decimal allowed)");
            database.Update(habit, date, quantity, id);
            Console.Clear();
            Console.WriteLine("Updated Record, Choose another option\n");
        }
    }

    static void DeleteHabit()
    {
        DisplayTable(habit);
        if (tableData.Count > 0)
        {
            int id = userInput.GetNumberInput("Enter the Id you want to delete from the list");
            if (!IdExists(id))
            {
                Console.WriteLine("No item with the following id, please select an id from the table");
                DeleteHabit();
                return;
            }
            database.Delete(habit, id);
            Console.Clear();
            Console.WriteLine("Deleted Record, Choose another option\n");
        }
  
    }
    public static bool IdExists(int id)
    {
        bool exist = false;
        foreach (var data in tableData)
        {
            if (data.Id == id)
            {
                exist = true;
                break;
            }
        }
        return exist;
    }

    public static bool HabitExists(string habit)
    {
        bool exist = database.table.Contains(habit);
        return exist;
    }

    public static void ChangeTable()
    {
        habit = "none";
        UseTable();
    }

    public static void UseTable()
    {
        ShowHabits();
        if(table.Count == 0)
        {
            CreateHabit();
            UseTable();
        }
        if(habit == "none")
        { 
            habit = userInput.GetHabitInput("Enter a Habit to perform actions on");
            Console.Clear();
            while (!HabitExists(habit))
            {
                ShowHabits();
                habit = userInput.GetHabitInput("No habit with the following habit with the following, Enter a valid habit");
                Console.Clear();
            }
        }
    }

    public static void DisplayTable(string habit)
    {

        tableData.Clear();
        database.Retrieve(habit);
        Console.WriteLine("------------------------------\n");
        Console.WriteLine($"Habit\t{habit}\n");
        Console.WriteLine("------------------------------\n");
        Console.WriteLine($"Id\tDate\t\tQuantity");

        foreach (var data in tableData)
        {
            Console.WriteLine($"{data.Id}\t{data.Date.ToString("dd-MM-yy")}\t{data.Quantity}");
        }
        Console.WriteLine("------------------------------\n");
    }

    public static void ShowHabits()
    {
        table.Clear();
        database.GetTables();

        if (table.Count > 0)
        {
            Console.WriteLine("------------------------------\n\n");
            Console.WriteLine("Habits\n");
            foreach (var data in table)
            {
                Console.WriteLine(data);
            }
            Console.WriteLine("------------------------------\n");
        }
    }

    public static void ViewReport()
    {
        database.Analysis(habit);
    }
}