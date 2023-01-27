using ConsoleHabitTracker.kraven88.DataAccess;

var connectionString = "Data Source=habits.db; Version=3";
var db = new SqliteDB(connectionString);

db.CreateTable("DrinkingWater");