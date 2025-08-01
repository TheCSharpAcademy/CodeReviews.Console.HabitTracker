using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitLoggerApp.Data;
using HabitLoggerApp.Models;

namespace HabitLoggerApp.Services
{
    public class HabitEntryService(DatabaseManager dbManager)
    {
        private readonly DatabaseManager _dbManager = dbManager;


        public bool LogEntry(int habitId, int quantity, DateTime date)
        {
            if (habitId <= 0 || quantity <= 0)
            {
                return false;
            }
            var habits = _dbManager.GetHabits();
            var habit = habits.Find(h => h.Id == habitId);
            if (habit == null)
                return false;

            var entry = new HabitEntry
            {
                HabitId = habitId,
                Quantity = quantity,
                Date = date
            };

            try
            {
                _dbManager.AddHabitEntry(entry);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<HabitEntry> GetEntriesForHabit(int habitId)
        {
            return _dbManager.GetEntriesByHabit(habitId);
        }
    }
    }

