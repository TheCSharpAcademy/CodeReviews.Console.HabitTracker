using Insert;
using UpdateRecord;
using ViewAllRecords;
using DeleteRecord;
using ChangeMeasurement;
using HabitTracker.SelectReport;

namespace EditTableOperations;

public class Operations
{
    string connectionString = @"Data Source=habit-Tracker2.db";

    public Operations()
    {
        
    }

    public void EditTableSwitch(int op)
    {
        switch (op)
        {
            case 0:
                Console.Clear();
                Console.WriteLine("Closing application");
                Environment.Exit(0);
                break;
            case 1:
                Console.Clear();
                ViewAll viewRecords = new ViewAll();
                viewRecords.ViewAllMethod();
                break;
            case 2:
                InsertRecord insertRecord = new InsertRecord();
                insertRecord.InsertMethod();
                break;
            case 3:
                Delete deleteRecord = new Delete();
                deleteRecord.DelRecMethod();
                break;
            case 4:
                Update updateDatabase = new Update();
                updateDatabase.UpdateRecMethod();
                break;
            case 5:
                ChangeMeasurementUnit changeUnit = new ChangeMeasurementUnit();
                changeUnit.MeasurementUnit();
                break;
            case 6:
                Console.WriteLine("Go to report options switch");
                ReportSwitch reportSwitch = new ReportSwitch();
                reportSwitch.ReportOptions();
                break;
            default:
                Console.WriteLine("\nInvalid selection please enter a number between 0 and 4.\n");
                break;
        }
    }

}