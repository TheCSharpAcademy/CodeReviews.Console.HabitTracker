namespace yashsachdev.HabitTracker;
internal static class UserInput
{
    public static string GetName()
    {
        Console.WriteLine("Please enter your name :");
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid inputs. Email and password are required.");
            return string.Empty;
        }
        return input;
    }
    public static string GetEmail()
    {
        Console.WriteLine("Please enter your email id :");
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid inputs. Email and password are required.");
            return string.Empty;
        }
        return input;
    }
    public static string GetPassword()
    {
        Console.WriteLine("Please enter your password:");
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid inputs. Email and password are required.");
            return string.Empty;
        }
        return input;
    
    }
    public static string GetHabitName()
    {
        Console.WriteLine("Please enter the habit Name:");
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid inputs. Email and password are required.");
            return string.Empty;
        }
        return input;
    }
    public static string GetHabitUnit()
    {
        Console.WriteLine("Please enter the unit:");
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid inputs. Email and password are required.");
            return string.Empty;
        }
        return input;
    }
    public static string GetUnitMeasurement(string habitUnit)
    {
        Console.WriteLine("Select a unit of measurement:");
        Console.WriteLine("1. Days");
        Console.WriteLine("2. Times");
        Console.WriteLine("3 Amount(eg: 8 Litres of water");
        int unitChoice = int.Parse(Console.ReadLine());
        string unit = string.Empty;
        switch (unitChoice)
        {
            case 1:
                unit = "Days";
                break;
            case 2:
                unit = "Times";
                break;
            case 3:
                unit = "Amount";
                break;
            default:
                Console.WriteLine("Invalid selection. Please choose again.");
                break;
        }
        return(habitUnit + unit);
    }
    public static DateTime GetStartDate()
    {
        DateTime dateTime;
        bool isValidDate = false;

        while (!isValidDate)
        {
            Console.WriteLine("Enter Start Date (YYYY-MM-DD): ");
            string input = Console.ReadLine();

            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out dateTime))
            {
                isValidDate = true;
                return dateTime;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please try again.");
            }
        }
        return DateTime.MinValue;
    }


}
