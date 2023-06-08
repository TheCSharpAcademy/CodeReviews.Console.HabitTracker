using ConsoleHabitTracker;

if (!Database.DatabaseExists())
{
    Database.PrepareDatabase();
    Database.SetDefaults();
}
Menu.ShowGreeting();
Menu.ShowMainMenu();