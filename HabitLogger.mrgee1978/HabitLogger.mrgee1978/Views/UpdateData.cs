using Spectre.Console;
using mrgee1978.HabitLogger.Controllers;
using mrgee1978.HabitLogger.Validation;

namespace mrgee1978.HabitLogger.Views;

// This class is responsible for querying the user about what data to update in the database
public class UpdateData
{
    private HabitController _habitController = new HabitController();
    private RecordController _recordController = new RecordController();
    private Validator _validator = new Validator();

    /// <summary>
    /// Queries the user for what data they want to update for the individual habit
    /// </summary>
    public void UpdateHabit()
    {
        int id = _validator.GetValidNumericInput("Please enter the id of the habit you wish to update: ");

        string name = string.Empty;
        bool updateName = AnsiConsole.Confirm("Update name of habit? ");

        if (updateName)
        {
            name = _validator.GetValidString("Enter updated name for the habit: ");
        }

        string description = string.Empty;
        bool updateDescription = AnsiConsole.Confirm("Update the description of the habit? ");

        if (updateDescription)
        {
            description = _validator.GetValidString("Enter the updated description for the habit: ");
        }

        string updateQuery = string.Empty;

        if (updateName && updateDescription)
        {
            updateQuery = $"UPDATE habits SET Name = '{name}', Description = {description} WHERE Id = {id}";
        }
        else if (updateName && !updateDescription)
        {
            updateQuery = $"UPDATE habits SET Name = '{name}' WHERE Id = {id}";
        }
        else
        {
            updateQuery = $"UPDATE habits SET Description = '{description}' WHERE Id = {id}";
        }

        _habitController.UpdateHabitInDatabase(updateQuery);
    }

    /// <summary>
    /// Queries the user for what data they want to update for the individual record
    /// </summary>
    public void UpdateRecord()
    {
        int id = _validator.GetValidNumericInput("Please enter the id of the record that you wish to update: ");

        string date = string.Empty;
        bool updateDate = AnsiConsole.Confirm("Update date of record? ");

        if (updateDate)
        {
            date = _validator.GetValidDateString("Please enter the updated date in the format dd-mm-yyyy: ");
        }

        int quantity = 0;
        bool updateQuantity = AnsiConsole.Confirm("Update quantity in the record? ");

        if (updateQuantity)
        {
            quantity = _validator.GetValidNumericInput("Please enter the updated quantity: ");
        }

        string updateQuery = string.Empty;

        if (updateDate && updateQuantity)
        {
            updateQuery = $"UPDATE records SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";
        }
        else if (updateDate && !updateQuantity)
        {
            updateQuery = $"UPDATE records SET Date = '{date}' WHERE Id = {id}";
        }
        else
        {
            updateQuery = $"UPDATE records SET Quantity = '{quantity}' WHERE Id = {id}";
        }

        _recordController.UpdateRecordInDatabase(updateQuery);
    }
}
