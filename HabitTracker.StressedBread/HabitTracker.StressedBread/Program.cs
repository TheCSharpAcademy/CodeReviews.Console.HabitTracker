using HabitTracker.StressedBread;

// Initialize the database input handler
DatabaseInput databaseInput = new();
AutomaticSeeding automaticSeeding = new();

ConsoleKeyInfo keyPressed;
bool endApp = false;

// Create the necessary table in the database
databaseInput.CreateTable();

// Automatically seed the database if the table is empty
automaticSeeding.IsTableEmpty();

while (!endApp)
{
    // Clear the console and display the menu
    Console.Clear();
    Console.WriteLine("Habit Tracker");
    Console.WriteLine("-------------");
    Console.WriteLine(@"Press 0 to close the application.
Press 1 to view all records.
Press 2 to create habit.
Press 3 to add data to habit.
Press 4 to update record.
Press 5 to delete record.");
    Console.WriteLine("-------------");

    // Read the key pressed by the user
    keyPressed = Console.ReadKey();

    // Handle the key press based on the user's input
    switch (keyPressed.Key)
    {
        case ConsoleKey.D0:
        case ConsoleKey.NumPad0:
            // Exit the application
            endApp = true;
            break;

        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            // View all records
            databaseInput.ViewControl();
            break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
            // Create a new habit
            databaseInput.InsertHabit();
            break;

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
            // Add data to an existing habit
            databaseInput.InsertHabitData();
            break;

        case ConsoleKey.D4:
        case ConsoleKey.NumPad4:
            // Update an existing record
            databaseInput.UpdateControl();
            break;

        case ConsoleKey.D5:
        case ConsoleKey.NumPad5:
            // Delete a record
            databaseInput.Delete();
            break;

        default:
            // Handle invalid input
            Console.WriteLine("\nInvalid Input! Press any key to reset.");
            Console.ReadKey();
            break;
    }
}