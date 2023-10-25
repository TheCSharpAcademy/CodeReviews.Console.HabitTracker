using UserInput;

namespace NumberInput;

public class GetNumberInput
{
    ClassUserInput rtnToMainMenu = new ClassUserInput();
    internal int GetNumInput(string message)
    {
        Console.WriteLine(message);
        string inputNum = Console.ReadLine();
        
        if(inputNum == "0") rtnToMainMenu.GetUserInput();

        while(!Int32.TryParse(inputNum, out _) || Convert.ToInt32(inputNum) < 0)
        {
            Console.WriteLine("\nInvalid number, enter again\n");
            inputNum = Console.ReadLine();
            if(inputNum == "0") rtnToMainMenu.GetUserInput();
        }

        int cleanInput = Convert.ToInt32(inputNum);

        return cleanInput;
    }
}