using System.Text;

namespace HabitLogger;

public class HabitNameMenu : IMenu
{
    public string GetMenu()
    {
        StringBuilder sbMenu = new StringBuilder();

        sbMenu.AppendLine("Introduce a Name for the Habit: ");

        return sbMenu.ToString();
    }
}
