using Nelson.Habit_Tracker.DataAccess;
using Nelson.Habit_Tracker.HabitApp;
using Nelson.Habit_Tracker.UserConsoleInteraction;
using Nelson.Habit_Tracker.Utils;

IConsoleInteraction consoleInteraction = new ConsoleInteraction();

IInputValidator inputValidator= new InputValidator(consoleInteraction);
IDateValidator dateValidator = new DateValidator(consoleInteraction);

IDatabaseInitializer databaseInitializer = new DatabaseInitializer(consoleInteraction);

IHabitRepository habitRepository = new HabitRepository(consoleInteraction, inputValidator, dateValidator, databaseInitializer);
var habitApp = new HabitApp(consoleInteraction, habitRepository, databaseInitializer);

habitApp.RunApp();