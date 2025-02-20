using Spectre.Console;
using HabitLogger.mrgee1978.DomainLayer;
using HabitLogger.mrgee1978.DataAccessLayer;
using System.Globalization;

namespace HabitLogger.mrgee1978.PresentationLayer.Views;

public class RecordView
{
    private readonly InsertData _insertData;
    private readonly UpdateData _updateData;
    private readonly RetrieveData _retrieveData;
    private readonly DeleteData _deleteData;
    private readonly ValidationOfRecord _validateRecord;

    public RecordView()
    {
        _insertData = new InsertData();
        _updateData = new UpdateData();
        _retrieveData = new RetrieveData();
        _deleteData = new DeleteData();
        _validateRecord = new ValidationOfRecord();
    }

    /// <summary>
    /// Add a new record to the database based on validated user input
    /// </summary>
    public void AddRecord()
    {
        if (GetHabitCount() == 0)
        {
            AnsiConsole.MarkupLine("[bold red]There are currently no habits. Cannot add a record\nPress any key to return to the main menu[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        Console.Clear();
        new HabitView().ViewHabits();
        Console.WriteLine();

        bool isValidHabitId;

        int habitId = _validateRecord.GetValidInteger("Please enter the habit id for this record: or press '0' to return to the main menu ", "Valid habit id \nor press '0' to return to the main menu");
        if (habitId == 0)
        {
            Console.Clear();
            return;
        }

        isValidHabitId = IsValidHabitId(_retrieveData.RetrieveHabits(), habitId);

        while (!isValidHabitId)
        {
            AnsiConsole.MarkupLine("[bold underline red]There is no habit id that matches the value that was entered\n[/]");
            habitId = _validateRecord.GetValidInteger("Please enter a valid habit id for this record: or press '0' to return to the main menu ", "Valid habit id\n or press '0' to return to the main menu");

            if (habitId == 0)
            {
                Console.Clear();
                return;
            }
            isValidHabitId = IsValidHabitId(_retrieveData.RetrieveHabits(), habitId);
        }


        string dateString = _validateRecord.GetValidDateString("Please enter the date in the following format: dd-mm-yy or press '0' to return to the main menu ", "\nor press '0' to return to the main menu");

        if (dateString.Equals("0"))
        {
            Console.Clear();
            return;
        }

        int habitQuantity = _validateRecord.GetValidInteger("Please enter the habit quantity for this record: or press '0' to return to the main menu ", "Habit's quantity \nor press '0' to return to the main menu");

        if (habitQuantity == 0)
        {
            Console.Clear();
            return;
        }

        if (_insertData.InsertRecord(dateString, habitQuantity, habitId))
        {
            Console.Clear();
            ViewRecords();
            Console.WriteLine();
            AnsiConsole.MarkupLine("[blue]\nRecord added successfully!\nPress any key to return to the main menu\n[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold red]\nERROR: Record not added!\nPress any key to return to the main menu\n[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }

    /// <summary>
    /// Updates a record in the database based on user input
    /// </summary>
    public void UpdateRecord()
    {
        Console.Clear();
        if (!IsAnyRecords())
        {
            AnsiConsole.Markup("[red]There are currently no records!\nNothing available to update\nPress any key to continue[/]\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        ViewRecords();
        Console.WriteLine();

        if (GetHabitCount() == 0)
        {
            AnsiConsole.MarkupLine("[bold underline red]There are currently no habits. Cannot update a record[/]\n");
            return;
        }

        bool isValidRecordId = false;

        int id = _validateRecord.GetValidInteger("Please enter the id of the record that you wish to update: or press '0' to return to the main menu ", "Valid record id \nor press '0' to return to the main menu");

        if (id == 0)
        {
            Console.Clear();
            return;
        }
        isValidRecordId = _retrieveData.RetrieveRecords().Any(r => r.Id == id);

        while (!isValidRecordId)
        {
            AnsiConsole.MarkupLine("[bold underline red]There is no record id that matches the value that was entered[/]\n");
            id = _validateRecord.GetValidInteger("Please enter the id of the record that you wish to update: or press '0' to return to the main menu ", "Valid record id \nor press '0' to return to the main menu");
            if (id == 0)
            {
                return;
            }
            isValidRecordId = _retrieveData.RetrieveRecords().Any(r => r.Id == id);
        }

        Console.WriteLine("Please enter an updated date for the record in the format dd-mm-yy or press enter to keep the same date\nor press '0' to return to the main menu: ");
        string? updatedRecordDate = Console.ReadLine();

        if (updatedRecordDate == "0")
        {
            Console.Clear();
            return;
        }

        if (string.IsNullOrEmpty(updatedRecordDate))
        {
            DateTime updatedDateTime;
            updatedDateTime = _retrieveData.RetrieveRecords()
                .Where(r => r.Id == id)
                .Select(r => r.Date)
                .FirstOrDefault();
            updatedRecordDate = updatedDateTime.ToString("dd-MM-yy");
        }
        else
        {
            while (!DateTime.TryParseExact(updatedRecordDate, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                updatedRecordDate = _validateRecord.GetValidDateString("Please enter the date in format dd-mm-yy or press '0' to return to the main menu: ", "\nor press '0' to return to the main menu");
            }
        }

        int updatedQuantity = 0;

        Console.WriteLine("Please enter the updated quantity for the record or press enter to keep the same quantity\nor press '0' to return to the main menu ");
        string? updatedStringQuantity = Console.ReadLine();

        if (string.IsNullOrEmpty(updatedStringQuantity))
        {
            updatedQuantity = _retrieveData.RetrieveRecords()
                .Where(r => r.Id == id)
                .Select(r => r.Quantity)
                .FirstOrDefault();
        }
        else
        {
            while (!int.TryParse(updatedStringQuantity, out updatedQuantity))
            {
                Console.WriteLine("Please enter valid numeric input or press '0' to return to the main menu: ");
                updatedStringQuantity = Console.ReadLine();
            }
        }

        if (updatedQuantity == 0)
        {
            Console.Clear();
            return;
        }

        if (_updateData.UpdateRecord(id, updatedRecordDate, updatedQuantity))
        {
            Console.Clear();
            ViewRecords();
            Console.WriteLine();
            AnsiConsole.MarkupLine("[blue]\nRecord updated successfully!\nPress any key to return to the main menu[/]\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold red]\nERROR: Habit not updated!\nPress any key to return to the main menu\n[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }


    }

    /// <summary>
    /// Deletes a record from the database based on user input
    /// </summary>
    public void DeleteRecord()
    {
        Console.Clear();

        if (!IsAnyRecords())
        {
            AnsiConsole.Markup("[red]There are currently no records!\nNothing available to delete\nPress any key to continue[/]\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        ViewRecords();
        Console.WriteLine();

        if (GetHabitCount() == 0)
        {
            AnsiConsole.MarkupLine("[bold underline red]There are currently no habits. Cannot delete a record\nPress any key to continue[/]\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        bool isValidRecordId = false;

        int id = _validateRecord.GetValidInteger("Please enter the id of the record that you wish to delete: or press '0' to return to the main menu: ", "Valid record id \nor press '0' to return to the main menu");

        isValidRecordId = _retrieveData.RetrieveRecords().Any(r => r.Id == id);

        while (!isValidRecordId)
        {
            AnsiConsole.MarkupLine("[bold underline red]There is no record id that matches the value that was entered[/]\n");
            id = _validateRecord.GetValidInteger("Please enter the id of the record that you wish to delete: or press '0' to return to the main menu: ", "Valid record id \nor press '0' to return to the main menu");
            if (id == 0)
            {
                Console.Clear();
                return;
            }
            isValidRecordId = _retrieveData.RetrieveRecords().Any(r => r.Id == id);
        }

        if (_deleteData.DeleteRecord(id))
        {
            Console.Clear();
            if (!IsAnyRecords())
            {
                AnsiConsole.MarkupLine("[underline red]No records available to display\n[/]");
            }
            else
            {
                ViewRecords();
            }

            Console.WriteLine();
            AnsiConsole.MarkupLine("[blue]\nRecord deleted successfully!\nPress any key to return to the main menu[/]\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold red]\nERROR: Record not deleted!\nPress any key to return to the main menu\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }

    /// <summary>
    /// Displays all valid records in the database to the user
    /// </summary>
    public void ViewRecords()
    {
        if (!IsAnyRecords())
        {
            AnsiConsole.MarkupLine("[bold red]No records to display\n[/]");
            return;
        }

        List<Record> allRecords = _retrieveData.RetrieveRecords();

        AnsiConsole.MarkupLine("[underline blue]Records\n[/]");
        Table table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Unit of measurement");
        table.AddColumn("Habit");

        foreach (Record record in allRecords)
        {
            table.AddRow(record.Id.ToString(), record.Date.ToString("D"), $"{record.Quantity} {record.HabitMeasurement}", record.HabitName.ToString());
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Simply checks to make sure that there are records in the database
    /// </summary>
    /// <returns>bool</returns>
    public bool IsAnyRecords()
    {
        return _retrieveData.RetrieveRecords().Count > 0;
    }

    /// <summary>
    /// Simply returns the count of habits in the database to ensure that 
    /// it is valid to create a record
    /// </summary>
    /// <returns>int</returns>
    private int GetHabitCount()
    {
        return _retrieveData.RetrieveHabits().Count;
    }

    /// <summary>
    /// Checks to make sure that the habit id is a valid id before 
    /// creating a record 
    /// </summary>
    /// <param name="records"></param>
    /// <param name="habitId"></param>
    /// <returns>bool</returns>
    private bool IsValidHabitId(List<Habit> habits, int habitId)
    {
        return habits.Any(h => h.Id == habitId);
    }
}
