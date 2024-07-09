// See https://aka.ms/new-console-template for more information
using csa_habit_logger;
using System.Configuration;

bool exit = false;
dal.InitialiseDatabase();
dal.SeedDatabase();

while (!exit)
{
    Console.Clear();
    Console.WriteLine("Habit Tracker");
    Console.WriteLine("R - Add, view, edit and delete logged records");
    Console.WriteLine("H - Add, view, edit and delete habits");
    Console.WriteLine("Q - Quit");

    ConsoleKeyInfo key = Console.ReadKey();

    switch (key.KeyChar)
    {
        case 'r':
        case 'R':
            HabitRecordsSubMenu();
            break;
        case 'H':
        case 'h':
            HabitsSubMenu();
            break;
        case 'q':
        case 'Q':
        case '0':
            Console.Clear();
            Console.WriteLine("Quit");
            exit = true;
            break;
        default:
            break;
    }
}

public partial class Program
{
    static string sqlDatabaseFileName = ConfigurationManager.AppSettings["SQLiteConnectionFileName"] ?? String.Empty;
    static string connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnectionString"]
                                                    .ConnectionString.Replace(ConfigurationManager.AppSettings["dbNamePlaceHolder"] ?? string.Empty,
                                                                               sqlDatabaseFileName);

    static HabitSqLiteDal dal = new HabitSqLiteDal(sqlDatabaseFileName, connectionString);


    public static int GetUserIntResponse()
    {
        bool success = false;
        int val = 0;

        while (!success)
        {
            string? resp = Console.ReadLine();
            if (resp != null)
            {
                success = Int32.TryParse(resp, out val);
            }
        }

        return val;
    }

    public static string GetUserStringResponse()
    {
        string? resp = Console.ReadLine();

        while (String.IsNullOrEmpty(resp))
        {
            resp = Console.ReadLine();
        }

        return resp.Trim();
    }

    public static bool GetUserBoolResponse()
    {
        string? resp = string.Empty;

        while (String.IsNullOrEmpty(resp))
        {
            resp = Console.ReadLine();

            if (resp is not null)
            {
                resp = resp.ToLower().Trim();
                if (resp.Equals("yes") || resp.Equals("y"))
                {
                    return true;
                }

                if (resp.Equals("no") || resp.Equals("n"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static void AddAHabit()
    {
        Console.Clear();

        Console.WriteLine("Add a new habit");

        Console.WriteLine("Enter Habit name");
        string name = GetUserStringResponse();

        if (name.Equals("0"))
        {
            return;
        }

        Console.WriteLine("Enter Habit unit");
        string unit = GetUserStringResponse();

        if (dal.Add(new NewHabit(name, unit)))
        {
            Console.WriteLine("Successfully added new habit");
        }
        else
        {
            Console.WriteLine("failed to add new habit");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static void ViewHabits()
    {
        Console.Clear();
        Console.WriteLine("View habits");

        List<Habit> habits = dal.GetAll();

        foreach (var habit in habits)
        {
            Console.WriteLine(habit.ToString());
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static void UpdateAHabit()
    {
        bool continueEditing = true;

        while (continueEditing)
        {
            Console.Clear();
            Console.WriteLine("Update habit");
            Console.WriteLine("Enter ID to update. (Enter 0 to exit)");
            int id = GetUserIntResponse();

            if (id == 0) break;

            Habit? habit = dal.Read(id);

            if (habit is null)
            {
                Console.WriteLine("Habit ID does not exist");
                return;
            }

            Console.WriteLine($"Edit this habit? {habit.ToString()}");
            Console.WriteLine("Enter y/n?");
            if (!GetUserBoolResponse()) continue;

            Console.Write($"Enter new name [currently '{habit.Name}']: ");
            string name = GetUserStringResponse();

            Console.Write($"Enter new unit [currently '{habit.Unit}']: ");
            string unit = GetUserStringResponse();

            habit.Name = String.IsNullOrEmpty(name) ? habit.Name: name;
            habit.Unit = String.IsNullOrEmpty(unit) ? habit.Unit : unit;

            bool updated = dal.Update(id, habit);

            Console.WriteLine("Habit update{0}", updated ? "d" : " failed");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    public static void DeleteAHabit()
    {
        Console.Clear();
        Console.WriteLine("Delete habit");
        Console.WriteLine("Enter ID to delete. (Enter 0 to exit)");
        int id = GetUserIntResponse();

        if (id == 0) return;

        if (dal.Delete(id))
        {
            Console.WriteLine("Found and deleted habit");
        }
        else
        {
            Console.WriteLine("Habit not found");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static void AddAHabitInstance(DateTime? inputDateTime)
    {
        Console.Clear();

        Console.WriteLine("Add a new record");

        Console.WriteLine("Enter Habit name");
        string name = GetUserStringResponse();

        if (name.Equals("0"))
        {
            return;
        }

        DateTime dt = DateTime.Now;
        bool timeOk = true;

        if (inputDateTime is null)
        {
            Console.WriteLine("Enter date & time (YYYY-MM-DD HH:MM:SS)");
            string dtime = GetUserStringResponse();
            timeOk = DateTime.TryParse(dtime, out dt);
        }
        else
        {
            dt = inputDateTime.Value;
        }

        Console.Write("Enter amount: ");
        int amount = GetUserIntResponse();

        if (!timeOk)
        {
            Console.WriteLine("Could not parse time");
        }
        else
        {
            Habit? habit = dal.GetHabitByName(name);
            bool created = false;

            if (habit is not null)
            {
                created = dal.AddInstance(new NewHabitRecord(habit, dt, amount));
            }

            if (created)
            {
                Console.WriteLine("Successfully added new habit");
            }
            else
            {
                Console.WriteLine("failed to add new habit");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static void UpdateAHabitInstance()
    {
        bool continueEditing = true;

        while (continueEditing)
        {
            Console.Clear();
            Console.WriteLine("Update record");
            Console.WriteLine("Enter ID to update. (Enter 0 to exit)");
            int id = GetUserIntResponse();

            if (id == 0) break;

            HabitRecord? record = dal.ReadInstance(id);

            if (record is null)
            {
                Console.WriteLine("Record does not exist");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Edit this instance? {record.ToString()}");
            Console.WriteLine("Enter y/n?");
            if (!GetUserBoolResponse()) continue;

            Console.Write($"Enter new time [currently '{record.DateTime.ToString()}']: ");
            string time = GetUserStringResponse();

            DateTime dt = record.DateTime;

            bool updated = true;

            if (DateTime.TryParse(time, out dt) && dt != record.DateTime)
            {
                record.DateTime = String.IsNullOrEmpty(time) ? record.DateTime : dt;
            }

            Console.Write($"Enter new amount [currently '{record.Amount}']: ");
            record.Amount = GetUserIntResponse();

            updated = dal.UpdateInstance(id, record);

            Console.WriteLine("Habit update{0}", updated ? "d" : " failed");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    public static void DeleteAHabitInstance()
    {
        Console.Clear();
        Console.WriteLine("Delete record");
        Console.WriteLine("Enter ID to delete. (Enter 0 to exit)");
        int id = GetUserIntResponse();

        if (id == 0) return;

        if (dal.Delete(id))
        {
            Console.WriteLine("Found and deleted record");
        }
        else
        {
            Console.WriteLine("record not found");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static void ViewHabitInstances()
    {
        Console.Clear();
        Console.WriteLine("View records");

        List<HabitRecord> habits = dal.GetAllInstances();

        if (habits.Count == 0)
        {
            Console.WriteLine("No records to show");
        }
        else
        {
            foreach (var habit in habits)
            {
                Console.WriteLine(habit.ToString());
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static void HabitsSubMenu()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Habits");
            Console.WriteLine("C - Add Habit");
            Console.WriteLine("R - View Habits");
            Console.WriteLine("U - Update Habit");
            Console.WriteLine("D - Delete Habit");
            Console.WriteLine("Q - Back to main menu");

            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.KeyChar)
            {
                case 'c':
                case 'C':
                    AddAHabit();
                    break;
                case 'd':
                case 'D':
                    DeleteAHabit();
                    break;
                case 'r':
                case 'R':
                    ViewHabits();
                    break;
                case 'u':
                case 'U':
                    UpdateAHabit();
                    break;
                case 'q':
                case 'Q':
                case '0':
                    Console.Clear();
                    exit = true;
                    break;
                default:
                    break;
            }
        }
    }

    public static void HabitRecordsSubMenu()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Habit Tracker");
            Console.WriteLine("C - Log habit");
            Console.WriteLine("N - Log habit now");
            Console.WriteLine("R - View habit logs");
            Console.WriteLine("U - Update habit log");
            Console.WriteLine("D - Remove a habit log record");
            Console.WriteLine("Q - Return to main menu");

            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.KeyChar)
            {
                case 'c':
                case 'C':
                    AddAHabitInstance(null);
                    break;
                case 'n':
                case 'N':
                    AddAHabitInstance(DateTime.Now);
                    break;
                case 'r':
                case 'R':
                    ViewHabitInstances();
                    break;
                case 'u':
                case 'U':
                    UpdateAHabitInstance();
                    break;
                case 'd':
                case 'D':
                    DeleteAHabitInstance();
                    break;
                case 'q':
                case 'Q':
                case '0':
                    Console.Clear();
                    Console.WriteLine("Quit");
                    exit = true;
                    break;
                default:
                    break;
            }
        }
    }

}