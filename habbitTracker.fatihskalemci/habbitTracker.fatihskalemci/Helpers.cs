using habbitTracker.fatihskalemci.Models;

namespace habbitTracker.fatihskalemci;

internal class Helpers
{
    static internal Habbit GenerateRandomEntry()
    {
        string[,] habbits = {
            {"Water Drinking","Glass"},
            {"Running","KM"},
            {"Reading","Pages"}
        };

        int quantity = 1;

        DateTime date = DateTime.Today;

        return new Habbit
        {
            Quantity = quantity,
            Date = date,
            Unit = "KM",
            HabbitName = "Running"
        };

    }
}
