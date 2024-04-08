using HabitLogger.BBualdo;
using HabitLogger.BBualdo.Database;

DbContext db = new DbContext();

bool isAppClosed = false;

while (!isAppClosed)
{
  MainMenu.ShowMenu();

  string userInput = MainMenu.GetUserInput();

  switch (userInput)
  {
    case "0":
      Console.WriteLine("\nSee you soon!\n");
      isAppClosed = true;
      break;
    case "1":
      db.GetAllData();
      Console.WriteLine("\nPress any key to return to Main Menu.");
      Console.ReadKey();
      break;
    case "2":
      db.InsertData(); break;
    case "3":
      db.DeleteData(); break;
    case "4":
      db.UpdateData(); break;
    default:
      break;
  }
}