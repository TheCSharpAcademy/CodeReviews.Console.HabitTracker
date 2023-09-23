using MiroiuDev.HabitTracker;

string connectionString = @"Data Source=habit-tracker.db";

var db = new Database(connectionString);

string initDbSql = @"CREATE TABLE IF NOT EXISTS drinking_water (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT,
    Quantity INTEGER
)";

db.Execute(initDbSql);

var drinkingWaterRepository = new DrinkingWaterRepository(db);

var menu = new Menu("MAIN MENU", drinkingWaterRepository);

menu.ShowMainMenu();
