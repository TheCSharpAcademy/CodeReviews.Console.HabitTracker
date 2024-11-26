using HabitLoggerApp;

var userInterface = new UserInterface();
var habitManager = new HabitManager();

var appIsRunning = true;
while (appIsRunning)
{
    userInterface.DisplayMenu(true);

    while (appIsRunning)
    {
        var input = userInterface.GetUserInput("Enter your choice: ");
        switch (input)
        {
            case "1":
                var habits = habitManager.GetAllHabits();
                userInterface.DisplayHabits(habits);
                break;
            case "2":
                var body = userInterface.GetUserInput("Enter habit body: ");
                var parsedDate = userInterface.GetUserDateInput("Enter habit date: ");
                var quantity = userInterface.GetUserNumberInput("Enter quantity: ");

                habitManager.CreateHabit(parsedDate, body, quantity);
                break;
            case "3":
                var updatingId = userInterface.GetUserNumberInput("Enter the id to be updated: ");
                var updatedBody = userInterface.GetUserInput("Enter habit body: ");
                var updatedDate = userInterface.GetUserDateInput("Enter habit date: ");
                var updatedQuantity = userInterface.GetUserNumberInput("Enter quantity: ");
                habitManager.UpdateHabit(updatingId, updatedBody, updatedDate, updatedQuantity);
                Console.WriteLine("Habit updated successfully!");
                break;
            case "4":
                var deletionId = userInterface.GetUserNumberInput("Enter habit id for deletion: ");
                habitManager.DeleteHabit(deletionId);
                Console.WriteLine("Habit deleted successfully!");

                break;
            case "5":
                appIsRunning = false;
                break;
        }

        userInterface.DisplayMenu();
    }
}

Console.WriteLine("Thank you for using this app!");