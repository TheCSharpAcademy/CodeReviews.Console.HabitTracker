using HabitLogger.Shared.Logger;
using HabitLogger.Data;
using HabitLogger.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HabitLogger.Shared;


namespace HabitLogger.Services
{
	public class HabitService
	{
		private readonly HabitRepository _repository;
        public HabitService(string connectionString)
		{
            _repository = new HabitRepository(connectionString);
        }

        public void AddHabit(Habit habit)
        {
            if(habit.Date < DateTime.UtcNow)
            {
                throw new Exception("Cannot start a habit in the past");
            }
            if (DuplicateHabit(habit.Id))
            {
                habit = GetByName(habit.Name);
                if (habit is not null)
                    throw new Exception("A habit already exist");
            }
            _repository.Create(habit);  
        }

        public void UpdateHabit(Habit habit)
        {
            if (habit.Date < DateTime.UtcNow)
            {
                throw new Exception("Cannot start a habit in the past");
            }
            _repository.Update(habit);
        }

        public Habit GetHabit(int id)
        {
            return _repository.Retrieve(id);
        }

        public void DeleteHabit(int id)
        {
            _repository.Delete(id);
        }
        public bool DuplicateHabit(int id)
        {
            var duplicateHabit = GetHabit(id);
            return duplicateHabit != null;
            
        }
        public Habit GetByName(string name)
        {
            return _repository.RetrieveByName(name);
        }

    }
}

