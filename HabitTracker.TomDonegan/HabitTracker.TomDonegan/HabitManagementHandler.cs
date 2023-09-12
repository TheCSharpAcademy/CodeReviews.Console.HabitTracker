using System.Collections;

namespace HabitTracker.TomDonegan
{
    internal static class HabitManagementHandler
    {
        internal static void AddRemoveHabit(string habitName, string action)
        {
            Console.Clear();

            Helpers.DisplayHeader($"{action} a Habit");

            Console.WriteLine($"Would you like to {action} a habit? (y/n)");
            string userInput = Console.ReadLine();

            switch (userInput.ToLower())
            {
                case "y":
                    if (action == "create")
                    {
                        GetHabitData(action);
                    }
                    else
                    {
                        GetHabitData(action);
                    }
                    break;
                case "n":
                    break;
                default:
                    Console.WriteLine($"'{userInput}' is not a valid selection, please try again.");
                    break;
            }
        }

        internal static void GetHabitData(string action)
        {
            bool habitConfirmed = false;

            ArrayList habitList = HabitDataHandler.ListDatabaseTables();

            Console.WriteLine("Please enter a habit name:");
            string habitName = Console.ReadLine();

            Console.WriteLine($"Please confirm habit name (y/n): {habitName}");
            string confirmation = Console.ReadLine();

            switch (confirmation.ToLower())
            {
                case "y":
                    if (action == "create")
                    {
                        Console.WriteLine(
                            "Please enter a unit of measuement you would like to use for this habit?"
                        );
                        string uom = Console.ReadLine();
                        DatabaseAccess.DatabaseCreation(habitName, uom);
                    }
                    else if (action == "delete")
                    {
                        while (true)
                        {
                            if (!habitList.Contains(habitName))
                            {
                                Console.WriteLine($"{habitName} does not exist. Please enter a valid habit name:");
                                habitName = Console.ReadLine();
                                continue;
                            }

                            if (habitName == "drinking_water")
                            {
                                Console.WriteLine($"You can't delete {habitName}. It's important to track your water intake!\nPlease enter another habit name:");
                                habitName = Console.ReadLine();
                                continue;
                            }

                            Console.WriteLine($"Are you sure you want to delete {habitName}? (y/n)");
                            string deleteConfirmation = Console.ReadLine();

                            switch (deleteConfirmation.ToLower())
                            {
                                case "y":
                                    DatabaseAccess.DeleteHabit(habitName);
                                    Console.WriteLine($"{habitName} has been deleted from the database.");
                                    Console.ReadLine();
                                    break;
                                case "n":
                                    
                                    break;
                                default:
                                    Console.WriteLine("Invalid input. Please enter 'y' for yes or 'n' for no.");
                                    break;
                            }
                            break;
                        }
                    }
                    habitConfirmed = true;
                    break;
                case "n":
                    break;
            }
        }
    }
}