using System.Globalization;
using HabitLogger.TildaDares;

var db = new HabitLoggerDatabase();

UserInput();
return;

void UserInput()
{
    var exit = false;
    while (!exit)
    {
        Console.WriteLine("\nWelcome to Habit Logger: ");
        Console.WriteLine("Select an option:");
        Console.WriteLine("1. Insert a habit");
        Console.WriteLine("2. Get a habit");
        Console.WriteLine("3. Get all habits");
        Console.WriteLine("4. Update a habit");
        Console.WriteLine("5. Delete a habit");
        Console.WriteLine("6. View report on habit by type and date");
        Console.WriteLine("7. View habits by date");
        Console.WriteLine("0. Exit");
        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                InsertHabit();
                break;
            case "2":
                GetHabit();
                break;
            case "3":
                GetHabits();
                break;
            case "4":
                UpdateHabit();
                break;
            case "5":
                DeleteHabit();
                break;
            case "6":
                ViewHabitReportByTypeAndDate();
                break;
            case "7":
                ViewHabitsByDate();
                break;
            default:
                exit = true;
                break;
        }
    }
}

void InsertHabit()
{
    Console.Clear();
    var date = GetDateInput("Enter the date you want to log in the format dd/mm/yyyy: ");
    Console.Clear();
    
    var habitType = GetStringInput("Please insert the habit type e.g Running, Sleeping e.t.c");
    habitType = habitType.ToLower();
    Console.Clear();
    
    var quantity = GetNumberInput("Please insert habit measure of your choice in integer (no decimals allowed): ");
    Console.Clear();
    
    var unit = GetStringInput("Please insert the unit of your habit e.g minutes, kilometres e.t.c");
    unit = unit.ToLower();
    
    db.InsertHabit(date, quantity, unit, habitType);
    ContinueMenu();
}

void GetHabit()
{
    if (!HasHabitsRecord())
    {
        ContinueMenu();
        return;
    }
    
    GetHabits();
    var id = GetNumberInput("Enter the habit ID you wish to retrieve: ");
    var habit = db.GetHabit(id);

    if (habit == null)
    {
        Console.WriteLine("No habit found with that ID!");
        ContinueMenu();
        return;
    }
    
    Console.Clear();
    Console.WriteLine($"Habit Record");
    BuildTableHeader();
    BuildTableRows(habit);
    ContinueMenu();
}

void GetHabits()
{
    Console.Clear();
    var habits = db.GetAllHabits();
    if (habits.Count == 0)
    {
        Console.WriteLine("No habits found!");
        ContinueMenu();
        return;
    }
    
    Console.WriteLine("All habit records:");
    BuildTableHeader();
    
    foreach (var habit in habits)
    {
        BuildTableRows(habit);
    }

    ContinueMenu();
}

void ViewHabitReportByTypeAndDate()
{
    Console.Clear();
    var habitType = GetStringInput("Enter the habit type to view report for: ");
    habitType = habitType.ToLower();
    Console.Clear();

    const string startDateMessage = "Enter the start date in the format dd/mm/yyyy (Leave blank if you don't want to enter the date)";
    DateOnly? startDate = GetDateInput(startDateMessage, paramIsOptional: true);
    startDate = startDate == default(DateOnly) ? null : startDate;
    Console.Clear();

    const string endDateMessage = "Enter the end date in the format dd/mm/yyyy (Leave blank if you don't want to enter the date)";
    DateOnly? endDate = GetDateInput(endDateMessage, minRange: startDate, paramIsOptional: true);
    endDate = endDate == default(DateOnly) ? null : endDate;
    Console.Clear();

    var habitQuantity = db.GetHabitsByTypeAndDate(habitType, startDate, endDate);
    if (habitQuantity == 0)
    {
        Console.WriteLine("No habits found for the specified type and date range!");
        ContinueMenu();
        return;
    }
    
    var dateMessage = (startDate != null && endDate != null) ? $"Between {startDate} and {endDate}," : string.Empty;
    Console.WriteLine($"{dateMessage} This is how much you performed the {habitType} activity: {habitQuantity}.");
    ContinueMenu();
}

void ViewHabitsByDate()
{
    Console.Clear();
    var startDate = GetDateInput("Enter the start date in the format dd/mm/yyyy: ");
    Console.Clear();

    var endDate = GetDateInput("Enter the end date in the format dd/mm/yyyy: ", minRange: startDate);
    Console.Clear();

    var habits = db.GetHabitsByDate(startDate, endDate);
    if (habits.Count == 0)
    {
        Console.WriteLine("No habits found for the specified date range!");
        ContinueMenu();
        return;
    }

    Console.WriteLine($"Habit Report between {startDate} and {endDate}:");
    BuildTableHeader();
    
    foreach (var habit in habits)
    {
        BuildTableRows(habit);
    }
    ContinueMenu();
}

void UpdateHabit()
{
    if (!HasHabitsRecord())
    {
        ContinueMenu();
        return;
    }
    
    GetHabits();
    var id = GetNumberInput("Enter the habit ID you wish to update: ");
    Console.Clear();
    var date = GetDateInput("Enter the updated date in the format dd/mm/yyyy: ");
    Console.Clear();
    
    var habitType = GetStringInput("Enter the updated habit type e.g Running, Sleeping e.t.c");
    Console.Clear();
    
    var quantity = GetNumberInput("Enter the updated habit measure of your choice in integer (no decimals allowed): ");
    Console.Clear();
    var unit = GetStringInput("Enter the updated unit of your habit e.g litres, glasses e.t.c");
    db.UpdateHabit(id, date, quantity, unit, habitType);
    ContinueMenu();
}

void DeleteHabit()
{
    if (!HasHabitsRecord())
    {
        ContinueMenu();
        return;
    }
    
    GetHabits();
    var id = GetNumberInput("Enter the habit ID you wish to delete: ");
    db.DeleteHabit(id);
    ContinueMenu();
}

bool HasHabitsRecord()
{
    var count = db.CountHabits();
    if (count >= 1) return true;
    
    Console.WriteLine("No habits found!");
    return false;

}

DateOnly GetDateInput(string message, DateOnly? minRange = null, DateOnly? maxRange = null, bool paramIsOptional = false)
{
    Console.WriteLine("--------------------------");

    minRange ??= DateOnly.MinValue;
    maxRange ??= DateOnly.MaxValue;

    DateOnly dateOnly;
    string input;
    do
    {
        Console.WriteLine(message);
        input = Console.ReadLine().Trim();
        
        // If input is empty and optional, return DateOnly default
        if (input == "" && paramIsOptional)
        {
            return default;
        }
    } while (!DateOnly.TryParseExact(input, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None,
                 out dateOnly) || dateOnly > maxRange || dateOnly < minRange);

    return dateOnly;
}

int GetNumberInput(string message)
{
    Console.WriteLine("--------------------------");

    int quantity;
    var input = "";
    do
    {
        Console.WriteLine(message);
        input = Console.ReadLine().Trim();
    } while (!int.TryParse(input, out quantity));

    return quantity;
}

string GetStringInput(string message)
{
    Console.WriteLine("--------------------------");
    Console.WriteLine(message);

    var input = "";
    do
    {
        input = Console.ReadLine().Trim();
    } while(string.IsNullOrEmpty(input));

    return input;
}

void BuildTableHeader()
{
    Console.WriteLine("-----------------------------------------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("|" + "ID".PadLeft(15) + "|" + "Date".PadLeft(15) + "|" + "Type".PadLeft(15) + "|" + "Quantity".PadLeft(15) + "|" +
                      "Unit".PadLeft(15) + "|");
    Console.ResetColor();
    Console.WriteLine("-----------------------------------------------------------------------------------");
}

void BuildTableRows(Habit habit)
{
    Console.WriteLine($"|{habit.Id,15}|{habit.Date,15}|{habit.Type,15}|{habit.Quantity,15}|{habit.Unit,15}|");
    Console.WriteLine("-----------------------------------------------------------------------------------");
}

void ContinueMenu()
{
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadLine();
}