using System.Text;

namespace HabitLogger;

public class HabitIdMenu : IMenu
{
    public string GetMenu()
    {
        StringBuilder sbMenu = new StringBuilder();

        sbMenu.AppendLine("Choose a Habit Id: ");

        return sbMenu.ToString();
    }
}
