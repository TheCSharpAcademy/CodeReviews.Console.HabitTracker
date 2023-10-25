using EditTableOperations;

namespace UserInput;

public class ClassUserInput
{
    public void GetUserInput()
    {
        Console.WriteLine("\n\n");
        Operations editTable = new Operations();

        bool closeApp = false;
        
        while (closeApp == false)
        {
            Console.WriteLine("What would you like to add to the table?");
            Console.WriteLine("Enter 0 to Close Application");
            Console.WriteLine("Enter 1 to View All Records");
            Console.WriteLine("Enter 2 to Insert Record");
            Console.WriteLine("Enter 3 to Delete Record");
            Console.WriteLine("Enter 4 to Update Record ");
            Console.WriteLine("Enter 5 to Change unit of measurement ");
            Console.WriteLine("Enter 6 to View a report ");

            Console.WriteLine("--------------------------------------------------");

            string userInput = Console.ReadLine();

            int cleanUserInput;
            // Parse input to ensure that it is valid
            while (!int.TryParse(userInput, out cleanUserInput))
            {
                Console.WriteLine("Please enter a valid number!");
                userInput = Console.ReadLine();
            }

            // Check to see if the number is greater than 4(valid selection)
            if (cleanUserInput > 6)
            {
                Console.WriteLine("\nInvalid selection please choose an option between 0 and 4. if statement version\n");
            }
            else
            {
                editTable.EditTableSwitch(cleanUserInput);

            }

            if(cleanUserInput == 0)
                closeApp = true;
                
        }

        
        
    }
}