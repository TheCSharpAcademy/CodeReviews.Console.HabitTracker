namespace MiroiuDev.HabitTracker;
internal class Menu
{
    internal string Title { get; private set; }
    private readonly DrinkingWaterRepository _drinkingWaterRepository;

    internal Menu(string title, DrinkingWaterRepository drinkingWaterRepository)
    {
        Title = title;
        _drinkingWaterRepository = drinkingWaterRepository;
    }

    internal void ShowMainMenu()
    {
        Console.Clear();
        bool closeApp = false;

        while (!closeApp)
        {
            Console.WriteLine($"\n\n{Title}");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application");
            Console.WriteLine("Type 1 to View All Records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            Console.WriteLine("Type 5 to View Report");

            Validation.PrintStartSeparator();

            var command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye\n");
                    closeApp = true;
                    break;
                case "1":
                    GetAllRecords();
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
                    ViewReport();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    private void ViewReport()
    {
        Console.Clear();
        GetAllRecords();

        Console.WriteLine(@"Choose to view a raport from the options below:
1. How many glasses (or your measurement) of water you drank in total
2. How many glasses (or your measurement) of water you drank this year
");
        var input = Validation.GetString();

        switch (input)
        {
            case "1":
                int totalWaterDrank = _drinkingWaterRepository.TotalWaterDrank();

                Validation.PrintStartSeparator();

                Console.WriteLine($"You've drank {totalWaterDrank} of glasses (or your measurement) of water in total.");

                Validation.PrintEndSeparator();
                break;
            case "2":
                int totalWaterDrankThisYear = _drinkingWaterRepository.TotalWaterDrankInYear(DateTime.Now.Year);

                Validation.PrintStartSeparator();

                Console.WriteLine($"You've drank {totalWaterDrankThisYear} of glasses (or your measurement) of water this year.");

                Validation.PrintEndSeparator();

                break;
            default:
                Console.WriteLine("Invalid command. Press any key to try again");
                Console.ReadLine();
                ViewReport();
                return;
        }

    }

    private void GetAllRecords()
    {
        Console.Clear();

        Console.WriteLine("All Records\n\n");

        Validation.PrintStartSeparator();

        var drinkingWaters = _drinkingWaterRepository.GetAll();

        if (drinkingWaters.Count == 0) Console.WriteLine("No rows found!");
        else
        {
            foreach (var drinkingWater in drinkingWaters)
            {
                Console.WriteLine(drinkingWater);
            }
        }

        Validation.PrintEndSeparator();
    }

    private void Update()
    {
        Console.Clear();
        GetAllRecords();

        Console.WriteLine("\n\nPlease type the Id of the record you would like to update. Type 0 to return to main menu");

        var recordId = Validation.GetString();

        if (recordId == "0") ShowMainMenu();

        bool exists = _drinkingWaterRepository.Exists(recordId);

        if (!exists)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. Press any key to retry. \n\n");
            Console.ReadLine();
            Update();
            return;
        }

        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

        _drinkingWaterRepository.Update(date, quantity, recordId);

        Console.WriteLine($"\n\nRecord with Id {recordId} was updated. \n\n");
    }

    private void Delete()
    {
        Console.Clear();
        GetAllRecords();

        Console.WriteLine("\n\nPlease type the Id of the record you want to delete. Type 0 to return to main menu");

        var recordId = Validation.GetString();

        if (recordId == "0") ShowMainMenu();

        int rowCount = _drinkingWaterRepository.Delete(recordId);

        if (rowCount == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. Press any key to try again. \n\n");
            Console.ReadLine();
            Delete();
            return;
        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
    }

    private void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

        _drinkingWaterRepository.Insert(date, quantity);
    }

    private string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yyyy). Type 0 to return to main menu");

        string dateInput = Validation.GetDateAsString();

        if (dateInput == "0") ShowMainMenu();

        return dateInput;
    }

    private int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        int numberInput = Validation.GetNumber();

        if (numberInput == 0) ShowMainMenu();

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
}
