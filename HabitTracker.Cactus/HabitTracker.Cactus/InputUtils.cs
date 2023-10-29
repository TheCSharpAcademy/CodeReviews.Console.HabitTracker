using System.Globalization;

internal static class InputUtils
{

    public static int GetInValidInputId(HashSet<int> ids)
    {
        Console.WriteLine($"Please type a id you want to operate:");
        int id = -1;
        string? idStr = Console.ReadLine();
        while (!int.TryParse(idStr, out id) || !ids.Contains(id))
        {
            Console.WriteLine($"Sorry, your id is invalid. Please type a valid id:");
            idStr = Console.ReadLine();
        }
        return id;
    }

    public static DateTime GetValidInputDate()
    {
        DateTime date;
        Console.WriteLine("Please type your date(dd-MM-yyyy):");
        string? dateStr = Console.ReadLine();
        while (!DateTime.TryParseExact(dateStr, "dd-MM-yyyy", new CultureInfo("en-US"),
                               DateTimeStyles.None, out date))
        {
            Console.WriteLine("Sorry, your date is invalid. Please type a valid date(dd-MM-yyyy):");
            dateStr = Console.ReadLine();
        }
        return date;
    }

    public static int GetValidInputQuantity()
    {
        int quantity;
        Console.WriteLine("Please type your quantity:");
        string? quantityStr = Console.ReadLine();
        while (!int.TryParse(quantityStr, out quantity))
        {
            Console.WriteLine("Sorry, your quantity is invalid. Please type a valid quantity:");
            quantityStr = Console.ReadLine();
        }
        return quantity;
    }
}