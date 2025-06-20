using Microsoft.Data.Sqlite;

class Program
{
    static void Main()
    {
        bool menuActive = true;

        System.Console.WriteLine("Welcome to the Habit Tracker");
        System.Console.WriteLine("Select an option:\n");

        while (menuActive)
        {

            System.Console.WriteLine("0: Exit Program");
            System.Console.Write("Your selection: ");

            string input = System.Console.ReadLine();

            switch (input)
            {
                case "0":
                    menuActive = false;
                    System.Console.WriteLine("Goodbye");
                    break;
            }

            // Menu header presented after any input
            System.Console.WriteLine("\nSelect an option: \n");
        }
    }
}