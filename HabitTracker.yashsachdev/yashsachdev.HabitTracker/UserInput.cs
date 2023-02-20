namespace yashsachdev.HabitTracker;
internal static class UserInput
{
    /// <summary>
    /// Enter Name of the User
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Enter user's Emaild.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Enter the password.
    /// </summary>
    /// <returns></returns>
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
            Console.WriteLine("Invalid inputs.Enter a proper Habit");
            return string.Empty;
        }

        return input;
    }

    public static string GetHabitUnit(string unit)
    {
        Console.WriteLine("Please enter the unit:");
        int input = Convert.ToInt32(Console.ReadLine());
        var inputString = Convert.ToString(input);
        if (string.IsNullOrWhiteSpace(inputString))
        {
            Console.WriteLine("Invalid inputs. Email and password are required.");
            return string.Empty;
        }

        return unit + " " + inputString;
    }

    public static string GetUnitMeasurement()
    {
        Console.WriteLine("Select a unit of measurement:");
        Console.WriteLine("1. Days");
        Console.WriteLine("2. Times");
        Console.WriteLine("3 Amount(eg: 8 Litres of water)");
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
                Console.WriteLine("Enter the amount (Eg: Litre, hours, kms)");
                unit = Console.ReadLine();
                break;
            default:
                Console.WriteLine("Invalid selection. Please choose again.");
                break;
        }

        return (unit);
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

    public static bool CheckPassword(string email, string password)
    {
        using (SqliteConnection cnn = new SqliteConnection(DatabaseClass.connectionString))
        {
            cnn.Open();
            SqliteCommand cmd = new SqliteCommand("SELECT Password FROM User WHERE Email = @email", cnn);
            cmd.Parameters.AddWithValue("@email", email);
            string correctPassword = (string)cmd.ExecuteScalar();
            return correctPassword == password;
        }
    }

    /// <summary>
    /// Refer https://mailtrap.io/blog/validate-email-address-c/
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool ValidateEmail(string email)
    {
        var valid = true;
        try
        {
            MailAddress emailAddress = new MailAddress(email);
        }
        catch
        {
            valid = false;
        }

        return valid;
    }

    public static bool ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        foreach (char c in name)
        {
            if (!char.IsLetter(c))
            {
                return false;
            }
        }

        return true;
    }
}
