using Nelson.Habit_Tracker.Models;
using Nelson.Habit_Tracker.UserConsoleInteraction;
using Nelson.Habit_Tracker.Utils;

namespace Nelson.Habit_Tracker.DataAccess
{
    public class HabitRepository : IHabitRepository
    {
        private readonly IConsoleInteraction? _consoleInteraction;
        private readonly IInputValidator? _inputValidator;
        private readonly IDateValidator? _dateValidator;
        private readonly IDatabaseInitializer? _databaseInitializer;

        public HabitRepository(IConsoleInteraction consoleInteraction, IInputValidator? inputValidator, IDateValidator dateValidator, IDatabaseInitializer? databaseInitializer)
        {
            _consoleInteraction = consoleInteraction;
            _inputValidator = inputValidator;
            _dateValidator = dateValidator;
            _databaseInitializer = databaseInitializer;
        }

        public string GetUserInput()
        {
            Console.Clear();
            _consoleInteraction.DisplayMenu();
            return Console.ReadLine();
        }

        public void GetAllHabits()
        {
            _databaseInitializer.GetFromDatabase();
        }

        public void InsertHabit(Habit habit)
        {
            habit.Date = _dateValidator.GetDateInput();
            habit.Name = _inputValidator.GetNameInput();
            habit.Measurement = _inputValidator.GetMeasurementInput();
            habit.Quantity = _inputValidator.GetQualityInput();

            _databaseInitializer.InsertToDatabase(habit.Date, habit.Name, habit.Measurement, habit.Quantity);
        }

        public void UpdateHabit()
        {
            // Display records
            _databaseInitializer.GetFromDatabase();

            _consoleInteraction.ShowMessageTime("\n\nPlease type the ID of the habit you would like to update. Type 0 to return to Main Menu.");

            int id = _inputValidator.ConvertToInt(_consoleInteraction.GetUserInput());

            if (id == 0) return;

            DateTime date = _dateValidator.GetDateInput();
            string name = _inputValidator.GetNameInput();
            string measure = _inputValidator.GetMeasurementInput();
            int quantity = _inputValidator.GetQualityInput();

            _databaseInitializer.UpdateToDatabase(id, date, name, measure, quantity);
        }

        public void DeleteHabit()
        {
            // Display records
            _databaseInitializer.GetFromDatabase();

            _consoleInteraction.ShowMessageTime("\n\nPlease type the ID of the habit to delete or 0 to return to Main Menu:");

            int id = _inputValidator.ConvertToInt(_consoleInteraction.GetUserInput());

            if (id == 0) return;

            _databaseInitializer.DeleteFromDatabase(id);
        }
    }
}