using iamryanmacdonald.Console.HabitTracker;

var database = new Database("habit-tracker.db");
var userInterface = new UserInterface(database);
userInterface.MainMenu();