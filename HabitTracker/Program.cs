using HabitTracker;

DatabaseManager db = new();
CliHandler app = new(db);

app.RunApplication();
