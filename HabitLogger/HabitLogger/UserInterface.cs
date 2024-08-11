using HabitLogger.Menus;
using HabitLoggerLibrary;

namespace HabitLogger;

public  class UserInterface
{
    private string _currentHabitTable;

    public void DisplayMainMenu()
    {
        bool continueRunning = true;
        MainMenu mainMenu = new MainMenu();

        while (continueRunning)
        {
            MainMenuOptions mainMenuOptionPicked = ShowMenu<MainMenuOptions>(mainMenu);
            HandleMenuOption(mainMenuOptionPicked, ref continueRunning);
        }
    }

    public void DisplayHabitMenu()
    {
        bool continueRunning = true;
        HabitMenu habitMenu = new HabitMenu();

        while(continueRunning)
        {
            HabitMenuOptions habitMenuOptionPicked = ShowMenu<HabitMenuOptions>(habitMenu);
            HandleHabitMenuOption(habitMenuOptionPicked, ref continueRunning);
        }
    }

    public void DisplayReportsMenu()
    {
        bool continueRunning = true;
        ReportsMenu reportsMenu = new ReportsMenu();

        while (continueRunning)
        {
            ReportsMenuOption reportsMenuOptionPicked = ShowMenu<ReportsMenuOption>(reportsMenu);
            HandleReportsMenuOption(reportsMenuOptionPicked, ref continueRunning);
        }
    }
    public void HandleMenuOption(MainMenuOptions option, ref bool continueRunning)
    {
        int userId, userQuantity;
        DateTime userDay;

        switch (option)
        {
            case MainMenuOptions.Quit:
                continueRunning = false;
                break;
            case MainMenuOptions.Create:
                Console.WriteLine("\nInserting...");
                userDay = ShowMenu<DateTime>(new DayMenu());
                userQuantity = ShowMenu<int>(new QuantityMenu());
                HabitController.InsertRecord(new HabitModel { Day = userDay, Quantity = userQuantity }, _currentHabitTable);
                Console.WriteLine("\nRecord inserted successfully");
                break;
            case MainMenuOptions.Update:
                Console.WriteLine("\nUpdating...");
                ShowRecords();
                userId = ShowMenu<int>(new IdMenu());
                userDay = ShowMenu<DateTime>(new DayMenu());
                userQuantity = ShowMenu<int>(new QuantityMenu());
                HabitController.UpdateRecord(new HabitModel { Id = userId, Day = userDay, Quantity = userQuantity }, _currentHabitTable);
                Console.WriteLine("\nRecord updated successfully");
                break;
            case MainMenuOptions.Delete:
                Console.WriteLine("\nDeleting...");
                ShowRecords();
                userId = ShowMenu<int>(new IdMenu());
                HabitController.DeleteRecord(new HabitModel { Id = userId }, _currentHabitTable);
                Console.WriteLine("\nRecord deleted successfully");
                break;
            case MainMenuOptions.Show:
                ShowRecords();
                break;
            case MainMenuOptions.Reports:
                DisplayReportsMenu();
                break;
            case MainMenuOptions.HabitMenu:
                DisplayHabitMenu();
                break;
            default:
                Console.WriteLine("Invalid option selected. Please try again.");
                break;
        }
    }
    public void HandleHabitMenuOption(HabitMenuOptions option, ref bool continueRunning)
    {
        int habitId;
        string habitName;
        Dictionary<int, string> habits = new();

        switch (option)
        {
            case HabitMenuOptions.Quit:
                continueRunning = false;
                break;
            case HabitMenuOptions.Choose:
                Console.WriteLine("\nChoosing habit...");
                habits = ShowHabits();
                Console.WriteLine("0: Go back to menu");
                habitId = ShowMenu<int>(new HabitIdMenu());
                if(habitId != 0)
                {
                    _currentHabitTable = habits[habitId];
                    continueRunning = false;
                }
                break;
            case HabitMenuOptions.Create:
                Console.WriteLine("\nCreating habit...");
                ShowHabits();
                habitName = ShowMenu<string>(new HabitNameMenu());
                UserDefinedHabitsController.CreateHabit(habitName, "INT");
                _currentHabitTable = habitName;
                continueRunning = false;

                Console.WriteLine("\nHabit created successfully");
                break;
            case HabitMenuOptions.Delete:
                Console.WriteLine("\nDeleting habit...");
                habits = ShowHabits();
                habitId = ShowMenu<int>(new HabitIdMenu());
                UserDefinedHabitsController.DeleteHabit(habits[habitId]);
                Console.WriteLine("\nHabit deleted successfully");
                break;
            default:
                Console.WriteLine("Invalid option selected. Please try again.");
                break;
        }
    }

    public void HandleReportsMenuOption(ReportsMenuOption option, ref bool continueRunning)
    {
        switch (option)
        {
            case ReportsMenuOption.Quit:
                continueRunning = false;
                break;
            case ReportsMenuOption.QuantityPerYear:
                ShowQuantityPerYear();
                break;
            case ReportsMenuOption.RecordsPerYear:
                ShowTimesPerYear();
                break;
            default:
                Console.WriteLine("Invalid option selected. Please try again.");
                break;
        }
    }

    public Dictionary<int, string> ShowHabits()
    {
        Console.WriteLine();
        Console.WriteLine("List of habits: ");
        Dictionary<int, string> habits = UserDefinedHabitsController.GetHabits();

        foreach (KeyValuePair<int, string> record in habits)
        {
            Console.WriteLine($"{record.Key}: {record.Value}");
        }

        return habits;
    }

    public void ShowRecords()
    {
        Console.WriteLine();
        Console.WriteLine("List of records: ");
        foreach (var record in HabitController.GetRecords(_currentHabitTable))
        {
            Console.WriteLine($"{record.Id}. Day: {record.Day.ToString("yyyy-MM-dd")}, Quantity: {record.Quantity}");
        }
    }

    public void ShowQuantityPerYear()
    {
        Console.WriteLine();
        Console.WriteLine("List of records: ");
        foreach ((int year, int totalQty) record in HabitController.GetQuantityPerYearRecords(_currentHabitTable))
        {
            Console.WriteLine($"Year: {record.year}, Total Quantity: {record.totalQty}");
        }
    }

    public void ShowTimesPerYear()
    {
        Console.WriteLine();
        Console.WriteLine("List of records: ");
        foreach ((int year, int times) record in HabitController.GetTimesPerYearRecords(_currentHabitTable))
        {
            Console.WriteLine($"Year: {record.year}, Times: {record.times}");
        }
    }

    public T ShowMenu<T>(IMenu menu)
    {

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine(menu.GetMenu());

            string? input = Console.ReadLine();

            IValidator<T> validator = ValidatorFactory.GetValidator<T>();
            (bool inputValid, T userInput) = validator.Validate(input);
            if (inputValid)
            {
                return userInput;
            }

            Console.WriteLine("Invalid input. Please try again.");
        }
    }
}
