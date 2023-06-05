using ConsoleHabitTracker;

if (!Database.DatabaseExists())
{
    Database.prepareDatabase();
    Database.setDefaults();
}
Menu.showGreeting();
Menu.showMainMenu();