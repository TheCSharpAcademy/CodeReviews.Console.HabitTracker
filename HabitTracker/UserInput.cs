namespace HabitTracker
{
    internal static class UserInput
    {
        private static readonly string dateFormat = "yyyy-MM-dd";

        internal static string GetUserInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
        internal static double GetNumberInput(string message, double min, double max, bool allowBlanks = false)
        {
            string numberInput = string.Empty;
            double output;
            bool firstTime = true;
            bool validNumber = false;

            do
            {
                numberInput = GetUserInput(message);
                firstTime = false;

                validNumber = Double.TryParse(numberInput, out output);

                validNumber = (output < min || output > max) ? false : true;

                if (allowBlanks == true && numberInput.Trim() == "")
                {
                    output = Int32.MinValue;
                    validNumber = true;
                }

                if (validNumber == false )
                {
                    Console.WriteLine("ERROR: Please enter a valid number!");
                }

            } while (validNumber == false);

            return output;
        }
        internal static DateTime GetDateInput(string message, string blankBehavior = "unused")
        {
            string dateInput;
            DateTime output;
            bool validDate = false;

            do
            {
                dateInput = GetUserInput(message);

                if (dateInput == "")
                {
                    if (blankBehavior == "today")
                    {
                        output = DateTime.Now;
                        validDate = true;
                    }
                    else if (blankBehavior == "original")
                    {
                        output = DateTime.MinValue;
                        validDate = true;
                    }
                    else
                    {
                        output = DateTime.MinValue;
                        validDate = false;
                    }
                }
                else
                {
                    validDate = DateTime.TryParse(dateInput, out output);

                    if (validDate == false)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"ERROR: Invalid date entered! Try following the format: \"{dateFormat.ToUpper()}\"");
                        Console.WriteLine();
                    }

                    if (output > DateTime.Now)
                    { 
                        validDate = false;
                        Console.WriteLine();
                        Console.WriteLine($"ERROR: Date cannot be greater than today! ");
                        Console.WriteLine();
                    }
                }
            } while (validDate == false);

            return output;
        }
        internal static void PressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
        internal static bool GetUserConfirmation(string action)
        {
            bool confirmationResult = false;
            bool responseValid = true;

            do
            {
                string response = UserInput.GetUserInput($"Confirm {action}: Press \"Y\" for yes or \"N\" for no: ");

                if (response.ToLower().Trim() == "y")
                {
                    confirmationResult = true;
                    responseValid = true;
                }
                else if (response.ToLower().Trim() == "n")
                {
                    confirmationResult = false;
                    responseValid = true;
                }
                else
                {
                    Console.Write("INVALID RESPONSE!");
                    responseValid = false;
                }
            } while (responseValid == false);

            return confirmationResult;
        }
    }
}
