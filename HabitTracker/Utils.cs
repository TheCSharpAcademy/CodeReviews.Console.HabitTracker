using System.Text.RegularExpressions;

namespace HabitTracker;

public static class Utils
{
    public static DateTime GetDateInput()
    {
        bool validDateEntered = false;
        DateTime habitDate = default; 
            
        while (!validDateEntered)
        {
            Console.Write("Type the date when the habit was done in dd/mm/yyyy format: ");
            string? date = Console.ReadLine();

            if (date is null || Regex.IsMatch(date, "[1-31]/[1-12]/[1-9999]"))
            {
                Console.WriteLine("Error: date entered is not valid");
            }
            else
            {
                string[] dateElements = date.Split("/");
                int day = int.Parse(dateElements[0]);
                int month = int.Parse(dateElements[1]);
                int year = int.Parse(dateElements[2]);
                try
                {
                    habitDate = new DateTime(year, month, day);
                    validDateEntered = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Error: Number of days entered greater than number of days in the given month");
                }
            }
        }

        return habitDate;
    }

    public static string GetHabitInput()
    {
        bool validHabitEntered = false;
        string? habit = "";
        
        while (!validHabitEntered)
        {
            Console.Write("Type the name of the habit: ");
            habit = Console.ReadLine();

            if (habit is null || !Regex.IsMatch(habit, "^[a-zA-Z ]+$"))
            {
                Console.WriteLine("Name of habit should contain only alphabetical letters");
            }
            else
            {
                validHabitEntered = true;
            }
        }

        return habit?.ToLower() ?? string.Empty;
    }

    public static int GetQuantityInput()
    {
        bool validQuantityEntered = false;
        int habitQuantity = 0;
        
        while (!validQuantityEntered)
        {
            Console.Write("Type the quantity of the habit done: ");
            string? quantity = Console.ReadLine();

            if (quantity is null || int.TryParse(quantity, out habitQuantity) == false)
            {
                Console.WriteLine("Quantity of the habit done must be an integer");
            }
            else
            {
                validQuantityEntered = true;
            }
        }

        return habitQuantity;
    }

    public static int GetIdOfRecord(HashSet<int> recordIndexes)
    {
        string? input;
        bool validIndexEntered = false;
        int recordIndex = 0;

        while (!validIndexEntered)
        {
            Console.Write("\nEnter the id of the record that you want to update: ");
            input = Console.ReadLine();

            if (input == null || int.TryParse(input, out recordIndex) == false)
            {
                Console.WriteLine("Error: Invalid Input");
            }
            else if (!recordIndexes.Contains(recordIndex))
            {
                Console.WriteLine("Error: Id entered is not in the list shown.");
            }
            else
            {
                validIndexEntered = true;
            }
        }

        return recordIndex;
    }
}