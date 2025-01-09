using HabitLoggerLibrary.Ui.Menu;

namespace HabitLoggerApp.Application.Handlers.HabitLogs;

public class HabitLogsMainMenuHandler(
    IHabitLogsMenuChoiceReader choiceReader,
    ViewHabitLogsHandler viewHabitLogsHandler,
    DeleteHabitLogHandler deleteHabitLogHandler,
    InsertHabitLogHandler insertHabitLogHandler,
    EditHabitLogHandler editHabitLogHandler)
{
    public void Handle()
    {
        do
        {
            Console.Clear();

            Console.WriteLine("Choose what you want to do.");
            HabitLogsMenuRenderer.Render();
            var choice = choiceReader.GetChoice();
            switch (choice)
            {
                case HabitLogsMenuChoice.GoBack:
                    return;
                case HabitLogsMenuChoice.DeleteLogEntry:
                    deleteHabitLogHandler.Handle();
                    break;
                case HabitLogsMenuChoice.EditLogEntry:
                    editHabitLogHandler.Handle();
                    break;
                case HabitLogsMenuChoice.InsertLogEntry:
                    insertHabitLogHandler.Handle();
                    break;
                case HabitLogsMenuChoice.ViewAllLogs:
                    viewHabitLogsHandler.Handle();
                    break;
            }
        } while (true);
    }
}