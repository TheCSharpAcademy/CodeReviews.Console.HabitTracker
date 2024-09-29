namespace HabitTracker;
using HabitTrackerLibrary;
public static class Program
{
    public static void Main()
    {
        Console.Clear();
        Console.WriteLine("===============================================");
        Console.WriteLine("=========== Welcome to HabitTracker ===========");
        Console.WriteLine("===============================================");
        HabitDatabase.CreateDatabase();   
        var isRunning = true;
        while (isRunning)
        {
            Menu.ShowOptions();
            Options option = Menu.GetOptions();
            switch (option)
            {
                case Options.ViewHabits:
                    HabitDatabase.DisplayHabitsHistory();
                    break;
                case Options.LogHabit:
                    LogHabit();
                    break;
                case Options.DeleteHabit:
                    //DeleteHabitLog()
                    break;
                case Options.UpdateHabit:
                    //UpdateHabits()
                    break;
                case Options.CreateNewHabits:
                    //ManageHabits()
                    break;
                case Options.Help:
                    Menu.ShowHelp();
                    break;
                case Options.Exit:
                    isRunning = false;
                    break;
            }
        }

    }

    private static void LogHabit()
    {
        Console.Clear();
        Console.WriteLine("--Log Habit--");
        Console.WriteLine("--Choose Habit Type--");
        var habitTypeNames = HabitDatabase.HabitTypes.Keys.ToList();

        for (int i = 0; i < habitTypeNames.Count; i++)
        {
            Console.WriteLine($"{i+1}->  {habitTypeNames[i]}");
        }
        
        //Gets habit type
        Console.WriteLine("Enter Choice's num:");
        int option;
        do
        {
            string? readLine = Console.ReadLine();
            int.TryParse(readLine, out option);
        } while (option < 1 || option > habitTypeNames.Count);
        string type = habitTypeNames[option - 1];
        
        //Gets Habit date log
        Console.WriteLine("press Enter to get to use current data or enter date with format YYYY-MM-DD");
        string date="";
        do
        {
            string? readLine = Console.ReadLine();
            if (String.IsNullOrEmpty(readLine))
            {
                date = DateTime.Now.ToString("yyyy-MM-dd");
                break;
            }
            else
            {
                if (DateTime.TryParse(readLine,out DateTime dateTime))
                {
                    date = dateTime.ToString("yyyy-MM-dd");
                    break;
                }
            }
        } while (true);
        //Get Amount
        string? quantity = null;
        if (HabitDatabase.HabitTypes[type] != null)
        {
            Console.WriteLine($"Enter Amount ##-{HabitDatabase.HabitTypes[type].Unit}");
            do
            {
                var readline = Console.ReadLine();
                if (int.TryParse(readline, out int amount))
                {
                    quantity = new Habit.Amount(amount,HabitDatabase.HabitTypes[type].Unit).ToString();
                    Console.WriteLine(quantity);
                    Console.ReadLine();
                    break;
                }
            } while (true);
        }
        Habit habit;
        try
        {
            habit = new Habit(type, date, quantity);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Making Habit : "+ex.Message);
            return;
        }
        
        HabitDatabase.AddHabit(habit);
        Console.WriteLine($"Habit added successfully - {habit.ToString()}");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();


    }
}
