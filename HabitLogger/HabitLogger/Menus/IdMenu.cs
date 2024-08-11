using System.Text;

namespace HabitLogger;

public class IdMenu : IMenu
{
    public string GetMenu()
    {
        StringBuilder sbMenu = new StringBuilder();

        sbMenu.AppendLine("Introduce an Id: ");

        return sbMenu.ToString();
    }
}
