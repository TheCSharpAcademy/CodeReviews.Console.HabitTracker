using HabitLogger;
using HabitLoggerLibrary;

SetupDatabase setupDatabase = new SetupDatabase();
setupDatabase.InitializeDatabase();

#if DEBUG
setupDatabase.SeedData();
#endif

UserInterface userInterface = new UserInterface();

userInterface.DisplayHabitMenu();
userInterface.DisplayMainMenu();