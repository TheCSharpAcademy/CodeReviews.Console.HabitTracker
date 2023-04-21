
namespace HabitTracker.Chad1082
{
    internal class Menu
    {
        internal void ShowMainMenu()
        {
            
            Console.WriteLine("Welcome to the Step logger app!");

            do
            {
                MainMenu();
            } while (true);
        }

        private void MainMenu()
        {

            Console.WriteLine("******************************");
            Console.WriteLine("Please select an option below:");
            Console.WriteLine("A - Add new number of steps");
            Console.WriteLine("D - Delete a previous entry");
            Console.WriteLine("U - Update an existing entry");
            Console.WriteLine("V - View logged entries");
            Console.WriteLine("0 - Exit the application");
            Console.WriteLine("******************************");

            string menuOption = Console.ReadLine().Trim().ToUpper();
            switch (menuOption)
            {
                case "A":
                    AddSteps();
                    Console.Clear();
                    break;
                case "D":
                    DeleteEntry();
                    Console.Clear();
                    break;
                case "U":
                    UpdateEntry();
                    Console.Clear();
                    break;
                case "V":
                    ViewLoggedEntries();
                    Console.Clear();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Option not recognised");
                    break;
            }
        }

        private void ViewLoggedEntries()
        {
            DisplayAllEntries();

            Console.WriteLine("Press enter to continue....");
            Console.Read();
        }

        private static void DisplayAllEntries()
        {
            Console.WriteLine("The following entries have been logged:\n");

            List<Entry> entries = new();
            entries = Database.GetEntries();

            foreach (Entry entry in entries)
            {
                Console.WriteLine("{0},\tDate: {1},\t Steps:{2}", entry.EntryID, entry.DateAdded, entry.Steps);
            }

        }

        private void DeleteEntry()
        {
            DisplayAllEntries();

            Console.WriteLine("Please enter the number of the entry you would like to Delete\nEnter Q to return to main menu.");

            string selection = Console.ReadLine();
            int entryID = 0;

            while (selection.ToLower() != "q" && !int.TryParse(selection, out entryID))
            {
                Console.Write("Unrecognised input, which entry would you like to delete?: ");
                selection = Console.ReadLine();
            }

            if (selection.ToLower() == "q")
            {
                Console.WriteLine("Returning to main menu.");
                Console.WriteLine("Press enter to continue....");
                Console.Read();

            } else
            {
                if (Database.DeleteEntry(entryID))
                {
                    Console.WriteLine("Entry deleted");
                    Console.WriteLine("Press enter to continue....");
                    Console.Read();
                }
                else
                {
                    Console.WriteLine("Entry Not Found!");
                    DeleteEntry();
                }
            }

            
        }

        private void AddSteps()
        {
            Console.WriteLine("Create a new steps entry:\n\n");
            Console.Write("When did you count these steps? (Enter date in dd/mm/yyyy format, leave blank for today):  ");
            string dateEntry = Console.ReadLine();
            DateTime tmpDate;

            while (dateEntry != "" && !DateTime.TryParse(dateEntry, out tmpDate))
            {
                Console.Write("Unrecognised input, Enter date in dd/mm/yyyy format or leave blank for today: ");
                dateEntry = Console.ReadLine();
            }

            if ( dateEntry == "")
            {
                dateEntry = DateTime.Now.ToShortDateString();
            }


            Console.Write("How many steps did you achieve?:  ");
            string stepEntry = Console.ReadLine();

            int stepAmount;
            while (!int.TryParse(stepEntry, out stepAmount))
            {
                Console.Write("Unrecognised input, How steps did you achieve in full numbers only?: ");
                stepEntry = Console.ReadLine();
            }

            
            Database.AddEntry(dateEntry, stepAmount);
            Console.WriteLine($"Recorded {stepAmount} steps for {dateEntry}. Press enter to continue....");
            Console.Read();
        }

        private void UpdateEntry()
        {
            DisplayAllEntries();

            Console.WriteLine("Please enter the number of the entry you would like to Update\nEnter Q to return to main menu.");

            string selection = Console.ReadLine();
            int entryID = 0;

            while (selection.ToLower() != "q" && !int.TryParse(selection, out entryID))
            {
                Console.Write("Unrecognised input, which entry would you like to Update?: ");
                selection = Console.ReadLine();
            }

            if (selection.ToLower() == "q")
            {
                Console.WriteLine("Returning to main menu.");
                Console.WriteLine("Press enter to continue....");
                Console.Read();

            }
            else
            {
                Entry entry = Database.GetSingleEntry(entryID);

                if (entry == null)
                {
                    Console.Clear();
                    Console.WriteLine("Entry Not Found!");
                    UpdateEntry();
                }
                else
                {
                    Console.WriteLine("{0},\tDate: {1},\t Steps:{2}", entry.EntryID, entry.DateAdded, entry.Steps);

                    Console.Write("Enter the new date (Enter date in dd/mm/yyyy format, leave blank for no change):  ");
                    string dateEntry = Console.ReadLine();
                    DateTime tmpDate;

                    while (dateEntry != "" && !DateTime.TryParse(dateEntry, out tmpDate))
                    {
                        Console.Write("Unrecognised input, Enter date in dd/mm/yyyy format or leave blank for no change: ");
                        dateEntry = Console.ReadLine();
                    }

                    if (dateEntry == "")
                    {
                        dateEntry = entry.DateAdded.ToString();
                    }


                    Console.Write("Enter the amount of steps to record, leave blank for no change: ");
                    string stepEntry = Console.ReadLine();

                    int stepAmount = 0;
                    while (stepEntry != "" && !int.TryParse(stepEntry, out stepAmount))
                    {
                        Console.Write("Unrecognised input, Enter the amount of steps to record, leave blank for no change: ");
                        stepEntry = Console.ReadLine();
                    }

                    if (stepEntry == "")
                    {
                        stepAmount = entry.Steps;
                    }

                    Database.UpdateEntry(entryID, dateEntry, stepAmount);

                    Console.WriteLine("Entry Updated");
                    Console.WriteLine("Press enter to continue....");
                    Console.Read();
                    
                }
            }
        }

    }

    public enum MenuOptions
    {
        Add,
        Delete,
        Update,
        View
    }
}
