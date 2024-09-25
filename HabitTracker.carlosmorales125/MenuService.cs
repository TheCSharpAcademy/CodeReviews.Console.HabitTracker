namespace HabitTracker.carlosmorales125;

public static class MenuService
{
    public static void PresentWelcomeMessage()
    {
        Console.WriteLine("***************************************");
        Console.WriteLine("*      Welcome to Habit Tracker!      *");
        Console.WriteLine("***************************************");
    }
    
    public static void PresentGoodbyeMessage()
    {
        Console.WriteLine("***************************************");
        Console.WriteLine("*      Goodbye! Thanks for using      *");
        Console.WriteLine("*          Habit Tracker!             *");
        Console.WriteLine("***************************************");
    }
    
    public static MenuChoice PresentMenu()
    {
        while (true)
        {
            Console.WriteLine("Menu: -----------------------------------");
            Console.WriteLine("1. Show all logged habits");
            Console.WriteLine("2. Add a habit");
            Console.WriteLine("3. Delete a habit");
            Console.WriteLine("4. Update a habit");
            Console.WriteLine("5. Exit");
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter an option: ");

            if (int.TryParse(Console.ReadLine(), out var option))
            {
                switch (option)
                {
                    case 1: return MenuChoice.ShowAllHabits;
                    case 2: return MenuChoice.AddHabit;
                    case 3: return MenuChoice.DeleteHabit;
                    case 4: return MenuChoice.UpdateHabit;
                    case 5: return MenuChoice.Exit;
                }
            }

            Console.WriteLine("Invalid option. Please try again.");
        }
    }

    private static string PresentPossibleHabitsMenu()
    {
        while (true)
        {
            Console.WriteLine("Possible Habits: ------------------------");
            Console.WriteLine("1. Exercise");
            Console.WriteLine("2. Reading");
            Console.WriteLine("3. Meditation");
            Console.WriteLine("4. Coding");
            Console.WriteLine("5. Writing");
            Console.WriteLine("6. Drawing");
            Console.WriteLine("7. Water Intake");
            Console.WriteLine("8. Healthy Eating");
            Console.WriteLine("9. Sleep");
            Console.WriteLine("10. Learning");
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter a habit: ");

            if (int.TryParse(Console.ReadLine(), out var option))
            {
                switch (option)
                {
                    case 1: return "Exercise";
                    case 2: return "Reading";
                    case 3: return "Meditation";
                    case 4: return "Coding";
                    case 5: return "Writing";
                    case 6: return "Drawing";
                    case 7: return "Water Intake";
                    case 8: return "Healthy Eating";
                    case 9: return "Sleep";
                    case 10: return "Learning";
                }
            }

            Console.WriteLine("Invalid option. Please try again.");
        }
    }
    
    private static int PresentQuantityMenu()
    {
        while (true)
        {
            Console.Write("Enter quantity: ");

            if (int.TryParse(Console.ReadLine(), out var quantity))
            {
                return quantity;
            }

            Console.WriteLine("Invalid quantity. Please try again.");
        }
    }
    
    private static DateTime PresentDateMenu()
    {
        while (true)
        {
            Console.Write("Enter date (yyyy-MM-dd): ");
            
            if (DateTime.TryParse(Console.ReadLine(), out var date))
            {
                return date;
            }
            
            Console.WriteLine("Invalid date. Please try again.");
        }
    }
    
    public static Habit PresentAddHabitMenu()
    {
        var name = PresentPossibleHabitsMenu();
        var quantity = PresentQuantityMenu();
        var date = PresentDateMenu();
        
        return new Habit
        {
            Name = name,
            Quantity = quantity,
            Date = date
        };
    }
    
    public static int PresentGetIdMenu()
    {
        while (true)
        {
            Console.Write("Enter habit id: ");
            
            if (int.TryParse(Console.ReadLine(), out var id))
            {
                return id;
            }
            
            Console.WriteLine("Invalid id. Please try again.");
        }
    }
    
    public static Habit PresentUpdateHabitMenu()
    {
        var id = PresentGetIdMenu();
        var name = PresentPossibleHabitsMenu();
        var quantity = PresentQuantityMenu();
        var date = PresentDateMenu();
        
        return new Habit
        {
            Id = id,
            Name = name,
            Quantity = quantity,
            Date = date
        };
    }
}