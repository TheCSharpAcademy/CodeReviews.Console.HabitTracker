using HabitLogger;
using HabitLoggerLibrary;

SetupDatabase setupDatabase = new SetupDatabase();
setupDatabase.InitializeDatabase();

#if DEBUG
setupDatabase.SeedData();
#endif

bool continueRunning = true;

UserInterface userInterface = new UserInterface();

userInterface.DisplayHabitMenu();
userInterface.DisplayMainMenu();