﻿using Microsoft.Data.Sqlite;

namespace yashsachdev.HabitTracker;
/// <summary>
/// Display actions 
/// </summary>
public class DisplayClass
{
    public DisplayClass() { }
    public bool LoggedIn { get; set; }
    private int HabitId { get; set; }
    private int UserId { get; set; }
    public string Email { get; set; }
    public void Register()
    {
        string name = UserInput.GetName();
        Email = UserInput.GetEmail();
        string password = UserInput.GetPassword();
        if(UserInput.ValidateName(name) == false || UserInput.ValidateEmail(Email) == false)
        {
            Console.WriteLine("Plase enter correct details");
            return;
        }
        UserRepo userRepo = new UserRepo();
        var emailCount = userRepo.CountofUser(Email);
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

    public void Login()
    {
        UserRepo userRepo = new UserRepo();
        Email = UserInput.GetEmail();
        if (userRepo.CountofUser(Email) == 0)
            {
                Console.WriteLine("Email doesnt exist Register First");
                return;
            }
        string password;
        int i = 0;
        do
        {
            if(i>0)
            {
                Console.WriteLine("Wrong Password. Please Try Again");
            }
            i++;
            password = UserInput.GetPassword();
        } while (!userRepo.CheckPassword(Email,password));
        Console.WriteLine("Password is correct");

        Console.WriteLine("Login successful.");
        LoggedIn = true;
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
            catch (Exception ex)
            {
                Console.WriteLine("Invalid input"+ex.Message);
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
        HabitRepo habit = new HabitRepo();
        UserRepo userRepo = new UserRepo();
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            Console.WriteLine("Enter habit Name you want to update");
            string habitname = UserInput.GetHabitName();
            UserId = userRepo.GetIdFromEmail(Email);
            if(UserInput.ValidateName(habitname) == false)
            {
                Console.WriteLine("Please enter correct details");
                return;
            }
            var habitCount = habit.CountofHabit(Email, habitname);
            if (habitCount == 0)
            {
                Console.WriteLine("Habit not found. Please Insert Habit first.");
                return;
            }
            if (!habitEnrollRepo.CheckIfHabitExistsForUser(habitname, UserId))
            {
                Console.WriteLine("You do not have a habit with that name. Please try again.");
                return;
            }
                Console.WriteLine($"Enter the values you want to update");
                string resunit = UserInput.GetUnitMeasurement();
                string habitUnit = UserInput.GetHabitUnit(resunit);
                habitEnrollRepo.UpdateUserHabit(Email, habitname, habitUnit);
                habitEnrollRepo.DisplayUserHabit(Email);
        }
    }
    private void DeleteHabit() 
    {
        HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
        HabitRepo habitRepo = new HabitRepo();  
        string habitname = UserInput.GetHabitName();
        if (!habitEnrollRepo.CheckIfHabitExistsForUser(habitname, UserId))
        {
            Console.WriteLine("You do not have a habit with that name. Please try again.");
            return;
        }
        habitRepo.DeleteHabit(Email, habitname);
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
        UserRepo userRepo = new UserRepo();
        string name = userRepo.GetNameFromEmail(Email);
        habitEnrollRepo.DisplayUserHabit(Email);
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    private void InsertHabit()
    {
        string habitName = UserInput.GetHabitName();
        if (UserInput.ValidateName(habitName) == false) 
        { Console.WriteLine("Please enter valid habit"); 
            return; 
        }
        HabitRepo habitRepo = new HabitRepo();
        UserRepo userRepo = new UserRepo();
        HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
        string unit = UserInput.GetUnitMeasurement();
        string habitUnit = UserInput.GetHabitUnit(unit);
        Habit habit = new Habit
        {
            Habit_Name = habitName,
            Unit = habitUnit
        };
        if (userRepo.GetIdFromEmail(Email) == 0)
        {
            Console.WriteLine("User with email '{0}' not found.", Email);
            return;
        }
        UserId = userRepo.GetIdFromEmail(Email);
        Console.WriteLine(habitEnrollRepo.CheckIfHabitExistsForUser(habitName, UserId));
        if (habitEnrollRepo.CheckIfHabitExistsForUser(habitName, UserId))
        {
            Console.WriteLine("Habit '{0}' already exists for user with email '{1}'.", habitName, Email);
            return;
        }
        habitRepo.save(habit);
        userRepo = new UserRepo();
        UserId = userRepo.GetIdFromEmail(Email);
        HabitId = habitRepo.GetLastInsertedId();
        Console.WriteLine("Habit created successfully");
        Console.WriteLine("-----------------------");
        var startDateInput = UserInput.GetStartDate();
        HabitEnroll habitEnroll = new HabitEnroll
        {
            User_Id = UserId,
            Habit_Id = HabitId,
            Date = startDateInput,
        };
        habitEnrollRepo.Save(habitEnroll);
        Console.WriteLine("User enrolled in habit successfully");
    }
}
