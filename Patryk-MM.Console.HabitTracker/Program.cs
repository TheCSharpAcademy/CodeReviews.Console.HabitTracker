using Patryk_MM.Console.HabitTracker;
using Spectre.Console;
Database db = new Database();
db.Connect();
AnsiConsole.Write(
    new FigletText("Habit Tracker")
        .Centered()
        .Color(Color.Green));



while (true) {

    var selection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title($"\t\nChoose an option:")
        .AddChoices([
            "View your habit entries", "Create a new habit", "Add a habit entry", "Update a habit entry", "Delete a habit entry",
            "Generate a report", "Exit the app"
        ]));

    switch (selection) {
        case "View your habit entries":
            Console.Clear();
            db.ViewHabits(db.GetHabits(db.ChooseTable()));
            break;
        case "Create a new habit":
            Console.Clear();
            db.CreateHabit();
            break;
        case "Add a habit entry":
            db.AddHabit();
            break;
        case "Update a habit entry":
            Console.Clear();
            db.UpdateHabit();
            break;
        case "Delete a habit entry":
            Console.Clear();
            db.DeleteHabit();
            break;
        case "Generate a report":
            Console.Clear();
            db.GenerateReport();
            break;
        case "Exit the app":
            Console.Clear();
            return;
    }

    


}



