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
            habit.Quantity = _inputValidator.ConvertToInt(_inputValidator.GetQualityInput());

            _databaseInitializer.InsertToDatabase(habit.Date, habit.Name, habit.Measurement, habit.Quantity);
        }

        public void UpdateHabit()
        {
            throw new NotImplementedException();
        }
        public void DeleteHabit()
        {
            _consoleInteraction.ShowMessage("\n\nPlease type the ID of the habit to delete or ");
        }
    }
}