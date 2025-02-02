using mrgee1978.HabitLogger.Controllers;
using mrgee1978.HabitLogger.Validation;

namespace mrgee1978.HabitLogger.Views;

// This class is responsible for querying the user about what data to remove from the database
public class DeleteData
{
    private HabitController _habitController = new HabitController();
    private RecordController _recordController = new RecordController();
    private Validator _validator = new Validator();

    /// <summary>
    /// Queries the user for the id of the habit to be removed
    /// </summary>
    public void DeleteHabit()
    {
        int id = _validator.GetValidNumericInput("Enter the id of the habit that you wish to delete: ");
        _habitController.DeleteHabitFromDatabase(id);
    }

    /// <summary>
    /// Queries the user for the id of the record to be removed
    /// </summary>
    public void DeleteRecord()
    {
        int id = _validator.GetValidNumericInput("Enter the id of the record that you wish to delete: ");
        _recordController.DeleteRecordFromDatabase(id);
    }
}
