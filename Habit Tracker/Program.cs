
using System.Text.RegularExpressions;
using CrudLibrary;
using ReportLibrary;
using UserInput;



bool endApp = false;
SqliteCrudController sqliteController = CreateCrudController();
Reports reports = new Reports(sqliteController);

sqliteController.CreateTable();

while (!endApp)
{
    string? menuChoice = GetMainMenuChoice();

    if (menuChoice == null || !Regex.IsMatch(menuChoice, "[0|1|2|3|4|5|6]"))
    {
        Console.WriteLine("Error unrecognised input.");
    }

    endApp = menuChoice == "0";

    switch (menuChoice)
    {
        case "1":
            sqliteController.ViewAll();
            break;
        case "2":
            string? date;
            do
            {
                date = GetDateInput();
            } while (string.IsNullOrWhiteSpace(date));
            sqliteController.InsertRecord(date, GetQuantityInput());
            break;
        case "3":
            int idtoDelete = GetIdInput();
            sqliteController.DeleteRecord(idtoDelete);
            break;
        case "4":
            string? updateChoice;
            do
            {
                updateChoice = GetUpdateInput();
            } while (string.IsNullOrWhiteSpace(updateChoice));
            PerformUpdateRecord(updateChoice, GetIdInput());
            break;
        case "5":
            reports.View();
            break;
        case "6":
            sqliteController.AddSeedData();
            break;
    }
}



// FUNCTIONS
string GetNonEmputStringInput(string prompt)
{
    string? input;
    do
    {
        Console.WriteLine(prompt);
        input = Console.ReadLine();
    } while (string.IsNullOrWhiteSpace(input));
    return input;
}

SqliteCrudController CreateCrudController()
{
    string habit = GetNonEmputStringInput("Enter a habit you would like to track: ");
    string unitOfMeasure = GetNonEmputStringInput("Enter the unit of measure (quantity only, time units are not permitted): ");
    return new SqliteCrudController(habit, unitOfMeasure);
}

string? GetDateInput()
{
    Console.WriteLine("What you like to use today's date? (y/n) ");
    string? useDateToday = Console.ReadLine();
    while (useDateToday == null || !Regex.IsMatch(useDateToday, "[y|n]"))
    {
        Console.WriteLine("Error unrecognized input. Please enter 'y' or 'n'. ");
        useDateToday = Console.ReadLine();
    }
    if (useDateToday == "y")
    {
        return GetDateToday();
    }
    else
    {
        Console.WriteLine("Enter date with format (dd-mm-yy). Type 0 to return to main menu");
        string? dateInput = Console.ReadLine();
        while (!UserInputLibrary.IsDateValid(dateInput))
        {
            Console.WriteLine("Enter a valid date with format (dd-mm-yy) ");
            dateInput = Console.ReadLine();
        }
        return dateInput == "0" ? GetMainMenuChoice() : dateInput;
    }
}

string GetDateToday() => DateTime.Now.ToString("dd-MM-yy");

int GetQuantityInput()
{
    int cleanQuantity;
    Console.WriteLine($"Enter number of {sqliteController.UnitOfMeasure}: ");
    string? quantity = Console.ReadLine();

    while (!int.TryParse(quantity, out cleanQuantity))
    {
        Console.WriteLine("Enter a number: ");
        quantity = Console.ReadLine();
    }
    return cleanQuantity;
}

int GetIdInput()
{
    int id;
    Console.WriteLine("Enter an ID: ");
    string? idInput = Console.ReadLine();
    while (!int.TryParse(idInput, out id) || id == 0)
    {
        Console.WriteLine("Error unrecognised input. Please enter an valid ID");
        idInput = Console.ReadLine();
    }
    return id;
}

string? GetUpdateInput()
{
    string? updateInput;
    Console.WriteLine("What would you like to update? (Date/Quantity/Both) ");
    Console.WriteLine("Type d - Date");
    Console.WriteLine("Type q - Quantity");
    Console.WriteLine("Type b - Both");
    updateInput = Console.ReadLine();

    while (updateInput == null || !Regex.IsMatch(updateInput, "[d|q|b]"))
    {
        Console.WriteLine("Error unrecognised input.");
        updateInput = Console.ReadLine();
    }
    return updateInput;
}

void PerformUpdateRecord(string updateChoice, int id)
{
    string? updateDate = "default";
    int updateQuantity = 0;

    if (updateChoice == "b" || updateChoice == "d")
    {
        updateDate = GetDateInput();
        while (string.IsNullOrWhiteSpace(updateDate))
        {
            updateDate = GetDateInput();
        }
    }

    if (updateChoice == "b")
    {
        updateQuantity = GetQuantityInput();
        sqliteController.UpdateAllRecord(id, updateDate, updateQuantity);
    }
    else if (updateChoice == "d")
    {
        sqliteController.UpdateRecordDate(id, updateDate);
    }
    else if (updateChoice == "q")
    {
        updateQuantity = GetQuantityInput();
        sqliteController.UpdateRecordQuantity(id, updateQuantity);
    }
}

string? GetMainMenuChoice()
{
    Console.WriteLine("MAIN MENU");
    Console.WriteLine();
    Console.WriteLine("What would you like to do? ");
    Console.WriteLine();
    Console.WriteLine("Type 0 to Close Application.");
    Console.WriteLine("Type 1 to View All Records.");
    Console.WriteLine("Type 2 to Insert Record.");
    Console.WriteLine("Type 3 to Delete Record.");
    Console.WriteLine("Type 4 to Update record.");
    Console.WriteLine("Type 5 to View Report.");
    Console.WriteLine("Type 6 to Add Seed Data");
    Console.WriteLine("------------------------------------------------------");
    return Console.ReadLine();
}