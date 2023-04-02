﻿using System.Globalization;

namespace ScreensLibrary;

public class AskInput
{
    public void ClearPreviousLines (int numberOfLines)
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
    
    public string? LettersNumberAndSpaces (string message)
    {
        string? returnString;
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
        while (!(returnString.All(c => Char.IsLetterOrDigit(c) || c == ' ') && returnString != ""));
        
        returnString.Trim();
        return returnString;
    }
    
    public int PositiveNumber(string message)
    {
        string? input;
        bool showError = false;
        int number;
        do
        {
            Console.WriteLine(message);
            if (showError)
            {
                ClearPreviousLines(3);
                Console.Write("Invalid Input. ");
            }

            Console.WriteLine("Use positive numbers only.");
            input = Console.ReadLine();
            showError = true;
        }
        //while it's not a number OR not a positive number
        while (!(Int32.TryParse(input, out number) && number >= 0));
        return number;
    }
    
    public string? Date(string message)
    {
        string? input;
        bool showError = false;
        do
        {
            if (!showError) Console.WriteLine(message);
            else
            { 
                ClearPreviousLines(2);
                Console.WriteLine("Please write a valid date. Or 0 to return"); 
            }

            input = Console.ReadLine();
            if (input is "0") return input;

            try
            {
                _ = DateTime.ParseExact(input, "dd-MM-yy", new CultureInfo("en-US"));
                return input;
            }
            catch (FormatException)
            {
                showError = true;
            }
        }
        while (true);
    }
    
    public void AnyAndEnterToContinue()
    {
        Console.WriteLine("Press any key and Enter to continue");
        Console.ReadLine();
    }
    
    public bool ZeroOrAnyKeyAndEnterToContinue()
    {
        Console.WriteLine("Press any key and Enter to continue. Or press 0 to return to the menu");
        if (Console.ReadLine() == "0") return true;
        else return false;

    }
}