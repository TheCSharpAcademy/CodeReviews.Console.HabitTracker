using Spectre.Console;
using HabitLogger.mrgee1978.DomainLayer;
using HabitLogger.mrgee1978.DataAccessLayer;


namespace HabitLogger.mrgee1978.PresentationLayer.Views;

public class HabitView
{
    private readonly InsertData _insertData;
    private readonly UpdateData _updateData;
    private readonly RetrieveData _retrieveData;
    private readonly DeleteData _deleteData;
    private readonly ValidationOfHabit _validateHabit;
    
    public HabitView()
    {
        _insertData = new InsertData();
        _updateData = new UpdateData();
        _retrieveData = new RetrieveData();
        _deleteData = new DeleteData();
        _validateHabit = new ValidationOfHabit();
    }

    /// <summary>
    /// Adds a habit to the database after getting valid input from the user
    /// </summary>
    public void AddHabit()
    {
        string name = _validateHabit.GetValidString("Please enter the name of your habit or press '0' to return to the main menu : ","Habit name or press '0' to return to the main menu");
        if (name == "0")
        {
            Console.Clear();
            return;
        }

        string unitOfMeasurement = _validateHabit.GetValidString("Please enter the unit of measurement for your habit or press '0' to return to the main menu: ", "Habit's unit of measurement \nor press '0' to return to the main menu");
        if (unitOfMeasurement == "0")
        {
            Console.Clear();
            return;
        }

        if (_insertData.InsertHabit(name, unitOfMeasurement))
        {
            Console.Clear();
            ViewHabits();
            Console.WriteLine();
            AnsiConsole.MarkupLine("[blue]\nHabit added successfully!\nPress any key to return to the main menu[/]\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold red]\nERROR: Habit not added!\nPress any key to return to the main menu\n[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }

    /// <summary>
    /// Updates a habit in the database after getting valid input from the user
    /// </summary>
    public void UpdateHabit()
    {
        Console.Clear();
        if (!IsAnyHabits())
        {
            AnsiConsole.MarkupLine("[red]No habits to display\nPress any key to continue\n[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        ViewHabits();
        Console.WriteLine();

        List<Habit> allHabits = _retrieveData.RetrieveHabits();
        bool isValidHabitId = false;

        int id = _validateHabit.GetValidInteger("Please enter the id of the habit that you wish to update: or press '0' to return to the main menu: ", "Habit id \nor press '0' to return to the main menu");
        if (id == 0)
        {
            Console.Clear();
            return;
        }
        isValidHabitId = allHabits.Any(h => h.Id == id);

        if (!isValidHabitId)
        {
            Console.WriteLine("The habit id you entered is not a valid id");
            id = _validateHabit.GetValidInteger("Please enter a valid id or press '0' to return to the main menu. ", "Habit id \nor press '0' to return to the main menu");
            if (id == 0)
            {
                Console.Clear();
                return;
            }
            isValidHabitId = allHabits.Any(h => h.Id == id);
        }

        Console.WriteLine("Please enter the updated name for the habit or press enter to keep the same habit name: ");
        string? updatedHabitName = Console.ReadLine();
        
        if (string.IsNullOrEmpty(updatedHabitName))
        {
            updatedHabitName = allHabits
                .Where(h => h.Id == id)
                .Select(h => h.Name)
                .FirstOrDefault();
        }

        Console.WriteLine("Please enter the updated unit of measurement or press enter to keep the same unit of measurement: ");
        string? updatedUnitOfMeasurement = Console.ReadLine();

        if (string.IsNullOrEmpty(updatedUnitOfMeasurement))
        {
            updatedUnitOfMeasurement = allHabits
                .Where(h => h.Id == id)
                .Select(h => h.UnitOfMeasurement)
                .FirstOrDefault();
        }
        
        if (_updateData.UpdateHabit(id, updatedHabitName, updatedUnitOfMeasurement))
        {
            Console.Clear();
            ViewHabits();
            Console.WriteLine();
            AnsiConsole.MarkupLine("[blue]\nHabit updated successfully!\nPress any key to return to the main menu[/]\n");
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
    /// Deletes a habit based on id
    /// </summary>
    public void DeleteHabit()
    {
        Console.Clear();
        if (!IsAnyHabits())
        {
            AnsiConsole.MarkupLine("[red]No habits to display\nPress any key to continue[/]\n");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        ViewHabits();
        Console.WriteLine();

        List<Habit> allHabits = _retrieveData.RetrieveHabits();
        bool isValidHabitId = false;

        int id = _validateHabit.GetValidInteger("Please enter the id of the habit that you wish to delete: or press '0' to return to the main menu ", "Habit id \nor press '0' to return to the main menu");
        if (id == 0)
        {
            Console.Clear();
            return;
        }
        isValidHabitId = allHabits.Any(h => h.Id == id);

        if (!isValidHabitId)
        {
            Console.WriteLine("The habit id that you entered is not a valid habit id: ");
            id = _validateHabit.GetValidInteger("Please enter a valid id or press '0' to return to the main menu. ", "Habit id \nor press '0' to return to the main menu");
            if (id == 0)
            {
                Console.Clear();
                return;
            }
            isValidHabitId = allHabits.Any(h => h.Id == id);
        }

        if (_deleteData.DeleteHabit(id))
        {
            Console.Clear();
            if (!IsAnyHabits())
            {
                AnsiConsole.MarkupLine("[underline red]No habits to display\n[/]");
            }
            else
            {
                ViewHabits();
            }
            Console.WriteLine();
            AnsiConsole.MarkupLine("[blue]\nHabit deleted successfully!\nPress any key to return to the main menu\n[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold red]\nERROR: Habit not deleted!\nPress any key to return to the main menu\n[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }

    /// <summary>
    /// Allows the user to view all habits currently in the database
    /// </summary>
    public void ViewHabits()
    {
        List<Habit> allHabits = _retrieveData.RetrieveHabits();

        if (!IsAnyHabits())
        {
            AnsiConsole.MarkupLine("[bold red]No habits to display\n");
            return;
        }
        AnsiConsole.MarkupLine("[underline blue]Habits\n[/]");
        Table table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Unit of measurement");

        foreach (Habit habit in allHabits)
        {
            table.AddRow(habit.Id.ToString(), habit.Name.ToString(), habit.UnitOfMeasurement.ToString());
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Simply checks to make sure that there are habits in the database
    /// </summary>
    /// <returns>bool</returns>
    private bool IsAnyHabits()
    {
        return _retrieveData.RetrieveHabits().Count > 0;
    }
}
