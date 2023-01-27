using ConsoleHabitTracker.kraven88;
using ConsoleHabitTracker.kraven88.DataAccess;
using System.Data.SQLite;

var DBname = "HabitTrackerDB";
var menu = new Menu(new SqliteDB(DBname));

menu.MainMenu();