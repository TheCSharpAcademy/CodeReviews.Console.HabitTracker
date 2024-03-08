using ConsoleUI;
using ConsoleUI.Logic;
using ConsoleTableExt;

ConsoleDisplayMessages.DisplayAppWelcomeMessage();
cls();

HabitConsoleLogic.UserId = ConsoleDataReader.ConsoleReadRangedInt("Please Enter Your Id<0,1,2>", 0, 3);
HabitConsoleLogic.UserName = ConsoleDataReader.GetStringFromConsole("username");
int choice;
do
{
    cls();

    ConsoleDisplayMessages.DisplayMainMenu();
    choice = ConsoleDataReader.ConsoleReadRangedInt("Please enter the number corresponds the desigred operation", 0, 5);
    cls();
    switch (choice)
    {
        case 0:
            ConsoleDisplayMessages.TerminateAppMessage();
            break;
        case 1:
            HabitConsoleLogic.ViewAllHabits();
            Console.Write("Press Any Key to Continue:");
            Console.ReadLine();
            break;
        case 2:
            HabitConsoleLogic.InsertHabit();
            break;
        case 3:
            HabitConsoleLogic.DeleteHabit();
            break;
        case 4:
            HabitConsoleLogic.UpdateHabit();
            break;
    }
} while (choice != 0);



static void cls()
{
    Console.Clear();
}
