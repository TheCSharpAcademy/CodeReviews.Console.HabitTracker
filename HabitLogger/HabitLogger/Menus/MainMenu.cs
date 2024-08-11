using System.Text;

namespace HabitLogger;

public enum MainMenuOptions { Quit, Create, Update, Delete, Show, Reports, HabitMenu }

public class MainMenu : IMenu
{
    public string GetMenu()
    {
        StringBuilder sbMenu = new StringBuilder();

        sbMenu.AppendLine("Choose an option: ");
        sbMenu.AppendLine("0. Quit");
        sbMenu.AppendLine("1. Create a record");
        sbMenu.AppendLine("2. Update a record");
        sbMenu.AppendLine("3. Delete a record");
        sbMenu.AppendLine("4. Show all records");
        sbMenu.AppendLine("5. Reports");
        sbMenu.AppendLine("6. Show habit management menu");

        return sbMenu.ToString();
    }
}
