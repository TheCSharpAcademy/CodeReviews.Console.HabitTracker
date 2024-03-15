// My Habit Logger Console application
// 
// Check ReadMe at GitHub for specific user instructions and documentation
//
// Current tasks/WIP:
// - Additional challenges
//
using DatabaseHelpers.HopelessCoding;
using DatabaseCommands.HopelessCoding;
using GenerateReports.HopelessCoding;

namespace HabitLogger
{
    static class Program
    {
        static void Main()
        {
            DbHelpers.InitializeDatabase();

            while (true)
            {
                Console.WriteLine("MAIN MENU\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("A - Add New Record");
                Console.WriteLine("V - View All Records");
                Console.WriteLine("U - Update Record");
                Console.WriteLine("D - Delete Record");
                Console.WriteLine("R - Generate Reports");
                Console.WriteLine("0 - Close Application");
                Console.WriteLine("----------------------------");

                switch (Console.ReadLine().ToUpper())
                {
                    case "A":
                        DbCommands.AddNewRecord();
                        break;
                    case "V":
                        DbHelpers.ViewRecords();
                        break;
                    case "U":
                        DbCommands.UpdateRecord();
                        break;
                    case "D":
                        DbCommands.DeleteRecord();
                        break;
                    case "R":
                        PrintReports();
                        break;
                    case "0":
                        Console.WriteLine("Application is closing...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input, try again.");
                        Console.WriteLine("----------------------------");
                        break;
                }
            }
        }

        private static void PrintReports()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("SHOW REPORTS\n");
                Console.WriteLine("What report would you like to see?\n");
                Console.WriteLine("3 - Last 3 Days");
                Console.WriteLine("7 - Last 7 Days");
                Console.WriteLine("A - Average Calories for Last X Days");
                Console.WriteLine("0 - Exit to Main Menu");
                Console.WriteLine("----------------------------");

                switch (Console.ReadLine().ToUpper())
                {
                    case "3":
                        Reports.TimelineReports(3);
                        break;
                    case "7":
                        Reports.TimelineReports(7);
                        break;
                    case "A":
                        Reports.AverageReports();
                        break;
                    case "0":
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Invalid input, try again.");
                        break;
                }
            }
        }
    }
}