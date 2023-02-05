// See https://aka.ms/new-console-template for more information
using yashsachdev.HabitTracker;

DatabaseClass.CreateDatabase();
DatabaseClass.CreateTable();
DisplayClass app = new DisplayClass();
while (!app.LoggedIn)
{
    Console.WriteLine("Welcome to the Habit Tracker");
    Console.WriteLine("1. Register");
    Console.WriteLine("2. Login");
    Console.Write("Enter your choice: ");
    int choice = int.Parse(Console.ReadLine());
    switch (choice)
    {
        case 1:
            app.Register();
            break;
        case 2:
            app.Login();
            break;
    }
}
app.DisplayMenu();












/*Console.WriteLine("Enter Habit Name: ");
string habitName = Console.ReadLine();
Console.WriteLine("Enter Habit Unit: ");
string habitUnit = Console.ReadLine();

Habit habit = new Habit
{
    Habit_Name = habitName,
    Unit = habitUnit
};
HabitRepo habitRepo = new HabitRepo();  
habitRepo.save(habit);
int habitId = habitRepo.GetLastInsertedId();
Console.WriteLine("Habit created successfully");
Console.WriteLine("UserId:" + userId);
Console.WriteLine("HabitId:" + habitId);
Console.WriteLine("-----------------------");
Console.WriteLine("Enter Start Date (YYYY-MM-DD): ");
var startDateInput = Console.ReadLine();

HabitEnroll habitEnroll = new HabitEnroll
{
    User_Id = userId,
    Habit_Id = habitId,
    Date = DateTime.Parse(startDateInput),
};
HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
habitEnrollRepo.Save(habitEnroll);  
Console.WriteLine("User enrolled in habit successfully");*/
/*Console.WriteLine("---------------------");
Console.WriteLine("Enter Email id to retrieve User: ");
string emailId = Console.ReadLine();
user = userRepo.GetByEmail(emailId);
if (user != null)
{
    Console.WriteLine("User Id:" + user.User_Id);
    Console.WriteLine("User Name: " + user.Name);
    Console.WriteLine("User Email: " + user.Email);
    Console.WriteLine("User Password: " + user.Password);
}
else
{
    Console.WriteLine("User not found.");
}
Console.WriteLine("Enter habit name to retrieve: ");
string Habitname = Console.ReadLine();
habit = habitRepo.GetByHabitName(Habitname);
if (user != null)
{
    Console.WriteLine("Habit_id: " + habit.Habit_Id);
    Console.WriteLine("Habit: " + habit.Habit_Name);
    Console.WriteLine("Unit: " + habit.Unit);
}
else
{
    Console.WriteLine("habit not found.");
}
Console.WriteLine("Enter email id to to see all Habits");*/
/*var user_email=Console.ReadLine();
Console.WriteLine("Enter name to to see all Habits");
var user_name = Console.ReadLine();
habitEnrollRepo.DisplayUserHabit(user_name, user_name);*/



