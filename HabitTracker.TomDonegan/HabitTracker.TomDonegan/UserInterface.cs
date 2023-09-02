using System.Text.RegularExpressions;

namespace HabitTracker.TomDonegan
{
    internal class UserInterface
    {
        internal static void MainMenu()
        {
            bool runningTracker = false;

            while (!runningTracker)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Welcome to your Habit Tracker");
                Console.WriteLine("-----------------------------\n");
                Console.WriteLine("What would like to do today?\n");
                Console.WriteLine("1 - View all habit data.");
                Console.WriteLine("2 - Add new habit entry.");
                Console.WriteLine("3 - Update an existing entry.");
                Console.WriteLine("4 - Delete an entry.");
                Console.WriteLine("0 - Exit Habit Tracker");

                string menuSelection = Console.ReadLine();

                switch (menuSelection)
                {
                    case "1":
                        ViewAllHabitData();
                        break;
                    case "2":
                        InsertHabitData();
                        break;
                    case "3":
                        UpdateEntry();
                        break;
                    case "4":
                        DeleteEntry();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        internal static async void ViewAllHabitData()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("        All Habit Records        ");
            Console.WriteLine("---------------------------------\n");

            await Database.AsyncDatabaseConnection($"SELECT * FROM drinking_water");

            Console.WriteLine("\nPress 'Enter' to return to the main menu.");
            Console.ReadLine();
        }

        internal static async void InsertHabitData()
        {
            string date = GetDateInput();
            double quantity = GetQuantityInput();

            Console.WriteLine($"Adding: Date: {date} Quantity: {quantity}L to the database.");

            await Database.AsyncDatabaseConnection(
                $"INSERT INTO drinking_water (Date, Quantity) VALUES ('{date}', {quantity})"
            );

            Console.WriteLine("New habit data added successfully.");
            Console.WriteLine("Press 'Enter' to return to the main menu.");
            Console.ReadLine();
        }

        internal static async void DeleteEntry()
        {
            bool runDelete = false;

            while (!runDelete)
            {
                Console.Clear();

                await Database.AsyncDatabaseConnection($"SELECT * FROM drinking_water");

                Console.WriteLine("-----------------------------");
                Console.WriteLine("     Habit Record Deleter    ");
                Console.WriteLine("-----------------------------\n");
                Console.WriteLine("Please type the Id of the record you want to delete.");

                string deleteSelection = Console.ReadLine() ;

                while (!int.TryParse(deleteSelection, out _)) {
                    Console.WriteLine("Please enter a valid number.");
                    deleteSelection = Console.ReadLine();
                }

                Console.Clear();

                bool recordExists = await Database.AsyncDatabaseConnection(
                    $"SELECT * FROM drinking_water WHERE Id = '{deleteSelection}'"
                );

                if (recordExists)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("     Habit Record Deleter    ");
                    Console.WriteLine("-----------------------------\n");
                    Console.WriteLine("Are you sure you want the delete the above record? (y/n)");

                    string confirmDelete = Console.ReadLine();

                    switch (confirmDelete.ToLower())
                    {
                        case "y":
                            await Database.AsyncDatabaseConnection(
                                $"DELETE FROM drinking_water WHERE Id = '{deleteSelection}'");
                            Console.WriteLine("Record deleted.");
                            runDelete = true;
                            break;
                        case "n":
                            Console.Clear();
                            break;
                    }
                } else
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("     Habit Record Deleter    ");
                    Console.WriteLine("-----------------------------\n");
                    Console.WriteLine($"Record {deleteSelection} does not exist in the database. Please check your selection.");
                    Console.WriteLine($"Press 'Enter' to try again.");
                    Console.ReadLine();
                }
            }

            MainMenu();
        }

        internal static async void UpdateEntry()
        {
            bool runUpdate = false;

            while (!runUpdate)
            {
                Console.Clear();
                await Database.AsyncDatabaseConnection($"SELECT * FROM drinking_water");

                Console.WriteLine("-----------------------------");
                Console.WriteLine("     Habit Record Updater    ");
                Console.WriteLine("-----------------------------\n");
                Console.WriteLine("Please type the Id of the record you want to update.");

                string updateSelection = Console.ReadLine();
                Console.Clear();

                await Database.AsyncDatabaseConnection(
                    $"SELECT * FROM drinking_water WHERE Id = '{updateSelection}'"
                );

                Console.WriteLine("-----------------------------");
                Console.WriteLine("     Habit Record Updater    ");
                Console.WriteLine("-----------------------------\n");
                Console.WriteLine("Are you sure you want the update the above record? (y/n)");

                string confirmUpdate = Console.ReadLine();

                switch (confirmUpdate.ToLower())
                {
                    case "y":
                        string newDate = GetDateInput();
                        double newQuantity = GetQuantityInput();
                        await Database.AsyncDatabaseConnection(
                            $"UPDATE drinking_water SET Date = '{newDate}', Quantity = {newQuantity} WHERE Id = {updateSelection}"
                        );
                        runUpdate = true;
                        break;
                    case "n":
                        Console.Clear();
                        break;
                }
            }

            MainMenu();
        }

        private static double GetQuantityInput()
        {
            Console.WriteLine(
                "Please insert the quantity of water consumed. Type 0 to return to the main menu."
            );

            string quantityInput = Console.ReadLine();

            while (!double.TryParse(quantityInput, out _))
            {
                Console.WriteLine("Your quantity must be a number. Try again.");
                quantityInput = Console.ReadLine();
            }

            if (quantityInput == "0")
                MainMenu();

            return double.Parse(quantityInput);
        }

        private static string GetDateInput()
        {
            Console.WriteLine(
                "\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu."
            );

            string dateInput = Console.ReadLine();

            if (dateInput == "0")
                MainMenu();

            string requiredFormat = @"\d{2}-\d{2}-\d{2}";

            while (!Regex.IsMatch(dateInput, requiredFormat) || dateInput.Length < 6)
            {
                Console.WriteLine(
                    "Please enter the date in the required format and length. Try again."
                );
                dateInput = Console.ReadLine();
            };

            int dayNumber = Convert.ToInt32(dateInput[..2]);
            int monthNumber = Convert.ToInt32(dateInput.Substring(3, 2));

            while (dayNumber < 01 || dayNumber > 31 || monthNumber < 01 || monthNumber > 12)
            {
                Console.WriteLine(
                    "Day date must be between 01 and 31.\nMonth date must be between 01 and 12. Try again."
                );

                dateInput = Console.ReadLine();
                dayNumber = Convert.ToInt32(dateInput[..2]);
                monthNumber = Convert.ToInt32(dateInput.Substring(3, 2));
            };

            return dateInput;
        }
    }
}
