using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitLoggerApp.Data;
using HabitLoggerApp.Models;

namespace HabitLoggerApp.Services
{
    public class HabitService(DatabaseManager dbManager)
    {
        private readonly DatabaseManager _dbManager = dbManager;

        public (bool isSuccess, string message) AddHabit(string name, string unit)
        {
            if (!IsInputValid(name, unit, out string validationMessage))
                return (false, validationMessage);

            if (IsDuplicate(name))
                return (false, "A habit with that name already exists.");

            bool created = CreateHabit(name, unit);

            return created
                ? (true, $"Habit '{name}' added successfully.")
                : (false, "An error occurred while creating the habit.");
        }

        private static bool IsInputValid(string name, string unit, out string message)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(unit))
            {
                message = "Habit name and unit of measure cannot be empty.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private bool IsDuplicate(string name)
        {
            var habits = _dbManager.GetHabits();
            return habits.Exists(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private bool CreateHabit(string name, string unit)
        {
            try
            {
                var habit = new Habit
                {
                    Name = name.Trim(),
                    UnitOfMeasure = unit.Trim()
                };

                _dbManager.AddHabit(habit);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Habit> GetAllHabits()
        {
            return _dbManager.GetHabits();
        }

        public bool SeedInitialData()
        {
            return _dbManager.SeedData();
        }

        public bool DeleteHabit(int habitId)
        {
            var habits = _dbManager.GetHabits();
            var habit = habits.Find(h => h.Id == habitId);

            if (habit == null)
                return false;

            try
            {
                _dbManager.DeleteHabit(habitId);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}

