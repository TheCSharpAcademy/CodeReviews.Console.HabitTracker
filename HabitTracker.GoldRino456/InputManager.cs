namespace HabitTracker.GoldRino456
{
    public sealed class InputManager
    {
        #region Singleton Logic & Properties
        private static InputManager? _instance;
        private static readonly object _instanceLock = new object();

        public static InputManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new InputManager();
                    }
                    return _instance;
                }

            }
        }
        #endregion

        public string GetValidUserStringInput()
        {
            string? input;

            while (true)
            {
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid Input.");
                    Console.WriteLine("Check input and please enter again: ");
                    continue;
                }
                else
                {
                    return input;
                }
            }
        }

        public int GetValidUserIntegerInput()
        {
            string? input;
            int output;

            while(true)
            {
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) || !Int32.TryParse(input, out output))
                {
                    Console.Write("Invalid Input. Please enter a valid integer number: ");
                    continue;
                }
                else
                {
                    return output;
                }
            }
        }

        public DateTime GetValidUserDateTimeInput()
        {
            string? input;
            DateTime output;

            while (true)
            {
                input = Console.ReadLine();

                if(string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid date input. Cannot be empty!");
                    Console.Write("Please Enter a Valid Date: ");
                    continue;
                }

                if(input.Equals("T") || input.Equals("t"))
                {
                    return DateTime.Today;
                }

                if (!DateTime.TryParse(input, out output))
                {
                    Console.WriteLine("Invalid date input. Please ensure date is in the following format (MM/DD/YYYY) or enter 'T' to use Today's date.");
                    Console.Write("Please Enter a Valid Date: ");
                    continue;
                }
               
                return output;
            }
        }
    }
}
