using System.Text;

namespace HabitLogger;

public enum HabitMenuOptions { Quit, Choose, Create, Delete }
public class HabitMenu : IMenu
{
    public string GetMenu()
    {
        StringBuilder sbMenu = new StringBuilder();

        sbMenu.AppendLine("Choose an option: ");
        sbMenu.AppendLine("0. Quit");
        sbMenu.AppendLine("1. Choose a habit");
        sbMenu.AppendLine("2. Create a new habit");
        sbMenu.AppendLine("3. Delete an existing habit");

        return sbMenu.ToString();
    }
}
