using System.Text;

namespace HabitLogger.Menus;

public class QuantityMenu : IMenu
{
    public string GetMenu()
    {
        StringBuilder sbMenu = new StringBuilder();

        sbMenu.AppendLine("Introduce a Quantity: ");

        return sbMenu.ToString();
    }
}
