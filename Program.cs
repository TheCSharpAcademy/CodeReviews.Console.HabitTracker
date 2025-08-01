using HabitLoggerApp.App;
using HabitLoggerApp.Data;
using HabitLoggerApp.Services;

string dbPath = "HabitLoggerApp.db";
var dbManager = new DatabaseManager(dbPath);
dbManager.InitializeDatabase();

var habitService = new HabitService(dbManager);
var habitEntryService = new HabitEntryService(dbManager);
var reportService = new ReportService(dbManager);
var app = new HabitTrackerApp(habitService, habitEntryService, reportService);

app.Run();

