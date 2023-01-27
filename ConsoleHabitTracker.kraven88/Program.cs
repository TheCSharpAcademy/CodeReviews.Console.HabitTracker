using ConsoleHabitTracker.kraven88;
using ConsoleHabitTracker.kraven88.DataAccess;
using System.Data.SQLite;

var DBname = "habits.db";
if (File.Exists(DBname) == false)
    SQLiteConnection.CreateFile(DBname);

var connectionString = $"Data Source={DBname}; Version=3";
var menu = new Menu(new SqliteDB(connectionString));

menu.MainMenu();