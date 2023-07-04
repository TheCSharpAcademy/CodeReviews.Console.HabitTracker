using HabitTracker.Mo3ses;

public class Program
{
    public static void Main(string[] args)
    {
        DbConn conn = new DbConn();
        conn.DbCreate();

        do
        {
            string inputValue;
            Menu.ShowMenu();
            Console.Write("OPTION: ");
            inputValue = Console.ReadLine();
            switch (inputValue)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    bool hasHabits = conn.GetHabits();
                    if (hasHabits)
                    {
                        Console.Write("CHOOSE HABIT ID YOU WANT TO RECORD: ");
                        inputValue = Console.ReadLine();
                        int trackId = ProgramHelpers.ValidateInputs(inputValue);
                        conn.TrackHabit(trackId);
                    }
                    break;
                case "2":
                    conn.GetHabits();
                    break;
                case "3":
                    Console.Write("CHOOSE HABIT NAME: ");
                    string habitName = Console.ReadLine();
                    Console.Write("CHOOSE THE MEASURE NAME(METERS, POUNDS, KILOMETERS, KILOGRAMS): ");
                    string habitMeasure = Console.ReadLine();
                    Console.Write("CHOOSE THE VALUE PER TRACK: ");
                    string habitValue = Console.ReadLine();    
                    if (!string.IsNullOrEmpty(habitName) && !string.IsNullOrEmpty(habitMeasure) && !(ProgramHelpers.ValidateInputs(habitValue) == 0)){
                        conn.CreateHabit(habitName, habitMeasure, ProgramHelpers.ValidateInputs(habitValue));
                    }
                    else
                    {
                        Console.WriteLine("ERROR: VERIFY THESE CONDITIONS NAMES CANT BE EMPTY AND THE VALUE CANT BE 0");
                    }
                    
                    break;
                case "4":
                    bool hasHabitsToUpdate = conn.GetHabits();
                    if (hasHabitsToUpdate)
                    {
                        Console.Write("CHOOSE THE HABIT ID TO CHANGE: ");
                        inputValue = Console.ReadLine();
                        int updateId = ProgramHelpers.ValidateInputs(inputValue);
                        if (updateId != 0)
                        {
                            Console.Write("CHOOSE THE NEW HABIT NAME: ");
                            string newHabitName = Console.ReadLine();
                            Console.Write("CHOOSE THE NEW HABIT MEASURE: ");
                            string newHabitMeasure = Console.ReadLine();
                            Console.Write("CHOOSE THE  NEW VALUE PER TRACK:");
                            string newHabitValue = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newHabitName) && !string.IsNullOrEmpty(newHabitMeasure) &&!(ProgramHelpers.ValidateInputs(newHabitValue) == 0))
                            {
                                conn.UpdateHabit(updateId, newHabitName, newHabitMeasure, ProgramHelpers.ValidateInputs(newHabitValue));
                            }
                            else
                            {
                                Console.WriteLine("ERROR: VERIFY THESE CONDITIONS NAMES CANT BE EMPTY AND THE VALUE CANT BE 0");
                            }
                            
                        }

                    }
                    break;
                case "5":
                    bool hasHabitsToDelete = conn.GetHabits();
                    if (hasHabitsToDelete)
                    {
                        Console.Write("CHOOSE THE HABIT ID YOU WANT TO DELETE: ");
                        inputValue = Console.ReadLine();
                        int deleteId = ProgramHelpers.ValidateInputs(inputValue);
                        if (deleteId != 0)
                        {
                            conn.DeleteHabit(deleteId);
                        }
                    }
                    break;
                    case "6":
                    bool hasHabitReport = conn.GetHabits();
                    if (hasHabitReport)
                    {
                        Console.Write("CHOSE THE HABIT ID YOU WANT TO VIEW THE REPORT: ");
                        inputValue = Console.ReadLine();
                        int habitReportId = ProgramHelpers.ValidateInputs(inputValue);
                        if (habitReportId != 0)
                        {
                            conn.HabitReport(habitReportId);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("PLEASE TYPE A NUMBER OF THIS LIST");
                    break;
            }
        } while (true);



    }
}