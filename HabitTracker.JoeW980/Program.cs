using ConsoleTables;
using HabitLogger;
using static HabitLogger.Database;
bool endAPp = false;
Database db = new Database();

while (!endAPp)
{    
    int menuSelection;
    int recordSelection;
    int userQuantity;
    bool validHabit = false;
    Habit userHabit;
    List<Entry> records;

    Console.WriteLine("""
        Habit Tracker
        -------------
        MAIN MENU

        0 - Close Application
        1 - View All Records
        2 - Insert Record
        3 - Delete Record
        4 - Update Record
        5 - Add New Habit
        6 - Reports
        -------------
        """);
    if (!IsHabits())
    {
        Console.WriteLine("There are no Habits logged, Please select Insert to add one.");
    }
    Console.Write("Select Option: ");
    menuSelection = ValidateMenu();
    Console.Clear();

    switch (menuSelection)
    {
        case 0:
            endAPp = true;
            Console.WriteLine("Goodbye");
            break;

        case 1:            
            Console.WriteLine("View Records \n");
            if (!IsHabits())
            {
                Console.WriteLine("No Habits to show, Return to Main Menu to add some.");
                break;
            }
            DisplayRecords();
            break;

        case 2:
            Console.WriteLine("Insert Record");
            Console.WriteLine("-------------");            

            if (!IsHabits())
            {
                Console.WriteLine("No Habits to show, Return to Main Menu to add some.");
                break;
            }

            validHabit = HabitMenu(out userHabit);
            if (!validHabit)
                break;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(userHabit.HabitName);
            Console.ResetColor();
            Console.WriteLine("------");            
            Console.Write($"Write Quantity in {userHabit.Unit}: ");
            userQuantity = ValidateMenu();
            if (userQuantity < 1)
            {
                Console.WriteLine("Invalid number - Returning to Main Menu");
                break;
            }
            db.Insert(userHabit, userQuantity);
            Console.WriteLine($"Added {userQuantity} {userHabit.Unit} to {userHabit.HabitName}");
            break;

        case 3:
            Console.WriteLine("Delete Record");
            Console.WriteLine("-------------");
            if (!IsHabits())
            {
                Console.WriteLine("No Habits to show, Return to Main Menu to add some.");
                break;
            }

            Console.WriteLine("""
                1 - Delete Habit & Records
                2 - Delete Record
                """);
            Console.Write("Select Option: ");
            menuSelection = ValidateMenu();
            Console.Clear();
            switch (menuSelection)
            {
                case 1:
                    validHabit = HabitMenu(out userHabit);
                    if (!validHabit)
                        break;

                    db.DeleteHabit(userHabit);
                    break;

                case 2:
                    validHabit = HabitMenu(out userHabit);
                    if (!validHabit)
                        break;

                    Console.Clear();
                    Console.WriteLine("Select Record to Delete");
                    Console.WriteLine("-----------------------");
                    records = db.GetHabitRecords(userHabit);
                    foreach (var record in records.Select((value, i) => new { i, value }))
                    {
                        Console.WriteLine($"{record.i + 1}: {record.value.Quantity} {userHabit.Unit} - {record.value.Date:ddd (dd/MM) HH:mm}");

                    }
                    Console.WriteLine("0 - Exit to Main Menu");
                    Console.Write("Select Option: ");
                    recordSelection = ValidateMenu();

                    if (recordSelection == 0) break;
                    while (recordSelection > records.Count)
                    {
                        Console.Write("Invalid Option, Try again: ");
                        recordSelection = ValidateMenu();
                    }
                    db.DeleteRecord(userHabit, records[recordSelection - 1]);
                    break;
            }
            break;

        case 4:
            Console.WriteLine("Update Record");
            Console.WriteLine("-------------");
            if (!IsHabits())
            {
                Console.WriteLine("No Habits to show, Return to Main Menu to add some.");
                break;
            }
            validHabit = HabitMenu(out userHabit);
            if (!validHabit)
                break;

            Console.Clear();
            records = db.GetHabitRecords(userHabit);
            foreach (var record in records.Select((value, i) => new { i, value }))
            {
                Console.WriteLine($"{record.i + 1}: {record.value.Quantity} {userHabit.Unit} - {record.value.Date:ddd (dd/MM) HH:mm}");                
            }
            Console.WriteLine("0 - Exit to Main Menu");
            Console.Write("Select Option: ");
            recordSelection = ValidateMenu();

            if (recordSelection == 0) break;
            while (recordSelection > records.Count)
            {
                Console.Write("Invalid Option, Try again: ");
                recordSelection = ValidateMenu();
            }
            recordSelection--;

            Console.Clear();
            Console.WriteLine($"Updating {records[recordSelection].Quantity} {userHabit.Unit} of {userHabit.HabitName}");
            Console.WriteLine("---------------------");
            Console.Write("Enter new Quantity: ");
            userQuantity = ValidateMenu();
            db.Update(userHabit, records[recordSelection], userQuantity);
            break;

        case 5:
            Console.WriteLine("Add Habit");
            Console.WriteLine("---------");
            Console.Write("Habit Name: ");
            string habitName = ValidateInput();

            if (IsHabits())
            {
                var existingHabits = new List<string>();
                foreach (var habit in db.GetHabits())
                {
                    existingHabits.Add(habit.HabitName);
                }

                if (existingHabits.Contains(habitName))
                {
                    Console.Write("Habit already exists, Please delete it first.");
                    break;
                }
            }
            Console.Write("Unit of measurement (e.g. 'Glasses' for Water): ");
            string unit = ValidateInput();

            db.AddNewHabit(habitName, unit);
            break;

        case 6:
            Console.WriteLine("Reporting");
            Console.WriteLine("---------");
            if (!IsHabits())
            {
                Console.WriteLine("No Habits to show, Return to Main Menu to add some.");
                break;
            }
            validHabit = HabitMenu(out userHabit);
            if (!validHabit)
                break;
            Console.Clear();
            Console.WriteLine("""
                1 - Yearly
                2 - Monthly
                3 - Weekly
                """);
            var timeFrame = ValidateMenu() switch
            {
                1 => "Yearly",
                2 => "Monthly",
                3 => "Weekly",
                _ => "Weekly"
            };

            Console.Clear();
            Report report = db.GetReport(userHabit, timeFrame);

            Console.WriteLine($"""
                {userHabit.HabitName} {timeFrame} Report
                -----------------
                Total {userHabit.Unit}: {report.TotalQuantity}
                Total Entries: {report.TotalRecords}
                """);

            break;
    }
    Console.WriteLine("Press any key to continue");
    Console.ReadKey(true);
    Console.Clear();
}

bool IsHabits() => db.GetHabits().Any();

bool HabitMenu(out Habit selectedHabit)
{
    var habits = db.GetHabits();
    foreach (var habit in habits.Select((value, i) => new { i, value }))
    {

      Console.WriteLine($"{habit.i + 1} - {habit.value.HabitName}");
    }
    Console.WriteLine("0 - Exit to Main Menu");
    Console.Write("Select Option: ");
    var menuSelection = ValidateMenu();
    while (menuSelection > habits.Count)
    {
        Console.Write("Invalid Option, Try again: ");
        menuSelection = ValidateMenu();
    }

    if (menuSelection == 0)
    {
        selectedHabit = new Habit(0, "", "", []);
        return false;
    }
    selectedHabit = habits[menuSelection - 1];
    return true; 
}

void DisplayRecords()
{
    var habits = db.ViewAllRecords();        
    foreach (var record in habits)
    {
        var table = new ConsoleTable("Quantity", "Unit");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(record.HabitName);
        Console.ResetColor();

        foreach (var row in record.Entries)
        {
            table.AddRow($"{row.Quantity} {record.Unit}", row.Date.Date.ToString("ddd (dd/MM/yy)"));
        }        
        table.Write(Format.MarkDown);
        Console.WriteLine();
    }
}

string ValidateInput()
{
    string? input = Console.ReadLine();
    while (String.IsNullOrEmpty(input)) 
    {
        Console.Write("Invalid entry, please try again: ");
        input = Console.ReadLine();
    }

    return input;
}

int ValidateMenu()
{    
    string? input = Console.ReadLine();
    int validNumber;
    while (!int.TryParse(input, out validNumber))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Value must be a number: ");
        Console.ResetColor();
        input = Console.ReadLine();
    }
    return validNumber;
}