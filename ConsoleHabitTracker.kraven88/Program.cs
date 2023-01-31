using ConsoleHabitTracker.kraven88;
using ConsoleHabitTracker.kraven88.DataAccess;

var DBname = "HabitTrackerDB.db";
var menu = new Menu(new SqliteDB(DBname));

menu.MainMenu();