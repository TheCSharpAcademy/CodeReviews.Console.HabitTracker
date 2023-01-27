namespace yashsachdev.HabitTracker;
/// <summary>
/// Display actions 
/// </summary>
public class DisplayClass
{
    DatabaseClass db = new DatabaseClass();
    public void UserLogin()
    {
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("Enter your choice:");
        string op = Console.ReadLine();
        switch (op)
        {
            case "1": db.ExistingUserLogin(); break;
            case "2": db.NewRegistration(); break;
            default: Console.WriteLine("you have entered a wrong choice");
                UserLogin();
                        break;
        }
    }
}
