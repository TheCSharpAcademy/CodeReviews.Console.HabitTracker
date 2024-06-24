using System;

public enum MenuOption
{
    Exit = 0,
    CreateHabitType,
    ViewHabitTypes,
    UpdateHabitType,
    DeleteHabitType,
    CreateHabit,
    ViewHabits,
    UpdateHabit,
    DeleteHabit,
    GenerateYearlyReport
}

public class HabitTracker
{
    private readonly DatabaseManager _databaseManager;
    private readonly ReportManager _reportManager;

    public HabitTracker()
    {
        _databaseManager = new DatabaseManager();
        _reportManager = new ReportManager(_databaseManager);
    }

    public void Run()
    {
        DisplayMenu();
    }

    private void DisplayMenu()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.Clear();
            Console.WriteLine("Habit Tracker Menu:");
            Console.WriteLine("1. Create Habit Type");
            Console.WriteLine("2. View Habit Types");
            Console.WriteLine("3. Update Habit Type");
            Console.WriteLine("4. Delete Habit Type");
            Console.WriteLine("5. Create Habit");
            Console.WriteLine("6. View Habits");
            Console.WriteLine("7. Update Habit");
            Console.WriteLine("8. Delete Habit");
            Console.WriteLine("9. Generate Yearly Report");
            Console.WriteLine("0. Exit");

            MenuOption menuChoice = GetValidMenuChoice();

            switch (menuChoice)
            {
                case MenuOption.CreateHabitType:
                    InsertHabitType();
                    break;
                case MenuOption.ViewHabitTypes:
                    ViewHabitTypes();
                    break;
                case MenuOption.UpdateHabitType:
                    UpdateHabitType();
                    break;
                case MenuOption.DeleteHabitType:
                    DeleteHabitType();
                    break;
                case MenuOption.CreateHabit:
                    InsertHabit();
                    break;
                case MenuOption.ViewHabits:
                    ViewHabits();
                    break;
                case MenuOption.UpdateHabit:
                    UpdateHabit();
                    break;
                case MenuOption.DeleteHabit:
                    DeleteHabit();
                    break;
                case MenuOption.GenerateYearlyReport:
                    GenerateYearlyReport();
                    break;
                case MenuOption.Exit:
                    exitRequested = true;
                    break;
            }

            if (!exitRequested)
            {
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }
    }

    private static MenuOption GetValidMenuChoice()
    {
        MenuOption choice;
        while (true)
        {
            Console.Write("\nEnter your choice: ");
            if (Enum.TryParse(Console.ReadLine(), true, out choice) &&
                Enum.IsDefined(typeof(MenuOption), choice))
            {
                return choice;
            }
            Console.WriteLine("Invalid input. Please enter a number between 0 and 9.");
        }
    }

    private void InsertHabitType()
    {
        Console.Clear();

        string name = GetNonEmptyTrimmedUppercaseInputString("Enter the name of the habit: ", "The name cannot be null or empty. Please try again.");
        string unit = GetNonEmptyTrimmedUppercaseInputString("Enter the unit of measurement (e.g., glasses, minutes, etc.): ", "The unit cannot be null or empty. Please try again.");

        _databaseManager.InsertHabitType(name, unit);
        Console.WriteLine("Habit type created successfully.");
    }


    private void ViewHabitTypes(bool clearAtStart = true)
    {
        if (clearAtStart)
            Console.Clear();
        Console.WriteLine("ID | Name       | Unit");
        Console.WriteLine("-----------------------");
        var habitTypes = _databaseManager.GetHabitTypes();
        foreach (var (Id, Name, Unit) in habitTypes)
        {
            Console.WriteLine($"{Id,2} | {Name,-10} | {Unit}");
        }
    }

    private void UpdateHabitType()
    {
        Console.Clear();
        ViewHabitTypes(false);
        Console.Write("Enter habit type ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            string name = GetNonEmptyTrimmedUppercaseInputString("Enter the name of the habit: ", "The name cannot be null or empty. Please try again.");
            string unit = GetNonEmptyTrimmedUppercaseInputString("Enter the unit of measurement (e.g., glasses, minutes, etc.): ", "The unit cannot be null or empty. Please try again.");
            try
            {
                _databaseManager.UpdateHabitType(id, name, unit);
                Console.WriteLine("Habit type updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void DeleteHabitType()
    {
        Console.Clear();
        ViewHabitTypes(false);
        Console.Write("Enter habit type ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                _databaseManager.DeleteHabitType(id);
                Console.WriteLine("Habit type deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void InsertHabit()
    {
        Console.Clear();
        var habitTypes = _databaseManager.GetHabitTypes();
        if (habitTypes.Count == 0)
        {
            Console.WriteLine("No habit types available. Please create a habit type first.");
            return;
        }

        int habitTypeId = GetValidHabitTypeId(habitTypes);
        if (habitTypeId == -1)
        {
            return;
        }

        Console.Write("Enter the quantity of the habit: ");
        if (int.TryParse(Console.ReadLine(), out int quantity))
        {
            var habit = new Habit
            {
                Quantity = quantity,
                Date = DateTime.Now,
                HabitTypeId = habitTypeId
            };
            _databaseManager.InsertHabit(habit);
            Console.WriteLine("Habit created successfully.");
        }
        else
        {
            Console.WriteLine("Invalid quantity.");
        }
    }

    private void ViewHabits(bool clearAtStart = true)
    {
        if (clearAtStart)
            Console.Clear();
        Console.WriteLine("ID | Habit Type | Quantity | Unit     | Date");
        Console.WriteLine("---------------------------------------------");
        var habits = _databaseManager.GetHabits();
        foreach (var habit in habits)
        {
            Console.WriteLine($"{habit.Id,3} | {habit.HabitTypeName,-10} | {habit.Quantity,8} | {habit.Unit,-8} | {habit.Date:dd/MM/yyyy}");
        }
    }


    private void UpdateHabit()
    {
        Console.Clear();
        ViewHabits(false);
        Console.Write("Enter habit ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var habitTypes = _databaseManager.GetHabitTypes();
            int habitTypeId = GetValidHabitTypeId(habitTypes);
            if (habitTypeId == -1)
            {
                return;
            }

            Console.Write("Enter new quantity of the habit: ");
            if (int.TryParse(Console.ReadLine(), out int quantity))
            {
                var habit = new Habit
                {
                    Id = id,
                    Quantity = quantity,
                    Date = DateTime.Now,
                    HabitTypeId = habitTypeId
                };
                try
                {
                    _databaseManager.UpdateHabit(habit);
                    Console.WriteLine("Habit updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid quantity.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }


    private void DeleteHabit()
    {
        Console.Clear();
        ViewHabits(false);
        Console.Write("Enter habit ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                _databaseManager.DeleteHabit(id);
                Console.WriteLine("Habit deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void GenerateYearlyReport()
    {
        Console.Clear();
        int year;
        while (true)
        {
            Console.Write("Enter the year for the report: ");
            if (int.TryParse(Console.ReadLine(), out year) && year > 0 && year <= DateTime.Now.Year)
            {
                break;
            }
            Console.WriteLine("Invalid year. Please enter a valid year (1-{0}).", DateTime.Now.Year);
        }

        Console.Clear();
        Console.WriteLine($"Yearly Report for {year}");
        Console.WriteLine("========================");
        _reportManager.GenerateYearlyReport(year, true);
    }

    private int GetValidHabitTypeId(List<(int Id, string Name, string Unit)> habitTypes)
    {
        Console.WriteLine("Available Habit Types:");
        ViewHabitTypes(false);

        Console.Write("Enter the ID of the habit type: ");
        if (int.TryParse(Console.ReadLine(), out int habitTypeId))
        {
            bool habitTypeExists = habitTypes.Exists(ht => ht.Id == habitTypeId);
            if (!habitTypeExists)
            {
                Console.WriteLine("Habit type ID does not exist.");
                Console.ReadKey();
                return -1;
            }
            return habitTypeId;
        }
        Console.WriteLine("Invalid habit type ID.");
        return -1;
    }

    private string GetNonEmptyTrimmedUppercaseInputString(string prompt, string errorMessage)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input) || input.Trim().Length == 0)
                Console.WriteLine(errorMessage);
            else
                input = input.Trim().ToUpper();
        } while (string.IsNullOrEmpty(input) || input.Trim().Length == 0);
        return input;
    }
}
