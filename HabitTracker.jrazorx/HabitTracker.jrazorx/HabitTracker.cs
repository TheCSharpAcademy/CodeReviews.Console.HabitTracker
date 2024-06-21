using System;
using System.Collections.Generic;

public class HabitTracker
{
    private readonly DatabaseManager _databaseManager;

    public HabitTracker()
    {
        _databaseManager = new DatabaseManager();
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
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
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        InsertHabitType();
                        break;
                    case 2:
                        ViewHabitTypes();
                        break;
                    case 3:
                        UpdateHabitType();
                        break;
                    case 4:
                        DeleteHabitType();
                        break;
                    case 5:
                        InsertHabit();
                        break;
                    case 6:
                        ViewHabits();
                        break;
                    case 7:
                        UpdateHabit();
                        break;
                    case 8:
                        DeleteHabit();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private void InsertHabitType()
    {
        Console.Clear();

        string name;
        do
        {
            Console.Write("Enter the name of the habit: ");
            name = Console.ReadLine();
            if (string.IsNullOrEmpty(name) || name.Trim().Length == 0)
                Console.WriteLine("The name cannot be null or empty. Please try again.");
            else
                name = name.Trim().ToUpper();
        } while (string.IsNullOrEmpty(name) || name.Trim().Length == 0);

        string unit;
        do
        {
            Console.Write("Enter the unit of measurement (e.g., glasses, minutes, etc.): ");
            unit = Console.ReadLine();
            if (string.IsNullOrEmpty(unit) || unit.Trim().Length == 0)
                Console.WriteLine("The unit cannot be null or empty. Please try again.");
            else
                unit = unit.Trim().ToUpper();
        } while (string.IsNullOrEmpty(unit) || unit.Trim().Length == 0);

        _databaseManager.InsertHabitType(name, unit);
        Console.WriteLine("Habit type created successfully. Press any key to continue...");
        Console.ReadKey();
    }


    private void ViewHabitTypes(bool clearAtStart = true, bool waitForInputAtTheEnd = true)
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
        if (waitForInputAtTheEnd)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(); 
        }
    }

    private void UpdateHabitType()
    {
        Console.Clear();
        ViewHabitTypes(false, false);
        Console.Write("Enter habit type ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            string name;
            do
            {
                Console.Write("Enter the new name of the habit: ");
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name) || name.Trim().Length == 0)
                    Console.WriteLine("The name cannot be null or empty. Please try again.");
                else
                    name = name.Trim().ToUpper();
            } while (string.IsNullOrEmpty(name) || name.Trim().Length == 0);

            string unit;
            do
            {
                Console.Write("Enter the new unit of measurement (e.g., glasses, minutes, etc.): ");
                unit = Console.ReadLine();
                if (string.IsNullOrEmpty(unit) || unit.Trim().Length == 0)
                    Console.WriteLine("The unit cannot be null or empty. Please try again.");
                else
                    unit = unit.Trim().ToUpper();
            } while (string.IsNullOrEmpty(unit) || unit.Trim().Length == 0);
            try
            {
                _databaseManager.UpdateHabitType(id, name, unit);
                Console.WriteLine("Habit type updated successfully. Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Press any key to continue...");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID. Press any key to continue...");
        }
        Console.ReadKey();
    }

    private void DeleteHabitType()
    {
        Console.Clear();
        ViewHabitTypes(false, false);
        Console.Write("Enter habit type ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                _databaseManager.DeleteHabitType(id);
                Console.WriteLine("Habit type deleted successfully. Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Press any key to continue...");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID. Press any key to continue...");
        }
        Console.ReadKey();
    }

    private void InsertHabit()
    {
        Console.Clear();
        var habitTypes = _databaseManager.GetHabitTypes();
        if (habitTypes.Count == 0)
        {
            Console.WriteLine("No habit types available. Please create a habit type first. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Available Habit Types:");
        ViewHabitTypes(false, false);

        Console.Write("Enter the ID of the habit type: ");
        if (int.TryParse(Console.ReadLine(), out int habitTypeId))
        {
            // Check if the habit type ID exists
            bool habitTypeExists = habitTypes.Exists(ht => ht.Id == habitTypeId);
            if (!habitTypeExists)
            {
                Console.WriteLine("Habit type ID does not exist. Press any key to continue...");
                Console.ReadKey();
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
                Console.WriteLine("Habit created successfully. Press any key to continue...");
            }
            else
            {
                Console.WriteLine("Invalid quantity. Press any key to continue...");
            }
        }
        else
        {
            Console.WriteLine("Invalid habit type ID. Press any key to continue...");
        }
        Console.ReadKey();
    }

    private void ViewHabits(bool clearAtStart = true, bool waitForInputAtTheEnd = true)
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
        if (waitForInputAtTheEnd)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(); 
        }
    }


    private void UpdateHabit()
    {
        Console.Clear();
        ViewHabits(false, false);
        Console.Write("Enter habit ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var habitTypes = _databaseManager.GetHabitTypes();
            Console.WriteLine("Available Habit Types:");
            ViewHabitTypes(false, false);

            Console.Write("Enter the ID of the habit type: ");
            if (int.TryParse(Console.ReadLine(), out int habitTypeId))
            {
                // Check if the habit type ID exists
                bool habitTypeExists = habitTypes.Exists(ht => ht.Id == habitTypeId);
                if (!habitTypeExists)
                {
                    Console.WriteLine("Habit type ID does not exist. Press any key to continue...");
                    Console.ReadKey();
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
                        Console.WriteLine("Habit updated successfully. Press any key to continue...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " Press any key to continue...");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid quantity. Press any key to continue...");
                }
            }
            else
            {
                Console.WriteLine("Invalid habit type ID. Press any key to continue...");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID. Press any key to continue...");
        }
        Console.ReadKey();
    }


    private void DeleteHabit()
    {
        Console.Clear();
        ViewHabits(false, false);
        Console.Write("Enter habit ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                _databaseManager.DeleteHabit(id);
                Console.WriteLine("Habit deleted successfully. Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Press any key to continue...");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID. Press any key to continue...");
        }
        Console.ReadKey();
    }
}
