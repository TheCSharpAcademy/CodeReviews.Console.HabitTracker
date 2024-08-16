using HabitLoggerLibrary.MansoorAZafar.Models;

namespace HabitLoggerLibrary.MansoorAZafar.Controllers
{
    internal class Reports
    {
        private DatabaseManager databaseManager;
        public Reports(DatabaseManager databaseManager)
        {
            this.databaseManager = databaseManager;
        }

        private ReportSelections GetReportInput()
        {
            ReportSelections selection = ReportSelections.None;

            Console.Write("> ");
            while (!(Enum.TryParse(System.Console.ReadLine()?.ToLower(), out selection))
                || selection == ReportSelections.None
                || !Enum.IsDefined(typeof(ReportSelections), selection))
                Console.Write("\nInvalid Selection, Please select a Valid input\n> ");

            return selection;
        }

        private void ViewReportOptions()
        {
            System.Console.Clear();
            System.Console.WriteLine("*".PadRight(29, '*'));
            System.Console.WriteLine(" These reports will show total hours for the given dates\n");
            System.Console.WriteLine(" Which Report?\n");
            System.Console.WriteLine
            (
                "1:   to start from X days ago\n" +
                "2:   to enter a specific starting date up to Today\n" +
                "3:   to see total of all time\n" +
                "4:   to choose a specific start and end date\n" +
                "5:   to see the total of a given month\n" +
                "0:   to return to the main menu\n" +
                "*".PadRight(29, '*')
            );
        }

        public void HandleReportSelection()
        {
            ViewReportOptions();
            ReportSelections selection = this.GetReportInput();

            switch (selection)
            {
                case ReportSelections.startFromXDaysAgo:
                    this.StartFromXDaysAgo();
                    break;
                case ReportSelections.dateToToday:
                    this.DateToToday();
                    break;
                case ReportSelections.totalAllTime:
                    this.TotalQuantityOfAllTime();
                    break;
                case ReportSelections.startToEnd:
                    this.StartToEnd();
                    break;
                case ReportSelections.totalForMonth:
                    this.TotalForAGivenMonth();
                    break;
                default:
                    break;
            }
        }

        private void StartFromXDaysAgo()
        {
            int daysAgo = 0;
            const int DaysInAYear = 365;
            Utilities.GetValidQuantity
            (
                quantity: ref daysAgo,
                message: "Please enter the How many days ago\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Number\n> ",
                lowerRange: 0,
                highRange: (Convert.ToInt32(DateTime.Now.ToString("yyyy")) * DaysInAYear) 
            );
            //The date from X days ago can only be an existing one otherwise it'll crash
            // so we take the current year -> turn into an int
            // multiply it by the days in a year so we can include all the days within the entire time-range


            DateTime startDate = DateTime.Now;
            startDate = startDate.AddDays(-daysAgo); // we want to subtract so we use negative

            DateTime endDate = DateTime.Now;
            System.Console.Clear();
            System.Console.WriteLine($"Displaying Dates from {startDate} till now\n");
            this.databaseManager.GetDataFromDate(start: ref startDate, end: ref endDate);


            Utilities.PressToContinue();

        }

        private void DateToToday()
        {
            DateTime startDate = DateTime.Now;
            Utilities.GetValidDateddMMyyFormat
            (
                date: ref startDate,
                message: "Please enter the Starting Date\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Date\n> "
            );

            DateTime endDate = DateTime.Now;
            System.Console.Clear();
            System.Console.WriteLine($"Displaying Dates from {startDate} till now\n");
            this.databaseManager.GetDataFromDate(start: ref startDate, end: ref endDate);


            Utilities.PressToContinue();
        }

        private void TotalQuantityOfAllTime()
        {
            System.Console.Clear();
            System.Console.WriteLine("The Total Quantity is ");
            Console.Write($"> {this.databaseManager.GetTotalQuantity()}\n\n");

            Utilities.PressToContinue();
        }

        private void StartToEnd()
        {
            DateTime startDate = DateTime.Now;
            Utilities.GetValidDateddMMyyFormat
            (
                date: ref startDate,
                message: "Please enter the Starting Date\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Date\n> "
            );

            DateTime endDate = DateTime.Now;
            Utilities.GetValidDateddMMyyFormat
            (
                date: ref endDate,
                message: "Please enter the Ending Date\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Date\n> "
            );


            System.Console.Clear();
            System.Console.WriteLine($"Displaying Dates from {startDate} till now\n");
            this.databaseManager.GetDataFromDate(start: ref startDate, end: ref endDate);


            Utilities.PressToContinue();
        }

        private void TotalForAGivenMonth()
        {
            System.Console.Clear();
            int month = 0;
            const int MonthLowerRange = 1;
            const int MonthUpperRange = 12;
            Utilities.GetValidQuantity
            (
                quantity: ref month,
                message: "Enter the Month Number\n> ",
                errorMessage: "Invalid Month\nPlease Enter the Month's Number\n> ",
                lowerRange: MonthLowerRange,
                highRange: MonthUpperRange
            ); 

            System.Console.WriteLine("\nThe Total Quantity is ");
            Console.Write($"> {this.databaseManager.GetTotalQuantityFromMonth(ref month)}\n\n");

            Utilities.PressToContinue();
        }
    }
}
