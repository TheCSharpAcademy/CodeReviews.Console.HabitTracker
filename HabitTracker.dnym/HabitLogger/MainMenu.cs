using HabitLogger.Database;

namespace HabitLogger;

internal class MainMenu
{
    private readonly IDatabase _database;
    private readonly InsertRecordScreen _insertRecordScreen;
    private readonly ManageRecordsMenu _manageRecordsMenu;

    public MainMenu(IDatabase database)
    {
        _database = database;
        _insertRecordScreen = new InsertRecordScreen(_database);
        _manageRecordsMenu = new ManageRecordsMenu(_database);
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(@"Habit Logger
============

1. Insert Record
2. Manage Records
0. Quit

------------
Press a number to select.");
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    _insertRecordScreen.Show();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    _manageRecordsMenu.Show();
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
