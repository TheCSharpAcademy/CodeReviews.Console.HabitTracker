using Classes;

// Establish DB Connection
DBService dBService = new DBService("Data source=local.db");
string menuSelection;

// TODO: Update an entry - HAVE THIS DISPLAY ALL OCCURENCES FIRST
Console.WriteLine
(@"
 Press one of the following
 1. Add a habit
 2. Update a habit
 3. Remove a habit
 4. Show habits
 ");

menuSelection = Console.ReadLine() ?? "";

switch (menuSelection)
{
    case "1":
        // Add a habit 
        break;
    case "2":
        // Show all habits
        // Update a habit 
        break;
    case "3":
        // Remove a habit 
        break;
    case "4":
        // Show a habits
        break;
    default:
        Console.WriteLine("Invalid Entry");
        break;
}

