using System.Globalization;
using UserInput;

namespace DateInput;

public class UserDateInput
{
    ClassUserInput rtnToMainMenu = new ClassUserInput();
    internal string getDate(string message)
    {
        Console.WriteLine(message);

        string dateInput = Console.ReadLine();

        if(dateInput == "0") rtnToMainMenu.GetUserInput();

        while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\nInvalid date format (Format: dd-mm-yy). Enter 0 to return to main menu or try again.");
            dateInput = Console.ReadLine();
            if(dateInput == "0") rtnToMainMenu.GetUserInput();
        }
        

        return dateInput;
    }
}