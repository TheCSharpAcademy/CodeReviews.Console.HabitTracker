using System.Globalization;

namespace HabitTracker
{
    internal static class Helpers
    {

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            var numberInput = Console.ReadLine().Trim();

            if (numberInput == "0")
                MainMenu.GetUserInput();

            while (Int32.TryParse(numberInput, out _) == false || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("Invalid number. Try again.");
                numberInput = Console.ReadLine();
            }

            var finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }

        internal static string GetDateInput()
        {
            Console.Clear();
            Console.WriteLine("Please enter the date: (mm-dd-yyyy). Type 0 to return to the Main Menu.");
            var dateInput = Console.ReadLine().Trim();

            if (dateInput == "0")
                MainMenu.GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "MM-dd-yyyy", new CultureInfo("en-us"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date. Enter a date in the following format: (mm-dd-yyyy)" +
                                  " or type 0 to return to the Main Menu.");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static string GetActivityInput()
        {
            Console.WriteLine("Enter the activity you would like to track or type 0 to return to the Main Menu");
            var activityInput = Console.ReadLine().Trim();

            if (activityInput == "0")
            {
                MainMenu.GetUserInput();
            }
            else if (String.IsNullOrEmpty(activityInput))
            {
                Console.WriteLine("Invalid input.");
            }

            return activityInput;
        }

        internal static string GetUnitInput()
        {
            Console.WriteLine("Enter the unit of measurement you would like to use (ex: miles, cups, laps, hours)" +
                              " or type 0 to return to the Main Menu");
            var unitInput = Console.ReadLine().Trim();

            if (unitInput == "0")
            {
                MainMenu.GetUserInput();
            }
            else if (String.IsNullOrEmpty(unitInput))
            {
                Console.WriteLine("Invalid input.");
            }

            return unitInput;
        }

        internal static string GetViewType()
        {
            Console.WriteLine("\n\nType 'all' to view all records, 'activity' to view by activity type, or 'date' to view by date.\n" +
                              "Type 0 to return to the Main Menu.\n\n");
            var viewInput = Console.ReadLine().Trim();

            if (viewInput == "0")
                MainMenu.GetUserInput();

            else if (viewInput != "all" && viewInput != "activity" && viewInput != "date")
            {
                Console.Clear();
                Console.WriteLine("\n\nInvalid command. Review the options and try again.\n");
                GetViewType();
            }

            return viewInput;
        }

        internal static string GetDateViewType()
        {
            Console.WriteLine("\n\nType 'specific' to view a record from a specific date, or type 'year' to view records from a year.\n" +
                              "Type 0 to return to the Main Menu\n\n");
            var dateViewType = Console.ReadLine().Trim();

            string date;
            string tableCmd = "";

            switch (dateViewType)
            {
                case "0":
                    MainMenu.GetUserInput();
                    break;

                case "specific":
                    date = Helpers.GetDateInput();
                    tableCmd = $"SELECT * FROM habits WHERE Date = '{date}'";
                    break;

                case "year":
                    Console.Clear();

                    Console.WriteLine("Enter a year to view all records from that year: (eg. 2023)");
                    date = Console.ReadLine();


                    while (!DateTime.TryParseExact(date, "yyyy", new CultureInfo("en-us"), DateTimeStyles.None, out _))
                    {
                        Console.WriteLine("Invalid year. Enter the year in the following format: (yyyy)");
                        date = Console.ReadLine();
                    }

                    tableCmd = $"SELECT * FROM habits WHERE Date LIKE '%{date}%'";
                    break;
            }
            return tableCmd;
        }
    }
}