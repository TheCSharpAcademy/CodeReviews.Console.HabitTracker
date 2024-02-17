using HabitTracker.Dejmenek.Models;
using System.Data.SQLite;

namespace HabitTracker.Dejmenek.DataAccess.Interfaces
{
    public interface IHabitRepository
    {
        void CreateTable();
        List<Habit> GetAllHabits();
        void AddHabit(string name, string description, string date, int quantity, string quantityUnit);
        void DeleteHabit(int id);
        void UpdateHabit(int id, string date, int quantity);
        void SeedData();
        bool IsEmptyTable();
    }
}
