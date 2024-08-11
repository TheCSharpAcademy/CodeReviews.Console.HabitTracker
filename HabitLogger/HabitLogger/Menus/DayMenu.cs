using System.Text;

namespace HabitLogger;

public class DayMenu : IMenu
{
    public string GetMenu()
    {
        StringBuilder sbMenu = new StringBuilder();

        sbMenu.AppendLine("Introduce a Day format (yyyy-MM-dd): ");

        return sbMenu.ToString();
    }
}
