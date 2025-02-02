using mrgee1978.HabitLogger.Controllers;
using mrgee1978.HabitLogger.Validation;

namespace mrgee1978.HabitLogger.Views;

// This class is responsible for querying the user about what data to add to the database
public class AddData
{
    private HabitController _habitController = new HabitController();
    private RecordController _recordController = new RecordController();
    private Validator _validator = new Validator();

    /// <summary>
    /// Queries the user to allow them to specify the information for each 
    /// individual habit added
    /// </summary>
    public void AddHabit()
    {
        string name = _validator.GetValidString("Please enter habit name: ");
        string description = _validator.GetValidString("Please enter a description of the habit: ");

        _habitController.AddHabitToDatabase(name, description);
    }

    /// <summary>
    /// Queries with the user to allow them to specify the information for each 
    /// individual record added
    /// </summary>
    public void AddRecord()
    {
        string date = _validator.GetValidDateString("Please enter a date in the format dd-mm-yyyy: ");
        int quantity = _validator.GetValidNumericInput("Please enter the quantity: ");
        int habitId = _validator.GetValidNumericInput("Please enter the Id of the habit you wish to add this record for: ");

        _recordController.AddRecordToDatabase(date, quantity, habitId);
    }
}
