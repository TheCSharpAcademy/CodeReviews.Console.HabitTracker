namespace TheCSharpAcademy.HabitTracker
{
  public class Program
  {
    public static void Main()
    {
      Program program = new();
      program.Run();
    }
    #region constructors
    Program()
    {
    }
    #endregion
    #region methods
    void Run()
    {
      UI ui = new();
      ui.MainMenu();
    }
    #endregion
  }
}