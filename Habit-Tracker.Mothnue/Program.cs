namespace Mothnue.Program
{
    class Client
    {
        private static void Main()
        {
            Client menu = new Client();
            bool application = true;

            while (application)
            {
                int response = menu.MainMenu();
                switch (response)
                {
                    case 0:
                        application = false;
                        break;
                    case 1:
                        // View all Records
                        break;
                    case 3:
                        // Delete a Record
                        break;
                    case 4:
                        // Update a Record
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private int MainMenu()
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("What would you like to do ?");

            Console.WriteLine("0 - Close");
            Console.WriteLine("1 - View all Records");
            Console.WriteLine("3 - Delete a Record");
            Console.WriteLine("4 - Update a Record");

            int userInput;
            while (!int.TryParse(Console.ReadLine(), out userInput))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }

            return userInput;
        }
    }
}
