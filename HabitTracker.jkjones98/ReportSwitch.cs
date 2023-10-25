using ColumnName;
using HabitTracker.DisplayReports;
using UserInput;

namespace HabitTracker.SelectReport;

public class ReportSwitch
{
    public void ReportOptions()
    {
        
        // boolean to check whether user has entered numbers
        bool checkForDigits;

        Console.WriteLine("\nPlease enter letter associated with the type of report you would like to view\n");
        Console.WriteLine("Enter T for the total number of runs complete");
        Console.WriteLine("Enter S for the total number of miles/kms ran");
        Console.WriteLine("Enter A for the average number of miles per run");
        Console.WriteLine("Enter M to return to the main menu");
        string reportSelection = Console.ReadLine();

        // If to set bool checkForDigits as true or false
        if(checkForDigits = reportSelection.Any(char.IsDigit))
        {
            Console.WriteLine("Please enter a valid option");
            ReportOptions();
        }
        else
        {
            // Make upper case so it will select regardles of upper or lower
            string validSelection = reportSelection.ToUpper();
            ChosenReportSwitch(validSelection);
        }

    }

    public void ChosenReportSwitch(string op)
    {
        ClassUserInput mainMenu = new ClassUserInput();

        DisplaySQLiteReports runReport = new DisplaySQLiteReports();

        GetColumnName columnNameObj = new GetColumnName();
        string columnName = columnNameObj.GetColName();
        switch(op)
        {
            case "T":
                string count = $"SELECT COUNT({columnName}) FROM drinking_water";
                string total = "Total runs complete";
                runReport.Reports(count, total);
                break;
            case "S":
                string sum = $"SELECT sum({columnName}) FROM drinking_water";
                string sumCol = "Total miles ran";
                runReport.Reports(sum, sumCol);
                break; 
            case "A":
                string average = $"SELECT avg({columnName}) FROM drinking_water";
                string avgCol = "Average miles per run";
                runReport.Reports(average, avgCol);
                break;
            case "M":
                mainMenu.GetUserInput();
                break;
            default:
                Console.Clear();
                Console.WriteLine($"Invalid option selected, please try again");
                ReportOptions();
                break;       
        }
    }
}