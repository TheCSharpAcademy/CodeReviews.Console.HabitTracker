using HabitsLibrary;
namespace ScreensLibrary;

public class AskInput
{
    private void ClearPreviousLines (int numberOfLines)
    {
        for (int i = 1; i <= numberOfLines; i++)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
    public string LettersNumberAndSpaces (string message)
    {
        string returnString;
        char c = default;
        bool showError = false;
        do
        {
            if (showError)
            {
                ClearPreviousLines(2);
                Console.Write("Invalid Input.");
            }
            else Console.Write(message);

            Console.WriteLine(" Use only letters, numbers and spaces");
            returnString = Console.ReadLine();
            showError= true;
        }
        while (!((returnString.All(c => Char.IsLetterOrDigit(c)) || c == ' ') && returnString != ""));
        
        returnString.Trim();
        return returnString;
    }

    public int Digits(string message)
    {
        string? input;
        bool showError = false;
        int number;
        do
        {
            if (showError)
            {
                ClearPreviousLines(2);
                Console.Write("Invalid Input.");
            }
            else Console.Write(message);

            Console.WriteLine(" Use numbers only.");
            input = Console.ReadLine();

            showError = true;
        }
        while (!(Int32.TryParse(input, out number) || Convert.ToInt32(input) < 0));
        return number;
    }
}