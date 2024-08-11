using System.Text;

namespace HabitLogger;

public enum ReportsMenuOption { Quit, QuantityPerYear, RecordsPerYear }
public class ReportsMenu : IMenu
{
    public string GetMenu()
    {
        StringBuilder sbMenu = new StringBuilder();

        sbMenu.AppendLine("Choose an option: ");
        sbMenu.AppendLine("0. Quit");
        sbMenu.AppendLine("1. Quantity per year");
        sbMenu.AppendLine("2. Records per year");

        return sbMenu.ToString();
    }
}
