// See https://aka.ms/new-console-template for more information
DatabaseClass.CreateDatabase();
DatabaseClass.CreateTable();
Console.WriteLine("Enter User Name: ");
string userName = Console.ReadLine();
Console.WriteLine("Enter User Email id: ");
string userEmail = Console.ReadLine();
Console.WriteLine("Enter User Password: ");
string userPass = Console.ReadLine();

User user = new User
{
    Name = userName,
    Password = userPass,
    Email = userEmail
};
UserRepo userRepo = new UserRepo();
userRepo.Save(user);
int userId = userRepo.GetLastInsertedId();
Console.WriteLine("User created successfully");
Console.WriteLine("Enter Habit Name: ");
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
Console.WriteLine("Enter Start Date (YYYY-MM-DD): ");
var startDateInput = Console.ReadLine();
Console.WriteLine("UserId:" + user.User_Id);
Console.WriteLine("HabitId:" + habit.Habit_Id);
HabitEnroll habitEnroll = new HabitEnroll
{
    User_Id = userId,
    Habit_Id = habitId,
    Date = DateTime.Parse(startDateInput),
};
HabitEnrollRepo habitEnrollRepo = new HabitEnrollRepo();
habitEnrollRepo.Save(habitEnroll);  
Console.WriteLine("User enrolled in habit successfully");
Console.WriteLine("---------------------");
Console.WriteLine("Enter Email id to retrieve User: ");
string emailId = Console.ReadLine();
user = userRepo.GetByEmail(emailId);
if (user != null)
{
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


