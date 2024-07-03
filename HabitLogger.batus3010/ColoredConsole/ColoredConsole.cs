namespace ColoredConsole
{
    public class ColoredConsole
    {
        public static void WriteLine(string text, ConsoleColor color)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previousColor;
        }

        public static void Write(string text, ConsoleColor color)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = previousColor;
        }

        public static string Prompt(string questionToAsk = "")
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(questionToAsk + " ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string input = Console.ReadLine() ?? ""; // If we got null, use empty string instead.
            Console.ForegroundColor = previousColor;
            return input;
        }
    }
}
