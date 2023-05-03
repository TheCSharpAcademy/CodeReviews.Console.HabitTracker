using ConsoleTables;
using HabitTracker.barakisbrown;

Helpers help = new();
DataLayer data = new();

int option = -1;
Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
{
    e.Cancel = true;
};

while(option != 0)
{
    Console.WriteLine("Welcome to Habit Tracker. This will be tracking my blood amount which can be multiple times a day.");
    Console.WriteLine("Each entry is a blood reading from my blood meter.");
    help.GetMenu();
    option = help.GetMenuSelection();
    switch (option)
    {
        case 0:
            break;
        case 1:
            ShowAllRecords();
            break;
        case 2:
            InsertRow();
            break;
        case 3:
            DeleteRow();
            break;
        case 4:
            UpdateRow();
            break;
        case 5:
            ShowReport();
            break;
        default:
            Console.WriteLine();
            Console.WriteLine("Input Entered is not valid. Please try again.");
            break;
    }
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Thank you for using the Habit Tracker for tracking your blood suger readings.");
}

void ShowReport()
{
    if (!data.IsTableEmpty())
    {
        Console.Clear();
        int avg = data.AVG();
        int max = data.MAX();
        int min = data.MIN();
        int ovr = data.Beyond200();

        Console.WriteLine("Report for the Habbit Tracker[Blood Sugar Readings]");
        
        Habit.DisplayAllRecords(data.SelectAll());

        var table = new ConsoleTable("MIN", "AVG", "MAX", "# > 200");
        table.Configure(o => o.EnableCount = false);
        table.AddRow(min, avg, max, ovr);
        table.Write();
    }
    else
    {
        Console.Clear();
        Console.WriteLine("Report for the Habbit Tracker[Blood Sugar Readings]\n");
        Console.WriteLine("There are no readings to generate a report.");
        Console.WriteLine("Come back once you have inserted some readings.");

    }
    Console.WriteLine("Press any key to return back to the main menu");
    Console.ReadKey();
    Console.Clear();
}

void InsertRow()
{
    Console.Clear();
    Console.WriteLine("INSERT a blood reading by value and date.\n");
    Habit habit = new()
    {
        Amount = help.GetAmount(),
        Date = help.GetDate()
    };
    if (data.Insert(habit))
    {
        Console.WriteLine();
        Console.WriteLine("Insert Succeed. Press any key to continue");
    }
    Console.ReadKey();
    Console.Clear();
}

void DeleteRow()
{
    Console.Clear();
    Console.WriteLine("DELETE A BLOOD SUGAR READING.\n");
    if (data.IsTableEmpty())
    {
        Console.WriteLine();
        Console.WriteLine("No records are found so nothing to be deleted. Returning back to the main menu");
        Console.Write("Press any key");
        Console.ReadKey();
    }
    else
    {
        Habit.DisplayAllRecords(data.SelectAll());
        Console.WriteLine();
        int id = help.GetNumberFromList(data.GetValidId());
        if (id != -1)
        {
            Console.WriteLine();
            Console.WriteLine($"Do you wish {id} to be deleted? (Y/N)");
            if (Helpers.GetYESNO())
            {
                if (id != 0)
                {
                    Console.WriteLine($"Deleting row {id}");
                    if (data.DeleteRow(id))
                        Console.WriteLine($"Row {id} was Successfully Deleted.");
                    else
                        Console.WriteLine($"Row {id} was not deleted.");
                }
            }
        }
    }
    Thread.Sleep(1000);
    Console.Clear();
}

void UpdateRow()
{
    Console.Clear();
    Console.WriteLine("UPDATING BLOOD SUGAR READING EITHER BY AMOUNT OR DATE.\n");
    if (data.IsTableEmpty())
    {
        Console.WriteLine();
        Console.WriteLine("No records found so no updates can be done. Returning back to the main menu");
        Console.Write("Press any key");
        Console.ReadKey(true);
        Console.Clear();
        return;
    }
    Habit.DisplayAllRecords(data.SelectAll());
    Console.WriteLine();   
    while (true)
    {
        int rowid = help.GetNumberFromList(data.GetValidId());
        if (rowid != -1)
        {
                Console.WriteLine();
                Console.Write("Update Amount or Date (A/D)? ");
                ConsoleKeyInfo input = Console.ReadKey();

                if (input.Key == ConsoleKey.A)
                {
                    Console.WriteLine();
                    int newAmount = help.GetAmount();
                    if (data.UpdateAmount(rowid, newAmount))
                        Console.WriteLine($"Row {rowid} Amount was successfully updated.");
                    break;
                }
                else
                {
                    Console.WriteLine();
                    DateTime date = help.GetDate();
                    if (data.UpdateDate(rowid, date))
                        Console.WriteLine($"Row {rowid} Date was successfully updated.");
                break;
                }
        }
        break;
    }
    Thread.Sleep(1000);
    Console.Clear();
}

void ShowAllRecords()
{
    if (!data.IsTableEmpty())
    {
        Console.Clear();
        Console.WriteLine("Displaying All Records.");
        Console.WriteLine();
        Habit.DisplayAllRecords(data.SelectAll());
        Console.WriteLine("Press any key to return to the menu");
        Console.ReadKey();
        Console.Clear();
    }
    else
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("There are no records found. Please insert some. Press any key to return to menu");
        Console.ReadKey();
        Console.Clear();
    }
}