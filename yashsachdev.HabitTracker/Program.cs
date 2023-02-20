// See https://aka.ms/new-console-template for more information
DatabaseClass.CreateDatabase();
DatabaseClass.CreateTable();
DisplayClass app = new DisplayClass();
while (!app.LoggedIn)
{
    Console.WriteLine("Welcome to the Habit Tracker");
    Console.WriteLine("1. Register");
    Console.WriteLine("2. Login");
    Console.Write("Enter your choice: ");
    try
    {
        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                app.Register();
                break;
            case 2:
                app.Login();
                break;
            default:
                Console.WriteLine("Incorrect choice pls try again");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Invalid input");
    }

}
Console.Clear();
app.DisplayMenu();


