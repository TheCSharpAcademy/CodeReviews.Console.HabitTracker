using UserInput;
using DisplayRecords;

namespace ViewAllRecords;

public class ViewAll
{
    AllRecords showRecords = new AllRecords();
    ClassUserInput mainMenu = new ClassUserInput();
    string connectionString = @"Data Source=habit-Tracker2.db";
    public void ViewAllMethod()
    {
        
        showRecords.DisplayRecs();
        mainMenu.GetUserInput();
        
    }
}