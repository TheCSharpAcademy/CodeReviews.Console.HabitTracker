using HabitTracker;
DataAccess da = new DataAccess();
da.CreateDatabase();

bool isRunning = true;

while (isRunning)
{
    
    Console.WriteLine("MAIN MENU");
    Console.WriteLine("Type 0 to close the Application");
    Console.WriteLine("Type 1 to view All Records");
    Console.WriteLine("Type 2 to Insert A Record");
    Console.WriteLine("Type 3 to Delete A Record");
    Console.WriteLine("Type 4 to Update A Record");

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "0":
            isRunning = false;
            break;
        case "1":
            da.ViewRecords();
            break;
        case "2":
            da.InsertRecord();
            break;
        case "3":
            da.DeleteRecord();
            break;
        case "4":
            da.UpdateRecord();
            break;
        default:
            Console.WriteLine("Invalid Input");
            break;
    }
}

