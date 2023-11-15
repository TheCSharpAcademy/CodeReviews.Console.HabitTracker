
using HabitLogger;

DbManager dbManager = new DbManager();
LogService logService = new LogService(dbManager);

logService.SetupDatabase("Time");

UI ui = new UI(logService);
ui.Start();