using HabitTracker.StressedBread;

DatabaseInput databaseInput = new();

ConsoleKeyInfo keyPressed;
bool endApp = false;

databaseInput.CreateTable();

while (!endApp)
{
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

    keyPressed = Console.ReadKey();

    switch (keyPressed.Key)
    {
        case ConsoleKey.D0:
        case ConsoleKey.NumPad0:
            endApp = true;
            break;

        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            databaseInput.View();
            break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
            databaseInput.InsertHabit();
            break;
        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
            databaseInput.InsertHabitData();
            break;

        case ConsoleKey.D4:
        case ConsoleKey.NumPad4:
            databaseInput.Update();
            break;

        case ConsoleKey.D5:
        case ConsoleKey.NumPad5:
            databaseInput.Delete();
            break;
        default:
            Console.WriteLine("\nInvalid Input! Press any key to reset.");
            Console.ReadKey();
            break;
    }
}