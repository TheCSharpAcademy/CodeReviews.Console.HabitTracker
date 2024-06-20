namespace HabitTrackerProgram.Util
{
    class Input
    {
        public static Tuple<bool, decimal> ParseDecimal(string input)
        {
            decimal value;

            bool validNumber = decimal.TryParse(input, out value);

            return new Tuple<bool, decimal>(validNumber, value);
        }

        public static Tuple<bool, DateTime> ParseDateTime(string input)
        {
            DateTime value;

            bool validDate = DateTime.TryParse(input, out value);

            return new Tuple<bool, DateTime>(validDate, value);
        }

        public static T TryReadInput<T>(string fieldName, Func<string, Tuple<bool, T>> parseDelegate)
        {
            T value;

            while (true)
            {
                Console.Write($"\n\t{fieldName}:");
                string? valueInput = Console.ReadLine();

                if (valueInput == null)
                {
                    Console.WriteLine("Please enter a valid value.");
                    continue;
                }

                Tuple<bool, T> parseResult = parseDelegate(valueInput);

                if (parseResult.Item1 == false)
                {
                    Console.WriteLine("\n\tPlease enter a valid value.");
                    continue;
                }

                value = parseResult.Item2;

                break;
            }

            return value;
        }
    }
}