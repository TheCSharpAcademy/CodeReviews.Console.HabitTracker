using System.Collections;

namespace HabitTracker.TomDonegan
{
    internal static class HabitDataHandler
    {
        internal static void ViewAllHabitData(string habit)
        {
            Console.Clear();

            Helpers.DisplayHeader($"Current Habit: {habit}");
            Helpers.DisplayHeader($"{habit} Habit Records");

            try
            {
                DatabaseAccess.QueryAndDisplayResults($"SELECT * FROM {habit}");

                Console.WriteLine("\nPress 'Enter' to return to the main menu.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal static void InsertHabitData(string habit)
        {
            string date = InputValidation.GetDateInput();
            double quantity = InputValidation.GetQuantityInput();

            Console.WriteLine($"Adding: Date: {date} Quantity: {quantity} to the database.");

            DatabaseAccess.QueryAndDisplayResults(
                $"INSERT INTO {habit} (Date, Quantity) VALUES ('{date}', {quantity})"
            );

            Console.WriteLine("New habit data added successfully.");
            Console.WriteLine("Press 'Enter' to return to the main menu.");
            Console.ReadLine();
        }

        internal static void ModifyEntry(string modifySelection, string habit)
        {
            string capitalizedSelection =
                char.ToUpper(modifySelection[0]) + modifySelection.Substring(1);
            bool runModify = false;

            while (!runModify)
            {
                Console.Clear();

                Helpers.DisplayHeader($"Current Habit: {habit}");

                DatabaseAccess.QueryAndDisplayResults($"SELECT * FROM {habit}");

                Helpers.DisplayHeader($"Habit Record {capitalizedSelection}");

                Console.WriteLine(
                    $"Please type the ID of the record you want to {modifySelection}. Type 0 to return to the main menu."
                );

                string selection = Console.ReadLine();

                if (selection == "0")
                {
                    UserInterface.MainMenu();
                }

                while (!int.TryParse(selection, out _))
                {
                    Console.WriteLine("Please enter a valid number.");
                    selection = Console.ReadLine();
                }
                Console.Clear();

                bool recordExists = DatabaseAccess.QueryAndDisplayResults(
                    $"SELECT * FROM {habit} WHERE Id = '{selection}'"
                );

                if (recordExists)
                {
                    Helpers.DisplayHeader($"Habit Record {capitalizedSelection}");

                    Console.WriteLine(
                        $"Are you sure you want to {modifySelection} the above record? (y/n)"
                    );

                    string confirmModification = Console.ReadLine();

                    switch (confirmModification.ToLower())
                    {
                        case "y":
                            if (modifySelection == "update")
                            {
                                string newDate = InputValidation.GetDateInput();
                                double newQuantity = InputValidation.GetQuantityInput();
                                DatabaseAccess.QueryAndDisplayResults(
                                    $"UPDATE {habit} SET Date = '{newDate}', Quantity = {newQuantity} WHERE Id = {selection}"
                                );
                            }
                            else
                            {
                                DatabaseAccess.QueryAndDisplayResults(
                                    $"DELETE FROM {habit} WHERE Id = '{selection}'"
                                );
                                Console.WriteLine("Record deleted.");
                            }
                            runModify = true;
                            break;
                        case "n":
                            Console.Clear();
                            break;
                    }
                }
                else
                {
                    Helpers.DisplayHeader($"Habit Record {capitalizedSelection}");

                    Console.WriteLine(
                        $"Record {selection} does not exist in the database. Please check your selection."
                    );
                    Console.WriteLine($"Press 'Enter' to try again.");
                    Console.ReadLine();
                }
            }
        }

        internal static ArrayList? ListDatabaseTables()
        {
            try
            {
                ArrayList tableList = DatabaseAccess.GetTableList();

                if (tableList.Contains("sqlite_sequence"))
                {
                    tableList.Remove("sqlite_sequence");
                }

                foreach (var arrayList in tableList)
                {
                    Console.WriteLine(arrayList);
                }
                return tableList;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sorry, the database tables could not be retrieved. Error: {e}");
                ;
                return null;
            }
        }

        internal static string SwitchHabit(int autoSwitch = 0)
        {
            if (autoSwitch == 1)
            {
                Console.WriteLine("Due to deletion of currently selected habit, the default drinking water habit has been selected.");
                return "drinking_water";
            } else
            {
                ArrayList habitList = ListDatabaseTables();

                Console.WriteLine("Please select a habit by typing its name.");
                string selectedHabit = Console.ReadLine();

                
                while (!habitList.Contains(selectedHabit))
                {
                    Console.WriteLine(
                        $"{selectedHabit} does not exist, please ensure the name is typed correctly."
                    );
                    selectedHabit = Console.ReadLine();
                }

                return selectedHabit;
            }            
        }
    }
}
