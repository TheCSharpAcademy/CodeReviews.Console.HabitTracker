using HabitTracker.Mo3ses;
using static System.Data.Entity.Infrastructure.Design.Executor;

public class Program
{
    public static void Main(string[] args)
    {
        DbConn conn = new DbConn();
        conn.DbCreate();

        do
        {
            string inputValue;
            bool habitChecker = false;
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

                        habitChecker = conn.HasHabit(trackId);
                        if (habitChecker) conn.TrackHabit(trackId);
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
                    if (!string.IsNullOrEmpty(habitName) && !string.IsNullOrEmpty(habitMeasure)){
                        conn.CreateHabit(habitName, habitMeasure);
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
                        habitChecker = conn.HasHabit(updateId);
                        if (updateId != 0 && habitChecker == true)
                        {
                            Console.Write("CHOOSE THE NEW HABIT NAME: ");
                            string newHabitName = Console.ReadLine();
                            Console.Write("CHOOSE THE NEW HABIT MEASURE: ");
                            string newHabitMeasure = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newHabitName) && !string.IsNullOrEmpty(newHabitMeasure) )
                            {
                                conn.UpdateHabit(updateId, newHabitName, newHabitMeasure);
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
                        habitChecker = conn.HasHabit(deleteId);
                        if (deleteId != 0 && habitChecker == true)
                        {
                            conn.DeleteHabit(deleteId);
                        }
                    }
                    break;


                    case "6":
                    
                    Menu.ShowReportMenu();
                    inputValue = Console.ReadLine();
                    int habitReportMenu = ProgramHelpers.ValidateInputs(inputValue);
                   
                    bool hasHabitReport = conn.GetHabits();
                    
                    if (hasHabitReport)
                    {
                        

                        switch (habitReportMenu)
                        {
                            case 0:
                                break;
                            case 1:
                                Console.Write("CHOOSE THE HABIT ID YOU WANT TO VIEW THE REPORT: ");

                                inputValue = Console.ReadLine();
                                int habitReportId = ProgramHelpers.ValidateInputs(inputValue);
                                habitChecker = conn.HasHabit(habitReportId);
                                if (habitReportId != 0 && habitChecker == true)
                                {
                                    conn.HabitReport(habitReportId);
                                }
                                break;
                             case 2:
                                Console.Write("CHOOSE THE HABIT ID YOU WANT TO VIEW THE REPORT: ");
                                inputValue = Console.ReadLine();
                                int habitIntervalReportId = ProgramHelpers.ValidateInputs(inputValue);
                                habitChecker = conn.HasHabit(habitIntervalReportId);
                               
                                if (habitIntervalReportId != 0 && habitChecker == true)
                                {
                                    Console.Write("CHOOSE START DATE (YYYY-MM-DD): ");
                                    string habitStartDate = Console.ReadLine();


                                    Console.Write("CHOOSE THE END DATE (YYYY-MM-DD): ");
                                    string habitEndDate = Console.ReadLine();
                                     

                                    conn.HabitReportInterval(habitIntervalReportId, habitStartDate, habitEndDate);
                                }
                                break;
                            default:
                                break;
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