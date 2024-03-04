namespace HabitLogger.Views;

public class MenuView
{
    private readonly int pageSize = 8;
    private int pageNumber;
    private int totalPages;
    public string Title { get; set; } = string.Empty;

    public int ShowMenu(IList<string> menuItems)
    {
        totalPages = (int)Math.Ceiling(menuItems.Count / (double)pageSize);

        while (true)
        {
            DisplayMenu(menuItems);
            var result = HandleUserInput(menuItems.Count);
            if (result.HasValue) return result.Value;
        }
    }

    private void DisplayMenu(IList<string> menuItems)
    {
        Console.Clear();
        Console.WriteLine(Title);
        Console.WriteLine(new string('-', Title.Length));

        var amountOfItemsToList = Math.Min(pageSize, menuItems.Count - pageNumber * pageSize);
        for (var i = 0; i < amountOfItemsToList; i++)
        {
            var menuItemIndex = pageNumber * pageSize + i;
            Console.WriteLine($"{i + 1}. {menuItems[menuItemIndex]}");
        }

        Console.WriteLine("\n0. Back to previous menu");
        if (totalPages > 1) Console.WriteLine("N. Next Page, P. Previous Page");

        Console.WriteLine("\nYour Choice: ");
    }

    private int? HandleUserInput(int itemCount)
    {
        var userInput = GetUserInput();
        if (int.TryParse(userInput, out var menuChoice))
        {
            if (menuChoice >= 1 && menuChoice <= pageSize)
            {
                var itemIndex = pageNumber * pageSize + menuChoice - 1;
                if (itemIndex < itemCount) return itemIndex;
            }
            else if (menuChoice == 0)
            {
                return -1; // Indicate that the user wants to go back
            }
        }
        else
        {
            HandlePagination(userInput);
        }

        return null; // Indicate no valid selection was made
    }

    private void HandlePagination(string userInput)
    {
        if (userInput == "n" && pageNumber < totalPages - 1)
        {
            pageNumber++;
        }
        else if (userInput == "p" && pageNumber > 0)
        {
            pageNumber--;
        }
        else if (userInput is not ("p" or "n"))
        {
            Console.WriteLine("Wrong input detected.");
            Console.WriteLine("Press any key to retry.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine(userInput == "n"
                ? "You already are on the last page."
                : "You already are on the first page.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }

    private static string GetUserInput()
    {
        return Console.ReadLine()?.ToLower().Trim();
    }
}