using System.Linq.Expressions;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("________________________________");
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("Choose from the options below:");
        Console.WriteLine("Press 1 to View Records");
        Console.WriteLine("Press 2 to Insert Record");
        Console.WriteLine("Press 3 to Delete Record");
        Console.WriteLine("Press 4 to Insert Record");
        Console.WriteLine("Press 0 to Quit the application");
        Console.WriteLine("________________________________");
        string input = Console.ReadLine();
        MenuChoice(input);
    }
    public static void MenuChoice(string message)
    {
        string nextTry;
        if (!String.IsNullOrEmpty(message))
        {
            switch (message[0])
            {
                case '4':
                    Console.WriteLine("You chose 4");
                    break;
                case '3':
                    Console.WriteLine("You chose 3");
                    break;
                case '2':
                    Console.WriteLine("You chose 2");
                    break;
                case '1':
                    Console.WriteLine("You chose 1");
                    break;
                case '0':
                    Console.WriteLine("You chose 0");
                    break;
                default:
                    
                    Console.WriteLine("Invalid input, try again!");
                    nextTry = Console.ReadLine();
                    MenuChoice(nextTry);
                    break;
            }

        }
        else
        {
            Console.WriteLine("Wrong input, try again by typing in a number of the menu option you'd like to do:");
            nextTry = Console.ReadLine();
            MenuChoice(nextTry);
        }
        
    }
}