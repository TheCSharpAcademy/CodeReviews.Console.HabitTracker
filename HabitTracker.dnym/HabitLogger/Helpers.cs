namespace HabitLogger
{
    internal static class Helpers
    {
        internal enum Signal
        {
            PG_UP,
            PG_DOWN,
        }

        internal static Tuple<bool, string, Signal?> ReadInput()
        {
            int cursorStartX = Console.CursorLeft;
            string userInput = "";
            int inputPosition = 0;
            bool pressedEscape = false;
            bool keepReadingKeys = true;
            while (!pressedEscape && keepReadingKeys)
            {
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        pressedEscape = true;
                        break;
                    case ConsoleKey.Enter:
                        keepReadingKeys = false;
                        break;
                    case ConsoleKey.Backspace:
                        if (inputPosition > 0)
                        {
                            inputPosition--;
                            userInput = userInput.Remove(inputPosition, 1);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            ClearRestOfLine();
                            Console.Write(userInput[inputPosition..]);
                            Console.SetCursorPosition(cursorStartX + inputPosition, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.Home:
                        inputPosition = 0;
                        Console.SetCursorPosition(cursorStartX, Console.CursorTop);
                        break;
                    case ConsoleKey.End:
                        inputPosition = userInput.Length;
                        Console.SetCursorPosition(cursorStartX + inputPosition, Console.CursorTop);
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        break;
                    case ConsoleKey.LeftArrow:
                        if (inputPosition > 0)
                        {
                            inputPosition--;
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (inputPosition < userInput.Length)
                        {
                            inputPosition++;
                            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.Delete:
                        if (inputPosition < userInput.Length)
                        {
                            userInput = userInput.Remove(inputPosition, 1);
                            ClearRestOfLine();
                            Console.Write(userInput[inputPosition..]);
                            Console.SetCursorPosition(cursorStartX + inputPosition, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.PageUp:
                        return new(false, string.Empty, Signal.PG_UP);
                    case ConsoleKey.PageDown:
                        return new(false, string.Empty, Signal.PG_DOWN);
                    default:
                        userInput = userInput.Insert(inputPosition, $"{keyInfo.KeyChar}");
                        inputPosition++;
                        Console.Write(keyInfo.KeyChar);
                        ClearRestOfLine();
                        Console.Write(userInput[inputPosition..]);
                        Console.SetCursorPosition(cursorStartX + inputPosition, Console.CursorTop);
                        break;
                }
            }
            return new(pressedEscape, userInput, null);
        }

        internal static void ClearRestOfLine()
        {
            var currentLine = Console.CursorTop;
            var currentColumn = Console.CursorLeft;
            Console.Write(new string(' ', Console.WindowWidth - currentColumn));
            Console.SetCursorPosition(currentColumn, currentLine);
        }
    }
}