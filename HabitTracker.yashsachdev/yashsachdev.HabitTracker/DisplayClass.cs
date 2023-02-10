using Microsoft.Data.Sqlite;

namespace yashsachdev.HabitTracker;
/// <summary>
/// Display actions 
/// </summary>
public class DisplayClass
{
    public DisplayClass(){}
    public bool LoggedIn { get; set; }
    private int HabitId { get; set; }
    private int UserId { get; set; }
    public string Email { get; set; }
    public void Register()
    {
        string name = UserInput.GetName();
        Email = UserInput.GetEmail();
        string password = UserInput.GetPassword();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Invalid inputs. Name, email and password are required.");
                return;
            }
            SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM User WHERE Email = @email", cnn);
            cmd.Parameters.AddWithValue("@email", Email);
            var emailCount = (Int64)cmd.ExecuteScalar();
            if (emailCount > 0)
            {
                Console.WriteLine("Email already exists. Please Login.");
                return;
            }
            User user = new User
            {
                Name = name,
                Email = Email,
                Password = password
            };
            UserRepo userRepo = new UserRepo();
            try
            {
                userRepo.Save(user);
                UserId = userRepo.GetIdFromEmail(Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("User created successfully");
        }
    }
    public void Login()
    {
        Email = UserInput.GetEmail();
        string password = UserInput.GetPassword();

        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Invalid inputs. Email and password are required.");
                return;
            }
            SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM User WHERE Email = @email", cnn);
            cmd.Parameters.AddWithValue("@email", Email);
            var emailCount = (Int64)cmd.ExecuteScalar();
            if (emailCount == 0)
            {
                Console.WriteLine("Email not found. Please register first.");
                return;
            }
            cmd = new SqliteCommand("SELECT Password FROM User WHERE Email = @email", cnn);
            cmd.Parameters.AddWithValue("@email", Email);
            string correctPassword = (string)cmd.ExecuteScalar();
            if (password != correctPassword)
            {
                Console.WriteLine("Incorrect password. Please try again.");
                return;
            }
            Console.WriteLine("Login successful.");
            LoggedIn = true;
        }
    }
    public void DisplayMenu()
    {
        Console.WriteLine("Login Successful!!!");
        while (LoggedIn)
        {
            Console.WriteLine("1. Insert habit");
            Console.WriteLine("2. Update habit");
            Console.WriteLine("3. Delete habit");
            Console.WriteLine("4. View habits");
            Console.WriteLine("5. Generate Report");
            Console.WriteLine("6. Logout");
            Console.Write("Enter your choice: ");
            try
            {
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        InsertHabit();
                        break;
                    case 2:
                        UpdateHabit();
                        break;
                    case 3:
                        DeleteHabit();
                        break;
                    case 4:
                        ViewHabits();
                        break;
                    case 5:
                        GenerateReport();
                        break;
                    case 6:
                        Logout();
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Invalid input");
            }
        }
    }
    private void GenerateReport()
    {
        HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
        habitEnrollRepo.GenerateReport(Email);
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    private void UpdateHabit()
    {
        HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            SqliteCommand cmd = new SqliteCommand("SELECT Name FROM User WHERE Email = @email", cnn);
            cmd.Parameters.AddWithValue("@email", Email);
            string name = (string)cmd.ExecuteScalar();
            Console.WriteLine("Enter habit Name you want to update");
            string habitname = UserInput.GetHabitName();
            SqliteCommand cmd1 = new SqliteCommand("SELECT COUNT(*) FROM Habit WHERE Habit_Name = @habitname", cnn);
            cmd1.Parameters.AddWithValue("@habitname", habitname);
            var habitCount = (Int64)cmd1.ExecuteScalar();
            if (habitCount == 0)
            {
                Console.WriteLine("Habit not found. Please Insert Habit first.");
                return;
            }
            Console.WriteLine($"Enter the values you want to update");
            string resunit = UserInput.GetUnitMeasurement();
            string habitUnit = UserInput.GetHabitUnit(resunit);
            habitEnrollRepo.UpdateUserHabit(name, Email, habitname, habitname, habitUnit);
            habitEnrollRepo.DisplayUserHabit(name, Email);
        }
    }
    private void DeleteHabit()
    {
        HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
        string habitname = UserInput.GetHabitName();
        habitEnrollRepo.DeleteHabit(Email, habitname);
        Console.WriteLine($"Deleted {habitname}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    private void Logout()
    {
        LoggedIn = false;
    }
    private void ViewHabits()
    {
        HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            SqliteCommand cmd = new SqliteCommand("SELECT Name FROM User WHERE Email = @email", cnn);
            cmd.Parameters.AddWithValue("@email", Email);
            string name = (string)cmd.ExecuteScalar();
            habitEnrollRepo.DisplayUserHabit(name, Email);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
    private void InsertHabit()
    {
        string habitName = UserInput.GetHabitName();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM Habit WHERE Habit_Name = @habitname", cnn);
            cmd.Parameters.AddWithValue("@habitname", habitName);
            var habitCount = (Int64)cmd.ExecuteScalar();
            if (habitCount > 0)
            {
                Console.WriteLine("Habit already exist.Goto to update habit to edit it");
                return;
            }
            string unit = UserInput.GetUnitMeasurement();
            string habitUnit = UserInput.GetHabitUnit(unit);
            Habit habit = new Habit
            {
                Habit_Name = habitName,
                Unit = habitUnit
            };
            HabitRepo habitRepo = new HabitRepo();
            habitRepo.save(habit);
            UserRepo userRepo = new UserRepo();
            UserId = userRepo.GetIdFromEmail(Email);
            HabitId = habitRepo.GetLastInsertedId();
            Console.WriteLine("Habit created successfully");
            Console.WriteLine("UserId:" + UserId);
            Console.WriteLine("HabitId:" + HabitId);
            Console.WriteLine("-----------------------");
            var startDateInput = UserInput.GetStartDate();
            HabitEnroll habitEnroll = new HabitEnroll
            {
                User_Id = UserId,
                Habit_Id = HabitId,
                Date = startDateInput,
            };
            HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
            habitEnrollRepo.Save(habitEnroll);
            Console.WriteLine("User enrolled in habit successfully");
        }
    }
}
