using HabitTracker;
using HabitTracker.Database;

DatatBaseOperations.CreateDatabase();

UserInterface userInterface = new UserInterface();
userInterface.ShowMenu();
